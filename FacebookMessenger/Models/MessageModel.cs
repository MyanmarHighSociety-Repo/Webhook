﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacebookMessenger.Models
{
    //Parent Message Class
    public class ResponseMessageModel 
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    } 
}
