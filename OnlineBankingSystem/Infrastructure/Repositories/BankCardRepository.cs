
using Domain.Entities.Entity;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class BankCardRepository : Repository<BankCard> , IBankCardInterface
{
    public BankCardRepository(ApplicationDbContext context) : base(context)
    {

    }
}