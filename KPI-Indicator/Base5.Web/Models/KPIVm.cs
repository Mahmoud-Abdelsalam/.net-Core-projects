using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base5.Domain.Enums;

namespace Base5.Web.Models
{
    public class KPIVm
    {
        public int KPIDNum { get; set; }
        public string KPIDDescription { get; set; }
        public Dep DepNo { get; set; }
        public bool MeasurmentUnit { get; set; }
        public int TargetValue { get; set; }
    }
}
