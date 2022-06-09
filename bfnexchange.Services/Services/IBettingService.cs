using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ExternalAPI.TO;
using System.Web.Script.Services;
using System.ServiceModel.Web;

namespace bfnexchange.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IBettingService" in both code and config file together.
    [ServiceContract]
    public interface IBettingService
    {
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IList<MarketCatalogue> listMarketCatalogue(string ID, List<string> lstMarketCatalogues, bool isInPlay, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IList<MarketBook> listMarketBookALL(string[] ID, int UserID, int UserTypeID, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IList<MarketBook> listMarketBook(string ID, int UserID, int UserTypeID, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IList<EventTypeResult> listEventTypes(bool isInPlay, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IList<EventTypeResult> listEventTypesWithMarketFilter(bool isInPlay, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IList<CompetitionResult> listCompetitions(string ID, bool isInPlay, string Password);

        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IList<EventResult> listEvents(string ID, bool isInPlay, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        List<ExternalAPI.TO.MarketBook> GetMarketDatabyIDResultsOnly(string marketID);
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
        void GetDataFromBetfairReadOnly();
        [OperationContract]
        void GetBallbyBallSummary();
        [OperationContract]
        void GetBallbyBallSummaryNew();
        [OperationContract]
        bfnexchange.wrBF.MarketBook[] GetCurrentMarketBookNew(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate, bool BettingAllowed, List<ExternalAPI.TO.Runner> OldRunners, MarketBook currentmarketobject, string Password);
        [OperationContract]
        List<ExternalAPI.TO.MarketBook> GetMarketDatabyIDLive(string[] marketID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory, string Password, string PasswordS);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataBP")]
        string GetAllMarketsBP(string[] marketIDs);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataOthers")]
        List<ExternalAPI.TO.MarketBookString> GetAllMarketsOthers(string[] marketIDs);
        [OperationContract]
        [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataOthersFancy")]
        List<ExternalAPI.TO.MarketBookString> GetAllMarketsOthersFancy(string[] marketIDs);
        [OperationContract]
        [WebInvoke(Method = "POST",  ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "/GetMarektDataBPFancy")]
        string GetAllMarketsBPFancy(string[] marketIDs);
        [OperationContract]
        List<MarketBook> GetAllMarketsFancy(string[] marketIDs);
    }
}
