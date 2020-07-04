using Newtonsoft.Json;

namespace FacebookMessenger.Models
{
    public class PersonModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }
    }
}