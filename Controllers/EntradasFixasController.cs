using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;

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
        public async Task<IActionResult> CriarEntradasFixas(EntradaFixaCreateModel model)
        {
            var repeteDescricao = await _context.EntradasFixas.FirstOrDefaultAsync(i => i.Descricao.ToLower() == model.Descricao.ToLower());

            if (repeteDescricao != null)
            {
                return BadRequest("Descrição repetida.");
            }

            var novaEntradaFixa = new EntradaFixa
            {
                Descricao = model.Descricao,
                Valor = model.Valor,
                DataReferencia = model.DataReferencia
            };

            _context.EntradasFixas.Add(novaEntradaFixa);
            await _context.SaveChangesAsync();

            return Ok("Entrada criada com sucesso!");
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> EditarEntradaFixa(int id, EntradaFixaEditModel model)
        {

            EntradaFixa? entradaFixa = await _context.EntradasFixas.FirstOrDefaultAsync(i => i.Id == id);

            if (entradaFixa == null)
            {
                return NotFound("Entrada não encontrado.");
            }

            // Verifica se a descrição já está em uso
            if (model.Descricao != entradaFixa.Descricao)
            {
                var descricaoExiste = await _context.EntradasFixas.AnyAsync(i => i.Descricao.ToLower() == model.Descricao.ToLower());
                if (descricaoExiste)
                {
                    return BadRequest("Entrada já existente.");
                }
            }

            entradaFixa.Descricao = model.Descricao;
            entradaFixa.Valor = model.Valor;
            entradaFixa.DataReferencia = model.DataReferencia;

            await _context.SaveChangesAsync();

            return Ok("Entrada atualizada com sucesso.");
        }
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletarEntradaFixa(int id)
        {
            var entradaFixa = await _context.EntradasFixas.FindAsync(id);

            if (entradaFixa == null)
                return NotFound("Entrada não encontrada.");

            _context.EntradasFixas.Remove(entradaFixa);
            await _context.SaveChangesAsync();

            return Ok("Entrada excluída.");
        }

        [HttpGet("Dsplay-EntradasFixas")]
        public async Task<IActionResult> ExibirEntradasFixas()
        {
            var entradaFixas = await _context.EntradasFixas.ToListAsync();

            return Ok(entradaFixas);
        }
    }
}