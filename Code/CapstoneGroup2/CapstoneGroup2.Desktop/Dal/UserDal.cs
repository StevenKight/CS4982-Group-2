using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Model;

namespace CapstoneGroup2.Desktop.Dal
{
    /// <summary>
    /// </summary>
    public class UserDal
    {
        #region Data members

        /// <summary>
        ///     The base URL
        /// </summary>
        private static readonly string baseUrl = "https://localhost:7048";

        /// <summary>
        ///     The client
        /// </summary>
        private readonly HttpClient client;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserDal" /> class.
        /// </summary>
        public UserDal()
        {
            this.client = new HttpClient();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserDal" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public UserDal(HttpClient client)
        {
            this.client = client;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        ///     <br />
        /// </returns>
        public async Task<User> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var hashedPassword = HashPassword(password);
            this.client.BaseAddress = new Uri(baseUrl);

            var user = new User { Username = username, Password = password };
            var response = await this.client.PostAsJsonAsync("/login", user);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadFromJsonAsync<User>();
                return user;
            }

            return null;
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        #endregion
    }
}