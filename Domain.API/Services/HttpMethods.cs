using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Domain.API.Services
{
    public static class HttpMethodAction
    {
        public static async Task<T> Get<T>(this HttpClient _httpClient, string url)
        {
            return await (await _httpClient.GetAsync(url)).ClientReturnResponse<T>();
        }

        public static async Task<HttpResponseMessage> Get(this HttpClient _httpClient, string url)
        {
            return await _httpClient.GetAsync(url);
        }
        public static async Task<T> Patch<T>(this HttpClient _httpClient, string url, object data)
        {
            return await (await _httpClient.PatchAsync(url, DataToStringConvert(data))).ClientReturnResponse<T>();
        }
        public static async Task<T> Post<T>(this HttpClient _httpClient, string url, object data)
        {
            return await (await _httpClient.PostAsync(url, DataToStringConvert(data))).ClientReturnResponse<T>();
        }
        public static async Task<HttpResponseMessage> Post(this HttpClient _httpClient, string url, object data)
        {
            return await _httpClient.PostAsync(url, DataToStringConvert(data));
        }
        public static async Task<T> Put<T>(this HttpClient _httpClient, string url, object data)
        {
            return await (await _httpClient.PutAsync(url, DataToStringConvert(data))).ClientReturnResponse<T>();
        }
        public static async Task Delete<T>(this HttpClient _httpClient, string url)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync(url);
            if (response.IsSuccessStatusCode)
                return;
            throw new Exception("Delete not accepted");
        }
        public static async Task Delete(this HttpClient _httpClient, string url, object data)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(_httpClient.BaseAddress + url),
                Content = DataToStringConvert(data)
            };
            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
                return;
            throw new Exception("Delete not accepted");
        }
        private static StringContent DataToStringConvert(object data)
        {
            return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        }
        private static async Task<T> ClientReturnResponse<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
            return default(T);

        }
    }
}