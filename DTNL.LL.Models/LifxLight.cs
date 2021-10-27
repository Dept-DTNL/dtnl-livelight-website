using System;
using System.ComponentModel.DataAnnotations;

namespace DTNL.LL.Models
{
    public class LifxLight : ILight
    {
        public int Id { get; set; }

        public virtual Project Project { get; set; }


        public Guid Uuid { get; set; }

        // Lamp connection
        public string LifxApiKey { get; set; }
        public string LightGroupName { get; set; }

        [Display(Name = "Is Active")]
        public bool Active { get; set; } = true;

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

        //Amount of time one flash takes in seconds
        public double ConversionPeriod { get; set; }
        public string ConversionColor { get; set; }
    }
}