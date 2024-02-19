using CapstoneGroup2.Desktop.Library.Model;

namespace CapstoneGroup2.Desktop.Tests.Model;

    [TestFixture]
    public class NoteTests
    {
        [Test]
        public void NoteProperties_SetAndGetCorrectly()
        {
            // Arrange
            Note note = new Note();

            // Act
            note.NoteId = 1;
            note.SourceId = 2;
            note.Username = "JohnDoe";
            note.NoteText = "Test note text";
            note.TagsString = "tag1,tag2,tag3";
            note.NoteDate = DateTime.Now;

            // Assert
            Assert.AreEqual(1, note.NoteId);
            Assert.AreEqual(2, note.SourceId);
            Assert.AreEqual("JohnDoe", note.Username);
            Assert.AreEqual("Test note text", note.NoteText);
            Assert.AreEqual("tag1,tag2,tag3", note.TagsString);
            Assert.AreEqual(DateTime.Now.Date, note.NoteDate.Date);
        }

        [Test]
        public void Tags_ReturnsCorrectList()
        {
            // Arrange
            Note note = new Note { TagsString = "tag1,tag2,tag3" };

            // Act
            var tags = note.Tags;

            // Assert
            Assert.AreEqual(3, tags.Count);
            Assert.Contains("tag1", tags);
            Assert.Contains("tag2", tags);
            Assert.Contains("tag3", tags);
        }
    }