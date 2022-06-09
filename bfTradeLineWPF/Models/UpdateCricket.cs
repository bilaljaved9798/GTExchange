using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace globaltraders.Models
{
   
    public partial class UpdateCricket
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("p1")]
        public string P1 { get; set; }

        [JsonProperty("p2")]
        public long P2 { get; set; }

        [JsonProperty("home")]
        public string Home { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("tv")]
        public long Tv { get; set; }

        [JsonProperty("room")]
        public long Room { get; set; }
    }
    public partial class reslt2
    {
        public string a { get; set; }
        public string b { get; set; }
        public string c { get; set; }
        public string cc { get; set; }
        public int d { get; set; }
        public string e { get; set; }
        public int f { get; set; }
        public int fs { get; set; }
        public int g { get; set; }
        public int gi { get; set; }
        public int h { get; set; }
        public int i { get; set; }
        public string j { get; set; }
        public string k { get; set; }
        public int n { get; set; }
        public int n1 { get; set; }
        public string q { get; set; }
        public int t { get; set; }
        public int ti { get; set; }
        public string v { get; set; }
    }
}


