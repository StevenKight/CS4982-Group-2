using API.Model;
using Microsoft.EntityFrameworkCore;

namespace API_Tests.Model;

[TestFixture]
public class NoteTests
{
    #region Methods

    [Test]
    public void Note_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange
        var note = new Note
        {
            // Act
            SourceId = 1,
            Username = "testUser",
            NoteText = "testNoteText",
            TagsString = "testTag1,testTag2"
        };

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(note.SourceId, Is.EqualTo(1));
            Assert.That(note.Username, Is.EqualTo("testUser"));
            Assert.That(note.NoteText, Is.EqualTo("testNoteText"));
            Assert.That(note.TagsString, Is.EqualTo("testTag1,testTag2"));
            Assert.That(note.Tags[0], Is.EqualTo("testTag1"));
            Assert.That(note.Tags[1], Is.EqualTo("testTag2"));
        });
    }

    [Test]
    public void NoteConfiguration_ShouldConfigureEntity()
    {
        // Arrange
        var noteConfiguration = new NoteConfiguration();
        var builder = new ModelBuilder();

        // Act
        noteConfiguration.Configure(builder.Entity<Note>());

        var entityType = builder.Model.FindEntityType(typeof(Note));

        Assert.That(entityType, Is.Not.Null, "entityType is null");

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(entityType!.GetTableName(), Is.EqualTo("Note"));
            Assert.That(entityType!.FindPrimaryKey()?.Properties[0].Name, Is.EqualTo("SourceId"));
            Assert.That(entityType!.FindPrimaryKey()?.Properties[1].Name, Is.EqualTo("Username"));
            Assert.That(entityType.GetProperties().ElementAt(0).GetColumnName(), Is.EqualTo("source_id"));
            Assert.That(entityType.GetProperties().ElementAt(1).GetColumnName(), Is.EqualTo("username"));
            Assert.That(entityType.GetProperties().ElementAt(2).GetColumnName(), Is.EqualTo("note"));
            Assert.That(entityType.GetProperties().ElementAt(3).GetColumnName(), Is.EqualTo("tags"));
        });
    }

    #endregion
}