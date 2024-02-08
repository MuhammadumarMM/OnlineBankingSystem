using Domain.Entities.Entity;

namespace Application.Common.Validators;

public static class CreditValidator
{
    public static bool IsValid(this Credit credit)
        => credit != null &&
            !string.IsNullOrEmpty(credit.PhoneNumber) &&
            !string.IsNullOrEmpty(credit.LastName) &&
            !string.IsNullOrEmpty(credit.FisrtName) &&
            !string.IsNullOrEmpty(credit.Address) &&
            !string.IsNullOrEmpty(credit.PasswordNumber) &&
            credit.CardNumber > 0 &&
            credit.Amount >= 0 &&
            credit.JSHR > 0
         ;

    public static bool IsExsit(this Credit credit, IEnumerable<Credit> credits)
        => credits.Any(c =>  c.PhoneNumber == credit.PhoneNumber &&
                             c.PasswordNumber == credit.PasswordNumber &&
                             c.JSHR == credit.JSHR);                        
}
