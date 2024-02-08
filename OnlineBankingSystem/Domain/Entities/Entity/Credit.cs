using Domain.Entities.Enams;
using Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Entity;

public class Credit :  BaseEntity
{
    [Required, StringLength(50)]
    public string? FisrtName { get; set; }
    [Required, StringLength(50)]
    public string? LastName { get; set; }
    [Required]
    public decimal JSHR { get; set; }
    [StringLength(50)]
    public string? PasswordNumber { get; set; }
    public DateTime Birthday { get; set; }
    [Required]
    public decimal? Amount { get; set; }
    [StringLength(50)]
    public string? Address { get; set; }
    [StringLength(50)]
    public string? PhoneNumber { get; set; }
    public DepositMonth DepositMonth { get; set; }
    public decimal? CardNumber { get; set; }
    public DateTime? CreditDay { get; set; }
    public decimal? MonthPay {  get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; }
}
