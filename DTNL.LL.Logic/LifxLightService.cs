using System;
using System.Threading.Tasks;
using DTNL.LL.Models;

namespace DTNL.LL.Logic
{
    public class LifxLightService
    {
        private const int SecondsInAMinute = 60;

        private readonly LifxClient _lifxClient;

        public LifxLightService(LifxClient lifxClient)
        {
            _lifxClient = lifxClient;
        }

        public Task UpdateLightColors(LifxLight lightGroup, int users)
        {
            bool awake = IsTimeOfDayBetween(DateTime.Now, lightGroup.TimeRangeStart, lightGroup.TimeRangeEnd);
            if (lightGroup.TimeRangeEnabled && !awake)
            {
                return _lifxClient.DisableLightsAsync(lightGroup);
            }
            LampColor color = GetActivityColor(lightGroup, users);
            return _lifxClient.SetLightsColorAsync(lightGroup, color);
        }

        private LampColor GetActivityColor(LifxLight lightGroup, int users)
        {
            if (lightGroup.HighTrafficAmount <= users)
            {
                return new LampColor()
                {
                    Color = lightGroup.HighTrafficColor,
                    Brightness = lightGroup.HighTrafficBrightness
                };
            }
            if (lightGroup.MediumTrafficAmount <= users)
            {
                return new LampColor()
                {
                    Color = lightGroup.MediumTrafficColor,
                    Brightness = lightGroup.MediumTrafficBrightness
                };
            }

            return new LampColor()
            {
                Color = lightGroup.LowTrafficColor,
                Brightness = lightGroup.LowTrafficBrightness
            };
        }

        public Task FlashLightForConversions(LifxLight lightGroup, int flashes, int activeUsers, int pollingTimeInMinutes)
        {

            bool awake = IsTimeOfDayBetween(DateTime.Now, lightGroup.TimeRangeStart, lightGroup.TimeRangeEnd);
            if ((lightGroup.TimeRangeEnabled && !awake) || flashes == 0)
                return Task.CompletedTask;

            // Makes sure the lamp doesn't flash more than the time it takes for the next polling.
            double maxAmountOfCycles = (pollingTimeInMinutes * SecondsInAMinute) / lightGroup.ConversionPeriod;
            int maxAmountOfCyclesRounded = Convert.ToInt32(maxAmountOfCycles);

            int cycles = Math.Min(flashes, maxAmountOfCyclesRounded);
            LampColor baseColor = GetActivityColor(lightGroup, activeUsers);
            return _lifxClient.BreatheLightsAsync(lightGroup, lightGroup.ConversionColor, baseColor, cycles, lightGroup.ConversionPeriod);
        }

        public bool IsTimeOfDayBetween(DateTime time,
            TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime < startTime)
                return time.TimeOfDay <= endTime || time.TimeOfDay >= startTime;

            return time.TimeOfDay >= startTime && time.TimeOfDay <= endTime;
        }

    }
}