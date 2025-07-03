using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create-Categoria")]
        public async Task<IActionResult> CriarCategoria(CategoriaCreateModel model)
        {
            var repeteCategoria = await _context.Categorias.FirstOrDefaultAsync(i => i.Nome.ToLower() == model.Nome.ToLower());

            if (repeteCategoria != null)
            {
                return BadRequest("Categoria repetida.");
            }

            var novaCategoria = new Categoria
            {
                Nome = model.Nome
            };

            _context.Categorias.Add(novaCategoria);
            await _context.SaveChangesAsync();

            return Ok("Categoria criada com sucesso!");
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> EditarCategoria(int id, CategoriaEditModel model)
        {

            Categoria? categoria = await _context.Categorias.FirstOrDefaultAsync(i => i.Id == id);

            if (categoria == null)
            {
                return NotFound("Categoria não encontrado.");
            }

            // Verifica se a descrição já está em uso
            if (model.Nome != categoria.Nome)
            {
                var nomeExiste = await _context.Categorias.AnyAsync(i => i.Nome.ToLower() == model.Nome.ToLower());
                if (nomeExiste)
                {
                    return BadRequest("Categoria já existente.");
                }
            }

            categoria.Nome = model.Nome;

            await _context.SaveChangesAsync();

            return Ok("Categoria atualizada com sucesso.");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletarCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
                return NotFound("Categoria não encontrada.");

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return Ok("Categoria excluída.");
        }

        [HttpGet("Display-Categorias")]
        public async Task<IActionResult> ExibirCategorias()
        {
            var categorias = await _context.Categorias.ToListAsync();

            return Ok(categorias);
        }

    }
}