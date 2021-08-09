using AutoMapper;
using Base5.Application.Interfaces;
using Base5.Application.KPI.Models;
using Base5.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Base5.Application.KPI.Queries.GetById
{
    public class GetKPIByDepartmentId : IRequest<List<KPIDetailsDto>>
    {
        public Dep Dep { get; set; }

        public class Handler : IRequestHandler<GetKPIByDepartmentId, List<KPIDetailsDto>>
        {
            private readonly IAppDbContext _context;
            private readonly ILogger<Handler> _logger;

            public Handler(IAppDbContext context,ILogger<Handler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<List<KPIDetailsDto>> Handle(GetKPIByDepartmentId request, CancellationToken cancellationToken)
            {
                try
                {
                    var kpis = await _context.KPIs.Where(a => a.DepNo == request.Dep).Select(a =>
                        new KPIDetailsDto
                        {
                            KPIDNum = a.KPIDNum,
                            KPIDDescription = a.KPIDDescription,
                            MeasurmentUnit = a.MeasurmentUnit,
                            TargetValue = a.TargetValue
                        }).ToListAsync(cancellationToken);

                    return kpis;
                }
                catch (Exception e)
                {
                   _logger.LogError($"Error in GetKPIByDepartmentId  : {e}");
                    throw;
                }
            }
        }
    }
}