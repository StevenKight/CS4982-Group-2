using System;
using CapstoneGroup2.Desktop.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneGroup2.Desktop.UnitTests.Model
{
    /* dotcover disable */
    [TestClass]
    public class SourceTests
    {
        #region Methods

        [TestMethod]
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
                CreatedAt = new DateTime(2021, 1, 1),
                UpdatedAt = new DateTime(2021, 1, 2)
            };

            // Assert
            Assert.AreEqual(source.SourceId, 1);
            Assert.AreEqual(source.Username, "testUser");
            Assert.AreEqual(source.Type, "Pdf");
            Assert.AreEqual(source.NoteType, SourceType.Pdf);
            Assert.AreEqual(source.Name, "testName");
            Assert.AreEqual(source.Description, "testDescription");
            Assert.AreEqual(source.IsLink, true);
            Assert.AreEqual(source.Link, "testLink");
            Assert.AreEqual(source.CreatedAt, new DateTime(2021, 1, 1));
            Assert.AreEqual(source.UpdatedAt, new DateTime(2021, 1, 2));
        }

        #endregion
    }
}