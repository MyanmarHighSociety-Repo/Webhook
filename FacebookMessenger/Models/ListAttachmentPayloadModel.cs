using FacebookMessenger.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
    public class ListAttachmentPayloadModel : AttachmentPayloadModel
    {
        [JsonProperty("top_element_style")]
        public WebviewHeightRatio? TopElementStyle { get; set; }

    }
}
