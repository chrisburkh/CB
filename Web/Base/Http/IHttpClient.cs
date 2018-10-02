

using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace Http
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string uri);

        Task<byte[]> GetRawBodyBytesAsync(string uri);

        Task<HttpResponseMessage> PostAsync<T>(string uri, T item);

        Task<string> PostAsync(string uri, IFormFile file);
        Task<HttpResponseMessage> DeleteAsync(string uri);
        Task<HttpResponseMessage> PutAsync<T>(string uri, T item);
    }
}
