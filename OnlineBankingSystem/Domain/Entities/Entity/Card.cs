using Domain.Entities.Enams;
using Domain.Entities.Identity;

namespace Domain.Entities.Entity;

public class Card : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? BankName { get; set; }
    public string? CardNumber { get; set;}
    public decimal? Balance { get; set; } = 1000000;
    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    public CardType? CardType { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; }

}
