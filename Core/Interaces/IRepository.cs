namespace Core;

public interface IRepository<T>
{
    public Task<Result<T>> Create(T entity);
    
    public Task<Result<IEnumerable<T>>> GetAll();
    public Task<Result<T>> GetById(Guid id);
    public Task<Result<T>> Get(T entity);
    
    public Task<Result> Update(T entity);
    public Task<Result> Delete(T entity);
}