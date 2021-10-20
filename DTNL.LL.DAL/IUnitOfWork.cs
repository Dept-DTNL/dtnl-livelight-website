using DTNL.LL.DAL.Repositories;
using System.Threading.Tasks;

namespace DTNL.LL.DAL
{
    public interface IUnitOfWork
    {
        IProjectRepository Projects { get; }
        Task<int> CommitAsync();
    }
}
