using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invoice_manger.Model
{
    public class TableRecord
    {
        public  int Id { get; set; }
        public Item Item { get; set; }
        public Unit Unit { get; set; }
        public int Price { get; set; }
        public int Qty { get; set; }
        public int Total { get; set; }
        public int Discount { get; set; }
        public int Net { get; set; }
       
    }
}
