


namespace Wallet.Domain.Repository
{
    public interface IWalletRepository
    {
        Task<Wallet.Domain.Entities.Wallet.Wallet?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Wallet.Domain.Entities.Wallet.Wallet>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<bool> ExistsByTitleAsync(Guid userId, string title, CancellationToken cancellationToken = default);
        Task<bool> ExistsByTitleAsync(Guid userId, string title, Guid excludedWalletId,CancellationToken cancellationToken = default);
        Task AddAsync(Wallet.Domain.Entities.Wallet.Wallet wallet, CancellationToken cancellationToken = default);
        void Update(Wallet.Domain.Entities.Wallet.Wallet wallet);
        void Delete(Wallet.Domain.Entities.Wallet.Wallet wallet);
    }
}
