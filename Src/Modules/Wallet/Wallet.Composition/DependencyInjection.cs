using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Infrastructure.Persistence.DBcontext;

namespace Wallet.Composition
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWalletModule(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("FinTrackerDb")
                ?? configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<WalletDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }
    }
}
