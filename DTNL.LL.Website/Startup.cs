using DTNL.LL.Logic;
using DTNL.LL.Logic.Analytics;
using DTNL.LL.Logic.Helper;
using DTNL.LL.Logic.Options;
using DTNL.LL.Logic.Workers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddApplicationInsightsTelemetry();

            services.AddHttpContextAccessor();
            services.RegisterDatabase(Configuration.GetConnectionString("DbDSN"));
            services.AddControllersWithViews();
            services.AddScoped<ProjectService>();
            services.AddScoped<AuthService>();
            services.AddScoped<LiveLightService>();
            services.AddScoped<LifxLightDbService>();
            services.AddScoped<LifxLightService>();
            services.AddScoped<LifxClient>();

            services.AddSingleton<ProjectTimerService>();
            services.AddSingleton<GaService>();
            services.AddSingleton<GoogleCredentialProviderService>();
            services.AddSingleton<V3Analytics>();
            services.AddSingleton<V4Analytics>();

            services.Configure<GAuthOptions>(Configuration.GetSection(GAuthOptions.GAuth));
            services.Configure<GaApiTagsOptions>(Configuration.GetSection(GaApiTagsOptions.GaApiTags));
            services.Configure<ServiceWorkerOptions>(Configuration.GetSection(ServiceWorkerOptions.ServiceWorker));

            services.AddHostedService<LiveLightWorker>();


            GAuthOptions gAuth = new();
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
