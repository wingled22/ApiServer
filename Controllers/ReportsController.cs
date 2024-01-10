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
                DateTime now = DateTime.Now;
                var result = (
                    from loan in _context.Loans
                    join client in _context.Clients on loan.ClientId equals client.Id
                    where loan.DueDate.HasValue && loan.DueDate < now && loan.Status != "Paid" && loan.TotalPenalty == 0.00m
                     && loan.DueDate >= s.startDate && loan.DueDate <= s.endDate
                    group new { client.Id, client.Name, loan } by new { client.Id, client.Name } into clientGroup
                    select new
                    {
                        ClientId = clientGroup.Key.Id,
                        ClientName = clientGroup.Key.Name,
                        TotalPayments = clientGroup.Sum(item => _context.Transactions
                            .Where(t => t.LoanId == item.loan.Id)
                            .Sum(t => t.Amount) ?? 0),
                        PastDueLoans = clientGroup.Select(item => new Loan
                        {
                            Id = item.loan.Id,
                            ClientId = item.loan.ClientId,
                            Type = item.loan.Type,
                            DeductCbu = item.loan.DeductCbu,
                            DeductInsurance = item.loan.DeductInsurance,
                            LoanAmount = (item.loan.LoanAmount ?? 0) - (_context.Transactions
                                .Where(t => t.LoanId == item.loan.Id)
                                .Sum(t => t.Amount) ?? 0),
                            Capital = item.loan.Capital,
                            Interest = item.loan.Interest,
                            InterestedAmount = item.loan.InterestedAmount,
                            LoanReceivable = item.loan.LoanReceivable,
                            NoPayment = item.loan.NoPayment,
                            Status = item.loan.Status,
                            TotalPenalty = item.loan.TotalPenalty,
                            OtherFee = item.loan.OtherFee,
                            DateTime = item.loan.DateTime,
                            DueDate = item.loan.DueDate,
                        }).ToList()
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