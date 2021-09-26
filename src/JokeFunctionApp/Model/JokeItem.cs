using Newtonsoft.Json;

namespace JokeFunctionApp.Model
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
}