﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GTWeb.BettingServiceTennisReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BettingServiceTennisReference.IBettingServiceTennis")]
    public interface IBettingServiceTennis {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceTennis/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingServiceTennis/GetDataFromBetfairReadOnlyResponse")]
        void GetDataFromBetfairReadOnly();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceTennis/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingServiceTennis/GetDataFromBetfairReadOnlyResponse")]
        System.Threading.Tasks.Task GetDataFromBetfairReadOnlyAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceTennis/GetCurrentMarketBookCricket", ReplyAction="http://tempuri.org/IBettingServiceTennis/GetCurrentMarketBookCricketResponse")]
        void GetCurrentMarketBookCricket(string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceTennis/GetCurrentMarketBookCricket", ReplyAction="http://tempuri.org/IBettingServiceTennis/GetCurrentMarketBookCricketResponse")]
        System.Threading.Tasks.Task GetCurrentMarketBookCricketAsync(string Password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBettingServiceTennisChannel : GTWeb.BettingServiceTennisReference.IBettingServiceTennis, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BettingServiceTennisClient : System.ServiceModel.ClientBase<GTWeb.BettingServiceTennisReference.IBettingServiceTennis>, GTWeb.BettingServiceTennisReference.IBettingServiceTennis {
        
        public BettingServiceTennisClient() {
        }
        
        public BettingServiceTennisClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BettingServiceTennisClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BettingServiceTennisClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BettingServiceTennisClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void GetDataFromBetfairReadOnly() {
            base.Channel.GetDataFromBetfairReadOnly();
        }
        
        public System.Threading.Tasks.Task GetDataFromBetfairReadOnlyAsync() {
            return base.Channel.GetDataFromBetfairReadOnlyAsync();
        }
        
        public void GetCurrentMarketBookCricket(string Password) {
            base.Channel.GetCurrentMarketBookCricket(Password);
        }
        
        public System.Threading.Tasks.Task GetCurrentMarketBookCricketAsync(string Password) {
            return base.Channel.GetCurrentMarketBookCricketAsync(Password);
        }
    }
}
