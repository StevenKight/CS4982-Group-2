using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Data;
using CapstoneGroup2.Desktop.Model;
using Newtonsoft.Json.Linq;
using Windows.Storage;

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
            var response = await this.client.GetAsync($"/source/{user.Username}");
            if (response.IsSuccessStatusCode)
            {
                var sources = await response.Content.ReadFromJsonAsync<IEnumerable<Source>>();
                return sources;
            }

            return null;
        }

        public async Task<bool> AddSourceForUser(User user, StorageFile storageFile)
        {
            Source source = new Source();
            source.UpdatedAt = DateTime.Now;
            source.CreatedAt = DateTime.Now;
            source.Username = user.Username;
            source.IsLink = false;
            source.Link = "";
            source.Description = "pdf";
            source.Type = "pdf";
            source.Name = storageFile.Name;
            source.Content = await DataManager.FileToBinary(storageFile);
            var response = await this.client.PostAsync($"/source/{user.Username}",new StringContent(JsonSerializer.Serialize(source), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return true;
            } else {
                Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");

                // Read and display the response content
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response Content:");
                Console.WriteLine(content);
                return false; 
            }
        }

        #endregion
    }
}