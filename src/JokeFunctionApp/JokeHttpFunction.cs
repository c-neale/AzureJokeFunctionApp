using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JokeFunctionApp.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace JokeFunctionApp
{
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

            var randomJokeIndex = rand.Next(0, jokeList.Count - 1);
            var randomJoke = jokeList[randomJokeIndex];

            var sb = new StringBuilder();

            sb.Append($"<p>{randomJoke.Title}</p>");
            sb.Append(randomJoke.Content);

            var bytes = Encoding.ASCII.GetBytes(sb.ToString());
            var contentResult = Encoding.UTF8.GetString(bytes);

            return new ContentResult
            {
                Content = contentResult,
                ContentType = "text/html"
            };
        }
    }
}
