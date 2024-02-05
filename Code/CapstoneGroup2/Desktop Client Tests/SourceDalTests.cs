using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using Desktop_Client.Dal;
using Desktop_Client.Model;
using Desktop_Client_Tests.Mocks;
using Moq;

namespace Desktop_Client_Tests
{
    [TestFixture]
    public class SourceDalTests
    {
        private SourceDal sourceDal;
        private Mock<HttpClientWrapper> fakeHttpClient;
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient();
            fakeHttpClient = new Mock<HttpClientWrapper>();
            sourceDal = new SourceDal(new HttpClientWrapper(_httpClient));
        }

        [Test]
        public void GetSourcesForUser_Successful()
        {
            // Arrange
            var fakeUser = new User { Username = "testuser" };
            var fakeSources = new List<Source>();
            var fakeSource = new Source
            {
                SourceId = 1,
                Type = "Pdf",
                Name = "FakeSource",
                IsLink = false,
                Link = "http://example.com"
            };
            fakeSources.Add(fakeSource);
            if (fakeSources != null)
            {
                fakeHttpClient.Setup(client => client.GetAsync($"Source/{fakeUser.Username}"))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Content = new StringContent(JsonSerializer.Serialize(fakeSources))
                    });
                sourceDal = new SourceDal(fakeHttpClient.Object);
                // Act
                var result = sourceDal.getSourcesForUser(fakeUser).Result;

                // Assert
                Assert.IsTrue(result[0].Name.Equals(fakeSource.Name));
            }
        }

        [Test]
        public void GetSources_Successful()
        {
            // Arrange
            var fakeSources = new List<Source>();
            var fakeSource = new Source
            {
                SourceId = 1,
                Type = "Pdf",
                Name = "FakeSource",
                IsLink = false,
                Link = "http://example.com"
            };
            fakeSources.Add(fakeSource);

            fakeHttpClient.Setup(client => client.GetAsync("Source"))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(fakeSources))
                });
            sourceDal = new SourceDal(fakeHttpClient.Object);
            // Act
            var result = sourceDal.getSources().Result;

            // Assert
            Assert.IsTrue(result[0].Name.Equals(fakeSource.Name));
        }

        [Test]
        public void SendPdfToServer_Successful()
        {
            // Arrange
            var fakeUser = new User { Username = "testuser" };
            var fakePdfBytes = new byte[] { /* Create sample PDF bytes */ };

            fakeHttpClient.Setup(client => client.PostAsync($"Source/{fakeUser.Username}", It.IsAny<ByteArrayContent>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                });
            sourceDal = new SourceDal(fakeHttpClient.Object);
            // Act
            var result  = sourceDal.SendPdfToServer(fakeUser, fakePdfBytes).Result;

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void DownloadFileAsync_Successful()
        {
            // Arrange
            var fakeSourceId = 123;
            var fakeFilePath = "fakePath";
            var fakeFileContent = "Fake file content";
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(fakeFileContent));

            fakeHttpClient.Setup(client => client.GetAsync($"Source/{fakeSourceId}"))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StreamContent(memoryStream)
                });
            sourceDal = new SourceDal(fakeHttpClient.Object);
            // Act
            var result = sourceDal.DownloadFileAsync(fakeSourceId, fakeFilePath).Result;


            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void DownloadFileAsync_Unsuccessful()
        {
            // Arrange
            var fakeSourceId = 123;
            var fakeFilePath = "fakePath";

            fakeHttpClient.Setup(client => client.GetAsync($"Source/{fakeSourceId}"))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Content = new StringContent("Fake file content"),
                });
            sourceDal = new SourceDal(fakeHttpClient.Object);
            // Act
            var result = sourceDal.DownloadFileAsync(fakeSourceId, fakeFilePath).Result;


            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetSourcesForUser_Unsuccessful()
        {
            // Arrange
            var fakeUser = new User { Username = "testuser" };

            var fakeSources = new List<Source>();
            var fakeSource = new Source
            {
                SourceId = 1,
                Type = "Pdf",
                Name = "FakeSource",
                IsLink = false,
                Link = "http://example.com"
            };
            fakeSources.Add(fakeSource);
            if (fakeSources != null)
            {
                fakeHttpClient.Setup(client => client.GetAsync($"Source/{fakeUser.Username}"))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Content = null
                    });
                sourceDal = new SourceDal(fakeHttpClient.Object);
                // Act
                var result = sourceDal.getSourcesForUser(fakeUser).Result;

                // Assert
                Assert.IsNull(result);
            }
        }

        [Test]
        public void GetSources_Unsuccessful()
        {
            // Arrange
            fakeHttpClient.Setup(client => client.GetAsync("Source"))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });
            sourceDal = new SourceDal(fakeHttpClient.Object);
            // Act
            var result = sourceDal.getSources().Result;

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void SendPdfToServer_Unsuccessful()
        {
            // Arrange
            var fakeUser = new User { Username = "testuser" };
            var fakePdfBytes = new byte[] { /* Create sample PDF bytes */ };

            fakeHttpClient.Setup(client => client.PostAsync("Source", It.IsAny<ByteArrayContent>()))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });
            sourceDal = new SourceDal(fakeHttpClient.Object);
            // Act
            var result =  sourceDal.SendPdfToServer(fakeUser, fakePdfBytes).Result;

            // Assert
            Assert.IsFalse(result);
        }
    }
}
