using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Model;
using Newtonsoft.Json;

namespace CapstoneGroup2.Desktop.Dal
{
    public class NotesDal
    {
        #region Data members

        private static readonly string baseUrl = "https://localhost:7048";

        private readonly HttpClient client;

        #endregion

        #region Constructors

        public NotesDal()
        {
            this.client = new HttpClient();
            this.client.BaseAddress = new Uri(baseUrl);
        }

        public NotesDal(HttpClient client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri(baseUrl);
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

        public async Task<bool> createNewNote(Note note, string authToken)
        {
            var jsonData = JsonConvert.SerializeObject(note);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            this.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            var response = await this.client.PostAsync("Notes", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateNote(Note note, string authToken)
        {
            var jsonData = JsonConvert.SerializeObject(note);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            this.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            var response = await this.client.PutAsync("Notes", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteNote(Note note, string authToken)
        {
            var jsonData = JsonConvert.SerializeObject(note);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            this.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            var response = await this.client.DeleteAsync("Notes");

            return response.IsSuccessStatusCode;
        }

        #endregion
    }
}