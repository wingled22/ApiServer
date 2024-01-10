using System;
using System.Collections.Generic;

namespace olappApi.Entities;

public partial class AppUser
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public long ClientId { get; set; }

    public int Usertype { get; set; }
}
