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

        var tags = this.context.Notes_Tags
            .Where(tag => tag.NoteID == noteId).Select(x => x.TagID).ToList();

        note.Tags = this.context.Tags.Where(tag => tags.Contains(tag.TagID)).ToList();

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
            .Where(note => note.Username.Equals(username) && note.SourceId == this.sourceId).ToList();

        foreach (var note in notes)
        {
            var tags = this.context.Notes_Tags
                .Where(tag => tag.NoteID == note.NoteId).Select(x => x.TagID).ToList();

            note.Tags = this.context.Tags.Where(tag => tags.Contains(tag.TagID)).ToList();
        }

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

        this.UpdateTags(entity);

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

    private void UpdateTags(Note entity)
    {
        var tags = this.context.Notes_Tags
            .Where(tag => tag.NoteID == entity.NoteId).ToList();

        foreach (var tag in tags)
        {
            this.context.Notes_Tags.Remove(tag);
        }

        foreach (var entityTag in entity.Tags)
        {
            var tag = this.context.Tags.FirstOrDefault(x => x.TagName == entityTag.TagName);
            if (tag == null)
            {
                tag = new Tag { TagName = entityTag.TagName };
                this.context.Tags.Add(tag);
                this.context.SaveChanges();
                tag = this.context.Tags.FirstOrDefault(x => x.TagName == entityTag.TagName);
            }

            var noteTag = new Note_Tag { NoteID = entity.NoteId, TagID = tag.TagID };
            this.context.Notes_Tags.Add(noteTag);
        }
    }

    #endregion
}