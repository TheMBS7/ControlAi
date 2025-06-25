using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;               
using WebAPI.Entities;           
using WebAPI.RequestModels; 

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
        public async Task<IActionResult> CriarExtratos(ExtratoCreateModel model)
        {
            var categoria = await _context.Categorias.FindAsync(model.CategoriaId);
            var pessoa = await _context.Pessoas.FindAsync(model.PessoaId);

            if (categoria == null || pessoa == null)
                return NotFound("Dados preenchidos incorretamente.");


            var novoExtrato = new Extrato
            {
                Descricao = model.Descricao,
                ValorTotal = model.Valor,
                Data = model.Data,
                NumeroParcelas = model.NumeroParcelas,
                Categoria = categoria,
                Pessoa = pessoa
            };

            _context.Extratos.Add(novoExtrato);
            await _context.SaveChangesAsync();

            return Ok("sucesso!");
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
            extrato.NumeroParcelas = model.NumeroParcelas;
            extrato.CategoriaId = model.CategoriaId;
            extrato.PessoaId = model.PessoaId;

            await _context.SaveChangesAsync();

            return Ok("Extrato atualizado com sucesso.");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeletarExtrato(int id)
        {
            var extrato = await _context.Extratos.FindAsync(id);

            if (extrato == null)
                return NotFound("Extrato não encontrada.");

            _context.Extratos.Remove(extrato);
            await _context.SaveChangesAsync();

            return Ok("Extrato excluído.");
        }
 
    }
}