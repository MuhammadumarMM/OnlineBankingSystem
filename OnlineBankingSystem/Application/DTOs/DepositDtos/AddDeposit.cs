using Domain.Entities.Enams;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.DepositDtos;

public class AddDeposit
{
    [Required, StringLength(50)]
    public string? FisrtName { get; set; }
    [Required, StringLength(50)]
    public string? DepositCardNumber { get; set; }
    public string? LastName { get; set; }
    [Required]
    public decimal JSHR { get; set; }
    [StringLength(50)]
    public string? PasswordNumber { get; set; }
    public DateTime? Birthday { get; set; }
    [Required]
    public decimal? Amount { get; set; }

    [StringLength(50)]
    public string? PhoneNumber { get; set; }
    public DepositMonth DepositMonth { get; set; }

    public string UserId { get; set; } = string.Empty;
}
