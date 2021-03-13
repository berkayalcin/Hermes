using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Hermes.API.User.Domain.Entities;
using Hermes.API.User.Domain.Requests;
using Hermes.API.User.Domain.Responses;
using Hermes.API.User.Domain.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Hermes.API.User.Domain.Services
{
    public class SignInService : ISignInService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IPasswordHasher<HermesUser> _passwordHasher;
        private readonly SignInManager<HermesUser> _signInManager;
        private readonly UserManager<HermesUser> _userManager;

        public SignInService(SignInManager<HermesUser> signInManager,
            UserManager<HermesUser> userManager, JwtOptions jwtOptions,
            IPasswordHasher<HermesUser> passwordHasher)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtOptions = jwtOptions;
            _passwordHasher = passwordHasher;
        }

        public async Task<SignInResponse> SignInAsync(SignInRequest signInRequest)
        {
            if (signInRequest == null) throw new ArgumentNullException(nameof(signInRequest));

            var user = await _userManager.FindByEmailAsync(signInRequest.Email);
            if (user == null)
                throw new InvalidOperationException("User Is Not Found!");

            var canSignIn = await _signInManager.CheckPasswordSignInAsync(user, signInRequest.Password, false);
            if (canSignIn.IsLockedOut)
                throw new InvalidOperationException("User Login Is Locked Out");

            if (canSignIn.IsNotAllowed)
                throw new InvalidOperationException("You Cannot Sign In");

            if (canSignIn.RequiresTwoFactor)
                throw new InvalidOperationException(
                    "To Sign In,Please verify your account by two factor authentication.");

            if (!canSignIn.Succeeded)
                throw new InvalidOperationException("Invalid Email or Password.");

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRole = (await _userManager.GetRolesAsync(user))?.FirstOrDefault();

            if (userClaims == null || userClaims.Count == 0)
                userClaims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Role, userRole)
                };

            var tokenResponse = GenerateToken(userClaims);

            return new SignInResponse
            {
                Message = "Sign In Succeeded",
                Token = tokenResponse,
                Claims = userClaims.ToList()
            };
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest changePasswordRequest)
        {
            var user = await _userManager.FindByEmailAsync(changePasswordRequest.Email);
            if (user == null) throw new InvalidOperationException("User Is Not Exists");

            var canSignIn =
                await _signInManager.CheckPasswordSignInAsync(user, changePasswordRequest.OldPassword, false);
            if (canSignIn.IsLockedOut)
                throw new InvalidOperationException("User Login Is Locked Out");

            if (canSignIn.IsNotAllowed)
                throw new InvalidOperationException("You Cannot Sign In");

            if (canSignIn.RequiresTwoFactor)
                throw new InvalidOperationException(
                    "To Sign In,Please verify your account by two factor authentication.");

            if (!canSignIn.Succeeded)
                throw new InvalidOperationException("Invalid Email or Password.");

            user.PasswordHash = _passwordHasher.HashPassword(user, changePasswordRequest.NewPassword);
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var errors = string.Join("\n", updateResult.Errors.Select(x => x.Description).ToList());
                throw new InvalidOperationException(errors);
            }
        }

        private TokenResponse GenerateToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);
            var expires = DateTime.UtcNow.AddDays(_jwtOptions.DefaultExpirationDays);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);


            return new TokenResponse
            {
                Token = jwtToken,
                ExpiresAt = expires
            };
        }
    }
}