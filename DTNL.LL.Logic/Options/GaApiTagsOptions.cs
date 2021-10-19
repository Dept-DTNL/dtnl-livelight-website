namespace DTNL.LL.Logic.Options
{
    public class GaApiTagsOptions
    {
        public const string GaApiTags = "GaApiTags";

        public string Ga4Properties { get; set; }
        public string Ga4ActiveUsers { get; set; }
        public string Ga4Conversions { get; set; }
        public string Ga4EventName { get; set; }
        public string Ga3ActiveUsers { get; set; }
        public string Ga3MinutesAgo { get; set; }
    }
}