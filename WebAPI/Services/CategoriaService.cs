using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ApplicationDbContext _context;

    public CategoriaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CategoriaDTO?> CriarCategoriaAsync(CategoriaCreateModel model)
    {
        bool repeteCategoria = await _context.Categorias.AnyAsync(i => i.Nome.ToLower() == model.Nome.ToLower());

        if (repeteCategoria)
        {
            return null;
        }

        Categoria novaCategoria = new Categoria
        {
            Nome = model.Nome
        };

        _context.Categorias.Add(novaCategoria);
        await _context.SaveChangesAsync();

        return CategoriaDTO.Map(novaCategoria);
    }
    public async Task<CategoriaDTO?> EditarCategoriaAsync(int id, CategoriaEditModel model)
    {
        Categoria? categoria = await _context.Categorias.FindAsync(id);

        if (categoria == null)
        {
            return null;
        }

        if (model.Nome != categoria.Nome)
        {
            bool nomeExiste = await _context.Categorias.AnyAsync(i => i.Nome.ToLower() == model.Nome.ToLower());
            if (nomeExiste)
            {
                return null; //dessa forma não da pra saber se não existe ou não
            }
        }

        categoria.Nome = model.Nome;

        await _context.SaveChangesAsync();

        return CategoriaDTO.Map(categoria);
    }
    public async Task<bool?> ExcluirCategoriaAsync(int id)
    {
        Categoria? categoria = await _context.Categorias.FindAsync(id);
        bool existeSaida = await _context.SaidasFixas.AnyAsync(saidaFixa => saidaFixa.CategoriaId == id);

        if (existeSaida)
        {
            return false;
        }

        if (categoria == null)
            return null;

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<CategoriaDTO>> MostrarCategoriasAsync()
    {
        List<Categoria> categoriasEncontradas = await _context.Categorias.ToListAsync();

        List<CategoriaDTO> categoriasExistente = new List<CategoriaDTO>();
        foreach (Categoria categoria in categoriasEncontradas)
        {
            CategoriaDTO categoriaDTO = CategoriaDTO.Map(categoria);
            categoriasExistente.Add(categoriaDTO);
        }

        return categoriasExistente;
    }

}