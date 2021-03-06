using System;
using System.ComponentModel.DataAnnotations;
using DTNL.LL.Models;

namespace DTNL.LL.Website.Models
{
    public class LifxLightDTO : ILightDto
    {
        public const string COLORREGEX = @"(hue:[0-360])|(#[a-z0-9]{6,})|(rgb:[0-255],[0-255],[0-255])|(white)|(red)|(orange)|(yellow)|(cyan)|(green)|(blue)|(purple)|(pink)";

        public ProjectDTO ProjectDto { get; set; }
        public Guid Uuid { get; set; }
        public bool Active { get; set; }

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
        [RegularExpression(COLORREGEX, ErrorMessage = "Please enter correct color.")]
        public string LowTrafficColor { get; set; }
        [Range(0, 1)]
        public double LowTrafficBrightness { get; set; }
        [RegularExpression(COLORREGEX, ErrorMessage = "Please enter correct color.")]
        public string MediumTrafficColor { get; set; }
        [Range(0, 1)]
        public double MediumTrafficBrightness { get; set; }
        public int MediumTrafficAmount { get; set; }
        [RegularExpression(COLORREGEX, ErrorMessage = "Please enter correct color.")]
        public string HighTrafficColor { get; set; }
        [Range(0, 1)]
        public double HighTrafficBrightness { get; set; }
        public int HighTrafficAmount { get; set; }

        //Amount of time one flash takes in seconds
        public double ConversionPeriod { get; set; }
        [RegularExpression(COLORREGEX, ErrorMessage = "Please enter correct color.")]
        public string ConversionColor { get; set; }

        public int VeryHighTrafficAmount { get; set; }
        [RegularExpression(COLORREGEX, ErrorMessage = "Please enter correct color.")]
        public string VeryHighTrafficFirstColor { get; set; }
        [RegularExpression(COLORREGEX, ErrorMessage = "Please enter correct color.")]
        public string VeryHighTrafficSecondColor { get; set; }
        public double VeryHighTrafficCycleTime { get; set; }
        public int EffectCooldownInMinutes { get; set; }
        public int PulseAmount { get; set; }


        public static LifxLightDTO LifxLightToLifxLightDTOWithProject(LifxLight lifxLight)
        {
            return new LifxLightDTO()
            {
                ProjectDto = ProjectDTO.TurnProjectToProjectDTO(lifxLight.Project),
                Active = lifxLight.Active,
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
                ConversionPeriod = lifxLight.ConversionPeriod,
                ConversionColor = lifxLight.ConversionColor,
                VeryHighTrafficAmount = lifxLight.VeryHighTrafficAmount,
                VeryHighTrafficCycleTime = lifxLight.VeryHighTrafficCycleTime,
                VeryHighTrafficFirstColor = lifxLight.VeryHighTrafficFirstColor,
                VeryHighTrafficSecondColor = lifxLight.VeryHighTrafficSecondColor,
                EffectCooldownInMinutes = lifxLight.EffectCooldownInMinutes,
                PulseAmount = lifxLight.PulseAmount,
            };
        }

        public static LifxLightDTO LifxLightToLifxLightDTO(LifxLight lifxLight)
        {
            return new LifxLightDTO()
            {
                Uuid = lifxLight.Uuid,
                Active = lifxLight.Active,
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
                ConversionPeriod = lifxLight.ConversionPeriod,
                ConversionColor = lifxLight.ConversionColor,
                VeryHighTrafficAmount = lifxLight.VeryHighTrafficAmount,
                VeryHighTrafficCycleTime = lifxLight.VeryHighTrafficCycleTime,
                VeryHighTrafficFirstColor = lifxLight.VeryHighTrafficFirstColor,
                VeryHighTrafficSecondColor = lifxLight.VeryHighTrafficSecondColor,
                EffectCooldownInMinutes = lifxLight.EffectCooldownInMinutes,
                PulseAmount = lifxLight.PulseAmount,
            };
        }

        public static LifxLight LifxLightDTOToLifxLight(LifxLightDTO lifxLightDto)
        {
            return new LifxLight()
            {
                Uuid = lifxLightDto.Uuid,
                Active = lifxLightDto.Active,
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
                ConversionPeriod = lifxLightDto.ConversionPeriod,
                ConversionColor = lifxLightDto.ConversionColor,
                VeryHighTrafficAmount = lifxLightDto.VeryHighTrafficAmount,
                VeryHighTrafficCycleTime = lifxLightDto.VeryHighTrafficCycleTime,
                VeryHighTrafficFirstColor = lifxLightDto.VeryHighTrafficFirstColor,
                VeryHighTrafficSecondColor = lifxLightDto.VeryHighTrafficSecondColor,
                EffectCooldownInMinutes = lifxLightDto.EffectCooldownInMinutes,
                PulseAmount = lifxLightDto.PulseAmount,
            };
        }
    }
}
