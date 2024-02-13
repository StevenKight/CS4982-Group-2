using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using CapstoneGroup2.Desktop.Dal;
using CapstoneGroup2.Desktop.Model;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;

namespace CapstoneGroup2.Desktop.ViewModel
{
    public class SourceViewModel
    {
        #region Data members

        private readonly ApplicationDataStorageHelper _applicationStorageHelper;
        private readonly SourceDal _sourceDal;

        public List<Source> sources { get; set; }

        public Source currentSource { get; set; }

        #endregion

        #region Constructors

        public SourceViewModel()
        {
            this._applicationStorageHelper = new ApplicationDataStorageHelper(ApplicationData.Current);
            this._sourceDal = new SourceDal();
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<Source>> GetSources()
        {
            var userSerialized = this._applicationStorageHelper.Read<string>("user");

            if (userSerialized == null)
            {
                return null;
            }

            var user = JsonConvert.DeserializeObject<User>(userSerialized);
            return await this._sourceDal.GetSourcesForUser(user);
        }

        public async Task<bool?> addNewSource(StorageFile storage)
        {
            var userSerialized = this._applicationStorageHelper.Read<string>("user");

            if (userSerialized == null)
            {
                return null;
            }

            var user = JsonConvert.DeserializeObject<User>(userSerialized);

            return await this._sourceDal.AddSourceForUser(user, storage);
        }


        #endregion
    }
}