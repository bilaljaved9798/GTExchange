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
    
    public partial class SP_UserMarket_GetDistinctKJMarketsbyEventID_Result
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
        public bool isOpenedbyUser { get; set; }
    }
}
