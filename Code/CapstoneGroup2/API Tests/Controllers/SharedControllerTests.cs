using API.Controllers;
using API.Dal;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Tests.Controllers;

[TestFixture]
public class SharedControllerTests
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

    private SharedDal _sharedDal;

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

        this._context.Database.EnsureDeleted();
        this._context.Database.EnsureCreated();

        this._context.SharedNotes.AddRange(this._sharedNotes);
        this._context.SaveChanges();

        this._sharedDal = new SharedDal(this._context);
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
        var sharedController = new SharedController(this._sharedDal);

        // Assert
        Assert.IsNotNull(sharedController);
    }

    [Test]
    [Order(2)]
    public void GetAllTest()
    {
        // Arrange
        var sharedController = new SharedController(this._sharedDal);

        // Act
        var result = sharedController.GetAll() as OkObjectResult;
        var resultList = result.Value as IEnumerable<Shared>;

        var expected = this._sharedNotes.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, resultList.Count());

        for (var i = 0; i < expected; i++)
        {
            var sharedNote = this._sharedNotes[i];
            var actual = resultList.ElementAt(i);
            Assert.AreEqual(sharedNote.SourceId, actual.SourceId);
            Assert.AreEqual(sharedNote.Username, actual.Username);
            Assert.AreEqual(sharedNote.SharedUsername, actual.SharedUsername);
            Assert.AreEqual(sharedNote.Comment, actual.Comment);
        }
    }

    [Test]
    [Order(3)]
    public void GetByIdTest()
    {
        // Arrange
        var sharedController = new SharedController(this._sharedDal);

        // Act
        var result = sharedController.GetById(1, "testUser2") as OkObjectResult;
        var resultObject = result.Value as Shared;

        var expected = this._sharedNotes[0];

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.IsNotNull(resultObject);
        Assert.AreEqual(expected.SourceId, resultObject.SourceId);
        Assert.AreEqual(expected.Username, resultObject.Username);
        Assert.AreEqual(expected.SharedUsername, resultObject.SharedUsername);
        Assert.AreEqual(expected.Comment, resultObject.Comment);
    }

    [Test]
    [Order(4)]
    public void CreateTest()
    {
        // Arrange
        var sharedController = new SharedController(this._sharedDal);
        var shared = new Shared
        {
            SourceId = 3,
            Username = "testUser3",
            SharedUsername = "testUser",
            Comment = "testComment3"
        };

        // Act
        var result = sharedController.Create(shared);

        this._sharedNotes.Add(shared);
        var expected = this._sharedNotes.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkResult>(result);
        Assert.AreEqual(expected, this._context.SharedNotes.Count());
        Assert.Contains(shared, this._context.SharedNotes.ToList());
    }

    [Test]
    [Order(5)]
    public void UpdateTest()
    {
        // Arrange
        var sharedController = new SharedController(this._sharedDal);
        var shared = new Shared
        {
            SourceId = 2,
            Username = "testUser2",
            SharedUsername = "testUser",
            Comment = "testComment6"
        };

        // Act
        var result = sharedController.Update(shared);

        this._sharedNotes.Add(shared);
        this._sharedNotes.Remove(this._sharedNotes[1]);

        // Assert
        var actual = this._context.SharedNotes.Find(2, "testUser2", "testUser");
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkResult>(result);
        Assert.AreEqual(shared.SourceId, actual.SourceId);
        Assert.AreEqual(shared.Username, actual.Username);
        Assert.AreEqual(shared.SharedUsername, actual.SharedUsername);
        Assert.AreEqual(shared.Comment, actual.Comment);
    }

    [Test]
    [Order(6)]
    public void DeleteTest()
    {
        // Arrange
        var sharedController = new SharedController(this._sharedDal);

        // Act
        var result = sharedController.Delete(this._sharedNotes[0]);

        this._sharedNotes.Remove(this._sharedNotes[0]);
        var expected = this._sharedNotes.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkResult>(result);
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