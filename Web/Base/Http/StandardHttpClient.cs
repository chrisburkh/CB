

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;


namespace Http
{
    public class StandardHttpClient : IHttpClient
    {
        private static readonly HttpClient Client = new HttpClient();

        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
            var response = await Client.SendAsync(requestMessage);
            return response;
        }

        public async Task<string> GetStringAsync(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await Client.SendAsync(requestMessage);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string uri, T item)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json")
            };

            var response = await Client.SendAsync(requestMessage);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }

            return response;
        }

        public async Task<string> PostAsync(string uri, IFormFile file)
        {

            byte[] data;
            using (var br = new BinaryReader(file.OpenReadStream()))
                data = br.ReadBytes((int)file.OpenReadStream().Length);

            ByteArrayContent bytes = new ByteArrayContent(data);

            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            multiContent.Add(bytes, "file", file.FileName);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = multiContent
            };

            var response = await Client.SendAsync(requestMessage);

            Console.WriteLine("Upload Status Code: " + response.StatusCode);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string uri, T item)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(item), System.Text.Encoding.UTF8, "application/json")
            };

            var response = await Client.SendAsync(requestMessage);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException();
            }

            return response;
        }

        public async Task<byte[]> GetRawBodyBytesAsync(string uri)
        {

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await Client.SendAsync(requestMessage);
            //return await response.Content.ReadAsStringAsync();

            using (var ms = new MemoryStream())
            {
                await response.Content.CopyToAsync(ms);

                return ms.ToArray();

            }
        }

    }
}
