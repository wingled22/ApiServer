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



        // // PUT: api/Loan/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutLoan(long id, Loan loan)
        // {
        //     if (id != loan.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(loan).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!LoanExists(id))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // }

        // // POST: api/Loan
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<Loan>> PostLoan(Loan loan)
        // {
        //   if (_context.Loans == null)
        //   {
        //       return Problem("Entity set 'OlappContext.Loans'  is null.");
        //   }
        //     _context.Loans.Add(loan);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetLoan", new { id = loan.Id }, loan);
        // }

        // DELETE: api/Loan/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteLoan(long id)
        // {
        //     if (_context.Loans == null)
        //     {
        //         return NotFound();
        //     }
        //     var loan = await _context.Loans.FindAsync(id);
        //     if (loan == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Loans.Remove(loan);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool LoanExists(long id)
        {
            return (_context.Loans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
