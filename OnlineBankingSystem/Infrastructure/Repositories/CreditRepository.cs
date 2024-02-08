using Domain.Entities.Entity;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class CreditRepository : Repository<Credit>, ICreditInterface
{
    public CreditRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
