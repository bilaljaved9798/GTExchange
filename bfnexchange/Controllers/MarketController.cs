using ExternalAPI;
using ExternalAPI.TO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using bfnexchange.UsersServiceReference;
using bfnexchange.Models;
using Newtonsoft.Json;
using bfnexchange.BettingServiceReference;
using System.IO;
using System.Data;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.SessionState;
using bfnexchange.Services;
using TestBF;
using Newtonsoft.Json.Linq;
using System.Net;

namespace bfnexchange.Controllers
{

    public class MarketController : Controller
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceClient objBettingClient = new BettingServiceClient();

        public static wsnew ws1 = new wsnew();

        public static wsnew ws2 = new wsnew();

        public static wsnew ws4 = new wsnew();

        public static wsnew ws7 = new wsnew();

        public static wsnew ws0 = new wsnew();
        //public static wsnew ws0t = new wsnew();
        public static wsnew ws4339 = new wsnew();
        public static wsnew wsFancy = new wsnew();

        private wsnew wsBFMatch = new wsnew();

       
        public void SetURLsData()
        {
            LoggedinUserDetail.URLsData = JsonConvert.DeserializeObject<List<bfnexchange.Services.DBModel.SP_URLsData_GetAllData_Result>>(objUsersServiceCleint.GetURLsData());
            ws1.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Soccer").FirstOrDefault().URLForData;
            ws2.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Tennis").FirstOrDefault().URLForData;
            ws4.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().URLForData;
            ws7.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Horse Racing").FirstOrDefault().URLForData;
            ws4339.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "GreyHound Racing").FirstOrDefault().URLForData;
            wsFancy.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Fancy").FirstOrDefault().URLForData;

