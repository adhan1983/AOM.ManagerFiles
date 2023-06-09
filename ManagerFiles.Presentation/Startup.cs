using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ManagerFiles.Presentation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ManagerFiles.Presentation
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services) =>  services.BuildConfigureServices();        
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) => app.BuildConfigureExtensions(env);
        
    }
}
