﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Model;

namespace CapstoneGroup2.Desktop.Dal
{
    /// <summary>
    ///     DAL class for Sources
    /// </summary>
    public class SourceDal
    {
        #region Data members

        private const string BaseUrl = "https://localhost:7048";
        private readonly HttpClient client;

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="SourceDal" /> class.</summary>
        public SourceDal()
        {
            this.client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        /// <summary>Initializes a new instance of the <see cref="SourceDal" /> class.</summary>
        public SourceDal(HttpClient client)
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

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Response Content:");
            Console.WriteLine(content);
            return false;
        }

        #endregion
    }
}