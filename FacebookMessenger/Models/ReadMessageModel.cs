using Newtonsoft.Json;
using System;

namespace FacebookMessenger.Models
{
    public class ReadMessageModel
    {
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