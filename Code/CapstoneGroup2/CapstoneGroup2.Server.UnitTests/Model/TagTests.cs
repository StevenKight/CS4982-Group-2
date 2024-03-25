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
    public class TagTests
    {
        [Test]
        public void Tag_PropertiesSetCorrectly()
        {
            // Arrange
            var tag = new Tag();

            // Act & Assert
            Assert.AreEqual(0, tag.TagID); // By default, TagID should be 0
            Assert.IsNull(tag.TagName); // By default, TagName should be null
        }
    }

    [TestFixture]
    public class TagConfigurationTests
    {
        [Test]
        public void TagConfiguration_CorrectlyConfiguresEntity()
        {
            // Arrange
            var modelBuilder = new ModelBuilder();
            var entityBuilder = modelBuilder.Entity<Tag>();
            var configuration = new TagConfiguration();

            // Act
            configuration.Configure(entityBuilder);

            // Assert
            var entityType = modelBuilder.Model.FindEntityType(typeof(Tag));
            Assert.AreEqual("Tag", entityType.GetTableName()); // Ensure table name is set correctly
            var primaryKey = entityType.FindPrimaryKey();
            Assert.AreEqual(1, primaryKey.Properties.Count); // Ensure primary key is set
            Assert.IsTrue(primaryKey.Properties[0].Name == "TagID"); // Ensure primary key property is correctly set
            var tagNameProperty = entityType.FindProperty("TagName");
            Assert.IsTrue(tagNameProperty != null); // Ensure property is configured
            Assert.AreEqual("tag_name", tagNameProperty.GetColumnName()); // Ensure column name is set correctly
        }
    }
}
