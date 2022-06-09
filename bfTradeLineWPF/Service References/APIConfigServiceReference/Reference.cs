﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace globaltraders.APIConfigServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="APIConfigServiceReference.IAPIConfigService")]
    public interface IAPIConfigService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAPIConfigService/GetAPIConfigData", ReplyAction="http://tempuri.org/IAPIConfigService/GetAPIConfigDataResponse")]
        string GetAPIConfigData(int ID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAPIConfigService/UpdateSession", ReplyAction="http://tempuri.org/IAPIConfigService/UpdateSessionResponse")]
        void UpdateSession(string session, int ID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAPIConfigService/GetPoundRate", ReplyAction="http://tempuri.org/IAPIConfigService/GetPoundRateResponse")]
        string GetPoundRate();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAPIConfigService/SetPoundRate", ReplyAction="http://tempuri.org/IAPIConfigService/SetPoundRateResponse")]
        void SetPoundRate(string Poundrate);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAPIConfigServiceChannel : globaltraders.APIConfigServiceReference.IAPIConfigService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class APIConfigServiceClient : System.ServiceModel.ClientBase<globaltraders.APIConfigServiceReference.IAPIConfigService>, globaltraders.APIConfigServiceReference.IAPIConfigService {
        
        public APIConfigServiceClient() {
        }
        
        public APIConfigServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public APIConfigServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public APIConfigServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public APIConfigServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetAPIConfigData(int ID) {
            return base.Channel.GetAPIConfigData(ID);
        }
        
        public void UpdateSession(string session, int ID) {
            base.Channel.UpdateSession(session, ID);
        }
        
        public string GetPoundRate() {
            return base.Channel.GetPoundRate();
        }
        
        public void SetPoundRate(string Poundrate) {
            base.Channel.SetPoundRate(Poundrate);
        }
    }
}
