using CampanhasApi.Application.Services;
using CampanhasApi.Domain;
using CampanhasApi.Domain.DTOs;
using CampanhasApi.Domain.Entities;
using CampanhasApi.Domain.Interfaces.Repositories;
using CampanhasApi.Domain.Interfaces.Services;
using Moq;
using Shared.Contracts.Events;

namespace CampanhasApi.Tests.Services
{
    public class DoacaoServiceTests
    {
        private readonly Mock<IDoacaoRepository> _repoMock;
        private readonly Mock<ICampanhaRepository> _campanhaRepoMock;
        private readonly Mock<IDoacaoPublisher> _publisherMock;
        private readonly InfoToken _infoToken;
        private readonly DoacaoService _service;

        public DoacaoServiceTests()
        {
            _repoMock = new Mock<IDoacaoRepository>();
            _campanhaRepoMock = new Mock<ICampanhaRepository>();
            _publisherMock = new Mock<IDoacaoPublisher>();
            _infoToken = new InfoToken { Id = 7, Role = ERole.Doador };
            _service = new DoacaoService(_repoMock.Object, _campanhaRepoMock.Object, _publisherMock.Object, _infoToken);
        }

        [Fact]
        public async Task DoarAsync_DevePublicarEvento_QuandoCampanhaAtiva()
        {
            _campanhaRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(new Campanha { Id = 1, Status = ECampanhaStatus.Ativa });
            _repoMock.Setup(r => r.Create(It.IsAny<Doacao>())).ReturnsAsync(true);

            var resultado = await _service.DoarAsync(new DoacaoRequest { IdCampanha = 1, ValorDoacao = 50m });

            Assert.Equal(50m, resultado.ValorDoacao);
            _publisherMock.Verify(p => p.PublishAsync(It.Is<DoacaoRecebidaEvent>(e =>
                e.CampanhaId == 1 && e.DoadorId == 7 && e.Valor == 50m)), Times.Once);
        }

        [Theory]
        [InlineData(ECampanhaStatus.Concluida)]
        [InlineData(ECampanhaStatus.Cancelada)]
        public async Task DoarAsync_DeveLancarExcecao_QuandoCampanhaNaoAtiva(ECampanhaStatus status)
        {
            _campanhaRepoMock.Setup(r => r.GetById(1)).ReturnsAsync(new Campanha { Id = 1, Status = status });

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.DoarAsync(new DoacaoRequest { IdCampanha = 1, ValorDoacao = 50m }));

            Assert.Equal("Não é possível doar para uma campanha encerrada ou cancelada", ex.Message);
            _repoMock.Verify(r => r.Create(It.IsAny<Doacao>()), Times.Never);
            _publisherMock.Verify(p => p.PublishAsync(It.IsAny<DoacaoRecebidaEvent>()), Times.Never);
        }

        [Fact]
        public async Task DoarAsync_DeveLancarExcecao_QuandoCampanhaNaoExiste()
        {
            _campanhaRepoMock.Setup(r => r.GetById(99)).ReturnsAsync((Campanha)null);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.DoarAsync(new DoacaoRequest { IdCampanha = 99, ValorDoacao = 50m }));

            Assert.Equal("Campanha não encontrada", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public async Task DoarAsync_DeveLancarExcecao_QuandoValorNaoPositivo(decimal valor)
        {
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.DoarAsync(new DoacaoRequest { IdCampanha = 1, ValorDoacao = valor }));

            Assert.Equal("O valor da doação deve ser maior que zero", ex.Message);
            _repoMock.Verify(r => r.Create(It.IsAny<Doacao>()), Times.Never);
        }
    }
}
