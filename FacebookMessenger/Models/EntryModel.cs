using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FacebookMessenger.Models
{
    public class EntryModel
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }

        [JsonProperty("messaging")]
        public List<RequestMessagingModel> Messaging { get; set; }


        public DateTime? dtTime { 
            get
            {   
                return Helper.DateConverter.ConvertJSDT_To_Datetime(Time);
                
            }
        }
         

    }
}