using CapstoneGroup2.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Server.UnitTests.Model;

[TestFixture]
public class Note_TagTests
{
    #region Methods

    [Test]
    public void Note_Tag_PropertiesSetCorrectly()
    {
        // Arrange
        var noteTag = new Note_Tag
        {
            NoteID = 5,
            TagID = 10
        };

        // Act & Assert
        Assert.AreEqual(5, noteTag.NoteID);
        Assert.AreEqual(10, noteTag.TagID);
    }

    #endregion
}

[TestFixture]
public class NoteTagConfigurationTests
{
    #region Methods

    [Test]
    public void NoteTagConfiguration_CorrectlyConfiguresEntity()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();
        var entityBuilder = modelBuilder.Entity<Note_Tag>();
        var configuration = new NoteTagConfiguration();

        // Act
        configuration.Configure(entityBuilder);

        // Assert
        var entityType = modelBuilder.Model.FindEntityType(typeof(Note_Tag));
        Assert.AreEqual("Note_Tag", entityType.GetTableName()); // Ensure table name is set correctly
        var primaryKey = entityType.FindPrimaryKey();
        Assert.AreEqual(2, primaryKey.Properties.Count); // Ensure composite primary key is set
        Assert.IsTrue(primaryKey.Properties[0].Name == "TagID" &&
                      primaryKey.Properties[1].Name == "NoteID"); // Ensure primary key properties are correctly set
        var tagIdProperty = entityType.FindProperty("TagID");
        Assert.IsTrue(tagIdProperty != null); // Ensure TagID property is configured
        Assert.AreEqual("tag_id", tagIdProperty.GetColumnName()); // Ensure column name for TagID is set correctly
        var noteIdProperty = entityType.FindProperty("NoteID");
        Assert.IsTrue(noteIdProperty != null); // Ensure NoteID property is configured
        Assert.AreEqual("note_id", noteIdProperty.GetColumnName()); // Ensure column name for NoteID is set correctly
    }

    #endregion
}