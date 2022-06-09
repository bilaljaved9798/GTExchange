using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bfnexchange.Services
{
    public class MatchScoreCard
    {
        public string MatchKey { get; set; }
        public string TeamAScore { get; set; }
        public string TeamBScore { get; set; }
        public string TeamAName { get; set; }
        public string TeamBName { get; set; }
        public string TeamAOver { get; set; }
        public string TeamBOver { get; set; }
        public string ReqRR { get; set; }
        public string CurrRR { get; set; }
        public MsgsNew Msgs { get; set; }
        public string RequireScore { get; set; }
        public string LastBallComment { get; set; }
        public string StrickerStr { get; set; }
        public string NonStrickerStr { get; set; }
        public List<string> RecentOverBalls { get; set; }
        public string BowlerStr { get; set; }
        public List<TeamsNew> Teams { get; set; }
        public string CurrentTeamForSelection { get; set; }

    }
    public class MsgsNew
    {
        public string info { get; set; }
        public string completed { get; set; }
        public string match_comments { get; set; }
        public string result { get; set; }
    }
    public class PlayerBatting
    {
        public string PlayerName { get; set; }
        public string Runs { get; set; }
        public string Balls { get; set; }
        public string Fours { get; set; }
        public string Sixes { get; set; }
        public string StrikeRate { get; set; }
        public string OutStr { get; set; }
    }
    public class PlayerBowling
    {
        public string PlayerName { get; set; }
        public string Overs { get; set; }
        public string MaidainOver { get; set; }
        public string Runs { get; set; }
        public string Wickets { get; set; }
        public string Economy { get; set; }
        public string Extras { get; set; }
    }
    public class Player
    {
        public string PlayerName { get; set; }
        public string PlayerRoll { get; set; }
        public string PlayerKey { get; set; }
        public PlayerBatting PlayerBetting { get; set; }
        public PlayerBowling PlayerBowling { get; set; }
        public string PlayerFeilding { get; set; }


    }
    public class TeamsNew
    {
        public string TeamName { get; set; }
        public string TeamKey { get; set; }
        public List<Player> Players { get; set; }

    }
}