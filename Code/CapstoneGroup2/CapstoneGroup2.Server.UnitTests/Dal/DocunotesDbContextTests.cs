using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.UnitTests.Dal;

/* dotcover disable */
[TestFixture]
public class DocunotesDbContextTests
{
    #region Data members

    private DbContextOptions<DocunotesDbContext> _options;
    private DocunotesDbContext _context;

    #endregion

    #region Methods

    [SetUp]
    public void Setup()
    {
        this._options = new DbContextOptionsBuilder<DocunotesDbContext>()
            .UseInMemoryDatabase("Docunotes")
            .Options;
        this._context = new DocunotesDbContext(this._options);
    }

    [Test]
    public void TestConstructor()
    {
        Assert.IsNotNull(this._context);
    }

    [Test]
    public void TestCurrentUser()
    {
        Assert.IsNull(this._context.CurrentUser);
        this._context.CurrentUser = new User();
        Assert.IsNotNull(this._context.CurrentUser);
    }

    [Test]
    public void TestNotes()
    {
        Assert.IsNotNull(this._context.Notes);
    }

    [Test]
    public void TestSources()
    {
        Assert.IsNotNull(this._context.Sources);
    }

    [Test]
    public void TestUsers()
    {
        Assert.IsNotNull(this._context.Users);
    }

    #endregion
}