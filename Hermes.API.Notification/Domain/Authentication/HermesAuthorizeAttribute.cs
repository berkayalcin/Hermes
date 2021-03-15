using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Hermes.API.Notification.Domain.Authentication
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
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                SetContextResultAsUnauthorized(context);
                return;
            }

            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")
                .LastOrDefault();

            if (string.IsNullOrEmpty(token))
            {
                SetContextResultAsUnauthorized(context);
                return;
            }

            try
            {
                var validatedToken = ValidateToken(context, token);
                if (validatedToken == null)
                {
                    SetContextResultAsUnauthorized(context);
                    return;
                }

                var jwtToken = (JwtSecurityToken) validatedToken;

                if (_roles == null || _roles.Length == 0) return;

                if (HasRequiredRoles(jwtToken)) return;

                SetContextResultAsUnauthorized(context);
            }
            catch (Exception e)
            {
                SetContextResultAsUnauthorized(context);
            }
        }

        private static SecurityToken ValidateToken(ActionContext context, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtOptions = context.HttpContext.RequestServices.GetRequiredService<JwtOptions>();
            var key = Encoding.ASCII.GetBytes(jwtOptions.SecretKey);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);
            return validatedToken;
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

        private static void SetContextResultAsUnauthorized(AuthorizationFilterContext context)
        {
            context.Result = new JsonResult(new {message = "Unauthorized"})
                {StatusCode = StatusCodes.Status401Unauthorized};
        }
    }
}