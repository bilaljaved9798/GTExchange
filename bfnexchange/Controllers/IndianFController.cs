using bfnexchange.BettingServiceReference;
using bfnexchange.UsersServiceReference;
using ExternalAPI.TO;
using globaltraders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace bfnexchange.Controllers
{
    public class IndianFController : Controller
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceClient objBettingClient = new BettingServiceClient();
        // GET: IndianF
        public ActionResult Index()
        { 
         ViewBag.backgrod = "#1D9BF0";
         ViewBag.color = "white";

            return View();
        }

        List<RunnerForIndianFancy> lstMarketBookRunnersIndianFancy = new List<RunnerForIndianFancy>();
       

       
        //List<ExternalAPI.TO.MarketBookForindianFancy> marketbook3 = new List<ExternalAPI.TO.MarketBookForindianFancy>();
        //MarketBookForindianFancy marketbook2 = new MarketBookForindianFancy();
        UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
        public PartialViewResult InitializeGetFancy(string EventID, string MarketBookID)
        {          
            return PartialView("InFancyMarketBook", new List<MarketBookForindianFancy>());
        }
        public PartialViewResult GetInFancyMarket(string EventID, string MarketBookID)
        {
            if (LoggedinUserDetail.GetUserTypeID() != 3)
            {
                ViewBag.backgrod = "#1D9BF0";
                ViewBag.color = "white";
            }

            int UserIDforLinevmarkets = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLinevmarkets = 73;
            }
            else
            {
                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            }

            var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(EventID,DateTime.Now, UserIDforLinevmarkets));

            ExternalAPI.TO.GetDataFancy GetDataFancy = new ExternalAPI.TO.GetDataFancy();
            List<ExternalAPI.TO.MarketBookForindianFancy> LastloadedLinMarkets2 = new List<ExternalAPI.TO.MarketBookForindianFancy>();
            try
            {
                lstMarketBookRunnersIndianFancy.Clear();

                 GetDataFancy = JsonConvert.DeserializeObject<ExternalAPI.TO.GetDataFancy>(objBettingClient.GetRunnersForFancy(EventID, MarketBookID));
                GetDataFancy.session = GetDataFancy.session.Take(40).OrderBy(s => s.SelectionId).ToList();
                foreach (var runners in GetDataFancy.session.Take(15))
                    {
                    //var a = linevmarkets.Where(item => item.SelectionID == runners.SelectionId).ToList();
                    
                    //if (a.Count > 0)
                    //{
                        var runner = new ExternalAPI.TO.RunnerForIndianFancy();
                    //marketbook2.MainSportsname = "Fancy";
                    //marketbook2.MarketBookName = runners.RunnerName;
                    //marketbook2.MarketStatusstr = runners.GameStatus;
                    //marketbook2.EventID = EventID;
                   
                            runner.BettingAllowed = true;
                            runner.StallDraw = "IN-PLAY";
                            runner.JockeyName = "Fancy";
                            //marketbook2.BettingAllowed = true;
                            //marketbook2.BettingAllowedOverAll = true;
                            //marketbook2.MarketId = a[0].MarketCatalogueID; ;
                            runner.RunnerName = runners.RunnerName;
                            runner.StatusStr = runners.GameStatus;
                            runner.SelectionId = runners.SelectionId;
                            if (runners.LaySize1 == "SUSPENDED" || runners.LaySize1 == "Running")
                            {
                                runner.LaySize = "0";
                                runner.Layprice = "0";
                                runner.BackSize = "0";
                                runner.Backprice = "0";
                            }
                            else
                            {

                                string[] arrLS = runners.LaySize1.Split('.').ToArray();
                                runner.LaySize = arrLS[0].ToString();
                                string[] arrL = runners.LayPrice1.Split('.').ToArray();
                                runner.Layprice = arrL[0].ToString();
                                string[] arrbS = runners.BackSize1.Split('.').ToArray();
                                runner.BackSize = arrbS[0].ToString();
                                string[] arrb = runners.BackPrice1.Split('.').ToArray();
                                runner.Backprice = arrb[0].ToString();
                            }

                            ExternalAPI.TO.MarketBookForindianFancy currentmarketsfancyPL = new ExternalAPI.TO.MarketBookForindianFancy();

                        if (LoggedinUserDetail.GetUserTypeID() == 3)
                        {
                            List<Models.UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                           // lstUserBets = lstUserBets.Where(item => item.MarketBookID == a[0].MarketCatalogueID).ToList();
                            if (lstUserBets.Count != 0)
                            {
                                //currentmarketsfancyPL = objUserbets.GetBookPositionIN(a[0].MarketCatalogueID, runner.SelectionId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforSamiadmin>(), new List<Models.UserBetsforAgent>(), lstUserBets);
                            currentmarketsfancyPL = objUserbets.GetBookPositionINNew( runner.SelectionId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforSamiadmin>(), new List<Models.UserBetsforAgent>(), lstUserBets);
                        }
                        }
                       
                            if (LoggedinUserDetail.GetUserTypeID() == 2)
                            {
                                List<Models.UserBetsforAgent> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(objUsersServiceCleint.GetUserbetsbyUserIDandAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                               // lstUserBets = lstUserBets.Where(item => item.MarketBookID == a[0].MarketCatalogueID).ToList();
                                if (lstUserBets.Count != 0)
                                {
                            //currentmarketsfancyPL = objUserbets.GetBookPositionIN(a[0].MarketCatalogueID, runner.SelectionId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforSamiadmin>(), lstUserBets, new List<Models.UserBets>());
                            currentmarketsfancyPL = objUserbets.GetBookPositionINNew( runner.SelectionId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforSamiadmin>(), lstUserBets, new List<Models.UserBets>());
                        }
                            }
                            
                                if (LoggedinUserDetail.GetUserTypeID() == 8)
                                {
                                    List<Models.UserBetsforSuper> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsforSuper>>(objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                                    lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
                                    if (lstUserBets.Count != 0)
                                    {
                            //currentmarketsfancyPL = objUserbets.GetBookPositionIN(a[0].MarketCatalogueID, runner.SelectionId, new List<Models.UserBetsForAdmin>(), lstUserBets, new List<Models.UserBetsforSamiadmin>(), new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                            currentmarketsfancyPL = objUserbets.GetBookPositionINNew( runner.SelectionId, new List<Models.UserBetsForAdmin>(), lstUserBets, new List<Models.UserBetsforSamiadmin>(), new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                        }
                                }
                        if (LoggedinUserDetail.GetUserTypeID() == 9)
                        {
                            List<Models.UserBetsforSamiadmin> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsforSamiadmin>>(objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                            lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
                            if (lstUserBets.Count != 0)
                            {
                            //currentmarketsfancyPL = objUserbets.GetBookPositionIN(a[0].MarketCatalogueID, runner.SelectionId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), lstUserBets, new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                            currentmarketsfancyPL = objUserbets.GetBookPositionINNew( runner.SelectionId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), lstUserBets, new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                        }
                        }
                        if (LoggedinUserDetail.GetUserTypeID() == 9)
                        {
                            {
                                List<Models.UserBetsForAdmin> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                                lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
                                if (lstUserBets.Count != 0)
                                {
                                //currentmarketsfancyPL = objUserbets.GetBookPositionIN(a[0].MarketCatalogueID, runner.SelectionId, lstUserBets, new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforSamiadmin>(), new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                                currentmarketsfancyPL = objUserbets.GetBookPositionINNew( runner.SelectionId, lstUserBets, new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforSamiadmin>(), new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
                            }
                            }
                        }
                            
                        double TotalProfit = 0;
                            double TotalLoss = 0;
                        if (currentmarketsfancyPL.RunnersForindianFancy != null)
                        {
                            TotalProfit = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Max(t => t.ProfitandLoss));
                            TotalLoss = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Min(t => t.ProfitandLoss));
                            runner.ProfitandLoss = TotalProfit;
                            runner.Loss = TotalLoss;
                        }
                        else
                        {
                            runner.ProfitandLoss = TotalLoss;
                            runner.Loss = TotalProfit;
                        }

                    runner.MarketBookID = EventID;//a[0].MarketCatalogueID ;
                            runner.WearingURL = GetDataFancy.session.Count.ToString();
                            lstMarketBookRunnersIndianFancy.Add(runner);
                       // }
                   
                    }
                              
                    return PartialView("InFancyMarketBook", lstMarketBookRunnersIndianFancy);            
            }
            catch (System.Exception ex)
            {
                return PartialView("InFancyMarketBook", new List<RunnerForIndianFancy>());
            }
           
        }


        //public string LoadFancyMarketIN(string EventID, string MarketBookID)
        //{
        //    var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(MarketBookID);
        //    int UserIDforLinevmarkets = 0;
        //    if (LoggedinUserDetail.GetUserTypeID() == 1)
        //    {
        //        UserIDforLinevmarkets = 73;
        //    }
        //    else
        //    {
        //        UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
        //    }

        //    var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(resultslinev.EventID, resultslinev.EventOpenDate.Value, UserIDforLinevmarkets));


        //    List<ExternalAPI.TO.MarketBookForindianFancy> LastloadedLinMarkets2 = new List<ExternalAPI.TO.MarketBookForindianFancy>();

        //        LastloadedLinMarkets2.Clear();
        //        LastloadedLinMarkets2.Add(objBettingClient.GetMarketDatabyIDIndianFancy(EventID, MarketBookID));

        //    foreach (var usermarket in LastloadedLinMarkets2)
        //    {
        //        foreach (var runners in usermarket.RunnersForindianFancy)
        //        {
        //            var a = linevmarkets.Where(item => item.SelectionID == runners.SelectionId).ToList();
        //            if (a.Count > 0)
        //            {
        //                var runner = new ExternalAPI.TO.RunnerForIndianFancy();
        //                marketbook2.MainSportsname = "Fancy";
        //                marketbook2.MarketBookName = runners.RunnerName;
        //                marketbook2.MarketStatusstr = runners.StatusStr;
        //                marketbook2.EventID = EventID;
        //                runner.BettingAllowed = a[0].BettingAllowed;
        //                runner.StallDraw = "IN-PLAY";
        //                runner.JockeyName = "Fancy";
        //                marketbook2.BettingAllowed = true;
        //                marketbook2.BettingAllowedOverAll = true;
        //                marketbook2.MarketId = a[0].MarketCatalogueID;
        //                runner.RunnerName = runners.RunnerName;
        //                runner.StatusStr = runners.MarketStatusStr;
        //                runner.SelectionId = runners.SelectionId;
        //                if (runners.LaySize == "SUSPENDED" || runners.LaySize == "Running")
        //                {
        //                    runner.LaySize = "0";
        //                    runner.Layprice = "0";
        //                    runner.BackSize = "0";
        //                    runner.Backprice = "0";
        //                }
        //                else
        //                {

        //                    string[] arrLS = runners.LaySize.Split('.').ToArray();
        //                    runner.LaySize = arrLS[0].ToString();
        //                    string[] arrL = runners.Layprice.Split('.').ToArray();
        //                    runner.Layprice = arrL[0].ToString();
        //                    string[] arrbS = runners.BackSize.Split('.').ToArray();
        //                    runner.BackSize = arrbS[0].ToString();
        //                    string[] arrb = runners.Backprice.Split('.').ToArray();
        //                    runner.Backprice = arrb[0].ToString();
        //                }

        //                ExternalAPI.TO.MarketBookForindianFancy currentmarketsfancyPL = new ExternalAPI.TO.MarketBookForindianFancy();

        //                if (LoggedinUserDetail.GetUserTypeID() == 3)
        //                {
        //                    List<Models.UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
        //                    lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
        //                    if (lstUserBets.Count != 0)
        //                    {
        //                        currentmarketsfancyPL = objUserbets.GetBookPositionIN(EventID, runner.SelectionId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforAgent>(), lstUserBets);
        //                    }
        //                }
        //                else
        //                {
        //                    if (LoggedinUserDetail.GetUserTypeID() == 2)
        //                    {
        //                        List<Models.UserBetsforAgent> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(objUsersServiceCleint.GetUserbetsbyUserIDandAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
        //                        lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
        //                        if (lstUserBets.Count != 0)
        //                        {
        //                            currentmarketsfancyPL = objUserbets.GetBookPositionIN(EventID, runner.SelectionId, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), lstUserBets, new List<Models.UserBets>());
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (LoggedinUserDetail.GetUserTypeID() == 8)
        //                        {
        //                            List<Models.UserBetsforSuper> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsforSuper>>(objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
        //                            lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
        //                            if (lstUserBets.Count != 0)
        //                            {
        //                                currentmarketsfancyPL = objUserbets.GetBookPositionIN(EventID, runner.SelectionId, new List<Models.UserBetsForAdmin>(), lstUserBets, new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
        //                            }
        //                        }
        //                        else
        //                        {
        //                            List<Models.UserBetsForAdmin> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
        //                            lstUserBets = lstUserBets.Where(item => item.SelectionID == runner.SelectionId).ToList();
        //                            if (lstUserBets.Count != 0)
        //                            {
        //                                currentmarketsfancyPL = objUserbets.GetBookPositionIN(EventID, runner.SelectionId, lstUserBets, new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());
        //                            }
        //                        }
        //                    }
        //                }
        //                double TotalProfit = 0;
        //                double TotalLoss = 0;
        //                if (currentmarketsfancyPL.RunnersForindianFancy != null)
        //                {
        //                    TotalProfit = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Max(t => t.ProfitandLoss));
        //                    TotalLoss = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Min(t => t.ProfitandLoss));
        //                    runner.ProfitandLoss = TotalProfit;
        //                    runner.Loss = TotalLoss;
        //                }
        //                else
        //                {
        //                    runner.ProfitandLoss = TotalLoss;
        //                    runner.Loss = TotalProfit;
        //                }

        //                runner.MarketBookID = a[0].MarketCatalogueID;
        //                runner.WearingURL = usermarket.RunnersForindianFancy.Count.ToString();
        //                lstMarketBookRunnersIndianFancy.Add(runner);
        //            }
        //        }
        //    }

        //        marketbook2.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>(lstMarketBookRunnersIndianFancy);
        //        marketbook3.Add(marketbook2);

        //        return LoggedinUserDetail.ConverttoJSONString(marketbook3);
        //    }
                                   
        } 
   
   }