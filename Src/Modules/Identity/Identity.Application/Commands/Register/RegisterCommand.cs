using FinTracker.SharedKernel.Results;
using Identity.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Commands.Register
{
    public sealed record RegisterCommand(
    string Email,
    string PhoneNumber,
    string FirstName,
    string LastName,
    string Password) : IRequest<Result<RegisterResponseDto>>;
}
