using DTNL.LL.DAL.Builders;
using DTNL.LL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DTNL.LL.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Project> Projects { get; set; }

        public static void RegisterDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connectionString)
            );
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ProjectConfiguration());
            builder.ApplyConfiguration(new LampConfiguration());
        }
    }
}