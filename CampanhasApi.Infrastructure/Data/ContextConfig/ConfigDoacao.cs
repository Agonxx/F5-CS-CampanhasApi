using CampanhasApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampanhasApi.Infrastructure.Data.ContextConfig
{
    public class ConfigDoacao : IEntityTypeConfiguration<Doacao>
    {
        public void Configure(EntityTypeBuilder<Doacao> builder)
        {
            builder.Property(d => d.ValorDoacao).HasPrecision(18, 2);
            builder.HasIndex(d => d.IdDoador);

            builder.HasOne(d => d.Campanha)
                   .WithMany()
                   .HasForeignKey(d => d.IdCampanha)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
