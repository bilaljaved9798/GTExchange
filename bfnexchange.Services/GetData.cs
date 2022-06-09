using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bfnexchange.Services
{
   
    public class Event
    {
        public string SelectionId { get; set; }
        public string RunnerName { get; set; }
        public string LayPrice1 { get; set; }
        public string LaySize1 { get; set; }
        public string LayPrice2 { get; set; }
        public string LaySize2 { get; set; }
        public string LayPrice3 { get; set; }
        public string LaySize3 { get; set; }
        public string BackPrice1 { get; set; }
        public string BackSize1 { get; set; }
        public string BackPrice2 { get; set; }
        public string BackSize2 { get; set; }
        public string BackPrice3 { get; set; }
        public string BackSize3 { get; set; }
    }

    public class Market
    {
        public string marketId { get; set; }
        public bool inplay { get; set; }
        public object totalMatched { get; set; }
        public object totalAvailable { get; set; }
        public string priceStatus { get; set; }
        public List<Event> events { get; set; }
    }

    public class Session
    {
        public string SelectionId { get; set; }
        public string RunnerName { get; set; }
        public string LayPrice1 { get; set; }
        public string LaySize1 { get; set; }
        public string BackPrice1 { get; set; }
        public string BackSize1 { get; set; }
        public string GameStatus { get; set; }
        public object FinalStatus { get; set; }
        public bool isActive { get; set; }
        public double min { get; set; }
        public double max { get; set; }
    }

    public class GetData
    {
        public List<Market> market { get; set; }
        public List<Session> session { get; set; }
        public object commentary { get; set; }
        public object MarketID { get; set; }
        public object marketId { get; set; }
        public object update_at { get; set; }
        public object score { get; set; }
        public int counter { get; set; }
    }


}