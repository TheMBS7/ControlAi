
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class MesService : IMesService
{
    private readonly ApplicationDbContext _context;

    public MesService(ApplicationDbContext context)
    {
        _context = context;
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
                DataInicial = DateTime.SpecifyKind(dataInicial, DateTimeKind.Utc),
                DataFinal = DateTime.SpecifyKind(dataFinal, DateTimeKind.Utc)
            });
        }

        _context.Meses.AddRange(meses);
        await _context.SaveChangesAsync();

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
}