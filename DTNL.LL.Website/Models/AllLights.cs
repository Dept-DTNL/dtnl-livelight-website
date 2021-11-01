using System.Collections.Generic;

namespace DTNL.LL.Website.Models
{
    public class AllLights
    {
        private List<string> lights = new List<string>() {"LIFX", "other"};
        public List<string> Lights { get; set; }
        public string WhichLightUsed { get; set; }
        public LifxLightDTO LifxLightDto { get; set; }
    }
}
