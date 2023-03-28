using ManagerFiles.Presentation.Services;
using Microsoft.Extensions.DependencyInjection;
using ManagerFiles.Presentation.ServicesInterfaces;

namespace ManagerFiles.Presentation.Extensions
{
    public static class ScopePersistenceExtensions
    {
        public static IServiceCollection AddingScopePersistence(this IServiceCollection services)
        {
            services.AddScoped<IFilePersistenceService, FilePersistenceService>();

            return services;
        }
    }
}
