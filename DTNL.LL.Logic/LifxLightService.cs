using System;
using System.Threading.Tasks;
using DTNL.LL.Models;
using Microsoft.Extensions.Caching.Memory;

namespace DTNL.LL.Logic
{
    public class LifxLightService
    {
        private const int SecondsInAMinute = 60;

        private readonly LifxClient _lifxClient;
        private readonly IMemoryCache _cache;

        public LifxLightService(LifxClient lifxClient, IMemoryCache memoryCache)
        {
            _lifxClient = lifxClient;
            _cache = memoryCache;
        }

        public async Task UpdateLightColors(LifxLight lightGroup, int users, int pollingTimeInMinutes)
        {
            bool awake = IsTimeOfDayBetween(DateTime.Now, lightGroup.TimeRangeStart, lightGroup.TimeRangeEnd);
            if (lightGroup.TimeRangeEnabled && !awake)
            {
                await _lifxClient.DisableLightsAsync(lightGroup);
            }

            // High volume animation check
            if (await ProcessedHighVolumeAnimation(lightGroup, users))
                return;
            
            LampColor color = GetActivityColor(lightGroup, users);

            // This would be smoother with a breathe effect with persist set to true.
            // However this is not yet in the lifxclient yet so that could be contributed in the future.
            await _lifxClient.SetLightsColorAsync(lightGroup, color);
        }


        //Todo: Clean this method up
        /// <summary>
        /// Method activates the special animation if the website has extremely high traffic and is not on cooldown.
        /// </summary>
        /// <param name="lightGroup"></param>
        /// <param name="users"></param>
        /// <returns>True if the light should not be processed further.</returns>
        private async Task<bool> ProcessedHighVolumeAnimation(LifxLight lightGroup, int users)
        {
            if (lightGroup.VeryHighTrafficAmount <= 0 || lightGroup.VeryHighTrafficAmount >= users)
                // Animation Disabled
                return false;

            // Append lg to Lightgroup as identifier.
            //LG Cooldown ID
            string cacheCDId = "lg:" + lightGroup.Id;

            bool animationOnCooldown = _cache.TryGetValue(cacheCDId, out DateTime animationActiveTime);
            if (animationActiveTime > DateTime.UtcNow)
                // Animation still active, return true to prevent change to normal color.
                return true;

            if (animationOnCooldown)
                return false;

            MemoryCacheEntryOptions cdCacheOptions = new()
            {
                Priority = CacheItemPriority.NeverRemove,
                AbsoluteExpirationRelativeToNow = new TimeSpan(0, lightGroup.EffectCooldownInMinutes, 0)
            };

            //Light should be pulsing until this timespan
            int pulsingTimeInSeconds = (int)(lightGroup.PulseAmount * lightGroup.VeryHighTrafficCycleTime);
            //The amount of pulses the light makes
            int pulseAmount = pulsingTimeInSeconds < 60 ? lightGroup.PulseAmount : int.MaxValue;
            // The datetime until which the pulsing animation lasts
            DateTime pulsingDateTime = DateTime.UtcNow.Add(new TimeSpan(0, 0, pulsingTimeInSeconds));
            _cache.Set(cacheCDId, pulsingDateTime, cdCacheOptions);

            await _lifxClient.BreatheLightsAsync(lightGroup, lightGroup.VeryHighTrafficFirstColor, new LampColor()
            {
                Color = lightGroup.VeryHighTrafficSecondColor
            }, int.MaxValue, pulseAmount);

            return true;
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

        public bool IsConversionValidColor(LifxLight lightGroup)
        {
            return  _lifxClient.IsConversionValidColor(lightGroup).Result;
        }
    }
}