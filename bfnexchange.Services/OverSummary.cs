using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bfnexchange.Services
{
  
        public class Fielding
        {
            public int? catches { get; set; }
            public int? runouts { get; set; }
            public int? stumbeds { get; set; }
        }

        public class Bowling
        {
            public int runs { get; set; }
            public int balls { get; set; }
            public int maiden_overs { get; set; }
            public int wickets { get; set; }
            public int extras { get; set; }
            public string overs { get; set; }
            public double economy { get; set; }
        }

        public class Bowler
        {
            public Fielding fielding { get; set; }
            public string key { get; set; }
            public Bowling bowling { get; set; }
        }

        public class DismissedAt
        {
            public int team_runs { get; set; }
            public int over { get; set; }
            public int ball { get; set; }
            public int wicket_index { get; set; }
            public string ball_key { get; set; }
        }

        public class Batsman2
        {
            public int runs { get; set; }
            public int dotball { get; set; }
            public int six { get; set; }
            public int four { get; set; }
            public int ball_count { get; set; }
            public string key { get; set; }
        }

        public class Fielder
        {
            public int @catch { get; set; }
            public int runout { get; set; }
            public string key { get; set; }
            public int stumbed { get; set; }
        }

        public class Bowler2
        {
            public int runs { get; set; }
            public int extras { get; set; }
            public int ball_count { get; set; }
            public string key { get; set; }
            public int wicket { get; set; }
        }

        public class Team
        {
            public int runs { get; set; }
            public int extras { get; set; }
            public int ball_count { get; set; }
            public int wicket { get; set; }
        }

        public class BallOfDismissed
        {
            public string comment { get; set; }
            public string wicket_type { get; set; }
            public int over { get; set; }
            public string ball_type { get; set; }
            public List<object> highlight_names_keys { get; set; }
            public string innings { get; set; }
            public object wagon_position { get; set; }
            public string wicket { get; set; }
            public string match { get; set; }
            public string status { get; set; }
            public DateTime updated { get; set; }
            public string ball { get; set; }
            public Batsman2 batsman { get; set; }
            public string striker { get; set; }
            public string batting_team { get; set; }
            public Fielder fielder { get; set; }
            public string nonstriker { get; set; }
            public int runs { get; set; }
            public Bowler2 bowler { get; set; }
            public string over_str { get; set; }
            public string extras { get; set; }
            public Team team { get; set; }
        }

        public class Batting
        {
            public int dots { get; set; }
            public int sixes { get; set; }
            public int runs { get; set; }
            public int balls { get; set; }
            public int fours { get; set; }
            public double strike_rate { get; set; }
            public DismissedAt dismissed_at { get; set; }
            public bool? dismissed { get; set; }
            public string out_str { get; set; }
            public BallOfDismissed ball_of_dismissed { get; set; }
        }

        public class Batsman
        {
            public Batting batting { get; set; }
            public string key { get; set; }
        }

        public class Match
        {
            public int runs { get; set; }
            public string score { get; set; }
            public double current_run_rate { get; set; }
            public int wicket { get; set; }
        }

        public class OversSummary
        {
            public string innings { get; set; }
            public int runs { get; set; }
            public List<Bowler> bowler { get; set; }
            public int over { get; set; }
            public List<Batsman> batsman { get; set; }
            public List<object> wicket_players_keys { get; set; }
            public int wicket { get; set; }
            public Match match { get; set; }
        }

        public class B1
        {
            public List<OversSummary> overs_summary { get; set; }
            public string key { get; set; }
        }

        public class Fielding2
        {
        }

        public class Bowling2
        {
            public int runs { get; set; }
            public int balls { get; set; }
            public int maiden_overs { get; set; }
            public int wickets { get; set; }
            public int extras { get; set; }
            public string overs { get; set; }
            public double economy { get; set; }
        }

        public class Bowler3
        {
            public Fielding2 fielding { get; set; }
            public string key { get; set; }
            public Bowling2 bowling { get; set; }
        }

        public class DismissedAt2
        {
            public int team_runs { get; set; }
            public int over { get; set; }
            public int ball { get; set; }
            public int wicket_index { get; set; }
            public string ball_key { get; set; }
        }

        public class Batsman4
        {
            public int runs { get; set; }
            public int dotball { get; set; }
            public int six { get; set; }
            public int four { get; set; }
            public int ball_count { get; set; }
            public string key { get; set; }
        }

        public class Fielder2
        {
            public int? @catch { get; set; }
            public int? runout { get; set; }
            public string key { get; set; }
            public int? stumbed { get; set; }
        }

        public class Bowler4
        {
            public int runs { get; set; }
            public int extras { get; set; }
            public int ball_count { get; set; }
            public string key { get; set; }
            public int wicket { get; set; }
        }

        public class Team2
        {
            public int runs { get; set; }
            public int extras { get; set; }
            public int ball_count { get; set; }
            public int wicket { get; set; }
        }

        public class BallOfDismissed2
        {
            public string comment { get; set; }
            public string wicket_type { get; set; }
            public int over { get; set; }
            public string ball_type { get; set; }
            public List<object> highlight_names_keys { get; set; }
            public string innings { get; set; }
            public object wagon_position { get; set; }
            public string wicket { get; set; }
            public string match { get; set; }
            public string status { get; set; }
            public DateTime updated { get; set; }
            public string ball { get; set; }
            public Batsman4 batsman { get; set; }
            public string striker { get; set; }
            public string batting_team { get; set; }
            public Fielder2 fielder { get; set; }
            public string nonstriker { get; set; }
            public int runs { get; set; }
            public Bowler4 bowler { get; set; }
            public string over_str { get; set; }
            public string extras { get; set; }
            public Team2 team { get; set; }
        }

        public class Batting2
        {
            public int dots { get; set; }
            public int sixes { get; set; }
            public int runs { get; set; }
            public int balls { get; set; }
            public int fours { get; set; }
            public double strike_rate { get; set; }
            public DismissedAt2 dismissed_at { get; set; }
            public bool? dismissed { get; set; }
            public string out_str { get; set; }
            public BallOfDismissed2 ball_of_dismissed { get; set; }
        }

        public class Batsman3
        {
            public Batting2 batting { get; set; }
            public string key { get; set; }
        }

        public class Match2
        {
            public int runs { get; set; }
            public int req_runs_balls { get; set; }
            public int wicket { get; set; }
            public int req_runs { get; set; }
            public string score { get; set; }
            public double req_run_rate { get; set; }
            public double current_run_rate { get; set; }
        }

        public class OversSummary2
        {
            public string innings { get; set; }
            public int runs { get; set; }
            public List<Bowler3> bowler { get; set; }
            public int over { get; set; }
            public List<Batsman3> batsman { get; set; }
            public List<object> wicket_players_keys { get; set; }
            public int wicket { get; set; }
            public Match2 match { get; set; }
        }

        public class A1
        {
            public List<OversSummary2> overs_summary { get; set; }
            public string key { get; set; }
        }

        public class Innings
        {
            public B1 b_1 { get; set; }
            public A1 a_1 { get; set; }
        }

        public class Data
        {
            public List<List<string>> batting_order { get; set; }
            public Innings innings { get; set; }
        }

        public class OverSummary
        {
            public bool status { get; set; }
            public string version { get; set; }
            public int status_code { get; set; }
            public string expires { get; set; }
            public string Etag { get; set; }
            public string cache_key { get; set; }
            public Data data { get; set; }
        }
    
}