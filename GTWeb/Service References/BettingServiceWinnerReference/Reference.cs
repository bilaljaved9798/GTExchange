﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GTWeb.BettingServiceWinnerReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BettingServiceWinnerReference.IBettingServiceWinner")]
    public interface IBettingServiceWinner {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceWinner/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingServiceWinner/GetDataFromBetfairReadOnlyResponse")]
        void GetDataFromBetfairReadOnly();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceWinner/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingServiceWinner/GetDataFromBetfairReadOnlyResponse")]
        System.Threading.Tasks.Task GetDataFromBetfairReadOnlyAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceWinner/GetCurrentMarketBookCricket", ReplyAction="http://tempuri.org/IBettingServiceWinner/GetCurrentMarketBookCricketResponse")]
        void GetCurrentMarketBookCricket(string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceWinner/GetCurrentMarketBookCricket", ReplyAction="http://tempuri.org/IBettingServiceWinner/GetCurrentMarketBookCricketResponse")]
        System.Threading.Tasks.Task GetCurrentMarketBookCricketAsync(string Password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBettingServiceWinnerChannel : GTWeb.BettingServiceWinnerReference.IBettingServiceWinner, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BettingServiceWinnerClient : System.ServiceModel.ClientBase<GTWeb.BettingServiceWinnerReference.IBettingServiceWinner>, GTWeb.BettingServiceWinnerReference.IBettingServiceWinner {
        
        public BettingServiceWinnerClient() {
        }
        
        public BettingServiceWinnerClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BettingServiceWinnerClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BettingServiceWinnerClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BettingServiceWinnerClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
