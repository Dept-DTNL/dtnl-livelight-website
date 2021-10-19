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
        public virtual ICollection<Lamp> Lamps { get; set; } = new List<Lamp>();

        public bool TimeRangeEnabled { get; set; } = true;
        public TimeSpan? TimeRangeStart { get; set; }
        public TimeSpan? TimeRangeEnd { get; set; }
        
        public string GaProperty { get; set; }

        public int PollingTimeInMinutes { get; set; }

        public AnalyticsVersion AnalyticsVersion { get; set; }

        public List<string> ConversionTags { get; set; }

        public virtual ICollection<Lamp> Lamps { get; set; } = new List<Lamp>();
    }
}
