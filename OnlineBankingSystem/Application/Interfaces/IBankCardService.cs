using Application.DTOs.BankCardDtos;

namespace Application.Interfaces;

public interface IBankCardService
{
    Task<List<BankCardDto>> GetAllAsync();
    Task<BankCardDto> GetByIdAsync(int id);
    Task AddAsync(AddBankCardDto bankCardDto);
    Task DeleteAsync(int id);
    Task UpdateAsync(UpdateBankCardDto bankCardDto);
}
