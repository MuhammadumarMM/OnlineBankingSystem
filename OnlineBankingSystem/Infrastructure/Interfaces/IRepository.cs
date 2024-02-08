using Domain.Entities.Entity;

namespace Infrastructure.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task<T?> UpdateAsync(T entity);
    void DeleteAsync(T entity);

}
