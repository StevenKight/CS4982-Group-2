using API.Dal;
using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API_Tests.Dal;

[TestFixture]
public class SharedDalTests
{
    #region Data members

    private readonly List<Shared> _sharedNotes = new()
    {
        new Shared
        {
            SourceId = 1, Username = "testUser2", SharedUsername = "testUser", Comment = "testComment"
        },
        new Shared
        {
            SourceId = 2, Username = "testUser2", SharedUsername = "testUser", Comment = "testComment2"
        }
    };

    private DbContextOptions<DocunotesDbContext> _options;
    private DocunotesDbContext _context;

    #endregion

    #region Methods

    [OneTimeSetUp]
    public void Setup()
    {
        this._options = new DbContextOptionsBuilder<DocunotesDbContext>()
            .UseInMemoryDatabase("Docunotes")
            .EnableSensitiveDataLogging()
            .Options;
        this._context = new DocunotesDbContext(this._options);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        this._context.SharedNotes.AddRange(this._sharedNotes);
        this._context.SaveChanges();
    }

    [SetUp]
    public void SetupEach()
    {
        this._context.ChangeTracker.Clear();
    }

    [Test]
    [Order(1)]
    public void ConstructorTest()
    {
        // Arrange
        var sharedDal = new SharedDal(this._context);

        // Assert
        Assert.IsNotNull(sharedDal);
    }

    [Test]
    [Order(2)]
    public void GetSharedByIdTest()
    {
        // Arrange
        var sharedDal = new SharedDal(this._context);

        // Act
        var result = sharedDal.Get(1, "testUser2");

        var expected = this._sharedNotes[0];

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expected.SourceId, result.SourceId);
        Assert.AreEqual(expected.Username, result.Username);
        Assert.AreEqual(expected.SharedUsername, result.SharedUsername);
        Assert.AreEqual(expected.Comment, result.Comment);
    }

    [Test]
    [Order(3)]
    public void GetAllSharedTest()
    {
        // Arrange
        var sharedDal = new SharedDal(this._context);

        // Act
        var result = sharedDal.GetAll();

        var expected = this._sharedNotes.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());

        for (var i = 0; i < expected; i++)
        {
            var sharedNote = this._sharedNotes[i];
            var actual = result.ElementAt(i);
            Assert.AreEqual(sharedNote.SourceId, actual.SourceId);
            Assert.AreEqual(sharedNote.Username, actual.Username);
            Assert.AreEqual(sharedNote.SharedUsername, actual.SharedUsername);
            Assert.AreEqual(sharedNote.Comment, actual.Comment);
        }
    }

    [Test]
    [Order(4)]
    public void AddSharedTest()
    {
        // Arrange
        var sharedDal = new SharedDal(this._context);
        var shared = new Shared
        {
            SourceId = 3,
            Username = "testUser3",
            SharedUsername = "testUser",
            Comment = "testComment3"
        };

        // Act
        var result = sharedDal.Add(shared);

        this._sharedNotes.Add(shared);
        var expected = this._sharedNotes.Count();

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, this._context.SharedNotes.Count());
        Assert.Contains(shared, this._context.SharedNotes.ToList());
    }

    [Test]
    [Order(5)]
    public void UpdateSharedTest()
    {
        // Arrange
        var sharedDal = new SharedDal(this._context);
        var shared = new Shared
        {
            SourceId = 2,
            Username = "testUser2",
            SharedUsername = "testUser",
            Comment = "testComment6"
        };

        // Act
        var result = sharedDal.Update(shared);

        this._sharedNotes.Add(shared);
        this._sharedNotes.Remove(this._sharedNotes[1]);

        // Assert
        var actual = this._context.SharedNotes.Find(2, "testUser2", "testUser");
        Assert.IsTrue(result);
        Assert.AreEqual(shared.SourceId, actual.SourceId);
        Assert.AreEqual(shared.Username, actual.Username);
        Assert.AreEqual(shared.SharedUsername, actual.SharedUsername);
        Assert.AreEqual(shared.Comment, actual.Comment);
    }

    [Test]
    [Order(6)]
    public void DeleteSharedTest()
    {
        // Arrange
        var sharedDal = new SharedDal(this._context);

        // Act
        var result = sharedDal.Delete(this._sharedNotes[0]);

        this._sharedNotes.Remove(this._sharedNotes[0]);
        var expected = this._sharedNotes.Count();

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, this._context.SharedNotes.Count());

        var sharedNotes = this._context.SharedNotes.ToList();
        for (var i = 0; i < expected; i++)
        {
            Assert.IsTrue(this._sharedNotes.Exists(
                x => sharedNotes.ElementAt(i).SourceId == x.SourceId
                     && sharedNotes.ElementAt(i).Username.Equals(x.Username)
                     && sharedNotes.ElementAt(i).SharedUsername.Equals(x.SharedUsername)));
        }
    }

    #endregion
}