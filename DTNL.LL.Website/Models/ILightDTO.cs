using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTNL.LL.Website.Models
{
    public interface ILightDTO
    {
        public Guid Uuid { get; set; }
        public ProjectDTO ProjectDto { get; set; }

        // Time range
        public bool TimeRangeEnabled { get; set; }
        public DateTime TimeRangeStart { get; set; }
        public DateTime TimeRangeEnd { get; set; }
    }
}
