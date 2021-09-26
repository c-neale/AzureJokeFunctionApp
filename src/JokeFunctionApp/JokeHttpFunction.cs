using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JokeFunctionApp
{
    public class JokeItem
    {
        [JsonProperty("id")]
        public string Id {  get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("title")]
        public string Title {  get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public static class JokeHttpFunction
    {
        [FunctionName("TellMeAJoke")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName:"joke-db", collectionName:"joke-container", ConnectionStringSetting = "CosmosConnectionString", SqlQuery = "select * from c")] IEnumerable<JokeItem> jokes,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var jokeList = jokes.ToList();

            var rand = new Random((int)DateTime.Now.Ticks);

            var randomJokeIndex = rand.Next(0, jokeList.Count() - 1);
            var randomJoke = jokeList[randomJokeIndex];

            var sb = new StringBuilder();

            sb.Append(randomJoke.Content);

            return new OkObjectResult(sb.ToString());
        }
    }
}
