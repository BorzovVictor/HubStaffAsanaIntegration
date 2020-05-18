using HI.Api.Services;
using HI.Api.UseCases;
using HI.Asana;
using HI.Hubstaff;
using Microsoft.Extensions.DependencyInjection;

namespace HI.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddTransient<IAsanaService, AsanaService>();
            services.AddTransient<IHubstaffService, HubstaffService>();

            services.AddTransient<IJsonStoreService, JsonStoreService>();

            // useCases
            services.AddUsesCases();
            
            return services;
        }

        private static IServiceCollection AddUsesCases(this IServiceCollection services)
        {
            services.AddTransient<IUpdateSumFieldsCase, UpdateSumFieldsCase>();

            return services;
        }
    }
}