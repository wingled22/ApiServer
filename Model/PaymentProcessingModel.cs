using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace olappApi.Model
{
    public class PaymentProcessingModel
    {
        public long SchedId { get; set; }
        public decimal Amount { get; set; }
    }

    public class PenaltyPaymentProcessingModel
    {
        public long LoanId { get; set; }
        public decimal Amount { get; set; }
    }
}