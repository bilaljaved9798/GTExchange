using bfnexchange.Services.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Activation;
namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AccountsService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AccountsService.svc or AccountsService.svc.cs at the Solution Explorer and start debugging.
   [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,InstanceContextMode =InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class AccountsService : IAccountsService
    {
        NExchangeEntities dbEntities = new NExchangeEntities();
        public string AddtoUsersAccounts(string AccountsTitle,string Debit,string Credit,int UserID,string MarketBookID,DateTime CreatedDate,string AgentRate, string SuperRate,string samiadminRate, string ComissionRate,decimal OpeningBalance,bool isCreditAmount,string EventType,string WinnerName,string EventId,string EventName,string MarketBookName)
        {
            SP_UsersAccounts_Insert_Result objUserAcounts = dbEntities.SP_UsersAccounts_Insert(AccountsTitle, Debit, Credit, UserID, MarketBookID, CreatedDate, AgentRate, SuperRate,samiadminRate, ComissionRate,OpeningBalance,isCreditAmount,EventType,WinnerName,EventId,EventName,MarketBookName).FirstOrDefault();     
            return objUserAcounts.ID.ToString();
        }
        public void AddtoComissionAccounts(string AccountsTitle,string Amount,long USerAccountsID,DateTime CreatedDate)
        {
            dbEntities.SP_CommisionAccounts_Insert(AccountsTitle, Amount, USerAccountsID, CreatedDate);
        }
        public List<SP_UserAccounts_GetDatabyUserIDandDateRange_Result> GetAccountsDatabyUserIDandDateRange(int userID,string From,string To, bool isCreditAmount)
        {
            List<SP_UserAccounts_GetDatabyUserIDandDateRange_Result> lstAccountsResults = dbEntities.SP_UserAccounts_GetDatabyUserIDandDateRange(userID, From, To,isCreditAmount).ToList();
            return lstAccountsResults;
        }
        public List<SP_UserAccounts_GetDatabyCreatedByID_Result> GetAccountsDatabyCreatedbyID(int userID, bool isCreditAmount)
        {
            List<SP_UserAccounts_GetDatabyCreatedByID_Result> lstAccountsResults = dbEntities.SP_UserAccounts_GetDatabyCreatedByID(userID,isCreditAmount).ToList();
            return lstAccountsResults;
        }
        public List<SP_UserAccounts_GetDatabyCreatedByIDForsuper_Result> GetAccountsDatabyCreatedByIDForSuper(int userID, bool isCreditAmount)
        {
            List<SP_UserAccounts_GetDatabyCreatedByIDForsuper_Result> lstAccountsResults = dbEntities.SP_UserAccounts_GetDatabyCreatedByIDForsuper(userID, isCreditAmount).ToList();
            return lstAccountsResults;
        }
        public List<SP_UserAccounts_GetDatabyCreatedByIDForSamiadmin_Result> GetAccountsDatabyCreatedByIDForSamiAdmin(int userID, bool isCreditAmount)
        {
            List<SP_UserAccounts_GetDatabyCreatedByIDForSamiadmin_Result> lstAccountsResults = dbEntities.SP_UserAccounts_GetDatabyCreatedByIDForSamiadmin(userID, isCreditAmount).ToList();
            return lstAccountsResults;
        }

        public List<SP_UserAccounts_GetDatabyAdminID_Result> GetAccountsDatabyAdmin(int UserID, bool isCreditAmount)
        {
            List<SP_UserAccounts_GetDatabyAdminID_Result> lstAccountsResults = dbEntities.SP_UserAccounts_GetDatabyAdminID(UserID,isCreditAmount).ToList();
            return lstAccountsResults;
        }

        public List<SP_UserAccounts_GetDatabySuperID_Result> GetAccountsDataForSuper(int UserID, bool isCreditAmount)
        {
            List<SP_UserAccounts_GetDatabySuperID_Result> lstAccountsResults = dbEntities.SP_UserAccounts_GetDatabySuperID(UserID, isCreditAmount).ToList();
            return lstAccountsResults;
        }
        public List<SP_UserAccounts_GetDatabySuperID_Result> GetAccountsDataForSamiAdmin(int UserID, bool isCreditAmount)
        {
            List<SP_UserAccounts_GetDatabySuperID_Result> lstAccountsResults = dbEntities.SP_UserAccounts_GetDatabySuperID(UserID, isCreditAmount).ToList();
            return lstAccountsResults;
        }
        public List<SP_UserAccounts_GetDatabyCommission_Result> GetAccountsDatabyCommision()
        {
            List<SP_UserAccounts_GetDatabyCommission_Result> lstAccountsResults = dbEntities.SP_UserAccounts_GetDatabyCommission().ToList();
            return lstAccountsResults;
        }
        public List<SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result> GetAccountsDatabyUserIdDateRangeandEventType(int userID, string From, string To, bool isCreditAmount,string Eventtype)
        {
            List<SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result> lstAccountsresults = dbEntities.SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType(userID, From, To, isCreditAmount, Eventtype).ToList();
            return lstAccountsresults;
        }
        public List<SP_UserAccounts_GetDistinctEventTypes_Result> GetDistinctEventTypesfromAccounts()
        {
            List<SP_UserAccounts_GetDistinctEventTypes_Result> lstEventTypesfromAccounts = dbEntities.SP_UserAccounts_GetDistinctEventTypes().ToList();
            return lstEventTypesfromAccounts;
        }
        public SP_UserAccounts_GetDatabyCashRecivedorPaidandDateRange_Result GetAccountsCashReceivedorPaidbyDataRange(int userID, string From, string To)
        {
            return dbEntities.SP_UserAccounts_GetDatabyCashRecivedorPaidandDateRange(userID, From, To).FirstOrDefault();
        }
    }
}
