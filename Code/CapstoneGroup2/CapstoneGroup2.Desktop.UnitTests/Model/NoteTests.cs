using System;
using CapstoneGroup2.Desktop.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneGroup2.Desktop.UnitTests.Model
{
    /* dotcover disable */
    [TestClass]
    public class NoteTests
    {
        #region Methods

        [TestMethod]
        public void Note_Properties_ShouldBeSettableAndGettable()
        {
            // Arrange
            var note = new Note
            {
                // Act
                NoteId = 1,
                SourceId = 1,
                Username = "testUser",
                NoteText = "testNoteText",
                TagsString = "testTag1,testTag2",
                NoteDate = new DateTime(2021, 1, 1)
            };

            // Assert
            Assert.AreEqual(note.SourceId, 1);
            Assert.AreEqual(note.Username, "testUser");
            Assert.AreEqual(note.NoteText, "testNoteText");
            Assert.AreEqual(note.TagsString, "testTag1,testTag2");
            Assert.AreEqual(note.Tags[0], "testTag1");
            Assert.AreEqual(note.Tags[1], "testTag2");
            Assert.AreEqual(note.NoteDate, new DateTime(2021, 1, 1));
        }

        #endregion
    }
}