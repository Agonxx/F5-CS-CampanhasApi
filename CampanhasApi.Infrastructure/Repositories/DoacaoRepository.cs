using CampanhasApi.Domain.DTOs;
using CampanhasApi.Domain.Entities;
using CampanhasApi.Domain.Interfaces.Repositories;
using CampanhasApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CampanhasApi.Infrastructure.Repositories
{
    public class DoacaoRepository : IDoacaoRepository
    {
        private readonly CampanhasDbContext _db;
        private readonly InfoToken _infoToken;

        public DoacaoRepository(CampanhasDbContext db, InfoToken infoToken)
        {
            _db = db;
            _infoToken = infoToken;
        }

        public async Task<bool> Create(Doacao doacaoObj)
        {
            _db.Doacoes.Add(doacaoObj);
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<List<Doacao>> GetMinhas()
        {
            return await _db.Doacoes
                            .Where(d => d.IdDoador == _infoToken.Id)
                            .OrderByDescending(d => d.DoadoEm)
                            .ToListAsync();
        }
    }
}
