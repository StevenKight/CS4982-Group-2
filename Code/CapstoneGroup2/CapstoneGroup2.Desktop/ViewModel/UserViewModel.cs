using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using CapstoneGroup2.Desktop.Dal;
using CapstoneGroup2.Desktop.Model;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;

namespace CapstoneGroup2.Desktop.ViewModel
{
    /// <summary>
    ///     Viewmodel for interactions is Users
    /// </summary>
    public class UserViewModel
    {
        #region Data members

        /// <summary>
        ///     The application storage helper
        /// </summary>
        private readonly ApplicationDataStorageHelper _applicationStorageHelper;

        /// <summary>
        ///     The user dal
        /// </summary>
        private readonly UserDal _userDal;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="UserViewModel" /> class.
        /// </summary>
        public UserViewModel()
        {
            this._userDal = new UserDal();
            this._applicationStorageHelper = new ApplicationDataStorageHelper(ApplicationData.Current);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<User> Login(string username, string password)
        {
            var user = new User
            {
                Username = username,
                Password = password
            };

            var validUser = await this._userDal.Login(user);

            if (validUser != null)
            {
                this._applicationStorageHelper.Save("user", JsonConvert.SerializeObject(validUser));
            }
            else
            {
                this._applicationStorageHelper.Clear();
            }

            return validUser;
        }

        /// <summary>
        ///     Gets the saved user.
        /// </summary>
        /// <returns> The locally saved user</returns>
        public User getSavedUser()
        {
            var userSerialized = this._applicationStorageHelper.Read<string>("user");
            var user = JsonConvert.DeserializeObject<User>(userSerialized);

            return user;
        }

        /// <summary>
        ///     Validates the authorization.
        /// </summary>
        /// <returns>True if authorized, false otherwise</returns>
        public bool ValidateAuthorization()
        {
            var userSerialized = this._applicationStorageHelper.Read<string>("user");

            return userSerialized != null;
        }

        /// <summary>
        ///     Hashes the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>hashed password</returns>
        private static string hashPassword(string password)
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