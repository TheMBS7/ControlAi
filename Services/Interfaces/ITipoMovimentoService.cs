using WebAPI.Entities;

namespace WebAPI.Services.Interfaces;

public interface ITipoMovimentoService
{
    public Task<IEnumerable<TiposMovimentoDTO>> MostrarMovimentosAsync();
}

public record TiposMovimentoDTO(int Id, string Tipo)
{
    public static TiposMovimentoDTO Map(TipoMovimento tipoMovimento)
    {
        return new TiposMovimentoDTO
        (
            tipoMovimento.Id,
            tipoMovimento.Tipo
        );
    }
}