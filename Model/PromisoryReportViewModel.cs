using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using olappApi.Entities;

namespace olappApi.Model
{
    public class PromisoryReportViewModel
    {
        public Client ClientInfo { get; set; }
        public Loan LoanInfo { get; set; }
        public List<Schedule> ListOfSchedules { get; set; }
    }
}