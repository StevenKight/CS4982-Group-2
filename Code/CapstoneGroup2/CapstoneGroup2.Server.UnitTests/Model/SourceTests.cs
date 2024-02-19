using CapstoneGroup2.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.UnitTests.Model;

/* dotcover disable */
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
            Description = "testDescription",
            IsLink = true,
            Link = "testLink",
            Content = new byte[2],
            AuthorsString = "testAuthors|testAuthors2",
            Publisher = "testPublisher",
            CreatedAt = new DateTime(2021, 1, 1),
            UpdatedAt = new DateTime(2021, 1, 2),
            AccessedAt = new DateTime(2021, 1, 3)
        };

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(source.SourceId, Is.EqualTo(1));
            Assert.That(source.Username, Is.EqualTo("testUser"));
            Assert.That(source.Type, Is.EqualTo("Pdf"));
            Assert.That(source.NoteType, Is.EqualTo(SourceType.Pdf));
            Assert.That(source.Name, Is.EqualTo("testName"));
            Assert.That(source.Description, Is.EqualTo("testDescription"));
            Assert.That(source.IsLink, Is.True);
            Assert.That(source.Link, Is.EqualTo("testLink"));
            Assert.That(source.Content, Is.EqualTo(new byte[2]));
            Assert.That(source.AuthorsString, Is.EqualTo("testAuthors|testAuthors2"));
            Assert.That(source.Authors, Is.EquivalentTo(new List<string> { "testAuthors", "testAuthors2" }));
            Assert.That(source.Publisher, Is.EqualTo("testPublisher"));
            Assert.That(source.CreatedAt, Is.EqualTo(new DateTime(2021, 1, 1)));
            Assert.That(source.UpdatedAt, Is.EqualTo(new DateTime(2021, 1, 2)));
            Assert.That(source.AccessedAt, Is.EqualTo(new DateTime(2021, 1, 3)));
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
            Assert.That(entityType.GetProperties().ElementAt(1).GetColumnName(), Is.EqualTo("accessed_at"));
            Assert.That(entityType.GetProperties().ElementAt(2).GetColumnName(), Is.EqualTo("authors"));
            Assert.That(entityType.GetProperties().ElementAt(3).GetColumnName(), Is.EqualTo("content"));
            Assert.That(entityType.GetProperties().ElementAt(4).GetColumnName(), Is.EqualTo("created_at"));
            Assert.That(entityType.GetProperties().ElementAt(5).GetColumnName(), Is.EqualTo("description"));
            Assert.That(entityType.GetProperties().ElementAt(6).GetColumnName(), Is.EqualTo("is_link"));
            Assert.That(entityType.GetProperties().ElementAt(7).GetColumnName(), Is.EqualTo("link"));
            Assert.That(entityType.GetProperties().ElementAt(8).GetColumnName(), Is.EqualTo("name"));
            Assert.That(entityType.GetProperties().ElementAt(9).GetColumnName(), Is.EqualTo("publisher"));
            Assert.That(entityType.GetProperties().ElementAt(10).GetColumnName(), Is.EqualTo("type"));
            Assert.That(entityType.GetProperties().ElementAt(11).GetColumnName(), Is.EqualTo("updated_at"));
            Assert.That(entityType.GetProperties().ElementAt(12).GetColumnName(), Is.EqualTo("username"));
        });
    }

    #endregion
}