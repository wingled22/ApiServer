using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace olappApi.Model
{
    public class AddPenaltyModel
    {
        public int LoanId { get; set; }
        public decimal? Amount { get; set; }
    }
}