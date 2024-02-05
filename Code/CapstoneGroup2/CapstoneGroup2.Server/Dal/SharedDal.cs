using CapstoneGroup2.Server.Model;

namespace CapstoneGroup2.Server.Dal;

public class SharedDal : IDbDal<Shared>
{
    #region Data members

    private readonly DocunotesDbContext context;

    #endregion

    #region Constructors

    public SharedDal(DocunotesDbContext context)
    {
        this.context = context;
    }

    #endregion

    #region Methods

    public Shared Get(params object?[]? keyValues)
    {
        if (keyValues is not { Length: 2 } ||
            keyValues[0] == null || typeof(int) != keyValues[0]?.GetType() ||
            keyValues[1] == null || typeof(string) != keyValues[1]?.GetType())
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

        var sharedUsername = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        var shared = this.context.SharedNotes
            .Find(sourceId, username, sharedUsername) ?? throw new InvalidOperationException();

        return shared;
    }

    public IEnumerable<Shared> GetAll()
    {
        var sharedUsername = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        return this.context.SharedNotes
            .Where(x => x.SharedUsername.Equals(sharedUsername));
    }

    public bool Add(Shared entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var sharedUsername = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        entity.SharedUsername = sharedUsername;

        this.context.SharedNotes.Add(entity);
        return this.context.SaveChanges() > 0;
    }

    public bool Update(Shared entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var sharedUsername = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        if (entity.SharedUsername != sharedUsername)
        {
            throw new UnauthorizedAccessException();
        }

        this.context.SharedNotes.Update(entity);
        return this.context.SaveChanges() > 0;
    }

    public bool Delete(Shared entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var sharedUsername = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        if (entity.SharedUsername != sharedUsername)
        {
            throw new UnauthorizedAccessException();
        }

        this.context.SharedNotes.Remove(entity);
        return this.context.SaveChanges() > 0;
    }

    public void SetUser(string username)
    {
        var user = new User { Username = username };
        this.context.CurrentUser = user;
    }

    #endregion
}