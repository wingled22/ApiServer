using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using olappApi.Entities;

namespace olappApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly OlappContext _context;

        public ScheduleController(OlappContext context)
        {
            _context = context;
        }

        [HttpGet("GetSchedulesByLoanId")]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedulesByLoanId(long id)
        {

            if (id != null || id != 0)
            {

                var res = await (
                    from s in _context.Schedules
                    where (long)s.LoanId == id
                    select new Schedule
                    {
                        Id = s.Id,
                        LoanId = s.LoanId,
                        Date = s.Date,
                        Status = s.Status,
                        Collectables = s.Collectables - (_context.Transactions.Where(x => x.ScheduleId == s.Id).Sum(s => s.Amount))
                    }

                ).ToListAsync();

                return res;

                // return await _context.Schedules.Where(x => x.LoanId == id).ToListAsync();

            }

            return NotFound();
        }

        [HttpGet("GetScheduleById")]
        public async Task<ActionResult<Schedule>> GetScheduleById(long id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Invalid id");
                }

                Schedule sched = await (
                    from s in _context.Schedules
                    where s.Id == id
                    select new Schedule
                    {
                        Id = s.Id,
                        LoanId = s.LoanId,
                        Date = s.Date,
                        Status = s.Status,
                        Collectables = s.Collectables - _context.Transactions
                            .Where(x => x.ScheduleId == s.Id)
                            .Sum(s => s.Amount)
                    }
                ).FirstOrDefaultAsync();

                return sched != null ? Ok(sched) : NotFound();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(500, "Internal Server Error");
            }
        }


    }
}