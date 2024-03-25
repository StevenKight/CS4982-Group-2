using CapstoneGroup2.Server.Model;

namespace CapstoneGroup2.Server.Dal;

/// <summary>
/// Class for accessing data for tags
/// </summary>
/// <seealso cref="CapstoneGroup2.Server.Dal.IDbDal&lt;CapstoneGroup2.Server.Model.Tag&gt;" />
public class TagDal : IDbDal<Tag>
{

    /// <summary>
    /// The context
    /// </summary>
    private readonly DocunotesDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="SourceDal" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public TagDal(DocunotesDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="System.UnauthorizedAccessException"></exception>
    public bool Add(Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        this.context.Add(entity);

        return this.context.SaveChanges() > 0;
    }

    /// <summary>
    /// Deletes the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public bool Delete(Tag entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if (entity.TagID < 1)
        {
            throw new ArgumentOutOfRangeException();
        }

        var notes = this.context.Notes_Tags.Where(x => x.TagID == entity.TagID);
        foreach (var note in notes)
        {
            this.context.Notes_Tags.Remove(note);
        }

        var tag = this.context.Tags.Find(entity.TagID);
        if (tag != null)
        {
            this.context.Tags.Remove(tag);
        }

        return this.context.SaveChanges() > 0;
    }

    /// <summary>
    /// Gets the specified key values.
    /// </summary>
    /// <param name="keyValues">The key values.</param>
    /// <returns></returns>
    /// <exception cref="System.InvalidCastException"></exception>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    /// <exception cref="System.UnauthorizedAccessException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    public Tag Get(params object?[]? keyValues)
    {
        if (keyValues is not { Length: 1 } ||
            keyValues[0] == null || typeof(int) != keyValues[0]?.GetType())
        {
            throw new InvalidCastException();
        }

        var tagId = (int)keyValues[0]!;
        if (tagId < 1)
        {
            throw new ArgumentOutOfRangeException();
        }

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        var tag = this.context.Tags.Find(tagId) ?? throw new InvalidOperationException();

        return  tag;
    }

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.UnauthorizedAccessException"></exception>
    public IEnumerable<Tag> GetAll()
    {
        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        return this.context.Tags;
    }

    /// <summary>
    /// Sets the source identifier.
    /// </summary>
    /// <param name="sourceId">The source identifier.</param>
    /// <exception cref="System.InvalidOperationException"></exception>
    public void SetSourceId(int sourceId)
    {
        throw new InvalidOperationException();
    }

    /// <summary>
    /// Sets the user.
    /// </summary>
    /// <param name="username">The username.</param>
    public void SetUser(string username)
    {
        var user = new User { Username = username };
        this.context.CurrentUser = user;
    }

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    /// <exception cref="System.UnauthorizedAccessException"></exception>
    public bool Update(Tag entity)
    {
        throw new NotImplementedException();
    }
}