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
    public class MesesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MesesController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpPost("Criar-Periodos")]
        public async Task<IActionResult> CriarPeriodos([FromServices] IMesService mesService)
        {
            IEnumerable<MesDTO> mesesCriados = await mesService.CriarPeriodoAsync();

            if (!mesesCriados.Any())
            {
                return BadRequest("Erro ao criar periodo");
            }

            return Ok(mesesCriados);
        }

        [HttpGet("Display-Periodos")]
        public async Task<IActionResult> MostrarPeriodos([FromServices] IMesService mesService)
        {
            IEnumerable<MesDTO> mesesEncontrados = await mesService.MostrarPeriodosAsync();

            if (!mesesEncontrados.Any())
            {
                return BadRequest("Erro ao mostrar periodos");
            }

            return Ok(mesesEncontrados);
        }

        [HttpGet("Display-Periodo/{id}")]
        public async Task<IActionResult> ExibirExtratoId(int id, [FromServices] IMesService mesService)
        {
            MesDTO? periodoEncontrado = await mesService.MostrarPeriodoIdAsync(id);

            if (periodoEncontrado == null)
                return NotFound("Periodo não encontrado.");

            return Ok(periodoEncontrado);
        }
    }
}