using System;
using DTNL.LL.Logic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace DTNL.LL.Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    //Todo: Api keys should be tested for active lights before adding them to the database.
}