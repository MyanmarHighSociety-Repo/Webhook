using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FacebookMessenger.Models
{
    public class DeliveryMessageModel
    {
        [JsonProperty("mids")]
        public List<string> MessageIDs { get; set; }

        [JsonProperty("watermark")]
        public long WaterMarkTime { get; set; }

        public DateTime? dtTime
        {
            get
            {
                return Helper.DateConverter.ConvertJSDT_To_Datetime(WaterMarkTime);

            }
        }
    }
}