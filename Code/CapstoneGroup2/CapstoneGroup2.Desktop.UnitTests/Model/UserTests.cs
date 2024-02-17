using CapstoneGroup2.Desktop.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneGroup2.Desktop.UnitTests.Model
{
    /* dotcover disable */
    [TestClass]
    public class UserTests
    {
        #region Methods

        [TestMethod]
        public void User_Properties_ShouldBeSettableAndGettable()
        {
            // Arrange
            var user = new User
            {
                // Act
                Username = "testUser",
                Password = "testPassword",
                Token = "testToken"
            };

            // Assert
            Assert.AreEqual(user.Username, "testUser");
            Assert.AreEqual(user.Password, "testPassword");
            Assert.AreEqual(user.Token, "testToken");
        }

        #endregion
    }
}