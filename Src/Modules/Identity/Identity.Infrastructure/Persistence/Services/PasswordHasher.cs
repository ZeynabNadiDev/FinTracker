using Identity.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Identity.Core;

namespace Identity.Infrastructure.Persistence.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly IPasswordHasher<object> _hasher;

        public PasswordHasher()
        {
            _hasher = new PasswordHasher<object>();
        }

        public string Hash(string password)
            => _hasher.HashPassword(null!, password);

        public bool Verify(string password, string hashPassword)
            => _hasher.VerifyHashedPassword(null!, hashPassword, password)
               != PasswordVerificationResult.Failed;
    }
}
