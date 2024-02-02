using API.Dal;
using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API_Tests.Dal;

[TestFixture]
public class NotesDalTests
{
    #region Data members

    private readonly List<Note> _notes = new()
    {
        new Note
        {
            SourceId = 1, Username = "testUser", NoteText = "testNote", TagsString = "testTag1,testTag2"
        },
        new Note
        {
            SourceId = 2, Username = "testUser", NoteText = "testNote2", TagsString = "testTag3,testTag4"
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

        // Act
        var result = noteDal.Get(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.SourceId);
        Assert.AreEqual("testUser", result.Username);
        Assert.AreEqual("testNote", result.NoteText);
        Assert.AreEqual("testTag1,testTag2", result.TagsString);
    }

    [Test]
    [Order(3)]
    public void GetAllNoteTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);

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
            Assert.AreEqual(note.TagsString, actual.TagsString);
        }
    }

    [Test]
    [Order(4)]
    public void AddNoteTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        var note = new Note
        {
            SourceId = 3,
            Username = "testUser",
            NoteText = "newTestNote",
            TagsString = "testTag1,testTag6"
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
    [Order(5)]
    public void UpdateNoteTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);
        var note = new Note
        {
            SourceId = 2,
            Username = "testUser",
            NoteText = "testNoteUpdated",
            TagsString = "testTag1,testTag2,newTag"
        };

        // Act
        var result = noteDal.Update(note);

        this._notes.Add(note);
        this._notes.Remove(this._notes[1]);

        // Assert
        var actual = this._context.Notes.Find(2, "testUser");
        Assert.IsTrue(result);
        Assert.AreEqual(note.SourceId, actual.SourceId);
        Assert.AreEqual(note.Username, actual.Username);
        Assert.AreEqual(note.NoteText, actual.NoteText);
        Assert.AreEqual(note.TagsString, actual.TagsString);
    }

    [Test]
    [Order(6)]
    public void DeleteNoteTest()
    {
        // Arrange
        var noteDal = new NotesDal(this._context);

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