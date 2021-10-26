using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTNL.LL.Website.Models
{
    public class AllLights
    {
        public List<string> Lights = new List<string>() {"LIFX", "other"};
        public string WhichLightUsed { get; set; }
        public LifxLightDTO LifxLightDto { get; set; }
    }
}
