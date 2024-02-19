using CapstoneGroup2.Desktop.Dal;
using CapstoneGroup2.Desktop.Library.Mocks;
using CapstoneGroup2.Desktop.Library.Model;
using Moq;

namespace CapstoneGroup2.Desktop.Tests.Dal;

[TestFixture]
public class UserDalTests
{
    [Test]
    public async Task Login_ValidUser_ReturnsUser()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = new ObjectContent<User>(user, new System.Net.Http.Formatting.JsonMediaTypeFormatter())
        };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.PostAsJsonAsync("/Login", user)).ReturnsAsync(expectedResponse);

        var userDal = new UserDal(httpClientMock.Object);

        // Act
        var result = await userDal.Login(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("JohnDoe", result.Username);
        Assert.AreEqual("SecurePassword", result.Password);
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Login", user), Times.Once);
    }

    [Test]
    public async Task Login_InvalidUser_ReturnsNull()
    {
        // Arrange
        var invalidUser = new User { Username = "", Password = "" };

        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act
        var result = await userDal.Login(invalidUser);

        // Assert
        Assert.IsNull(result);
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Login", invalidUser), Times.Never);
    }
}