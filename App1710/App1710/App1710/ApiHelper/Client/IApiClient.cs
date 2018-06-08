using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace App1710.ApiHelper.Client
{
    public interface IApiClient
    {
        Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);
        Task<HttpResponseMessage> PostFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);
        Task<HttpResponseMessage> PostJsonEncodedContent<T>(string requestUri, T content) where T : Model.ApiModel;
    }
}
