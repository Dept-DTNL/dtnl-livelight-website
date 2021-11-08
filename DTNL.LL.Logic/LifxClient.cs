using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTNL.LL.Logic.Exceptions;
using DTNL.LL.Models;
using LifxCloud.NET;
using LifxCloud.NET.Models;

namespace DTNL.LL.Logic
{
    public class LifxClient
    {
        private const double PeakValue = 0.5d;
        private const string InvalidCredentialsErrorMessage = "Invalid credentials";
        private const string InvalidGroupNameErrorMessage = "Could not find group";
        
        private static Task<LifxCloudClient> CreateClientAsync(string token) => LifxCloudClient.CreateAsync(token);

        private Selector CreateLabel(LifxLight l) => new Selector.GroupLabel(l.LightGroupName);

        private SetStateRequest PowerOffState() => new() {Power = PowerState.Off};

        private SetStateRequest ColorState(string color, double brightness) => new() {Power = PowerState.On, Color = color, Brightness = brightness, Fast = true};

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color">A color string: https://api.developer.lifx.com/v1/docs/colors </param>
        /// <param name="cycles">The amount of cycles that the lamp will show</param>
        /// <param name="period">Time in seconds for one cycle.</param>
        /// <returns></returns>
        private BreatheEffectRequest BreatheEffect(string color, string baseColor, double cycles, double period) => new()
            {
                Color = color,
                FromColor = baseColor,
                Cycles = cycles,
                Period = period,
                Peak = PeakValue,
                PowerOn = true
            };

        public Task<ApiResponse> DisableLightsAsync(LifxLight lightGroup) => SetLightStateAsync(lightGroup, PowerOffState());

        private async Task<ApiResponse> SetLightStateAsync(LifxLight lightGroup, SetStateRequest state)
        {
            LifxCloudClient client = await lightGroup.GetClient();
            return await client.SetState(CreateLabel(lightGroup), state);
        }

        public Task<ApiResponse> SetLightsColorAsync(LifxLight lightGroup, LampColor color)
        {
            SetStateRequest colorState = ColorState(color.Color, color.Brightness);
            return SetLightStateAsync(lightGroup, colorState);
        }

        public async Task<ApiResponse> BreatheLightsAsync(LifxLight lightGroup, string color, LampColor baseColor, int cycles, double period)
        {
            LifxCloudClient client = await lightGroup.GetClient();
            return await client.BreathEffect(CreateLabel(lightGroup), BreatheEffect(color, baseColor.Color, cycles, period));
        }

        /// <summary>
        /// This method checks if there is any Lifx light connected to a group. If the method succeeds nothing is returned. In case of failure an exception is thrown.
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="groupName"></param>
        /// <exception cref="InvalidApiKeyException">The given api key is invalid</exception>
        /// <exception cref="GroupNotFoundException">A group with the specified name does not exist.</exception>
        /// <exception cref="NoLightsOnlineException">The key and group name are valid but there is no light connected to the internet.</exception>
        /// <returns></returns>
        public async Task HasValidLightAsync(string apiKey, string groupName)
        {
            try
            {
                Selector selector = new Selector.GroupLabel(groupName);
                LifxCloudClient client = await CreateClientAsync(apiKey);
                IEnumerable<Light> lights = await client.ListLights(selector);

                bool anyLightConnected = false;

                foreach (Light light in lights)
                {
                    if (!light.IsConnected) continue;
                    anyLightConnected = true;
                    break;
                }

                if (!anyLightConnected)
                    throw new NoLightsOnlineException();
            }
            catch (Exception exception)
            {
                if (exception.Message.StartsWith(InvalidCredentialsErrorMessage))
                    throw new InvalidApiKeyException();

                if (exception.Message.StartsWith(InvalidGroupNameErrorMessage))
                    throw new GroupNotFoundException();

                throw;
            }
        }
    }
}