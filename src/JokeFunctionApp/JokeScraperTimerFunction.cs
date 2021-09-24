using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace JokeFunctionApp
{
    public static class JokeScraperTimerFunction
    {
        [FunctionName("JokeScraperTimerFunction")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer,
                                    [Queue("joke-queue"), StorageAccount("AzureWebJobsStorage")] ICollector<string> msg, 
                                    ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            const string JokeSourceRss = "https://www.reddit.com/r/Jokes/top/.rss?t=day";

            var rssString = string.Empty;
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(JokeSourceRss))
                    {
                        using (HttpContent content = response.Content)
                        {
                            rssString = await content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.LogError($"Exception fetching url contents [{JokeSourceRss}]");
                return;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(rssString);

            var nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            nsManager.AddNamespace("f", "http://www.w3.org/2005/Atom");

            var nodes = xmlDoc.DocumentElement.SelectNodes("/f:feed/f:entry", nsManager);

            foreach(XmlNode node in nodes)
            {
                msg.Add(node.OuterXml);
            }
            
        }
    }
}
