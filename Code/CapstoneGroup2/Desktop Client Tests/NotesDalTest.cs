using System.Text;
using System.Text.Json;
using Desktop_Client.Dal;
using Desktop_Client.Model;
using Desktop_Client_Tests.Mocks;
using Moq;


namespace Desktop_Client_Tests
{
    [TestFixture]
    public class NotesDalTests
    {
        private NotesDal notesDal;
        private Mock<HttpClientWrapper> fakeHttpClient;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
            fakeHttpClient = new Mock<HttpClientWrapper>();
            notesDal = new NotesDal(new HttpClientWrapper(_httpClient));
        }

        [Test]
        public async Task GetUsersNotesAsync_Successful()
        {
            // Arrange
            var fakeNotes = new List<Note> { /* Create sample notes */ };
            if (fakeNotes != null)
            {
                    fakeHttpClient.Setup(client => client.GetAsync("Notes"))
                        .ReturnsAsync(new HttpResponseMessage
                        {
                            StatusCode = System.Net.HttpStatusCode.OK,
                            Content = new StringContent(JsonSerializer.Serialize(fakeNotes), Encoding.UTF8,
                                "application/json")
                        });
                    notesDal = new NotesDal(fakeHttpClient.Object);
                    // Act
                    var result = await notesDal.GetUsersNotesAsync();

                    // Assert
                    CollectionAssert.AreEqual(fakeNotes, result);
            }
        }

        [Test]
        public async Task createNewNote_Successful()
        {
            // Arrange
            var fakeSource = new Source
            {
                SourceId = 1,
                Type = "Pdf",
                Name = "FakeSource",
                IsLink = false,
                Link = "http://example.com"
            };
            var fakeNote = new Note
            {
                SourceId = 1,
                Username = "FakeUser",
                NoteText = "This is a fake note.",
                TagsString = "tag1,tag2,tag3",
                Source = fakeSource
            };
            var fakeAuthToken = "fakeToken";

            fakeHttpClient.Setup(client => client.PostAsync("Notes", It.IsAny<StringContent>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                });
            notesDal = new NotesDal(fakeHttpClient.Object);
            // Act
            var result = await notesDal.createNewNote(fakeNote, fakeAuthToken);

            // Assert
            Assert.IsTrue(result as bool?);
        }

        [Test]
        public async Task UpdateNote_Successful()
        {
            // Arrange
            var fakeSource = new Source
            {
                SourceId = 1,
                Type = "Pdf",
                Name = "FakeSource",
                IsLink = false,
                Link = "http://example.com"
            };
            var fakeNote = new Note
            {
                SourceId = 1,
                Username = "FakeUser",
                NoteText = "This is a fake note.",
                TagsString = "tag1,tag2,tag3",
                Source = fakeSource
            };
            var fakeAuthToken = "fakeToken";

            fakeHttpClient.Setup(client => client.PutAsync("Notes", It.IsAny<StringContent>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                });
            notesDal = new NotesDal(fakeHttpClient.Object);
            // Act
            var result = await notesDal.UpdateNote(fakeNote, fakeAuthToken);

            // Assert
            Assert.IsTrue(result as bool?);
        }

        [Test]
        public async Task DeleteNote_Successful()
        {
            // Arrange
            var fakeSource = new Source
            {
                SourceId = 1,
                Type = "Pdf",
                Name = "FakeSource",
                IsLink = false,
                Link = "http://example.com"
            };
            var fakeNote = new Note
            {
                SourceId = 1,
                Username = "FakeUser",
                NoteText = "This is a fake note.",
                TagsString = "tag1,tag2,tag3",
                Source = fakeSource
            };
            var fakeAuthToken = "fakeToken";

            fakeHttpClient.Setup(client => client.DeleteAsync("Notes"))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                });
            notesDal = new NotesDal(fakeHttpClient.Object);
            // Act
            var result = await notesDal.DeleteNote(fakeNote, fakeAuthToken);

            // Assert
            Assert.IsTrue(result as bool?);
        }
    }
}
