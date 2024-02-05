using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.UnitTests.Dal;

[TestFixture]
public class SourceDalTests
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

        this._context.Sources.AddRange(this._sources);
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
        var sourceDal = new SourceDal(this._context);

        // Assert
        Assert.IsNotNull(sourceDal);
    }

    [Test]
    [Order(2)]
    public void GetSourceByIdTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);

        // Act
        var result = sourceDal.Get(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.SourceId);
        Assert.AreEqual("testSource", result.Name);
    }

    [Test]
    [Order(3)]
    public void GetAllSourceTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);

        // Act
        var result = sourceDal.GetAll();

        var expected = this._sources.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());

        for (var i = 0; i < expected; i++)
        {
            var source = this._sources[i];
            var actual = result.ElementAt(i);
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
    [Order(4)]
    public void AddSourceTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);
        var source = new Source
        {
            SourceId = 3, Name = "testSource3", IsLink = true, Link = "testUrl3", Type = "pdf", Username = "testUser"
        };

        // Act
        var result = sourceDal.Add(source);

        this._sources.Add(source);
        var expected = this._sources.Count();

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, this._context.Sources.Count());
        Assert.Contains(source, this._context.Sources.ToList());
    }

    [Test]
    [Order(5)]
    public void UpdateSourceTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);
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
        var result = sourceDal.Update(source);

        this._sources.Add(source);
        this._sources.Remove(this._sources[1]);

        // Assert
        var actual = this._context.Sources.Find(2);
        Assert.IsTrue(result);
        Assert.AreEqual(source.Name, actual.Name);
        Assert.AreEqual(source.Link, actual.Link);
        Assert.AreEqual(source.Type, actual.Type);
        Assert.AreEqual(source.Username, actual.Username);
    }

    [Test]
    [Order(6)]
    public void DeleteSourceTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);

        // Act
        var result = sourceDal.Delete(this._sources[0]);

        this._sources.Remove(this._sources[0]);
        var expected = this._sources.Count();

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, this._context.Sources.Count());

        var sources = this._context.Sources.ToList();
        for (var i = 0; i < expected; i++)
        {
            Assert.IsTrue(this._sources.Exists(x => sources.ElementAt(i).Name.Equals(x.Name)));
        }
    }

    #endregion
}