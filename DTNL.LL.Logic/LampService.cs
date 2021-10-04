using System.Threading.Tasks;
using DTNL.LL.DAL;
using DTNL.LL.Models;

namespace DTNL.LL.Logic
{
    public class LampService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LampService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddLampsAsync(Lamp lamp, int projectId)
        {
            Project project = await _unitOfWork.Projects.GetByIdAsync(projectId);
            
            if (project is null) return;

            project.Lamps.Add(lamp);

            await _unitOfWork.Lamps.AddAsync(lamp);
            await _unitOfWork.CommitAsync();
        }
    }
}
