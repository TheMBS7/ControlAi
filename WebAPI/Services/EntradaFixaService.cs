using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class EntradaFixaService : IEntradaFixaService
{
    private readonly ApplicationDbContext _context;

    public EntradaFixaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EntradaFixaDTO?> CriarEntradaFixaAsync(EntradaFixaCreateModel model)
    {
        EntradaFixa? repeteDescricao = await _context.EntradasFixas.FirstOrDefaultAsync(i => i.Descricao.ToLower() == model.Descricao.ToLower());

        if (repeteDescricao != null)
        {
            return null;
        }

        EntradaFixa novaEntradaFixa = new EntradaFixa
        {
            Descricao = model.Descricao,
            Valor = model.Valor,
            DataReferencia = model.DataReferencia
        };

        _context.EntradasFixas.Add(novaEntradaFixa);
        await _context.SaveChangesAsync();

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

        await _context.SaveChangesAsync();

        return EntradaFixaDTO.Map(entradaFixa);
    }
    public async Task<bool> ExcluirEntradaFixaAsync(int id)
    {
        EntradaFixa? entradaFixa = await _context.EntradasFixas.FindAsync(id);

        if (entradaFixa == null)
            return false;

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