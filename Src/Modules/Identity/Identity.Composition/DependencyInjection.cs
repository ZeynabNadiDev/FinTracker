using Identity.Application;
using Identity.Application.Commands.Register.Handler;
using Identity.Domain.Repositories;
using Identity.Domain.UnitOfWork;
using Identity.Infrastructure;
using Identity.Infrastructure.Persistence.DBcontext;
using Identity.Infrastructure.Persistence.Repositories;
using Identity.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Composition
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Infrastructure
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Application
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly));

            return services;
        }
    }
}