using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Project.Models
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

    public class UserData
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("rider_id")]
        public string riderId { get; set; }

        [JsonProperty("rank")]
        public string rank { get; set; }

        [JsonProperty("nickname")]
        public string nickname { get; set; }
    }
    
    public class User
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("first_name")]
        public string firstName { get; set; }

        [JsonProperty("last_name")]
        public string lastName { get; set; }
    }
}