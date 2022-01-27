using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTNL.LL.Models;
using Microsoft.EntityFrameworkCore;

namespace DTNL.LL.DAL.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(DbContext context) : base(context)
        {
        }

        private DatabaseContext DatabaseContext => _context as DatabaseContext;

        public Task<List<Project>> GetActiveProjectsAsync()
        {
            return _context.Set<Project>().Where(e => e.Active).ToListAsync();
        }
    }
}