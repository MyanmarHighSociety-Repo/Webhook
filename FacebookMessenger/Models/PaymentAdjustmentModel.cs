using Newtonsoft.Json;

namespace FacebookMessenger.Models
{
    public class PaymentAdjustmentModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("amount")]
        public float Amount { get; set; }


    }
}