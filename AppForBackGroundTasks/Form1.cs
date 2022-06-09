using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AppForBackGroundTasks.UserServicesReference;

using System.Diagnostics;
using System.IO;

using AppForBackGroundTasks.BettingServiceReference;

using AppForBackGroundTasks.BettingServiceCricketCompletedMatchReference;
using AppForBackGroundTasks.BettingServiceCricketMatchOddsReference;
using AppForBackGroundTasks.BettingServiceCricketInningsRunsReference;
using AppForBackGroundTasks.BettingServiceSoccerReference;
using AppForBackGroundTasks.BettingServiceTennisReference;
using AppForBackGroundTasks.BettingServiceHorseRacePlaceReference;
using AppForBackGroundTasks.BettingServiceHorseWinReference;
using AppForBackGroundTasks.BettingServiceGrayHoundPlaceReference;
using AppForBackGroundTasks.BettingServiceGrayHoundWinReference;
using AppForBackGroundTasks.BettingServiceWinnerReference;
using System.Configuration;
namespace AppForBackGroundTasks
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (1 == 1)
                {

                    
                   


                 
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



                   
                }
            }
            catch (Exception ex)
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
                   
                    worker.RunWorkerAsync();
                }
            }
        }
        public static BettingServiceClient objBettingServiceClientforMAtchUpdates = new BettingServiceClient();
        private void DoWorkforworkerDownloadOddsCricketBallbyBall(object sender, DoWorkEventArgs e)
        {
            try
            {

                objBettingServiceClientforMAtchUpdates.GetBallbyBallSummaryAsync();
                System.Threading.Thread.Sleep(5000);

            }
            catch (System.Exception ex)
            {
                LogError(ex);
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
                  
                    worker.RunWorkerAsync();
                }
            }
        }
        public static UserServicesClient objUserServiceClientAutoEmail = new UserServicesClient();
        private void DoWorkAutoEmail(object sender, DoWorkEventArgs e)
        {
            try
            {

                objUserServiceClientAutoEmail.SendBalanceSheettoEmailAutomaticAsync(ConfigurationManager.AppSettings["PasswordForValidate"]);
                System.Threading.Thread.Sleep(30000);
            }
            catch (System.Exception ex)
            {
                LogError(ex);
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
                   
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsWinner(object sender, DoWorkEventArgs e)
        {
            objBettingClientWinner.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
            System.Threading.Thread.Sleep(200);
        }

        private void WorkerCompletedforworkerDownloadOddsGrayHoundRaceWin(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                 
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsGrayHoundRaceWin(object sender, DoWorkEventArgs e)
        {
            objBettingClientGrayHoundWin.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
            System.Threading.Thread.Sleep(200);
        }

        private void WorkerCompletedforworkerDownloadOddsGrayHoundRacePlace(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                 
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsGrayHoundRacePlace(object sender, DoWorkEventArgs e)
        {
            objBettingClientGrayHoundPlace.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
            System.Threading.Thread.Sleep(200);
        }

        private void WorkerCompletedforworkerDownloadOddsHorseRaceWin(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                    
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsHorseRaceWin(object sender, DoWorkEventArgs e)
        {
            try
            {

           
            objBettingClientHorseWin.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
            System.Threading.Thread.Sleep(200);
            }
            catch (Exception ex)
            {


            }
        }

        private void WorkerCompletedforworkerDownloadOddsHorseRacePlace(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                  
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsHorseRacePlace(object sender, DoWorkEventArgs e)
        {
            objBettingClientHorseRacePlace.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
            System.Threading.Thread.Sleep(200);
        }

        private void WorkerCompletedforworkerDownloadOddsTennis(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs

                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsTennis(object sender, DoWorkEventArgs e)
        {
            objBettingClientTennis.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
            System.Threading.Thread.Sleep(200);
        }

        private void WorkerCompletedforworkerDownloadOddsSoccer(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
              
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsSoccer(object sender, DoWorkEventArgs e)
        {
            objBettingClientSoccer.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
            System.Threading.Thread.Sleep(200);
        }

        private void WorkerCompletedforworkerDownloadOddsCricketInningRunns(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                 
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsCricketCricketInningRunns(object sender, DoWorkEventArgs e)
        {
            objBettingClientCricketInngsRunns.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
            System.Threading.Thread.Sleep(200);
        }

        private void WorkerCompletedforworkerDownloadOddsCricketMatchOdds(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                 
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsCricketMatchOdds(object sender, DoWorkEventArgs e)
        {
            objBettingClientCricketMatchOdds.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
            System.Threading.Thread.Sleep(200);
        }

        private void WorkerCompletedforworkerDownloadOddsCricketCompletedMatch(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {


                    // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
                   
                    worker.RunWorkerAsync();
                }
            }
        }

        private void DoWorkforDownloadOddsCricketCompletedMatch(object sender, DoWorkEventArgs e)
        {
            objBettingClientCricketCompletedMAtch.GetCurrentMarketBookCricket(ConfigurationManager.AppSettings["PasswordForValidate"]);
            System.Threading.Thread.Sleep(200);
        }
        public static UserServicesClient objUserServiceClientCloseMarket = new UserServicesClient();
        private static void DoWorkforworkerCloseAllClosedMarkets(object sender, DoWorkEventArgs e)
        {

            objUserServiceClientCloseMarket.CloseAllClosedMarketsAsync();
            System.Threading.Thread.Sleep(60000);

            // Sync up the Details
            // Loading ForEx Rates from external vendor System using Web Service
            // Log("The Elapsed event was raised at " + DateTime.Now.ToString());
        }
        public static UserServicesClient objUserServiceClientMatchCompelted = new UserServicesClient();
        private static void DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {


                objUserServiceClientMatchCompelted.CheckforMatchCompletedAsync();
                System.Threading.Thread.Sleep(30000);
            }
            catch (System.Exception ex)
            {
                LogError(ex);
            }


        }

        private static void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
               
                worker.RunWorkerAsync();
            }
        }
        private static void WorkerCompletedforworkerworkerCloseAllClosedMarkets(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                // sleep for 20 secs and again call DoWork to get FxRates..we can increase the time to sleep and make it configurable to the needs
               
                worker.RunWorkerAsync();
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



        public static UserServicesClient objBettingClientDownloadHorse = new UserServicesClient();
        private static void DoWorkforDownloadHorseRacingandGrayHound(object sender, DoWorkEventArgs e)
        {
            try
            {


                objBettingClientDownloadHorse.DownloadAllMarketHorseRaceAsync(ConfigurationManager.AppSettings["PasswordForValidate"]);
                System.Threading.Thread.Sleep(28800000);

            }
            catch (System.Exception ex)
            {
                LogError(ex);
            }


        }

        private static void WorkerCompletedforworkerDownloadHorseRacingandGrayHound(object sender, RunWorkerCompletedEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.IsBusy == false)
                {

                    worker.RunWorkerAsync();
                }
            }
        }
        public static UserServicesClient objUSerserviceWriteerror = new UserServicesClient();
        public static void WriteErrorToDB(string exception)
        {

            objUSerserviceWriteerror.AddUserActivity(exception, DateTime.Now, "", "", "", 1);
        }

        public static void LogError(Exception ex)
        {
            try
            {
                WriteErrorToDB(ex.Message + " " + ex.StackTrace + " " + ex.Source + " " + ex.TargetSite.ToString());
            }
            catch (System.Exception ex1)
            {

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
