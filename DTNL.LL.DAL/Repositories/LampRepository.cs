using DTNL.LL.Models;
using Microsoft.EntityFrameworkCore;

namespace DTNL.LL.DAL.Repositories
{
    public class LampRepository : Repository<Lamp>, ILampRepository
    {
        public LampRepository(DbContext context) : base(context)
        {
        }

        private DatabaseContext DatabaseContext => _context as DatabaseContext;
    }
}