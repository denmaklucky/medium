using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineCinema.Interfaces;
using OnlineCinema.Models;
using System;

namespace OnlineCinema
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddSingleton<ICinemaRepository,CinemaRepository>();
            services.AddServerSideBlazor();
            services.AddHttpClient("Cinema Api", cnf => 
            {
                cnf.BaseAddress = new Uri("https://localhost:5001");
            }) ;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{Controller=Home}/{Action=Index}"
                );
                endpoints.MapBlazorHub();
            });
        }
    }
}
