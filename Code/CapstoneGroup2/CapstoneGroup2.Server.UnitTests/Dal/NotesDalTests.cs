using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.UnitTests.Dal;

/* dotcover disable */
[TestFixture]
public class NotesDalTests
{
    #region Data members

    private readonly List<Note> _notes =
    [
        new Note
        {
            NoteId = 1,
            SourceId = 1,
            Username = "testUser",
            NoteText = "testNote",
            NoteDate = new DateTime(2021, 1, 1)
        },

        new Note
        {
            NoteId = 2,
            SourceId = 1,
            Username = "testUser",
            NoteText = "testNote2",
            NoteDate = new DateTime(2021, 1, 1)
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

        this._context.Notes.AddRange(this._notes);
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
        var noteDal = new NotesDal(this._context);

        // Assert
        Assert.IsNotNull(noteDal);
    }

    [Test]
    [Order(2)]
    public void GetNoteByIdTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        // Act
        var result = noteDal.Get(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.SourceId);
        Assert.AreEqual("testUser", result.Username);
        Assert.AreEqual("testNote", result.NoteText);
    }

    [Test]
    [Order(3)]
    public void InvalidNumberOfKeysGetNoteByIdTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        // Act, Assert
        Assert.Throws<InvalidCastException>(() => noteDal.Get(1, "test"));
    }

    [Test]
    [Order(4)]
    public void InvalidNullKeyGetNoteByIdTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        // Act, Assert
        Assert.Throws<InvalidCastException>(() => noteDal.Get(null));
    }

    [Test]
    [Order(5)]
    public void InvalidNonIntKeyGetNoteByIdTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        // Act, Assert
        Assert.Throws<InvalidCastException>(() => noteDal.Get("test"));
    }

    [Test]
    [Order(6)]
    public void InvalidZeroKeyGetNoteByIdTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        // Act, Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => noteDal.Get(0));
    }

    [Test]
    [Order(7)]
    public void InvalidNonExistentKeyGetNoteByIdTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        // Act, Assert
        Assert.Throws<InvalidOperationException>(() => noteDal.Get(10));
    }

    [Test]
    [Order(8)]
    public void GetAllNoteTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };
        noteDal.SetSourceId(1);

        // Act
        var result = noteDal.GetAll();

        var expected = this._notes.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());

        for (var i = 0; i < expected; i++)
        {
            var note = this._notes[i];
            var actual = result.ElementAt(i);
            Assert.AreEqual(note.SourceId, actual.SourceId);
            Assert.AreEqual(note.Username, actual.Username);
            Assert.AreEqual(note.NoteText, actual.NoteText);
        }
    }

    [Test]
    [Order(8)]
    public void InvalidSourceIdGetAllNoteTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };
        noteDal.SetSourceId(0);

        // Act, Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => noteDal.GetAll());
    }

    [Test]
    [Order(9)]
    public void AddNoteTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };
        var note = new Note
        {
            NoteId = 3,
            SourceId = 3,
            Username = "testUser",
            NoteText = "testNote3",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act
        var result = noteDal.Add(note);

        this._notes.Add(note);
        var expected = this._notes.Count();

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, this._context.Notes.Count());
        Assert.Contains(note, this._context.Notes.ToList());
    }

    [Test]
    [Order(10)]
    public void UpdateNoteTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };
        var note = new Note
        {
            NoteId = 2,
            SourceId = 3,
            Username = "testUser",
            NoteText = "testNote4",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act
        var result = noteDal.Update(note);

        this._notes.Add(note);
        this._notes.Remove(this._notes[1]);

        // Assert
        var actual = this._context.Notes.Find(2);
        Assert.IsTrue(result);
        Assert.AreEqual(note.SourceId, actual.SourceId);
        Assert.AreEqual(note.Username, actual.Username);
        Assert.AreEqual(note.NoteText, actual.NoteText);
    }

    [Test]
    [Order(11)]
    public void InvalidUserInvalidUpdateNoteTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser2", Password = "testPassword" };
        var note = new Note
        {
            NoteId = 2,
            SourceId = 3,
            Username = "testUser",
            NoteText = "testNote4",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act, Assert
        Assert.Throws<UnauthorizedAccessException>(() => noteDal.Update(note));
    }

    [Test]
    [Order(12)]
    public void NoUserInvalidUpdateNoteTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        var note = new Note
        {
            NoteId = 2,
            SourceId = 3,
            Username = "testUser",
            NoteText = "testNote4",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act, Assert
        Assert.Throws<UnauthorizedAccessException>(() => noteDal.Update(note));
    }

    [Test]
    [Order(13)]
    public void DeleteNoteTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };

        // Act
        var result = noteDal.Delete(this._notes[0]);

        this._notes.Remove(this._notes[0]);
        var expected = this._notes.Count();

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, this._context.Notes.Count());

        var notes = this._context.Notes.ToList();
        for (var i = 0; i < expected; i++)
        {
            Assert.IsTrue(this._notes.Exists(
                x => notes.ElementAt(i).SourceId == x.SourceId && notes.ElementAt(i).Username.Equals(x.Username)));
        }
    }

    #endregion
}