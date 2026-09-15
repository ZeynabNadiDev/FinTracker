using FinTracker.SharedKernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Identity.Domain.ValueObject
{
    public sealed class Email
    {
        public string Value { get; }

        private Email() { }
        public Email(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new DomainException("Email cannot be null or empty.");

            value = value.Trim();

            try
            {
                var mailAddress = new MailAddress(value);

                if (mailAddress.Address != value)
                    throw new DomainException("Invalid email format.");

                Value = value.ToLowerInvariant();
            }
            catch (FormatException)
            {
                throw new DomainException("Invalid email format.");
            }

        }

        
    }
}
