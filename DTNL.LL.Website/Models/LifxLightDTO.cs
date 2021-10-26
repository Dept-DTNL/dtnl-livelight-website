using System;
using System.ComponentModel.DataAnnotations;
using DTNL.LL.Models;

namespace DTNL.LL.Website.Models
{
    public class LifxLightDTO : ILightDTO
    {
        public ProjectDTO ProjectDto { get; set; }
        public Guid Uuid { get; set; }
        public bool GuideEnabled { get; set; }

        // Lamp connection
        public string LifxApiKey { get; set; }
        public string LightGroupName { get; set; }

        // Time range
        public bool TimeRangeEnabled { get; set; } = true;
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [DataType(DataType.Time)]
        public DateTime TimeRangeStart { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [DataType(DataType.Time)]
        public DateTime TimeRangeEnd { get; set; }

        // Color Settings
        [RegularExpression(@"(hue:[0-360])|(#[a-z0-9]{6,})|(rgb:[0-255],[0-255],[0-255])|(white)|(red)|(orange)|(yellow)|(cyan)|(green)|(blue)|(purple)|(pink)", ErrorMessage = "Please enter correct color.")]
        public string LowTrafficColor { get; set; }
        [Range(0, 1)]
        public double LowTrafficBrightness { get; set; }
        [RegularExpression(@"(hue:[0-360])|(#[a-z0-9]{6,})|(rgb:[0-255],[0-255],[0-255])|(white)|(red)|(orange)|(yellow)|(cyan)|(green)|(blue)|(purple)|(pink)", ErrorMessage = "Please enter correct color.")]
        public string MediumTrafficColor { get; set; }
        [Range(0, 1)]
        public double MediumTrafficBrightness { get; set; }
        [RegularExpression(@"(hue:[0-360])|(#[a-z0-9]{6,})|(rgb:[0-255],[0-255],[0-255])|(white)|(red)|(orange)|(yellow)|(cyan)|(green)|(blue)|(purple)|(pink)", ErrorMessage = "Please enter correct color.")]
        public int MediumTrafficAmount { get; set; }
        public string HighTrafficColor { get; set; }
        [Range(0, 1)]
        public double HighTrafficBrightness { get; set; }
        public int HighTrafficAmount { get; set; }
        public int ConversionCycle { get; set; }

        //Amount of time one flash takes in seconds
        public double ConversionPeriod { get; set; }
        [RegularExpression(@"(hue:[0-360])|(#[a-z0-9]{6,})|(rgb:[0-255],[0-255],[0-255])|(white)|(red)|(orange)|(yellow)|(cyan)|(green)|(blue)|(purple)|(pink)", ErrorMessage = "Please enter correct color.")]
        public string ConversionColor { get; set; }

        public static LifxLightDTO LifxLightToLifxLightDTOWithProject(LifxLight lifxLight)
        {
            return new LifxLightDTO()
            {
                ProjectDto = ProjectDTO.TurnProjectToProjectDTO(lifxLight.Project),
                Uuid = lifxLight.Uuid,
                LifxApiKey = lifxLight.LifxApiKey,
                LightGroupName = lifxLight.LightGroupName,
                TimeRangeEnabled = lifxLight.TimeRangeEnabled,
                TimeRangeStart = new DateTime(1, 1, 1, lifxLight.TimeRangeStart.Hours, lifxLight.TimeRangeStart.Minutes, lifxLight.TimeRangeStart.Seconds),
                TimeRangeEnd = new DateTime(1, 1, 1, lifxLight.TimeRangeEnd.Hours, lifxLight.TimeRangeEnd.Minutes, lifxLight.TimeRangeEnd.Seconds),
                LowTrafficColor = lifxLight.LowTrafficColor,
                LowTrafficBrightness = lifxLight.LowTrafficBrightness,
                MediumTrafficAmount = lifxLight.MediumTrafficAmount,
                MediumTrafficColor = lifxLight.MediumTrafficColor,
                MediumTrafficBrightness = lifxLight.MediumTrafficBrightness,
                HighTrafficColor = lifxLight.HighTrafficColor,
                HighTrafficBrightness = lifxLight.HighTrafficBrightness,
                HighTrafficAmount = lifxLight.HighTrafficAmount,
                ConversionCycle = lifxLight.ConversionCycle,
                ConversionPeriod = lifxLight.ConversionPeriod,
                ConversionColor = lifxLight.ConversionColor,
                GuideEnabled = lifxLight.GuideEnabled
            };
        }

        public static LifxLightDTO LifxLightToLifxLightDTO(LifxLight lifxLight)
        {
            return new LifxLightDTO()
            {
                Uuid = lifxLight.Uuid,
                LifxApiKey = lifxLight.LifxApiKey,
                LightGroupName = lifxLight.LightGroupName,
                TimeRangeEnabled = lifxLight.TimeRangeEnabled,
                TimeRangeStart = new DateTime(1, 1, 1, lifxLight.TimeRangeStart.Hours, lifxLight.TimeRangeStart.Minutes, lifxLight.TimeRangeStart.Seconds),
                TimeRangeEnd = new DateTime(1, 1, 1, lifxLight.TimeRangeEnd.Hours, lifxLight.TimeRangeEnd.Minutes, lifxLight.TimeRangeEnd.Seconds),
                LowTrafficColor = lifxLight.LowTrafficColor,
                LowTrafficBrightness = lifxLight.LowTrafficBrightness,
                MediumTrafficAmount = lifxLight.MediumTrafficAmount,
                MediumTrafficColor = lifxLight.MediumTrafficColor,
                MediumTrafficBrightness = lifxLight.MediumTrafficBrightness,
                HighTrafficColor = lifxLight.HighTrafficColor,
                HighTrafficBrightness = lifxLight.HighTrafficBrightness,
                HighTrafficAmount = lifxLight.HighTrafficAmount,
                ConversionCycle = lifxLight.ConversionCycle,
                ConversionPeriod = lifxLight.ConversionPeriod,
                ConversionColor = lifxLight.ConversionColor,
                GuideEnabled = lifxLight.GuideEnabled
            };
        }

        public static LifxLight LifxLightDTOToLifxLight(LifxLightDTO lifxLightDto)
        {
            return new LifxLight()
            {
                Uuid = lifxLightDto.Uuid,
                LifxApiKey = lifxLightDto.LifxApiKey,
                LightGroupName = lifxLightDto.LightGroupName,
                TimeRangeEnabled = lifxLightDto.TimeRangeEnabled,
                TimeRangeStart = new TimeSpan(lifxLightDto.TimeRangeStart.Hour, lifxLightDto.TimeRangeStart.Minute, lifxLightDto.TimeRangeStart.Second),
                TimeRangeEnd = new TimeSpan(lifxLightDto.TimeRangeEnd.Hour, lifxLightDto.TimeRangeEnd.Minute, lifxLightDto.TimeRangeEnd.Second),
                LowTrafficColor = lifxLightDto.LowTrafficColor,
                LowTrafficBrightness = lifxLightDto.LowTrafficBrightness,
                MediumTrafficAmount = lifxLightDto.MediumTrafficAmount,
                MediumTrafficColor = lifxLightDto.MediumTrafficColor,
                MediumTrafficBrightness = lifxLightDto.MediumTrafficBrightness,
                HighTrafficColor = lifxLightDto.HighTrafficColor,
                HighTrafficBrightness = lifxLightDto.HighTrafficBrightness,
                HighTrafficAmount = lifxLightDto.HighTrafficAmount,
                ConversionCycle = lifxLightDto.ConversionCycle,
                ConversionPeriod = lifxLightDto.ConversionPeriod,
                ConversionColor = lifxLightDto.ConversionColor,
                GuideEnabled = lifxLightDto.GuideEnabled
            };
        }
    }
}
