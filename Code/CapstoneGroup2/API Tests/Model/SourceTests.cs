using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API_Tests.Model;

[TestFixture]
public class SourceTests
{
    #region Methods

    [Test]
    public void Source_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var source = new Source
        {
            // Act
            SourceId = 1,
            Username = "testUser",
            Type = "Pdf",
            Name = "testName",
            IsLink = true,
            Link = "testLink"
        };

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(source.SourceId, Is.EqualTo(1));
            Assert.That(source.Username, Is.EqualTo("testUser"));
            Assert.That(source.Type, Is.EqualTo("Pdf"));
            Assert.That(source.NoteType, Is.EqualTo(SourceType.Pdf));
            Assert.That(source.Name, Is.EqualTo("testName"));
            Assert.That(source.IsLink, Is.True);
            Assert.That(source.Link, Is.EqualTo("testLink"));
        });
    }

    [Test]
    public void SourceConfiguration_ShouldConfigureEntity()
    {
        // Arrange
        var sourceConfiguration = new SourceConfiguration();
        var builder = new ModelBuilder();

        // Act
        sourceConfiguration.Configure(builder.Entity<Source>());

        var entityType = builder.Model.FindEntityType(typeof(Source));

        Assert.That(entityType, Is.Not.Null, "entityType is null");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(entityType!.GetTableName(), Is.EqualTo("Source"));
            Assert.That(entityType!.FindPrimaryKey()?.Properties[0].Name, Is.EqualTo("SourceId"));
            Assert.That(entityType.GetProperties().ElementAt(0).GetColumnName(), Is.EqualTo("source_id"));
            Assert.That(entityType.GetProperties().ElementAt(1).GetColumnName(), Is.EqualTo("is_link"));
            Assert.That(entityType.GetProperties().ElementAt(2).GetColumnName(), Is.EqualTo("link"));
            Assert.That(entityType.GetProperties().ElementAt(3).GetColumnName(), Is.EqualTo("name"));
            Assert.That(entityType.GetProperties().ElementAt(4).GetColumnName(), Is.EqualTo("type"));
            Assert.That(entityType.GetProperties().ElementAt(5).GetColumnName(), Is.EqualTo("username"));
        });
    }

    #endregion
}