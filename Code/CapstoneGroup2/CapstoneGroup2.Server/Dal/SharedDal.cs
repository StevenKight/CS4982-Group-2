using API.Model;

namespace API.Dal;

public class SharedDal : IDbDal<Shared>
{
    #region Data members

    private readonly StudyApiDbContext context;

    #endregion

    #region Constructors

    public SharedDal(StudyApiDbContext context)
    {
        this.context = context;
    }

    #endregion

    #region Methods

    public Shared Get(params object?[]? keyValues)
    {
        if (keyValues is not { Length: 3 } ||
            keyValues[0] == null || typeof(int) != keyValues[0]?.GetType() ||
            keyValues[1] == null || typeof(string) != keyValues[1]?.GetType() ||
            keyValues[2] == null || typeof(string) != keyValues[2]?.GetType())
        {
            throw new InvalidCastException();
        }

        var sourceId = (int)(keyValues[0] ?? throw new ArgumentNullException());
        var username = (string)(keyValues[1] ?? throw new ArgumentNullException());
        if (sourceId < 1 ||
            string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentOutOfRangeException();
        }

        var sharedUsername = this.context.CurrentUser?.Username ?? throw new InvalidOperationException();

        return this.context.SharedNotes
            .Find(sourceId, username, sharedUsername) ?? throw new InvalidOperationException();
    }

    public IEnumerable<Shared> GetAll()
    {
        var sharedUsername = this.context.CurrentUser?.Username ?? throw new InvalidOperationException();

        return this.context.SharedNotes
            .Where(x => x.Username.Equals(sharedUsername));
    }

    public bool Add(Shared entity)
    {
        throw new NotImplementedException();
    }

    public bool Update(Shared entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Shared entity)
    {
        throw new NotImplementedException();
    }

    #endregion
}