﻿using System.Net;
using System.Net.Http.Formatting;
using CapstoneGroup2.Desktop.Library.Dal;
using CapstoneGroup2.Desktop.Library.Mocks;
using CapstoneGroup2.Desktop.Library.Model;
using Moq;

namespace CapstoneGroup2.Desktop.Tests.Dal;

[TestFixture]
public class SourceDalTests
{
    #region Methods

    [Test]
    public void ValidConstructor()
    {
        // Arrange, Act
        var sourceDal = new SourceDal();

        // Assert
        Assert.IsNotNull(sourceDal);
    }

    [Test]
    public async Task GetSourcesForUser_ValidInput_ReturnsSources()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };

        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new ObjectContent<IEnumerable<Source>>(
                new List<Source> { new() { SourceId = 1, Username = "JohnDoe", Name = "Test Source", Type = "pdf" } },
                new JsonMediaTypeFormatter())
        };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.GetAsync($"/Source/{user.Username}")).ReturnsAsync(expectedResponse);

        var sourceDal = new SourceDal(httpClientMock.Object);

        // Act
        var result = await sourceDal.GetSourcesForUser(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count());
        Assert.AreEqual("JohnDoe", result.First().Username);
        Assert.AreEqual("Test Source", result.First().Name);
        httpClientMock.Verify(x => x.GetAsync($"/Source/{user.Username}"), Times.Once);
    }

    [Test]
    public async Task GetSourcesForUser_InvalidUser_ReturnsSources()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };

        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest
        };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.GetAsync($"/Source/{user.Username}")).ReturnsAsync(expectedResponse);

        var sourceDal = new SourceDal(httpClientMock.Object);

        // Act
        var result = await sourceDal.GetSourcesForUser(user);

        // Assert
        Assert.IsNull(result);
        httpClientMock.Verify(x => x.GetAsync($"/Source/{user.Username}"), Times.Once);
    }

    [Test]
    public async Task AddSourceForUser_ValidInput_ReturnsTrue()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var newSource = new Source { SourceId = 2, Name = "New Test Source", Type = "pdf" };

        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.PostAsync($"/Source/{user.Username}", It.IsAny<StringContent>()))
            .ReturnsAsync(expectedResponse);

        var sourceDal = new SourceDal(httpClientMock.Object);

        // Act
        var result = await sourceDal.AddSourceForUser(user, newSource);

        // Assert
        Assert.IsTrue(result);
        httpClientMock.Verify(x => x.PostAsync($"/Source/{user.Username}", It.IsAny<StringContent>()), Times.Once);
    }

    [Test]
    public async Task AddSourceForUser_InvalidInput_ReturnsTrue()
    {
        // Arrange
        var user = new User { Username = "JohnDoe", Password = "SecurePassword" };
        var newSource = new Source { SourceId = 2, Name = "New Test Source", Type = "pdf" };

        var expectedUri = new Uri("https://localhost:7048");
        var expectedResponse = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest };

        var httpClientMock = new Mock<IHttpClientWrapper>();
        httpClientMock.Setup(x => x.BaseAddress).Returns(expectedUri);
        httpClientMock.Setup(x => x.PostAsync($"/Source/{user.Username}", It.IsAny<StringContent>()))
            .ReturnsAsync(expectedResponse);

        var sourceDal = new SourceDal(httpClientMock.Object);

        // Act
        var result = await sourceDal.AddSourceForUser(user, newSource);

        // Assert
        Assert.IsFalse(result);
        httpClientMock.Verify(x => x.PostAsync($"/Source/{user.Username}", It.IsAny<StringContent>()), Times.Once);
    }

    #endregion
}