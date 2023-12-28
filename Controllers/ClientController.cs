using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Configuration;
using olappApi.Entities;

namespace olappApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly OlappContext _context;
        private readonly ILogger<ClientController> _logger;


        public ClientController(OlappContext context, ILogger<ClientController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Client
        [HttpGet("GetClients")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            if (_context.Clients == null)
            {
                return NotFound();
            }
            return await _context.Clients.ToListAsync();
        }

        [HttpPost("PostClient")]
        public IActionResult PostClient(ClientAndLoanCreation c)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //create client
            Client client = new Client()
            {
                Name = c.FullName,
                Email = c.Email,
                Birthdate = c.BirthDate,
                Province = c.Province,
                City = c.City,
                Barangay = c.Barangay,
                AdditionalAddressInfo = c.AdditionalAddressInfo,
                ContactNumber = c.ContactNumber,
                Gender = c.Gender
            };
            _context.Clients.Add(client);
            _context.SaveChanges();

            //TODO: create loan and calculation
            Loan l = new Loan();
            l.ClientId = client.Id;
            l.Capital = c.Capital;
            l.Interest = c.Interest;
            l.NoPayment = c.NoOfPayments;
            l.DeductCbu = c.DeductCBU;
            l.DeductInsurance = c.DeductInsurance;
            l.OtherFee = c.DeductOther;
            l.Type = c.LoanType;
            l.Status = "Unpaid";

            if (l.Interest == null || l.Interest == 0)
                l.InterestedAmount = 0;
            else
                l.InterestedAmount = (l.Interest / 100) * l.Capital;

            l.LoanAmount = l.Capital + l.InterestedAmount;
            l.LoanReceivable = l.Capital - (l.DeductCbu + l.DeductInsurance + l.OtherFee);

            l.DateTime = DateTime.Now.ToString();
            if (l.Type == "Daily")
                l.DueDate = DateTime.Now.AddDays(Convert.ToInt32(l.NoPayment));

            //Emergency
            if (l.Type == "Weekly" || l.Type == "Emergency")
            {
                l.DueDate = DateTime.Now.AddDays(Convert.ToInt32(l.NoPayment) * 7);
            }

            //P.O Cash
            if (l.Type == "Bi-Monthly" || l.Type == "P.O Cash")
            {
                l.DueDate = DateTime.Now.AddDays(Convert.ToInt32(l.NoPayment) * 15);
            }

            //Others
            if (l.Type == "Monthly" || l.Type == "Others")
            {
                l.DueDate = DateTime.Now.AddMonths(Convert.ToInt32(l.NoPayment));
            }
            if (l.Type == "Annualy")
            {
                l.DueDate = DateTime.Now.AddYears(Convert.ToInt32(l.NoPayment));
            }

            decimal dailyPayment = Convert.ToDecimal(l.LoanAmount / l.NoPayment);

            _context.Loans.Add(l);
            _context.SaveChanges();

            //TODO: create schedules if loan is created
            List<Schedule> schedules = new List<Schedule>();

            for (int i = 1; i <= l.NoPayment; i++)
            {
                Schedule temp = new Schedule();
                temp.LoanId = l.Id;
                temp.Collectables = dailyPayment;
                temp.Status = "Unpaid";

                if (l.Type == "Daily")
                {
                    temp.Date = DateTime.Now.AddDays(i);
                    schedules.Add(temp);
                }

                //Emergency
                if (l.Type == "Weekly" || l.Type == "Emergency")
                {
                    temp.Date = DateTime.Now.AddDays(i * 7);
                    schedules.Add(temp);
                }

                //P.O Cash
                if (l.Type == "Bi-Monthly" || l.Type == "P.O Cash")
                {
                    temp.Date = DateTime.Now.AddDays(i * 15);
                    schedules.Add(temp);
                }

                //Others
                if (l.Type == "Monthly" || l.Type == "Others")
                {
                    temp.Date = DateTime.Now.AddMonths(i);
                    schedules.Add(temp);
                }
                if (l.Type == "Annualy")
                {
                    temp.Date = DateTime.Now.AddYears(i);
                    schedules.Add(temp);
                }

            }

            _context.Schedules.AddRange(schedules);
            _context.SaveChanges();


            return Ok();
        }

        [HttpGet("GetClientLoans")]
        public async Task<ActionResult<IEnumerable<Loan>>> GetClientLoans(long id)
        {

            if (id != null || id != 0)
            {

                return await _context.Loans.Where(x => x.ClientId == id).ToListAsync();

            }

            return NotFound();
        }

        [HttpGet("SearchClientByName")]
        public async Task<ActionResult<IEnumerable<Client>>> SearchClientByName(string searchString)
        {

            _logger.LogError(searchString);
            try
            {
                var clients = await _context.Clients
                    .Where(c => EF.Functions.Like(c.Name, $"%{searchString}%"))
                    .ToListAsync();

                if (clients.Any())
                {
                    return Ok(clients);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

            return NotFound();
        }


        // // GET: api/Client/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<Client>> GetClient(long id)
        // {
        //   if (_context.Clients == null)
        //   {
        //       return NotFound();
        //   }
        //     var client = await _context.Clients.FindAsync(id);

        //     if (client == null)
        //     {
        //         return NotFound();
        //     }

        //     return client;
        // }

        // // PUT: api/Client/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutClient(long id, Client client)
        // {
        //     if (id != client.Id)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(client).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ClientExists(id))
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

        // // POST: api/Client
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPost]
        // public async Task<ActionResult<Client>> PostClient(Client client)
        // {
        //   if (_context.Clients == null)
        //   {
        //       return Problem("Entity set 'OlappContext.Clients'  is null.");
        //   }
        //     _context.Clients.Add(client);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction("GetClient", new { id = client.Id }, client);
        // }

        // // DELETE: api/Client/5
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteClient(long id)
        // {
        //     if (_context.Clients == null)
        //     {
        //         return NotFound();
        //     }
        //     var client = await _context.Clients.FindAsync(id);
        //     if (client == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Clients.Remove(client);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool ClientExists(long id)
        {
            return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
