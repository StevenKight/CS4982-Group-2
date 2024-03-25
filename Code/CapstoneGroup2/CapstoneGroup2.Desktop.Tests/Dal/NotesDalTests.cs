using System.Net;
using System.Net.Http.Formatting;
using CapstoneGroup2.Desktop.Library.Dal;
using CapstoneGroup2.Desktop.Library.Mocks;
using CapstoneGroup2.Desktop.Library.Model;
using Moq;

namespace CapstoneGroup2.Desktop.Tests.Dal;

[TestFixture]
public class NotesDalTests
{
    #region Methods

    [Test]
    public void ValidConstructor()
    {
        // Arrange, Act
        var notesDal = new NotesDal();

        // Assert
        Assert.IsNotNull(notesDal);
    }

    [Test]
    public async Task GetUserSourceNotes_ValidInput_ReturnsNotes()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var source = new Source { SourceId = 1, Username = "JohnDoe", Type = "Pdf" };

        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new ObjectContent<IEnumerable<Note>>(
                new List<Note>
                    { new() { NoteId = 1, Username = "JohnDoe", NoteText = "Test note" } },
                new JsonMediaTypeFormatter())
        };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.GetAsync($"/Notes/{source.SourceId}-{user.Username}"))
            .ReturnsAsync(expectedResponse);

        var notesDal = new NotesDal(httpClientMock.Object);

        // Act
        var result = await notesDal.GetUserSourceNotes(user, source);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("JohnDoe", result.First().Username);
        Assert.AreEqual("Test note", result.First().NoteText);
        httpClientMock.Verify(x => x.GetAsync($"/Notes/{source.SourceId}-{user.Username}"), Times.Once);
    }

    [Test]
    public async Task GetUserSourceNotes_InvalidInput_ReturnsNull()
    {
        // Arrange
        var user = new User { Username = "", Password = "" };
        var source = new Source { SourceId = 1, Username = "", Type = "" };

        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest
        };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.GetAsync($"/Notes/{source.SourceId}-{user.Username}"))
            .ReturnsAsync(expectedResponse);

        var notesDal = new NotesDal(httpClientMock.Object);

        // Act
        var result = await notesDal.GetUserSourceNotes(user, source);

        // Assert
        Assert.IsNull(result);
        httpClientMock.Verify(x => x.GetAsync($"/Notes/{source.SourceId}-{user.Username}"), Times.Once);
    }

    [Test]
    public async Task createNewNote_ValidInput_ReturnsTrue()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var note = new Note { NoteId = 1, NoteText = "New test note" };

        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.PostAsync($"/Notes/{user.Username}", It.IsAny<StringContent>()))
            .ReturnsAsync(expectedResponse);

        var notesDal = new NotesDal(httpClientMock.Object);

        // Act
        var result = await notesDal.createNewNote(user, note);

        // Assert
        Assert.IsTrue(result);
        httpClientMock.Verify(x => x.PostAsync($"/Notes/{user.Username}", It.IsAny<StringContent>()), Times.Once);
    }

    [Test]
    public async Task UpdateNote_ValidInput_ReturnsTrue()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var note = new Note { NoteId = 1, NoteText = "Updated test note" };

        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.PutAsync($"/Notes/{user.Username}", It.IsAny<StringContent>()))
            .ReturnsAsync(expectedResponse);

        var notesDal = new NotesDal(httpClientMock.Object);

        // Act
        var result = await notesDal.UpdateNote(note, user);

        // Assert
        Assert.IsTrue(result);
        httpClientMock.Verify(x => x.PutAsync($"/Notes/{user.Username}", It.IsAny<StringContent>()), Times.Once);
    }

    [Test]
    public async Task DeleteNote_ValidInput_ReturnsTrue()
    {
        // Arrange
        var note = new Note { NoteId = 1, NoteText = "Test note to delete" };

        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.DeleteAsync($"/Notes/{note.NoteId}")).ReturnsAsync(expectedResponse);

        var notesDal = new NotesDal(httpClientMock.Object);

        // Act
        var result = await notesDal.DeleteNote(note);

        // Assert
        Assert.IsTrue(result);
        httpClientMock.Verify(x => x.DeleteAsync($"/Notes/{note.NoteId}"), Times.Once);
    }

    #endregion
}