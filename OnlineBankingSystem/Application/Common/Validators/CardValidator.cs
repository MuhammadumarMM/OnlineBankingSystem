using Domain.Entities.Entity;

namespace Application.Common.Validators;

public static class CardValidator
{
    public static bool IsValid(this Card card)
        => card != null && 
          !string.IsNullOrEmpty(card.FirstName) &&
          !string.IsNullOrEmpty(card.LastName) &&
          !string.IsNullOrEmpty(card.BankName) 
         ;

    public static bool IsExsit(this Card card, IEnumerable<Card> cards)
        => cards.Any(c =>  c.BankName == card.BankName &&
                           c.CardType == card.CardType &&
                           c.Id != card.Id);                        
}
