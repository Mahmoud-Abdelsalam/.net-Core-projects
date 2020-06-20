using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace invoice_manger.Model.Repository
{
    public interface IInvoiceRepository
    {
        Invoice Add(Invoice invoice);
        IEnumerable<Invoice> GetAllInvoices();

    }
    
}
