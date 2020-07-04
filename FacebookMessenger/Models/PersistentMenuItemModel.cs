using Newtonsoft.Json;
using System.Collections.Generic;

namespace FacebookMessenger.Models
{
    public class PersistentMenuItemModel
    {
        [JsonProperty("locale")]
        public string Locale  { get; set; }

        [JsonProperty("composer_input_disabled")]
        public bool ComposerInputDisabled { get; set; }

        [JsonProperty("call_to_actions")]
        public List<MenuActionModel> Actions { get; set; }



    }
}