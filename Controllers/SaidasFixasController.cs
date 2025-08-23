using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;

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
        public async Task<IActionResult> CriarSaidasFixas(SaidaFixaCreateModel model)
        {
            var repeteDescricao = await _context.SaidasFixas.FirstOrDefaultAsync(i => i.Descricao.ToLower() == model.Descricao.ToLower());
            var categoria = await _context.Categorias.FindAsync(model.CategoriaId);

            if (categoria == null)
                return NotFound("Categoria não encontrada.");

            if (repeteDescricao != null)
            {
                return BadRequest("Descrição repetida.");
            }

            var novaSaidaFixa = new SaidaFixa
            {
                Descricao = model.Descricao,
                Valor = model.Valor,
                DataVencimento = model.DataVencimento,
                Categoria = categoria
            };

            _context.SaidasFixas.Add(novaSaidaFixa);
            await _context.SaveChangesAsync();

            return Ok(novaSaidaFixa);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> EditarSaidaFixa(int id, SaidaFixaEditModel model)
        {

            SaidaFixa? saidaFixa = await _context.SaidasFixas.FirstOrDefaultAsync(i => i.Id == id);

            if (saidaFixa == null)
            {
                return NotFound("Saída não encontrado.");
            }

            // Verifica se a descrição já está em uso
            if (model.Descricao != saidaFixa.Descricao)
            {
                var descricaoExiste = await _context.SaidasFixas.AnyAsync(i => i.Descricao.ToLower() == model.Descricao.ToLower());
                if (descricaoExiste)
                {
                    return BadRequest("Saída já existente.");
                }
            }

            saidaFixa.Descricao = model.Descricao;
            saidaFixa.Valor = model.Valor;
            saidaFixa.DataVencimento = model.DataVencimento;
            saidaFixa.CategoriaId = model.CategoriaId;

            await _context.SaveChangesAsync();

            return Ok(saidaFixa);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletarSaidaFixa(int id)
        {
            var saidaFixa = await _context.SaidasFixas.FindAsync(id);

            if (saidaFixa == null)
                return NotFound("Saída não encontrada.");

            _context.SaidasFixas.Remove(saidaFixa);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("Display-SaidasFixas")]
        public async Task<IActionResult> ExibirSaidasFixas()
        {
            var saidaFixas = await _context.SaidasFixas
            .Include(e => e.Categoria)
            .OrderBy(saidaFixa => saidaFixa.DataVencimento)
            .ToListAsync();

            return Ok(saidaFixas);
        }

    }
}