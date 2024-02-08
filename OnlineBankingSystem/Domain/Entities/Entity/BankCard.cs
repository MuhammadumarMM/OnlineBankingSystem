
namespace Domain.Entities.Entity;

public class BankCard : BaseEntity
{
    public decimal Balance { get; set; } = 0;
    public string BankShot { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
}
