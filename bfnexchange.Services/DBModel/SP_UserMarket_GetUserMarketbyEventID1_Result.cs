//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bfnexchange.Services.DBModel
{
    using System;
    
    public partial class SP_UserMarket_GetUserMarketbyEventID1_Result
    {
        public long ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string EventTypeID { get; set; }
        public string CompetitionID { get; set; }
        public string EventID { get; set; }
        public string MarketCatalogueID { get; set; }
        public Nullable<int> UpdatedbyID { get; set; }
        public string EventTypeName { get; set; }
        public string CompetitionName { get; set; }
        public string EventName { get; set; }
        public string MarketCatalogueName { get; set; }
        public Nullable<bool> isOpenedbyUser { get; set; }
        public Nullable<System.DateTime> EventOpenDate { get; set; }
        public string SheetName { get; set; }
        public bool BettingAllowed { get; set; }
        public string AssociateEventID { get; set; }
        public string MarketStatus { get; set; }
        public string CricketAPIMatchKey { get; set; }
        public Nullable<System.DateTime> marketClosedTime { get; set; }
        public string GetMatchUpdatesFrom { get; set; }
        public string CountryCode { get; set; }
        public string TotalOvers { get; set; }
        public Nullable<bool> RaceInPlayAllow { get; set; }
    }
}
