using API.Model;

namespace API.Dal;

public class SourceDal : IDbDal<Source>
{
    #region Data members

    private readonly StudyApiDbContext context;

    #endregion

    #region Constructors

    public SourceDal(StudyApiDbContext context)
    {
        this.context = context;
    }

    #endregion

    #region Methods

    public Source Get(params object?[]? keyValues) // TODO: Only if current user has access
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

        return this.context.Sources
            .Find(sourceId, username) ?? throw new InvalidOperationException();
    }

    public IEnumerable<Source> GetAll() // TODO: Only current user's sources
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

    #endregion
}