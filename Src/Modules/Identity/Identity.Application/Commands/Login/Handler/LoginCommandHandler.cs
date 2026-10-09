using Identity.Application.Common;
using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using Identity.Domain.Repositories;
using Identity.Domain.ValueObject;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Commands.Login.Handler
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Create Email value object and retrieve user by email
            var email = new Email(request.Email);
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

            if (user is null)
            {
                throw new Exception("Invalid email or password.");
            }

            // 2. Verify password hash
            var isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new Exception("Invalid email or password.");
            }

            // 3. Generate JWT access token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new LoginResponseDto(
                user.Id,
                user.Email.Value,
                $"{user.FirstName} {user.LastName}",
                token
            );
        }
    }
}
