using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExternalAPI.TO
{
   

        public partial class Root
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
            public long P1 { get; set; }

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
    public partial class Home
    {
        [JsonProperty("ao")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Ao { get; set; }

        [JsonProperty("b1s")]
        public string B1S { get; set; }

        [JsonProperty("b2s")]
        public string B2S { get; set; }

        [JsonProperty("bw")]
        public string Bw { get; set; }

        [JsonProperty("cd")]
        public string Cd { get; set; }

        [JsonProperty("con")]
        public Con Con { get; set; }

        [JsonProperty("cs")]
        public Cs Cs { get; set; }

        [JsonProperty("day")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Day { get; set; }

        [JsonProperty("hms")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Hms { get; set; }

        [JsonProperty("i")]
        public string I { get; set; }

        [JsonProperty("i1")]
        public I1 I1 { get; set; }

        [JsonProperty("i1b")]
        public string I1B { get; set; }

        [JsonProperty("i2")]
        public I1 I2 { get; set; }

        [JsonProperty("i3")]
        public I1 I3 { get; set; }

        [JsonProperty("i3b")]
        public string I3B { get; set; }

        [JsonProperty("i4")]
        public I1 I4 { get; set; }

        //[JsonProperty("iov")]
        //[JsonConverter(typeof(ParseStringConverter))]
        //public long? Iov { get; set; }
        [JsonProperty("iov")]
        public string Iov { get; set; }

        [JsonProperty("lw")]
        public string Lw { get; set; }

        [JsonProperty("md")]
        public string Md { get; set; }

        [JsonProperty("mit")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Mit { get; set; }

        [JsonProperty("mk")]
        public string Mk { get; set; }

        [JsonProperty("oh")]
        public string Oh { get; set; }

        [JsonProperty("os")]
        public string Os { get; set; }

        [JsonProperty("p1")]
        public string P1 { get; set; }

        [JsonProperty("p2")]
        public string P2 { get; set; }

        [JsonProperty("pb")]
        public string Pb { get; set; }

        [JsonProperty("pt")]
        public string Pt { get; set; }

        [JsonProperty("rt")]
        public string Rt { get; set; }

        [JsonProperty("sk")]
        public string Sk { get; set; }

        [JsonProperty("sn")]
        public string Sn { get; set; }

        [JsonProperty("ssns")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Ssns { get; set; }

        [JsonProperty("t1")]
        public T1 T1 { get; set; }

        [JsonProperty("t2")]
        public T1 T2 { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("tt")]
        public string Tt { get; set; }
    }

    public partial class Con
    {
        [JsonProperty("g")]
        public string G { get; set; }

        [JsonProperty("lt")]
        public string Lt { get; set; }

        [JsonProperty("mf")]
        public string Mf { get; set; }

        [JsonProperty("mn")]
        public string Mn { get; set; }

        [JsonProperty("mstus")]
        public string Mstus { get; set; }

        [JsonProperty("mtm")]
        public string Mtm { get; set; }

        [JsonProperty("ostus")]
        public string Ostus { get; set; }

        [JsonProperty("sr")]
        public string Sr { get; set; }
    }

    public partial class Cs
    {
        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("ts")]
        public string Ts { get; set; }
    }

    public partial class I1
    {
        [JsonProperty("ov")]
        public string Ov { get; set; }

        [JsonProperty("sc")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Sc { get; set; }

        [JsonProperty("wk")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Wk { get; set; }

        [JsonProperty("tr", NullValueHandling = NullValueHandling.Ignore)]
        public string Tr { get; set; }
    }

    public partial class T1
    {
        [JsonProperty("f")]
        public string F { get; set; }

        [JsonProperty("ic")]
        public string Ic { get; set; }

        [JsonProperty("n")]
        public string N { get; set; }
    }
}

    


