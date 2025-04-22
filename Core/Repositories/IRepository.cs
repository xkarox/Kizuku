namespace Core.Repositories;

public interface IRepository<T>
{
    List<T> ReadAll();

    T ReadById(int criteria);
    T Create(T entity);
    T Update(T entity);
    T Delete(T entity);
}