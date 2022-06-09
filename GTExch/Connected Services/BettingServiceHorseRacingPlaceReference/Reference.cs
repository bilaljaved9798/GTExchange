﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BettingServiceHorseRacingPlaceReference
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BettingServiceHorseRacingPlaceReference.IBettingServiceHorseRacePlace")]
    internal interface IBettingServiceHorseRacePlace
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceHorseRacePlace/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingServiceHorseRacePlace/GetDataFromBetfairReadOnlyRespon" +
            "se")]
        void GetDataFromBetfairReadOnly();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceHorseRacePlace/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingServiceHorseRacePlace/GetDataFromBetfairReadOnlyRespon" +
            "se")]
        System.Threading.Tasks.Task GetDataFromBetfairReadOnlyAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceHorseRacePlace/GetCurrentMarketBookCricket", ReplyAction="http://tempuri.org/IBettingServiceHorseRacePlace/GetCurrentMarketBookCricketRespo" +
            "nse")]
        void GetCurrentMarketBookCricket(string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingServiceHorseRacePlace/GetCurrentMarketBookCricket", ReplyAction="http://tempuri.org/IBettingServiceHorseRacePlace/GetCurrentMarketBookCricketRespo" +
            "nse")]
        System.Threading.Tasks.Task GetCurrentMarketBookCricketAsync(string Password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    internal interface IBettingServiceHorseRacePlaceChannel : BettingServiceHorseRacingPlaceReference.IBettingServiceHorseRacePlace, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3-preview3.21351.2")]
    internal partial class BettingServiceHorseRacePlaceClient : System.ServiceModel.ClientBase<BettingServiceHorseRacingPlaceReference.IBettingServiceHorseRacePlace>, BettingServiceHorseRacingPlaceReference.IBettingServiceHorseRacePlace
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public BettingServiceHorseRacePlaceClient() : 
                base(BettingServiceHorseRacePlaceClient.GetDefaultBinding(), BettingServiceHorseRacePlaceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IBettingServiceHorseRacePlace.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BettingServiceHorseRacePlaceClient(EndpointConfiguration endpointConfiguration) : 
                base(BettingServiceHorseRacePlaceClient.GetBindingForEndpoint(endpointConfiguration), BettingServiceHorseRacePlaceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BettingServiceHorseRacePlaceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(BettingServiceHorseRacePlaceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BettingServiceHorseRacePlaceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(BettingServiceHorseRacePlaceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BettingServiceHorseRacePlaceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public void GetDataFromBetfairReadOnly()
        {
            base.Channel.GetDataFromBetfairReadOnly();
        }
        
        public System.Threading.Tasks.Task GetDataFromBetfairReadOnlyAsync()
        {
            return base.Channel.GetDataFromBetfairReadOnlyAsync();
        }
        
        public void GetCurrentMarketBookCricket(string Password)
        {
            base.Channel.GetCurrentMarketBookCricket(Password);
        }
        
        public System.Threading.Tasks.Task GetCurrentMarketBookCricketAsync(string Password)
        {
            return base.Channel.GetCurrentMarketBookCricketAsync(Password);
        }
        
        public virtual System.Threading.Tasks.Task OpenAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndOpen));
        }
        
        public virtual System.Threading.Tasks.Task CloseAsync()
        {
            return System.Threading.Tasks.Task.Factory.FromAsync(((System.ServiceModel.ICommunicationObject)(this)).BeginClose(null, null), new System.Action<System.IAsyncResult>(((System.ServiceModel.ICommunicationObject)(this)).EndClose));
        }
        
        private static System.ServiceModel.Channels.Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IBettingServiceHorseRacePlace))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IBettingServiceHorseRacePlace))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:54524/Services/BettingServiceHorseRacePlace.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return BettingServiceHorseRacePlaceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IBettingServiceHorseRacePlace);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return BettingServiceHorseRacePlaceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IBettingServiceHorseRacePlace);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IBettingServiceHorseRacePlace,
        }
    }
}
