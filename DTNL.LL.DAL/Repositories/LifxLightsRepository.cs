using DTNL.LL.Models;
using Microsoft.EntityFrameworkCore;

namespace DTNL.LL.DAL.Repositories
{
    public class LifxLightsRepository : Repository<LifxLight>, ILifxLightsRepository
    {
        public LifxLightsRepository(DbContext context) : base(context)
        {
        }

        private DatabaseContext DatabaseContext => _context as DatabaseContext;

    }
}