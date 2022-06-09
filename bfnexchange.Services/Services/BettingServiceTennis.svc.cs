using bfnexchange.Services.DBModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TestBF;
using System.ServiceModel.Activation;

namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BettingServiceTennis" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BettingServiceTennis.svc or BettingServiceTennis.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BettingServiceTennis : IBettingServiceTennis
    {
        NExchangeEntities dbEntities = new NExchangeEntities();
        UserServices objUserServices = new UserServices();
        BettingService objBettingClient = new BettingService();
        public static wsnew ws1 = new wsnew();
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
        public void GetDataFromBetfairReadOnly()
        {
            string marketIDs = dbEntities.SP_UserMarket_GetDistinctMarketsOpenedTennis().FirstOrDefault();
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

                        APIConfigforResults.JsonResultsArrTennis = client.DownloadString(url);
                    }

                }
                catch (System.Exception ex)
                {
                    APIConfigforResults.GetSessionFromBeftair();
                    using (var client = new WebClient())
                    {
                        client.Headers.Add(HttpRequestHeader.Cookie, "ssoid=" + APIConfigforResults.SessionKey.ToString());

                        APIConfigforResults.JsonResultsArrTennis = client.DownloadString(url);
                    }
                }



            }
            else
            {

            }
        }

        BettingService objBettingService = new BettingService();
        public void GetCurrentMarketBookCricket(string Password)
        {
            try
            {
                if (APIConfig.isUpdatingBettingAllowed == true)
                {
                    return;
                }
                if (APIConfig.GetCricketDataFrom == "Live" && ConfigurationManager.AppSettings["GetDataFromLiveOtherEventTypes"].ToString() == "1")
                {
                    string marketIDs = dbEntities.SP_UserMarket_GetDistinctMarketsOpenedTennis().FirstOrDefault();
                    if (marketIDs != "")
                    {
                        string[] marketIds = marketIDs.Split(new string[] { ", " }, StringSplitOptions.None);
                        var results = objBettingService.listMarketBookLive(marketIds, Password);
                        APIConfig.AddMarketstoStaticVariableLive(results);
                    }
                    
                }
                else
                {
                    if (APIConfig.GetCricketDataFrom == "BP")
                    {
                       // string marketIDs = dbEntities.SP_UserMarket_GetDistinctMarketsOpenedTennis().FirstOrDefault();
                        string marketIDs = "";
                        var lstofobjects = OpenMarkets.OpenMarketlst.ToList().Where(item => item.EventTypeID == "2").Select(item => item.MarketCatalogueID).ToList();
                        if (lstofobjects.Count > 0)
                        {
                            marketIDs = string.Join(", ", lstofobjects);
                        }
                        if (marketIDs != "")
                        {
                            if (APIConfig.objCurrSession.token != null)
                            {
                                string[] marketIds = marketIDs.Split(new string[] { ", " }, StringSplitOptions.None);
                                var client1 = new RestClient(ConfigurationManager.AppSettings["URLForData"]);


                                foreach (var item in marketIds)
                                {
                                    // APIConfig.WriteErrorToDB(DateTime.Now.ToString() + " " + item.ToString());
                                    var request1 = new RestRequest("/getMarketBook/{marketId}", Method.GET);
                                    request1.RequestFormat = DataFormat.Json;
                                    request1.AddUrlSegment("marketId", item);
                                    request1.AddHeader("X-Authentication", APIConfig.objCurrSession.token);
                                    request1.AddHeader("X-Client", "BetPro");
                                    request1.JsonSerializer.ContentType = "application/json; charset=utf-8";
                                    request1.AddHeader("Accept-Encoding", "gzip");

                                    var response1 = client1.Execute(request1);
                                    // var newresponse = ReturnPureJson(response1.Content);
                                    if (response1.Content.ToString().Contains("SESSION_INVALID"))
                                    {
                                        APIConfig.WriteErrorToDB("go for get password" + " " + item.ToString());
                                        GetUserSessionForData(ConfigurationManager.AppSettings["UserNameforData"], ConfigurationManager.AppSettings["UserPasswordforData"]);
                                        return;
                                    }
                                    try
                                    {


                                        var arr = JsonConvert.DeserializeObject<JValue>(response1.Content);

                                        var objmarket = JsonConvert.DeserializeObject<SampleResponse1[]>(arr.Value.ToString());
                                        if (APIConfig.BFMarketBooks != null)
                                        {
                                            if (APIConfig.BFMarketBooks.Count() > 0)
                                            {
                                                var currbpmarket = APIConfig.BFMarketBooks.Where(item2 => item2.MarketId == objmarket[0].MarketId).FirstOrDefault();
                                                if (currbpmarket != null)
                                                {
                                                    var index = APIConfig.BFMarketBooks.IndexOf(currbpmarket);

                                                    if (index != -1)
                                                        APIConfig.BFMarketBooks[index] = objmarket[0];

                                                    // APIConfig.BFMarketBooks.Remove(currbpmarket);

                                                    //  APIConfig.BFMarketBooks.Add(objmarket[0]);
                                                }
                                                else
                                                {
                                                    APIConfig.BFMarketBooks.Add(objmarket[0]);
                                                }
                                            }
                                            else
                                            {
                                                APIConfig.BFMarketBooks.Add(objmarket[0]);
                                            }
                                        }
                                        //  APIConfig.WriteErrorToDB(DateTime.Now.ToString() + " " + item.ToString());
                                    }
                                    catch (System.Exception ex)
                                    {
                                        APIConfig.LogError(ex);
                                    }

                                }

                                return;
                            }
                            else
                            {
                                GetUserSessionForData(ConfigurationManager.AppSettings["UserNameforData"], ConfigurationManager.AppSettings["UserPasswordforData"]);
                            }
                        }
                    }
                    else
                    {
                        if (APIConfig.URLsData.Count == 0)
                        {
                            objBettingClient.SetURLsData();
                        }
                        ws1.Url = APIConfig.URLsData.Where(item => item.EventType == "Tennis").FirstOrDefault().URLForData;
                       // string marketIDs = dbEntities.SP_UserMarket_GetDistinctMarketsOpenedTennis().FirstOrDefault();
                        string marketIDs = "";
                        var lstofobjects = OpenMarkets.OpenMarketlst.ToList().Where(item => item.EventTypeID == "2").Select(item => item.MarketCatalogueID).ToList();
                        if (lstofobjects.Count > 0)
                        {
                            marketIDs = string.Join(", ", lstofobjects);
                        }
                        if (marketIDs != "")
                        {

                            GetDataFromOtherSource objGetData = new GetDataFromOtherSource();
                            objGetData.GetCurrentMarketBooksOther123(Password, marketIDs, ws1,2,false);



                        }
                    }
                }




            }
            catch (System.Exception ex)
            {
                //  GetUserSessionForData(ConfigurationManager.AppSettings["UserNameforData"], ConfigurationManager.AppSettings["UserPasswordforData"]);
                APIConfig.WriteErrorToDB(ex.Message);
                APIConfig.LogError(ex);

            }
        }
    }
}
