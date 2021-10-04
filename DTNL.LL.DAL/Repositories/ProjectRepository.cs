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
    }
}
