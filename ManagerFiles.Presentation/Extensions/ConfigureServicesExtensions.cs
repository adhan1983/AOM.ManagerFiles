using Microsoft.Extensions.DependencyInjection;

namespace ManagerFiles.Presentation.Extensions
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection BuildConfigureServices(this IServiceCollection services) 
        {
            services.AddControllersWithViews();

            services.AddSignalR();
            
            services.AddingScopePersistence();
            
            services.AddMvcCore();

            return services;
        }
    }
}
