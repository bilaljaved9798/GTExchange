using bfnexchange.Services.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TestBF;
using System.ServiceModel.Activation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using RestSharp;

namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BettingServiceCricketInningsRuns" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BettingServiceCricketInningsRuns.svc or BettingServiceCricketInningsRuns.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BettingServiceCricketInningsRuns : IBettingServiceCricketInningsRuns
    {
        NExchangeEntities dbEntities = new NExchangeEntities();
        UserServices objUserServices = new UserServices();
        BettingService objBettingClient = new BettingService();
        public static wsnew ws1 = new wsnew();

        public void GetDataFromBetfairReadOnly()
        {
            string marketIDs = dbEntities.SP_UserMarket_GetDistinctMarketsOpenedCricketInningsRuns().FirstOrDefault();
            var LinevMarkets =objUserServices.GetLinevMarketsbyEventID2();
            APIConfig.LinevMarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUserServices.GetLinevMarketsbyEventID2());

            if (marketIDs != "")
            {
                if (APIConfigforResults.Url == "")
                {
                    APIConfigforResults.InitiateAPIConfiguration();

                }

                string url = "http://www.betfair.com/www/sports/exchange/readonly/v1/bymarket?alt=json&currencyCode=GBP&locale=en_GB&marketIds=" + marketIDs + "&rollupLimit=4&rollupModel=STAKE&types=MARKET_STATE,RUNNER_STATE,RUNNER_EXCHANGE_PRICES_BEST,RUNNER_DESCRIPTION";


                //string JsonResultsArr = "";
                try
                {
                    if (APIConfigforResults.SessionKey == "")
                    {
                        APIConfigforResults.GetSessionFromBeftair();
                    }
                    using (var client = new WebClient())
                    {
                        client.Headers.Add(HttpRequestHeader.Cookie, "ssoid=" + APIConfigforResults.SessionKey.ToString());

                        APIConfigforResults.JsonResultsArrCricketInningsRuns = client.DownloadString(url);
                    }

                }
                catch (System.Exception ex)
                {
                    APIConfigforResults.GetSessionFromBeftair();
                    using (var client = new WebClient())
                    {
                        client.Headers.Add(HttpRequestHeader.Cookie, "ssoid=" + APIConfigforResults.SessionKey.ToString());

                        APIConfigforResults.JsonResultsArrCricketInningsRuns = client.DownloadString(url);
                    }
                }



            }
            else
            {

            }
        }
        public void GetCurrentMarketBookCricket(string Password)
        {
            try
            {
                APIConfig.LinevMarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUserServices.GetLinevMarketsbyEventID2());

                if (APIConfig.isUpdatingBettingAllowed == true)
                {
                    return;
                }                           
                        if (APIConfig.URLsData.Count == 0)
                        {
                            objBettingClient.SetURLsData();
                        }
                        ws1.Url = APIConfig.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().URLForData;
                        //string marketIDs = dbEntities.SP_UserMarket_GetDistinctMarketsOpenedCricketInningsRuns().FirstOrDefault();
                        string marketIDs = "";
                string events = "";
                // var lstofobjects = OpenMarkets.OpenMarketlst.ToList().Where(item => item.EventTypeID == "4" && (item.EventName.Contains("Line v Markets"))).Select(item => item.MarketCatalogueID).ToList();
                var lstofobjects = OpenMarketsFancy.OpenMarketlstfancy.ToList().Where(item => item.EventTypeID == "4" && (item.MarketCatalogueName == "Match Odds")).ToList();
                //var lstofobjectsevent = OpenMarketsFancy.OpenMarketlstfancy.ToList().Where(item => item.EventTypeID == "4" && (item.MarketCatalogueName == "Match Odds")).Select(item => item.EventID).ToList();

                if (lstofobjects.Count > 0)
                        {
                            marketIDs = string.Join(", ", lstofobjects);
                           // events = string.Join(", ", lstofobjectsevent);
                        }
                        if (marketIDs != "")
                        {
                            GetDataFromOtherSource objGetData = new GetDataFromOtherSource();
                            objGetData.GetCurrentMarketFancy(lstofobjects);
                        }                   
            }
            catch (System.Exception ex)
            {
                //  GetUserSessionForData(ConfigurationManager.AppSettings["UserNameforData"], ConfigurationManager.AppSettings["UserPasswordforData"]);
                APIConfig.WriteErrorToDB(ex.Message);
                APIConfig.LogError(ex);

            }
        }
        public void GetUserSessionForData(string Username, string Password)
        {
            try
            {
                UserLoginObject objuser = new UserLoginObject();
                objuser.Username = Username;
                objuser.Password = Password;
                objuser.SoftwareId = "BetPro";
                objuser.DeviceId = "AEBD9416 IQBALPRACHA@DESKTOP-SN5MV98";
                var client = new RestClient(ConfigurationManager.AppSettings["URLForData"]);
                var request = new RestRequest("/UserLogin", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(objuser);

                var response = client.Execute(request);

                var obj = JObject.Parse(response.Content);
                BetterInfo objBetter = JsonConvert.DeserializeObject<BetterInfo>(obj.ToString());


                APIConfig.objCurrSession.id = objBetter.BetterId;
                APIConfig.objCurrSession.token = objBetter.PrivateKey;
                APIConfig.objCurrSession.Username = objBetter.BetterName;
                APIConfig.objCurrSession.lastReq = Convert.ToDateTime(DateTime.Now.Date.ToString());
                APIConfig.objCurrSession.CurrentReq = Convert.ToDateTime(DateTime.Now.Date.ToString());
                APIConfig.objCurrSession.isAllowedBetting = false;
                APIConfig.objCurrSession.active = false;
                APIConfig.objCurrSession.anouncement = "";
                ///
                //APIConfig.objCurrSessionNew.id = objResult.BetterId;
                //APIConfig.objCurrSessionNew.token = objResult.PrivateKey;
                //APIConfig.objCurrSessionNew.username = objResult.BetterName;
                //APIConfig.objCurrSessionNew.lastReq = Convert.ToDateTime(DateTime.Now.Date.ToString());
                //APIConfig.objCurrSessionNew.CurrentReq = Convert.ToDateTime(DateTime.Now.Date.ToString());
                //APIConfig.objCurrSessionNew.isAllowedBetting = false;
                //APIConfig.objCurrSessionNew.active = false;
                //APIConfig.objCurrSessionNew.anouncement = "";
            }
            catch (System.Exception ex)
            {
                APIConfig.WriteErrorToDB(ex.Message);
            }
        }
    }
}
