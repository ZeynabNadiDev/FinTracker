using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Application.Queries.GetNotificationsByUserId;
using Notification.Domain.Repositories;
using Notification.Domain.UOW;
using Notification.Infrastructure.Persistence.DBcontext;
using Notification.Infrastructure.Persistence.Repositories;
using Notification.Infrastructure.Persistence.Uow;
using System.Reflection;

namespace Notification.Composition
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNotificationModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 1. Persistence & DbContext
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? configuration.GetConnectionString("FinTrackerDb");

            services.AddDbContext<NotificationDbContext>(options =>
                options.UseSqlServer(connectionString, b =>
                    b.MigrationsAssembly(typeof(NotificationDbContext).Assembly.FullName)));

            // 2. Repositories & Unit of Work
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 3. Application Services (MediatR & FluentValidation)
            var applicationAssembly = typeof(GetNotificationsByUserIdQuery).Assembly;

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(applicationAssembly));

            services.AddValidatorsFromAssembly(applicationAssembly);

            return services;
        }
    }
}
