using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
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
        var categoria = await _context.Categorias.FindAsync(model.CategoriaId);
        var pessoa = await _context.Pessoas.FindAsync(model.PessoaId);
        var mesInicial = await _context.Meses.FindAsync(model.MesId);
        var novosExtratos = new List<Extrato>();
        //var periodoAtual = mes;

        if (categoria == null || pessoa == null || mesInicial == null)
            return Enumerable.Empty<ExtratoDTO>();

        var mesAtual = mesInicial;

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
                Categoria = categoria,
                Pessoa = pessoa,
                Mes = mesAtual
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
        extrato.ValorTotal = model.Valor;
        extrato.Data = model.Data;
        extrato.NumeroMaxParcelas = model.NumeroMaxParcelas;
        extrato.NumeroParcela = model.NumeroParcela;
        extrato.CategoriaId = model.CategoriaId;
        extrato.PessoaId = model.PessoaId;

        await _context.SaveChangesAsync();

        return ExtratoDTO.Map(extrato);
    }

    public async Task<bool?> ExcluirExtratoAsync(int id) //verificar se ? é a melhor alternativa para esses casos
    {
        var extrato = await _context.Extratos.FindAsync(id);

        if (extrato == null) return false;

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
}