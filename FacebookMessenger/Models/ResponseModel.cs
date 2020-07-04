using FacebookMessenger.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookMessenger.Models
{
    public class ResponseModel
    {
        [JsonProperty("recipient")]
        public PersonModel Receiver { get; set; }

        [JsonProperty("messaging_type")]
        public MessageType MessageType { get; set; }

        [JsonProperty("message")]
        public ResponseMessageModel Message { get; set; } 

    } 
}
