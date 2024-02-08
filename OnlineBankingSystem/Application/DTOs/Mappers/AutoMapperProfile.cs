using Application.DTOs.BankCardDtos;
using Application.DTOs.CardDtos;
using Application.DTOs.CreditDtos;
using Application.DTOs.DepositDtos;
using AutoMapper;
using Domain.Entities.Entity;

namespace Application.DTOs.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CardDto, Card>().ReverseMap();
        CreateMap<AddCard, Card>();
        CreateMap<UpdateCard, Card>();
        CreateMap<TrancationDto, Card>().ReverseMap();
        CreateMap<OutTrancationDto , Card>().ReverseMap();


        CreateMap<CreditDto, Credit>().ReverseMap();
        CreateMap<UpdateCredit, Credit>();
        CreateMap<AddCredit, Credit>();

        CreateMap<BankCardDto, BankCard>().ReverseMap();
        CreateMap<AddBankCardDto, BankCard>();
        CreateMap<UpdateBankCardDto, BankCard>();


        
        CreateMap<DepositDto, Deposit>().ReverseMap();
        CreateMap<AddDeposit, Deposit>();
        CreateMap<UpdateDeposit, Deposit>();

        
    }
}
