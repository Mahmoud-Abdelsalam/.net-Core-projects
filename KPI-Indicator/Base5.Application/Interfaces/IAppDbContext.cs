using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Base5.Application.Interfaces
{
    public interface IAppDbContext 
    {
        DbSet<Domain.Entities.KPI> KPIs { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
