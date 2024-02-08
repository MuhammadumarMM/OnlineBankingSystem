using Application.DTOs.CardDtos;

namespace Application.Interfaces
{
    public  interface ITrancationService
    {
        Task<string> InTransaction(TrancationDto userCardNumber, OutTrancationDto outCardNumber, decimal balance);

    }
}
