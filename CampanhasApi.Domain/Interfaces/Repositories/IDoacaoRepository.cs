using CampanhasApi.Domain.Entities;

namespace CampanhasApi.Domain.Interfaces.Repositories
{
    public interface IDoacaoRepository
    {
        Task<bool> Create(Doacao doacaoObj);
        Task<List<Doacao>> GetMinhas();
    }
}
