using System;
using System.Collections.Generic;
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
    }
}