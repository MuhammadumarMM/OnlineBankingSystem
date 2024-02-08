using Application.Common.Exceptions;
using Application.DTOs.CardDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities.Entity;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class TrancationService(ITrancationInterface trancationInterface, IMapper mapper, IUnitOfWork unitOfWork) : ITrancationService
{
    private readonly ITrancationInterface _trancationInterface = trancationInterface;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<string> InTransaction(TrancationDto userCardNumber, OutTrancationDto outCardNumber, decimal balance)
    {

       var cards = await _unitOfWork.CardInterface.GetAllAsync();
        var card = cards.FirstOrDefault(c => c.CardNumber == userCardNumber.CardNumber);
        if (card == null)
        {
            throw new NotFoundException("Card topilmadi");
        }
        if(userCardNumber.CardNumber == outCardNumber.CardNumber)
        {
            throw new CustomException("Bir hil kartalardan pul o'tkazilmaydi");
        }
        if(balance < 1000)
        {
            throw new CustomException("Pul miqdiri yetarli emas");
        }
        await _trancationInterface.InTransaction( userCardNumber.CardNumber, outCardNumber.CardNumber, balance);

        return "Succecfuly";
    }

}
