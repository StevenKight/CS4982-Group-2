using API.Model;

namespace API.Dal;

public class NotesDal : IDbDal<Note>
{
    #region Data members

    private readonly StudyApiDbContext context;

    private readonly SourceDal sourceDal;

    #endregion

    #region Constructors

    public NotesDal(StudyApiDbContext context, SourceDal sourceDal)
    {
        this.context = context;
        this.sourceDal = sourceDal;
    }

    #endregion

    public Note Get(params object?[]? keyValues) // TODO: Only if current user has access
    {
        if (keyValues is not { Length: 2 } ||
            keyValues[0] == null || typeof(int) != keyValues[0]?.GetType() ||
            keyValues[1] == null || typeof(string) != keyValues[1]?.GetType())
        {
            throw new InvalidCastException();
        }

        var sourceId = (int)(keyValues[0] ?? throw new ArgumentNullException());
        var username = (string)(keyValues[1] ?? throw new ArgumentNullException());
        if (sourceId < 1 || string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentOutOfRangeException();
        }

        var note = this.context.Notes
            .Find(sourceId, username) ?? throw new InvalidOperationException();

        note.Source = this.sourceDal.Get(note.SourceId);

        return note;
    }

    public IEnumerable<Note> GetAll() // TODO: Only current user's notes
    {
        var notes = this.context.Notes;

        Parallel.ForEach(notes, note => note.Source = this.sourceDal.Get(note.SourceId));

        return notes;
    }

    public bool Add(Note entity)
    {
        throw new NotImplementedException();
    }

    public bool Update(Note entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Note entity)
    {
        throw new NotImplementedException();
    }
}