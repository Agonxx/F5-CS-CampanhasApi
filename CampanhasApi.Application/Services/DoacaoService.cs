using CampanhasApi.Domain;
using CampanhasApi.Domain.DTOs;
using CampanhasApi.Domain.Entities;
using CampanhasApi.Domain.Interfaces.Repositories;
using CampanhasApi.Domain.Interfaces.Services;
using Shared.Contracts.Events;

namespace CampanhasApi.Application.Services
{
    public class DoacaoService : IDoacaoService
    {
        private readonly IDoacaoRepository _repo;
        private readonly ICampanhaRepository _campanhaRepo;
        private readonly IDoacaoPublisher _publisher;
        private readonly InfoToken _infoToken;

        public DoacaoService(IDoacaoRepository repo, ICampanhaRepository campanhaRepo, IDoacaoPublisher publisher, InfoToken infoToken)
        {
            _repo = repo;
            _campanhaRepo = campanhaRepo;
            _publisher = publisher;
            _infoToken = infoToken;
        }

        public async Task<DoacaoResponse> DoarAsync(DoacaoRequest request)
        {
            if (request.ValorDoacao <= 0)
                throw new Exception("O valor da doação deve ser maior que zero");

            var campanha = await _campanhaRepo.GetById(request.IdCampanha);

            if (campanha is null)
                throw new Exception("Campanha não encontrada");

            if (campanha.Status != ECampanhaStatus.Ativa)
                throw new Exception("Não é possível doar para uma campanha encerrada ou cancelada");

            var doacao = new Doacao
            {
                IdCampanha = campanha.Id,
                IdDoador = _infoToken.Id,
                ValorDoacao = request.ValorDoacao
            };

            await _repo.Create(doacao);

            // O ValorArrecadado da campanha só é atualizado pelo Worker, ao consumir o evento
            await _publisher.PublishAsync(new DoacaoRecebidaEvent(
                doacao.Id,
                doacao.IdCampanha,
                doacao.IdDoador,
                doacao.ValorDoacao,
                doacao.DoadoEm));

            return ToResponse(doacao);
        }

        public async Task<List<DoacaoResponse>> GetMinhasAsync()
        {
            var doacoes = await _repo.GetMinhas();
            return doacoes.Select(ToResponse).ToList();
        }

        private static DoacaoResponse ToResponse(Doacao doacao)
        {
            return new DoacaoResponse
            {
                Id = doacao.Id,
                IdCampanha = doacao.IdCampanha,
                ValorDoacao = doacao.ValorDoacao,
                DoadoEm = doacao.DoadoEm
            };
        }
    }
}
