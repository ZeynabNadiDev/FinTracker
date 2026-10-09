using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.Repository;
using Wallet.Domain.UOW;

namespace Wallet.Application.Commands.DeleteWallet.Handler
{
    public class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand, Result>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWalletCommandHandler(IWalletRepository walletRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
        {
            // 1. Find wallet
            var wallet = await _walletRepository.GetByIdAsync(request.WalletId, cancellationToken);
            if (wallet is null || wallet.IsRemoved)
            {
                return Result.Failure($"Wallet with ID '{request.WalletId}' was not found.");
            }

            // 2. Check ownership
            if (wallet.UserId != request.UserId)
            {
                return Result.Failure("You do not have access to this wallet.");
            }

            // 3. Mark as removed using domain logic
            wallet.Remove();
            _walletRepository.Update(wallet);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
