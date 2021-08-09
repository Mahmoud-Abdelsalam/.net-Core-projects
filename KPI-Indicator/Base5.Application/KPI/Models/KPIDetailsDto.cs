using AutoMapper;
using Base5.Application.Interfaces.Mapping;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Base5.Application.KPI.Models
{
    public class KPIDetailsDto : IHaveCustomMapping
    {
        [Required]
        [Remote(action: "IsKpiInUse", controller: "KPI")]
        public int KPIDNum { get; set; }

        [Required]
        public string KPIDDescription { get; set; }

        public bool MeasurmentUnit { get; set; }

        [Required]
        public int TargetValue { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Domain.Entities.KPI, KPIDetailsDto>();
        }
    }
}