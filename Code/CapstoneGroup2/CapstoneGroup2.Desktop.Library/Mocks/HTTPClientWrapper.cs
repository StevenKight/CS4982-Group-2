using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CapstoneGroup2.Desktop.Library.Model;

namespace CapstoneGroup2.Desktop.Library.Mocks
{
    public interface IHttpClientWrapper
    {
        #region Properties

        Uri BaseAddress { get; set; }
        HttpRequestHeaders DefaultRequestHeaders { get; }

        #endregion

        #region Methods

        Task<HttpResponseMessage> GetAsync(string requestUri);
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content);
        Task<HttpResponseMessage> DeleteAsync(string requestUri);
        Task<HttpResponseMessage> PostAsJsonAsync(string login, User user);

        #endregion
    }

    public class HttpClientWrapper : IHttpClientWrapper, IDisposable
    {
        #region Data members

        private readonly HttpClient _httpClient;

        #endregion

        #region Properties

        public Uri BaseAddress
        {
            get => this._httpClient.BaseAddress;
            set => this._httpClient.BaseAddress = value;
        }

        public HttpRequestHeaders DefaultRequestHeaders => this._httpClient.DefaultRequestHeaders;

        #endregion

        #region Constructors

        public HttpClientWrapper()
        {
            this._httpClient = new HttpClient();
        }

        public HttpClientWrapper(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        #endregion

        #region Methods

        public void Dispose()
        {
            this._httpClient.Dispose();
        }

        public virtual Task<HttpResponseMessage> PostAsJsonAsync(string url, User user)
        {
            return this._httpClient.PostAsJsonAsync(url, user);
        }

        public virtual Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return this._httpClient.GetAsync(requestUri);
        }

        public virtual Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return this._httpClient.PostAsync(requestUri, content);
        }

        public virtual Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content)
        {
            return this._httpClient.PutAsync(requestUri, content);
        }

        public virtual Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return this._httpClient.DeleteAsync(requestUri);
        }

        #endregion
    }
}