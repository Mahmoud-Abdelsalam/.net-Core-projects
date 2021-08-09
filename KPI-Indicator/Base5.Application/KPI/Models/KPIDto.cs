using Base5.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace Base5.Application.KPI.Models
{
    public class KPIDto
    {
        public KPIDto()
        {
            KPIDetails = new List<KPIDetailsDto>();
        }

        [DisplayName("Department")]
        public Dep DepNo { get; set; }

        [DisplayName("Percentage")]
        public bool MeasurmentUnit { get; set; }

        public int Total { get; set; }

        public List<KPIDetailsDto> KPIDetails { get; set; }
    }
}