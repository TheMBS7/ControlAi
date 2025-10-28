using WebAPI.Entities;
using WebAPI.RequestModels;

namespace WebAPI.Services.Interfaces;

public interface IPessoaService
{
    public Task<PessoaDTO?> CriarPessoaAsync(PessoaCreateModel model);
    public Task<PessoaDTO?> EditarPessoaAsync(int id, PessoaEditModel model);
    public Task<bool> ExcluirPessoaAsync(int id);
    public Task<IEnumerable<PessoaDTO>> MostrarPessoasAsync();
}

public record PessoaDTO(int Id, string Nome)
{
    public static PessoaDTO Map(Pessoa pessoa)
    {
        return new PessoaDTO(
                pessoa.Id,
                pessoa.Nome
            );
    }
}