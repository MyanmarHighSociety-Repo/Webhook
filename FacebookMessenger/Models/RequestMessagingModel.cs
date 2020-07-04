using Newtonsoft.Json;
using System;

namespace FacebookMessenger.Models
{
    public class RequestMessagingModel
    {
        [JsonProperty("sender")]
        public PersonModel Sender { get; set; }

        [JsonProperty("recipient")]
        public PersonModel Receiver { get; set; }

        [JsonProperty("timestamp")]
        public long Time { get; set; }

        [JsonProperty("message")]
        public RequestMessageModel Message { get; set; }

        [JsonProperty("delivery")]
        public DeliveryMessageModel DeliveryMessage { get; set; }

        [JsonProperty("postback")]
        public PostbackModel Postback { get; set; }

        [JsonProperty("read")]
        public ReadMessageModel ReadMessage { get; set; }


        public DateTime? dtTime
        {
            get
            {
                return Helper.DateConverter.ConvertJSDT_To_Datetime(Time);
            }
        }

    }
}