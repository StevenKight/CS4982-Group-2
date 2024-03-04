using CapstoneGroup2.Server.Model;

namespace CapstoneGroup2.Server.Dal;

/// <summary>
///     Class for accessing db for notes logic
/// </summary>
/// <seealso cref="CapstoneGroup2.Server.Dal.IDbDal&lt;CapstoneGroup2.Server.Model.Note&gt;" />
public class NotesDal : IDbDal<Note>
{
    #region Data members

    /// <summary>
    ///     The context
    /// </summary>
    private readonly DocunotesDbContext context;

    /// <summary>
    ///     The source identifier
    /// </summary>
    private int sourceId;

    #endregion

    #region Constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="NotesDal" /> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public NotesDal(DocunotesDbContext context)
    {
        this.context = context;
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Gets the specified key values.
    /// </summary>
    /// <param name="keyValues">The key values.</param>
    /// <returns>Note for given key values</returns>
    /// <exception cref="System.InvalidCastException"></exception>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    /// <exception cref="System.InvalidOperationException"></exception>
    public Note Get(params object?[]? keyValues)
    {
        if (keyValues is not { Length: 1 } ||
            keyValues[0] == null || typeof(int) != keyValues[0]?.GetType())
        {
            throw new InvalidCastException();
        }

        var noteId = (int)keyValues[0]!;
        if (noteId < 1)
        {
            throw new ArgumentOutOfRangeException();
        }

        var note = this.context.Notes
            .Find(noteId) ?? throw new InvalidOperationException();

        return note;
    }

    /// <summary>
    ///     Gets all.
    /// </summary>
    /// <returns>All notes</returns>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    /// <exception cref="System.UnauthorizedAccessException"></exception>
    public IEnumerable<Note> GetAll()
    {
        if (this.sourceId < 1)
        {
            throw new ArgumentOutOfRangeException();
        }

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        var notes = this.context.Notes
            .Where(note => note.Username.Equals(username) && note.SourceId == this.sourceId);

        return notes;
    }

    /// <summary>
    ///     Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>true if success, false otherwise</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="System.UnauthorizedAccessException"></exception>
    public bool Add(Note entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        entity.Username = username;

        this.context.Notes.Add(entity);
        return this.context.SaveChanges() > 0;
    }

    /// <summary>
    ///     Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>true if success, false otherwise</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    /// <exception cref="System.UnauthorizedAccessException"></exception>
    public bool Update(Note entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        if (entity.Username != username)
        {
            throw new UnauthorizedAccessException();
        }

        this.context.Notes.Update(entity);
        return this.context.SaveChanges() > 0;
    }

    /// <summary>
    ///     Deletes the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>true if success, false otherwise</returns>
    /// <exception cref="System.ArgumentNullException"></exception>
    public bool Delete(Note entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        this.context.Notes.Remove(entity);
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
    public void SetSourceId(int sourceId)
    {
        this.sourceId = sourceId;
    }

    #endregion
}