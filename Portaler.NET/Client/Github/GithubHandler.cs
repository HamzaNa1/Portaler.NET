using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace Portaler.NET.Client.Github
{
    public static class GithubHandler
    {
        private static readonly HttpClient Client = new HttpClient();
        public static async Task GetLastCommitMessage(Action<string> changeCommitMessage)
        {
            string? url = await GetLatestCommitUrl();

            if (url is null)
                return;

            string? message = await GetCommitMessage(url);
            if (message is null)
                return;
            
            changeCommitMessage(message);
        }

        private static async Task<string?> GetCommitMessage(string url)
        {
            Dictionary<string, object>? json =
                await Client.GetFromJsonAsync<Dictionary<string, object>>(url);

            if (json is null)
                return null;

            JsonElement obj = (JsonElement)json["message"];
            string? message = obj.Deserialize<string>();
            
            return message;
        }

        private static async Task<string?> GetLatestCommitUrl()
        {
            Dictionary<string, object>? json =
                await Client.GetFromJsonAsync<Dictionary<string, object>>("https://api.github.com/repos/hamzana1/portaler.net/git/refs/heads/main");

            if (json is null)
                return null;        

            JsonElement obj = (JsonElement)json["object"];
            Dictionary<string, string>? commitData = obj.Deserialize<Dictionary<string, string>>();

            return commitData is null ? null : commitData.TryGetValue("url", out string? url) ? url : null;
        }
    }
}