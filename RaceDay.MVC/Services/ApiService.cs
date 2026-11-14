using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RaceDay.MVC.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;
        public ApiService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("RaceDayAPI");
        }

        public async Task<(bool success, string content)> PostAsync(string endpoint, object data, string token = null)
        {
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            else
                _client.DefaultRequestHeaders.Authorization = null;

            var json = JsonSerializer.Serialize(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(endpoint, httpContent);
            var respString = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, respString);
        }

        public async Task<(bool success, string content)> GetAsync(string endpoint, string token = null)
        {
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            else
                _client.DefaultRequestHeaders.Authorization = null;

            var response = await _client.GetAsync(endpoint);
            var respString = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, respString);
        }

        public async Task<(bool success, string content)> PutAsync(string endpoint, object data, string token = null)
        {
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            else
                _client.DefaultRequestHeaders.Authorization = null;

            var json = JsonSerializer.Serialize(data);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(endpoint, httpContent);
            var respString = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, respString);
        }

        public async Task<(bool success, string content)> DeleteAsync(string endpoint, string token = null)
        {
            if (!string.IsNullOrEmpty(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            else
                _client.DefaultRequestHeaders.Authorization = null;

            var response = await _client.DeleteAsync(endpoint);
            var respString = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, respString);
        }
    }
}