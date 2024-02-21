using CapstoneGroup2.Server.Model;

namespace CapstoneGroup2.Server.Dal;

/// <summary>
///     Class for accessing db for Source logic
/// </summary>
/// <seealso cref="CapstoneGroup2.Server.Dal.IDbDal&lt;CapstoneGroup2.Server.Model.Source&gt;" />
public class SourceDal : IDbDal<Source>
{
    #region Data members

    /// <summary>
    ///     The context
    /// </summary>
    private readonly DocunotesDbContext context;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="SourceDal" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public SourceDal(DocunotesDbContext context)
    {
        this.context = context;
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Gets the specified key values.
    /// </summary>
    /// <param name="keyValues">The key values.</param>
    /// <returns>Source for with given key values</returns>
    /// <exception cref="System.InvalidCastException"></exception>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    /// <exception cref="System.UnauthorizedAccessException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    public Source Get(params object?[]? keyValues)
    {
        if (keyValues is not { Length: 1 } ||
            keyValues[0] == null || typeof(int) != keyValues[0]?.GetType())
        {
            throw new InvalidCastException();
        }

        var sourceId = (int)keyValues[0]!;
        if (sourceId < 1)
        {
            throw new ArgumentOutOfRangeException();
        }

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        var source = this.context.Sources.Find(sourceId) ?? throw new InvalidOperationException();

        return username.Equals(source.Username) ? source : throw new UnauthorizedAccessException();
    }

    /// <summary>
    ///     Gets all.
    /// </summary>
    /// <returns>Sources</returns>
    /// <exception cref="System.UnauthorizedAccessException"></exception>
    public IEnumerable<Source> GetAll()
    {
        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        return this.context.Sources.Where(x => x.Username.Equals(username));
    }

    /// <summary>
    ///     Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>true if success, false otherwise</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="System.UnauthorizedAccessException"></exception>
    public bool Add(Source entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        entity.Username = username;

        entity.CreatedAt = DateTime.Now;

        this.context.Sources.Add(entity);
        return this.context.SaveChanges() > 0;
    }

    /// <summary>
    ///     Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>true if success, false otherwise</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="System.UnauthorizedAccessException"></exception>
    public bool Update(Source entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        if (entity.Username != username)
        {
            throw new UnauthorizedAccessException();
        }

        this.context.Sources.Update(entity);
        return this.context.SaveChanges() > 0;
    }

    /// <summary>
    ///     Deletes the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>true if success, false otherwise</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public bool Delete(Source entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity.SourceId < 1)
        {
            throw new ArgumentOutOfRangeException();
        }

        // TODO: Cascade delete instead of manual delete
        var notes = this.context.Notes.Where(x => x.SourceId == entity.SourceId);
        foreach (var note in notes)
        {
            this.context.Notes.Remove(note);
        }

        var source = this.context.Sources.Find(entity.SourceId);
        if (source != null)
        {
            this.context.Sources.Remove(source);
        }

        return this.context.SaveChanges() > 0;
    }

    /// <summary>
    ///     Sets the user.
    /// </summary>
    /// <param name="username">The username.</param>
    public void SetUser(string username)
    {
        var user = new User { Username = username };
        this.context.CurrentUser = user;
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