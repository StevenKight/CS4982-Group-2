using CapstoneGroup2.Desktop.Library.Model;

namespace CapstoneGroup2.Desktop.Tests.Model;

[TestFixture]
public class UserTests
{
    #region Methods

    [Test]
    public void UserProperties_SetAndGetCorrectly()
    {
        // Arrange
        var user = new User();

        // Act
        user.Username = "JohnDoe";
        user.Password = "SecurePassword";
        user.Token = "Token123";

        // Assert
        Assert.AreEqual("JohnDoe", user.Username);
        Assert.AreEqual("SecurePassword", user.Password);
        Assert.AreEqual("Token123", user.Token);
    }

    #endregion
}