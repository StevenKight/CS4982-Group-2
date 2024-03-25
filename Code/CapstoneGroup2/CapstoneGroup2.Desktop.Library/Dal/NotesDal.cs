using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Library.Mocks;
using CapstoneGroup2.Desktop.Library.Model;
using Newtonsoft.Json;

namespace CapstoneGroup2.Desktop.Library.Dal
{
    /// <summary>
    /// </summary>
    public class NotesDal
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
        ///     Initializes a new instance of the <see cref="NotesDal" /> class.
        /// </summary>
        public NotesDal()
        {
            this.client = new HttpClientWrapper(new HttpClient());
            this.client.BaseAddress = new Uri(BaseUrl);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotesDal" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public NotesDal(IHttpClientWrapper client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri(BaseUrl);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the user source notes.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="source">The source.</param>
        /// <returns>Enumerable of notes in db or null if none</returns>
        public async Task<IEnumerable<Note>> GetUserSourceNotes(User user, Source source)
        {
            var response = await this.client.GetAsync($"/Notes/{source.SourceId}-{user.Username}");
            if (response.IsSuccessStatusCode)
            {
                var notes = await response.Content.ReadFromJsonAsync<IEnumerable<Note>>();
                return notes;
            }

            return null;
        }

        /// <summary>
        ///     Creates the new note.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="note">The note.</param>
        /// <returns>true if successful, false otherwise</returns>
        public async Task<bool> createNewNote(User user, Note note)
        {
            note.NoteDate = DateTime.Now;
            note.Username = user.Username;
            var jsonData = JsonConvert.SerializeObject(note);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await this.client.PostAsync($"/Notes/{user.Username}", content);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        ///     Updates the note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <param name="user">The user.</param>
        /// <returns>true if successful, false otherwise</returns>
        public async Task<bool> UpdateNote(Note note, User user)
        {
            var jsonData = JsonConvert.SerializeObject(note);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await this.client.PutAsync($"/Notes/{user.Username}", content);

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        ///     Deletes the note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns>true if successful, false otherwise</returns>
        public async Task<bool> DeleteNote(Note note)
        {
            var response = await this.client.DeleteAsync($"/Notes/{note.NoteId}");

            return response.IsSuccessStatusCode;
        }

        #endregion
    }
}