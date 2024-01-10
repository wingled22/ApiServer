using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using olappApi.Entities;

namespace olappApi.Model
{
    public class DelinquentReportModel
    {
        public long ClientId { get; set; }
        public string ClientName { get; set; }
         public decimal TotalPayments { get; set; }
        public List<Loan> PastDueLoans { get; set; }
    }
}