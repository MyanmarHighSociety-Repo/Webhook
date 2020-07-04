using Newtonsoft.Json;

namespace FacebookMessenger.Models
{
    public class QuickReplyResponseModel
    {
        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        //Postback payload defined by developer
        [JsonProperty("payload")]
        public string Payload { get; set; }

        [JsonProperty("image_url")]
        public string Image { get; set; }

    }
}