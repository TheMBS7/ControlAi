using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.Mapping;

internal class MesMapping : IEntityTypeConfiguration<Mes>
{
    public void Configure(EntityTypeBuilder<Mes> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Descricao)
            .IsRequired();

        builder
            .Property(x => x.DataInicial)
            .IsRequired();

        builder
            .Property(x => x.DataFinal)
            .IsRequired();
    }

}