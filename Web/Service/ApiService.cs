using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CBAdmin.Models;
using Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Web.Models;

namespace CBAdmin.Service
{
    public class ApiService<T> : IApiService<T>
    {
        private readonly string _infraUrl;
        private readonly IHttpClient _apiClient;
        private string _baseUrl;
        private string _personsUrl;
        private string _photoUrl;

        public ApiService(IHttpClient httpClient)
        {
            _infraUrl = Environment.GetEnvironmentVariable("Infra_url");
            if (string.IsNullOrEmpty(_infraUrl))
            {
                throw new Exception("Please provide port for Infrastructure with enviroment Infra_port ");
            }

            _personsUrl = Environment.GetEnvironmentVariable("Persons_url");

            _photoUrl = Environment.GetEnvironmentVariable("Photo_url");

            Console.WriteLine("Listening for Infrastructure on port: " + _infraUrl);
            _apiClient = httpClient;
        }


        public async Task Create(T entity)
        {
            var response = await _apiClient.PostAsync(_personsUrl + _baseUrl, entity);
            response.EnsureSuccessStatusCode();
        }



        public async Task Delete(string id)
        {
            var response = await _apiClient.DeleteAsync(_personsUrl + _baseUrl + "/" + id);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T> Get(string id)
        {
            var dataString = await _apiClient.GetStringAsync(_personsUrl + _baseUrl + "/" + id);
            return JsonConvert.DeserializeObject<T>(dataString);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            var dataString = await _apiClient.GetStringAsync(_personsUrl + _baseUrl);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(dataString);
        }


        public async Task<IEnumerable<T>> GetAll(string searchstring)
        {
            var dataString = await _apiClient.GetStringAsync(_personsUrl + _baseUrl + "?search=" + searchstring);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(dataString);
        }

        public async Task Write(T student)
        {
            var response = await _apiClient.PutAsync(_personsUrl + _baseUrl, student);
            response.EnsureSuccessStatusCode();
        }

        public void SetBaseUrl(string url)
        {
            _baseUrl = url;
        }

        public async Task<string> GetSystemInformation()
        {

            string url = _infraUrl + "systemsettings/";
            Console.WriteLine(url);

            var dataString = await _apiClient.GetStringAsync(url);

            return dataString;
        }

        public async Task<string> UploadImage(string id, IFormFile avatar)
        {

            var dataString = await _apiClient.PostAsync(_photoUrl + id, avatar);
            return dataString;
        }

        public async Task<byte[]> DownloadImage(string id)
        {
            return await _apiClient.GetRawBodyBytesAsync(_photoUrl + id);
        }
    }
}
