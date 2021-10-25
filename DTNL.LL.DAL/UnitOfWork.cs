using System.Threading.Tasks;

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