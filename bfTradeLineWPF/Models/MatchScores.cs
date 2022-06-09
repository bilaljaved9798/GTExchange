using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bftradeline.Models
{
   public class MatchScores
    {
        public string TeamName { get; set; }
        public int Wickets { get; set; }
        public double Overs { get; set; }
        public int Scores { get; set; }
        public int Innings { get; set; }
        public string MatchStatus { get; set; }
        public string MatchType { get; set; }
        public string TotalOvers { get; set; }
    }
}
