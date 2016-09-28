using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.PlatformAbstractions;

namespace TogglImporter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.WaitAll(Run(args));
        }

        public static async Task Run(string [] args)
        {
            Console.WriteLine("Starting Toggl Import...");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(PlatformServices.Default.Application.ApplicationBasePath)
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.production.json", true)
                .AddEnvironmentVariables()
                .Build();


            var queries = new Queries(configuration["togglApiKey"]);
            var storage = new CloudStorage(configuration["storageAccount"], "toggl-rawdata");
            Console.WriteLine("Initializing storage...");
            await storage.InitializeAsync();

            Console.WriteLine("Saving workspaces to storage...");
            var workspaces = await queries.GetWorkspacesAsync();
            var novawebWorkspace = workspaces.First(x => x.Name == "Novaweb");
            await storage.SaveWorkspace(novawebWorkspace);
            await Task.Delay(250);

            Console.WriteLine("Saving clients to storage...");
            var clients = await queries.GetWorkspaceClientsAsync(novawebWorkspace.Id);
            await storage.SaveClients(clients);
            await Task.Delay(250);

            Console.WriteLine("Saving projects to storage...");
            var projects = await queries.GetWorkspaceProjectsAsync(novawebWorkspace.Id);
            await storage.SaveProjects(projects);
            await Task.Delay(250);

            Console.WriteLine("Saving time entries to storage...");
            const int monthsToRetrieve = 2;
            for (int monthsAgo = 0; monthsAgo > -monthsToRetrieve; monthsAgo--)
            {
                var timeEntries = await queries.GetAllTimeEntriesForXMonthsAgoAsync(monthsAgo);
                await storage.SaveTimeEntriesAsync(DateTime.UtcNow.AddMonths(monthsAgo), timeEntries);
                await Task.Delay(500);
            }

            Console.WriteLine("Toggl import completed.");
        }
    }
}
