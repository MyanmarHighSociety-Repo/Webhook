using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
    public class TemplateMessageModel : ResponseMessageModel
    {
        [JsonProperty("attachment")]
        public AttachmentModel Attachment { get; set; }

    }
}
