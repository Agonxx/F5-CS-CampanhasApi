using Shared.Contracts.Events;

namespace CampanhasApi.Tests.Events
{
    public class DoacaoRecebidaEventTests
    {
        [Fact]
        public void DoacaoRecebidaEvent_DeveManterNomeCompletoDoContrato()
        {
            // O MassTransit roteia pelo nome completo do tipo: publisher e consumer precisam bater
            Assert.Equal("Shared.Contracts.Events.DoacaoRecebidaEvent", typeof(DoacaoRecebidaEvent).FullName);
        }
    }
}
