using System;
using System.Collections.Generic;

namespace olappApi.Entities;

public partial class Client
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string Gender { get; set; } = null!;

    public string? Email { get; set; }

    public string? Bday { get; set; }

    public string Province { get; set; } = null!;

    public string? Municipal { get; set; }

    public string? Barangay { get; set; }

    public int? Purok { get; set; }

    public string? Address { get; set; }

    public string? AdditionalAddressInfo { get; set; }

    public string? City { get; set; }

    public string? EmailAddress { get; set; }

    public string? ContactNumber { get; set; }
}
