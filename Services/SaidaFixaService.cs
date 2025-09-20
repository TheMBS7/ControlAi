using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class SaidaFixaService : ISaidaFixaService
{
    private readonly ApplicationDbContext _context;

    public SaidaFixaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SaidaFixaDTO?> CriarSaidaFixaAsync(SaidaFixaCreateModel model)
    {
        SaidaFixa? repeteDescricao = await _context.SaidasFixas.FirstOrDefaultAsync(i => i.Descricao.ToLower() == model.Descricao.ToLower());
        Categoria? categoria = await _context.Categorias.FindAsync(model.CategoriaId);

        if (categoria == null)
            return null;

        if (repeteDescricao != null)
        {
            return null;
        }

        var novaSaidaFixa = new SaidaFixa
        {
            Descricao = model.Descricao,
            Valor = model.Valor,
            DataVencimento = model.DataVencimento,
            Categoria = categoria
        };

        _context.SaidasFixas.Add(novaSaidaFixa);
        await _context.SaveChangesAsync();

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
            var descricaoExiste = await _context.SaidasFixas.AnyAsync(i => i.Descricao.ToLower() == model.Descricao.ToLower());
            if (descricaoExiste)
            {
                return null;
            }
        }

        saidaFixa.Descricao = model.Descricao;
        saidaFixa.Valor = model.Valor;
        saidaFixa.DataVencimento = model.DataVencimento;
        saidaFixa.CategoriaId = model.CategoriaId;

        await _context.SaveChangesAsync();

        return SaidaFixaDTO.Map(saidaFixa);
    }
    public async Task<bool> ExcluirSaidaFixaAsync(int id)
    {
        var saidaFixa = await _context.SaidasFixas.FindAsync(id);

        if (saidaFixa == null)
            return false;

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