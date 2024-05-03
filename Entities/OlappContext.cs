using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace olappApi.Entities;

public partial class OlappContext : DbContext
{
    public OlappContext()
    {
    }

    public OlappContext(DbContextOptions<OlappContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<DeductionCbu> DeductionCbus { get; set; }

    public virtual DbSet<DeductionInsurance> DeductionInsurances { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Usertype> Usertypes { get; set; }

}
