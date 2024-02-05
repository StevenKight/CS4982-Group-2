using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Desktop_Client.Dal;
using Desktop_Client.Model;
using NUnit.Framework;
using Moq;
using Desktop_Client_Tests.Mocks;


namespace Desktop_Client_Tests
{

        [TestFixture]
        public class UserDalTests
        {
        [Test]
        public void Login_Successful()
        {
            // Arrange
            var fakeHttpClient = new Mock<HttpClientWrapper>();
            var httpclient = new HttpClient();
            httpclient.BaseAddress = new Uri("https://localhost:7041/");
            string username = "testuser";
            string password = "testpassword";
            var fakeUser = new User { /* Populate user properties */ };

            fakeHttpClient.Setup(client =>
                client.GetAsync($"Login/{username}"))
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(fakeUser), Encoding.UTF8, "application/json")
                });
            var userDal = new UserDal(fakeHttpClient.Object);
            // Act
            var result = userDal.Login(username, password).Result;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
            public void Login_Unsuccessful()
            {
                // Arrange
                var fakeHttpClient = new Mock<HttpClientWrapper>();
                var httpclient = new HttpClient();
                httpclient.BaseAddress = new Uri("https://localhost:7041/");

                string username = "testuser";
                string password = "testpassword";

                fakeHttpClient.Setup(client =>
                    client.GetAsync($"Login/{username}"))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Content = null
                    });
                var userDal = new UserDal(fakeHttpClient.Object);
                // Act
                object? result = userDal.Login(username, password).Result;

                // Assert
                Assert.IsNull(result);
            }
        }
    }
