using System;

namespace DTNL.LL.Website.Models
{
    public interface ILightDto
    {
        public Guid Uuid { get; set; }
        public ProjectDTO ProjectDto { get; set; }
        public bool GuideEnabled { get; set; }
        public bool Active { get; set; }

        // Time range
        public bool TimeRangeEnabled { get; set; }
        public DateTime TimeRangeStart { get; set; }
        public DateTime TimeRangeEnd { get; set; }
    }
}
