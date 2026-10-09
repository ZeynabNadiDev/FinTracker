using Identity.Domain.Entities.User;
using Identity.Domain.Repositories;
using Identity.Domain.ValueObject;
using Identity.Infrastructure.Persistence.DBcontext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Infrastructure.Persistence.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly IdentityDbContext _context;

        public UserRepository(IdentityDbContext context)
        {
            _context = context;
        }

        /// Retrieves a user by their unique identifier.
        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FindAsync(new object[] { id }, cancellationToken);
        }

        /// Retrieves a user by their email address.
        public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.Value == email.Value, cancellationToken);
        }

        /// Retrieves a user by their phone number.
        public async Task<User?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
        }

        /// Adds a new user to the repository. Changes are saved via UnitOfWork.
        public async Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(user, cancellationToken);
        }

        /// Updates an existing user in the repository. Changes are saved via UnitOfWork.
        public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        /// Checks if the provided email address is unique among existing users.
        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
        {
            return !await _context.Users.AnyAsync(u => u.Email.Value == email, cancellationToken);
        }

       /// Checks if the provided phone number is unique among existing users.
        public async Task<bool> IsPhoneNumberUniqueAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            return !await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
        }
    }
}
