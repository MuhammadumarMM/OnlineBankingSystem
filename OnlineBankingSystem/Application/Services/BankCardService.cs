using Application.Common.Exceptions;
using Application.Common.Validators;
using Application.DTOs.BankCardDtos;
using Application.DTOs.CardDtos;
using Application.DTOs.CreditDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Entity;
using Infrastructure.Interfaces;

namespace Application.Services;

public class BankCardService(IUnitOfWork unitOfWork,
                             IMapper mapper) : IBankCardService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task AddAsync(AddBankCardDto bankCardDto)
    {
        if(bankCardDto == null)
        {
            throw new ArgumentNullException("BankCard is null");
        }
        var bankCard = _mapper.Map<BankCard>(bankCardDto);
        if(bankCard == null)
        {
            throw new CustomException("BankCardga o'tishda muammo bo'ldi");
        }
        if (!bankCard.IsValid())
        {
            throw new CustomException("bankCard is not valid");
        }
        var bankCards = await _unitOfWork.BankCardInterface.GetAllAsync();
        if(bankCards == null)
        {
            throw new NotFoundException("NotFound");
        }
        if(bankCard.IsExsit(bankCards))
        {
            throw new CustomException("IsExsit is already exist");
        }

        await _unitOfWork.BankCardInterface.CreateAsync(bankCard);
        await _unitOfWork.SaveChangeAsync();

    }

    public async Task DeleteAsync(int id)
    {
        if (id < 0)
        {
            throw new CustomException("id manfiy bo'lmaydi");
        }
        var bankCards = await _unitOfWork.BankCardInterface.GetByIdAsync(id);
        if (bankCards == null)
        {
            throw new NotFoundException("Bunday kredit yo'q");
        }
        _unitOfWork.BankCardInterface.DeleteAsync(bankCards);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<List<BankCardDto>> GetAllAsync()
    {
        var bankcards = await _unitOfWork.BankCardInterface.GetAllAsync();
        if (bankcards == null)
        {
            throw new NotFoundException("BOSH");
        }
        return bankcards.Select(c => _mapper.Map<BankCardDto>(c)).ToList();
    }

    public async Task<BankCardDto> GetByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new CustomException("id manfiy bolaolmaydi");
        }
        var bankcards = await _unitOfWork.BankCardInterface.GetByIdAsync(id);
        if (bankcards == null)
        {
            throw new NotFoundException("Bunday id topilmadi");
        }
        return _mapper.Map<BankCardDto>(bankcards);
    }

    public async Task UpdateAsync(UpdateBankCardDto bankCardDto)
    {
        if (bankCardDto == null)
        {
            throw new ArgumentNullException("BankCard is null");
        }
        var bankCard = _mapper.Map<BankCard>(bankCardDto);
        if (bankCard == null)
        {
            throw new CustomException("BankCardga o'tishda muammo bo'ldi");
        }
        if (!bankCard.IsValid())
        {
            throw new CustomException("bankCard is not valid");
        }
        var bankCards = await _unitOfWork.BankCardInterface.GetAllAsync();
        if (bankCards == null)
        {
            throw new NotFoundException("NotFound");
        }
        if (bankCard.IsExsit(bankCards))
        {
            throw new CustomException("IsExsit is already exist");
        }

        await _unitOfWork.BankCardInterface.UpdateAsync(bankCard);
        await _unitOfWork.SaveChangeAsync();
    }
}
