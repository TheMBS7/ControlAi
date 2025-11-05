using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebAPI.Entities;

namespace WebAPI.Mapping;

internal class ExtratoMapping : IEntityTypeConfiguration<Extrato>
{
    public void Configure(EntityTypeBuilder<Extrato> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Descricao)
            .IsRequired();

        builder
            .Property(x => x.ValorTotal)
            .IsRequired();

        builder
            .Property(x => x.Data)
            .IsRequired();

        builder
            .Property(x => x.NumeroMaxParcelas)
            .IsRequired();

        builder
            .Property(x => x.NumeroParcela)
            .IsRequired();

        builder
            .Property(x => x.IdParcelas)
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

        builder
            .HasOne(x => x.Mes)
            .WithMany()
            .HasForeignKey(x => x.MesId)
            .IsRequired();

        builder
            .HasOne(x => x.TipoMovimento)
            .WithMany()
            .HasForeignKey(x => x.TipoMovimentoId)
            .IsRequired();

        builder
            .HasOne(x => x.SaidaFixa)
            .WithMany()
            .HasForeignKey(x => x.SaidaFixaId);

        builder
            .HasOne(x => x.EntradaFixa)
            .WithMany()
            .HasForeignKey(x => x.EntradaFixaId);

        builder
            .Property(x => x.SaidaFixaId)
            .HasDefaultValue(null);

        builder
            .Property(x => x.EntradaFixaId)
            .HasDefaultValue(null);
    }
}