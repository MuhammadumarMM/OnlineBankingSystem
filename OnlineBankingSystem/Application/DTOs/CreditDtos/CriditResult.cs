

using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.CreditDtos;

public class CriditResult
{
    public string? FisrtName { get; set; }
    public string? LastName { get; set; }
    public decimal? Amount { get; set; }

    public DateTime PayDay { get; set; } 

    public decimal PayMonth { get; set; }

}
