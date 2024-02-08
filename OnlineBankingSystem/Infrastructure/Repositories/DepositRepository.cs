using Domain.Entities.Entity;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class DepositRepository : Repository<Deposit>, IDepositInterface
{
    public DepositRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
