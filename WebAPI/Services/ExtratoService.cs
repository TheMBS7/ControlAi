using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Entities;
using WebAPI.RequestModels;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services;

public class ExtratoService : IExtratoService
{
    private readonly ApplicationDbContext _context;

    public ExtratoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExtratoDTO>> CriarExtratosAsync(int id, ExtratoFixoCreateModel model)
    {
        Categoria? categoria = await _context.Categorias.FindAsync(model.CategoriaId);
        Pessoa? pessoa = await _context.Pessoas.FindAsync(model.PessoaId);
        DateTime dataTratada = new DateTime(model.Data.Year, model.Data.Month, 1, 0, 0, 0, DateTimeKind.Utc); // verificar de remover a hora do banco
        TipoMovimento? tipoMovimento = await _context.TiposMovimentos.FindAsync(model.TipoMovimentoId);
        List<Extrato> novosExtratos = new List<Extrato>();
        SaidaFixa? saidaFixa = await _context.SaidasFixas.FindAsync(id);
        EntradaFixa? entradaFixa = await _context.EntradasFixas.FindAsync(id);

        if (categoria == null || pessoa == null || tipoMovimento == null)
        {
            return Enumerable.Empty<ExtratoDTO>();
        }

        int quantidadeMeses = await _context.Meses.CountAsync(m => m.DataInicial > dataTratada);

        Mes? mesInicial = await _context.Meses
            .FirstOrDefaultAsync(m => m.DataInicial.Date == dataTratada.Date);


        Mes? mesAtual = mesInicial;
        DateTime dataAtual = model.Data;
        Guid idParcelas = Guid.NewGuid();

        for (int i = 1; i <= quantidadeMeses; i++)
        {
            if (mesAtual == null) break;

            Extrato extrato = new Extrato
            {
                Descricao = model.Descricao,
                ValorTotal = model.ValorTotal,
                Data = dataAtual,
                NumeroMaxParcelas = 1,
                NumeroParcela = 1,
                IdParcelas = idParcelas,
                Categoria = categoria,
                Pessoa = pessoa,
                Mes = mesAtual,
                TipoMovimento = tipoMovimento,
            };
            if (tipoMovimento.Id == 1)
            {
                extrato.EntradaFixa = entradaFixa;
            }
            else
            {
                extrato.SaidaFixa = saidaFixa;
            }

            novosExtratos.Add(extrato);
            mesAtual = await ObterProximoMesAsync(mesAtual);
            dataAtual = LidarProximaData(dataAtual);
        }
        _context.Extratos.AddRange(novosExtratos);
        await _context.SaveChangesAsync();

        List<ExtratoDTO> extratosCriados = new List<ExtratoDTO>();
        foreach (Extrato extrato in novosExtratos)
        {
            ExtratoDTO extratoDTO = ExtratoDTO.Map(extrato);
            extratosCriados.Add(extratoDTO);
        }

        return extratosCriados;
    }


    public async Task<IEnumerable<ExtratoDTO>> CriarExtratosAsync(ExtratoCreateModel model)
    {
        Categoria? categoria = await _context.Categorias.FindAsync(model.CategoriaId);
        Pessoa? pessoa = await _context.Pessoas.FindAsync(model.PessoaId);
        Mes? mesInicial = await _context.Meses.FindAsync(model.MesId);
        TipoMovimento? tipoMovimento = await _context.TiposMovimentos.FindAsync(model.TipoMovimentoId);
        List<Extrato> novosExtratos = new List<Extrato>();


        if (categoria == null || pessoa == null || mesInicial == null || tipoMovimento == null)
            return Enumerable.Empty<ExtratoDTO>();

        Mes? mesAtual = mesInicial;
        Guid idParcelas = Guid.NewGuid();
        DateTime dataAtual = model.Data;

        for (int i = 1; i <= model.NumeroMaxParcelas; i++)
        {
            if (mesAtual == null) break;

            Extrato extrato = new Extrato
            {
                Descricao = model.Descricao,
                ValorTotal = model.ValorTotal,
                Data = dataAtual,
                NumeroMaxParcelas = model.NumeroMaxParcelas,
                NumeroParcela = i,
                IdParcelas = idParcelas,
                Categoria = categoria,
                Pessoa = pessoa,
                Mes = mesAtual,
                TipoMovimento = tipoMovimento
            };

            novosExtratos.Add(extrato);
            mesAtual = await ObterProximoMesAsync(mesAtual);
            dataAtual = LidarProximaData(dataAtual); //verificar alternativa
        }

        _context.Extratos.AddRange(novosExtratos);
        await _context.SaveChangesAsync();

        List<ExtratoDTO> extratosCriados = new List<ExtratoDTO>();
        foreach (Extrato extrato in novosExtratos)
        {
            ExtratoDTO extratoDTO = ExtratoDTO.Map(extrato);
            extratosCriados.Add(extratoDTO);
        }

        return extratosCriados;
    }

