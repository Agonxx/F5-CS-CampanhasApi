using CampanhasApi.Domain.Entities;

namespace CampanhasApi.Domain.Interfaces.Repositories
{
    public interface ICampanhaRepository
    {
        Task<Campanha> GetById(int id);
        Task<List<Campanha>> GetAll();
        Task<List<Campanha>> GetAtivas();
        Task<bool> Create(Campanha campanhaObj);
        Task<bool> Update(Campanha campanhaObj);
    }
}
