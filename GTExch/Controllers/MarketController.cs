using BettingServiceReference;
using GTExch.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExternalAPI.TO;
using UsersServiceReference;

namespace GTExch.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceClient objBettingClient = new BettingServiceClient();
        //public static wsnew ws1 = new wsnew();

        //public static wsnew ws2 = new wsnew();

        //public static wsnew ws4 = new wsnew();

        //public static wsnew ws7 = new wsnew();

        //public static wsnew ws0 = new wsnew();

        //public static wsnew ws4339 = new wsnew();
        //public static wsnew wsFancy = new wsnew();

        //private wsnew wsBFMatch = new wsnew();

        [HttpGet]
        public void SetURLsData()
        {
            LoggedinUserDetail.URLsData = JsonConvert.DeserializeObject<List<GTExch.Models.SP_URLsData_GetAllData_Result>>(objUsersServiceCleint.GetURLsData());
            //ws1.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Soccer").FirstOrDefault().URLForData;
            //ws2.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Tennis").FirstOrDefault().URLForData;
            //ws4.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().URLForData;
            //ws7.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Horse Racing").FirstOrDefault().URLForData;
            //ws4339.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "GreyHound Racing").FirstOrDefault().URLForData;
            //wsFancy.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Fancy").FirstOrDefault().URLForData;

            //ws0.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Other").FirstOrDefault().URLForData;
            // LoggedinUserDetail.SecurityCode = LoggedinUserDetail.URLsData.FirstOrDefault().Scd;
            LoggedinUserDetail.GetCricketDataFrom = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().GetDataFrom;
        }
        public static string strWsMatch = "";
       
        public MarketBook GetCurrentMarketBook(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate)
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
                    var marketbook = objBettingClient.GetMarketDatabyID(marketIds.ToList(), sheetname, marketopendate, MainSportsCategory, LoggedinUserDetail.PasswordForValidate);
                    if (marketbook.Count() > 0)
                    {
                        return marketbook[0];
                    }
                    else
                    {
                        return new MarketBook();
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetCricketDataFrom == "BP")
                    {
                        var marketbook = objBettingClient.GetMarketDatabyID(marketIds.ToList(), sheetname, marketopendate, MainSportsCategory, LoggedinUserDetail.PasswordForValidate);
                        if (marketbook.Count() > 0)
                        {
                            return marketbook[0];
                        }
                        else
                        {
                            return new MarketBook();
                        }
                    }
                    else
                    {
                        var marketbook = objBettingClient.GetMarketDatabyID(marketIds.ToList(), sheetname, marketopendate, MainSportsCategory, LoggedinUserDetail.PasswordForValidate);
                        if (marketbook.Count() > 0)
                        {
                            return marketbook[0];
                        }
                        else
                        {
                            return new MarketBook();
                        }
                        //IList<bfnexchange.wrBF.MarketBook> list;
                        //if (MainSportsCategory == "Soccer")
                        //{
                        //    list = ws1.GD(LoggedinUserDetail.SecurityCode, marketIds, 1);
                        //}
                        //else if (MainSportsCategory == "Tennis")
                        //{
                        //    list = ws2.GD(LoggedinUserDetail.SecurityCode, marketIds, 2);
                        //}
                        //else if (MainSportsCategory == "Cricket")
                        //{
                        //    list = ws4.GD(LoggedinUserDetail.SecurityCode, marketIds, 4);
                        //}
                        //else if (MainSportsCategory.Contains("Horse Racing"))
                        //{
                        //    list = ws7.GD(LoggedinUserDetail.SecurityCode, marketIds, 7);
                        //}
                        //else if (MainSportsCategory.Contains("Greyhound Racing"))
                        //{
                        //    list = ws4339.GD(LoggedinUserDetail.SecurityCode, marketIds, 13);
                        //}
                        //else
                        //{
                        //    list = ws0.GD(LoggedinUserDetail.SecurityCode, marketIds, 0);                            
                        //}
                        //if (list.Count > 0)
                        //{
                         // MarketBook objMarketbook = ConvertJsontoMarketObjectBF(list[0], marketid, marketopendate, sheetname, MainSportsCategory);
                        //    return objMarketbook;
                        //}
                        //else
                        //{
                        //    return new MarketBook();
                        //}
                    }
                }
            }
            catch (System.Exception ex)
            {
                //APIConfig.LogError(ex);
                return new MarketBook();
            }
        }
        //public MarketBook ConvertJsontoMarketObjectBF(bfnexchange.wrBF.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory)
        //{
        //    try
        //    {
        //        if (1 == 1)
        //        {
        //            var marketbook = new MarketBook();

        //            marketbook.MarketId = BFMarketbook.MarketId;
        //            marketbook.SheetName = "";
        //            marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
        //            marketbook.PoundRate = objUsersServiceCleint.GetPoundRatebyUserID(LoggedinUserDetail.GetUserID());
        //            marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
        //            marketbook.MarketBookName = sheetname;
        //            marketbook.MainSportsname = MainSportsCategory;
        //            marketbook.OrignalOpenDate = marketopendate;
        //            marketbook.Version = BFMarketbook.Version;
        //            marketbook.TotalMatched = BFMarketbook.TotalMatched;
        //            DateTime OpenDate = marketbook.OrignalOpenDate.Value.AddHours(5);
        //            DateTime CurrentDate = DateTime.Now;
        //            TimeSpan remainingdays = (CurrentDate - OpenDate);
        //            if (OpenDate < CurrentDate)
        //            {
        //                marketbook.OpenDate = "-" + remainingdays.Days.ToString() + ":" + remainingdays.Hours.ToString() + ":" + remainingdays.Minutes.ToString() + ":" + remainingdays.Seconds.ToString();
        //            }
        //            else
        //            {
        //                marketbook.OpenDate = (-1 * remainingdays.Days).ToString() + ":" + (-1 * remainingdays.Hours).ToString() + ":" + (-1 * remainingdays.Minutes).ToString() + ":" + (-1 * remainingdays.Seconds).ToString();
        //            }

        //            if (BFMarketbook.IsInplay == true && BFMarketbook.Status.ToString() == "OPEN")
        //            {

        //                marketbook.MarketStatusstr = "In Play";
        //            }
        //            else
        //            {
        //                if (BFMarketbook.Status.ToString() == "CLOSED")
        //                {
        //                    marketbook.MarketStatusstr = "Closed";
        //                }
        //                else
        //                {
        //                    if (BFMarketbook.Status.ToString() == "SUSPENDED")
        //                    {
        //                        marketbook.MarketStatusstr = "Suspended";
        //                    }
        //                    else
        //                    {
        //                        marketbook.MarketStatusstr = "Active";
        //                    }

        //                }

        //            }

        //            List<Runner> lstRunners = new List<Runner>();
        //            foreach (var runneritem in BFMarketbook.Runners)
        //            {
        //                var runner = new Runner();
        //                runner.Handicap = runneritem.Handicap;
        //                runner.StatusStr = runneritem.Status.ToString();
        //                runner.SelectionId = runneritem.SelectionId.ToString();
        //                runner.LastPriceTraded = runneritem.LastPriceTraded;
        //                var lstpricelist = new List<PriceSize>();
        //                if (runneritem.ExchangePrices.AvailableToBack != null && runneritem.ExchangePrices.AvailableToBack.Count() > 0)
        //                {
        //                    if (BFMarketbook.Runners.Count() == 1)
        //                    {
        //                        try
        //                        {


        //                            if (runneritem.ExchangePrices.AvailableToBack[0].Price.ToString().Contains("."))
        //                            {
        //                                foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
        //                                {
        //                                    var pricesize = new PriceSize();

        //                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

        //                                    pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

        //                                    lstpricelist.Add(pricesize);
        //                                }
        //                            }
        //                            else
        //                            {
        //                                foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
        //                                {
        //                                    var pricesize = new PriceSize();

        //                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

        //                                    pricesize.Price = backitems.Price;

        //                                    lstpricelist.Add(pricesize);
        //                                }
        //                            }
        //                        }
        //                        catch (System.Exception ex)
        //                        {

        //                        }
        //                    }
        //                    else
        //                    {
        //                        foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
        //                        {
        //                            var pricesize = new PriceSize();

        //                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

        //                            pricesize.Price = backitems.Price;

        //                            lstpricelist.Add(pricesize);
        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 3; i++)
        //                    {
        //                        var pricesize = new PriceSize();

        //                        pricesize.Size = 0;

        //                        pricesize.Price = 0;

        //                        lstpricelist.Add(pricesize);
        //                    }
        //                }

        //                runner.ExchangePrices = new ExchangePrices();
        //                runner.ExchangePrices.AvailableToBack = lstpricelist;
        //                lstpricelist = new List<PriceSize>();
        //                if (runneritem.ExchangePrices.AvailableToLay != null && runneritem.ExchangePrices.AvailableToLay.Count() > 0)
        //                {
        //                    if (BFMarketbook.Runners.Count() == 1)
        //                    {
        //                        if (runneritem.ExchangePrices.AvailableToLay[0].Price.ToString().Contains("."))
        //                        {
        //                            foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
        //                            {
        //                                var pricesize = new PriceSize();

        //                                pricesize.Size = Convert.ToInt64((backitems.Size) * Convert.ToDouble(marketbook.PoundRate));

        //                                pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

        //                                lstpricelist.Add(pricesize);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
        //                            {
        //                                var pricesize = new PriceSize();

        //                                pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

        //                                pricesize.Price = backitems.Price;

        //                                lstpricelist.Add(pricesize);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        foreach (var backitems in runneritem.ExchangePrices.AvailableToLay)
        //                        {
        //                            var pricesize = new PriceSize();

        //                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));

        //                            pricesize.Price = backitems.Price;

        //                            lstpricelist.Add(pricesize);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    for (int i = 0; i < 3; i++)
        //                    {
        //                        var pricesize = new PriceSize();

        //                        pricesize.Size = 0;

        //                        pricesize.Price = 0;

        //                        lstpricelist.Add(pricesize);
        //                    }
        //                }

        //                runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
        //                runner.ExchangePrices.AvailableToLay = lstpricelist;
        //                lstRunners.Add(runner);
        //            }
        //            marketbook.Runners = new List<Runner>(lstRunners);

        //            double lastback = 0;
        //            double lastbackSize = 0;
        //            double lastLaySize = 0;
        //            double lastlay = 0;

        //            if (marketbook.MarketStatusstr != "Suspended")
        //            {

        //                double favBack = marketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price;
        //                string selectionIDfav = marketbook.Runners[0].SelectionId;
        //                foreach (var favoriteitem in marketbook.Runners)
        //                {
        //                    if (favoriteitem.ExchangePrices.AvailableToBack != null && favoriteitem.ExchangePrices.AvailableToBack.Length > 0)
        //                        if (marketbook.MainSportsname.Contains("Racing"))
        //                        {
        //                            if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack && favoriteitem.ExchangePrices.AvailableToBack[0].Price > 0)
        //                            {
        //                                favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
        //                                selectionIDfav = favoriteitem.SelectionId;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (favoriteitem.ExchangePrices.AvailableToBack[0].Price < favBack)
        //                            {
        //                                favBack = favoriteitem.ExchangePrices.AvailableToBack[0].Price;
        //                                selectionIDfav = favoriteitem.SelectionId;
        //                            }
        //                        }

        //                }
        //                if (marketbook.MarketStatusstr == "Closed")
        //                {
        //                    var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER");
        //                    if (resultsfav != null && resultsfav.Count() > 0)
        //                    {
        //                        selectionIDfav = resultsfav.FirstOrDefault().SelectionId;
        //                    }

        //                }
        //                var favoriteteam = marketbook.Runners.Where(ii => ii.SelectionId == selectionIDfav).FirstOrDefault();



        //                string selectionname = favoriteteam.RunnerName;
        //                if (favoriteteam.ExchangePrices.AvailableToBack.Length > 0)
        //                {
        //                    lastback = favoriteteam.ExchangePrices.AvailableToBack[0].Price;
        //                    lastbackSize = favoriteteam.ExchangePrices.AvailableToBack[0].Size;


        //                }
        //                if (favoriteteam.ExchangePrices.AvailableToLay.Length>0)
        //                {

        //                    lastLaySize = favoriteteam.ExchangePrices.AvailableToLay[0].Size;
        //                    lastlay = favoriteteam.ExchangePrices.AvailableToLay[0].Price;

        //                }
        //                marketbook.FavoriteBack = (lastback - 1).ToString("F2");
        //                marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
        //                marketbook.FavoriteSelectionName = selectionname;
        //                marketbook.FavoriteBackSize = lastbackSize.ToString();
        //                marketbook.FavoriteLaySize = lastLaySize.ToString();
        //                marketbook.FavoriteID = selectionIDfav;

        //            }
        //            else
        //            {
        //                marketbook.FavoriteBack = (lastback - 1).ToString("F2");
        //                marketbook.FavoriteLay = (lastlay - 1).ToString("F2");
        //                marketbook.FavoriteSelectionName = "";
        //                marketbook.FavoriteBackSize = lastbackSize.ToString();
        //                marketbook.FavoriteLaySize = lastLaySize.ToString();
        //                marketbook.FavoriteID = "0";
        //            }
        //            if (Convert.ToDouble(marketbook.FavoriteBack) < 0)
        //            {
        //                marketbook.FavoriteBack = "0";
        //                marketbook.FavoriteBackSize = "0";
        //            }
        //            if (Convert.ToDouble(marketbook.FavoriteLay) < 0)
        //            {
        //                marketbook.FavoriteLay = "0";
        //                marketbook.FavoriteLaySize = "0";
        //            }
        //            return marketbook;

        //        }
        //        else
        //        {
        //            return new MarketBook();
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        APIConfig.LogError(ex);
        //        return new MarketBook();
        //    }

        //}

        public List<MarketBook> GetMarketDatabyID(string[] marketID, string sheetname, DateTime OrignalOpenDate, string MainSportsCategory)
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

        public string MarketBook(string ID)
        {
            try
            {

                //Session["TWT"] = "";
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
                if (ID != "" && LoggedinUserDetail.GetUserTypeID() == 1 || LoggedinUserDetail.GetUserTypeID() == 8)
                {

                    objUsersServiceCleint.SetMarketBookOpenbyUSer(73, ID);
                }
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    //Session["Eventid"] = null;
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
                                    //Session["Eventid"] = item.EventID;

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
                        

                        List<UserBets> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate));
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

                        //Session["userbets"] = lstUserBet;
                        //long Liabality = 0;
                        //Session["liabality"] = Liabality;
                        //decimal TotLiabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBet);
                        //Session["totliabality"] = TotLiabality;
                        //ViewBag.totliabalityNew = TotLiabality;

                        //return RenderRazorViewToString("MarketBook", marketbooks);
                        return marketbooks.ToString();
                    }
                    else
                    {
                        //List<UserBets> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                        //Session["userbets"] = lstUserBet;
                        //Session["liabality"] = 0;

                        //decimal TotLiabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBet);
                        //Session["totliabality"] = TotLiabality;
                        //ViewBag.totliabalityNew = TotLiabality;
                        var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                        //return RenderRazorViewToString("MarketBook", marketbooks);
                        return marketbooks.ToString();
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
                                        //Session["Eventid"] = item.EventID;
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

                            List<UserBetsforAgent> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(objUsersServiceCleint.GetUserbetsbyUserIDandAgentID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate));
                            //Session["userbets"] = lstUserBet;
                            //Session["liabality"] = 0;

                            //decimal TotLiabality = objUserBets.GetLiabalityofCurrentAgent(lstUserBet);
                            //Session["totliabality"] = 0;
                            //ViewBag.totliabalityNew = 0;
                            //return RenderRazorViewToString("MarketBook", marketbooks);
                            return marketbooks.ToString();
                        }
                        else
                        {

                            List<UserBetsforAgent> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(objUsersServiceCleint.GetUserbetsbyUserIDandAgentID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate));
                            //Session["userbets"] = lstUserBet;
                            //Session["liabality"] = 0;

                            //decimal TotLiabality = objUserBets.GetLiabalityofCurrentAgent(lstUserBet);
                            //Session["totliabality"] = 0;
                            //ViewBag.totliabalityNew = 0;
                            var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                            //return RenderRazorViewToString("MarketBook", marketbooks);
                            return marketbooks.ToString();
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
                                           //Session["Eventid"] = item.EventID;


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
                                    
                                    marketbooks.Add(item2);
                                }

                                List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(LoggedinUserDetail.PasswordForValidate));

                                //Session["userbets"] = lstUserBet;
                                //Session["liabality"] = 0;

                                //decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                                //Session["totliabality"] = 0;
                                //// Updateunmatchbets(marketbooks);
                                //ViewBag.totliabalityNew = 0;
                                //return RenderRazorViewToString("MarketBook", marketbooks);
                                return marketbooks.ToString();
                            }
                            else
                            {
                                List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(LoggedinUserDetail.PasswordForValidate));

                                //Session["userbets"] = lstUserBet;
                                //Session["liabality"] = 0;

                                //decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                                //Session["totliabality"] = 0;
                                //ViewBag.totliabalityNew = 0;
                                var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                                //return RenderRazorViewToString("MarketBook", marketbooks);
                                return marketbooks.ToString();
                            }

                        }

                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
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
                                                item2.MarketBookName = item.EventName + " / " + item.Name;
                                                item2.OrignalOpenDate = item.EventOpenDate;
                                                item2.MainSportsname = item.EventTypeName;
                                                item2.BettingAllowed = item.BettingAllowed;
                                                item2.BettingAllowedOverAll = CheckForAllowedBettingOverAll(item.EventTypeName, item2.MarketBookName);
                                                item2.GetMatchUpdatesFrom = item.GetMatchUpdatesFrom;
                                                item2.EventID = item.EventID;
                                                //Session["Eventid"] = item.EventID;


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
                                        
                                        marketbooks.Add(item2);
                                    }

                                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(LoggedinUserDetail.PasswordForValidate));

                                    //Session["userbets"] = lstUserBet;
                                    //Session["liabality"] = 0;

                                    //decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                                    //Session["totliabality"] = 0;
                                    //// Updateunmatchbets(marketbooks);
                                    //ViewBag.totliabalityNew = 0;
                                    //return RenderRazorViewToString("MarketBook", marketbooks);
                                    return marketbooks.ToString();
                                }
                                else
                                {
                                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(LoggedinUserDetail.PasswordForValidate));

                                    //Session["userbets"] = lstUserBet;
                                    //Session["liabality"] = 0;

                                    //decimal TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBet);
                                    //Session["totliabality"] = 0;
                                    //ViewBag.totliabalityNew = 0;
                                    var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                                    //return RenderRazorViewToString("MarketBook", marketbooks);
                                    return marketbooks.ToString();
                                }
                            }

                            else
                            {
                                var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                                //return RenderRazorViewToString("MarketBook", marketbooks);
                                return marketbooks.ToString();
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
    }
}
