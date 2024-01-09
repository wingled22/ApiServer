using System;
using System.Collections.Generic;
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
    }
}