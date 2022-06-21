using bfnexchange.BettingServiceReference;
using bfnexchange.Models;
using bfnexchange.UsersServiceReference;
using ExternalAPI.TO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bfnexchange.Controllers
{
    public class KJController : Controller
    {

        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        // GET: KJ
        public ActionResult Index()
        {
            ViewBag.backgrod = "#1D9BF0";
            ViewBag.color = "white";
            return View();
        }
        List<MarketBookForindianFancy> lstMarketBookRunnersKJ = new List<MarketBookForindianFancy>();
        List<RunnerForIndianFancy> lstMarketBookRunnersIndianFancy = new List<RunnerForIndianFancy>();
        List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
        MarketBook MarketBook = new MarketBook();
       
        public PartialViewResult GetFancyMarketKJ(string eventID,bool Bool)
        {
            if (Bool == true)
            {
                int id = LoggedinUserDetail.GetUserID();
                var KJvmarket = JsonConvert.DeserializeObject<List<ExternalAPI.TO.SP_UserMarket_GetDistinctKJMarketsbyEventID_Result>>(objUsersServiceCleint.KJMarketsbyEventID(eventID, LoggedinUserDetail.GetUserID()));
                Session["Allmarkets"] = KJvmarket;
                if (KJvmarket.Count > 0)
                {
                    KJvmarket = KJvmarket.OrderBy(x => x.MarketCatalogueName).ToList();
                    MarketBook.KJMarkets = KJvmarket.Where(item => item.EventName == "Kali v Jut").ToList();
                    MarketBook.KJMarkets.FirstOrDefault().EventID = eventID;
                    MarketBook.KJMarkets = MarketBook.KJMarkets.Where(item => item.isOpenedbyUser == true).ToList();
                }
              
                if (MarketBook.KJMarkets != null)
                {
                    foreach (var bfobject in MarketBook.KJMarkets)
                    {
                        try
                        {
                            LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBFNewSource("369646", bfobject.MarketCatalogueID, bfobject.MarketCatalogueName, bfobject.BettingAllowed));
                        }
                        catch (System.Exception ex)
                        {

                        }
                    }                  
                }
                return PartialView("FancyKJMarketBook", LastloadedLinMarkets1);
            }                               
                return PartialView("FancyKJMarketBook", LastloadedLinMarkets1);        
        }

        UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
      
        public MarketBook ConvertJsontoMarketObjectBFNewSource(string SelectionID, string marketid, string MarketCatalogueName, bool BettingAllowed)
        {

            if (1 == 1)
            {
                var marketbook = new MarketBook();
                marketbook.MarketId = marketid;
                marketbook.SheetName = "";                          
                marketbook.MarketBookName = MarketCatalogueName;
                marketbook.MainSportsname = "Fancy";
                marketbook.MarketStatusstr = "In Play";               
                marketbook.BettingAllowed = BettingAllowed;
                marketbook.BettingAllowedOverAll = true;
                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                var runner = new ExternalAPI.TO.Runner();
                runner.Handicap = 0;
                //runner.StatusStr = runnerinfo[6].Trim();
                runner.SelectionId = SelectionID;
                try
                {
                    ExternalAPI.TO.MarketBookForindianFancy currentmarketsfancyPL = new ExternalAPI.TO.MarketBookForindianFancy();

                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {
                        List<Models.UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                        lstUserBets = lstUserBets.Where(item => item.MarketBookID == marketbook.MarketId).ToList();
                        if (lstUserBets.Count != 0)
                        {
                            currentmarketsfancyPL = objUserbets.GetBookPositioninKJ(marketid, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforSamiadmin>(), new List<Models.UserBetsforAgent>(), lstUserBets);
                        }
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            List<Models.UserBetsforAgent> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(objUsersServiceCleint.GetUserbetsbyUserIDandAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                            lstUserBets = lstUserBets.Where(item => item.MarketBookID == marketbook.MarketId).ToList();
                            if (lstUserBets.Count != 0)
                            {
                                currentmarketsfancyPL = objUserbets.GetBookPositioninKJ(marketid, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforSamiadmin>(), lstUserBets, new List<Models.UserBets>());
                            }
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                List<Models.UserBetsforSuper> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsforSuper>>(objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                                lstUserBets = lstUserBets.Where(item => item.MarketBookID == marketbook.MarketId).ToList();
                                if (lstUserBets.Count != 0)
                                {
                                    currentmarketsfancyPL = objUserbets.GetBookPositioninKJ(marketid, new List<Models.UserBetsForAdmin>(), lstUserBets, new List<Models.UserBetsforSamiadmin>(), new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                                }
                            }
                            if (LoggedinUserDetail.GetUserTypeID() == 9)
                            {
                                List<Models.UserBetsforSamiadmin> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsforSamiadmin>>(objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                                lstUserBets = lstUserBets.Where(item => item.MarketBookID == marketbook.MarketId).ToList();
                                if (lstUserBets.Count != 0)
                                {
                                    currentmarketsfancyPL = objUserbets.GetBookPositioninKJ(marketid, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), lstUserBets , new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                                }
                            }
                            else
                            {
                                List<Models.UserBetsForAdmin> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                                lstUserBets = lstUserBets.Where(item => item.MarketBookID == marketbook.MarketId).ToList();
                                if (lstUserBets.Count != 0)
                                {
                                    currentmarketsfancyPL = objUserbets.GetBookPositioninKJ(marketid, lstUserBets, new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforSamiadmin>(), new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                                }
                            }
                        }
                    }
                    double TotalProfit = 0;
                    double TotalLoss = 0;
                    if (currentmarketsfancyPL.RunnersForindianFancy != null)
                    {
                        TotalProfit = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Max(t => t.ProfitandLoss));
                        TotalLoss = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Min(t => t.ProfitandLoss));
                        runner.ProfitandLoss =Convert.ToInt64( TotalProfit);
                        runner.Loss = Convert.ToInt64( TotalLoss);
                    }
                    else
                    {
                        runner.ProfitandLoss = Convert.ToInt64(TotalProfit);
                        runner.Loss = Convert.ToInt64(TotalLoss);
                    }
                }
                catch (System.Exception ex)
                {

                }
                //if(Session["KaliJutData"] ==null)
                //{
                //    GetKJData();
                //}
               
               //  KJs = Session["KaliJutData"] as Sp_GetKalijut_Result;
                var lstpricelist = new List<PriceSize>();
                //if (KJs.KaliSizeBack != null)
                //{
                    var pricesize = new PriceSize();
                    pricesize.OrignalSize = Convert.ToDouble(0);
                    pricesize.Size = 98; //Convert.ToDouble(KJs.KaliPriceBack);                  
                    pricesize.Price = 1;// Convert.ToDouble(KJs.KaliSizeBack);
                    lstpricelist.Add(pricesize);
               // }

                runner.ExchangePrices = new ExchangePrices();
                runner.ExchangePrices.AvailableToBack = lstpricelist;
                lstpricelist = new List<PriceSize>();
               // if (KJs.KaliSizeLay != null)
               // {
                    var pricesize1 = new PriceSize();
                    pricesize1.OrignalSize = Convert.ToDouble(0);
                    pricesize1.Size = 102; //Convert.ToDouble(KJs.KaliPriceLay);                  
                    pricesize1.Price = 1; //Convert.ToDouble(KJs.KaliSizeLay);
                    lstpricelist.Add(pricesize1);
                //}
                runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                runner.ExchangePrices.AvailableToLay = lstpricelist;
                lstRunners.Add(runner);
                marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);

                return marketbook;
            }
            else
            {
                return new MarketBook();
            }
        }

        Sp_GetKalijut_Result KJs = new Sp_GetKalijut_Result();
        public void GetKJData()
        {
            KJs = JsonConvert.DeserializeObject<List<ExternalAPI.TO.Sp_GetKalijut_Result>>(objUsersServiceCleint.GetKalijut()).FirstOrDefault();
            Session["KaliJutData"] = KJs;
        }
        public string LoadMarketKJ(string eventID)
        {
            int id = LoggedinUserDetail.GetUserID();
            var KJvmarket = JsonConvert.DeserializeObject<List<ExternalAPI.TO.SP_UserMarket_GetDistinctKJMarketsbyEventID_Result>>(objUsersServiceCleint.KJMarketsbyEventID(eventID, LoggedinUserDetail.GetUserID()));
            Session["Allmarkets"] = KJvmarket;
            if (KJvmarket.Count > 0)
            {
                MarketBook.KJMarkets = KJvmarket.Where(item => item.EventName == "Kali v Jut").ToList();
                MarketBook.KJMarkets.FirstOrDefault().EventID = eventID;
                MarketBook.KJMarkets = MarketBook.KJMarkets.Where(item => item.isOpenedbyUser == true).ToList();
            }

            List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
            LastloadedLinMarkets1.Clear();
            if (MarketBook.KJMarkets != null)
            {
                foreach (var bfobject in MarketBook.KJMarkets)
                {
                    try
                    {
                        LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBFNewSource("369646", bfobject.MarketCatalogueID, bfobject.MarketCatalogueName, bfobject.BettingAllowed));
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
      
    } 
    }
