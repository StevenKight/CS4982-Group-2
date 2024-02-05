using Desktop_Client.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Desktop_Client_Tests.Mocks;

namespace Desktop_Client.Dal
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDal
    {
        /// <summary>
        /// The client
        /// </summary>
        private IHttpClientWrapper client;

        /// <summary>
        /// The base URL
        /// </summary>
        private static readonly string baseUrl = "https://localhost:7041/";


        /// <summary>
        /// Initializes a new instance of the <see cref="UserDal"/> class.
        /// </summary>
        public UserDal()
        {
            this.client = new HttpClientWrapper(new HttpClient());
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDal"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public UserDal(IHttpClientWrapper client)
        {
            this.client = client;
        }
        /// <summary>
        /// Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<User?> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;
            var hashedPassword = HashPassword(password);
            client.BaseAddress = new Uri(baseUrl);

            var user = new User();
            var response = await client.GetAsync($"Login/{username}");
            if (response.IsSuccessStatusCode)
            {
                user = await HttpContentJsonExtensions.ReadFromJsonAsync<User>(response.Content);
                return user;
            }
            else
            {
                return null;
            }
            
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
