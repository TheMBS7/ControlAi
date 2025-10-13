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
    public class TiposMovimentosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TiposMovimentosController(ApplicationDbContext context)
        {
            _context = context;

        }

        [HttpGet("Display-Movimentos")]
        public async Task<IActionResult> MostrarMovimentos([FromServices] ITipoMovimentoService tipoMovimentoService)
        {
            IEnumerable<TiposMovimentoDTO> tiposMovimentosEncontrados = await tipoMovimentoService.MostrarMovimentosAsync();

            if (!tiposMovimentosEncontrados.Any())
            {
                return BadRequest("Erro ao mostrar movimentos");
            }

            return Ok(tiposMovimentosEncontrados);
        }
    }
}