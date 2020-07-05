using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
    public class OrderModel
    {
        [JsonProperty("item_id")]
        public string ItemID { get; set; }

        [JsonProperty("page_id")]
        public string PageID { get; set; }

        [JsonProperty("recipient_id")]
        public string RecipentID { get; set; }
    }
}
