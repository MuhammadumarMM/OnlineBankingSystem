using Application.Common.Exceptions;
using Application.Common.Validators;
using Application.DTOs.CreditDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Entity;
using Domain.Entities.Identity;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class CreditService(IUnitOfWork unitOfWork,
                           IMapper mapper,
                           UserManager<ApplicationUser> userManager) : ICreditService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<CriditResult> AddAsync(AddCredit creditDto)
    {
        if (creditDto == null)
        {
            throw new ArgumentNullException("credit is null");
        }
        var credit = _mapper.Map<Credit>(creditDto);
        if (credit == null)
        {
            throw new CustomException("creditga o'tishda muammo bo'ldi");
        }
        if (!credit.IsValid())
        {
            throw new CustomException("credit is not valid");
        }
        var credits = await _unitOfWork.CreditInterface.GetAllAsync();


        if (credits == null)
        {
            throw new NotFoundException("NotFound");
        }
        if (credit.IsExsit(credits))
        {
            throw new CustomException("credit is already exist");
        }
        var cards = await _unitOfWork.CardInterface.GetAllAsync();
        var card = cards.FirstOrDefault(c => c.CardNumber == creditDto.CardNumber.ToString());

        card.Balance += credit.Amount;
       
        switch (credit.DepositMonth)
        {
            case Domain.Entities.Enams.DepositMonth.oy12:
                credit.MonthPay = (credit.Amount * 140 / 100)/12;
                await _unitOfWork.SaveChangeAsync();
                break;

            case Domain.Entities.Enams.DepositMonth.oy18:
                credit.MonthPay = (credit.Amount * 130 / 100) / 18;
                await _unitOfWork.SaveChangeAsync();

                break;
            case Domain.Entities.Enams.DepositMonth.oy24:
                credit.MonthPay = (credit.Amount * 120 / 100) / 24;
                await _unitOfWork.SaveChangeAsync();

                break;
            default:
                break;

        }

        _unitOfWork.CardInterface.UpdateAsync(card);


        await _unitOfWork.CreditInterface.CreateAsync(credit);
        await _unitOfWork.SaveChangeAsync();

        return new CriditResult
        {
            FisrtName = credit.FisrtName,
            LastName = credit.LastName,
            Amount = credit.Amount,
            PayDay = (DateTime)credit.CreditDay,
            PayMonth = (decimal)credit.MonthPay
        };
    }

    public async Task DeleteAsync(int id)
    {
        if(id < 0)
        {
            throw new CustomException("id manfiy bo'lmaydi");
        }
        var credit = await _unitOfWork.CreditInterface.GetByIdAsync(id);
        if (credit == null)
        {
            throw new NotFoundException("Bunday kredit yo'q");
        }
        _unitOfWork.CreditInterface.DeleteAsync(credit);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<List<CreditDto>> GetAllAsync()
    {
        var resalt =  await _unitOfWork.CreditInterface.GetAllAsync();
        if (resalt == null)
        {
            throw new NotFoundException("BOSH");
        }
        return resalt.Select(c => _mapper.Map<CreditDto>(c)).ToList();
    }

    public async Task<CreditDto> GetByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new CustomException("id manfiy bolaolmaydi");
        }
        var credit = await _unitOfWork.CreditInterface.GetByIdAsync(id);
        if (credit == null)
        {
            throw new NotFoundException("Bunday id topilmadi");
        }
        return _mapper.Map<CreditDto>(credit);
    }

    public async Task<List<CreditDto>> GetByUserIdAsync(string userId)
    {
       if(string.IsNullOrEmpty(userId))
        {
            throw new CustomException("User Id is Empity");
        }
       var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        var credits = await _unitOfWork.CreditInterface.GetAllAsync();
        var usercredits = credits.Where(c => c.UserId == userId).ToList();
        if(usercredits.Count() == 0)
        {
            throw new CustomException("Userda credit mavjud emas");
        }
        
        return usercredits.Select(u => _mapper.Map<CreditDto>(u)).ToList(); 
    }

    public async Task UpdateAsync(UpdateCredit creditDto)
    {
        if (creditDto == null)
        {
            throw new ArgumentNullException("credit is null");
        }
        var credit = _mapper.Map<Credit>(creditDto);
        if (credit == null)
        {
            throw new CustomException("creditga o'tishda muammo bo'ldi");
        }
        if (!credit.IsValid())
        {
            throw new CustomException("credit is not valid");
        }
        var credits = await _unitOfWork.CreditInterface.GetAllAsync();
 

        if (credits == null)
        {
            throw new NotFoundException("NotFound");
        }
        if (credit.IsExsit(credits))
        {
            throw new CustomException("credit is already exist");
        }
        var cards = await _unitOfWork.CardInterface.GetAllAsync();
        var card = cards.FirstOrDefault(c => c.CardNumber == creditDto.CardNumber.ToString());
        card.Balance += credit.Amount;

        await _unitOfWork.CreditInterface.UpdateAsync(credit);
        await _unitOfWork.SaveChangeAsync();

    }
}
