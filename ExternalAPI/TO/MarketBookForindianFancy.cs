using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExternalAPI.TO
{
    public class MarketBookForindianFancy
    {
        [JsonProperty(PropertyName = "marketId")]
        public string MarketId { get; set; }

        [JsonProperty(PropertyName = "isMarketDataDelayed")]
        public bool IsMarketDataDelayed { get; set; }

        [JsonProperty(PropertyName = "status")]
        public MarketStatus Status { get; set; }

        [JsonProperty(PropertyName = "betDelay")]
        public int BetDelay { get; set; }

        [JsonProperty(PropertyName = "bspReconciled")]
        public bool IsBspReconciled { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool IsComplete { get; set; }

        [JsonProperty(PropertyName = "inplay")]
        public bool IsInplay { get; set; }

        [JsonProperty(PropertyName = "numberOfWinners")]
        public int NumberOfWinners { get; set; }

        [JsonProperty(PropertyName = "numberOfRunners")]
        public int NumberOfRunners { get; set; }

        [JsonProperty(PropertyName = "numberOfActiveRunners")]
        public int NumberOfActiveRunners { get; set; }

        [JsonProperty(PropertyName = "lastMatchTime")]
        public DateTime? LastMatchTime { get; set; }

        [JsonProperty(PropertyName = "totalMatched")]
        public double TotalMatched { get; set; }

        [JsonProperty(PropertyName = "totalAvailable")]
        public double TotalAvailable { get; set; }

        [JsonProperty(PropertyName = "crossMatching")]
        public bool IsCrossMatching { get; set; }

        [JsonProperty(PropertyName = "runnersVoidable")]
        public bool IsRunnersVoidable { get; set; }

        [JsonProperty(PropertyName = "version")]
        public long Version { get; set; }

        [JsonProperty(PropertyName = "runners")]
        public List<RunnerForIndianFancy> RunnersForindianFancy { get; set; }


        public decimal PoundRate { get; set; }
        public List<DebitCredit> DebitCredit { get; set; }
        public string MarketBookName { get; set; }
        public string FavoriteSelectionName { get; set; }
        public string FavoriteBack { get; set; }
        public string FavoriteLay { get; set; }
        public string FavoriteID { get; set; }
        public string MarketStatusstr { get; set; }
        public string FavoriteBackSize { get; set; }
        public string FavoriteLaySize { get; set; }
        public string OpenDate { get; set; }
        public DateTime? OrignalOpenDate { get; set; }
        public string UserBetsEndUser { get; set; }
        public string UserBetsAgent { get; set; }
        public string UserBetsAdmin { get; set; }
        public int marketsopened { get; set; }
        public string SheetName { get; set; }
        public string MainSportsname { get; set; }
        public bool BettingAllowed { get; set; }
        public bool BettingAllowedOverAll { get; set; }

        public List<string> lstMultipleSelectionforBets { get; set; } = new List<string>();
        public List<LinevMarkets> LineVMarkets { get; set; }
        public string CricketMatchKey { get; set; }
        public bool isOpenExternally { get; set; } = false;
        public bool isWinTheTossMarket { get; set; } = false;
        public string GetMatchUpdatesFrom { get; set; }
        public string CricketAPIMatchKey { get; set; }
        public string TotalOvers { get; set; }
        public string EventID { get; set; }
        public override string ToString()
        {
            var sb = new StringBuilder().AppendFormat("{0}", "MarketBook")
                        .AppendFormat(" : MarketId={0}", MarketId)
                        .AppendFormat(" : Status={0}", Status)
                        .AppendFormat(" : BetDelay={0}", BetDelay)

                        .AppendFormat(" : IsMarketDataDelayed={0}", IsMarketDataDelayed)
                        .AppendFormat(" : IsBspReconciled={0}", IsBspReconciled)
                        .AppendFormat(" : IsComplete={0}", IsComplete)
                        .AppendFormat(" : IsInplay={0}", IsInplay)
                        .AppendFormat(" : IsCrossMatching={0}", IsCrossMatching)
                        .AppendFormat(" : IsRunnersVoidable={0}", IsRunnersVoidable)

                        .AppendFormat(" : NumberOfWinners={0}", NumberOfWinners)
                        .AppendFormat(" : NumberOfRunners={0}", NumberOfRunners)
                        .AppendFormat(" : NumberOfActiveRunners={0}", NumberOfActiveRunners)

                        .AppendFormat(" : LastMatchTime={0}", LastMatchTime)
                        .AppendFormat(" : TotalMatched={0}", TotalMatched)
                        .AppendFormat(" : TotalAvailable={0}", TotalAvailable)

                        .AppendFormat(" : Version={0}", Version);

            if (RunnersForindianFancy != null && RunnersForindianFancy.Count > 0)
            {
                int idx = 0;
                foreach (var runner in RunnersForindianFancy)
                {
                    sb.AppendFormat(" : Runner[{0}]={1}", idx++, runner);
                }
            }

            return sb.ToString();
        }


    }
}
