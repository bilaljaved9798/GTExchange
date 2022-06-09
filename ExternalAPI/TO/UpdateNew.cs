using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExternalAPI.TO
{
   public class UpdateNew
    {
        [JsonProperty("p1")]
        public string P1 { get; set; }
        [JsonProperty("p2")]
        public string P2 { get; set; }
        [JsonProperty("home")]
        public string Home { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("tv")]
        public long Tv { get; set; }
        [JsonProperty("room")]
        public long Room { get; set; }
    }
    public class UpdateResult
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("result")]
        public UpdateNew Result { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

    }
}
