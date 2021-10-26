using System;

namespace DTNL.LL.Models
{
    public interface ILight
    {
        public int Id { get; set; }
        public Guid Uuid { get; set; }

        public Project Project { get; set; }

        // Time range
        public bool TimeRangeEnabled { get; set; }
        public TimeSpan TimeRangeStart { get; set; }
        public TimeSpan TimeRangeEnd { get; set; }

    }
}