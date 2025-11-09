using WebAPI.Entities;
using WebAPI.RequestModels;

namespace WebAPI.Services.Interfaces;

public interface IEntradaFixaService
{
    public Task<EntradaFixaDTO?> CriarEntradaFixaAsync(EntradaFixaCreateModel model);
    public Task<EntradaFixaDTO?> EditarEntradaFixaAsync(int id, EntradaFixaEditModel model);
    public Task<bool> ExcluirEntradaFixaAsync(int id);
    public Task<IEnumerable<EntradaFixaDTO>> MostrarEntradasFixasAsync();
}

public record EntradaFixaDTO(int Id, string Descricao, decimal Valor, DateTime DataReferencia, int CategoriaId, int PessoaId)
{
    public static EntradaFixaDTO Map(EntradaFixa entradaFixa)
    {
        return new EntradaFixaDTO(
            entradaFixa.Id,
            entradaFixa.Descricao,
            entradaFixa.Valor,
            entradaFixa.DataReferencia,
            entradaFixa.CategoriaId,
            entradaFixa.PessoaId
            );
    }
}