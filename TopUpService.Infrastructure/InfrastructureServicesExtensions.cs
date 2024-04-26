using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using TopUpService.Common.Repositiory;
using TopUpService.Common.Service;
using TopUpService.Infrastructure.Repositiory;
using TopUpService.Infrastructure.Service;

namespace TopUpService.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class InfrastructureServicesExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IBeneficiaryService, BeneficiaryService>();
            services.AddSingleton<IBeneficiaryRepository, BeneficiaryRepository>();
        }
    }
}