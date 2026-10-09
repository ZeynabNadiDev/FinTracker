using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Report.Common.Interfaces;
using Report.Queries.GetFinancialPeriodSummary;
using Report.Infrastructure.Persistence.Services;



namespace Report.Composition
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddReportModule(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            
            services.AddHttpClient<IFinancialAiAdvisorService, OpenAiFinancialAdvisorService>();

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(GetFinancialPeriodSummaryQuery).Assembly));

            services.AddValidatorsFromAssembly(typeof(GetFinancialPeriodSummaryQuery).Assembly);

            return services;
        }
    }
}
