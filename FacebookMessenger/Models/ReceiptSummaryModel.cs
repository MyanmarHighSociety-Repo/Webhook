using Newtonsoft.Json;

namespace FacebookMessenger.Models
{
    //don't use decimal nor long. Javascript/JSON object has limitation on long and decimal numbers
    public class ReceiptSummaryModel
    {
        [JsonProperty("subtotal")]
        public float Subtotal { get; set; }

        [JsonProperty("shipping_cost")]
        public float ShippingCost { get; set; }

        [JsonProperty("total_tax")]
        public float TotalTax { get; set; }

        [JsonProperty("total_cost")]
        public float TotalCost { get; set; }
    }
}