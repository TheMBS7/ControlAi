using WebAPI.Entities;

namespace WebAPI.Services.Interfaces;

public interface IMesService
{
    public Task<IEnumerable<MesDTO>> CriarPeriodoAsync();
    public Task<IEnumerable<MesDTO>> MostrarPeriodosAsync();
    public Task<MesDTO?> MostrarPeriodoIdAsync(int id);
}

public record MesDTO(int Id, string Descricao, DateTime DataInicial, DateTime DataFinal)
{
    public static MesDTO Map(Mes mes)
    {
        return new MesDTO
        (
            mes.Id,
            mes.Descricao,
            mes.DataInicial,
            mes.DataFinal
        );
    }
}