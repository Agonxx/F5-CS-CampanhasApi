using CampanhasApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampanhasApi.Infrastructure.Data.ContextConfig
{
    public class ConfigCampanha : IEntityTypeConfiguration<Campanha>
    {
        public void Configure(EntityTypeBuilder<Campanha> builder)
        {
            builder.Property(c => c.MetaFinanceira).HasPrecision(18, 2);
            builder.Property(c => c.ValorArrecadado).HasPrecision(18, 2);
            builder.HasIndex(c => c.Status);
        }
    }
}
