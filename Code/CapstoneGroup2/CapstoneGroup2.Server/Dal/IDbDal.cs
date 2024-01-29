namespace API.Dal;

public interface IDbDal<T>
{
    public T Get(params object?[]? keyValues);

    public IEnumerable<T> GetAll();

    public bool Add(T entity);

    public bool Update(T entity);

    public bool Delete(T entity);
}