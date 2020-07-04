using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
   public class GetStartModel
    {
        [JsonProperty("payload")]
        public string Payload { get; set; }
    }
}
