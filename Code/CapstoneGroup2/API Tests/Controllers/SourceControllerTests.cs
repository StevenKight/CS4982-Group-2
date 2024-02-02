using API.Controllers;
using API.Dal;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Tests.Controllers;

[TestFixture]
public class SourceControllerTests
{
    #region Data members

    private readonly List<Source> _sources = new()
    {
        new Source
        {
            SourceId = 1, Name = "testSource", IsLink = true, Link = "testUrl", Type = "pdf", Username = "testUser"
        },
        new Source
        {
            SourceId = 2, Name = "testSource2", IsLink = true, Link = "testUrl2", Type = "pdf", Username = "testUser"
        }
    };

    private DbContextOptions<DocunotesDbContext> _options;
    private DocunotesDbContext _context;

    private SourceDal _sourceDal;

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

        this._context.Sources.AddRange(this._sources);
        this._context.SaveChanges();

        this._sourceDal = new SourceDal(this._context);
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
        var sourceController = new SourceController(this._sourceDal);

        // Assert
        Assert.IsNotNull(sourceController);
    }

    [Test]
    [Order(2)]
    public void GetAllTest()
    {
        // Arrange
        var sourceController = new SourceController(this._sourceDal);

        // Act
        var result = sourceController.GetAll() as OkObjectResult;
        var resultList = result.Value as IEnumerable<Source>;

        var expected = this._sources.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.IsNotNull(resultList);
        Assert.AreEqual(expected, resultList.Count());

        for (var i = 0; i < expected; i++)
        {
            var source = this._sources[i];
            var actual = resultList.ElementAt(i);
            Assert.AreEqual(source.SourceId, actual.SourceId);
            Assert.AreEqual(source.Username, actual.Username);
            Assert.AreEqual(source.Type, actual.Type);
            Assert.AreEqual(source.NoteType, actual.NoteType);
            Assert.AreEqual(source.Name, actual.Name);
            Assert.AreEqual(source.IsLink, actual.IsLink);
            Assert.AreEqual(source.Link, actual.Link);
        }
    }

    [Test]
    [Order(3)]
    public void GetByIdTest()
    {
        // Arrange
        var sourceController = new SourceController(this._sourceDal);

        // Act
        var result = sourceController.GetById(1) as OkObjectResult;
        var resultObject = result.Value as Source;

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.IsNotNull(resultObject);
        Assert.AreEqual(1, resultObject.SourceId);
        Assert.AreEqual("testSource", resultObject.Name);
    }

    [Test]
    [Order(4)]
    public void CreateTest()
    {
        // Arrange
        var sourceController = new SourceController(this._sourceDal);
        var source = new Source
        {
            SourceId = 3,
            Name = "testSource3",
            IsLink = true,
            Link = "testUrl3",
            Type = "pdf",
            Username = "testUser"
        };

        // Act
        var result = sourceController.Create(source);

        this._sources.Add(source);
        var expected = this._sources.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkResult>(result);
        Assert.AreEqual(expected, this._context.Sources.Count());
        Assert.Contains(source, this._context.Sources.ToList());
    }

    [Test]
    [Order(5)]
    public void UpdateTest()
    {
        // Arrange
        var sourceController = new SourceController(this._sourceDal);
        var source = new Source
        {
            SourceId = 2,
            Name = "testSource4",
            IsLink = true,
            Link = "testUrl4",
            Type = "pdf",
            Username = "testUser"
        };

        // Act
        var result = sourceController.Update(source);

        this._sources.Add(source);
        this._sources.Remove(this._sources[1]);

        // Assert
        var actual = this._context.Sources.Find(2);
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkResult>(result);
        Assert.AreEqual(source.Name, actual.Name);
        Assert.AreEqual(source.Link, actual.Link);
        Assert.AreEqual(source.Type, actual.Type);
        Assert.AreEqual(source.Username, actual.Username);
    }

    [Test]
    [Order(6)]
    public void DeleteTest()
    {
        // Arrange
        var sourceController = new SourceController(this._sourceDal);

        // Act
        var result = sourceController.Delete(this._sources[0]);

        this._sources.Remove(this._sources[0]);
        var expected = this._sources.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkResult>(result);
        Assert.AreEqual(expected, this._context.Sources.Count());

        var sources = this._context.Sources.ToList();
        for (var i = 0; i < expected; i++)
        {
            Assert.IsTrue(this._sources.Exists(x => sources.ElementAt(i).Name.Equals(x.Name)));
        }
    }

    #endregion
}