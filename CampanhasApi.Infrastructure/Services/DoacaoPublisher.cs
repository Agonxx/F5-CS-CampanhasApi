using CampanhasApi.Domain.Interfaces.Services;
using MassTransit;
using Shared.Contracts.Events;

namespace CampanhasApi.Infrastructure.Services
{
    public class DoacaoPublisher : IDoacaoPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public DoacaoPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync(DoacaoRecebidaEvent evento)
        {
            await _publishEndpoint.Publish(evento);
        }
    }
}
