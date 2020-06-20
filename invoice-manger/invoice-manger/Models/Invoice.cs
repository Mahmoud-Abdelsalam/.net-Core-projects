using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invoice_manger.Model
{
    public class Invoice
    {
        public int  Id { get; set; }
        public Store Store { get; set; }
        public  DateTime Date { get; set; }
        public int TotalInvoice { get; set; }
        public int TotalTaxes { get; set; }
        public int TotalNet { get; set; }
        public List<TableRecord> RecordList { get; set; }

    }
}
