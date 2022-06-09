﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bfnexchange.AccountsServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="AccountsServiceReference.IAccountsService")]
    public interface IAccountsService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountsService/AddtoUsersAccounts", ReplyAction="http://tempuri.org/IAccountsService/AddtoUsersAccountsResponse")]
        string AddtoUsersAccounts(
                    string AccountsTitle, 
                    string Debit, 
                    string Credit, 
                    int UserID, 
                    string MarketBookID, 
                    System.DateTime CreatedDate, 
                    string AgentRate, 
                    string SuperRate, 
                    string samiadminrate, 
                    string ComissionRate, 
                    decimal OpeningBalance, 
                    bool isCreditAmount, 
                    string EventType, 
                    string WinnerName, 
                    string EventId, 
                    string EventName, 
                    string MarketBookName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountsService/AddtoUsersAccounts", ReplyAction="http://tempuri.org/IAccountsService/AddtoUsersAccountsResponse")]
        System.Threading.Tasks.Task<string> AddtoUsersAccountsAsync(
                    string AccountsTitle, 
                    string Debit, 
                    string Credit, 
                    int UserID, 
                    string MarketBookID, 
                    System.DateTime CreatedDate, 
                    string AgentRate, 
                    string SuperRate, 
                    string samiadminrate, 
                    string ComissionRate, 
                    decimal OpeningBalance, 
                    bool isCreditAmount, 
                    string EventType, 
                    string WinnerName, 
                    string EventId, 
                    string EventName, 
                    string MarketBookName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountsService/GetAccountsDatabyUserIdDateRangeandEventType", ReplyAction="http://tempuri.org/IAccountsService/GetAccountsDatabyUserIdDateRangeandEventTypeR" +
            "esponse")]
        bfnexchange.Services.DBModel.SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result[] GetAccountsDatabyUserIdDateRangeandEventType(int userID, string From, string To, bool isCreditAmount, string Eventtype);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountsService/GetAccountsDatabyUserIdDateRangeandEventType", ReplyAction="http://tempuri.org/IAccountsService/GetAccountsDatabyUserIdDateRangeandEventTypeR" +
            "esponse")]
        System.Threading.Tasks.Task<bfnexchange.Services.DBModel.SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result[]> GetAccountsDatabyUserIdDateRangeandEventTypeAsync(int userID, string From, string To, bool isCreditAmount, string Eventtype);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountsService/GetDistinctEventTypesfromAccounts", ReplyAction="http://tempuri.org/IAccountsService/GetDistinctEventTypesfromAccountsResponse")]
        bfnexchange.Services.DBModel.SP_UserAccounts_GetDistinctEventTypes_Result[] GetDistinctEventTypesfromAccounts();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountsService/GetDistinctEventTypesfromAccounts", ReplyAction="http://tempuri.org/IAccountsService/GetDistinctEventTypesfromAccountsResponse")]
        System.Threading.Tasks.Task<bfnexchange.Services.DBModel.SP_UserAccounts_GetDistinctEventTypes_Result[]> GetDistinctEventTypesfromAccountsAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountsService/GetAccountsCashReceivedorPaidbyDataRange", ReplyAction="http://tempuri.org/IAccountsService/GetAccountsCashReceivedorPaidbyDataRangeRespo" +
            "nse")]
        bfnexchange.Services.DBModel.SP_UserAccounts_GetDatabyCashRecivedorPaidandDateRange_Result GetAccountsCashReceivedorPaidbyDataRange(int userID, string From, string To);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountsService/GetAccountsCashReceivedorPaidbyDataRange", ReplyAction="http://tempuri.org/IAccountsService/GetAccountsCashReceivedorPaidbyDataRangeRespo" +
            "nse")]
        System.Threading.Tasks.Task<bfnexchange.Services.DBModel.SP_UserAccounts_GetDatabyCashRecivedorPaidandDateRange_Result> GetAccountsCashReceivedorPaidbyDataRangeAsync(int userID, string From, string To);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAccountsServiceChannel : bfnexchange.AccountsServiceReference.IAccountsService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AccountsServiceClient : System.ServiceModel.ClientBase<bfnexchange.AccountsServiceReference.IAccountsService>, bfnexchange.AccountsServiceReference.IAccountsService {
        
        public AccountsServiceClient() {
        }
        
        public AccountsServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AccountsServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AccountsServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AccountsServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string AddtoUsersAccounts(
                    string AccountsTitle, 
                    string Debit, 
                    string Credit, 
                    int UserID, 
                    string MarketBookID, 
                    System.DateTime CreatedDate, 
                    string AgentRate, 
                    string SuperRate, 
                    string samiadminrate, 
                    string ComissionRate, 
                    decimal OpeningBalance, 
                    bool isCreditAmount, 
                    string EventType, 
                    string WinnerName, 
                    string EventId, 
                    string EventName, 
                    string MarketBookName) {
            return base.Channel.AddtoUsersAccounts(AccountsTitle, Debit, Credit, UserID, MarketBookID, CreatedDate, AgentRate, SuperRate, samiadminrate, ComissionRate, OpeningBalance, isCreditAmount, EventType, WinnerName, EventId, EventName, MarketBookName);
        }
        
        public System.Threading.Tasks.Task<string> AddtoUsersAccountsAsync(
                    string AccountsTitle, 
                    string Debit, 
                    string Credit, 
                    int UserID, 
                    string MarketBookID, 
                    System.DateTime CreatedDate, 
                    string AgentRate, 
                    string SuperRate, 
                    string samiadminrate, 
                    string ComissionRate, 
                    decimal OpeningBalance, 
                    bool isCreditAmount, 
                    string EventType, 
                    string WinnerName, 
                    string EventId, 
                    string EventName, 
                    string MarketBookName) {
            return base.Channel.AddtoUsersAccountsAsync(AccountsTitle, Debit, Credit, UserID, MarketBookID, CreatedDate, AgentRate, SuperRate, samiadminrate, ComissionRate, OpeningBalance, isCreditAmount, EventType, WinnerName, EventId, EventName, MarketBookName);
        }
        
        public bfnexchange.Services.DBModel.SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result[] GetAccountsDatabyUserIdDateRangeandEventType(int userID, string From, string To, bool isCreditAmount, string Eventtype) {
            return base.Channel.GetAccountsDatabyUserIdDateRangeandEventType(userID, From, To, isCreditAmount, Eventtype);
        }
        
        public System.Threading.Tasks.Task<bfnexchange.Services.DBModel.SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result[]> GetAccountsDatabyUserIdDateRangeandEventTypeAsync(int userID, string From, string To, bool isCreditAmount, string Eventtype) {
            return base.Channel.GetAccountsDatabyUserIdDateRangeandEventTypeAsync(userID, From, To, isCreditAmount, Eventtype);
        }
        
        public bfnexchange.Services.DBModel.SP_UserAccounts_GetDistinctEventTypes_Result[] GetDistinctEventTypesfromAccounts() {
            return base.Channel.GetDistinctEventTypesfromAccounts();
        }
        
        public System.Threading.Tasks.Task<bfnexchange.Services.DBModel.SP_UserAccounts_GetDistinctEventTypes_Result[]> GetDistinctEventTypesfromAccountsAsync() {
            return base.Channel.GetDistinctEventTypesfromAccountsAsync();
        }
        
        public bfnexchange.Services.DBModel.SP_UserAccounts_GetDatabyCashRecivedorPaidandDateRange_Result GetAccountsCashReceivedorPaidbyDataRange(int userID, string From, string To) {
            return base.Channel.GetAccountsCashReceivedorPaidbyDataRange(userID, From, To);
        }
        
        public System.Threading.Tasks.Task<bfnexchange.Services.DBModel.SP_UserAccounts_GetDatabyCashRecivedorPaidandDateRange_Result> GetAccountsCashReceivedorPaidbyDataRangeAsync(int userID, string From, string To) {
            return base.Channel.GetAccountsCashReceivedorPaidbyDataRangeAsync(userID, From, To);
        }
    }
}
