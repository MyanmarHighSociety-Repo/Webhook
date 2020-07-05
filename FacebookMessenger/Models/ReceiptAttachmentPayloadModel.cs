using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
    public class ReceiptAttachmentPayloadModel : AttachmentPayloadModel
    {
        [JsonProperty("recipient_name")]
        public string RecipientName { get; set; }

        [JsonProperty("order_number")]
        public string OrderNumber { get; set; }

        [JsonProperty("currency")]
        public string CurrencyCode { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("order_url")]
        public string OrderURL { get; set; }

        [JsonProperty("timestamp")]
        public string OrderTime { get; set; }

        [JsonProperty("address")]
        public AddressModel Address { get; set; }

        [JsonProperty("summary")]
        public ReceiptSummaryModel Summary { get; set; }

        [JsonProperty("adjustments")]
        public List<PaymentAdjustmentModel> Adjustments { get; set; }


        [JsonProperty("elements")]
        public List<ReceiptElementModel> Elements { get; set; }

    }
}
