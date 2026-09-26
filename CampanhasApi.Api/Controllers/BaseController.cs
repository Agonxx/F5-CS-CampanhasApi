using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampanhasApi.Domain.DTOs;

namespace CampanhasApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class BaseController : ControllerBase
    {
        public readonly InfoToken _infoToken;

        public BaseController(IHttpContextAccessor httpContextAccessor, InfoToken infoToken)
        {
            _infoToken = infoToken;
        }
    }
}
