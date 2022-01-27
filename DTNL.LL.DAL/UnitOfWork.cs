using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DTNL.LL.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private ProjectRepository _projectRepository;
        private LifxLightsRepository _lifxLightsRepository;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public async Task MigrateDatabaseAsync()
        {
            await _context.Database.MigrateAsync();
        }

        public IProjectRepository Projects =>
            _projectRepository ??= new ProjectRepository(_context);

        public ILifxLightsRepository LifxLights =>
            _lifxLightsRepository ??= new LifxLightsRepository(_context);

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
