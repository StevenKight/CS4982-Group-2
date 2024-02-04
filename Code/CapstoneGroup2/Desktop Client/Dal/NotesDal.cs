using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Desktop_Client.Model;
using Windows.Web.Http;
using HttpClient = System.Net.Http.HttpClient;
using HttpResponseMessage = System.Net.Http.HttpResponseMessage;

namespace Desktop_Client.Dal
{
    public class NotesDal
    {
        #region Data members

        private System.Net.Http.HttpClient client;

        private static readonly string baseUrl = "https://localhost:7041/";

        #endregion

        public NotesDal()
        {
            this.client = new HttpClient();
        }

        public NotesDal(HttpClient client)
        {
            this.client = client;
        }

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

        public async  Task<bool> createNewNote(Note note, string authToken)
        {
            client.BaseAddress = new Uri(baseUrl);

            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(note);

            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            HttpResponseMessage response = await this.client.PostAsync("Notes", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateNote(Note note, string authToken)
        {
            client.BaseAddress = new Uri(baseUrl);

            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(note);

            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            this.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            HttpResponseMessage response = await this.client.PutAsync("Notes", content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteNote(Note note, string authToken)
        {
            this.client.BaseAddress = new Uri(baseUrl);

            string jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(note);

            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            this.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");

            HttpResponseMessage response = await this.client.DeleteAsync("Notes");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}