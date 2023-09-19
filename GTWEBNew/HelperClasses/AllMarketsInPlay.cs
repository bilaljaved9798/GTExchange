using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTExchNew.HelperClasses
{
   public class AllMarketsInPlay
    {
        public string MarketBookID { get; set; }
        public string MarketBookName { get; set; }
        public string ImagePath { get; set; }
        public string MarketStatus { get; set; }
        public string MarketStartTime { get; set; }
        public string Runner1 { get; set; }
        public string Runner2 { get; set; }
        public string Runner3 { get; set; }
        public string Runner1Back { get; set; }
        public string Runner1Lay { get; set; }
        public string Runner2Back { get; set; }
        public string Runner2Lay { get; set; }
        public string Runner3Back { get; set; }
        public string Runner3Lay { get; set; }
        public string Matched { get; set; }
        public string CountryCode { get; set; }
    }
}
