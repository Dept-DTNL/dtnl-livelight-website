using System;
using System.ComponentModel.DataAnnotations;

namespace DTNL.LL.Website.Models
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public bool Active { get; set; }
        
        public bool HasTimeRange { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [DataType(DataType.Time)]
        public DateTime TimeRangeStart { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [DataType(DataType.Time)]
        public DateTime TimeRangeEnd { get; set; }
    }
}
