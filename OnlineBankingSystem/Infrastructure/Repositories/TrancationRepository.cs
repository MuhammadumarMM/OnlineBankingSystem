using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TrancationRepository(ApplicationDbContext dbContext) : ITrancationInterface
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<string> InTransaction( string userCardNumber, string outCardNumber, decimal balance)
        {


        if (((userCardNumber.StartsWith("8600") || userCardNumber.StartsWith("9860") ||
            userCardNumber.StartsWith("4546") || userCardNumber.StartsWith("6062"))
            && userCardNumber.Length == 16)

            && ((outCardNumber.StartsWith("8600") || outCardNumber.StartsWith("9860") ||
                outCardNumber.StartsWith("4546") || outCardNumber.StartsWith("6062"))
            && outCardNumber.Length == 16))
        {
            var userCard = await _dbContext.cards.FirstOrDefaultAsync(c => c.CardNumber == userCardNumber);
            var outCardDb = await _dbContext.cards.FirstOrDefaultAsync(c => c.CardNumber == outCardNumber);
            var bank = await _dbContext.bankcards.ToListAsync();
            var bankCard = bank.First();

            if (userCard == null)
            {
                throw new Exception("User card not found");
            }

            if (outCardDb == null)
            {
                throw new Exception("Out card not found");
            }
      

            if (userCard.Balance >= balance)
            {
                if(bankCard == null)
                {
                    return "Invalid card information";

                }

                userCard.Balance -= balance;
                bankCard.Balance += balance/100;

                outCardDb.Balance += balance - balance/100;
                await  _dbContext.SaveChangesAsync();
                

                await _dbContext.SaveChangesAsync(); 

                return "Transaction successful";
            }
            else
            {
                return "Insufficient balance";
            }
        }
        else
        {
            return "Invalid card information";
        }
    }


    public Task<string> OutTrancation( string userCardNumber, string outCardNumber, decimal balance)
    {
        throw new NotImplementedException();
    }
}
