using CampanhasApi.Domain;
using CampanhasApi.Domain.Entities;
using CampanhasApi.Domain.Interfaces.Repositories;
using CampanhasApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CampanhasApi.Infrastructure.Repositories
{
    public class CampanhaRepository : ICampanhaRepository
    {
        private readonly CampanhasDbContext _db;

        public CampanhaRepository(CampanhasDbContext db)
        {
            _db = db;
        }

        public async Task<Campanha> GetById(int id)
        {
            return await _db.Campanhas.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Campanha>> GetAll()
        {
            return await _db.Campanhas.OrderByDescending(c => c.CriadoEm).ToListAsync();
        }

        public async Task<List<Campanha>> GetAtivas()
        {
            return await _db.Campanhas
                            .Where(c => c.Status == ECampanhaStatus.Ativa)
                            .OrderByDescending(c => c.CriadoEm)
                            .ToListAsync();
        }

        public async Task<bool> Create(Campanha campanhaObj)
        {
            _db.Campanhas.Add(campanhaObj);
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Campanha campanhaObj)
        {
            _db.Campanhas.Update(campanhaObj);
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }
    }
}
