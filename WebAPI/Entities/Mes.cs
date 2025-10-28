namespace WebAPI.Entities;

public class Mes
{
    public int Id { get; set; }
    public string Descricao { get; set; } = default!;
    public DateTime DataInicial { get; set; }
    public DateTime DataFinal { get; set; }
}
