using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBettingServiceRest" in both code and config file together.
    [ServiceContract]
    public interface IBettingServiceRest
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektData/{sheetname}/{MainSportsCategory}")]
        string GetMarketDatabyIDJsonString(string sheetname, string MainSportsCategory);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetUserbetsForAdmin")]
        string GetUserbetsForAdmin();
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetUserbetsForAgent/?AgentID={AgentID}")]
        string GetUserBetsbyAgentIDwithZeroReferer(int AgentID);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataBP/?marketID={marketID}")]
        string GetAllMarketsBP(string marketID);
        //[OperationContract]
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataBPFancy/?marketID={marketID}")]
        //string GetAllMarketsBPFancy(string marketID);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataBPFancy/?marketID={marketID}")]
        ExternalAPI.TO.Root GetAllMarketsBPFancy(string marketID);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataBPFancyNew/?marketID={marketID}")]
        ExternalAPI.TO.UpdateResult GetAllMarketsBPFancyNew(string marketID);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataOther/?marketID={marketID}")]
        List<ExternalAPI.TO.MarketBookString> GetAllMarketsOthers(string marketID);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataOtherFancy/?marketID={marketID}")]
        List<ExternalAPI.TO.MarketBookString> GetAllMarketsOthersFancy(string marketID);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataOtherFancyIN/?marketID={marketID}")]
        ExternalAPI.TO.GetDataFancy GetAllMarketsOthersFancyIN(string marketID);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataL/?marketID={marketID}")]
        List<ExternalAPI.TO.MarketBook> GetAllMarkets(string marketID);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataFancy/?marketID={marketID}")]
        List<ExternalAPI.TO.MarketBook> GetAllMarketsFancy(string marketID);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataIndiaFancy/?marketID={marketID}")]
        string GetAllMarketsFancyIN(string marketID);      
        [OperationContract]

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetScoresbyEventID/?EventId={EventId}")]
        string GetScoresbyEventIDandDate(string EventId);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataFancyStr/?marketID={marketID}")]
        string GetAllMarketsFancyString(string marketID);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataStr/?marketID={marketID}")]
        string GetAllMarketsString(string marketID);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetUserbetsForAgentNew/?AgentID={AgentID}&Password={Password}")]
        string GetUserBetsbyAgentIDNew(int AgentID, string Password);
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetUserbetsbyUserID/?UserID={UserID}&Password={Password}")]
        string GetUserbetsbyUserID(int UserID, string Password);
    }
}
