using CapstoneGroup2.Server.Model;

namespace CapstoneGroup2.Server.Dal;

public class SourceDal : IDbDal<Source>
{
    #region Data members

    private readonly DocunotesDbContext context;

    #endregion

    #region Constructors

    public SourceDal(DocunotesDbContext context)
    {
        this.context = context;
    }

    #endregion

    #region Methods

    public Source Get(params object?[]? keyValues)
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

        var source = this.context.Sources.Find(sourceId) ?? throw new InvalidOperationException();

        return username.Equals(source.Username) ? source : throw new UnauthorizedAccessException();
    }

    public IEnumerable<Source> GetAll()
    {
        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        return this.context.Sources.Where(x => x.Username.Equals(username));
    }

    public bool Add(Source entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        entity.Username = username;

        this.context.Sources.Add(entity);
        return this.context.SaveChanges() > 0;
    }

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

    public bool Delete(Source entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        var username = this.context.CurrentUser?.Username ?? throw new UnauthorizedAccessException();

        if (entity.Username != username)
        {
            throw new UnauthorizedAccessException();
        }

        this.context.Sources.Remove(entity);
        return this.context.SaveChanges() > 0;
    }

    public void SetUser(string username)
    {
        var user = new User { Username = username };
        this.context.CurrentUser = user;
    }

    public void SetSourceId(int sourceId)
    {
        throw new InvalidOperationException();
    }

    #endregion
}