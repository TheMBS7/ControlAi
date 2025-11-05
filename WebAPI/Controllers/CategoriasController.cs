using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;
using WebAPI.Services.Interfaces;

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
        public async Task<IActionResult> CriarCategoria(CategoriaCreateModel model, [FromServices] ICategoriaService categoriaService)
        {
            CategoriaDTO? categoriaCriada = await categoriaService.CriarCategoriaAsync(model);

            if (categoriaCriada == null)
            {
                return BadRequest("Pessoa já existe.");
            }

            return Ok(categoriaCriada);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> EditarCategoria(int id, CategoriaEditModel model, [FromServices] ICategoriaService categoriaService)
        {
            CategoriaDTO? categoriaEditada = await categoriaService.EditarCategoriaAsync(id, model);

            if (categoriaEditada == null)
            {
                return BadRequest("Erro ao editar pessoa");
            }

            return Ok(categoriaEditada);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletarCategoria(int id, [FromServices] ICategoriaService categoriaService)
        {
            bool? resultadoDelete = await categoriaService.ExcluirCategoriaAsync(id);

            if (resultadoDelete == false)
            {
                return BadRequest("Ops! Esta categoria está vinculada a uma ou mais saídas e, por isso, não pode ser excluída.");
            }

            if (resultadoDelete == null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("Display-Categorias")]
        public async Task<IActionResult> ExibirCategorias([FromServices] ICategoriaService categoriaService)
        {
            IEnumerable<CategoriaDTO>? categorias = await categoriaService.MostrarCategoriasAsync();

            return Ok(categorias);
        }
    }
}