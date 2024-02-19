using CapstoneGroup2.Desktop.Library.Model;

namespace CapstoneGroup2.Desktop.Tests.Model;

    [TestFixture]
    public class SourceTests
    {
        [Test]
        public void SourceProperties_SetAndGetCorrectly()
        {
            // Arrange
            Source source = new Source();

            // Act
            source.SourceId = 1;
            source.Username = "JohnDoe";
            source.Type = SourceType.Pdf.ToString();
            source.Name = "Test Source";
            source.Description = "Test source description";
            source.IsLink = true;
            source.Link = "https://example.com";
            source.Content = new byte[] { 1, 2, 3 };
            source.CreatedAt = DateTime.Now;
            source.UpdatedAt = DateTime.Now;
            source.AuthorsString = "Author1|Author2";
            source.Publisher = "Test Publisher";
            source.AccessedAt = DateTime.Now;

            // Assert
            Assert.AreEqual(1, source.SourceId);
            Assert.AreEqual("JohnDoe", source.Username);
            Assert.AreEqual(SourceType.Pdf.ToString(), source.Type);
            Assert.AreEqual(SourceType.Pdf, source.NoteType);
            Assert.AreEqual("Test Source", source.Name);
            Assert.AreEqual("Test source description", source.Description);
            Assert.IsTrue(source.IsLink);
            Assert.AreEqual("https://example.com", source.Link);
            Assert.AreEqual(new byte[] { 1, 2, 3 }, source.Content);
            Assert.AreEqual(DateTime.Now.Date, source.CreatedAt.Date);
            Assert.AreEqual(DateTime.Now.Date, source.UpdatedAt?.Date);
            Assert.AreEqual("Author1|Author2", source.AuthorsString);
            CollectionAssert.AreEqual(new string[] { "Author1", "Author2" }, source.Authors);
            Assert.AreEqual("Test Publisher", source.Publisher);
            Assert.AreEqual(DateTime.Now.Date, source.AccessedAt?.Date);
        }

        [Test]
        public void Authors_ReturnsCorrectListWhenAuthorsStringIsNull()
        {
            // Arrange
            Source source = new Source();

            // Act
            var authors = source.Authors;

            // Assert
            Assert.IsNull(authors);
        }

        [Test]
        public void Authors_ReturnsCorrectListWhenAuthorsStringIsNotNull()
        {
            // Arrange
            Source source = new Source { AuthorsString = "Author1|Author2" };

            // Act
            var authors = source.Authors;

            // Assert
            CollectionAssert.AreEqual(new string[] { "Author1", "Author2" }, authors);
        }
    }