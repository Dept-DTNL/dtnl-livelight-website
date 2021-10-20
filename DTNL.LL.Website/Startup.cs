using System;
using System.Net;
using System.Security.Policy;
using DTNL.LL.DAL;
using DTNL.LL.Logic;
using DTNL.LL.Logic.Helper;
using DTNL.LL.Logic.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DTNL.LL.Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.RegisterDatabase(Configuration["database"]);
            services.AddControllersWithViews();
            services.AddScoped<ProjectService>();
            services.AddScoped<AuthService>();


            var gAuth = new GAuthOptions();
            Configuration.GetSection(GAuthOptions.GAuth).Bind(gAuth);

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "External";
                })
                .AddCookie("External", o =>
                {
                    o.LoginPath = "/auth/login";
                })
                .AddGoogle(options =>
                    {
                        options.ClientId = gAuth.ClientId;
                        options.ClientSecret = gAuth.ClientSecret;
                    }
                );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
