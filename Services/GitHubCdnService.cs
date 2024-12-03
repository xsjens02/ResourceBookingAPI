using Microsoft.Extensions.Options;
using ResourceBookingAPI.Configuration;
using ResourceBookingAPI.Interfaces.Services;
using System.Text;
using System.Text.Json;

namespace ResourceBookingAPI.Services
{
    public class GitHubCdnService : ICdnService
    {
        private readonly HttpClient _httpClient;
        private readonly GitHubCdnConfig _gitHubCdnConfig;
        public GitHubCdnService(IHttpClientFactory httpClientFactory, IOptions<GitHubCdnConfig> gitHubCdnConfig)
        {
            _gitHubCdnConfig = gitHubCdnConfig.Value;

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _gitHubCdnConfig.PAT);
        }
        public async Task<string?> Upload(IFormFile file)
        {
            using var form = new MultipartFormDataContent();
            using var stream = file.OpenReadStream();
            var fileContent = new StreamContent(stream);
            form.Add(fileContent, "file", file.FileName);

            var postUrl = $"{_gitHubCdnConfig.ApiURL}{file.FileName}";

            var response = await _httpClient.PostAsync(postUrl, form);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            return null;
        }
        public async Task<bool> Delete(string url)
        {
            var fileName = Path.GetFileName(new Uri(url).LocalPath);
            var deleteUrl = $"{_gitHubCdnConfig.ApiURL}{fileName}";

            var sha = await GetFileSha(fileName);
            if (sha == null)
                return false;

            var requestBody = new { message = "Deleting file", Sha = sha };

            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, deleteUrl)
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                )
            };

            var response = await _httpClient.SendAsync(requestMessage);
            return response.IsSuccessStatusCode;
        }

        private async Task<string?> GetFileSha(string fileName)
        {
            var fileUrl = $"{_gitHubCdnConfig.ApiURL}{fileName}";

            var response = await _httpClient.GetAsync(fileUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(content);
                return jsonResponse.GetProperty("sha").GetString();
            }

            return null;
        }
    }
}
