using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Model;

namespace CapstoneGroup2.Desktop.Dal
{
    public class SourceDal
    {
        #region Data members

        private const string BaseUrl = "https://localhost:7048";
        private readonly HttpClient client;

        #endregion

        #region Constructors

        public SourceDal()
        {
            this.client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public SourceDal(HttpClient client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri(BaseUrl);
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<Source>> GetSourcesForUser(User user)
        {
            var response = await this.client.GetAsync($"/Source/{user.Username}");
            if (response.IsSuccessStatusCode)
            {
                var sources = await response.Content.ReadFromJsonAsync<IEnumerable<Source>>();
                return sources;
            }

            return null;
        }

        #endregion
    }
}