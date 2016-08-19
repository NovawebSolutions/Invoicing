using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TogglImporter.Model;

namespace TogglImporter
{
    public class Queries
    {
        private readonly HttpClient _client;

        public Queries(string apiKey)
        {
            _client = new HttpClient { BaseAddress = new Uri("https://www.toggl.com/api/v8/") };
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(apiKey + ":api_token"));
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
        }

        public async Task<IEnumerable<Workspace>> GetWorkspacesAsync()
        {
            
            return JsonConvert.DeserializeObject<List<Workspace>>(await _client.GetStringAsync("workspaces"));
        }

        public async Task<IEnumerable<Client>> GetWorkspaceClientsAsync(int workspaceId)
        {
            return JsonConvert.DeserializeObject<List<Client>>(await _client.GetStringAsync($"workspaces/{workspaceId}/clients"));
        }

        public async Task<IEnumerable<Project>> GetWorkspaceProjectsAsync(int workspaceId)
        {

            return JsonConvert.DeserializeObject<List<Project>>(await _client.GetStringAsync($"workspaces/{workspaceId}/projects"));
        }

        public async Task<IEnumerable<TimeEntry>> GetAllTimeEntriesForXMonthsAgoAsync(int monthsAgo)
        {
            var lastMonth = DateTime.UtcNow.AddMonths(monthsAgo);
            var startDate = new DateTime(lastMonth.Year, lastMonth.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endDate = startDate.AddMonths(1).AddSeconds(-1);
            return
                JsonConvert.DeserializeObject<List<TimeEntry>>(
                    await
                        _client.GetStringAsync(
                            $"time_entries?start_date={startDate.ToString("o")}&end_date={endDate.ToString("o")}"));
        }
        public async Task<IEnumerable<TimeEntry>> GetAllTimeEntriesThisMonthAsync()
        {
            return await GetAllTimeEntriesForXMonthsAgoAsync(0);
        }

        public async Task<IEnumerable<TimeEntry>> GetAllTimeEntriesLastMonthAsync()
        {
            return await GetAllTimeEntriesForXMonthsAgoAsync(-1);
        }
    }
}