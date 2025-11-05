using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;
using WebAPI.Services.Interfaces;


namespace WebAPI.Services;

public class SaidaFixaService : ISaidaFixaService
{
    private readonly ApplicationDbContext _context;
    private readonly IExtratoService _extratoService;

    public SaidaFixaService(ApplicationDbContext context, IExtratoService extratoService)
    {
        _context = context;
        _extratoService = extratoService;
    }

    public async Task<SaidaFixaDTO?> CriarSaidaFixaAsync(SaidaFixaCreateModel model)
    {
        SaidaFixa? repeteDescricao = await _context.SaidasFixas.FirstOrDefaultAsync(i => i.Descricao.ToLower() == model.Descricao.ToLower());
        Categoria? categoria = await _context.Categorias.FindAsync(model.CategoriaId);
        Pessoa? pessoa = await _context.Pessoas.FindAsync(model.PessoaId);

        if (categoria == null)
            return null;

        if (repeteDescricao != null)
        {
            return null;
        }

        if (pessoa == null)
        {
            return null;
        }

        SaidaFixa novaSaidaFixa = new SaidaFixa
        {
            Descricao = model.Descricao,
            Valor = model.Valor,
            DataVencimento = model.DataVencimento,
            Categoria = categoria,
            Pessoa = pessoa
        };

        _context.SaidasFixas.Add(novaSaidaFixa);
        await _context.SaveChangesAsync();

        int idSaidaFixa = novaSaidaFixa.Id;

        ExtratoFixoCreateModel novaModel = new ExtratoFixoCreateModel
        (
            model.Descricao,
            model.Valor,
            model.DataVencimento,
            categoria.Id,
            pessoa.Id
        );

        IEnumerable<ExtratoDTO> resultadoExtrato = await _extratoService.CriarExtratosAsync(idSaidaFixa, novaModel);

        if (resultadoExtrato == null)
        {
            return null;
        }

        return SaidaFixaDTO.Map(novaSaidaFixa);
    }
    public async Task<SaidaFixaDTO?> EditarSaidaFixaAsync(int id, SaidaFixaEditModel model)
    {
        SaidaFixa? saidaFixa = await _context.SaidasFixas.FirstOrDefaultAsync(i => i.Id == id);

        if (saidaFixa == null)
        {
            return null;
        }

        // Verifica se a descrição já está em uso
        if (model.Descricao != saidaFixa.Descricao)
        {
            bool descricaoExiste = await _context.SaidasFixas.AnyAsync(i => i.Descricao.ToLower() == model.Descricao.ToLower());
            if (descricaoExiste)
            {
                return null;
            }
        }

        saidaFixa.Descricao = model.Descricao;
        saidaFixa.Valor = model.Valor;
        saidaFixa.DataVencimento = model.DataVencimento;
        saidaFixa.CategoriaId = model.CategoriaId;
        saidaFixa.PessoaId = model.PessoaId;

        await _context.SaveChangesAsync();

        ExtratoFixoEditModel novaModel = new ExtratoFixoEditModel
        (
            model.Descricao,
            model.Valor,
            model.DataVencimento,
            model.CategoriaId,
            model.PessoaId
        );

        IEnumerable<ExtratoDTO?> resultadoExtrato = await _extratoService.EditarExtratoAsync(id, novaModel);

        if (!resultadoExtrato.Any())
        {
            return null;
        }


        return SaidaFixaDTO.Map(saidaFixa);
    }
    public async Task<bool> ExcluirSaidaFixaAsync(int id)
    {
        SaidaFixa? saidaFixa = await _context.SaidasFixas.FindAsync(id);

        if (saidaFixa == null)
            return false;

        bool? resultadoExtrato = await _extratoService.ExcluirExtratoFixosAsync(id);

        if (resultadoExtrato == false)
        {
            return false;
        }

        _context.SaidasFixas.Remove(saidaFixa);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<SaidaFixaDTO>> MostrarSaidasFixasAsync()
    {
        List<SaidaFixa> saidasFixasEncontradas = await _context.SaidasFixas
            .Include(e => e.Categoria)
            .OrderBy(saidaFixa => saidaFixa.DataVencimento)
            .ToListAsync();

        List<SaidaFixaDTO> saidasFixasMostrar = new List<SaidaFixaDTO>();
        foreach (SaidaFixa saidaFixa in saidasFixasEncontradas)
        {
            SaidaFixaDTO saidaFixaDTO = SaidaFixaDTO.Map(saidaFixa);
            saidasFixasMostrar.Add(saidaFixaDTO);
        }

        return saidasFixasMostrar;
    }

}