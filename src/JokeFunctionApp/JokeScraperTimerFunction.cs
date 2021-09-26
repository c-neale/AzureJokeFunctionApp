using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace JokeFunctionApp
{
    public static class JokeScraperTimerFunction
    {
        [FunctionName("JokeScraperTimerFunction")]
        public static async Task Run([TimerTrigger("0 0 0/5 * * *"
#if DEBUG     // look horrible, but allows us to run instantly when debugging instead of waiting on the timer. (https://stackoverflow.com/a/51775445/6795212)
                , RunOnStartup = true
#endif       
                )]TimerInfo myTimer,
                                    [Queue("joke-queue"), StorageAccount("AzureWebJobsStorage")] ICollector<string> msg, 
                                    ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            const string jokeSourceRss = "https://www.reddit.com/r/Jokes/top/.rss?t=day";

            string rssString;
            try
            {
                using (var client = new HttpClient())
                {
                    using (var response = await client.GetAsync(jokeSourceRss))
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
                log.LogError($"Exception fetching url contents [{jokeSourceRss}]");
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
