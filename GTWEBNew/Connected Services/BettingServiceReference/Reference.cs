﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BettingServiceReference
{
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BettingServiceReference.IBettingService")]
    public interface IBettingService
    {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/listMarketCatalogue", ReplyAction="http://tempuri.org/IBettingService/listMarketCatalogueResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.MarketCatalogue[]> listMarketCatalogueAsync(string ID, string[] lstMarketCatalogues, bool isInPlay, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/listMarketBookALL", ReplyAction="http://tempuri.org/IBettingService/listMarketBookALLResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.MarketBook[]> listMarketBookALLAsync(string[] ID, int UserID, int UserTypeID, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/listMarketBook", ReplyAction="http://tempuri.org/IBettingService/listMarketBookResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.MarketBook[]> listMarketBookAsync(string ID, int UserID, int UserTypeID, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/listEventTypes", ReplyAction="http://tempuri.org/IBettingService/listEventTypesResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.EventTypeResult[]> listEventTypesAsync(bool isInPlay, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/listEventTypesWithMarketFilter", ReplyAction="http://tempuri.org/IBettingService/listEventTypesWithMarketFilterResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.EventTypeResult[]> listEventTypesWithMarketFilterAsync(bool isInPlay, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/listCompetitions", ReplyAction="http://tempuri.org/IBettingService/listCompetitionsResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.CompetitionResult[]> listCompetitionsAsync(string ID, bool isInPlay, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/listEvents", ReplyAction="http://tempuri.org/IBettingService/listEventsResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.EventResult[]> listEventsAsync(string ID, bool isInPlay, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetMarketDatabyIDResultsOnly", ReplyAction="http://tempuri.org/IBettingService/GetMarketDatabyIDResultsOnlyResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.MarketBook[]> GetMarketDatabyIDResultsOnlyAsync(string marketID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetMarketDatabyID", ReplyAction="http://tempuri.org/IBettingService/GetMarketDatabyIDResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.MarketBook[]> GetMarketDatabyIDAsync(string[] marketID, string sheetname, System.DateTime OrignalOpenDate, string MainSportsCategory, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetMarketDatabyIDIndianFancy", ReplyAction="http://tempuri.org/IBettingService/GetMarketDatabyIDIndianFancyResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.MarketBookForindianFancy> GetMarketDatabyIDIndianFancyAsync(string EventID, string MarketBookID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetRunnersForFancy", ReplyAction="http://tempuri.org/IBettingService/GetRunnersForFancyResponse")]
        System.Threading.Tasks.Task<string> GetRunnersForFancyAsync(string EventID, string MarketBookID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetUpdate", ReplyAction="http://tempuri.org/IBettingService/GetUpdateResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.Home> GetUpdateAsync(string EventID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetUpdateNew", ReplyAction="http://tempuri.org/IBettingService/GetUpdateNewResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.UpdateNew> GetUpdateNewAsync(string EventID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetUpdate2", ReplyAction="http://tempuri.org/IBettingService/GetUpdate2Response")]
        System.Threading.Tasks.Task<ExternalAPI.TO.Root> GetUpdate2Async(string EventID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetUpdateSCT", ReplyAction="http://tempuri.org/IBettingService/GetUpdateSCTResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.RootSCT[]> GetUpdateSCTAsync(string Eventtypeid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetSoccorUpdate", ReplyAction="http://tempuri.org/IBettingService/GetSoccorUpdateResponse")]
        System.Threading.Tasks.Task<string> GetSoccorUpdateAsync(string Eventtypeid);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetDataFromBetfairReadOnly", ReplyAction="http://tempuri.org/IBettingService/GetDataFromBetfairReadOnlyResponse")]
        System.Threading.Tasks.Task GetDataFromBetfairReadOnlyAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetBallbyBallSummary", ReplyAction="http://tempuri.org/IBettingService/GetBallbyBallSummaryResponse")]
        System.Threading.Tasks.Task GetBallbyBallSummaryAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetBallbyBallSummaryNew", ReplyAction="http://tempuri.org/IBettingService/GetBallbyBallSummaryNewResponse")]
        System.Threading.Tasks.Task GetBallbyBallSummaryNewAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetCurrentMarketBookNew", ReplyAction="http://tempuri.org/IBettingService/GetCurrentMarketBookNewResponse")]
        System.Threading.Tasks.Task<bfnexchange.wrBF.MarketBook[]> GetCurrentMarketBookNewAsync(string marketid, string sheetname, string MainSportsCategory, System.DateTime marketopendate, bool BettingAllowed, ExternalAPI.TO.Runner[] OldRunners, ExternalAPI.TO.MarketBook currentmarketobject, string Password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetMarketDatabyIDLive", ReplyAction="http://tempuri.org/IBettingService/GetMarketDatabyIDLiveResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.MarketBook[]> GetMarketDatabyIDLiveAsync(string[] marketID, string sheetname, System.DateTime OrignalOpenDate, string MainSportsCategory, string Password, string PasswordS);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetAllMarketsBP", ReplyAction="http://tempuri.org/IBettingService/GetAllMarketsBPResponse")]
        System.Threading.Tasks.Task<string> GetAllMarketsBPAsync(string[] marketIDs);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetAllMarketsOthers", ReplyAction="http://tempuri.org/IBettingService/GetAllMarketsOthersResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.MarketBookString[]> GetAllMarketsOthersAsync(string[] marketIDs);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetAllMarketsOthersFancy", ReplyAction="http://tempuri.org/IBettingService/GetAllMarketsOthersFancyResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.MarketBookString[]> GetAllMarketsOthersFancyAsync(string[] marketIDs);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetAllMarketsBPFancy", ReplyAction="http://tempuri.org/IBettingService/GetAllMarketsBPFancyResponse")]
        System.Threading.Tasks.Task<string> GetAllMarketsBPFancyAsync(string[] marketIDs);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBettingService/GetAllMarketsFancy", ReplyAction="http://tempuri.org/IBettingService/GetAllMarketsFancyResponse")]
        System.Threading.Tasks.Task<ExternalAPI.TO.MarketBook[]> GetAllMarketsFancyAsync(string[] marketIDs);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public interface IBettingServiceChannel : BettingServiceReference.IBettingService, System.ServiceModel.IClientChannel
    {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Tools.ServiceModel.Svcutil", "2.0.3")]
    public partial class BettingServiceClient : System.ServiceModel.ClientBase<BettingServiceReference.IBettingService>, BettingServiceReference.IBettingService
    {
        
        /// <summary>
        /// Implement this partial method to configure the service endpoint.
        /// </summary>
        /// <param name="serviceEndpoint">The endpoint to configure</param>
        /// <param name="clientCredentials">The client credentials</param>
        static partial void ConfigureEndpoint(System.ServiceModel.Description.ServiceEndpoint serviceEndpoint, System.ServiceModel.Description.ClientCredentials clientCredentials);
        
        public BettingServiceClient() : 
                base(BettingServiceClient.GetDefaultBinding(), BettingServiceClient.GetDefaultEndpointAddress())
        {
            this.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IUserServices.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BettingServiceClient(EndpointConfiguration endpointConfiguration) : 
                base(BettingServiceClient.GetBindingForEndpoint(endpointConfiguration), BettingServiceClient.GetEndpointAddress(endpointConfiguration))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BettingServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress) : 
                base(BettingServiceClient.GetBindingForEndpoint(endpointConfiguration), new System.ServiceModel.EndpointAddress(remoteAddress))
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BettingServiceClient(EndpointConfiguration endpointConfiguration, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(BettingServiceClient.GetBindingForEndpoint(endpointConfiguration), remoteAddress)
        {
            this.Endpoint.Name = endpointConfiguration.ToString();
            ConfigureEndpoint(this.Endpoint, this.ClientCredentials);
        }
        
        public BettingServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress)
        {
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.MarketCatalogue[]> listMarketCatalogueAsync(string ID, string[] lstMarketCatalogues, bool isInPlay, string Password)
        {
            return base.Channel.listMarketCatalogueAsync(ID, lstMarketCatalogues, isInPlay, Password);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.MarketBook[]> listMarketBookALLAsync(string[] ID, int UserID, int UserTypeID, string Password)
        {
            return base.Channel.listMarketBookALLAsync(ID, UserID, UserTypeID, Password);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.MarketBook[]> listMarketBookAsync(string ID, int UserID, int UserTypeID, string Password)
        {
            return base.Channel.listMarketBookAsync(ID, UserID, UserTypeID, Password);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.EventTypeResult[]> listEventTypesAsync(bool isInPlay, string Password)
        {
            return base.Channel.listEventTypesAsync(isInPlay, Password);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.EventTypeResult[]> listEventTypesWithMarketFilterAsync(bool isInPlay, string Password)
        {
            return base.Channel.listEventTypesWithMarketFilterAsync(isInPlay, Password);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.CompetitionResult[]> listCompetitionsAsync(string ID, bool isInPlay, string Password)
        {
            return base.Channel.listCompetitionsAsync(ID, isInPlay, Password);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.EventResult[]> listEventsAsync(string ID, bool isInPlay, string Password)
        {
            return base.Channel.listEventsAsync(ID, isInPlay, Password);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.MarketBook[]> GetMarketDatabyIDResultsOnlyAsync(string marketID)
        {
            return base.Channel.GetMarketDatabyIDResultsOnlyAsync(marketID);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.MarketBook[]> GetMarketDatabyIDAsync(string[] marketID, string sheetname, System.DateTime OrignalOpenDate, string MainSportsCategory, string Password)
        {
            return base.Channel.GetMarketDatabyIDAsync(marketID, sheetname, OrignalOpenDate, MainSportsCategory, Password);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.MarketBookForindianFancy> GetMarketDatabyIDIndianFancyAsync(string EventID, string MarketBookID)
        {
            return base.Channel.GetMarketDatabyIDIndianFancyAsync(EventID, MarketBookID);
        }
        
        public System.Threading.Tasks.Task<string> GetRunnersForFancyAsync(string EventID, string MarketBookID)
        {
            return base.Channel.GetRunnersForFancyAsync(EventID, MarketBookID);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.Home> GetUpdateAsync(string EventID)
        {
            return base.Channel.GetUpdateAsync(EventID);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.UpdateNew> GetUpdateNewAsync(string EventID)
        {
            return base.Channel.GetUpdateNewAsync(EventID);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.Root> GetUpdate2Async(string EventID)
        {
            return base.Channel.GetUpdate2Async(EventID);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.RootSCT[]> GetUpdateSCTAsync(string Eventtypeid)
        {
            return base.Channel.GetUpdateSCTAsync(Eventtypeid);
        }
        
        public System.Threading.Tasks.Task<string> GetSoccorUpdateAsync(string Eventtypeid)
        {
            return base.Channel.GetSoccorUpdateAsync(Eventtypeid);
        }
        
        public System.Threading.Tasks.Task GetDataFromBetfairReadOnlyAsync()
        {
            return base.Channel.GetDataFromBetfairReadOnlyAsync();
        }
        
        public System.Threading.Tasks.Task GetBallbyBallSummaryAsync()
        {
            return base.Channel.GetBallbyBallSummaryAsync();
        }
        
        public System.Threading.Tasks.Task GetBallbyBallSummaryNewAsync()
        {
            return base.Channel.GetBallbyBallSummaryNewAsync();
        }
        
        public System.Threading.Tasks.Task<bfnexchange.wrBF.MarketBook[]> GetCurrentMarketBookNewAsync(string marketid, string sheetname, string MainSportsCategory, System.DateTime marketopendate, bool BettingAllowed, ExternalAPI.TO.Runner[] OldRunners, ExternalAPI.TO.MarketBook currentmarketobject, string Password)
        {
            return base.Channel.GetCurrentMarketBookNewAsync(marketid, sheetname, MainSportsCategory, marketopendate, BettingAllowed, OldRunners, currentmarketobject, Password);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.MarketBook[]> GetMarketDatabyIDLiveAsync(string[] marketID, string sheetname, System.DateTime OrignalOpenDate, string MainSportsCategory, string Password, string PasswordS)
        {
            return base.Channel.GetMarketDatabyIDLiveAsync(marketID, sheetname, OrignalOpenDate, MainSportsCategory, Password, PasswordS);
        }
        
        public System.Threading.Tasks.Task<string> GetAllMarketsBPAsync(string[] marketIDs)
        {
            return base.Channel.GetAllMarketsBPAsync(marketIDs);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.MarketBookString[]> GetAllMarketsOthersAsync(string[] marketIDs)
        {
            return base.Channel.GetAllMarketsOthersAsync(marketIDs);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.MarketBookString[]> GetAllMarketsOthersFancyAsync(string[] marketIDs)
        {
            return base.Channel.GetAllMarketsOthersFancyAsync(marketIDs);
        }
        
        public System.Threading.Tasks.Task<string> GetAllMarketsBPFancyAsync(string[] marketIDs)
        {
            return base.Channel.GetAllMarketsBPFancyAsync(marketIDs);
        }
        
        public System.Threading.Tasks.Task<ExternalAPI.TO.MarketBook[]> GetAllMarketsFancyAsync(string[] marketIDs)
        {
            return base.Channel.GetAllMarketsFancyAsync(marketIDs);
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IUserServices))
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
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IUserServices))
            {
                return new System.ServiceModel.EndpointAddress("http://localhost:54524/Services/BettingService.svc");
            }
            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }
        
        private static System.ServiceModel.Channels.Binding GetDefaultBinding()
        {
            return BettingServiceClient.GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IUserServices);
        }
        
        private static System.ServiceModel.EndpointAddress GetDefaultEndpointAddress()
        {
            return BettingServiceClient.GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IUserServices);
        }
        
        public enum EndpointConfiguration
        {
            
            BasicHttpBinding_IUserServices,
        }
    }
}
