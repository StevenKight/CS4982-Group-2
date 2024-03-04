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
    public async Task Login_InvalidUser_ReturnsNull()
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
    public async Task Login_NullUser_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await userDal.Login(null));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task Login_NullUsername_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await userDal.Login(new User { Password = "test" }));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task Login_NullPassword_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await userDal.Login(new User { Username = "test" }));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task Login_EmptyUsername_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await userDal.Login(new User { Username = "", Password = "test" }));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task Login_EmptyPassword_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await userDal.Login(new User { Username = "test", Password = "" }));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task Login_BlankUsername_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await userDal.Login(new User { Username = "  ", Password = "test" }));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task Login_BlankPassword_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await userDal.Login(new User { Username = "test", Password = "  " }));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
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
    public async Task CreateAccount_InvalidUser_ThrowsException()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Conflict
        };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.PostAsJsonAsync("/Sign-up", user)).ReturnsAsync(expectedResponse);

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<Exception>(async () => await userDal.CreateAccount(user));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", user), Times.Once);
    }

    [Test]
    public async Task CreateAccount_BadRequest_ThrowsException()
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

        // Act, Assert
        Assert.ThrowsAsync<Exception>(async () => await userDal.CreateAccount(user));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", user), Times.Once);
    }

    [Test]
    public async Task CreateAccount_Invalid_ReturnsNull()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Unauthorized
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
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await userDal.CreateAccount(null));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task CreateAccount_NullUsername_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await userDal.CreateAccount(new User { Password = "test" }));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task CreateAccount_NullPassword_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await userDal.CreateAccount(new User { Username = "test" }));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task CreateAccount_EmptyUsername_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await userDal.CreateAccount(new User { Username = "", Password = "test" }));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task CreateAccount_EmptyPassword_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await userDal.CreateAccount(new User { Username = "test", Password = "" }));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task CreateAccount_BlankUsername_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await userDal.CreateAccount(new User { Username = "  ", Password = "test" }));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    [Test]
    public async Task CreateAccount_BlankPassword_ThrowsOutOfRangeException()
    {
        // Arrange
        var httpClientMock = new Mock<IHttpClientWrapper>();

        var userDal = new UserDal(httpClientMock.Object);

        // Act, Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            await userDal.CreateAccount(new User { Username = "test", Password = "  " }));
        httpClientMock.Verify(x => x.PostAsJsonAsync("/Sign-up", null), Times.Never);
    }

    #endregion
}