using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Common;
using NuGet.Configuration;
using olappApi.Entities;
using olappApi.Model;

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

            // Loan l = new Loan();
            // l.ClientId = client.Id;
            // l.Capital = c.Capital;
            // l.Interest = c.Interest;
            // l.NoPayment = c.NoOfPayments;
            // l.DeductCbu = c.DeductCBU;
            // l.DeductInsurance = c.DeductInsurance;
            // l.OtherFee = c.DeductOther;
            // l.Type = c.LoanType;
            // l.Status = "Unpaid";

            // if (l.Interest == null || l.Interest == 0)
            //     l.InterestedAmount = 0;
            // else
            //     l.InterestedAmount = (l.Interest / 100) * l.Capital;

            // l.LoanAmount = l.Capital + l.InterestedAmount;
            // l.LoanReceivable = l.Capital - (l.DeductCbu + l.DeductInsurance + l.OtherFee);

            // l.DateTime = DateTime.Now;
            // if (l.Type == "Daily")
            //     l.DueDate = DateTime.Now.AddDays(Convert.ToInt32(l.NoPayment));

            // //Emergency
            // if (l.Type == "Weekly" || l.Type == "Emergency")
            // {
            //     l.DueDate = DateTime.Now.AddDays(Convert.ToInt32(l.NoPayment) * 7);
            // }

            // //P.O Cash
            // if (l.Type == "Bi-Monthly" || l.Type == "PO Cash")
            // {
            //     l.DueDate = DateTime.Now.AddDays(Convert.ToInt32(l.NoPayment) * 15);
            // }

            // //Others
            // if (l.Type == "Monthly" || l.Type == "Others")
            // {
            //     l.DueDate = DateTime.Now.AddMonths(Convert.ToInt32(l.NoPayment));
            // }
            // if (l.Type == "Annualy")
            // {
            //     l.DueDate = DateTime.Now.AddYears(Convert.ToInt32(l.NoPayment));
            // }

            // decimal dailyPayment = Convert.ToDecimal(l.LoanAmount / l.NoPayment);

            // _context.Loans.Add(l);
            // _context.SaveChanges();

            // //add deductCBU
            // DeductionCbu deductionCbu = new DeductionCbu
            // {
            //     LoanId = l.Id,
            //     TotalCbu = l.DeductCbu,
            //     DateAdded = DateTime.Now
            // };
            // _context.DeductionCbus.Add(deductionCbu);
            // _context.SaveChanges();

            // DeductionInsurance deductionInsurance = new DeductionInsurance
            // {
            //     LoanId = l.Id,
            //     TotalInsurance = l.DeductInsurance,
            //     DateAdded = DateTime.Now
            // };
            // _context.DeductionInsurances.Add(deductionInsurance);
            // _context.SaveChanges();

            // List<Schedule> schedules = new List<Schedule>();

            // for (int i = 1; i <= l.NoPayment; i++)
            // {
            //     Schedule temp = new Schedule();
            //     temp.LoanId = l.Id;
            //     temp.Collectables = dailyPayment;
            //     temp.Status = "Unpaid";

            //     if (l.Type == "Daily")
            //     {
            //         temp.Date = DateTime.Now.AddDays(i);
            //         schedules.Add(temp);
            //     }

            //     //Emergency
            //     if (l.Type == "Weekly" || l.Type == "Emergency")
            //     {
            //         temp.Date = DateTime.Now.AddDays(i * 7);
            //         schedules.Add(temp);
            //     }

            //     //P.O Cash
            //     if (l.Type == "Bi-Monthly" || l.Type == "PO Cash")
            //     {
            //         temp.Date = DateTime.Now.AddDays(i * 15);
            //         schedules.Add(temp);
            //     }

            //     //Others
            //     if (l.Type == "Monthly" || l.Type == "Others")
            //     {
            //         temp.Date = DateTime.Now.AddMonths(i);
            //         schedules.Add(temp);
            //     }
            //     if (l.Type == "Annualy")
            //     {
            //         temp.Date = DateTime.Now.AddYears(i);
            //         schedules.Add(temp);
            //     }

            // }

            // _context.Schedules.AddRange(schedules);
            // _context.SaveChanges();


            // return Ok();
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

            if (c.DateTime == null)
                l.DateTime = DateTime.Now;
            else
                l.DateTime = c.DateTime;

            if (l.Type == "Daily")
                l.DueDate = ((DateTime)(l.DateTime)).AddDays(Convert.ToInt32(l.NoPayment));

            //Emergency
            if (l.Type == "Weekly" || l.Type == "Emergency")
            {
                l.DueDate = ((DateTime)(l.DateTime)).AddDays(Convert.ToInt32(l.NoPayment) * 7);
            }

            //P.O Cash
            if (l.Type == "Bi-Monthly" || l.Type == "PO Cash")
            {
                l.DueDate = ((DateTime)(l.DateTime)).AddDays(Convert.ToInt32(l.NoPayment) * 15);
            }

            //Others
            if (l.Type == "Monthly" || l.Type == "Others")
            {
                l.DueDate = ((DateTime)(l.DateTime)).AddMonths(Convert.ToInt32(l.NoPayment));
            }
            if (l.Type == "Annualy")
            {
                l.DueDate = ((DateTime)(l.DateTime)).AddYears(Convert.ToInt32(l.NoPayment));
            }

            decimal dailyPayment = Convert.ToDecimal(l.LoanAmount / l.NoPayment);

            _context.Loans.Add(l);
            _context.SaveChanges();


            //add deductCBU
            DeductionCbu deductionCbu = new DeductionCbu
            {
                LoanId = l.Id,
                TotalCbu = l.DeductCbu,
                DateAdded = l.DateTime
            };
            _context.DeductionCbus.Add(deductionCbu);
            _context.SaveChanges();

            DeductionInsurance deductionInsurance = new DeductionInsurance
            {
                LoanId = l.Id,
                TotalInsurance = l.DeductInsurance,
                DateAdded = l.DateTime
            };
            _context.DeductionInsurances.Add(deductionInsurance);
            _context.SaveChanges();

            List<Schedule> schedules = new List<Schedule>();

            for (int i = 1; i <= l.NoPayment; i++)
            {
                Schedule temp = new Schedule();
                temp.LoanId = l.Id;
                temp.Collectables = dailyPayment;
                temp.Status = "Unpaid";

                if (l.Type == "Daily")
                {
                    temp.Date = ((DateTime)(l.DateTime)).AddDays(i);
                    schedules.Add(temp);
                }

                //Emergency
                if (l.Type == "Weekly" || l.Type == "Emergency")
                {
                    temp.Date = ((DateTime)(l.DateTime)).AddDays(i * 7);
                    schedules.Add(temp);
                }

                //P.O Cash
                if (l.Type == "Bi-Monthly" || l.Type == "PO Cash")
                {
                    temp.Date = ((DateTime)(l.DateTime)).AddDays(i * 15);
                    schedules.Add(temp);
                }

                //Others
                if (l.Type == "Monthly" || l.Type == "Others")
                {
                    temp.Date = ((DateTime)(l.DateTime)).AddMonths(i);
                    schedules.Add(temp);
                }
                if (l.Type == "Annualy")
                {
                    temp.Date = ((DateTime)(l.DateTime)).AddYears(i);
                    schedules.Add(temp);
                }

            }

            _context.Schedules.AddRange(schedules);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("PostClientLoan")]
        public async Task<IActionResult> PostClientLoan(LoanCreation c)
        {
            Loan l = new Loan();
            l.ClientId = c.ClientId;
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

            if (c.DateTime == null)
                l.DateTime = DateTime.Now;
            else
                l.DateTime = c.DateTime;

            if (l.Type == "Daily")
                l.DueDate = ((DateTime)(l.DateTime)).AddDays(Convert.ToInt32(l.NoPayment));

            //Emergency
            if (l.Type == "Weekly" || l.Type == "Emergency")
            {
                l.DueDate = ((DateTime)(l.DateTime)).AddDays(Convert.ToInt32(l.NoPayment) * 7);
            }

            //P.O Cash
            if (l.Type == "Bi-Monthly" || l.Type == "PO Cash")
            {
                l.DueDate = ((DateTime)(l.DateTime)).AddDays(Convert.ToInt32(l.NoPayment) * 15);
            }

            //Others
            if (l.Type == "Monthly" || l.Type == "Others")
            {
                l.DueDate = ((DateTime)(l.DateTime)).AddMonths(Convert.ToInt32(l.NoPayment));
            }
            if (l.Type == "Annualy")
            {
                l.DueDate = ((DateTime)(l.DateTime)).AddYears(Convert.ToInt32(l.NoPayment));
            }

            decimal dailyPayment = Convert.ToDecimal(l.LoanAmount / l.NoPayment);

            _context.Loans.Add(l);
            await _context.SaveChangesAsync();


            //add deductCBU
            DeductionCbu deductionCbu = new DeductionCbu
            {
                LoanId = l.Id,
                TotalCbu = l.DeductCbu,
                DateAdded = l.DateTime
            };
            _context.DeductionCbus.Add(deductionCbu);
            _context.SaveChanges();

            DeductionInsurance deductionInsurance = new DeductionInsurance
            {
                LoanId = l.Id,
                TotalInsurance = l.DeductInsurance,
                DateAdded = l.DateTime
            };
            _context.DeductionInsurances.Add(deductionInsurance);
            _context.SaveChanges();

            List<Schedule> schedules = new List<Schedule>();

            for (int i = 1; i <= l.NoPayment; i++)
            {
                Schedule temp = new Schedule();
                temp.LoanId = l.Id;
                temp.Collectables = dailyPayment;
                temp.Status = "Unpaid";

                if (l.Type == "Daily")
                {
                    temp.Date = ((DateTime)(l.DateTime)).AddDays(i);
                    schedules.Add(temp);
                }

                //Emergency
                if (l.Type == "Weekly" || l.Type == "Emergency")
                {
                    temp.Date = ((DateTime)(l.DateTime)).AddDays(i * 7);
                    schedules.Add(temp);
                }

                //P.O Cash
                if (l.Type == "Bi-Monthly" || l.Type == "PO Cash")
                {
                    temp.Date = ((DateTime)(l.DateTime)).AddDays(i * 15);
                    schedules.Add(temp);
                }

                //Others
                if (l.Type == "Monthly" || l.Type == "Others")
                {
                    temp.Date = ((DateTime)(l.DateTime)).AddMonths(i);
                    schedules.Add(temp);
                }
                if (l.Type == "Annualy")
                {
                    temp.Date = ((DateTime)(l.DateTime)).AddYears(i);
                    schedules.Add(temp);
                }

            }

            _context.Schedules.AddRange(schedules);
            await _context.SaveChangesAsync();

            return Ok(l);
        }

        [HttpGet("GetClientById")]
        public async Task<ActionResult<Client>> GetClientById(long id)
        {
            if (id != null || id != 0)
            {
                return await _context.Clients.Where(x => x.Id == id).FirstOrDefaultAsync();
            }
            return NotFound();
        }

        [HttpGet("GetClientLoans")]
        public async Task<ActionResult<IEnumerable<ClientLoanInfo>>> GetClientLoans(long id)
        {
            if (id != null || id != 0)
            {
                List<ClientLoanInfo> res = await (
                    from l in _context.Loans
                    where l.ClientId == id
                    select new ClientLoanInfo
                    {
                        Id = l.Id,
                        ClientId = l.ClientId ?? 0, // Assuming ClientId is nullable, replace 0 with the default value you prefer
                        Type = l.Type,
                        DeductCbu = l.DeductCbu ?? 0,
                        DeductInsurance = l.DeductInsurance ?? 0,
                        OtherFee = l.OtherFee ?? 0,
                        LoanAmount = l.LoanAmount ?? 0,
                        Capital = l.Capital ?? 0,
                        Interest = l.Interest ?? 0,
                        InterestedAmount = l.InterestedAmount ?? 0,
                        LoanReceivable = l.LoanReceivable ?? 0,
                        NoPayment = l.NoPayment ?? 0,
                        Status = l.Status,
                        DateTime = l.DateTime.ToString(),
                        DueDate = l.DueDate ?? DateTime.MinValue, // Assuming DueDate is nullable, replace DateTime.MinValue with the default value you prefer
                        TotalPenalty = (long)l.TotalPenalty,
                        AddedInterest = (long)l.AddedInterest,
                        Collected = _context.Transactions
                                    .Where(x => x.LoanId == l.Id)
                                    .Sum(s => (decimal?)s.Amount) ?? 0,
                    }
                ).ToListAsync();

                // return await _context.Loans.Where(x => x.ClientId == id).ToListAsync();
                return res;
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

        }

        private bool ClientExists(long id)
        {
            return (_context.Clients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
