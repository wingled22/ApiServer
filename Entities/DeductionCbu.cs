using System;
using System.Collections.Generic;

namespace olappApi.Entities;

public partial class DeductionCbu
{
    public long Id { get; set; }

    public long? LoanId { get; set; }

    public decimal? TotalCbu { get; set; }

    public DateTime? DateAdded { get; set; }

    public string? DateSadded { get; set; }
}
