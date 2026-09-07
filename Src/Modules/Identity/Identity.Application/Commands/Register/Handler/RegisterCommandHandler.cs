using FinTracker.SharedKernel.Results;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Domain.Entities.User;
using Identity.Domain.Repositories;
using Identity.Domain.UnitOfWork;
using Identity.Domain.ValueObject;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Commands.Register.Handler
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<RegisterResponseDto>> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Instantiate and validate the Email Value Object
            Email email;
            try
            {
                email = new Email(request.Email);
            }
            catch (Exception ex)
            {
                return Result<RegisterResponseDto>.Failure(ex.Message);
            }

            // 2. Ensure email address uniqueness across the system
            var isEmailUnique = await _userRepository.IsEmailUniqueAsync(email.Value, cancellationToken);
            if (!isEmailUnique)
            {
                return Result<RegisterResponseDto>.Failure("A user with this email already exists.");
            }

            // 3. Ensure phone number uniqueness across the system
            var isPhoneUnique = await _userRepository.IsPhoneNumberUniqueAsync(request.PhoneNumber, cancellationToken);
            if (!isPhoneUnique)
            {
                return Result<RegisterResponseDto>.Failure("A user with this phone number already exists.");
            }

            // 4. Hash the raw password securely
            var passwordHash = _passwordHasher.Hash(request.Password);

            // 5. Create the User Aggregate Root
            var user = new User(
                Guid.NewGuid(),
                email,
                request.PhoneNumber,
                request.FirstName,
                request.LastName,
                passwordHash);

            // 6. Persist changes into the database
            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 7. Map to response DTO and return success
            var response = new RegisterResponseDto(
                UserId: user.Id,
                Email: user.Email.Value,
                FullName: $"{user.FirstName} {user.LastName}"
            );

            return Result<RegisterResponseDto>.Succcess(response);
        }
    }
}
