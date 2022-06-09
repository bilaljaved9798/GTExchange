using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ExternalAPI.TO
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum MarketBettingType
    {
        ODDS,
        LINE,
        OVER_UNDER_35,
        OVER_UNDER_05,
        OVER_UNDER_15,
        OVER_UNDER_25,
        OVER_UNDER_45,
        OVER_UNDER_55,
        RANGE,
        ASIAN_HANDICAP_DOUBLE_LINE,
        ASIAN_HANDICAP_SINGLE_LINE,
        FIXED_ODDS
    }
}
