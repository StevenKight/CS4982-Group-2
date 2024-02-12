using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CapstoneGroup2.Server.Controllers;
using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CapstoneGroup2.Server.UnitTests.Controllers;

/* dotcover disable */
[TestFixture]
public class UserControllerTests
{
    #region Data members

    private readonly List<User> _users =
    [
        new User { Username = "testUser", Password = "testPassword", Token = "testToken" },
        new User { Username = "testUser2", Password = "testPassword2", Token = "testToken2" }
    ];

    private DbContextOptions<DocunotesDbContext> _options;
    private DocunotesDbContext _context;

    private UserDal _userDal;

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

        this._context.Database.EnsureDeleted();
        this._context.Database.EnsureCreated();

        this._context.Users.AddRange(this._users);
        this._context.SaveChanges();

        this._userDal = new UserDal(this._context);
    }

    [Test]
    public void ConstructorTest()
    {
        // Arrange
        var userDal = new UserController(this._userDal);

        // Assert
        Assert.IsNotNull(userDal);
    }

    [Test]
    public void LoginTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);

        // Act
        var user = new User
        {
            Username = "testUser",
            Password = "testPassword"
        };
        var result = userController.Login(user);

        var token = new JwtSecurityToken(
            "CapstoneGroup2",
            "CapstoneGroup2",
            new[] { new Claim(JwtRegisteredClaimNames.UniqueName, "testUser") },
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey("CapstoneGroup2SecretKey"u8.ToArray()),
                SecurityAlgorithms.Aes128CbcHmacSha256
            )
        );
        var expectedToken = new JwtSecurityTokenHandler().WriteToken(token);

        // Assert
        var okResult = (result as OkObjectResult).Value as User;
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.AreEqual("testUser", okResult.Username);
        Assert.IsNull(okResult.Password);
        Assert.AreEqual(expectedToken, okResult.Token);
    }

    [Test]
    public void NullUserLoginTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);

        // Act
        var result = userController.Login(null);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    public void NullUsernameLoginTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);

        // Act
        var user = new User
        {
            Password = "testPassword"
        };
        var result = userController.Login(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    public void EmptyUsernameLoginTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);

        // Act
        var user = new User
        {
            Username = "",
            Password = "testPassword"
        };
        var result = userController.Login(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    public void WhitespaceUsernameLoginTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);

        // Act
        var user = new User
        {
            Username = "    ",
            Password = "testPassword"
        };
        var result = userController.Login(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    public void NullPasswordLoginTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);

        // Act
        var user = new User
        {
            Username = "testUser"
        };
        var result = userController.Login(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    public void EmptyPasswordLoginTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);

        // Act
        var user = new User
        {
            Username = "testUser",
            Password = ""
        };
        var result = userController.Login(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    public void WhitespacePasswordLoginTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);

        // Act
        var user = new User
        {
            Username = "testUser",
            Password = "    "
        };
        var result = userController.Login(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    public void InvalidUsernameLoginTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);

        // Act
        var user = new User
        {
            Username = "invalidUser",
            Password = "testPassword"
        };
        var result = userController.Login(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public void InvalidPasswordLoginTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);

        // Act
        var user = new User
        {
            Username = "testUser",
            Password = "invalidPassword"
        };
        var result = userController.Login(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<UnauthorizedResult>(result);
    }

    [Test]
    public void AddUserTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);
        var user = new User { Username = "testUser3", Password = "test" };

        // Act
        var result = userController.AddUser(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkResult>(result);

        Assert.AreEqual(3, this._context.Users.Count());
        Assert.Contains(user, this._context.Users.ToList());
    }

    [Test]
    public void DuplicateUsernameAddUserTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);
        var user = new User { Username = "testUser", Password = "testPassword" };

        // Act
        var result = userController.AddUser(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    [Test]
    public void NullAddUserTest()
    {
        // Arrange
        var userController = new UserController(this._userDal);

        // Act
        var result = userController.AddUser(null);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    #endregion
}