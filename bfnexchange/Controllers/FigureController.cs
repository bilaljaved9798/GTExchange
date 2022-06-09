
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
    public class FigureController : Controller
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        // GET: Figure
        public ActionResult Index()
        {
            return View();
        }
        
       
        MarketBook MarketBook = new MarketBook();
      public  List<MarketBook> LastloadedLinMarkets1 = new List<MarketBook>();
       public List<MarketBook> MarketBookFigure = new List<MarketBook>();

        public PartialViewResult GetMarketFigure(string EventID, bool Boolf)
        {
          
                int id = LoggedinUserDetail.GetUserID();
                var Figurevmarket = JsonConvert.DeserializeObject<List<ExternalAPI.TO.SP_UserMarket_GetDistinctKJMarketsbyEventID_Result>>(objUsersServiceCleint.KJMarketsbyEventID(EventID, LoggedinUserDetail.GetUserID()));
                Session["Allmarkets"] = Figurevmarket;
                if (Figurevmarket.Count > 0)
                {
                    MarketBook.FigureMarkets = Figurevmarket;
                    MarketBook.FigureMarkets.FirstOrDefault().EventID = EventID;
                    MarketBook.FigureMarkets = MarketBook.FigureMarkets.Where(item => item.isOpenedbyUser == true && item.EventName == "Figure").ToList();
                }

                if (MarketBook.FigureMarkets != null)
                {
                    foreach (var bfobject in MarketBook.FigureMarkets)
                    {
                        try
                        {
                            LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBFNewSourceFigure(bfobject.MarketCatalogueID, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed));
                            MarketBookFigure = LastloadedLinMarkets1;
                            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                            if (MarketBookFigure != null)
                            {
                                MarketBookFigure[0].MarketBookName = MarketBookFigure[0].MarketBookName;
                                MarketBookFigure[0].MainSportsname = MarketBookFigure[0].MainSportsname;
                                if (LoggedinUserDetail.GetUserTypeID() == 3)
                                {
                                    List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                                    lstUserBets = lstUserBets.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBookFigure[0].MarketId).ToList();
                                    if (lstUserBets.Count > 0)
                                    {
                                        MarketBookFigure[0].DebitCredit = objUserBets.ceckProfitandLossFig(MarketBookFigure[0], lstUserBets);
                                        foreach (var runner in MarketBookFigure[0].Runners)
                                        {
                                            runner.ProfitandLoss = Convert.ToInt64(MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        }
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {

                        }
                    }

                }

                return PartialView("FigureMarketBook", MarketBookFigure);
            }
                  
        
          
            
        
       
        
        List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
        public MarketBook ConvertJsontoMarketObjectBFNewSourceFigure(string marketid, string sheetname, string MainSportsCategory, bool BettingAllowed)
        {          
                var marketbook = new MarketBook();
                marketbook.MarketId = marketid;
                marketbook.BettingAllowed = BettingAllowed;
                marketbook.MainSportsname = "Fancy";
                marketbook.MarketBookName = sheetname;
                marketbook.BettingAllowedOverAll = BettingAllowed;
                marketbook.MarketStatusstr = "In Play";
                

                int seletionID = 001;
                int RunnerName = 0;
                //if (Session["Runnserdata"] == null)
                //{
                  
                    for (int i = 0; i <= 9; i++)
                    {
                        var runner = new ExternalAPI.TO.Runner();

                        runner.SelectionId = seletionID + i.ToString(); //runnerinfo[0].Trim().ToString();
                        runner.RunnerName = sheetname + i.ToString();
                     
                        var lstpricelist = new List<PriceSize>();

                        var pricesize = new PriceSize();
                        pricesize.Size = 900; //Convert.ToDouble(FigureOdds.FigPriceBack0);
                        pricesize.Price = i;
                        lstpricelist.Add(pricesize);
                        runner.ExchangePrices = new ExchangePrices();
                        runner.ExchangePrices.AvailableToBack = lstpricelist;
                        lstpricelist = new List<PriceSize>();

                        var pricesize1 = new PriceSize();
                        pricesize1.Size = 1025; //Convert.ToDouble(FigureOdds.FigPriceLay0);
                        pricesize1.Price = i;
                        lstpricelist.Add(pricesize1);

                        runner.ExchangePrices.AvailableToLay = new List<PriceSize>();
                        runner.ExchangePrices.AvailableToLay = lstpricelist;
                        lstRunners.Add(runner);
                        //Session["Runnserdata"] = lstRunners;
                    }
                //}
                //else
                //{
                //    lstRunners = Session["Runnserdata"] as List<ExternalAPI.TO.Runner>;
                //}

                marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);

                return marketbook;
            
        }

        public string LoadMarketFig(string EventID)
        {
           
           // List<ExternalAPI.TO.SP_UserMarket_GetDistinctKJMarketsbyEventID_Result> Figurevmarket = (List<ExternalAPI.TO.SP_UserMarket_GetDistinctKJMarketsbyEventID_Result>)Session["Allmarkets"];
           int id = LoggedinUserDetail.GetUserID();
            var Figurevmarket = JsonConvert.DeserializeObject<List<ExternalAPI.TO.SP_UserMarket_GetDistinctKJMarketsbyEventID_Result>>(objUsersServiceCleint.KJMarketsbyEventID(EventID, LoggedinUserDetail.GetUserID()));
            if (Figurevmarket.Count > 0)
            {
                MarketBook.FigureMarkets = Figurevmarket;
                MarketBook.FigureMarkets.FirstOrDefault().EventID = EventID;
                MarketBook.FigureMarkets = MarketBook.FigureMarkets.Where(item => item.isOpenedbyUser == true && item.EventName == "Figure").ToList();

            }
           
            MarketBookFigure.Clear();
            if (MarketBook.FigureMarkets != null)
            {
                foreach (var bfobject in MarketBook.FigureMarkets)
                {
                    try
                    {
                        LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBFNewSourceFigure(bfobject.MarketCatalogueID, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed));
                        MarketBookFigure = LastloadedLinMarkets1;
                       
                        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                        if (MarketBookFigure != null)
                        {
                            MarketBookFigure[0].MarketBookName = MarketBookFigure[0].MarketBookName;
                            MarketBookFigure[0].MainSportsname = MarketBookFigure[0].MainSportsname;
                            if (LoggedinUserDetail.GetUserTypeID() == 3)
                            {
                                List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                                lstUserBets = lstUserBets.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBookFigure[0].MarketId).ToList();

                                MarketBookFigure[0].DebitCredit = objUserBets.ceckProfitandLossFig(MarketBookFigure[0], lstUserBets);
                                foreach (var runner in MarketBookFigure[0].Runners)
                                {
                                    runner.ProfitandLoss = Convert.ToInt64(MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                }
                            }
                            if (LoggedinUserDetail.GetUserTypeID() == 2)
                            {
                                List<UserBetsforAgent> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(objUsersServiceCleint.GetUserbetsbyUserIDandAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                                lstUserBets = lstUserBets.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBookFigure[0].MarketId).ToList();

                                MarketBookFigure[0].DebitCredit = objUserBets.ceckProfitandLossAgentFig(MarketBookFigure[0], lstUserBets);
                                foreach (var runner in MarketBookFigure[0].Runners)
                                {
                                    runner.ProfitandLoss = Convert.ToInt64(MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                }
                            }
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                List<UserBetsforSuper> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforSuper>>(objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                                lstUserBets = lstUserBets.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBookFigure[0].MarketId).ToList();
                                MarketBookFigure[0].DebitCredit = objUserBets.ceckProfitandLossSuperFig(MarketBookFigure[0], lstUserBets);
                                foreach (var runner in MarketBookFigure[0].Runners)
                                {
                                    runner.ProfitandLoss = Convert.ToInt64(MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                }
                            }
                            if (LoggedinUserDetail.GetUserTypeID() == 9)
                            {
                                List<UserBetsforSamiadmin> lstUserBets = JsonConvert.DeserializeObject<List<UserBetsforSamiadmin>>(objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                                lstUserBets = lstUserBets.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBookFigure[0].MarketId).ToList();
                                MarketBookFigure[0].DebitCredit = objUserBets.ceckProfitandLossSamiadminFig(MarketBookFigure[0], lstUserBets);
                                foreach (var runner in MarketBookFigure[0].Runners)
                                {
                                    runner.ProfitandLoss = Convert.ToInt64(MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBookFigure[0].DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                }
                            }

                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
                return LoggedinUserDetail.ConverttoJSONString(MarketBookFigure);

            }
            else
            {
                return "";
            }

        }
    }
}