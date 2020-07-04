using Newtonsoft.Json;

namespace FacebookMessenger.Models
{
    public class ReceiptElementModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public float Price { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("image_url")]
        public string Image { get; set; }
 
    }
}