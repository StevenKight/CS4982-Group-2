using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Library.Mocks;
using CapstoneGroup2.Desktop.Library.Model;

namespace CapstoneGroup2.Desktop.Library.Dal
{
    /// <summary>
    /// </summary>
    public class UserDal
    {
        #region Data members

        /// <summary>
        ///     The base URL
        /// </summary>
        private static readonly string BaseUrl = "https://localhost:7048";

        /// <summary>
        ///     The client
        /// </summary>
        private readonly IHttpClientWrapper client;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserDal" /> class.
        /// </summary>
        public UserDal()
        {
            this.client = new HttpClientWrapper(new HttpClient());

            this.client.BaseAddress = new Uri(BaseUrl);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserDal" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public UserDal(IHttpClientWrapper client)
        {
            this.client = client;

            this.client.BaseAddress = new Uri(BaseUrl);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Logins the specified username.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>
        ///     <br />
        /// </returns>
        public async Task<User> Login(User user)
        {
            if (user == null ||
                string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentOutOfRangeException();
            }

            var response = await this.client.PostAsJsonAsync("/Login", user);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadFromJsonAsync<User>();
                return user;
            }

            return null;
        }

        /// <summary>
        ///     Creates a new account using given user.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>The new user that was created if successful, null otherwise</returns>
        public async Task<User> CreateAccount(User user)
        {
            if (user == null ||
                string.IsNullOrWhiteSpace(user.Username) ||
                string.IsNullOrWhiteSpace(user.Password))
            {
                throw new ArgumentOutOfRangeException();
            }

            var response = await this.client.PostAsJsonAsync("/Sign-up", user);

            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadFromJsonAsync<User>();
                return user;
            }

            return response.StatusCode switch
            {
                HttpStatusCode.Conflict => throw new Exception("Username already exists"),
                HttpStatusCode.BadRequest => throw new Exception("Invalid username or password"),
                _ => null
            };
        }

        #endregion
    }
}