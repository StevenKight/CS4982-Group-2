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

        return this.context.Sources
            .Find(sourceId) ?? throw new InvalidOperationException();
    }

    public IEnumerable<Source> GetAll() // TODO: Only current user's sources
    {
        return this.context.Sources;
    }

    public bool Add(Source entity)
    {
        throw new NotImplementedException();
    }

    public bool Update(Source entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Source entity)
    {
        throw new NotImplementedException();
    }

    #endregion
}