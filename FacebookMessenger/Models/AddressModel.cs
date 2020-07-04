﻿using Newtonsoft.Json;

namespace FacebookMessenger.Models
{
    public class AddressModel
    {
        [JsonProperty("street_1")]
        public string Street1 { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("ountry")]
        public string Country { get; set; }

    }
}