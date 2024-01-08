using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using olappApi.Entities;

namespace olappApi.Model
{
    public class PastDueLoans
    {
        public long Id { get; set; }

        public long? ClientId { get; set; }

        public string? Type { get; set; }

        public decimal? DeductCbu { get; set; }

        public decimal? DeductInsurance { get; set; }

        public decimal? LoanAmount { get; set; }

        public decimal? Capital { get; set; }

        public decimal? Interest { get; set; }

        public decimal? InterestedAmount { get; set; }

        public decimal? LoanReceivable { get; set; }

        public int? NoPayment { get; set; }

        public string? Status { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? TotalPenalty { get; set; }

        public decimal? AddedInterest { get; set; }

        public decimal? OtherFee { get; set; }

        public DateTime? DateTime { get; set; }

        public Client Client { get; set; }
        public decimal? Collected { get; internal set; }
    }

    public class PastDueLoanPayable{
        public long LoanId { get; set; }
        public decimal Payable { get; set; }
        public decimal SubTotalPayment { get; set; }
    }
}