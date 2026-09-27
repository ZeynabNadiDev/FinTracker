using Budget.Application.Commands.CreateBudget;
using Budget.Domain.Repositories;
using Budget.Domain.UOW;
using Budget.Infrastructure.Persistence.DBcontext;
using Budget.Infrastructure.Persistence.Repositories;
using Budget.Infrastructure.Persistence.Uow;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Composition
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBudgetModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Database Context
            services.AddDbContext<BudgetDbContext>(options =>
                options.UseSqlServer(connectionString, b =>
                    b.MigrationsHistoryTable("__EFMigrationsHistory", "budget")));

            // Repositories & Unit of Work
            services.AddScoped<IBudgetRepository, BudgetRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Application Layer (MediatR & Validators)
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CreateBudgetCommand).Assembly));

            services.AddValidatorsFromAssembly(typeof(CreateBudgetCommand).Assembly);

            return services;
        }
    }
}
