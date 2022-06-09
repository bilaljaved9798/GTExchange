using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ExternalAPI.TO
{
    public class Competition
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        public bool isFavorite { get; set; } = false;

        public override string ToString()
        {
            return new StringBuilder().AppendFormat("{0}", "Competition")
                        .AppendFormat(" : Id={0}", Id)
                        .AppendFormat(" : Name={0}", Name)
                        .ToString();
        }
    }
}
