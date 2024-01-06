using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using olappApi.Entities;
using olappApi.Model;

namespace olappApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ILogger<ReportsController> _logger;
        private readonly OlappContext _context;
        public ReportsController(ILogger<ReportsController> logger, OlappContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("PromisoryReport")]
        public async Task<ActionResult<PromisoryReportViewModel>> PromisoryReport(long loanId)
        {

            if (loanId == null || loanId <= 0)
                return NoContent();

            Loan l = await _context.Loans.Where(x => x.Id == loanId).FirstOrDefaultAsync();

            if (l == null)
                return NoContent();

            Client c = await _context.Clients.Where(x => x.Id == l.ClientId).FirstOrDefaultAsync();
            List<Schedule> schedules = await _context.Schedules.Where(x => x.LoanId == l.Id).ToListAsync();


            PromisoryReportViewModel r = new PromisoryReportViewModel();
            r.LoanInfo = l;
            r.ClientInfo = c;
            r.ListOfSchedules = schedules;


            return Ok(r);

        }

        public ActionResult GetDeductionCBU()
        {
            try
            {
                // List<

                return Ok();


            }
            catch (System.Exception)
            {

                return NoContent();
            }
        }
    }
}