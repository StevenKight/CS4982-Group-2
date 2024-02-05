using CapstoneGroup2.Server.Model;

namespace CapstoneGroup2.Server.Dal;

public class UserDal : IDbDal<User>
{
    #region Data members

    private readonly DocunotesDbContext context;

    #endregion

    #region Constructors

    public UserDal(DocunotesDbContext context)
    {
        this.context = context;
    }

    #endregion

    #region Methods

    public User Get(params object?[]? keyValues)
    {
        if (keyValues is not { Length: 1 } ||
            keyValues[0] == null || typeof(string) != keyValues[0]?.GetType())
        {
            throw new InvalidCastException();
        }

        var username = (string)(keyValues[0] ?? throw new ArgumentNullException());
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentOutOfRangeException();
        }

        return this.context.Users
            .Find(username) ?? throw new InvalidOperationException();
    }

    public IEnumerable<User> GetAll()
    {
        return this.context.Users;
    }

    public bool Add(User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        this.context.Users.Add(entity);
        return this.context.SaveChanges() > 0;
    }

    public bool Update(User entity)
    {
        throw new InvalidOperationException();
    }

    public bool Delete(User entity)
    {
        throw new InvalidOperationException();
    }

    #endregion
}