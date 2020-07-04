using Newtonsoft.Json;

namespace FacebookMessenger.Models
{
    public class PostbackModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }
    }
}