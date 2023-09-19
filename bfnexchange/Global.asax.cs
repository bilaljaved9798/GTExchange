using bfnexchange.Controllers;
using bfnexchange.Models;
using bfnexchange.UsersServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using bfnexchange.BettingServiceReference;
using WebGrease;
using bfnexchange.BettingServiceCricketCompletedMatchReference;
using bfnexchange.BettingServiceCricketMatchOddsReference;
using bfnexchange.BettingServiceCricketInningsRunsReference;
using bfnexchange.BettingServiceSoccerReference;
using bfnexchange.BettingServiceTennisReference;
using bfnexchange.BettingServiceHorseRacingPlaceReference;
using bfnexchange.BettingServiceHorseWinReference;
using bfnexchange.BettingServiceGrayHoundPlaceReference;
using bfnexchange.BettingServiceGrayHoundWinReference;
using bfnexchange.BettingServiceWinnerReference;
using System.Configuration;
using Microsoft.ApplicationInsights.Extensibility;
using System.Web.Hosting;

namespace bfnexchange
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            try
            {
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(new RazorViewEngine());

                AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
              
                TelemetryConfiguration.Active.DisableTelemetry = true;
                if (1 == 1)
                {
                    //SetMarketOpened
                    BackgroundWorker workerSetMarketOpened = new BackgroundWorker();
                    workerSetMarketOpened.DoWork += new DoWorkEventHandler(DoWorkforworkerSetMarketOpened);
                    workerSetMarketOpened.WorkerReportsProgress = false;
                    workerSetMarketOpened.WorkerSupportsCancellation = true;
                    workerSetMarketOpened.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerSetMarketOpened);
                    workerSetMarketOpened.RunWorkerAsync();

                    //////CricketBallbyBallSummary
                    BackgroundWorker workerDownloadOddsCricketBallbyBall = new BackgroundWorker();
                    workerDownloadOddsCricketBallbyBall.DoWork += new DoWorkEventHandler(DoWorkforworkerDownloadOddsCricketBallbyBall);
                    workerDownloadOddsCricketBallbyBall.WorkerReportsProgress = false;
                    workerDownloadOddsCricketBallbyBall.WorkerSupportsCancellation = true;
                    workerDownloadOddsCricketBallbyBall.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOddsCricketBallbyBall);
                    workerDownloadOddsCricketBallbyBall.RunWorkerAsync();//we can als

                    //////CricketBallbyBallSummary
                    BackgroundWorker workerDownloadOddsCricketBallbyBallNew = new BackgroundWorker();
                    workerDownloadOddsCricketBallbyBallNew.DoWork += new DoWorkEventHandler(DoWorkforworkerDownloadOddsCricketBallbyBallNew);
                    workerDownloadOddsCricketBallbyBallNew.WorkerReportsProgress = false;
                    workerDownloadOddsCricketBallbyBallNew.WorkerSupportsCancellation = true;
                    workerDownloadOddsCricketBallbyBallNew.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOddsCricketBallbyBallNew);
                    workerDownloadOddsCricketBallbyBallNew.RunWorkerAsync();//we can als

                    //////GetAllMarkets
                    BackgroundWorker workerDownloadAllMarkets = new BackgroundWorker();
                    workerDownloadAllMarkets.DoWork += new DoWorkEventHandler(DoWorkforworkerDownloadAllMarkets);
                    workerDownloadAllMarkets.WorkerReportsProgress = false;
                    workerDownloadAllMarkets.WorkerSupportsCancellation = true;
                    workerDownloadAllMarkets.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadAllMarkets);
                    workerDownloadAllMarkets.RunWorkerAsync();//we can als

                    //CloseAllCloseMarket                                                 ////
                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += new DoWorkEventHandler(DoWork);
                    worker.WorkerReportsProgress = false;
                    worker.WorkerSupportsCancellation = true;
                    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompleted);
                    worker.RunWorkerAsync();


                    ///AutomaticEmailBalanceSheet
                    BackgroundWorker workerAutoEmail = new BackgroundWorker();
                    workerAutoEmail.DoWork += new DoWorkEventHandler(DoWorkAutoEmail);
                    workerAutoEmail.WorkerReportsProgress = false;
                    workerAutoEmail.WorkerSupportsCancellation = true;
                    workerAutoEmail.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(WorkerCompletedAutoEmail);
                    workerAutoEmail.RunWorkerAsync();

                    ///AutomaticMarket
                    //BackgroundWorker workerGetmarket = new BackgroundWorker();
                    //workerGetmarket.DoWork += new DoWorkEventHandler(DoWorkGetmarket);
                    //workerGetmarket.WorkerReportsProgress = false;
                    //workerGetmarket.WorkerSupportsCancellation = true;
                    //workerGetmarket.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedAutoEmail);
                    //workerGetmarket.RunWorkerAsync();


                    ////Updateunmatchedbets
                    BackgroundWorker worker1 = new BackgroundWorker();
                    worker1.DoWork += new DoWorkEventHandler(DoWorkforDownloadMarket);
                    worker1.WorkerReportsProgress = false;
                    worker1.WorkerSupportsCancellation = true;
                    worker1.RunWorkerCompleted +=
                    new RunWorkerCompletedEventHandler(WorkerCompletedforDownloadMarket);
                    worker1.RunWorkerAsync();
                    //we can als

                    BackgroundWorker workerDownloadOdds = new BackgroundWorker();
                    workerDownloadOdds.DoWork += new DoWorkEventHandler(DoWorkforDownloadOdds);
                    workerDownloadOdds.WorkerReportsProgress = false;
                    workerDownloadOdds.WorkerSupportsCancellation = true;
                    workerDownloadOdds.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOdds);
                    workerDownloadOdds.RunWorkerAsync();//we can als
                    //Winner Markets
                    BackgroundWorker workerDownloadOddsWinner = new BackgroundWorker();
                    workerDownloadOddsWinner.DoWork += new DoWorkEventHandler(DoWorkforDownloadOddsWinner);
                    workerDownloadOddsWinner.WorkerReportsProgress = false;
                    workerDownloadOddsWinner.WorkerSupportsCancellation = true;
                    workerDownloadOddsWinner.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOddsWinner);
                    workerDownloadOddsWinner.RunWorkerAsync();//we can als
                    //CricketCompletedMatch
                    BackgroundWorker workerDownloadOddsCricketCompletedMatch = new BackgroundWorker();
                    workerDownloadOddsCricketCompletedMatch.DoWork += new DoWorkEventHandler(DoWorkforDownloadOddsCricketCompletedMatch);
                    workerDownloadOddsCricketCompletedMatch.WorkerReportsProgress = false;
                    workerDownloadOddsCricketCompletedMatch.WorkerSupportsCancellation = true;
                    workerDownloadOddsCricketCompletedMatch.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOddsCricketCompletedMatch);
                    workerDownloadOddsCricketCompletedMatch.RunWorkerAsync();//we can als

                    //CricketMatchOdds
                    BackgroundWorker workerDownloadOddsCricketMatchOdds = new BackgroundWorker();
                    workerDownloadOddsCricketMatchOdds.DoWork += new DoWorkEventHandler(DoWorkforDownloadOddsCricketMatchOdds);
                    workerDownloadOddsCricketMatchOdds.WorkerReportsProgress = false;
                    workerDownloadOddsCricketMatchOdds.WorkerSupportsCancellation = true;
                    workerDownloadOddsCricketMatchOdds.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOddsCricketMatchOdds);
                    workerDownloadOddsCricketMatchOdds.RunWorkerAsync();//we can als
                    //CricketInningRunns
                    BackgroundWorker workerDownloadOddsCricketInningRunns = new BackgroundWorker();
                    workerDownloadOddsCricketInningRunns.DoWork += new DoWorkEventHandler(DoWorkforDownloadOddsCricketCricketInningRunns);
                    workerDownloadOddsCricketInningRunns.WorkerReportsProgress = false;
                    workerDownloadOddsCricketInningRunns.WorkerSupportsCancellation = true;
                    workerDownloadOddsCricketInningRunns.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOddsCricketInningRunns);
                    workerDownloadOddsCricketInningRunns.RunWorkerAsync();//we can als
                    // Soccer
                    BackgroundWorker workerDownloadOddsSoccer = new BackgroundWorker();
                    workerDownloadOddsSoccer.DoWork += new DoWorkEventHandler(DoWorkforDownloadOddsSoccer);
                    workerDownloadOddsSoccer.WorkerReportsProgress = false;
                    workerDownloadOddsSoccer.WorkerSupportsCancellation = true;
                    workerDownloadOddsSoccer.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOddsSoccer);
                    workerDownloadOddsSoccer.RunWorkerAsync();//we can als
                    //Tennis
                    BackgroundWorker workerDownloadOddsTennis = new BackgroundWorker();
                    workerDownloadOddsTennis.DoWork += new DoWorkEventHandler(DoWorkforDownloadOddsTennis);
                    workerDownloadOddsTennis.WorkerReportsProgress = false;
                    workerDownloadOddsTennis.WorkerSupportsCancellation = true;
                    workerDownloadOddsTennis.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOddsTennis);
                    workerDownloadOddsTennis.RunWorkerAsync();//we can als
                    //Horse Race Place
                    BackgroundWorker workerDownloadOddsHorseRacePlace = new BackgroundWorker();
                    workerDownloadOddsHorseRacePlace.DoWork += new DoWorkEventHandler(DoWorkforDownloadOddsHorseRacePlace);
                    workerDownloadOddsHorseRacePlace.WorkerReportsProgress = false;
                    workerDownloadOddsHorseRacePlace.WorkerSupportsCancellation = true;
                    workerDownloadOddsHorseRacePlace.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOddsHorseRacePlace);
                    workerDownloadOddsHorseRacePlace.RunWorkerAsync();//we can als
                    //Horse Race Win
                    BackgroundWorker workerDownloadOddsHorseRaceWin = new BackgroundWorker();
                    workerDownloadOddsHorseRaceWin.DoWork += new DoWorkEventHandler(DoWorkforDownloadOddsHorseRaceWin);
                    workerDownloadOddsHorseRaceWin.WorkerReportsProgress = false;
                    workerDownloadOddsHorseRaceWin.WorkerSupportsCancellation = true;
                    workerDownloadOddsHorseRaceWin.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOddsHorseRaceWin);
                    workerDownloadOddsHorseRaceWin.RunWorkerAsync();//we can als
                    // GrayHound Place
                    BackgroundWorker workerDownloadOddsGrayHoundRacePlace = new BackgroundWorker();
                    workerDownloadOddsGrayHoundRacePlace.DoWork += new DoWorkEventHandler(DoWorkforDownloadOddsGrayHoundRacePlace);
                    workerDownloadOddsGrayHoundRacePlace.WorkerReportsProgress = false;
                    workerDownloadOddsGrayHoundRacePlace.WorkerSupportsCancellation = true;
                    workerDownloadOddsGrayHoundRacePlace.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOddsGrayHoundRacePlace);
                    workerDownloadOddsGrayHoundRacePlace.RunWorkerAsync();//we can als
                    // GrayHound Win
                    BackgroundWorker workerDownloadOddsGrayHoundRaceWin = new BackgroundWorker();
                    workerDownloadOddsGrayHoundRaceWin.DoWork += new DoWorkEventHandler(DoWorkforDownloadOddsGrayHoundRaceWin);
                    workerDownloadOddsGrayHoundRaceWin.WorkerReportsProgress = false;
                    workerDownloadOddsGrayHoundRaceWin.WorkerSupportsCancellation = true;
                    workerDownloadOddsGrayHoundRaceWin.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadOddsGrayHoundRaceWin);
                    workerDownloadOddsGrayHoundRaceWin.RunWorkerAsync();//we can als

                    /////////////////////
                    BackgroundWorker workerDownloadHorseRacingandGrayHound = new BackgroundWorker();
                    workerDownloadHorseRacingandGrayHound.DoWork += new DoWorkEventHandler(DoWorkforDownloadHorseRacingandGrayHound);
                    workerDownloadHorseRacingandGrayHound.WorkerReportsProgress = false;
                    workerDownloadHorseRacingandGrayHound.WorkerSupportsCancellation = true;
                    workerDownloadHorseRacingandGrayHound.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerDownloadHorseRacingandGrayHound);
                    workerDownloadHorseRacingandGrayHound.RunWorkerAsync();//we can als
                    /////////////////////////////
                    BackgroundWorker workerCloseAllClosedMarkets = new BackgroundWorker();
                    workerCloseAllClosedMarkets.DoWork += new DoWorkEventHandler(DoWorkforworkerCloseAllClosedMarkets);
                    workerCloseAllClosedMarkets.WorkerReportsProgress = false;
                    workerCloseAllClosedMarkets.WorkerSupportsCancellation = true;
                    workerCloseAllClosedMarkets.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompletedforworkerworkerCloseAllClosedMarkets);
                    workerCloseAllClosedMarkets.RunWorkerAsync();//we can als
                }
            }
            catch (Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
            }
        }
        private void WorkerCompletedforworkerSetMarketOpened(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }
        public static UserServicesClient objUserServiceforSetMarketOpened = new UserServicesClient();
        private void DoWorkforworkerSetMarketOpened(object sender, DoWorkEventArgs e)
        {
            try
            {
                objUserServiceforSetMarketOpened.SetMarketOpenedbyuserinAPP();

            }
            catch (Exception)
            {


            }
        }

        private void WorkerCompletedforworkerDownloadOddsCricketBallbyBall(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }

        private void WorkerCompletedforworkerDownloadOddsCricketBallbyBallNew(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }

        private void WorkerCompletedforworkerDownloadAllMarkets(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(500000);
                    worker.RunWorkerAsync();
                }
            }
        }
        public static BettingServiceClient objBettingServiceClientforMAtchUpdates = new BettingServiceClient();
        private void DoWorkforworkerDownloadOddsCricketBallbyBall(object sender, DoWorkEventArgs e)
        {
            try
            {
                objBettingServiceClientforMAtchUpdates.GetBallbyBallSummary();
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
            }
        }

        private void DoWorkforworkerDownloadOddsCricketBallbyBallNew(object sender, DoWorkEventArgs e)
        {
            try
            {
                objBettingServiceClientforMAtchUpdates.GetBallbyBallSummaryNew();
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
            }
        }
        public static UserServicesClient users = new UserServicesClient();
        private void DoWorkforworkerDownloadAllMarkets(object sender, DoWorkEventArgs e)
        {
            try
            {
                //objBettingServiceClientforMAtchUpdates.GetBallbyBallSummary();
                users.GetInPlayMatcheswithRunners1(73);
               // GetInPlayMatcheswithRunners1(73);
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
            }
        }

        private void WorkerCompletedAutoEmail(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(30000);
                    worker.RunWorkerAsync();
                }
            }
        }
        public static UserServicesClient objUserServiceClientAutoEmail = new UserServicesClient();
        private void DoWorkAutoEmail(object sender, DoWorkEventArgs e)
        {
            try
            {
                objUserServiceClientAutoEmail.SendBalanceSheettoEmailAutomatic(ConfigurationManager.AppSettings["PasswordForValidate"]);

            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
            }
        }
       

        private void WorkerCompletedforworkerDownloadOddsWinner(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsWinner(object sender, DoWorkEventArgs e)
        {
            objBettingClientWinner.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
        }

        private void WorkerCompletedforworkerDownloadOddsGrayHoundRaceWin(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {


                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsGrayHoundRaceWin(object sender, DoWorkEventArgs e)
        {
            objBettingClientGrayHoundWin.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
        }

        private void WorkerCompletedforworkerDownloadOddsGrayHoundRacePlace(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsGrayHoundRacePlace(object sender, DoWorkEventArgs e)
        {
            objBettingClientGrayHoundPlace.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
        }

        private void WorkerCompletedforworkerDownloadOddsHorseRaceWin(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsHorseRaceWin(object sender, DoWorkEventArgs e)
        {
            objBettingClientHorseWin.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
        }

        private void WorkerCompletedforworkerDownloadOddsHorseRacePlace(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsHorseRacePlace(object sender, DoWorkEventArgs e)
        {
            objBettingClientHorseRacePlace.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
        }

        private void WorkerCompletedforworkerDownloadOddsTennis(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsTennis(object sender, DoWorkEventArgs e)
        {
            objBettingClientTennis.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
        }

        private void WorkerCompletedforworkerDownloadOddsSoccer(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsSoccer(object sender, DoWorkEventArgs e)
        {
            objBettingClientSoccer.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
        }

        private void WorkerCompletedforworkerDownloadOddsCricketInningRunns(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }

        private void WorkerCompletedforworkerDownloadOddsCricketInningRunnslive(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }
        
        private void DoWorkforDownloadOddsCricketCricketInningRunns(object sender, DoWorkEventArgs e)
        {
            objBettingClientCricketInngsRunns.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
        }
       
        

        private void WorkerCompletedforworkerDownloadOddsCricketMatchOdds(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsCricketMatchOdds(object sender, DoWorkEventArgs e)
        {
            objBettingClientCricketMatchOdds.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
        }

        private void WorkerCompletedforworkerDownloadOddsCricketCompletedMatch(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(200);
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsCricketCompletedMatch(object sender, DoWorkEventArgs e)
        {
            objBettingClientCricketCompletedMAtch.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
        }
        public static UserServicesClient objUserServiceClientCloseMarket = new UserServicesClient();
        private static void DoWorkforworkerCloseAllClosedMarkets(object sender, DoWorkEventArgs e)
        {

            //objUserServiceClientCloseMarket.CloseAllClosedMarkets();

            // Sync up the Details
            // Loading ForEx Rates from external vendor System using Web Service
            // Log("The Elapsed event was raised at " + DateTime.Now.ToString());
        }
        public static UserServicesClient objUserServiceClientMatchCompelted = new UserServicesClient();
        private static void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //objUserServiceClientMatchCompelted.CheckforMatchCompleted();
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
            }
        }

        private static void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                System.Threading.Thread.Sleep(30000);
                worker.RunWorkerAsync();
            }
        }
        private static void WorkerCompletedforworkerworkerCloseAllClosedMarkets(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                System.Threading.Thread.Sleep(60000);
                worker.RunWorkerAsync();
            }
        }
        public static UserBetsUpdateUnmatcedBets objUserBetsUpdateUnMtachBets = new UserBetsUpdateUnmatcedBets();
        private static void DoWorkforDownloadMarket(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (LoggedinUserDetail.isWorkingonBets == false)
                {

                   // objUserBetsUpdateUnMtachBets.UpdateUnMatchBetsforAllUsers();
                    //MarketController objMarketcontroller = new MarketController();
                    //objMarketcontroller.UpdateUnMatchBetsforAllUsers();
                }
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
            }

            //UserServicesClient objUserServiceClient = new UserServicesClient();
            //objUserServiceClient.DownloadAllMarket();
            // Sync up the Details
            // Loading ForEx Rates from external vendor System using Web Service
            // Log("The Elapsed event was raised at " + DateTime.Now.ToString());
        }

        private static void WorkerCompletedforDownloadMarket(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(10);
                    worker.RunWorkerAsync();
                }
            }
        }
        public static BettingServiceCricketCompletedMatchClient objBettingClientCricketCompletedMAtch = new BettingServiceCricketCompletedMatchClient();
        public static BettingServiceCricketMatchOddsClient objBettingClientCricketMatchOdds = new BettingServiceCricketMatchOddsClient();
        public static BettingServiceCricketInningsRunsClient objBettingClientCricketInngsRunns = new BettingServiceCricketInningsRunsClient();
        public static BettingServiceSoccerClient objBettingClientSoccer = new BettingServiceSoccerClient();
        public static BettingServiceTennisClient objBettingClientTennis = new BettingServiceTennisClient();
        public static BettingServiceHorseRacePlaceClient objBettingClientHorseRacePlace = new BettingServiceHorseRacePlaceClient();
        public static BettingServiceHorseWinClient objBettingClientHorseWin = new BettingServiceHorseWinClient();
        public static BettingServiceGrayHoundPlaceClient objBettingClientGrayHoundPlace = new BettingServiceGrayHoundPlaceClient();
        public static BettingServiceGrayHoundWinClient objBettingClientGrayHoundWin = new BettingServiceGrayHoundWinClient();
        public static BettingServiceWinnerClient objBettingClientWinner = new BettingServiceWinnerClient();


        private static void DoWorkforDownloadOdds(object sender, DoWorkEventArgs e)
        {
            try
            {

                //using (BettingServiceClient objBettingClient = new BettingServiceClient())
                //{
                //    objBettingClient.GetDataFromBetfairReadOnly();
                //}


            }
            catch (System.Exception ex)
            {

            }
        }
        public static UserServicesClient objBettingClientDownloadHorse = new UserServicesClient();
        private static void DoWorkforDownloadHorseRacingandGrayHound(object sender, DoWorkEventArgs e)
        {
            try
            {
              //  objBettingClientDownloadHorse.DownloadAllMarketHorseRace(ConfigurationManager.AppSettings["PasswordForValidate"]);              
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
            }


        }
        private static void WorkerCompletedforworkerDownloadOdds(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    System.Threading.Thread.Sleep(5);
                    worker.RunWorkerAsync();
                }
            }
        }
        private static void WorkerCompletedforworkerDownloadHorseRacingandGrayHound(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {
                    System.Threading.Thread.Sleep(28800000);
                    worker.RunWorkerAsync();
                }
            }
        }

        public static void Log(string str)
        {

            StreamWriter Tex = File.AppendText(@"c:\Backuplog.txt");
            Tex.WriteLine(DateTime.Now.ToString() + " " + str);
            Tex.Close();

        }
        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown
            //If background worker process is running then clean up that object.
            //if (CacheManager.IsExists("BackgroundWorker"))
            //{
            //    BackgroundWorker worker = (BackgroundWorker)CacheManager.Get("BackgroundWorker");
            //    if (worker != null)
            //        worker.CancelAsync();
            //}
        }
    }
}
