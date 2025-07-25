using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PessoasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create-Pessoa")]
        public async Task<IActionResult> CriarPessoas(PessoaCreateModel model)
        {
            var repetePessoa = await _context.Pessoas.FirstOrDefaultAsync(i => i.Nome.ToLower() == model.Nome.ToLower());

            if (repetePessoa != null)
            {
                return BadRequest("Pessoa repetida.");
            }

            var novaPessoa = new Pessoa
            {
                Nome = model.Nome
            };

            _context.Pessoas.Add(novaPessoa);
            await _context.SaveChangesAsync();

            return Ok(novaPessoa);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> EditarPessoa(int id, PessoaEditModel model)
        {

            Pessoa? pessoa = await _context.Pessoas.FirstOrDefaultAsync(i => i.Id == id);

            if (pessoa == null)
            {
                return NotFound("Pessoa não encontrado.");
            }

            // Verifica se a descrição já está em uso
            if (model.Nome != pessoa.Nome)
            {
                var nomeExiste = await _context.Pessoas.AnyAsync(i => i.Nome.ToLower() == model.Nome.ToLower());
                if (nomeExiste)
                {
                    return BadRequest("Pessoa já existente.");
                }
            }

            pessoa.Nome = model.Nome;

            await _context.SaveChangesAsync();

            return Ok(pessoa);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletarPessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);

            if (pessoa == null)
                return NotFound("Pessoa não encontrada.");

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return Ok(pessoa);
        }

        [HttpGet("Display-Pessoas")]
        public async Task<IActionResult> ExibirPessoas()
        {
            var pessoas = await _context.Pessoas.ToListAsync();

            return Ok(pessoas);
        }



    }
}