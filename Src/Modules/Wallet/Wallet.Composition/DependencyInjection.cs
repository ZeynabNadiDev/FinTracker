using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Domain.Repository;
using Wallet.Domain.UOW;
using Wallet.Infrastructure.Persistence.DBcontext;
using Wallet.Infrastructure.Persistence.Repositories;
using Wallet.Infrastructure.Persistence.UOW;

namespace Wallet.Composition
{
public static class DependencyInjection
{
    public static IServiceCollection AddWalletModule(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Repositories and UnitOfWork
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // 2. DbContext Configuration
        var connectionString = configuration.GetConnectionString("FinTrackerDb")
            ?? configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'FinTrackerDb' or 'DefaultConnection' was not found.");

        services.AddDbContext<WalletDbContext>(options =>
            options.UseSqlServer(connectionString));

        // 3. MediatR Handlers for Wallet Application
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Wallet.Application.Commands.CreateWallet.CreateWalletCommand).Assembly);
        });

        // 4. FluentValidation Validators for Wallet Application
        services.AddValidatorsFromAssembly(typeof(Wallet.Application.Commands.CreateWallet.CreateWalletCommand).Assembly);

        return services;
    }
}
}
