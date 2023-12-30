using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

                return await _context.Schedules.Where(x => x.LoanId == id).ToListAsync();

            }

            return NotFound();
        }

        [HttpGet("GetScheduleById")]
        public async Task<ActionResult<Schedule>> GetScheduleById(long id)
        {

            if (id != null || id != 0)
            {

                return await _context.Schedules
                    .Where(x => x.Id == id)
                    .Where(x => x.Status.ToLower() == "unpaid")
                    .FirstOrDefaultAsync();

            }

            return NotFound();
        }

    }
}