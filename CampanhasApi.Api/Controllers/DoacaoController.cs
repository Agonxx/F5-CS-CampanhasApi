using CampanhasApi.Domain.Constants;
using CampanhasApi.Domain.DTOs;
using CampanhasApi.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CampanhasApi.Api.Controllers
{
    public class DoacaoController : BaseController
    {
        private readonly IDoacaoService _service;

        public DoacaoController(IHttpContextAccessor httpContextAccessor,
                                    IDoacaoService service,
                                    InfoToken infoToken) : base(httpContextAccessor, infoToken)
        {
            _service = service;
        }

        [HttpPost(DoacaoApi.Doar)]
        [RoleAuthorize(Roles.DoadorAccess)]
        public async Task<IActionResult> Doar([FromBody] DoacaoRequest request)
        {
            var doacao = await _service.DoarAsync(request);
            return Ok(doacao);
        }

        [HttpGet(DoacaoApi.MinhasDoacoes)]
        [RoleAuthorize(Roles.DoadorAccess)]
        public async Task<IActionResult> MinhasDoacoes()
        {
            var doacoes = await _service.GetMinhasAsync();
            return Ok(doacoes);
        }
    }
}
