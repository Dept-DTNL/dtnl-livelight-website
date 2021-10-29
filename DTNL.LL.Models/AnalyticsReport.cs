namespace DTNL.LL.Models
{
    public class AnalyticsReport
    {
        public int ActiveUsers { get; set; } = 0;

        public int Conversions { get; set; } = 0;

        public Project Project { get; set; }
    }
}
