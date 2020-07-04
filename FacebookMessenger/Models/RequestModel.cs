using FacebookMessenger.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
    public class RequestModel
    {
        [JsonProperty("object")]
        public string RequestObject { get; set; }

        [JsonProperty("entry")]
        public List<EntryModel> Entries { get; set; }
         
    }
}
