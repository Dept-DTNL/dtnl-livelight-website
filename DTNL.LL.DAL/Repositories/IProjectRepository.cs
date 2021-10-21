using System.Collections.Generic;
using System.Threading.Tasks;
using DTNL.LL.Models;

namespace DTNL.LL.DAL.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        public Task<List<Project>> GetActiveProjectsAsync();
    }
}