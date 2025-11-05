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

            ExtratoDTO? extratoAtualizado = await extratoService.EditarExtratoAsync(id, model);

            if (extratoAtualizado == null)
            {
                return NotFound("Extrato não encontrado.");
            }

            return Ok(extratoAtualizado);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletarExtrato(int id, ExtratoDeleteModel model, [FromServices] IExtratoService extratoService)
        {
            bool? resultadoDelete = await extratoService.ExcluirExtratoAsync(id, model);

            if (resultadoDelete == null)
                return NotFound("Extrato não encontrada.");

            return Ok();
        }

        [HttpGet("Display-Extratos")]
        public async Task<IActionResult> ExibirExtratos([FromServices] IExtratoService extratoService)
        {
            IEnumerable<ExtratoDTO> extratos = await extratoService.MostrarExtratosAsync();

            if (!extratos.Any())
                return NotFound("Nenhum extrato encontrado.");

            return Ok(extratos);
        }

        [HttpGet("Display-Extrato/{mesId}")]
        public async Task<IActionResult> ExibirExtratoId(int mesId, [FromServices] IExtratoService extratoService)
        {
            IEnumerable<ExtratoDTO> extrato = await extratoService.MostrarExtratoIdAsync(mesId);

            // if (!extrato.Any())
            //     return NotFound("Nenhum extrato encontrado.");

            return Ok(extrato);
        }

        [HttpGet("Display-Total/{ano}")]
        public async Task<IActionResult> ExibirTotalAno(int ano, [FromServices] IExtratoService extratoService)
        {
            IEnumerable<TotalPeriodo>? valoresTotais = await extratoService.CalcularTotalAnoAsync(ano);

            return Ok(valoresTotais);
        }

        [HttpGet("Display-Total-Mes/{id}")]
        public async Task<IActionResult> ExibirTotalMes(int id, [FromServices] IExtratoService extratoService)
        {
            TotalPeriodo? valorTotal = await extratoService.CalcularTotalMesIdAsync(id);

            return Ok(valorTotal);
        }
    }
}