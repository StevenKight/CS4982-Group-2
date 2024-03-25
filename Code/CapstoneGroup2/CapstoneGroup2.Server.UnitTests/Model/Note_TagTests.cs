using CapstoneGroup2.Server.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapstoneGroup2.Server.UnitTests.Model
{
    [TestFixture]
    public class Note_TagTests
    {
        [Test]
        public void Note_Tag_PropertiesSetCorrectly()
        {
            // Arrange
            var noteTag = new Note_Tag();

            // Act & Assert
            Assert.AreEqual(0, noteTag.NoteID); // By default, NoteID should be 0
            Assert.AreEqual(0, noteTag.TagID); // By default, TagID should be 0
        }
    }

    [TestFixture]
    public class NoteTagConfigurationTests
    {
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
            Assert.IsTrue(primaryKey.Properties[0].Name == "TagID" && primaryKey.Properties[1].Name == "NoteID"); // Ensure primary key properties are correctly set
            var tagIdProperty = entityType.FindProperty("TagID");
            Assert.IsTrue(tagIdProperty != null); // Ensure TagID property is configured
            Assert.AreEqual("tag_id", tagIdProperty.GetColumnName()); // Ensure column name for TagID is set correctly
            var noteIdProperty = entityType.FindProperty("NoteID");
            Assert.IsTrue(noteIdProperty != null); // Ensure NoteID property is configured
            Assert.AreEqual("note_id", noteIdProperty.GetColumnName()); // Ensure column name for NoteID is set correctly
        }
    }
}
