using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bfnexchange.Services
{
    public class Season
    {
        public string name { get; set; }
    }

    public class A
    {
        public string name { get; set; }
        public string key { get; set; }
    }

    public class B
    {
        public string name { get; set; }
        public string key { get; set; }
    }

    public class Teams
    {
        public A a { get; set; }
        public B b { get; set; }
    }

    public class Msgs
    {
        public List<object> others { get; set; }
        public string info { get; set; }
        public string completed { get; set; }
    }

    public class StartDate
    {
        public string iso { get; set; }
    }
    public class Card
    {
        public string status { get; set; }
        public string related_name { get; set; }
        public string expires { get; set; }
        public string name { get; set; }
        public string short_name { get; set; }
        public string title { get; set; }
        public Season season { get; set; }
        public string format { get; set; }
        public string venue { get; set; }
        public Teams teams { get; set; }
        public string winner_team { get; set; }
        public string key { get; set; }
        public Msgs msgs { get; set; }
        public string @ref { get; set; }
        public StartDate start_date { get; set; }
        public int? approx_completed_ts { get; set; }
    }

    public class Data1
    {
        public string card_type { get; set; }
        public List<Card> cards { get; set; }
        public List<string> intelligent_order { get; set; }
    }

    public class AllMatches
    {
        public bool status { get; set; }
        public string version { get; set; }
        public int status_code { get; set; }
        public string expires { get; set; }
        public object Etag { get; set; }
        public string cache_key { get; set; }
        public Data1 data { get; set; }
    }
    public class RecentMatches
    {
        public string Key { get; set; }
        public string MatchName { get; set; }
    }
}