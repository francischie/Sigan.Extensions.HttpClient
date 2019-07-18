using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Sigan.Extensions.HttpClient
{
   public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> GetAsync(this System.Net.Http.HttpClient httpClient, string uri, Action<HttpRequestMessage> preAction = null)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            preAction?.Invoke(httpRequestMessage);
            return httpClient.SendAsync(httpRequestMessage);
        }

        public static async Task<T> GetAsync<T>(this System.Net.Http.HttpClient httpClient, string uri, Action<HttpRequestMessage> preAction = null)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            preAction?.Invoke(httpRequestMessage);
            var response = await httpClient.SendAsync(httpRequestMessage);
            var obj = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
            return obj;
        }


        public static Task<HttpResponseMessage> PostAsJsonAsync<T>(this System.Net.Http.HttpClient httpClient, string uri, T value, Action<HttpRequestMessage> preAction = null)
        {
            return SendAsync(httpClient, HttpMethod.Post, uri, value, preAction);
        }
        
        public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this System.Net.Http.HttpClient httpClient, string uri, T value, Action<HttpRequestMessage> preAction = null)
        {
            return SendAsync(httpClient, HttpMethod.Patch, uri, value, preAction);
        }
        
        public static Task<HttpResponseMessage> PutAsJsonAsync<T>(this System.Net.Http.HttpClient httpClient, string uri, T value, Action<HttpRequestMessage> preAction = null)
        {
            return SendAsync(httpClient, HttpMethod.Put, uri, value, preAction);
        }

        public static Task<HttpResponseMessage> DeleteAsync(this System.Net.Http.HttpClient httpClient, string uri, Action<HttpRequestMessage> preAction = null)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);
            preAction?.Invoke(httpRequestMessage);
            return httpClient.SendAsync(httpRequestMessage);
        }

        private static Task<HttpResponseMessage> SendAsync<T>(this System.Net.Http.HttpClient httpClient, HttpMethod method, string uri, T value, Action<HttpRequestMessage> preAction = null)
        {
            var content = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
            var httpRequestMessage = new HttpRequestMessage(method, uri)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
            preAction?.Invoke(httpRequestMessage);
            return httpClient.SendAsync(httpRequestMessage);
        }
    }
}