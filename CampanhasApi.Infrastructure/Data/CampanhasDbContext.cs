using CampanhasApi.Domain.Entities;
using CampanhasApi.Infrastructure.Data.ContextConfig;
using Microsoft.EntityFrameworkCore;

namespace CampanhasApi.Infrastructure.Data
{
    public class CampanhasDbContext : DbContext
    {
        public CampanhasDbContext(DbContextOptions<CampanhasDbContext> options) : base(options)
        {
        }

        public DbSet<Campanha> Campanhas { get; set; }
        public DbSet<Doacao> Doacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConfigCampanha());
            modelBuilder.ApplyConfiguration(new ConfigDoacao());
        }
    }
}
