using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PowerRiders.Models
{
     public class Event
    {   
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Title")]
        
        public string Title { get; set; }
        [JsonProperty("LocationName")]
        public string LocationName { get; set; }
        
        [JsonProperty("EventTime")]
        public string EventTime { get; set; }

        [JsonProperty("Content")]
        public string Content { get; set; }
    }

    public class Ticket
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }
}