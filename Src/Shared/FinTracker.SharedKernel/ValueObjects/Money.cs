using FinTracker.SharedKernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTracker.SharedKernel.ValueObjects
{
    public sealed class Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new DomainException("Amount cannot be negative.");

            if (string.IsNullOrWhiteSpace(currency))
                throw new DomainException("Currency cannot be null or empty.");

            Amount = amount;
            Currency = currency;
        }
    }
}
