using FinTracker.SharedKernel.Results;
using MediatR;
using Wallet.Application.Commands.CreateWallet;
using Wallet.Application.DTO;
using Wallet.Domain.Repository;
using Wallet.Domain.UOW;

namespace Wallet.Application.Commands.CreateWallet.Handler
{
    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, Result<WalletResponseDto>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWalletCommandHandler(IWalletRepository walletRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<WalletResponseDto>> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            // 1. Check title uniqueness for the user
            var exists = await _walletRepository.ExistsByTitleAsync(request.UserId, request.Title, cancellationToken);
            if (exists)
            {
                return Result<WalletResponseDto>.Failure($"A wallet with title '{request.Title}' already exists.");
            }

            // 2. Create the Wallet entity
            var wallet = Wallet.Domain.Entities.Wallet.Wallet.Create(
                Guid.NewGuid(),
                request.Title,
                request.UserId,
                request.Currency
            );

            // 3. Persist to database
            await _walletRepository.AddAsync(wallet, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 4. Map to DTO
            var response = new WalletResponseDto(
                wallet.Id,
                wallet.Title,
                wallet.Balance.Amount,
                wallet.Balance.Currency,
                wallet.UserId
            );

            return Result<WalletResponseDto>.Success(response);
        }
    }
}
