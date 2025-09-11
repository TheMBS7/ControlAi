using System.Reflection.Metadata;
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


}