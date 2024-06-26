using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ExternalAPI.TO
{
    public class RunnerForIndianFancy
    {
        [JsonProperty(PropertyName = "selectionId")]
        public string SelectionId { get; set; }

        public string RunnerName { get; set; }

        [JsonProperty(PropertyName = "handicap")]
        public double? Handicap { get; set; }

        [JsonProperty(PropertyName = "status")]
        public RunnerStatus Status { get; set; }

        [JsonProperty(PropertyName = "adjustmentFactor")]
        public double? AdjustmentFactor { get; set; }

        [JsonProperty(PropertyName = "lastPriceTraded")]
        public double? LastPriceTraded { get; set; }

        [JsonProperty(PropertyName = "totalMatched")]
        public double TotalMatched { get; set; }
        public string TotalMatchedStr { get; set; }

        [JsonProperty(PropertyName = "removalDate")]
        public DateTime? RemovalDate { get; set; }

        [JsonProperty(PropertyName = "sp")]
        public StartingPrices StartingPrices { get; set; }

        [JsonProperty(PropertyName = "ex")]
        public ExchangePrices ExchangePrices { get; set; }

        [JsonProperty(PropertyName = "orders")]
        public List<Order> Orders { get; set; }

        [JsonProperty(PropertyName = "matches")]
        public List<Match> Matches { get; set; }

        public double ProfitandLoss { get; set; }
        public double Loss { get; set; } = 0;
        public string WearingURL { get; set; }
        public string JockeyName { get; set; }
        public string WearingDesc { get; set; }
        public string StatusStr { get; set; }
        public string Clothnumber { get; set; }
        public string StallDraw { get; set; }
        public bool isShow { get; set; } = true;
        public string MarketBookID { get; set; }
        public bool BettingAllowed { get; set; }
        public string MarketStatusStr { get; set; }
        public string Average { get; set; }
        public string Backprice { get; set; }
        public string BackSize { get; set; }
        public string Layprice { get; set; }
        public string LaySize { get; set; }
        public override string ToString()
        {
            var sb = new StringBuilder().AppendFormat("SelectionId={0}", SelectionId)
                        .AppendFormat(" : Handicap={0}", Handicap)
                        .AppendFormat(" : Status={0}", Status)
                        .AppendFormat(" : AdjustmentFactor={0}", AdjustmentFactor)
                        .AppendFormat(" : LastPriceTraded={0}", LastPriceTraded)
                        .AppendFormat(" : TotalMatched={0}", TotalMatched)
                        .AppendFormat(" : RemovalDate={0}", RemovalDate);

            if (StartingPrices != null)
            {
                sb.AppendFormat(": {0}", StartingPrices);
            }

            if (ExchangePrices != null)
            {
                sb.AppendFormat(": {0}", ExchangePrices);

            }

            if (Orders != null && Orders.Count > 0)
            {
                int idx = 0;
                foreach (var order in Orders)
                {
                    sb.AppendFormat(" : Order[{0}]={1}", idx++, order);
                }
            }

            if (Matches != null && Matches.Count > 0)
            {
                int idx = 0;
                foreach (var match in Matches)
                {
                    sb.AppendFormat(" : Match[{0}]={1}", idx++, match);
                }
            }

            return sb.ToString();
        }
    }
}
