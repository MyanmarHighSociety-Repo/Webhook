using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
    public class QuickReplyMessageModel : ResponseMessageModel
    { 
        [JsonProperty("quick_replies")]
        public List<QuickReplyResponseModel> QickReplies { get; set; }
    }
}
