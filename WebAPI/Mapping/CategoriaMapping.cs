using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.Mapping;

internal class CategoriaMapping : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Nome)
            .IsRequired();
    }//colocar riqueride em tudo

}