using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WebAPI.Entities;


namespace WebAPI.RequestModels;

//Criação
public record CategoriaCreateModel([Required] string Nome);
public record EntradaFixaCreateModel([Required(ErrorMessage = "A descrição é obrigatória.")] string Descricao, [Required] decimal Valor, [Required] DateTime DataReferencia, int CategoriaId, int PessoaId);
public record SaidaFixaCreateModel([Required] string Descricao, [Required] decimal Valor, [Required] DateTime DataVencimento, [Required] int CategoriaId, [Required] int PessoaId);
public record PessoaCreateModel([Required] string Nome);
public record ExtratoCreateModel([Required(ErrorMessage = "A descrição é obrigatória.")] string Descricao, [Required] decimal ValorTotal, [Required] DateTime Data, [Required][Range(1, int.MaxValue, ErrorMessage = "Número de parcelas inválido.")] int NumeroParcela, int NumeroMaxParcelas, [Required] int CategoriaId, [Required] int PessoaId, int MesId, int TipoMovimentoId);
public record ExtratoFixoCreateModel([Required] string Descricao, [Required] decimal ValorTotal, [Required] DateTime Data, [Required] int CategoriaId, [Required] int PessoaId, int TipoMovimentoId);

//Edição
public record CategoriaEditModel(string Nome);
public record EntradaFixaEditModel(string Descricao, decimal Valor, DateTime DataReferencia, int CategoriaId, int PessoaId);
public record SaidaFixaEditModel(string Descricao, decimal Valor, DateTime DataVencimento, int CategoriaId, int PessoaId);
public record PessoaEditModel(string Nome);
public record ExtratoEditModel(string Descricao, decimal ValorTotal, DateTime Data, [Range(1, int.MaxValue, ErrorMessage = "Número de parcelas inválido.")] int NumeroParcela, int NumeroMaxParcelas, int CategoriaId, int PessoaId);
public record ExtratoFixoEditModel([Required] string Descricao, [Required] decimal ValorTotal, [Required] DateTime Data, [Required] int CategoriaId, [Required] int PessoaId);

//Exclusão
public record ExtratoDeleteModel(bool ExcluirParcelas);