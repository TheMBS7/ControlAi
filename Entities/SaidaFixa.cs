namespace WebAPI.Entities
{
    public class SaidaFixa
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = default!;
        public decimal Valor { get; set; }
        public DateTime Vencimento { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = default!;
    }
}
