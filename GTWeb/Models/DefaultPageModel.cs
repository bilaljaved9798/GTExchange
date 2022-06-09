using GTWeb.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTWeb.Models
{
    public class DefaultPageModel
    {
        public string WelcomeMessage { get; set; }
        public string WelcomeHeading { get; set; }
        public string Rule { get; set; }
        public List<string> ModalContent { get; set; }
        public List<AllMarketsInPlay> AllMarkets { get; set; }
        public List<TodayHorserace> TodayHorseRacing { get; set; }
        public List<TodayHorserace> TodayGreyRacing { get; set; }
        public string ViewType { get; set; }

    }
   
    public class TodayHorserace
    {
        public string RaceName { get; set; }
        public string MarketBookID { get; set; }
    }
}