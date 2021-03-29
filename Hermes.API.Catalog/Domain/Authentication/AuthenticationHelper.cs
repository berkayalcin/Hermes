using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace Hermes.API.Catalog.Domain.Authentication
{
    public static class AuthenticationHelper
    {
        public static void SetContextResultAsUnauthorized(AuthorizationFilterContext context)
        {
            context.Result = new JsonResult(new {message = "Unauthorized"})
                {StatusCode = StatusCodes.Status401Unauthorized};
        }

        public static bool HasAuthorizationHeader(HttpRequest request)
        {
            return request.Headers.ContainsKey("Authorization");
        }

        public static bool TryGetAuthenticationToken(HttpRequest request, out string token)
        {
            token = request.Headers["Authorization"].FirstOrDefault()?.Split(" ")
                .LastOrDefault();

            return !string.IsNullOrEmpty(token);
        }

        public static bool TryValidateToken(string token, string secretKey, out JwtSecurityToken jwtSecurityToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var securityToken);

            jwtSecurityToken = (JwtSecurityToken) securityToken;
            return securityToken != null;
        }
    }
}