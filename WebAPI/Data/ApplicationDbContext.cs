namespace WebAPI.Data
{
    using Microsoft.EntityFrameworkCore;
    using WebAPI.Entities;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Extrato> Extratos { get; set; }
        public DbSet<EntradaFixa> EntradasFixas { get; set; }
        public DbSet<SaidaFixa> SaidasFixas { get; set; }
        public DbSet<Mes> Meses { get; set; }
        public DbSet<TipoMovimento> TiposMovimentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
