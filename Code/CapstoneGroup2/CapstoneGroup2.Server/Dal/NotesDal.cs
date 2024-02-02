using API.Model;

namespace API.Dal;

public class NotesDal : IDbDal<Note>
{
    #region Data members

    private readonly StudyApiDbContext context;

    #endregion

    #region Constructors

    public NotesDal(StudyApiDbContext context) // , IDbDal<Source> sourceDal
    {
        this.context = context;
        //this.sourceDal = sourceDal;
    }

    #endregion

    #region Methods

    public Note Get(params object?[]? keyValues)
    {
        if (keyValues is not { Length: 1 } ||
            keyValues[0] == null || typeof(int) != keyValues[0]?.GetType())
        {
            throw new InvalidCastException();
        }

        var sourceId = (int)(keyValues[0] ?? throw new ArgumentNullException());
        if (sourceId < 1)
        {
            throw new ArgumentOutOfRangeException();
        }

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        var note = this.context.Notes
            .Find(sourceId, username) ?? throw new InvalidOperationException();

        //note.Source = this.sourceDal.Get(note.SourceId);

        return note;
    }

    public IEnumerable<Note> GetAll()
    {
        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        var notes = this.context.Notes
            .Where(note => note.Username.Equals(username));

        //Parallel.ForEach(notes, note => note.Source = this.sourceDal.Get(note.SourceId));

        return notes;
    }

    public bool Add(Note entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        entity.Username = username;

        this.context.Notes.Add(entity);
        return this.context.SaveChanges() > 0;
    }

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

    public bool Delete(Note entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        if (entity.Username != username)
        {
            throw new UnauthorizedAccessException();
        }

        this.context.Notes.Remove(entity);
        return this.context.SaveChanges() > 0;
    }

    #endregion
}