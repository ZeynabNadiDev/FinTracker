using FinTracker.SharedKernel.Contracts;
using FinTracker.SharedKernel.Events;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Transaction.Application.Commands.CreateTransaction;
using Transaction.Domain.Repositories;
using Transaction.Domain.UOW;
using Transaction.Infrastructure.Persistence.DBcontext;
using Transaction.Infrastructure.Persistence.Repositories;
using Transaction.Infrastructure.Persistence.Services;
using Transaction.Infrastructure.Persistence.Uow;

namespace Transaction.Composition
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTransactionModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 1. Application Layer (MediatR & Validators)
            services.AddMediatR(cfg =>
            {
                // Register all Handlers & NotificationHandlers in Transaction.Application
                cfg.RegisterServicesFromAssembly(typeof(CreateTransactionCommand).Assembly);
            });

            services.AddValidatorsFromAssemblyContaining<CreateTransactionCommand>();

            // 2. Infrastructure Layer
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<TransactionDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Registering UnitOfWork & Repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITransactionContract, TransactionContract>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
