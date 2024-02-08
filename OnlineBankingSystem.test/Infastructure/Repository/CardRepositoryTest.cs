using Domain.Entities.Enams;
using Domain.Entities.Entity;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace OnlineBankingSystem.test.Infastructure.Repository;

[TestFixture]
public class CardRepositoryTest
{
    private DbContextOptions<ApplicationDbContext> options =
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("CardDb")
            .Options;

    private ApplicationDbContext _dbContext;
    private CardRepository _cardRepository;

    [SetUp]
    public void Setup()
    {
        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureCreated();
        _cardRepository = new CardRepository(_dbContext);
    }

    [Test]
    public async Task Test1_AddNewCard()
    {
        // Arrange
        var card = new Card
        {
            FirstName = "TestCard",
            BankName = "TestBank",
            CardNumber = "1234567890123456",
            CardType = CardType.Uzcard
        };

        // Act
        await _cardRepository.CreateAsync(card);
        await _dbContext.SaveChangesAsync();

        // Assert
        Assert.That(_dbContext.cards.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task Test2_UpdateCard()
    {
        // Arrange
        var card = new Card
        {
            FirstName = "TestCard",
            BankName = "TestBank",
            CardNumber = "1234567890123456",
            CardType = CardType.Uzcard
        };
        await _cardRepository.CreateAsync(card);
        await _dbContext.SaveChangesAsync();

        // Act
        card.Balance = 500000; // Update balance
        await _cardRepository.UpdateAsync(card);
        await _dbContext.SaveChangesAsync();

        // Assert
        var updatedCard = await _cardRepository.GetByIdAsync(card.Id);
        Assert.That(updatedCard.Balance, Is.EqualTo(500000));
    }

    [Test]
    public async Task Test3_GetAllCards()
    {
        // Arrange
        var cards = new List<Card>
        {
            new Card { FirstName = "Card1", BankName = "Bank1", CardType = CardType.Uzcard },
            new Card { FirstName = "Card2", BankName = "Bank2", CardType = CardType.Uzcard },
            new Card { FirstName = "Card3", BankName = "Bank3", CardType = CardType.Uzcard }
        };

        foreach (var card in cards)
        {
            await _cardRepository.CreateAsync(card);
        }
        await _dbContext.SaveChangesAsync();

        // Act
        var retrievedCards = await _cardRepository.GetAllAsync();

        // Assert
        Assert.That(retrievedCards.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task Test4_GetCardById()
    {
        // Arrange
        var card = new Card { FirstName = "TestCard", BankName = "TestBank", CardType = CardType.Uzcard };
        await _cardRepository.CreateAsync(card);
        await _dbContext.SaveChangesAsync();

        // Act
        var retrievedCard = await _cardRepository.GetByIdAsync(card.Id);

        // Assert
        Assert.That(card.CardType, Is.EqualTo(retrievedCard.CardType));
    }

    [Test]
    public async Task Test5_DeleteCard()
    {
        // Arrange
        var card = new Card { FirstName = "TestCard", BankName = "TestBank", CardType = CardType.Uzcard };
        await _cardRepository.CreateAsync(card);
        await _dbContext.SaveChangesAsync();

        // Act
        _cardRepository.DeleteAsync(card);
        await _dbContext.SaveChangesAsync();

        var retrievedCard = await _cardRepository.GetByIdAsync(card.Id);

        // Assert
        Assert.That(retrievedCard, Is.EqualTo(null));
    }
}
