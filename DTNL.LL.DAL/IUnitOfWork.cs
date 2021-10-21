using System.Threading.Tasks;
using DTNL.LL.DAL.Repositories;

namespace DTNL.LL.DAL
{
    public interface IUnitOfWork
    {
        IProjectRepository Projects { get; }
        Task<int> CommitAsync();
    }
}