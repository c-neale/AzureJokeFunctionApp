using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace JokeFunctionApp
{
    public static class JokeProcessorQueueFunction
    {
        [FunctionName("JokeProcessorQueueFunction")]
        public static async Task Run([QueueTrigger("joke-queue"), StorageAccount("AzureWebJobsStorage")] string myQueueItem, 
                                [CosmosDB(databaseName: "joke-db", collectionName: "joke-container", CreateIfNotExists = true, ConnectionStringSetting = "CosmosConnectionString")] IAsyncCollector<dynamic> documentsOut,
                                ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            
            // write to the database!
            await documentsOut.AddAsync(new
            {
                id = "test",
                data = "test"
            });
        }
    }
}
