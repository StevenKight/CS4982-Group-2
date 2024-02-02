using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Desktop_Client.Model;

namespace Desktop_Client.Dal
{
    public class NotesDal
    {
        #region Data members

        private static readonly HttpClient client = new HttpClient();

        private static readonly string baseUrl = "https://localhost:7041/";

        #endregion

        #region Methods

        public static async Task<List<UserNote>> GetUsersNotesAsync()
        {
            client.BaseAddress = new Uri(baseUrl);

            var notes = new List<UserNote>();
            var response = await client.GetAsync("Notes");
            if (response.IsSuccessStatusCode)
            {
                notes = await response.Content.ReadFromJsonAsync<List<UserNote>>();
            }

            return notes;
        }

        #endregion
    }
}