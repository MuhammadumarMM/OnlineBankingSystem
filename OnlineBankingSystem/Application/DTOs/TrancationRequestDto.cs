using Application.DTOs.CardDtos;

namespace Application.DTOs
{
    public class TrancationRequestDto
    {
        public TrancationDto GiveCard { get; set; }
        public OutTrancationDto TakeCard { get; set; }
        public decimal Amount { get; set; } = 0;
    }
}
