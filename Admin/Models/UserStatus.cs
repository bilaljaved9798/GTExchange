using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bftradeline.Models
{
    public class UserStatus
    {
        public bool isBlocked { get; set; }
        public bool isDeleted { get; set; }
        public bool Loggedin { get; set; }
        public long CurrentLoggedInID { get; set; }
    }
}

namespace bftradeline.Models123
{
    using System;
    public partial class SP_Users_GetCommissionAccountIDandBookAccountID_Result
    {
        public Nullable<int> CommisionAccountID { get; set; }
        public Nullable<int> BookAccountID { get; set; }
    }
    public partial class SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result
    {
        public string AccountsTitle { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string CreatedDate { get; set; }
        public Nullable<decimal> OpeningBalance { get; set; }
        public Nullable<int> UserID { get; set; }
        public string MarketBookID { get; set; }
        public string UserName { get; set; }
        public string AgentRate { get; set; }
    }
    public partial class SP_URLsData_GetAllData_Result
    {
        public string URLForData { get; set; }
        public string EventType { get; set; }
        public string Scd { get; set; }
        public string GetDataFrom { get; set; }
    }
    public partial class SP_Users_GetMaxOddBackandLay_Result
    {
        public Nullable<decimal> MaxOddBack { get; set; }
        public Nullable<decimal> MaxOddLay { get; set; }
        public Nullable<bool> CheckforMaxOddBack { get; set; }
        public Nullable<bool> CheckforMaxOddLay { get; set; }
    }
    public partial class SP_UserMarket_GetMarketForAllowedBetting_Result
    {
        public string Market { get; set; }
        public string MarketCatalogueID { get; set; }
        public string EventTypeName { get; set; }
        public bool BettingAllowed { get; set; }
        public Nullable<System.DateTime> EventOpenDate { get; set; }
    }
    public partial class SP_UserAccounts_GetDistinctEventTypes_Result
    {
        public string Eventtype { get; set; }
    }
}