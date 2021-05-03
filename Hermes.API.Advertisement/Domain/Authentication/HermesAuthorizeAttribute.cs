using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Hermes.API.Advertisement.Domain.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class HermesAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private string[] _roles;

        public HermesAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!AuthenticationHelper.HasAuthorizationHeader(context.HttpContext.Request))
            {
                AuthenticationHelper.SetContextResultAsUnauthorized(context);
                return;
            }

            if (!AuthenticationHelper.TryGetAuthenticationToken(context.HttpContext.Request, out var token))
            {
                AuthenticationHelper.SetContextResultAsUnauthorized(context);
                return;
            }

            try
            {
                var jwtOptions = context.HttpContext.RequestServices.GetRequiredService<JwtOptions>();
                if (!AuthenticationHelper.TryValidateToken(token, jwtOptions.SecretKey, out var jwtToken))
                {
                    AuthenticationHelper.SetContextResultAsUnauthorized(context);
                    return;
                }

                if (_roles == null || _roles.Length == 0)
                {
                    SetTokenAndClaimsToSession(context, jwtToken, token);
                    return;
                }

                if (HasRequiredRoles(jwtToken))
                {
                    SetTokenAndClaimsToSession(context, jwtToken, token);
                    return;
                }

                AuthenticationHelper.SetContextResultAsUnauthorized(context);
            }
            catch (Exception e)
            {
                AuthenticationHelper.SetContextResultAsUnauthorized(context);
            }
        }

        private bool HasRequiredRoles(JwtSecurityToken jwtToken)
        {
            var userRoles = jwtToken!.Claims
                .Where(c => c.Type == "role")
                .Select(claim => claim.Value)
                .ToList();
            var hasRequiredRoles = _roles.Any(r => userRoles.Contains(r));
            return hasRequiredRoles;
        }

        private void SetTokenAndClaimsToSession(AuthorizationFilterContext context, JwtSecurityToken jwtSecurityToken,
            string bearerToken)
        {
            var httpContextAccessor = context.HttpContext.RequestServices.GetRequiredService<IHttpContextAccessor>();
            var session = httpContextAccessor.HttpContext!.Session;
            foreach (var claim in jwtSecurityToken!.Claims)
            {
                session.Remove(claim.Type);
                session.SetString(claim.Type, claim.Value);
            }

            session.SetString("AuthToken", bearerToken);
        }
    }
}