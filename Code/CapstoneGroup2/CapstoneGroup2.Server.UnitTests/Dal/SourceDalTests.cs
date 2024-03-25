using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.UnitTests.Dal;

/* dotcover disable */
[TestFixture]
public class SourceDalTests
{
    #region Data members

    private readonly List<Source> _sources =
    [
        new Source
        {
            SourceId = 1,
            Name = "testSource",
            Description = "testDescription",
            IsLink = true,
            Link = "testUrl",
            Type = "pdf",
            Username = "testUser",
            CreatedAt = new DateTime(2021, 1, 1),
            UpdatedAt = new DateTime(2021, 1, 2)
        },

        new Source
        {
            SourceId = 2,
            Name = "testSource2",
            Description = "testDescription2",
            IsLink = true,
            Link = "testUrl2",
            Type = "pdf",
            Username = "testUser",
            CreatedAt = new DateTime(2021, 1, 1),
            UpdatedAt = new DateTime(2021, 1, 2)
        }
    ];

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
    public void InvalidNumberOfKeysGetNoteByIdTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        // Act, Assert
        Assert.Throws<InvalidCastException>(() => sourceDal.Get(1, "test"));
    }

    [Test]
    [Order(4)]
    public void InvalidNullKeyGetNoteByIdTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        // Act, Assert
        Assert.Throws<InvalidCastException>(() => sourceDal.Get(null));
    }

    [Test]
    [Order(5)]
    public void InvalidNonIntKeyGetNoteByIdTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        // Act, Assert
        Assert.Throws<InvalidCastException>(() => sourceDal.Get("test"));
    }

    [Test]
    [Order(6)]
    public void InvalidZeroKeyGetNoteByIdTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        // Act, Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => sourceDal.Get(0));
    }

    [Test]
    [Order(7)]
    public void InvalidNonExistentKeyGetNoteByIdTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        // Act, Assert
        Assert.Throws<InvalidOperationException>(() => sourceDal.Get(10));
    }

    [Test]
    [Order(8)]
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
    [Order(9)]
    public void AddSourceTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);
        var source = new Source
        {
            SourceId = 3,
            Name = "testSource3",
            Description = "testDescription3",
            IsLink = true,
            Link = "testUrl3",
            Type = "pdf",
            Username = "testUser",
            CreatedAt = new DateTime(2021, 1, 1),
            UpdatedAt = new DateTime(2021, 1, 2)
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
    [Order(10)]
    public void UpdateSourceTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);
        var source = new Source
        {
            SourceId = 2,
            Name = "testSource4",
            Description = "testDescription4",
            IsLink = true,
            Link = "testUrl4",
            Type = "pdf",
            Username = "testUser",
            CreatedAt = new DateTime(2021, 1, 1),
            UpdatedAt = new DateTime(2021, 1, 2)
        };

        // Act
        var result = sourceDal.Update(source);

        this._sources.Add(source);
        this._sources.Remove(this._sources[1]);

        // Assert
        var actual = this._context.Sources.Find(2);
        Assert.IsTrue(result);
        Assert.AreEqual(source.Name, actual.Name);
        Assert.AreEqual(source.Description, actual.Description);
        Assert.AreEqual(source.Link, actual.Link);
        Assert.AreEqual(source.Type, actual.Type);
        Assert.AreEqual(source.Username, actual.Username);
    }

    [Test]
    [Order(11)]
    public void InvalidUserInvalidUpdateNoteTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser2", Password = "testPassword" };
        var source = new Source
        {
            SourceId = 2,
            Name = "testSource4",
            Description = "testDescription4",
            IsLink = true,
            Link = "testUrl4",
            Type = "pdf",
            Username = "testUser",
            CreatedAt = new DateTime(2021, 1, 1),
            UpdatedAt = new DateTime(2021, 1, 2)
        };

        // Act, Assert
        Assert.Throws<UnauthorizedAccessException>(() => sourceDal.Update(source));
    }

    [Test]
    [Order(12)]
    public void NoUserInvalidUpdateNoteTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);
        var source = new Source
        {
            SourceId = 2,
            Name = "testSource4",
            Description = "testDescription4",
            IsLink = true,
            Link = "testUrl4",
            Type = "pdf",
            Username = "testUser",
            CreatedAt = new DateTime(2021, 1, 1),
            UpdatedAt = new DateTime(2021, 1, 2)
        };

        // Act, Assert
        Assert.Throws<UnauthorizedAccessException>(() => sourceDal.Update(source));
    }

    [Test]
    [Order(13)]
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

    [Test]
    [Order(14)]
    public void DeleteSourceCascadeTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);
        var noteDal = new NotesDal(this._context);
        var note = new Note
        {
            NoteId = 1,
            SourceId = 1,
            Username = "testUser",
            NoteText = "testNote",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act
        var result = sourceDal.Delete(this._sources[0]);

        this._sources.Remove(this._sources[0]);
        var expected = this._sources.Count();

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, this._context.Sources.Count());
        Assert.AreEqual(0, this._context.Notes.Count());
    }

    [Test]
    [Order(15)]
    public void InvalidSourceIdDeleteTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);

        // Act, Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => sourceDal.Delete(new Source { SourceId = 0 }));
    }

    [Test]
    [Order(16)]
    public void SetUserTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);

        // Act
        sourceDal.SetUser("testUser");

        // Assert
        Assert.AreEqual("testUser", this._context.CurrentUser?.Username);
    }

    [Test]
    [Order(17)]
    public void InvalidSetSourceIdTest()
    {
        // Arrange
        var sourceDal = new SourceDal(this._context);

        // Act, Assert
        Assert.Throws<InvalidOperationException>(() => sourceDal.SetSourceId(1));
    }

    #endregion
}