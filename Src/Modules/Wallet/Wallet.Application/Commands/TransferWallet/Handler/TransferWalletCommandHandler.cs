using FinTracker.SharedKernel.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.UOW;
using Wallet.Domain.Repository;

namespace Wallet.Application.Commands.TransferWallet.Handler
{
    public class TransferWalletCommandHandler : IRequestHandler<TransferWalletCommand>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferWalletCommandHandler(IWalletRepository walletRepository, IUnitOfWork unitOfWork)
        {
            _walletRepository = walletRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(TransferWalletCommand request, CancellationToken cancellationToken)
        {
            var sourceWallet = await _walletRepository.GetByIdAsync(request.SourceWalletId, cancellationToken);
            if (sourceWallet is null)
                throw new DomainException("Source wallet was not found.");

            if (sourceWallet.UserId != request.UserId)
                throw new DomainException("Unauthorized access to source wallet.");

            var destinationWallet = await _walletRepository.GetByIdAsync(request.DestinationWalletId, cancellationToken);
            if (destinationWallet is null)
                throw new DomainException("Destination wallet was not found.");

            if (destinationWallet.UserId != request.UserId)
                throw new DomainException("Destination wallet belongs to another user.");

            // Call domain method
            sourceWallet.TransferTo(destinationWallet, request.Amount);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
