using bfnexchange.Services.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAccountsService" in both code and config file together.
    [ServiceContract]
    public interface IAccountsService
    {
        [OperationContract]
        string AddtoUsersAccounts(string AccountsTitle, string Debit, string Credit, int UserID, string MarketBookID, DateTime CreatedDate, string AgentRate, string SuperRate,string samiadminrate, string ComissionRate,decimal OpeningBalance, bool isCreditAmount, string EventType, string WinnerName, string EventId, string EventName, string MarketBookName);
        List<SP_UserAccounts_GetDatabyUserIDandDateRange_Result> GetAccountsDatabyUserIDandDateRange(int userID, string From, string To, bool isCreditAmount);
        List<SP_UserAccounts_GetDatabyCreatedByID_Result> GetAccountsDatabyCreatedbyID(int userID, bool isCreditAmount);
        List<SP_UserAccounts_GetDatabyAdminID_Result> GetAccountsDatabyAdmin(int UserID,bool isCreditAmount);

        List<SP_UserAccounts_GetDatabyCommission_Result> GetAccountsDatabyCommision();
        [OperationContract]
        List<SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result> GetAccountsDatabyUserIdDateRangeandEventType(int userID, string From, string To, bool isCreditAmount, string Eventtype);
        [OperationContract]
        List<SP_UserAccounts_GetDistinctEventTypes_Result> GetDistinctEventTypesfromAccounts();
        [OperationContract]
        SP_UserAccounts_GetDatabyCashRecivedorPaidandDateRange_Result GetAccountsCashReceivedorPaidbyDataRange(int userID, string From, string To);
    }
}
