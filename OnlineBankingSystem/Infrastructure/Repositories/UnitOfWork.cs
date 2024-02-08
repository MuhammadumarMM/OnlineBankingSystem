using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext dbContext,
                        ICreditInterface creditInterface,
                        ICardInterface cardInterface,
                        IDepositInterface depositInterface,
                        IBankCardInterface bankCardInterface ) : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext  = dbContext;

    public IDepositInterface DepositInterface { get; } = depositInterface;

    public ICardInterface CardInterface {  get; } = cardInterface;

    public ICreditInterface CreditInterface {  get; } = creditInterface;

    public IBankCardInterface BankCardInterface {  get; } = bankCardInterface;

    public void Dispose()
        => GC.SuppressFinalize(this);

    public  async Task SaveChangeAsync()
        => await _dbContext.SaveChangesAsync();
}
