using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class EntradaFixaService : IEntradaFixaService
{
    private readonly ApplicationDbContext _context;
    private readonly IExtratoService _extratoService;

    public EntradaFixaService(ApplicationDbContext context, IExtratoService extratoService)
    {
        _context = context;
        _extratoService = extratoService;
    }

    public async Task<EntradaFixaDTO?> CriarEntradaFixaAsync(EntradaFixaCreateModel model)
    {
        EntradaFixa? repeteDescricao = await _context.EntradasFixas.FirstOrDefaultAsync(i => i.Descricao.ToLower() == model.Descricao.ToLower());
        Categoria? categoria = await _context.Categorias.FindAsync(model.CategoriaId);
        Pessoa? pessoa = await _context.Pessoas.FindAsync(model.PessoaId);

        if (categoria == null || repeteDescricao != null || pessoa == null)
        {
            return null;

        }

        EntradaFixa novaEntradaFixa = new EntradaFixa
        {
            Descricao = model.Descricao,
            Valor = model.Valor,
            DataReferencia = model.DataReferencia,
            Categoria = categoria,
            Pessoa = pessoa
        };

        _context.EntradasFixas.Add(novaEntradaFixa);
        await _context.SaveChangesAsync();

        int id = novaEntradaFixa.Id;

        ExtratoFixoCreateModel novaModel = new ExtratoFixoCreateModel
        (
            model.Descricao,
            model.Valor,
            model.DataReferencia,
            categoria.Id,
            pessoa.Id,
            1
        );

        IEnumerable<ExtratoDTO> resultadoExtrato = await _extratoService.CriarExtratosAsync(id, novaModel);

        if (resultadoExtrato == null)
        {
            return null;
        }

        return EntradaFixaDTO.Map(novaEntradaFixa);
    }
    public async Task<EntradaFixaDTO?> EditarEntradaFixaAsync(int id, EntradaFixaEditModel model)
    {

        EntradaFixa? entradaFixa = await _context.EntradasFixas.FirstOrDefaultAsync(i => i.Id == id);

        if (entradaFixa == null)
        {
            return null;
        }

        // Verifica se a descrição já está em uso
        if (model.Descricao != entradaFixa.Descricao)
        {
            bool descricaoExiste = await _context.EntradasFixas.AnyAsync(i => i.Descricao.ToLower() == model.Descricao.ToLower());

            if (descricaoExiste)
            {
                return null;
            }
        }

        entradaFixa.Descricao = model.Descricao;
        entradaFixa.Valor = model.Valor;
        entradaFixa.DataReferencia = model.DataReferencia;
        entradaFixa.CategoriaId = model.CategoriaId;
        entradaFixa.PessoaId = model.PessoaId;

        await _context.SaveChangesAsync();

        ExtratoFixoEditModel novaModel = new ExtratoFixoEditModel
        (
            model.Descricao,
            model.Valor,
            model.DataReferencia,
            model.CategoriaId,
            model.PessoaId
        );

        IEnumerable<ExtratoDTO?> resultadoExtrato = await _extratoService.EditarExtratoAsync(id, 1, novaModel);

        if (!resultadoExtrato.Any())
        {
            return null;
        }

        return EntradaFixaDTO.Map(entradaFixa);
    }
    public async Task<bool> ExcluirEntradaFixaAsync(int id)
    {
        EntradaFixa? entradaFixa = await _context.EntradasFixas.FindAsync(id);

        if (entradaFixa == null)
            return false;

        bool? resultadoExtrato = await _extratoService.ExcluirExtratoFixosAsync(id, 1);

        if (resultadoExtrato == false)
        {
            return false;
        }

        _context.EntradasFixas.Remove(entradaFixa);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<EntradaFixaDTO>> MostrarEntradasFixasAsync()
    {
        List<EntradaFixa> entradaFixasEncontradas = await _context.EntradasFixas
               .OrderBy(entradaFixa => entradaFixa.DataReferencia)
               .ToListAsync();


        List<EntradaFixaDTO> entradasFixasMostrar = new List<EntradaFixaDTO>();
        foreach (EntradaFixa entradaFixa in entradaFixasEncontradas)
        {
            EntradaFixaDTO entradaFixaDTO = EntradaFixaDTO.Map(entradaFixa);
            entradasFixasMostrar.Add(entradaFixaDTO);
        }

        return entradasFixasMostrar;
    }

}