namespace Core;

public interface IRepositoryBase<T>
{
    public Task<Result<T>> Create(T entity);
    public Task<Result<T>> Get(T entity);
    public Task<Result<IEnumerable<T>>> GetAll();
    public Task<Result> Update(T entity);
    public Task<Result> Delete(T entity);
}