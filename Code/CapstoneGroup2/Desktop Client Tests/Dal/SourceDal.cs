using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Desktop_Client.Model;
using System.IO;
using Desktop_Client_Tests.Mocks;

namespace Desktop_Client.Dal
{
    public class SourceDal(HttpClientWrapper client)
    {
        private static readonly string baseUrl = "https://localhost:7041/";

        public SourceDal() : this(new HttpClientWrapper(new HttpClient()))
        {
        }

        public async Task<List<Source>> getSourcesForUser(User user)
        {
            var sources = new List<Source>();
            var response = await client.GetAsync($"Source/{user.Username}");
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
            var response = await client.GetAsync($"Source");
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

        public async Task<bool> SendPdfToServer(User user, byte[] pdfBytes)
        {
            try
            {
                ByteArrayContent byteArrayContent = new ByteArrayContent(pdfBytes);

                HttpResponseMessage response = await client.PostAsync($"Source/{user.Username}", byteArrayContent);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("PDF file uploaded successfully.");
                    return true;
                   
                }
                else
                {
                    Debug.WriteLine($"Error uploading PDF file: {response.StatusCode}");
                    return false;
                   
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error uploading PDF file: {ex.Message}");
                return false;
            }
        }

        public async Task<string> DownloadFileAsync(int sourceId, string filePath)
        {
            try
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

                        
                    Debug.WriteLine($"File downloaded successfully: {filePath}");
                    return filePath;
                }
                else
                {
                    Debug.WriteLine($"Error downloading file: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error downloading file: {ex.Message}");
                return null;
            }
        }

    }
}
