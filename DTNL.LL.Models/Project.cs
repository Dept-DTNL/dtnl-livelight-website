using System;
using System.Collections.Generic;

namespace DTNL.LL.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }

        // Worker
        public int PollingTimeInMinutes { get; set; }

        public bool Active { get; set; } = true;

        /// <summary>
        /// This is used to
        /// </summary>
        public int ConversionDivision { get; set; }

        public virtual ICollection<LifxLight> LifxLights { get; set; }

        /// <summary>
        /// New model should be appended when a new ILight type is added to the model.
        /// </summary>
        /// <returns></returns>
        public List<ILight> GetLights()
        {
            List<ILight> lights = new();
            if (LifxLights is not null)
                lights.AddRange(LifxLights);

            return lights;
        }

        public void AddLight(ILight light)
        {
            switch (light)
            {
                case LifxLight lifx:
                    LifxLights.Add(lifx);
                    return;
                default:
                    throw new NotImplementedException("Light type not implemented!");
            }
        }

        // Analytics
        public string GaProperty { get; set; }
        public AnalyticsVersion AnalyticsVersion { get; set; }
        public List<string> ConversionTags { get; set; }

    }
}
