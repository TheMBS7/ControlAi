using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.Mapping;

internal class TipoMovimentoMapping : IEntityTypeConfiguration<TipoMovimento>
{
    public void Configure(EntityTypeBuilder<TipoMovimento> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Tipo)
            .IsRequired();
    }

}