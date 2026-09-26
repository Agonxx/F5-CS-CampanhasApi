using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using CampanhasApi.Domain.DTOs;

namespace CampanhasApi.Api.Controllers
{
    public class RoleAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public RoleAuthorize(string roles)
        {
            _roles = roles.Split(',');
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.User.FindFirst(nameof(InfoToken.Role))?.Value;

            if (!_roles.Contains(role))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
                return;
            }
        }
    }
}
