using Microsoft.Extensions.Options;
using ResourceBookingAPI.Configuration;
using ResourceBookingAPI.Interfaces.Services;
using System.Text;
using System.Text.Json;

namespace ResourceBookingAPI.Services
{
    /// <summary>
    /// Service for handling operations related to GitHub CDN.
    /// Includes methods for uploading and deleting files on the GitHub CDN.
    /// </summary>
    public class GitHubCdnService : ICdnService
    {
        private readonly HttpClient _httpClient;
        private readonly GitHubCdnConfig _gitHubCdnConfig;

        /// <summary>
        /// Initializes the GitHubCdnService with HTTP client and configuration settings.
        /// </summary>
        /// <param name="httpClientFactory">Factory used to create HTTP clients.</param>
        /// <param name="gitHubCdnConfig">Configuration settings for GitHub CDN.</param>
        public GitHubCdnService(IHttpClientFactory httpClientFactory, IOptions<GitHubCdnConfig> gitHubCdnConfig)
        {
            _gitHubCdnConfig = gitHubCdnConfig.Value;

            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _gitHubCdnConfig.PAT);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "ResourceBookingApp/1.0");
        }

        /// <summary>
        /// Uploads a file to the GitHub CDN.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <returns>A URL of the uploaded file if successful, otherwise null.</returns>
        public async Task<string?> Upload(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            var fileContentBase64 = Convert.ToBase64String(memoryStream.ToArray());

            var commitData = new { message = $"Upload {file.FileName}", content = fileContentBase64 };
            var commitDataJson = JsonSerializer.Serialize(commitData);
            var content = new StringContent(commitDataJson, Encoding.UTF8, "application/json");

            var apiUrl = $"{_gitHubCdnConfig.ApiURL}{file.FileName}";

            var response = await _httpClient.PutAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
                return $"{_gitHubCdnConfig.PagesURL}{file.FileName}";

            return null;
        }

        /// <summary>
        /// Deletes a file from the GitHub CDN.
        /// </summary>
        /// <param name="filePath">The URL or path of the file to delete.</param>
        /// <returns>True if the file was deleted successfully, otherwise false.</returns>
        public async Task<bool> Delete(string filePath)
        {
            var fileName = Path.GetFileName(new Uri(filePath).LocalPath);
            var apiUrl = $"{_gitHubCdnConfig.ApiURL}{fileName}";

            var fileSha = await GetFileSha(fileName);
            if (fileSha == null)
                return false;

            var requestBody = new { message = "Deleting file", sha = fileSha };

            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, apiUrl)
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

        /// <summary>
        /// Retrieves the SHA of a file from the GitHub CDN.
        /// </summary>
        /// <param name="fileName">The name of the file to retrieve the SHA for.</param>
        /// <returns>The SHA of the file if found, otherwise null.</returns>
        private async Task<string?> GetFileSha(string fileName)
        {
            var apiUrl = $"{_gitHubCdnConfig.ApiURL}{fileName}";

            var response = await _httpClient.GetAsync(apiUrl);
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