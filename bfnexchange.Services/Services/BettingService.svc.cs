using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ExternalAPI.TO;
using ExternalAPI;
using bfnexchange.Services.Services;

using System.Configuration;
using Newtonsoft.Json;
using bfnexchange;
using bfnexchange.Services.DBModel;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

using TestBF;
using System.Web.UI;
using System.ServiceModel.Activation;

namespace bfnexchange.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BettingService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BettingService.svc or BettingService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class BettingService : IBettingService
    {
        UserServices u = new UserServices();
        public BettingService()
        {
            if (APIConfig.UrlLive == "")
            {
                APIConfig.InitiateAPIConfigurationLive();
            }
        }
        IClient client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
        IClient clientLive = new JsonRpcClient(APIConfig.UrlLive, APIConfig.AppKeyLive, APIConfig.SessionKeyLive);
        NExchangeEntities dbEntities = new NExchangeEntities();
        UserServices objUserServices = new UserServices();

        public void GetDataFromBetfairReadOnly()
        {
            string marketIDs = dbEntities.SP_UserMarket_GetDistinctMarketsOpened().FirstOrDefault();
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

                        APIConfigforResults.JsonResultsArr = client.DownloadString(url);
                    }

                }
                catch (System.Exception ex)
                {
                    APIConfigforResults.GetSessionFromBeftair();
                    using (var client = new WebClient())
                    {
                        client.Headers.Add(HttpRequestHeader.Cookie, "ssoid=" + APIConfigforResults.SessionKey.ToString());

                        APIConfigforResults.JsonResultsArr = client.DownloadString(url);
                    }
                }
                //if (JsonResultsArr != "")
                //{

                //    dbEntities.SP_OddsData_Update(JsonResultsArr);
                //}
                // if (APIConfigforResults.Url == "")
                // {
                //     APIConfigforResults.InitiateAPIConfiguration();

                // }

                // string url = "http://www.betfair.com/www/sports/exchange/readonly/v1/bymarket?alt=json&currencyCode=GBP&locale=en_GB&marketIds=" + marketIDs + "&rollupLimit=4&rollupModel=STAKE&types=MARKET_STATE,RUNNER_STATE,RUNNER_EXCHANGE_PRICES_BEST,RUNNER_DESCRIPTION,RUNNER_METADATA";


                //// string JsonResultsArr = "";
                // try
                // {
                //     using (var client = new WebClient())
                //     {
                //         if (APIConfigforResults.SessionKey == "")
                //         {
                //             APIConfigforResults.GetSessionFromBeftair();
                //         }
                //         //client.Proxy = null;
                //         client.Headers.Add(HttpRequestHeader.Cookie, "ssoid=" + APIConfigforResults.SessionKey.ToString());

                //         APIConfigforResults.JsonResultsArr = client.DownloadString(url);
                //     }
                // }

                // catch (System.Exception ex)
                // {
                //     APIConfigforResults.GetSessionFromBeftair();
                //     using (var client = new WebClient())
                //     {
                //         client.Headers.Add(HttpRequestHeader.Cookie, "ssoid=" + APIConfigforResults.SessionKey.ToString());

                //         APIConfigforResults.JsonResultsArr = client.DownloadString(url);
                //     }
                // }


            }
            else
            {
                //APIConfigforResults.JsonResultsArr = "";
            }
        }

        


        public static wsnew ws1 = new wsnew();

        public static wsnew ws2 = new wsnew();

        public static wsnew ws4 = new wsnew();

        public static wsnew ws7 = new wsnew();

        public static wsnew ws0 = new wsnew();

        public static wsnew ws4339 = new wsnew();

        public List<ExternalAPI.TO.MarketBook> GetMarketDatabyIDLive(string[] marketID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory, string Password,string PasswordS)
        {
            var marketbooks = new List<MarketBook>();
            if (ValidatePasswordS(PasswordS) == false)
            {
                return marketbooks;
            }
            try
            {
                if (1 == 1)
                {
                    var marketbookfromlive = new List<ExternalAPI.TO.MarketBook>();
                    if (APIConfig.GetCricketDataFrom == "Live")
                    {
                        marketbookfromlive = APIConfig.LiveCricketMarketBooks.Where(item => item.MarketId == marketID[0]).ToList();
                        if (marketbookfromlive.Count == 0)
                        {
                            marketbookfromlive = APIConfig.LiveCricketMarketBooksFancy.Where(item => item.MarketId == marketID[0]).ToList();
                        }
                    }
                    else
                    {
                        marketbookfromlive = listMarketBookLive(marketID, Password).ToList();
                    }

                    if (marketbookfromlive != null)
                    {
                        if (marketbookfromlive.Count > 0)
                        {
                            if (marketbookfromlive[0].MarketId != null)
                            {
                                if (marketbookfromlive[0].IsMarketDataDelayed == false)
                                {
                                    var marketbook = ConvertJsontoMarketObjectBFLive(marketbookfromlive[0], marketID[0], OrignalOpenDate, sheetname, MainSportsCategory);
                                    marketbooks.Add(marketbook);
                                }
                                else
                                {
                                    var marketbook = GetMarketDatabyID(marketID, sheetname, OrignalOpenDate, MainSportsCategory, Password);
                                    // var marketbook = GetCurrentMarketBook(marketID[0], sheetname, MainSportsCategory, OrignalOpenDate);

                                    marketbooks.Add(marketbook[0]);
                                }
                            }

                        }
                    }
                }
                else
                {

                    var marketbook = GetCurrentMarketBook(marketID[0], sheetname, MainSportsCategory, OrignalOpenDate);

                    marketbooks.Add(marketbook);
                }


                return marketbooks;
            }
            catch (System.Exception ex)
            {
                return marketbooks;
            }
        }
        public string GetAllMarketsBP(string[] marketIDs)
        {
            List<SampleResponse1> lstClientMarkes = new List<SampleResponse1>();
            if (APIConfig.GetCricketDataFrom == "BP")

            {
                if (APIConfig.BFMarketBooks != null)
                {
                    // var admins = APIConfig.BFMarketBooks.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
                    foreach (var marketitem in marketIDs)
                    {
                        var currmarketbook = APIConfig.BFMarketBooks.Where(item => item.MarketId == marketitem).FirstOrDefault();
                        if (currmarketbook != null)
                        {



                            lstClientMarkes.Add(currmarketbook);
                        }

                    }

                }
            }
            return ConverttoJSONString(lstClientMarkes);
        }
        public string GetAllMarketsBPFancy(string[] marketIDs)
        {
            List<SampleResponse1> lstClientMarkes = new List<SampleResponse1>();
            if (APIConfig.GetCricketDataFrom == "BP")

            {
                if (APIConfigforResults.BFMarketBooksFancy != null)
                {
                    // var admins = APIConfig.BFMarketBooks.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
                    foreach (var marketitem in marketIDs)
                    {
                        var currmarketbook = APIConfigforResults.BFMarketBooksFancy.Where(item => item.MarketId == marketitem).FirstOrDefault();
                        if (currmarketbook != null)
                        {
                            lstClientMarkes.Add(currmarketbook);
                        }
                    }

                }
            }
            return ConverttoJSONString(lstClientMarkes);
        }
        public List<ExternalAPI.TO.MarketBookString> GetAllMarketsOthers(string[] marketIDs)
        {
            List<ExternalAPI.TO.MarketBookString> lstClientMarkes = new List<ExternalAPI.TO.MarketBookString>();
            if (APIConfig.GetCricketDataFrom == "Other")

            {
                if (APIConfig.BFMarketBooksOther123 != null)
                {
                    //  var admins = APIConfig.BFMarketBooksOther.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));

                    foreach (var marketitem in marketIDs)
                    {
                        var currmarketbook = APIConfig.BFMarketBooksOther123.Where(item => item.MarketBookId == marketitem).FirstOrDefault();
                        if (currmarketbook != null)
                        {



                            lstClientMarkes.Add(currmarketbook);
                        }
                        else
                        {

                        }
                    }

                }
            }
            return (lstClientMarkes);
        }
        public List<ExternalAPI.TO.MarketBookString> GetAllMarketsOthersFancy(string[] marketIDs)
        {
            List<ExternalAPI.TO.MarketBookString> lstClientMarkes = new List<ExternalAPI.TO.MarketBookString>();
            if (APIConfig.GetCricketDataFrom == "Other")

            {
                if (APIConfig.BFMarketBooksOther123 != null)
                {
                    //  var admins = APIConfig.BFMarketBooksOther.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));

                    foreach (var marketitem in marketIDs)
                    {
                        var currmarketbook = APIConfig.BFMarketBooksOther123.Where(item => item.MarketBookId == marketitem).FirstOrDefault();
                        if (currmarketbook != null)
                        {
                            lstClientMarkes.Add(currmarketbook);
                        }
                        else
                        {

                        }
                    }
                }
            }
            return (lstClientMarkes);
        }
        public List<MarketBook> GetAllMarketsFancy(string[] marketIDs)
        {
            List<MarketBook> lstClientMarkes = new List<MarketBook>();
            if (APIConfig.GetCricketDataFrom == "Live")
            {
                if (APIConfig.LiveCricketMarketBooksFancy != null)
                {
                    //  var admins = APIConfig.BFMarketBooksOther.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));

                    foreach (var marketitem in marketIDs)
                    {
                        var currmarketbook = APIConfig.LiveCricketMarketBooksFancy.Where(item => item.MarketId == marketitem).FirstOrDefault();
                        if (currmarketbook != null)
                        {
                            lstClientMarkes.Add(currmarketbook);
                        }
                        else
                        {

                        }
                    }
                }
            }
            return (lstClientMarkes);
        }
        public List<MarketBook> GetAllMarkets(string[] marketIDs)
        {
            List<MarketBook> lstClientMarkes = new List<MarketBook>();
            if (APIConfig.GetCricketDataFrom == "Live")

            {
                if (APIConfig.LiveCricketMarketBooks != null)
                {
                    //  var admins = APIConfig.BFMarketBooksOther.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));

                    foreach (var marketitem in marketIDs)
                    {
                        var currmarketbook = APIConfig.LiveCricketMarketBooks.Where(item => item.MarketId == marketitem).FirstOrDefault();
                        if (currmarketbook != null)
                        {

                            lstClientMarkes.Add(currmarketbook);
                        }
                        else
                        {

                        }
                    }

                }
            }
            return (lstClientMarkes);
        }

        public string GetSoccorUpdate(string Eventtypeid)
        {

            List<ExternalAPI.TO.RootSCT> myDeserializedClass;
            
            string urltennis = "https://ips.betfair.com/inplayservice/v1.0/eventsInPlay?locale=en&regionCode=UK&alt=json&eventTypeIds="+ Eventtypeid + "";
            string url = "https://ips.betfair.com/inplayservice/v1/eventTimeline?_ak=nzIFcwyWhrlwYMrh&alt=json&eventId="+Eventtypeid+"&locale=en&productType=EXCHANGE&regionCode=uk";
            //string url = "https://ips.betfair.com/inplayservice/v1/scores?_ak=nzIFcwyWhrlwYMrh&alt=json&eventIds=" + Eventid + "&locale=en_GB&productType=EXCHANGE&regionCode=UK";
            string JsonResultsArr = "";
            try
            {
                if (Eventtypeid == "2")
                {
                    using (var client = new WebClient())
                    {
                        APIConfigforResults.InitiateAPIConfiguration();
                        APIConfigforResults.GetSessionFromBeftair();
                        JsonResultsArr = client.DownloadString(urltennis);
                        // myDeserializedClass = JsonConvert.DeserializeObject<List<ExternalAPI.TO.RootSCT>>(JsonResultsArr);
                    }
                }
                else
                {
                    using (var client = new WebClient())
                    {
                        APIConfigforResults.InitiateAPIConfiguration();
                        APIConfigforResults.GetSessionFromBeftair();
                        JsonResultsArr = client.DownloadString(url);
                        // myDeserializedClass = JsonConvert.DeserializeObject<List<ExternalAPI.TO.RootSCT>>(JsonResultsArr);
                    }
                }

                return JsonResultsArr;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);       
                return JsonResultsArr;
            }
        }
        bool chk = true;
        public List<ExternalAPI.TO.RootSCT> GetUpdateSCT(string Eventtypeid)
        {

            List<ExternalAPI.TO.RootSCT> myDeserializedClass;

            string url = " https://ips.betfair.com/inplayservice/v1.0/eventsInPlay?locale=en&regionCode=UK&alt=json&maxResults=500&eventTypeIds="+ Eventtypeid + "";
            string url2 = " https://ips.betfair.com/inplayservice/v1.0/eventsInPlay?locale=en&regionCode=UK&alt=json&maxResults=500&eventTypeIds=" + 2 + "";
            //string url = "https://ips.betfair.com/inplayservice/v1/scores?_ak=nzIFcwyWhrlwYMrh&alt=json&eventIds=" + Eventid + "&locale=en_GB&productType=EXCHANGE&regionCode=UK";

            string JsonResultsArr = "";
            string JsonResultsArr2 = "";
            try
            {
                using (var client = new WebClient())
                {
                    APIConfigforResults.InitiateAPIConfiguration();
                    APIConfigforResults.GetSessionFromBeftair();
                    JsonResultsArr2 = client.DownloadString(url2);
                    myDeserializedClass = JsonConvert.DeserializeObject<List<ExternalAPI.TO.RootSCT>>(JsonResultsArr2);
                }
                try
                {
                    using (var client = new WebClient())
                    {
                        APIConfigforResults.InitiateAPIConfiguration();
                        APIConfigforResults.GetSessionFromBeftair();
                        JsonResultsArr = client.DownloadString(url);
                        myDeserializedClass.AddRange(JsonConvert.DeserializeObject<List<ExternalAPI.TO.RootSCT>>(JsonResultsArr));
                    }
                }
                catch (System.Exception ex)
                {
                }
                    return myDeserializedClass;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                List<ExternalAPI.TO.RootSCT> myDeserializedClassa = new List<RootSCT>();
                return myDeserializedClassa;
            }

        }
        public void GetBallbyBallSummary()
        {
            try
            {
                List<SP_UserMarket_GetDistinctLinevMarkets_Result> lstEvents = dbEntities.SP_UserMarket_GetDistinctLinevMarkets().ToList<SP_UserMarket_GetDistinctLinevMarkets_Result>();
                if (lstEvents.Count > 0)
                {
                    foreach (var item in lstEvents)
                    {
                        //item.EventOpenDate >= DateTime.Now.AddHours(-5) ||
                        if (item.MarketStatus != "Closed")
                        {                                               
                            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("http://139.59.82.99:3000/api/match/getMatchScore?eventId=" + item.EventID + ""));
                            WebReq.Method = "GET";
                            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                            Console.WriteLine(WebResp.StatusCode);
                            Console.WriteLine(WebResp.Server);
                            string jsonString;
                            using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
                            {
                                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                                jsonString = reader.ReadToEnd();
                            }
                            try
                            {
                                ExternalAPI.TO.Root myDeserializedClass = JsonConvert.DeserializeObject<ExternalAPI.TO.Root>(jsonString);

                                if (APIConfig.Listcricketupdate != null)
                                {
                                    // string marketid = myDeserializedClass.result.room[0].marketId;
                                    myDeserializedClass.Result.Room = Convert.ToInt32(item.EventID);
                                    if (APIConfig.Listcricketupdate.ToList().Count() > 0)
                                    {
                                        var currbpmarket = APIConfig.Listcricketupdate.ToList().Where(item2 => item2.Result.Room == Convert.ToInt32(item.EventID)).FirstOrDefault();
                                        if (currbpmarket != null)
                                        {
                                            var index = APIConfig.Listcricketupdate.ToList().IndexOf(currbpmarket);
                                            if (index != -1)
                                                APIConfig.Listcricketupdate[index] = myDeserializedClass;
                                        }
                                        else
                                        {
                                            APIConfig.Listcricketupdate.Add(myDeserializedClass);
                                        }
                                    }
                                    else
                                    {
                                        APIConfig.Listcricketupdate.Add(myDeserializedClass);
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                APIConfig.WriteErrorToDB("Get Data from server " + ex.Message.ToString());
                            }
                        }
                    }
                }
            }


            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);

            }
        }


        public void GetBallbyBallSummaryNew()
        {
            try
            {
                List<SP_UserMarket_GetDistinctLinevMarkets_Result> lstEvents = dbEntities.SP_UserMarket_GetDistinctLinevMarkets().ToList<SP_UserMarket_GetDistinctLinevMarkets_Result>();
                if (lstEvents.Count > 0)
                {
                    foreach (var item in lstEvents)
                    {
                        //item.EventOpenDate >= DateTime.Now.AddHours(-5) ||
                        if (item.MarketStatus != "Closed")
                        {
                            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("http://139.59.82.99:3000/api/match/getMatchScore?eventId=" + item.EventID + ""));
                            WebReq.Method = "GET";
                            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                            Console.WriteLine(WebResp.StatusCode);
                            Console.WriteLine(WebResp.Server);
                            string jsonString;
                            using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
                            {
                                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                                jsonString = reader.ReadToEnd();
                            }
                            try
                            {
                                ExternalAPI.TO.UpdateResult myDeserializedClass = JsonConvert.DeserializeObject<ExternalAPI.TO.UpdateResult>(jsonString);

                                if (APIConfig.ListcricketupdateNew != null)
                                {
                                    // string marketid = myDeserializedClass.result.room[0].marketId;
                                    myDeserializedClass.Result.Room = Convert.ToInt32(item.EventID);
                                    if (APIConfig.ListcricketupdateNew.ToList().Count() > 0)
                                    {
                                        var currbpmarket = APIConfig.ListcricketupdateNew.ToList().Where(item2 => item2.Result.Room == Convert.ToInt32(item.EventID)).FirstOrDefault();
                                        if (currbpmarket != null)
                                        {
                                            var index = APIConfig.ListcricketupdateNew.ToList().IndexOf(currbpmarket);
                                            if (index != -1)
                                                APIConfig.ListcricketupdateNew[index] = myDeserializedClass;
                                        }
                                        else
                                        {
                                            APIConfig.ListcricketupdateNew.Add(myDeserializedClass);
                                        }
                                    }
                                    else
                                    {
                                        APIConfig.ListcricketupdateNew.Add(myDeserializedClass);
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {
                                APIConfig.WriteErrorToDB("Get Data from server " + ex.Message.ToString());
                            }
                        }
                    }
                }
            }


            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);

            }
        }

        public ExternalAPI.TO.Home GetUpdate(string EventID)
        {
            //http://172.104.164.101:3000/mscore/30830827
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("http://139.59.82.99:3000/api/match/getMatchScore?eventId=" + EventID + ""));

            WebReq.Method = "GET";
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
            Console.WriteLine(WebResp.StatusCode);
            Console.WriteLine(WebResp.Server);

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }
            try
            {
                ExternalAPI.TO.Root root = JsonConvert.DeserializeObject<ExternalAPI.TO.Root>(jsonString);
                //return root;
                string convertstring = root.Result.Home;
                ExternalAPI.TO.Home update = new ExternalAPI.TO.Home();
                update = JsonConvert.DeserializeObject<ExternalAPI.TO.Home>(convertstring);
                return update;
            }
            catch (System.Exception ex)
            {
                ExternalAPI.TO.Home myDeserializedClass = new ExternalAPI.TO.Home();
                APIConfig.LogError(ex);
                return myDeserializedClass;
            }

        }

        public ExternalAPI.TO.UpdateNew GetUpdateNew(string EventID)
        {
            //http://172.104.164.101:3000/mscore/30830827
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("http://139.59.82.99:3000/api/match/getMatchScore?eventId=" + EventID + ""));

            WebReq.Method = "GET";
            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
            Console.WriteLine(WebResp.StatusCode);
            Console.WriteLine(WebResp.Server);

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }
            try
            {
                ExternalAPI.TO.UpdateResult root = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateResult>(jsonString);
               // ExternalAPI.TO.UpdateResult root = JsonConvert.DeserializeObject<ExternalAPI.TO.UpdateResult>(jsonString);
                //return root;
                string convertstring = root.Result.ToString();
                ExternalAPI.TO.UpdateNew update = new ExternalAPI.TO.UpdateNew();
                update = JsonConvert.DeserializeObject<ExternalAPI.TO.UpdateNew>(convertstring);
                return update;
            }
            catch (System.Exception ex)
            {
                ExternalAPI.TO.UpdateNew myDeserializedClass = new ExternalAPI.TO.UpdateNew();
                APIConfig.LogError(ex);
                return myDeserializedClass;
            }

        }

        public ExternalAPI.TO.MarketBookForindianFancy GetMarketDatabyIDIndianFancy(string EventID, string MarketBookID)
        {
            //http://royalold.com/api/MatchOdds/GetOddslite/4/1.181537772/30404597
                 //HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("http://139.162.20.164:3000/getdemodata?eventId="+EventID+""));
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("http://royalold.com/api/MatchOdds/GetOddslite/4/"+ MarketBookID + "/"+EventID+""));

            WebReq.Method = "GET";

            HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

            Console.WriteLine(WebResp.StatusCode);
            Console.WriteLine(WebResp.Server);

            string jsonString;
            using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }
            try
            {
                //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
                GetData myDeserializedClass = JsonConvert.DeserializeObject<GetData>(jsonString);
                MarketBookForindianFancy marketbook = new MarketBookForindianFancy();
                List<ExternalAPI.TO.RunnerForIndianFancy> lstRunners = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                var lstpricelistback = new List<PriceSize>();
                var lstpricelistlay = new List<PriceSize>();

                for (int i = 0; i < myDeserializedClass.session.Count; i++)
                {
                    var runner = new ExternalAPI.TO.RunnerForIndianFancy();
                    runner.RunnerName = myDeserializedClass.session[i].RunnerName;
                    marketbook.MarketId = MarketBookID;
                    runner.SelectionId = myDeserializedClass.session[i].SelectionId;
                    marketbook.BettingAllowed = true;
                    marketbook.MarketBookName = myDeserializedClass.session[i].RunnerName;
                    runner.MarketStatusStr = myDeserializedClass.session[i].GameStatus;

                    runner.LaySize = myDeserializedClass.session[i].LaySize1;
                    runner.Layprice = myDeserializedClass.session[i].LayPrice1;

                    runner.BackSize = myDeserializedClass.session[i].BackSize1;
                    runner.Backprice = myDeserializedClass.session[i].BackPrice1;

                    // Cricket
                    lstRunners.Add(runner);

                }

                marketbook.MainSportsname = "Cricket";

                marketbook.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>(lstRunners);

                return marketbook;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                var marketbook = new MarketBookForindianFancy();
                return marketbook;
            }

        }

        public ExternalAPI.TO.Root GetUpdate2(string EventID)
        {
            var marketbooks = new ExternalAPI.TO.Root();
            try
            {

                if (APIConfig.Listcricketupdate != null)
                {
                       var currmarketbook = APIConfig.Listcricketupdate.ToList().Where(item => item.Result.Room ==Convert.ToInt32(EventID)).FirstOrDefault();

                    marketbooks = currmarketbook;
                  
                }
                return marketbooks;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                var marketbook = new ExternalAPI.TO.Root();
                return marketbook;
            }
        }

        public string GetRunnersForFancy(string EventID, string MarketBookID)
        {
            ExternalAPI.TO.GetDataFancy marketbooks = new ExternalAPI.TO.GetDataFancy();
            try
            {
              
                if (APIConfig.MarketBookForindianFancy != null)
                {
                  
                    var currmarketbook = APIConfig.MarketBookForindianFancy.ToList().Where(item => item.MarketID == MarketBookID).FirstOrDefault();
               
                    marketbooks = currmarketbook;

                }
                string s = JsonConvert.SerializeObject(marketbooks);
                return s;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                var marketbook = new GetDataFancy();
                return marketbook.ToString(); ;
            }

        }

       
        public List<ExternalAPI.TO.LinevMarkets> GetEventIDFancyMarket(string EventID, string MarketBookID)
        {
            var marketbooks = new List<ExternalAPI.TO.LinevMarkets>();
            try
            {

                if (APIConfig.LinevMarkets != null)
                {

                    var currmarketbook = APIConfig.LinevMarkets.ToList().Where(item => item.EventID == EventID).ToList();

                    marketbooks = currmarketbook;
                  
                }
                return marketbooks;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                var marketbook = new List<ExternalAPI.TO.LinevMarkets>();
                return marketbook;
            }

        }


        public List<ExternalAPI.TO.MarketBook> GetMarketDatabyID(string[] marketID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory, string Password)
        {
            //GetBallbyBallSummary();
            if (1 == 1)
            {
                if (APIConfig.URLsData.Count == 0)
                {
                    SetURLsData();
                }
               
                try
                {
                    var marketbooks = new List<MarketBook>();
                    if (APIConfig.GetCricketDataFrom == "Live")
                    {
                        if (APIConfig.LiveCricketMarketBooks.Count > 0)
                        {
                            foreach (var marketitem in marketID)
                            {
                                var currmarketbook = APIConfig.LiveCricketMarketBooks.ToList().Where(item => item.MarketId == marketitem).FirstOrDefault();
                                if (currmarketbook != null)
                                {
                                    var marketbook = ConvertJsontoMarketObjectBFLive(currmarketbook, marketitem, OrignalOpenDate, sheetname, MainSportsCategory);

                                    marketbooks.Add(marketbook);
                                }
                                else
                                {
                                    currmarketbook = APIConfig.LiveCricketMarketBooksFancy.ToList().Where(item => item.MarketId == marketitem).FirstOrDefault();
                                    if (currmarketbook != null)
                                    {
                                        var marketbook = ConvertJsontoMarketObjectBFLive(currmarketbook, marketitem, OrignalOpenDate, sheetname, MainSportsCategory);


                                        marketbooks.Add(marketbook);
                                    }
                                }
                            }
                           
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        if (APIConfig.GetCricketDataFrom == "BP")

                        {
                            if (APIConfig.BFMarketBooks != null)
                            {
                                foreach (var marketitem in marketID)
                                {
                                    var currmarketbook = APIConfig.BFMarketBooks.ToList().Where(item => item.MarketId == marketitem).FirstOrDefault();
                                    if (currmarketbook != null)
                                    {
                                        var marketbook = ConvertJsontoMarketObjectBF123(currmarketbook, marketitem, OrignalOpenDate, sheetname, MainSportsCategory);


                                        marketbooks.Add(marketbook);
                                    }
                                    else
                                    {
                                        currmarketbook = APIConfigforResults.BFMarketBooksFancy.ToList().Where(item => item.MarketId == marketitem).FirstOrDefault();
                                        if (currmarketbook != null)
                                        {
                                            var marketbook = ConvertJsontoMarketObjectBF123(currmarketbook, marketitem, OrignalOpenDate, sheetname, MainSportsCategory);


                                            marketbooks.Add(marketbook);
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {

                            if (APIConfig.BFMarketBooksOther123 != null)
                            {
                                foreach (var marketitem in marketID)
                                {
                               
                                var currmarketbook = APIConfig.BFMarketBooksOther123.ToList().Where(item => item.MarketBookId == marketitem).FirstOrDefault();
                                    if (currmarketbook != null)
                                    {
                                        var marketbook1 = ConvertJsontoMarketObjectBFNewSource(currmarketbook, marketitem, OrignalOpenDate, sheetname, MainSportsCategory);


                                        marketbooks.Add(marketbook1);
                                    }
                                    else
                                    {
                                        //currmarketbook = APIConfigforResults.BFMarketBooksOtherFancy.Where(item => item.MarketId == marketitem).FirstOrDefault();
                                        //if (currmarketbook != null)
                                        //{
                                        //    var marketbook1 = ConvertJsontoMarketObjectBF(currmarketbook, marketitem, OrignalOpenDate, sheetname, MainSportsCategory);


                                        //    marketbooks.Add(marketbook1);
                                        //}
                                    }
                                }

                            }

                            //var marketbook = GetCurrentMarketBook(marketID[0], sheetname, MainSportsCategory, OrignalOpenDate);
                            //  var marketbook = GetCurrentMarketBook123(marketID[0], sheetname, MainSportsCategory, OrignalOpenDate);
                            //marketbooks.Add(marketbook);
                        }

                    }

                    return marketbooks;
                    //var marketbooks = new List<MarketBook>();
                    //var marketbook = GetCurrentMarketBook(marketID[0], sheetname, MainSportsCategory, OrignalOpenDate);
                    //// var marketbook = GetCurrentMarketBook123(marketID[0], sheetname, MainSportsCategory, OrignalOpenDate);
                    //marketbooks.Add(marketbook);
                    //return marketbooks;


                }
                catch (System.Exception ex)
                {
                    APIConfig.LogError(ex);
                    var marketbooks = new List<MarketBook>();
                    return marketbooks;
                }
            }
            else
            {
                var marketbooks = new List<MarketBook>();
                return marketbooks;
            }
        }
        private bool ValidatePassword(string Password)
        {
            if (Password == ConfigurationManager.AppSettings["PasswordForValidate"])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ValidatePasswordS(string Password)
        {
            if (Password == ConfigurationManager.AppSettings["PasswordForValidateS"])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bfnexchange.wrBF.MarketBook[] GetCurrentMarketBookNew(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate, bool BettingAllowed, List<ExternalAPI.TO.Runner> OldRunners, MarketBook currentmarketobject, string Password)
        {
            bfnexchange.wrBF.MarketBook[] list;

            if (ValidatePassword(Password) == true)
            {


                string[] marketIds = new string[]
                      {
                    marketid
                      };



                if (MainSportsCategory == "Soccer")
                {
                    ws1.Url = "http://5.133.176.76/ws0/wsnew.asmx";
                    list = ws1.GDLBF("6456(^(647HD764fufy", marketIds, 1);
                }
                else if (MainSportsCategory == "Tennis")
                {
                    ws2.Url = "http://5.133.176.76/ws0/wsnew.asmx";
                    list = ws2.GDLBF("6456(^(647HD764fufy", marketIds, 2);
                }
                else if (MainSportsCategory == "Cricket")
                {
                    ws4.Url = "http://5.133.176.76/ws4/wsnew.asmx";
                    list = ws4.GDLBF("6456(^(647HD764fufy", marketIds, 4);
                }
                else if (MainSportsCategory.Contains("Horse Racing"))
                {
                    ws7.Url = "http://5.133.176.76/ws4/wsnew.asmx";
                    list = ws7.GDLBF("6456(^(647HD764fufy", marketIds, 7);
                }
                else if (MainSportsCategory.Contains("Greyhound Racing"))
                {
                    ws7.Url = "http://5.133.176.76/ws4/wsnew.asmx";
                    list = ws7.GDLBF("6456(^(647HD764fufy", marketIds, 13);
                }
                else
                {
                    ws0.Url = "http://5.133.176.76/ws0/wsnew.asmx";
                    list = ws0.GDLBF("6456(^(647HD764fufy", marketIds, 0);
                }
                return list;

            }
            else
            {
                return new wrBF.MarketBook[0];
            }
        }

        public MarketBook ConvertJsontoMarketObjectBFLive(ExternalAPI.TO.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory)
        {

            try
            {



                if (1 == 1)
                {
                    var marketbook = new MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    if (sheetname.Contains("(US)") && MainSportsCategory.Contains("Horse Racing"))
                    {
                        marketbook.PoundRate = Convert.ToDecimal(ConfigurationManager.AppSettings["PoundRateForUS"]);
                    }
                    else
                    {
                        marketbook.PoundRate = Convert.ToDecimal(ConfigurationManager.AppSettings["PoundRateForOThers"]);
                    }

                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.Version = BFMarketbook.Version;
                    marketbook.TotalMatched = BFMarketbook.TotalMatched;
                    DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
                    DateTime CurrentDate = DateTime.Now;
                    TimeSpan remainingdays = (CurrentDate - OpenDate);
                    if (OpenDate < CurrentDate)
                    {
                        marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                    }
                    else
                    {
                        marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                    }

                    if (BFMarketbook.IsInplay == true && BFMarketbook.Status.ToString() == "OPEN")
                    {

                        marketbook.MarketStatusstr = "In Play";
                        marketbook.PoundRate = 50;
                    }
                    else
                    {
                        if (BFMarketbook.Status.ToString() == "CLOSED")
                        {
                            marketbook.MarketStatusstr = "Closed";
                        }
                        else
                        {
                            if (BFMarketbook.Status.ToString() == "SUSPENDED")
                            {
                                marketbook.MarketStatusstr = "Suspended";
                            }
                            else
                            {
                                marketbook.MarketStatusstr = "Active";
                            }

                        }

                    }

                    List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                    // var selectionnames = objUserServices.GetSelectionNamesbyMarketID(marketbook.MarketId);
                    List<SP_MarketCatalogueSelections_Get_Result> selectionnames = new List<SP_MarketCatalogueSelections_Get_Result>();
                    try
                    {
                        //  selectionnames = objUserServices.GetSelectionNamesbyMarketID(marketbook.MarketId);
                    }
                    catch (System.Exception ex)
                    {

                    }
                    foreach (var runneritem in BFMarketbook.Runners)
                    {
                        var runner = new ExternalAPI.TO.Runner();
                        try
                        {
                            if (selectionnames.Count > 0)
                            {
                                runner.RunnerName = selectionnames.Where(item2 => item2.SelectionID == runneritem.SelectionId.ToString()).FirstOrDefault().SelectionName;
                            }

                        }
                        catch (System.Exception ex)
                        {

                        }

                        runner.Handicap = runneritem.Handicap;
                        runner.StatusStr = runneritem.Status.ToString();
                        runner.SelectionId = runneritem.SelectionId.ToString();
                        runner.LastPriceTraded = runneritem.LastPriceTraded;
                        var lstpricelist = new List<PriceSize>();
                        if (runneritem.ExchangePrices.AvailableToBack != null && runneritem.ExchangePrices.AvailableToBack.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {
                                try
                                {


                                    if (runneritem.ExchangePrices.AvailableToBack[0].Price.ToString().Contains("."))
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                                        {
                                            var pricesize = new PriceSize();
                                            pricesize.OrignalSize = backitems.Size;
                                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                                        {
                                            var pricesize = new PriceSize();
                                            pricesize.OrignalSize = backitems.Size;
                                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = backitems.Price;

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                                {
                                    var pricesize = new PriceSize();
                                    pricesize.OrignalSize = backitems.Size;
                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new PriceSize();
                                pricesize.OrignalSize = 0;
                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices = new ExchangePrices();
                        runner.ExchangePrices.AvailableToBack = lstpricelist;
                        lstpricelist = new List<PriceSize>();
                        if (runneritem.ExchangePrices.AvailableToLay != null && runneritem.ExchangePrices.AvailableToLay.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {
                                if (runneritem.ExchangePrices.AvailableToLay[0].Price.ToString().Contains("."))
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                                    {
                                        var pricesize = new PriceSize();
                                        pricesize.OrignalSize = backitems.Size;
                                        pricesize.Size = Convert.ToInt64((backitems.Size) * Convert.ToDouble(marketbook.PoundRate));
                                        pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                        pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                                else
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                                    {
                                        var pricesize = new PriceSize();
                                        pricesize.OrignalSize = backitems.Size;
                                        pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                        pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                        pricesize.Price = backitems.Price;

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                                {
                                    var pricesize = new PriceSize();
                                    pricesize.OrignalSize = backitems.Size;
                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new PriceSize();
                                pricesize.OrignalSize = 0;
                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                        runner.ExchangePrices.AvailableToLay = lstpricelist;
                        lstRunners.Add(runner);
                    }


                    marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);

                    double lastback = 0;
                    double lastbackSize = 0;
                    double lastLaySize = 0;
                    double lastlay = 0;

                    if (marketbook.MarketStatusstr != "Suspended")
                    {

                        double favBack = marketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price;
                        string selectionIDfav = marketbook.Runners[0].SelectionId;
                        foreach (var favoriteitem in marketbook.Runners)
                        {
                            if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Count > 0)
                                if (marketbook.MainSportsname.Contains("Racing"))
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack && favoriteitem.ExchangePrices.AvailableToBack[0].Price > 0)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }
                                else
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }

                        }
                        if (marketbook.MarketStatusstr == "Closed")
                        {
                            var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER");
                            if (resultsfav != null && resultsfav.Count() > 0)
                            {
                                selectionIDfav = resultsfav.FirstOrDefault().SelectionId;
                            }
                        }
                        var favoriteteam = marketbook.Runners.Where(ii => ii.SelectionId == selectionIDfav).LastOrDefault();
                        string selectionname = favoriteteam.RunnerName;
                        if (favoriteteam.ExchangePrices.AvailableToBack.Count > 0)
                        {
                            lastback = favoriteteam.ExchangePrices.AvailableToBack[0].Price;
                            lastbackSize = favoriteteam.ExchangePrices.AvailableToBack[0].Size;


                        }
                        if (favoriteteam.ExchangePrices.AvailableToLay.Count > 0)
                        {

                            lastLaySize = favoriteteam.ExchangePrices.AvailableToLay[0].Size;
                            lastlay = favoriteteam.ExchangePrices.AvailableToLay[0].Price;

                        }
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = selectionname;
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = selectionIDfav;
                    }
                    else
                    {
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = "";
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = "0";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteBack) < 0)
                    {
                        marketbook.FavoriteBack = "0";
                        marketbook.FavoriteBackSize = "0";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteLay) < 0)
                    {
                        marketbook.FavoriteLay = "0";
                        marketbook.FavoriteLaySize = "0";
                    }
                    return marketbook;

                }
                else
                {
                    return new MarketBook();
                }
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                return new MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBFNew(bfnexchange.wrBF.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, List<ExternalAPI.TO.Runner> OldRunners, MarketBook marketbook)
        {


            try
            {


                if (1 == 1)
                {
                    // var marketbook = new MarketBook();

                    //  marketbook.MarketId = BFMarketbook.MarketId;
                    // marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    //  marketbook.PoundRate = LoggedinUserDetail.PoundRate;
                    //  marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    //  marketbook.MarketBookName = sheetname;
                    //  marketbook.MainSportsname = MainSportsCategory;
                    //   marketbook.OrignalOpenDate = marketopendate;
                    DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
                    DateTime CurrentDate = DateTime.Now;
                    TimeSpan remainingdays = (CurrentDate - OpenDate);
                    if (OpenDate < CurrentDate)
                    {
                        marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                    }
                    else
                    {
                        marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                    }

                    if (BFMarketbook.IsInplay == true && BFMarketbook.Status.ToString() == "OPEN")
                    {

                        marketbook.MarketStatusstr = "IN-PLAY";
                        marketbook.PoundRate = 50;
                    }
                    else
                    {
                        if (BFMarketbook.Status.ToString() == "CLOSED")
                        {
                            marketbook.MarketStatusstr = "CLOSED";
                        }
                        else
                        {
                            if (BFMarketbook.Status.ToString() == "SUSPENDED")
                            {
                                marketbook.MarketStatusstr = "SUSPENDED";
                            }
                            else
                            {
                                marketbook.MarketStatusstr = "ACTIVE";
                            }

                        }

                    }

                    List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                    foreach (var runneritem in BFMarketbook.Runners)
                    {
                        var runner = OldRunners.Where(item1 => item1.SelectionId == runneritem.SelectionId.ToString()).FirstOrDefault();
                        //if (OldRunners != null)
                        //{
                        //    runner.RunnerName = OldRunners.Where(item => item.SelectionId == runneritem.SelectionId.ToString()).First().RunnerName;
                        //}


                        runner.Handicap = runneritem.Handicap;
                        runner.StatusStr = runneritem.Status.ToString();
                        runner.SelectionId = runneritem.SelectionId.ToString();
                        runner.LastPriceTraded = runneritem.LastPriceTraded;
                        var lstpricelist = new List<PriceSize>();
                        if (runneritem.ExchangePrices.AvailableToBack != null && runneritem.ExchangePrices.AvailableToBack.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                                //foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                                //{
                                //    var pricesize = new PriceSize();

                                //    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

                                //    pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                //    lstpricelist.Add(pricesize);
                                //}
                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new PriceSize();

                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        // runner.ExchangePrices = new ExchangePrices();
                        runner.ExchangePrices.AvailableToBack = lstpricelist;
                        lstpricelist = new List<PriceSize>();
                        if (runneritem.ExchangePrices.AvailableToLay != null && runneritem.ExchangePrices.AvailableToLay.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                                //foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                //{
                                //    var pricesize = new PriceSize();

                                //    pricesize.Size = Convert.ToInt64((backitems.Size) * Convert.ToDouble(marketbook.PoundRate));

                                //    pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                //    lstpricelist.Add(pricesize);
                                //}
                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new PriceSize();

                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        //  runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                        runner.ExchangePrices.AvailableToLay = lstpricelist;
                        lstRunners.Add(runner);
                    }


                    // marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);

                    double lastback = 0;
                    double lastbackSize = 0;
                    double lastLaySize = 0;
                    double lastlay = 0;

                    if (marketbook.MarketStatusstr != "SUSPENDED")
                    {

                        double favBack = marketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price;
                        string selectionIDfav = marketbook.Runners[0].SelectionId;
                        foreach (var favoriteitem in marketbook.Runners)
                        {
                            if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Count > 0)
                                if (marketbook.MainSportsname.Contains("Racing"))
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack && favoriteitem.ExchangePrices.AvailableToBack[0].Price > 0)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }
                                else
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }

                        }
                        if (marketbook.MarketStatusstr == "CLOSED")
                        {
                            var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER");
                            if (resultsfav != null && resultsfav.Count() > 0)
                            {
                                selectionIDfav = resultsfav.FirstOrDefault().SelectionId;
                            }
                            // selectionIDfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER").FirstOrDefault().SelectionId;
                        }
                        var favoriteteam = marketbook.Runners.Where(ii => ii.SelectionId == selectionIDfav).LastOrDefault();
                        string selectionname = favoriteteam.RunnerName;
                        if (favoriteteam.ExchangePrices.AvailableToBack.Count > 0)
                        {
                            lastback = favoriteteam.ExchangePrices.AvailableToBack[0].Price;
                            lastbackSize = favoriteteam.ExchangePrices.AvailableToBack[0].Size;


                        }
                        if (favoriteteam.ExchangePrices.AvailableToLay.Count > 0)
                        {

                            lastLaySize = favoriteteam.ExchangePrices.AvailableToLay[0].Size;
                            lastlay = favoriteteam.ExchangePrices.AvailableToLay[0].Price;

                        }
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = selectionname;
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = selectionIDfav;
                    }
                    else
                    {
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = "";
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = "0";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteBack) <= 0)
                    {
                        marketbook.FavoriteBack = "";
                        marketbook.FavoriteBackSize = "";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteLay) <= 0)
                    {
                        marketbook.FavoriteLay = "";
                        marketbook.FavoriteLaySize = "";
                    }
                    return marketbook;

                }
                else
                {
                    return new MarketBook();
                }
            }
            catch (System.Exception ex)
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        private wsnew wsBFMatch = new wsnew();
        public static string strWsMatch = "http://rsexch.bfbetpro.com/theService/MainSvc.asmx";
        public ExternalAPI.TO.MarketBook GetCurrentMarketBook123(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate)
        {
            try
            {
                this.wsBFMatch.Url = strWsMatch;
                if (APIConfig.objCurrSession.token != null)
                {
                    ExternalAPI.TO.MarketBook marketbook;
                    this.wsBFMatch.Url = strWsMatch;
                    //  SampleResponse1[] objmarketbook2 = JsonConvert.DeserializeObject<SampleResponse1[]>(objMainServiceClient.getMarketBook(marketid, APIConfig.objCurrSessionNew));
                    SampleResponse1[] objmarketbook2 = JsonConvert.DeserializeObject<SampleResponse1[]>(this.wsBFMatch.getMarketBook(marketid, APIConfig.objCurrSession));

                    marketbook = ConvertJsontoMarketObjectBF123(objmarketbook2[0], marketid, marketopendate, sheetname, MainSportsCategory);
                    return marketbook;
                }
                else
                {
                    GetUserSessionForData(ConfigurationManager.AppSettings["UserNameforData"], ConfigurationManager.AppSettings["UserPasswordforData"]);
                    ExternalAPI.TO.MarketBook marketbook;
                    this.wsBFMatch.Url = strWsMatch;
                    // SampleResponse1[] objmarketbook2 = JsonConvert.DeserializeObject<SampleResponse1[]>(objMainServiceClient.getMarketBook(marketid, APIConfig.objCurrSessionNew));
                    SampleResponse1[] objmarketbook2 = JsonConvert.DeserializeObject<SampleResponse1[]>(this.wsBFMatch.getMarketBook(marketid, APIConfig.objCurrSession));

                    marketbook = ConvertJsontoMarketObjectBF123(objmarketbook2[0], marketid, marketopendate, sheetname, MainSportsCategory);
                    return marketbook;
                }

            }
            catch (System.Exception ex)
            {
                //  GetUserSessionForData(Configurationr Manager.AppSettings["UserNameforData"], ConfigurationManager.AppSettings["UserPasswordforData"]);
                APIConfig.WriteErrorToDB(ex.Message);
                APIConfig.LogError(ex);
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public void GetUserSessionForData(string Username, string Password)
        {
            try
            {


                BetterInfo objResult = this.wsBFMatch.UserLogin("", Username, Password, "");
                APIConfig.objCurrSession.id = objResult.BetterId;
                APIConfig.objCurrSession.token = objResult.PrivateKey;
                APIConfig.objCurrSession.Username = objResult.BetterName;
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

        public void SetURLsData()
        {
            APIConfig.URLsData = JsonConvert.DeserializeObject<List<bfnexchange.Services.DBModel.SP_URLsData_GetAllData_Result>>(objUserServices.GetURLsData());
            ws1.Url = APIConfig.URLsData.Where(item => item.EventType == "Soccer").FirstOrDefault().URLForData;
            ws2.Url = APIConfig.URLsData.Where(item => item.EventType == "Tennis").FirstOrDefault().URLForData;
            ws4.Url = APIConfig.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().URLForData;
            ws7.Url = APIConfig.URLsData.Where(item => item.EventType == "Horse Racing").FirstOrDefault().URLForData;
            ws4339.Url = APIConfig.URLsData.Where(item => item.EventType == "GreyHound Racing").FirstOrDefault().URLForData;


            ws0.Url = APIConfig.URLsData.Where(item => item.EventType == "Other").FirstOrDefault().URLForData;
            APIConfig.SecurityCode = APIConfig.URLsData.FirstOrDefault().Scd;
            APIConfig.GetCricketDataFrom = APIConfig.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().GetDataFrom;

        }
        public ExternalAPI.TO.MarketBook GetCurrentMarketBook(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate)
        {
            try
            {

                if (APIConfig.URLsData.Count == 0)
                {
                    SetURLsData();
                }
                string[] marketIds = new string[]
                    {
                    marketid
                    };
                IList<bfnexchange.wrBF.MarketBook> list;
                if (MainSportsCategory == "Soccer")
                {

                    list = ws1.GD(APIConfig.SecurityCode, marketIds, 1);
                }
                else if (MainSportsCategory == "Tennis")
                {

                    list = ws2.GD(APIConfig.SecurityCode, marketIds, 2);
                }
                else if (MainSportsCategory == "Cricket")
                {

                    list = ws4.GD(APIConfig.SecurityCode, marketIds, 4);
                }
                else if (MainSportsCategory.Contains("Horse Racing"))
                {

                    list = ws7.GD(APIConfig.SecurityCode, marketIds, 7);
                }
                else if (MainSportsCategory.Contains("Greyhound Racing"))
                {

                    list = ws4339.GD(APIConfig.SecurityCode, marketIds, 13);
                }
                else
                {

                    list = ws0.GD(APIConfig.SecurityCode, marketIds, 0);
                }
                if (list.Count > 0)
                {
                    ExternalAPI.TO.MarketBook objMarketbook = ConvertJsontoMarketObjectBF(list[0], marketid, marketopendate, sheetname, MainSportsCategory);
                    return objMarketbook;
                }
                else
                {
                    return new ExternalAPI.TO.MarketBook();
                }
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBF(bfnexchange.wrBF.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory)
        {



            if (1 == 1)
            {
                var marketbook = new MarketBook();

                marketbook.MarketId = BFMarketbook.MarketId;
                marketbook.SheetName = "";
                marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                if (sheetname.Contains("(US)") && MainSportsCategory.Contains("Horse Racing"))
                {
                    marketbook.PoundRate = Convert.ToDecimal(ConfigurationManager.AppSettings["PoundRateForUS"]);
                }
                else
                {
                    marketbook.PoundRate = Convert.ToDecimal(ConfigurationManager.AppSettings["PoundRateForOThers"]);
                }
                marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                marketbook.MarketBookName = sheetname;
                marketbook.MainSportsname = MainSportsCategory;
                marketbook.OrignalOpenDate = marketopendate;
                marketbook.Version = BFMarketbook.Version;
                marketbook.TotalMatched = BFMarketbook.TotalMatched;
                DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
                DateTime CurrentDate = DateTime.Now;
                TimeSpan remainingdays = (CurrentDate - OpenDate);
                if (OpenDate < CurrentDate)
                {
                    marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                }
                else
                {
                    marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                }

                if (BFMarketbook.IsInplay == true && BFMarketbook.Status.ToString() == "OPEN")
                {

                    marketbook.MarketStatusstr = "In Play";
                    marketbook.PoundRate = 50;
                }
                else
                {
                    if (BFMarketbook.Status.ToString() == "CLOSED")
                    {
                        marketbook.MarketStatusstr = "Closed";
                    }
                    else
                    {
                        if (BFMarketbook.Status.ToString() == "SUSPENDED")
                        {
                            marketbook.MarketStatusstr = "Suspended";
                        }
                        else
                        {
                            marketbook.MarketStatusstr = "Active";
                        }

                    }

                }

                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                foreach (var runneritem in BFMarketbook.Runners)
                {
                    var runner = new ExternalAPI.TO.Runner();
                    runner.Handicap = runneritem.Handicap;
                    runner.StatusStr = runneritem.Status.ToString();
                    runner.SelectionId = runneritem.SelectionId.ToString();
                    runner.LastPriceTraded = runneritem.LastPriceTraded;
                    var lstpricelist = new List<PriceSize>();
                    if (runneritem.ExchangePrices.AvailableToBack != null && runneritem.ExchangePrices.AvailableToBack.Count() > 0)
                    {
                        if (BFMarketbook.Runners.Count() == 1)
                        {
                            try
                            {


                                if (runneritem.ExchangePrices.AvailableToBack[0].Price.ToString().Contains("."))
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                                    {
                                        var pricesize = new PriceSize();
                                        pricesize.OrignalSize = backitems.Size;
                                        pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                        pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                        pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                                else
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                    {
                                        var pricesize = new PriceSize();
                                        pricesize.OrignalSize = backitems.Size;
                                        pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                        pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                        pricesize.Price = backitems.Price;

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        else
                        {
                            foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                            {
                                var pricesize = new PriceSize();
                                pricesize.OrignalSize = backitems.Size;
                                pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                pricesize.Price = backitems.Price;

                                lstpricelist.Add(pricesize);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            var pricesize = new PriceSize();
                            pricesize.OrignalSize = 0;
                            pricesize.Size = 0;

                            pricesize.Price = 0;

                            lstpricelist.Add(pricesize);
                        }
                    }

                    runner.ExchangePrices = new ExchangePrices();
                    runner.ExchangePrices.AvailableToBack = lstpricelist;
                    lstpricelist = new List<PriceSize>();
                    if (runneritem.ExchangePrices.AvailableToLay != null && runneritem.ExchangePrices.AvailableToLay.Count() > 0)
                    {
                        if (BFMarketbook.Runners.Count() == 1)
                        {
                            if (runneritem.ExchangePrices.AvailableToLay[0].Price.ToString().Contains("."))
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                {
                                    var pricesize = new PriceSize();
                                    pricesize.OrignalSize = backitems.Size;
                                    pricesize.Size = Convert.ToInt64((backitems.Size) * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                    lstpricelist.Add(pricesize);
                                }
                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                                {
                                    var pricesize = new PriceSize();
                                    pricesize.OrignalSize = backitems.Size;
                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }
                        }
                        else
                        {
                            foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                            {
                                var pricesize = new PriceSize();
                                pricesize.OrignalSize = backitems.Size;
                                pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                pricesize.Price = backitems.Price;

                                lstpricelist.Add(pricesize);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            var pricesize = new PriceSize();
                            pricesize.OrignalSize = 0;
                            pricesize.Size = 0;

                            pricesize.Price = 0;

                            lstpricelist.Add(pricesize);
                        }
                    }

                    runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                    runner.ExchangePrices.AvailableToLay = lstpricelist;
                    lstRunners.Add(runner);
                }


                marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);

                double lastback = 0;
                double lastbackSize = 0;
                double lastLaySize = 0;
                double lastlay = 0;

                if (marketbook.MarketStatusstr != "Suspended")
                {

                    double favBack = marketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price;
                    string selectionIDfav = marketbook.Runners[0].SelectionId;
                    foreach (var favoriteitem in marketbook.Runners)
                    {
                        if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Count > 0)
                            if (marketbook.MainSportsname.Contains("Racing"))
                            {
                                if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack && favoriteitem.ExchangePrices.AvailableToBack[0].Price > 0)
                                {
                                    favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                    selectionIDfav = favoriteitem.SelectionId;
                                }
                            }
                            else
                            {
                                if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack)
                                {
                                    favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                    selectionIDfav = favoriteitem.SelectionId;
                                }
                            }

                    }
                    if (marketbook.MarketStatusstr == "Closed")
                    {
                        var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER");
                        if (resultsfav != null && resultsfav.Count() > 0)
                        {
                            selectionIDfav = resultsfav.FirstOrDefault().SelectionId;
                        }
                    }
                    var favoriteteam = marketbook.Runners.Where(ii => ii.SelectionId == selectionIDfav).LastOrDefault();
                    string selectionname = favoriteteam.RunnerName;
                    if (favoriteteam.ExchangePrices.AvailableToBack.Count > 0)
                    {
                        lastback = favoriteteam.ExchangePrices.AvailableToBack[0].Price;
                        lastbackSize = favoriteteam.ExchangePrices.AvailableToBack[0].Size;


                    }
                    if (favoriteteam.ExchangePrices.AvailableToLay.Count > 0)
                    {

                        lastLaySize = favoriteteam.ExchangePrices.AvailableToLay[0].Size;
                        lastlay = favoriteteam.ExchangePrices.AvailableToLay[0].Price;

                    }
                    marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                    marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                    marketbook.FavoriteSelectionName = selectionname;
                    marketbook.FavoriteBackSize = lastbackSize.ToString();
                    marketbook.FavoriteLaySize = lastLaySize.ToString();
                    marketbook.FavoriteID = selectionIDfav;
                }
                else
                {
                    marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                    marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                    marketbook.FavoriteSelectionName = "";
                    marketbook.FavoriteBackSize = lastbackSize.ToString();
                    marketbook.FavoriteLaySize = lastLaySize.ToString();
                    marketbook.FavoriteID = "0";
                }
                return marketbook;

            }
            else
            {
                return new MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBFNewSource(ExternalAPI.TO.MarketBookString BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory)
        {

            if (1 == 1)
            {
                var marketbook = new MarketBook();
                string[] newres = BFMarketbook.MarketBookData.Split(':').Select(tag => tag.Trim()).ToArray();
                string[] BFMarketBookDetail = newres[0].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();

                marketbook.MarketId = BFMarketbook.MarketBookId;
                marketbook.SheetName = "";
                marketbook.IsMarketDataDelayed = false;
                if (sheetname.Contains("(US)") && MainSportsCategory.Contains("Horse Racing"))
                {
                    marketbook.PoundRate = Convert.ToDecimal(ConfigurationManager.AppSettings["PoundRateForUS"]);
                }
                else
                {
                    marketbook.PoundRate = Convert.ToDecimal(ConfigurationManager.AppSettings["PoundRateForOThers"]);
                }
                marketbook.NumberOfWinners = Convert.ToInt32(BFMarketBookDetail[6].Trim());
                marketbook.MarketBookName = sheetname;
                marketbook.MainSportsname = MainSportsCategory;
                marketbook.OrignalOpenDate = marketopendate;
                try
                {
                    marketbook.TotalMatched = Convert.ToInt64(Convert.ToDouble(BFMarketBookDetail[10]) * Convert.ToDouble(marketbook.PoundRate));
                }
                catch (System.Exception ex)
                {


                }

                DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
                DateTime CurrentDate = DateTime.Now;
                TimeSpan remainingdays = (CurrentDate - OpenDate);
                if (OpenDate < CurrentDate)
                {
                    marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                }
                else
                {
                    marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                }

                if (Convert.ToInt32(BFMarketBookDetail[5]) == 1 && BFMarketBookDetail[2].Trim().ToString() == "OPEN")
                {
                  
                    marketbook.MarketStatusstr = "In Play";
                    marketbook.PoundRate = 50;
                   
                    u.UpdateMarketStatusbyMarketBookID(marketbook.MarketId, marketbook.MarketStatusstr);


                }
                else
                {
                    if (BFMarketBookDetail[2].Trim().ToString() == "CLOSED")
                    {
                        marketbook.MarketStatusstr = "Closed";
                    }
                    else
                    {
                        if (BFMarketBookDetail[2].Trim().ToString() == "SUSPENDED")
                        {
                            marketbook.MarketStatusstr = "Suspended";
                        }
                        else
                        {
                            marketbook.MarketStatusstr = "Active";
                        }

                    }

                }

                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                for (int i = 1; i < newres.Count(); i++)
                {
                    if (newres[i] != "Æ  Æ" && newres[i] !="Æ  Æ UnitedBet Æ 1 Æ 1 Æ" && newres[i] != "Æ  Æ UnitedBet Æ 2 Æ 2 Æ")
                    {
                        string[] runnerdetails = newres[i].Split(new string[] { "|" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();
                        string[] runnerinfo = runnerdetails[0].Split('~').Select(tag => tag.Trim()).ToArray();
                        string[] runnerbackdata = runnerdetails[1].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                        string[] runnerlaydata = runnerdetails[2].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                        var runner = new ExternalAPI.TO.Runner();
                        try
                        {
                            runner.Handicap = Convert.ToDouble(runnerinfo[4].Trim());
                        }
                        catch (System.Exception)
                        {
                            runner.Handicap = 0;

                        }
                        runner.StatusStr = runnerinfo[6].Trim();
                        runner.SelectionId = runnerinfo[0].Trim().ToString();
                        try
                        {
                            runner.LastPriceTraded = Convert.ToDouble(runnerinfo[3].Trim().ToString());
                            runner.TotalMatchedStr = APIConfig.FormatNumber(Convert.ToInt64(Convert.ToDouble(runnerinfo[2]) * Convert.ToDouble(marketbook.PoundRate)));
                        }
                        catch (System.Exception ex)
                        {


                        }

                        var lstpricelist = new List<PriceSize>();
                        if (runnerbackdata.Count() > 0)
                        {
                            if (newres.Count() == 2)
                            {
                                try
                                {


                                    if (runnerbackdata[0].ToString().Contains("."))
                                    {
                                        for (int j = 0; j < runnerlaydata.Count();)
                                        {
                                            if (j < runnerlaydata.Count())
                                            {
                                                var pricesize = new PriceSize();
                                                pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1].Trim());
                                                pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1].Trim()) * Convert.ToDouble(marketbook.PoundRate));
                                                pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                                pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j].Trim()) + 0.5).ToString("F2"));

                                                lstpricelist.Add(pricesize);
                                                j = j + 4;

                                            }

                                        }
                                    }
                                    else
                                    {
                                        for (int j = 0; j < runnerbackdata.Count();)
                                        {
                                            if (j < runnerbackdata.Count())
                                            {
                                                var pricesize = new PriceSize();
                                                pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1].Trim());
                                                pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1].Trim()) * Convert.ToDouble(marketbook.PoundRate));
                                                pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                                pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j].Trim())).ToString("F2"));

                                                lstpricelist.Add(pricesize);
                                                j = j + 4;

                                            }

                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                            else
                            {
                                for (int j = 0; j < runnerbackdata.Count();)
                                {
                                    if (j < runnerbackdata.Count())
                                    {
                                        var pricesize = new PriceSize();
                                        pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1].Trim());
                                        pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1].Trim()) * Convert.ToDouble(marketbook.PoundRate));
                                        pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                        pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j].Trim())).ToString("F2"));

                                        lstpricelist.Add(pricesize);
                                        j = j + 4;

                                    }

                                }
                            }
                        }
                        else
                        {
                            for (int ii = 0; ii < 3; ii++)
                            {
                                var pricesize = new PriceSize();
                                pricesize.OrignalSize = 0;
                                pricesize.Size = 0;
                                pricesize.SizeStr = "0";
                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices = new ExchangePrices();
                        runner.ExchangePrices.AvailableToBack = lstpricelist;
                        lstpricelist = new List<PriceSize>();
                        if (runnerlaydata.Count() > 0)
                        {
                            if (newres.Count() == 2)
                            {
                                try
                                {


                                    if (runnerlaydata[0].ToString().Contains("."))
                                    {
                                        for (int j = 0; j < runnerbackdata.Count();)
                                        {
                                            if (j < runnerbackdata.Count())
                                            {
                                                var pricesize = new PriceSize();
                                                pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1].Trim());
                                                pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1].Trim()) * Convert.ToDouble(marketbook.PoundRate));
                                                pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                                pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j].Trim()) + 0.5).ToString("F2"));

                                                lstpricelist.Add(pricesize);
                                                j = j + 4;

                                            }

                                        }
                                    }
                                    else
                                    {
                                        for (int j = 0; j < runnerlaydata.Count();)
                                        {
                                            if (j < runnerlaydata.Count())
                                            {
                                                var pricesize = new PriceSize();
                                                pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1].Trim());
                                                pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1].Trim()) * Convert.ToDouble(marketbook.PoundRate));
                                                pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                                pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j].Trim())).ToString("F2"));

                                                lstpricelist.Add(pricesize);
                                                j = j + 4;

                                            }

                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                            else
                            {
                                for (int j = 0; j < runnerlaydata.Count();)
                                {
                                    if (j < runnerlaydata.Count())
                                    {
                                        var pricesize = new PriceSize();
                                        pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1].Trim());
                                        pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1].Trim()) * Convert.ToDouble(marketbook.PoundRate));
                                        pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                        pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j].Trim())).ToString("F2"));

                                        lstpricelist.Add(pricesize);
                                        j = j + 4;

                                    }

                                }
                            }
                        }
                        else
                        {
                            for (int ii = 0; ii < 3; ii++)
                            {
                                var pricesize = new PriceSize();
                                pricesize.OrignalSize = 0;
                                pricesize.Size = 0;
                                pricesize.SizeStr = "0";
                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                        runner.ExchangePrices.AvailableToLay = lstpricelist;
                        lstRunners.Add(runner);
                    }
                }


                marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);

                double lastback = 0;
                double lastbackSize = 0;
                double lastLaySize = 0;
                double lastlay = 0;

                if (marketbook.MarketStatusstr != "Suspended")
                {

                    double favBack = marketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price;
                    string selectionIDfav = marketbook.Runners[0].SelectionId;
                    foreach (var favoriteitem in marketbook.Runners)
                    {
                        if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Count > 0)
                            if (marketbook.MainSportsname.Contains("Racing"))
                            {
                                if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack && favoriteitem.ExchangePrices.AvailableToBack[0].Price > 0)
                                {
                                    favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                    selectionIDfav = favoriteitem.SelectionId;
                                }
                            }
                            else
                            {
                                if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack)
                                {
                                    favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                    selectionIDfav = favoriteitem.SelectionId;
                                }
                            }

                    }
                    if (marketbook.MarketStatusstr == "Closed")
                    {
                        var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER");
                        if (resultsfav != null && resultsfav.Count() > 0)
                        {
                            selectionIDfav = resultsfav.FirstOrDefault().SelectionId;
                        }
                    }
                    var favoriteteam = marketbook.Runners.Where(ii => ii.SelectionId == selectionIDfav).LastOrDefault();
                    string selectionname = favoriteteam.RunnerName;
                    if (favoriteteam.ExchangePrices.AvailableToBack.Count > 0)
                    {
                        lastback = favoriteteam.ExchangePrices.AvailableToBack[0].Price;
                        lastbackSize = favoriteteam.ExchangePrices.AvailableToBack[0].Size;


                    }
                    if (favoriteteam.ExchangePrices.AvailableToLay.Count > 0)
                    {

                        lastLaySize = favoriteteam.ExchangePrices.AvailableToLay[0].Size;
                        lastlay = favoriteteam.ExchangePrices.AvailableToLay[0].Price;

                    }
                    marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                    marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                    marketbook.FavoriteSelectionName = selectionname;
                    marketbook.FavoriteBackSize = lastbackSize.ToString();
                    marketbook.FavoriteLaySize = lastLaySize.ToString();
                    marketbook.FavoriteID = selectionIDfav;
                }
                else
                {
                    marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                    marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                    marketbook.FavoriteSelectionName = "";
                    marketbook.FavoriteBackSize = lastbackSize.ToString();
                    marketbook.FavoriteLaySize = lastLaySize.ToString();
                    marketbook.FavoriteID = "0";
                }
                return marketbook;

            }
            else
            {
                return new MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBF123(SampleResponse1 BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory)
        {
            try
            {



                if (1 == 1)
                {
                    var marketbook = new MarketBook();
                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    if (sheetname.Contains("(US)") && MainSportsCategory.Contains("Horse Racing"))
                    {
                        marketbook.PoundRate = Convert.ToDecimal(ConfigurationManager.AppSettings["PoundRateForUS"]);
                    }
                    else
                    {
                        marketbook.PoundRate = Convert.ToDecimal(ConfigurationManager.AppSettings["PoundRateForOThers"]);
                    }
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                   
                    DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
                    DateTime CurrentDate = DateTime.Now;
                    TimeSpan remainingdays = (CurrentDate - OpenDate);
                    if (OpenDate < CurrentDate)
                    {
                        marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                    }
                    else
                    {
                        marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                    }

                    if (BFMarketbook.Inplay == true && BFMarketbook.Status.ToString() == "OPEN")
                    {

                        marketbook.MarketStatusstr = "In Play";
                        marketbook.PoundRate = 50;
                    }
                    else
                    {
                        if (BFMarketbook.Status.ToString() == "CLOSED")
                        {
                            marketbook.MarketStatusstr = "Closed";
                        }
                        else
                        {
                            if (BFMarketbook.Status.ToString() == "SUSPENDED")
                            {
                                marketbook.MarketStatusstr = "Suspended";
                            }
                            else
                            {
                                marketbook.MarketStatusstr = "Active";
                            }

                        }

                    }

                    List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                    List<SP_MarketCatalogueSelections_Get_Result> selectionnames = new List<SP_MarketCatalogueSelections_Get_Result>();
                    try
                    {
                        //  selectionnames = objUserServices.GetSelectionNamesbyMarketID(marketbook.MarketId);
                    }
                    catch (System.Exception ex)
                    {

                    }

                    foreach (var runneritem in BFMarketbook.Runners)
                    {
                        var runner = new ExternalAPI.TO.Runner();
                        try
                        {
                            if (selectionnames.Count > 0)
                            {
                                runner.RunnerName = selectionnames.Where(item2 => item2.SelectionID == runneritem.SelectionId.ToString()).FirstOrDefault().SelectionName;
                            }

                        }
                        catch (System.Exception ex)
                        {

                        }


                        runner.Handicap = runneritem.Handicap;
                        runner.StatusStr = runneritem.Status.ToString();
                        runner.SelectionId = runneritem.SelectionId.ToString();
                        runner.LastPriceTraded = runneritem.LastPriceTraded;
                        var lstpricelist = new List<PriceSize>();
                        if (runneritem.Ex.AvailableToBack != null && runneritem.Ex.AvailableToBack.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {
                                try
                                {


                                    if (runneritem.Ex.AvailableToBack[0].Price.ToString().Contains("."))
                                    {
                                        foreach (var backitems in runneritem.Ex.AvailableToLay)
                                        {
                                            var pricesize = new PriceSize();
                                            pricesize.OrignalSize = backitems.Size;
                                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var backitems in runneritem.Ex.AvailableToBack)
                                        {
                                            var pricesize = new PriceSize();
                                            pricesize.OrignalSize = backitems.Size;
                                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = backitems.Price;

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                                //foreach (var backitems in runneritem.Ex.AvailableToLay)
                                //{
                                //    var pricesize = new PriceSize();

                                //    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

                                //    pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                //    lstpricelist.Add(pricesize);
                                //}
                            }
                            else
                            {
                                foreach (var backitems in runneritem.Ex.AvailableToBack)
                                {
                                    var pricesize = new PriceSize();
                                    pricesize.OrignalSize = backitems.Size;
                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }

                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new PriceSize();
                                pricesize.OrignalSize = 0;
                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices = new ExchangePrices();
                        runner.ExchangePrices.AvailableToBack = lstpricelist;
                        lstpricelist = new List<PriceSize>();
                        if (runneritem.Ex.AvailableToLay != null && runneritem.Ex.AvailableToLay.Count() > 0)
                        {
                            if (BFMarketbook.Runners.Count() == 1)
                            {

                                if (runneritem.Ex.AvailableToLay[0].Price.ToString().Contains("."))
                                {
                                    foreach (var backitems in runneritem.Ex.AvailableToBack)
                                    {
                                        var pricesize = new PriceSize();
                                        pricesize.OrignalSize = backitems.Size;
                                        pricesize.Size = Convert.ToInt64((backitems.Size) * Convert.ToDouble(marketbook.PoundRate));
                                        pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                        pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                                else
                                {
                                    foreach (var backitems in runneritem.Ex.AvailableToLay)
                                    {
                                        var pricesize = new PriceSize();
                                        pricesize.OrignalSize = backitems.Size;
                                        pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                        pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                        pricesize.Price = backitems.Price;

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                            }
                            else
                            {
                                foreach (var backitems in runneritem.Ex.AvailableToLay)
                                {
                                    var pricesize = new PriceSize();
                                    pricesize.OrignalSize = backitems.Size;
                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = backitems.Price;

                                    lstpricelist.Add(pricesize);
                                }
                            }

                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                var pricesize = new PriceSize();
                                pricesize.OrignalSize = 0;
                                pricesize.Size = 0;

                                pricesize.Price = 0;

                                lstpricelist.Add(pricesize);
                            }
                        }

                        runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                        runner.ExchangePrices.AvailableToLay = lstpricelist;
                        lstRunners.Add(runner);
                    }


                    marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);

                    double lastback = 0;
                    double lastbackSize = 0;
                    double lastLaySize = 0;
                    double lastlay = 0;

                    if (marketbook.MarketStatusstr != "Suspended")
                    {

                        double favBack = marketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price;
                        string selectionIDfav = marketbook.Runners[0].SelectionId;
                        foreach (var favoriteitem in marketbook.Runners)
                        {
                            if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Count > 0)
                                if (marketbook.MainSportsname.Contains("Racing"))
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack && favoriteitem.ExchangePrices.AvailableToBack[0].Price > 0)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }
                                else
                                {
                                    if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack)
                                    {
                                        favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
                                        selectionIDfav = favoriteitem.SelectionId;
                                    }
                                }

                        }
                        if (marketbook.MarketStatusstr == "Closed")
                        {
                            var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER").ToList();
                            if (resultsfav != null && resultsfav.Count() > 0)
                            {
                                selectionIDfav = resultsfav.FirstOrDefault().SelectionId;
                            }
                        }
                        var favoriteteam = marketbook.Runners.Where(ii => ii.SelectionId == selectionIDfav).LastOrDefault();
                        string selectionname = favoriteteam.RunnerName;
                        if (favoriteteam.ExchangePrices.AvailableToBack.Count > 0)
                        {
                            lastback = favoriteteam.ExchangePrices.AvailableToBack[0].Price;
                            lastbackSize = favoriteteam.ExchangePrices.AvailableToBack[0].Size;


                        }
                        if (favoriteteam.ExchangePrices.AvailableToLay.Count > 0)
                        {

                            lastLaySize = favoriteteam.ExchangePrices.AvailableToLay[0].Size;
                            lastlay = favoriteteam.ExchangePrices.AvailableToLay[0].Price;

                        }
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = selectionname;
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = selectionIDfav;
                    }
                    else
                    {
                        marketbook.FavoriteBack = (lastback - 1).ToString("F2");
                        marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
                        marketbook.FavoriteSelectionName = "";
                        marketbook.FavoriteBackSize = lastbackSize.ToString();
                        marketbook.FavoriteLaySize = lastLaySize.ToString();
                        marketbook.FavoriteID = "0";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteBack) < 0)
                    {
                        marketbook.FavoriteBack = "0";
                        marketbook.FavoriteBackSize = "0";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteLay) < 0)
                    {
                        marketbook.FavoriteLay = "0";
                        marketbook.FavoriteLaySize = "0";
                    }
                    return marketbook;

                }
                else
                {
                    return new MarketBook();
                }
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                return new MarketBook();
            }

        }



       
        public class Cricscore
        {
            public string de { get; set; }
            public int id { get; set; }
            public string si { get; set; }
        }
        public List<ExternalAPI.TO.MarketBook> GetMarketDatabyIDResultsOnly(string marketID)
        {
            try
            {
                var marketbooks = new List<MarketBook>();
               // string url = "http://www.betfair.com/www/sports/exchange/readonly/v1/bymarket?alt=json&currencyCode=GBP&locale=en_GB&marketIds=" + marketIDs + "&rollupLimit=4&rollupModel=STAKE&types=MARKET_STATE,RUNNER_STATE,RUNNER_EXCHANGE_PRICES_BEST,RUNNER_DESCRIPTION";

                string url = "http://www.betfair.com/www/sports/exchange/readonly/v1/bymarket?alt=json&currencyCode=GBP&locale=en_GB&marketIds=" + marketID + "&rollupLimit=4&rollupModel=STAKE&types=MARKET_STATE,RUNNER_STATE,RUNNER_EXCHANGE_PRICES_BEST,RUNNER_DESCRIPTION";
                string JsonResultsArr = "";
                try
                {
                    if (APIConfigforResults.SessionKey == "")
                    {
                        APIConfigforResults.GetSessionFromBeftair();
                    }
                    using (var client = new WebClient())
                    {
                        client.Headers.Add(HttpRequestHeader.Cookie, "ssoid=" + APIConfigforResults.SessionKey.ToString());

                     JsonResultsArr = client.DownloadString(url);

                    }

                }
                catch (System.Exception ex)
                {
                    //APIConfigforResults.GetSessionFromBeftair();
                    using (var client = new WebClient())
                    {
                        //client.Headers.Add(HttpRequestHeader.Cookie, "ssoid=" + APIConfigforResults.SessionKey.ToString());

                        JsonResultsArr = client.DownloadString(url);
                    }
                }

                //string JsonResultsArr = "";
                //try
                //{

                //    using (var client = new WebClient())
                //    {


                //        JsonResultsArr = client.DownloadString(url);
                //    }
                //}
                //catch (System.Exception ex)
                //{
                //    APIConfig.LogError(ex);
                //    //using (var client = new WebClient())
                //    //{


                //    //    JsonResultsArr = client.DownloadString(url);
                //    //}
                //}
                //JsonResultsArr = dbEntities.SP_OddsData_GetData().FirstOrDefault();
                var marketbook1 = new RootObject();
                marketbook1 = JsonConvert.DeserializeObject<RootObject>(JsonResultsArr);
                MarketNode marketbookorignal = marketbook1.eventTypes.SelectMany(eventtype => eventtype.eventNodes)
                            .SelectMany(eventnode => eventnode.marketNodes).FirstOrDefault(marketnode => marketnode.marketId == marketID);

                var marketbook = new MarketBook();

                marketbook.MarketId = marketbookorignal.marketId;
                marketbook.OpenDate = marketbookorignal.OpenDate;
                marketbook.NumberOfWinners = marketbookorignal.state.numberOfWinners;
                //INACTIVE, OPEN, SUSPENDED, CLOSED
                if (marketbookorignal.state.status == "OPEN")
                {
                    marketbook.Status = MarketStatus.OPEN;
                }
                else
                {
                    if (marketbookorignal.state.status == "SUSPENDED")
                    {
                        marketbook.Status = MarketStatus.SUSPENDED;
                    }
                    else
                    {
                        if (marketbookorignal.state.status == "CLOSED")
                        {
                            marketbook.Status = MarketStatus.CLOSED;
                        }
                        else
                        {
                            if (marketbookorignal.state.status == "INACTIVE")
                            {
                                marketbook.Status = MarketStatus.INACTIVE;
                            }
                        }
                    }
                }
                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                foreach (var runneritem in marketbookorignal.runners)
                {
                    // ACTIVE, WINNER, LOSER, REMOVED_VACANT, REMOVED
                    var runner = new ExternalAPI.TO.Runner();
                    runner.SelectionId = runneritem.selectionId.ToString();
                    runner.RunnerName = runneritem.description.runnerName;
                    runner.AdjustmentFactor = runneritem.state.adjustmentFactor;
                    if (runneritem.state.status == "WINNER")
                    {
                        runner.Status = RunnerStatus.WINNER;
                    }
                    else
                    {
                        if (runneritem.state.status == "ACTIVE")
                        {
                            runner.Status = RunnerStatus.ACTIVE;
                        }
                        else
                        {
                            if (runneritem.state.status == "LOSER")
                            {
                                runner.Status = RunnerStatus.LOSER;
                            }
                            else
                            {
                                if (runneritem.state.status == "REMOVED_VACANT")
                                {
                                    runner.Status = RunnerStatus.REMOVED_VACANT;
                                }
                                else
                                {
                                    if (runneritem.state.status == "REMOVED")
                                    {
                                        runner.Status = RunnerStatus.REMOVED;
                                    }
                                }
                            }
                        }
                    }
                    lstRunners.Add(runner);
                }


                marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);



                marketbooks.Add(marketbook);


                return marketbooks;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                var marketbooks = new List<MarketBook>();
                return marketbooks;
            }
        }

        public IList<MarketBook> updatemarketstatus(IList<MarketBook> marketbooks, int UserID, int userTypeID)
        {
            try
            {

                if (UserID > 0)
                {
                    if (marketbooks.Count > 0)
                    {
                      
                        foreach (var item in marketbooks)
                        {

                          
                            if (userTypeID == 3)
                            {

                               
                                var lstUserBet = dbEntities.SP_UserBets_GetDatabyUserID(UserID).ToList<SP_UserBets_GetDatabyUserID_Result>();
                                //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
                                List<SP_UserBets_GetDatabyUserID_Result> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == item.MarketId).ToList();
                                item.DebitCredit = ceckProfitandLoss(item, lstUserBets);
                                foreach (var runner in item.Runners)
                                {
                                    runner.ProfitandLoss = Convert.ToInt64(item.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - item.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                }
                                item.UserBetsEndUser = ConverttoJSONString(lstUserBet);
                            }
                            if (userTypeID == 2)
                            {
                                item.MarketBookName = dbEntities.SP_UserMarket_GetEventNamebyMarketID(item.MarketId).FirstOrDefault().EventName;
                                var lstUserBet = dbEntities.SP_UserBets_GetDatabyAgentID(UserID).ToList<SP_UserBets_GetDatabyAgentID_Result>();

                                List<SP_UserBets_GetDatabyAgentID_Result> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == item.MarketId).ToList();

                                var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                                foreach (var userid in lstUsers)
                                {
                                    List<SP_UserBets_GetDatabyAgentID_Result> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();

                                    item.DebitCredit = ceckProfitandLossAgent(item, lstuserbet);
                                    foreach (var runner in item.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(item.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - item.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        var agentrate = Crypto.Decrypt(objUserServices.GetAgentRate(Convert.ToInt32(userid.UserID)));
                                        decimal profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                    }


                                }

                                item.UserBetsAgent = ConverttoJSONString(lstUserBet.Take(10).ToList());

                            }
                            if (userTypeID == 1)
                            {

                                item.MarketBookName = dbEntities.SP_UserMarket_GetEventNamebyMarketID(item.MarketId).FirstOrDefault().EventName;
                                List<SP_UserBets_GetDataForAdmin_Result> lstUserBet = dbEntities.SP_UserBets_GetDataForAdmin().ToList<SP_UserBets_GetDataForAdmin_Result>();
                                //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
                                List<SP_UserBets_GetDataForAdmin_Result> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == item.MarketId).ToList();

                                var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                                foreach (var userid in lstUsers)
                                {
                                    List<SP_UserBets_GetDataForAdmin_Result> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();

                                    item.DebitCredit = ceckProfitandLossAdmin(item, lstuserbet);
                                    foreach (var runner in item.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(item.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - item.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        var agentrate = Crypto.Decrypt(objUserServices.GetAgentRate(Convert.ToInt32(userid.UserID)));
                                        decimal adminrate = 100 - Convert.ToDecimal(agentrate);
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                    }


                                }
                                //var lstUserBetnew = lstUserBet.Take(10).ToList();
                                item.UserBetsAdmin = ConverttoJSONString(lstUserBet.Take(10).ToList());

                            }

                            //item.FavoriteBack = (lastback - 1).ToString("F2");
                            //item.FavoriteLay = (lastlay - 1).ToString("F2");
                            //item.FavoriteSelectionName = selectionname;
                            //item.FavoriteBackSize = lastbackSize.ToString();
                            //item.FavoriteLaySize = lastLaySize.ToString();

                        }


                    }

                }
                return marketbooks;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                List<MarketBook> marketbooks1 = new List<MarketBook>();
                return marketbooks1;
            }
        }
        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossAgent(ExternalAPI.TO.MarketBook marketbookstatus, List<SP_UserBets_GetDatabyAgentID_Result> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);
            if (marketbookstatus.Runners[0].Handicap < 0)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }
                    else
                    {
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }


                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }
            else
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {

                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }
                    else
                    {
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }


        }
        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossAdmin(ExternalAPI.TO.MarketBook marketbookstatus, List<SP_UserBets_GetDataForAdmin_Result> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);
            if (marketbookstatus.Runners[0].Handicap < 0)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }
                    else
                    {
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }


                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }
            else
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {

                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }
                    else
                    {
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }


        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLoss(ExternalAPI.TO.MarketBook marketbookstatus, List<SP_UserBets_GetDatabyUserID_Result> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);
            if (marketbookstatus.Runners[0].Handicap < 0)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }
                    else
                    {
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }


                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }
            else
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {

                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }
                    else
                    {
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }


        }
        public IList<ExternalAPI.TO.MarketBook> listMarketBookALL(string[] ID, int UserID, int UserTypeID, string Password)
        {
            if (APIConfig.Url == "")
            {
                APIConfig.InitiateAPIConfiguration();
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
            }
            if (APIConfig.SessionKey == "")
            {
                APIConfig.GetSessionFromBeftair();
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
            }

            var marketFilter = new MarketFilter();
            //ISet<string> MarketBookIDs = new HashSet<string>();
            //foreach (var item in ID)
            //{
            //    MarketBookIDs.Add(item);
            //}


            //   marketFilter.MarketIds = MarketBookIDs;
            ISet<MarketProjection> marketProjections = new HashSet<MarketProjection>();
            marketProjections.Add(MarketProjection.RUNNER_METADATA);
            OrderProjection orderdata = new OrderProjection();
            orderdata = OrderProjection.ALL;
            ISet<PriceData> priceData = new HashSet<PriceData>();
            //ISet<MatchProjection> matchProjections = new HashSet<MatchProjection>();
            //matchProjections.Add(MatchProjection.ROLLED_UP_BY_PRICE);

            ////get all prices from the exchange
            priceData.Add(PriceData.EX_BEST_OFFERS);
            priceData.Add(PriceData.EX_TRADED);
            IList<string> marketIds = new List<string>();
            //  marketIds.Add(ID);
            foreach (var item in ID)
            {
                marketIds.Add(item);
            }
            var priceProjection = new PriceProjection();
            priceProjection.PriceData = priceData;
            priceProjection.Virtualise = true;
            var exbestoffers = new ExBestOffersOverrides();
            exbestoffers.BestPricesDepth = 3;

            priceProjection.ExBestOffersOverrides = exbestoffers;
            try

            {
                var marketbooks = client.listMarketBook(marketIds, priceProjection);
                marketbooks = updatemarketstatus(marketbooks, UserID, UserTypeID);
                return marketbooks;
            }
            catch (APINGException ex)
            {
                APIConfig.WriteErrorToDB(ex.ErrorCode.ToString());
                if (ex.ErrorCode == "INVALID_SESSION_INFORMATION")
                {

                    APIConfig.GetSessionFromBeftair();
                }
                // listMarketBookALL(ID);
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
                var marketbooks = client.listMarketBook(marketIds, priceProjection);
                marketbooks = updatemarketstatus(marketbooks, UserID, UserTypeID);
                return marketbooks;
            }





        }
        public string ConverttoJSONString(object result)
        {
            if (result != null)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(result.GetType());
                MemoryStream memoryStream = new MemoryStream();
                serializer.WriteObject(memoryStream, result);

                // Return the results serialized as JSON
                string json = Encoding.Default.GetString(memoryStream.ToArray());
                return json;
            }
            else
            {
                return "";
            }
        }
        public IList<MarketBook> listMarketBookLive(string[] ID, string Password)
        {

            if (APIConfig.UrlLive == "")
            {
                APIConfig.InitiateAPIConfigurationLive();
                clientLive = new JsonRpcClient(APIConfig.UrlLive, APIConfig.AppKeyLive, APIConfig.SessionKeyLive);
            }
            else
            {
                // clientLive = new JsonRpcClient(APIConfig.UrlLive, APIConfig.AppKeyLive, APIConfig.SessionKeyLive);
            }
            if (APIConfig.SessionKeyLive == "")
            {
                APIConfig.GetSessionFromBeftairLive();
                clientLive = new JsonRpcClient(APIConfig.UrlLive, APIConfig.AppKeyLive, APIConfig.SessionKeyLive);
            }


            var marketFilter = new MarketFilter();
            ISet<PriceData> priceData = new HashSet<PriceData>();


            //get all prices from the exchange
            priceData.Add(PriceData.EX_BEST_OFFERS);
            // priceData.Add(PriceData.EX_TRADED);
            IList<string> marketIds = new List<string>();
            foreach (var item in ID)
            {
                marketIds.Add(item);
            }

            var priceProjection = new PriceProjection();
            priceProjection.PriceData = priceData;
            priceProjection.Virtualise = true;
            //  var exbestoffers = new ExBestOffersOverrides();
            // exbestoffers.BestPricesDepth = 20;

            //  priceProjection.ExBestOffersOverrides = exbestoffers;
            try
            {
                var marketbooks = clientLive.listMarketBook(marketIds, priceProjection);


                return marketbooks;
            }
            catch (APINGException ex)
            {
                APIConfig.WriteErrorToDB(ex.ErrorCode.ToString());
                if (ex.ErrorCode == "INVALID_SESSION_INFORMATION" || ex.ErrorCode == "INVALID_APP_KEY"  || ex.ErrorCode == "NO_APP_KEY")
                {

                    APIConfig.GetSessionFromBeftairLive();
                    clientLive = new JsonRpcClient(APIConfig.UrlLive, APIConfig.AppKeyLive, APIConfig.SessionKeyLive);
                    var marketbooks = clientLive.listMarketBook(marketIds, priceProjection);

                    return marketbooks;
                }
                else
                {
                    return new List<MarketBook>();
                }

            }

        }
        public IList<MarketBook> listMarketBook(string ID, int UserID, int UserTypeID, string Password)
        {
            if (APIConfig.Url == "")
            {
                APIConfig.InitiateAPIConfiguration();
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
            }
            if (APIConfig.SessionKey == "")
            {
                APIConfig.GetSessionFromBeftair();
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
            }


            var marketFilter = new MarketFilter();
            //ISet<string> MarketBookIDs = new HashSet<string>();
            //foreach (var item in ID)
            //{
            //    MarketBookIDs.Add(item);
            //}


            //   marketFilter.MarketIds = MarketBookIDs;

            ISet<PriceData> priceData = new HashSet<PriceData>();


            //get all prices from the exchange
            priceData.Add(PriceData.EX_BEST_OFFERS);
            priceData.Add(PriceData.EX_TRADED);
            IList<string> marketIds = new List<string>();
            marketIds.Add(ID);
            //foreach (var item in ID)
            //{
            //    marketIds.Add(item);
            //}
            var priceProjection = new PriceProjection();
            priceProjection.PriceData = priceData;
            priceProjection.Virtualise = true;
            var exbestoffers = new ExBestOffersOverrides();
            exbestoffers.BestPricesDepth = 20;

            priceProjection.ExBestOffersOverrides = exbestoffers;
            try
            {
                var marketbooks = client.listMarketBook(marketIds, priceProjection);
                updatemarketstatus(marketbooks, UserID, UserTypeID);
                //if (marketbooks.Count > 0)
                //{
                //    APIConfigService objApiconfig = new APIConfigService();
                //    marketbooks[0].PoundRate = Convert.ToDecimal(Crypto.Decrypt(objApiconfig.GetPoundRate()));
                //}
                return marketbooks;
            }
            catch (APINGException ex)
            {
                APIConfig.WriteErrorToDB(ex.ErrorCode.ToString());
                if (ex.ErrorCode == "INVALID_SESSION_INFORMATION")
                {

                    APIConfig.GetSessionFromBeftair();
                }
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
                var marketbooks = client.listMarketBook(marketIds, priceProjection);
                updatemarketstatus(marketbooks, UserID, UserTypeID);
                return marketbooks;
            }

        }

        public IList<MarketCatalogue> listMarketCatalogue(string ID, List<string> lstMarketCatalogues, bool isInPlay, string Password)
        {
            if (APIConfig.Url == "")
            {
                APIConfig.InitiateAPIConfiguration();
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
            }

            var marketFilter = new MarketFilter();
            ISet<string> EventIDs = new HashSet<string>();
            EventIDs.Add(ID);


            marketFilter.EventIds = EventIDs;
            if (lstMarketCatalogues.Count > 0)
            {
                ISet<string> MarketCatalogueIDs = new HashSet<string>();
                foreach (var item in lstMarketCatalogues)
                {
                    MarketCatalogueIDs.Add(item);
                }
                marketFilter.MarketIds = MarketCatalogueIDs;
            }
            var lstEventTypes = client.listEventTypes(marketFilter);
            if (lstEventTypes.Count > 0)
            {
                if (lstEventTypes[0].EventType.Name.Contains("Racing") && ID == "7")
                {
                    ISet<string> MarketTypesstr = new HashSet<string>();
                    MarketTypesstr.Add("WIN");
                    MarketTypesstr.Add("PLACE");
                    marketFilter.MarketTypeCodes = MarketTypesstr;
                }
                if (lstEventTypes[0].EventType.Name.Contains("Racing") && ID=="4339" )
                {
                    ISet<string> MarketTypesstr = new HashSet<string>();
                    MarketTypesstr.Add("WIN");
                   // MarketTypesstr.Add("PLACE");
                    marketFilter.MarketTypeCodes = MarketTypesstr;
                }
                if (lstEventTypes[0].EventType.Name.Contains("Soccer") || lstEventTypes[0].EventType.Name.Contains("Tennis"))
                {
                    ISet<string> MarketTypesstr = new HashSet<string>();
                    MarketTypesstr.Add("MATCH_ODDS");
                   // MarketTypesstr.Add("OVER_UNDER_05");
                    MarketTypesstr.Add("OVER_UNDER_15");
                    MarketTypesstr.Add("OVER_UNDER_25");
                    MarketTypesstr.Add("OVER_UNDER_35");
                    //MarketTypesstr.Add("OVER_UNDER_45");                                    
                    marketFilter.MarketTypeCodes = MarketTypesstr;
                }
            }


            ISet<MarketProjection> marketProjections = new HashSet<MarketProjection>();
            marketProjections.Add(MarketProjection.RUNNER_METADATA);
            marketProjections.Add(MarketProjection.MARKET_DESCRIPTION);
            marketProjections.Add(MarketProjection.EVENT);
            var marketSort = MarketSort.FIRST_TO_START;
            var maxResults = "100";
            if (isInPlay == true)
            {
                marketFilter.InPlayOnly = true;
            }
            try
            {
                var marketCatalogues = client.listMarketCatalogue(marketFilter, marketProjections, marketSort, maxResults);

                return marketCatalogues;
            }
            catch (APINGException ex)
            {
                APIConfig.WriteErrorToDB(ex.ErrorCode.ToString());
                if (ex.ErrorCode == "INVALID_SESSION_INFORMATION")
                {

                    APIConfig.GetSessionFromBeftair();
                }
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
                var marketCatalogues = client.listMarketCatalogue(marketFilter, marketProjections, marketSort, maxResults);
                return marketCatalogues;
            }

        }

        public IList<EventTypeResult> listEventTypes(bool isInPlay, string Password)
        {
            if (APIConfig.Url == "")
            {
                APIConfig.InitiateAPIConfiguration();
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
            }

            var marketFilter = new MarketFilter();
            try
            {
                if (isInPlay == true)
                {
                    marketFilter.InPlayOnly = true;
                }
                // marketFilter.TextQuery = "Soccer, Tennis,Cricket,Horse Racing,Greyhound Racing";
                var eventTypes = client.listEventTypes(marketFilter);
                return eventTypes;
            }
            catch (APINGException ex)
            {
                APIConfig.WriteErrorToDB(ex.ErrorCode.ToString());
                if (ex.ErrorCode == "INVALID_SESSION_INFORMATION")
                {

                    APIConfig.GetSessionFromBeftair();
                }



                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
                var eventTypes = client.listEventTypes(marketFilter);
                return eventTypes;
            }

        }
        public IList<EventTypeResult> listEventTypesWithMarketFilter(bool isInPlay, string Password)
        {
            if (APIConfig.Url == "")
            {
                APIConfig.InitiateAPIConfiguration();
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
            }

            var marketFilter = new MarketFilter();
            try
            {
                if (isInPlay == true)
                {
                    marketFilter.InPlayOnly = true;
                }
                ISet<string> eventypeIds = new HashSet<string>();
                eventypeIds.Add("1");
                eventypeIds.Add("2");
                marketFilter.EventTypeIds = eventypeIds;
                // marketFilter.TextQuery = "Soccer, Tennis,Cricket,Horse Racing,Greyhound Racing";
                var eventTypes = client.listEventTypes(marketFilter);
                return eventTypes;
            }
            catch (APINGException ex)
            {
                APIConfig.WriteErrorToDB(ex.ErrorCode.ToString());
                if (ex.ErrorCode == "INVALID_SESSION_INFORMATION")
                {

                    APIConfig.GetSessionFromBeftair();
                }



                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
                var eventTypes = client.listEventTypes(marketFilter);
                return eventTypes;
            }

        }
        public IList<CompetitionResult> listCompetitions(string ID, bool isInPlay, string Password)
        {

            if (APIConfig.Url == "")
            {
                APIConfig.InitiateAPIConfiguration();
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
            }

            var marketFilter = new MarketFilter();
            try
            {
                if (isInPlay == true)
                {
                    marketFilter.InPlayOnly = true;
                }
                ISet<string> eventypeIds = new HashSet<string>();
                eventypeIds.Add(ID);

                marketFilter.EventTypeIds = eventypeIds;
                if (ID == "7" || ID == "4339")
                {
                    Competition objCompetition = new Competition();
                    objCompetition.Id = "7";
                    objCompetition.Name = "Horse Racing";
                    CompetitionResult objCompetitionResult = new CompetitionResult();
                    objCompetitionResult.Competition = new Competition();
                    objCompetitionResult.Competition = objCompetition;
                    List<CompetitionResult> lstCompetiton = new List<CompetitionResult>();
                    lstCompetiton.Add(objCompetitionResult);
                    return lstCompetiton;
                }               
                    else
                    {
                        var competitions = client.listCompetitions(marketFilter);
                        return competitions;
                    }             
            }
            catch (APINGException ex)
            {
                APIConfig.WriteErrorToDB(ex.ErrorCode.ToString());
                if (ex.ErrorCode == "INVALID_SESSION_INFORMATION")
                {
                    APIConfig.GetSessionFromBeftair();
                }
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
                var competitions = client.listCompetitions(marketFilter);
                return competitions;
            }
        }
        public IList<EventResult> listEvents(string ID, bool isInPlay, string Password)
        {
            if (APIConfig.Url == "")
            {
                APIConfig.InitiateAPIConfiguration();
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
            }
            var marketFilter = new MarketFilter();
            ISet<string> competitionIDs = new HashSet<string>();
            competitionIDs.Add(ID);
            try
            {
                if (isInPlay == true)
                {
                    marketFilter.InPlayOnly = true;
                }
                if (ID == "7" || ID == "4339")
                {
                    marketFilter.EventTypeIds = competitionIDs;
                    ISet<string> MarketTypesstr = new HashSet<string>();
                    MarketTypesstr.Add("WIN");
                    //MarketTypesstr.Add("PLACE");
                    marketFilter.MarketTypeCodes = MarketTypesstr;
                }                               
                 else
                  {
                        marketFilter.CompetitionIds = competitionIDs;
                  }
                  
                var events = client.listEvents(marketFilter);

                return events;
            }
            catch (APINGException ex)
            {
                APIConfig.WriteErrorToDB(ex.ErrorCode.ToString());
                if (ex.ErrorCode == "INVALID_SESSION_INFORMATION" || ex.ErrorCode == "INVALID_APP_KEY")
                {

                    APIConfig.GetSessionFromBeftair();
                }
                client = new JsonRpcClient(APIConfig.Url, APIConfig.AppKey, APIConfig.SessionKey);
                var events = client.listEvents(marketFilter);

                return events;
            }

        }

        public string GetAllMarketsString(string marketID)
        {
            try
            {
                string[] marketIDs = marketID.Split(',');
                List<ExternalAPI.TO.MarketBook> lstClientMarkes = new List<ExternalAPI.TO.MarketBook>();

                if (APIConfig.LiveCricketMarketBooks != null)
                {
                    // var admins = APIConfig.BFMarketBooks.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
                    foreach (var marketitem in marketIDs)
                    {
                        var currmarketbook = APIConfig.LiveCricketMarketBooks.Where(item => item.MarketId == marketitem).FirstOrDefault();
                        if (currmarketbook != null)
                        {
                            lstClientMarkes.Add(currmarketbook);
                        }
                    }
                }

                return ConverttoJSONString(lstClientMarkes);
            }
            catch (System.Exception ex)
            {
                return "";

            }
        }
        public string GetAllMarketsFancyString(string marketID)
        {
            string[] marketIDs = marketID.Split(',');
            List<ExternalAPI.TO.MarketBook> lstClientMarkes = new List<ExternalAPI.TO.MarketBook>();

            if (APIConfig.LiveCricketMarketBooksFancy != null)
            {
                // var admins = APIConfig.BFMarketBooks.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
                foreach (var marketitem in marketIDs)
                {
                   // var currmarketbook = APIConfig.LiveCricketMarketBooks.ToList().Where(item => item.MarketId == marketitem).FirstOrDefault();
                    var currmarketbook = APIConfig.LiveCricketMarketBooksFancy.ToList().Where(item => item.MarketId == marketitem).FirstOrDefault();
                    if (currmarketbook != null)
                    {
                        lstClientMarkes.Add(currmarketbook);
                    }
                }
            }
            return ConverttoJSONString(lstClientMarkes);
        }
    }
}
