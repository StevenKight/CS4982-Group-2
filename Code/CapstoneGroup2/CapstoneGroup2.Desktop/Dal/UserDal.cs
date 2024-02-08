using System;
using System.Net.Http;
using System.Net.Http.Json;
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
        private static readonly string BaseUrl = "https://localhost:7048";

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

            this.client.BaseAddress = new Uri(BaseUrl);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserDal" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public UserDal(HttpClient client)
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
                string.IsNullOrEmpty(user.Username) ||
                string.IsNullOrEmpty(user.Password))
            {
                return null;
            }

            var response = await this.client.PostAsJsonAsync("/login", user);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadFromJsonAsync<User>();
                return user;
            }

            return null;
        }

        #endregion
    }
}