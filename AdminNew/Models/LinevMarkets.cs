using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bftradeline.Models
{
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
    }
}
