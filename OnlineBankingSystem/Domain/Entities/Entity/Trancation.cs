using Domain.Entities.Enams;

namespace Domain.Entities.Entity;

public class Trancation : BaseEntity
{
    public string? CardName { get; set; }
    public decimal? Balanse { get; set; } = 0;
    public string? CardNumber { get; set; }
    public CardType? CardType { get; set; }


}
