using System.ComponentModel.DataAnnotations;

public class ClientAndLoanCreation
{
    public long ClientId { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public string Gender { get; set; }
    [Required]
    public string BirthDate { get; set; }
    [Required]
    public string Province { get; set; }
    [Required]
    public string City { get; set; }
    [Required]
    public string Barangay { get; set; }
    [Required]
    public string AdditionalAddressInfo { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string ContactNumber { get; set; }
    [Required]
    public string LoanType { get; set; }
    [Required]
    public decimal Capital { get; set; }
    [Required]
    public decimal Interest { get; set; }
    [Required]
    public int NoOfPayments { get; set; }
    [Required]
    public decimal DeductCBU { get; set; }
    [Required]
    public decimal DeductInsurance { get; set; }
    [Required]
    public decimal DeductOther { get; set; }

    public DateTime? DateTime { get; set; }
}

public class LoanCreation
{
    [Required]

    public long ClientId { get; set; }
    
    [Required]
    public string LoanType { get; set; }
    [Required]
    public decimal Capital { get; set; }
    [Required]
    public decimal Interest { get; set; }
    [Required]
    public int NoOfPayments { get; set; }
    [Required]
    public decimal DeductCBU { get; set; }
    [Required]
    public decimal DeductInsurance { get; set; }
    [Required]
    public decimal DeductOther { get; set; }

    public DateTime DateTime { get; set; }
}
