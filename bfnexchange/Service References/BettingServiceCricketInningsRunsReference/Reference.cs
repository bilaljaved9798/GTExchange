﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bfnexchange.BettingServiceCricketInningsRunsReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BettingServiceCricketInningsRunsReference.IBettingServiceCricketInningsRuns")]
    public interface IBettingServiceCricketInningsRuns {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceCricketInningsRuns/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingServiceCricketInningsRuns/GetDataFromBetfairReadOnlyRe" +
            "sponse")]
        void GetDataFromBetfairReadOnly();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceCricketInningsRuns/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingServiceCricketInningsRuns/GetDataFromBetfairReadOnlyRe" +
            "sponse")]
        System.Threading.Tasks.Task GetDataFromBetfairReadOnlyAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceCricketInningsRuns/GetCurrentMarketBookCricket", ReplyAction="http://tempuri.org/IBettingServiceCricketInningsRuns/GetCurrentMarketBookCricketR" +
            "esponse")]
        void GetCurrentMarketBookCricket(string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceCricketInningsRuns/GetCurrentMarketBookCricket", ReplyAction="http://tempuri.org/IBettingServiceCricketInningsRuns/GetCurrentMarketBookCricketR" +
            "esponse")]
        System.Threading.Tasks.Task GetCurrentMarketBookCricketAsync(string Password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBettingServiceCricketInningsRunsChannel : bfnexchange.BettingServiceCricketInningsRunsReference.IBettingServiceCricketInningsRuns, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BettingServiceCricketInningsRunsClient : System.ServiceModel.ClientBase<bfnexchange.BettingServiceCricketInningsRunsReference.IBettingServiceCricketInningsRuns>, bfnexchange.BettingServiceCricketInningsRunsReference.IBettingServiceCricketInningsRuns {
        
        public BettingServiceCricketInningsRunsClient() {
        }
        
        public BettingServiceCricketInningsRunsClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BettingServiceCricketInningsRunsClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BettingServiceCricketInningsRunsClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BettingServiceCricketInningsRunsClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
