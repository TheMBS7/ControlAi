namespace WebAPI.Entities
{
    public class EntradaFixa
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = default!;
        public decimal Valor { get; set; }
        public DateTime DataReferencia { get; set; }
    }
}
