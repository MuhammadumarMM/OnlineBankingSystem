using Domain.Entities.Entity;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class CardRepository : Repository<Card>, ICardInterface
{
    public CardRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
