using ExternalAPI.TO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService123" in both code and config file together.
    [ServiceContract]
    public interface IService123
    {
        
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        List<ExternalAPI.TO.MarketBook> GetMarketDatabyID(string[] marketID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        ExternalAPI.TO.MarketBookForindianFancy GetMarketDatabyIDIndianFancy(string EventID, string MarketBookID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetRunnersForFancy(string EventID, string MarketBookID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        List<ExternalAPI.TO.LinevMarkets> GetEventIDFancyMarket(string EventID, string MarketBookID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        ExternalAPI.TO.Home GetUpdate(string EventID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        ExternalAPI.TO.UpdateNew GetUpdateNew(string EventID);
        [OperationContract]     
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        ExternalAPI.TO.Root GetUpdate2(string EventID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        List<ExternalAPI.TO.RootSCT> GetUpdateSCT(string Eventtypeid);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAllMarketsFancyString(string marketID);
        [OperationContract]
        string GetAllMarketsString(string marketID);
        [OperationContract]
        string GetAllMarketsBP(string marketID);
        [OperationContract]
        string GetAllMarketsBPFancy(string marketID);


    }
}
