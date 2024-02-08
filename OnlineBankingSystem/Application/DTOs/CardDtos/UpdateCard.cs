using Domain.Entities.Enams;

namespace Application.DTOs.CardDtos;

public class UpdateCard : BaseDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BankName { get; set; }
    public string? CardNumber { get; set; }
    public decimal? Balance { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime? CreatedDate { get; set; } = null;
    public CardType? CardType { get; set; }
}
