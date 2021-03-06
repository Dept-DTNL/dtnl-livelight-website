using System;
using System.Collections.Generic;
using System.Linq;
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
            lifxLight.Uuid = Guid.NewGuid();

            await _unitOfWork.LifxLights.AddAsync(lifxLight);
            Task.WaitAll();
            await _unitOfWork.CommitAsync();
        }

        public LifxLight FindByUuidAsync(string uuid)
        {
            return _unitOfWork.LifxLights.Find(p => p.Uuid.ToString() == uuid).FirstOrDefault(); ;
        }

        public async Task Update(string uuid, LifxLight newValues)
        {
            LifxLight oldValues = FindByUuidAsync(uuid);

            //TODO: make specific exception
            if (oldValues is null) throw new Exception("Uuid not found");

            oldValues.TimeRangeEnabled = newValues.TimeRangeEnabled;
            oldValues.TimeRangeStart = newValues.TimeRangeStart;
            oldValues.TimeRangeEnd = newValues.TimeRangeEnd;

            
            if (newValues.MediumTrafficAmount > 0 || newValues.MediumTrafficAmount < newValues.HighTrafficAmount) oldValues.MediumTrafficAmount = newValues.MediumTrafficAmount;
            if (newValues.MediumTrafficAmount > 0 || newValues.MediumTrafficAmount > newValues.HighTrafficAmount) oldValues.HighTrafficAmount = newValues.HighTrafficAmount;

            if (newValues.LowTrafficColor is not null) oldValues.LowTrafficColor = newValues.LowTrafficColor;
            oldValues.LowTrafficBrightness = newValues.LowTrafficBrightness;
            if (newValues.MediumTrafficColor is not null) oldValues.MediumTrafficColor = newValues.MediumTrafficColor;
            oldValues.MediumTrafficBrightness = newValues.MediumTrafficBrightness;
            if (newValues.HighTrafficColor is not null) oldValues.HighTrafficColor = newValues.HighTrafficColor;
            oldValues.HighTrafficBrightness = newValues.HighTrafficBrightness;

            oldValues.Active = newValues.Active;

            oldValues.VeryHighTrafficAmount = newValues.VeryHighTrafficAmount;
            oldValues.VeryHighTrafficCycleTime = newValues.VeryHighTrafficCycleTime;
            oldValues.VeryHighTrafficFirstColor = newValues.VeryHighTrafficFirstColor;
            oldValues.VeryHighTrafficSecondColor = newValues.VeryHighTrafficSecondColor;
            oldValues.EffectCooldownInMinutes = newValues.EffectCooldownInMinutes;
            oldValues.PulseAmount = newValues.PulseAmount;

            oldValues.LifxApiKey = newValues.LifxApiKey;

            if (oldValues.LightGroupName is not null) oldValues.LightGroupName = newValues.LightGroupName;

            _unitOfWork.LifxLights.Update(oldValues);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(string uuid)
        {
            _unitOfWork.LifxLights.Remove(FindByUuidAsync(uuid));
            await _unitOfWork.CommitAsync();
        }
    }
}
