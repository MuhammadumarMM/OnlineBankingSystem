using Domain.Entities.Entity;

namespace Application.Common.Validators;

public static class DepositValidator
{
    public static bool IsValid(this Deposit deposit)
    {
        return deposit != null &&
            !string.IsNullOrEmpty(deposit.PhoneNumber) &&
            !string.IsNullOrEmpty(deposit.LastName) &&
            !string.IsNullOrEmpty(deposit.FisrtName) &&  
            !string.IsNullOrEmpty(deposit.PasswordNumber) &&
            deposit.Amount >= 0 &&
            deposit.JSHR > 0 
      ;
    }

    public static bool IsExsit(this Deposit deposit, IEnumerable<Deposit> deposits)
        => deposits.Any(c =>  c.PhoneNumber == deposit.PhoneNumber &&
                             c.PasswordNumber == deposit.PasswordNumber &&
                             c.JSHR == deposit.JSHR);                        
}
