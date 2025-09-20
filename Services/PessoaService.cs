using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class PessoaService : IPessoaService
{
    private readonly ApplicationDbContext _context;

    public PessoaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PessoaDTO?> CriarPessoaAsync(PessoaCreateModel model)
    {
        var repetePessoa = await _context.Pessoas.AnyAsync(i => i.Nome.ToLower() == model.Nome.ToLower());

        if (repetePessoa)
        {
            return null;
        }

        var novaPessoa = new Pessoa
        {
            Nome = model.Nome
        };

        _context.Pessoas.Add(novaPessoa);
        await _context.SaveChangesAsync();

        return PessoaDTO.Map(novaPessoa);
    }

    public async Task<PessoaDTO?> EditarPessoaAsync(int id, PessoaEditModel model)
    {
        Pessoa? pessoa = await _context.Pessoas.FindAsync(id);

        if (pessoa == null)
        {
            return null;
        }

        if (model.Nome != pessoa.Nome)
        {
            bool nomeExiste = await _context.Pessoas.AnyAsync(i => i.Nome.ToLower() == model.Nome.ToLower());
            if (nomeExiste)
            {
                return null; //dessa forma não da pra saber se não existe ou não
            }
        }

        pessoa.Nome = model.Nome;

        await _context.SaveChangesAsync();

        return PessoaDTO.Map(pessoa);
    }

    public async Task<bool> ExcluirPessoaAsync(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);

        if (pessoa == null)
            return false;

        _context.Pessoas.Remove(pessoa);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<PessoaDTO>> MostrarPessoasAsync()
    {
        List<Pessoa> pessoasEncontradas = await _context.Pessoas.ToListAsync();

        List<PessoaDTO> pessoasExistente = new List<PessoaDTO>();
        foreach (Pessoa pessoa in pessoasEncontradas)
        {
            PessoaDTO pessoaDTO = PessoaDTO.Map(pessoa);
            pessoasExistente.Add(pessoaDTO);
        }

        return pessoasExistente;
    }

}