using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
    public class ButtomAttachmentPayloadModel : AttachmentPayloadModel
    {
        [JsonProperty("text")]
        public string Text { get; set; }

    }
}
