using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.Mapping;

internal class SaidaFixaFixaMapping : IEntityTypeConfiguration<SaidaFixa>
{
    public void Configure(EntityTypeBuilder<SaidaFixa> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Descricao)
            .IsRequired();

        builder
            .Property(x => x.Valor)
            .IsRequired();

        builder
            .Property(x => x.DataVencimento)
            .IsRequired();

        builder
            .HasOne(x => x.Categoria)
            .WithMany()
            .HasForeignKey(x => x.CategoriaId)
            .IsRequired();

        builder
            .HasOne(x => x.Pessoa)
            .WithMany()
            .HasForeignKey(x => x.PessoaId)
            .IsRequired();
    }

}