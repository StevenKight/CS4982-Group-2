using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Library.Mocks;
using CapstoneGroup2.Desktop.Library.Model;

namespace CapstoneGroup2.Desktop.Library.Dal
{
    /// <summary>
    ///     DAL class for Sources
    /// </summary>
    public class SourceDal
    {
        #region Data members

        private const string BaseUrl = "https://localhost:7048";
        private readonly IHttpClientWrapper client;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="SourceDal" /> class.</summary>
        public SourceDal()
        {
            this.client = new HttpClientWrapper(new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            });
        }

        /// <summary>Initializes a new instance of the <see cref="SourceDal" /> class.</summary>
        public SourceDal(IHttpClientWrapper client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri(BaseUrl);
        }

        #endregion

        #region Methods

        /// <summary>Gets the sources for user.</summary>
        /// <param name="user">The user.</param>
        /// <returns>
        ///     The sources from db or null if none
        /// </returns>
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

        /// <summary>Adds the source for user.</summary>
        /// <param name="user">The user.</param>
        /// <param name="newSource">The new source to add.</param>
        /// <returns>
        ///     True if success, false otherwise
        /// </returns>
        public async Task<bool> AddSourceForUser(User user, Source newSource)
        {
            newSource.Username = user.Username;

            var response = await this.client.PostAsync($"/Source/{user.Username}",
                new StringContent(JsonSerializer.Serialize(newSource), Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Source>> GetSourcesForTags(User user, Tag[] tags)
        {
            var sources = new List<Source>();
            foreach (var tag in tags)
            {
                var response = await this.client.GetAsync($"/Source/{tag}-{user.Username}");
                if (response.IsSuccessStatusCode)
                {
                    var sourcesForTag = await response.Content.ReadFromJsonAsync<IEnumerable<Source>>();
                    foreach (var source in sourcesForTag)
                    {
                        sources.Add(source);
                    }
                    return sources;
                }
            }
            
            return null;
        }

        #endregion
    }
}