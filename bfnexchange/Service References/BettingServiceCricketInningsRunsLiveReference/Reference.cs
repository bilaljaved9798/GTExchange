﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bfnexchange.BettingServiceCricketInningsRunsLiveReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BettingServiceCricketInningsRunsLiveReference.IBettingServicesCricketInningsRunLi" +
        "ve")]
    public interface IBettingServicesCricketInningsRunLive {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServicesCricketInningsRunLive/GetDataFromBetfairReadOn" +
            "ly", ReplyAction="http://tempuri.org/IBettingServicesCricketInningsRunLive/GetDataFromBetfairReadOn" +
            "lyResponse")]
        void GetDataFromBetfairReadOnly();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServicesCricketInningsRunLive/GetDataFromBetfairReadOn" +
            "ly", ReplyAction="http://tempuri.org/IBettingServicesCricketInningsRunLive/GetDataFromBetfairReadOn" +
            "lyResponse")]
        System.Threading.Tasks.Task GetDataFromBetfairReadOnlyAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServicesCricketInningsRunLive/GetCurrentMarketBookCric" +
            "ketlive", ReplyAction="http://tempuri.org/IBettingServicesCricketInningsRunLive/GetCurrentMarketBookCric" +
            "ketliveResponse")]
        void GetCurrentMarketBookCricketlive(string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServicesCricketInningsRunLive/GetCurrentMarketBookCric" +
            "ketlive", ReplyAction="http://tempuri.org/IBettingServicesCricketInningsRunLive/GetCurrentMarketBookCric" +
            "ketliveResponse")]
        System.Threading.Tasks.Task GetCurrentMarketBookCricketliveAsync(string Password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBettingServicesCricketInningsRunLiveChannel : bfnexchange.BettingServiceCricketInningsRunsLiveReference.IBettingServicesCricketInningsRunLive, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BettingServicesCricketInningsRunLiveClient : System.ServiceModel.ClientBase<bfnexchange.BettingServiceCricketInningsRunsLiveReference.IBettingServicesCricketInningsRunLive>, bfnexchange.BettingServiceCricketInningsRunsLiveReference.IBettingServicesCricketInningsRunLive {
        
        public BettingServicesCricketInningsRunLiveClient() {
        }
        
        public BettingServicesCricketInningsRunLiveClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BettingServicesCricketInningsRunLiveClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BettingServicesCricketInningsRunLiveClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BettingServicesCricketInningsRunLiveClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void GetDataFromBetfairReadOnly() {
            base.Channel.GetDataFromBetfairReadOnly();
        }
        
        public System.Threading.Tasks.Task GetDataFromBetfairReadOnlyAsync() {
            return base.Channel.GetDataFromBetfairReadOnlyAsync();
        }
        
        public void GetCurrentMarketBookCricketlive(string Password) {
            base.Channel.GetCurrentMarketBookCricketlive(Password);
        }
        
        public System.Threading.Tasks.Task GetCurrentMarketBookCricketliveAsync(string Password) {
            return base.Channel.GetCurrentMarketBookCricketliveAsync(Password);
        }
    }
}
