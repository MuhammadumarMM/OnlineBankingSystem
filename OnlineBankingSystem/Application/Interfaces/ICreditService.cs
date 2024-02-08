using Application.DTOs.CreditDtos;

namespace Application.Interfaces;

public interface ICreditService
{
    Task<List<CreditDto>> GetAllAsync();
    Task<CreditDto> GetByIdAsync(int id);
    Task<CriditResult> AddAsync(AddCredit creditDto);
    Task DeleteAsync(int id);
    Task UpdateAsync(UpdateCredit creditDto);
    Task<List<CreditDto>> GetByUserIdAsync(string userId);
}
