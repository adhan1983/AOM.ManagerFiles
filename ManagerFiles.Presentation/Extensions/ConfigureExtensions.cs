using ManagerFiles.Presentation.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ManagerFiles.Presentation.Extensions
{
    public static class ConfigureExtensions
    {
        public static IApplicationBuilder BuildConfigureExtensions(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");                
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Upload}/{action=Index}/{id?}");
                endpoints.MapHub<BroadCastHubService>("/broadcast");
            });

            app.BuildingDirectories();

            return app;
        }
    }
}
