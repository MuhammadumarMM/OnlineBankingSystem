

using Domain.Entities.Enams;
using Domain.Entities.Entity;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace OnlineBankingSystem.test.Infastructure.Repository;

[TestFixture]
public class CreditRepositoryTest
{
    private DbContextOptions<ApplicationDbContext> options =
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("CreditDb")
            .Options;

    private ApplicationDbContext _dbContext;
    private CreditRepository _creditRepository;

    [SetUp]
    public void Setup()
    {
        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureCreated();
        _creditRepository = new CreditRepository(_dbContext);
    }

    [Test]
    public async Task Test1_AddNewCredit()
    {
        // Arrange
        var credit = new Credit
        {
            FisrtName = "John",
            LastName = "Doe",
            JSHR = 12345,
            Birthday = new DateTime(1990, 1, 1),
            Amount = 10000,
            DepositMonth = DepositMonth.oy12,
            CreditDay = DateTime.Now,
            MonthPay = 500
        };

        // Act
        await _creditRepository.CreateAsync(credit);
        await _dbContext.SaveChangesAsync();

        // Assert
        Assert.That(_dbContext.credits.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task Test2_UpdateCredit()
    {
        // Arrange
        var credit = new Credit
        {
            FisrtName = "John",
            LastName = "Doe",
            JSHR = 12345,
            Birthday = new DateTime(1990, 1, 1),
            Amount = 10000,
            DepositMonth = DepositMonth.oy12,
            CreditDay = DateTime.Now,
            MonthPay = 500
        };
        await _creditRepository.CreateAsync(credit);
        await _dbContext.SaveChangesAsync();

        // Act
        credit.Amount = 8000; // Update amount
        await _creditRepository.UpdateAsync(credit);
        await _dbContext.SaveChangesAsync();

        // Assert
        var updatedCredit = await _creditRepository.GetByIdAsync(credit.Id);
        Assert.That(updatedCredit.Amount, Is.EqualTo(8000));
    }

    [Test]
    public async Task Test3_GetAllCredits()
    {
        // Arrange
        var credits = new List<Credit>
            {
                new Credit { FisrtName = "Alice", LastName = "Smith", JSHR = 54321, Amount = 12000, DepositMonth = DepositMonth.oy18},
                new Credit { FisrtName = "Bob", LastName = "Johnson", JSHR = 98765, Amount = 15000, DepositMonth = DepositMonth.oy24 },
                new Credit { FisrtName = "Charlie", LastName = "Brown", JSHR = 11111, Amount = 8000, DepositMonth = DepositMonth.oy12 }
            };

        foreach (var credit in credits)
        {
            await _creditRepository.CreateAsync(credit);
        }
        await _dbContext.SaveChangesAsync();

        // Act
        var retrievedCredits = await _creditRepository.GetAllAsync();

        // Assert
        Assert.That(retrievedCredits.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task Test4_GetCreditById()
    {
        // Arrange
        var credit = new Credit { FisrtName = "John", LastName = "Doe", JSHR = 12345, Amount = 10000, DepositMonth = DepositMonth.oy12 };
        await _creditRepository.CreateAsync(credit);
        await _dbContext.SaveChangesAsync();

        // Act
        var retrievedCredit = await _creditRepository.GetByIdAsync(credit.Id);

        // Assert
        Assert.That(credit.Amount, Is.EqualTo(retrievedCredit.Amount));
    }

    [Test]
    public async Task Test5_DeleteCredit()
    {
        // Arrange
        var credit = new Credit { FisrtName = "John", LastName = "Doe", JSHR = 12345, Amount = 10000, DepositMonth = DepositMonth.oy12 };
        await _creditRepository.CreateAsync(credit);
        await _dbContext.SaveChangesAsync();

        // Act
        _creditRepository.DeleteAsync(credit);
        await _dbContext.SaveChangesAsync();

        var retrievedCredit = await _creditRepository.GetByIdAsync(credit.Id);

        // Assert
        Assert.That(retrievedCredit, Is.EqualTo(null));
    }
}
