using CapstoneGroup2.Server.Controllers;
using CapstoneGroup2.Server.Dal;
using CapstoneGroup2.Server.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CapstoneGroup2.Server.UnitTests.Controllers;

[TestFixture]
public class TagControllerTests
{
    private Mock<IDbDal<Tag>> mockDal;
    private TagController tagController;

    [SetUp]
    public void Setup()
    {
        mockDal = new Mock<IDbDal<Tag>>();
        tagController = new TagController(mockDal.Object);
    }

    [Test]
    public void GetAll_WithValidUsername_ReturnsOkResult()
    {
        // Arrange
        string username = "testUser";
        mockDal.Setup(m => m.GetAll()).Returns(new List<Tag>());

        // Act
        var result = tagController.GetAll(username);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public void GetAll_WithInvalidUsername_ReturnsUnauthorizedResult()
    {
        // Arrange
        string username = null;

        // Act
        var result = tagController.GetAll(username);

        // Assert
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
    }

    [Test]
    public void GetById_WithValidIdAndUsername_ReturnsOkResult()
    {
        // Arrange
        int tagId = 1;
        string username = "testUser";
        var tag = new Tag { TagID = tagId, TagName = "TestTag" };
        mockDal.Setup(m => m.Get(tagId)).Returns(tag);

        // Act
        var result = tagController.GetById(tagId, username);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public void GetById_WithInvalidId_ReturnsBadRequestResult()
    {
        // Arrange
        int tagId = -1;
        string username = "testUser";

        // Act
        var result = tagController.GetById(tagId, username);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }

    // Similar tests for Create, Update, and Delete methods

    // Example test for Create method
    [Test]
    public void Create_WithValidUsernameAndTag_ReturnsOkResult()
    {
        // Arrange
        string username = "testUser";
        var newTag = new Tag { TagName = "NewTag" };
        mockDal.Setup(m => m.Add(newTag)).Returns(true);

        // Act
        var result = tagController.Create(username, newTag);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public void Update_WithValidUsernameAndTag_ReturnsOkResult()
    {
        // Arrange
        string username = "testUser";
        var updatedTag = new Tag {TagID = 1, TagName = "UpdatedTag" };
        mockDal.Setup(m => m.Update(updatedTag)).Returns(true);

        // Act
        var result = tagController.Update(username, updatedTag);

        // Assert
        Assert.IsInstanceOf<OkResult>(result);
    }

    [Test]
    public void Update_WithInvalidUsername_ReturnsUnauthorizedResult()
    {
        // Arrange
        string username = null;
        var updatedTag = new Tag { TagID = 1, TagName = "UpdatedTag" };

        // Act
        var result = tagController.Update(username, updatedTag);

        // Assert
        Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
    }

    [Test]
    public void Delete_WithValidTagId_ReturnsOkResult()
    {
        // Arrange
        int tagId = 1;
        var mockResult = true;
        mockDal.Setup(m => m.Delete(It.IsAny<Tag>())).Returns(mockResult);

        // Act
        var result = tagController.Delete(tagId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public void Delete_WithInvalidTagId_ReturnsBadRequestResult()
    {
        // Arrange
        int tagId = -1;

        // Act
        var result = tagController.Delete(tagId);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
    }
}