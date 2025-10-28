using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;
using WebAPI.Services;
using WebAPI.Services.Interfaces;

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
        public async Task<IActionResult> CriarPessoas(PessoaCreateModel model, [FromServices] IPessoaService pessoaService)
        {
            var pessoaCriada = await pessoaService.CriarPessoaAsync(model);

            if (pessoaCriada == null)
            {
                return BadRequest("Pessoa já existe.");
            }

            return Ok(pessoaCriada);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> EditarPessoa(int id, PessoaEditModel model, [FromServices] IPessoaService pessoaService)
        {
            var pessoaEditada = await pessoaService.EditarPessoaAsync(id, model);

            if (pessoaEditada == null)
            {
                return BadRequest("Erro ao editar pessoa");
            }

            return Ok(pessoaEditada);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletarPessoa(int id, [FromServices] IPessoaService pessoaService)
        {
            bool resultadoDelete = await pessoaService.ExcluirPessoaAsync(id);

            if (!resultadoDelete)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("Display-Pessoas")]
        public async Task<IActionResult> ExibirPessoas([FromServices] IPessoaService pessoaService)
        {
            var pessoas = await pessoaService.MostrarPessoasAsync();

            return Ok(pessoas);
        }

    }
}