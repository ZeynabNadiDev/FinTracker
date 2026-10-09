using Identity.Domain.Entities.User;
using Identity.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

        Task<User?> GetByEmailAsync(
            Email email,
            CancellationToken cancellationToken = default);

        Task<User?> GetByPhoneNumberAsync(
            string phoneNumber,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            User user,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
            User user,
            CancellationToken cancellationToken = default);

        Task<bool> IsEmailUniqueAsync(
            string email, CancellationToken cancellationToken = default);
        Task<bool> IsPhoneNumberUniqueAsync(
            string phoneNumber, CancellationToken cancellationToken = default);
    }
}
