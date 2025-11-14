
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class MesService : IMesService
{
    private readonly ApplicationDbContext _context;
    private readonly IExtratoService _extratoService;

    public MesService(ApplicationDbContext context, IExtratoService extratoService)
    {
        _context = context;
        _extratoService = extratoService;
    }

    public async Task<IEnumerable<MesDTO>> CriarPeriodoAsync()
    {
        // pega o último ano que já existe na tabela
        int? ultimoAno = await _context.Meses
            .OrderByDescending(m => m.DataInicial)
            .Select(m => (int?)m.DataInicial.Year)
            .FirstOrDefaultAsync();

        // se não existir nada, começa do ano atual
        int anoBase = ultimoAno.HasValue ? ultimoAno.Value + 1 : DateTime.Now.Year;

        List<Mes> meses = new List<Mes>();

        for (int i = 1; i <= 12; i++)
        {
            DateTime dataInicial = new DateTime(anoBase, i, 1);
            DateTime dataFinal = dataInicial.AddMonths(1).AddDays(-1); // pega o primeiro dia do mês seguinte a dataInicial e o -1 volta um dia, fazendo ser o ultimo dia do mês da data inicial

            meses.Add(new Mes
            {
                Descricao = dataInicial.ToString("MMMM", new System.Globalization.CultureInfo("pt-BR")),
                DataInicial = DateTime.SpecifyKind(dataInicial, DateTimeKind.Local).ToUniversalTime(),
                DataFinal = DateTime.SpecifyKind(dataFinal, DateTimeKind.Local).ToUniversalTime()
            });
        }

        _context.Meses.AddRange(meses);
        await _context.SaveChangesAsync();

        try
        {
            await CriarMovimentosAsync(anoBase);
        }
        catch
        {
            return [];
        }


        List<MesDTO> mesesCriados = new List<MesDTO>();
        foreach (Mes mes in meses)
        {
            MesDTO mesDTO = MesDTO.Map(mes);
            mesesCriados.Add(mesDTO);
        }

        return mesesCriados;
    }

    public async Task<IEnumerable<MesDTO>> MostrarPeriodosAsync()
    {
        List<Mes> mesesEncontrados = await _context.Meses.ToListAsync();

        List<MesDTO> mostrarMeses = new List<MesDTO>();
        foreach (Mes mes in mesesEncontrados)
        {
            MesDTO mesDTO = MesDTO.Map(mes);
            mostrarMeses.Add(mesDTO);
        }

        return mostrarMeses;
    }

    public async Task<MesDTO?> MostrarPeriodoIdAsync(int id)
    {
        Mes? mesEncontrado = await _context.Meses.FirstOrDefaultAsync(m => m.Id == id);

        if (mesEncontrado == null)
        {
            return null;
        }
        return MesDTO.Map(mesEncontrado);
    }


    private async Task<bool> CriarMovimentosAsync(int ano)
    {
        int anoAnterior = ano - 1;

        DateTime primeiroDiaUltimoMes = new DateTime(anoAnterior, 12, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime ultimoDiaUltimoMes = primeiroDiaUltimoMes.AddMonths(1).AddDays(-1);

        IEnumerable<Extrato> extratosFixos = await _context.Extratos
            .Where(e => (e.SaidaFixaId != null || e.EntradaFixaId != null) && e.Data.Date >= primeiroDiaUltimoMes.Date && e.Data.Date <= ultimoDiaUltimoMes.Date)
            .ToListAsync();

        if (!extratosFixos.Any())
        {
            return false;
        }

        int id = 0;

        foreach (Extrato? extrato in extratosFixos)
        {
            if (extrato?.SaidaFixaId != null)
            {
                id = extrato.SaidaFixaId.Value;
            }
            else if (extrato?.EntradaFixaId != null)
            {
                id = extrato.EntradaFixaId.Value;
            }
            else
            {
                return false;
            }

            DateTime dataAjustada = extrato.Data.AddMonths(1);

            ExtratoFixoCreateModel model = new ExtratoFixoCreateModel
            (
                extrato.Descricao,
                extrato.ValorTotal,
                dataAjustada,
                extrato.CategoriaId,
                extrato.PessoaId,
                extrato.TipoMovimentoId
            );

            await _extratoService.CriarExtratosAsync(id, model);
        }

        return true;
    }
}