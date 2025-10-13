using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class ExtratoService : IExtratoService
{
    private readonly ApplicationDbContext _context;

    public ExtratoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExtratoDTO>> CriarExtratosAsync(ExtratoCreateModel model)
    {
        Categoria? categoria = await _context.Categorias.FindAsync(model.CategoriaId);
        Pessoa? pessoa = await _context.Pessoas.FindAsync(model.PessoaId);
        Mes? mesInicial = await _context.Meses.FindAsync(model.MesId);
        TipoMovimento? tipoMovimento = await _context.TiposMovimentos.FindAsync(model.TipoMovimentoId);
        List<Extrato> novosExtratos = new List<Extrato>();


        if (categoria == null || pessoa == null || mesInicial == null || tipoMovimento == null)
            return Enumerable.Empty<ExtratoDTO>();

        var mesAtual = mesInicial;
        Guid idParcelas = Guid.NewGuid();

        for (int i = 1; i <= model.NumeroMaxParcelas; i++)
        {
            if (mesAtual == null) break;

            var extrato = new Extrato
            {
                Descricao = model.Descricao,
                ValorTotal = model.ValorTotal,
                Data = model.Data,
                NumeroMaxParcelas = model.NumeroMaxParcelas,
                NumeroParcela = i,
                IdParcelas = idParcelas,
                Categoria = categoria,
                Pessoa = pessoa,
                Mes = mesAtual,
                TipoMovimento = tipoMovimento
            };

            novosExtratos.Add(extrato);
            mesAtual = await ObterProximoMesAsync(mesAtual);
        }

        _context.Extratos.AddRange(novosExtratos);
        await _context.SaveChangesAsync();

        List<ExtratoDTO> extratosCriados = new List<ExtratoDTO>();
        foreach (Extrato extrato in novosExtratos)
        {
            ExtratoDTO extratoDTO = ExtratoDTO.Map(extrato);
            extratosCriados.Add(extratoDTO);
        }

        return extratosCriados;
    }

    private async Task<Mes?> ObterProximoMesAsync(Mes mesAtual)
    {
        var proximoMesId = mesAtual.Id + 1;

        return await _context.Meses.FindAsync(proximoMesId);
    }

    public async Task<ExtratoDTO?> EditarExtratoAsync(int id, ExtratoEditModel model)
    {
        var extrato = await _context.Extratos.FirstOrDefaultAsync(i => i.Id == id);

        if (extrato == null) return null;

        extrato.Descricao = model.Descricao;
        extrato.ValorTotal = model.ValorTotal;
        extrato.Data = model.Data;
        extrato.NumeroMaxParcelas = model.NumeroMaxParcelas;
        extrato.NumeroParcela = model.NumeroParcela;
        extrato.CategoriaId = model.CategoriaId;
        extrato.PessoaId = model.PessoaId;

        await _context.SaveChangesAsync();

        return ExtratoDTO.Map(extrato);
    }

    public async Task<bool?> ExcluirExtratoAsync(int id, ExtratoDeleteModel model) //verificar se ? é a melhor alternativa para esses casos
    {
        Extrato? extrato = await _context.Extratos.FindAsync(id);

        if (extrato == null) return false;

        if (model.ExcluirParcelas) //verifica se selecionou o campo de excluir todas
        {
            List<Extrato> listaExtratos = await _context.Extratos
                .Where(i => i.IdParcelas == extrato.IdParcelas)
                .ToListAsync();

            if (!listaExtratos.Any()) return null;

            _context.Extratos.RemoveRange(listaExtratos);
            await _context.SaveChangesAsync();

            return true;
        }

        _context.Extratos.Remove(extrato);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ExtratoDTO>> MostrarExtratosAsync()
    {
        List<Extrato> extratosEncontrados = await _context.Extratos
            .Include(e => e.Categoria)
            .Include(e => e.Pessoa)
            .Include(e => e.Mes)
            .OrderBy(e => e.Data)
            .ToListAsync();

        List<ExtratoDTO> extratosExistentes = new List<ExtratoDTO>();
        foreach (Extrato extrato in extratosEncontrados)
        {
            ExtratoDTO extratoDTO = ExtratoDTO.Map(extrato);
            extratosExistentes.Add(extratoDTO);
        }

        return extratosExistentes;
    }

    public async Task<IEnumerable<ExtratoDTO>> MostrarExtratoIdAsync(int mesId)
    {
        List<Extrato> extratoEncontrado = await _context.Extratos
            .Include(e => e.Categoria)
            .Include(e => e.Pessoa)
            .Include(e => e.Mes)
            .Where(e => e.MesId == mesId)
            .OrderBy(e => e.Data)
            .ToListAsync();

        List<ExtratoDTO> extratoExistente = new List<ExtratoDTO>();
        foreach (Extrato extrato in extratoEncontrado)
        {
            ExtratoDTO extratoDTO = ExtratoDTO.Map(extrato);
            extratoExistente.Add(extratoDTO);
        }

        return extratoExistente;
    }
}