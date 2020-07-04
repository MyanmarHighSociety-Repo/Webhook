using Newtonsoft.Json;

namespace FacebookMessenger.Models
{
    public class RequestMessageModel
    {
        [JsonProperty("mid")]
        public string MessageID { get; set; }

        [JsonProperty("is_echo")]
        public string IsEcho { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("app_id")]
        public string AppID { get; set; }

        [JsonProperty("quick_reply")]
        public QuickReplyRequestModel QuickReply { get; set; }



    }
}