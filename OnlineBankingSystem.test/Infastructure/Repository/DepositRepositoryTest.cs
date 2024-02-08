using Domain.Entities.Enams;
using Domain.Entities.Entity;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace OnlineBankingSystem.test.Infastructure.Repository;

[TestFixture]
public class DepositRepositoryTest
{
    private DbContextOptions<ApplicationDbContext> options =
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("DepositDb")
            .Options;

    private ApplicationDbContext _dbContext;
    private DepositRepository _depositRepository;

    [SetUp]
    public void Setup()
    {
        _dbContext = new ApplicationDbContext(options);
        _dbContext.Database.EnsureCreated();
        _depositRepository = new DepositRepository(_dbContext);
    }

    [Test]
    public async Task Test1_AddNewDeposit()
    {
        // Arrange
        var deposit = new Deposit
        {
            FisrtName = "Alice",
            LastName = "Smith",
            JSHR = 54321,
            Birthday = new DateTime(1985, 5, 10),
            Amount = 12000,
            DepositMonth = DepositMonth.oy12,
            MonthPay = 600
        };

        // Act
        await _depositRepository.CreateAsync(deposit);
        await _dbContext.SaveChangesAsync();

        // Assert
        Assert.That(_dbContext.deposits.Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task Test2_UpdateDeposit()
    {
        // Arrange
        var deposit = new Deposit
        {
            FisrtName = "Alice",
            LastName = "Smith",
            JSHR = 54321,
            Birthday = new DateTime(1985, 5, 10),
            Amount = 12000,
            DepositMonth = DepositMonth.oy12,
            MonthPay = 600
        };
        await _depositRepository.CreateAsync(deposit);
        await _dbContext.SaveChangesAsync();

        // Act
        deposit.Amount = 10000; // Update amount
        await _depositRepository.UpdateAsync(deposit);
        await _dbContext.SaveChangesAsync();

        // Assert
        var updatedDeposit = await _depositRepository.GetByIdAsync(deposit.Id);
        Assert.That(updatedDeposit.Amount, Is.EqualTo(10000));
    }

    [Test]
    public async Task Test3_GetAllDeposits()
    {
        // Arrange
        var deposits = new List<Deposit>
        {
            new Deposit { FisrtName = "Alice", LastName = "Smith", JSHR = 54321, Amount = 12000, DepositMonth = DepositMonth.oy18, MonthPay = 600 },
            new Deposit { FisrtName = "Bob", LastName = "Johnson", JSHR = 98765, Amount = 15000, DepositMonth = DepositMonth.oy24, MonthPay = 750 },
            new Deposit { FisrtName = "Charlie", LastName = "Brown", JSHR = 11111, Amount = 8000, DepositMonth = DepositMonth.oy12, MonthPay = 400 }
        };

        foreach (var deposit in deposits)
        {
            await _depositRepository.CreateAsync(deposit);
        }
        await _dbContext.SaveChangesAsync();

        // Act
        var retrievedDeposits = await _depositRepository.GetAllAsync();

        // Assert
        Assert.That(retrievedDeposits.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task Test4_GetDepositById()
    {
        // Arrange
        var deposit = new Deposit { FisrtName = "Alice", LastName = "Smith", JSHR = 54321, Amount = 12000, DepositMonth = DepositMonth.oy18, MonthPay = 600 };
        await _depositRepository.CreateAsync(deposit);
        await _dbContext.SaveChangesAsync();

        // Act
        var retrievedDeposit = await _depositRepository.GetByIdAsync(deposit.Id);

        // Assert
        Assert.That(deposit.Amount, Is.EqualTo(retrievedDeposit.Amount));
    }

    [Test]
    public async Task Test5_DeleteDeposit()
    {
        // Arrange
        var deposit = new Deposit { FisrtName = "Alice", LastName = "Smith", JSHR = 54321, Amount = 12000, DepositMonth = DepositMonth.oy18, MonthPay = 600 };
        await _depositRepository.CreateAsync(deposit);
        await _dbContext.SaveChangesAsync();

        // Act
        _depositRepository.DeleteAsync(deposit);
        await _dbContext.SaveChangesAsync();

        var retrievedDeposit = await _depositRepository.GetByIdAsync(deposit.Id);

        // Assert
        Assert.That(retrievedDeposit, Is.EqualTo(null));
    }
}
