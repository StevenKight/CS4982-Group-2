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

        private static readonly string baseUrl = "https://localhost:7041/";

        private readonly HttpClient client;

        #endregion

        #region Constructors

        public NotesDal()
        {
            this.client = new HttpClient();
        }

        public NotesDal(HttpClient client)
        {
            this.client = client;
        }

        #endregion

        #region Methods

        public async Task<List<UserNote>> GetUsersNotesAsync()
        {
            this.client.BaseAddress = new Uri(baseUrl);

            var notes = new List<UserNote>();
            var response = await this.client.GetAsync("Notes");
            if (response.IsSuccessStatusCode)
            {
                notes = await response.Content.ReadFromJsonAsync<List<UserNote>>();
            }

            return notes;
        }

        public async Task<bool> createNewNote(Note note, string authToken)
        {
            this.client.BaseAddress = new Uri(baseUrl);

            var jsonData = JsonConvert.SerializeObject(note);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            this.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            var response = await this.client.PostAsync("Notes", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateNote(Note note, string authToken)
        {
            this.client.BaseAddress = new Uri(baseUrl);

            var jsonData = JsonConvert.SerializeObject(note);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            this.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            var response = await this.client.PutAsync("Notes", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteNote(Note note, string authToken)
        {
            this.client.BaseAddress = new Uri(baseUrl);

            var jsonData = JsonConvert.SerializeObject(note);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            this.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            var response = await this.client.DeleteAsync("Notes");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}