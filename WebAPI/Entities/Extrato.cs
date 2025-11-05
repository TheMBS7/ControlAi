namespace WebAPI.Entities
{
    public class Extrato
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = default!;
        public decimal ValorTotal { get; set; }
        public DateTime Data { get; set; }
        public int NumeroMaxParcelas { get; set; }
        public int NumeroParcela { get; set; }
        public Guid IdParcelas { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = default!;
        public int PessoaId { get; set; }
        public Pessoa Pessoa { get; set; } = default!;
        public int MesId { get; set; }
        public Mes Mes { get; set; } = default!;
        public int TipoMovimentoId { get; set; }
        public TipoMovimento TipoMovimento { get; set; } = default!;
        public int? SaidaFixaId { get; set; }
        public SaidaFixa? SaidaFixa { get; set; }
        public int? EntradaFixaId { get; set; }
        public EntradaFixa? EntradaFixa { get; set; }
    }
}
