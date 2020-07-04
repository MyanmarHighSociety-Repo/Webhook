using Newtonsoft.Json;

namespace FacebookMessenger.Models
{
    public class QuickReplyRequestModel
    {
        [JsonProperty("payload")]
        public string Payload { get; set; } 
    }
}