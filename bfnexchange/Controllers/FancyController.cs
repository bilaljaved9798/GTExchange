using bfnexchange.UsersServiceReference;
using ExternalAPI.TO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bfnexchange.Controllers
{
    public class FancyController : Controller
    {
        public static wsnew wsFancy = new wsnew();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceReference.BettingServiceClient objBettingClient = new BettingServiceReference.BettingServiceClient();
        // GET: Fancy
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult GetFancyMarket(string MarketBookID, DateTime EventOpendate)
        {

            var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(MarketBookID);
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
            Session["linevmarkets"] = linevmarkets;

            List<string> lstIds = linevmarkets.Select(item => item.MarketCatalogueID).ToList();

            string[] marketIds = lstIds.ToArray();


            if (LoggedinUserDetail.GetCricketDataFrom == "BP")
            {
                var list = JsonConvert.DeserializeObject<List<bfnexchange.Services.SampleResponse1>>(objBettingClient.GetAllMarketsBPFancy(marketIds));

                if (list.Count() > 0)
                {

                    MarketController objcontroller = new MarketController();
                    bool BettingAllowedoverall = objcontroller.CheckForAllowedBettingOverAll("Cricket", "Line");
                    List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
                    foreach (var bfobject in linevmarkets)
                    {
                        try
                        {
                            bfnexchange.Services.SampleResponse1 objmarketbookBF1 = list.Where(item => item.MarketId == bfobject.MarketCatalogueID).First();
                            LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBF123(objmarketbookBF1, bfobject.MarketCatalogueID, EventOpendate, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed, BettingAllowedoverall));
                        }
                        catch (System.Exception ex)
                        {

                        }



                    }
                    //return PartialView("FancyMarketBookNew", LastloadedLinMarkets1);
                    return PartialView("FancyMarketBook", LastloadedLinMarkets1);

                }
                else
                {
                    //return PartialView("FancyMarketBookNew", new List<MarketBook>());
                    return PartialView("FancyMarketBook", new List<MarketBook>());
                }
            }
            else
            {
                if (LoggedinUserDetail.GetCricketDataFrom == "Live")
                {
                    var list = (objBettingClient.GetAllMarketsFancy(marketIds));

                    if (list.Count() > 0)
                    {

                        MarketController objcontroller = new MarketController();
                        bool BettingAllowedoverall = objcontroller.CheckForAllowedBettingOverAll("Cricket", "Line");
                        List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
                        foreach (var bfobject in linevmarkets)
                        {
                            try
                            {
                                MarketBook objmarketbookBF1 = list.Where(item => item.MarketId == bfobject.MarketCatalogueID).First();
                                LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectLive(objmarketbookBF1, bfobject.MarketCatalogueID, EventOpendate, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed, BettingAllowedoverall));
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        //return PartialView("FancyMarketBookNew", LastloadedLinMarkets1);
                        return PartialView("FancyMarketBook", LastloadedLinMarkets1);

                    }
                    else
                    {
                        //return PartialView("FancyMarketBookNew", new List<MarketBook>());
                        return PartialView("FancyMarketBook", new List<MarketBook>());
                    }
                }
                else
                {


                    var list = (objBettingClient.GetAllMarketsOthersFancy(marketIds));

                    if (list.Count() > 0)
                    {

                        MarketController objcontroller = new MarketController();
                        bool BettingAllowedoverall = objcontroller.CheckForAllowedBettingOverAll("Cricket", "Line");
                        List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
                        foreach (var bfobject in linevmarkets)
                        {
                            try
                            {
                                ExternalAPI.TO.MarketBookString objmarketbookBF1 = list.Where(item => item.MarketBookId == bfobject.MarketCatalogueID).First();
                                LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBFNewSource(objmarketbookBF1, bfobject.MarketCatalogueID, EventOpendate, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed, BettingAllowedoverall));
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        //return PartialView("FancyMarketBookNew", LastloadedLinMarkets1);
                        return PartialView("FancyMarketBook", LastloadedLinMarkets1);

                    }
                    else
                    {
                        //return PartialView("FancyMarketBookNew", new List<MarketBook>());
                        return PartialView("FancyMarketBook", new List<MarketBook>());
                    }
                }



                //    IList<bfnexchange.wrBF.MarketBook> list;


                //wsFancy.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Fancy").FirstOrDefault().URLForData;
                //list = wsFancy.GD(LoggedinUserDetail.SecurityCode, marketIds, 4);

                //if (list.Count > 0)
                //{

                //    MarketController objcontroller = new MarketController();
                //    bool BettingAllowedoverall = objcontroller.CheckForAllowedBettingOverAll("Cricket", "Line");
                //    List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
                //    foreach (var bfobject in linevmarkets)
                //    {
                //        try
                //        {
                //            wrBF.MarketBook objmarketbookBF1 = list.Where(item => item.MarketId == bfobject.MarketCatalogueID).First();
                //            LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBF(objmarketbookBF1, bfobject.MarketCatalogueID, EventOpendate, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed, BettingAllowedoverall));
                //        }
                //        catch(System.Exception ex)
                //        {

                //        }



                //    }
                //    return PartialView("FancyMarketBook",LastloadedLinMarkets1);

                //}
                //else
                //{
                //    return PartialView("FancyMarketBook", new List<MarketBook>());
                //}
            }



        }
        public ExternalAPI.TO.MarketBook ConvertJsontoMarketObjectBF123(Services.SampleResponse1 BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool BettingAllowed, bool BettingAllowedoverall)
        {
            try
            {
                if (1 == 1)
                {
                    var marketbook = new ExternalAPI.TO.MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = objUsersServiceCleint.GetPoundRatebyUserID(LoggedinUserDetail.GetUserID());
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.BettingAllowed = BettingAllowed;
                    marketbook.BettingAllowedOverAll = BettingAllowedoverall;
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
                        //try
                        //{
                        //    runner.RunnerName = selectionnames.Where(item2 => item2.SelectionID == runneritem.SelectionId.ToString()).FirstOrDefault().SelectionName;
                        //}
                        //catch (System.Exception ex)
                        //{

                        ////}

                        runner.RunnerName = sheetname;
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

                        runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                        runner.ExchangePrices.AvailableToLay = lstpricelist;
                        lstRunners.Add(runner);
                    }


                    marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);

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
        public string LoadFancyMarket(string MarketBookID, DateTime EventOpendate)
        {

            //var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(MarketBookID);
            //int UserIDforLinevmarkets = 0;
            //if (LoggedinUserDetail.GetUserTypeID() == 1)
            //{
            //    UserIDforLinevmarkets = 73;
            //}
            //else
            //{
            //    UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            //}
            //var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));
            List<ExternalAPI.TO.LinevMarkets> linevmarkets = (List<ExternalAPI.TO.LinevMarkets>)Session["linevmarkets"];
            if (Session["linevmarkets"] == null)
            {
                GetFancyMarket(MarketBookID, EventOpendate);
                return "";
            }
            List<string> lstIds = linevmarkets.Select(item => item.MarketCatalogueID).ToList();

            string[] marketIds = lstIds.ToArray();
            if (LoggedinUserDetail.GetCricketDataFrom == "BP")
            {
                UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();


                var list = JsonConvert.DeserializeObject<List<bfnexchange.Services.SampleResponse1>>(objBettingClient.GetAllMarketsBPFancy(marketIds));

                if (list.Count() > 0)
                {

                    MarketController objcontroller = new MarketController();
                    bool BettingAllowedoverall = objcontroller.CheckForAllowedBettingOverAll("Cricket", "Line");
                    List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
                    foreach (var bfobject in linevmarkets)
                    {
                        try
                        {
                            bfnexchange.Services.SampleResponse1 objmarketbookBF1 = list.Where(item => item.MarketId == bfobject.MarketCatalogueID).First();
                            ExternalAPI.TO.MarketBook objnewmarketbook = ConvertJsontoMarketObjectBF123(objmarketbookBF1, bfobject.MarketCatalogueID, EventOpendate, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed, true);
                            ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = new MarketBook();
                            if (LoggedinUserDetail.GetUserTypeID() == 3)
                            {
                                List<Models.UserBets> lstUserBets = (List<Models.UserBets>)Session["userbet"];
                                CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforAgent>(), lstUserBets);
                            }
                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 2)
                                {
                                    List<Models.UserBetsforAgent> lstUserBets = (List<Models.UserBetsforAgent>)Session["userbets"];
                                    CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), lstUserBets, new List<Models.UserBets>());
                                }
                                else
                                {
                                    if (LoggedinUserDetail.GetUserTypeID() == 8)
                                    {
                                        List<Models.UserBetsforSuper> lstUserBets = (List<Models.UserBetsforSuper>)Session["userbets"];
                                        CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, new List<Models.UserBetsForAdmin>(), lstUserBets, new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());

                                    }

                                    else
                                    {
                                        List<Models.UserBetsForAdmin> lstUserBets = (List<Models.UserBetsForAdmin>)Session["userbets"];
                                        CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, lstUserBets, new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                                    }
                                }
                            }


                            if (CurrentMarketProfitandloss.Runners != null)
                            {
                                objnewmarketbook.Runners[0].ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                objnewmarketbook.Runners[0].Loss = CurrentMarketProfitandloss.Runners.Max(t => t.ProfitandLoss);
                            }
                            LastloadedLinMarkets1.Add(objnewmarketbook);
                        }
                        catch (System.Exception ex)
                        {

                        }



                    }
                    return LoggedinUserDetail.ConverttoJSONString(LastloadedLinMarkets1);

                }
                else
                {
                    return "";
                }
            }
            else
            {

                if (LoggedinUserDetail.GetCricketDataFrom == "Live")
                {
                    var list = (objBettingClient.GetAllMarketsFancy(marketIds));

                    if (list.Count() > 0)
                    {
                        UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();

                        MarketController objcontroller = new MarketController();
                        bool BettingAllowedoverall = objcontroller.CheckForAllowedBettingOverAll("Cricket", "Line");
                        List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
                        foreach (var bfobject in linevmarkets)
                        {
                            try
                            {
                                MarketBook objmarketbookBF1 = list.Where(item => item.MarketId == bfobject.MarketCatalogueID).First();
                                ExternalAPI.TO.MarketBook objnewmarketbook = ConvertJsontoMarketObjectLive(objmarketbookBF1, bfobject.MarketCatalogueID, EventOpendate, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed, true);
                                ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = new MarketBook();
                                if (LoggedinUserDetail.GetUserTypeID() == 3)
                                {
                                    List<Models.UserBets> lstUserBets = (List<Models.UserBets>)Session["userbet"];
                                    CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforAgent>(), lstUserBets);
                                }
                                else
                                {
                                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                                    {
                                        List<Models.UserBetsforAgent> lstUserBets = (List<Models.UserBetsforAgent>)Session["userbets"];
                                        CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), lstUserBets, new List<Models.UserBets>());
                                    }
                                    else
                                    {
                                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                                        {
                                            List<Models.UserBetsforSuper> lstUserBets = (List<Models.UserBetsforSuper>)Session["userbets"];
                                            CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, new List<Models.UserBetsForAdmin>(), lstUserBets, new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                                        }
                                        else
                                        {
                                            List<Models.UserBetsForAdmin> lstUserBets = (List<Models.UserBetsForAdmin>)Session["userbets"];
                                            CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, lstUserBets, new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());

                                        }
                                    }
                                }
                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    objnewmarketbook.Runners[0].ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                    objnewmarketbook.Runners[0].Loss = CurrentMarketProfitandloss.Runners.Max(t => t.ProfitandLoss);
                                }
                                LastloadedLinMarkets1.Add(objnewmarketbook);
                            }
                            catch (System.Exception ex)
                            {

                            }



                        }
                        return LoggedinUserDetail.ConverttoJSONString(LastloadedLinMarkets1);

                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();


                    var list = objBettingClient.GetAllMarketsOthersFancy(marketIds);

                    if (list.Count() > 0)
                    {

                        MarketController objcontroller = new MarketController();
                        bool BettingAllowedoverall = objcontroller.CheckForAllowedBettingOverAll("Cricket", "Line");
                        List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
                        foreach (var bfobject in linevmarkets)
                        {
                            try
                            {
                                ExternalAPI.TO.MarketBookString objmarketbookBF1 = list.Where(item => item.MarketBookId == bfobject.MarketCatalogueID).First();
                                ExternalAPI.TO.MarketBook objnewmarketbook = ConvertJsontoMarketObjectBFNewSource(objmarketbookBF1, bfobject.MarketCatalogueID, EventOpendate, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed, true);
                                ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = new MarketBook();
                                if (LoggedinUserDetail.GetUserTypeID() == 3)
                                {
                                    List<Models.UserBets> lstUserBets = (List<Models.UserBets>)Session["userbet"];
                                    CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforAgent>(), lstUserBets);
                                }
                                else
                                {
                                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                                    {
                                        List<Models.UserBetsforAgent> lstUserBets = (List<Models.UserBetsforAgent>)Session["userbets"];
                                        CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), lstUserBets, new List<Models.UserBets>());
                                    }
                                    else
                                    {
                                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                                        {
                                            List<Models.UserBetsforSuper> lstUserBets = (List<Models.UserBetsforSuper>)Session["userbets"];
                                            CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, new List<Models.UserBetsForAdmin>(), lstUserBets, new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                                        }

                                        else
                                        {
                                            List<Models.UserBetsForAdmin> lstUserBets = (List<Models.UserBetsForAdmin>)Session["userbets"];
                                            CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, lstUserBets, new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                                        }
                                    }
                                }
                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    objnewmarketbook.Runners[0].ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                    objnewmarketbook.Runners[0].Loss = CurrentMarketProfitandloss.Runners.Max(t => t.ProfitandLoss);
                                }
                                LastloadedLinMarkets1.Add(objnewmarketbook);
                            }
                            catch (System.Exception ex)
                            {

                            }



                        }
                        return LoggedinUserDetail.ConverttoJSONString(LastloadedLinMarkets1);

                    }
                    else
                    {
                        return "";
                    }
                }
                //IList<bfnexchange.wrBF.MarketBook> list;


                //wsFancy.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Fancy").FirstOrDefault().URLForData;
                //list = wsFancy.GD(LoggedinUserDetail.SecurityCode, marketIds, 4);
                //UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
                //List<Models.UserBets> lstUserBets = (List<Models.UserBets>)Session["userbet"];
                //if (list.Count > 0)
                //{



                //    List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
                //    foreach (var bfobject in linevmarkets)
                //    {
                //        try
                //        {
                //            wrBF.MarketBook objmarketbookBF1 = list.Where(item => item.MarketId == bfobject.MarketCatalogueID).First();
                //            ExternalAPI.TO.MarketBook objnewmarketbook = ConvertJsontoMarketObjectBF(objmarketbookBF1, bfobject.MarketCatalogueID, EventOpendate, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed, true);
                //            ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = objUserbets.GetBookPosition(objnewmarketbook.MarketId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforAgent>(), lstUserBets);
                //            if (CurrentMarketProfitandloss.Runners != null)
                //            {
                //                objnewmarketbook.Runners[0].ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                //                objnewmarketbook.Runners[0].Loss = CurrentMarketProfitandloss.Runners.Max(t => t.ProfitandLoss);
                //            }
                //            LastloadedLinMarkets1.Add(objnewmarketbook);

                //        }
                //        catch (System.Exception ex)
                //        {

                //        }






                //    }
                //    return LoggedinUserDetail.ConverttoJSONString(LastloadedLinMarkets1);

                //}
                //else
                //{
                //    return "";
                //}
            }



        }
        public void SetLinevMarketsNull()
        {
            Session["linevmarkets"] = null;
        }
        public MarketBook ConvertJsontoMarketObjectBFNewSource(ExternalAPI.TO.MarketBookString BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory,bool BettingAllowed, bool BettingAllowedoverall)
        {

            if (1 == 1)
            {
                var marketbook = new MarketBook();
                string[] newres = BFMarketbook.MarketBookData.Split(':').Select(tag => tag.Trim()).ToArray(); 
                string[] BFMarketBookDetail = newres[0].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();

                marketbook.MarketId = BFMarketbook.MarketBookId;
                marketbook.SheetName = "";
                marketbook.IsMarketDataDelayed = false;
                marketbook.PoundRate = LoggedinUserDetail.GetPoundRate();
                marketbook.NumberOfWinners = Convert.ToInt32(BFMarketBookDetail[6]);
                marketbook.MarketBookName = sheetname;
                marketbook.MainSportsname = MainSportsCategory;
                marketbook.OrignalOpenDate = marketopendate;
                marketbook.BettingAllowed = BettingAllowed;
                marketbook.BettingAllowedOverAll = BettingAllowedoverall;
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
                    string[] runnerdetails = newres[i].Split(new string[] { "|" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray(); 
                    string[] runnerinfo = runnerdetails[0].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray(); 
                    string[] runnerbackdata = runnerdetails[1].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray() ;
                    string[] runnerlaydata = runnerdetails[2].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                    var runner = new ExternalAPI.TO.Runner();
                    runner.Handicap = 0;
                    runner.StatusStr = runnerinfo[6].Trim();
                    runner.SelectionId = runnerinfo[0].Trim().ToString();
                    runner.RunnerName = sheetname;
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
                                            pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1]);
                                            pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j]) + 0.5).ToString("F2"));

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
                                            pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1]);
                                            pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j])).ToString("F2"));

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
                                    pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1]);
                                    pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j])).ToString("F2"));

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
                                            pricesize.OrignalSize = Convert.ToDouble(runnerbackdata[j + 1]);
                                            pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerbackdata[j]) + 0.5).ToString("F2"));

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
                                            pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1]);
                                            pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                            pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j])).ToString("F2"));

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
                                    pricesize.OrignalSize = Convert.ToDouble(runnerlaydata[j + 1]);
                                    pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = APIConfig.FormatNumber(pricesize.Size);
                                    pricesize.Price = Convert.ToDouble((Convert.ToDouble(runnerlaydata[j])).ToString("F2"));

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


                marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);
                
                return marketbook;

            }
            else
            {
                return new MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBF(bfnexchange.wrBF.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool BettingAllowed, bool BettingAllowedoverall)
        {
            try
            {

                if (1 == 1)
                {
                    var marketbook = new MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = LoggedinUserDetail.GetPoundRate();
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.BettingAllowed = BettingAllowed;
                    marketbook.Version = BFMarketbook.Version;
                    marketbook.TotalMatched = BFMarketbook.TotalMatched;
                    marketbook.BettingAllowedOverAll = BettingAllowedoverall;
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
                        runner.RunnerName = sheetname;
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

                        runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                        runner.ExchangePrices.AvailableToLay = lstpricelist;
                        lstRunners.Add(runner);
                    }
                    marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);


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
        public MarketBook ConvertJsontoMarketObjectLive(MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool BettingAllowed, bool BettingAllowedoverall)
        {
            try
            {



                if (1 == 1)
                {
                    var marketbook = new MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = LoggedinUserDetail.GetPoundRate();
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.BettingAllowed = BettingAllowed;
                    marketbook.Version = BFMarketbook.Version;
                    marketbook.TotalMatched = BFMarketbook.TotalMatched;
                    marketbook.BettingAllowedOverAll = BettingAllowedoverall;
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
                        runner.RunnerName = sheetname;
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

                        runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                        runner.ExchangePrices.AvailableToLay = lstpricelist;
                        lstRunners.Add(runner);
                    }
                    marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);


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
    }
}