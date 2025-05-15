namespace DrkbWikiFileSaver.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(Guid id);
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task DeleteAsync(T entity);
    Task UpdateAsync(T entity);
}