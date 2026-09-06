using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Destonize.Services
{
    public static class UpdateService
    {
        private const string UserAgent = "Destonize";

        public static async Task<bool> CheckForUpdatesAsync(string updateSource, string currentVersion)
        {
            if (string.IsNullOrWhiteSpace(updateSource))
                return false;

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
                var json = await client.GetStringAsync(updateSource);
                using var doc = JsonDocument.Parse(json);
                var tag = doc.RootElement.GetProperty("tag_name").GetString()?.TrimStart('v');
                if (string.IsNullOrEmpty(tag)) return false;
                return Version.TryParse(tag, out var latest) && Version.TryParse(currentVersion, out var current) && latest > current;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<string?> GetLatestVersionAsync(string updateSource)
        {
            if (string.IsNullOrWhiteSpace(updateSource))
                return null;

            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
                var json = await client.GetStringAsync(updateSource);
                using var doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty("tag_name").GetString()?.TrimStart('v');
            }
            catch
            {
                return null;
            }
        }
    }
}