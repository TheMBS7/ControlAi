using WebAPI.Entities;
using WebAPI.RequestModels;

namespace WebAPI.Services.Interfaces;

public interface ISaidaFixaService
{
    public Task<SaidaFixaDTO?> CriarSaidaFixaAsync(SaidaFixaCreateModel model);
    public Task<SaidaFixaDTO?> EditarSaidaFixaAsync(int id, SaidaFixaEditModel model);
    public Task<bool> ExcluirSaidaFixaAsync(int id);
    public Task<IEnumerable<SaidaFixaDTO>> MostrarSaidasFixasAsync();
}

public record SaidaFixaDTO(int Id, string Descricao, decimal Valor, DateTime DataVencimento, int CategoriaId)
{
    public static SaidaFixaDTO Map(SaidaFixa saidaFixa)
    {
        return new SaidaFixaDTO(
            saidaFixa.Id,
            saidaFixa.Descricao,
            saidaFixa.Valor,
            saidaFixa.DataVencimento,
            saidaFixa.CategoriaId
            );
    }
}