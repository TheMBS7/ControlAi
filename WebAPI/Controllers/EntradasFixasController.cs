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
    public class EntradasFixasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EntradasFixasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create-EntradasFixas")]
        public async Task<IActionResult> CriarEntradasFixas(EntradaFixaCreateModel model, [FromServices] IEntradaFixaService entradaFixaService)
        {
            EntradaFixaDTO? entradaFixaCriada = await entradaFixaService.CriarEntradaFixaAsync(model);

            if (entradaFixaCriada == null)
            {
                return BadRequest("Erro ao criar entrada fixa.");
            }

            return Ok(entradaFixaCriada);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> EditarEntradaFixa(int id, EntradaFixaEditModel model, [FromServices] IEntradaFixaService entradaFixaService)
        {
            EntradaFixaDTO? entradaFixaEditada = await entradaFixaService.EditarEntradaFixaAsync(id, model);

            if (entradaFixaEditada == null)
            {
                return BadRequest("Erro ao editar entrada fixa.");
            }

            return Ok(entradaFixaEditada);
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletarEntradaFixa(int id, [FromServices] IEntradaFixaService entradaFixaService)
        {
            bool resultadoDelete = await entradaFixaService.ExcluirEntradaFixaAsync(id);

            if (!resultadoDelete)
            {
                BadRequest("Erro ao excluir entrada fixa.");
            }

            return Ok(new { message = "Entrada fixa excluída com sucesso" });

        }

        [HttpGet("Display-EntradasFixas")]
        public async Task<IActionResult> ExibirEntradasFixas([FromServices] IEntradaFixaService entradaFixaService)
        {
            IEnumerable<EntradaFixaDTO> entradaFixas = await entradaFixaService.MostrarEntradasFixasAsync();

            return Ok(entradaFixas);
        }
    }
}