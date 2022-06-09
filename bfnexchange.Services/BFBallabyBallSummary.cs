using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bfnexchange.Services
{

    public class Inning1
    {
        public string runs { get; set; }
        public string wickets { get; set; }
        public string overs { get; set; }
    }

    public class Home
    {
        public string name { get; set; }
        public string halfTimeScore { get; set; }
        public string fullTimeScore { get; set; }
        public string penaltiesScore { get; set; }
        public List<object> penaltiesSequence { get; set; }
        public bool highlight { get; set; }
        public Inning1 inning1 { get; set; }
        public Inning2 inning2 { get; set; }
    }
    public class Inning2
    {
        public string runs { get; set; }
        public string wickets { get; set; }
        public string overs { get; set; }
    }
    public class Inning12
    {
        public string runs { get; set; }
        public string wickets { get; set; }
        public string overs { get; set; }
    }

    public class Away
    {
        public string name { get; set; }
        public string halfTimeScore { get; set; }
        public string fullTimeScore { get; set; }
        public string penaltiesScore { get; set; }
        public List<object> penaltiesSequence { get; set; }
        public bool highlight { get; set; }
        public Inning12 inning1 { get; set; }
        public Inning2 inning2 { get; set; }
    }

    public class Score
    {
        public Home home { get; set; }
        public Away away { get; set; }
    }

    public class StateOfBall
    {
        public string overNumber { get; set; }
        public string overBallNumber { get; set; }
        public string bowlerName { get; set; }
        public string batsmanName { get; set; }
        public string batsmanRuns { get; set; }
        public string appealId { get; set; }
        public string appealTypeName { get; set; }
        public string wide { get; set; }
        public string bye { get; set; }
        public string legBye { get; set; }
        public string noBall { get; set; }
        public string outcomeId { get; set; }
        public string dismissalTypeName { get; set; }
        public string referralOutcome { get; set; }
    }

    public class FullTimeElapsed
    {
        public int hour { get; set; }
        public int min { get; set; }
        public int sec { get; set; }
    }

    public class BFBallabyBallSummary
    {
        public int eventTypeId { get; set; }
        public int eventId { get; set; }
        public Score score { get; set; }
        public string description { get; set; }
        public int currentSet { get; set; }
        public StateOfBall stateOfBall { get; set; }
        public string currentDay { get; set; }
        public string matchType { get; set; }
        public FullTimeElapsed fullTimeElapsed { get; set; }
        public string matchStatus { get; set; }
    }
}