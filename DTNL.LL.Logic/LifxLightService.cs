using System;
using System.Threading.Tasks;
using DTNL.LL.Models;

namespace DTNL.LL.Logic
{
    public static  class LifxLightService
    {
        private const int SecondsInAMinute = 60;

        public static Task UpdateLightColors(LifxLight lightGroup, int users)
        {
            bool awake = IsTimeOfDayBetween(DateTime.Now, lightGroup.TimeRangeStart, lightGroup.TimeRangeEnd);
            if (!awake)
            {
                return LifxApiService.DisableLightsAsync(lightGroup);
            }
            LampColor color = GetActivityColor(lightGroup, users);
            return LifxApiService.SetLightsColorAsync(lightGroup, color);
        }

        private static LampColor GetActivityColor(LifxLight lightGroup, int users)
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

        public static Task FlashLightForConversions(LifxLight lightGroup, int flashes, int pollingTimeInMinutes)
        {

            bool awake = IsTimeOfDayBetween(DateTime.Now, lightGroup.TimeRangeStart, lightGroup.TimeRangeEnd);
            if (!awake || flashes == 0)
                return Task.CompletedTask;

            // Makes sure the lamp doesn't flash more than the time it takes for the next polling.
            double maxAmountOfCycles = (pollingTimeInMinutes * SecondsInAMinute) / lightGroup.ConversionPeriod;
            int maxAmountOfCyclesRounded = Convert.ToInt32(maxAmountOfCycles);

            int cycles = Math.Min(flashes, maxAmountOfCyclesRounded);

            return LifxApiService.BreatheLightsAsync(lightGroup, lightGroup.ConversionColor, cycles, lightGroup.ConversionPeriod);
        }

        public static bool IsTimeOfDayBetween(DateTime time,
            TimeSpan startTime, TimeSpan endTime)
        {
            if (endTime < startTime)
                return time.TimeOfDay <= endTime || time.TimeOfDay >= startTime;

            return time.TimeOfDay >= startTime && time.TimeOfDay <= endTime;
        }
    }
}