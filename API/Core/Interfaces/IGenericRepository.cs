using Core.Entities;

namespace Core.Interfaces;

public interface IGenericRepository<T> where T : BaseEntity
{
    public Task<T?> GetByIdAsync(int id);
    public Task<IReadOnlyList<T>> ListAllAsync();

    public Task<T?> GetEntityWithSpec(ISpecification<T> spec);
    public Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    
    public Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> spec);
    public Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec);
    
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
    Task<bool> SaveAllAsync();
    Task<bool> Exists(int id);
    Task<int> CountAsync(ISpecification<T> spec);
}