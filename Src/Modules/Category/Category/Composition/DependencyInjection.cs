using Category.Application.Commands.CreateCategory.Handler;
using Category.Domain.Repository;
using Category.Domain.Uow;
using Category.Infrastructure.Persistence.DBcontext;
using Category.Infrastructure.Persistence.Repository;
using Category.Infrastructure.Persistence.Uow;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Composition
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCategoryModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 1. Infrastructure (Persistence)
            services.AddDbContext<CategoryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))); 

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 2. Application (MediatR & Validation)
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CreateCategoryCommandHandler).Assembly));

            services.AddValidatorsFromAssembly(typeof(CreateCategoryCommandHandler).Assembly);

            return services;
        }
    }
}
