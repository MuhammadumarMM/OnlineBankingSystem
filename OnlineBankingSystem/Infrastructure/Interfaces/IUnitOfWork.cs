namespace Infrastructure.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IDepositInterface DepositInterface { get; }
    ICardInterface CardInterface { get; }
    ICreditInterface CreditInterface { get; }
    IBankCardInterface BankCardInterface { get; }

    Task SaveChangeAsync();
}
