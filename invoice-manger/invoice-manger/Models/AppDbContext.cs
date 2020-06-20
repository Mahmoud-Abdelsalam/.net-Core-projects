using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invoice_manger.Model;
using Microsoft.EntityFrameworkCore;

namespace invoice_manger.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Invoice> Invoices { get; set; }
    }
}
