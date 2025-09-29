using WebAPI.Entities;
using WebAPI.RequestModels;

namespace WebAPI.Services.Interfaces;

public interface IExtratoService
{
    public Task<IEnumerable<ExtratoDTO>> CriarExtratosAsync(ExtratoCreateModel model);
    public Task<ExtratoDTO?> EditarExtratoAsync(int id, ExtratoEditModel model);
    public Task<bool?> ExcluirExtratoAsync(int id, ExtratoDeleteModel model);
    public Task<IEnumerable<ExtratoDTO>> MostrarExtratosAsync();
}

public record ExtratoDTO(int Id, string Descricao, decimal ValorTotal, DateTime Data, int NumeroMaxParcelas, int NumeroParcela, Guid IdParcelas, int CategoriaId, int PessoaId, int MesId)
{
    public static ExtratoDTO Map(Extrato extrato)
    {
        return new ExtratoDTO(
                extrato.Id,
                extrato.Descricao,
                extrato.ValorTotal,
                extrato.Data,
                extrato.NumeroMaxParcelas,
                extrato.NumeroParcela,
                extrato.IdParcelas,
                extrato.CategoriaId,
                extrato.PessoaId,
                extrato.MesId
            );
    }
}