            ws0.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Other").FirstOrDefault().URLForData;
           // ws0t.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "BP").FirstOrDefault().URLForData;
            LoggedinUserDetail.SecurityCode = LoggedinUserDetail.URLsData.FirstOrDefault().Scd;
            LoggedinUserDetail.GetCricketDataFrom = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().GetDataFrom;
        }
        public static string strWsMatch = "";
        public ExternalAPI.TO.MarketBook GetCurrentMarketBook123(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate)
        {
            try
            {
                ExternalAPI.TO.MarketBook marketbook;
                this.wsBFMatch.Url = strWsMatch;
                SampleResponse1[] objmarketbook2 = JsonConvert.DeserializeObject<SampleResponse1[]>(this.wsBFMatch.getMarketBook(marketid));

                marketbook = ConvertJsontoMarketObjectBF123(objmarketbook2[0], marketid, marketopendate, sheetname, MainSportsCategory);
                return marketbook;

            }
            catch (System.Exception ex)
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
               
        public ExternalAPI.TO.MarketBook GetCurrentMarketBook(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate)
        {
            try
            {

                if (LoggedinUserDetail.URLsData.Count == 0)
                {
                    SetURLsData();
                }
                string[] marketIds = new string[]
                   {
                    marketid
                   };
                if (LoggedinUserDetail.GetCricketDataFrom == "Live")
                {
                    var marketbook = objBettingClient.GetMarketDatabyID(marketIds, sheetname, marketopendate, MainSportsCategory, ConfigurationManager.AppSettings["PasswordForValidate"]);
                    if (marketbook.Count() > 0)
                    {
                        return marketbook[0];
                    }
                    else
                    {
                        return new ExternalAPI.TO.MarketBook();
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetCricketDataFrom == "BP" )
                    {
                        var marketbook = objBettingClient.GetMarketDatabyID(marketIds, sheetname, marketopendate, MainSportsCategory, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        if (marketbook.Count() > 0)
                        {
                            return marketbook[0];
                        }
                        else
                        {
                            return new ExternalAPI.TO.MarketBook();
                        }
                    }
                    else
                    {

                       var  marketbook = objBettingClient.GetMarketDatabyID(marketIds, sheetname, marketopendate, MainSportsCategory, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        if (marketbook.Count() > 0)
                        {
                            return marketbook[0];
                        }
                        else
                        {
                            return new ExternalAPI.TO.MarketBook();
                        }
                        IList<bfnexchange.wrBF.MarketBook> list;
                        if (MainSportsCategory == "Soccer")
                        {
                            list = ws1.GD(LoggedinUserDetail.SecurityCode, marketIds, 1);
                        }
                        else if (MainSportsCategory == "Tennis")
                        {
                            list = ws2.GD(LoggedinUserDetail.SecurityCode, marketIds, 2);
                        }
                        else if (MainSportsCategory == "Cricket")
                        {
                            list = ws4.GD(LoggedinUserDetail.SecurityCode, marketIds, 4);
                        }
                        else if (MainSportsCategory.Contains("Horse Racing"))
                        {
                            list = ws7.GD(LoggedinUserDetail.SecurityCode, marketIds, 7);
                        }
                        else if (MainSportsCategory.Contains("Greyhound Racing"))
                        {
                            list = ws4339.GD(LoggedinUserDetail.SecurityCode, marketIds, 13);
                        }
                        else
                        {
                            list = ws0.GD(LoggedinUserDetail.SecurityCode, marketIds, 0);                          
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
                }
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                return new ExternalAPI.TO.MarketBook();
            }
           }
    
        public ActionResult UserPosition(string marektbookID)
        {           

            var results = objUsersServiceCleint.GetSelectionNamesbyMarketID(marektbookID);
           
            List<MarketRunner> lstUsers = new List<MarketRunner>();

            foreach (var a in results)
            {
                //List<MarketRunner> reunner = new List<MarketRunner>();
                var reunner = new MarketRunner();
                reunner.selectionID = a.SelectionID;
                reunner.SelectionName = a.SelectionName;
                lstUsers.Add(reunner);
            }

            ViewBag.allrunner = lstUsers;
            ViewBag.marketid = marektbookID;
           
            return View();
        }

        public ExternalAPI.TO.MarketBook MarketBook1 = new ExternalAPI.TO.MarketBook();
        public string ShowPosition(string userid, string runnerid, string marketboodid)
        {
            long currentprofitandloss = 0;
                string[] marketIds = new string[]
                   {
                    marketboodid
                   };
               // var marketbooks = new List<MarketBook>();
                var marketbook = objBettingClient.GetMarketDatabyID(marketIds, "", DateTime.Now, "", ConfigurationManager.AppSettings["PasswordForValidate"]);

                    if (Convert.ToInt32(userid) > 0 && Convert.ToInt32(runnerid) > 0)
                    {
                        string userbets = objUsersServiceCleint.GetUserbetsbyUserID(Convert.ToInt32(userid), ConfigurationManager.AppSettings["PasswordForValidate"]);
                        List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(userbets);
                        if (lstUserBets.Count > 0)
                        {
                            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                            List<UserBets> lstUserBets1 = lstUserBets.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook[0].MarketId).ToList();

                            var currentusermarketbook = marketbook[0];
                            currentusermarketbook.DebitCredit = objUserBets.ceckProfitandLoss(currentusermarketbook, lstUserBets1);
                            if (currentusermarketbook.MarketBookName.Contains("To Be Placed"))
                            {
                                foreach (var runner in currentusermarketbook.Runners)
                                {
                                    runner.ProfitandLoss = Convert.ToInt64(currentusermarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                    runner.Loss = Convert.ToInt64(currentusermarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                }
                            }
                            else
                            {
                                foreach (var runner in currentusermarketbook.Runners)
                                {
                                    runner.ProfitandLoss = Convert.ToInt64(currentusermarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - currentusermarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                }
                            }
                             currentprofitandloss = currentusermarketbook.Runners.Where(item => item.SelectionId == runnerid.ToString()).FirstOrDefault().ProfitandLoss;
                            //if (currentprofitandloss >= 0)
                            //{
                            //    //lblCurrentPostitionUserPL1Agent.Foreground = Brushes.Green;
                            //}
                            //else
                            //{
                            //    //lblCurrentPostitionUserPL1Agent.Foreground = Brushes.Red;
                            //}
                                //lblCurrentPostitionUserPL1Agent.Content =
                                return currentprofitandloss.ToString();

                        }
                        else
                        {
                             return  "0";
                               
                        }
                    }
            return currentprofitandloss.ToString();

        }
        
        public void UpdateBettingAllowed(bool BettingAllow)
        {
            LoggedinUserDetail.CheckifUserLogin();

            if (LoggedinUserDetail.GetUserTypeID() == 8)
            {
                if (BettingAllow == true)
                {
                    //objUsersServiceCleint.UpdateBettingAllowed(marketid, "0");
                }

               // return "Agent Rate cannot greater than 50 %.";

            }
        }    
             
        //public ExternalAPI.TO.MarketBook GetCurrentMarketBook(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate)
        //{
        //    try
        //    {


        //        string[] marketIds = new string[]
        //            {
        //            marketid
        //            };
        //        IList<bfnexchange.wrBF.MarketBook> list;
        //        if (MainSportsCategory == "Soccer")
        //        {
        //            ws1.Url = "http://5.133.176.76/ws7/wsnew.asmx";
        //            list = ws1.GD("6456(^(647HD764fufy", marketIds, 1);
        //        }
        //        else if (MainSportsCategory == "Tennis")
        //        {
        //            ws2.Url = "http://5.133.176.76/ws7/wsnew.asmx";
        //            list = ws2.GD("6456(^(647HD764fufy", marketIds, 2);
        //        }
        //        else if (MainSportsCategory == "Cricket")
        //        {
        //            ws4.Url = "http://5.133.176.76/ws7/wsnew.asmx";
        //            list = ws4.GD("6456(^(647HD764fufy", marketIds, 4);
        //        }
        //        else if (MainSportsCategory.Contains("Racing"))
        //        {
        //            ws7.Url = "http://5.133.176.76/ws7/wsnew.asmx";
        //            list = ws7.GD("6456(^(647HD764fufy", marketIds, 7);
        //        }
        //        else
        //        {
        //            ws0.Url = "http://5.133.176.76/ws0/wsnew.asmx";
        //            list = ws0.GD("6456(^(647HD764fufy", marketIds, 0);
        //        }
        //        if (list.Count > 0)
        //        {
        //            ExternalAPI.TO.MarketBook objMarketbook = ConvertJsontoMarketObjectBF(list[0], marketid, marketopendate, sheetname, MainSportsCategory);
        //            return objMarketbook;
        //        }
        //        else
        //        {
        //            return new ExternalAPI.TO.MarketBook();
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        APIConfig.LogError(ex);
        //        return new ExternalAPI.TO.MarketBook();
        //    }
        //}

        public PartialViewResult LoadLiabalitybyMarket()
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            try
            {
                List<LiabalitybyMarket> lstLibalitybymrakets = new List<LiabalitybyMarket>();
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    //List<Models.TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "4339"));

                    // List<UserBets> lstUserBets = (List<UserBets>)Session["userbet"];
                    var lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                    List<UserBets> lstuserbetfancy = lstUserBets.Where(item => item.location == "9" || item.location == "8").ToList();
                    List<UserBets> lstuserbet = lstUserBets.Where(item => item.location != "9" && item.location != "8").ToList();
                    UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                    lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentUserbyMarkets(LoggedinUserDetail.GetUserID(), lstuserbet);
                    lstLibalitybymrakets.AddRange(objUserBets.GetLiabalityofCurrentUserbyMarketsfancy(LoggedinUserDetail.GetUserID(), lstuserbetfancy));
                    //return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
                    return PartialView("LiabalitybyMarket", lstLibalitybymrakets);

                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                        
                        string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                        var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(userbets);
                        UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                        lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentAgentbyMarkets(lstUserBet);

                        //return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
                        return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {


                            string userbets = objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]);
                            List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(userbets);
                            UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                            lstLibalitybymrakets = objUserBets.GetLiabalityofAdminbyMarkets(lstUserBet);

                            // return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);

                            return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                string userbets = objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                List<UserBetsforSuper> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforSuper>>(userbets);
                                UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                                lstLibalitybymrakets = objUserBets.GetLiabalityofSuperbyMarkets(lstUserBet);

                                //return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
                                return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                            }

                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 9)
                                {
                                    string userbets = objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                    List<UserBetsforSamiadmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforSamiadmin>>(userbets);
                                    UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                                    lstLibalitybymrakets = objUserBets.GetLiabalityofSamiadminbyMarkets(lstUserBet);

                                    //return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
                                    return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                                }
                                else
                                {
                                    // return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
                                    return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                                }
                            }
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                List<LiabalitybyMarket> lstLibalitybymrakets = new List<LiabalitybyMarket>();
                //return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
                return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
            }


        }
        
        public List<ExternalAPI.TO.MarketBook> GetMarketDatabyID(string[] marketID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory)
        {
            if (1 == 1)
            {
                try
                {
                    var marketbooks = new List<MarketBook>();
                    var marketbook = GetCurrentMarketBook(marketID[0], sheetname, MainSportsCategory, OrignalOpenDate);
                    // var marketbook = GetCurrentMarketBook123(marketID[0], sheetname, MainSportsCategory, OrignalOpenDate);
                    
                    if (marketbook.MarketId != null)
                    {
                        marketbooks.Add(marketbook);
                    }
                    return marketbooks;
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

        public MarketBook ConvertJsontoMarketObjectBF(bfnexchange.wrBF.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory)
        {
            try
            {



                if (1 == 1)
                {
                    var marketbook = new MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = objUsersServiceCleint.GetPoundRatebyUserID(LoggedinUserDetail.GetUserID());
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

                                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

                                            pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                    else
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

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

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

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

                                        pricesize.Size = Convert.ToInt64((backitems.Size) * Convert.ToDouble(marketbook.PoundRate));

                                        pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

                                        lstpricelist.Add(pricesize);
                                    }
                                }
                                else
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
                                    {
                                        var pricesize = new PriceSize();

                                        pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

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

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

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
                        var favoriteteam = marketbook.Runners.Where(ii => ii.SelectionId == selectionIDfav).FirstOrDefault();



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
                    marketbook.PoundRate = objUsersServiceCleint.GetPoundRatebyUserID(LoggedinUserDetail.GetUserID());
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

                    if (BFMarketbook.Inplay == true && BFMarketbook.Status.ToString() == "OPEN")
                    {

                        marketbook.MarketStatusstr = "In Play";
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
                        if (runneritem.Ex.AvailableToBack != null && runneritem.Ex.AvailableToBack.Count() > 0)
                        {
                            foreach (var backitems in runneritem.Ex.AvailableToBack)
                            {
                                var pricesize = new PriceSize();

                                pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

                                pricesize.Price = backitems.Price;

                                lstpricelist.Add(pricesize);
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

                        runner.ExchangePrices = new ExchangePrices();
                        runner.ExchangePrices.AvailableToBack = lstpricelist;
                        lstpricelist = new List<PriceSize>();
                        if (runneritem.Ex.AvailableToLay != null && runneritem.Ex.AvailableToLay.Count() > 0)
                        {
                            foreach (var backitems in runneritem.Ex.AvailableToLay)
                            {
                                var pricesize = new PriceSize();

                                pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

                                pricesize.Price = backitems.Price;

                                lstpricelist.Add(pricesize);
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
                            if (resultsfav != null)
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
                    marketbook.FavoriteSelectionName = marketbook.FavoriteSelectionName == null ? "" : marketbook.FavoriteSelectionName;
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

        
        public PartialViewResult Index()
        {
           
            if (LoggedinUserDetail.GetUserTypeID() == 1 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
            {
                ViewBag.backgrod = "#1D9BF0";
                ViewBag.color = "white";
            }
            
            LoggedinUserDetail.CheckifUserLogin();
            var marketFilter = new MarketFilter();

            if (LoggedinUserDetail.GetUserTypeID() != 1)
            {
                List<Models.EventType> lstClientlist = JsonConvert.DeserializeObject<List<Models.EventType>>(objUsersServiceCleint.GetEventTypeIDs(LoggedinUserDetail.GetUserID()));

                return PartialView("EventType", lstClientlist);
            }
            else
            {
                List<Models.EventType> lstClientlist = new List<Models.EventType>();


                return PartialView("EventType", lstClientlist);
            }



        }

        public PartialViewResult Competiion(string ID)
        {

            try
            {
                LoggedinUserDetail.CheckifUserLogin();


                if (LoggedinUserDetail.GetUserTypeID() != 1)
                {

                    List<Models.Competition> lstClientlist = JsonConvert.DeserializeObject<List<Models.Competition>>(objUsersServiceCleint.GetCompetitionIDs(ID.ToString(), LoggedinUserDetail.GetUserID()));
                    return PartialView("Competition", lstClientlist);

                }
                else
                {

                    List<Models.Competition> lstClientlist = new List<Models.Competition>();
                    return PartialView("Competition", lstClientlist);
                }

            }
            catch (System.Exception ex)
            {
                List<Models.Competition> lstClientlist = new List<Models.Competition>();

                return PartialView("Competition", lstClientlist);
            }
        }

        public PartialViewResult Events(string ID)
        {
            LoggedinUserDetail.CheckifUserLogin();

            if (LoggedinUserDetail.GetUserTypeID() != 1)
            {
                List<Models.Event> lstClientlist = JsonConvert.DeserializeObject<List<Models.Event>>(objUsersServiceCleint.GetEventsIDs(ID.ToString(), LoggedinUserDetail.GetUserID()));
                return PartialView("Events", lstClientlist);

            }
            else
            {
                List<Models.Event> lstClientlist = new List<Models.Event>();
                return PartialView("Events", lstClientlist);
            }

        }
        public PartialViewResult InPlayMatchesC()
        {
            
            if (LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9 || LoggedinUserDetail.GetUserTypeID() == 1)
            {
                ViewBag.backgrod = "#1D9BF0";
                ViewBag.color = "white";
                List<Models.TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "4"));
                lstTodayHorseRacing = lstTodayHorseRacing.Where(a => a.EventName != "Line v Markets").ToList();
             
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
            else
            {
          
                List<Models.TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "4"));
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }

        }
        public PartialViewResult InPlayMatchesF()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9 || LoggedinUserDetail.GetUserTypeID() == 1)
            {
                ViewBag.backgrod = "#1D9BF0";
                ViewBag.color = "white";
            }
            if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
            {
                

                List<Models.TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "1"));
                
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
            else
            {
                List<Models.TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "1"));
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }

        }
        public PartialViewResult InPlayMatchesT()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9 || LoggedinUserDetail.GetUserTypeID() == 1)
            {
                ViewBag.backgrod = "#1D9BF0";
                ViewBag.color = "white";
            }
            if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
            {
               

                List<Models.TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "2"));
      
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
            else
            {
                List<Models.TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "2"));
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }

        }
        public PartialViewResult TodayHorseRacing()
        {
             if (LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9 ||  LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    ViewBag.backgrod = "#1D9BF0";
                    ViewBag.color = "white";
                }
            LoggedinUserDetail.CheckifUserLogin();
            if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2)
            {
               
                List<Models.TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "7"));
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
            else
            {
                List<Models.TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "7")); ;
                TodayHorseRacing s = new Models.TodayHorseRacing();
                
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
        }
        public PartialViewResult TodayGreyHoundRacing()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9 || LoggedinUserDetail.GetUserTypeID() == 1)
            {
                ViewBag.backgrod = "#1D9BF0";
                ViewBag.color = "white";
            }
            LoggedinUserDetail.CheckifUserLogin();
            if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
            {
                List<Models.TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "4339"));
                lstTodayHorseRacing = lstTodayHorseRacing.Take(100).ToList();
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
            else
            {
                List<Models.TodayHorseRacing> lstTodayHorseRacing = new List<Models.TodayHorseRacing>();
                lstTodayHorseRacing = lstTodayHorseRacing.Take(100).ToList();
                return PartialView("TodayHorseRace", lstTodayHorseRacing);
            }
        }


        public PartialViewResult MarketCatalogue(string ID)
        {
            LoggedinUserDetail.CheckifUserLogin();
            //List<Models.MarketCatalgoue> lstClientlist = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketCatalogueIDs(ID.ToString(), LoggedinUserDetail.GetUserID()));

            //return PartialView("MarketCatalogue", lstClientlist);

            List<string> lstMarketCatalogueIDs = new List<string>();
            if (LoggedinUserDetail.GetUserTypeID() != 1)
            {
                List<Models.MarketCatalgoue> lstClientlist = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketCatalogueIDs(ID.ToString(), LoggedinUserDetail.GetUserID()));
                LoggedinUserDetail.InsertActivityLog(LoggedinUserDetail.GetUserID(), "View Market (" + ID.ToString() + ")");
                return PartialView("MarketCatalogue", lstClientlist);
            }
            else
            {
                List<Models.MarketCatalgoue> lstClientlist = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketCatalogueIDs(ID.ToString(), LoggedinUserDetail.GetUserID()));
                LoggedinUserDetail.InsertActivityLog(LoggedinUserDetail.GetUserID(), "View Market (" + ID.ToString() + ")");
                return PartialView("MarketCatalogue", lstClientlist);

            }




        }

        public string GetMarketDataforCheck(string ID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory, string SelectionID, string BetType, string LastSize)
        {

            if (BetType == "back")
            {

                var newsize = "";
                for (int i = 0; i < 3; i++)
                {
                    var marketbooks = GetCurrentMarketBook(ID, sheetname, MainSportsCategory, OrignalOpenDate);
                    if (marketbooks.MarketId != null)
                    {
                        newsize = marketbooks.Runners[0].ExchangePrices.AvailableToBack[0].Size.ToString();
                    }
                }
                if (LastSize == newsize)
                {

                    return "false";
                }
                else
                {
                    return "true";
                }
            }
            else
            {
                var newsize = "";
                for (int i = 0; i < 3; i++)
                {
                    var marketbooks = GetCurrentMarketBook(ID, sheetname, MainSportsCategory, OrignalOpenDate);
                    if (marketbooks.MarketId != null)
                    {
                        newsize = marketbooks.Runners[0].ExchangePrices.AvailableToLay[0].Size.ToString();
                    }
                }
                if (LastSize == newsize)
                {

                    return "false";
                }
                else
                {
                    return "true";
                }
            }

        }
        public void CalculateAvearageforAllUsers(ExternalAPI.TO.MarketBook objMarketBook, string RunnersForAverage)
        {
            try
            {

                if (RunnersForAverage == "")
                {
                    return;

                }

                string[] runnersarr1 = JsonConvert.DeserializeObject<string[]>(RunnersForAverage);

                if (runnersarr1.Count() == 2)
                {
                    double runner1profit = 0;
                    double runner2profit = 0;

                    ExternalAPI.TO.Runner runner1 = objMarketBook.Runners.Where(item => item.SelectionId == runnersarr1[0]).FirstOrDefault();
                    ExternalAPI.TO.Runner runner2 = objMarketBook.Runners.Where(item => item.SelectionId == runnersarr1[1]).FirstOrDefault();
                    if (runner1 != null)
                    {
                        runner1profit = runner1.ProfitandLoss;
                        runner2profit = runner2.ProfitandLoss;
                    }
                    if (runner1profit == 0 || runner2profit == 0)
                    {
                        runner1.Average = "";
                        runner2.Average = "";
                    }
                    if ((runner1profit > 0 && runner2profit > 0) || (runner1profit < 0 && runner2profit < 0))
                    {
                        runner1.Average = "0.00";
                        runner2.Average = "0.00";

                        return;
                    }
                    if (runner1profit > 0)
                    {
                        runner1.Average = " L";

                        runner2.Average = " K";


                    }
                    else
                    {

                        if (runner2profit > 0)
                        {
                            runner1.Average = " K";

                            runner2.Average = " L";
                        }
                    }
                    if (runner1profit < 0) { runner1profit = runner1profit * -1; }
                    if (runner2profit < 0) { runner2profit = runner2profit * -1; }
                    if (runner1profit > 0 && runner2profit > 0)
                    {
                        double result = runner1profit / runner2profit;
                        double result2 = runner2profit / runner1profit;
                        runner1.Average = result.ToString("F2") + runner1.Average;
                        runner2.Average = result2.ToString("F2") + runner2.Average;
                    }

                }

            }
            catch (System.Exception ex)
            {

            }
        }
        
        public void Update()
        {
            try {
                CricketUpdate Update = new CricketUpdate();
                Models.Score score = new Models.Score();
                Models.Away away = new Models.Away();
               

                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("https://ips.betfair.com/inplayservice/v1/scores?_ak=nzIFcwyWhrlwYMrh&alt=json&eventIds=30140594&locale=en_GB&productType=EXCHANGE&regionCode=UK"));

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
               
                var mode = new System.Web.Script.Serialization.JavaScriptSerializer();

                List<CricketUpdate> ss= (List<CricketUpdate>)mode.Deserialize(jsonString, typeof(List<CricketUpdate>));

                score.Away.Inning1.Overs = ss[0].Score.Away.Inning1.Overs;


                score.Away.Inning1.Runs = ss[0].Score.Away.Inning1.Runs;


            }
            catch (System.Exception ex)
            {

            }
       
    }
        
        public void GetBallbyBallSummary()
        {
            
            BFBallabyBallSummary sumery = new BFBallabyBallSummary();
            try
            {


                string over = "";
                string score = "";
                string innings = "";
                string wickets = "";
                string teamname = "";
                string matchstatus = "";
                string matchType = "";
                string TotalOvers = "";

               
                //List<SP_UserMarket_GetDistinctLinevMarkets_Result> lstEvents = dbEntities.SP_UserMarket_GetDistinctLinevMarkets().ToList<SP_UserMarket_GetDistinctLinevMarkets_Result>();
                //if (lstEvents.Count > 0)
                //{

                //foreach (var item in lstEvents)
                //{

                //if (item.EventOpenDate >= DateTime.Now.AddHours(-5) || item.MarketStatus == "Closed")
                //{
                //    continue;
                //}
                if (1 == 2)
                {

                }
                else
                {

                    //if (item.GetMatchUpdatesFrom == "Local")
                    //{


                    string URL = "https://ips.betfair.com/inplayservice/v1/scores?_ak=nzIFcwyWhrlwYMrh&alt=json&eventIds=30092870&locale=en_GB&productType=EXCHANGE&regionCode=UK";
                    string JsonResultsArr = "";
                    try
                    {


                        using (var client = new WebClient())
                        {
                            JsonResultsArr = client.DownloadString(URL);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        APIConfig.LogError(ex);

                    }
                    if (JsonResultsArr != "")
                    {
                        var marketballbyaball = new List<BFBallabyBallSummary>();
                        marketballbyaball = JsonConvert.DeserializeObject<List<BFBallabyBallSummary>>(JsonResultsArr);
                        if (marketballbyaball.Count > 0)
                        {

                            //over = marketballbyaball[0].score.home.inning1.overs;
                            //score = marketballbyaball[0].score.home.inning1.runs;
                            //innings = "2";
                            matchstatus = marketballbyaball[0].matchStatus;
                            matchType = marketballbyaball[0].matchType;
                            if (marketballbyaball[0].matchType == "ODI" || marketballbyaball[0].matchType == "T20" || marketballbyaball[0].matchType == "LIMITED_OVER")
                            {
                                if (marketballbyaball[0].score.away.inning1 != null && marketballbyaball[0].score.home.inning1 == null)
                                {

                                    over = marketballbyaball[0].score.away.inning1.overs;
                                    score = marketballbyaball[0].score.away.inning1.runs;
                                    innings = "1";
                                    wickets = marketballbyaball[0].score.away.inning1.wickets;
                                    teamname = marketballbyaball[0].score.away.name;

                                }
                                else
                                {
                                    if (marketballbyaball[0].score.away.inning1 == null && marketballbyaball[0].score.home.inning1 != null)
                                    {

                                        over = marketballbyaball[0].score.home.inning1.overs;
                                        score = marketballbyaball[0].score.home.inning1.runs;
                                        innings = "1";
                                        wickets = marketballbyaball[0].score.home.inning1.wickets;
                                        teamname = marketballbyaball[0].score.home.name;
                                    }
                                    else
                                    {
                                        if (marketballbyaball[0].score.home.highlight == true)
                                        {
                                            over = marketballbyaball[0].score.home.inning1.overs;
                                            score = marketballbyaball[0].score.home.inning1.runs;
                                            innings = "2";
                                            wickets = marketballbyaball[0].score.home.inning1.wickets;
                                            teamname = marketballbyaball[0].score.home.name;


                                        }
                                        else
                                        {
                                            over= marketballbyaball[0].score.away.inning1.overs;
                                            score = marketballbyaball[0].score.away.inning1.runs;
                                            innings = "2";
                                            wickets = marketballbyaball[0].score.away.inning1.wickets;
                                            teamname = marketballbyaball[0].score.away.name;


                                        }
                                    }

                                }
                                try
                                {
                                    if (wickets == "ALL_OUT")
                                    {
                                        wickets = "10";
                                    }
                                    try
                                    {


                                        //if (item.TotalOvers != null && item.TotalOvers != "")
                                        //{
                                        //    TotalOvers = item.TotalOvers.ToString();
                                        //}
                                    }
                                    catch (System.Exception ex)
                                    {

                                    }
                                    //if (teamname.Contains(" "))
                                    //{
                                    //    string[] teamnamearr = teamname.Split(' ');
                                    //    teamname = "";
                                    //    foreach (var str in teamnamearr)
                                    //    {
                                    //        teamname += str.Substring(0, 1).ToString().ToUpper();
                                    //    }
                                    //}
                                   // dbEntities.SP_BallbyBallSummary_Insert(item.EventID.ToString(), item.MarketCatalogueID, Convert.ToDouble(over), Convert.ToInt32(score), Convert.ToInt32(innings), Convert.ToDateTime(item.EventOpenDate), Convert.ToInt32(wickets), teamname, matchstatus, matchType, TotalOvers);
                                }
                                catch (System.Exception ex)
                                {
                                    APIConfig.LogError(ex);
                                }
                            }
                            else
                            {
                                ////TEstMatch
                                if (marketballbyaball[0].score.home.inning1 != null && marketballbyaball[0].score.away.inning1 == null && marketballbyaball[0].score.home.highlight == true)
                                {
                                    over = marketballbyaball[0].score.home.inning1.overs;
                                    score = marketballbyaball[0].score.home.inning1.runs;
                                    innings = "1";
                                    wickets = marketballbyaball[0].score.home.inning1.wickets;
                                    teamname = marketballbyaball[0].score.home.name;
                                }
                                if (marketballbyaball[0].score.home.inning1 == null && marketballbyaball[0].score.away.inning1 != null && marketballbyaball[0].score.away.highlight == true)
                                {
                                    over = marketballbyaball[0].score.away.inning1.overs;
                                    score = marketballbyaball[0].score.away.inning1.runs;
                                    innings = "1";
                                    wickets = marketballbyaball[0].score.away.inning1.wickets;
                                    teamname = marketballbyaball[0].score.away.name;
                                }


                                if (marketballbyaball[0].score.home.inning1 != null && marketballbyaball[0].score.away.inning1 != null && marketballbyaball[0].score.away.highlight == true)
                                {
                                    over = marketballbyaball[0].score.away.inning1.overs;
                                    score = marketballbyaball[0].score.away.inning1.runs;
                                    innings = "2";
                                    wickets = marketballbyaball[0].score.away.inning1.wickets;
                                    teamname = marketballbyaball[0].score.away.name;
                                }
                                if (marketballbyaball[0].score.home.inning1 != null && marketballbyaball[0].score.away.inning1 != null && marketballbyaball[0].score.home.highlight == true)
                                {
                                    over = marketballbyaball[0].score.home.inning1.overs;
                                    score = marketballbyaball[0].score.home.inning1.runs;
                                    innings = "2";
                                    wickets = marketballbyaball[0].score.home.inning1.wickets;
                                    teamname = marketballbyaball[0].score.home.name;
                                }


                                if (marketballbyaball[0].score.home.inning1 != null && marketballbyaball[0].score.away.inning1 != null && marketballbyaball[0].score.home.inning2 != null && marketballbyaball[0].score.home.highlight == true)
                                {
                                    over = marketballbyaball[0].score.home.inning2.overs;
                                    score = marketballbyaball[0].score.home.inning2.runs;
                                    innings = "3";
                                    wickets = marketballbyaball[0].score.home.inning2.wickets;
                                    teamname = marketballbyaball[0].score.home.name;
                                }
                                if (marketballbyaball[0].score.home.inning1 != null && marketballbyaball[0].score.away.inning1 != null && marketballbyaball[0].score.away.inning2 != null && marketballbyaball[0].score.away.highlight == true)
                                {
                                    over = marketballbyaball[0].score.away.inning2.overs;
                                    score = marketballbyaball[0].score.away.inning2.runs;
                                    innings = "3";
                                    wickets = marketballbyaball[0].score.away.inning2.wickets;
                                    teamname = marketballbyaball[0].score.away.name;
                                }


                                if (marketballbyaball[0].score.home.inning1 != null && marketballbyaball[0].score.away.inning1 != null && marketballbyaball[0].score.home.inning2 != null && marketballbyaball[0].score.away.inning2 != null && marketballbyaball[0].score.away.highlight == true)
                                {
                                    over = marketballbyaball[0].score.away.inning2.overs;
                                    score = marketballbyaball[0].score.away.inning2.runs;
                                    innings = "4";
                                    wickets = marketballbyaball[0].score.away.inning2.wickets;
                                    teamname = marketballbyaball[0].score.away.name;
                                }
                                /////



                                if (marketballbyaball[0].score.home.inning1 != null && marketballbyaball[0].score.away.inning1 != null && marketballbyaball[0].score.away.inning2 != null && marketballbyaball[0].score.home.inning2 != null && marketballbyaball[0].score.home.highlight == true)
                                {
                                    over = marketballbyaball[0].score.home.inning2.overs;
                                    score = marketballbyaball[0].score.home.inning2.runs;
                                    innings = "4";
                                    wickets = marketballbyaball[0].score.home.inning2.wickets;
                                    teamname = marketballbyaball[0].score.home.name;
                                }
                                if (wickets == "ALL_OUT")
                                {
                                    wickets = "10";
                                }
                                try
                                {
                                    try
                                    {


                                        //if (item.TotalOvers != null && item.TotalOvers != "")
                                        //{
                                        //    TotalOvers = item.TotalOvers.ToString();
                                        //}
                                    }
                                    catch (System.Exception ex)
                                    {

                                    }
                                     //dbEntities.SP_BallbyBallSummary_Insert(item.EventID.ToString(), item.MarketCatalogueID, Convert.ToDouble(over), Convert.ToInt32(score), Convert.ToInt32(innings), Convert.ToDateTime(item.EventOpenDate), Convert.ToInt32(wickets), teamname, matchstatus, matchType, TotalOvers);
                                }
                                catch (System.Exception ex)
                                {
                                    APIConfig.LogError(ex);
                                }


                            }
                        }



                    }

                } }

            // }

            //}

            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);

            }


          
        }


        //public ExternalAPI.TO.MarketBookForindianFancy GetMarketDatabyIDIndianFancy()
        //{
            
        //    HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("http://139.162.20.164:3000/getdemodata?eventId=30380969"));


        //    WebReq.Method = "GET";

        //    HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();

        //    Console.WriteLine(WebResp.StatusCode);
        //    Console.WriteLine(WebResp.Server);

        //    string jsonString;
        //    using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
        //    {
        //        StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
        //        jsonString = reader.ReadToEnd();
        //    }
        //    try
        //    {
        //        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(jsonString);
        //        MarketBookForindianFancy marketbook = new MarketBookForindianFancy();
        //        List<ExternalAPI.TO.RunnerForIndianFancy> lstRunners = new List<ExternalAPI.TO.RunnerForIndianFancy>();
        //        var lstpricelistback = new List<PriceSize>();
        //        var lstpricelistlay = new List<PriceSize>();

        //        for (int i = 0; i < myDeserializedClass.t3.Count; i++)
        //        {
        //            var runner = new ExternalAPI.TO.RunnerForIndianFancy();
        //            runner.RunnerName = myDeserializedClass.t3[i].nat;
        //            marketbook.MarketId = myDeserializedClass.t3[i].mid;
        //            runner.SelectionId = myDeserializedClass.t3[i].sid;
        //            marketbook.BettingAllowed = true;
        //            marketbook.MarketBookName = myDeserializedClass.t3[i].nat;
        //            runner.MarketStatusStr = myDeserializedClass.t3[i].gstatus;

        //            runner.LaySize =Convert.ToInt64(myDeserializedClass.t3[i].ls1);
        //            runner.Layprice = Convert.ToInt64(myDeserializedClass.t3[i].l1);

        //            runner.BackSize = Convert.ToInt64(myDeserializedClass.t3[i].bs1);
        //            runner.Backprice = Convert.ToInt64(myDeserializedClass.t3[i].b1);

        //            // Cricket
        //            lstRunners.Add(runner);

        //        }


        //        marketbook.MainSportsname = "Cricket";

        //        marketbook.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>(lstRunners);

        //        return marketbook;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        APIConfig.LogError(ex);
        //        var marketbook = new MarketBookForindianFancy();
        //        return marketbook;
        //    }

        //}

        public string LoadActiveMarket(string ID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory, string RunnersForAverage)
        {
            try
            {            
                UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                if (ID != "")
                {                  
                    List<string> lstIDs = new List<string>();

                    lstIDs.Add(ID);
                    var marketbooks = GetMarketDatabyID(lstIDs.ToArray(), sheetname, OrignalOpenDate, MainSportsCategory);
                    if (marketbooks.Count() > 0)
                    {
                        marketbooks[0].MarketBookName = sheetname;
                        marketbooks[0].MainSportsname = MainSportsCategory;
                        marketbooks[0].OrignalOpenDate = OrignalOpenDate;
                        marketbooks[0].BettingAllowedOverAll = CheckForAllowedBettingOverAll(MainSportsCategory, sheetname);

                        ViewBag.TotalMarketsOpened = marketbooks[0].marketsopened;
                        if (LoggedinUserDetail.GetUserTypeID() == 3)
                        {

                            var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));

                            List<UserBets> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();


                            marketbooks[0].DebitCredit = objUserBets.ceckProfitandLoss(marketbooks[0], lstUserBets);
                            if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                            {
                                foreach (var runner in marketbooks[0].Runners)
                                {
                                    runner.ProfitandLoss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                    runner.Loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                }
                            }
                            else
                            {
                                if (marketbooks[0].Runners.Count() == 1)
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        runner.ProfitandLoss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (runner.ProfitandLoss > 0)
                                        {
                                            runner.ProfitandLoss = -1 * runner.ProfitandLoss;
                                        }
                                    }
                                }
                                else
                                {

                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        runner.ProfitandLoss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                    }
                                    if (marketbooks[0].MainSportsname == "Cricket" && marketbooks[0].MarketBookName.Contains("Match Odds"))
                                    {
                                        CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);
                                    }
                                    //CalculateAvearageforAllUsers(marketbooks[0]);
                                }

                            }

                            Session["userbets"] = lstUserBet.Where(item2 => item2.MarketBookID == marketbooks[0].MarketId).ToList();
                            Session["userbet"] = lstUserBet;
                            List<UserBets> lstAllUserBets = new List<Models.UserBets>();
                            if (Session["linevmarkets"] != null)
                            {
                                List<ExternalAPI.TO.LinevMarkets> linevmarketsfig = new List<LinevMarkets>();
                                List<ExternalAPI.TO.LinevMarkets> linevmarkets = (List<ExternalAPI.TO.LinevMarkets>)Session["linevmarkets"];
                                if (linevmarkets != null)
                                {
                                    linevmarketsfig = linevmarkets.GroupBy(item => item.MarketCatalogueName).Select(g => g.First()).Where(item2 => item2.EventName == "Figure").ToList();
                                    linevmarkets = linevmarkets.Where(item => item.EventName != "Figure").ToList();

                                    List<UserBets> lstFancyBets = new List<UserBets>();
                                    foreach (var lineitem in linevmarkets)
                                    {
                                        lstFancyBets = lstUserBet.Where(item => item.MarketBookID == lineitem.MarketCatalogueID).ToList();

                                        lstAllUserBets.AddRange(lstFancyBets);
                                    }
                                    List<UserBets> lstFancyfigBets = new List<UserBets>();
                                    foreach (var lineitem in linevmarketsfig)
                                    {
                                        lstFancyfigBets = lstUserBet.Where(item => item.MarketBookID == lineitem.MarketCatalogueID).ToList();

                                        lstAllUserBets.AddRange(lstFancyfigBets);
                                    }
                                }
                            }
                            lstAllUserBets.AddRange(lstUserBets);
                            lstAllUserBets = lstAllUserBets.Where(x => x.location != "9").ToList();
                            long Liabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstAllUserBets);
                            Session["liabality"] = Liabality;
                            lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                            decimal TotLiabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBet);
                            Session["totliabality"] = TotLiabality;
                            ViewBag.totliabalityNew = 0;
                            // Updateunmatchbets(marketbooks.ToArray(), lstUserBet);
                            // marketbooks[0].UserBetsEndUser = ConverttoJSONString(lstUserBet);

                        }
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                            var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(userbets);

                            List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();

                            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                            foreach (var userid in lstUsers)
                            {

                                // var agentrate = 0;
                                List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();

                                marketbooks[0].DebitCredit = objUserBets.ceckProfitandLossAgent(marketbooks[0], lstuserbet);

                                if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, loss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.Loss += Convert.ToInt64(-1 * profit);


                                    }
                                }
                                else
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                                        //  decimal profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
                                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                    }
                                    if (marketbooks[0].MainSportsname == "Cricket" && marketbooks[0].MarketBookName.Contains("Match Odds"))
                                    {
                                        CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);
                                    }
                                }

                            }

                         
                            Session["userbets"] = lstUserBet;
                            lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                            decimal CurrentLiabality = objUserBets.GetLiabalityofCurrentAgent(lstUserBet);
                            Session["liabality"] = CurrentLiabality;
                            
                            decimal TotLiabality = objUserBets.GetLiabalityofCurrentAgent(lstUserBet);
                            Session["totliabality"] = TotLiabality;
                            ViewBag.totliabalityNew = 0;
                            //  marketbooks[0].UserBetsAgent = ConverttoJSONString(userbets);

                        }
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {

                            string userbets = objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]);
                            List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(userbets);
                            //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
                            List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();

                            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                            foreach (var userid in lstUsers)
                            {

                                // var agentrate = 0;
                                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                                var agentrate = lstuserbet[0].AgentRate;
                                var superrate = lstuserbet[0].SuperAgentRateB;
                                bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                                decimal adminrate1 = ((100 - Convert.ToDecimal(TransferAdminPercentage)) / 100);
                                decimal adminrateafteragnetrateminus = (100 - Convert.ToDecimal(agentrate));
                                decimal adminrateacc = adminrateafteragnetrateminus * adminrate1;

                                var superpercent = 0;
                                if (superrate > 0)
                                {
                                    superpercent = superrate - Convert.ToInt32(agentrate);
                                }
                                else
                                {
                                    superpercent = 0;
                                }
                                marketbooks[0].DebitCredit = objUserBets.ceckProfitandLossAdmin(marketbooks[0], lstuserbet);

                                if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (adminrate / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);

                                    }
                                }
                                else
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {

                                        long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (marketbooks[0].Runners.Count == 1)
                                        {
                                            if (profitorloss > 0)
                                            {
                                                profitorloss = -1 * profitorloss;
                                            }
                                        }
                                        ///decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        // decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(TransferAdminPercentage)- Convert.ToDecimal(superpercent);
                                        // decimal profit = (adminrate / 100) * profitorloss;
                                        // runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(superpercent) : adminrateacc;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                    }
                                    if (marketbooks[0].MainSportsname == "Cricket" && marketbooks[0].MarketBookName.Contains("Match Odds"))
                                    {
                                        CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);
                                    }
                                }
                            }
                            Session["userbets"] = lstUserBet;

                            decimal CurrentLiabality = objUserBets.GetLiabalityofAdmin(lstUserBets);
                            Session["liabality"] = CurrentLiabality;
                           
                            decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                            Session["totliabality"] = TotLiabality;
                            ViewBag.totliabalityNew = 0;
                            //var lstUserBetnew = lstUserBet.Take(10).ToList();
                            // marketbooks[0].UserBetsAdmin = userbets;

                        }

                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                        {
                            string userbets = objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                            var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforSuper>>(userbets);
                             List<UserBetsforSuper> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();

                            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                            foreach (var userid in lstUsers)
                            {
                                // var agentrate = 0;
                                List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                                var agentrate = lstuserbet[0].AgentRate;
                                var supertrate = lstuserbet[0].SuperAgentRateB;
                                bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                                var superpercent = supertrate - Convert.ToInt32(agentrate);
                              

                                var adminpercent = 100 - (superpercent + Convert.ToInt32(agentrate) + TransferAdminPercentage);

                                marketbooks[0].DebitCredit = objUserBets.ceckProfitandLossSuper(marketbooks[0], lstuserbet);
                                if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {                                   
                                        long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal superfinal = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                                        decimal profit = (superfinal / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (superfinal / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);
                                    }
                                }
                                else
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (marketbooks[0].Runners.Count == 1)
                                        {
                                            if (profitorloss > 0)
                                            {
                                                profitorloss = -1 * profitorloss;
                                            }
                                        }
                                        ///decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                    }
                                    if (marketbooks[0].MainSportsname == "Cricket" && marketbooks[0].MarketBookName.Contains("Match Odds"))
                                    {
                                        CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);
                                    }
                                }
                            }
                            Session["userbets"] = lstUserBet;
                            lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                            decimal CurrentLiabality = objUserBets.GetLiabalityofSuper(lstUserBet);
                            Session["liabality"] = CurrentLiabality;
                           
                            decimal TotLiabality = objUserBets.GetLiabalityofSuper(lstUserBet);
                            Session["totliabality"] = TotLiabality;
                            ViewBag.totliabalityNew = 0;
                        }

                        if (LoggedinUserDetail.GetUserTypeID() == 9)
                        {
                            string userbets = objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                            var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforSamiadmin>>(userbets);
                            List<UserBetsforSamiadmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();

                            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                            foreach (var userid in lstUsers)
                            {
                                // var agentrate = 0;
                                List<UserBetsforSamiadmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                                var agentrate = lstuserbet[0].AgentRate;
                                var supertrate = lstuserbet[0].SuperAgentRateB;
                                var samiadminrate = lstuserbet[0].SamiAdminRate;
                                bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                                var superpercent = 0;
                                if (supertrate > 0)
                                {
                                    superpercent = supertrate - Convert.ToInt32(agentrate);
                                }
                                else
                                {
                                    superpercent = 0;
                                }
                                var samiadminpercent = 0;
                                if (samiadminrate > 0)
                                {
                                    samiadminpercent =Convert.ToInt32(samiadminrate) -(superpercent + Convert.ToInt32(agentrate));
                                }
                                else
                                {
                                    samiadminpercent = 0;
                                }

                                var adminpercent = 100 - (samiadminpercent + superpercent + Convert.ToInt32(agentrate) + TransferAdminPercentage);

                                marketbooks[0].DebitCredit = objUserBets.ceckProfitandLossSamiadmin(marketbooks[0], lstuserbet);
                                if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal superfinal = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                                        decimal profit = (superfinal / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (superfinal / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);
                                    }
                                }
                                else
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (marketbooks[0].Runners.Count == 1)
                                        {
                                            if (profitorloss > 0)
                                            {
                                                profitorloss = -1 * profitorloss;
                                            }
                                        }
                                        ///decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - superpercent - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                    }
                                    if (marketbooks[0].MainSportsname == "Cricket" && marketbooks[0].MarketBookName.Contains("Match Odds"))
                                    {
                                        CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);
                                    }
                                }
                            }
                            Session["userbets"] = lstUserBet;
                            lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                            decimal CurrentLiabality = objUserBets.GetLiabalityofSamiadmin(lstUserBet);
                            Session["liabality"] = CurrentLiabality;

                            decimal TotLiabality = objUserBets.GetLiabalityofSamiadmin(lstUserBet);
                            Session["totliabality"] = TotLiabality;
                            ViewBag.totliabalityNew = 0;
                        }

                         return ConverttoJSONString(marketbooks);
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                return "";
            }
        }
        public string LoadActiveMarketTWT(string ID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory, string RunnersForAverage)
        {
            try
            {
                UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                if (ID != "")
                {
                    List<string> lstIDs = new List<string>();
                    lstIDs.Add(ID);
                    var marketbooks = GetMarketDatabyID(lstIDs.ToArray(), sheetname, OrignalOpenDate, MainSportsCategory);
                    if (marketbooks.Count() > 0)
                    {
                        marketbooks[0].MarketBookName = sheetname;
                        marketbooks[0].MainSportsname = MainSportsCategory;
                        marketbooks[0].OrignalOpenDate = OrignalOpenDate;
                        marketbooks[0].BettingAllowedOverAll = CheckForAllowedBettingOverAll(MainSportsCategory, sheetname);
                        ViewBag.TotalMarketsOpened = marketbooks[0].marketsopened;
                        if (LoggedinUserDetail.GetUserTypeID() == 3)
                        {
                            var lstUserBet = (List<UserBets>)Session["userbet"];
                            List<UserBets> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();
                            marketbooks[0].DebitCredit = objUserBets.ceckProfitandLoss(marketbooks[0], lstUserBets);
                            if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                            {
                                foreach (var runner in marketbooks[0].Runners)
                                {
                                    runner.ProfitandLoss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                    runner.Loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                }
                            }
                            else
                            {
                                if (marketbooks[0].Runners.Count() == 1)
                                {                         
                                    foreach (var runner in marketbooks[0].Runners)
                                    {

                                        runner.ProfitandLoss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (runner.ProfitandLoss > 0)
                                        {
                                            runner.ProfitandLoss = -1 * runner.ProfitandLoss;
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        runner.ProfitandLoss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                    }
                                    CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);                                                                   
                                }
                            }

                        }
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                           
                            var lstUserBet = (List<UserBetsforAgent>)Session["userbets"];

                            List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();

                            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                            foreach (var userid in lstUsers)
                            {
                                // var agentrate = 0;
                                List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();

                                marketbooks[0].DebitCredit = objUserBets.ceckProfitandLossAgent(marketbooks[0], lstuserbet);

                                if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, loss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.Loss += Convert.ToInt64(-1 * profit);
                                    }
                                }
                                else
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                                        //  decimal profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
                                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                    }
                                    if (marketbooks[0].MainSportsname == "Cricket" && marketbooks[0].MarketBookName.Contains("Match Odds"))
                                    {
                                        CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);
                                    }
                                }
                            }
                            //  marketbooks[0].UserBetsAgent = ConverttoJSONString(userbets);
                        }
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            List<UserBetsForAdmin> lstUserBet = (List<UserBetsForAdmin>)Session["userbets"];
                            //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
                            List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbooks[0].MarketId).ToList();

                            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                            foreach (var userid in lstUsers)
                            {

                                // var agentrate = 0;
                                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                                var agentrate = lstuserbet[0].AgentRate;
                                bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                marketbooks[0].DebitCredit = objUserBets.ceckProfitandLossAdmin(marketbooks[0], lstuserbet);
                                if (marketbooks[0].MarketBookName.Contains("To Be Placed"))
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (adminrate / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);


                                    }
                                }
                                else
                                {
                                    foreach (var runner in marketbooks[0].Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbooks[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                    }
                                    if (marketbooks[0].MainSportsname == "Cricket" && marketbooks[0].MarketBookName.Contains("Match Odds"))
                                    {
                                        CalculateAvearageforAllUsers(marketbooks[0], RunnersForAverage);
                                    }
                                }




                            }
                     

                        }

                       
                        return ConverttoJSONString(marketbooks);
                        // return RenderRazorViewToString("MarketBookSelected", marketbooks);
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                 
                    return "";
                }


            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                return "";
            }
        }

        public string LoadActiveMarketSoccerGoal(string EventID )
        {
            try
            {
                List<string> data = new List<string>();
                UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                List<string> lstIDs = new List<string>();
             

                LoggedinUserDetail.CheckifUserLogin();
                var Soccergoalmarket = objUsersServiceCleint.GetSoccergoalbyeventId(LoggedinUserDetail.GetUserID(), EventID);

               

                var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));

                results = results.Where(item => item.EventID == EventID && item.Name != "Match Odds").ToList();
                var marketbooks = new List<MarketBook>();
                var marketbooks1 = new List<MarketBook>();

                foreach (var item in results)
                {
                    lstIDs = new List<string>();

                    lstIDs.Add(item.ID);
                    var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                    if (marketbook.Count() > 0)

                    {
                        if (marketbook[0].Runners != null)
                        {
                            marketbooks.Add(marketbook[0]);
                        }

                    }
                    else
                    {

                    }

                }


                if (lstIDs.Count > 0)
                {
                   
                        foreach (var item2 in marketbooks)
                        {
                            if (marketbooks.Count() > 0)
                            {
                            //item2.MarketBookName = item.EventName + " / " + item.Name;
                            //item2.OrignalOpenDate = item.EventOpenDate;
                            //item2.MainSportsname = item.EventTypeName;
                                  item2.BettingAllowed=true;
                                item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item2.MainSportsname, item2.MarketBookName);
                               // item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                               // item2.EventID = item.EventID;

                                ViewBag.TotalMarketsOpened = marketbooks[0].marketsopened;

                                if (LoggedinUserDetail.GetUserTypeID() == 3)
                                {
                                    var lstUserBet = (List<UserBets>)Session["userbet"];
                                    List<UserBets> lstUserBets = lstUserBet.Where(item3 => item3.isMatched == true && item3.MarketBookID == item2.MarketId).ToList();
                                    item2.DebitCredit = objUserBets.ceckProfitandLoss(item2, lstUserBets);
                                
                                        foreach (var runner in item2.Runners)
                                        {
                                            runner.ProfitandLoss = Convert.ToInt64(item2.DebitCredit.Where(item5 => item5.SelectionID == runner.SelectionId).Sum(item5 => item5.Debit) - item2.DebitCredit.Where(item5 => item5.SelectionID == runner.SelectionId).Sum(item5 => item5.Credit));
                                        }
                                    //CalculateAvearageforAllUsers(item2, RunnersForAverage);

                                   
                                }
                                if (LoggedinUserDetail.GetUserTypeID() == 2)
                                {

                                    var lstUserBet = (List<UserBetsforAgent>)Session["userbets"];

                                    List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item3 => item3.isMatched == true && item3.MarketBookID == item2.MarketId).ToList();

                                    var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                                    foreach (var userid in lstUsers)
                                    {
                                        // var agentrate = 0;
                                        List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item4 => item4.UserID == Convert.ToInt32(userid.UserID)).ToList();

                                        item2.DebitCredit = objUserBets.ceckProfitandLossAgent(item2, lstuserbet);


                                        foreach (var runner in item2.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(item2.DebitCredit.Where(item5 => item5.SelectionID == runner.SelectionId).Sum(item5 => item5.Debit) - item2.DebitCredit.Where(item5 => item5.SelectionID == runner.SelectionId).Sum(item5 => item5.Credit));

                                            //  decimal profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
                                            decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        }

                                    }
                                    
                                }
                                if (LoggedinUserDetail.GetUserTypeID() == 1)
                                {
                                    List<UserBetsForAdmin> lstUserBet = (List<UserBetsForAdmin>)Session["userbets"];
                                    //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
                                    List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item3 => item3.isMatched == true && item3.MarketBookID == item2.MarketId).ToList();

                                    var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                                    foreach (var userid in lstUsers)
                                    {
                                        // var agentrate = 0;
                                        List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item4 => item4.UserID == Convert.ToInt32(userid.UserID)).ToList();
                                        var agentrate = lstuserbet[0].AgentRate;
                                        bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                        item2.DebitCredit = objUserBets.ceckProfitandLossAdmin(item2, lstuserbet);

                                        foreach (var runner in item2.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(item2.DebitCredit.Where(item5 => item5.SelectionID == runner.SelectionId).Sum(item5 => item5.Debit) - item2.DebitCredit.Where(item5 => item5.SelectionID == runner.SelectionId).Sum(item5 => item5.Credit));

                                            decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                            decimal profit = (adminrate / 100) * profitorloss;
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        }
                                    }                          
                                }

                                marketbooks1.Add(item2);
                            }                           
                        }                      
                   // }
                    return ConverttoJSONString(marketbooks1);
                } 
                else
                {
                    return "";
                }
     
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                return "";
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

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        //public string LoadLiabalitybyMarket()
        //{
        //    UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
        //    try
        //    {
        //        List<LiabalitybyMarket> lstLibalitybymrakets = new List<LiabalitybyMarket>();
        //        if (LoggedinUserDetail.GetUserTypeID() == 3)
        //        {

        //            List<UserBets> lstUserBets = (List<UserBets>)Session["userbet"];
        //            UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
        //            lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentUserbyMarkets(LoggedinUserDetail.GetUserID(), lstUserBets);

        //            return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);

        //        }
        //        else
        //        {
        //            if (LoggedinUserDetail.GetUserTypeID() == 2)
        //            {

        //                string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID());
        //                var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(userbets);
        //                UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
        //                lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentAgentbyMarkets(lstUserBet);

        //                return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
        //            }
        //            else
        //            {
        //                if (LoggedinUserDetail.GetUserTypeID() == 1)
        //                {


        //                    string userbets = objUsersServiceCleint.GetUserbetsForAdmin();
        //                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(userbets);
        //                    UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
        //                    lstLibalitybymrakets = objUserBets.GetLiabalityofAdminbyMarkets(lstUserBet);

        //                    return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
        //                }
        //                else
        //                {

        //                    return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
        //                }

        //            }
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        List<LiabalitybyMarket> lstLibalitybymrakets = new List<LiabalitybyMarket>();
        //        return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
        //    }

        //    //List<LiabalitybyMarket> lstLiabalitymarket = new List<LiabalitybyMarket>();
        //    //if (Session["LiabalitybyMarket"] != null)
        //    //{
        //    //    lstLiabalitymarket = (List<LiabalitybyMarket>)Session["LiabalitybyMarket"];
        //    //    return RenderRazorViewToString("LiabalitybyMarket", lstLiabalitymarket);
        //    //}
        //    //else
        //    //{
        //    //    return RenderRazorViewToString("LiabalitybyMarket", lstLiabalitymarket);
        //    //}
        //}
        public bool CheckForAllowedBettingOverAll(string categoryname, string marketbookname)
        {
            AllowedMarketWeb AllowedMarketsForUser = JsonConvert.DeserializeObject<AllowedMarketWeb>(objUsersServiceCleint.GetAllowedMarketsbyUserID(Convert.ToInt32(LoggedinUserDetail.GetUserID())));
            bool AllowedBet = false;

            if (marketbookname.Contains("Line"))
            {
                AllowedBet = AllowedMarketsForUser.isFancyMarketAllowed;
                return AllowedBet;
            }

            if (categoryname.Contains("Horse Racing") && !marketbookname.Contains("To Be Placed"))
            {
                AllowedBet = AllowedMarketsForUser.isHorseRaceWinAllowedForBet;

            }
            else
            {
                if (categoryname.Contains("Horse Racing") && marketbookname.Contains("To Be Placed"))
                {
                    AllowedBet = AllowedMarketsForUser.isHorseRacePlaceAllowedForBet;

                }
                else
                {
                    if (categoryname.Contains("Greyhound Racing") && marketbookname.Contains("To Be Placed"))
                    {
                        AllowedBet = AllowedMarketsForUser.isGrayHoundRacePlaceAllowedForBet;

                    }
                    else
                    {
                        if (categoryname.Contains("Greyhound Racing") && !marketbookname.Contains("To Be Placed"))
                        {
                            AllowedBet = AllowedMarketsForUser.isGrayHoundRaceWinAllowedForBet;

                        }
                        else
                        {
                            if (marketbookname.Contains("Completed Match"))
                            {
                                AllowedBet = AllowedMarketsForUser.isCricketCompletedMatchAllowedForBet;

                            }
                            else
                            {
                                if (marketbookname.Contains("Innings Runs") || marketbookname.Contains("Inns Runs"))
                                {
                                    AllowedBet = AllowedMarketsForUser.isCricketInningsRunsAllowedForBet;

                                }
                                else
                                {
                                    if (categoryname == "Tennis")
                                    {
                                        AllowedBet = AllowedMarketsForUser.isTennisAllowedForBet;

                                    }
                                    else
                                    {
                                        if (categoryname == "Soccer")
                                        {
                                            AllowedBet = AllowedMarketsForUser.isSoccerAllowedForBet;

                                        }
                                        else
                                        {
                                            if (marketbookname.Contains("Tied Match"))
                                            {
                                                AllowedBet = AllowedMarketsForUser.isCricketTiedMatchAllowedForBet;

                                            }
                                            else
                                            {
                                                if (marketbookname.Contains("Winner"))
                                                {
                                                    AllowedBet = AllowedMarketsForUser.isWinnerMarketAllowedForBet;

                                                }
                                                else
                                                {

                                                    AllowedBet = AllowedMarketsForUser.isCricketMatchOddsAllowedForBet;



                                                }
                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return AllowedBet;
        }

        public string MarketBook(string ID)
        {
            try
            {

                Session["TWT"] = "";
                Session["TM"] = "";
                UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();

                //  return RenderRazorViewToString("MarketBook", marketbooks1);
                LoggedinUserDetail.CheckifUserLogin();
                if (LoggedinUserDetail.GetUserTypeID() != 1)
                {
                    objUsersServiceCleint.UpdateAllMarketClosedbyUserID(LoggedinUserDetail.GetUserID());
                }

                if (ID != "" && LoggedinUserDetail.GetUserTypeID() != 1)
                {
                    var resultsalreadyopened = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                    if (resultsalreadyopened != null && resultsalreadyopened.Count >= 10)
                    {
                        return "Limit exceed";
                    }
                    objUsersServiceCleint.SetMarketBookOpenbyUSer(LoggedinUserDetail.GetUserID(), ID);
                }
                if (ID != "" && LoggedinUserDetail.GetUserTypeID() == 1 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
                {

                    objUsersServiceCleint.SetMarketBookOpenbyUSer(73, ID);
                }
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    Session["Eventid"] =null;
                    var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                    //  System.Threading.Thread.Sleep(500);
                    if (results != null)
                    {
                        results = results.Where(item => item.ID == ID).ToList();
                        var marketbooks = new List<MarketBook>();
                        List<string> lstIDs = new List<string>();
                        foreach (var item in results)
                        {
                            lstIDs = new List<string>();

                            lstIDs.Add(item.ID);
                            var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                            if (marketbook.Count() > 0)

                            {
                                if (marketbook[0].Runners != null)
                                {
                                    marketbooks.Add(marketbook[0]);
                                }

                            }
                            else
                            {

                            }

                        }


                        foreach (var item in results)
                        {
                            foreach (var item2 in marketbooks)
                            {
                                if (item.ID == item2.MarketId)
                                {
                                    //item2.MarketBookName = item.Name + " / " + item.EventName;
                                    item2.MarketBookName = item.EventName + " / " + item.Name;
                                    item2.OrignalOpenDate = item.EventOpenDate;
                                    item2.MainSportsname = item.EventTypeName;
                                    item2.BettingAllowed = item.BettingAllowed;
                                    item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                    item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                    item2.EventID = item.EventID;
                                    Session["Eventid"] = item.EventID;


                                    var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                    foreach (var runnermarketitem in runnerdesc)
                                    {
                                        foreach (var runneritem in item2.Runners)
                                        {
                                            if (runnermarketitem.SelectionID == runneritem.SelectionId.Trim())
                                            {
                                                runneritem.RunnerName = runnermarketitem.SelectionName;
                                                runneritem.JockeyName = runnermarketitem.JockeyName;
                                                runneritem.WearingURL = runnermarketitem.Wearing;
                                                runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                runneritem.StallDraw = runnermarketitem.StallDraw;
                                               

                                            }
                                        }

                                    }

                                    // 

                                    if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                    {
                                        item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                        var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                        int UserIDforLinevmarkets = 0;
                                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                                        {
                                            UserIDforLinevmarkets = 73;
                                        }
                                        else
                                        {
                                            UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                        }
                                        var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                                        if (linevmarkets.Count() > 0)
                                        {
                                            item2.LineVMarkets = linevmarkets;
                                        }
                                    }
                                    ////

                                }
                            }

                        }
                        if (marketbooks.Count == 0)
                        {
                            ExternalAPI.TO.MarketBook item2 = new ExternalAPI.TO.MarketBook();
                            var item = results[0];
                            item2.MarketId = item.ID;
                            //item2.MarketBookName = item.Name + " / " + item.EventName;
                            item2.MarketBookName = item.EventName + " / " + item.Name;
                            item2.OrignalOpenDate = item.EventOpenDate;
                            item2.MainSportsname = item.EventTypeName;
                            item2.BettingAllowed = item.BettingAllowed;
                            item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                            item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                            item2.EventID = item.EventID;
                          
                            DateTime OpenDate = item.EventOpenDate.AddHours(5);
                            DateTime CurrentDate = DateTime.Now;
                            TimeSpan remainingdays = (CurrentDate - OpenDate);
                            if (OpenDate < CurrentDate)
                            {
                                item2.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                            }
                            else
                            {
                                item2.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                            }
                            item2.MarketStatusstr = "Active";

                            var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                            item2.Runners = new List<ExternalAPI.TO.Runner>();

                            foreach (var runnermarketitem in runnerdesc)
                            {
                                var runneritem = new ExternalAPI.TO.Runner();
                                runneritem.SelectionId = runnermarketitem.SelectionID;
                                runneritem.RunnerName = runnermarketitem.SelectionName;
                                runneritem.JockeyName = runnermarketitem.JockeyName;
                                runneritem.WearingURL = runnermarketitem.Wearing;
                                runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                runneritem.StallDraw = runnermarketitem.StallDraw;
                               
                                runneritem.ExchangePrices = new ExchangePrices();
                                var lstpricelist = new List<PriceSize>();
                                for (int i = 0; i < 3; i++)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = 0;

                                    pricesize.Price = 0;

                                    lstpricelist.Add(pricesize);
                                }
                                runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                                lstpricelist = new List<PriceSize>();
                                for (int i = 0; i < 3; i++)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = 0;

                                    pricesize.Price = 0;

                                    lstpricelist.Add(pricesize);
                                }
                                runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                                item2.Runners.Add(runneritem);


                            }
                            item2.FavoriteID = "0";
                            item2.FavoriteBack = "0";
                            item2.FavoriteBackSize = "0";
                            item2.FavoriteLay = "0";
                            item2.FavoriteLaySize = "0";
                            item2.FavoriteSelectionName = "";
                          
                            marketbooks.Add(item2);
                        }
                       
                        List<UserBets> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                       // List<UserBets> lstUserBets1 = lstUserBet.Where(item => item.isMatched == true && item.location !="8").ToList();
                        List<UserBets> lstUserBets = lstUserBet.Where(item => item.isMatched == true && item.location != "9").ToList();
                        var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                        foreach (var item in lstMarketIDS)
                        {
                            var objMarketbook = marketbooks.Where(item2 => item2.MarketId == item).FirstOrDefault();
                            if (objMarketbook != null)
                            {
                                objMarketbook.DebitCredit = objUserBets.ceckProfitandLoss(objMarketbook, lstUserBets);

                                foreach (var runner in objMarketbook.Runners)
                                {
                                    runner.ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                }
                                if (objMarketbook.MainSportsname == "Cricket" && objMarketbook.MarketBookName.Contains("Match Odds"))
                                {
                                    //CalculateAvearageforAllUsers(objMarketbook);
                                }
                                //CalculateAvearageforAllUsers(objMarketbook);
                            }

                        }

                        Session["userbets"] = lstUserBet;
                        long Liabality = 0;
                        Session["liabality"] = Liabality;
                        lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                        decimal TotLiabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBet);
                        Session["totliabality"] = TotLiabality;
                        ViewBag.totliabalityNew = TotLiabality;

                        return RenderRazorViewToString("MarketBook", marketbooks);
                    }
                    else
                    {
                        List<UserBets> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                        Session["userbets"] = lstUserBet;
                        Session["liabality"] = 0;
                      
                        decimal TotLiabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBet);
                        Session["totliabality"] = TotLiabality;
                        ViewBag.totliabalityNew = TotLiabality;
                        var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                        return RenderRazorViewToString("MarketBook", marketbooks);
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                        ViewBag.backgrod = "#1D9BF0";
                        ViewBag.color = "white";
                        var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                        if (results != null)
                        {
                            results = results.Where(item => item.ID == ID).ToList();
                            var marketbooks = new List<MarketBook>();
                            List<string> lstIDs = new List<string>();
                            foreach (var item in results)
                            {
                                lstIDs = new List<string>();

                                lstIDs.Add(item.ID);
                                var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                                if (marketbook.Count() > 0)

                                {
                                    if (marketbook[0].Runners != null)
                                    {
                                        marketbooks.Add(marketbook[0]);
                                    }

                                }
                                else
                                {

                                }

                            }


                            foreach (var item in results)
                            {
                                foreach (var item2 in marketbooks)
                                {
                                    if (item.ID == item2.MarketId)
                                    {
                                        //item2.MarketBookName = item.Name + " / " + item.EventName;
                                        item2.MarketBookName = item.EventName + " / " + item.Name;
                                        item2.OrignalOpenDate = item.EventOpenDate;
                                        item2.MainSportsname = item.EventTypeName;
                                        item2.BettingAllowed = item.BettingAllowed;
                                        item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                        item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                        item2.EventID = item.EventID;
                                        Session["Eventid"] = item.EventID;


                                        var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                        foreach (var runnermarketitem in runnerdesc)
                                        {
                                            foreach (var runneritem in item2.Runners)
                                            {
                                                if (runnermarketitem.SelectionID == runneritem.SelectionId)
                                                {
                                                    runneritem.RunnerName = runnermarketitem.SelectionName;
                                                    runneritem.JockeyName = runnermarketitem.JockeyName;
                                                    runneritem.WearingURL = runnermarketitem.Wearing;
                                                    runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                    runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                    runneritem.StallDraw = runnermarketitem.StallDraw;

                                                }
                                            }

                                        }

                                        // 

                                        if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                        {
                                            item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                            var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                            int UserIDforLinevmarkets = 0;
                                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                                            {
                                                UserIDforLinevmarkets = 73;
                                            }
                                            else
                                            {
                                                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                            }
                                            var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                                            if (linevmarkets.Count() > 0)
                                            {
                                                item2.LineVMarkets = linevmarkets;
                                            }
                                        }


                                    }
                                }

                            }
                            if (marketbooks.Count == 0)
                            {
                                ExternalAPI.TO.MarketBook item2 = new ExternalAPI.TO.MarketBook();
                                var item = results[0];
                                item2.MarketId = item.ID;
                                //item2.MarketBookName = item.Name + " / " + item.EventName;
                                item2.MarketBookName = item.EventName + " / " + item.Name;
                                item2.OrignalOpenDate = item.EventOpenDate;
                                item2.MainSportsname = item.EventTypeName;
                                item2.BettingAllowed = item.BettingAllowed;
                                item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                item2.EventID = item.EventID;
                                DateTime OpenDate = item.EventOpenDate.AddHours(5);
                                DateTime CurrentDate = DateTime.Now;
                                TimeSpan remainingdays = (CurrentDate - OpenDate);
                                if (OpenDate < CurrentDate)
                                {
                                    item2.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                                }
                                else
                                {
                                    item2.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                                }
                                item2.MarketStatusstr = "Active";

                                var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                item2.Runners = new List<ExternalAPI.TO.Runner>();

                                foreach (var runnermarketitem in runnerdesc)
                                {
                                    var runneritem = new ExternalAPI.TO.Runner();
                                    runneritem.SelectionId = runnermarketitem.SelectionID;
                                    runneritem.RunnerName = runnermarketitem.SelectionName;
                                    runneritem.JockeyName = runnermarketitem.JockeyName;
                                    runneritem.WearingURL = runnermarketitem.Wearing;
                                    runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                    runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                    runneritem.StallDraw = runnermarketitem.StallDraw;
                                    runneritem.ExchangePrices = new ExchangePrices();
                                    var lstpricelist = new List<PriceSize>();
                                    for (int i = 0; i < 3; i++)
                                    {
                                        var pricesize = new PriceSize();

                                        pricesize.Size = 0;

                                        pricesize.Price = 0;

                                        lstpricelist.Add(pricesize);
                                    }
                                    runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                                    lstpricelist = new List<PriceSize>();
                                    for (int i = 0; i < 3; i++)
                                    {
                                        var pricesize = new PriceSize();

                                        pricesize.Size = 0;

                                        pricesize.Price = 0;

                                        lstpricelist.Add(pricesize);
                                    }
                                    runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                                    item2.Runners.Add(runneritem);


                                }
                                item2.FavoriteID = "0";
                                item2.FavoriteBack = "0";
                                item2.FavoriteBackSize = "0";
                                item2.FavoriteLay = "0";
                                item2.FavoriteLaySize = "0";
                                item2.FavoriteSelectionName = "";
                                // 
                                if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                {
                                    item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                    var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                    int UserIDforLinevmarkets = 0;
                                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                                    {
                                        UserIDforLinevmarkets = 73;
                                    }
                                    else
                                    {
                                        UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                    }
                                    var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                                    if (linevmarkets.Count() > 0)
                                    {
                                        item2.LineVMarkets = linevmarkets;
                                    }
                                }
                                marketbooks.Add(item2);

                               
                            }
                            
                            List<UserBetsforAgent> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(objUsersServiceCleint.GetUserbetsbyUserIDandAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                             Session["userbets"] = lstUserBet;
                            Session["liabality"] = 0;
                            lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                            decimal TotLiabality = objUserBets.GetLiabalityofCurrentAgent(lstUserBet);
                            Session["totliabality"] = 0;
                            ViewBag.totliabalityNew = 0;
                            return RenderRazorViewToString("MarketBook", marketbooks);
                        }
                        else
                        {

                            List<UserBetsforAgent> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(objUsersServiceCleint.GetUserbetsbyUserIDandAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                            Session["userbets"] = lstUserBet;
                            Session["liabality"] = 0;
                            
                            decimal TotLiabality = objUserBets.GetLiabalityofCurrentAgent(lstUserBet);
                            Session["totliabality"] = 0;
                            ViewBag.totliabalityNew = 0;
                            var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                            return RenderRazorViewToString("MarketBook", marketbooks);
                        }
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            ViewBag.backgrod = "#1D9BF0";
                            ViewBag.color = "white";
                            var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(73));
                            if (results != null)
                            {
                                results = results.Where(item => item.ID == ID).ToList();
                                var marketbooks = new List<MarketBook>();
                                List<string> lstIDs = new List<string>();
                                foreach (var item in results)
                                {
                                    lstIDs = new List<string>();

                                    lstIDs.Add(item.ID);
                                    var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                                    if (marketbook.Count() > 0)

                                    {
                                        if (marketbook[0].Runners != null)
                                        {
                                            marketbooks.Add(marketbook[0]);
                                        }

                                    }
                                    else
                                    {

                                    }

                                }

                                foreach (var item in results)
                                {
                                    foreach (var item2 in marketbooks)
                                    {
                                        if (item.ID == item2.MarketId)
                                        {
                                            //item2.MarketBookName = item.Name + " / " + item.EventName;
                                            item2.MarketBookName = item.EventName + " / " + item.Name;
                                            item2.OrignalOpenDate = item.EventOpenDate;
                                            item2.MainSportsname = item.EventTypeName;
                                            item2.BettingAllowed = item.BettingAllowed;
                                            item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                            item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                            item2.EventID = item.EventID;
                                            Session["Eventid"] = item.EventID;


                                            var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                            foreach (var runnermarketitem in runnerdesc)
                                            {
                                                foreach (var runneritem in item2.Runners)
                                                {
                                                    if (runnermarketitem.SelectionID == runneritem.SelectionId)
                                                    {
                                                        runneritem.RunnerName = runnermarketitem.SelectionName;
                                                        runneritem.JockeyName = runnermarketitem.JockeyName;
                                                        runneritem.WearingURL = runnermarketitem.Wearing;
                                                        runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                        runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                        runneritem.StallDraw = runnermarketitem.StallDraw;

                                                    }
                                                }

                                            }

                                            // 
                                            

                                            if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                            {
                                                item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                                var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                                int UserIDforLinevmarkets = 0;
                                                if (LoggedinUserDetail.GetUserTypeID() == 1)
                                                {
                                                    UserIDforLinevmarkets = 73;
                                                }
                                                else
                                                {
                                                    UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                                }
                                                var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                                                if (linevmarkets.Count() > 0)
                                                {
                                                    item2.LineVMarkets = linevmarkets;
                                                }
                                            }

                                            ////

                                        }
                                    }

                                }
                                if (marketbooks.Count == 0)
                                {
                                    ExternalAPI.TO.MarketBook item2 = new ExternalAPI.TO.MarketBook();
                                    var item = results[0];
                                    item2.MarketId = item.ID;
                                    //item2.MarketBookName = item.Name + " / " + item.EventName;
                                    item2.MarketBookName = item.EventName + " / " + item.Name;
                                    item2.OrignalOpenDate = item.EventOpenDate;
                                    item2.MainSportsname = item.EventTypeName;
                                    item2.BettingAllowed = item.BettingAllowed;
                                    item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                    item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                    item2.EventID = item.EventID;
                                    DateTime OpenDate = item.EventOpenDate.AddHours(5);
                                    DateTime CurrentDate = DateTime.Now;
                                    TimeSpan remainingdays = (CurrentDate - OpenDate);
                                    if (OpenDate < CurrentDate)
                                    {
                                        item2.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                                    }
                                    else
                                    {
                                        item2.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                                    }
                                    item2.MarketStatusstr = "Active";

                                    var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                    item2.Runners = new List<ExternalAPI.TO.Runner>();

                                    foreach (var runnermarketitem in runnerdesc)
                                    {
                                        var runneritem = new ExternalAPI.TO.Runner();
                                        runneritem.SelectionId = runnermarketitem.SelectionID;
                                        runneritem.RunnerName = runnermarketitem.SelectionName;
                                        runneritem.JockeyName = runnermarketitem.JockeyName;
                                        runneritem.WearingURL = runnermarketitem.Wearing;
                                        runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                        runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                        runneritem.StallDraw = runnermarketitem.StallDraw;
                                        runneritem.ExchangePrices = new ExchangePrices();
                                        var lstpricelist = new List<PriceSize>();
                                        for (int i = 0; i < 3; i++)
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = 0;

                                            pricesize.Price = 0;

                                            lstpricelist.Add(pricesize);
                                        }
                                        runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                                        lstpricelist = new List<PriceSize>();
                                        for (int i = 0; i < 3; i++)
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = 0;

                                            pricesize.Price = 0;

                                            lstpricelist.Add(pricesize);
                                        }
                                        runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                                        item2.Runners.Add(runneritem);


                                    }
                                    item2.FavoriteID = "0";
                                    item2.FavoriteBack = "0";
                                    item2.FavoriteBackSize = "0";
                                    item2.FavoriteLay = "0";
                                    item2.FavoriteLaySize = "0";
                                    item2.FavoriteSelectionName = "";
                                    // 
                                    // 

                                    if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                    {
                                        item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                        var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                        int UserIDforLinevmarkets = 0;
                                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                                        {
                                            UserIDforLinevmarkets = 73;
                                        }
                                        else
                                        {
                                            UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                        }
                                        var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                                        if (linevmarkets.Count() > 0)
                                        {
                                            item2.LineVMarkets = linevmarkets;
                                        }
                                    }

                                    marketbooks.Add(item2);
                                }

                                List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]));

                                Session["userbets"] = lstUserBet;
                                Session["liabality"] = 0;
                                lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                                decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                                Session["totliabality"] = 0;
                                // Updateunmatchbets(marketbooks);
                                ViewBag.totliabalityNew = 0;
                                return RenderRazorViewToString("MarketBook", marketbooks);
                            }
                            else
                            {
                                List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]));

                                 Session["userbets"] = lstUserBet;
                                Session["liabality"] = 0;
                                
                                decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                                Session["totliabality"] = 0;
                                ViewBag.totliabalityNew = 0;
                                var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                                return RenderRazorViewToString("MarketBook", marketbooks);
                            }

                        }

                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8 )
                            {
                                ViewBag.backgrod = "#1D9BF0";
                                ViewBag.color = "white";
                                var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(73));
                                if (results != null)
                                {
                                    results = results.Where(item => item.ID == ID).ToList();
                                    var marketbooks = new List<MarketBook>();
                                    List<string> lstIDs = new List<string>();
                                    foreach (var item in results)
                                    {
                                        lstIDs = new List<string>();
                                        lstIDs.Add(item.ID);
                                        var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                                        if (marketbook.Count() > 0)

                                        {
                                            if (marketbook[0].Runners != null)
                                            {
                                                marketbooks.Add(marketbook[0]);
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }

                                    foreach (var item in results)
                                    {
                                        foreach (var item2 in marketbooks)
                                        {
                                            if (item.ID == item2.MarketId)
                                            {
                                                item2.MarketBookName = item.EventName + " / " + item.Name;
                                                item2.OrignalOpenDate = item.EventOpenDate;
                                                item2.MainSportsname = item.EventTypeName;
                                                item2.BettingAllowed = item.BettingAllowed;
                                                item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                                item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                                item2.EventID = item.EventID;
                                                Session["Eventid"] = item.EventID;


                                                var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                                foreach (var runnermarketitem in runnerdesc)
                                                {
                                                    foreach (var runneritem in item2.Runners)
                                                    {
                                                        if (runnermarketitem.SelectionID == runneritem.SelectionId)
                                                        {
                                                            runneritem.RunnerName = runnermarketitem.SelectionName;
                                                            runneritem.JockeyName = runnermarketitem.JockeyName;
                                                            runneritem.WearingURL = runnermarketitem.Wearing;
                                                            runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                            runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                            runneritem.StallDraw = runnermarketitem.StallDraw;

                                                        }
                                                    }
                                                }
                                                // 


                                                if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                                {
                                                    item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                                    var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                                    int UserIDforLinevmarkets = 0;
                                                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                                                    {
                                                        UserIDforLinevmarkets = 73;
                                                    }
                                                    else
                                                    {
                                                        UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                                    }
                                                    var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                                                    if (linevmarkets.Count() > 0)
                                                    {
                                                        item2.LineVMarkets = linevmarkets;
                                                    }
                                                }

                                                ////

                                            }
                                        }

                                    }
                                    if (marketbooks.Count == 0)
                                    {
                                        ExternalAPI.TO.MarketBook item2 = new ExternalAPI.TO.MarketBook();
                                        var item = results[0];
                                        item2.MarketId = item.ID;
                                        //item2.MarketBookName = item.Name + " / " + item.EventName;
                                        item2.MarketBookName = item.EventName + " / " + item.Name;
                                        item2.OrignalOpenDate = item.EventOpenDate;
                                        item2.MainSportsname = item.EventTypeName;
                                        item2.BettingAllowed = item.BettingAllowed;
                                        item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                        item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                        item2.EventID = item.EventID;
                                        DateTime OpenDate = item.EventOpenDate.AddHours(5);
                                        DateTime CurrentDate = DateTime.Now;
                                        TimeSpan remainingdays = (CurrentDate - OpenDate);
                                        if (OpenDate < CurrentDate)
                                        {
                                            item2.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                                        }
                                        else
                                        {
                                            item2.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                                        }
                                        item2.MarketStatusstr = "Active";

                                        var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                        item2.Runners = new List<ExternalAPI.TO.Runner>();

                                        foreach (var runnermarketitem in runnerdesc)
                                        {
                                            var runneritem = new ExternalAPI.TO.Runner();
                                            runneritem.SelectionId = runnermarketitem.SelectionID;
                                            runneritem.RunnerName = runnermarketitem.SelectionName;
                                            runneritem.JockeyName = runnermarketitem.JockeyName;
                                            runneritem.WearingURL = runnermarketitem.Wearing;
                                            runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                            runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                            runneritem.StallDraw = runnermarketitem.StallDraw;
                                            runneritem.ExchangePrices = new ExchangePrices();
                                            var lstpricelist = new List<PriceSize>();
                                            for (int i = 0; i < 3; i++)
                                            {
                                                var pricesize = new PriceSize();

                                                pricesize.Size = 0;

                                                pricesize.Price = 0;

                                                lstpricelist.Add(pricesize);
                                            }
                                            runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                                            lstpricelist = new List<PriceSize>();
                                            for (int i = 0; i < 3; i++)
                                            {
                                                var pricesize = new PriceSize();

                                                pricesize.Size = 0;

                                                pricesize.Price = 0;

                                                lstpricelist.Add(pricesize);
                                            }
                                            runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                                            item2.Runners.Add(runneritem);


                                        }
                                        item2.FavoriteID = "0";
                                        item2.FavoriteBack = "0";
                                        item2.FavoriteBackSize = "0";
                                        item2.FavoriteLay = "0";
                                        item2.FavoriteLaySize = "0";
                                        item2.FavoriteSelectionName = "";
                                        // 
                                       


                                        if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                        {
                                            item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                            var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                            int UserIDforLinevmarkets = 0;
                                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                                            {
                                                UserIDforLinevmarkets = 73;
                                            }
                                            else
                                            {
                                                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                            }
                                            var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                                            if (linevmarkets.Count() > 0)
                                            {
                                                item2.LineVMarkets = linevmarkets;
                                            }
                                        }

                                        ////

                                        if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                        {
                                            item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                            var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                            int UserIDforLinevmarkets = 0;
                                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                                            {
                                                UserIDforLinevmarkets = 73;
                                            }
                                            else
                                            {
                                                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                            }
                                            var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                                            if (linevmarkets.Count() > 0)
                                            {
                                                item2.LineVMarkets = linevmarkets;
                                            }
                                        }

                                        marketbooks.Add(item2);
                                    }

                                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]));

                                    Session["userbets"] = lstUserBet;
                                    Session["liabality"] = 0;
                                    lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                                    decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                                    Session["totliabality"] = 0;
                                    // Updateunmatchbets(marketbooks);
                                    ViewBag.totliabalityNew = 0;
                                    return RenderRazorViewToString("MarketBook", marketbooks);
                                }
                                else
                                {
                                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]));

                                    Session["userbets"] = lstUserBet;
                                    Session["liabality"] = 0;

                                    decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                                    Session["totliabality"] = 0;
                                    ViewBag.totliabalityNew = 0;
                                    var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                                    return RenderRazorViewToString("MarketBook", marketbooks);
                                }

                            }
                            if ( LoggedinUserDetail.GetUserTypeID() == 9)
                            {
                                ViewBag.backgrod = "#1D9BF0";
                                ViewBag.color = "white";
                                var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(73));
                                if (results != null)
                                {
                                    results = results.Where(item => item.ID == ID).ToList();
                                    var marketbooks = new List<MarketBook>();
                                    List<string> lstIDs = new List<string>();
                                    foreach (var item in results)
                                    {
                                        lstIDs = new List<string>();
                                        lstIDs.Add(item.ID);
                                        var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                                        if (marketbook.Count() > 0)

                                        {
                                            if (marketbook[0].Runners != null)
                                            {
                                                marketbooks.Add(marketbook[0]);
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }

                                    foreach (var item in results)
                                    {
                                        foreach (var item2 in marketbooks)
                                        {
                                            if (item.ID == item2.MarketId)
                                            {
                                                item2.MarketBookName = item.EventName + " / " + item.Name;
                                                item2.OrignalOpenDate = item.EventOpenDate;
                                                item2.MainSportsname = item.EventTypeName;
                                                item2.BettingAllowed = item.BettingAllowed;
                                                item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                                item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                                item2.EventID = item.EventID;
                                                Session["Eventid"] = item.EventID;


                                                var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                                foreach (var runnermarketitem in runnerdesc)
                                                {
                                                    foreach (var runneritem in item2.Runners)
                                                    {
                                                        if (runnermarketitem.SelectionID == runneritem.SelectionId)
                                                        {
                                                            runneritem.RunnerName = runnermarketitem.SelectionName;
                                                            runneritem.JockeyName = runnermarketitem.JockeyName;
                                                            runneritem.WearingURL = runnermarketitem.Wearing;
                                                            runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                            runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                            runneritem.StallDraw = runnermarketitem.StallDraw;

                                                        }
                                                    }
                                                }
                                                // 


                                                if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                                {
                                                    item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                                    var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                                    int UserIDforLinevmarkets = 0;
                                                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                                                    {
                                                        UserIDforLinevmarkets = 73;
                                                    }
                                                    else
                                                    {
                                                        UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                                    }
                                                    var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                                                    if (linevmarkets.Count() > 0)
                                                    {
                                                        item2.LineVMarkets = linevmarkets;
                                                    }
                                                }

                                                ////

                                            }
                                        }

                                    }
                                    if (marketbooks.Count == 0)
                                    {
                                        ExternalAPI.TO.MarketBook item2 = new ExternalAPI.TO.MarketBook();
                                        var item = results[0];
                                        item2.MarketId = item.ID;
                                        //item2.MarketBookName = item.Name + " / " + item.EventName;
                                        item2.MarketBookName = item.EventName + " / " + item.Name;
                                        item2.OrignalOpenDate = item.EventOpenDate;
                                        item2.MainSportsname = item.EventTypeName;
                                        item2.BettingAllowed = item.BettingAllowed;
                                        item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                        item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                        item2.EventID = item.EventID;
                                        DateTime OpenDate = item.EventOpenDate.AddHours(5);
                                        DateTime CurrentDate = DateTime.Now;
                                        TimeSpan remainingdays = (CurrentDate - OpenDate);
                                        if (OpenDate < CurrentDate)
                                        {
                                            item2.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                                        }
                                        else
                                        {
                                            item2.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                                        }
                                        item2.MarketStatusstr = "Active";

                                        var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                        item2.Runners = new List<ExternalAPI.TO.Runner>();

                                        foreach (var runnermarketitem in runnerdesc)
                                        {
                                            var runneritem = new ExternalAPI.TO.Runner();
                                            runneritem.SelectionId = runnermarketitem.SelectionID;
                                            runneritem.RunnerName = runnermarketitem.SelectionName;
                                            runneritem.JockeyName = runnermarketitem.JockeyName;
                                            runneritem.WearingURL = runnermarketitem.Wearing;
                                            runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                            runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                            runneritem.StallDraw = runnermarketitem.StallDraw;
                                            runneritem.ExchangePrices = new ExchangePrices();
                                            var lstpricelist = new List<PriceSize>();
                                            for (int i = 0; i < 3; i++)
                                            {
                                                var pricesize = new PriceSize();

                                                pricesize.Size = 0;

                                                pricesize.Price = 0;

                                                lstpricelist.Add(pricesize);
                                            }
                                            runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                                            lstpricelist = new List<PriceSize>();
                                            for (int i = 0; i < 3; i++)
                                            {
                                                var pricesize = new PriceSize();

                                                pricesize.Size = 0;

                                                pricesize.Price = 0;

                                                lstpricelist.Add(pricesize);
                                            }
                                            runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                                            item2.Runners.Add(runneritem);


                                        }
                                        item2.FavoriteID = "0";
                                        item2.FavoriteBack = "0";
                                        item2.FavoriteBackSize = "0";
                                        item2.FavoriteLay = "0";
                                        item2.FavoriteLaySize = "0";
                                        item2.FavoriteSelectionName = "";
                                        // 
                                        // 


                                        if (item2.MarketBookName.Contains("Match Odds") && item2.MainSportsname == "Cricket" && item2.MarketStatusstr != "Closed")
                                        {
                                            item2.CricketMatchKey = objUsersServiceCleint.GetCricketMatchKey(item2.MarketId);
                                            var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(item2.MarketId);
                                            int UserIDforLinevmarkets = 0;
                                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                                            {
                                                UserIDforLinevmarkets = 73;
                                            }
                                            else
                                            {
                                                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                                            }
                                            var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


                                            if (linevmarkets.Count() > 0)
                                            {
                                                item2.LineVMarkets = linevmarkets;
                                            }
                                        }

                                        ////

                                        marketbooks.Add(item2);
                                    }

                                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]));

                                    Session["userbets"] = lstUserBet;
                                    Session["liabality"] = 0;
                                    lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                                    decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                                    Session["totliabality"] = 0;
                                    // Updateunmatchbets(marketbooks);
                                    ViewBag.totliabalityNew = 0;
                                    return RenderRazorViewToString("MarketBook", marketbooks);
                                }
                                else
                                {
                                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]));

                                    Session["userbets"] = lstUserBet;
                                    Session["liabality"] = 0;
                                    lstUserBet = lstUserBet.Where(x => x.location != "9").ToList();
                                    decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                                    Session["totliabality"] = 0;
                                    ViewBag.totliabalityNew = 0;
                                    var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                                    return RenderRazorViewToString("MarketBook", marketbooks);
                                }

                            }

                            else
                            {
                                var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                                return RenderRazorViewToString("MarketBook", marketbooks);

                            }

                        }
                    }
                }
            } 

            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                return "";
            }
        }
        public string MarketBookToWinTheToss(string ID)
        {
            try
            {


                UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();

                //  return RenderRazorViewToString("MarketBook", marketbooks1);
                LoggedinUserDetail.CheckifUserLogin();
                

                if (ID != "" && LoggedinUserDetail.GetUserTypeID() != 1)
                {
                    
                    objUsersServiceCleint.SetMarketBookOpenbyUSer(LoggedinUserDetail.GetUserID(), ID);
                }
                if (ID != "" && LoggedinUserDetail.GetUserTypeID() == 1)
                {

                    objUsersServiceCleint.SetMarketBookOpenbyUSer(73, ID);
                }
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {

                    var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                    
                    if (results != null)
                    {
                        results = results.Where(item => item.ID == ID).ToList();
                        var marketbooks = new List<MarketBook>();
                        List<string> lstIDs = new List<string>();
                        foreach (var item in results)
                        {
                            lstIDs = new List<string>();

                            lstIDs.Add(item.ID);
                            var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                            if (marketbook.Count() > 0)

                            {
                                if (marketbook[0].Runners != null)
                                {
                                    marketbooks.Add(marketbook[0]);
                                }

                            }
                            else
                            {

                            }

                        }


                        foreach (var item in results)
                        {
                            foreach (var item2 in marketbooks)
                            {
                                if (item.ID == item2.MarketId)
                                {
                                    //item2.MarketBookName = item.Name + " / " + item.EventName;
                                    item2.MarketBookName = item.EventName + " / " + item.Name;
                                    item2.OrignalOpenDate = item.EventOpenDate;
                                    item2.MainSportsname = item.EventTypeName;
                                    item2.BettingAllowed = item.BettingAllowed;
                                    item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                    item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                    item2.EventID = item.EventID;


                                    var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                    foreach (var runnermarketitem in runnerdesc)
                                    {
                                        foreach (var runneritem in item2.Runners)
                                        {
                                            if (runnermarketitem.SelectionID == runneritem.SelectionId.Trim())
                                            {
                                                runneritem.RunnerName = runnermarketitem.SelectionName;
                                                runneritem.JockeyName = runnermarketitem.JockeyName;
                                                runneritem.WearingURL = runnermarketitem.Wearing;
                                                runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                runneritem.StallDraw = runnermarketitem.StallDraw;

                                            }
                                        }

                                    }

                                }
                            }

                        }
                        if (marketbooks.Count == 0)
                        {
                            ExternalAPI.TO.MarketBook item2 = new ExternalAPI.TO.MarketBook();
                            var item = results[0];
                            item2.MarketId = item.ID;
                            //item2.MarketBookName = item.Name + " / " + item.EventName;
                            item2.MarketBookName = item.EventName + " / " + item.Name;
                            item2.OrignalOpenDate = item.EventOpenDate;
                            item2.MainSportsname = item.EventTypeName;
                            item2.BettingAllowed = item.BettingAllowed;
                            item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                            item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                            item2.EventID = item.EventID;
                            DateTime OpenDate = item.EventOpenDate.AddHours(5);
                            DateTime CurrentDate = DateTime.Now;
                            TimeSpan remainingdays = (CurrentDate - OpenDate);
                            if (OpenDate < CurrentDate)
                            {
                                item2.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                            }
                            else
                            {
                                item2.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                            }
                            item2.MarketStatusstr = "Active";

                            var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                            item2.Runners = new List<ExternalAPI.TO.Runner>();

                            foreach (var runnermarketitem in runnerdesc)
                            {
                                var runneritem = new ExternalAPI.TO.Runner();
                                runneritem.SelectionId = runnermarketitem.SelectionID;
                                runneritem.RunnerName = runnermarketitem.SelectionName;
                                runneritem.JockeyName = runnermarketitem.JockeyName;
                                runneritem.WearingURL = runnermarketitem.Wearing;
                                runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                runneritem.StallDraw = runnermarketitem.StallDraw;
                                runneritem.ExchangePrices = new ExchangePrices();
                                var lstpricelist = new List<PriceSize>();
                                for (int i = 0; i < 3; i++)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = 0;

                                    pricesize.Price = 0;

                                    lstpricelist.Add(pricesize);
                                }
                                runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                                lstpricelist = new List<PriceSize>();
                                for (int i = 0; i < 3; i++)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = 0;

                                    pricesize.Price = 0;

                                    lstpricelist.Add(pricesize);
                                }
                                runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                                item2.Runners.Add(runneritem);
                            }
                            item2.FavoriteID = "0";
                            item2.FavoriteBack = "0";
                            item2.FavoriteBackSize = "0";
                            item2.FavoriteLay = "0";
                            item2.FavoriteLaySize = "0";
                            item2.FavoriteSelectionName = "";

                            marketbooks.Add(item2);
                        }
                        

                        return RenderRazorViewToString("MarketBookToWinTheToss", marketbooks);
                    }
                    else
                    {
                       
                        var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                        return RenderRazorViewToString("MarketBookToWinTheToss", marketbooks);
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {

                        var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                        if (results != null)
                        {
                            results = results.Where(item => item.ID == ID).ToList();
                            var marketbooks = new List<MarketBook>();
                            List<string> lstIDs = new List<string>();
                            foreach (var item in results)
                            {
                                lstIDs = new List<string>();

                                lstIDs.Add(item.ID);
                                var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                                if (marketbook.Count() > 0)

                                {
                                    if (marketbook[0].Runners != null)
                                    {
                                        marketbooks.Add(marketbook[0]);
                                    }

                                }
                                else
                                {

                                }

                            }


                            foreach (var item in results)
                            {
                                foreach (var item2 in marketbooks)
                                {
                                    if (item.ID == item2.MarketId)
                                    {
                                        //item2.MarketBookName = item.Name + " / " + item.EventName;
                                        item2.MarketBookName = item.EventName + " / " + item.Name;
                                        item2.OrignalOpenDate = item.EventOpenDate;
                                        item2.MainSportsname = item.EventTypeName;
                                        item2.BettingAllowed = item.BettingAllowed;
                                        item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                        item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                        item2.EventID = item.EventID;


                                        var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                        foreach (var runnermarketitem in runnerdesc)
                                        {
                                            foreach (var runneritem in item2.Runners)
                                            {
                                                if (runnermarketitem.SelectionID == runneritem.SelectionId)
                                                {
                                                    runneritem.RunnerName = runnermarketitem.SelectionName;
                                                    runneritem.JockeyName = runnermarketitem.JockeyName;
                                                    runneritem.WearingURL = runnermarketitem.Wearing;
                                                    runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                    runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                    runneritem.StallDraw = runnermarketitem.StallDraw;

                                                }
                                            }

                                        }


                                    }
                                }

                            }
                            if (marketbooks.Count == 0)
                            {
                                ExternalAPI.TO.MarketBook item2 = new ExternalAPI.TO.MarketBook();
                                var item = results[0];
                                item2.MarketId = item.ID;
                                //item2.MarketBookName = item.Name + " / " + item.EventName;
                                item2.MarketBookName = item.EventName + " / " + item.Name;
                                item2.OrignalOpenDate = item.EventOpenDate;
                                item2.MainSportsname = item.EventTypeName;
                                item2.BettingAllowed = item.BettingAllowed;
                                item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                item2.EventID = item.EventID;
                                DateTime OpenDate = item.EventOpenDate.AddHours(5);
                                DateTime CurrentDate = DateTime.Now;
                                TimeSpan remainingdays = (CurrentDate - OpenDate);
                                if (OpenDate < CurrentDate)
                                {
                                    item2.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                                }
                                else
                                {
                                    item2.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                                }
                                item2.MarketStatusstr = "Active";

                                var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                item2.Runners = new List<ExternalAPI.TO.Runner>();

                                foreach (var runnermarketitem in runnerdesc)
                                {
                                    var runneritem = new ExternalAPI.TO.Runner();
                                    runneritem.SelectionId = runnermarketitem.SelectionID;
                                    runneritem.RunnerName = runnermarketitem.SelectionName;
                                    runneritem.JockeyName = runnermarketitem.JockeyName;
                                    runneritem.WearingURL = runnermarketitem.Wearing;
                                    runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                    runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                    runneritem.StallDraw = runnermarketitem.StallDraw;
                                    runneritem.ExchangePrices = new ExchangePrices();
                                    var lstpricelist = new List<PriceSize>();
                                    for (int i = 0; i < 3; i++)
                                    {
                                        var pricesize = new PriceSize();

                                        pricesize.Size = 0;

                                        pricesize.Price = 0;

                                        lstpricelist.Add(pricesize);
                                    }
                                    runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                                    lstpricelist = new List<PriceSize>();
                                    for (int i = 0; i < 3; i++)
                                    {
                                        var pricesize = new PriceSize();

                                        pricesize.Size = 0;

                                        pricesize.Price = 0;

                                        lstpricelist.Add(pricesize);
                                    }
                                    runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                                    item2.Runners.Add(runneritem);

                                }
                                item2.FavoriteID = "0";
                                item2.FavoriteBack = "0";
                                item2.FavoriteBackSize = "0";
                                item2.FavoriteLay = "0";
                                item2.FavoriteLaySize = "0";
                                item2.FavoriteSelectionName = "";
                                // 

                                marketbooks.Add(item2);
                            }
                            
                            return RenderRazorViewToString("MarketBookToWinTheToss", marketbooks);
                        }
                        else
                        {



                            var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                            return RenderRazorViewToString("MarketBookToWinTheToss", marketbooks);
                        }
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {

                            var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(73));
                            if (results != null)
                            {
                                results = results.Where(item => item.ID == ID).ToList();
                                var marketbooks = new List<MarketBook>();
                                List<string> lstIDs = new List<string>();
                                foreach (var item in results)
                                {
                                    lstIDs = new List<string>();

                                    lstIDs.Add(item.ID);
                                    var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                                    if (marketbook.Count() > 0)

                                    {
                                        if (marketbook[0].Runners != null)
                                        {
                                            marketbooks.Add(marketbook[0]);
                                        }

                                    }
                                    else
                                    {

                                    }

                                }


                                foreach (var item in results)
                                {
                                    foreach (var item2 in marketbooks)
                                    {
                                        if (item.ID == item2.MarketId)
                                        {
                                            //item2.MarketBookName = item.Name + " / " + item.EventName;
                                            item2.MarketBookName = item.EventName + " / " + item.Name;
                                            item2.OrignalOpenDate = item.EventOpenDate;
                                            item2.MainSportsname = item.EventTypeName;
                                            item2.BettingAllowed = item.BettingAllowed;
                                            item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                            item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                            item2.EventID = item.EventID;


                                            var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                            foreach (var runnermarketitem in runnerdesc)
                                            {
                                                foreach (var runneritem in item2.Runners)
                                                {
                                                    if (runnermarketitem.SelectionID == runneritem.SelectionId)
                                                    {
                                                        runneritem.RunnerName = runnermarketitem.SelectionName;
                                                        runneritem.JockeyName = runnermarketitem.JockeyName;
                                                        runneritem.WearingURL = runnermarketitem.Wearing;
                                                        runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                        runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                        runneritem.StallDraw = runnermarketitem.StallDraw;

                                                    }
                                                }

                                            }

                                        }
                                    }

                                }
                                if (marketbooks.Count == 0)
                                {
                                    ExternalAPI.TO.MarketBook item2 = new ExternalAPI.TO.MarketBook();
                                    var item = results[0];
                                    item2.MarketId = item.ID;
                                    //item2.MarketBookName = item.Name + " / " + item.EventName;
                                    item2.MarketBookName = item.EventName + " / " + item.Name;
                                    item2.OrignalOpenDate = item.EventOpenDate;
                                    item2.MainSportsname = item.EventTypeName;
                                    item2.BettingAllowed = item.BettingAllowed;
                                    item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                    item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                    item2.EventID = item.EventID;
                                    DateTime OpenDate = item.EventOpenDate.AddHours(5);
                                    DateTime CurrentDate = DateTime.Now;
                                    TimeSpan remainingdays = (CurrentDate - OpenDate);
                                    if (OpenDate < CurrentDate)
                                    {
                                        item2.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
                                    }
                                    else
                                    {
                                        item2.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
                                    }
                                    item2.MarketStatusstr = "Active";

                                    var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                    item2.Runners = new List<ExternalAPI.TO.Runner>();

                                    foreach (var runnermarketitem in runnerdesc)
                                    {
                                        var runneritem = new ExternalAPI.TO.Runner();
                                        runneritem.SelectionId = runnermarketitem.SelectionID;
                                        runneritem.RunnerName = runnermarketitem.SelectionName;
                                        runneritem.JockeyName = runnermarketitem.JockeyName;
                                        runneritem.WearingURL = runnermarketitem.Wearing;
                                        runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                        runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                        runneritem.StallDraw = runnermarketitem.StallDraw;
                                        runneritem.ExchangePrices = new ExchangePrices();
                                        var lstpricelist = new List<PriceSize>();
                                        for (int i = 0; i < 3; i++)
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = 0;

                                            pricesize.Price = 0;

                                            lstpricelist.Add(pricesize);
                                        }
                                        runneritem.ExchangePrices.AvailableToBack = lstpricelist;
                                        lstpricelist = new List<PriceSize>();
                                        for (int i = 0; i < 3; i++)
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = 0;

                                            pricesize.Price = 0;

                                            lstpricelist.Add(pricesize);
                                        }
                                        runneritem.ExchangePrices.AvailableToLay = lstpricelist;
                                        item2.Runners.Add(runneritem);


                                    }
                                    item2.FavoriteID = "0";
                                    item2.FavoriteBack = "0";
                                    item2.FavoriteBackSize = "0";
                                    item2.FavoriteLay = "0";
                                    item2.FavoriteLaySize = "0";
                                    item2.FavoriteSelectionName = "";
                                    // 

                                    marketbooks.Add(item2);
                                }

                             
                                return RenderRazorViewToString("MarketBookToWinTheToss", marketbooks);
                            }
                            else
                            {
                                var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                                return RenderRazorViewToString("MarketBookToWinTheToss", marketbooks);
                            }

                        }
                        else
                        {
                            var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                            return RenderRazorViewToString("MarketBookToWinTheToss", marketbooks);

                        }


                    }
                }
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                return "";
            }
        }

        public string MarketBooksoccergoal(string EventID)
        {
            try
            {
                List<string> data = new List<string>();
                UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                //  return RenderRazorViewToString("MarketBook", marketbooks1);
                LoggedinUserDetail.CheckifUserLogin();
                var Soccergoalmarket = objUsersServiceCleint.GetSoccergoalbyeventId(LoggedinUserDetail.GetUserID(), EventID);
        
                if (Soccergoalmarket != null)
                {
                    foreach (var item in Soccergoalmarket)
                    {
                        //  data.Add(item.MarketCatalogueID);
                        if (item.MarketCatalogueID != "" && LoggedinUserDetail.GetUserTypeID() != 1)
                        {
                            objUsersServiceCleint.SetMarketBookOpenbyUSer(LoggedinUserDetail.GetUserID(), item.MarketCatalogueID);
                        }
                        if (item.MarketCatalogueID != "" && LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            objUsersServiceCleint.SetMarketBookOpenbyUSer(73, item.MarketCatalogueID);
                        }
                    }                 
                }
                
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));

                    if (results != null)
                    {
                        results = results.Where(item => item.EventID == EventID && item.Name != "Match Odds").ToList();
                        var marketbooks = new List<MarketBook>();
                        List<string> lstIDs = new List<string>();
                        foreach (var item in results)
                        {
                            lstIDs = new List<string>();

                            lstIDs.Add(item.ID);
                            var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                            if (marketbook.Count() > 0)

                            {
                                if (marketbook[0].Runners != null)
                                {
                                    marketbooks.Add(marketbook[0]);
                                }
                            }
                            else
                            {

                            }
                        }

                        foreach (var item in results)
                        {
                            foreach (var item2 in marketbooks)
                            {
                                if (item.ID == item2.MarketId)
                                {
                                    //item2.MarketBookName = item.Name + " / " + item.EventName;
                                    item2.MarketBookName = item.EventName + " / " + item.Name;
                                    item2.OrignalOpenDate = item.EventOpenDate;
                                    item2.MainSportsname = item.EventTypeName;
                                    item2.MarketStatusstr = item2.MarketStatusstr;
                                    item2.BettingAllowed = item.BettingAllowed;
                                    item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                    item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                    item2.EventID = item.EventID;

                                    var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                    foreach (var runnermarketitem in runnerdesc)
                                    {
                                        foreach (var runneritem in item2.Runners)
                                        {
                                            if (runnermarketitem.SelectionID == runneritem.SelectionId.Trim())
                                            {
                                                runneritem.RunnerName = runnermarketitem.SelectionName;
                                                runneritem.JockeyName = runnermarketitem.JockeyName;
                                                runneritem.WearingURL = runnermarketitem.Wearing;
                                                runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                runneritem.StallDraw = runnermarketitem.StallDraw;

                                            }
                                            var lstUserBet = (List<UserBets>)Session["userbet"];
                                            List<UserBets> lstUserBets = lstUserBet.Where(item3 => item3.isMatched == true && item3.MarketBookID == item2.MarketId).ToList();
                                            item2.DebitCredit = objUserBets.ceckProfitandLoss(item2, lstUserBets);
                                            runneritem.ProfitandLoss = Convert.ToInt64(item2.DebitCredit.Where(item5 => item5.SelectionID == runneritem.SelectionId).Sum(item5 => item5.Debit) - item2.DebitCredit.Where(item5 => item5.SelectionID == runneritem.SelectionId).Sum(item5 => item5.Credit));

                                        }
                                    }

                                }
                            }

                        }

                        return RenderRazorViewToString("MarketBookSoccerGoal", marketbooks.Take(1));
                    }                  
                }
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                    if (results != null)
                    {
                        results = results.Where(item => item.EventID == EventID && item.Name != "Match Odds").ToList();
                        var marketbooks = new List<MarketBook>();
                        List<string> lstIDs = new List<string>();
                        foreach (var item in results)
                        {
                            lstIDs = new List<string>();

                            lstIDs.Add(item.ID);
                            var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                            if (marketbook.Count() > 0)

                            {
                                if (marketbook[0].Runners != null)
                                {
                                    marketbooks.Add(marketbook[0]);
                                }
                            }
                            else
                            {

                            }
                        }

                        foreach (var item in results)
                        {
                            foreach (var item2 in marketbooks)
                            {
                                if (item.ID == item2.MarketId)
                                {
                                    //item2.MarketBookName = item.Name + " / " + item.EventName;
                                    item2.MarketBookName = item.EventName + " / " + item.Name;
                                    item2.OrignalOpenDate = item.EventOpenDate;
                                    item2.MainSportsname = item.EventTypeName;
                                    item2.BettingAllowed = item.BettingAllowed;
                                    item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                    item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                    item2.EventID = item.EventID;
                                    var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                    foreach (var runnermarketitem in runnerdesc)
                                    {
                                        var lstUserBet = (List<UserBetsforAgent>)Session["userbets"];
                                        List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item3 => item3.isMatched == true && item3.MarketBookID == item2.MarketId).ToList();
                                        var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                                        foreach (var userid in lstUsers)
                                        {
                                            // var agentrate = 0;
                                            List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item4 => item4.UserID == Convert.ToInt32(userid.UserID)).ToList();
                                            item2.DebitCredit = objUserBets.ceckProfitandLossAgent(item2, lstuserbet);
                                            foreach (var runneritem in item2.Runners)
                                            {
                                                if (runnermarketitem.SelectionID == runneritem.SelectionId)
                                                {
                                                    runneritem.RunnerName = runnermarketitem.SelectionName;
                                                    runneritem.JockeyName = runnermarketitem.JockeyName;
                                                    runneritem.WearingURL = runnermarketitem.Wearing;
                                                    runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                    runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                    runneritem.StallDraw = runnermarketitem.StallDraw;
                                                }
                                                long profitorloss = Convert.ToInt64(item2.DebitCredit.Where(item5 => item5.SelectionID == runneritem.SelectionId).Sum(item5 => item5.Debit) - item2.DebitCredit.Where(item5 => item5.SelectionID == runneritem.SelectionId).Sum(item5 => item5.Credit));

                                                //  decimal profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
                                                decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                                runneritem.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        return RenderRazorViewToString("MarketBookSoccerGoal", marketbooks);
                    }
                }

                if (LoggedinUserDetail.GetUserTypeID() == 1)

                {

                    var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(73));
                    if (results != null)
                    {
                        results = results.Where(item => item.EventID == EventID && item.Name != "Match Odds").ToList();
                        var marketbooks = new List<MarketBook>();
                        List<string> lstIDs = new List<string>();
                        foreach (var item in results)
                        {
                            lstIDs = new List<string>();

                            lstIDs.Add(item.ID);
                            var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                            if (marketbook.Count() > 0)

                            {
                                if (marketbook[0].Runners != null)
                                {
                                    marketbooks.Add(marketbook[0]);
                                }
                            }
                            else
                            {

                            }
                        }
                        foreach (var item in results)
                        {
                            foreach (var item2 in marketbooks)
                            {
                                if (item.ID == item2.MarketId)
                                {
                                    //item2.MarketBookName = item.Name + " / " + item.EventName;
                                    item2.MarketBookName = item.EventName + " / " + item.Name;
                                    item2.OrignalOpenDate = item.EventOpenDate;
                                    item2.MainSportsname = item.EventTypeName;
                                    item2.BettingAllowed = item.BettingAllowed;
                                    item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                    item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                    item2.EventID = item.EventID;
                                    var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                    foreach (var runnermarketitem in runnerdesc)
                                    {
                                        List<UserBetsForAdmin> lstUserBet = (List<UserBetsForAdmin>)Session["userbets"];
                                        List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item3 => item3.isMatched == true && item3.MarketBookID == item2.MarketId).ToList();
                                        var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                                        foreach (var userid in lstUsers)
                                        {
                                            // var agentrate = 0;
                                            List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item4 => item4.UserID == Convert.ToInt32(userid.UserID)).ToList();
                                            var agentrate = lstuserbet[0].AgentRate;
                                            bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                            item2.DebitCredit = objUserBets.ceckProfitandLossAdmin(item2, lstuserbet);
                                            foreach (var runneritem in item2.Runners)
                                            {
                                                if (runnermarketitem.SelectionID == runneritem.SelectionId)
                                                {
                                                    runneritem.RunnerName = runnermarketitem.SelectionName;
                                                    runneritem.JockeyName = runnermarketitem.JockeyName;
                                                    runneritem.WearingURL = runnermarketitem.Wearing;
                                                    runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                    runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                    runneritem.StallDraw = runnermarketitem.StallDraw;
                                                }
                                                long profitorloss = Convert.ToInt64(item2.DebitCredit.Where(item5 => item5.SelectionID == runneritem.SelectionId).Sum(item5 => item5.Debit) - item2.DebitCredit.Where(item5 => item5.SelectionID == runneritem.SelectionId).Sum(item5 => item5.Credit));

                                                decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                                decimal profit = (adminrate / 100) * profitorloss;
                                                runneritem.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        return RenderRazorViewToString("MarketBookSoccerGoal", marketbooks);

                    }
                }



                if (LoggedinUserDetail.GetUserTypeID() == 8 )
                {
                    var results = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(73));
                    if (results != null)
                    {
                        results = results.Where(item => item.EventID == EventID && item.Name != "Match Odds").ToList();
                        var marketbooks = new List<MarketBook>();
                        List<string> lstIDs = new List<string>();
                        foreach (var item in results)
                        {
                            lstIDs = new List<string>();

                            lstIDs.Add(item.ID);
                            var marketbook = GetMarketDatabyID(lstIDs.ToArray(), item.Name, item.EventOpenDate, item.EventTypeName);
                            if (marketbook.Count() > 0)

                            {
                                if (marketbook[0].Runners != null)
                                {
                                    marketbooks.Add(marketbook[0]);
                                }
                            }
                            else
                            {

                            }
                        }
                        foreach (var item in results)
                        {
                            foreach (var item2 in marketbooks)
                            {
                                if (item.ID == item2.MarketId)
                                {
                                    //item2.MarketBookName = item.Name + " / " + item.EventName;
                                    item2.MarketBookName = item.EventName + " / " + item.Name;
                                    item2.OrignalOpenDate = item.EventOpenDate;
                                    item2.MainSportsname = item.EventTypeName;
                                    item2.BettingAllowed = item.BettingAllowed;
                                    item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                    item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                    item2.EventID = item.EventID;
                                    var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(item2.MarketId);
                                    foreach (var runnermarketitem in runnerdesc)
                                    {
                                      
                                       
                                        string userbets = objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforSuper>>(userbets);
                                        List<UserBetsforSuper> lstUserBets = lstUserBet.Where(item3 => item3.isMatched == true && item3.MarketBookID == marketbooks[0].MarketId).ToList();

                                        var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
                                        foreach (var userid in lstUsers)
                                        {
                                            
                                            List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item4 => item4.UserID == Convert.ToInt32(userid.UserID)).ToList();
                                            var agentrate = lstuserbet[0].AgentRate;
                                            var supertrate = lstuserbet[0].SuperAgentRateB;
                                            bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                            var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                                            var superpercent = 0;
                                            if (supertrate > 0)
                                            {
                                                superpercent = supertrate - Convert.ToInt32(agentrate);
                                            }
                                            else
                                            {
                                                superpercent = 0;
                                            }

                                            var adminpercent = 100 - (superpercent + Convert.ToInt32(agentrate) + TransferAdminPercentage);

                                            item2.DebitCredit = objUserBets.ceckProfitandLossSuper(marketbooks[0], lstuserbet);

                                            foreach (var runneritem in item2.Runners)
                                            {
                                                if (runnermarketitem.SelectionID == runneritem.SelectionId)
                                                {
                                                    runneritem.RunnerName = runnermarketitem.SelectionName;
                                                    runneritem.JockeyName = runnermarketitem.JockeyName;
                                                    runneritem.WearingURL = runnermarketitem.Wearing;
                                                    runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                    runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                    runneritem.StallDraw = runnermarketitem.StallDraw;
                                                }
                                                long profitorloss = Convert.ToInt64(marketbooks[0].DebitCredit.Where(item4 => item4.SelectionID == runneritem.SelectionId).Sum(item4 => item4.Debit) - marketbooks[0].DebitCredit.Where(item4 => item4.SelectionID == runneritem.SelectionId).Sum(item4 => item4.Credit));
                                                if (marketbooks[0].Runners.Count == 1)
                                                {
                                                    if (profitorloss > 0)
                                                    {
                                                        profitorloss = -1 * profitorloss;
                                                    }
                                                }
                                                decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                                                decimal profit = (adminrate / 100) * profitorloss;
                                                runneritem.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        return RenderRazorViewToString("MarketBookSoccerGoal", marketbooks);
                    }
                    else
                    {
                        var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                        return RenderRazorViewToString("MarketBookSoccerGoal", marketbooks);

                    }
                }
                else
                {
                    var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                    return RenderRazorViewToString("MarketBookToWinTheToss", marketbooks);
                }
                   
                
            }
            catch (System.Exception ex)
            {
             
                    var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                    return RenderRazorViewToString("MarketBookSoccerGoal", marketbooks);           
            }
        }

        private JsonResult SerializeControl(string controlPath, object model)
        {
            ViewPage page = new ViewPage();
            ViewUserControl ctl = (ViewUserControl)page.LoadControl(controlPath);
            page.Controls.Add(ctl);
            page.ViewData.Model = model;
            page.ViewContext = new ViewContext();
            System.IO.StringWriter writer = new System.IO.StringWriter();
            System.Web.HttpContext.Current.Server.Execute(page, writer, false);
            string outputToReturn = writer.ToString();
            writer.Close();
            return this.Json(outputToReturn.Trim());
        }
        //public PartialViewResult InPlayMatches()
        //{
        //    if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8)
        //    {
        //        List<Models.InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<Models.InPlayMatches>>(objUsersServiceCleint.GetInPlayMatches(LoggedinUserDetail.GetUserID()));
        //        return PartialView("InPlayMatches", lstInPlayMatches);
        //    }
        //    else
        //    {
        //        List<Models.InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<Models.InPlayMatches>>(objUsersServiceCleint.GetInPlayMatches(73));
        //        return PartialView("InPlayMatches", lstInPlayMatches);
        //    }

        //}
        public PartialViewResult AllMatches()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8)
            {
                List<Models.InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<Models.InPlayMatches>>(objUsersServiceCleint.GetAllMatches(LoggedinUserDetail.GetUserID()));
                return PartialView("InPlayMatches", lstInPlayMatches);
            }
            else
            {
                List<Models.InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<Models.InPlayMatches>>(objUsersServiceCleint.GetAllMatches(73));
                return PartialView("InPlayMatches", lstInPlayMatches);
            }

        }

        public string AddtoFavoritesEventType(string eventtypeID)
        {
            objUsersServiceCleint.AddtoFavoriteEventTypes(eventtypeID, LoggedinUserDetail.GetUserID());
            return "True";
        }

        public string RemoveFavoritesEventType(string eventtypeID)
        {
            objUsersServiceCleint.DeleteFromFavoriteEventTypes(eventtypeID, LoggedinUserDetail.GetUserID());
            return "True";
        }

        public string AddtoFavoritesEvent(string eventID)
        {
            objUsersServiceCleint.AddtoFavoriteEvents(eventID, LoggedinUserDetail.GetUserID());
            return "True";
        }

        public string RemoveFromFavoritesEvent(string eventID)
        {
            objUsersServiceCleint.DeleteFromFavoriteEvents(eventID, LoggedinUserDetail.GetUserID());
            return "True";
        }

        public string AddtoFavoritesCompetition(string competitionID)
        {
            objUsersServiceCleint.AddtoFavoriteCompetitions(competitionID, LoggedinUserDetail.GetUserID());
            return "True";
        }

        public string RemoveFromFavoritesCompetition(string competitionID)
        {
            objUsersServiceCleint.DeleteFromFavoriteCompetitions(competitionID, LoggedinUserDetail.GetUserID());
            return "True";
        }

        //      public string UserBets()
        //      {
        //          try
        //          {
        //              if (LoggedinUserDetail.GetUserTypeID() == 3)
        //              {

        //                  List<UserBets> lstUserBets = (List<UserBets>)Session["userbets"];
        //long Liabality =objUserBets. GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBets);
        //                  ViewData["liabality"] = Liabality;
        //                  ViewData["totliabality"] = Session["totliabality"];

        //                  return RenderRazorViewToString("UserBets", lstUserBets);

        //              }
        //              else
        //              {
        //                  if (LoggedinUserDetail.GetUserTypeID() == 2)
        //                  {

        //                      List<UserBetsforAgent> lstUserBets = (List<UserBetsforAgent>)Session["userbets"];

        //                      long Liabality = 0;




        //                      ViewData["liabality"] = Liabality;
        //                      ViewData["totliabality"] = (decimal)Session["totliabality"];

        //                      return RenderRazorViewToString("UserBetsAgent", lstUserBets);
        //                  }
        //                  else
        //                  {
        //                      if (LoggedinUserDetail.GetUserTypeID() == 1)
        //                      {


        //                          List<UserBetsForAdmin> lstUserBets = (List<UserBetsForAdmin>)Session["userbets"];

        //                          long Liabality = 0;




        //                          ViewData["liabality"] = Liabality;
        //                          ViewData["totliabality"] = (decimal)Session["totliabality"];

        //                          return RenderRazorViewToString("UserBetsForAdmin", lstUserBets);
        //                      }
        //                      else
        //                      {
        //                          List<UserBets> lstUserBets = new List<UserBets>();
        //                          return RenderRazorViewToString("UserBets", lstUserBets);
        //                      }

        //                  }
        //              }

        //          }
        //          catch (System.Exception ex)
        //          {
        //              List<UserBets> lstUserBets = new List<UserBets>();
        //              return RenderRazorViewToString("UserBets", lstUserBets);
        //          }
        //      }

        //      public string CheckforPlaceBet(string Amount, string Odd, string BetType, string[] SelectionID, string marketbookID)
        //      {
        //          try
        //          {
        //              if (LoggedinUserDetail.GetUserTypeID() == 3)
        //              {
        //                  //List<UserLiabality> lstUserLiabality = JsonConvert.DeserializeObject<List<Models.UserLiabality>>(objUsersServiceCleint.GetCurrentLiabality(LoggedinUserDetail.GetUserID()));
        //                  decimal TotLiabality = 0;
        //                  foreach (var selectionIDitem in SelectionID)
        //                  {
        //                      TotLiabality +=objUserBets. GetLiabalityofCurrentUserActual(LoggedinUserDetail.GetUserID(), selectionIDitem, BetType, marketbookID);
        //                  }
        //                  TotLiabality +=objUserBets. GetLiabalityofCurrentUserActualforOtherMarkets(LoggedinUserDetail.GetUserID(), "", BetType, marketbookID);

        //                  decimal CurrentBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.GetUserID()));
        //                  decimal TotBalance = CurrentBalance + TotLiabality;
        //                  if (TotBalance >= Convert.ToDecimal(Amount))
        //                  {
        //                      return "True";
        //                  }
        //                  else
        //                  {
        //                      return "Available balance is less then your amount";
        //                  }

        //              }
        //              else
        //              {
        //                  return "You are not allowed to perform this operation.";
        //              }
        //          }
        //          catch (System.Exception ex)
        //          {
        //              LoggedinUserDetail.LogError(ex);
        //              return "False";
        //          }
        //      }
        //      public string InsertUserBet(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string marketbookid, string marketbookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID)
        //      {
        //          try
        //          {
        //              if (LoggedinUserDetail.GetUserTypeID() == 3)
        //              {
        //                  selecitonname = selecitonname.Trim();
        //                  var ID = objUsersServiceCleint.InsertUserBet(SelectionID, LoggedinUserDetail.GetUserID(), userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, selecitonname, marketbookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID);
        //                  return "True" + "|" + ID.ToString();
        //              }
        //              else
        //              {
        //                  return "You are not allowed to perform this operation";
        //              }
        //          }
        //          catch (System.Exception ex)
        //          {
        //              return "False";
        //          }


        //      }
        //      public string InsertUserBetAdmin(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string marketbookid, string marketbookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID,int UserID)
        //      {
        //          try
        //          {
        //              if (LoggedinUserDetail.GetUserTypeID() != 3)
        //              {
        //                  selecitonname = selecitonname.Trim();
        //                  var ID = objUsersServiceCleint.InsertUserBet(SelectionID, UserID, userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, selecitonname, marketbookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID);
        //                  return "True" + "|" + ID.ToString();
        //              }
        //              else
        //              {
        //                  return "You are not allowed to perform this operation";
        //              }
        //          }
        //          catch (System.Exception ex)
        //          {
        //              return "False";
        //          }


        //      }


        public string GetMarketStatus(string marketbookID)
        {
            return "True";
            //var marketbooks = client.listMarketBook(marketbookID, LoggedinUserDetail.GetUserID(), LoggedinUserDetail.GetUserTypeID());
            //if (marketbooks[0].IsInplay == true)
            //{
            //    return "True";
            //}
            //else
            //{
            //    return "False";
            //}
            //foreach (var item in marketbooks)
            //{

            //}

        }

        public string SetMarketClosedbyUser(string marketbookID)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 3)
            {
                objUsersServiceCleint.SetMarketBookClosedbyUser(LoggedinUserDetail.GetUserID(), marketbookID);


            }
            return "True";
            //else
            //{
            //    objUsersServiceCleint.SetMarketClosedAllUsers(marketbookID);
            //    return "True";
            //}

        }

    }
}