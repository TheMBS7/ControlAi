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
    public class SaidasFixasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SaidasFixasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create-SaidasFixas")]
        public async Task<IActionResult> CriarSaidasFixas(SaidaFixaCreateModel model, [FromServices] ISaidaFixaService saidaFixaService)
        {
            SaidaFixaDTO? saidaFixaCriada = await saidaFixaService.CriarSaidaFixaAsync(model);

            if (saidaFixaCriada == null)
            {
                return BadRequest("Erro ao criar saída fixa");
            }

            return Ok(saidaFixaCriada);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> EditarSaidaFixa(int id, SaidaFixaEditModel model, [FromServices] ISaidaFixaService saidaFixaService)
        {

            SaidaFixaDTO? saidaFixaEditada = await saidaFixaService.EditarSaidaFixaAsync(id, model);

            if (saidaFixaEditada == null)
            {
                return BadRequest("Erro ao editar Saida Fixa");
            }

            return Ok(saidaFixaEditada);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletarSaidaFixa(int id, [FromServices] ISaidaFixaService saidaFixaService)
        {
            bool resultadoDelete = await saidaFixaService.ExcluirSaidaFixaAsync(id);

            if (!resultadoDelete)
                return NotFound("Saída não encontrada.");

            return Ok();
        }

        [HttpGet("Display-SaidasFixas")]
        public async Task<IActionResult> ExibirSaidasFixas([FromServices] ISaidaFixaService saidaFixaService)
        {
            IEnumerable<SaidaFixaDTO> saidasFixas = await saidaFixaService.MostrarSaidasFixasAsync();

            return Ok(saidasFixas);
        }

    }
}