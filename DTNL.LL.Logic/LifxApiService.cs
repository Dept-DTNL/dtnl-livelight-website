using System.Threading.Tasks;
using DTNL.LL.Models;
using LifxCloud.NET;
using LifxCloud.NET.Models;

namespace DTNL.LL.Logic
{
    public static class LifxApiService
    {
        private const double PeakValue = 0.5d;

        private static async Task<LifxCloudClient> CreateClientAsync(string token) => await LifxCloudClient.CreateAsync(token);

        private static Selector CreateLabel(LifxLight l) => new Selector.GroupLabel(l.LightGroupName);

        private static SetStateRequest PowerOffState() => new() {Power = PowerState.Off};

        private static SetStateRequest ColorState(string color, double brightness) => new() {Power = PowerState.On, Color = color, Brightness = brightness, Fast = true};

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color">A color string: https://api.developer.lifx.com/v1/docs/colors </param>
        /// <param name="cycles">The amount of cycles that the lamp will show</param>
        /// <param name="period">Time in seconds for one cycle.</param>
        /// <returns></returns>
        private static BreatheEffectRequest BreatheEffect(string color, double cycles, double period) => new()
            {
                Color = color,
                Cycles = cycles,
                Period = period,
                Peak = PeakValue,
                PowerOn = true
            };

        public static async Task<ApiResponse> DisableLightsAsync(LifxLight lightGroup)
        {
            return await SetLightStateAsync(lightGroup, PowerOffState());
        }

        private static async Task<ApiResponse> SetLightStateAsync(LifxLight lightGroup, SetStateRequest state)
        {
            LifxCloudClient client = await CreateClientAsync(lightGroup.LifxApiKey);
            return await client.SetState(CreateLabel(lightGroup), state);
        }

        public static async Task<ApiResponse> SetLightsColorAsync(LifxLight lightGroup, LampColor color)
        {
            SetStateRequest colorState = ColorState(color.Color, color.Brightness);
            return await SetLightStateAsync(lightGroup, colorState);
        }

        public static async Task<ApiResponse> BreatheLightsAsync(LifxLight lightGroup, string color, int cycles, double period)
        {
            LifxCloudClient client = await CreateClientAsync(lightGroup.LifxApiKey);
            return await client.BreathEffect(CreateLabel(lightGroup), BreatheEffect(color, cycles, period));
        }

    }
}