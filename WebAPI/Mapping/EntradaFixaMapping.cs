using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.Mapping;

internal class EntradaFixaMapping : IEntityTypeConfiguration<EntradaFixa>
{
    public void Configure(EntityTypeBuilder<EntradaFixa> builder)
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
            .Property(x => x.DataReferencia)
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