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
    public class UserViewModel
    {
        #region Data members

        private readonly ApplicationDataStorageHelper _applicationStorageHelper;
        private readonly UserDal _userDal;

        #endregion

        #region Constructors

        public UserViewModel()
        {
            this._userDal = new UserDal();
            this._applicationStorageHelper = new ApplicationDataStorageHelper(ApplicationData.Current);
        }

        #endregion

        #region Methods

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

        public bool ValidateAuthorization()
        {
            var userSerialized = this._applicationStorageHelper.Read<string>("user");

            return userSerialized != null;
        }

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