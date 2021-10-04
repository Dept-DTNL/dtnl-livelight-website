using System.Threading.Tasks;

namespace DTNL.LL.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly DatabaseContext _context;
        private ProjectRepository _projectRepository;
        private LampRepository _lampRepository;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public IProjectRepository Projects =>
            _projectRepository ??= new ProjectRepository(_context);

        public ILampRepository Lamps =>
            _lampRepository ??= new LampRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
