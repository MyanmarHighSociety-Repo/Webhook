using FacebookMessenger.Enums;
using Newtonsoft.Json;

namespace FacebookMessenger.Models
{
    public class ButtonModel : ActionModel
    {
       

        [JsonProperty("title")]
        public string Title { get; set; }       

        [JsonProperty("payload")]
        public string Payload { get; set; }
 

    }
}