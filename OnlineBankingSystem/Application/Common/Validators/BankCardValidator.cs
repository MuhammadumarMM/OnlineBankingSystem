using Domain.Entities.Entity;

namespace Application.Common.Validators;

public static class BankCardValidator
{
    public static bool IsValid(this BankCard bankCard)
        => bankCard != null &&
           !string.IsNullOrEmpty(bankCard.BankName) &&
           !string.IsNullOrEmpty(bankCard.BankShot) &&
            bankCard.Balance >= 0;

    public static bool IsExsit(this BankCard bankCard, IEnumerable<BankCard> bankCards)
            => bankCards.Any(c => c.BankName == bankCard.BankName &&
                                  c.BankShot == bankCard.BankShot &&
                                  c.Id != bankCard.Id);
}
