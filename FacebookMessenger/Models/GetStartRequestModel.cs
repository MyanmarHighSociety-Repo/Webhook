using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
    public class GetStartRequestModel
    {
        [JsonProperty("get_started")] 
        public GetStartModel GetStart{get;set;}
    }
}
