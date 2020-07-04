using FacebookMessenger.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
    public class MediaElementModel : ElementModel
    {
        [JsonProperty("media_type")]
        public MediaType MediaType { get; set; }

        [JsonProperty("attachment_id")]
        public string MediaAttachmentID { get; set; }

        [JsonProperty("url")]
        public string MediaURL { get; set; }

    }
}
