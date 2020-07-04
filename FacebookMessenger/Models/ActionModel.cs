using FacebookMessenger.Enums;
using Newtonsoft.Json;

namespace FacebookMessenger.Models
{
    public class ActionModel
    {
        [JsonProperty("type")]
        public ActionType? Type {get;set;}

        [JsonProperty("url")]
        public string URL { get; set; }

        [JsonProperty("fallback_url")]
        public string FallbackURL { get; set; }

        [JsonProperty("webview_height_ratio")]
        public WebviewHeightRatio? WebviewHeight { get; set; }

        [JsonProperty("messenger_extensions")]
        public bool? MessengerExtensions { get; set; }


    }
}