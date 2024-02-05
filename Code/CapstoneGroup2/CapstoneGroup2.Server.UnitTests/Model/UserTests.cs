using CapstoneGroup2.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.UnitTests.Model;

[TestFixture]
public class UserTests
{
    #region Methods

    [Test]
    public void User_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var user = new User
        {
            // Act
            Username = "testUser",
            Password = "testPassword",
            Token = "testToken"
        };

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(user.Username, Is.EqualTo("testUser"));
            Assert.That(user.Password, Is.EqualTo("testPassword"));
            Assert.That(user.Token, Is.EqualTo("testToken"));
        });
    }

    [Test]
    public void UserConfiguration_ShouldConfigureEntity()
    {
        // Arrange
        var userConfiguration = new UserConfiguration();
        var builder = new ModelBuilder();

        // Act
        userConfiguration.Configure(builder.Entity<User>());

        var entityType = builder.Model.FindEntityType(typeof(User));

        Assert.That(entityType, Is.Not.Null, "entityType is null");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(entityType!.GetTableName(), Is.EqualTo("User"));
            Assert.That(entityType!.FindPrimaryKey()?.Properties[0].Name, Is.EqualTo("Username"));
            Assert.That(entityType.GetProperties().ElementAt(0).GetColumnName(), Is.EqualTo("username"));
            Assert.That(entityType.GetProperties().ElementAt(1).GetColumnName(), Is.EqualTo("password"));
        });
    }

    #endregion
}