using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.UnitTests.Dal;

[TestFixture]
public class UserDalTests
{
    #region Data members

    private readonly List<User> _users = new()
    {
        new User { Username = "testUser", Password = "testPassword", Token = "testToken" },
        new User { Username = "testUser2", Password = "testPassword2", Token = "testToken2" }
    };

    private DbContextOptions<DocunotesDbContext> _options;
    private DocunotesDbContext _context;

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
    }

    [Test]
    public void ConstructorTest()
    {
        // Arrange
        var userDal = new UserDal(this._context);

        // Assert
        Assert.IsNotNull(userDal);
    }

    [Test]
    public void GetUserByIdTest()
    {
        // Arrange
        var userDal = new UserDal(this._context);

        // Act
        var result = userDal.Get("testUser");

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("testUser", result.Username);
    }

    [Test]
    public void GetAllUsersTest()
    {
        // Arrange
        var userDal = new UserDal(this._context);

        // Act
        var result = userDal.GetAll();

        var expected = this._users.Count();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expected, result.Count());
        Assert.AreEqual(this._users, result.ToList());
    }

    [Test]
    public void AddUserTest()
    {
        // Arrange
        var userDal = new UserDal(this._context);
        var user = new User { Username = "testUser3", Password = "test", Token = "test" };

        // Act
        var result = userDal.Add(user);

        this._users.Add(user);
        var expected = this._users.Count();

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(expected, this._context.Users.Count());
        Assert.Contains(user, this._context.Users.ToList());
    }

    [Test]
    public void UpdateUserTest()
    {
        // Arrange
        var userDal = new UserDal(this._context);

        // Assert
        Assert.Throws<InvalidOperationException>(() => userDal.Update(new User()));
    }

    [Test]
    public void DeleteUserTest()
    {
        // Arrange
        var userDal = new UserDal(this._context);

        // Assert
        Assert.Throws<InvalidOperationException>(() => userDal.Delete(new User()));
    }

    #endregion
}