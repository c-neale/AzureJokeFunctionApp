using System.Threading.Tasks;
using System.Xml;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace JokeFunctionApp
{
    public static class JokeProcessorQueueFunction
    {
        [FunctionName("JokeProcessorQueueFunction")]
        public static async Task Run([QueueTrigger("joke-queue"), StorageAccount("AzureWebJobsStorage")] string myQueueItem, 
                                [CosmosDB(databaseName: "joke-db",  collectionName: "joke-container", CreateIfNotExists = true, ConnectionStringSetting = "CosmosConnectionString")] IAsyncCollector<dynamic> documentsOut,
                                ILogger log)
        {
            //log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(myQueueItem);

            var nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            nsManager.AddNamespace("f", "http://www.w3.org/2005/Atom");

            var idNode = xmlDoc.SelectSingleNode("/f:entry/f:id", nsManager);
            var dateNode = xmlDoc.SelectSingleNode("/f:entry/f:published", nsManager);
            var titleNode = xmlDoc.SelectSingleNode("/f:entry/f:title", nsManager);
            var contentNode = xmlDoc.SelectSingleNode("/f:entry/f:content", nsManager);

            //write to the database!
            await documentsOut.AddAsync(new
            {
                id = idNode.InnerText,
                date = dateNode.InnerText,
                title = titleNode.InnerText,
                content = contentNode.InnerText
            });
        }
    }
}
