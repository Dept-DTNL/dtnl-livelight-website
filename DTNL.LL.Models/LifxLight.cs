using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using LifxCloud.NET;

namespace DTNL.LL.Models
{
    /// <summary>
    /// This class represents a group in the Lifx app. API requests will change all lights contained in the group. 
    /// </summary>
    public class LifxLight : ILight
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }

        public virtual Project Project { get; set; }
        public bool Active { get; set; }

        // Lamp connection
        public string LifxApiKey { get; set; }
        public string LightGroupName { get; set; }

        // Time range
        public bool TimeRangeEnabled { get; set; } = true;
        public TimeSpan TimeRangeStart { get; set; }
        public TimeSpan TimeRangeEnd { get; set; }


        // Color Settings
        public string LowTrafficColor { get; set; }
        public double LowTrafficBrightness { get; set; }
        public int MediumTrafficAmount { get; set; }
        public string MediumTrafficColor { get; set; }
        public double MediumTrafficBrightness { get; set; }
        public string HighTrafficColor { get; set; }
        public double HighTrafficBrightness { get; set; }
        public int HighTrafficAmount { get; set; }


        public int VeryHighTrafficAmount { get; set; }
        public string VeryHighTrafficFirstColor { get; set; }
        public string VeryHighTrafficSecondColor { get; set; }
        public double VeryHighTrafficCycleTime { get; set; }

        [Range(1, int.MaxValue)]
        public int PulseAmount { get; set; }
        [Range(1,10080)]
        public int EffectCooldownInMinutes { get; set; }

        //Amount of time one flash takes in seconds
        public double ConversionPeriod { get; set; }
        public string ConversionColor { get; set; }

        [NotMapped] private LifxCloudClient _lifxClient;

        public async Task<LifxCloudClient> GetClient() => _lifxClient ??= await LifxCloudClient.CreateAsync(LifxApiKey);
    }
}
