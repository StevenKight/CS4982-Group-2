using CapstoneGroup2.Server.Controllers;
using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.UnitTests.Controllers;

/* dotcover disable */
[TestFixture]
public class NotesControllerTests
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
            TagsString = "testTag1,testTag2",
            NoteDate = new DateTime(2021, 1, 1)
        },

        new Note
        {
            NoteId = 2,
            SourceId = 1,
            Username = "testUser",
            NoteText = "testNote2",
            TagsString = "testTag3,testTag4",
            NoteDate = new DateTime(2021, 1, 1)
        }
    ];

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
    public void NullUsernameGetAllTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);

        // Act
        var result = notesController.GetAll(1, null);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
    }

    [Test]
    [Order(4)]
    public void EmptyUsernameGetAllTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);

        // Act
        var result = notesController.GetAll(1, "");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
    }

    [Test]
    [Order(5)]
    public void WhitespaceUsernameGetAllTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);

        // Act
        var result = notesController.GetAll(1, "    ");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
    }

    [Test]
    [Order(6)]
    public void InvalidSourceIdGetAllTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);

        // Act
        var result = notesController.GetAll(0, "testUser");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    [Order(7)]
    public void CreateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);
        var note = new Note
        {
            NoteId = 3,
            SourceId = 3,
            Username = "testUser",
            NoteText = "testNote3",
            TagsString = "testTag5,testTag6",
            NoteDate = new DateTime(2021, 1, 1)
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
    [Order(8)]
    public void NullUsernameCreateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);
        var note = new Note
        {
            NoteId = 3,
            SourceId = 3,
            Username = "testUser",
            NoteText = "testNote3",
            TagsString = "testTag5,testTag6",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act
        var result = notesController.Create(null, note);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
    }

    [Test]
    [Order(9)]
    public void EmptyUsernameCreateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);
        var note = new Note
        {
            NoteId = 3,
            SourceId = 3,
            Username = "testUser",
            NoteText = "testNote3",
            TagsString = "testTag5,testTag6",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act
        var result = notesController.Create("", note);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
    }

    [Test]
    [Order(10)]
    public void WhitespaceUsernameCreateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);
        var note = new Note
        {
            NoteId = 3,
            SourceId = 3,
            Username = "testUser",
            NoteText = "testNote3",
            TagsString = "testTag5,testTag6",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act
        var result = notesController.Create("    ", note);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
    }

    [Test]
    [Order(11)]
    public void NullNoteCreateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);

        // Act
        var result = notesController.Create("testUser", null);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    [Order(12)]
    public void DuplicateNoteCreateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);
        var note = new Note
        {
            NoteId = 1,
            SourceId = 1,
            Username = "testUser",
            NoteText = "testNote",
            TagsString = "testTag1,testTag2",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act
        var result = notesController.Create("testUser", note);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    [Order(13)]
    public void UpdateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);
        var note = new Note
        {
            NoteId = 2,
            SourceId = 3,
            Username = "testUser",
            NoteText = "testNote4",
            TagsString = "testTag5,testTag6",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act
        var result = notesController.Update("testUser", note);

        this._notes.Add(note);
        this._notes.Remove(this._notes[1]);

        // Assert
        var actual = this._context.Notes.Find(2);
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkResult>(result);
        Assert.AreEqual(note.SourceId, actual.SourceId);
        Assert.AreEqual(note.Username, actual.Username);
        Assert.AreEqual(note.NoteText, actual.NoteText);
        Assert.AreEqual(note.TagsString, actual.TagsString);
    }

    [Test]
    [Order(13)]
    public void NullUsernameUpdateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);
        var note = new Note
        {
            NoteId = 3,
            SourceId = 3,
            Username = "testUser",
            NoteText = "testNote3",
            TagsString = "testTag5,testTag6",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act
        var result = notesController.Update(null, note);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
    }

    [Test]
    [Order(14)]
    public void EmptyUsernameUpdateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);
        var note = new Note
        {
            NoteId = 3,
            SourceId = 3,
            Username = "testUser",
            NoteText = "testNote3",
            TagsString = "testTag5,testTag6",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act
        var result = notesController.Update("", note);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
    }

    [Test]
    [Order(15)]
    public void WhitespaceUsernameUpdateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);
        var note = new Note
        {
            NoteId = 3,
            SourceId = 3,
            Username = "testUser",
            NoteText = "testNote3",
            TagsString = "testTag5,testTag6",
            NoteDate = new DateTime(2021, 1, 1)
        };

        // Act
        var result = notesController.Update("    ", note);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
    }

    [Test]
    [Order(16)]
    public void NullNoteUpdateTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);

        // Act
        var result = notesController.Update("testUser", null);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    [Order(17)]
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

    [Test]
    [Order(18)]
    public void InvalidNoteIdDeleteTest()
    {
        // Arrange
        var notesController = new NotesController(this._notesDal);

        // Act
        var result = notesController.Delete(0);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    #endregion
}