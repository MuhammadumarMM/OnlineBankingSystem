using Domain.Entities.Entity;

namespace Infrastructure.Interfaces;

public  interface ITrancationInterface
{
    Task<string> OutTrancation(string userCardNumber, string outCardNumber, decimal balance);
    Task<string> InTransaction(string userCardNumber, string outCardNumber, decimal balance);
}
