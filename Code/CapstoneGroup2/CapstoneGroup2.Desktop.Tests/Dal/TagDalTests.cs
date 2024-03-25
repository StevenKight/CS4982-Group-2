using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Library.Dal;
using CapstoneGroup2.Desktop.Library.Mocks;
using CapstoneGroup2.Desktop.Library.Model;
using Microsoft.EntityFrameworkCore;

namespace CapstoneGroup2.Desktop.Tests.Dal
{
    [TestFixture]
    public class TagDalTests
    {
        private TagDal _tagDal;
        private Mock<IHttpClientWrapper> _httpClientWrapperMock;

        [SetUp]
        public void SetUp()
        {
            _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            _tagDal = new TagDal(_httpClientWrapperMock.Object);
        }

        [Test]
        public async Task GetTags_Success_ReturnsTags()
        {
            // Arrange
            var user = new User { Username = "testUser" };
            var expectedTags = new List<Tag> { new Tag { TagID = 1, TagName = "Tag1" }, new Tag { TagID = 2, TagName = "Tag2" } };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("[{\"TagID\":1,\"Name\":\"Tag1\"},{\"TagID\":2,\"Name\":\"Tag2\"}]")
            };
            _httpClientWrapperMock.Setup(client => client.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _tagDal.GetTags(user);

            // Assert
            Assert.AreEqual(expectedTags.Count, result.Count());
        }

        [Test]
        public async Task GetTags_Failure_ReturnsNull()
        {
            // Arrange
            var user = new User { Username = "testUser" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            _httpClientWrapperMock.Setup(client => client.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _tagDal.GetTags(user);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateNewNote_Success_ReturnsTrue()
        {
            // Arrange
            var user = new User { Username = "testUser" };
            var tag = new Tag { TagID = 1, TagName = "Tag1" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            _httpClientWrapperMock.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _tagDal.createNewNote(user, tag);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CreateNewNote_Failure_ReturnsFalse()
        {
            // Arrange
            var user = new User { Username = "testUser" };
            var tag = new Tag { TagID = 1, TagName = "Tag1" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            _httpClientWrapperMock.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _tagDal.createNewNote(user, tag);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateTag_Success_ReturnsTrue()
        {
            // Arrange
            var user = new User { Username = "testUser" };
            var tag = new Tag { TagID = 1, TagName = "Tag1" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            _httpClientWrapperMock.Setup(client => client.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _tagDal.UpdateTag(tag, user);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UpdateTag_Failure_ReturnsFalse()
        {
            // Arrange
            var user = new User { Username = "testUser" };
            var tag = new Tag { TagID = 1, TagName = "Tag1" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            _httpClientWrapperMock.Setup(client => client.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _tagDal.UpdateTag(tag, user);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteTag_Success_ReturnsTrue()
        {
            // Arrange
            var tag = new Tag { TagID = 1, TagName = "Tag1" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            _httpClientWrapperMock.Setup(client => client.DeleteAsync(It.IsAny<string>()))
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _tagDal.DeleteTag(tag);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task DeleteTag_Failure_ReturnsFalse()
        {
            // Arrange
            var tag = new Tag { TagID = 1, TagName = "Tag1" };
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            _httpClientWrapperMock.Setup(client => client.DeleteAsync(It.IsAny<string>()))
                                  .ReturnsAsync(httpResponseMessage);

            // Act
            var result = await _tagDal.DeleteTag(tag);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