    private DateTime LidarProximaData(DateTime dataAtual)
    {
        DateTime proximaData = dataAtual.AddMonths(1);

        return proximaData;
    }
    private async Task<Mes?> ObterProximoMesAsync(Mes mesAtual)
    {
        return await _context.Meses
            .Where(m => m.DataInicial > mesAtual.DataInicial)
            .OrderBy(m => m.DataInicial)
            .FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<ExtratoDTO>> EditarExtratoAsync(int id, int tipoMovimentoId, ExtratoFixoEditModel model)
    {
        List<Extrato> extratos = await _context.Extratos
            .Where(e => (tipoMovimentoId == 1 && e.EntradaFixaId == id) || (tipoMovimentoId == 2 && e.SaidaFixaId == id))
            .ToListAsync();

        if (!extratos.Any()) return [];

        DateTime dataAtual = model.Data;

        foreach (Extrato extrato in extratos)
        {
            if (extrato.Data.Year > model.Data.Year || extrato.Data.Year == model.Data.Year && extrato.Data.Month >= model.Data.Month)
            {
                extrato.Descricao = model.Descricao;
                extrato.ValorTotal = model.ValorTotal;
                extrato.Data = dataAtual;
                extrato.CategoriaId = model.CategoriaId;
                extrato.PessoaId = model.PessoaId;
            }
            dataAtual = LidarProximaData(dataAtual);
        }


        await _context.SaveChangesAsync();

        // List<ExtratoDTO> extratosDTOs = new List<ExtratoDTO>();
        // foreach (Extrato extrato in extratosEditados)
        // {
        //     ExtratoDTO extratoDTO = ExtratoDTO.Map(extrato);
        //     extratosDTOs.Add(extratoDTO);
        // }

        return extratos.Select(ExtratoDTO.Map).ToList();
    }
    public async Task<ExtratoDTO?> EditarExtratoAsync(int id, ExtratoEditModel model)
    {
        Extrato? extrato = await _context.Extratos.FirstOrDefaultAsync(i => i.Id == id);

        if (extrato == null) return null;

        extrato.Descricao = model.Descricao;
        extrato.ValorTotal = model.ValorTotal;
        extrato.Data = model.Data;
        extrato.NumeroMaxParcelas = model.NumeroMaxParcelas;
        extrato.NumeroParcela = model.NumeroParcela;
        extrato.CategoriaId = model.CategoriaId;
        extrato.PessoaId = model.PessoaId;

        await _context.SaveChangesAsync();

        return ExtratoDTO.Map(extrato);
    }

    public async Task<bool?> ExcluirExtratoAsync(int id, ExtratoDeleteModel model) //verificar se ? é a melhor alternativa para esses casos
    {
        Extrato? extrato = await _context.Extratos.FindAsync(id);

        if (extrato == null) return false;

        if (model.ExcluirParcelas) //verifica se selecionou o campo de excluir todas
        {
            List<Extrato> listaExtratos = await _context.Extratos
                .Where(i => i.IdParcelas == extrato.IdParcelas)
                .ToListAsync();

            if (!listaExtratos.Any()) return null;

            _context.Extratos.RemoveRange(listaExtratos);
            await _context.SaveChangesAsync();

            return true;
        }

        _context.Extratos.Remove(extrato);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool?> ExcluirExtratoFixosAsync(int id, int tipoMovimentoId)
    {
        List<Extrato> listaExtratos = await _context.Extratos
                .Where(e => (tipoMovimentoId == 1 && e.EntradaFixaId == id) || (tipoMovimentoId == 2 && e.SaidaFixaId == id))
                .ToListAsync();

        if (!listaExtratos.Any()) return false;

        _context.Extratos.RemoveRange(listaExtratos);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ExtratoDTO>> MostrarExtratosAsync()
    {
        List<Extrato> extratosEncontrados = await _context.Extratos
            .Include(e => e.Categoria)
            .Include(e => e.Pessoa)
            .Include(e => e.Mes)
            .OrderBy(e => e.Data)
            .ToListAsync();

        List<ExtratoDTO> extratosExistentes = new List<ExtratoDTO>();
        foreach (Extrato extrato in extratosEncontrados)
        {
            ExtratoDTO extratoDTO = ExtratoDTO.Map(extrato);
            extratosExistentes.Add(extratoDTO);
        }

        return extratosExistentes;
    }

    public async Task<IEnumerable<ExtratoDTO>> MostrarExtratoIdAsync(int mesId)
    {
        List<Extrato> extratoEncontrado = await _context.Extratos
            .Include(e => e.Categoria)
            .Include(e => e.Pessoa)
            .Include(e => e.Mes)
            .Where(e => e.MesId == mesId)
            .OrderBy(e => e.Data)
            .ToListAsync();

        List<ExtratoDTO> extratoExistente = new List<ExtratoDTO>();
        foreach (Extrato extrato in extratoEncontrado)
        {
            ExtratoDTO extratoDTO = ExtratoDTO.Map(extrato);
            extratoExistente.Add(extratoDTO);
        }

        return extratoExistente;
    }

    public async Task<IEnumerable<TotalPeriodo>> CalcularTotalAnoAsync(int ano)
    {
        List<Mes> mesesEncontrados = await _context.Meses
            .Where(m => m.DataInicial.Year == ano)
            .ToListAsync();

        List<TotalPeriodo> valoresTotais = new List<TotalPeriodo>();

        foreach (Mes mes in mesesEncontrados)
        {
            List<Extrato> extratosFiltrados = await _context.Extratos
                .Where(e => e.MesId == mes.Id)
                .ToListAsync();

            decimal totalMes = 0;

            foreach (Extrato extrato in extratosFiltrados)
            {
                if (extrato.TipoMovimentoId == 1)
                {
                    totalMes += extrato.ValorTotal;
                }
                else
                {
                    totalMes -= extrato.ValorTotal;
                }
            }

            valoresTotais.Add(new TotalPeriodo(mes.Id, totalMes));
        }

        return valoresTotais;
    }

    public async Task<TotalPeriodo> CalcularTotalMesIdAsync(int id)
    {
        List<Extrato> extratosEncontrados = await _context.Extratos
            .Where(e => e.MesId == id)
            .ToListAsync();

        decimal totalMes = 0;

        foreach (Extrato extrato in extratosEncontrados)
        {
            if (extrato.TipoMovimentoId == 1)
            {
                totalMes += extrato.ValorTotal;
            }
            else
            {
                totalMes -= extrato.ValorTotal;
            }
        }

        return new TotalPeriodo(id, totalMes);
    }
}