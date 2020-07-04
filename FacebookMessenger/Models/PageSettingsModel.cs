using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
    public class FacebookSettingModel
    {
        public string AppToken { get; set; }
        public List<PageModel> Pages { get; set; }
    }
}
