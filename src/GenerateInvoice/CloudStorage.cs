using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Invoicing.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace GenerateInvoice
{
    public class CloudStorage
    {
        private readonly CloudTable _invoiceTable;
        private readonly CloudTable _projectTables;
        private readonly CloudTable _timeEntryTable;
        private readonly CloudTable _clientTable;
        private readonly CloudBlobClient _blobClient;

        public CloudStorage(string connectionString, string containerName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);

            var tableClient = storageAccount.CreateCloudTableClient();
            _blobClient = storageAccount.CreateCloudBlobClient();
            _clientTable = tableClient.GetTableReference("Client");
            _timeEntryTable = tableClient.GetTableReference("TimeEntries");
            _projectTables = tableClient.GetTableReference("Projects");
            _invoiceTable = tableClient.GetTableReference("Invoices");
        }

        public async Task InitializeAsync()
        {
            await _clientTable.CreateIfNotExistsAsync();
            await _timeEntryTable.CreateIfNotExistsAsync();
            await _projectTables.CreateIfNotExistsAsync();
            await _invoiceTable.CreateIfNotExistsAsync();
        }


        public async Task GenerateInvoices(string partitionKey)
        {
            //var monthlyTimeEntries = TableQuery.CombineFilters(
            //    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
            //    TableOperators.And,
            //    TableQuery.CombineFilters(TableQuery.GenerateFilterConditionForDate("At", QueryComparisons.GreaterThanOrEqual, new DateTime(2016, 9, 1)),
            //    TableOperators.And, TableQuery.GenerateFilterConditionForDate("At", QueryComparisons.LessThanOrEqual, new DateTime(2016, 9, 30)))
            //    );
            //var entries = await _timeEntryTable.ExecuteQuerySegmentedAsync(new TableQuery<TimeEntry>().Where(monthlyTimeEntries), new TableContinuationToken());
            //var timeEntries = entries.Results;

            var process = Process.Start("wkhtmltopdf.exe", "invoice.html invoice.pdf");

            if (File.Exists("invoice.pdf"))
            {
                var container = _blobClient.GetContainerReference("invoices");
                await container.CreateIfNotExistsAsync();
                var blob = container.GetBlockBlobReference("invoice.pdf");
                await blob.UploadFromFileAsync("invoice.pdf");
            }

            File.Delete("invoice.pdf");
            process.WaitForExit(5000);
        }

    }
}