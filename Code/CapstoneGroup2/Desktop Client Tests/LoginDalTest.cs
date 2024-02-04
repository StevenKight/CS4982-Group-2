using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Newtonsoft.Json;


namespace Desktop_Client_Tests
{

        [TestFixture]
        public class UserDalTests
        {
            [Test]
            public async Task Login_Successful()
            {
                // Arrange
                var fakeHttpClient = new Mock<HttpClient>();
                var userDal = new UserDal(fakeHttpClient.Object);

                string username = "testuser";
                string password = "testpassword";

                // Assume that you have a user object and JSON content here
                var fakeUser = new User { /* Populate user properties */ };

                fakeHttpClient.Setup(client =>
                    client.GetAsync($"Login/{username}"))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Content = new StringContent(JsonSerializer.Serialize(fakeUser), Encoding.UTF8, "application/json")
                    });

                // Act
                var result = await userDal.Login(username, password);

                // Assert
                Assert.IsNotNull(result);
                // Add more assertions based on your expected behavior
            }

            [Test]
            public async Task Login_Unsuccessful()
            {
                // Arrange
                var fakeHttpClient = new Mock<HttpClient>();
                var userDal = new UserDal(fakeHttpClient.Object);

                string username = "testuser";
                string password = "testpassword";

                fakeHttpClient.Setup(client =>
                    client.GetAsync($"Login/{username}"))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound // or any other error status code
                    });

                // Act
                var result = await userDal.Login(username, password);

                // Assert
                Assert.IsNull(result);
                // Add more assertions based on your expected behavior
            }
        }
    }
}
