using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Model;

namespace CapstoneGroup2.Desktop.Dal
{
    public class NotesDal
    {
        #region Data members

        private static readonly string BaseUrl = "https://localhost:7048";

        private readonly HttpClient client;

        #endregion

        #region Constructors

        public NotesDal()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(BaseUrl);
        }

        public NotesDal(HttpClient client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri(BaseUrl);
        }

        #endregion

        #region Methods

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

        #endregion
    }
}