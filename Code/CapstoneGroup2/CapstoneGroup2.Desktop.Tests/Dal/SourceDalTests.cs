using CapstoneGroup2.Desktop.Dal;
using CapstoneGroup2.Desktop.Library.Mocks;
using CapstoneGroup2.Desktop.Library.Model;
using Moq;

namespace CapstoneGroup2.Desktop.Tests.Dal;

[TestFixture]
public class SourceDalTests
{
    [Test]
    public async Task GetSourcesForUser_ValidInput_ReturnsSources()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };

        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = new ObjectContent<IEnumerable<Source>>(
                new List<Source> { new Source { SourceId = 1, Username = "JohnDoe", Name = "Test Source", Type = "pdf" } },
                new System.Net.Http.Formatting.JsonMediaTypeFormatter())
        };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.GetAsync($"/Source/{user.Username}")).ReturnsAsync(expectedResponse);

        var sourceDal = new SourceDal(httpClientMock.Object);

        // Act
        var result = await sourceDal.GetSourcesForUser(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("JohnDoe", result.First().Username);
        Assert.AreEqual("Test Source", result.First().Name);
        httpClientMock.Verify(x => x.GetAsync($"/Source/{user.Username}"), Times.Once);
    }

    [Test]
    public async Task AddSourceForUser_ValidInput_ReturnsTrue()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var newSource = new Source { SourceId = 2, Name = "New Test Source", Type = "pdf"};

        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.OK };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.PostAsync($"/Source/{user.Username}", It.IsAny<StringContent>()))
            .ReturnsAsync(expectedResponse);

        var sourceDal = new SourceDal(httpClientMock.Object);

        // Act
        var result = await sourceDal.AddSourceForUser(user, newSource);

        // Assert
        Assert.IsTrue(result);
        httpClientMock.Verify(x => x.PostAsync($"/Source/{user.Username}", It.IsAny<StringContent>()), Times.Once);
    }
}