﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace ExternalAPI.TO
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BetStatus
    {
        SETTLED,
        VOIDED,
        LAPSED,
        CANCELLED
    }
}
