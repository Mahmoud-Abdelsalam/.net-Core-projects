using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base5.Domain.Enums;

namespace Base5.Domain.Entities
{
    public class KPI
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KPIDNum { get; set; }
        public string KPIDDescription { get; set; }
        public Dep DepNo { get; set; }
        public bool MeasurmentUnit { get; set; }
        public int TargetValue { get; set; }


    }
}
