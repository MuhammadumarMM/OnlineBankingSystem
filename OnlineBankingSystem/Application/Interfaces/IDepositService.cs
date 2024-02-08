using Application.DTOs.CreditDtos;
using Application.DTOs.DepositDtos;

namespace Application.Interfaces;

public interface  IDepositService
{
    Task<List<DepositDto>> GetAllAsync();
    Task<DepositDto> GetByIdAsync(int id);
    Task<DepositResult> AddAsync(AddDeposit depositDto);
    Task DeleteAsync(int id);
    Task<DepositResult> UpdateAsync(UpdateDeposit depositDto);
    Task<List<DepositDto>> GetByUserIdAsync(string userId);
}
