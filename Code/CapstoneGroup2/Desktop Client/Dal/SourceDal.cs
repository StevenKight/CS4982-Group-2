using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Net.Http;
using Desktop_Client.Model;
using System.IO;
using Windows.Storage.Streams;

namespace Desktop_Client.Dal
{
    public class SourceDal
    {
        private HttpClient client;

        private static readonly string baseUrl = "https://localhost:7041/";

        public SourceDal()
        {
            this.client = new HttpClient();
        }

        public SourceDal(HttpClient client)
        {
            this.client = client;
        }

        public async Task<List<Source>> getSourcesForUser(User user)
        {
            var sources = new List<Source>();
            var response = await client.GetAsync(new Uri($"Source/{user.Username}"));
            if (response.IsSuccessStatusCode)
            {
                sources = await HttpContentJsonExtensions.ReadFromJsonAsync<List<Source>>(response.Content);
                return sources;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Source>> getSources()
        {
            var sources = new List<Source>();
            var response = await client.GetAsync(new Uri($"Source"));
            if (response.IsSuccessStatusCode)
            {
                sources = await HttpContentJsonExtensions.ReadFromJsonAsync<List<Source>>(response.Content);
                return sources;
            }
            else
            {
                return null;
            }
        }

        public async Task SendPdfToServer(User user, byte[] pdfBytes)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                        ByteArrayContent byteArrayContent = new ByteArrayContent(pdfBytes);

                        // Send the POST request
                        HttpResponseMessage response = await client.PostAsync($"Source/{user.Username}", byteArrayContent);

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

        static async Task DownloadFileAsync(int sourceId, string filePath)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"Source/{sourceId}");

                    if (response.IsSuccessStatusCode)
                    {
                        using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                        {
                            using (FileStream fileStream = File.Create(filePath))
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

    }
}
