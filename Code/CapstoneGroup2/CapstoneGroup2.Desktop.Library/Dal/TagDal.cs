using CapstoneGroup2.Desktop.Library.Mocks;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Library.Model;
using Newtonsoft.Json;

namespace CapstoneGroup2.Desktop.Library.Dal
{
    public class TagDal
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
        public TagDal()
        {
            this.client = new HttpClientWrapper(new HttpClient());
            this.client.BaseAddress = new Uri(BaseUrl);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NotesDal" /> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public TagDal(IHttpClientWrapper client)
        {
            this.client = client;
            this.client.BaseAddress = new Uri(BaseUrl);
        }
        #endregion

        public async Task<IEnumerable<Tag>> GetTags(User user)
        {
            var response = await this.client.GetAsync($"/Tag/{user.Username}");
            if (response.IsSuccessStatusCode)
            {
                var tags = await response.Content.ReadFromJsonAsync<IEnumerable<Tag>>();
                return tags;
            }

            return null;
        }

        public async Task<IEnumerable<Note>> GetTagsById(User user, Tag tag)
        {
            var response = await this.client.GetAsync($"/Notes/{tag.TagID}-{user.Username}");
            if (response.IsSuccessStatusCode)
            {
                var notes = await response.Content.ReadFromJsonAsync<IEnumerable<Note>>();
                return notes;
            }

            return null;
        }

        public async Task<bool> createNewNote(User user, Tag tag)
        {
            var jsonData = JsonConvert.SerializeObject(tag);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await this.client.PostAsync($"/Notes/{user.Username}", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTag(Tag tag, User user)
        {
            var jsonData = JsonConvert.SerializeObject(tag);

            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await this.client.PutAsync($"/Tag/{user.Username}", content);

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> DeleteTag(Tag tag)
        {
            var response = await this.client.DeleteAsync($"/Tag/{tag.TagID}");

            return response.IsSuccessStatusCode;
        }

    }
}
