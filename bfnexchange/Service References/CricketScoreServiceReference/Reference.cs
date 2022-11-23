﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bfnexchange.CricketScoreServiceReference {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CricketData_GetMatchKeys_Result", Namespace="http://schemas.datacontract.org/2004/07/CricketScoreService.DBModel")]
    [System.SerializableAttribute()]
    public partial class CricketData_GetMatchKeys_Result : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CricketMatchKeyField;
        
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
        public string CricketMatchKey {
            get {
                return this.CricketMatchKeyField;
            }
            set {
                if ((object.ReferenceEquals(this.CricketMatchKeyField, value) != true)) {
                    this.CricketMatchKeyField = value;
                    this.RaisePropertyChanged("CricketMatchKey");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CricketScoreServiceReference.IService1")]
    public interface IService1 {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/AddCricketMatchKey", ReplyAction="http://tempuri.org/IService1/AddCricketMatchKeyResponse")]
        void AddCricketMatchKey(string CricketMatchKey, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/AddCricketMatchKey", ReplyAction="http://tempuri.org/IService1/AddCricketMatchKeyResponse")]
        System.Threading.Tasks.Task AddCricketMatchKeyAsync(string CricketMatchKey, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetMatchDatabyKey", ReplyAction="http://tempuri.org/IService1/GetMatchDatabyKeyResponse")]
        string GetMatchDatabyKey(string CricketMatchKey, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetMatchDatabyKey", ReplyAction="http://tempuri.org/IService1/GetMatchDatabyKeyResponse")]
        System.Threading.Tasks.Task<string> GetMatchDatabyKeyAsync(string CricketMatchKey, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/UpdateMatchData", ReplyAction="http://tempuri.org/IService1/UpdateMatchDataResponse")]
        void UpdateMatchData(string CricketMatchKey, string MatchData, string Password, string MatchStatus);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/UpdateMatchData", ReplyAction="http://tempuri.org/IService1/UpdateMatchDataResponse")]
        System.Threading.Tasks.Task UpdateMatchDataAsync(string CricketMatchKey, string MatchData, string Password, string MatchStatus);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/DeleteMatchbyKey", ReplyAction="http://tempuri.org/IService1/DeleteMatchbyKeyResponse")]
        void DeleteMatchbyKey(string CricketMatchKey, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/DeleteMatchbyKey", ReplyAction="http://tempuri.org/IService1/DeleteMatchbyKeyResponse")]
        System.Threading.Tasks.Task DeleteMatchbyKeyAsync(string CricketMatchKey, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetMatchKeys", ReplyAction="http://tempuri.org/IService1/GetMatchKeysResponse")]
        bfnexchange.CricketScoreServiceReference.CricketData_GetMatchKeys_Result[] GetMatchKeys(string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService1/GetMatchKeys", ReplyAction="http://tempuri.org/IService1/GetMatchKeysResponse")]
        System.Threading.Tasks.Task<bfnexchange.CricketScoreServiceReference.CricketData_GetMatchKeys_Result[]> GetMatchKeysAsync(string Password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IService1Channel : bfnexchange.CricketScoreServiceReference.IService1, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Service1Client : System.ServiceModel.ClientBase<bfnexchange.CricketScoreServiceReference.IService1>, bfnexchange.CricketScoreServiceReference.IService1 {
        
        public Service1Client() {
        }
        
        public Service1Client(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Service1Client(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Service1Client(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void AddCricketMatchKey(string CricketMatchKey, string Password) {
            base.Channel.AddCricketMatchKey(CricketMatchKey, Password);
        }
        
        public System.Threading.Tasks.Task AddCricketMatchKeyAsync(string CricketMatchKey, string Password) {
            return base.Channel.AddCricketMatchKeyAsync(CricketMatchKey, Password);
        }
        
        public string GetMatchDatabyKey(string CricketMatchKey, string Password) {
            return base.Channel.GetMatchDatabyKey(CricketMatchKey, Password);
        }
        
        public System.Threading.Tasks.Task<string> GetMatchDatabyKeyAsync(string CricketMatchKey, string Password) {
            return base.Channel.GetMatchDatabyKeyAsync(CricketMatchKey, Password);
        }
        
        public void UpdateMatchData(string CricketMatchKey, string MatchData, string Password, string MatchStatus) {
            base.Channel.UpdateMatchData(CricketMatchKey, MatchData, Password, MatchStatus);
        }
        
        public System.Threading.Tasks.Task UpdateMatchDataAsync(string CricketMatchKey, string MatchData, string Password, string MatchStatus) {
            return base.Channel.UpdateMatchDataAsync(CricketMatchKey, MatchData, Password, MatchStatus);
        }
        
        public void DeleteMatchbyKey(string CricketMatchKey, string Password) {
            base.Channel.DeleteMatchbyKey(CricketMatchKey, Password);
        }
        
        public System.Threading.Tasks.Task DeleteMatchbyKeyAsync(string CricketMatchKey, string Password) {
            return base.Channel.DeleteMatchbyKeyAsync(CricketMatchKey, Password);
        }
        
        public bfnexchange.CricketScoreServiceReference.CricketData_GetMatchKeys_Result[] GetMatchKeys(string Password) {
            return base.Channel.GetMatchKeys(Password);
        }
        
        public System.Threading.Tasks.Task<bfnexchange.CricketScoreServiceReference.CricketData_GetMatchKeys_Result[]> GetMatchKeysAsync(string Password) {
            return base.Channel.GetMatchKeysAsync(Password);
        }
    }
}
