using Domain.Entities.Enams;

namespace Application.DTOs.CardDtos;

public class AddCard 
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BankName { get; set; }
    public string UserId { get; set; } = string.Empty;
    public CardType? CardType { get; set; }
}
