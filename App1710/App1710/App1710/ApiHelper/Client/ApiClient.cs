using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using App1710.ApiHelper.Model;

namespace App1710.ApiHelper.Client
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient httpClient;
        private readonly ITokenContainer _iTokenContainer;

        public ApiClient(HttpClient httpClient, ITokenContainer iTokenContainer)
        {
            this.httpClient = httpClient;
            this._iTokenContainer = iTokenContainer;
        }

        public async Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
        {
            AddToken();
            using (var content = new FormUrlEncodedContent(values))
            {
                var query = await content.ReadAsStringAsync();
                var requestUriWithQuery = string.Concat(requestUri, "?", query);
                var response = await httpClient.GetAsync(requestUriWithQuery);
                return response;
            }
        }

        public async Task<HttpResponseMessage> PostFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
        {
            using (var content = new FormUrlEncodedContent(values))
            {
                var response = await httpClient.PostAsync(requestUri, content);
                return response;
            }
        }

        public async Task<HttpResponseMessage> PostJsonEncodedContent<T>(string requestUri, T content) where T : ApiModel
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            AddToken();
            var response = await httpClient.PostAsJsonAsync(requestUri, content);
            return response;
        }

        private void AddToken()
        {
            if (_iTokenContainer.ApiToken != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _iTokenContainer.ApiToken.ToString());
            }
        }
    }
}
