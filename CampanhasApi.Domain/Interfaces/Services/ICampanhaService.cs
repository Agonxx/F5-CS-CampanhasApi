using CampanhasApi.Domain.DTOs;

namespace CampanhasApi.Domain.Interfaces.Services
{
    public interface ICampanhaService
    {
        Task<List<CampanhaTransparenciaResponse>> GetTransparenciaAsync();
        Task<List<CampanhaResponse>> GetAllAsync();
        Task<CampanhaResponse> GetByIdAsync(int id);
        Task<CampanhaResponse> CriarAsync(CampanhaRequest request);
        Task<CampanhaResponse> AtualizarAsync(int id, CampanhaRequest request);
    }
}
