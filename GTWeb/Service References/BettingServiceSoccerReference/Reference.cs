﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GTWeb.BettingServiceSoccerReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BettingServiceSoccerReference.IBettingServiceSoccer")]
    public interface IBettingServiceSoccer {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceSoccer/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingServiceSoccer/GetDataFromBetfairReadOnlyResponse")]
        void GetDataFromBetfairReadOnly();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceSoccer/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingServiceSoccer/GetDataFromBetfairReadOnlyResponse")]
        System.Threading.Tasks.Task GetDataFromBetfairReadOnlyAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceSoccer/GetCurrentMarketBookCricket", ReplyAction="http://tempuri.org/IBettingServiceSoccer/GetCurrentMarketBookCricketResponse")]
        void GetCurrentMarketBookCricket(string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceSoccer/GetCurrentMarketBookCricket", ReplyAction="http://tempuri.org/IBettingServiceSoccer/GetCurrentMarketBookCricketResponse")]
        System.Threading.Tasks.Task GetCurrentMarketBookCricketAsync(string Password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBettingServiceSoccerChannel : GTWeb.BettingServiceSoccerReference.IBettingServiceSoccer, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BettingServiceSoccerClient : System.ServiceModel.ClientBase<GTWeb.BettingServiceSoccerReference.IBettingServiceSoccer>, GTWeb.BettingServiceSoccerReference.IBettingServiceSoccer {
        
        public BettingServiceSoccerClient() {
        }
        
        public BettingServiceSoccerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BettingServiceSoccerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BettingServiceSoccerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BettingServiceSoccerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
