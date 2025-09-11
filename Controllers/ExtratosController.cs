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
        public async Task<IActionResult> EditarExtrato(int id, ExtratoEditModel model)
        {

            Extrato? extrato = await _context.Extratos.FirstOrDefaultAsync(i => i.Id == id);

            if (extrato == null)
            {
                return NotFound("Extrato não encontrado.");
            }

            extrato.Descricao = model.Descricao;
            extrato.ValorTotal = model.Valor;
            extrato.Data = model.Data;
            extrato.NumeroMaxParcelas = model.NumeroMaxParcelas;
            extrato.NumeroParcela = model.NumeroParcela;
            extrato.CategoriaId = model.CategoriaId;
            extrato.PessoaId = model.PessoaId;

            await _context.SaveChangesAsync();

            return Ok(extrato);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletarExtrato(int id)
        {
            var extrato = await _context.Extratos.FindAsync(id);

            if (extrato == null)
                return NotFound("Extrato não encontrada.");

            _context.Extratos.Remove(extrato);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("Display-Extratos")]
        public async Task<IActionResult> ExibirExtratos()
        {
            var extratos = await _context.Extratos
                .Include(e => e.Categoria)
                .Include(e => e.Pessoa)
                .Include(e => e.Mes)
                .OrderBy(e => e.Data)
                .ToListAsync();

            return Ok(extratos);
        }



    }
}