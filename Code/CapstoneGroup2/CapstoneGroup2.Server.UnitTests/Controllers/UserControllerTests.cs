﻿using CapstoneGroup2.Server.Controllers;
using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

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

        // Assert
        var okResult = (result as OkObjectResult).Value as User;
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<OkObjectResult>(result);
        Assert.AreEqual("testUser", okResult.Username);
        Assert.IsNull(okResult.Password);
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
        Assert.IsInstanceOf<OkObjectResult>(result);

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
        Assert.IsInstanceOf<ConflictResult>(result);
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
        Assert.IsInstanceOf<NoContentResult>(result);
    }

    [Test]
    public void AddUser_ExceptionThrown_ReturnsBadRequest()
    {
        // Arrange
        var mockDbDal = new Mock<IDbDal<User>>();
        mockDbDal.Setup(x => x.Add(It.IsAny<User>())).Throws<Exception>();

        var controller = new UserController(mockDbDal.Object);
        var newUser = new User { Username = "testuser", Password = "testpassword" };

        // Act
        var result = controller.AddUser(newUser);

        // Assert
        Assert.IsInstanceOf<BadRequestResult>(result);
    }

    #endregion
}