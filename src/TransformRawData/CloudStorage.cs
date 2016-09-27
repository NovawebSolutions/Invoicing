using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace TransformRawData
{
    public class CloudStorage
    {
        private readonly CloudBlobContainer _container;
        private readonly CloudTable _invoiceTable;
        private readonly CloudTable _projectTables;
        private readonly CloudTable _timeEntryTable;
        private readonly CloudTable _clientTable;

        public CloudStorage(string connectionString, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);

            var tableClient = storageAccount.CreateCloudTableClient();
            _clientTable = tableClient.GetTableReference("Client");
            _timeEntryTable = tableClient.GetTableReference("TimeEntries");
            _projectTables= tableClient.GetTableReference("Projects");
            _invoiceTable = tableClient.GetTableReference("Invoices");
        }

        public async Task InitializeAsync()
        {
            await _container.CreateIfNotExistsAsync();
            await _clientTable.CreateIfNotExistsAsync();
            await _timeEntryTable.CreateIfNotExistsAsync();
            await _projectTables.CreateIfNotExistsAsync();
            await _invoiceTable.CreateIfNotExistsAsync();
        }


        public async Task SyncClientsToTableStorage(string partitionKey)
        {
            var allClientsBlockBlob = _container.GetBlockBlobReference("Clients/all.json");
            if (await allClientsBlockBlob.ExistsAsync())
            {
                string rawJson = await allClientsBlockBlob.DownloadTextAsync();
                var clients = JsonConvert.DeserializeObject<IEnumerable<TogglModel.Client>>(rawJson);

                foreach (var client in clients)
                {
                    var operation = TableOperation.InsertOrMerge(new Invoicing.Model.Client
                    {
                        PartitionKey = partitionKey,
                        RowKey = client.Id.ToString(),
                        Id = client.Id,
                        Wid = client.Wid,
                        Name = client.Name
                    });
                    await _clientTable.ExecuteAsync(operation);
                }

            }
        }

        public async Task SyncProjectsToTableStorage(string partitionKey)
        {
            var allProjectsBlockBlob = _container.GetBlockBlobReference("Projects/all.json");
            if (await allProjectsBlockBlob.ExistsAsync())
            {
                string rawJson = await allProjectsBlockBlob.DownloadTextAsync();
                var projects = JsonConvert.DeserializeObject<IEnumerable<TogglModel.Project>>(rawJson);

                foreach (var project in projects)
                {
                    var operation = TableOperation.InsertOrMerge(new Invoicing.Model.Project
                    {
                        PartitionKey = partitionKey,
                        RowKey = project.Id.ToString(),
                        Id = project.Id,
                        Wid = project.Wid,
                        Cid = project.Cid,
                        Name = project.Name,
                        Active = project.Active,
                        Billable = project.Billable,
                        IsPrivate = project.IsPrivate,
                        Rate = project.Rate
                    });
                    await _projectTables.ExecuteAsync(operation);
                }

            }
        }
        public async Task SyncTimeEntriesToTableStorage(string partitionKey)
        {
            var segment = await _container.ListBlobsSegmentedAsync("TimeEntries/", new BlobContinuationToken());

            foreach (var timeEntriesBlockBlob in segment.Results.Cast<CloudBlockBlob>())
            {
                Console.WriteLine($"\tSyncing {timeEntriesBlockBlob.Name}");
                string rawJson = await timeEntriesBlockBlob.DownloadTextAsync();
                var timeEntries = JsonConvert.DeserializeObject<IEnumerable<TogglModel.TimeEntry>>(rawJson);

                foreach (var timeEntry in timeEntries)
                {
                    var operation = TableOperation.InsertOrMerge(new Invoicing.Model.TimeEntry
                    {
                        PartitionKey = partitionKey,
                        RowKey = timeEntry.Id.ToString(),
                        Id = timeEntry.Id,
                        Guid = timeEntry.Guid,
                        Wid = timeEntry.Wid,
                        Pid = timeEntry.Pid,
                        Uid = timeEntry.Uid,
                        Start = timeEntry.Start,
                        Stop = timeEntry.Stop,
                        Billable = timeEntry.Billable,
                        Duration = timeEntry.Duration,
                        Description = timeEntry.Description,
                        DurationOnly = timeEntry.DurationOnly,
                        At = timeEntry.At
                    });
                    await _timeEntryTable.ExecuteAsync(operation);
                }
            }
        }
    }
}