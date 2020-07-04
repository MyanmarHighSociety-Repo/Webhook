using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
    public class PersistentMenuRequestModel
    {
        [JsonProperty("psid")]
        public string PSID { get; set; }

        [JsonProperty("persistent_menu")]
        public List<PersistentMenuItemModel> Menu { get; set; }
    }
}
