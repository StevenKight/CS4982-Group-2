using System;
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
    ///     Viewmodel for Notes interactions
    /// </summary>
    public class NotesViewModel
    {
        #region Data members

        /// <summary>
        ///     The application storage helper
        /// </summary>
        private readonly ApplicationDataStorageHelper _applicationStorageHelper;

        /// <summary>
        ///     The notes dal
        /// </summary>
        private readonly NotesDal notesDal;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotesViewModel" /> class.
        /// </summary>
        public NotesViewModel()
        {
            this._applicationStorageHelper = new ApplicationDataStorageHelper(ApplicationData.Current);
            this.notesDal = new NotesDal();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotesViewModel" /> class.
        /// </summary>
        /// <param name="notesDal">The notes dal.</param>
        public NotesViewModel(NotesDal notesDal)
        {
            this.notesDal = notesDal;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the source notes.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///     notes if success, null otherwise
        /// </returns>
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

        /// <summary>
        ///     Adds the new note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns>
        ///     True if success, null if no user, False otherwise
        /// </returns>
        public async Task<bool?> AddNewNote(Note note)
        {
            var userSerialized = this._applicationStorageHelper.Read<string>("user");

            if (userSerialized == null)
            {
                return null;
            }

            var user = JsonConvert.DeserializeObject<User>(userSerialized);

            return await this.notesDal.createNewNote(user, note);
        }

        /// <summary>
        ///     Updates the note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns>
        ///     True if success, null if no user, False otherwise
        /// </returns>
        public async Task<bool?> updateNote(Note note)
        {
            var userSerialized = this._applicationStorageHelper.Read<string>("user");

            if (userSerialized == null)
            {
                return null;
            }

            if (note != null)
            {
                try
                {
                    var user = JsonConvert.DeserializeObject<User>(userSerialized);
                    return await this.notesDal.UpdateNote(note, user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating note: {ex.Message}");
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        ///     Deletes the note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns>
        ///     True if success, null if no user, False otherwise
        /// </returns>
        public async Task<bool> DeleteNote(Note note)
        {
            if (note != null)
            {
                try
                {
                    return await this.notesDal.DeleteNote(note);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting note: {ex.Message}");
                    return false;
                }
            }

            return false;
        }

        #endregion
    }
}