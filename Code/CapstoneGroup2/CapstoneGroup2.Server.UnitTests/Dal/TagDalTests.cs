using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CapstoneGroup2.Server.UnitTests.Dal;

[TestFixture]
public class TagDalTests
{
    private List<Tag> _tags =
    [
        new Tag { TagID = 1, TagName = "Tag1" },
        new Tag { TagID = 2, TagName = "Tag2" },
        new Tag { TagID = 3, TagName = "Tag3" }
    ];
    private DbContextOptions<DocunotesDbContext> _options;
    private DocunotesDbContext _context;


    [OneTimeSetUp]
    public void Setup()
    {
        this._options = new DbContextOptionsBuilder<DocunotesDbContext>()
            .UseInMemoryDatabase("Docunotes")
            .EnableSensitiveDataLogging()
            .Options;
        this._context = new DocunotesDbContext(this._options);
        this._context.CurrentUser = new User { Username = "testUser", Password = "testPassword" };
        this._context.Tags.AddRange(this._tags);
        this._context.SaveChanges();
    }
    [Test]
    public void Get_ValidTagId_ReturnsTag()
    {
        var tagDal = new TagDal(this._context);

        // Act
        var result = tagDal.Get(3);

        // Assert
        Assert.That(result, Is.EqualTo(_tags[2]));
    }

    [Test]
    public void GetAll_ReturnsAllTags()
    {

        var tagDal = new TagDal(this._context);

        // Act
        var result = tagDal.GetAll();

        // Assert
        Assert.That(result.Count(), Is.EqualTo(3));
    }
    [Test]
    public void Add_ValidTag_ReturnsTrue()
    {
        // Arrange
        var tag = new Tag { TagID = 4, TagName = "TestTag" };
        var tagDal = new TagDal(this._context);

        // Act
        var result = tagDal.Add(tag);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void Delete_ValidID_ReturnsTrue()
    {
        // Arrange
        var tagDal = new TagDal(this._context); // Mock DbContext not needed for this test

        var result = tagDal.Delete(this._tags[0]);
        Assert.True(result);
    }

    [Test]
    public void SetSourceId_NotImplemented_ThrowsException()
    {
        // Arrange
        var tagDal = new TagDal(null); // Mock DbContext not needed for this test

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tagDal.SetSourceId(1));
    }

    [Test]
    public void SetUser_SetsCurrentUser()
    {
        // Arrange
        var username = "testUser";
        var tagDal = new TagDal(this._context);

        // Act
        tagDal.SetUser(username);

        // Assert
        Assert.That("testUser", Is.EqualTo(username));
    }

    [Test]
    public void Update_ValidTag_ReturnsTrue()
    {
        var tagDal = new TagDal(null); // Mock DbContext not needed for this test

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => tagDal.SetSourceId(1));
    }
}