using CampanhasApi.Domain.Constants;
using CampanhasApi.Domain.DTOs;
using CampanhasApi.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampanhasApi.Api.Controllers
{
    public class CampanhaController : BaseController
    {
        private readonly ICampanhaService _service;

        public CampanhaController(IHttpContextAccessor httpContextAccessor,
                                    ICampanhaService service,
                                    InfoToken infoToken) : base(httpContextAccessor, infoToken)
        {
            _service = service;
        }

        [HttpGet(CampanhaApi.Transparencia)]
        [AllowAnonymous]
        public async Task<IActionResult> Transparencia()
        {
            var campanhas = await _service.GetTransparenciaAsync();
            return Ok(campanhas);
        }

        [HttpGet(CampanhaApi.GetAll)]
        [RoleAuthorize(Roles.GestorAccess)]
        public async Task<IActionResult> GetAll()
        {
            var campanhas = await _service.GetAllAsync();
            return Ok(campanhas);
        }

        [HttpGet(CampanhaApi.GetById)]
        [RoleAuthorize(Roles.GestorAccess)]
        public async Task<IActionResult> GetById(int id)
        {
            var campanha = await _service.GetByIdAsync(id);
            return Ok(campanha);
        }

        [HttpPost(CampanhaApi.Criar)]
        [RoleAuthorize(Roles.GestorAccess)]
        public async Task<IActionResult> Criar([FromBody] CampanhaRequest request)
        {
            var campanha = await _service.CriarAsync(request);
            return Ok(campanha);
        }

        [HttpPut(CampanhaApi.Atualizar)]
        [RoleAuthorize(Roles.GestorAccess)]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CampanhaRequest request)
        {
            var campanha = await _service.AtualizarAsync(id, request);
            return Ok(campanha);
        }
    }
}
