using CapstoneGroup2.Server.Model;

namespace CapstoneGroup2.Server.Dal;

/// <summary>
///     Class for accessing db for user logic
/// </summary>
/// <seealso cref="CapstoneGroup2.Server.Dal.IDbDal&lt;CapstoneGroup2.Server.Model.User&gt;" />
public class UserDal : IDbDal<User>
{
    #region Data members

    /// <summary>
    ///     The context
    /// </summary>
    private readonly DocunotesDbContext context;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserDal" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public UserDal(DocunotesDbContext context)
    {
        this.context = context;
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Gets the specified key values.
    /// </summary>
    /// <param name="keyValues">The key values.</param>
    /// <returns>User object</returns>
    /// <exception cref="System.InvalidCastException"></exception>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    public User Get(params object?[]? keyValues)
    {
        if (keyValues is not { Length: 1 } ||
            keyValues[0] == null || typeof(string) != keyValues[0]?.GetType())
        {
            throw new InvalidCastException();
        }

        var username = (string)keyValues[0]!;
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentOutOfRangeException();
        }

        return this.context.Users
            .Find(username) ?? throw new InvalidOperationException();
    }

    /// <summary>
    ///     Gets all.
    /// </summary>
    /// <returns>Users</returns>
    public IEnumerable<User> GetAll()
    {
        return this.context.Users;
    }

    /// <summary>
    ///     Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>true if success, false otherwise</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public bool Add(User entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        this.context.Users.Add(entity);
        return this.context.SaveChanges() > 0;
    }

    /// <summary>
    ///     Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>true if success false otherwise</returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    public bool Update(User entity)
    {
        throw new InvalidOperationException();
    }

    /// <summary>
    ///     Deletes the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns> true if success, false otherwise</returns>
    /// <exception cref="System.InvalidOperationException"></exception>
    public bool Delete(User entity)
    {
        throw new InvalidOperationException();
    }

    /// <summary>
    ///     Sets the user.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <exception cref="System.InvalidOperationException"></exception>
    public void SetUser(string username)
    {
        throw new InvalidOperationException();
    }

    /// <summary>
    ///     Sets the source identifier.
    /// </summary>
    /// <param name="sourceId">The source identifier.</param>
    /// <exception cref="System.InvalidOperationException"></exception>
    public void SetSourceId(int sourceId)
    {
        throw new InvalidOperationException();
    }

    #endregion
}