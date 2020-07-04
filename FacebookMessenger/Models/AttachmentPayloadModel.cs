using FacebookMessenger.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookMessenger.Models
{
    public class AttachmentPayloadModel
    {
        [JsonProperty("template_type")]
        public TemplateType Type { get; set; }

        [JsonProperty("top_element_style")]
        public WebviewHeightRatio? TopElementStyle { get; set; }

        [JsonProperty("elements")]
        public List<ElementModel> Elements { get; set; }

        [JsonProperty("buttons")]
        public List<ButtonModel> Buttons { get; set; }
    }

    

   

   

}
