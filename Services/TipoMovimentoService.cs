
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class TipoMovimentoService : ITipoMovimentoService
{
    private readonly ApplicationDbContext _context;

    public TipoMovimentoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TiposMovimentoDTO>> MostrarMovimentosAsync()
    {
        List<TipoMovimento> tipoMovimentosEncontrados = await _context.TiposMovimentos.ToListAsync();

        List<TiposMovimentoDTO> mostrarTipoMovimento = new List<TiposMovimentoDTO>();
        foreach (TipoMovimento tipoMovimento in tipoMovimentosEncontrados)
        {
            TiposMovimentoDTO tiposMovimentoDTO = TiposMovimentoDTO.Map(tipoMovimento);
            mostrarTipoMovimento.Add(tiposMovimentoDTO);
        }

        return mostrarTipoMovimento;
    }
}