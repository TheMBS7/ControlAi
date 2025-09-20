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
    public class ExtratosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExtratosController(ApplicationDbContext context)
        {
            _context = context;
            //verificar se não é mais facil injetar o FromServices aqui direto
        }

        [HttpPost("Create-Extratos")]
        public async Task<IActionResult> CriarExtratos(ExtratoCreateModel model, [FromServices] IExtratoService extratoService)
        {

            IEnumerable<ExtratoDTO> extratosCriados = await extratoService.CriarExtratosAsync(model);

            if (!extratosCriados.Any())
            {
                return NotFound("Dados preenchidos incorretamente.");
            }

            return Ok(extratosCriados);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> EditarExtrato(int id, ExtratoEditModel model, [FromServices] IExtratoService extratoService)
        {

            var extratoAtualizado = await extratoService.EditarExtratoAsync(id, model);

            if (extratoAtualizado == null)
            {
                return NotFound("Extrato não encontrado.");
            }

            return Ok(extratoAtualizado);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletarExtrato(int id, [FromServices] IExtratoService extratoService)
        {
            var resultadoDelete = await extratoService.ExcluirExtratoAsync(id);

            if (resultadoDelete == null)
                return NotFound("Extrato não encontrada.");

            return Ok();
        }

        [HttpGet("Display-Extratos")]
        public async Task<IActionResult> ExibirExtratos([FromServices] IExtratoService extratoService)
        {
            var extratos = await extratoService.MostrarExtratosAsync();

            if (!extratos.Any())
                return NotFound("Nenhum extrato encontrado.");

            return Ok(extratos);
        }

    }
}