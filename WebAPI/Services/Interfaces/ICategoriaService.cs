using WebAPI.Entities;
using WebAPI.RequestModels;

namespace WebAPI.Services.Interfaces;

public interface ICategoriaService
{
    public Task<CategoriaDTO?> CriarCategoriaAsync(CategoriaCreateModel model);
    public Task<CategoriaDTO?> EditarCategoriaAsync(int id, CategoriaEditModel model);
    public Task<bool?> ExcluirCategoriaAsync(int id);
    public Task<IEnumerable<CategoriaDTO>> MostrarCategoriasAsync();
}

public record CategoriaDTO(int Id, string Nome)
{
    public static CategoriaDTO Map(Categoria categoria)
    {
        return new CategoriaDTO(
                categoria.Id,
                categoria.Nome
            );
    }
}