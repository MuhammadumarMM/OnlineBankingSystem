using Application.DTOs.CardDtos;

namespace Application.Interfaces;

public interface ICardService
{
    Task<List<CardDto>> GetAllAsync(); 
    Task<CardDto> GetByIdAsync(int id);

    Task<List<CardDto>> GetByUserId(string userId);
    Task AddAsync(AddCard cardDto);
    Task DeleteAsync(int id);
}
