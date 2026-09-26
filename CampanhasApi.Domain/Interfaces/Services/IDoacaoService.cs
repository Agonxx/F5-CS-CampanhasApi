using CampanhasApi.Domain.DTOs;

namespace CampanhasApi.Domain.Interfaces.Services
{
    public interface IDoacaoService
    {
        Task<DoacaoResponse> DoarAsync(DoacaoRequest request);
        Task<List<DoacaoResponse>> GetMinhasAsync();
    }
}
