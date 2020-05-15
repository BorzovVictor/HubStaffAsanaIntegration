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

            return services;
        }
    }
}