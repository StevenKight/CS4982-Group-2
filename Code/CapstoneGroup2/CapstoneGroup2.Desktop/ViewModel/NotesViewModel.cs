using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using CapstoneGroup2.Desktop.Dal;
using CapstoneGroup2.Desktop.Model;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;

namespace CapstoneGroup2.Desktop.ViewModel
{
    public class NotesViewModel
    {
        #region Data members

        private readonly ApplicationDataStorageHelper _applicationStorageHelper;
        private readonly NotesDal notesDal;

        #endregion

        #region Constructors

        public NotesViewModel()
        {
            this._applicationStorageHelper = new ApplicationDataStorageHelper(ApplicationData.Current);
            this.notesDal = new NotesDal();
        }

        public NotesViewModel(NotesDal notesDal)
        {
            this.notesDal = notesDal;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<Note>> GetSourceNotes(Source source)
        {
            var userSerialized = this._applicationStorageHelper.Read<string>("user");

            if (userSerialized == null)
            {
                return null;
            }

            var user = JsonConvert.DeserializeObject<User>(userSerialized);
            return await this.notesDal.GetUserSourceNotes(user, source);
        }

        #endregion
    }
}