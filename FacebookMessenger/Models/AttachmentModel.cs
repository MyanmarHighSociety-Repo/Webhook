using FacebookMessenger.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookMessenger.Models
{
    public class AttachmentModel
    {
        [JsonProperty("type")]
        public AttachmentType Type { get; set; }

        [JsonProperty("payload")]
        public AttachmentPayloadModel Payload { get; set; }
    }
}
