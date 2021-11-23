using System.Threading.Tasks;
using DTNL.LL.DAL.Repositories;

namespace DTNL.LL.DAL
{
    public interface IUnitOfWork
    {
        IProjectRepository Projects { get; }
        ILifxLightsRepository LifxLights { get; }
        Task<int> CommitAsync();
    }
}