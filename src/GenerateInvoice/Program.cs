using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace GenerateInvoice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.WaitAll(Run(args));
        }

        public static async Task Run(string[] args)
        {

            Console.WriteLine("Starting Raw data transformation...");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(PlatformServices.Default.Application.ApplicationBasePath)
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.production.json", true)
                .AddEnvironmentVariables()
                .Build();


            var storage = new CloudStorage(configuration["storageAccount"], "toggl-rawdata");
            Console.WriteLine("Initializing storage...");
            await storage.InitializeAsync();

            Console.WriteLine("Generating invoices...");
            await storage.GenerateInvoices("Novaweb");
        }

    }
}
