using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bftradeline.Models
{
    public class AllUserMarkets
    {
        public string EventTypeID { get; set; }
        public string CompetitionID { get; set; }
        public string EventID { get; set; }
        public string MarketCatalogueID { get; set; }
        public Nullable<int> UpdatedbyID { get; set; }
        public string EventTypeName { get; set; }
        public string CompetitionName { get; set; }
        public string EventName { get; set; }
        public string MarketCatalogueName { get; set; }
        public Nullable<System.DateTime> EventOpenDate { get; set; }

        public string SheetName { get; set; }
        public string AssociateEventID { get; set; }
        public string GetMatchUpdatesFrom { get; set; }
        public string TotalOvers { get; set; }
        public string CountryCode { get; set; }

    }
}