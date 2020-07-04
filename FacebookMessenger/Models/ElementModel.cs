using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Mime;

namespace FacebookMessenger.Models
{
    public class ElementModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("image_url")]
        public string ImageURL { get; set; }

        [JsonProperty("subtitle")]
        public string SubTitle { get; set; }

        [JsonProperty("default_action")]
        public ActionModel DefaultAction { get; set; }

        [JsonProperty("buttons")]
        public List<ButtonModel> Buttons { get; set; } 
    }

   
}