using System.Threading.Tasks;
using DTNL.LL.Models;
using LifxCloud.NET;
using LifxCloud.NET.Models;

namespace DTNL.LL.Logic
{
    public static class LifxService
    {
        private const double PeakValue = 0.5d;

        private static async Task<LifxCloudClient> CreateClientAsync(string token) => await LifxCloudClient.CreateAsync(token);

        private static Selector CreateLabel(Project p) => new Selector.GroupLabel(p.LightGroupName);

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

        public static async Task<ApiResponse> DisableLightsAsync(Project project)
        {
            return await SetLightStateAsync(project, PowerOffState());
        }

        private static async Task<ApiResponse> SetLightStateAsync(Project project, SetStateRequest state)
        {
            var client = await CreateClientAsync(project.LifxApiKey);
            return await client.SetState(CreateLabel(project), state);
        }

        public static async Task<ApiResponse> SetLightsColorAsync(Project project, LampColor color)
        {
            var colorState = ColorState(color.Color, color.Brightness);
            return await SetLightStateAsync(project, colorState);
        }

        public static async Task<ApiResponse> BreatheLightsAsync(Project project, string color, int cycles, double period)
        {
            var client = await CreateClientAsync(project.LifxApiKey);
            return await client.BreathEffect(CreateLabel(project), BreatheEffect(color, cycles, period));
        }

        //---------------------------------------------------------------------------------------------\\
        private const string apikey = "cfcd89c1b94fe18c1dbb15f11904182dc39e7a707b923223932ae105a5adc772";
        public static async Task TestEnableLight()
        {
            var client = await CreateClientAsync(apikey);
            if(true)
                await client.SetState(new Selector.GroupLabel("LL"), new SetStateRequest()
                    {
                        Power = PowerState.Off,
                        Brightness = 0.25,
                        Color = "green",
                        Fast = true
                    });
            // await client.BreathEffect(new Selector.GroupLabel("LL"), new BreatheEffectRequest()
            //     {
            //         Color = "white saturation:0.0 brightness:0.5",
            //         Cycles = 10,
            //         Peak = 0.5,
            //         Period = 1,
            //         PowerOn = true
            // });
        }
    }
}