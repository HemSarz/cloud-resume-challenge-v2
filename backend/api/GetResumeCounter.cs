using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker.Extensions.CosmosDB;
// using Microsoft.Azure.WebJobs;
// Using Newtonsoft.Json for serialization/deserialization
// You could also use System.Text.Json which is built-in to .NET.

namespace api // <--- Make sure this namespace matches your project name
{
    // Ensure your Counter class is accessible, ideally in its own file: Counter.cs
    // public class Counter
    // {
    //     public string id { get; set; } = "Counter";
    //     public int count { get; set; }
    // }

    public class GetResumeCounter(ILoggerFactory loggerFactory)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<GetResumeCounter>();

        [Function("GetResumeCounter")] // This defines the name of your Azure Function
        [CosmosDBOutput(
            databaseName: "VisitorDb",          // Replace with your actual Cosmos DB Database ID
            containerName: "Counters",          // Replace with your actual Cosmos DB Container ID
            Connection = "CosmosDbConnection")] // Name of the app setting/local setting for Cosmos DB connection string
        public Counter Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req,
            // Input Binding: Reads the current visitor counter document from Cosmos DB
            [CosmosDBInput(
                databaseName: "VisitorDb",          // Replace with your actual Cosmos DB Database ID
                containerName: "Counters",          // Replace with your actual Cosmos DB Container ID
                Id = "Counter",              // The 'id' field of the document we want to read
                PartitionKey = "Counter",    // The partition key value for that document
                Connection = "CosmosDbConnection")] // Name of the app setting/local setting for Cosmos DB connection string
                IEnumerable<Counter>? currentCounters // The result of the input binding (can be null/empty if document doesn't exist)

        )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            Counter counter;

            // Check if the visitor counter document exists
            if (currentCounters == null || !currentCounters.Any())
            {
                // If it doesn't exist (first run or deleted), create a new one
                _logger.LogInformation("Visitor counter document not found. Initializing Count to 0.");
                counter = new Counter { Id = "Counter", Count = 0 };
            }
            else
            {
                // If it exists, retrieve the existing counter document
                _logger.LogInformation("Visitor counter document found.");
                counter = currentCounters.First();
            }

            // Increment the count
            counter.Count++;
            _logger.LogInformation($"Incremented visitor count to: {counter.Count}");

            // The Azure Functions runtime handles inserting or updating based on the 'id' field
            _logger.LogInformation("Visitor counter document will be saved/updated in Cosmos DB.");

            // Optionally, you can log or handle the HTTP response here if needed

            return counter;
        }
    }
}