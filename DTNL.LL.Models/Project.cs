using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTNL.LL.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string ProjectName { get; set; }
        
        [Required]
        [DataType(DataType.Text)]
        public string CustomerName { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        [Range(typeof(bool), "false", "true")]
        public bool Active { get; set; } = true;
        public virtual ICollection<Lamp> Lamps { get; set; } = new List<Lamp>();

        public bool HasTimeRange { get; set; } = true;
        public TimeSpan? TimeRangeStart { get; set; }
        public TimeSpan? TimeRangeEnd { get; set; }
    }
}
