using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using olappApi.Entities;
using olappApi.Model;

namespace olappApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly OlappContext _context;

        public LoanController(OlappContext context)
        {
            _context = context;
        }

        // GET: api/Loan
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans()
        {
            if (_context.Loans == null)
            {
                return NotFound();
            }


            return await _context.Loans.ToListAsync();
        }

        // GET: api/Loan/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(long id)
        {
            if (_context.Loans == null)
            {
                return NotFound();
            }



            var loan = await _context.Loans.FindAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            return loan;
        }

        [HttpGet("GetPastDueLoans")]
        public ActionResult GetPastDueLoans()
        {
            try
            {
                DateTime now = DateTime.Now;
                List<PastDueLoans> pastDueLoans = (
                    from loan in _context.Loans
                    where loan.DueDate.HasValue && loan.DueDate < now && loan.Status != "Paid" && loan.TotalPenalty == 0.00m
                    select new PastDueLoans
                    {
                        Id = loan.Id,
                        ClientId = loan.ClientId,
                        Type = loan.Type,
                        DeductCbu = loan.DeductCbu,
                        DeductInsurance = loan.DeductInsurance,
                        LoanAmount = loan.LoanAmount,
                        Capital = loan.Capital,
                        Interest = loan.Interest,
                        InterestedAmount = loan.InterestedAmount,
                        LoanReceivable = loan.LoanReceivable,
                        NoPayment = loan.NoPayment,
                        Status = loan.Status,
                        DueDate = loan.DueDate,
                        TotalPenalty = loan.TotalPenalty,
                        AddedInterest = loan.AddedInterest,
                        OtherFee = loan.OtherFee,
                        DateTime = loan.DateTime,
                        Collected = _context.Transactions
                                    .Where(x => x.LoanId == loan.Id)
                                    .Sum(s => (decimal?)s.Amount) ?? 0,
                        Client = _context.Clients.Where(x => x.Id == loan.ClientId).FirstOrDefault()
                    }
                ).ToList();

                return Ok(pastDueLoans);
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        [HttpGet("GetPenalizedLoan")]
        public ActionResult GetPenalizedLoan()
        {
            try
            {
                DateTime now = DateTime.Now;
                List<PastDueLoans> pastDueLoans = (
                    from loan in _context.Loans
                    where loan.DueDate.HasValue && loan.DueDate < now && loan.Status != "Paid" && loan.TotalPenalty > 0.00m
                    select new PastDueLoans
                    {
                        Id = loan.Id,
                        ClientId = loan.ClientId,
                        Type = loan.Type,
                        DeductCbu = loan.DeductCbu,
                        DeductInsurance = loan.DeductInsurance,
                        LoanAmount = loan.LoanAmount,
                        Capital = loan.Capital,
                        Interest = loan.Interest,
                        InterestedAmount = loan.InterestedAmount,
                        LoanReceivable = loan.LoanReceivable,
                        NoPayment = loan.NoPayment,
                        Status = loan.Status,
                        DueDate = loan.DueDate,
                        TotalPenalty = loan.TotalPenalty,
                        AddedInterest = loan.AddedInterest,
                        OtherFee = loan.OtherFee,
                        DateTime = loan.DateTime,
                        Collected = _context.Transactions
                                    .Where(x => x.LoanId == loan.Id)
                                    .Sum(s => (decimal?)s.Amount) ?? 0,
                        Client = _context.Clients.Where(x => x.Id == loan.ClientId).FirstOrDefault()
                    }
                ).ToList();

                return Ok(pastDueLoans);
            }
            catch (Exception)
            {
                return NoContent();
            }
        }

        [HttpPost("AddPenalty")]
        public IActionResult AddPenalty(AddPenaltyModel penalty)
        {
            try
            {
                Loan loan = _context.Loans.Where(x => x.Id == penalty.LoanId).FirstOrDefault();

                if (loan == null)
                    return BadRequest();

                loan.TotalPenalty = penalty.Amount;

                _context.Loans.Update(loan);
                _context.SaveChanges();
                return Ok();
            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("GetPenalizedLoanInfo")]
        public IActionResult GetPenalizedLoanInfo(long loanID)
        {
            try
            {
                Loan l = _context.Loans.Where(q => q.Id == loanID).FirstOrDefault();

                if (l == null)
                    return BadRequest();

                var pastDueLoan = (
                    from loan in _context.Loans
                    where loan.Id == loanID
                    select new PastDueLoanPayable
                    {
                        LoanId = loan.Id,
                        SubTotalPayment = _context.Transactions
                                    .Where(x => x.LoanId == loan.Id)
                                    .Sum(s => (decimal?)s.Amount) ?? 0,
                    }
                ).FirstOrDefault();

                pastDueLoan.Payable = (decimal)(l.TotalPenalty + l.LoanAmount) - pastDueLoan.SubTotalPayment;

                return Ok(pastDueLoan);
            }
            catch (System.Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet("GetTodaySchedule")]
        public async Task<IActionResult> GetTodaySchedule()
        {
            try
            {
                // Get today's date
                DateTime today = DateTime.Today;

                // // Retrieve schedules for today including related client information using a join
                // var result = await (from schedule in _context.Schedules
                //                     join loan in _context.Loans on schedule.LoanId equals loan.Id
                //                     join client in _context.Clients on loan.ClientId equals client.Id
                //                     where schedule.Date != null && schedule.Date.Value.Date == today
                //                     select new
                //                     {
                //                         ScheduleId = schedule.Id,
                //                         Collectables = schedule.Collectables,
                //                         ScheduleDate = schedule.Date,
                //                         Status = schedule.Status,
                //                         ClientName = client.Name,
                //                         City = client.City,
                //                         Barangay = client.Barangay,
                //                         Province = client.Province,
                //                     }).ToListAsync();

                var result = await (from schedule in _context.Schedules
                                    join loan in _context.Loans on schedule.LoanId equals loan.Id
                                    join client in _context.Clients on loan.ClientId equals client.Id
                                    where schedule.Date != null && schedule.Date.Value.Date == today
                                    group new { schedule, loan, client } by new
                                    {
                                        client.Province,
                                        client.City,
                                        client.Barangay
                                    } into groupedData
                                    select new
                                    {
                                        Province = groupedData.Key.Province,
                                        City = groupedData.Key.City,
                                        Barangay = groupedData.Key.Barangay,
                                        Schedules = groupedData.Select(g => new
                                        {
                                            ScheduleId = g.schedule.Id,
                                            Collectables = g.schedule.Collectables,
                                            ScheduleDate = g.schedule.Date,
                                            Status = g.schedule.Status,
                                            ClientName = g.client.Name
                                        })
                                    }).ToListAsync();


                if (result == null || !result.Any())
                {
                    return NotFound("No schedules found for today.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetSchedulesByDate")]
        public async Task<IActionResult> GetSchedulesByDate([FromQuery] DateTime date)
        {
            try
            {
                // Retrieve schedules for the specified date including related client information using a join
                var result = await (from schedule in _context.Schedules
                                    join loan in _context.Loans on schedule.LoanId equals loan.Id
                                    join client in _context.Clients on loan.ClientId equals client.Id
                                    where schedule.Date != null && schedule.Date.Value.Date == date.Date
                                    group new { schedule, loan, client } by new
                                    {
                                        client.Province,
                                        client.City,
                                        client.Barangay
                                    } into groupedData
                                    select new
                                    {
                                        Province = groupedData.Key.Province,
                                        City = groupedData.Key.City,
                                        Barangay = groupedData.Key.Barangay,
                                        Schedules = groupedData.Select(g => new
                                        {
                                            ScheduleId = g.schedule.Id,
                                            Collectables = g.schedule.Collectables,
                                            ScheduleDate = g.schedule.Date,
                                            Status = g.schedule.Status,
                                            ClientName = g.client.Name,
                                            AdditionalAddressInfo = g.client.AdditionalAddressInfo
                                        })
                                    }).ToListAsync();

                if (result == null || !result.Any())
                {
                    return NotFound($"No schedules found for {date.ToShortDateString()}.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        private bool LoanExists(long id)
        {
            return (_context.Loans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
