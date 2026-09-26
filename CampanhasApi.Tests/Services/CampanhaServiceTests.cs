using CampanhasApi.Application.Services;
using CampanhasApi.Domain;
using CampanhasApi.Domain.DTOs;
using CampanhasApi.Domain.Entities;
using CampanhasApi.Domain.Interfaces.Repositories;
using Moq;

namespace CampanhasApi.Tests.Services
{
    public class CampanhaServiceTests
    {
        private readonly Mock<ICampanhaRepository> _repoMock;
        private readonly CampanhaService _service;

        public CampanhaServiceTests()
        {
            _repoMock = new Mock<ICampanhaRepository>();
            _service = new CampanhaService(_repoMock.Object);
        }

        private static CampanhaRequest RequestValido() => new()
        {
            Titulo = "Cestas básicas",
            Descricao = "Arrecadação para cestas básicas",
            DataInicio = DateTime.UtcNow.Date,
            DataFim = DateTime.UtcNow.Date.AddDays(30),
            MetaFinanceira = 10000m,
            Status = ECampanhaStatus.Ativa
        };

        [Fact]
        public async Task CriarAsync_DeveCriarCampanha_QuandoDadosValidos()
        {
            _repoMock.Setup(r => r.Create(It.IsAny<Campanha>())).ReturnsAsync(true);

            var resultado = await _service.CriarAsync(RequestValido());

            Assert.Equal("Cestas básicas", resultado.Titulo);
            Assert.Equal(0m, resultado.ValorArrecadado);
            _repoMock.Verify(r => r.Create(It.IsAny<Campanha>()), Times.Once);
        }

        [Fact]
        public async Task CriarAsync_DeveLancarExcecao_QuandoDataFimNoPassado()
        {
            var request = RequestValido();
            request.DataInicio = DateTime.UtcNow.Date.AddDays(-10);
            request.DataFim = DateTime.UtcNow.Date.AddDays(-1);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CriarAsync(request));

            Assert.Equal("A data de fim não pode estar no passado", ex.Message);
            _repoMock.Verify(r => r.Create(It.IsAny<Campanha>()), Times.Never);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-50)]
        public async Task CriarAsync_DeveLancarExcecao_QuandoMetaNaoPositiva(decimal meta)
        {
            var request = RequestValido();
            request.MetaFinanceira = meta;

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.CriarAsync(request));

            Assert.Equal("A meta financeira deve ser maior que zero", ex.Message);
        }

        [Fact]
        public async Task AtualizarAsync_DeveLancarExcecao_QuandoCampanhaNaoExiste()
        {
            _repoMock.Setup(r => r.GetById(99)).ReturnsAsync((Campanha)null);

            var ex = await Assert.ThrowsAsync<Exception>(() => _service.AtualizarAsync(99, RequestValido()));

            Assert.Equal("Campanha não encontrada", ex.Message);
        }

        [Fact]
        public async Task AtualizarAsync_DevePermitirConcluir_QuandoDataFimJaPassouMasNaoFoiAlterada()
        {
            var dataFimPassada = DateTime.UtcNow.Date.AddDays(-2);
            var campanha = new Campanha { Id = 1, Titulo = "Antiga", DataInicio = dataFimPassada.AddDays(-30), DataFim = dataFimPassada, MetaFinanceira = 500m };
            _repoMock.Setup(r => r.GetById(1)).ReturnsAsync(campanha);
            _repoMock.Setup(r => r.Update(It.IsAny<Campanha>())).ReturnsAsync(true);

            var request = RequestValido();
            request.DataInicio = campanha.DataInicio;
            request.DataFim = dataFimPassada;
            request.Status = ECampanhaStatus.Concluida;

            var resultado = await _service.AtualizarAsync(1, request);

            Assert.Equal(ECampanhaStatus.Concluida, resultado.Status);
        }

        [Fact]
        public async Task GetTransparenciaAsync_DeveRetornarApenasDadosPublicos()
        {
            _repoMock.Setup(r => r.GetAtivas()).ReturnsAsync(new List<Campanha>
            {
                new() { Id = 1, Titulo = "A", MetaFinanceira = 1000m, ValorArrecadado = 250m }
            });

            var resultado = await _service.GetTransparenciaAsync();

            var item = Assert.Single(resultado);
            Assert.Equal("A", item.Titulo);
            Assert.Equal(1000m, item.MetaFinanceira);
            Assert.Equal(250m, item.ValorArrecadado);
        }
    }
}
