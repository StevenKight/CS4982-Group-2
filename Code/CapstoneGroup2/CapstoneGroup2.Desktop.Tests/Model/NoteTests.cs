using CapstoneGroup2.Desktop.Library.Model;

namespace CapstoneGroup2.Desktop.Tests.Model;

[TestFixture]
public class NoteTests
{
    #region Methods

    [Test]
    public void NoteProperties_SetAndGetCorrectly()
    {
        // Arrange
        var note = new Note();

        // Act
        note.NoteId = 1;
        note.SourceId = 2;
        note.Username = "JohnDoe";
        note.NoteText = "Test note text";
        note.NoteDate = DateTime.Now;

        // Assert
        Assert.AreEqual(1, note.NoteId);
        Assert.AreEqual(2, note.SourceId);
        Assert.AreEqual("JohnDoe", note.Username);
        Assert.AreEqual("Test note text", note.NoteText);
        Assert.AreEqual(DateTime.Now.Date, note.NoteDate.Date);
    }

    #endregion
}