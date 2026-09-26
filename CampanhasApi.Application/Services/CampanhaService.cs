using CampanhasApi.Domain;
using CampanhasApi.Domain.DTOs;
using CampanhasApi.Domain.Entities;
using CampanhasApi.Domain.Interfaces.Repositories;
using CampanhasApi.Domain.Interfaces.Services;

namespace CampanhasApi.Application.Services
{
    public class CampanhaService : ICampanhaService
    {
        private readonly ICampanhaRepository _repo;

        public CampanhaService(ICampanhaRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CampanhaTransparenciaResponse>> GetTransparenciaAsync()
        {
            var campanhas = await _repo.GetAtivas();

            return campanhas.Select(c => new CampanhaTransparenciaResponse
            {
                Id = c.Id,
                Titulo = c.Titulo,
                MetaFinanceira = c.MetaFinanceira,
                ValorArrecadado = c.ValorArrecadado
            }).ToList();
        }

        public async Task<List<CampanhaResponse>> GetAllAsync()
        {
            var campanhas = await _repo.GetAll();
            return campanhas.Select(ToResponse).ToList();
        }

        public async Task<CampanhaResponse> GetByIdAsync(int id)
        {
            var campanha = await _repo.GetById(id);

            if (campanha is null)
                throw new Exception("Campanha não encontrada");

            return ToResponse(campanha);
        }

        public async Task<CampanhaResponse> CriarAsync(CampanhaRequest request)
        {
            Validar(request);

            if (request.DataFim.Date < DateTime.UtcNow.Date)
                throw new Exception("A data de fim não pode estar no passado");

            var campanha = new Campanha
            {
                Titulo = request.Titulo,
                Descricao = request.Descricao,
                DataInicio = request.DataInicio,
                DataFim = request.DataFim,
                MetaFinanceira = request.MetaFinanceira,
                Status = request.Status
            };

            await _repo.Create(campanha);

            return ToResponse(campanha);
        }

        public async Task<CampanhaResponse> AtualizarAsync(int id, CampanhaRequest request)
        {
            Validar(request);

            var campanha = await _repo.GetById(id);

            if (campanha is null)
                throw new Exception("Campanha não encontrada");

            if (request.DataFim != campanha.DataFim && request.DataFim.Date < DateTime.UtcNow.Date)
                throw new Exception("A data de fim não pode estar no passado");

            campanha.Titulo = request.Titulo;
            campanha.Descricao = request.Descricao;
            campanha.DataInicio = request.DataInicio;
            campanha.DataFim = request.DataFim;
            campanha.MetaFinanceira = request.MetaFinanceira;
            campanha.Status = request.Status;

            await _repo.Update(campanha);

            return ToResponse(campanha);
        }

        private static void Validar(CampanhaRequest request)
        {
            if (request.MetaFinanceira <= 0)
                throw new Exception("A meta financeira deve ser maior que zero");

            if (request.DataFim < request.DataInicio)
                throw new Exception("A data de fim não pode ser anterior à data de início");
        }

        private static CampanhaResponse ToResponse(Campanha campanha)
        {
            return new CampanhaResponse
            {
                Id = campanha.Id,
                Titulo = campanha.Titulo,
                Descricao = campanha.Descricao,
                DataInicio = campanha.DataInicio,
                DataFim = campanha.DataFim,
                MetaFinanceira = campanha.MetaFinanceira,
                ValorArrecadado = campanha.ValorArrecadado,
                Status = campanha.Status
            };
        }
    }
}
