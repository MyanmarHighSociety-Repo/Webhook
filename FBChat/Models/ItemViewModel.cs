using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBChat.Models
{
    public class ItemViewModel
    {
        public string Name { get; set; }
        public string Descriptions { get; set; }
        public float Price { get; set; }
        public string Summary { get; set; }
        public string ID { get; internal set; }
    }


}
