using Base5.Application.Interfaces;
using Base5.Application.KPI.Commands.Create;
using Base5.Application.KPI.Models;
using Base5.Application.KPI.Queries.GetById;
using Base5.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Base5.Web.Controllers
{
    public class KPIController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IAppDbContext _context;

        public KPIController(IMediator mediator, IAppDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpGet]
        public IActionResult Manage()
        {
            return View();
        }

        public async Task<IActionResult> Manage(KPIDto model)
        {
            var command = new CreateKPICommand
            {
                KpiDetailsDtos = model.KPIDetails,
                DepNo = model.DepNo,
                MeasurementUnit = model.MeasurmentUnit
            };
            await _mediator.Send(command);

            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<ActionResult<List<KPIDetailsDto>>> GetDepartmentKPIs(Dep department)
        {
            var request = new GetKPIByDepartmentId
            {
                Dep = department
            };

            var kpis = await _mediator.Send(request);

            return kpis;
        }

        [Route("IsKpiInUse")]
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsKpiInUse(int id)
        {
            var kpid = await _context.KPIs.FirstOrDefaultAsync(a => a.KPIDNum == id);

            if (kpid == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Kpid {id} is already in use");
            }
        }
    }
}