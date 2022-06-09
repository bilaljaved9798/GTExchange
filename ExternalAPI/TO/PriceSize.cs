using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ExternalAPI.TO
{
    public class PriceSize
    {
        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "size")]
        public double Size { get; set; }

        public double OrignalSize { get; set; } = 0;

        public string SizeStr { get; set; } = "0";

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}@{1}", Size, Price)
                        .ToString();
        }
    }
}
