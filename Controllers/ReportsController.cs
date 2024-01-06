using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<PromisoryReportViewModel> PromisoryReport(long loanId){
            
            PromisoryReportViewModel r = new PromisoryReportViewModel();


            return Ok(r);
        
        } 
    }
}