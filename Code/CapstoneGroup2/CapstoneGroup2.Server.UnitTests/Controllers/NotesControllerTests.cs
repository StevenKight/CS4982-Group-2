using CapstoneGroup2.Server.Controllers;
using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.UnitTests.Controllers;

[TestFixture]
public class NotesControllerTests
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

    private NotesDal _notesDal;

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

        this._context.Notes.AddRange(this._notes);
        this._context.SaveChanges();

        this._notesDal = new NotesDal(this._context);
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
        var notesController = new NotesController(this._notesDal);

        // Assert
        Assert.IsNotNull(notesController);
    }

    [Test]
    [Order(2)]
    public void GetAllTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);

        // Act
        var result = notesController.GetAll(1, "testUser") as OkObjectResult;
        var resultList = result.Value as IEnumerable<Note>;

        var expected = this._notes.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.IsNotNull(resultList);
        Assert.AreEqual(expected, resultList.Count());

        for (var i = 0; i < expected; i++)
        {
            var note = this._notes[i];
            var actual = resultList.ElementAt(i);
            Assert.AreEqual(note.SourceId, actual.SourceId);
            Assert.AreEqual(note.Username, actual.Username);
            Assert.AreEqual(note.NoteText, actual.NoteText);
            Assert.AreEqual(note.TagsString, actual.TagsString);
        }
    }

    [Test]
    [Order(3)]
    public void CreateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);
        var note = new Note
        {
            SourceId = 3,
            Username = "testUser",
            NoteText = "newTestNote",
            TagsString = "testTag1,testTag6"
        };

        // Act
        var result = notesController.Create("testUser", note);

        this._notes.Add(note);
        var expected = this._notes.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkResult>(result);
        Assert.AreEqual(expected, this._context.Notes.Count());
        Assert.Contains(note, this._context.Notes.ToList());
    }

    [Test]
    [Order(4)]
    public void UpdateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);
        var note = new Note
        {
            SourceId = 2,
            Username = "testUser",
            NoteText = "testNoteUpdated",
            TagsString = "testTag1,testTag2,newTag"
        };

        // Act
        var result = notesController.Update("testUser", note);

        this._notes.Add(note);
        this._notes.Remove(this._notes[1]);

        // Assert
        var actual = this._context.Notes.Find(2, "testUser");
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkResult>(result);
        Assert.AreEqual(note.SourceId, actual.SourceId);
        Assert.AreEqual(note.Username, actual.Username);
        Assert.AreEqual(note.NoteText, actual.NoteText);
        Assert.AreEqual(note.TagsString, actual.TagsString);
    }

    [Test]
    [Order(5)]
    public void DeleteTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);

        // Act
        var result = notesController.Delete(this._notes[0].NoteId);

        this._notes.Remove(this._notes[0]);
        var expected = this._notes.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkResult>(result);
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