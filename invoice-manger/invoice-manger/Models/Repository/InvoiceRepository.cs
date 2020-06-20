using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invoice_manger.Model.Repository
{
  
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly List<Invoice> _invoiceList;
        private readonly List<TableRecord> _tableRecordList;
        public InvoiceRepository()
        {
            _invoiceList = new List<Invoice>()
            {
                new Invoice { Id = 1, Store = Store.Store1, TotalInvoice = 1500 , Date = DateTime.UtcNow}
            };
            _tableRecordList = new List<TableRecord>()
            {
                new TableRecord { Id = 1, Item = Item.Item1, Unit = Unit.A , Price = 200 , Qty = 3 , Total = 700, Discount = 40 , Net = 660}
            };
        }

        public IEnumerable<Invoice> GetAllInvoices()
        {
            return _invoiceList;
        }

      

        public Invoice Add(Invoice invoice)
        {
            invoice.Id = _invoiceList.Max(i => i.Id) + 1;
            _invoiceList.Add(invoice);
            return invoice;
        }

       
    
    }
}
