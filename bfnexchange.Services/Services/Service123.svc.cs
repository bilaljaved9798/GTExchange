using bfnexchange.Services.DBModel;
using ExternalAPI.TO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service123" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service123.svc or Service123.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior( InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service123 : IService123
    {

        BettingService objBettingService = new BettingService();


        public List<ExternalAPI.TO.MarketBook> GetMarketDatabyID(string[] marketID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory, string Password)
        {
            return objBettingService.GetMarketDatabyID(marketID, sheetname, OrignalOpenDate, MainSportsCategory, Password);
        }

        public ExternalAPI.TO.MarketBookForindianFancy GetMarketDatabyIDIndianFancy(string EventID, string MarketBookID)
        {
            return objBettingService.GetMarketDatabyIDIndianFancy(EventID, MarketBookID);
        }
        public string GetRunnersForFancy(string EventID, string MarketBookID)
        {
            return objBettingService.GetRunnersForFancy(EventID, MarketBookID);
        }
        public List<ExternalAPI.TO.LinevMarkets> GetEventIDFancyMarket(string EventID, string MarketBookID)
        {
            return objBettingService.GetEventIDFancyMarket(EventID, MarketBookID);
        }
        
        public ExternalAPI.TO.Home GetUpdate(string EventID)
        {
            return objBettingService.GetUpdate(EventID);
        }
        public ExternalAPI.TO.UpdateNew GetUpdateNew(string EventID)
        {
            return objBettingService.GetUpdateNew(EventID);
        }
        
        public string GetIndianFancy(string EventID, string MarketBookID)
        {
            return objBettingService.GetRunnersForFancy(EventID, MarketBookID);
        }
        
        public ExternalAPI.TO.Root GetUpdate2(string EventID)
        {
            return objBettingService.GetUpdate2(EventID);
        }

        public List<ExternalAPI.TO.RootSCT> GetUpdateSCT(string Eventtypeid)
        {
            return objBettingService.GetUpdateSCT(Eventtypeid);
        }
        

        public string GetAllMarketsString(string marketID)
        {
            return objBettingService.GetAllMarketsString(marketID);
        }
        public string GetAllMarketsFancyString(string marketID)
        {
            return objBettingService.GetAllMarketsFancyString(marketID);
        }
        public string GetAllMarketsBP(string marketID)
        {
            string[] marketIDs = marketID.Split(',');
            return objBettingService.GetAllMarketsBP(marketIDs);
        }
        public string GetAllMarketsBPFancy(string marketID)
        {
            string[] marketIDs = marketID.Split(',');
            return objBettingService.GetAllMarketsBPFancy(marketIDs);
        }
        public List<ExternalAPI.TO.MarketBookString> GetAllMarketsOthers(string[] marketIDs)
        {
            return objBettingService.GetAllMarketsOthers(marketIDs);
        }
        public List<ExternalAPI.TO.MarketBookString> GetAllMarketsOthersFancy(string[] marketIDs)
        {
            return objBettingService.GetAllMarketsOthersFancy(marketIDs);
        }
    }
}
