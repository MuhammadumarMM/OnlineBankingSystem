using Application.Common.Exceptions;
using Application.Common.Validators;
using Application.DTOs.CardDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Entity;
using Domain.Entities.Identity;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class CardService(IUnitOfWork unitOfWork,
                         IMapper mapper, 
                         UserManager<ApplicationUser> userManager) : ICardService
{ 
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task AddAsync(AddCard cardDto)
    {
        if(cardDto == null)
        {
            throw new ArgumentNullException("Card is null");
        }
        var card = _mapper.Map<Card>(cardDto);
        if(card == null)
        {
            throw new CustomException("Cardga o'tishda muammo bo'ldi");
        }
        long min = 100_000_000_000;
        long max = 999_999_999_999;
        Random random = new Random();

        if((int)card.CardType > 5 || (int)card.CardType <= 0)
        {
            throw new CustomException("Bunday carta turi  mavjud emas");
        }
        switch (card.CardType)
        {
            case Domain.Entities.Enams.CardType.Humo:
                card.CardNumber = "9860" + random.NextInt64(min, max + 1).ToString();
                break;

            case Domain.Entities.Enams.CardType.Uzcard:
                card.CardNumber = "8600" + random.NextInt64(min, (max + 1)).ToString();
                break;

            case Domain.Entities.Enams.CardType.MasterCard:
                card.CardNumber = "4546" + random.NextInt64(min, max + 1).ToString();
                break;

            case Domain.Entities.Enams.CardType.Viza:
                card.CardNumber = "6062" + random.NextInt64(min, max + 1).ToString();
                break;

            default:
                break;
        }
        if (!card.IsValid())
        {
            throw new CustomException("Card is not valid");
        }
        var cards = await _unitOfWork.CardInterface.GetAllAsync();

        if(cards == null)
        {
            throw new NotFoundException("NotFound");
        }
        if(card.IsExsit(cards))
        {
            throw new CustomException("Card is already exist");
        }

        await _unitOfWork.CardInterface.CreateAsync(card);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id < 0)
        {
            throw new CustomException($"Invalid id {id}");
        }
        var card = await _unitOfWork.CardInterface.GetByIdAsync(id);
        if (card == null)
        {
            throw new NotFoundException("Bunday card mavjud emas");
        }
        _unitOfWork.CardInterface.DeleteAsync(card);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<List<CardDto>> GetAllAsync()
    {
        var cards = await _unitOfWork.CardInterface.GetAllAsync();

        var cardDtos = cards.Select(c => _mapper.Map<CardDto>(c)).ToList();

        foreach (var item in cardDtos)
        {
            item.CartTypeName = item.CardType.ToString();

            // Add 5 years to ExpirationDate
            if (item.ExpirationDate.HasValue)
            {
                item.ExpirationDate = item.ExpirationDate.Value.AddYears(5);

                // Update month to the original month (optional)
                item.ExpirationDate = new DateTime(item.ExpirationDate.Value.Year, item.ExpirationDate.Value.Month, item.ExpirationDate.Value.Day);
            }
        }

        return cardDtos;
    }



    public async Task<CardDto> GetByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new CustomException($"Invalid id {id}");
        }
        var card = await _unitOfWork.CardInterface.GetByIdAsync(id);
        var cardDto =  _mapper.Map<CardDto>(card);
        cardDto.CartTypeName = card.CardType.ToString();
        return cardDto;
    }

    public async Task<List<CardDto>> GetByUserId(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentNullException("userId");
        }
        var cards = await _unitOfWork.CardInterface.GetAllAsync();
        var UserCards = cards.Where(x => x.UserId == userId).ToList();

        var userCards = UserCards.Select(u => _mapper.Map<CardDto>(u)).ToList();
        foreach (var item in userCards)
        {
            item.CartTypeName = item.CardType.ToString();

        }

        return userCards;
    }
}
