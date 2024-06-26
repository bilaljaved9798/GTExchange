﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace globaltraders.AccountsServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result", Namespace="http://schemas.datacontract.org/2004/07/bfnexchange.Services.DBModel")]
    [System.SerializableAttribute()]
    public partial class SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AccountsTitleField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string AgentRateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CreatedDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CreditField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DebitField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MarketBookIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<decimal> OpeningBalanceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> UserIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AccountsTitle {
            get {
                return this.AccountsTitleField;
            }
            set {
                if ((object.ReferenceEquals(this.AccountsTitleField, value) != true)) {
                    this.AccountsTitleField = value;
                    this.RaisePropertyChanged("AccountsTitle");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string AgentRate {
            get {
                return this.AgentRateField;
            }
            set {
                if ((object.ReferenceEquals(this.AgentRateField, value) != true)) {
                    this.AgentRateField = value;
                    this.RaisePropertyChanged("AgentRate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CreatedDate {
            get {
                return this.CreatedDateField;
            }
            set {
                if ((object.ReferenceEquals(this.CreatedDateField, value) != true)) {
                    this.CreatedDateField = value;
                    this.RaisePropertyChanged("CreatedDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Credit {
            get {
                return this.CreditField;
            }
            set {
                if ((object.ReferenceEquals(this.CreditField, value) != true)) {
                    this.CreditField = value;
                    this.RaisePropertyChanged("Credit");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Debit {
            get {
                return this.DebitField;
            }
            set {
                if ((object.ReferenceEquals(this.DebitField, value) != true)) {
                    this.DebitField = value;
                    this.RaisePropertyChanged("Debit");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MarketBookID {
            get {
                return this.MarketBookIDField;
            }
            set {
                if ((object.ReferenceEquals(this.MarketBookIDField, value) != true)) {
                    this.MarketBookIDField = value;
                    this.RaisePropertyChanged("MarketBookID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<decimal> OpeningBalance {
            get {
                return this.OpeningBalanceField;
            }
            set {
                if ((this.OpeningBalanceField.Equals(value) != true)) {
                    this.OpeningBalanceField = value;
                    this.RaisePropertyChanged("OpeningBalance");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> UserID {
            get {
                return this.UserIDField;
            }
            set {
                if ((this.UserIDField.Equals(value) != true)) {
                    this.UserIDField = value;
                    this.RaisePropertyChanged("UserID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserName {
            get {
                return this.UserNameField;
            }
            set {
                if ((object.ReferenceEquals(this.UserNameField, value) != true)) {
                    this.UserNameField = value;
                    this.RaisePropertyChanged("UserName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SP_UserAccounts_GetDistinctEventTypes_Result", Namespace="http://schemas.datacontract.org/2004/07/bfnexchange.Services.DBModel")]
    [System.SerializableAttribute()]
    public partial class SP_UserAccounts_GetDistinctEventTypes_Result : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EventtypeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Eventtype {
            get {
                return this.EventtypeField;
            }
            set {
                if ((object.ReferenceEquals(this.EventtypeField, value) != true)) {
                    this.EventtypeField = value;
                    this.RaisePropertyChanged("Eventtype");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SP_UserAccounts_GetDatabyCashRecivedorPaidandDateRange_Result", Namespace="http://schemas.datacontract.org/2004/07/bfnexchange.Services.DBModel")]
    [System.SerializableAttribute()]
    public partial class SP_UserAccounts_GetDatabyCashRecivedorPaidandDateRange_Result : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal TotCreditField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private decimal TotDebitField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal TotCredit {
            get {
                return this.TotCreditField;
            }
            set {
                if ((this.TotCreditField.Equals(value) != true)) {
                    this.TotCreditField = value;
                    this.RaisePropertyChanged("TotCredit");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public decimal TotDebit {
            get {
                return this.TotDebitField;
            }
            set {
                if ((this.TotDebitField.Equals(value) != true)) {
                    this.TotDebitField = value;
                    this.RaisePropertyChanged("TotDebit");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="AccountsServiceReference.IAccountsService")]
    public interface IAccountsService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountsService/AddtoUsersAccounts", ReplyAction="http://tempuri.org/IAccountsService/AddtoUsersAccountsResponse")]
        string AddtoUsersAccounts(string AccountsTitle, string Debit, string Credit, int UserID, string MarketBookID, System.DateTime CreatedDate, string AgentRate, string ComissionRate, decimal OpeningBalance, bool isCreditAmount, string EventType, string WinnerName, string EventId, string EventName, string MarketBookName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountsService/GetAccountsDatabyUserIdDateRangeandEventType", ReplyAction="http://tempuri.org/IAccountsService/GetAccountsDatabyUserIdDateRangeandEventTypeR" +
            "esponse")]
        globaltraders.AccountsServiceReference.SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result[] GetAccountsDatabyUserIdDateRangeandEventType(int userID, string From, string To, bool isCreditAmount, string Eventtype);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountsService/GetDistinctEventTypesfromAccounts", ReplyAction="http://tempuri.org/IAccountsService/GetDistinctEventTypesfromAccountsResponse")]
        globaltraders.AccountsServiceReference.SP_UserAccounts_GetDistinctEventTypes_Result[] GetDistinctEventTypesfromAccounts();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountsService/GetAccountsCashReceivedorPaidbyDataRange", ReplyAction="http://tempuri.org/IAccountsService/GetAccountsCashReceivedorPaidbyDataRangeRespo" +
            "nse")]
        globaltraders.AccountsServiceReference.SP_UserAccounts_GetDatabyCashRecivedorPaidandDateRange_Result GetAccountsCashReceivedorPaidbyDataRange(int userID, string From, string To);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAccountsServiceChannel : globaltraders.AccountsServiceReference.IAccountsService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AccountsServiceClient : System.ServiceModel.ClientBase<globaltraders.AccountsServiceReference.IAccountsService>, globaltraders.AccountsServiceReference.IAccountsService {
        
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
        
        public string AddtoUsersAccounts(string AccountsTitle, string Debit, string Credit, int UserID, string MarketBookID, System.DateTime CreatedDate, string AgentRate, string ComissionRate, decimal OpeningBalance, bool isCreditAmount, string EventType, string WinnerName, string EventId, string EventName, string MarketBookName) {
            return base.Channel.AddtoUsersAccounts(AccountsTitle, Debit, Credit, UserID, MarketBookID, CreatedDate, AgentRate, ComissionRate, OpeningBalance, isCreditAmount, EventType, WinnerName, EventId, EventName, MarketBookName);
        }
        
        public globaltraders.AccountsServiceReference.SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result[] GetAccountsDatabyUserIdDateRangeandEventType(int userID, string From, string To, bool isCreditAmount, string Eventtype) {
            return base.Channel.GetAccountsDatabyUserIdDateRangeandEventType(userID, From, To, isCreditAmount, Eventtype);
        }
        
        public globaltraders.AccountsServiceReference.SP_UserAccounts_GetDistinctEventTypes_Result[] GetDistinctEventTypesfromAccounts() {
            return base.Channel.GetDistinctEventTypesfromAccounts();
        }
        
        public globaltraders.AccountsServiceReference.SP_UserAccounts_GetDatabyCashRecivedorPaidandDateRange_Result GetAccountsCashReceivedorPaidbyDataRange(int userID, string From, string To) {
            return base.Channel.GetAccountsCashReceivedorPaidbyDataRange(userID, From, To);
        }
    }
}
