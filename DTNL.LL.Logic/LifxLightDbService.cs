using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTNL.LL.DAL;
using DTNL.LL.Models;

namespace DTNL.LL.Logic
{
    public class LifxLightDbService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LifxLightDbService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateLifxLight(LifxLight lifxLight)
        {
            await _unitOfWork.LifxLights.AddAsync(lifxLight);
            await _unitOfWork.CommitAsync();
        }
    }
}
