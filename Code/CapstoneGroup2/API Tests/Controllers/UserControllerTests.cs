using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Controllers;
using API.Dal;
using API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API_Tests.Controllers;

[TestFixture]
public class UserControllerTests
{
    #region Data members

    private readonly List<User> _users = new()
    {
        new User { Username = "testUser", Password = "testPassword", Token = "testToken" },
        new User { Username = "testUser2", Password = "testPassword2", Token = "testToken2" }
    };

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
        var result = userController.Login("testUser", "testPassword");

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
    public void LoginTest_InvalidUsername()
    {
        // Arrange
        var userController = new UserController(this._userDal);

        // Act
        var result = userController.Login("invalidUser", "testPassword");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<NotFoundResult>(result);
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

    #endregion
}