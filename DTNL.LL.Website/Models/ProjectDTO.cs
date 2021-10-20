using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.RegularExpressions;
using DTNL.LL.Models;
using Microsoft.JSInterop.Infrastructure;

namespace DTNL.LL.Website.Models
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public bool Active { get; set; }

        //Time Range
        public bool HasTimeRange { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [DataType(DataType.Time)]
        public DateTime TimeRangeStart { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        [DataType(DataType.Time)]
        public DateTime TimeRangeEnd { get; set; }

        // Lamp connection
        public string LightGroupName { get; set; }
        public bool GuideEnabled { get; set; }

        // Lamp light setting
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
        public double ConversionPeriod { get; set; }
        [RegularExpression(@"(hue:[0-360])|(#[a-z0-9]{6,})|(rgb:[0-255],[0-255],[0-255])|(white)|(red)|(orange)|(yellow)|(cyan)|(green)|(blue)|(purple)|(pink)", ErrorMessage = "Please enter correct color.")]
        public string ConversionColor { get; set; }

        // Turns Project to a ProjectDTO
        public static ProjectDTO TurnProjectToProjectDTO(Project project)
        {
            ProjectDTO dto = new ProjectDTO()
            {
                ProjectName = project.ProjectName,
                Active = project.Active,
                CustomerName = project.CustomerName,
                Id = project.Id,
                GuideEnabled = project.GuideEnabled,
                LightGroupName = project.LightGroupName,
                HasTimeRange = project.TimeRangeEnabled,
                TimeRangeStart = new DateTime(1, 1, 1, project.TimeRangeStart.Hours, project.TimeRangeStart.Minutes, project.TimeRangeStart.Seconds),
                TimeRangeEnd = new DateTime(1, 1, 1, project.TimeRangeEnd.Hours, project.TimeRangeEnd.Minutes, project.TimeRangeEnd.Seconds),
                LowTrafficColor = project.LowTrafficColor,
                LowTrafficBrightness = project.LowTrafficBrightness,
                MediumTrafficAmount = project.MediumTrafficAmount,
                MediumTrafficColor = project.MediumTrafficColor,
                MediumTrafficBrightness = project.MediumTrafficBrightness,
                HighTrafficColor = project.HighTrafficColor,
                HighTrafficBrightness = project.HighTrafficBrightness,
                HighTrafficAmount = project.HighTrafficAmount,
                ConversionColor = project.ConversionColor,
                ConversionCycle = project.ConversionCycle,
                ConversionPeriod = project.ConversionPeriod
            };

            return dto;
        }

        // Turns Project to a ProjectDTO
        public static Project TurnProjectDTOToProject(ProjectDTO dto)
        {
            Project project = new Project()
            {
                ProjectName = dto.ProjectName,
                Active = dto.Active,
                CustomerName = dto.CustomerName,
                Id = dto.Id,
                GuideEnabled = dto.GuideEnabled,
                LightGroupName = dto.LightGroupName,
                TimeRangeEnabled = dto.HasTimeRange,
                TimeRangeStart = new TimeSpan(dto.TimeRangeStart.Hour, dto.TimeRangeStart.Minute, dto.TimeRangeStart.Second),
                TimeRangeEnd = new TimeSpan(dto.TimeRangeEnd.Hour, dto.TimeRangeEnd.Minute, dto.TimeRangeEnd.Second),
                LowTrafficColor = dto.LowTrafficColor,
                LowTrafficBrightness = dto.LowTrafficBrightness,
                MediumTrafficAmount = dto.MediumTrafficAmount,
                MediumTrafficColor = dto.MediumTrafficColor,
                MediumTrafficBrightness = dto.MediumTrafficBrightness,
                HighTrafficColor = dto.HighTrafficColor,
                HighTrafficBrightness = dto.HighTrafficBrightness,
                HighTrafficAmount = dto.HighTrafficAmount,
                ConversionColor = dto.ConversionColor,
                ConversionCycle = dto.ConversionCycle,
                ConversionPeriod = dto.ConversionPeriod
            };

            return project;
        }
    }
}
