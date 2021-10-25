using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTNL.LL.Models
{
    public class Project
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        public string ProjectName { get; set; }
        
        [DataType(DataType.Text)]
        public string CustomerName { get; set; }
        
        [Display(Name = "Is Active")]
        [Range(typeof(bool), "false", "true")]
        public bool Active { get; set; } = true;

        // Time range
        public bool TimeRangeEnabled { get; set; } = true;
        public TimeSpan TimeRangeStart { get; set; }
        public TimeSpan TimeRangeEnd { get; set; }
        
        public string GaProperty { get; set; }

        public int PollingTimeInMinutes { get; set; }

        public AnalyticsVersion AnalyticsVersion { get; set; }

        public List<string> ConversionTags { get; set; }
        

        // Lamp connection
        public string LifxApiKey { get; set; }
        public string LightGroupName { get; set; }
        public Guid Uuid { get;  set; } 
        public bool GuideEnabled { get; set; }

        // Lamp light setting
        public string LowTrafficColor { get; set; }
        public double LowTrafficBrightness { get; set; }
        public string MediumTrafficColor { get; set; }
        public double MediumTrafficBrightness { get; set; }
        public int MediumTrafficAmount { get; set; }
        public string HighTrafficColor { get; set; }
        public double HighTrafficBrightness { get; set; }
        public int HighTrafficAmount { get; set; }
        public int ConversionCycle { get; set; }
        public double ConversionPeriod { get; set; }
        public string ConversionColor { get; set; }
    }
}
