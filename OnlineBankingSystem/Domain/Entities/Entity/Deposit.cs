using Domain.Entities.Enams;
using Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Entity;

public class Deposit : BaseEntity
{
    [Required, StringLength(50)]
    public string? FisrtName { get; set; }
    [Required, StringLength(50)]
    public string? LastName  { get; set; }
    [Required]
    public decimal JSHR { get; set; }
    [StringLength(50)]
    public string? PasswordNumber { get; set; }
    public string? DepositCardNumber { get; set; }
    public DateTime? Birthday { get; set; }
    [Required]
    public decimal? Amount { get; set;}
    
    [StringLength(50)]
    public string? PhoneNumber { get; set; }
    public DepositMonth DepositMonth { get; set; }

    public DateTime? DepositDay { get; set; } = DateTime.Now;
    public decimal? MonthPay { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; }
}
