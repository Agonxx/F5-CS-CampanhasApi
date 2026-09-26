using Shared.Contracts.Events;

namespace CampanhasApi.Domain.Interfaces.Services
{
    public interface IDoacaoPublisher
    {
        Task PublishAsync(DoacaoRecebidaEvent evento);
    }
}
