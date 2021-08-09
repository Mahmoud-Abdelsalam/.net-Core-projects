using Base5.Application.Interfaces;
using Base5.Application.KPI.Models;
using Base5.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Base5.Application.KPI.Commands.Create
{
    public class CreateKPICommand : IRequest<Unit>
    {
        public List<KPIDetailsDto> KpiDetailsDtos { get; set; }
        public Dep DepNo { get; set; }
        public bool MeasurementUnit { get; set; }

        public class Handler : IRequestHandler<CreateKPICommand, Unit>
        {
            private readonly IAppDbContext _context;
            private readonly ILogger<Handler> _logger;

            public Handler(IAppDbContext context, ILogger<Handler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<Unit> Handle(CreateKPICommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var kpids = _context.KPIs.Where(a => a.DepNo == request.DepNo).Select(a => a.KPIDNum).ToList();

                    foreach (var k in request.KpiDetailsDtos)
                    {
                        var kpi = await _context.KPIs.FirstOrDefaultAsync(a => a.KPIDNum == k.KPIDNum, cancellationToken);
                        if (kpi is not null)
                        {
                            kpi.DepNo = request.DepNo;
                            kpi.KPIDDescription = k.KPIDDescription;
                            kpi.TargetValue = k.TargetValue;
                            kpi.MeasurmentUnit = request.MeasurementUnit;

                            kpids.Remove(kpi.KPIDNum);
                        }
                        else
                        {

                            var newKpi = new Domain.Entities.KPI
                            {
                                KPIDNum = k.KPIDNum,
                                DepNo = request.DepNo,
                                KPIDDescription = k.KPIDDescription,
                                MeasurmentUnit = request.MeasurementUnit,
                                TargetValue = k.TargetValue
                            };
                            await _context.KPIs.AddAsync(newKpi, cancellationToken);
                            kpids.Remove(k.KPIDNum);
                        }
                    }

                    foreach (var id in kpids)
                    {
                        var kpi = await _context.KPIs.FirstOrDefaultAsync(a => a.KPIDNum == id, cancellationToken);
                        _context.KPIs.Remove(kpi);
                    }

                    await _context.SaveChangesAsync(cancellationToken);

                    return Unit.Value;
                }
                catch (Exception e)
                {
                    _logger.LogError($"Error in CreateKPICommand : {e}");
                    return Unit.Value;
                }
            }
        }
    }
}