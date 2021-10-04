using DTNL.LL.DAL;
using DTNL.LL.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DTNL.LL.Logic.Helper
{
    public static class DALInjector
    {
        public static void RegisterDatabase(this IServiceCollection services, string connectionString)
        {
            DatabaseContext.RegisterDbContext(services, connectionString);

            //Add the UnitOfWork to the DI system for DB access.
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
