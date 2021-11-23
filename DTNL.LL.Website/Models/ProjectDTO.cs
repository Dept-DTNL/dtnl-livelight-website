﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DTNL.LL.Models;

namespace DTNL.LL.Website.Models
{
    public class ProjectDTO
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string ProjectName { get; set; }
        public bool Active { get; set; }

        [Range(1, int.MaxValue)]
        public int PollingTimeInMinutes { get; set; }
        public AnalyticsVersion AnalyticsVersion { get; set; }
        public virtual ICollection<LifxLightDTO> LifxLight { get; set; }
        public string GaProperty { get; set; }
        public List<string> ConversionTags { get; set; }

        public ProjectDTO()
        {
            LifxLight = new List<LifxLightDTO>();
            ConversionTags = new List<string>();
        }


        // Turns Project to a ProjectDTO
        public static ProjectDTO TurnProjectToProjectDTO(Project project)
        {
            ProjectDTO dto = new ProjectDTO()
            {
                ProjectName = project.ProjectName,
                Active = project.Active,
                CustomerName = project.CustomerName,
                Id = project.Id,
                PollingTimeInMinutes = project.PollingTimeInMinutes,
                AnalyticsVersion = project.AnalyticsVersion,
                GaProperty = project.GaProperty,
                ConversionTags = project.ConversionTags,
                LifxLight = project.LifxLights.Select(p => LifxLightDTO.LifxLightToLifxLightDTO(p)).ToList()
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
                PollingTimeInMinutes = dto.PollingTimeInMinutes,
                AnalyticsVersion = dto.AnalyticsVersion,
                GaProperty = dto.GaProperty,
                ConversionTags = dto.ConversionTags,
                LifxLights = dto.LifxLight.Select(p => LifxLightDTO.LifxLightDTOToLifxLight(p)).ToList()


            };

            return project;
        }
    }
}
