using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bftradeline.HelperClasses
{
  public  class AllowedMarkets
    {
        public bool isCricketMatchOddsAllowedForBet { get; set; }
        public bool isCricketTiedMatchAllowedForBet { get; set; }
        public bool isCricketCompletedMatchAllowedForBet { get; set; }
        public bool isCricketInningsRunsAllowedForBet { get; set; }
        public bool isSoccerAllowedForBet { get; set; }
        public bool isTennisAllowedForBet { get; set; }
        public bool isHorseRaceWinAllowedForBet { get; set; }
        public bool isHorseRacePlaceAllowedForBet { get; set; }
        public bool isGrayHoundRaceWinAllowedForBet { get; set; }
        public bool isGrayHoundRacePlaceAllowedForBet { get; set; }
        public bool isWinnerMarketAllowedForBet { get; set; }
        public bool isFancyMarketAllowed { get; set; }

    }
    public class ScoresForResultPosting
    {
        public string TeamName { get; set; }
        public Nullable<int> Wickets { get; set; }
        public Nullable<double> Overs { get; set; }
        public Nullable<int> Scores { get; set; }
        public Nullable<int> Innings { get; set; }
        public string MatchStatus { get; set; }
    }
}
