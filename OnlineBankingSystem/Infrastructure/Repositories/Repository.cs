using Domain.Entities.Entity;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class Repository<T>(ApplicationDbContext dbContext) : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _dbContext  = dbContext;

    public async Task<T> CreateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("Null");
        }
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public void DeleteAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException("Null");
        }
         _dbContext.Set<T>().Remove(entity);
        
    }

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbContext.Set<T>().AsNoTracking().ToListAsync();

    public async Task<T> GetByIdAsync(int id)
    {
        if(id < 0)
        {
            throw new ArgumentNullException("id");
        }
        var resalt =  await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        if (resalt == null)
        {
            throw new ArgumentNullException("Null");
        }
        return resalt;
    }

    public async Task<T?> UpdateAsync(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity cannot be null");
        }

        _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

}
