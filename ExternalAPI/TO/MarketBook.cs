using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ExternalAPI.TO
{
    public class MarketBookString
    {
        public string MarketBookId { get; set; }
        public string MarketBookData { get; set; }
    }
    public class DebitCredit
    {
        public string SelectionID { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
    public class MarketBook
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
        public List<Runner> Runners { get; set; }


        public decimal PoundRate { get; set; }
        public List<DebitCredit> DebitCredit {get;set;}
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
        public List<SP_UserMarket_GetDistinctKJMarketsbyEventID_Result> KJMarkets { get; set; }
        public List<SP_UserMarket_GetDistinctKJMarketsbyEventID_Result> SFigMarkets { get; set; }
        public List<SP_UserMarket_GetDistinctKJMarketsbyEventID_Result> FigureMarkets { get; set; }
        public List<LinevMarketsIN> LineVMarketsIN { get; set; }
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

            if (Runners != null && Runners.Count > 0)
            {
                int idx = 0;
                foreach (var runner in Runners)
                {
                    sb.AppendFormat(" : Runner[{0}]={1}", idx++, runner);
                }
            }

            return sb.ToString();
        }

       
    }

    [Serializable()]
    public class LinevMarkets
    {
        public string MarketCatalogueID { get; set; }
        public string MarketCatalogueName { get; set; }
        public string SelectionID { get; set; }
        public string SelectionName { get; set; }
        public string EventName { get; set; }
        public string EventID { get; set; }
        public string CompetitionName { get; set; }
        public string CompetitionID { get; set; }
        public bool BettingAllowed { get; set; }
        public string AssociateeventID { get; set; }
        public bool isOpenedbyUser { get; set; } = false;
    }

    [Serializable()]
    public  class SP_UserMarket_GetDistinctKJMarketsbyEventID_Result
    {
        public string MarketCatalogueID { get; set; }
        public string MarketCatalogueName { get; set; }      
        public string EventName { get; set; }
        public string EventID { get; set; }
        public string CompetitionName { get; set; }
        public string CompetitionID { get; set; }
        public bool BettingAllowed { get; set; }
        public bool isOpenedbyUser { get; set; }
    }


    [Serializable()]
    public class LinevMarketsIN
    {         
        public string SelectionID { get; set; }
        public string SelectionName { get; set; }
       
        public string MarketBookID { get; set; }
       
    }

    [Serializable()]
    public class Sp_GetKalijut_Result
    {
        public int ID { get; set; }
        public Nullable<double> KaliSizeBack { get; set; }
        public Nullable<double> KaliPriceBack { get; set; }
        public Nullable<double> KaliSizeLay { get; set; }
        public Nullable<double> KaliPriceLay { get; set; }

    }
    [Serializable()]
    public  class Sp_GetFigureOdds_Result
    {
        public int ID { get; set; }
        public double FigSizeBack0 { get; set; }
        public Nullable<double> FigPriceBack0 { get; set; }
        public Nullable<double> FigSizeBack1 { get; set; }
        public Nullable<double> FigPriceBack1 { get; set; }
        public Nullable<double> FigSizeBack2 { get; set; }
        public Nullable<double> FigPriceBack2 { get; set; }
        public Nullable<double> FigSizeBack3 { get; set; }
        public Nullable<double> FigPriceBack3 { get; set; }
        public Nullable<double> FigSizeBack4 { get; set; }
        public Nullable<double> FigPriceBack4 { get; set; }
        public Nullable<double> FigSizeBack5 { get; set; }
        public Nullable<double> FigPriceBack5 { get; set; }
        public Nullable<double> FigSizeBack6 { get; set; }
        public Nullable<double> FigPriceBack6 { get; set; }
        public Nullable<double> FigSizeBack7 { get; set; }
        public Nullable<double> FigPriceBack7 { get; set; }
        public Nullable<double> FigSizeBack8 { get; set; }
        public Nullable<double> FigPriceBack8 { get; set; }
        public Nullable<double> FigSizeBack9 { get; set; }
        public Nullable<double> FigPriceBack9 { get; set; }
        public Nullable<double> FigSizeLay0 { get; set; }
        public Nullable<double> FigPriceLay0 { get; set; }
        public Nullable<double> FigSizeLay1 { get; set; }
        public Nullable<double> FigPriceLay1 { get; set; }
        public Nullable<double> FigSizeLay2 { get; set; }
        public Nullable<double> FigPriceLay2 { get; set; }
        public Nullable<double> FigSizeLay3 { get; set; }
        public Nullable<double> FigPriceLay3 { get; set; }
        public Nullable<double> FigSizeLay4 { get; set; }
        public Nullable<double> FigPriceLay4 { get; set; }
        public Nullable<double> FigSizeLay5 { get; set; }
        public Nullable<double> FigPriceLay5 { get; set; }
        public Nullable<double> FigSizeLay6 { get; set; }
        public Nullable<double> FigPriceLay6 { get; set; }
        public Nullable<double> FigSizeLay7 { get; set; }
        public Nullable<double> FigPriceLay7 { get; set; }
        public Nullable<double> FigSizeLay8 { get; set; }
        public Nullable<double> FigPriceLay8 { get; set; }
        public Nullable<double> FigSizeLay9 { get; set; }
        public Nullable<double> FigPriceLay9 { get; set; }
    }
}
