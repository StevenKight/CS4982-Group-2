using System.Net;
using System.Net.Http.Formatting;
using CapstoneGroup2.Desktop.Library.Dal;
using CapstoneGroup2.Desktop.Library.Mocks;
using CapstoneGroup2.Desktop.Library.Model;
using Moq;

namespace CapstoneGroup2.Desktop.Tests.Dal;

[TestFixture]
public class UserDalTests
{
    #region Methods

    [Test]
    public void ValidConstructor()
    {
        // Arrange, Act
        var userDal = new UserDal();

        // Assert
        Assert.IsNotNull(userDal);
    }

    [Test]
    public async Task Login_ValidUser_ReturnsUser()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new ObjectContent<User>(user, new JsonMediaTypeFormatter())
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
    public async Task Login_ValidInser_ReturnsNull()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest
        };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.PostAsJsonAsync("/Login", user)).ReturnsAsync(expectedResponse);

        var userDal = new UserDal(httpClientMock.Object);

        // Act
        var result = await userDal.Login(user);

        // Assert
        Assert.IsNull(result);
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Login", user), Times.Once);
    }

    [Test]
    public async Task Login_InvalidUser_ThrowsOutOfRangeException()
    {
        // Arrange
        var invalidUser = new User { Username = "", Password = "" };

        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await userDal.Login(invalidUser));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Login", invalidUser), Times.Never);
    }

    [Test]
    public async Task CreateAccount_ValidUser_ReturnsUser()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new ObjectContent<User>(user, new JsonMediaTypeFormatter())
        };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.PostAsJsonAsync("/Sign-up", user)).ReturnsAsync(expectedResponse);

        var userDal = new UserDal(httpClientMock.Object);

        // Act
        var result = await userDal.CreateAccount(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("JohnDoe", result.Username);
        Assert.AreEqual("SecurePassword", result.Password);
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", user), Times.Once);
    }

    [Test]
    public async Task CreateAccount_InvalidUser_ReturnsNull()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest
        };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.PostAsJsonAsync("/Sign-up", user)).ReturnsAsync(expectedResponse);

        var userDal = new UserDal(httpClientMock.Object);

        // Act
        var result = await userDal.CreateAccount(user);

        // Assert
        Assert.IsNull(result);
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", user), Times.Once);
    }

    [Test]
    public async Task CreateAccount_NullUser_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await userDal.Login(null));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task CreateAccount_InvalidUsername_ThrowsOutOfRangeException()
    {
        // Arrange
        var invalidUser = new User { Username = "", Password = "Password" };

        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await userDal.Login(invalidUser));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", invalidUser), Times.Never);
    }

    [Test]
    public async Task CreateAccount_InvalidPassword_ThrowsOutOfRangeException()
    {
        // Arrange
        var invalidUser = new User { Username = "Username", Password = "" };

        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await userDal.Login(invalidUser));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", invalidUser), Times.Never);
    }

    #endregion
}