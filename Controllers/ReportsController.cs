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

        [HttpPost("GetDeductionCBUs")]
        public ActionResult GetDeductionCBUs(BetweenDateSearchModel s)
        {
            try
            {
                // Retrieve DeductionCBU records between the specified dates
                List<DeductionCbu> deductionCbus = _context.DeductionCbus
                    .Where(dc => dc.DateAdded >= s.startDate && dc.DateAdded <= s.endDate)
                    .ToList();

                return Ok(deductionCbus);
            }
            catch (System.Exception)
            {
                return NoContent();
            }
        }

        [HttpPost("GetDeductionInsurances")]
        public ActionResult GetDeductionInsurances(BetweenDateSearchModel s)
        {
            try
            {
                // Retrieve DeductionCBU records between the specified dates
                List<DeductionInsurance> deductionInsurances = _context.DeductionInsurances
                    .Where(dc => dc.DateAdded >= s.startDate && dc.DateAdded <= s.endDate)
                    .ToList();

                return Ok(deductionInsurances);
            }
            catch (System.Exception)
            {
                return NoContent();
            }
        }

        [HttpPost("GetDelinquentReport")]
        public IActionResult GetDelinquentReport(BetweenDateSearchModel s)
        {

            try
            {
                var result = (
                    from loan in _context.Loans
                    join client in _context.Clients on loan.ClientId equals client.Id
                    where loan.DueDate >= s.startDate && loan.DueDate <= s.endDate && loan.Status != "Paid"
                    group loan by new { client.Id, client.Name } into clientGroup
                    select new DelinquentReportModel
                    {
                        ClientId = clientGroup.Key.Id,
                        ClientName = clientGroup.Key.Name,
                        PastDueLoans = clientGroup.ToList()
                    }
                ).ToList();


                return Ok(result);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (log, return error response, etc.)
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }

        }



    }
}