using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using olappApi.Entities;

namespace olappApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {

        private readonly OlappContext _context;
        private readonly ILogger<ClientController> _logger;


        public TransactionController(OlappContext context, ILogger<ClientController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetTransactions")]
        public IActionResult GetTransactions(long loanId)
        {

            List<Transaction> transactions = _context.Transactions
                                            .Where(x => x.LoanId == loanId).ToList();

            return Ok(transactions);
        }

        [HttpGet("GetTransaction")]
        public IActionResult GetTransaction(long transId)
        {

            Transaction trans = _context.Transactions
                                            .Where(x => x.TransId == transId).FirstOrDefault();

            return Ok(trans);
        }

        [HttpGet("{clientId}")]
    public async Task<IActionResult> GetLoanDetails(long clientId)
    {
        var res = (from loan in _context.Loans
                    join transaction in _context.Transactions on loan.Id equals transaction.LoanId
                    where loan.ClientId == clientId 
                    select new
                    {
                        loan.Id,
                        loan.Type,
                        loan.DueDate,
                        Transactions = _context.Transactions.Where(x=> x.LoanId == loan.Id).ToList()
                    }).ToList();

        
        if (res == null)
        {
            return NotFound();
        }

        return Ok(res);
    }
    }
}