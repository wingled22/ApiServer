using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace olappApi.Model
{
    public class ClientLoanInfo
    {
        public long Id { get; set; }

        public long? ClientId { get; set; }

        public string? Type { get; set; }

        public decimal? DeductCbu { get; set; }

        public decimal? DeductInsurance { get; set; }

        public decimal? LoanAmount { get; set; }

        public decimal? Capital { get; set; }

        public decimal ?Interest { get; set; }

        public decimal? InterestedAmount { get; set; }

        public decimal? LoanReceivable { get; set; }

        public int? NoPayment { get; set; }

        public string? Status { get; set; }

        public string? DateTime { get; set; }

        public DateTime DueDate { get; set; }

        public decimal? TotalPenalty { get; set; }

        public decimal? AddedInterest { get; set; } 

        public decimal? OtherFee { get; set; }

        public decimal? Collected { get; set; }
        public decimal? Collectables { get; set; }
    }
}