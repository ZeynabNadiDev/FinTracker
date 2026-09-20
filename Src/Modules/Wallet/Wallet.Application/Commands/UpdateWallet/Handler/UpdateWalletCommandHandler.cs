using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.DTO;
using Wallet.Domain.Repository;
using Wallet.Domain.UOW;

namespace Wallet.Application.Commands.UpdateWallet.Handler
{
    public class UpdateWalletCommandHandler : IRequestHandler<UpdateWalletCommand, Result<WalletResponseDto>>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWalletCommandHandler(IWalletRepository walletRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<WalletResponseDto>> Handle(UpdateWalletCommand request, CancellationToken cancellationToken)
        {
            // 1. Fetch wallet by Id
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId, cancellationToken);
            if (wallet is null || wallet.IsRemoved)
            {
                return Result<WalletResponseDto>.Failure($"Wallet with ID '{request.WalletId}' was not found.");
            }

            // 2. Check ownership
            if (wallet.UserId != request.UserId)
            {
                return Result<WalletResponseDto>.Failure("You do not have access to this wallet.");
            }

            // 3. Check title uniqueness (excluding current wallet)
            var exists = await _walletRepository.ExistsByTitleAsync(
                request.UserId,
                request.Title,
                request.WalletId,
                cancellationToken);

            if (exists)
            {
                return Result<WalletResponseDto>.Failure($"A wallet with title '{request.Title}' already exists.");
            }

            // 4. Update the entity using YOUR domain method!
            wallet.Update(request.Title);
            _walletRepository.Update(wallet);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. Map to DTO and return
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
