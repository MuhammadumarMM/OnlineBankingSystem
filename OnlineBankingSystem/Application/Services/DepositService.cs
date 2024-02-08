using Application.Common.Exceptions;
using Application.Common.Validators;
using Application.DTOs.CardDtos;
using Application.DTOs.CreditDtos;
using Application.DTOs.DepositDtos;
using Application.DTOs.DepositDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Entity;
using Domain.Entities.Identity;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class DepositService(IUnitOfWork unitOfWork,
                           IMapper mapper,
                           UserManager<ApplicationUser> userManager) : IDepositService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<DepositResult> AddAsync(AddDeposit depositDto)
    {
        // Validate input
        if (depositDto == null)
        {
            throw new ArgumentNullException(nameof(depositDto), "Deposit data is null");
        }

        // Map DTO to domain entity
        var deposit = _mapper.Map<Deposit>(depositDto);

        // Validate mapped entity
        if (deposit == null)
        {
            throw new CustomException("An issue occurred during deposit mapping");
        }

        // Check if deposit is valid
        if (!deposit.IsValid())
        {
            throw new CustomException("Invalid deposit data");
        }
        if (deposit.JSHR.ToString().Length != 14)
        {
            throw new CustomException("JSHR hato");
        }
        if (!(deposit.PasswordNumber.Length == 9 &&
              char.IsLetter(deposit.PasswordNumber[0]) &&
              char.IsLetter(deposit.PasswordNumber[1]) &&
              deposit.PasswordNumber.Substring(2).All(char.IsDigit)))
        {
            throw new CustomException("Password Number hato");
        }

        // Get all deposits
        var deposits = await _unitOfWork.DepositInterface.GetAllAsync();

    

        // Get bank card
        var bankcards = await _unitOfWork.BankCardInterface.GetAllAsync();
        var bankcard = bankcards.FirstOrDefault();

        // Check if bank card exists
        if (bankcard == null)
        {
            throw new CustomException("Bank card not found");
        }

        // Update bank card balance


        bankcard.Balance += (decimal)deposit.Amount!;

        var cards = await _unitOfWork.CardInterface.GetAllAsync();
        var card = cards.FirstOrDefault(c => c.CardNumber == deposit.DepositCardNumber);

        // Deduct deposit amount
        if(card == null)
        {
            throw new CustomException("card not found");

        }
        if(card.Balance < (decimal)depositDto.Amount)
        {
            throw new CustomException("Cardda mablag' yetarli emas ");

        }
        card.Balance -= (decimal)depositDto.Amount;

        await _unitOfWork.CardInterface.UpdateAsync(card);
        // Update deposit based on DepositMonth
        switch (deposit.DepositMonth)
        {
            case Domain.Entities.Enams.DepositMonth.oy12:
                deposit.MonthPay = (deposit.Amount * 40 / 100) / 12;
                break;
            case Domain.Entities.Enams.DepositMonth.oy18:
                deposit.MonthPay = (deposit.Amount * 30 / 100) / 18;
                break;
            case Domain.Entities.Enams.DepositMonth.oy24:
                deposit.MonthPay = (deposit.Amount * 20 / 100) / 24;
                break;
            // Add more cases if needed
            default:
                break;
        }

        // Save changes
        await _unitOfWork.SaveChangeAsync();

        // Update bank card and create deposit
        await _unitOfWork.BankCardInterface.UpdateAsync(bankcard);
        await _unitOfWork.DepositInterface.CreateAsync(deposit);
        await _unitOfWork.SaveChangeAsync();

        // Return deposit result
        return new DepositResult
        {
            FisrtName = deposit.FisrtName,
            LastName = deposit.LastName,
            Amount = deposit.Amount,
            PayDay = DateTime.Now,
            PayMonth = (decimal)deposit.MonthPay!
        };
    }

    public async Task DeleteAsync(int id)
    {
        if(id < 0)
        {
            throw new CustomException("id manfiy bo'lmaydi");
        }
        var credit = await _unitOfWork.DepositInterface.GetByIdAsync(id);
        if (credit == null)
        {
            throw new NotFoundException("Bunday kredit yo'q");
        }
        _unitOfWork.DepositInterface.DeleteAsync(credit);
        await _unitOfWork.SaveChangeAsync();
    }

    public async Task<List<DepositDto>> GetAllAsync()
    {
        var resalt =  await _unitOfWork.DepositInterface.GetAllAsync();
        if (resalt == null)
        {
            throw new NotFoundException("BOSH");
        }
        return resalt.Select(c => _mapper.Map<DepositDto>(c)).ToList();
    }

    public async Task<DepositDto> GetByIdAsync(int id)
    {
        if (id < 0)
        {
            throw new CustomException("id manfiy bolaolmaydi");
        }
        var credit = await _unitOfWork.DepositInterface.GetByIdAsync(id);
        if (credit == null)
        {
            throw new NotFoundException("Bunday id topilmadi");
        }
        return _mapper.Map<DepositDto>(credit);
    }

    public async Task<List<DepositDto>> GetByUserIdAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new CustomException("User Id is Empity");
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }
        var credits = await _unitOfWork.DepositInterface.GetAllAsync();
        var usercredits = credits.Where(c => c.UserId == userId).ToList();
        if (usercredits.Count() == 0)
        {
            throw new CustomException("Userda deposit mavjud emas");
        }

        return usercredits.Select(u => _mapper.Map<DepositDto>(u)).ToList();
    }

    public async Task<DepositResult> UpdateAsync(UpdateDeposit depositDto)
    {
        // Validate input
        if (depositDto == null)
        {
            throw new ArgumentNullException(nameof(depositDto), "Deposit data is null");
        }

        // Map DTO to domain entity
        var deposit = _mapper.Map<Deposit>(depositDto);

        // Validate mapped entity
        if (deposit == null)
        {
            throw new CustomException("An issue occurred during deposit mapping");
        }

        // Check if deposit is valid
        if (!deposit.IsValid())
        {
            throw new CustomException("Invalid deposit data");
        }
        if (deposit.JSHR.ToString().Length == 14)
        {
            throw new CustomException("JSHR hato");
        }
        if (!(deposit.PasswordNumber.Length == 9 &&
              char.IsLetter(deposit.PasswordNumber[0]) &&
              char.IsLetter(deposit.PasswordNumber[1]) &&
              deposit.PasswordNumber.Substring(2).All(char.IsDigit)))
        {
            throw new CustomException("Password Number hato");
        }

        // Get all deposits
        var deposits = await _unitOfWork.DepositInterface.GetAllAsync();

        // Check if deposits exist
        if (deposits == null || !deposits.Any())
        {
            throw new NotFoundException("No deposits found");
        }

        // Check if deposit already exists


        // Get bank card
        var bankcards = await _unitOfWork.BankCardInterface.GetAllAsync();
        var bankcard = bankcards.FirstOrDefault();

        // Check if bank card exists
        if (bankcard == null)
        {
            throw new CustomException("Bank card not found");
        }

        // Update bank card balance


        bankcard.Balance += (decimal)deposit.Amount!;

        var cards = await _unitOfWork.CardInterface.GetAllAsync();
        var card = cards.FirstOrDefault(c => c.CardNumber == deposit.DepositCardNumber);

        // Deduct deposit amount
        if (card == null)
        {
            throw new CustomException("card not found");

        }
        if (card.Balance < (decimal)depositDto.Amount)
        {
            throw new CustomException("Cardda mablag' yetarli emas ");

        }
        card.Balance -= (decimal)depositDto.Amount;

        await _unitOfWork.CardInterface.UpdateAsync(card);
        // Update deposit based on DepositMonth
        switch (deposit.DepositMonth)
        {
            case Domain.Entities.Enams.DepositMonth.oy12:
                deposit.MonthPay = (deposit.Amount * 40 / 100) / 12;
                break;
            case Domain.Entities.Enams.DepositMonth.oy18:
                deposit.MonthPay = (deposit.Amount * 30 / 100) / 18;
                break;
            case Domain.Entities.Enams.DepositMonth.oy24:
                deposit.MonthPay = (deposit.Amount * 20 / 100) / 24;
                break;
            // Add more cases if needed
            default:
                break;
        }

        // Save changes
        await _unitOfWork.SaveChangeAsync();

        // Update bank card and create deposit
        await _unitOfWork.BankCardInterface.UpdateAsync(bankcard);
        await _unitOfWork.DepositInterface.UpdateAsync(deposit);
        await _unitOfWork.SaveChangeAsync();

        // Return deposit result
        return new DepositResult
        {
            FisrtName = deposit.FisrtName,
            LastName = deposit.LastName,
            Amount = deposit.Amount,
            PayDay = DateTime.Now,
            PayMonth = (decimal)deposit.MonthPay!
        };
    }

}
