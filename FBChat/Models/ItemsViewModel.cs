using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBChat.Models
{
    public class ItemsViewModel : List<ItemsViewModel>
    {
        public string Summary { get; set; }
        public float Total { get; set; }
        public float Tax { get; set; }
    }
}
