using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invoice_manger.Model;
using invoice_manger.Model.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Clauses;

namespace invoice_manger.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IInvoiceRepository invoiceRepository;
        public InvoiceController(IInvoiceRepository invoiceRepository)
        {
            this.invoiceRepository = invoiceRepository;
           
        }
        public ActionResult Index()
        {
            var model = invoiceRepository.GetAllInvoices();
            return View(model);
        }

        [HttpGet]
        public ActionResult Create( int id)
        {
            Invoice model = new Invoice
            {
                Id = invoiceRepository.GetAllInvoices().Count() + 1
        };
           
            model.RecordList = new List< TableRecord>();
            model.RecordList.Add(new TableRecord());
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(Invoice model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                
                  invoiceRepository.Add(model);

                  return RedirectToAction(nameof(Index));
                }
                catch 
                {
                    return View();
                }
            }
            ModelState.AddModelError("", "You have to fill all the required fields!");
            return View(model);
        }
    }
}