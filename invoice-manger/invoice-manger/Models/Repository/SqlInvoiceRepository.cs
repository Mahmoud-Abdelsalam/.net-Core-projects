using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invoice_manger.Model;
using invoice_manger.Model.Repository;

namespace invoice_manger.Models
{
  
    public class SqlInvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public SqlInvoiceRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Invoice> GetAllInvoices()
        {
            return _context.Invoices;
        }
      

        public Invoice Add(Invoice invoice)
        {
            _context.Add(invoice);
            _context.SaveChanges();
            return invoice;
        }

       

       
    }
}
