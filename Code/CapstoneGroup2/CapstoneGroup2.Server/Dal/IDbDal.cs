namespace CapstoneGroup2.Server.Dal;

public interface IDbDal<T>
{
    #region Methods

    public T Get(params object?[]? keyValues);

    public IEnumerable<T> GetAll();

    public bool Add(T entity);

    public bool Update(T entity);

    public bool Delete(T entity);

    public void SetUser(string username);
    void SetSourceId(int sourceId);

    #endregion
}