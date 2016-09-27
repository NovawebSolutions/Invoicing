using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using TogglModel;

namespace TogglImporter
{
    public class CloudStorage
    {
        private readonly CloudBlobContainer _container;
        private readonly Formatting _defaultFormatting;

        public CloudStorage(string connectionString, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);

            _defaultFormatting = Formatting.Indented;
        }

        public async Task InitializeAsync()
        {
            await _container.CreateIfNotExistsAsync();
        }


        public async Task SaveTimeEntriesAsync(DateTime timeStamp, IEnumerable<TimeEntry> timeEntries)
        {
            await _container.GetBlockBlobReference($"TimeEntries/{timeStamp.ToString("yyyy-MM")}.json")
                .UploadTextAsync(JsonConvert.SerializeObject(timeEntries, _defaultFormatting));
        }

        public async Task SaveWorkspace(Workspace workspace)
        {
            await _container.GetBlockBlobReference($"Workspaces/{workspace.Name}.json")
                .UploadTextAsync(JsonConvert.SerializeObject(workspace, _defaultFormatting));
        }

        public async Task SaveClients(IEnumerable<Client> clients)
        {
            await _container.GetBlockBlobReference($"Clients/all.json")
                .UploadTextAsync(JsonConvert.SerializeObject(clients, _defaultFormatting));
        }

        public async Task SaveProjects(IEnumerable<Project> projects)
        {
            await _container.GetBlockBlobReference($"Projects/all.json")
                .UploadTextAsync(JsonConvert.SerializeObject(projects, _defaultFormatting));
        }
    }
}