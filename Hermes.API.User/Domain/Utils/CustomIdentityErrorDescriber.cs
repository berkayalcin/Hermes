using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Hermes.API.User.Domain.Utils
{
    [ExcludeFromCodeCoverage]
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DefaultError()
        {
            return new() {Code = nameof(DefaultError), Description = "Bilinmeyen bir hata oluştu."};
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new()
                {Code = nameof(ConcurrencyFailure), Description = "İyimser eşzamanlılık hatası, nesne değiştirildi."};
        }

        public override IdentityError PasswordMismatch()
        {
            return new() {Code = nameof(PasswordMismatch), Description = "Geçersiz Şifre."};
        }

        public override IdentityError InvalidToken()
        {
            return new() {Code = nameof(InvalidToken), Description = "Geçersiz Kod."};
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new()
            {
                Code = nameof(LoginAlreadyAssociated), Description = "Bu giriş bilgisine sahip bir kullanıcı zaten var."
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new()
            {
                Code = nameof(InvalidUserName),
                Description = $"Kullanıcı adı '{userName}' geçersiz, yalnızca harf veya rakam içerebilir."
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new() {Code = nameof(InvalidEmail), Description = $"Email '{email}' geçersiz."};
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new() {Code = nameof(DuplicateUserName), Description = $"Kullanıcı adı '{userName}' zaten kayıtlı."};
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new() {Code = nameof(DuplicateEmail), Description = $"Mail '{email}' zaten kayıtlı."};
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return new() {Code = nameof(InvalidRoleName), Description = $"Role name '{role}' is invalid."};
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new() {Code = nameof(DuplicateRoleName), Description = $"Role name '{role}' is already taken."};
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new()
                {Code = nameof(UserAlreadyHasPassword), Description = "Kullanıcının zaten bir şifre belirlenmiş."};
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new()
                {Code = nameof(UserLockoutNotEnabled), Description = "Bu kullanıcı için kilitleme etkin değil."};
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new() {Code = nameof(UserAlreadyInRole), Description = $"User already in role '{role}'."};
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new() {Code = nameof(UserNotInRole), Description = $"User is not in role '{role}'."};
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new() {Code = nameof(PasswordTooShort), Description = $"Şifre en az {length} karakter olmalıdır."};
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new()
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "Şifre en az bir özel karakter içermelidir."
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new() {Code = nameof(PasswordRequiresDigit), Description = "Şifre en az bir rakam içermelidir."};
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new()
            {
                Code = nameof(PasswordRequiresLower), Description = "Şifre en az bir küçük harf içermelidir. ('a'-'z')."
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new()
            {
                Code = nameof(PasswordRequiresUpper), Description = "Şifre en az bir büyük harf içermelidir. ('A'-'Z')."
            };
        }
    }
}