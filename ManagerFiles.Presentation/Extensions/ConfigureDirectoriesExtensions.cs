using ManagerFiles.Presentation.Contants;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace ManagerFiles.Presentation.Extensions
{
    public static class ConfigureDirectoriesExtensions
    {
        public static void BuildingDirectories(this IApplicationBuilder app) 
        {
            var _environment = app.ApplicationServices.GetRequiredService<IHostEnvironment>();            

            string filesFolder = $"{_environment.ContentRootPath}{ManagerFilesConstants.FILE}";

            if (!Directory.Exists(filesFolder))
            {
                Directory.CreateDirectory(Path.Combine(filesFolder, ManagerFilesConstants.ORIGIN));

                Directory.CreateDirectory(Path.Combine(filesFolder, ManagerFilesConstants.DESTINY));

            }
        }
    }
}
