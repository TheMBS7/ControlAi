namespace WebAPI.Entities
{
    public class Extrato
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = default!;
        public decimal ValorTotal { get; set; }
        public DateTime Data { get; set; }
        public int NumeroParcelas { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = default!;
        public int PessoaId { get; set; }
        public Pessoa Pessoa { get; set; } = default!;
        
    }
}
