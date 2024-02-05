using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Model;

namespace CapstoneGroup2.Desktop.Dal
{
    public class SourceDal
    {
        #region Data members

        private static readonly string baseUrl = "https://localhost:7041/";
        private readonly HttpClient client;

        #endregion

        #region Constructors

        public SourceDal()
        {
            this.client = new HttpClient();
        }

        public SourceDal(HttpClient client)
        {
            this.client = client;
        }

        #endregion

        #region Methods

        public async Task<List<Source>> getSourcesForUser(User user)
        {
            var sources = new List<Source>();
            var response = await this.client.GetAsync(new Uri($"Source/{user.Username}"));
            if (response.IsSuccessStatusCode)
            {
                sources = await response.Content.ReadFromJsonAsync<List<Source>>();
                return sources;
            }

            return null;
        }

        public async Task<List<Source>> getSources()
        {
            var sources = new List<Source>();
            var response = await this.client.GetAsync(new Uri("Source"));
            if (response.IsSuccessStatusCode)
            {
                sources = await response.Content.ReadFromJsonAsync<List<Source>>();
                return sources;
            }

            return null;
        }

        public async Task SendPdfToServer(User user, byte[] pdfBytes)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var byteArrayContent = new ByteArrayContent(pdfBytes);

                    // Send the POST request
                    var response = await client.PostAsync($"Source/{user.Username}", byteArrayContent);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("PDF file uploaded successfully.");
                    }
                    else
                    {
                        // Handle the error (log, display, etc.)
                        Console.WriteLine($"Error uploading PDF file: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, display, etc.)
                Console.WriteLine($"Error uploading PDF file: {ex.Message}");
            }
        }

        private static async Task DownloadFileAsync(int sourceId, string filePath)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"Source/{sourceId}");

                    if (response.IsSuccessStatusCode)
                    {
                        using (var responseStream = await response.Content.ReadAsStreamAsync())
                        {
                            using (var fileStream = File.Create(filePath))
                            {
                                await responseStream.CopyToAsync(fileStream);
                            }
                        }

                        Console.WriteLine($"File downloaded successfully: {filePath}");
                    }
                    else
                    {
                        Console.WriteLine($"Error downloading file: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
            }
        }

        #endregion
    }
}