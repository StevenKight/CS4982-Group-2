using CapstoneGroup2.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.UnitTests.Model;

[TestFixture]
public class SharedTests
{
    #region Methods

    [Test]
    public void Shared_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var sharedNote = new Shared
        {
            // Act
            SourceId = 1,
            Username = "testUser",
            SharedUsername = "testSharedUser",
            Comment = "testComment"
        };

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(sharedNote.SourceId, Is.EqualTo(1));
            Assert.That(sharedNote.Username, Is.EqualTo("testUser"));
            Assert.That(sharedNote.SharedUsername, Is.EqualTo("testSharedUser"));
            Assert.That(sharedNote.Comment, Is.EqualTo("testComment"));
        });
    }

    [Test]
    public void SharedConfiguration_ShouldConfigureEntity()
    {
        // Arrange
        var sharedConfiguration = new SharedConfiguration();
        var builder = new ModelBuilder();

        // Act
        sharedConfiguration.Configure(builder.Entity<Shared>());

        var entityType = builder.Model.FindEntityType(typeof(Shared));

        Assert.That(entityType, Is.Not.Null, "entityType is null");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(entityType!.GetTableName(), Is.EqualTo("Shared"));
            Assert.That(entityType!.FindPrimaryKey()?.Properties[0].Name, Is.EqualTo("SourceId"));
            Assert.That(entityType!.FindPrimaryKey()?.Properties[1].Name, Is.EqualTo("Username"));
            Assert.That(entityType!.FindPrimaryKey()?.Properties[2].Name, Is.EqualTo("SharedUsername"));
            Assert.That(entityType.GetProperties().ElementAt(0).GetColumnName(), Is.EqualTo("source_id"));
            Assert.That(entityType.GetProperties().ElementAt(1).GetColumnName(), Is.EqualTo("username"));
            Assert.That(entityType.GetProperties().ElementAt(2).GetColumnName(), Is.EqualTo("shared_username"));
            Assert.That(entityType.GetProperties().ElementAt(3).GetColumnName(), Is.EqualTo("comment"));
        });
    }

    #endregion
}