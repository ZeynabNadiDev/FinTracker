using FinTracker.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Application.DTO;
using Wallet.Domain.Repository;

namespace Wallet.Application.Queries.GetWalletsByUserId.Handler
{
    public class GetWalletsByUserIdQueryHandler : IRequestHandler<GetWalletsByUserIdQuery, Result<IReadOnlyList<WalletResponseDto>>>
    {
        private readonly IWalletRepository _walletRepository;

        public GetWalletsByUserIdQueryHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Result<IReadOnlyList<WalletResponseDto>>> Handle(GetWalletsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var wallets = await _walletRepository.GetAllByUserIdAsync(request.UserId, cancellationToken);

             var result = wallets.Select(w => new WalletResponseDto(
              w.Id,
              w.Title,
              w.Balance.Amount,
              w.Balance.Currency,
              w.UserId
              )).ToList();

            return Result<IReadOnlyList<WalletResponseDto>>.Success(result);
        }
    }
}
