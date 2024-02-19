using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using CapstoneGroup2.Desktop.Dal;
using CapstoneGroup2.Desktop.Model;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;

namespace CapstoneGroup2.Desktop.ViewModel
{
    /// <summary>
    ///     Viewmodel for interactions with source
    /// </summary>
    public class SourceViewModel
    {
        #region Data members

        /// <summary>
        ///     The application storage helper
        /// </summary>
        private readonly ApplicationDataStorageHelper _applicationStorageHelper;

        /// <summary>
        ///     The source dal
        /// </summary>
        private readonly SourceDal _sourceDal;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the sources.
        /// </summary>
        /// <value>
        ///     The sources.
        /// </value>
        public List<Source> sources { get; set; }

        /// <summary>
        ///     Gets or sets the current source.
        /// </summary>
        /// <value>
        ///     The current source.
        /// </value>
        public Source currentSource { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="SourceViewModel" /> class.
        /// </summary>
        public SourceViewModel()
        {
            this._applicationStorageHelper = new ApplicationDataStorageHelper(ApplicationData.Current);
            this._sourceDal = new SourceDal();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the sources.
        /// </summary>
        /// <returns>Null if no user, the sources from db otherwise</returns>
        public async Task<IEnumerable<Source>> GetSources()
        {
            var userSerialized = this._applicationStorageHelper.Read<string>("user");

            if (userSerialized == null)
            {
                return null; // TODO: Throw exception
            }

            var user = JsonConvert.DeserializeObject<User>(userSerialized);
            return await this._sourceDal.GetSourcesForUser(user);
        }

        /// <summary>
        ///     Adds the new source.
        /// </summary>
        /// <param name="newSource">The new source to add.</param>
        /// <returns>True if success, null if no user, false otherwise</returns>
        public async Task<bool?> addNewSource(Source newSource)
        {
            var userSerialized = this._applicationStorageHelper.Read<string>("user");

            if (userSerialized == null)
            {
                return null; // TODO: Throw exception
            }

            var user = JsonConvert.DeserializeObject<User>(userSerialized);

            return await this._sourceDal.AddSourceForUser(user, newSource);
        }

        #endregion
    }
}