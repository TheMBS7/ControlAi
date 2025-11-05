using WebAPI.Entities;
using WebAPI.RequestModels;
using static WebAPI.Services.ExtratoService;

namespace WebAPI.Services.Interfaces;

public interface IExtratoService
{
    public Task<IEnumerable<ExtratoDTO>> CriarExtratosAsync(int idSaidaFixa, ExtratoFixoCreateModel model);
    public Task<IEnumerable<ExtratoDTO>> CriarExtratosAsync(ExtratoCreateModel model);
    public Task<ExtratoDTO?> EditarExtratoAsync(int id, ExtratoEditModel model);
    public Task<IEnumerable<ExtratoDTO>> EditarExtratoAsync(int id, ExtratoFixoEditModel model);
    public Task<bool?> ExcluirExtratoAsync(int id, ExtratoDeleteModel model);
    public Task<bool?> ExcluirExtratoFixosAsync(int saidaFixaId);
    public Task<IEnumerable<ExtratoDTO>> MostrarExtratosAsync();
    public Task<IEnumerable<ExtratoDTO>> MostrarExtratoIdAsync(int mesId);
    public Task<IEnumerable<TotalPeriodo>> CalcularTotalAnoAsync(int ano);
    public Task<TotalPeriodo> CalcularTotalMesIdAsync(int id);
}

public record TotalPeriodo(int MesId, decimal TotalMes);
public record ExtratoDTO(int Id, string Descricao, decimal ValorTotal, DateTime Data, int NumeroMaxParcelas, int NumeroParcela, Guid IdParcelas, int CategoriaId, int PessoaId, int MesId, int TipoMovimentoId)
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
                extrato.MesId,
                extrato.TipoMovimentoId
            );
    }
}