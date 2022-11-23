using bftradeline;

using ExternalAPI.TO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using RestSharp;
using globaltraders.UserServiceReference;
using System.Windows.Media.Animation;
using globaltraders.HelperClasses;
using bftradeline.Models;
using System.Text.RegularExpressions;
using globaltraders.Service123Reference;
using System.Runtime.InteropServices;
using WinInterop = System.Windows.Interop;
using bftradeline.HelperClasses;
using System.Threading;

using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections.Specialized;
//using Litzscore;
using globaltraders.CricketScoreServiceReference;
using System.IO;
using System.Xml;
using globaltraders.Models;
using System.ServiceModel;

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for MarketWindow.xaml
    /// </summary>
    public partial class MarketWindowUserControl
    {

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Get the sender observable collection
            ObservableCollection<UserBetsForAdmin> obsSender = sender as ObservableCollection<UserBetsForAdmin>;
            NotifyCollectionChangedAction action = e.Action;

            //Get the sender observable collection         
        }



        public void setFirstTimeLoadEvents()
        {
            this.lstCurrentBetsAdmin = new ObservableCollection<UserBetsForAdmin>();
            this.lstCurrentBetsSuper = new ObservableCollection<UserBetsforSuper>();
            this.lstCurrentBetssamiadmin = new ObservableCollection<UserBetsforSamiAdmin>();
            this.lstCurrentBets = new ObservableCollection<UserBets>();
            this.lstCurrentBetsAgent = new ObservableCollection<UserBetsforAgent>();
            this.lstCurrentBetsAdminUnMAtched = new ObservableCollection<UserBetsForAdmin>();
            this.lstCurrentBetsUnMatched = new ObservableCollection<UserBets>();
            this.lstCurrentBetsAgentUnMatched = new ObservableCollection<UserBetsforAgent>();
            this.lstCurrentBetSuperUnMAtched = new ObservableCollection<UserBetsforSuper>();
            this.lstCurrentBetsamiadminUnMAtched = new ObservableCollection<UserBetsforSamiAdmin>();

            DGVMarket.DataContext = lstMarketBookRunners;
            DGVMarketToWintheToss.DataContext = lstMarketBookRunnersToWintheToss;
            //DGVMarketSoccer.DataContext = lstMarketBookRunnersToWintheToss;
            //DGVMarketToWintheToss1.DataContext = lstMarketBookRunnersToWintheToss1;
            //DGVMarketToWintheToss2.DataContext = lstMarketBookRunnersToWintheToss2;


            //DGVMarketFancy.DataContext = lstMarketBookRunnersFancy;
            // DGVMarketSoccer.DataContext = lstMarketBookRunnersSoccer;
            DGVMarketIndianFancy.DataContext = lstMarketBookRunnersFancyin;
            DGVMarketFigure.DataContext = lstMarketBookRunnersFigure;
            DGVMarketKalijut.DataContext = lstMarketBookRunnerKalijut;
            // DGVMatchedBetsSuper.DataContext = lstCurrentBetsSuper;
            DGVMatchedBetsAdmin.DataContext = lstCurrentBetsAdmin;
            DGVMatchedBets.DataContext = lstCurrentBets;
            DGVMatchedBetsaGENT.DataContext = lstCurrentBetsAgent;
            //
            if (LoggedinUserDetail.GetUserTypeID() == 8)
            {
                DGVMatchedBetsAdmin.DataContext = lstCurrentBetsSuper;
                DGVUnMatchedAdmin.DataContext = lstCurrentBetSuperUnMAtched;
            }
            if (LoggedinUserDetail.GetUserTypeID() == 9)
            {
                DGVMatchedBetsAdmin.DataContext = lstCurrentBetssamiadmin;
                DGVUnMatchedAdmin.DataContext = lstCurrentBetsamiadminUnMAtched;
            }
            //DGVUnMatchedSuper.DataContext = lstCurrentBetSuperUnMAtched;
            DGVUnMatched.DataContext = lstCurrentBetsUnMatched;
            DGVUnMatchedAgent.DataContext = lstCurrentBetsAgentUnMatched;
            DGVUnMatchedAdmin.DataContext = lstCurrentBetsAdminUnMAtched;


            ////  backgroundWorkertotalmatched = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            //  backgroundWorkertotalmatched.DoWork += backgroundWorkertotalmatched_DoWork;
            // backgroundWorkertotalmatched.RunWorkerCompleted += backgroundWorkertotalmatched_RunWorkerCompleted;

            backgroundWorkerInsertBet = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerInsertBet.DoWork += BackgroundWorkerInsertBet_DoWork;
            backgroundWorkerInsertBet.RunWorkerCompleted += BackgroundWorkerInsertBet_RunWorkerCompleted;

            backgroundWorkerInsertBetin = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerInsertBetin.DoWork += BackgroundWorkerInsertBetin_DoWork;
            backgroundWorkerInsertBetin.RunWorkerCompleted += BackgroundWorkerInsertBetin_RunWorkerCompleted;

            backgroundWorkerInsertBetin1 = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerInsertBetin1.DoWork += BackgroundWorkerInsertBetin1_DoWork;
            backgroundWorkerInsertBetin1.RunWorkerCompleted += BackgroundWorkerInsertBetin1_RunWorkerCompleted;

            backgroundWorkerGetScore = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerGetScore.DoWork += backgroundWorker_DoWork;
            backgroundWorkerGetScore.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            backgroundWorkerGetScoreSCT = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerGetScoreSCT.DoWork += backgroundWorkerSCT_DoWork;
            backgroundWorkerGetScoreSCT.RunWorkerCompleted += backgroundWorkerSCT_RunWorkerCompleted;

            backgroundWorkerUpdateData = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerUpdateData.DoWork += BackgroundWorkerUpdateData_DoWork;
            backgroundWorkerUpdateData.RunWorkerCompleted += BackgroundWorkerUpdateData_RunWorkerCompleted;

            backgroundWorkerUpdateFancyData = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerUpdateFancyData.DoWork += BackgroundWorkerFancyData_DoWork;
            backgroundWorkerUpdateFancyData.RunWorkerCompleted += BackgroundWorkerFancyData_RunWorkerCompleted;

            backgroundWorkerUpdateFigData = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerUpdateFigData.DoWork += BackgroundWorkerUpdateFigData_DoWork;
            backgroundWorkerUpdateFigData.RunWorkerCompleted += BackgroundWorkerUpdateFigData_RunWorkerCompleted;


            backgroundWorkerUpdateAllotherData = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerUpdateAllotherData.DoWork += BackgroundWorkerUpdateAllotherData_DoWork;
            backgroundWorkerUpdateAllotherData.RunWorkerCompleted += BackgroundWorkerUpdateAllotherData_RunWorkerCompleted;

            backgroundWorkerLiabalityandScore = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerLiabalityandScore.DoWork += BackgroundWorkerLiabalityandScore_DoWork;
            backgroundWorkerLiabalityandScore.RunWorkerCompleted += BackgroundWorkerLiabalityandScore_RunWorkerCompleted;

            backgroundWorkerProfitandlossbyAgent = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerProfitandlossbyAgent.DoWork += BackgroundWorkerProfitandlossbyAgent_DoWork;
            backgroundWorkerProfitandlossbyAgent.RunWorkerCompleted += BackgroundWorkerProfitandlossbyAgent_RunWorkerCompleted;



            //  timerCountdown.Tick += TimerCountdown_Tick;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                DGVMatchedBets.Visibility = Visibility.Collapsed;
                DGVMatchedBetsAdmin.Visibility = Visibility.Visible;
                DGVMatchedBetsaGENT.Visibility = Visibility.Collapsed;
                DGVMatchedBetsAll.Visibility = Visibility.Collapsed;
                DGVMatchedBetsAdminAll.Visibility = Visibility.Visible;
                DGVMatchedBetsaGENTAll.Visibility = Visibility.Collapsed;
                DGVUnMatched.Visibility = Visibility.Collapsed;
                DGVUnMatchedAdmin.Visibility = Visibility.Visible;
                DGVUnMatchedAgent.Visibility = Visibility.Collapsed;
                stkpnlAdminArea.Visibility = Visibility.Visible;
                btnCancelAllBets.Visibility = Visibility.Collapsed;
                btnCuttingBets.Visibility = Visibility.Visible;
                stkpnlAdminAreaAgent.Visibility = Visibility.Collapsed;
                lblFavoriteBack.Visibility = Visibility.Hidden;
                lblFavoriteLay.Visibility = Visibility.Hidden;
                lblFavoriteNAme.Visibility = Visibility.Hidden;
                DGVMarketKalijut.Columns[1].Visibility = Visibility.Visible;
                DGVMarketFigure.Columns[1].Visibility = Visibility.Visible;
                DGVMarketSFig.Columns[1].Visibility = Visibility.Visible;
                sktrace.Visibility = Visibility.Collapsed;
            }

            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    DGVMatchedBets.Visibility = Visibility.Collapsed;
                    DGVMatchedBetsAdmin.Visibility = Visibility.Collapsed;
                    DGVMatchedBetsaGENT.Visibility = Visibility.Visible;
                    DGVMatchedBetsAll.Visibility = Visibility.Collapsed;
                    DGVMatchedBetsAdminAll.Visibility = Visibility.Collapsed;
                    DGVMatchedBetsaGENTAll.Visibility = Visibility.Visible;
                    DGVUnMatched.Visibility = Visibility.Collapsed;
                    DGVUnMatchedAdmin.Visibility = Visibility.Collapsed;
                    DGVUnMatchedAgent.Visibility = Visibility.Visible;
                    stkpnlAdminArea.Visibility = Visibility.Collapsed;
                    btnCancelAllBets.Visibility = Visibility.Collapsed;
                    stkpnlAdminAreaAgent.Visibility = Visibility.Visible;
                    sktrace.Visibility = Visibility.Collapsed;
                }
                else
                if (LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
                {
                    DGVMatchedBets.Visibility = Visibility.Collapsed;
                    DGVMatchedBetsAdmin.Visibility = Visibility.Visible;
                    DGVMatchedBetsaGENT.Visibility = Visibility.Collapsed;
                    DGVMatchedBetsAll.Visibility = Visibility.Collapsed;
                    DGVMatchedBetsAdminAll.Visibility = Visibility.Visible;
                    DGVMatchedBetsaGENTAll.Visibility = Visibility.Collapsed;
                    DGVUnMatched.Visibility = Visibility.Collapsed;
                    DGVUnMatchedAdmin.Visibility = Visibility.Visible;
                    DGVUnMatchedAgent.Visibility = Visibility.Collapsed;
                    stkpnlAdminArea.Visibility = Visibility.Collapsed;
                    btnCancelAllBets.Visibility = Visibility.Collapsed;
                    btnCuttingBets.Visibility = Visibility.Visible;
                    stkpnlAdminAreaAgent.Visibility = Visibility.Collapsed;
                    lblFavoriteBack.Visibility = Visibility.Hidden;
                    lblFavoriteLay.Visibility = Visibility.Hidden;
                    lblFavoriteNAme.Visibility = Visibility.Hidden;
                    DGVUnMatchedAdmin.Columns[4].Visibility = Visibility.Collapsed;
                    sktrace.Visibility = Visibility.Collapsed;

                }

                else
                {
                    DGVMatchedBets.Visibility = Visibility.Visible;
                    DGVMatchedBetsAdmin.Visibility = Visibility.Collapsed;
                    DGVMatchedBetsaGENT.Visibility = Visibility.Collapsed;
                    DGVMatchedBetsAll.Visibility = Visibility.Visible;
                    DGVMatchedBetsAdminAll.Visibility = Visibility.Collapsed;
                    DGVMatchedBetsaGENTAll.Visibility = Visibility.Collapsed;
                    DGVUnMatched.Visibility = Visibility.Visible;
                    DGVUnMatchedAdmin.Visibility = Visibility.Collapsed;
                    DGVUnMatchedAgent.Visibility = Visibility.Collapsed;
                    stkpnlAdminArea.Visibility = Visibility.Collapsed;
                    stkpnlAdminAreaAgent.Visibility = Visibility.Collapsed;
                }

            }

            SetBetSlipKeys();
        }


        public void SetWindowHeight()
        {
            try
            {
                if (MarketBook.Runners.Count <= 3)
                {
                    double fancygridheight = 0;
                    if (DGVMarketIndianFancy.Visibility == Visibility.Visible && lstMarketBookRunnersFancyin != null)
                    {
                        if (lstMarketBookRunnersFancyin.Count > 0)
                        {
                            var isshownitems = lstMarketBookRunnersFancyin.Where(item => item.isShow == true).ToList();
                            if (isshownitems.Count > 0)
                            {
                                if (isshownitems.Count == 1)
                                {
                                    fancygridheight = 150;
                                }
                                else
                                {
                                    fancygridheight = (isshownitems.Count * 50) + 75;
                                }
                                // DGVMarketIndianFancy.Height = fancygridheight;

                                //fancygridheight = (isshownitems.Count * 50) + 45;
                                //if (fancygridheight > 300)
                                //{
                                //    fancygridheight = 300;
                                //}
                                //else
                                //{
                                //    // fancygridheight = DGVMarketFancy.ActualHeight;
                                //}
                            }
                            else
                            {
                                // DGVMarketIndianFancy.Height = 100;
                            }

                        }
                        else
                        {
                            // DGVMarketIndianFancy.Height = 100;
                        }

                    }
                    else
                    {
                        if (lstMarketBookRunnersKJ.Count > 0)
                        {
                            fancygridheight = (lstMarketBookRunnersKJ.Count * 50) + 50;
                        }
                    }
                    // double newheight = upperportion.ActualHeight + DGVMarket.ActualHeight + fancygridheight + 50;
                    if (stkpnlTowintheToss.Visibility == Visibility.Visible)
                    {
                        //  newheight += DGVMarketToWintheToss.ActualHeight + 35;
                    }
                    //if (this.Height != newheight)
                    //{
                    //  //  this.Height = newheight;

                    //}


                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {


            }
        }
        private void BackgroundWorkerUpdateAllotherData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                UpdateUserBetsData();
                UpdateUserBetsDataUnMatched();
            }
            catch (System.Exception ex)
            {

            }
            backgroundWorkerUpdateAllotherData.RunWorkerAsync();
        }

        private void BackgroundWorkerUpdateAllotherData_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(200);
        }

        private void BackgroundWorkerProfitandlossbyAgent_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (isProfitLossbyAgentShown == true)
                {
                    CalculateAvearageforSelectedAgent();
                }

                backgroundWorkerProfitandlossbyAgent.RunWorkerAsync();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        public int SelectedAgentForProfitandLoss = 73;

        private void BackgroundWorkerProfitandlossbyAgent_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;

                    return;
                }

                if (isProfitLossbyAgentShown == true)
                {
                    ProfitandLoss objProfitandlossAgent = new ProfitandLoss();
                    string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetUserbetsForAgent/?AgentID=" + SelectedAgentForProfitandLoss.ToString();

                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RestAPIPath1);
                    request.Method = "GET";
                    request.Proxy = null;
                    request.Timeout = 5000;
                    request.KeepAlive = false;
                    request.ServicePoint.ConnectionLeaseTimeout = 5000;
                    request.ServicePoint.MaxIdleTime = 5000;
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    String test = String.Empty;
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {

                            System.Runtime.Serialization.Json.DataContractJsonSerializer obj1 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(string));
                            test = obj1.ReadObject(dataStream).ToString();

                            dataStream.Close();
                        }

                    }
                    var lstUserBetAgent = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(test);

                    CurrentMarketProfitandLossForAgent = objProfitandlossAgent.CalculateProfitandLossAgent(MarketBookForProfitandlossAgent, lstUserBetAgent);

                }
                System.Threading.Thread.Sleep(300);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void ObjUsersServiceCleint_GetUserBetsbyAgentIDwithZeroRefererCompleted1(object sender, GetUserBetsbyAgentIDwithZeroRefererCompletedEventArgs e)
        {

            ProfitandLoss objProfitandlossAgent = new ProfitandLoss();
            var lstUserBetAgent = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(e.Result);
            CurrentMarketProfitandLossForAgent = objProfitandlossAgent.CalculateProfitandLossAgent(MarketBookForProfitandlossAgent, lstUserBetAgent);
            CalculateAvearageforSelectedAgent();
        }

        public MarketWindowUserControl()
        {

            InitializeComponent();

            setFirstTimeLoadEvents();
            //sizechanged();

        }
        public MatchScoreCard objMatchScoreCard = new MatchScoreCard();


        public dynamic match;

        public class LZWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                return request;
            }
        }
        Service1Client objCricketScoreClient = new Service1Client();


        private void ObjUsersServiceCleint_GetScoresbyEventIDandDateCompleted(object sender, GetScoresbyEventIDandDateCompletedEventArgs e)
        {

            scores = JsonConvert.DeserializeObject<List<MatchScores>>(e.Result);
        }





        private void BackgroundWorkerUpdateData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {



                // UpdateAllData();
                if (MarketBook.MainSportsname == "Cricket" && MarketBook.MarketBookName.Contains("Match Odds"))
                {

                    //UpdateLineMarketsData(false);
                    //GetINFancy(MarketBook.EventID, MarketBook.MarketId);

                    // CreateScoreCard();
                    // updatematchscorefromscorecard();
                }
                if (popupBetslip.IsOpen == true && runnerscount == "1")
                {
                    var currmarketbook = LastloadedLinMarkets.Where(item => item.MarketId == CurrentMarketBookId).FirstOrDefault();
                    currmarketbookforbet = currmarketbook;
                    lblBetSlipBack.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price.ToString();
                    //lblBetslipBackOdd1.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[1].Price.ToString();
                    // lblBetslipBackOdd2.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[2].Price.ToString();
                    lblBetslipBackSize0.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[0].Size.ToString();
                    // lblBetslipBackSize1.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[1].Size.ToString();
                    // lblBetslipBackSize2.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[2].Size.ToString();

                    lblBetSlipLay.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[0].Price.ToString();
                    // lblBetslipLayOdd1.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[1].Price.ToString();
                    // lblBetslipLayOdd2.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[2].Price.ToString();
                    lblBetslipLaySize0.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[0].Size.ToString();
                    // lblBetslipLaySize1.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[1].Size.ToString();
                    // lblBetslipLaySize2.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[2].Size.ToString();
                    if (BetType == "back")
                    {
                        Betslipsize.Text = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[0].Size.ToString();
                    }
                    else
                    {
                        Betslipsize.Text = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[0].Size.ToString();
                    }
                }
                if (isProfitLossbyAgentShown == true)
                {
                    CalculateAvearageforSelectedAgent();
                }
                backgroundWorkerUpdateData.RunWorkerAsync();

                //backgroundWorkertotalmatched.RunWorkerAsync();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }



        private void backgroundWorkertotalmatched_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;

                    return;
                }
                var currmarket = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == MarketBook.MarketId);
                if (currmarket.Count() == 0)
                {
                    Removeallbackgroundworkers();
                }
                System.Threading.Thread.Sleep(200);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

        }

        private void BackgroundWorkerUpdateData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;

                    return;
                }
                var currmarket = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == MarketBook.MarketId);
                if (currmarket.Count() == 0)
                {
                    Removeallbackgroundworkers();
                }
                System.Threading.Thread.Sleep(200);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

        }

        private void BackgroundWorkerFancyData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;

                    return;
                }
                var currmarket = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == MarketBook.MarketId);
                if (currmarket.Count() == 0)
                {
                    Removeallbackgroundworkers();
                }
                System.Threading.Thread.Sleep(3000);
            }
            catch (System.Exception ex)
            {

            }

        }



        private void BackgroundWorkerFancyData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                if (MarketBook.MainSportsname == "Cricket" && MarketBook.MarketBookName.Contains("Match Odds"))
                {
                    //GetINFancy(MarketBook.EventID, MarketBook.MarketId);
                }


                backgroundWorkerUpdateFancyData.RunWorkerAsync();

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void BackgroundWorkerUpdateFigData_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
                System.Threading.Thread.Sleep(4000);
            }
            catch (System.Exception ex)
            {

            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                //&& lblMarketStatus.Content.ToString() == "IN-PLAY")
                if (MarketBook.MainSportsname == "Cricket" && MarketBook.MarketBookName.Contains("Match Odds"))
                {
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }





        public void BetSlipButtonClickMultiple(object sender, RoutedEventArgs e)
        {
            var betlowerlimit = setBetslipamountlowerlimit() * 2;
            if (isFirstClickonslip == true)
            {
                isFirstClickonslip = false;
                nupdnUserAmountMultiple.Value = 0;
            }
            //if (betlowerlimit == nudownAmountMultiple.Value)
            //{
            //    nudownAmountMultiple.Value = 0;
            //}
            if (LoggedinUserDetail.isInserting == true)
            {
                return;
            }
            string amount = (string)((Button)sender).Tag;
            if (amount.Contains("+"))
            {
                nupdnUserAmountMultiple.Value = nupdnUserAmountMultiple.Value + Convert.ToInt32(amount);
            }
            else
            {
                nupdnUserAmountMultiple.Value = Convert.ToInt32(amount);
            }
            CalculateAmountsMultiple();
        }
        public void BetSlipButtonClickSimple(object sender, RoutedEventArgs e)
        {
            var betlowerlimit = setBetslipamountlowerlimit();
            if (isFirstClickonslip == true)
            {
                isFirstClickonslip = false;
                nupdnUserAmount.Value = 0;
                // nupdnUserAmountin.Value = 0;
            }

            if (LoggedinUserDetail.isInserting == true)
            {
                return;
            }
            string amount = (string)((Button)sender).Tag;
            if (amount.Contains("+"))
            {
                nupdnUserAmount.Value = nupdnUserAmount.Value + Convert.ToInt32(amount);
                // nupdnUserAmountin.Value = nupdnUserAmountin.Value + Convert.ToInt32(amount);
            }
            else
            {
                nupdnUserAmount.Value = Convert.ToInt32(amount);
                // nupdnUserAmountin.Value = Convert.ToInt32(amount);
            }
            calculateProfitandLossonBetSlip();
        }
        public decimal setBetslipamountlowerlimit()
        {
            // var betamounttobeplaced=parseFloat($("#betslipamountmultiple").val().trim());
            decimal lowerbetlimit = 2000;

            var categoryname = MarketBook.MainSportsname;
            var marketbookname = MarketBook.MarketBookName;
            if (categoryname.Contains("Horse Racing") && !marketbookname.Contains("To Be Placed"))
            {
                lowerbetlimit = LoggedinUserDetail.GetBetLowerLimit();

            }
            else
            {
                if (categoryname.Contains("Horse Racing") && marketbookname.Contains("To Be Placed"))
                {
                    lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitHorsePlace();

                }
                else
                {
                    if (categoryname.Contains("Greyhound Racing") && marketbookname.Contains("To Be Placed"))
                    {
                        lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitGrayHoundPlace();

                    }
                    else
                    {
                        if (categoryname.Contains("Greyhound Racing") && !marketbookname.Contains("To Be Placed"))
                        {
                            lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitGrayHoundWin();

                        }
                        else
                        {
                            if (marketbookname.Contains("Completed Match"))
                            {
                                lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitCompletedMatch();

                            }
                            else
                            {
                                if (marketbookname.Contains("Innings Runs") || marketbookname.Contains("Inns Runs"))
                                {
                                    lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitInningsRuns();

                                }
                                else
                                {
                                    if (categoryname == "Tennis")
                                    {
                                        lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitMatchOddsTennis();

                                    }
                                    else
                                    {
                                        if (categoryname == "Soccer")
                                        {
                                            lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitMatchOddsSoccer();

                                        }
                                        else
                                        {
                                            if (marketbookname.Contains("Tied Match"))
                                            {
                                                lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitTiedMatch();

                                            }
                                            else
                                            {
                                                if (marketbookname.Contains("Winner"))
                                                {
                                                    lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitWinner();

                                                }
                                                else
                                                {
                                                    lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitMatchOdds();

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
            return lowerbetlimit;
        }
        public void SetBetSlipKeys()
        {
            try
            {


                BetSlipKeys objBetSlipKeys = LoggedinUserDetail.objBetSlipKeys;
                btnSimple1.Content = objBetSlipKeys.SimpleBtn1;
                btnSimple1.Tag = objBetSlipKeys.SimpleBtn1;
                btnSimple2.Content = objBetSlipKeys.SimpleBtn2;
                btnSimple2.Tag = objBetSlipKeys.SimpleBtn2;
                btnSimple3.Content = objBetSlipKeys.SimpleBtn3;
                btnSimple3.Tag = objBetSlipKeys.SimpleBtn3;
                btnSimple4.Content = objBetSlipKeys.SimpleBtn4;
                btnSimple4.Tag = objBetSlipKeys.SimpleBtn4;
                btnSimple5.Content = objBetSlipKeys.SimpleBtn5;
                btnSimple5.Tag = objBetSlipKeys.SimpleBtn5;
                btnSimple6.Content = objBetSlipKeys.SimpleBtn6;
                btnSimple6.Tag = objBetSlipKeys.SimpleBtn6;
                btnSimple7.Content = objBetSlipKeys.SimpleBtn7;
                btnSimple7.Tag = objBetSlipKeys.SimpleBtn7;
                btnSimple8.Content = objBetSlipKeys.SimpleBtn8;
                btnSimple8.Tag = objBetSlipKeys.SimpleBtn8;
                btnSimple9.Content = objBetSlipKeys.SimpleBtn9;
                btnSimple9.Tag = objBetSlipKeys.SimpleBtn9;

                //slipforin
                btnSimple1in.Content = objBetSlipKeys.SimpleBtn1;
                btnSimple1in.Tag = objBetSlipKeys.SimpleBtn1;
                btnSimple2in.Content = objBetSlipKeys.SimpleBtn2;
                btnSimple2in.Tag = objBetSlipKeys.SimpleBtn2;
                btnSimple3in.Content = objBetSlipKeys.SimpleBtn3;
                btnSimple3in.Tag = objBetSlipKeys.SimpleBtn3;
                btnSimple4in.Content = objBetSlipKeys.SimpleBtn4;
                btnSimple4in.Tag = objBetSlipKeys.SimpleBtn4;
                btnSimple5in.Content = objBetSlipKeys.SimpleBtn5;
                btnSimple5in.Tag = objBetSlipKeys.SimpleBtn5;
                btnSimple6in.Content = objBetSlipKeys.SimpleBtn6;
                btnSimple6in.Tag = objBetSlipKeys.SimpleBtn6;
                btnSimple7in.Content = objBetSlipKeys.SimpleBtn7;
                btnSimple7in.Tag = objBetSlipKeys.SimpleBtn7;
                btnSimple8in.Content = objBetSlipKeys.SimpleBtn8;
                btnSimple8in.Tag = objBetSlipKeys.SimpleBtn8;
                btnSimple9in.Content = objBetSlipKeys.SimpleBtn9;
                btnSimple9in.Tag = objBetSlipKeys.SimpleBtn9;

                //slipforin1
                btnSimple1in1.Content = objBetSlipKeys.SimpleBtn1;
                btnSimple1in1.Tag = objBetSlipKeys.SimpleBtn1;
                btnSimple2in1.Content = objBetSlipKeys.SimpleBtn2;
                btnSimple2in1.Tag = objBetSlipKeys.SimpleBtn2;
                btnSimple3in1.Content = objBetSlipKeys.SimpleBtn3;
                btnSimple3in1.Tag = objBetSlipKeys.SimpleBtn3;
                btnSimple4in1.Content = objBetSlipKeys.SimpleBtn4;
                btnSimple4in1.Tag = objBetSlipKeys.SimpleBtn4;
                btnSimple5in1.Content = objBetSlipKeys.SimpleBtn5;
                btnSimple5in1.Tag = objBetSlipKeys.SimpleBtn5;
                btnSimple6in1.Content = objBetSlipKeys.SimpleBtn6;
                btnSimple6in1.Tag = objBetSlipKeys.SimpleBtn6;
                btnSimple7in1.Content = objBetSlipKeys.SimpleBtn7;
                btnSimple7in1.Tag = objBetSlipKeys.SimpleBtn7;
                btnSimple8in1.Content = objBetSlipKeys.SimpleBtn8;
                btnSimple8in1.Tag = objBetSlipKeys.SimpleBtn8;
                btnSimple9in1.Content = objBetSlipKeys.SimpleBtn9;
                btnSimple9in1.Tag = objBetSlipKeys.SimpleBtn9;

                ///Mutliple
                btnMultiple1.Content = objBetSlipKeys.MutipleBtn1;
                btnMultiple1.Tag = objBetSlipKeys.MutipleBtn1;
                btnMultiple2.Content = objBetSlipKeys.MutipleBtn2;
                btnMultiple2.Tag = objBetSlipKeys.MutipleBtn2;
                btnMultiple3.Content = objBetSlipKeys.MutipleBtn3;
                btnMultiple3.Tag = objBetSlipKeys.MutipleBtn3;
                btnMultiple4.Content = objBetSlipKeys.MutipleBtn4;
                btnMultiple4.Tag = objBetSlipKeys.MutipleBtn4;
                btnMultiple5.Content = objBetSlipKeys.MutipleBtn5;
                btnMultiple5.Tag = objBetSlipKeys.MutipleBtn5;
                btnMultiple6.Content = objBetSlipKeys.MutipleBtn6;
                btnMultiple6.Tag = objBetSlipKeys.MutipleBtn6;
                btnMultiple7.Content = objBetSlipKeys.MutipleBtn7;
                btnMultiple7.Tag = objBetSlipKeys.MutipleBtn7;
                btnMultiple8.Content = objBetSlipKeys.MutipleBtn8;
                btnMultiple8.Tag = objBetSlipKeys.MutipleBtn8;
                btnMultiple9.Content = objBetSlipKeys.MutipleBtn9;
                btnMultiple9.Tag = objBetSlipKeys.MutipleBtn9;

                //TextBoxes
                txtSimpleBtn1.Text = objBetSlipKeys.SimpleBtn1;

                txtSimpleBtn2.Text = objBetSlipKeys.SimpleBtn2;

                txtSimpleBtn3.Text = objBetSlipKeys.SimpleBtn3;

                txtSimpleBtn4.Text = objBetSlipKeys.SimpleBtn4;

                txtSimpleBtn5.Text = objBetSlipKeys.SimpleBtn5;

                txtSimpleBtn6.Text = objBetSlipKeys.SimpleBtn6;

                txtSimpleBtn7.Text = objBetSlipKeys.SimpleBtn7;

                txtSimpleBtn8.Text = objBetSlipKeys.SimpleBtn8;

                txtSimpleBtn9.Text = objBetSlipKeys.SimpleBtn9;



                ///Mutliple
                txtMultpleBtn1.Text = objBetSlipKeys.MutipleBtn1;

                txtMultpleBtn2.Text = objBetSlipKeys.MutipleBtn2;

                txtMultpleBtn3.Text = objBetSlipKeys.MutipleBtn3;

                txtMultpleBtn4.Text = objBetSlipKeys.MutipleBtn4;

                txtMultpleBtn5.Text = objBetSlipKeys.MutipleBtn5;

                txtMultpleBtn6.Text = objBetSlipKeys.MutipleBtn6;

                txtMultpleBtn7.Text = objBetSlipKeys.MutipleBtn7;

                txtMultpleBtn8.Text = objBetSlipKeys.MutipleBtn8;

                txtMultpleBtn9.Text = objBetSlipKeys.MutipleBtn9;


                txtDefaultStakeBack.Text = Properties.Settings.Default.DefaultStakeBack.ToString();
                txtDefaultStakeBackMultiple.Text = Properties.Settings.Default.DefaultStakeBackMultiple.ToString();
                txtDefaultStakeLay.Text = Properties.Settings.Default.DefaultStakeLay.ToString();
                txtDefaultStakeLayMultiple.Text = Properties.Settings.Default.DefaultStakeLayMultiple.ToString();

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

        }
        public double CurrentLiabality = 0;


        private void BackgroundWorkerUpdateFigData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (MarketBook.MainSportsname == "Cricket" && MarketBook.MarketBookName.Contains("Match Odds"))
                {

                    if (MarketBook.LineVMarkets.Where(item => item.isOpenedbyUser == true && item.EventName == "Kali v Jut").ToList().Count > 0)
                    {

                        AssignKalijutdata();
                        DGVMarketKalijut.ItemsSource = lstMarketBookRunnerKalijut;
                        if (lstMarketBookRunnerKalijut.Count > 0)
                        {
                            DGVMarketKalijut.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        DGVMarketKalijut.Visibility = Visibility.Collapsed;
                    }
                    //if (MarketBook.LineVMarkets.Where(item => item.isOpenedbyUser == true && item.EventName == "SmallFig").ToList().Count > 0)
                    //{
                    //    AssignSFigdata();

                    //    DGVMarketSFig.ItemsSource = lstMarketBookRunnerSFigure;
                    //    if (lstMarketBookRunnerSFigure.Count > 0)
                    //    {
                    //        DGVMarketSFig.Visibility = Visibility.Visible;
                    //    }
                    //}
                    //else
                    //{
                    //    DGVMarketSFig.Visibility = Visibility.Collapsed;
                    //}

                    if (MarketBook.LineVMarkets.GroupBy(item => item.MarketCatalogueName).Select(g => g.First()).Where(item2 => item2.EventName == "Figure").ToList().Count > 0)
                    {
                        AssignFiguredata();
                        DGVMarketFigure.ItemsSource = lstMarketBookRunnersFigure;
                        if (lstMarketBookRunnersFigure.Count > 0)
                        {
                            DGVMarketFigure.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        DGVMarketFigure.Visibility = Visibility.Collapsed;
                    }


                }
                backgroundWorkerUpdateFigData.RunWorkerAsync();

            }
            catch (System.Exception ex)
            {
                backgroundWorkerUpdateFigData.RunWorkerAsync();
            }

        }
        private void BackgroundWorkerLiabalityandScore_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (MarketBook.MainSportsname == "Cricket" && MarketBook.MarketBookName.Contains("Match Odds"))
                {
                    UpdateLineMarketsData(false);

                }
                backgroundWorkerLiabalityandScore.RunWorkerAsync();

            }
            catch (System.Exception ex)
            {
                backgroundWorkerLiabalityandScore.RunWorkerAsync();
            }

        }
        public List<MatchScores> scores = new List<MatchScores>();
        public List<ExternalAPI.TO.MarketBook> CurrentMarketProfitandLoss = new List<ExternalAPI.TO.MarketBook>();
        public List<ExternalAPI.TO.MarketBook> CurrentMarketProfitandLossToWinTheToss = new List<ExternalAPI.TO.MarketBook>();
        public List<ExternalAPI.TO.MarketBook> CurrentMarketProfitandLossToFigre = new List<ExternalAPI.TO.MarketBook>();
        public List<ExternalAPI.TO.MarketBook> CurrentMarketProfitandLossForAgent = new List<ExternalAPI.TO.MarketBook>();



        public ExternalAPI.TO.MarketBookForindianFancy GetBookPositionin(string marketBookID, string selectionID)
        {

            ExternalAPI.TO.MarketBookForindianFancy objmarketbookin = new ExternalAPI.TO.MarketBookForindianFancy();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                List<bftradeline.Models.UserBetsForAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.SelectionID == selectionID && item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbookin.RunnersForindianFancy != null)
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }
                    }
                    ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        decimal samiadminrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].samiadminrate);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = 0;
                        if (superrate > 0)
                        {
                            superpercent = superrate - agentrate;
                        }
                        else
                        {
                            superpercent = 0;
                        }
                        decimal samiadminpercent = 0;
                        if (samiadminrate > 0)
                        {
                            samiadminpercent = samiadminrate-(superpercent + agentrate);
                        }
                        else
                        {
                            samiadminpercent = 0;
                        }
                        agentrate = agentrate + superpercent+ samiadminpercent;

                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));

                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        decimal a = (Convert.ToDecimal(userbet.BetSize) / 100);
                                        objDebitCredit.Credit = totamount * (Convert.ToDecimal(userbet.BetSize) / 100);
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }

                            }
                            else
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {

                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                    }

                }
            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    //20_over_runs_slkslk_vs_sknpadv   20_over_runs_slkslk_vs_sknpadv//@if (LoggedinUserDetail.GetUserTypeID() == 1)
                    List<bftradeline.Models.UserBetsforAgent> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAgentBets.ToList().Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID && item.isMatched == true).ToList();
                    if (lstCurrentBetsAdmin.Count > 0)
                    {
                        lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                        objmarketbookin.MarketId = marketBookID;
                        objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                        ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                        objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                        objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                        objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                        foreach (var userbet in lstCurrentBetsAdmin)
                        {
                            if (objmarketbookin.RunnersForindianFancy != null)
                            {
                                ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                if (objexistingrunner == null)
                                {
                                    ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                    objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                }
                            }
                            else
                            {
                                ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }



                        }
                        ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                        objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                        objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                        objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                        ///calculation
                        var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                        foreach (var userid in lstUsers)
                        {
                            var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();

                            foreach (var userbet in lstCurrentBetsbyUser)
                            {
                                decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
                                double num = Convert.ToDouble(userbet.BetSize) / 100;

                                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                if (userbet.BetType == "back")
                                {
                                    double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num); ;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = totamount;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }

                                }
                                else
                                {
                                    double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = totamount;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                }
                                //userbet.lstDebitCredit = new List<DebitCredit>();
                                //userbet.lstDebitCredit = lstDebitCredit;
                            }
                        }

                        objmarketbookin.DebitCredit = lstDebitCredit;
                        foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                        {
                            runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                            runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                        }
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {

                        List<bftradeline.Models.UserBets> lstCurrentBets = LoggedinUserDetail.CurrentUserBets.Where(item => item.SelectionID == selectionID && item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                        if (lstCurrentBets.Count > 0)
                        {
                            lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                            objmarketbookin.MarketId = marketBookID;
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBets[0].UserOdd) - 1).ToString();
                            objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
                            objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                            foreach (var userbet in lstCurrentBets)
                            {
                                if (objmarketbookin.RunnersForindianFancy != null)
                                {
                                    ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                    if (objexistingrunner == null)
                                    {
                                        ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                        objRunner.SelectionId = userbet.UserOdd;
                                        objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                        objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                    }
                                }
                                else
                                {
                                    ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                    objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                }



                            }
                            ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBets.Last().UserOdd) + 1).ToString();
                            objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                            objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                            ///calculation
                            foreach (var userbet in lstCurrentBets)
                            {
                                decimal num = Convert.ToDecimal(userbet.BetSize) / 100;
                                var totamount = (Convert.ToDecimal(userbet.Amount) * num);
                                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                if (userbet.BetType == "back")
                                {
                                    double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;

                                    objDebitCredit.Debit = totamount;//Convert.ToDecimal(userbet.Amount);
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = totamount;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
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
                                    double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = totamount;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            //decimal num = Convert.ToDecimal(userbet.BetSize) / 100;
                                            objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }


                                }

                                //userbet.lstDebitCredit = new List<DebitCredit>();
                                //userbet.lstDebitCredit = lstDebitCredit;

                            }
                            objmarketbookin.DebitCredit = lstDebitCredit;
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {

                                runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                            }
                        }
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                        {

                            List<bftradeline.Models.UserBetsforSuper> lstCurrentBetsSuper = LoggedinUserDetail.CurrentSuperBets.ToList().Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID && item.isMatched == true).ToList();
                            if (lstCurrentBetsSuper.Count > 0)
                            {
                                lstCurrentBetsSuper = lstCurrentBetsSuper.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                                objmarketbookin.MarketId = marketBookID;
                                objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper[0].UserOdd) - 1).ToString();
                                objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper[0].UserOdd) - 1);
                                objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                                foreach (var userbet in lstCurrentBetsSuper)
                                {
                                    if (objmarketbookin.RunnersForindianFancy != null)
                                    {
                                        ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                        if (objexistingrunner == null)
                                        {
                                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                            objRunner.SelectionId = userbet.UserOdd;
                                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                        }
                                    }
                                    else
                                    {
                                        ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                        objRunner.SelectionId = userbet.UserOdd;
                                        objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                        objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                        objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                    }
                                }
                                ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper.Last().UserOdd) + 1).ToString();
                                objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper.Last().UserOdd) + 1);
                                objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

                                ///calculation
                                var lstUsers = lstCurrentBetsSuper.Select(item => new { item.UserID }).Distinct().ToArray();
                                foreach (var userid in lstUsers)
                                {
                                    var lstCurrentBetsbyUser = lstCurrentBetsSuper.Where(item => item.UserID.Value == userid.UserID).ToList();
                                    decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                                    decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                                    bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                                    var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                                    decimal superpercent = superrate - agentrate;

                                    foreach (var userbet in lstCurrentBetsbyUser)
                                    {
                                        double num = Convert.ToDouble(userbet.BetSize) / 100;
                                        var totamount1 = (Convert.ToDecimal(userbet.Amount) * (Convert.ToDecimal(num)));
                                        var totamount = (superpercent / 100) * (totamount1);
                                        var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        if (userbet.BetType == "back")
                                        {
                                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                            objDebitCredit.SelectionID = userbet.UserOdd;
                                            objDebitCredit.Debit = totamount;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                            {
                                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                                {
                                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                                    objDebitCredit.Debit = totamount;
                                                    objDebitCredit.Credit = 0;
                                                    lstDebitCredit.Add(objDebitCredit);
                                                }
                                            }
                                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                            {
                                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                                {
                                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                                    objDebitCredit.Debit = 0;
                                                    objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
                                                    lstDebitCredit.Add(objDebitCredit);
                                                }
                                            }

                                        }
                                        else
                                        {
                                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                            objDebitCredit.SelectionID = userbet.UserOdd;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = totamount;
                                            lstDebitCredit.Add(objDebitCredit);
                                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                            {
                                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                                {
                                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                                    objDebitCredit.Debit = 0;
                                                    objDebitCredit.Credit = totamount;
                                                    lstDebitCredit.Add(objDebitCredit);
                                                }
                                            }
                                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                            {
                                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                                {
                                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                                    objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
                                                    objDebitCredit.Credit = 0;
                                                    lstDebitCredit.Add(objDebitCredit);
                                                }
                                            }


                                        }

                                        //userbet.lstDebitCredit = new List<DebitCredit>();
                                        //userbet.lstDebitCredit = lstDebitCredit;

                                    }
                                }

                                objmarketbookin.DebitCredit = lstDebitCredit;
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                                    runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;


                                }

                            }
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 9)
                            {

                                List<bftradeline.Models.UserBetsforSamiAdmin> lstCurrentBetsSuper = LoggedinUserDetail.CurrentsamiadminBets.ToList().Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID && item.isMatched == true).ToList();
                                if (lstCurrentBetsSuper.Count > 0)
                                {
                                    lstCurrentBetsSuper = lstCurrentBetsSuper.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                                    objmarketbookin.MarketId = marketBookID;
                                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper[0].UserOdd) - 1).ToString();
                                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper[0].UserOdd) - 1);
                                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                                    foreach (var userbet in lstCurrentBetsSuper)
                                    {
                                        if (objmarketbookin.RunnersForindianFancy != null)
                                        {
                                            ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                            if (objexistingrunner == null)
                                            {
                                                ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                                objRunner.SelectionId = userbet.UserOdd;
                                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                            }
                                        }
                                        else
                                        {
                                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                            objRunner.SelectionId = userbet.UserOdd;
                                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                        }
                                    }
                                    ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper.Last().UserOdd) + 1).ToString();
                                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper.Last().UserOdd) + 1);
                                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

                                    ///calculation
                                    var lstUsers = lstCurrentBetsSuper.Select(item => new { item.UserID }).Distinct().ToArray();
                                    foreach (var userid in lstUsers)
                                    {
                                        var lstCurrentBetsbyUser = lstCurrentBetsSuper.Where(item => item.UserID.Value == userid.UserID).ToList();
                                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                                        decimal samiadminrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SamiAdminRate);
                                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                                        decimal superpercent = superrate - agentrate;
                                        decimal samiadminpercent = samiadminrate - (superpercent + agentrate);


                                        foreach (var userbet in lstCurrentBetsbyUser)
                                        {
                                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                                            var totamount1 = (Convert.ToDecimal(userbet.Amount) * (Convert.ToDecimal(num)));
                                            var totamount = (samiadminpercent / 100) * (totamount1);
                                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            if (userbet.BetType == "back")
                                            {
                                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                                objDebitCredit.SelectionID = userbet.UserOdd;
                                                objDebitCredit.Debit = totamount;
                                                objDebitCredit.Credit = 0;
                                                lstDebitCredit.Add(objDebitCredit);
                                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                                {
                                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                                    {
                                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                                        objDebitCredit.Debit = totamount;
                                                        objDebitCredit.Credit = 0;
                                                        lstDebitCredit.Add(objDebitCredit);
                                                    }
                                                }
                                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                                {
                                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                                    {
                                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                                        objDebitCredit.Debit = 0;
                                                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
                                                        lstDebitCredit.Add(objDebitCredit);
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                                objDebitCredit.SelectionID = userbet.UserOdd;
                                                objDebitCredit.Debit = 0;
                                                objDebitCredit.Credit = totamount;
                                                lstDebitCredit.Add(objDebitCredit);
                                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                                {
                                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                                    {
                                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                                        objDebitCredit.Debit = 0;
                                                        objDebitCredit.Credit = totamount;
                                                        lstDebitCredit.Add(objDebitCredit);
                                                    }
                                                }
                                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                                {
                                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                                    {
                                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
                                                        objDebitCredit.Credit = 0;
                                                        lstDebitCredit.Add(objDebitCredit);
                                                    }
                                                }


                                            }

                                            //userbet.lstDebitCredit = new List<DebitCredit>();
                                            //userbet.lstDebitCredit = lstDebitCredit;

                                        }
                                    }

                                    objmarketbookin.DebitCredit = lstDebitCredit;
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;


                                    }

                                }
                            }
                        }
                    }
                }


            }
            return objmarketbookin;
        }

        public ExternalAPI.TO.MarketBookForindianFancy GetBookPositioninNew(string selectionID)
        {

            ExternalAPI.TO.MarketBookForindianFancy objmarketbookin = new ExternalAPI.TO.MarketBookForindianFancy();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                List<bftradeline.Models.UserBetsForAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.SelectionID == selectionID && item.isMatched == true).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbookin.MarketId = selectionID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbookin.RunnersForindianFancy != null)
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }
                    }
                    ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        decimal samiadminrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].samiadminrate);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = 0;
                        if (superrate > 0)
                        {
                            superpercent = superrate - agentrate;
                        }
                        else
                        {
                            superpercent = 0;
                        }
                        decimal samiadminpercent = 0;
                        if (samiadminrate > 0)
                        {
                            samiadminpercent = samiadminrate - (superpercent + agentrate);
                        }
                        else
                        {
                            samiadminpercent = 0;
                        }
                        agentrate = agentrate + superpercent + samiadminpercent;

                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));

                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount * (Convert.ToDecimal(userbet.BetSize) / 100); ;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount * (Convert.ToDecimal(userbet.BetSize) / 100); ;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        decimal a = (Convert.ToDecimal(userbet.BetSize) / 100);
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }

                            }
                            else
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount * (Convert.ToDecimal(userbet.BetSize) / 100); ;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount * (Convert.ToDecimal(userbet.BetSize) / 100); ;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {

                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                    }

                }
            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    //20_over_runs_slkslk_vs_sknpadv   20_over_runs_slkslk_vs_sknpadv//@if (LoggedinUserDetail.GetUserTypeID() == 1)
                    List<bftradeline.Models.UserBetsforAgent> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAgentBets.ToList().Where(item => item.SelectionID == selectionID && item.isMatched == true).ToList();
                    if (lstCurrentBetsAdmin.Count > 0)
                    {
                        lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                        objmarketbookin.MarketId = selectionID;
                        objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                        ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                        objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                        objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                        objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                        foreach (var userbet in lstCurrentBetsAdmin)
                        {
                            if (objmarketbookin.RunnersForindianFancy != null)
                            {
                                ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                if (objexistingrunner == null)
                                {
                                    ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                    objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                }
                            }
                            else
                            {
                                ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }



                        }
                        ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                        objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                        objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                        objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                        ///calculation
                        var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                        foreach (var userid in lstUsers)
                        {
                            var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();

                            foreach (var userbet in lstCurrentBetsbyUser)
                            {
                                decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
                                double num = Convert.ToDouble(userbet.BetSize) / 100;

                                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                if (userbet.BetType == "back")
                                {
                                    double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num); ;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = totamount;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }

                                }
                                else
                                {
                                    double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = totamount;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                }
                                //userbet.lstDebitCredit = new List<DebitCredit>();
                                //userbet.lstDebitCredit = lstDebitCredit;
                            }
                        }

                        objmarketbookin.DebitCredit = lstDebitCredit;
                        foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                        {
                            runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                            runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                        }
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {

                        List<bftradeline.Models.UserBets> lstCurrentBets = LoggedinUserDetail.CurrentUserBets.Where(item => item.SelectionID == selectionID && item.isMatched == true).ToList();
                        if (lstCurrentBets.Count > 0)
                        {
                            lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                            objmarketbookin.MarketId = selectionID;
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBets[0].UserOdd) - 1).ToString();
                            objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
                            objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                            foreach (var userbet in lstCurrentBets)
                            {
                                if (objmarketbookin.RunnersForindianFancy != null)
                                {
                                    ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                    if (objexistingrunner == null)
                                    {
                                        ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                        objRunner.SelectionId = userbet.UserOdd;
                                        objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                        objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                    }
                                }
                                else
                                {
                                    ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                    objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                }



                            }
                            ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBets.Last().UserOdd) + 1).ToString();
                            objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                            objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                            ///calculation
                            foreach (var userbet in lstCurrentBets)
                            {
                                decimal num = Convert.ToDecimal(userbet.BetSize) / 100;
                                var totamount = (Convert.ToDecimal(userbet.Amount) * num);
                                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                if (userbet.BetType == "back")
                                {
                                    double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;

                                    objDebitCredit.Debit = totamount;//Convert.ToDecimal(userbet.Amount);
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = totamount;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
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
                                    double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = totamount;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            //decimal num = Convert.ToDecimal(userbet.BetSize) / 100;
                                            objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }


                                }

                                //userbet.lstDebitCredit = new List<DebitCredit>();
                                //userbet.lstDebitCredit = lstDebitCredit;

                            }
                            objmarketbookin.DebitCredit = lstDebitCredit;
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {

                                runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                            }
                        }
                    }

                }


            }
            return objmarketbookin;
        }


        public ExternalAPI.TO.MarketBookForindianFancy GetBookPositioninKJ(string marketBookID, string selectionID)
        {
            ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
            ExternalAPI.TO.MarketBookForindianFancy objmarketbookin = new ExternalAPI.TO.MarketBookForindianFancy();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                int a, b;
                List<bftradeline.Models.UserBetsForAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID && item.isMatched == true).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
                    double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
                    a = Convert.ToInt32(aa);
                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbookin.RunnersForindianFancy != null)
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }
                    }
                    ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunnerlast.SelectionId = (a + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        decimal samiadminrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].samiadminrate);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = 0;
                        if (superrate > 0)
                        {
                            superpercent = superrate - agentrate;
                        }
                        else
                        {
                            superpercent = 0;
                        }
                        decimal samiadminpercent = 0;
                        if (samiadminrate > 0)
                        {
                            samiadminpercent = samiadminrate - (superpercent + agentrate);
                        }
                        else
                        {
                            samiadminpercent = 0;
                        }
                        agentrate = agentrate + superpercent+ samiadminpercent;
                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            var totamount1 = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                            var totamount = Convert.ToDecimal(totamount1) * Convert.ToDecimal(userbet.BetSize) / 100;
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount1;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }
                            else
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount1;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }
                        }
                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                    }
                }
            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    int a, b;
                    List<bftradeline.Models.UserBetsforAgent> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAgentBets.Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID && item.isMatched == true).ToList();
                    if (lstCurrentBetsAdmin.Count > 0)
                    {
                        lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
                        double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
                        a = Convert.ToInt32(aa);
                        objmarketbookin.MarketId = marketBookID;
                        objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                        ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                        objRunner1.SelectionId = (a - 1).ToString();
                        // objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
                        objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                        objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                        foreach (var userbet in lstCurrentBetsAdmin)
                        {
                            if (objmarketbookin.RunnersForindianFancy != null)
                            {
                                ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                if (objexistingrunner == null)
                                {
                                    ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                    // objRunner.StallDraw = userbet.SelectionID;
                                    objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                }
                            }
                            else
                            {
                                ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                //objRunner.StallDraw = userbet.SelectionID;
                                objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }

                        ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                        double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
                        b = Convert.ToInt32(bb);
                        objRunnerlast.SelectionId = ((b) + 1).ToString();
                        objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                        // objRunnerlast.StallDraw = lstCurrentBetsAdmin.Last().SelectionID;
                        objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                        ///calculation
                        foreach (var userbet in lstCurrentBetsAdmin)
                        {
                            decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
                            // var totamount = (Convert.ToDecimal(userbet.Amount));
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        double num = Convert.ToDouble(userbet.BetSize) / 100;
                                        objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                        // objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        double num = Convert.ToDouble(userbet.BetSize) / 100;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        //objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }

                            }
                            else
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        double num = Convert.ToDouble(userbet.BetSize) / 100;
                                        objDebitCredit.Debit = 0;
                                        // objDebitCredit.Credit = totamount;
                                        objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        double num = Convert.ToDouble(userbet.BetSize) / 100;
                                        objDebitCredit.Debit = totamount;
                                        // objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }
                        }

                        objmarketbookin.DebitCredit = lstDebitCredit;
                        foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                        {
                            runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                            runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                        }
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {
                        int a, b;

                        List<bftradeline.Models.UserBets> lstCurrentBets = LoggedinUserDetail.CurrentUserBets.Where(item => item.SelectionID == selectionID && item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                        if (lstCurrentBets.Count > 0)
                        {
                            lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
                            double aa = Convert.ToDouble(lstCurrentBets[0].UserOdd);

                            a = Convert.ToInt32(aa);
                            objmarketbookin.MarketId = marketBookID;
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner1.SelectionId = (a - 1).ToString();
                            objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
                            objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
                            objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                            foreach (var userbet in lstCurrentBets)
                            {
                                if (objmarketbookin.RunnersForindianFancy != null)
                                {
                                    ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                    if (objexistingrunner == null)
                                    {
                                        ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                        objRunner.SelectionId = userbet.UserOdd;
                                        objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                        objRunner.StallDraw = userbet.SelectionID;
                                        objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                    }
                                }
                                else
                                {
                                    ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                    objRunner.StallDraw = userbet.SelectionID;
                                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                    objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                }

                            }
                            ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                            double bb = Convert.ToDouble(lstCurrentBets.Last().UserOdd);
                            b = Convert.ToInt32(bb);
                            objRunnerlast.SelectionId = ((b) + 1).ToString();
                            objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                            objRunnerlast.StallDraw = lstCurrentBets.Last().SelectionID;
                            objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                            ///calculation
                            foreach (var userbet in lstCurrentBets)
                            {

                                var totamount = (Convert.ToDecimal(userbet.Amount));
                                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                if (userbet.BetType == "back")
                                {
                                    double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount;

                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                                            objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                            // objDebitCredit.Debit = totamount;
                                            objDebitCredit.Credit = 0;

                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                            //objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }

                                }
                                else
                                {
                                    double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = totamount;
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                                            objDebitCredit.Debit = 0;
                                            // objDebitCredit.Credit = totamount;
                                            objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                                            objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                            // objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                }
                            }
                            objmarketbookin.DebitCredit = lstDebitCredit;
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {

                                runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                            }
                        }
                    }
                    else
                    {

                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                        {
                            int a, b;
                            List<bftradeline.Models.UserBetsforSuper> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentSuperBets.ToList().Where(item => item.SelectionID == selectionID && item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                            if (lstCurrentBetsAdmin.Count > 0)
                            {
                                lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();


                                double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
                                a = Convert.ToInt32(aa);
                                objmarketbookin.MarketId = marketBookID;
                                objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunner1.SelectionId = (a - 1).ToString();
                                objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                                objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                                foreach (var userbet in lstCurrentBetsAdmin)
                                {
                                    if (objmarketbookin.RunnersForindianFancy != null)
                                    {
                                        ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                        if (objexistingrunner == null)
                                        {
                                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                            objRunner.SelectionId = userbet.UserOdd;
                                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                        }
                                    }
                                    else
                                    {
                                        ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                        objRunner.SelectionId = userbet.UserOdd;
                                        objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                        objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                        objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                    }

                                }

                                ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                                double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
                                b = Convert.ToInt32(bb);
                                objRunnerlast.SelectionId = ((b) + 1).ToString();
                                objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                                // objRunnerlast.StallDraw = lstCurrentBetsAdmin.Last().SelectionID;
                                objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                                ///calculation
                                var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                                foreach (var userid in lstUsers)
                                {
                                    var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                                    decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                                    decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                                    bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                                    var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                                    decimal superpercent = superrate - agentrate;

                                    foreach (var userbet in lstCurrentBetsbyUser)
                                    {
                                        var totamount = (superpercent / 100) * ((Convert.ToDecimal(userbet.Amount)) * (Convert.ToDecimal(userbet.BetSize) / 100)); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                                        var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        if (userbet.BetType == "back")
                                        {
                                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                            objDebitCredit.SelectionID = userbet.UserOdd;
                                            objDebitCredit.Debit = totamount;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                            {
                                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                                {
                                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                                    objDebitCredit.Debit = totamount;
                                                    objDebitCredit.Credit = 0;
                                                    lstDebitCredit.Add(objDebitCredit);
                                                }
                                            }
                                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                            {
                                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                                {
                                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                                    objDebitCredit.Debit = 0;
                                                    objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
                                                    lstDebitCredit.Add(objDebitCredit);
                                                }
                                            }

                                        }
                                        else
                                        {
                                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                            objDebitCredit.SelectionID = userbet.UserOdd;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = totamount;
                                            lstDebitCredit.Add(objDebitCredit);
                                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                            {
                                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                                {
                                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                                    objDebitCredit.Debit = 0;
                                                    objDebitCredit.Credit = totamount;
                                                    lstDebitCredit.Add(objDebitCredit);
                                                }
                                            }
                                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                            {
                                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                                {
                                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                                    objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
                                                    objDebitCredit.Credit = 0;
                                                    lstDebitCredit.Add(objDebitCredit);
                                                }
                                            }


                                        }

                                        //userbet.lstDebitCredit = new List<DebitCredit>();
                                        //userbet.lstDebitCredit = lstDebitCredit;

                                    }
                                }

                                objmarketbookin.DebitCredit = lstDebitCredit;
                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                {

                                    runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                                    runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                                }
                            }
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 9)
                            {
                                int a, b;
                                List<bftradeline.Models.UserBetsforSamiAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentsamiadminBets.ToList().Where(item => item.SelectionID == selectionID && item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                                if (lstCurrentBetsAdmin.Count > 0)
                                {
                                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();


                                    double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
                                    a = Convert.ToInt32(aa);
                                    objmarketbookin.MarketId = marketBookID;
                                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                                    objRunner1.SelectionId = (a - 1).ToString();
                                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                                    foreach (var userbet in lstCurrentBetsAdmin)
                                    {
                                        if (objmarketbookin.RunnersForindianFancy != null)
                                        {
                                            ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                            if (objexistingrunner == null)
                                            {
                                                ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                                objRunner.SelectionId = userbet.UserOdd;
                                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                            }
                                        }
                                        else
                                        {
                                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                            objRunner.SelectionId = userbet.UserOdd;
                                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                        }

                                    }

                                    ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                                    double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
                                    b = Convert.ToInt32(bb);
                                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                                    // objRunnerlast.StallDraw = lstCurrentBetsAdmin.Last().SelectionID;
                                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                                    ///calculation
                                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                                    foreach (var userid in lstUsers)
                                    {
                                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                                        decimal samiadiminrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SamiAdminRate);
                                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                                        decimal superpercent = superrate - agentrate;
                                        decimal samiadminpercent = samiadiminrate -(superpercent + agentrate);

                                        foreach (var userbet in lstCurrentBetsbyUser)
                                        {
                                            var totamount = (samiadminpercent / 100) * ((Convert.ToDecimal(userbet.Amount)) * (Convert.ToDecimal(userbet.BetSize) / 100)); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            if (userbet.BetType == "back")
                                            {
                                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                                objDebitCredit.SelectionID = userbet.UserOdd;
                                                objDebitCredit.Debit = totamount;
                                                objDebitCredit.Credit = 0;
                                                lstDebitCredit.Add(objDebitCredit);
                                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                                {
                                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                                    {
                                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                                        objDebitCredit.Debit = totamount;
                                                        objDebitCredit.Credit = 0;
                                                        lstDebitCredit.Add(objDebitCredit);
                                                    }
                                                }
                                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                                {
                                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                                    {
                                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                                        objDebitCredit.Debit = 0;
                                                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
                                                        lstDebitCredit.Add(objDebitCredit);
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                                objDebitCredit.SelectionID = userbet.UserOdd;
                                                objDebitCredit.Debit = 0;
                                                objDebitCredit.Credit = totamount;
                                                lstDebitCredit.Add(objDebitCredit);
                                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                                {
                                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                                    {
                                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                                        objDebitCredit.Debit = 0;
                                                        objDebitCredit.Credit = totamount;
                                                        lstDebitCredit.Add(objDebitCredit);
                                                    }
                                                }
                                                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                                {
                                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                                    {
                                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
                                                        objDebitCredit.Credit = 0;
                                                        lstDebitCredit.Add(objDebitCredit);
                                                    }
                                                }


                                            }

                                            //userbet.lstDebitCredit = new List<DebitCredit>();
                                            //userbet.lstDebitCredit = lstDebitCredit;

                                        }
                                    }

                                    objmarketbookin.DebitCredit = lstDebitCredit;
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {

                                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                                    }
                                }
                            }
                        }
                    }


                }
            }
            return objmarketbookin;
        }


        

        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
      
        public void UpdateUserLiablity()
        {
            try
            {

                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    List<UserBets> lstCurrentmarketbets = LoggedinUserDetail.CurrentUserBets.Where(item => item.MarketBookID == MarketBook.MarketId).ToList();
                    CurrentLiabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), CurrentMarketProfitandLoss[0], LoggedinUserDetail.CurrentUserBets);
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                        List<UserBetsforAgent> lstCurrentmarketbets = LoggedinUserDetail.CurrentAgentBets.Where(item => item.MarketBookID == MarketBook.MarketId).ToList();
                        CurrentLiabality = objUserBets.GetLiabalityofCurrentAgent(lstCurrentmarketbets, CurrentMarketProfitandLoss[0]);
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            List<UserBetsForAdmin> lstCurrentmarketbets = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.MarketBookID == MarketBook.MarketId).ToList();
                            CurrentLiabality = objUserBets.GetLiabalityofAdmin(lstCurrentmarketbets, CurrentMarketProfitandLoss[0]);
                            if (MarketBook.LineVMarkets != null)
                            {

                                MarketBook currentmarketsfancyPL = new ExternalAPI.TO.MarketBook();
                                foreach (var lineitem in MarketBook.LineVMarkets.ToList())
                                {
                                    MarketBook linevmarketbook = LastloadedLinMarkets.Where(item => item.MarketId == lineitem.MarketCatalogueID).FirstOrDefault();
                                    if (linevmarketbook != null)
                                    {

                                        currentmarketsfancyPL = GetBookPosition(linevmarketbook.MarketId);
                                        if (currentmarketsfancyPL.Runners != null)
                                        {
                                            CurrentLiabality += currentmarketsfancyPL.Runners.Min(t => t.ProfitandLoss);
                                        }

                                    }


                                }
                            }
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 9)
                            {
                                List<UserBetsforSamiAdmin> lstCurrentmarketbets = LoggedinUserDetail.CurrentsamiadminBets.ToList().Where(item => item.MarketBookID == MarketBook.MarketId).ToList();
                                CurrentLiabality = objUserBets.GetLiabalityofsamiadmin(lstCurrentmarketbets, CurrentMarketProfitandLoss[0]);
                            }
                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 8)
                                {
                                    List<UserBetsforSuper> lstCurrentmarketbets = LoggedinUserDetail.CurrentSuperBets.ToList().Where(item => item.MarketBookID == MarketBook.MarketId).ToList();
                                    CurrentLiabality = objUserBets.GetLiabalityofSuper(lstCurrentmarketbets, CurrentMarketProfitandLoss[0]);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
        }
        public void UpdateLiabaliteies()
        {
            try
            {

                if (MarketBook.MarketId != null)
                {
                    ProfitandLoss objProfitandloss = new ProfitandLoss();
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        CurrentMarketProfitandLoss = objProfitandloss.CalculateProfitandLossAdmin(MarketBookForProfitandloss, LoggedinUserDetail.CurrentAdminBets.ToList());
                        try
                        {
                            if (MarketBookFigure.Runners != null)
                            {
                                CurrentMarketProfitandLossToFigre = objProfitandloss.CalculateProfitandLossAdminFig(MarketBookFigure, LoggedinUserDetail.CurrentAdminBets.ToList());
                            }
                        }
                        catch (System.Exception ex)
                        {

                        }
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            CurrentMarketProfitandLoss = objProfitandloss.CalculateProfitandLossAgent(MarketBookForProfitandloss, LoggedinUserDetail.CurrentAgentBets.ToList());
                            try
                            {
                                if (MarketBookFigure.Runners != null)
                                {
                                    CurrentMarketProfitandLossToFigre = objProfitandloss.CalculateProfitandLossAgentFig(MarketBookFigure, LoggedinUserDetail.CurrentAgentBets.ToList());
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 9 )
                            {
                                CurrentMarketProfitandLoss = objProfitandloss.CalculateProfitandLossSamiAdmin(MarketBookForProfitandloss, LoggedinUserDetail.CurrentsamiadminBets.ToList());
                                try
                                {
                                    if (MarketBookFigure.Runners != null)
                                    {
                                        CurrentMarketProfitandLossToFigre = objProfitandloss.CalculateProfitandLossSamiAdminFig(MarketBookFigure, LoggedinUserDetail.CurrentsamiadminBets.ToList());
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                            }
                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 8)
                                {
                                    CurrentMarketProfitandLoss = objProfitandloss.CalculateProfitandLossSuper(MarketBookForProfitandloss, LoggedinUserDetail.CurrentSuperBets.ToList());
                                    try
                                    {
                                        if (MarketBookFigure.Runners != null)
                                        {
                                            CurrentMarketProfitandLossToFigre = objProfitandloss.CalculateProfitandLossSuperFig(MarketBookFigure, LoggedinUserDetail.CurrentSuperBets.ToList());
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {

                                    }
                                }
                                else
                                {
                                    CurrentMarketProfitandLoss = objProfitandloss.CalculateProfitandLossEndUser(MarketBookForProfitandloss, LoggedinUserDetail.CurrentUserBets.ToList());
                                    try
                                    {
                                        if (MarketBookFigure.Runners != null)
                                        {
                                            CurrentMarketProfitandLossToFigre = objProfitandloss.CalculateProfitandLossEndUserFig(MarketBookFigure, LoggedinUserDetail.CurrentUserBets.ToList());
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {

                                    }
                                }
                            }
                        }

                        //  CurrentMarketProfitandLoss = objProfitandloss.CalculateProfitandLoss(lastloadedmarketforprofitandloss.MarketId, lastloadedmarketforprofitandloss.MarketBookName, lastloadedmarketforprofitandloss.OrignalOpenDate, lastloadedmarketforprofitandloss.MainSportsname);
                    }
                    CalculateAvearageforAllUsers();
                }
                UpdateUserLiablity();



                //  System.Threading.Thread.Sleep(500);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }


        public void UpdateFigurePL()
        {
            try
            {

                if (MarketBook.MarketId != null)
                {
                    ProfitandLoss objProfitandloss = new ProfitandLoss();
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        //  CurrentMarketProfitandLossToFigre = objProfitandloss.CalculateProfitandLossEndUserFig(MarketBookFigure, LoggedinUserDetail.CurrentUserBets.ToList());

                        //  CurrentMarketProfitandLoss = objProfitandloss.CalculateProfitandLossAdmin(MarketBookForProfitandloss, LoggedinUserDetail.CurrentAdminBets.ToList());

                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            CurrentMarketProfitandLossToFigre = objProfitandloss.CalculateProfitandLossEndUserFig(MarketBookFigure, LoggedinUserDetail.CurrentUserBets.ToList());

                            CurrentMarketProfitandLoss = objProfitandloss.CalculateProfitandLossAgent(MarketBookForProfitandloss, LoggedinUserDetail.CurrentAgentBets.ToList());
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                CurrentMarketProfitandLossToFigre = objProfitandloss.CalculateProfitandLossEndUserFig(MarketBookFigure, LoggedinUserDetail.CurrentUserBets.ToList());

                                CurrentMarketProfitandLoss = objProfitandloss.CalculateProfitandLossSuper(MarketBookForProfitandloss, LoggedinUserDetail.CurrentSuperBets.ToList());
                            }
                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 9)
                                {
                                    CurrentMarketProfitandLossToFigre = objProfitandloss.CalculateProfitandLossEndUserFig(MarketBookFigure, LoggedinUserDetail.CurrentUserBets.ToList());

                                    CurrentMarketProfitandLoss = objProfitandloss.CalculateProfitandLossSuper(MarketBookForProfitandloss, LoggedinUserDetail.CurrentSuperBets.ToList());

                                }

                                else
                                {

                                    CurrentMarketProfitandLossToFigre = objProfitandloss.CalculateProfitandLossEndUserFig(MarketBookFigure, LoggedinUserDetail.CurrentUserBets.ToList());
                                }
                            }
                        }

                        //  CurrentMarketProfitandLoss = objProfitandloss.CalculateProfitandLoss(lastloadedmarketforprofitandloss.MarketId, lastloadedmarketforprofitandloss.MarketBookName, lastloadedmarketforprofitandloss.OrignalOpenDate, lastloadedmarketforprofitandloss.MainSportsname);
                    }
                    CalculateAvearageforAllUsers();
                }
                UpdateUserLiablity();



                //  System.Threading.Thread.Sleep(500);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        public void UpdateLiabaliteiesToWintheToss()
        {
            try
            {

                if (MarketBookForProfitandlossToWinTheToss.MarketId != null)
                {
                    ProfitandLoss objProfitandloss = new ProfitandLoss();
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        CurrentMarketProfitandLossToWinTheToss = objProfitandloss.CalculateProfitandLossAdmin(MarketBookForProfitandlossToWinTheToss, LoggedinUserDetail.CurrentAdminBets.ToList());
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            CurrentMarketProfitandLossToWinTheToss = objProfitandloss.CalculateProfitandLossAgent(MarketBookForProfitandlossToWinTheToss, LoggedinUserDetail.CurrentAgentBets);
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                CurrentMarketProfitandLossToWinTheToss = objProfitandloss.CalculateProfitandLossSuper(MarketBookForProfitandlossToWinTheToss, LoggedinUserDetail.CurrentSuperBets.ToList());

                            }
                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 9)
                                {

                                }                          
                            else
                            {
                                CurrentMarketProfitandLossToWinTheToss = objProfitandloss.CalculateProfitandLossEndUser(MarketBookForProfitandlossToWinTheToss, LoggedinUserDetail.CurrentUserBets);
                            }
                        }
                        }

                        //  CurrentMarketProfitandLoss = objProfitandloss.CalculateProfitandLoss(lastloadedmarketforprofitandloss.MarketId, lastloadedmarketforprofitandloss.MarketBookName, lastloadedmarketforprofitandloss.OrignalOpenDate, lastloadedmarketforprofitandloss.MainSportsname);
                    }
                    CalculateAvearageforAllUsersTowinTheToss();
                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {
                        List<UserBets> lstCurrentmarketbets = LoggedinUserDetail.CurrentUserBets.Where(item => item.MarketBookID == MarketBookForProfitandlossToWinTheToss.MarketId).ToList();
                        CurrentLiabality += objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), CurrentMarketProfitandLossToWinTheToss[0], LoggedinUserDetail.CurrentUserBets);
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            List<UserBetsforAgent> lstCurrentmarketbets = LoggedinUserDetail.CurrentAgentBets.Where(item => item.MarketBookID == MarketBookForProfitandlossToWinTheToss.MarketId).ToList();
                            CurrentLiabality += objUserBets.GetLiabalityofCurrentAgent(lstCurrentmarketbets, CurrentMarketProfitandLossToWinTheToss[0]);
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                            {
                                List<UserBetsForAdmin> lstCurrentmarketbets = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.MarketBookID == MarketBookForProfitandlossToWinTheToss.MarketId).ToList();
                                CurrentLiabality += objUserBets.GetLiabalityofAdmin(lstCurrentmarketbets, CurrentMarketProfitandLossToWinTheToss[0]);
                            }
                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 8)
                                {
                                    List<UserBetsforSuper> lstCurrentmarketbets = LoggedinUserDetail.CurrentSuperBets.ToList().Where(item => item.MarketBookID == MarketBookForProfitandlossToWinTheToss.MarketId).ToList();
                                    CurrentLiabality += objUserBets.GetLiabalityofSuper(lstCurrentmarketbets, CurrentMarketProfitandLossToWinTheToss[0]);

                                }
                                else
                                {
                                    List<UserBetsforSuper> lstCurrentmarketbets = LoggedinUserDetail.CurrentSuperBets.ToList().Where(item => item.MarketBookID == MarketBookForProfitandlossToWinTheToss.MarketId).ToList();
                                    CurrentLiabality += objUserBets.GetLiabalityofSuper(lstCurrentmarketbets, CurrentMarketProfitandLossToWinTheToss[0]);
                                }

                            }
                        }
                    }
                }




                //  System.Threading.Thread.Sleep(500);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void ObjUsersServiceCleint_GetUserBetsbyAgentIDwithZeroRefererCompleted(object sender, GetUserBetsbyAgentIDwithZeroRefererCompletedEventArgs e)
        {
            ProfitandLoss objProfitandloss = new ProfitandLoss();
            var lstUserBetAgent = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(e.Result);
            CurrentMarketProfitandLossForAgent = objProfitandloss.CalculateProfitandLossAgent(MarketBookForProfitandlossAgent, lstUserBetAgent);
            CalculateAvearageforSelectedAgent();
        }



        private void BackgroundWorkerLiabalityandScore_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;

                    return;
                }
                System.Threading.Thread.Sleep(2500);
                GetDataForFancy(true);
                if (LoggedinUserDetail.user.isGrayHoundRaceAllowed == true)
                {
                    GetDataForFancy(MarketBook.EventID, MarketBook.MarketId);
                }


            }
            catch (System.Exception ex)
            {
                //scores = new List<MatchScores>();
            }
        }
        public void UpdateUserBetsData()
        {
            try
            {

                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {

                    List<bftradeline.Models.UserBets> lstCurrentmarketbets = LoggedinUserDetail.CurrentUserBets.Where(item => item.MarketBookID == MarketBook.MarketId).ToList();
                    int totmatchedbets = 0;
                    List<UserBets> lstMatchbets = lstCurrentmarketbets.Where(item => item.isMatched == true).ToList();
                    totmatchedbets = lstCurrentmarketbets.Where(item => item.isMatched == true).Count();

                    List<UserBets> lstFancyBetsin = new List<UserBets>();

                    // lstFancyBetsin = LoggedinUserDetail.CurrentUserBets.Where(item => item.isMatched == true && item.MarketBookID == MarketBook.EventID).ToList();
                    // totmatchedbets += lstFancyBetsin.Count;
                    // lstMatchbets.AddRange(lstFancyBetsin);

                   // if (MarketBook.LineVMarkets != null && MarketBook.MarketBookName.Contains("Match Odds"))
                    //{
                        List<UserBets> lstFancyBetsNew = new List<UserBets>();
                    // var a = MarketBook.LineVMarkets.Select(p => p.MarketCatalogueID).Distinct().ToArray();
                    //foreach (var lineitems in a)
                    //{
                            lstFancyBetsNew = LoggedinUserDetail.CurrentUserBets.Where(item => item.isMatched == true && item.MarketBookID == MarketBook.EventID).ToList();
                            totmatchedbets += lstFancyBetsNew.Count;
                            lstMatchbets.AddRange(lstFancyBetsNew);
                        //}
                   // }

                    if (MarketBook.KJMarkets != null && MarketBook.MarketBookName.Contains("Match Odds"))
                    {
                        List<UserBets> lstFancyBets = new List<UserBets>();
                        foreach (var lineitem1 in MarketBook.KJMarkets)
                        {
                            lstFancyBets = LoggedinUserDetail.CurrentUserBets.Where(item => item.isMatched == true && item.MarketBookID == lineitem1.MarketCatalogueID).ToList();
                            totmatchedbets += lstFancyBets.Count;
                            lstMatchbets.AddRange(lstFancyBets);
                        }
                    }
                    if (MarketBook.FigureMarkets != null && MarketBook.MarketBookName.Contains("Match Odds"))
                    {
                        List<UserBets> lstFancyBets = new List<UserBets>();
                        foreach (var lineitem1 in MarketBook.FigureMarkets)
                        {
                            lstFancyBets = LoggedinUserDetail.CurrentUserBets.Where(item => item.isMatched == true && item.MarketBookID == lineitem1.MarketCatalogueID).ToList();
                            totmatchedbets += lstFancyBets.Count;
                            lstMatchbets.AddRange(lstFancyBets);
                        }
                    }
                    if (MarketBook.SFigMarkets != null && MarketBook.MarketBookName.Contains("Match Odds"))
                    {
                        List<UserBets> lstFancyBets = new List<UserBets>();
                        foreach (var lineitem2 in MarketBook.SFigMarkets)
                        {
                            lstFancyBets = LoggedinUserDetail.CurrentUserBets.Where(item => item.isMatched == true && item.MarketBookID == lineitem2.MarketCatalogueID).ToList();
                            totmatchedbets += lstFancyBets.Count;
                            lstMatchbets.AddRange(lstFancyBets);
                        }
                    }


                    if (MarketBookForProfitandlossToWinTheToss.MarketId != null)
                    {
                        List<UserBets> lstWintheToss = new List<UserBets>();

                        lstWintheToss = LoggedinUserDetail.CurrentUserBets.Where(item => item.isMatched == true && item.MarketBookID == MarketBookForProfitandlossToWinTheToss.MarketId).ToList();
                        totmatchedbets += lstWintheToss.Count;
                        lstMatchbets.AddRange(lstWintheToss);

                    }

                    lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
                    var result = lstMatchbets.Take(20).Where(p => !lstCurrentBets.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                    var result1 = lstCurrentBets.Where(p => !lstMatchbets.Take(20).Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                    if (result.Count() > 0 || result1.Count() > 0)
                    {

                        lstCurrentBets.Clear();
                        foreach (var item in lstMatchbets.Take(20))
                        {
                            lstCurrentBets.Add(new UserBets() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, BetType = item.BetType, MarketBookID = item.MarketBookID });
                        }
                        LoggedinUserDetail.RefreshLiabality = true;
                    }
                    //&& btnShowHideBets.Tag.ToString() == "0"
                    if (lblMatchBetsCount.Content.ToString() != totmatchedbets.ToString())
                    {
                        // btnShowHideBets.Tag = "1";
                        Resizewindow();

                    }
                    lblMatchBetsCount.Content = totmatchedbets.ToString();

                    // DGVMatchedBets.ItemsSource = lstMatchbets.Take(10).ToList();
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {

                        List<UserBetsforAgent> lstMatchbets = LoggedinUserDetail.CurrentAgentBets.Where(item => item.isMatched == true).ToList();
                        lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
                        var result = lstMatchbets.Take(20).Where(p => !lstCurrentBetsAgent.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                        var result1 = lstCurrentBetsAgent.Where(p => !lstMatchbets.Take(20).Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                        if (result.Count() > 0 || result1.Count() > 0)
                        {

                            lstCurrentBetsAgent.Clear();
                            foreach (var item in lstMatchbets.Take(20))
                            {
                                lstCurrentBetsAgent.Add(new UserBetsforAgent() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, Name = item.Name, BetType = item.BetType, MarketBookID = item.MarketBookID });
                            }
                            LoggedinUserDetail.RefreshLiabality = true;
                        }
                        lblMatchBetsCount.Content = LoggedinUserDetail.CurrentAgentBets.Where(item => item.isMatched == true).Count().ToString();


                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                        {

                            List<UserBetsforSuper> lstMatchbets = LoggedinUserDetail.CurrentSuperBets.Where(item => item.isMatched == true).ToList();
                            lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
                            var result = lstMatchbets.Take(20).Where(p => !lstCurrentBetsSuper.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                            var result1 = lstCurrentBetsSuper.Where(p => !lstMatchbets.Take(20).Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                            if (result.Count() > 0 || result1.Count() > 0)
                            {

                                lstCurrentBetsSuper.Clear();
                                foreach (var item in lstMatchbets.Take(20))
                                {
                                    lstCurrentBetsSuper.Add(new UserBetsforSuper() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID });
                                }
                                LoggedinUserDetail.RefreshLiabality = true;
                            }
                            lblMatchBetsCount.Content = LoggedinUserDetail.CurrentSuperBets.ToList().Where(item => item.isMatched == true).Count().ToString();

                            // DGVMatchedBetsAdmin.DataContext = lstCurrentBetsSuper;
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                            {
                                
                                List<UserBetsForAdmin> lstMatchbets = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true).ToList();
                                 
                                lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
                                
                                //lstMatchbets = lstMatchbets.Where(x => x.location != "9").ToList();
                                var result = lstMatchbets.Take(20).Where(p => !lstCurrentBetsAdmin.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount ));
                                var result1 = lstCurrentBetsAdmin.Where(p => !lstMatchbets.Take(20).Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                                if (result.Count() > 0 || result1.Count() > 0)
                                {


                                    lstCurrentBetsAdmin.Clear();
                                    foreach (var item in lstMatchbets.Take(20))
                                    {
                                        if (item.DealerName == "Admin")
                                        {
                                            item.DealerName = item.AgentName;
                                        }
                                        if (item.location == "9")
                                        {
                                            lstCurrentBetsAdmin.Add(new UserBetsForAdmin() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.BetSize, Amount = item.Amount, LiveOdd = item.LiveOdd, CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID });
                                        }
                                        else
                                        {
                                            lstCurrentBetsAdmin.Add(new UserBetsForAdmin() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID });
                                        }
                                        }
                                    LoggedinUserDetail.RefreshLiabality = true;

                                }
                                lblMatchBetsCount.Content = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true).Count().ToString();
                                return;

                            }
                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 9)
                                {

                                    List<UserBetsforSamiAdmin> lstMatchbets = LoggedinUserDetail.CurrentsamiadminBets.Where(item => item.isMatched == true).ToList();
                                    lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
                                    var result = lstMatchbets.Take(20).Where(p => !lstCurrentBetssamiadmin.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                                    var result1 = lstCurrentBetssamiadmin.Where(p => !lstMatchbets.Take(20).Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                                    if (result.Count() > 0 || result1.Count() > 0)
                                    {

                                        lstCurrentBetssamiadmin.Clear();
                                        foreach (var item in lstMatchbets.Take(20))
                                        {
                                            lstCurrentBetssamiadmin.Add(new UserBetsforSamiAdmin() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID });
                                        }
                                        LoggedinUserDetail.RefreshLiabality = true;
                                    }
                                    lblMatchBetsCount.Content = LoggedinUserDetail.CurrentSuperBets.ToList().Where(item => item.isMatched == true).Count().ToString();

                                    // DGVMatchedBetsAdmin.DataContext = lstCurrentBetsSuper;
                                }
                            }



                        }
                    }
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        //public void UpdateUserBetsData()
        //{
        //    try
        //    {

        //        if (LoggedinUserDetail.GetUserTypeID() == 3)
        //        {

        //            List<bftradeline.Models.UserBets> lstCurrentmarketbets = LoggedinUserDetail.CurrentUserBets.Where(item => item.MarketBookID == MarketBook.MarketId).ToList();
        //            int totmatchedbets = 0;
        //            List<UserBets> lstMatchbets = lstCurrentmarketbets.Where(item => item.isMatched == true).ToList();
        //            totmatchedbets = lstCurrentmarketbets.Where(item => item.isMatched == true).Count();
        //            if (MarketBook.LineVMarkets != null && MarketBook.MarketBookName.Contains("Match Odds"))
        //            {
        //                List<UserBets> lstFancyBets = new List<UserBets>();
        //                foreach (var lineitem in MarketBook.LineVMarkets)
        //                {
        //                    lstFancyBets = LoggedinUserDetail.CurrentUserBets.Where(item => item.isMatched == true && item.MarketBookID == lineitem.MarketCatalogueID).ToList();
        //                    totmatchedbets += lstFancyBets.Count;
        //                    lstMatchbets.AddRange(lstFancyBets);
        //                }
        //            }
        //            lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
        //            var gridbetsold = (List<UserBets>)DGVMatchedBets.ItemsSource;
        //            if (gridbetsold == null)
        //            {
        //                DGVMatchedBets.ItemsSource = lstMatchbets.Take(20).ToList();
        //            }
        //            else
        //            {
        //                var result = lstMatchbets.Take(20).Where(p => !gridbetsold.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));
        //                var result1 = gridbetsold.Take(20).Where(p => !lstMatchbets.Take(20).Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));


        //                if (result.Count() > 0 || result1.Count() > 0)
        //                {
        //                    DGVMatchedBets.ItemsSource = lstMatchbets.Take(20).ToList();
        //                }

        //                else
        //                {

        //                    if (lstMatchbets.Count == 0)
        //                    {
        //                        if (DGVMatchedBets.Items.Count > 0)
        //                        {
        //                            DGVMatchedBets.ItemsSource = lstMatchbets;
        //                        }
        //                    }
        //                }
        //            }
        //            // DGVMatchedBets.ItemsSource = lstMatchbets.Take(10).ToList();

        //            if (lblMatchBetsCount.Content.ToString() != totmatchedbets.ToString() && btnShowHideBets.Tag.ToString() == "0")
        //            {
        //                btnShowHideBets.Tag = "1";
        //                Resizewindow();

        //            }
        //            lblMatchBetsCount.Content = totmatchedbets.ToString();





        //        }
        //        else
        //        {
        //            if (LoggedinUserDetail.GetUserTypeID() == 2)
        //            {

        //                List<UserBetsforAgent> lstMatchbets = LoggedinUserDetail.CurrentAgentBets.Where(item => item.isMatched == true).ToList();
        //                var gridbetsold = (List<UserBetsforAgent>)DGVMatchedBetsaGENT.ItemsSource;
        //                if (gridbetsold == null)
        //                {
        //                    DGVMatchedBetsaGENT.ItemsSource = lstMatchbets.Take(20).ToList();
        //                }
        //                else
        //                {

        //                    var result = lstMatchbets.Take(20).Where(p => !gridbetsold.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));
        //                    var result1 = gridbetsold.Take(20).Where(p => !lstMatchbets.Take(20).Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));


        //                    if (result.Count() > 0 || result1.Count() > 0)
        //                    {
        //                        DGVMatchedBetsaGENT.ItemsSource = lstMatchbets.Take(20).ToList();
        //                    }
        //                    else
        //                    {

        //                        if (lstMatchbets.Count == 0)
        //                        {
        //                            if (DGVMatchedBetsaGENT.Items.Count > 0)
        //                            {
        //                                DGVMatchedBetsaGENT.ItemsSource = lstMatchbets;
        //                            }
        //                        }
        //                    }
        //                }

        //                lblMatchBetsCount.Content = LoggedinUserDetail.CurrentAgentBets.Where(item => item.isMatched == true).Count().ToString();

        //            }
        //            else
        //            {
        //                if (LoggedinUserDetail.GetUserTypeID() == 1)
        //                {
        //                    List<UserBetsForAdmin> lstMatchbets = LoggedinUserDetail.CurrentAdminBets.Where(item => item.isMatched == true).ToList();
        //                    var gridbetsold = (List<UserBetsForAdmin>)DGVMatchedBetsAdmin.ItemsSource;
        //                    if (gridbetsold == null)
        //                    {
        //                        DGVMatchedBetsAdmin.ItemsSource = lstMatchbets.Take(12).ToList();
        //                    }
        //                    else
        //                    {

        //                        var result = lstMatchbets.Take(12).Where(p => !gridbetsold.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));
        //                        var result1 = gridbetsold.Take(12).Where(p => !lstMatchbets.Take(12).Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));


        //                        if (result.Count() > 0 || result1.Count() > 0)
        //                        {
        //                            DGVMatchedBetsAdmin.ItemsSource = lstMatchbets.Take(12).ToList();
        //                        }
        //                        else
        //                        {

        //                            if (lstMatchbets.Count == 0)
        //                            {
        //                                if (DGVMatchedBetsAdmin.Items.Count > 0)
        //                                {
        //                                    DGVMatchedBetsAdmin.ItemsSource = lstMatchbets;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //if (lstMatchbets.Count > 0)
        //                    //{
        //                    //    if (LoggedinUserDetail.CurrentAdminBets.Where(item => item.isMatched == true).Count() != Convert.ToInt32(lblMatchBetsCount.Content.ToString()))
        //                    //    {

        //                    //        PlayMessage("New Matched Bet.");
        //                    //        // Thread.Sleep(1000);
        //                    //    }
        //                    //}
        //                    lblMatchBetsCount.Content = LoggedinUserDetail.CurrentAdminBets.Where(item => item.isMatched == true).Count().ToString();

        //                }
        //            }
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {

        //    }
        //}
        public void UpdateUserBetsDataAll()
        {
            try
            {

                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {

                    List<bftradeline.Models.UserBets> lstCurrentmarketbets = LoggedinUserDetail.CurrentUserBets.Where(item => item.MarketBookID == MarketBook.MarketId).ToList();
                    int totmatchedbets = 0;
                    List<UserBets> lstMatchbets = lstCurrentmarketbets.Where(item => item.isMatched == true).ToList();
                    totmatchedbets = lstCurrentmarketbets.Where(item => item.isMatched == true).Count();
                    if (MarketBook.LineVMarkets != null && MarketBook.MarketBookName.Contains("Match Odds"))
                    {
                        List<UserBets> lstFancyBets = new List<UserBets>();
                        foreach (var lineitem in MarketBook.LineVMarkets)
                        {
                            lstFancyBets = LoggedinUserDetail.CurrentUserBets.Where(item => item.isMatched == true && item.MarketBookID == lineitem.MarketCatalogueID).ToList();
                            totmatchedbets += lstFancyBets.Count;
                            lstMatchbets.AddRange(lstFancyBets);
                        }
                    }
                    if (MarketBookForProfitandlossToWinTheToss.MarketId != null)
                    {
                        List<UserBets> lstWintheToss = new List<UserBets>();

                        lstWintheToss = LoggedinUserDetail.CurrentUserBets.Where(item => item.isMatched == true && item.MarketBookID == MarketBookForProfitandlossToWinTheToss.MarketId).ToList();
                        totmatchedbets += lstWintheToss.Count;
                        lstMatchbets.AddRange(lstWintheToss);

                    }
                    lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
                    DGVMatchedBetsAll.ItemsSource = lstMatchbets;
                    lblAllMatchedBets.Content = "Matched Bets " + totmatchedbets.ToString();

                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {

                        List<UserBetsforAgent> lstMatchbets = LoggedinUserDetail.CurrentAgentBets.Where(item => item.isMatched == true).ToList();
                        lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
                        DGVMatchedBetsaGENTAll.ItemsSource = lstMatchbets;

                        lblAllMatchedBets.Content = "Matched Bets " + LoggedinUserDetail.CurrentAgentBets.Where(item => item.isMatched == true).Count().ToString();

                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            List<UserBetsForAdmin> lstMatchbets = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true).ToList();
                            lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
                            DGVMatchedBetsAdminAll.ItemsSource = lstMatchbets;

                            lblAllMatchedBets.Content = "Matched Bets " + LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true).Count().ToString();

                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                List<UserBetsforSuper> lstMatchbets = LoggedinUserDetail.CurrentSuperBets.ToList().Where(item => item.isMatched == true).ToList();
                                lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
                                DGVMatchedBetsAdminAll.ItemsSource = lstMatchbets;

                                lblAllMatchedBets.Content = "Matched Bets " + LoggedinUserDetail.CurrentSuperBets.ToList().Where(item => item.isMatched == true).Count().ToString();

                            }
                        }
                    }
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        public void UpdateUserBetsDataUnMatched()
        {
            try
            {

                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    // UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();


                    List<UserBets> lstCurrentmarketbets = LoggedinUserDetail.CurrentUserBets.Where(item => item.MarketBookID == MarketBook.MarketId).ToList();
                    List<UserBets> lstUnMAtchBets = lstCurrentmarketbets.Where(item => item.isMatched == false).ToList();
                    if (MarketBook.LineVMarkets != null)
                    {
                        foreach (var lineitem in MarketBook.LineVMarkets)
                        {
                            lstUnMAtchBets.AddRange(LoggedinUserDetail.CurrentUserBets.Where(item => item.isMatched == false && item.MarketBookID == lineitem.MarketCatalogueID).ToList());
                        }
                    }
                    if (MarketBookForProfitandlossToWinTheToss.MarketId != null)
                    {
                        List<UserBets> lstWintheToss = new List<UserBets>();

                        lstWintheToss = LoggedinUserDetail.CurrentUserBets.Where(item => item.isMatched == false && item.MarketBookID == MarketBookForProfitandlossToWinTheToss.MarketId).ToList();

                        lstUnMAtchBets.AddRange(lstWintheToss);

                    }
                    lstUnMAtchBets = lstUnMAtchBets.OrderByDescending(item => item.ID).ToList();
                    var result = lstUnMAtchBets.Where(p => !lstCurrentBetsUnMatched.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));
                    var result1 = lstCurrentBetsUnMatched.Where(p => !lstUnMAtchBets.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                    if (result.Count() > 0 || result1.Count() > 0)
                    {

                        lstCurrentBetsUnMatched.Clear();
                        foreach (var item in lstUnMAtchBets)
                        {
                            lstCurrentBetsUnMatched.Add(new UserBets() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, BetType = item.BetType, MarketBookID = item.MarketBookID });
                        }

                    }
                    lblUnMatchBetsCount.Content = lstUnMAtchBets.Count().ToString();
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                        // UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                        List<UserBetsforAgent> lstUnMAtchBets = LoggedinUserDetail.CurrentAgentBets.Where(item => item.isMatched == false).ToList();
                        lstUnMAtchBets = lstUnMAtchBets.OrderByDescending(item => item.ID).ToList();
                        var result = lstUnMAtchBets.Where(p => !lstCurrentBetsAgentUnMatched.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));
                        var result1 = lstCurrentBetsAgentUnMatched.Where(p => !lstUnMAtchBets.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                        if (result.Count() > 0 || result1.Count() > 0)
                        {

                            lstCurrentBetsAgentUnMatched.Clear();
                            foreach (var item in lstUnMAtchBets)
                            {
                                lstCurrentBetsAgentUnMatched.Add(new UserBetsforAgent() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, Name = item.Name, BetType = item.BetType, MarketBookID = item.MarketBookID });
                            }

                        }
                        lblUnMatchBetsCount.Content = lstUnMAtchBets.Count().ToString();
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            //   UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();



                            List<UserBetsForAdmin> lstUnMAtchBets = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == false).ToList();
                            lstUnMAtchBets = lstUnMAtchBets.OrderByDescending(item => item.ID).ToList();
                            lblUnMatchBetsCount.Content = lstUnMAtchBets.Count().ToString();
                            var result = lstUnMAtchBets.Where(p => !lstCurrentBetsAdminUnMAtched.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));
                            var result1 = lstCurrentBetsAdminUnMAtched.Where(p => !lstUnMAtchBets.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                            if (result.Count() > 0 || result1.Count() > 0)
                            {

                                lstCurrentBetsAdminUnMAtched.Clear();
                                foreach (var item in lstUnMAtchBets)
                                {
                                    if (item.DealerName == "Admin")
                                    {
                                        item.DealerName = item.AgentName;
                                    }
                                    lstCurrentBetsAdminUnMAtched.Add(new UserBetsForAdmin() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID });
                                }

                            }

                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                //   UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                                List<UserBetsforSuper> lstUnMAtchBets = LoggedinUserDetail.CurrentSuperBets.ToList().Where(item => item.isMatched == false).ToList();
                                lstUnMAtchBets = lstUnMAtchBets.OrderByDescending(item => item.ID).ToList();
                                lblUnMatchBetsCount.Content = lstUnMAtchBets.Count().ToString();
                                var result = lstUnMAtchBets.Where(p => !lstCurrentBetSuperUnMAtched.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));
                                var result1 = lstCurrentBetSuperUnMAtched.Where(p => !lstUnMAtchBets.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                                if (result.Count() > 0 || result1.Count() > 0)
                                {

                                    lstCurrentBetSuperUnMAtched.Clear();
                                    foreach (var item in lstUnMAtchBets)
                                    {
                                        lstCurrentBetSuperUnMAtched.Add(new UserBetsforSuper() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID });
                                    }

                                }
                                DGVUnMatchedAdmin.DataContext = lstCurrentBetSuperUnMAtched;
                            }
                        }
                    }
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void TimerCountdown_Tick(object sender, EventArgs e)
        {
            if (MarketBook.MainSportsname.Contains("Racing"))
            {
                if (lblWaitTimer.Content.ToString() == "1")
                {
                    lblWaitTimer.Content = "";
                    timerCountdown.Stop();
                }
            }
            else
            {
                if (lblWaitTimer.Content.ToString() == "4")
                {
                    lblWaitTimer.Content = "";
                    timerCountdown.Stop();
                }
            }



            if (lblWaitTimer.Content.ToString() != "")
            {
                lblWaitTimer.Content = (Convert.ToInt32(lblWaitTimer.Content.ToString()) + 1).ToString();
            }
        }

        public decimal clickedodd = 0;
        public double clickedbetsize = 0;
        public double newamount = 0;
        public int loadedlocation = -1;
        public int ParentID = 0;
        public string Selectionname = "";
        public string Marketbookname = "";
        public string SelectionID = "";
        public string MarketID = "";
        public string SelectionIDpublic = "";
        public decimal BackPrice = 0;
        public decimal Layprice = 0;
        public double LaySizevalue = 0;
        public double BackSizevalue = 0;
        public int location = -1;
        public int Clickedlocation = -1;
        public int Clickedlocationin;
        public void insertbetslip()
        {
            try
            {
               // bool objUserStatusInplay = objUsersServiceCleint.GetBettingAllowedbyMarketIDandUserIDInplay(LoggedinUserDetail.GetUserID());

                if (currmarketbookforbet.Runners.Count == 1)
                {
                    currmarketbookforbet.MarketBookName = currmarketbookforbet.MarketBookName + "/" + MarketBook.MarketBookName.Replace("Match Odds / ", "");
                }
                string a = currmarketbookforbet.MainSportsname;
                decimal userodd1 = Convert.ToDecimal(nupdownOdd.Value.Value.ToString("F2"));

                //if (objUserStatusInplay == true && a == "Horse Racing" && userodd1 > 8 && BetType == "lay")
                //{

                //}
                //else
                //{
                    objUsersServiceCleint.InsertUserBetNew(userodd1, SelectionID, Selectionname, BetType, nupdnUserAmount.Value.ToString(), betslipamountlabel.Content.ToString(), LoggedinUserDetail.user.MaxOddBack, LoggedinUserDetail.user.MaxOddLay, LoggedinUserDetail.user.CheckforMaxOddBack, LoggedinUserDetail.user.CheckforMaxOddLay, Clickedlocation, LoggedinUserDetail.GetUserID(), Betslipsize.Text, LoggedinUserDetail.PasswordForValidate, currmarketbookforbet.MarketId, currmarketbookforbet.MarketBookName, true);
                    return;
                //}


            }
            catch (System.Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message.ToString());
            }
        }

        public void insertbetslipin()
        {
            try
            {

                decimal userodd1 = Convert.ToDecimal(nupdownOddin.Value.Value.ToString("F2"));
                objUsersServiceCleint.InsertUserBetNew(userodd1, SelectionID, Selectionname, BetType, nupdnUserAmountin.Value.ToString(), betslipamountlabelin.Content.ToString(), LoggedinUserDetail.user.MaxOddBack, LoggedinUserDetail.user.MaxOddLay, LoggedinUserDetail.user.CheckforMaxOddBack, LoggedinUserDetail.user.CheckforMaxOddLay, Clickedlocationin, LoggedinUserDetail.GetUserID(), Betslipsizein.Text, LoggedinUserDetail.PasswordForValidate, CurrentMarketBookId, MarketbooknameBet, false);
                return;
            }
            catch (System.Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message.ToString());
            }
        }

        public void insertbetslipin1()
        {
            try
            {
                bool objUserStatusInplay = objUsersServiceCleint.GetBettingAllowedbyMarketIDandUserIDInplay(LoggedinUserDetail.GetUserID());

                if (MarketBook.Runners.Count == 1)
                {
                    currmarketbookforbet.MarketBookName = currmarketbookforbet.MarketBookName + "/" + MarketBook.MarketBookName.Replace("Match Odds / ", "");
                }
                string a = currmarketbookforbet.MainSportsname;
                //decimal userodd1 = Convert.ToDecimal(Betslipsizein.Text);
                decimal userodd1 = Convert.ToDecimal(nupdownOddin1.Value.Value.ToString("F2"));

                if (objUserStatusInplay == true && a == "Horse Racing" && userodd1 > 8 && BetType == "lay")
                {

                }
                else
                {
                    objUsersServiceCleint.InsertUserBetNew(userodd1, SelectionID, Selectionname, BetType, nupdnUserAmountin1.Value.ToString(), betslipamountlabelin1.Content.ToString(), LoggedinUserDetail.user.MaxOddBack, LoggedinUserDetail.user.MaxOddLay, LoggedinUserDetail.user.CheckforMaxOddBack, LoggedinUserDetail.user.CheckforMaxOddLay, Clickedlocationin, LoggedinUserDetail.GetUserID(), Betslipsizein1.Text, LoggedinUserDetail.PasswordForValidate, CurrentMarketBookId, MarketbooknameBet, false);
                    return;
                }


            }
            catch (System.Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message.ToString());
            }
        }
        private void BackgroundWorkerInsertBet_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                LoggedinUserDetail.isInserting = true;
                insertbetslip();
                HideProgressBar();
                popupBetslip.IsOpen = false;
                LoggedinUserDetail.isInserting = false;

                ParentID = 0;
                btnSubmitBetSlip.IsEnabled = true;
                btnResetBetSlip.IsEnabled = true;
            }
            catch (System.Exception ex)
            {
                // tmrUpdateMarket.Start();
                HideProgressBar();
                LoggedinUserDetail.isInserting = false;

                ParentID = 0;
                btnSubmitBetSlip.IsEnabled = true;
                btnResetBetSlip.IsEnabled = true;
                PlayMessage("Something went wrong, Please bet again");
                Xceed.Wpf.Toolkit.MessageBox.Show("Something went wrong, Please bet again.");
                LoggedinUserDetail.LogError(ex);
            }
        }

        private void BackgroundWorkerInsertBet_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(BetPlaceWait);
        }
        private void BackgroundWorkerInsertBetin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                LoggedinUserDetail.isInserting = true;
                insertbetslipin();
                HideProgressBar();
                popupBetslipforin.IsOpen = false;
                LoggedinUserDetail.isInserting = false;

                ParentID = 0;
                btnSubmitBetSlipin.IsEnabled = true;
                btnResetBetSlipin.IsEnabled = true;
            }
            catch (System.Exception ex)
            {
                // tmrUpdateMarket.Start();
                HideProgressBar();
                LoggedinUserDetail.isInserting = false;
                ParentID = 0;
                btnSubmitBetSlipin.IsEnabled = true;
                btnResetBetSlipin.IsEnabled = true;
                PlayMessage("Something went wrong, Please bet again");
                Xceed.Wpf.Toolkit.MessageBox.Show("Something went wrong, Please bet again.");
                LoggedinUserDetail.LogError(ex);
            }
        }

        private void BackgroundWorkerInsertBetin1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                LoggedinUserDetail.isInserting = true;
                insertbetslipin1();
                HideProgressBar();
                popupBetslipforin1.IsOpen = false;
                LoggedinUserDetail.isInserting = false;
                ParentID = 0;
                btnSubmitBetSlipin1.IsEnabled = true;
                btnResetBetSlipin1.IsEnabled = true;
            }
            catch (System.Exception ex)
            {
                // tmrUpdateMarket.Start();
                HideProgressBar();
                LoggedinUserDetail.isInserting = false;

                ParentID = 0;
                btnSubmitBetSlipin1.IsEnabled = true;
                btnResetBetSlipin1.IsEnabled = true;
                PlayMessage("Something went wrong, Please bet again");
                Xceed.Wpf.Toolkit.MessageBox.Show("Something went wrong, Please bet again.");
                LoggedinUserDetail.LogError(ex);
            }
        }

        private void BackgroundWorkerInsertBetin_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(BetPlaceWait);
        }
        private void BackgroundWorkerInsertBetin1_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(BetPlaceWait);
        }

        public DispatcherTimer timerCountdown = new DispatcherTimer();
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (MarketBook.MainSportsname == "Cricket" && MarketBook.MarketBookName.Contains("Match Odds"))
                {
                    //GetUpdate(MarketBook.EventID);
                }
                backgroundWorkerGetScore.RunWorkerAsync();
            }
            catch (System.Exception ex)
            {

            }
        }


        private void backgroundWorkerSCT_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                if (MarketBook.MainSportsname.Contains("Soccer"))
                {
                    //  CreateScoreCard();
                    GetUpdateSCT("1");
                }
                else
                {
                    GetUpdateSCT("2");
                }
                backgroundWorkerGetScoreSCT.RunWorkerAsync();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }


        }
        public static string FormatNumber(double n)
        {
            if (n < 1000)
                return n.ToString();

            if (n < 10000)
                return String.Format("{0:#,.##}K", n - 5);

            if (n < 100000)
                return String.Format("{0:#,.#}K", n - 50);

            if (n < 1000000)
                return String.Format("{0:#,.}K", n - 500);

            if (n < 10000000)
                return String.Format("{0:#,,.##}M", n - 5000);

            if (n < 100000000)
                return String.Format("{0:#,,.#}M", n - 50000);

            if (n < 1000000000)
                return String.Format("{0:#,,.}M", n - 500000);

            return String.Format("{0:#,,,.##}B", n - 5000000);
        }

        public static string strWsMatch = ConfigurationManager.AppSettings["URLForData"];
        public MarketBook ConvertJsontoMarketObjectBFNewSource(ExternalAPI.TO.MarketBookString BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool BettingAllowed)
        {



            if (1 == 1)
            {
                var marketbook = new MarketBook();
                string[] newres = BFMarketbook.MarketBookData.Split(':').Select(tag => tag.Trim()).ToArray();
                string[] BFMarketBookDetail = newres[0].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();

                marketbook.MarketId = BFMarketbook.MarketBookId;
                marketbook.SheetName = "";
                marketbook.IsMarketDataDelayed = false;
                marketbook.PoundRate = LoggedinUserDetail.PoundRate;
                marketbook.NumberOfWinners = Convert.ToInt32(BFMarketBookDetail[6]);
                marketbook.MarketBookName = sheetname;
                marketbook.MainSportsname = MainSportsCategory;
                marketbook.OrignalOpenDate = marketopendate;
                marketbook.BettingAllowed = BettingAllowed;
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

                    marketbook.MarketStatusstr = "IN-PLAY";
                }
                else
                {
                    if (BFMarketBookDetail[2].Trim().ToString() == "CLOSED")
                    {
                        marketbook.MarketStatusstr = "CLOSED";
                    }
                    else
                    {
                        if (BFMarketBookDetail[2].Trim().ToString() == "SUSPENDED")
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


                for (int i = 1; i < newres.Count(); i++)
                {
                    if (newres[i] != "Æ  Æ")
                    {
                        string[] runnerdetails = newres[i].Split(new string[] { "|" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();
                        string[] runnerinfo = runnerdetails[0].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();
                        string[] runnerbackdata = runnerdetails[1].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                        string[] runnerlaydata = runnerdetails[2].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                        var runner = new ExternalAPI.TO.Runner();

                        runner.Handicap = 0;
                        runner.StatusStr = runnerinfo[6].Trim();
                        runner.SelectionId = runnerinfo[0].Trim().ToString();
                        runner.RunnerName = sheetname;
                        runner.LastPriceTraded = 0;
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
                                                pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]));
                                                pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                                pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]));
                                                pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                        pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]));
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                                pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]));
                                                pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                                pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]));
                                                pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                        pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]));
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
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

                return marketbook;

            }
            else
            {
                return new MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBF123Fancy(SampleResponse1 BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, bool BettingAllowed)
        {


            try
            {


                if (1 == 1)
                {
                    var marketbook = new MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.BettingAllowed = BettingAllowed;
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = LoggedinUserDetail.PoundRate;
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

                        marketbook.MarketStatusstr = "IN-PLAY";
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
                        var runner = new ExternalAPI.TO.Runner();

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
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = backitems.Price;

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {

                                }
                            }
                            else
                            {
                                foreach (var backitems in runneritem.Ex.AvailableToBack)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
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
#pragma warning disable CS0162 // Unreachable code detected
                    return new MarketBook();
#pragma warning restore CS0162 // Unreachable code detected
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new MarketBook();
            }
        }
        //public MarketBook ConvertJsontoMarketObjectBF123Fancy(SampleResponse1 BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, bool BettingAllowed)
        //{


        //    try
        //    {


        //        if (1 == 1)
        //        {
        //            var marketbook = new MarketBook();

        //            marketbook.MarketId = BFMarketbook.MarketId;
        //            marketbook.SheetName = "";
        //            marketbook.BettingAllowed = BettingAllowed;
        //            marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
        //            marketbook.PoundRate = LoggedinUserDetail.PoundRate;
        //            marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
        //            marketbook.MarketBookName = sheetname;
        //            marketbook.MainSportsname = MainSportsCategory;
        //            marketbook.OrignalOpenDate = marketopendate;
        //            marketbook.Version = BFMarketbook.Version;
        //            marketbook.TotalMatched = BFMarketbook.TotalMatched;
        //            DateTime OpenDate = marketbook.OrignalOpenDate.AddHours(5);
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

        //            if (BFMarketbook.Inplay == true && BFMarketbook.Status.ToString() == "OPEN")
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

        //            List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
        //            foreach (var runneritem in BFMarketbook.Runners)
        //            {
        //                var runner = new ExternalAPI.TO.Runner();


        //                runner.Handicap = runneritem.Handicap;
        //                runner.StatusStr = runneritem.Status.ToString();
        //                runner.SelectionId = runneritem.SelectionId.ToString();
        //                runner.LastPriceTraded = runneritem.LastPriceTraded;
        //                var lstpricelist = new List<PriceSize>();
        //                if (runneritem.Ex.AvailableToBack != null && runneritem.Ex.AvailableToBack.Count() > 0)
        //                {
        //                    if (1 == 2)
        //                    {

        //                        foreach (var backitems in runneritem.Ex.AvailableToLay)
        //                        {
        //                            var pricesize = new PriceSize();

        //                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
        //  pricesize.SizeStr = FormatNumber(pricesize.Size);
        //                            pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

        //                            lstpricelist.Add(pricesize);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        foreach (var backitems in runneritem.Ex.AvailableToBack)
        //                        {
        //                            var pricesize = new PriceSize();

        //                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
        //pricesize.SizeStr = FormatNumber(pricesize.Size);
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
        //                if (runneritem.Ex.AvailableToLay != null && runneritem.Ex.AvailableToLay.Count() > 0)
        //                {
        //                    if (1 == 2)
        //                    {

        //                        foreach (var backitems in runneritem.Ex.AvailableToBack)
        //                        {
        //                            var pricesize = new PriceSize();

        //                            pricesize.Size = Convert.ToInt64((backitems.Size) * Convert.ToDouble(marketbook.PoundRate));

        //                            pricesize.Price = Convert.ToDouble((backitems.Price + 0.5).ToString("F2"));

        //                            lstpricelist.Add(pricesize);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        foreach (var backitems in runneritem.Ex.AvailableToLay)
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


        //            marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);


        //            return marketbook;

        //        }
        //        else
        //        {
        //            return new MarketBook();
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return new MarketBook();
        //    }
        //}
        public MarketBook ConvertJsontoMarketObjectBFFancy(bftradeline.wrBF.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool BetingAllowed)
        {

            try
            {
                if (BFMarketbook != null)
                {
                    var marketbook = new MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = LoggedinUserDetail.PoundRate;
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.BettingAllowed = BetingAllowed;
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
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = backitems.Price;

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {

                                }

                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
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
#pragma warning disable CS0162 // Unreachable code detected
                    double lastback = 0;
#pragma warning restore CS0162 // Unreachable code detected
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


                }
                else
                {
                    return new MarketBook();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBFFancyLive(MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool BetingAllowed)
        {


            try
            {


                if (BFMarketbook != null)
                {
                    var marketbook = new MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = LoggedinUserDetail.PoundRate;
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.BettingAllowed = BetingAllowed;
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
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                                        {
                                            var pricesize = new PriceSize();

                                            pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = backitems.Price;

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {

                                }

                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBFFancyOthers(bftradeline.wrBF.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool BetingAllowed)
        {


            try
            {


                if (BFMarketbook != null)
                {
                    var marketbook = new MarketBook();

                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = LoggedinUserDetail.PoundRate;
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.BettingAllowed = BetingAllowed;
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
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
                                            pricesize.Price = backitems.Price;

                                            lstpricelist.Add(pricesize);
                                        }
                                    }
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {

                                }

                            }
                            else
                            {
                                foreach (var backitems in runneritem.ExchangePrices.AvailableToBack)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size * Convert.ToDouble(marketbook.PoundRate));
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                        pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                    pricesize.SizeStr = FormatNumber(pricesize.Size);
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new MarketBook();
            }
        }




        public int checkint = 0;



        OddsData objOddsData = new OddsData();
        string matchCricketAPIKey = "";
        public void Removeallbackgroundworkers()
        {
            try
            {
                backgroundWorkerGetScore.CancelAsync();
                backgroundWorkerGetScore.Dispose();
                backgroundWorkerGetScore = null;
                // backgroundWorkertotalmatched.CancelAsync();
                // backgroundWorkertotalmatched.Dispose();
                // backgroundWorkertotalmatched = null;
                backgroundWorkerLiabalityandScore.CancelAsync();
                backgroundWorkerLiabalityandScore.Dispose();
                backgroundWorkerLiabalityandScore = null;
                backgroundWorkerProfitandlossbyAgent.CancelAsync();
                backgroundWorkerProfitandlossbyAgent.Dispose();
                backgroundWorkerProfitandlossbyAgent = null;
                backgroundWorkerUpdateData.CancelAsync();
                backgroundWorkerUpdateData.Dispose();
                backgroundWorkerUpdateData = null;
                backgroundWorkerUpdateFancyData.CancelAsync();
                backgroundWorkerUpdateFancyData.Dispose();
                backgroundWorkerUpdateFancyData = null;
                backgroundWorkerUpdateFigData.CancelAsync();
                backgroundWorkerUpdateFigData.Dispose();
                backgroundWorkerUpdateFigData = null;

                GC.Collect();
                tmrUpdateMarket.Stop();
                //tmrUpdateMarketKJ.Stop();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                tmrUpdateMarket.Stop();
                tmrUpdateMarketKJ.Stop();
            }
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;

                    return;
                }

                if (MarketBook.MainSportsname.Contains("Cricket") && MarketBook.MarketBookName.Contains("Match Odds"))
                {
                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

            // objOddsData.  GetCurrentMarketBook(MarketBook.MarketId, MarketBook.MarketBookName, MarketBook.MainSportsname, MarketBook.OrignalOpenDate, MarketBook.BettingAllowed,MarketBook.Runners,MarketBook);

            // GetDataForFancy(false);

        }

        private void backgroundWorkerSCT_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;

                    return;
                }

                if (MarketBook.MainSportsname.Contains("Soccer"))
                {
                    
                    System.Threading.Thread.Sleep(5000);
                    // InitiateLZCricketAPI(matchCricketAPIKey);

                }
                else
                {

                    System.Threading.Thread.Sleep(10000);
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

            // objOddsData.  GetCurrentMarketBook(MarketBook.MarketId, MarketBook.MarketBookName, MarketBook.MainSportsname, MarketBook.OrignalOpenDate, MarketBook.BettingAllowed,MarketBook.Runners,MarketBook);

            // GetDataForFancy(false);

        }

        public List<MarketBook> LastloadedLinMarkets = new List<ExternalAPI.TO.MarketBook>();
        public BackgroundWorker backgroundWorkerGetScore;
        public BackgroundWorker backgroundWorkerGetScoreSCT;
        // public BackgroundWorker backgroundWorkertotalmatched;
        public BackgroundWorker backgroundWorkerUpdateData;
        public BackgroundWorker backgroundWorkerUpdateFancyData;
        public BackgroundWorker backgroundWorkerUpdateFigData;
        public BackgroundWorker backgroundWorkerInsertBet;
        public BackgroundWorker backgroundWorkerInsertBetin;
        public BackgroundWorker backgroundWorkerInsertBetin1;
        public BackgroundWorker backgroundWorkerLiabalityandScore;
        public BackgroundWorker backgroundWorkerProfitandlossbyAgent;
        public BackgroundWorker backgroundWorkerUpdateAllotherData;
        //private ObservableCollection<UserBetsForAdmin> _lstCurrentAdminBets;
        //public ObservableCollection<UserBetsForAdmin> lstCurrentAdminBets
        //{
        //    get { return _lstCurrentAdminBets; }
        //    set
        //    {

        //        _lstCurrentAdminBets = value;
        //        OnPropertyChanged("lstCurrentAdminBets");
        //    }
        //}
        private ObservableCollection<UserBets> lstCurrentBets = new ObservableCollection<UserBets>();
        private ObservableCollection<UserBetsforAgent> lstCurrentBetsAgent = new ObservableCollection<UserBetsforAgent>();
        private ObservableCollection<UserBetsforSuper> lstCurrentBetsSuper = new ObservableCollection<UserBetsforSuper>();
        private ObservableCollection<UserBetsforSamiAdmin> lstCurrentBetssamiadmin = new ObservableCollection<UserBetsforSamiAdmin>();
        private ObservableCollection<UserBetsForAdmin> lstCurrentBetsAdmin;
        private ObservableCollection<UserBets> lstCurrentBetsUnMatched = new ObservableCollection<UserBets>();
        private ObservableCollection<UserBetsforAgent> lstCurrentBetsAgentUnMatched = new ObservableCollection<UserBetsforAgent>();
        private ObservableCollection<UserBetsForAdmin> lstCurrentBetsAdminUnMAtched = new ObservableCollection<UserBetsForAdmin>();
        private ObservableCollection<UserBetsforSuper> lstCurrentBetSuperUnMAtched = new ObservableCollection<UserBetsforSuper>();
        private ObservableCollection<UserBetsforSamiAdmin> lstCurrentBetsamiadminUnMAtched = new ObservableCollection<UserBetsforSamiAdmin>();

        private ObservableCollection<MarketBookShow> _lstMarketBookRunners;

        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersToWintheToss;
        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersToWintheToss0;
        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersToWintheToss1;
        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersToWintheToss2;

        private ObservableCollection<MarketBookShow> _lstMarketBookRunnerKalijut;

        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersFancy;

        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersFancyin;

        private ObservableCollection<MarketBookShow> _lstMarketBookRunnerSFigure;

        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersFigure;

        public ObservableCollection<MarketBookShow> lstMarketBookRunners
        {
            get { return _lstMarketBookRunners; }
            set
            {
                if (_lstMarketBookRunners == value)
                    return;

                _lstMarketBookRunners = value;
                OnPropertyChanged("lstMarketBookRunners");
            }
        }
        public ObservableCollection<MarketBookShow> lstMarketBookRunnersToWintheToss
        {
            get { return _lstMarketBookRunnersToWintheToss; }
            set
            {
                if (_lstMarketBookRunnersToWintheToss == value)
                    return;

                _lstMarketBookRunnersToWintheToss = value;
                OnPropertyChanged("lstMarketBookRunnersToWintheToss");
            }
        }
        public ObservableCollection<MarketBookShow> lstMarketBookRunnersToWintheToss0
        {
            get { return _lstMarketBookRunnersToWintheToss0; }
            set
            {
                if (_lstMarketBookRunnersToWintheToss0 == value)
                    return;

                _lstMarketBookRunnersToWintheToss0 = value;
                OnPropertyChanged("lstMarketBookRunnersToWintheToss0");
            }
        }
        public ObservableCollection<MarketBookShow> lstMarketBookRunnersToWintheToss1
        {
            get { return _lstMarketBookRunnersToWintheToss1; }
            set
            {
                if (_lstMarketBookRunnersToWintheToss1 == value)
                    return;

                _lstMarketBookRunnersToWintheToss1 = value;
                OnPropertyChanged("lstMarketBookRunnersToWintheToss1");
            }
        }
        public ObservableCollection<MarketBookShow> lstMarketBookRunnersToWintheToss2
        {
            get { return _lstMarketBookRunnersToWintheToss2; }
            set
            {
                if (_lstMarketBookRunnersToWintheToss2 == value)
                    return;

                _lstMarketBookRunnersToWintheToss2 = value;
                OnPropertyChanged("lstMarketBookRunnersToWintheToss2");
            }
        }
        public ObservableCollection<MarketBookShow> lstMarketBookRunnersFancy
        {
            get { return _lstMarketBookRunnersFancy; }
            set
            {
                if (_lstMarketBookRunnersFancy == value)
                    return;

                _lstMarketBookRunnersFancy = value;
                OnPropertyChanged("lstMarketBookRunnersFancy");
            }
        }

        public ObservableCollection<MarketBookShow> lstMarketBookRunnersSoccer
        {
            get { return _lstMarketBookRunnersFancy; }
            set
            {
                if (_lstMarketBookRunnersFancy == value)
                    return;

                _lstMarketBookRunnersFancy = value;
                OnPropertyChanged("lstMarketBookRunnersSoccer");
            }
        }

        public ObservableCollection<MarketBookShow> lstMarketBookRunnersFancyin
        {
            get { return _lstMarketBookRunnersFancyin; }
            set
            {
                if (_lstMarketBookRunnersFancyin == value)
                    return;

                _lstMarketBookRunnersFancyin = value;
                OnPropertyChanged("lstMarketBookRunnersFancyin");
            }
        }
        public ObservableCollection<MarketBookShow> lstMarketBookRunnerKalijut
        {
            get { return _lstMarketBookRunnerKalijut; }
            set
            {
                if (_lstMarketBookRunnerKalijut == value)
                    return;

                _lstMarketBookRunnerKalijut = value;
                OnPropertyChanged("lstMarketBookRunnerKalijut");
            }
        }

        public ObservableCollection<MarketBookShow> lstMarketBookRunnerSFigure
        {
            get { return _lstMarketBookRunnerSFigure; }
            set
            {
                if (_lstMarketBookRunnerSFigure == value)
                    return;

                _lstMarketBookRunnerSFigure = value;
                OnPropertyChanged("lstMarketBookRunnerSFigure");
            }
        }

        public ObservableCollection<MarketBookShow> lstMarketBookRunnersFigure
        {
            get { return _lstMarketBookRunnersFigure; }
            set
            {
                if (_lstMarketBookRunnersFigure == value)
                    return;

                _lstMarketBookRunnersFigure = value;
                OnPropertyChanged("lstMarketBookRunnersFigure");
            }
        }


        public DispatcherTimer tmrUpdateMarket = new DispatcherTimer();
        public DispatcherTimer tmrUpdateMarketKJ = new DispatcherTimer();
        public DispatcherTimer tmrUpdateLiabalities = new DispatcherTimer();

        public ExternalAPI.TO.MarketBook MarketBook = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookFigure = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookWintheToss = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookWintheToss0 = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookWintheToss1 = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookWintheToss2 = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookForProfitandloss = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookForProfitandlossToWinTheToss = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookForProfitandlossToWinTheToss0 = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookForProfitandlossToWinTheToss1 = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookForProfitandlossToWinTheToss2 = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookForProfitandlossToFigure = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookForProfitandlossAgent = new ExternalAPI.TO.MarketBook();


        private void TmrUpdateLiabalities_Tick(object sender, EventArgs e)
        {
            // UpdateLiabaliteies();
            UpdateLineMarketsDataIN(false);
        }

#pragma warning disable CS0169 // The field 'MarketWindowUserControl.cancellationToken' is never used
        CancellationToken cancellationToken;
#pragma warning restore CS0169 // The field 'MarketWindowUserControl.cancellationToken' is never used
        //public async Task DoOperationsConcurrentlyAsyncForOdds(CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        Task.Factory.StartNew(async () =>
        //        {
        //            Task[] tasks = new Task[1];
        //            tasks[0] = DoWorkAsyncGetDataForGetOdds();




        //        }).ContinueWith(async task =>
        //        {
        //            await Task.Delay(50, cancellationToken);
        //            //this code runs back on the UI thread
        //            DoOperationsConcurrentlyAsyncForOdds(cancellationToken);
        //        }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        //        //await Task.Run(async () =>
        //        //{
        //        //    while (true)
        //        //    {
        //        //        Task[] tasks = new Task[3];
        //        //        tasks[0] = DoWorkAsyncGetDataForFancy();
        //        //        tasks[1] = DoWorkAsyncGetScores();
        //        //        await Task.Delay(200, cancellationToken);

        //        //    }
        //        //});
        //    }

        //    catch (System.Exception ex)
        //    {

        //    }
        //}

        //public async Task DoOperationsConcurrentlyAsync(CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        Task.Factory.StartNew(() =>
        //        {
        //            Task[] tasks = new Task[3];
        //            tasks[0] = DoWorkAsyncGetDataForFancy();
        //            tasks[1] = DoWorkAsyncGetScores();



        //        }).ContinueWith(async task =>
        //        {
        //            await Task.Delay(200, cancellationToken);
        //            //this code runs back on the UI thread
        //            DoOperationsConcurrentlyAsync(cancellationToken);
        //        }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
        //        //await Task.Run(async () =>
        //        //{
        //        //    while (true)
        //        //    {
        //        //        Task[] tasks = new Task[3];
        //        //        tasks[0] = DoWorkAsyncGetDataForFancy();
        //        //        tasks[1] = DoWorkAsyncGetScores();
        //        //        await Task.Delay(200, cancellationToken);

        //        //    }
        //        //});
        //    }

        //    catch (System.Exception ex)
        //    {

        //    }
        //}




        public void updatetotalmatched()
        {


        }

        public void updatelabelstimerandstatus()
        {
            if (txtCurrentLiabality.Content != null)
            {
                if (txtCurrentLiabality.Content.ToString() != CurrentLiabality.ToString("N0"))
                {
                    txtCurrentLiabality.Content = CurrentLiabality.ToString("N0");
                }
            }
            else
            {
                txtCurrentLiabality.Content = CurrentLiabality.ToString("N0");
            }

            if (MarketBook.MarketStatusstr == "ACTIVE" || MarketBook.MarketStatusstr == "Active")
            {
                if (lblMarketStatus.Content != null)
                {
                    if (lblMarketStatus.Content.ToString() != "GOING LIVE")
                    {
                        lblMarketStatus.Content = "GOING LIVE";
                    }
                }
                else
                {
                    lblMarketStatus.Content = "GOING LIVE";
                }
            }
            else
            {
                // stkpnlTowintheToss.Visibility = Visibility.Collapsed;
                if (lblMarketStatus.Content != null)
                {
                    if (lblMarketStatus.Content.ToString() != MarketBook.MarketStatusstr)
                    {
                        lblMarketStatus.Content = MarketBook.MarketStatusstr;
                    }
                }
                else
                {
                    lblMarketStatus.Content = MarketBook.MarketStatusstr;
                }

            }


            lblMarketTime.Content = MarketBook.OpenDate;
            if (lblMarketTime.Content.ToString().Contains("-"))
            {
                if (lblMarketTime.Foreground != Brushes.Red)
                {
                    lblMarketTime.Foreground = Brushes.Red;
                }

            }

            //totalmatchedvalue.Content = MarketBook.TotalMatched;

            if (popupBetslip.IsOpen == true && CurrentMarketBookId == MarketBook.MarketId)
            {
                currmarketbookforbet = MarketBook;
                var selectedrunner = MarketBook.Runners.Where(item => item.SelectionId == SelectionID).FirstOrDefault();
                if (LoggedinUserDetail.isInserting == false)
                {
                    lblBetSlipBack.Content = selectedrunner.ExchangePrices.AvailableToBack[0].Price.ToString();
                    // lblBetslipBackOdd1.Content = selectedrunner.ExchangePrices.AvailableToBack[1].Price.ToString();
                    // lblBetslipBackOdd2.Content = selectedrunner.ExchangePrices.AvailableToBack[2].Price.ToString();
                    lblBetslipBackSize0.Content = selectedrunner.ExchangePrices.AvailableToBack[0].Size.ToString();
                    //lblBetslipBackSize1.Content = selectedrunner.ExchangePrices.AvailableToBack[1].Size.ToString();
                    // lblBetslipBackSize2.Content = selectedrunner.ExchangePrices.AvailableToBack[2].Size.ToString();

                    lblBetSlipLay.Content = selectedrunner.ExchangePrices.AvailableToLay[0].Price.ToString();
                    // lblBetslipLayOdd1.Content = selectedrunner.ExchangePrices.AvailableToLay[1].Price.ToString();
                    // lblBetslipLayOdd2.Content = selectedrunner.ExchangePrices.AvailableToLay[2].Price.ToString();
                    lblBetslipLaySize0.Content = selectedrunner.ExchangePrices.AvailableToLay[0].Size.ToString();
                    // lblBetslipLaySize1.Content = selectedrunner.ExchangePrices.AvailableToLay[1].Size.ToString();
                    // lblBetslipLaySize2.Content = selectedrunner.ExchangePrices.AvailableToLay[2].Size.ToString();
                    if (BetType == "back")
                    {
                        Betslipsize.Text = selectedrunner.ExchangePrices.AvailableToBack[0].Size.ToString();
                    }
                    else
                    {
                        Betslipsize.Text = selectedrunner.ExchangePrices.AvailableToLay[0].Size.ToString();
                    }
                }

            }

            if (popupBetslip.IsOpen == true && CurrentMarketBookId == MarketBookForProfitandlossToWinTheToss.MarketId)
            {
                currmarketbookforbet = MarketBookWintheToss;
                var selectedrunner = MarketBookWintheToss.Runners.Where(item => item.SelectionId == SelectionID).FirstOrDefault();
                if (LoggedinUserDetail.isInserting == false)
                {
                    lblBetSlipBack.Content = selectedrunner.ExchangePrices.AvailableToBack[0].Price.ToString();
                    // lblBetslipBackOdd1.Content = selectedrunner.ExchangePrices.AvailableToBack[1].Price.ToString();
                    // lblBetslipBackOdd2.Content = selectedrunner.ExchangePrices.AvailableToBack[2].Price.ToString();
                    lblBetslipBackSize0.Content = selectedrunner.ExchangePrices.AvailableToBack[0].Size.ToString();
                    //lblBetslipBackSize1.Content = selectedrunner.ExchangePrices.AvailableToBack[1].Size.ToString();
                    // lblBetslipBackSize2.Content = selectedrunner.ExchangePrices.AvailableToBack[2].Size.ToString();

                    lblBetSlipLay.Content = selectedrunner.ExchangePrices.AvailableToLay[0].Price.ToString();
                    // lblBetslipLayOdd1.Content = selectedrunner.ExchangePrices.AvailableToLay[1].Price.ToString();
                    // lblBetslipLayOdd2.Content = selectedrunner.ExchangePrices.AvailableToLay[2].Price.ToString();
                    lblBetslipLaySize0.Content = selectedrunner.ExchangePrices.AvailableToLay[0].Size.ToString();
                    // lblBetslipLaySize1.Content = selectedrunner.ExchangePrices.AvailableToLay[1].Size.ToString();
                    // lblBetslipLaySize2.Content = selectedrunner.ExchangePrices.AvailableToLay[2].Size.ToString();
                    if (BetType == "back")
                    {
                        Betslipsize.Text = selectedrunner.ExchangePrices.AvailableToBack[0].Size.ToString();
                    }
                    else
                    {
                        Betslipsize.Text = selectedrunner.ExchangePrices.AvailableToLay[0].Size.ToString();
                    }
                }

            }
            if (popupBetslip.IsOpen == true && CurrentMarketBookId == MarketBookForProfitandlossToWinTheToss0.MarketId)
            {
                currmarketbookforbet = MarketBookWintheToss0;
                var selectedrunner = MarketBookWintheToss0.Runners.Where(item => item.SelectionId == SelectionID).FirstOrDefault();
                if (LoggedinUserDetail.isInserting == false)
                {
                    lblBetSlipBack.Content = selectedrunner.ExchangePrices.AvailableToBack[0].Price.ToString();
                    // lblBetslipBackOdd1.Content = selectedrunner.ExchangePrices.AvailableToBack[1].Price.ToString();
                    // lblBetslipBackOdd2.Content = selectedrunner.ExchangePrices.AvailableToBack[2].Price.ToString();
                    lblBetslipBackSize0.Content = selectedrunner.ExchangePrices.AvailableToBack[0].Size.ToString();
                    //lblBetslipBackSize1.Content = selectedrunner.ExchangePrices.AvailableToBack[1].Size.ToString();
                    // lblBetslipBackSize2.Content = selectedrunner.ExchangePrices.AvailableToBack[2].Size.ToString();

                    lblBetSlipLay.Content = selectedrunner.ExchangePrices.AvailableToLay[0].Price.ToString();
                    // lblBetslipLayOdd1.Content = selectedrunner.ExchangePrices.AvailableToLay[1].Price.ToString();
                    // lblBetslipLayOdd2.Content = selectedrunner.ExchangePrices.AvailableToLay[2].Price.ToString();
                    lblBetslipLaySize0.Content = selectedrunner.ExchangePrices.AvailableToLay[0].Size.ToString();
                    // lblBetslipLaySize1.Content = selectedrunner.ExchangePrices.AvailableToLay[1].Size.ToString();
                    // lblBetslipLaySize2.Content = selectedrunner.ExchangePrices.AvailableToLay[2].Size.ToString();
                    if (BetType == "back")
                    {
                        Betslipsize.Text = selectedrunner.ExchangePrices.AvailableToBack[0].Size.ToString();
                    }
                    else
                    {
                        Betslipsize.Text = selectedrunner.ExchangePrices.AvailableToLay[0].Size.ToString();
                    }
                }

            }

            if (lblMarketStatus.Content.ToString() == "IN-PLAY")
            {
                // statusimg.Source = new BitmapImage(new Uri("dark-green-marker.png", UriKind.Relative));

                statusimg.Visibility = Visibility.Visible;
                statusimg1.Visibility = Visibility.Collapsed;
            }
            else
            {

                if (lblMarketStatus.Content.ToString() == "SUSPENDED")
                {
                    //statusimg.Source = new BitmapImage(new Uri("dark-orange-marker.png", UriKind.Relative));

                    statusimg.Visibility = Visibility.Collapsed;
                    statusimg1.Visibility = Visibility.Visible;
                }
                else
                {
                    statusimg.Visibility = Visibility.Collapsed;
                    statusimg1.Visibility = Visibility.Collapsed;

                }
            }

        }
        public string Insertfancy(string EventID, string MarketBookID)
        {
            List<string> n = new List<string>();
            List<ExternalAPI.TO.MarketBookForindianFancy> LastloadedLinMarkets2 = new List<ExternalAPI.TO.MarketBookForindianFancy>();
            LastloadedLinMarkets2.Clear();
            LastloadedLinMarkets2.Add(objBettingClient.GetMarketDatabyIDIndianFancy(EventID, MarketBookID));
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                // objUsersServiceCleint.InsertIndainFancy(LastloadedLinMarkets2.ToArray(), "pass");
                return "True";
            }
            return "";
        }

        public void UpdateAllData()
        {
            try
            {
                // UpdateFancyData();
                UpdateLiabaliteies();

                GetRunnersDataSource(MarketBook.Runners.ToList(), MarketBook);
                UpdateUserBetsData();
                UpdateUserBetsDataUnMatched();
                if (MarketBookWintheToss.Runners != null)
                {
                    lbltoos.Visibility = Visibility.Visible;
                    UpdateLiabaliteiesToWintheToss();
                    GetRunnersDataSourceToWintheToss(MarketBookWintheToss.Runners, MarketBookWintheToss);
                }
                if (MarketBook.MainSportsname == "Soccer")
                {
                    lbltoos.Visibility = Visibility.Collapsed;
                    GetRunnersDataSourceToWintheToss(MarketBookWintheToss.Runners, MarketBookWintheToss);
                }

                if (LoggedinUserDetail.GetUserTypeID() != 1)
                {
                    UpdateGridDatabymarketFavouriteandOtherlabels();
                }
                updatelabelstimerandstatus();

            }
            catch (System.Exception ex)
            {

            }
        }


      


        private void TmrUpdateMarket_Tick(object sender, EventArgs e)
        {

            try
            {
                UpdateAllData();
                //SetWindowHeight();
                if (MarketBook.MainSportsname == "Cricket" && MarketBook.MarketBookName.Contains("Match Odds"))
                {
                   // UpdateLineMarketsData(false);
                    if (LoggedinUserDetail.isTVShown == true)
                    {                     
                        CreateScoreCard(MarketBook.EventID);                      
                    }
                }
                //if (popupBetslip.IsOpen == true && runnerscount == "1")
                //{
                //    var currmarketbook = LastloadedLinMarkets.Where(item => item.MarketId == CurrentMarketBookId).FirstOrDefault();
                //    currmarketbookforbet = currmarketbook;
                //    lblBetSlipBack.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price.ToString();
                //    //lblBetslipBackOdd1.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[1].Price.ToString();
                //    // lblBetslipBackOdd2.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[2].Price.ToString();
                //    lblBetslipBackSize0.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[0].Size.ToString();
                //    // lblBetslipBackSize1.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[1].Size.ToString();
                //    // lblBetslipBackSize2.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[2].Size.ToString();

                //    lblBetSlipLay.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[0].Price.ToString();
                //    // lblBetslipLayOdd1.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[1].Price.ToString();
                //    // lblBetslipLayOdd2.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[2].Price.ToString();
                //    lblBetslipLaySize0.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[0].Size.ToString();
                //    // lblBetslipLaySize1.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[1].Size.ToString();
                //    // lblBetslipLaySize2.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[2].Size.ToString();
                //    if (BetType == "back")
                //    {
                //        Betslipsize.Text = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[0].Size.ToString();
                //    }
                //    else
                //    {
                //        Betslipsize.Text = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[0].Size.ToString();
                //    }
                //}

            }
            catch (System.Exception ex)
            {

            }

        }

        public void GetRunnersDataSource(List<ExternalAPI.TO.Runner> runners, MarketBook objMarketBook)
        {
            try
            {
                if (lstMarketBookRunners == null)
                {

                    lstMarketBookRunners = new ObservableCollection<MarketBookShow>();
                    lstMarketBookRunners.Clear();

                    // runners = runners.OrderBy(item => item.StatusStr).ToList();
                    // ObservableCollection<MarketBookShow> lstMArketbookshow = new ObservableCollection<MarketBookShow>();
                    foreach (var item in runners)
                    {
                        MarketBookShow objmarketbookshow = new MarketBookShow();
                        objmarketbookshow.Selection = item.RunnerName.ToString().ToUpper();
                        objmarketbookshow.SelectionID = item.SelectionId;
                        objmarketbookshow.Price = item.LastPriceTraded.ToString();
                        objmarketbookshow.PL = item.ProfitandLoss.ToString();
                        objmarketbookshow.RunnerStatusstr = item.StatusStr;
                        objmarketbookshow.JockeyName = item.JockeyName;

                        try
                        {

                            objmarketbookshow.ImageURL = item.WearingURL.ToString().Replace(".betfair.com", ".cdnbf.net");
                        }
                        catch (System.Exception ex)
                        {

                        }

                        objmarketbookshow.Clothnumber = item.Clothnumber;
                        objmarketbookshow.StallDraw = item.StallDraw;
                        objmarketbookshow.JockeyHeading = "";
                        if (MarketBook.MainSportsname.Contains("Horse Racing"))
                        {
                            DGVMarket.Columns[4].Header = "JOCKEY";
                            objmarketbookshow.JockeyHeading = "JOCKEY";
                        }


                        //
                        objmarketbookshow.CategoryName = objMarketBook.MainSportsname;
                        objmarketbookshow.MarketbooknameBet = objMarketBook.MarketBookName;
                        objmarketbookshow.Marketstatusstr = objMarketBook.MarketStatusstr;
                        objmarketbookshow.BettingAllowed = objMarketBook.BettingAllowed;
                        objmarketbookshow.OpenDate = objMarketBook.OpenDate;
                        objmarketbookshow.runnerscount = objMarketBook.Runners.Count.ToString();
                        objmarketbookshow.CurrentMarketBookId = objMarketBook.MarketId;
                        //objmarketbookshow.totalmatched = item.TotalMatched.ToString();
                        //
                        if (objmarketbookshow.StallDraw != "Not" && objmarketbookshow.StallDraw != "" && objmarketbookshow.StallDraw != null)
                        {
                            objmarketbookshow.StallDraw = "(" + objmarketbookshow.StallDraw + ")";
                        }


                        if (item.ExchangePrices.AvailableToBack.Count == 3)
                        {
                            objmarketbookshow.Backprice2 = item.ExchangePrices.AvailableToBack[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].Price.ToString();
                            objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize2 = item.ExchangePrices.AvailableToBack[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].SizeStr.ToString();
                            objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 2)
                        {
                            objmarketbookshow.Backprice2 = "";
                            objmarketbookshow.Backsize2 = "";
                            objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 1)
                        {
                            objmarketbookshow.Backprice2 = "";
                            objmarketbookshow.Backprice1 = "";
                            objmarketbookshow.Backsize2 = "";
                            objmarketbookshow.Backsize1 = "";
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 3)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                            objmarketbookshow.Layprice2 = item.ExchangePrices.AvailableToLay[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                            objmarketbookshow.Laysize2 = item.ExchangePrices.AvailableToLay[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 2)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                            objmarketbookshow.Layprice2 = "";
                            objmarketbookshow.Laysize2 = "";

                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 1)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Layprice1 = "";
                            objmarketbookshow.Layprice2 = "";
                            objmarketbookshow.Laysize1 = "";
                            objmarketbookshow.Laysize2 = "";

                        }
                        lstMarketBookRunners.Add(objmarketbookshow);
                        // lstMArketbookshow.Add(objmarketbookshow);

                    }
                    if (MarketBook.Runners.Count > 1)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            lstMarketBookRunners[i].isSelectedForLK = true;
                        }
                    }


                }
                else
                {
                    foreach (var item in runners)
                    {
                        MarketBookShow objmarketbookshow = lstMarketBookRunners.Where(item1 => item1.SelectionID == item.SelectionId).FirstOrDefault();

                        //objmarketbookshow.Selection = item.RunnerName;
                        //objmarketbookshow.SelectionID = item.SelectionId;
                        try
                        {

                            if (MarketBook.MarketStatusstr != "CLOSED" && MarketBook.MarketStatusstr != "SUSPENDED")
                            {


                                if (objmarketbookshow.SelectionID == MarketBook.FavoriteID)
                                {
                                    objmarketbookshow.isFav = true;
                                }
                                else
                                {
                                    objmarketbookshow.isFav = false;

                                }
                            }
                            else
                            {
                                objmarketbookshow.isFav = false;
                            }
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {

                        }
                        objmarketbookshow.Price = item.LastPriceTraded.ToString();
                        try
                        {
                            objmarketbookshow.OpenDate = objMarketBook.OpenDate;
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {

                        }
                        try
                        {


                            objmarketbookshow.PL = CurrentMarketProfitandLoss[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().ProfitandLoss.ToString();
                            objmarketbookshow.Loss = CurrentMarketProfitandLoss[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().Loss.ToString();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {

                        }
                        objmarketbookshow.RunnerStatusstr = item.StatusStr;
                        objmarketbookshow.Marketstatusstr = objMarketBook.MarketStatusstr;
                        if (objmarketbookshow.isSelectedForLK == false)
                        {
                            objmarketbookshow.Average = "";
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 3)
                        {
                            objmarketbookshow.Backprice2 = item.ExchangePrices.AvailableToBack[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].Price.ToString();
                            objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize2 = item.ExchangePrices.AvailableToBack[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].SizeStr.ToString();
                            objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 2)
                        {
                            objmarketbookshow.Backprice2 = "";
                            objmarketbookshow.Backsize2 = "";
                            objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 1)
                        {
                            objmarketbookshow.Backprice2 = "";
                            objmarketbookshow.Backprice1 = "";
                            objmarketbookshow.Backsize2 = "";
                            objmarketbookshow.Backsize1 = "";
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 3)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                            objmarketbookshow.Layprice2 = item.ExchangePrices.AvailableToLay[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                            objmarketbookshow.Laysize2 = item.ExchangePrices.AvailableToLay[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 2)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                            objmarketbookshow.Layprice2 = "";
                            objmarketbookshow.Laysize2 = "";

                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 1)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Layprice1 = "";
                            objmarketbookshow.Layprice2 = "";
                            objmarketbookshow.Laysize1 = "";
                            objmarketbookshow.Laysize2 = "";

                        }
                        // lstMarketBookRunners.Add(objmarketbookshow);
                        // lstMArketbookshow.Add(objmarketbookshow);

                    }
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

            //  return lstMArketbookshow;
        }


        // public void GetRunnersDataSourceFigure(List<ExternalAPI.TO.Runner> runners,string MarketCatalogueID,string MarketCatalogueName,bool BettingAllowed)
        public void GetRunnersDataSourceFigure(List<ExternalAPI.TO.Runner> runners, MarketBook obj)
        {
            try
            {
                if (runners.Count > 0)
                {
                    lstMarketBookRunnersFigure = new ObservableCollection<MarketBookShow>();
                    lstMarketBookRunnersFigure.Clear();

                    // runners = runners.OrderBy(item => item.StatusStr).ToList();
                    // ObservableCollection<MarketBookShow> lstMArketbookshow = new ObservableCollection<MarketBookShow>();
                    foreach (var item in runners)
                    {
                        MarketBookShow objmarketbookshow = new MarketBookShow();
                        objmarketbookshow.Selection = obj.MarketBookName + item.RunnerName.ToString().ToUpper();
                        objmarketbookshow.SelectionID = item.SelectionId;
                        // objmarketbookshow.isSelectedForLK = obj.BettingAllowed;
                        if (LoggedinUserDetail.GetUserTypeID() == 3)
                        {
                            List<bftradeline.Models.UserBets> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentUserBets.ToList().Where(item1 => item1.MarketBookID == obj.MarketId).ToList();
                            if (lstCurrentBetsAdmin.Count > 0)
                            {
                                try
                                {
                                    objmarketbookshow.PL = CurrentMarketProfitandLossToFigre[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().ProfitandLoss.ToString();
                                    objmarketbookshow.Loss = CurrentMarketProfitandLossToFigre[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().Loss.ToString();
                                }
                                catch (System.Exception ex)
                                {
                                    objmarketbookshow.PL = "0";
                                    objmarketbookshow.Loss = "0";
                                }
                            }
                            else
                            {
                                objmarketbookshow.PL = "0";
                                objmarketbookshow.Loss = "0";
                            }
                        }
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            List<bftradeline.Models.UserBetsforAgent> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAgentBets.ToList().Where(item1 => item1.MarketBookID == obj.MarketId).ToList();
                            if (lstCurrentBetsAdmin.Count > 0)
                            {
                                try
                                {
                                    objmarketbookshow.PL = CurrentMarketProfitandLossToFigre[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().ProfitandLoss.ToString();
                                    objmarketbookshow.Loss = CurrentMarketProfitandLossToFigre[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().Loss.ToString();
                                }
                                catch (System.Exception ex)
                                {
                                    objmarketbookshow.PL = "0";
                                    objmarketbookshow.Loss = "0";

                                }
                            }
                            else
                            {
                                objmarketbookshow.PL = "0";
                                objmarketbookshow.Loss = "0";
                            }
                        }

                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            List<bftradeline.Models.UserBetsForAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item1 => item1.MarketBookID == obj.MarketId).ToList();
                            if (lstCurrentBetsAdmin.Count > 0)
                            {
                                try
                                {
                                    objmarketbookshow.PL = CurrentMarketProfitandLossToFigre[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().ProfitandLoss.ToString();
                                    objmarketbookshow.Loss = CurrentMarketProfitandLossToFigre[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().Loss.ToString();
                                }
                                catch (System.Exception ex)
                                {
                                    objmarketbookshow.PL = "0";
                                    objmarketbookshow.Loss = "0";

                                }
                            }
                            else
                            {
                                objmarketbookshow.PL = "0";
                                objmarketbookshow.Loss = "0";
                            }
                        }

                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                        {
                            List<bftradeline.Models.UserBetsforSuper> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentSuperBets.ToList().Where(item1 => item1.MarketBookID == obj.MarketId).ToList();
                            if (lstCurrentBetsAdmin.Count > 0)
                            {
                                try
                                {
                                    objmarketbookshow.PL = CurrentMarketProfitandLossToFigre[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().ProfitandLoss.ToString();
                                    objmarketbookshow.Loss = CurrentMarketProfitandLossToFigre[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().Loss.ToString();
                                }
                                catch (System.Exception ex)
                                {
                                    objmarketbookshow.PL = "0";
                                    objmarketbookshow.Loss = "0";
                                }
                            }
                            else
                            {
                                objmarketbookshow.PL = "0";
                                objmarketbookshow.Loss = "0";
                            }
                        }

                        objmarketbookshow.RunnerStatusstr = item.StatusStr;
                        objmarketbookshow.JockeyName = item.JockeyName;
                        objmarketbookshow.CategoryName = "Cricket";
                        objmarketbookshow.MarketbooknameBet = obj.MarketBookName;
                        objmarketbookshow.Marketstatusstr = "IN-PLAY";
                        objmarketbookshow.BettingAllowed = obj.BettingAllowed;
                        //objmarketbookshow.OpenDate = objMarketBook.OpenDate;
                        objmarketbookshow.runnerscount = runners.Count.ToString();
                        objmarketbookshow.CurrentMarketBookId = obj.MarketId;
                        //objmarketbookshow.totalmatched = item.TotalMatched.ToString();


                        if (item.ExchangePrices.AvailableToBack.Count == 1)
                        {

                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Size.ToString();
                        }

                        if (item.ExchangePrices.AvailableToLay.Count == 1)
                        {

                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Size.ToString();
                        }

                        lstMarketBookRunnersFigure.Add(objmarketbookshow);
                        // lstMArketbookshow.Add(objmarketbookshow);

                    }

                }
            }
            catch (System.Exception ex)
            {

            }

            //  return lstMArketbookshow;
        }
        public void GetRunnersDataSourceToWintheToss(List<ExternalAPI.TO.Runner> runners, MarketBook objMarketBook)
        {
            try
            {
                if (lstMarketBookRunnersToWintheToss == null || lstMarketBookRunnersToWintheToss.Count == 0)
                {

                    lstMarketBookRunnersToWintheToss = new ObservableCollection<MarketBookShow>();
                    lstMarketBookRunnersToWintheToss.Clear();

                    // runners = runners.OrderBy(item => item.StatusStr).ToList();
                    // ObservableCollection<MarketBookShow> lstMArketbookshow = new ObservableCollection<MarketBookShow>();
                    foreach (var item in runners)
                    {
                        MarketBookShow objmarketbookshow = new MarketBookShow();
                        objmarketbookshow.Selection = item.RunnerName.ToString().ToUpper();
                        objmarketbookshow.SelectionID = item.SelectionId;
                        objmarketbookshow.Price = item.LastPriceTraded.ToString();
                        objmarketbookshow.PL = item.ProfitandLoss.ToString();
                        objmarketbookshow.RunnerStatusstr = item.StatusStr;
                        //
                        objmarketbookshow.CategoryName = objMarketBook.MainSportsname;
                        objmarketbookshow.MarketbooknameBet = objMarketBook.MarketBookName;
                        objmarketbookshow.Marketstatusstr = objMarketBook.MarketStatusstr;
                        objmarketbookshow.BettingAllowed = objMarketBook.BettingAllowed;
                        objmarketbookshow.OpenDate = objMarketBook.OpenDate;
                        objmarketbookshow.runnerscount = objMarketBook.Runners.Count.ToString();
                        objmarketbookshow.CurrentMarketBookId = objMarketBook.MarketId;
                        //


                        if (item.ExchangePrices.AvailableToBack.Count == 3)
                        {
                            objmarketbookshow.Backprice2 = item.ExchangePrices.AvailableToBack[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].Price.ToString();
                            objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize2 = item.ExchangePrices.AvailableToBack[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].SizeStr.ToString();
                            objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 2)
                        {
                            objmarketbookshow.Backprice2 = "";
                            objmarketbookshow.Backsize2 = "";
                            objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 1)
                        {
                            objmarketbookshow.Backprice2 = "";
                            objmarketbookshow.Backprice1 = "";
                            objmarketbookshow.Backsize2 = "";
                            objmarketbookshow.Backsize1 = "";
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 3)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                            objmarketbookshow.Layprice2 = item.ExchangePrices.AvailableToLay[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                            objmarketbookshow.Laysize2 = item.ExchangePrices.AvailableToLay[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 2)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                            objmarketbookshow.Layprice2 = "";
                            objmarketbookshow.Laysize2 = "";

                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 1)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Layprice1 = "";
                            objmarketbookshow.Layprice2 = "";
                            objmarketbookshow.Laysize1 = "";
                            objmarketbookshow.Laysize2 = "";

                        }
                        lstMarketBookRunnersToWintheToss.Add(objmarketbookshow);
                        // lstMArketbookshow.Add(objmarketbookshow);

                    }
                    if (MarketBookForProfitandlossToWinTheToss.Runners.Count > 1)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            lstMarketBookRunnersToWintheToss[i].isSelectedForLK = true;
                        }
                    }


                }
                else
                {
                    if (MarketBookWintheToss.MarketStatusstr.ToUpper() != "CLOSED")
                    {
                        stkpnlTowintheToss.Visibility = Visibility.Visible;

                    }
                    foreach (var item in runners)
                    {
                        MarketBookShow objmarketbookshow = lstMarketBookRunnersToWintheToss.Where(item1 => item1.SelectionID == item.SelectionId).FirstOrDefault();

                        //objmarketbookshow.Selection = item.RunnerName;
                        //objmarketbookshow.SelectionID = item.SelectionId;
                        try
                        {

                            if (MarketBookWintheToss.MarketStatusstr.ToUpper() != "CLOSED" && MarketBookWintheToss.MarketStatusstr.ToUpper() != "SUSPENDED")
                            {


                                if (objmarketbookshow.SelectionID == MarketBook.FavoriteID)
                                {
                                    objmarketbookshow.isFav = true;
                                }
                                else
                                {
                                    objmarketbookshow.isFav = false;

                                }
                            }
                            else
                            {
                                objmarketbookshow.isFav = false;
                            }
                        }
                        catch (System.Exception ex)
                        {

                        }
                        objmarketbookshow.Price = item.LastPriceTraded.ToString();
                        try
                        {
                            objmarketbookshow.OpenDate = objMarketBook.OpenDate;
                        }
                        catch (System.Exception ex)
                        {

                        }
                        try
                        {


                            objmarketbookshow.PL = CurrentMarketProfitandLossToWinTheToss[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().ProfitandLoss.ToString();
                            objmarketbookshow.Loss = CurrentMarketProfitandLossToWinTheToss[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().Loss.ToString();
                        }

                        catch (System.Exception ex)

                        {

                        }
                        objmarketbookshow.RunnerStatusstr = item.StatusStr;
                        objmarketbookshow.Marketstatusstr = objMarketBook.MarketStatusstr;
                        if (objmarketbookshow.isSelectedForLK == false)
                        {
                            objmarketbookshow.Average = "";
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 3)
                        {
                            objmarketbookshow.Backprice2 = item.ExchangePrices.AvailableToBack[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].Price.ToString();
                            objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize2 = item.ExchangePrices.AvailableToBack[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].SizeStr.ToString();
                            objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 2)
                        {
                            objmarketbookshow.Backprice2 = "";
                            objmarketbookshow.Backsize2 = "";
                            objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 1)
                        {
                            objmarketbookshow.Backprice2 = "";
                            objmarketbookshow.Backprice1 = "";
                            objmarketbookshow.Backsize2 = "";
                            objmarketbookshow.Backsize1 = "";
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 3)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                            objmarketbookshow.Layprice2 = item.ExchangePrices.AvailableToLay[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                            objmarketbookshow.Laysize2 = item.ExchangePrices.AvailableToLay[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 2)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                            objmarketbookshow.Layprice2 = "";
                            objmarketbookshow.Laysize2 = "";

                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 1)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Layprice1 = "";
                            objmarketbookshow.Layprice2 = "";
                            objmarketbookshow.Laysize1 = "";
                            objmarketbookshow.Laysize2 = "";

                        }
                        // lstMarketBookRunners.Add(objmarketbookshow);
                        // lstMArketbookshow.Add(objmarketbookshow);

                    }
                }

            }
            catch (System.Exception ex)
            {

            }

            //  return lstMArketbookshow;
        }
        public void GetRunnersDataSourceToWintheToss0(List<ExternalAPI.TO.Runner> runners, MarketBook objMarketBook)
        {
            try
            {
                if (lstMarketBookRunnersToWintheToss0 == null || lstMarketBookRunnersToWintheToss0.Count == 0)
                {
                    lstMarketBookRunnersToWintheToss0 = new ObservableCollection<MarketBookShow>();
                    lstMarketBookRunnersToWintheToss0.Clear();

                    // runners = runners.OrderBy(item => item.StatusStr).ToList();
                    // ObservableCollection<MarketBookShow> lstMArketbookshow = new ObservableCollection<MarketBookShow>();
                    foreach (var item in runners)
                    {
                        MarketBookShow objmarketbookshow = new MarketBookShow();
                        objmarketbookshow.Selection = item.RunnerName.ToString().ToUpper();
                        objmarketbookshow.SelectionID = item.SelectionId;
                        objmarketbookshow.Price = item.LastPriceTraded.ToString();
                        objmarketbookshow.PL = item.ProfitandLoss.ToString();
                        objmarketbookshow.RunnerStatusstr = item.StatusStr;
                        //
                        objmarketbookshow.CategoryName = objMarketBook.MainSportsname;
                        objmarketbookshow.MarketbooknameBet = objMarketBook.MarketBookName;
                        objmarketbookshow.Marketstatusstr = objMarketBook.MarketStatusstr;
                        objmarketbookshow.BettingAllowed = objMarketBook.BettingAllowed;
                        objmarketbookshow.OpenDate = objMarketBook.OpenDate;
                        objmarketbookshow.runnerscount = objMarketBook.Runners.Count.ToString();
                        objmarketbookshow.CurrentMarketBookId = objMarketBook.MarketId;
                        //
                        if (item.ExchangePrices.AvailableToBack.Count == 3)
                        {
                            objmarketbookshow.Backprice2 = item.ExchangePrices.AvailableToBack[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].Price.ToString();
                            objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize2 = item.ExchangePrices.AvailableToBack[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].SizeStr.ToString();
                            objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 2)
                        {
                            objmarketbookshow.Backprice2 = "";
                            objmarketbookshow.Backsize2 = "";
                            objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 1)
                        {
                            objmarketbookshow.Backprice2 = "";
                            objmarketbookshow.Backprice1 = "";
                            objmarketbookshow.Backsize2 = "";
                            objmarketbookshow.Backsize1 = "";
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 3)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                            objmarketbookshow.Layprice2 = item.ExchangePrices.AvailableToLay[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                            objmarketbookshow.Laysize2 = item.ExchangePrices.AvailableToLay[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 2)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                            objmarketbookshow.Layprice2 = "";
                            objmarketbookshow.Laysize2 = "";

                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 1)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Layprice1 = "";
                            objmarketbookshow.Layprice2 = "";
                            objmarketbookshow.Laysize1 = "";
                            objmarketbookshow.Laysize2 = "";
                        }
                        lstMarketBookRunnersToWintheToss0.Add(objmarketbookshow);
                        // lstMArketbookshow.Add(objmarketbookshow);

                    }
                    if (MarketBookForProfitandlossToWinTheToss0.Runners.Count > 1)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            lstMarketBookRunnersToWintheToss0[i].isSelectedForLK = true;
                        }
                    }
                }
                else
                {
                    if (MarketBookWintheToss0.MarketStatusstr.ToUpper() != "CLOSED")
                    {
                        //stkpnlSoccer.Visibility = Visibility.Visible;
                    }
                    foreach (var item in runners)
                    {
                        MarketBookShow objmarketbookshow = lstMarketBookRunnersToWintheToss0.Where(item1 => item1.SelectionID == item.SelectionId).FirstOrDefault();

                        //objmarketbookshow.Selection = item.RunnerName;
                        //objmarketbookshow.SelectionID = item.SelectionId;
                        try
                        {

                            if (MarketBookWintheToss0.MarketStatusstr.ToUpper() != "CLOSED" && MarketBookWintheToss0.MarketStatusstr.ToUpper() != "SUSPENDED")
                            {
                                if (objmarketbookshow.SelectionID == MarketBook.FavoriteID)
                                {
                                    objmarketbookshow.isFav = true;
                                }
                                else
                                {
                                    objmarketbookshow.isFav = false;
                                }
                            }
                            else
                            {
                                objmarketbookshow.isFav = false;
                            }
                        }

                        catch (System.Exception ex)
                        {

                        }
                        objmarketbookshow.Price = item.LastPriceTraded.ToString();
                        try
                        {
                            objmarketbookshow.OpenDate = objMarketBook.OpenDate;
                        }

                        catch (System.Exception ex)
                        {

                        }
                        try
                        {
                            objmarketbookshow.PL = CurrentMarketProfitandLossToWinTheToss[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().ProfitandLoss.ToString();
                            objmarketbookshow.Loss = CurrentMarketProfitandLossToWinTheToss[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().Loss.ToString();
                        }
                        catch (System.Exception ex)
                        {

                        }
                        objmarketbookshow.RunnerStatusstr = item.StatusStr;
                        objmarketbookshow.Marketstatusstr = objMarketBook.MarketStatusstr;
                        if (objmarketbookshow.isSelectedForLK == false)
                        {
                            objmarketbookshow.Average = "";
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 3)
                        {
                            objmarketbookshow.Backprice2 = item.ExchangePrices.AvailableToBack[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].Price.ToString();
                            objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize2 = item.ExchangePrices.AvailableToBack[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].SizeStr.ToString();
                            objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 2)
                        {
                            objmarketbookshow.Backprice2 = "";
                            objmarketbookshow.Backsize2 = "";
                            objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToBack.Count == 1)
                        {
                            objmarketbookshow.Backprice2 = "";
                            objmarketbookshow.Backprice1 = "";
                            objmarketbookshow.Backsize2 = "";
                            objmarketbookshow.Backsize1 = "";
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 3)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                            objmarketbookshow.Layprice2 = item.ExchangePrices.AvailableToLay[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                            objmarketbookshow.Laysize2 = item.ExchangePrices.AvailableToLay[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].SizeStr.ToString();
                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 2)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                            objmarketbookshow.Layprice2 = "";
                            objmarketbookshow.Laysize2 = "";

                        }
                        if (item.ExchangePrices.AvailableToLay.Count == 1)
                        {
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                            objmarketbookshow.Layprice1 = "";
                            objmarketbookshow.Layprice2 = "";
                            objmarketbookshow.Laysize1 = "";
                            objmarketbookshow.Laysize2 = "";
                        }
                        // lstMarketBookRunners.Add(objmarketbookshow);
                        // lstMArketbookshow.Add(objmarketbookshow);
                    }
                }
            }

            catch (System.Exception ex)

            {

            }

            //  return lstMArketbookshow;
        }

        public MarketBook GetBookPosition(string marketBookID)
        {

            ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                List<bftradeline.Models.UserBetsForAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbook.MarketId = marketBookID;
                    objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                    ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbook.Runners.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbook.Runners != null)
                        {
                            ExternalAPI.TO.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                objmarketbook.Runners.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                            objmarketbook.Runners.Add(objRunner);
                        }
                    }
                    ExternalAPI.TO.Runner objRunnerlast = new ExternalAPI.TO.Runner();
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objmarketbook.Runners.Add(objRunnerlast);


                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;

                        //int transferadmin1percent = (TransferAdminpercentage / 100);
                        decimal transadpercen = ((100 - Convert.ToDecimal(TransferAdminPercentage)) / 100);

                        foreach (var userbet in lstCurrentBetsbyUser)
                        {

                            var adminamunt = (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100));
                            var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                           // var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (adminamunt * transadpercen);
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }
                            else
                            {
                                double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }
                        }
                    }

                    objmarketbook.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbook.Runners)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                    }
                }
            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    List<bftradeline.Models.UserBetsforAgent> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAgentBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                    if (lstCurrentBetsAdmin.Count > 0)
                    {
                        lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                        objmarketbook.MarketId = marketBookID;
                        objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                        ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                        objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                        objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                        objmarketbook.Runners.Add(objRunner1);
                        foreach (var userbet in lstCurrentBetsAdmin)
                        {
                            if (objmarketbook.Runners != null)
                            {
                                ExternalAPI.TO.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                if (objexistingrunner == null)
                                {
                                    ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                    objmarketbook.Runners.Add(objRunner);
                                }
                            }
                            else
                            {
                                ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                                objmarketbook.Runners.Add(objRunner);
                            }



                        }
                        ExternalAPI.TO.Runner objRunnerlast = new ExternalAPI.TO.Runner();
                        objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                        objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                        objmarketbook.Runners.Add(objRunnerlast);
                        ///calculation
                        var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                        foreach (var userid in lstUsers)
                        {
                            var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();

                            foreach (var userbet in lstCurrentBetsbyUser)
                            {
                                decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);

                                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                if (userbet.BetType == "back")
                                {
                                    double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = totamount;
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbook.Runners)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = totamount;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbook.Runners)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = totamount;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }

                                }
                                else
                                {
                                    double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbook.Runners)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = totamount;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbook.Runners)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = totamount;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                }
                            }
                        }

                        objmarketbook.DebitCredit = lstDebitCredit;
                        foreach (var runneritem in objmarketbook.Runners)
                        {

                            runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                            runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                        }

                    }
                }
                else
                {

                }


            }
            return objmarketbook;
        }
        public void GetRunnersDataSourceFancy(List<ExternalAPI.TO.Runner> runners)
        {
            if (lstMarketBookRunnersFancy == null || lstMarketBookRunnersFancy.Count == 0)
            {


            }
            else
            {
               // lstMarketBookRunnersFancy.Clear();
                foreach (var item in runners)
                {
                    MarketBookShow objmarketbookshow = lstMarketBookRunnersFancy.Where(item1 => item1.SelectionID == item.SelectionId && item1.Selection == item.RunnerName.ToUpper()).FirstOrDefault();
                    if (objmarketbookshow == null)
                    {
                        GetDataForFancy(true);
                        DGVMarketFancy.ItemsSource = lstMarketBookRunnersFancy;
                        break;

                    }

                    objmarketbookshow.Price = item.LastPriceTraded.ToString();
                    MarketBook currentmarketsfancyPL = new MarketBook();
                    if (objmarketbookshow.CurrentMarketBookId != null)
                    {
                        currentmarketsfancyPL = GetBookPosition(objmarketbookshow.CurrentMarketBookId);
                    }

                    double TotalProfit = 0;
                    double TotalLoss = 0;
                    if (currentmarketsfancyPL.Runners != null)
                    {
                        TotalProfit = currentmarketsfancyPL.Runners.Max(t => t.ProfitandLoss);
                        TotalLoss = currentmarketsfancyPL.Runners.Min(t => t.ProfitandLoss);
                    }

                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {
                        objmarketbookshow.PL = TotalProfit.ToString();

                        objmarketbookshow.Loss = TotalLoss.ToString();
                    }
                    else
                    {
                        objmarketbookshow.PL = TotalLoss.ToString();

                        objmarketbookshow.Loss = TotalProfit.ToString();
                    }

                    objmarketbookshow.RunnerStatusstr = item.StatusStr;
                    objmarketbookshow.Marketstatusstr = item.MarketStatusStr;
                    objmarketbookshow.isShow = item.isShow;
                    if (item.ExchangePrices.AvailableToBack.Count == 3)
                    {
                        objmarketbookshow.Backprice2 = item.ExchangePrices.AvailableToBack[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].Price.ToString();
                        objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                        objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                        objmarketbookshow.Backsize2 = item.ExchangePrices.AvailableToBack[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].SizeStr.ToString();
                        objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                        objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                    }
                    if (item.ExchangePrices.AvailableToBack.Count == 2)
                    {
                        objmarketbookshow.Backprice2 = "";
                        objmarketbookshow.Backsize2 = "";
                        objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                        objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                        objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                        objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                    }
                    if (item.ExchangePrices.AvailableToBack.Count == 1)
                    {
                        objmarketbookshow.Backprice2 = "";
                        objmarketbookshow.Backprice1 = "";
                        objmarketbookshow.Backsize2 = "";
                        objmarketbookshow.Backsize1 = "";
                        objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                        objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                    }
                    if (item.ExchangePrices.AvailableToLay.Count == 3)
                    {
                        objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                        objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                        objmarketbookshow.Layprice2 = item.ExchangePrices.AvailableToLay[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].Price.ToString();
                        objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                        objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                        objmarketbookshow.Laysize2 = item.ExchangePrices.AvailableToLay[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].SizeStr.ToString();
                    }
                    if (item.ExchangePrices.AvailableToLay.Count == 2)
                    {
                        objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                        objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                        objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                        objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                        objmarketbookshow.Layprice2 = "";
                        objmarketbookshow.Laysize2 = "";

                    }
                    if (item.ExchangePrices.AvailableToLay.Count == 1)
                    {
                        objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                        objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                        objmarketbookshow.Layprice1 = "";
                        objmarketbookshow.Layprice2 = "";
                        objmarketbookshow.Laysize1 = "";
                        objmarketbookshow.Laysize2 = "";

                    }

                }
            }


        }

        public void GetRunnersDataSourceFancyFirstTime(List<ExternalAPI.TO.Runner> runners, MarketBook objMarketBook)
        {


            foreach (var item in runners)
            {
                MarketBookShow objmarketbookshow = new MarketBookShow();
                objmarketbookshow.Selection = item.RunnerName.ToString().ToUpper();
                objmarketbookshow.SelectionID = item.SelectionId;
                objmarketbookshow.Price = item.LastPriceTraded.ToString();
                objmarketbookshow.PL = item.ProfitandLoss.ToString();
                objmarketbookshow.RunnerStatusstr = item.StatusStr;
                objmarketbookshow.isShow = item.isShow;
                objmarketbookshow.CategoryName = objMarketBook.MainSportsname;
                objmarketbookshow.MarketbooknameBet = objMarketBook.MarketBookName;
                objmarketbookshow.Marketstatusstr = objMarketBook.MarketStatusstr;
                objmarketbookshow.BettingAllowed = objMarketBook.BettingAllowed;
                objmarketbookshow.OpenDate = objMarketBook.OpenDate;
                objmarketbookshow.runnerscount = objMarketBook.Runners.Count.ToString();
                objmarketbookshow.CurrentMarketBookId = objMarketBook.MarketId;
                if (item.ExchangePrices.AvailableToBack.Count == 3)
                {
                    objmarketbookshow.Backprice2 = item.ExchangePrices.AvailableToBack[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].Price.ToString();
                    objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                    objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                    objmarketbookshow.Backsize2 = item.ExchangePrices.AvailableToBack[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[2].SizeStr.ToString();
                    objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                    objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                }
                if (item.ExchangePrices.AvailableToBack.Count == 2)
                {
                    objmarketbookshow.Backprice2 = "";
                    objmarketbookshow.Backsize2 = "";
                    objmarketbookshow.Backprice1 = item.ExchangePrices.AvailableToBack[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].Price.ToString();
                    objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                    objmarketbookshow.Backsize1 = item.ExchangePrices.AvailableToBack[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[1].SizeStr.ToString();
                    objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                }
                if (item.ExchangePrices.AvailableToBack.Count == 1)
                {
                    objmarketbookshow.Backprice2 = "";
                    objmarketbookshow.Backprice1 = "";
                    objmarketbookshow.Backsize2 = "";
                    objmarketbookshow.Backsize1 = "";
                    objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString();
                    objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].SizeStr.ToString();
                }
                if (item.ExchangePrices.AvailableToLay.Count == 3)
                {
                    objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                    objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                    objmarketbookshow.Layprice2 = item.ExchangePrices.AvailableToLay[2].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].Price.ToString();
                    objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                    objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                    objmarketbookshow.Laysize2 = item.ExchangePrices.AvailableToLay[2].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[2].SizeStr.ToString();
                }
                if (item.ExchangePrices.AvailableToLay.Count == 2)
                {
                    objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                    objmarketbookshow.Layprice1 = item.ExchangePrices.AvailableToLay[1].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].Price.ToString();
                    objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                    objmarketbookshow.Laysize1 = item.ExchangePrices.AvailableToLay[1].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[1].SizeStr.ToString();
                    objmarketbookshow.Layprice2 = "";
                    objmarketbookshow.Laysize2 = "";

                }
                if (item.ExchangePrices.AvailableToLay.Count == 1)
                {
                    objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString();
                    objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].SizeStr.ToString();
                    objmarketbookshow.Layprice1 = "";
                    objmarketbookshow.Layprice2 = "";
                    objmarketbookshow.Laysize1 = "";
                    objmarketbookshow.Laysize2 = "";

                }
                lstMarketBookRunnersFancy.Add(objmarketbookshow);
            }

        }

        List<RootSCT> rootsct = new List<RootSCT>();
        public void GetUpdateSCT(string Eventtypeid)
        {

            stkpcricket.Visibility = Visibility.Collapsed;
            RootSCT Data = new RootSCT();
            try
            {
                rootsct = objBettingClient.GetUpdateSCT(Eventtypeid).ToList();
                Data = rootsct.Where(item => item.EventId == Convert.ToInt32(MarketBook.EventID)).FirstOrDefault();
                if (lblMarketStatus.Content.ToString() == "IN-PLAY")
                {
                    if (Data.State != null)
                    {
                        if (MarketBook.MainSportsname.Contains("Soccer"))
                        {

                            stkpsoccer.Visibility = Visibility.Visible;
                            string time = lblMarketTime.Content.ToString();
                            string[] starttime = time.Split(':');
                            if (starttime != null)
                            {
                                int hours = Convert.ToInt16(starttime[1]) * 60;
                                int mint = Convert.ToInt16(starttime[2]);
                                int timefinal = hours + mint;
                                lbltime.Text = timefinal.ToString();
                            }
                            lbltime.Text = Data.State.ElapsedRegularTime.ToString();
                            lblteamAS.Text = Data.AwayName;
                            lblteamBS.Text = Data.HomeName;
                            lblteamAG.Text = Data.State.Score.Away.Score.ToString();
                            lblteamBG.Text = Data.State.Score.Home.Score.ToString();
                            lblYcardN.Text = Data.State.Score.NumberOfYellowCards.ToString();
                            lblGcardN.Text = Data.State.Score.NumberOfRedCards.ToString();
                        }
                        else
                        {
                            stkptinnes.Visibility = Visibility.Visible;
                            lblrun1.Text = Data.AwayName;
                            lblrun2.Text = Data.HomeName;
                            lblscore1.Text = Data.State.Score.Away.Score.ToString();
                            lblscore2.Text = Data.State.Score.Home.Score.ToString();
                            gamesaw.Text = Data.State.Score.Away.Games.ToString();
                            gamesho.Text = Data.State.Score.Home.Games.ToString();
                            gameseqaw1.Text = Data.State.Score.Away.GameSequence[0].ToString();
                            gameseqho1.Text = Data.State.Score.Home.GameSequence[0].ToString();
                            gameseqaw2.Text = Data.State.Score.Away.GameSequence[1].ToString();
                            gameseqho2.Text = Data.State.Score.Home.GameSequence[1].ToString();
                            lblsbreakaw.Text = Data.State.Score.Away.ServiceBreaks.ToString();
                            lblsbreakho.Text = Data.State.Score.Home.ServiceBreaks.ToString();
                            Setaw.Text = Data.State.Score.Away.Sets.ToString();
                            Setho.Text = Data.State.Score.Home.Sets.ToString();
                            //gameseq1.Text
                        }
                    }
                }
                else
                {
                    stkptinnes.Visibility = Visibility.Collapsed;
                    stkpsoccer.Visibility = Visibility.Collapsed;
                }
            }

            catch (System.Exception EX)
            {
            }

        }


        ExternalAPI.TO.Root root2 = new ExternalAPI.TO.Root();
       
        public void CreateScoreCard(string marketIds)
        {
            try
            {                
                    string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataBPFancy/?marketID=" + string.Join(",", marketIds);
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RestAPIPath1);
                    request.Method = "GET";
                    request.Proxy = null;
                    request.Timeout = 5000;
                    request.KeepAlive = false;
                    request.ServicePoint.ConnectionLeaseTimeout = 5000;
                    request.ServicePoint.MaxIdleTime = 5000;
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    // String test = String.Empty;
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {
                            System.Runtime.Serialization.Json.DataContractJsonSerializer obj1 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(ExternalAPI.TO.Root));
                            root2 = (ExternalAPI.TO.Root)obj1.ReadObject(dataStream);
                            dataStream.Close();
                        }
                    }
               
                UpdateCricketUpdate();
                // scores = JsonConvert.DeserializeObject<List<MatchScores>>(objUsersServiceCleint.GetScoresbyEventIDandDate(MarketBook.EventID, MarketBook.OrignalOpenDate.Value));
            }
            catch (System.Exception ex)
            {

            }

        }

        public void UpdateCricketUpdate()
        {
            string over;
            string[] over1;
            int of;
            int os;
            decimal tball;
            string CR;
            // root2 = objBettingClient.GetUpdate2(MarketBook.EventID);

            string convertstring = root2.Result.Home;
            ExternalAPI.TO.Home root = new ExternalAPI.TO.Home();
            root = JsonConvert.DeserializeObject<ExternalAPI.TO.Home>(convertstring);
            string status;
            if (root.Cs.Msg != "")
            {
                stkpcricket.Visibility = Visibility.Visible;
                if (root.Cs.Msg == "WD")
                {
                    status = "WIDE";
                }
                else
                {
                    if (root.Cs.Msg == "B")
                    {
                        status = "Ball";
                    }
                    else
                    {
                        if (root.Cs.Msg == "OC")
                        {
                            status = "Over";
                        }
                        else
                        {
                            if (root.Cs.Msg == "BS")
                            {
                                status = "Bowler Stop";
                            }
                            else
                            {
                                if (root.Cs.Msg == "BR")
                                {
                                    status = "Innings Break";
                                }

                                else
                                {
                                    if (root.Cs.Msg == "DB")
                                    {
                                        status = "Drinks Break";
                                    }
                                    else
                                    {
                                        if (root.Cs.Msg == "1")
                                        {
                                            status = "Single";
                                        }
                                        else
                                        {
                                            if (root.Cs.Msg == "0")
                                            {
                                                status = "Zero Run";
                                            }
                                            else
                                            {
                                                if (root.Cs.Msg == "2")
                                                {
                                                    status = "Double";
                                                }
                                                else
                                                {
                                                    if (root.Cs.Msg == "3")
                                                    {
                                                        status = "Three Runs";
                                                    }
                                                    else
                                                    {
                                                        if (root.Cs.Msg == "4")
                                                        {
                                                            status = "Four Runs";
                                                        }
                                                        else
                                                        {
                                                            if (root.Cs.Msg == "5")
                                                            {
                                                                status = "Five Runs";
                                                            }
                                                            else
                                                            {
                                                                if (root.Cs.Msg == "NB+1")
                                                                {
                                                                    status = "No Ball + 1";
                                                                }
                                                                else
                                                                {
                                                                    if (root.Cs.Msg == "WK")
                                                                    {
                                                                        status = "Wicket";
                                                                    }
                                                                    else
                                                                    {
                                                                        if (root.Cs.Msg == "6")
                                                                        {
                                                                            status = "Sixer";
                                                                        }
                                                                        else
                                                                        {
                                                                            if (root.Cs.Msg == "WD+1")
                                                                            {
                                                                                status = "wide + 1 run";
                                                                            }
                                                                            else
                                                                            {
                                                                                if (root.Cs.Msg == "C")
                                                                                {
                                                                                    status = "Confirming";
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (root.Cs.Msg == "NB")
                                                                                    {
                                                                                        status = "NO Ball";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (root.Cs.Msg == "FH")
                                                                                        {
                                                                                            status = "Free Hit";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (root.Cs.Msg == "SB")
                                                                                            {
                                                                                                status = "Spinner";
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if (root.Cs.Msg == "FB")
                                                                                                {
                                                                                                    status = "Faster";
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    if (root.Cs.Msg == "3U")
                                                                                                    {
                                                                                                        status = "3rd Umpire";
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if (root.Cs.Msg == "NO")
                                                                                                        {
                                                                                                            status = "Not Out";
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            status = root.Cs.Msg;

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
                        }

                    }
                }


                lblStricker1.Text = status;
                if (root.Con.Mf == "Test")
                {
                    lblr.Visibility = Visibility.Collapsed;
                    lblRequiredScore2.Visibility = Visibility.Collapsed;
                    if (root.I == "i1" || root.I == "i3")
                    {
                        lblteamA.Text = root.T1.N;
                        if (root.I3.Sc > 0)
                        {
                            lblTeamAScore1.Text = root.I1.Sc.ToString() + " & " + root.I3.Sc.ToString();
                            lblw.Text = root.I3.Wk.ToString();
                            lblovs.Text = root.I3.Ov.ToString();
                            over = root.I1.Ov.ToString();
                            over1 = over.Split('.');
                            of = Convert.ToInt32(over1[0]) * 6;
                            os = 0;
                            if (over1.Length > 1)
                            {
                                os = Convert.ToInt32(over1[1]);
                            }
                            tball = of + os;
                            try
                            {
                                CR = ((Convert.ToDecimal(root.I1.Sc) / tball) * 6).ToString("F2");
                                lblCRR.Text = CR;
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                        else
                        {
                            lblTeamAScore1.Text = root.I1.Sc.ToString();
                            lblw.Text = root.I1.Wk.ToString();
                            lblovs.Text = root.I1.Ov.ToString();
                            over = root.I1.Ov.ToString();
                            over1 = over.Split('.');
                            of = Convert.ToInt32(over1[0]) * 6;
                            os = 0;
                            if (over1.Length > 1)
                            {
                                os = Convert.ToInt32(over1[1]);
                            }
                            tball = of + os;
                            try
                            {
                                CR = ((Convert.ToDecimal(root.I1.Sc) / tball) * 6).ToString("F2");
                                lblCRR.Text = CR;
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                    }
                    else
                    {


                        lblteamA.Text = root.T2.N;
                        if (MarketBook.Runners.Count == 3)
                        {

                            if (root.I4.Sc > 0)
                            {
                                lblTeamAScore1.Text = root.I2.Sc.ToString() + " & " + root.I4.Sc.ToString();
                                lblw.Text = root.I4.Wk.ToString();
                                lblovs.Text = root.I4.Ov.ToString();
                                over = root.I2.Ov.ToString();
                                over1 = over.Split('.');
                                of = Convert.ToInt32(over1[0]) * 6;
                                os = 0;
                                if (over1.Length > 1)
                                {
                                    os = Convert.ToInt32(over1[1]);
                                }
                                tball = of + os;
                                try
                                {
                                    CR = ((Convert.ToDecimal(root.I2.Sc) / tball) * 6).ToString("F2");
                                    lblCRR.Text = CR;
                                }
                                catch (System.Exception ex)
                                {
                                }
                            }
                            else
                            {
                                lblTeamAScore1.Text = root.I2.Sc.ToString();
                                lblw.Text = root.I2.Wk.ToString();
                                lblovs.Text = root.I2.Ov.ToString();
                                over = root.I2.Ov.ToString();
                                over1 = over.Split('.');
                                of = Convert.ToInt32(over1[0]) * 6;
                                os = 0;
                                if (over1.Length > 1)
                                {
                                    os = Convert.ToInt32(over1[1]);
                                }
                                tball = of + os;
                                try
                                {
                                    CR = ((Convert.ToDecimal(root.I2.Sc) / tball) * 6).ToString("F2");
                                    lblCRR.Text = CR;
                                }
                                catch (System.Exception ex)
                                {
                                }
                            }
                        }
                        decimal overs = Convert.ToDecimal(root.Iov) * 6;//- root.I2.ov;                   
                        int Rscore = Convert.ToInt32(root.I2.Tr) - Convert.ToInt32(root.I2.Sc);

                    }

                }

                else
                {
                    if (root.I2.Tr == "0")
                    {
                        lblteamA.Text = root.T1.N;
                        lblTeamAScore1.Text = root.I1.Sc.ToString();
                        lblw.Text = root.I1.Wk.ToString();
                        lblovs.Text = root.I1.Ov.ToString();
                        over = root.I1.Ov.ToString();
                        over1 = over.Split('.');
                        of = Convert.ToInt32(over1[0]) * 6;
                        os = 0;
                        if (over1.Length > 1)
                        {
                            os = Convert.ToInt32(over1[1]);
                        }
                        tball = of + os;
                        try
                        {
                            CR = ((Convert.ToDecimal(root.I1.Sc) / tball) * 6).ToString("F2");
                            lblCRR.Text = CR;
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                    else
                    {
                        lblr.Visibility = Visibility.Visible;
                        lblRequiredScore2.Visibility = Visibility.Visible;
                        lblRscore.Visibility = Visibility.Visible;
                        lblteamA.Text = root.T2.N;
                        lblRRR.Visibility = Visibility.Visible;
                        lblTeamAScore1.Text = root.I2.Sc.ToString();
                        lblw.Text = root.I2.Wk.ToString();
                        lblovs.Text = root.I2.Ov.ToString();
                        lblRscore.Text = root.I2.Tr.ToString();
                        over = root.I2.Ov.ToString();
                        over1 = over.Split('.');
                        of = Convert.ToInt32(over1[0]) * 6;
                        os = 0;
                        if (over1.Length > 1)
                        {
                            os = Convert.ToInt32(over1[1]);
                        }
                        tball = of + os;
                        try
                        {
                            CR = ((Convert.ToDecimal(root.I2.Sc) / tball) * 6).ToString("F2");
                            lblCRR.Text = CR;
                        }
                        catch (System.Exception ex)
                        {
                        }
                        decimal overs = Convert.ToDecimal(root.Iov) * 6;//- root.I2.ov;
                        decimal ball = overs - tball;

                        int Rscore = Convert.ToInt32(root.I2.Tr) - Convert.ToInt32(root.I2.Sc);
                        string msg = "need " + Rscore + " from " + ball + "";
                        lblRRR.Text = (Rscore / (ball / 6)).ToString("F2");
                        lblLastBallComment1.Text = msg;
                    }

                }
                try
                {
                    lblballs.Text = root.Pb.Substring(root.Pb.Length - 11);
                }
                catch (System.Exception ex)
                {
                }
            }
            else
            {
                stkpcricket.Visibility = Visibility.Collapsed;
            }

        }

        MarketBookShow runner1 = new MarketBookShow();
        ExternalAPI.TO.MarketBookForindianFancy currentmarketsfancyPL = new ExternalAPI.TO.MarketBookForindianFancy();
        ObservableCollection<ExternalAPI.TO.MarketBookForindianFancy> allmarkets = new ObservableCollection<ExternalAPI.TO.MarketBookForindianFancy>();
        ExternalAPI.TO.GetDataFancy GetDataFancy = new ExternalAPI.TO.GetDataFancy();
        ExternalAPI.TO.GetDataFancy test = new ExternalAPI.TO.GetDataFancy();

        public void GetDataForFancy(string EventID, string marketIds)
        {
            try
            {   
                string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataIndiaFancy/?marketID=" + string.Join(",", marketIds);
     
                //string RestAPIPath1 = "http://royalold.com/api/MatchOdds/GetOddslite/4/" + marketIds + "/" + EventID + "";
                //ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataFancy/?marketID=" + string.Join(",", marketIds);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RestAPIPath1);
                request.Method = "GET";
                request.Proxy = null;
                request.Timeout = 5000;
                request.KeepAlive = false;
                request.ServicePoint.ConnectionLeaseTimeout = 5000;
                request.ServicePoint.MaxIdleTime = 5000;
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls | SecurityProtocolType.Tls;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ServicePointManager.DefaultConnectionLimit = 9999;
                
                String test1 = String.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {

                    using (Stream dataStream = response.GetResponseStream())
                    {
                        System.Runtime.Serialization.Json.DataContractJsonSerializer obj1 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(string));
                        test1 = obj1.ReadObject(dataStream).ToString();

                        dataStream.Close();
                    }
                    // response.Close();


                }
                test = JsonConvert.DeserializeObject<GetDataFancy>(test1);
               // var results = JsonConvert.DeserializeObject<List<SampleResponse1>>(test1);



            }

            catch (System.Exception ex)
            {

            }
        }


        public void UpdateLineMarketsData(bool isFirstTime)
        {
            try
            {

                if (lstMarketBookRunnersFancy == null && LastloadedLinMarkets.Count > 0)
                {
                    GetDataForFancy(true);
                    DGVMarketFancy.ItemsSource = lstMarketBookRunnersFancy;


                }
                ExternalAPI.TO.MarketBook objFancyMarketBook = new MarketBook();
                objFancyMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                foreach (var item in LastloadedLinMarkets)
                {
                    try
                    {
                        item.Runners[0].RunnerName = item.MarketBookName;
                        item.Runners[0].MarketStatusStr = item.MarketStatusstr;




                        if ((item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 0 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 0) && (item.MarketStatusstr == "In Play" || item.MarketStatusstr == "IN-PLAY"))
                        {
                            item.Runners[0].isShow = true;
                        }
                        else
                        {
                            item.Runners[0].isShow = false;
                        }

                        objFancyMarketBook.Runners.Add(item.Runners[0]);
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
                if (objFancyMarketBook.Runners.Count > 0)
                {
                    GetRunnersDataSourceFancy(objFancyMarketBook.Runners);
                    if (objFancyMarketBook.Runners.Where(item => item.isShow == true).Count() > 0)
                    {
                        DGVMarketFancy.ItemsSource = lstMarketBookRunnersFancy;
                        DGVMarketFancy.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        DGVMarketFancy.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    DGVMarketFancy.Visibility = Visibility.Collapsed;
                }
            }
            catch (System.Exception ex)
            {


            }


        }

        public void UpdateLineMarketsDataIN(bool isFirstTime)
        {
            try
            {

                ExternalAPI.TO.MarketBookForindianFancy objFancyMarketBook = new ExternalAPI.TO.MarketBookForindianFancy();

                GetRunnersDataSourceFancy(test);
                DGVMarketIndianFancy.ItemsSource = lstMarketBookRunnersFancyin;
                if (lstMarketBookRunnersFancyin.Count > 0)
                {

                    DGVMarketIndianFancy.Visibility = Visibility.Visible;
                }
                else
                {
                    DGVMarketIndianFancy.Visibility = Visibility.Collapsed;
                }

            }
            catch (System.Exception ex)
            {


            }

        }

        public void GetRunnersDataSourceFancy(ExternalAPI.TO.GetDataFancy runners)
        {
            try
            {
                lstMarketBookRunnersFancyin = new ObservableCollection<MarketBookShow>();
                lstMarketBookRunnersFancyin.Clear();
                runners.session = runners.session.Take(40).OrderBy(s => s.SelectionId).ToList();
                foreach (var item in runners.session.Take(15))
                {
                    //var a = test.LinevMarkets.Where(aa => aa.SelectionID == item.SelectionId).Distinct().ToList();
                    //if (a.Count > 0)
                    //{
                        MarketBookShow objmarketbookshow = new MarketBookShow(); //lstMarketBookRunnersFancy.Where(item1 => item1.SelectionID == item.SelectionId && item1.Selection == item.RunnerName.ToUpper()).FirstOrDefault();
                        objmarketbookshow.isSelectedForLK = true;
                        objmarketbookshow.CategoryName = "Cricket";
                        objmarketbookshow.MarketbooknameBet = item.RunnerName;
                        objmarketbookshow.RunnerStatusstr = item.GameStatus;
                        objmarketbookshow.Marketstatusstr = lblMarketStatus.Content.ToString();
                        objmarketbookshow.BettingAllowed = true;
                        objmarketbookshow.Selection = item.RunnerName;
                        objmarketbookshow.SelectionID = item.SelectionId;
                        ExternalAPI.TO.MarketBookForindianFancy currentmarketsfancyPL = new ExternalAPI.TO.MarketBookForindianFancy();
                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {
                        if (LoggedinUserDetail.CurrentUserBets.Where(x => x.location == "9").Count() > 0)
                        {
                            currentmarketsfancyPL = GetBookPositioninNew(item.SelectionId);
                        }
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            if (LoggedinUserDetail.CurrentAgentBets.Where(x => x.location == "9").Count() > 0)
                            {
                                currentmarketsfancyPL = GetBookPositioninNew(item.SelectionId);
                            }
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                            {
                                if (LoggedinUserDetail.CurrentAdminBets.Where(x => x.location == "9").Count() > 0)
                                {
                                    currentmarketsfancyPL = GetBookPositioninNew(item.SelectionId);
                                }
                            }
                            else
                            {
                                if (LoggedinUserDetail.CurrentSuperBets.Where(x => x.location == "9").Count() > 0)
                                {
                                    currentmarketsfancyPL = GetBookPositioninNew(item.SelectionId);
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
                            objmarketbookshow.PL = TotalProfit.ToString();
                            objmarketbookshow.Loss = TotalLoss.ToString();
                        }
                        else
                        {
                            objmarketbookshow.PL = TotalLoss.ToString();
                            objmarketbookshow.Loss = TotalProfit.ToString();
                        }
                        //  objmarketbookshow.PL = item.ProfitandLoss.ToString();
                        if (objmarketbookshow.RunnerStatusstr == "SUSPENDED")
                    {
                        objmarketbookshow.StatusStr = "Collapsed";
                        objmarketbookshow.JockeyName = "SUSPE";
                        objmarketbookshow.JockeyHeading = "NDED";
                        objmarketbookshow.StallDraw = "Visible";
                        objmarketbookshow.Price = "0,0,0,0";
                    }
                    else
                    {
                            if (objmarketbookshow.RunnerStatusstr == "Ball Running")
                            {
                                if (popupBetslipforin1.IsOpen == true)
                                {
                                    popupBetslipforin1.IsOpen = false;
                                }
                                objmarketbookshow.StatusStr = "Collapsed";
                                objmarketbookshow.JockeyName = "BALLRU";
                                objmarketbookshow.JockeyHeading = "NNING";
                                objmarketbookshow.StallDraw = "Visible";
                                objmarketbookshow.Price = "0,0,0,0";
                            }
                            else
                            {
                                objmarketbookshow.StallDraw = "Collapsed";
                                objmarketbookshow.Laysize0 = item.LaySize1.ToString();
                                objmarketbookshow.Layprice0 = item.LayPrice1.ToString();
                                objmarketbookshow.Backsize0 = item.BackSize1.ToString();
                                objmarketbookshow.Backprice0 = item.BackPrice1.ToString();
                                objmarketbookshow.StatusStr = "Visible";
                                objmarketbookshow.Price = "1,0,0,0";
                                objmarketbookshow.isShow = true;
                            }
                        }

                    objmarketbookshow.CurrentMarketBookId = MarketBook.EventID; //a[0].MarketCatalogueID;

                        objmarketbookshow.OpenDate = MarketBook.OpenDate;

                        lstMarketBookRunnersFancyin.Add(objmarketbookshow);

                    //}
                }
               // return lstMarketBookRunnersFancyin;

            }
            catch (System.Exception ex)
            {
                //lstMarketBookRunnersFancyin = new ObservableCollection<MarketBookShow>();
                //return lstMarketBookRunnersFancyin;
            }

            //  return lstMArketbookshow;
        }

        
        ObservableCollection<MarketBookShow> lstMarketBookRunnersKJ = new ObservableCollection<MarketBookShow>();
        bool data = true;

        public void updatefancymarket()
        {

            var LineVMarkets = objBettingClient.GetEventIDFancyMarket(MarketBook.EventID, MarketBook.MarketId); //JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(MarketBook.EventID, MarketBook.OrignalOpenDate.Value, UserIDforLinevmarkets));
            MarketBook.LineVMarkets = LineVMarkets.ToList();
        }
        public void AssignKalijutdata()
        {
            try
            {

                // var results = objUsersServiceCleint.GetEventDetailsbyMarketBook(MarketBook.MarketId);
                //MarketBook.LineVMarkets = linevmarkets;
                lstMarketBookRunnerKalijut = new ObservableCollection<MarketBookShow>();
                lstMarketBookRunnerKalijut.Clear();
                var KJMarkets = test.LinevMarkets.Where(item => item.isOpenedbyUser == true && item.EventName == "Kali v Jut").ToList();

                if (KJMarkets.Count() > 0)
                {
                    foreach (var runners in KJMarkets)
                    {
                        MarketBookShow objmarketbookshow = new MarketBookShow();
                        objmarketbookshow.Selection = runners.MarketCatalogueName;
                        objmarketbookshow.SelectionID = "369646";
                        objmarketbookshow.CategoryName = "Line v Markets";
                        objmarketbookshow.MarketbooknameBet = lblMarketName.Content.ToString();
                        objmarketbookshow.RunnerStatusstr = MarketBook.MarketStatusstr;
                        objmarketbookshow.Marketstatusstr = lblMarketStatus.Content.ToString();
                        objmarketbookshow.BettingAllowed = runners.BettingAllowed;
                        //objmarketbookshow.isSelectedForLK = runners.BettingAllowed;
                        objmarketbookshow.CurrentMarketBookId = runners.MarketCatalogueID;

                        objmarketbookshow.Laysize0 = "102";//KJs.KaliPriceLay.ToString();
                        objmarketbookshow.Layprice0 = "1";//KJs.KaliSizeLay.ToString();
                        objmarketbookshow.Backsize0 = "98";//KJs.KaliPriceBack.ToString();
                        objmarketbookshow.Backprice0 = "1";//KJs.KaliSizeBack.ToString();

                        ExternalAPI.TO.MarketBookForindianFancy currentmarketsfancyPL = new ExternalAPI.TO.MarketBookForindianFancy();
                        ProfitandLoss objProfitandloss = new ProfitandLoss();

                        currentmarketsfancyPL = GetBookPositioninKJ(runners.MarketCatalogueID, "369646");

                        double TotalProfit = 0;
                        double TotalLoss = 0;
                        if (currentmarketsfancyPL.RunnersForindianFancy != null)
                        {
                            TotalProfit = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Max(t => t.ProfitandLoss));
                            TotalLoss = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Min(t => t.ProfitandLoss));

                            objmarketbookshow.PL = TotalProfit.ToString();
                            objmarketbookshow.Loss = TotalLoss.ToString();
                        }
                        else
                        {
                            objmarketbookshow.PL = TotalLoss.ToString();
                            objmarketbookshow.Loss = TotalProfit.ToString();
                        }

                        lstMarketBookRunnerKalijut.Add(objmarketbookshow);
                    }
                }
            }

            catch (System.Exception ex)
            {
                DGVMarketKalijut.Visibility = Visibility.Collapsed;
            }

        }

        ObservableCollection<MarketBookShow> lstMarketBookRunnersSFIg = new ObservableCollection<MarketBookShow>();
        public void AssignSFigdata()
        {
            try
            {
                lstMarketBookRunnerSFigure = new ObservableCollection<MarketBookShow>();
                lstMarketBookRunnerSFigure.Clear();

                var KJMarkets = test.LinevMarkets.Where(item => item.isOpenedbyUser == true && item.EventName == "SmallFig").ToList();

                if (KJMarkets.Count() > 0)
                {
                    foreach (var runners in KJMarkets)
                    {
                        MarketBookShow objmarketbookshow = new MarketBookShow();
                        objmarketbookshow.CategoryName = "Line v Markets";
                        objmarketbookshow.MarketbooknameBet = lblMarketName.Content.ToString();
                        //runner.RunnerStatusstr = runners.StatusStr;
                        objmarketbookshow.RunnerStatusstr = MarketBook.MarketStatusstr;
                        objmarketbookshow.Marketstatusstr = lblMarketStatus.Content.ToString();
                        objmarketbookshow.BettingAllowed = true;
                        objmarketbookshow.Selection = runners.MarketCatalogueName;
                        objmarketbookshow.CurrentMarketBookId = runners.MarketCatalogueID;
                        objmarketbookshow.SelectionID = "3121";
                        objmarketbookshow.Laysize0 = "102"; //KJs.KaliPriceLay.ToString();
                        objmarketbookshow.Layprice0 = "1";//KaliSizeLay.ToString();
                        objmarketbookshow.Backsize0 = "98";//.KaliPriceBack.ToString();
                        objmarketbookshow.Backprice0 = "1";//KJs.KaliSizeBack.ToString();
                        //objmarketbookshow.isSelectedForLK = runners.BettingAllowed;

                        ExternalAPI.TO.MarketBookForindianFancy currentmarketsfancyPL = new ExternalAPI.TO.MarketBookForindianFancy();
                        ProfitandLoss objProfitandloss = new ProfitandLoss();

                        currentmarketsfancyPL = GetBookPositioninKJ(runners.MarketCatalogueID, "3121");

                        double TotalProfit = 0;
                        double TotalLoss = 0;
                        if (currentmarketsfancyPL.RunnersForindianFancy != null)
                        {
                            TotalProfit = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Max(t => t.ProfitandLoss));
                            TotalLoss = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Min(t => t.ProfitandLoss));
                            objmarketbookshow.PL = TotalProfit.ToString();
                            objmarketbookshow.Loss = TotalLoss.ToString();
                        }
                        else
                        {
                            objmarketbookshow.PL = TotalLoss.ToString();
                            objmarketbookshow.Loss = TotalProfit.ToString();
                        }

                        lstMarketBookRunnerSFigure.Add(objmarketbookshow);
                    }
                }
                else
                {
                    DGVMarketSFig.Visibility = Visibility.Collapsed;
                }

            }
            catch (System.Exception ex)
            {
                //AssignKalijutdata();
                // return ExternalAPI.TO.MarketBookForindianFancy;
            }

        }


        bool time = false;
        List<MarketBook> LastloadedLinMarketsFig = new List<MarketBook>();
        public void AssignFiguredata()
        {
            lstMarketBookRunnersFigure = new ObservableCollection<MarketBookShow>();
            lstMarketBookRunnersFigure.Clear();

            var FigureMarkets = test.LinevMarkets.GroupBy(item => item.MarketCatalogueName).Select(g => g.First()).Where(item2 => item2.EventName == "Figure").ToList(); //MarketBook.LineVMarkets.Where(item => item.isOpenedbyUser == true && item.EventName== "Figure").ToList();
            FigureMarkets = FigureMarkets.Where(item => item.isOpenedbyUser == true).ToList();
            if (FigureMarkets.Count > 0)
            {
                foreach (var bfobject in FigureMarkets)
                {
                    try
                    {
                        if (bfobject.MarketCatalogueID != MarketBookFigure.MarketId && MarketBookFigure.MarketId != null)
                        {
                            LastloadedLinMarketsFig.Clear();
                            time = false;
                        }
                        if (time == false)
                        {
                            LastloadedLinMarketsFig.Add(ConvertJsontoMarketObjectBFNewSourceFigure(bfobject.MarketCatalogueID, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed));
                            MarketBookFigure = LastloadedLinMarketsFig.FirstOrDefault();
                            time = true;
                        }
                        GetRunnersDataSourceFigure(MarketBookFigure.Runners, MarketBookFigure);
                    }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    catch (System.Exception ex)

                    {

                    }
                }
            }
            else
            {
                DGVMarketFigure.Visibility = Visibility.Collapsed;
            }

            // DGVMarketFigure.ItemsSource = lstMarketBookRunnersFigure;
            if (lstMarketBookRunnersFigure == null)
            {
                DGVMarketFigure.Visibility = Visibility.Collapsed;
            }

        }

        public MarketBook ConvertJsontoMarketObjectBFNewSourceFigure(string marketid, string sheetname, string MainSportsCategory, bool BettingAllowed)
        {
            if (1 == 1)
            {
                var marketbook = new MarketBook();
                marketbook.MarketId = marketid;
                marketbook.BettingAllowed = BettingAllowed;
                marketbook.MainSportsname = "Cricket";
                marketbook.MarketBookName = sheetname;
                marketbook.MarketStatusstr = lblMarketStatus.Content.ToString();

                int seletionID = 001;

                int RunnerName = 0;
                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                for (int i = 0; i <= 9; i++)
                {


                    var runner = new ExternalAPI.TO.Runner();
                    runner.SelectionId = seletionID + i.ToString(); //runnerinfo[0].Trim().ToString();
                    runner.RunnerName = i.ToString();
                    //FigureOdds.FigPriceBack0.ToString();

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
                }


                marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);

                return marketbook;


            }
        }


        private void TmrUpdateMarket_Elapsed(object sender, ElapsedEventArgs e)
        {

        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //  this.WindowState = WindowState.Minimized;
        }

        private void TextBlock_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            //if (this.WindowState == WindowState.Maximized)
            //{
            //    this.WindowState = WindowState.Normal;
            //    this.Height = 400;
            //    this.Width = 676;

            //}
            //else
            //{
            //    this.WindowState = WindowState.Maximized;


            //}

        }

        private void TextBlock_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {


        }
        public void Resizewindow()
        {
            //if ((this.Width < 880 && this.Width > 675 && btnShowHideBets.Tag.ToString() == "1") || (this.Width < 675 && btnShowHideBets.Tag.ToString() == "0"))
            //{
            //    DGVMarket.Columns[7].Visibility = Visibility.Collapsed;
            //    DGVMarket.Columns[8].Visibility = Visibility.Visible;
            //    DGVMarket.Columns[11].Visibility = Visibility.Visible;
            //    DGVMarket.Columns[12].Visibility = Visibility.Collapsed;
            //    DGVMarket.Columns[0].Visibility = Visibility.Collapsed;
            //    DGVMarket.Columns[4].Visibility = Visibility.Collapsed;
            //    DGVMarketFancy.Columns[7].Visibility = Visibility.Collapsed;
            //    DGVMarketFancy.Columns[8].Visibility = Visibility.Visible;
            //    DGVMarketFancy.Columns[11].Visibility = Visibility.Visible;
            //    DGVMarketFancy.Columns[12].Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    if (this.Width < 675)
            //    {
            //        DGVMarket.Columns[7].Visibility = Visibility.Collapsed;
            //        DGVMarket.Columns[8].Visibility = Visibility.Collapsed;
            //        DGVMarket.Columns[11].Visibility = Visibility.Collapsed;
            //        DGVMarket.Columns[12].Visibility = Visibility.Collapsed;
            //        DGVMarket.Columns[0].Visibility = Visibility.Collapsed;
            //        DGVMarket.Columns[4].Visibility = Visibility.Collapsed;
            //        DGVMarketFancy.Columns[7].Visibility = Visibility.Collapsed;
            //        DGVMarketFancy.Columns[8].Visibility = Visibility.Collapsed;
            //        DGVMarketFancy.Columns[11].Visibility = Visibility.Collapsed;
            //        DGVMarketFancy.Columns[12].Visibility = Visibility.Collapsed;
            //    }
            //    else
            //    {
            //        DGVMarket.Columns[7].Visibility = Visibility.Visible;
            //        DGVMarket.Columns[8].Visibility = Visibility.Visible;
            //        DGVMarket.Columns[11].Visibility = Visibility.Visible;
            //        DGVMarket.Columns[12].Visibility = Visibility.Visible;
            //        DGVMarket.Columns[0].Visibility = Visibility.Visible;
            //        DGVMarket.Columns[4].Visibility = Visibility.Visible;
            //        DGVMarketFancy.Columns[7].Visibility = Visibility.Visible;
            //        DGVMarketFancy.Columns[8].Visibility = Visibility.Visible;
            //        DGVMarketFancy.Columns[11].Visibility = Visibility.Visible;
            //        DGVMarketFancy.Columns[12].Visibility = Visibility.Visible;
            //    }

            //}
            //btnShowHideBets.Tag.ToString() == "1" &&
            if (btnShowHideMenu.Tag.ToString() == "1")
            {
                stkpnlBets.Visibility = Visibility.Visible;
                stkpnlMenu.Visibility = Visibility.Visible;

                stkpnlBets.Width = this.Width * 0.25;
                stkpnlMenu.Width = 260;
                stkpnlMarketGrid.Width = this.Width * 0.54;
                DGVMarket.Width = this.Width * 0.54;
                DGVMarketToWintheToss.Width = this.Width * 0.54;

            }
            else
            {
                //btnShowHideBets.Tag.ToString() == "0" &&
                if (btnShowHideMenu.Tag.ToString() == "1")
                {
                    stkpnlBets.Visibility = Visibility.Collapsed;
                    stkpnlMenu.Visibility = Visibility.Visible;
                    stkpnlBets.Width = this.Width * 0.20;
                    stkpnlMenu.Width = 260;
                    stkpnlMarketGrid.Width = this.Width * 0.79;
                    DGVMarket.Width = this.Width * 0.79;
                    DGVMarketToWintheToss.Width = this.Width * 0.79;

                }
                else
                {
                    //btnShowHideBets.Tag.ToString() == "1" &&
                    if (btnShowHideMenu.Tag.ToString() == "0")
                    {
                        stkpnlBets.Visibility = Visibility.Visible;

                        stkpnlMenu.Visibility = Visibility.Collapsed;
                        stkpnlBets.Width = this.Width * 0.33;
                        stkpnlBets.Width += 5;
                        stkpnlMarketGrid.Width = this.Width * 0.65;
                        DGVMarket.Width = this.Width * 0.65;
                        DGVMarketToWintheToss.Width = this.Width * 0.65;
                        // txtRulesNew.Width = DGVMarket.Width - 470;
                        //if (LoggedinUserDetail.GetUserTypeID() == 1)
                        //{
                        //    stkpnlBets.Width = 350;
                        //    stkpnlMarketGrid.Width = this.Width - 370;
                        //    DGVMarket.Width = this.Width - 370;
                        //}
                        //else
                        //{

                        //    //stkpnlMarketGrid.Width = this.Width - 270;
                        //    //DGVMarket.Width = this.Width - 270;
                        //}


                    }
                    else
                    {
                        stkpnlBets.Visibility = Visibility.Collapsed;
                        stkpnlMenu.Visibility = Visibility.Collapsed;
                        stkpnlMarketGrid.Width = this.Width - 12;
                        DGVMarket.Width = this.Width - 12;
                        DGVMarketToWintheToss.Width = this.Width - 12;
                        //   txtRulesNew.Width = DGVMarket.Width - 600;
                    }

                }


            }

            try
            {
                lblUnMatchBetsCount.Width = stkpnlBets.Width - 200;
                txtblockallbetsandcutting.Width = stkpnlBets.Width - 205;
            }
            catch (System.Exception ex)

            {

            }

        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }



        private void stkpnlmarketname_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {

            }

        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void stkpnlmarketname_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (e.ChangedButton == MouseButton.Left)
                {

                }
            }

            catch (System.Exception ex)

            {

            }


        }

        private void stkpnlmarketname_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void btnShowHideBets_Click(object sender, RoutedEventArgs e)
        {
            //if (btnShowHideBets.Tag.ToString() == "1")
            //{ btnShowHideBets.Tag = "0"; }
            //else
            //{
            //    btnShowHideBets.Tag = "1";
            //}
            //Resizewindow();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            tmrUpdateMarket.Stop();
            tmrUpdateLiabalities.Stop();
            ExternalAPI.TO.MarketBook markettoremove = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == MarketBook.MarketId).First();
            LoggedinUserDetail.MarketBooks.Remove(markettoremove);
            LoggedinUserDetail.OpenMarkets.Remove("marketwin" + markettoremove.MarketId.Replace(".", ""));

        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private void stkpnlMarketGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popupBetslip.Visibility = Visibility.Collapsed;
            popupBetslip.IsOpen = false;
        }
        public string BetSlipSelectionID = "";
        public string BetType = "";

        public bool CheckForAllowedBettingOverAll(string categoryname, string marketbookname, string RunnersCount)
        {

            bool AllowedBet = false;
            // var categoryname = lastloadedmarket.MainSportsname;
            // var marketbookname = lastloadedmarket.MarketBookName;
            if (RunnersCount == "1")
            {
                AllowedBet = LoggedinUserDetail.AllowedMarketsForUser.isFancyMarketAllowed;
                return AllowedBet;
            }
            if (categoryname.Contains("Horse Racing") && !marketbookname.Contains("To Be Placed"))
            {
                AllowedBet = LoggedinUserDetail.AllowedMarketsForUser.isHorseRaceWinAllowedForBet;

            }
            else
            {
                if (categoryname.Contains("Horse Racing") && marketbookname.Contains("To Be Placed"))
                {
                    AllowedBet = LoggedinUserDetail.AllowedMarketsForUser.isHorseRacePlaceAllowedForBet;

                }
                else
                {
                    if (categoryname.Contains("Greyhound Racing") && marketbookname.Contains("To Be Placed"))
                    {
                        AllowedBet = LoggedinUserDetail.AllowedMarketsForUser.isGrayHoundRacePlaceAllowedForBet;

                    }
                    else
                    {
                        if (categoryname.Contains("Greyhound Racing") && !marketbookname.Contains("To Be Placed"))
                        {
                            AllowedBet = LoggedinUserDetail.AllowedMarketsForUser.isGrayHoundRaceWinAllowedForBet;

                        }
                        else
                        {
                            if (marketbookname.Contains("Completed Match"))
                            {
                                AllowedBet = LoggedinUserDetail.AllowedMarketsForUser.isCricketCompletedMatchAllowedForBet;

                            }
                            else
                            {
                                if (marketbookname.Contains("Innings Runs") || marketbookname.Contains("Inns Runs"))
                                {
                                    AllowedBet = LoggedinUserDetail.AllowedMarketsForUser.isCricketInningsRunsAllowedForBet;

                                }
                                else
                                {
                                    if (categoryname == "Tennis")
                                    {
                                        AllowedBet = LoggedinUserDetail.AllowedMarketsForUser.isTennisAllowedForBet;

                                    }
                                    else
                                    {
                                        if (categoryname == "Soccer")
                                        {
                                            AllowedBet = LoggedinUserDetail.AllowedMarketsForUser.isSoccerAllowedForBet;

                                        }
                                        else
                                        {
                                            if (marketbookname.Contains("Tied Match"))
                                            {
                                                AllowedBet = LoggedinUserDetail.AllowedMarketsForUser.isCricketTiedMatchAllowedForBet;

                                            }
                                            else
                                            {
                                                if (marketbookname.Contains("Winner"))
                                                {
                                                    AllowedBet = LoggedinUserDetail.AllowedMarketsForUser.isWinnerMarketAllowedForBet;

                                                }
                                                else
                                                {
                                                    AllowedBet = LoggedinUserDetail.AllowedMarketsForUser.isCricketMatchOddsAllowedForBet;

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
        public bool Allowedbetting(bool BettingAllowed, string MarketStatusstr, string MarketBookName, string MainSportsCategory, string MarketOpendate, string Runnerscount, string MarketbookID, bool checkforBettAllowed)
        {
            bool allowedbetoverall = CheckForAllowedBettingOverAll(MainSportsCategory, MarketBookName, Runnerscount);
            if (allowedbetoverall == false)
            {
                return false;
            }
            bool allowedstatus = false;
            var timeofmatch = MarketOpendate;
            var categoryname = MainSportsCategory;
            if (MainSportsCategory == "Cricket" && checkforBettAllowed == true)
            {
                bool BettingAlowedforthismarket = objUsersServiceCleint.GetBettingAllowedbyMarketIDandUserID(LoggedinUserDetail.GetUserID(), MarketbookID);
                if (BettingAlowedforthismarket == false)
                {
                    return false;
                }
            }
            else
            {
                if (BettingAllowed == false)
                {
                    return false;
                }
            }


            if (categoryname.Contains("Racing"))
            {
                if (MarketStatusstr == "IN-PLAY")
                {
                    allowedstatus = true;
                    //allowedstatus = false;
                }
                else
                {
                    if (MarketStatusstr == "ACTIVE")
                    {

                        var minutesremaining = timeofmatch.Split(':');
                        if (minutesremaining[0].Contains("-"))
                        {
                            allowedstatus = true;
                        }
                        else
                        {
                            if (MarketBookName.Contains("(US)"))
                            {
                                if (Convert.ToDouble(minutesremaining[0]) >= 0 && Convert.ToDouble(minutesremaining[2]) <= LoggedinUserDetail.BetPlaceWaitInterval.RaceMinutesBeforeStart.Value && Convert.ToDouble(minutesremaining[1]) == 0)
                                {
                                    allowedstatus = true;
                                }
                            }
                            else
                            {
                                if (Convert.ToDouble(minutesremaining[0]) >= 0 && Convert.ToDouble(minutesremaining[2]) <= LoggedinUserDetail.BetPlaceWaitInterval.RaceMinutesBeforeStart.Value && Convert.ToDouble(minutesremaining[1]) == 0)
                                {
                                    allowedstatus = true;
                                }
                            }

                        }

                    }

                }
            }
            else
            {
                //|| MarketStatusstr == "GOING LIVE" || MarketStatusstr == "Active" || MarketStatusstr == "ACTIVE"
                if (MarketStatusstr == "IN-PLAY" || MarketStatusstr == "In Play" )
                {
                    allowedstatus = true;
                }
            }
            if (MarketBookName.Contains("Winner") || MarketBookName.Contains("To Win the Toss"))
            {
                allowedstatus = true;
            }

            return allowedstatus;
        }

        public MarketBook currmarketbookforbet = new ExternalAPI.TO.MarketBook();
        string CategoryName = "";
        string MarketbooknameBet = "";
        string Marketstatusstr = "";
        bool BettingAllowed = false;
        string OpenDate = "";
        string runnerscount = "";
        string CurrentMarketBookId = "";


        private void DGVMarket_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DGVMarket.Items.Count > 0)
                {
                    DataGrid objSender = (DataGrid)sender;
                    MarketBookShow objSelectedRow = (MarketBookShow)objSender.SelectedItem;
                    int currcellindx = objSender.CurrentCell.Column.DisplayIndex;

                    CategoryName = objSelectedRow.CategoryName;
                    MarketbooknameBet = objSelectedRow.MarketbooknameBet;
                    Marketstatusstr = objSelectedRow.Marketstatusstr;
                    BettingAllowed = objSelectedRow.BettingAllowed;
                    OpenDate = objSelectedRow.OpenDate;
                    runnerscount = objSelectedRow.runnerscount;
                    CurrentMarketBookId = objSelectedRow.CurrentMarketBookId;
                    if (currcellindx == 0 && runnerscount == "1")
                    {

                        foreach (Window win in App.Current.Windows)
                        {
                            if (win.Name == "BookPositionWin" + CurrentMarketBookId.Replace(".", ""))
                            {

                                win.Close();

                            }
                        }
                        BookPosition objbookpostion = new BookPosition();
                        objbookpostion.Name = "BookPositionWin" + CurrentMarketBookId.Replace(".", "");
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            objbookpostion.CurrentUserbetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList();

                        }

                        objbookpostion.marketBookID = CurrentMarketBookId;
                        objbookpostion.marketbookName = MarketbooknameBet + "(" + lblMarketName.Content.ToString() + ")";
                        objbookpostion.UserTypeID = LoggedinUserDetail.GetUserTypeID();
                        objbookpostion.userID = LoggedinUserDetail.GetUserID();
                        objbookpostion.Show();
                        return;
                    }
                    if (CategoryName== "Soccer" || CategoryName=="Greyhound Racing" || CategoryName == "Horse Racing")
                    {
                        nupdownOdd.IsReadOnly = true;
                    }

                    if (objSelectedRow.RunnerStatusstr == "REMOVED")
                    {
                        return;
                    }

                    if ((currcellindx >= 7 && currcellindx <= 12))
                    {
                        //if (Allowedbetting(BettingAllowed, Marketstatusstr, MarketbooknameBet, CategoryName, OpenDate, runnerscount, CurrentMarketBookId, true) == true)
                        //{
                        if (LoggedinUserDetail.GetUserTypeID() == 3 && LoggedinUserDetail.isInserting == false)
                        {
                            loadedlocation = -1;
                            clickedbetsize = -1;
                            clickedodd = 0;
                            ParentID = 0;
                            SelectionID = objSelectedRow.SelectionID;
                            Selectionname = objSelectedRow.Selection;
                            if (objSelectedRow.Clothnumber != null && objSelectedRow.Clothnumber != "Not")
                            {
                                Selectionname = objSelectedRow.Clothnumber + "-" + Selectionname;
                            }
                            if (currcellindx >= 7 && currcellindx <= 9)
                            {
                                Clickedlocation = 9 - currcellindx;
                                betslipoddarea.Background = Brushes.LightSkyBlue;
                                lblBetSlipHeading.Content = "You are going to back " + objSelectedRow.Selection;
                                lblBetSelectionname.Content = objSelectedRow.Selection.ToString();
                                lblBetSlipBack.Content = objSelectedRow.Backprice0;
                                lblBetslipBackOdd1.Content = objSelectedRow.Backprice1;
                                lblBetslipBackOdd2.Content = objSelectedRow.Backprice2;
                                lblBetslipBackSize0.Content = objSelectedRow.Backsize0;
                                lblBetSlipLay.Content = objSelectedRow.Layprice0;
                                lblBetslipLaySize0.Content = objSelectedRow.Laysize0;
                                nupdownOdd.Text = objSelectedRow.Backprice0;
                                SetIncrementforNumericUpdown(Convert.ToDecimal(objSelectedRow.Backprice0));
                                BetSlipSelectionID = objSelectedRow.SelectionID;
                                BetType = "back";
                                popupBetslip.Visibility = Visibility.Visible;
                                popupBetslip.IsOpen = true;
                                calculateProfitandLossonBetSlip();

                                nupdnUserAmount.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeBack);

                            }
                            else
                            {
                                Clickedlocation = currcellindx - 10;
                                betslipoddarea.Background = Brushes.LightPink;
                                lblBetSlipHeading.Content = "You are going to lay " + objSelectedRow.Selection;
                                lblBetSelectionname.Content = objSelectedRow.Selection.ToString();
                                lblBetSlipBack.Content = objSelectedRow.Backprice0;
                                lblBetslipBackOdd1.Content = objSelectedRow.Backprice1;
                                lblBetslipBackOdd2.Content = objSelectedRow.Backprice2;
                                lblBetslipBackSize0.Content = objSelectedRow.Backsize0;
                                lblBetslipBackSize1.Content = objSelectedRow.Backsize1;
                                lblBetslipBackSize2.Content = objSelectedRow.Backsize2;

                                lblBetSlipLay.Content = objSelectedRow.Layprice0;
                                lblBetslipLayOdd1.Content = objSelectedRow.Layprice1;
                                lblBetslipLayOdd2.Content = objSelectedRow.Layprice2;
                                lblBetslipLaySize0.Content = objSelectedRow.Laysize0;
                                lblBetslipLaySize1.Content = objSelectedRow.Laysize1;
                                lblBetslipLaySize2.Content = objSelectedRow.Laysize2;
                                nupdownOdd.Text = objSelectedRow.Layprice0;
                                SetIncrementforNumericUpdown(Convert.ToDecimal(objSelectedRow.Layprice0));
                                BetSlipSelectionID = objSelectedRow.SelectionID;
                                BetType = "lay";
                                popupBetslip.Visibility = Visibility.Visible;
                                popupBetslip.IsOpen = true;
                                calculateProfitandLossonBetSlip();

                                nupdnUserAmount.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeLay);

                            }
                        }

                        //}
                        //else
                        //{
                        //    PlayMessage("Betting is not allowed");
                        //    Xceed.Wpf.Toolkit.MessageBox.Show("Betting is not allowed.", "Global Traders", MessageBoxButton.OK);

                        //}

                    }
                    if ((currcellindx < 5 && currcellindx > 0))
                    {

                        if (Marketstatusstr == "Closed" || Marketstatusstr == "Suspended")
                        {

                            return;
                        }
                        if (!MarketBook.MainSportsname.Contains("Racing"))
                        {

                            return;
                        }
                        if (objSelectedRow.isSelected == true)
                        {
                            objSelectedRow.isSelected = false;
                        }

                        else
                        {
                            if (!MarketBook.MainSportsname.Contains("Greyhound Racing"))
                            {
                                objSelectedRow.isSelected = true;
                            }
                        }
                    }
                    else
                    {

                    }

                }
            }

            catch (System.Exception ex)

            {

            }


        }
        public void SetIncrementforNumericUpdown(decimal OddValue)
        {
            if (nupdownOdd.Value > 1 && nupdownOdd.Value <= 1000)
            {

                if (nupdownOdd.Value > 1 && nupdownOdd.Value < 2)
                {
                    nupdownOdd.Increment = Convert.ToDecimal(0.01);
                }
                if (nupdownOdd.Value >= 2 && nupdownOdd.Value < 3)
                {
                    nupdownOdd.Increment = Convert.ToDecimal(0.02);

                }
                if (nupdownOdd.Value >= 3 && nupdownOdd.Value < 4)
                {
                    nupdownOdd.Increment = Convert.ToDecimal(0.05);

                }
                if (nupdownOdd.Value >= 4 && nupdownOdd.Value < 5)
                {
                    nupdownOdd.Increment = Convert.ToDecimal(0.10);

                }
                if (nupdownOdd.Value >= 5 && nupdownOdd.Value < 6)
                {
                    nupdownOdd.Increment = Convert.ToDecimal(0.20);

                }
                if (nupdownOdd.Value >= 6 && nupdownOdd.Value < 10)
                {
                    nupdownOdd.Increment = Convert.ToDecimal(0.20);

                }
                if (nupdownOdd.Value >= 10 && nupdownOdd.Value < 20)
                {
                    nupdownOdd.Increment = Convert.ToDecimal(0.50);

                }
                if (nupdownOdd.Value >= 20 && nupdownOdd.Value < 30)
                {
                    nupdownOdd.Increment = Convert.ToDecimal(1);

                }
                if (nupdownOdd.Value >= 30 && nupdownOdd.Value < 50)
                {
                    nupdownOdd.Increment = Convert.ToDecimal(2);

                }
                if (nupdownOdd.Value >= 50 && nupdownOdd.Value < 100)
                {
                    nupdownOdd.Increment = Convert.ToDecimal(5);

                }
                if (nupdownOdd.Value >= 100 && nupdownOdd.Value <= 1000)
                {
                    nupdownOdd.Increment = Convert.ToDecimal(10);

                }

            }
        }
        public void UpdateGridDatabymarketFavouriteandOtherlabels()
        {
            string selectionname = "";
            try
            {
                if (1 == 1)
                {
                    if (1 == 1)
                    {
                        if (MarketBook.MarketStatusstr != "CLOSED" && MarketBook.MarketStatusstr != "SUSPENDED")
                        {
                            //lblFavoriteBack.Content = MarketBook.FavoriteBack;
                            //lblFavoriteLay.Content = MarketBook.FavoriteLay;
                            if (lblFavoriteBack.Content.ToString() != MarketBook.FavoriteBack)
                            {
                                lblFavoriteBack.Content = MarketBook.FavoriteBack;
                            }
                            if (lblFavoriteLay.Content.ToString() != MarketBook.FavoriteLay)
                            {
                                lblFavoriteLay.Content = MarketBook.FavoriteLay;
                            }
                            if (MarketBook.MainSportsname.Contains("Horse Racing"))
                            {
                                var selectedrunner = lstMarketBookRunners.Where(item => item.SelectionID == MarketBook.FavoriteID).FirstOrDefault();
                                // var selectedrunner= MarketBook.Runners.Where(item => item.SelectionId == MarketBook.FavoriteID).FirstOrDefault();
                                if (selectedrunner.Clothnumber != null && selectedrunner.Clothnumber != "Not")
                                {
                                    lblFavoriteNAme.Content = selectedrunner.Clothnumber.ToString() + "." + selectedrunner.Selection;
                                }
                                else
                                {
                                    lblFavoriteNAme.Content = selectedrunner.Selection;
                                }
                            }
                            else
                            {
                                lblFavoriteNAme.Content = MarketBookForProfitandloss.Runners.Where(item => item.SelectionId == MarketBook.FavoriteID).FirstOrDefault().RunnerName;

                            }

                        }
                        else
                        {
                            var runnerwinner = MarketBook.Runners.Where(item => item.StatusStr == "WINNER").ToList();
                            if (runnerwinner.Count() > 0)
                            {
                                lblFavoriteNAme.Content = MarketBookForProfitandloss.Runners.Where(item => item.SelectionId == runnerwinner.FirstOrDefault().SelectionId).FirstOrDefault().RunnerName;
                            }
                            else
                            {
                                lblFavoriteNAme.Content = "";
                            }
                            lblFavoriteBack.Content = MarketBook.FavoriteBack;
                            lblFavoriteLay.Content = MarketBook.FavoriteLay;
                            lblFavoriteBack.Margin = new Thickness(40, 0, 0, 0);
                            lblFavoriteLay.Margin = new Thickness(4, 0, 0, 0);
                            return;
                        }

                        try
                        {
                            if (lstMarketBookRunners.Where(item => item.isSelected == true).Count() > 1 && MarketBook.Runners.Count > 3)
                            {
                                var lstSelectedRunners = lstMarketBookRunners.Where(item => item.isSelected == true).ToList();
                                double lastoddback = MarketBook.Runners.Where(item => item.SelectionId == lstSelectedRunners[0].SelectionID).Select(item => item.ExchangePrices.AvailableToBack[0].Price).FirstOrDefault();
                                lastoddback = lastoddback - 1;
                                double lastoddlay = Convert.ToDouble(MarketBook.Runners.Where(item => item.SelectionId == lstSelectedRunners[0].SelectionID).Select(item => item.ExchangePrices.AvailableToLay[0].Price).FirstOrDefault());
                                lastoddlay = lastoddlay - 1;


                                if (lstSelectedRunners[0].Clothnumber == "" || lstSelectedRunners[0].Clothnumber == "Not")
                                {
                                    var selectionnameactual = lstSelectedRunners[0].Selection.ToString().Split('.');
                                    // var selectionnameactual = lastloadedmarketforlabelsandother.Runners.Where(item => item.SelectionId == lstMultiplebetelection[0]).Select(item => item.RunnerName).FirstOrDefault().ToString().Split('.');
                                    selectionname = selectionnameactual[0];
                                }
                                else
                                {
                                    selectionname = lstSelectedRunners[0].Clothnumber;
                                }
                                if (lastoddback < 0)
                                {
                                    lastoddback = 0;
                                }
                                if (lastoddlay < 0)
                                {
                                    lastoddlay = 0;
                                }
                                for (int i = 0; i <= lstSelectedRunners.Count - 1; i++)
                                {
                                    if (i + 1 < lstSelectedRunners.Count)
                                    {
                                        double currentrunneroddback = 0;
                                        if (MarketBook.Runners.Where(item => item.SelectionId == lstSelectedRunners[i + 1].SelectionID).First().ExchangePrices.AvailableToBack != null)
                                        {
                                            currentrunneroddback = Convert.ToDouble(MarketBook.Runners.Where(item => item.SelectionId == lstSelectedRunners[i + 1].SelectionID).Select(item => item.ExchangePrices.AvailableToBack[0].Price).FirstOrDefault());
                                            currentrunneroddback = currentrunneroddback - 1;
                                        }
                                        double currentrunneroddlay = 0;

                                        if (MarketBook.Runners.Where(item => item.SelectionId == lstSelectedRunners[i + 1].SelectionID).First().ExchangePrices.AvailableToLay != null)
                                        {
                                            currentrunneroddlay = Convert.ToDouble(MarketBook.Runners.Where(item => item.SelectionId == lstSelectedRunners[i + 1].SelectionID).Select(item => item.ExchangePrices.AvailableToLay[0].Price).FirstOrDefault());
                                            currentrunneroddlay = currentrunneroddlay - 1;
                                        }
                                        var equation1back = (lastoddback + currentrunneroddback) + 2;
                                        var equation2back = (lastoddback * currentrunneroddback) - 1;
                                        try
                                        {
                                            lastoddback = Math.Round(equation2back / equation1back, 2);
                                        }

                                        catch (System.Exception ex)

                                        {
                                            lastoddback = 0;
                                        }

                                        if (lastoddback <= 0)
                                        {
                                            lastoddback = 0;
                                        }
                                        var equation2lay = (lastoddlay * currentrunneroddlay) - 1;

                                        var equation1lay = (lastoddlay + currentrunneroddlay) + 2;
                                        try
                                        {
                                            lastoddlay = Math.Round(equation2lay / equation1lay, 2);
                                        }

                                        catch (System.Exception ex)

                                        {
                                            lastoddlay = 0;
                                        }

                                        if (lastoddlay <= 0)
                                        {
                                            lastoddlay = 0;
                                        }

                                        if (lstSelectedRunners[i + 1].Clothnumber == "" || lstSelectedRunners[i + 1].Clothnumber == null || lstSelectedRunners[i + 1].Clothnumber == "Not")
                                        {
                                            var selectionnameactual = lstSelectedRunners[i + 1].Selection.ToString().Split('.');
                                            selectionname = selectionname + "+" + selectionnameactual[0];

                                        }
                                        else
                                        {
                                            selectionname = selectionname + "+" + lstSelectedRunners[i + 1].Clothnumber;
                                        }
                                    }
                                }
                                lblFavoriteNAme.Content = selectionname;
                                lblFavoriteBack.Content = lastoddback.ToString("F2");
                                lblFavoriteLay.Content = lastoddlay.ToString("F2");
                                betslipbackmultiple.Content = lastoddback.ToString("F2");
                                betsliplaymultiple.Content = lastoddlay.ToString("F2");
                                DGVMultipleBetsSelection.ItemsSource = lstMarketBookRunners.Where(item => item.isSelected == true).ToList();
                            }
                            else
                            {

                                DGVMultipleBetsSelection.ItemsSource = lstMarketBookRunners.Where(item => item.isSelected == true).ToList();
                            }
                            lblFavoriteNAme.Content = lblFavoriteNAme.Content.ToString().ToUpper();
                        }
                        catch (System.Exception ex)

                        {

                        }

                    }


                }
            }
            catch (System.Exception ex)

            {

            }
        }
        public void CalculateAmountsMultiple()
        {
            try
            {
                if (BetType == "lay")
                {
                    double odd = Convert.ToDouble(betsliplaymultiple.Content) + 1;

                    var amount = Convert.ToDouble(nupdnUserAmountMultiple.Value);
                    if (odd > 0)
                    {
                        betslipamountlabelMultiple.Content = ((odd * amount) - amount).ToString("F2");
                    }
                    else
                    {
                        betslipamountlabelMultiple.Content = "0.00";
                    }

                    betoddlabelMultiple.Content = (amount).ToString("F2");
                }
                else
                {
                    double odd = Convert.ToDouble(betslipbackmultiple.Content)+1;
                  
                    var amount = Convert.ToDouble(nupdnUserAmountMultiple.Value);
                    if (odd > 0)
                    {
                        betoddlabelMultiple.Content = ((odd * amount) - amount).ToString("F2");
                    }
                    else
                    {
                        betoddlabelMultiple.Content = "0.00";
                    }

                    betslipamountlabelMultiple.Content = (amount).ToString("F2");
                    
        
                }
            }

            catch (System.Exception ex)
            {

            }
        }
        public void ShowBetSlipMultiple()
        {
            if (MarketBook.MainSportsname.Contains("Racing") && MarketBook.MarketBookName.Contains("To Be Placed"))
            {
                PlayMessage("Jori Taggari not allowed in place markets.");
                Xceed.Wpf.Toolkit.MessageBox.Show("Jori Taggari not allowed in place markets.");
                return;
            }

            if (Allowedbetting(MarketBook.BettingAllowed, MarketBook.MarketStatusstr, MarketBook.MarketBookName, MarketBook.MainSportsname, MarketBook.OpenDate, MarketBook.Runners.Count.ToString(), MarketBook.MarketId, false) == true && LoggedinUserDetail.isInserting == false)
            {

                if (DGVMultipleBetsSelection.Items.Count > 1)
                {

                    if (BetType == "back")
                    {
                        lblBetSlipHeadingMultiple.Content = "You are going to place back bets.";
                        betslipoddareaMultiple.Background = Brushes.LightSkyBlue;
                        nupdnUserAmountMultiple.Value = 4000;
                        nupdnUserAmountMultiple.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeBackMultiple);
                    }
                    else
                    {
                        lblBetSlipHeadingMultiple.Content = "You are going to place lay bets.";
                        betslipoddareaMultiple.Background = Brushes.LightPink;
                        nupdnUserAmountMultiple.Value = 4000;
                        nupdnUserAmountMultiple.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeLayMultiple);
                    }
                    betslipbackmultiple.Content = lblFavoriteBack.Content;
                    betsliplaymultiple.Content = lblFavoriteLay.Content;
                    // nudownAmountMultiple.Value = (setBetslipamountlowerlimit() * 2);
                    CalculateAmountsMultiple();

                    popupBetslipMultiple.IsOpen = true;

                    currmarketbookforbet = MarketBook;
                }
                else
                {
                    PlayMessage("Please select atleast two runners.");
                    Xceed.Wpf.Toolkit.MessageBox.Show("Please select atleast two runners.");
                }
            }
            else
            {
                PlayMessage("Betting is not allowed Yet");
                Xceed.Wpf.Toolkit.MessageBox.Show("Betting not allowed Yet.");
            }
        }
        public void PlayMessage(string msg)
        {
            System.Speech.Synthesis.SpeechSynthesizer synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = 1;     // -10...10

            // Synchronous
            // synthesizer.Speak(msg);
            synthesizer.SelectVoiceByHints(System.Speech.Synthesis.VoiceGender.Neutral, System.Speech.Synthesis.VoiceAge.Adult);
            // Asynchronous
            synthesizer.SpeakAsync(msg);
        }
        private void nupdnUserOdd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                calculateProfitandLossonBetSlip();
                //if (Convert.ToDecimal(e.OldValue) > Convert.ToDecimal(e.NewValue))
                //{
                //    increment();
                //}
                //else
                //{
                //    decrement();
                //}
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

        }
        public void increment()
        {


            if (nupdownOdd.Value > 1 && nupdownOdd.Value <= 1000)
            {

                if (nupdownOdd.Value > 1 && nupdownOdd.Value < 2)
                {
                    nupdownOdd.Value = nupdownOdd.Value + Convert.ToDecimal(0.01);
                }
                if (nupdownOdd.Value >= 2 && nupdownOdd.Value < 3)
                {
                    nupdownOdd.Value = nupdownOdd.Value + Convert.ToDecimal(0.02);
                }
                if (nupdownOdd.Value >= 3 && nupdownOdd.Value < 4)
                {
                    nupdownOdd.Value = nupdownOdd.Value + Convert.ToDecimal(0.05);
                }
                if (nupdownOdd.Value >= 4 && nupdownOdd.Value < 5)
                {
                    nupdownOdd.Value = nupdownOdd.Value + Convert.ToDecimal(0.10);
                }
                if (nupdownOdd.Value >= 5 && nupdownOdd.Value < 6)
                {
                    nupdownOdd.Value = nupdownOdd.Value + Convert.ToDecimal(0.20);
                }
                if (nupdownOdd.Value >= 6 && nupdownOdd.Value < 10)
                {
                    nupdownOdd.Value = nupdownOdd.Value + Convert.ToDecimal(0.20);
                }
                if (nupdownOdd.Value >= 10 && nupdownOdd.Value < 20)
                {
                    nupdownOdd.Value = nupdownOdd.Value + Convert.ToDecimal(0.50);
                }
                if (nupdownOdd.Value >= 20 && nupdownOdd.Value < 30)
                {
                    nupdownOdd.Value = nupdownOdd.Value + Convert.ToDecimal(1);
                }
                if (nupdownOdd.Value >= 30 && nupdownOdd.Value < 50)
                {
                    nupdownOdd.Value = nupdownOdd.Value + Convert.ToDecimal(2);
                }
                if (nupdownOdd.Value >= 50 && nupdownOdd.Value < 100)
                {
                    nupdownOdd.Value = nupdownOdd.Value + Convert.ToDecimal(5);
                }
                if (nupdownOdd.Value >= 100 && nupdownOdd.Value <= 1000)
                {
                    nupdownOdd.Value = nupdownOdd.Value + Convert.ToDecimal(10);
                }

            }
            calculateProfitandLossonBetSlip();

        }
        public void decrement()
        {


            if (nupdownOdd.Value > 1 && nupdownOdd.Value <= 1000)
            {

                if (nupdownOdd.Value > 1 && nupdownOdd.Value < 2)
                {
                    nupdownOdd.Value = nupdownOdd.Value - Convert.ToDecimal(0.01);
                }
                if (nupdownOdd.Value >= 2 && nupdownOdd.Value < 3)
                {
                    nupdownOdd.Value = nupdownOdd.Value - Convert.ToDecimal(0.02);
                }
                if (nupdownOdd.Value >= 3 && nupdownOdd.Value < 4)
                {
                    nupdownOdd.Value = nupdownOdd.Value - Convert.ToDecimal(0.05);
                }
                if (nupdownOdd.Value >= 4 && nupdownOdd.Value < 5)
                {
                    nupdownOdd.Value = nupdownOdd.Value - Convert.ToDecimal(0.10);
                }
                if (nupdownOdd.Value >= 5 && nupdownOdd.Value < 6)
                {
                    nupdownOdd.Value = nupdownOdd.Value - Convert.ToDecimal(0.20);
                }
                if (nupdownOdd.Value >= 6 && nupdownOdd.Value < 10)
                {
                    nupdownOdd.Value = nupdownOdd.Value - Convert.ToDecimal(0.20);
                }
                if (nupdownOdd.Value >= 10 && nupdownOdd.Value < 20)
                {
                    nupdownOdd.Value = nupdownOdd.Value - Convert.ToDecimal(0.50);
                }
                if (nupdownOdd.Value >= 20 && nupdownOdd.Value < 30)
                {
                    nupdownOdd.Value = nupdownOdd.Value - Convert.ToDecimal(1);
                }
                if (nupdownOdd.Value >= 30 && nupdownOdd.Value < 50)
                {
                    nupdownOdd.Value = nupdownOdd.Value - Convert.ToDecimal(2);
                }
                if (nupdownOdd.Value >= 50 && nupdownOdd.Value < 100)
                {
                    nupdownOdd.Value = nupdownOdd.Value - Convert.ToDecimal(5);
                }
                if (nupdownOdd.Value >= 100 && nupdownOdd.Value <= 1000)
                {
                    nupdownOdd.Value = nupdownOdd.Value - Convert.ToDecimal(10);
                }

            }
            calculateProfitandLossonBetSlip();

        }
        public void calculateProfitandLossonBetSlip()
        {
            try
            {
                decimal odd = nupdownOdd.Value.Value;
                decimal oddamount = odd - 1;
                decimal amount = nupdnUserAmount.Value.Value;

                if (BetType == "lay")
                {
                    string betslipamount = betslipamountlabel.Content.ToString();
                    betslipamountlabel.Content = amount * oddamount;//betoddlabel.Content;
                    betoddlabel.Content = amount;
                }
                else
                {
                    betslipamountlabel.Content = amount.ToString("F2");
                    betoddlabel.Content = amount * oddamount;
                }
            }
            catch (System.Exception ex)
            {
            }
        }



        public void calculateProfitandLossonBetSlipin()
        {

            decimal odd = nupdownsizein.Value.Value;
            decimal oddamount = odd / 100;
            decimal amount = nupdnUserAmountin.Value.Value;

            if (BetType == "lay")
            {
                string betslipamount = betslipamountlabelin.Content.ToString();
                betslipamountlabelin.Content = amount * oddamount;

                betoddlabelin.Content = amount.ToString("F2");
            }
            else
            {
                betoddlabelin.Content = amount * oddamount;
                betslipamountlabelin.Content = amount.ToString("F2");
            }
        }

        public void calculateProfitandLossonBetSlipin1()
        {
            decimal odd = nupdownsizein1.Value.Value / 100;
            decimal amount = nupdnUserAmountin1.Value.Value;

            if (BetType == "lay")
            {
                string betslipamount = betslipamountlabelin1.Content.ToString();
                betslipamountlabelin1.Content = odd * amount;
                betoddlabelin1.Content = amount.ToString("F2");
            }
            else
            {
                betoddlabelin1.Content = odd * amount;
                betslipamountlabelin1.Content = amount.ToString("F2");
            }

        }

        private void nupdnUserAmount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                calculateProfitandLossonBetSlip();
            }
            catch (System.Exception ex)
            {
            }
        }
        private void nupdnUserAmountin_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                calculateProfitandLossonBetSlipin();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void nupdnUserAmountin1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                calculateProfitandLossonBetSlipin1();
            }
            catch (System.Exception ex)
            {
            }

        }

        private void btnCancelBetSlip_Click(object sender, RoutedEventArgs e)
        {
            popupBetslip.IsOpen = false;
            popupBetslipforin.IsOpen = false;
        }


        public UserServicesClient objUsersServiceCleint = new UserServicesClient();
        Sp_GetKalijut_Result KJs = new Sp_GetKalijut_Result();
        Sp_GetFigureOdds_Result FigureOdds = new Sp_GetFigureOdds_Result();
        public void GetFancyMarkets()
        {
            try
            {
                if (1 == 1)
                {

                    if (MarketBook.MarketBookName.Contains("Match Odds") && MarketBook.MainSportsname == "Cricket" && MarketBook.MarketStatusstr != "CLOSED")
                    {
                        var results = objUsersServiceCleint.GetEventDetailsbyMarketBook(MarketBook.MarketId);
                        int UserIDforLinevmarkets = 0;
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            UserIDforLinevmarkets = 73;
                        }
                        else
                        {
                            UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                        }
                        var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(results.EventID, results.EventOpenDate.Value, UserIDforLinevmarkets));
                        // var KJvmarket = JsonConvert.DeserializeObject<List<ExternalAPI.TO.SP_UserMarket_GetDistinctKJMarketsbyEventID_Result>>(objUsersServiceCleint.KJMarketsbyEventID(results.EventID, UserIDforLinevmarkets));
                        if (linevmarkets.Count() > 0)
                        {

                            linevmarkets = linevmarkets.OrderBy(x => x.MarketCatalogueName).ToList();
                            MarketBook.LineVMarkets = linevmarkets;

                            MarketBook.LineVMarkets.FirstOrDefault().AssociateeventID = results.EventID;
                             GetDataForFancy(true);
                              DGVMarketFancy.ItemsSource = lstMarketBookRunnersFancy;
                            try
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 1)
                                {
                                    dgvFAncySyncONOFF.ItemsSource = MarketBook.LineVMarkets;
                                }
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                        else
                        {
                            //DGVMarketFancy.Visibility = Visibility.Collapsed;
                        }

                        if (linevmarkets.Count() > 0)
                        {
                            try
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 1)
                                {
                                    dgvKalijuttSyncONOFF.ItemsSource = linevmarkets.Where(item => item.EventName == "Kali v Jut").ToList();
                                    //  = linevmarkets.Where(item => item.EventName == "Figure").ToList();
                                    dgvSFigSyncONOFF.ItemsSource = linevmarkets.Where(item => item.EventName == "SmallFig").ToList();
                                    dgvFGSyncONOFF.ItemsSource = linevmarkets.GroupBy(item => item.MarketCatalogueName).Select(g => g.First()).Where(item2 => item2.EventName == "Figure").ToList();
                                }
                                //GetDataForFancy(true);
                                //DGVMarketFancy.ItemsSource = lstMarketBookRunnersFancy;
                                GetDataForFancy(MarketBook.EventID, MarketBook.MarketId);
                                DGVMarketIndianFancy.ItemsSource = lstMarketBookRunnersFancyin;
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                        else
                        {
                            // DGVMarketFancy.Visibility = Visibility.Collapsed;
                        }
                        //InitiateLZCricketAPI("iplt20_2018_g6");
                    }
                    else
                    {
                        //stkpnlScores.Visibility = Visibility.Collapsed;
                        // imgScoreCard.Visibility = Visibility.Visible;
                        // DGVMarketFancy.Visibility = Visibility.Collapsed;
                    }
                }


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        public void GetDataForFancy(bool isFirstTime)
        {
            try
            {

                if (MarketBook.LineVMarkets != null)
                {

                    List<string> lstIds = MarketBook.LineVMarkets.Select(item => item.MarketCatalogueID).ToList();

                    string[] marketIds = lstIds.ToArray();
                    if (LoggedinUserDetail.GetCricketDataFrom == "BP")
                    {

                        string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataBPFancy/?marketID=" + string.Join(",", marketIds);
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RestAPIPath1);
                        request.Method = "GET";
                        request.Proxy = null;
                        request.Timeout = 5000;
                        request.KeepAlive = false;
                        request.ServicePoint.ConnectionLeaseTimeout = 5000;
                        request.ServicePoint.MaxIdleTime = 5000;
                        request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                        String test = String.Empty;
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            using (Stream dataStream = response.GetResponseStream())
                            {

                                System.Runtime.Serialization.Json.DataContractJsonSerializer obj1 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(string));
                                test = obj1.ReadObject(dataStream).ToString();

                                dataStream.Close();
                            }

                        }

                        var list = JsonConvert.DeserializeObject<List<SampleResponse1>>(test);

                        if (list.Count() > 0)
                        {


                            List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
                            foreach (var bfobject in MarketBook.LineVMarkets)
                            {
                                if (bfobject != null)
                                {
                                    try
                                    {
                                        SampleResponse1 objmarketbookBF1 = list.Where(item => item.MarketId == bfobject.MarketCatalogueID).FirstOrDefault();
                                        if (objmarketbookBF1 != null)
                                        {
                                            LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBF123Fancy(objmarketbookBF1, bfobject.MarketCatalogueID, MarketBook.OrignalOpenDate.Value, bfobject.MarketCatalogueName, "Cricket", false, bfobject.BettingAllowed));
                                        }


                                    }
                                    catch (System.Exception ex)
                                    {

                                    }


                                }



                            }
                            LastloadedLinMarkets = LastloadedLinMarkets1;
                            if (isFirstTime == true)
                            {
                                lstMarketBookRunnersFancy = new ObservableCollection<MarketBookShow>();
                                lstMarketBookRunnersFancy.Clear();
                                foreach (var item in LastloadedLinMarkets)
                                {
                                    try
                                    {
                                        item.Runners[0].RunnerName = item.MarketBookName;



                                        if ((item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 20 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 20) && item.MarketStatusstr == "In Play")
                                        {
                                            item.Runners[0].isShow = true;
                                        }
                                        else
                                        {
                                            item.Runners[0].isShow = false;
                                        }
                                        GetRunnersDataSourceFancyFirstTime(item.Runners, item);
                                    }
                                    catch (System.Exception ex)
                                    {

                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        if (LoggedinUserDetail.GetCricketDataFrom == "Live")
                        {

                            //string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataFancyStr/?marketID=" + string.Join(",", marketIds);
                            string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataFancy/?marketID=" + string.Join(",", marketIds);
                            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RestAPIPath1);
                            request.Method = "GET";
                            request.Proxy = null;
                            request.Timeout = 5000;
                            request.KeepAlive = false;
                            request.ServicePoint.ConnectionLeaseTimeout = 5000;
                            request.ServicePoint.MaxIdleTime = 5000;
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            List<MarketBook> test = new List<MarketBook>();
                            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                            {
                                using (Stream dataStream = response.GetResponseStream())
                                {

                                    System.Runtime.Serialization.Json.DataContractJsonSerializer obj1 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<MarketBook>));
                                    test = (List<MarketBook>)obj1.ReadObject(dataStream);

                                    dataStream.Close();
                                }
                            }

                            var list1 = test;
                            if (list1.Count() > 0)
                            {


                                List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
                                foreach (var bfobject in MarketBook.LineVMarkets)
                                {
                                    if (bfobject != null)
                                    {
                                        try
                                        {
                                            MarketBook objmarketbookBF1 = list1.Where(item => item.MarketId == bfobject.MarketCatalogueID).FirstOrDefault();
                                            if (objmarketbookBF1 != null)
                                            {
                                                LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBFFancyLive(objmarketbookBF1, bfobject.MarketCatalogueID, MarketBook.OrignalOpenDate.Value, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed));
                                            }


                                        }
                                        catch (System.Exception ex)
                                        {

                                        }


                                    }



                                }
                                LastloadedLinMarkets = LastloadedLinMarkets1;
                                if (isFirstTime == true)
                                {
                                    lstMarketBookRunnersFancy = new ObservableCollection<MarketBookShow>();
                                    lstMarketBookRunnersFancy.Clear();
                                    foreach (var item in LastloadedLinMarkets)
                                    {
                                        try
                                        {
                                            item.Runners[0].RunnerName = item.MarketBookName;
                                            if ((item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 20 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 20) && item.MarketStatusstr == "In Play")
                                            {
                                                item.Runners[0].isShow = true;
                                            }
                                            else
                                            {
                                                item.Runners[0].isShow = false;
                                            }
                                            GetRunnersDataSourceFancyFirstTime(item.Runners, item);
                                        }
                                        catch (System.Exception ex)
                                        {

                                        }
                                    }
                                }
                            }

                        }
                        else
                        {


                            if (LoggedinUserDetail.GetCricketDataFrom == "Other")
                            {
                                string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataOtherFancy/?marketID=" + string.Join(",", marketIds);
                                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RestAPIPath1);
                                request.Method = "GET";
                                request.Proxy = null;
                                request.Timeout = 5000;
                                request.ReadWriteTimeout = 30000;
                                request.KeepAlive = false;
                                request.ServicePoint.ConnectionLeaseTimeout = 5000;
                                request.ServicePoint.MaxIdleTime = 5000;
                                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                                List<ExternalAPI.TO.MarketBookString> test = new List<ExternalAPI.TO.MarketBookString>();
                                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                                {
                                    using (Stream dataStream = response.GetResponseStream())
                                    {

                                        System.Runtime.Serialization.Json.DataContractJsonSerializer obj1 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<ExternalAPI.TO.MarketBookString>));
                                        test = (List<ExternalAPI.TO.MarketBookString>)obj1.ReadObject(dataStream);

                                        dataStream.Close();
                                    }

                                }

                                var list1 = test;
                                if (list1.Count() > 0)
                                {


                                    List<ExternalAPI.TO.MarketBook> LastloadedLinMarkets1 = new List<ExternalAPI.TO.MarketBook>();
                                    foreach (var bfobject in MarketBook.LineVMarkets)
                                    {
                                        if (bfobject != null)
                                        {
                                            try
                                            {
                                                ExternalAPI.TO.MarketBookString objmarketbookBF1 = list1.Where(item => item.MarketBookId == bfobject.MarketCatalogueID).FirstOrDefault();
                                                if (objmarketbookBF1 != null)
                                                {
                                                    LastloadedLinMarkets1.Add(ConvertJsontoMarketObjectBFNewSource(objmarketbookBF1, bfobject.MarketCatalogueID, MarketBook.OrignalOpenDate.Value, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed));
                                                }


                                            }
                                            catch (System.Exception ex)
                                            {

                                            }
                                        }
                                    }
                                    LastloadedLinMarkets = LastloadedLinMarkets1;
                                    if (isFirstTime == true)
                                    {
                                        lstMarketBookRunnersFancy = new ObservableCollection<MarketBookShow>();
                                        lstMarketBookRunnersFancy.Clear();
                                        foreach (var item in LastloadedLinMarkets)
                                        {
                                            try
                                            {
                                                item.Runners[0].RunnerName = item.MarketBookName;



                                                if ((item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 20 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 20) && item.MarketStatusstr == "In Play")
                                                {
                                                    item.Runners[0].isShow = true;
                                                }
                                                else
                                                {
                                                    item.Runners[0].isShow = false;
                                                }
                                                GetRunnersDataSourceFancyFirstTime(item.Runners, item);
                                            }
                                            catch (System.Exception ex)
                                            {

                                            }
                                        }
                                    }
                                }

                            }
                        }
                        return;

                    }
                }
            }
            catch (System.Exception ex)
            {

            }

        }
        public void GetTowWinTheTossMarket()
        {
            try
            {
                if (1 == 1)
                {
                    if (MarketBook.MarketBookName.Contains("Match Odds") && MarketBook.MainSportsname == "Cricket" && MarketBook.MarketStatusstr != "CLOSED")
                    {
                        var results = objUsersServiceCleint.GetEventDetailsbyMarketBook(MarketBook.MarketId);
                        int UserIDforLinevmarkets = 0;
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            UserIDforLinevmarkets = 73;
                        }
                        else
                        {
                            UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                        }
                        var wintethossmarket = (objUsersServiceCleint.GetToWintheTossbyeventId(UserIDforLinevmarkets, results.EventID));
                        // panelWidgets.Visible = true;
                        // webBrowserWidget.Navigate(ConfigurationManager.AppSettings["WidgetURL"]);
                        if (wintethossmarket != null)
                        {
                            if (wintethossmarket.MarketCatalogueID != null)
                            {
                                objUsersServiceCleint.SetMarketBookOpenbyUSerandGetAsync(UserIDforLinevmarkets, wintethossmarket.MarketCatalogueID);
                                objUsersServiceCleint.SetMarketBookOpenbyUSerandGetCompleted += ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGetCompleted; ;
                            }


                        }
                        else
                        {

                        }
                        //InitiateLZCricketAPI("iplt20_2018_g6");
                    }
                    else
                    {

                    }
                }


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        public void GetTiedMarket()
        {
            try
            {
                if (1 == 1)
                {
                    if (MarketBook.MarketBookName.Contains("Match Odds") && MarketBook.MainSportsname == "Cricket" && MarketBook.MarketStatusstr != "CLOSED")
                    {
                        var results = objUsersServiceCleint.GetEventDetailsbyMarketBook(MarketBook.MarketId);
                        int UserIDforLinevmarkets = 0;
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            UserIDforLinevmarkets = 73;
                        }
                        else
                        {
                            UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                        }
                        var tiedmarket = (objUsersServiceCleint.GetToTiedMarketbyEventID(UserIDforLinevmarkets, results.EventID));
                        // panelWidgets.Visible = true;
                        // webBrowserWidget.Navigate(ConfigurationManager.AppSettings["WidgetURL"]);
                        if (tiedmarket != null)
                        {
                            if (tiedmarket.MarketCatalogueID != null)
                            {
                                objUsersServiceCleint.SetMarketBookOpenbyUSerandGetAsync(UserIDforLinevmarkets, tiedmarket.MarketCatalogueID);
                                objUsersServiceCleint.SetMarketBookOpenbyUSerandGetCompleted += ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGetCompleted; ;
                            }


                        }
                        else
                        {

                        }
                        //InitiateLZCricketAPI("iplt20_2018_g6");
                    }
                    else
                    {

                    }
                }


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }


        private void ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGetCompleted(object sender, SetMarketBookOpenbyUSerandGetCompletedEventArgs e)
        {
            objUsersServiceCleint.SetMarketBookOpenbyUSerandGetCompleted -= ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGetCompleted;
            try
            {
                var results = JsonConvert.DeserializeObject<List<MarketCatalogueandSelectionNames>>(e.Result);
                if (results.Count == 0)
                {

                    return;
                }
                var newmarkettobeopened = results;
                List<string> lstIDs = new List<string>();

                var newmarketbook = new ExternalAPI.TO.MarketBook();

                ExternalAPI.TO.MarketBook marketbook1;
                marketbook1 = GetCurrentMarketBook(newmarkettobeopened[0].ID, newmarkettobeopened[0].Name, newmarkettobeopened[0].EventTypeName, newmarkettobeopened[0].EventOpenDate, newmarkettobeopened[0].BettingAllowed);

                if (marketbook1.MarketId != null)
                {
                    newmarketbook = (marketbook1);
                    newmarketbook.MarketBookName = newmarkettobeopened[0].Name + " / " + newmarkettobeopened[0].EventName;
                    newmarketbook.OrignalOpenDate = newmarkettobeopened[0].EventOpenDate;
                    newmarketbook.MainSportsname = newmarkettobeopened[0].EventTypeName;
                    newmarketbook.BettingAllowed = newmarkettobeopened[0].BettingAllowed;
                    newmarketbook.GetMatchUpdatesFrom = newmarkettobeopened[0].GetMatchUpdatesFrom;
                    newmarketbook.EventID = newmarkettobeopened[0].EventID;
                    if (1 == 1)
                    {

                        if (1 == 2)
                        {

                        }
                        else
                        {
                            foreach (var runnermarketitem in newmarkettobeopened)
                            {
                                var runneritem = newmarketbook.Runners.Where(item => item.SelectionId == runnermarketitem.SelectionID).First();

                                runneritem.SelectionId = runnermarketitem.SelectionID;
                                runneritem.RunnerName = runnermarketitem.SelectionName;
                                runneritem.JockeyName = runnermarketitem.JockeyName;
                                runneritem.WearingURL = runnermarketitem.Wearing;
                                runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                runneritem.StallDraw = runnermarketitem.StallDraw;
                            }
                        }
                    }
                    ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
                    objmarketbook = newmarketbook;
                    if (objmarketbook != null)
                    {
                        objmarketbook.isWinTheTossMarket = true;
                        if (objmarketbook.MarketStatusstr == "Closed")
                        {
                            return;
                        }
                        MarketBookWintheToss = objmarketbook;
                        MarketBookForProfitandlossToWinTheToss.MarketBookName = objmarketbook.MarketBookName;
                        MarketBookForProfitandlossToWinTheToss.MarketId = objmarketbook.MarketId;
                        MarketBookForProfitandlossToWinTheToss.Runners = new List<ExternalAPI.TO.Runner>();
                        foreach (var runneritem in objmarketbook.Runners)
                        {
                            ExternalAPI.TO.Runner objNewRunner = new ExternalAPI.TO.Runner();
                            objNewRunner.SelectionId = runneritem.SelectionId;
                            objNewRunner.Handicap = runneritem.Handicap;
                            MarketBookForProfitandlossToWinTheToss.Runners.Add(objNewRunner);
                        }

                        LoggedinUserDetail.MarketBooks.Add(objmarketbook);
                        if (MarketBookWintheToss.Runners != null)
                        {
                            GetRunnersDataSourceToWintheToss(MarketBookWintheToss.Runners, MarketBookWintheToss);
                            DGVMarketToWintheToss.ItemsSource = lstMarketBookRunnersToWintheToss;
                        }

                    }


                }
                else
                {

                }

            }
            catch (System.Exception ex)
            {

            }
        }

        private void ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGet0Completed(object sender, SetMarketBookOpenbyUSerandGet0CompletedEventArgs e)
        {
            objUsersServiceCleint.SetMarketBookOpenbyUSerandGet0Completed -= ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGet0Completed;
            try
            {
                var results = JsonConvert.DeserializeObject<List<MarketCatalogueandSelectionNames>>(e.Result);
                if (results.Count == 0)
                {

                    return;
                }
                var newmarkettobeopened = results;
                List<string> lstIDs = new List<string>();

                var newmarketbook = new ExternalAPI.TO.MarketBook();

                ExternalAPI.TO.MarketBook marketbook1;
                marketbook1 = GetCurrentMarketBook(newmarkettobeopened[0].ID, newmarkettobeopened[0].Name, newmarkettobeopened[0].EventTypeName, newmarkettobeopened[0].EventOpenDate, newmarkettobeopened[0].BettingAllowed);

                if (marketbook1.MarketId != null)
                {
                    newmarketbook = (marketbook1);
                    newmarketbook.MarketBookName = newmarkettobeopened[0].Name + " / " + newmarkettobeopened[0].EventName;
                    newmarketbook.OrignalOpenDate = newmarkettobeopened[0].EventOpenDate;
                    newmarketbook.MainSportsname = newmarkettobeopened[0].EventTypeName;
                    newmarketbook.BettingAllowed = newmarkettobeopened[0].BettingAllowed;
                    newmarketbook.GetMatchUpdatesFrom = newmarkettobeopened[0].GetMatchUpdatesFrom;
                    newmarketbook.EventID = newmarkettobeopened[0].EventID;


                    if (1 == 1)
                    {

                        if (1 == 2)
                        {

                        }
                        else
                        {
                            foreach (var runnermarketitem in newmarkettobeopened)
                            {

                                var runneritem = newmarketbook.Runners.Where(item => item.SelectionId == runnermarketitem.SelectionID).First();

                                runneritem.SelectionId = runnermarketitem.SelectionID;
                                runneritem.RunnerName = runnermarketitem.SelectionName;
                                runneritem.JockeyName = runnermarketitem.JockeyName;
                                runneritem.WearingURL = runnermarketitem.Wearing;
                                runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                runneritem.StallDraw = runnermarketitem.StallDraw;
                            }
                        }
                    }

                    ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
                    objmarketbook = newmarketbook;
                    if (objmarketbook != null)
                    {
                        objmarketbook.isWinTheTossMarket = true;
                        if (objmarketbook.MarketStatusstr == "Closed")
                        {
                            return;
                        }
                        MarketBookWintheToss0 = objmarketbook;
                        MarketBookForProfitandlossToWinTheToss0.MarketBookName = objmarketbook.MarketBookName;
                        MarketBookForProfitandlossToWinTheToss0.MarketId = objmarketbook.MarketId;
                        MarketBookForProfitandlossToWinTheToss0.Runners = new List<ExternalAPI.TO.Runner>();
                        foreach (var runneritem in objmarketbook.Runners)
                        {
                            ExternalAPI.TO.Runner objNewRunner = new ExternalAPI.TO.Runner();
                            objNewRunner.SelectionId = runneritem.SelectionId;
                            objNewRunner.Handicap = runneritem.Handicap;
                            MarketBookForProfitandlossToWinTheToss0.Runners.Add(objNewRunner);
                        }

                        LoggedinUserDetail.MarketBooks.Add(objmarketbook);
                        if (MarketBookWintheToss0.Runners != null)
                        {
                            GetRunnersDataSourceToWintheToss0(MarketBookWintheToss0.Runners, MarketBookWintheToss0);
                            // DGVMarketSoccer.ItemsSource = lstMarketBookRunnersToWintheToss0;
                        }

                    }


                }
                else
                {

                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        //        private void ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGet1Completed(object sender, SetMarketBookOpenbyUSerandGet1CompletedEventArgs e)
        //        {
        //            objUsersServiceCleint.SetMarketBookOpenbyUSerandGet1Completed -= ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGet1Completed;
        //            try
        //            {
        //                var results = JsonConvert.DeserializeObject<List<MarketCatalogueandSelectionNames>>(e.Result);
        //                if (results.Count == 0)
        //                {

        //                    return;
        //                }
        //                var newmarkettobeopened = results;
        //                List<string> lstIDs = new List<string>();

        //                var newmarketbook = new ExternalAPI.TO.MarketBook();

        //                ExternalAPI.TO.MarketBook marketbook1;
        //                marketbook1 = GetCurrentMarketBook(newmarkettobeopened[0].ID, newmarkettobeopened[0].Name, newmarkettobeopened[0].EventTypeName, newmarkettobeopened[0].EventOpenDate, newmarkettobeopened[0].BettingAllowed);

        //                if (marketbook1.MarketId != null)
        //                {
        //                    newmarketbook = (marketbook1);
        //                    newmarketbook.MarketBookName = newmarkettobeopened[0].Name + " / " + newmarkettobeopened[0].EventName;
        //                    newmarketbook.OrignalOpenDate = newmarkettobeopened[0].EventOpenDate;
        //                    newmarketbook.MainSportsname = newmarkettobeopened[0].EventTypeName;
        //                    newmarketbook.BettingAllowed = newmarkettobeopened[0].BettingAllowed;
        //                    newmarketbook.GetMatchUpdatesFrom = newmarkettobeopened[0].GetMatchUpdatesFrom;
        //                    newmarketbook.EventID = newmarkettobeopened[0].EventID;


        //                    if (1 == 1)
        //                    {

        //                        if (1 == 2)
        //                        {

        //                        }
        //                        else
        //                        {
        //                            foreach (var runnermarketitem in newmarkettobeopened)
        //                            {

        //                                var runneritem = newmarketbook.Runners.Where(item => item.SelectionId == runnermarketitem.SelectionID).First();

        //                                runneritem.SelectionId = runnermarketitem.SelectionID;
        //                                runneritem.RunnerName = runnermarketitem.SelectionName;
        //                                runneritem.JockeyName = runnermarketitem.JockeyName;
        //                                runneritem.WearingURL = runnermarketitem.Wearing;
        //                                runneritem.WearingDesc = runnermarketitem.WearingDesc;
        //                                runneritem.Clothnumber = runnermarketitem.ClothNumber;
        //                                runneritem.StallDraw = runnermarketitem.StallDraw;
        //                            }
        //                        }
        //                    }

        //                    ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
        //                    objmarketbook = newmarketbook;
        //                    if (objmarketbook != null)
        //                    {
        //                        objmarketbook.isWinTheTossMarket = true;
        //                        if (objmarketbook.MarketStatusstr == "Closed")
        //                        {


        //                            return;
        //                        }
        //                        MarketBookWintheToss1 = objmarketbook;
        //                        MarketBookForProfitandlossToWinTheToss1.MarketBookName = objmarketbook.MarketBookName;
        //                        MarketBookForProfitandlossToWinTheToss1.MarketId = objmarketbook.MarketId;
        //                        MarketBookForProfitandlossToWinTheToss1.Runners = new List<ExternalAPI.TO.Runner>();
        //                        foreach (var runneritem in objmarketbook.Runners)
        //                        {
        //                            ExternalAPI.TO.Runner objNewRunner = new ExternalAPI.TO.Runner();
        //                            objNewRunner.SelectionId = runneritem.SelectionId;
        //                            objNewRunner.Handicap = runneritem.Handicap;
        //                            MarketBookForProfitandlossToWinTheToss1.Runners.Add(objNewRunner);
        //                        }

        //                        LoggedinUserDetail.MarketBooks.Add(objmarketbook);
        //                        if (MarketBookWintheToss1.Runners != null)
        //                        {
        //                            GetRunnersDataSourceToWintheToss1(MarketBookWintheToss1.Runners, MarketBookWintheToss1);
        //                            DGVMarketToWintheToss1.ItemsSource = lstMarketBookRunnersToWintheToss1;
        //                        }

        //                    }


        //                }
        //                else
        //                {

        //                }

        //            }
        //#pragma warning disable CS0168 // The variable 'ex' is declared but never used
        //            catch (System.Exception ex)
        //#pragma warning restore CS0168 // The variable 'ex' is declared but never used
        //            {

        //            }
        //        }

        //        private void ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGet2Completed(object sender, SetMarketBookOpenbyUSerandGet2CompletedEventArgs e)
        //        {
        //            objUsersServiceCleint.SetMarketBookOpenbyUSerandGet2Completed -= ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGet2Completed;
        //            try
        //            {
        //                var results = JsonConvert.DeserializeObject<List<MarketCatalogueandSelectionNames>>(e.Result);
        //                if (results.Count == 0)
        //                {

        //                    return;
        //                }
        //                var newmarkettobeopened = results;
        //                List<string> lstIDs = new List<string>();

        //                var newmarketbook = new ExternalAPI.TO.MarketBook();

        //                ExternalAPI.TO.MarketBook marketbook1;
        //                marketbook1 = GetCurrentMarketBook(newmarkettobeopened[0].ID, newmarkettobeopened[0].Name, newmarkettobeopened[0].EventTypeName, newmarkettobeopened[0].EventOpenDate, newmarkettobeopened[0].BettingAllowed);

        //                if (marketbook1.MarketId != null)
        //                {
        //                    newmarketbook = (marketbook1);
        //                    newmarketbook.MarketBookName = newmarkettobeopened[0].Name + " / " + newmarkettobeopened[0].EventName;
        //                    newmarketbook.OrignalOpenDate = newmarkettobeopened[0].EventOpenDate;
        //                    newmarketbook.MainSportsname = newmarkettobeopened[0].EventTypeName;
        //                    newmarketbook.BettingAllowed = newmarkettobeopened[0].BettingAllowed;
        //                    newmarketbook.GetMatchUpdatesFrom = newmarkettobeopened[0].GetMatchUpdatesFrom;
        //                    newmarketbook.EventID = newmarkettobeopened[0].EventID;


        //                    if (1 == 1)
        //                    {

        //                        if (1 == 2)
        //                        {

        //                        }
        //                        else
        //                        {
        //                            foreach (var runnermarketitem in newmarkettobeopened)
        //                            {

        //                                var runneritem = newmarketbook.Runners.Where(item => item.SelectionId == runnermarketitem.SelectionID).First();

        //                                runneritem.SelectionId = runnermarketitem.SelectionID;
        //                                runneritem.RunnerName = runnermarketitem.SelectionName;
        //                                runneritem.JockeyName = runnermarketitem.JockeyName;
        //                                runneritem.WearingURL = runnermarketitem.Wearing;
        //                                runneritem.WearingDesc = runnermarketitem.WearingDesc;
        //                                runneritem.Clothnumber = runnermarketitem.ClothNumber;
        //                                runneritem.StallDraw = runnermarketitem.StallDraw;



        //                            }
        //                        }


        //                    }
        //                    ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
        //                    objmarketbook = newmarketbook;
        //                    if (objmarketbook != null)
        //                    {
        //                        objmarketbook.isWinTheTossMarket = true;
        //                        if (objmarketbook.MarketStatusstr == "Closed")
        //                        {


        //                            return;
        //                        }
        //                        MarketBookWintheToss2 = objmarketbook;
        //                        MarketBookForProfitandlossToWinTheToss2.MarketBookName = objmarketbook.MarketBookName;
        //                        MarketBookForProfitandlossToWinTheToss2.MarketId = objmarketbook.MarketId;
        //                        MarketBookForProfitandlossToWinTheToss2.Runners = new List<ExternalAPI.TO.Runner>();
        //                        foreach (var runneritem in objmarketbook.Runners)
        //                        {
        //                            ExternalAPI.TO.Runner objNewRunner = new ExternalAPI.TO.Runner();
        //                            objNewRunner.SelectionId = runneritem.SelectionId;
        //                            objNewRunner.Handicap = runneritem.Handicap;
        //                            MarketBookForProfitandlossToWinTheToss2.Runners.Add(objNewRunner);
        //                        }

        //                        LoggedinUserDetail.MarketBooks.Add(objmarketbook);
        //                        if (MarketBookWintheToss2.Runners != null)
        //                        {
        //                            GetRunnersDataSourceToWintheToss2(MarketBookWintheToss2.Runners, MarketBookWintheToss2);
        //                            DGVMarketToWintheToss2.ItemsSource = lstMarketBookRunnersToWintheToss2;
        //                        }

        //                    }


        //                }
        //                else
        //                {

        //                }

        //            }
        //#pragma warning disable CS0168 // The variable 'ex' is declared but never used
        //            catch (System.Exception ex)
        //#pragma warning restore CS0168 // The variable 'ex' is declared but never used
        //            {

        //            }
        //        }





        List<string> Soccergoalmarkets = new List<string>();
        public void GetToSoccerGoalMarket()
        {
            try
            {

                if (1 == 1)
                {

                    if (MarketBook.MarketBookName.Contains("Match Odds") && MarketBook.MainSportsname == "Soccer" && MarketBook.MarketStatusstr != "CLOSED")
                    {
                        var results = objUsersServiceCleint.GetEventDetailsbyMarketBook(MarketBook.MarketId);
                        int UserIDforLinevmarkets = 0;
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            UserIDforLinevmarkets = 73;
                        }
                        else
                        {
                            UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                        }
                        var Soccergoalmarket = objUsersServiceCleint.GetSoccergoalbyeventId(UserIDforLinevmarkets, results.EventID);

                        if (Soccergoalmarket != null)
                        {
                            foreach (var item in Soccergoalmarket)
                            {
                                Soccergoalmarkets.Add(item.MarketCatalogueID);
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
                        // Soccergoalmarket = Soccergoalmarket.Where(item => item.MarketCatalogueName != "").ToArray();

                        if (Soccergoalmarket != null)
                        {
                            if (Soccergoalmarket[0] != null)
                            {

                                objUsersServiceCleint.SetMarketBookOpenbyUSerandGetAsync(UserIDforLinevmarkets, Soccergoalmarket[0].MarketCatalogueID);
                                objUsersServiceCleint.SetMarketBookOpenbyUSerandGetCompleted += ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGetCompleted; ;

                            }
                            DGVMarketToWintheToss.ItemsSource = lstMarketBookRunnersToWintheToss;



                        }
                        else
                        {

                        }

                    }
                    else
                    {

                    }
                }


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }


        int BetPlaceWait = 0;
        int BetWaitTimerInterval = 0;
        public void SetBetPlacewaitTimerandInterval(string categoryname, string marketbookname, string runnerscount)
        {
            //if (runnerscount == "1")
            //{
            //    BetPlaceWait = LoggedinUserDetail.BetPlaceWaitInterval.FancyBetPlaceWait.Value;
            //    BetWaitTimerInterval = LoggedinUserDetail.BetPlaceWaitInterval.FancyTimerInterval.Value;
            //}
            //else
            //{
            if (categoryname.Contains("Horse Racing"))
            {
                BetPlaceWait = LoggedinUserDetail.BetPlaceWaitInterval.HorseRaceBetPlaceWait;
                BetWaitTimerInterval = LoggedinUserDetail.BetPlaceWaitInterval.HorseRaceTimerInterval;

            }
            else
            {

                if (categoryname.Contains("Greyhound Racing"))
                {
                    BetPlaceWait = LoggedinUserDetail.BetPlaceWaitInterval.GrayHoundBetPlaceWait;
                    BetWaitTimerInterval = LoggedinUserDetail.BetPlaceWaitInterval.GrayHoundTimerInterval;

                }
                else
                {

                    if (marketbookname.Contains("Completed Match"))
                    {
                        BetPlaceWait = LoggedinUserDetail.BetPlaceWaitInterval.CompletedMatchBetPlaceWait;
                        BetWaitTimerInterval = LoggedinUserDetail.BetPlaceWaitInterval.CompletedMatchTimerInterval;

                    }
                    else
                    {
                        if (marketbookname.Contains("Innings Runs") || marketbookname.Contains("Inns Runs"))
                        {
                            BetPlaceWait = LoggedinUserDetail.BetPlaceWaitInterval.InningsRunsBetPlaceWait;
                            BetWaitTimerInterval = LoggedinUserDetail.BetPlaceWaitInterval.InningsRunsTimerInterval;

                        }
                        else
                        {
                            if (categoryname == "Tennis")
                            {
                                BetPlaceWait = LoggedinUserDetail.BetPlaceWaitInterval.TennisBetPlaceWait;
                                BetWaitTimerInterval = LoggedinUserDetail.BetPlaceWaitInterval.TennisTimerInterval;

                            }
                            else
                            {
                                if (categoryname == "Soccer")
                                {
                                    BetPlaceWait = LoggedinUserDetail.BetPlaceWaitInterval.SoccerBetPlaceWait;
                                    BetWaitTimerInterval = LoggedinUserDetail.BetPlaceWaitInterval.SoccerTimerInterval;

                                }
                                else
                                {
                                    if (marketbookname.Contains("Tied Match"))
                                    {
                                        BetPlaceWait = LoggedinUserDetail.BetPlaceWaitInterval.TiedMatchBetPlaceWait;
                                        BetWaitTimerInterval = LoggedinUserDetail.BetPlaceWaitInterval.TiedMatchTimerInterval;
                                    }
                                    else
                                    {
                                        if (marketbookname.Contains("Winner") || marketbookname.Contains("To Win the Toss"))
                                        {
                                            BetPlaceWait = LoggedinUserDetail.BetPlaceWaitInterval.WinnerBetPlaceWait;
                                            BetWaitTimerInterval = LoggedinUserDetail.BetPlaceWaitInterval.WinnerTimerInterval;
                                        }
                                        else
                                        {

                                            BetPlaceWait = LoggedinUserDetail.BetPlaceWaitInterval.CricketMatchOddsBetPlaceWait;
                                            BetWaitTimerInterval = LoggedinUserDetail.BetPlaceWaitInterval.CricketMatchOddsTimerInterval;

                                        }
                                    }
                                }
                            }
                        }
                    }
                    //}
                }
            }

        }
        public void ShowProgressBar()
        {
            tbLiabality.Visibility = Visibility.Collapsed;
            if (MarketBook.MainSportsname.Contains("Racing"))
            {
                Duration duration = new System.Windows.Duration(TimeSpan.FromMilliseconds(BetWaitTimerInterval * 1));
                DoubleAnimation doubleanimaiton = new DoubleAnimation(0, 100, duration);
                doubleanimaiton.RepeatBehavior = new RepeatBehavior(1);
                //updateProgressBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimaiton);
                bsyindicator.IsBusy = true;
                bsyindicator.BeginAnimation(ProgressBar.ValueProperty, doubleanimaiton);
                // progressGrid.Visibility = Visibility.Visible;
            }
            else
            {
                Duration duration = new System.Windows.Duration(TimeSpan.FromMilliseconds(BetWaitTimerInterval * 5));
                DoubleAnimation doubleanimaiton = new DoubleAnimation(0, 100, duration);
                doubleanimaiton.RepeatBehavior = new RepeatBehavior(1);
                //updateProgressBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimaiton);
                bsyindicator.IsBusy = true;
                bsyindicator.BeginAnimation(ProgressBar.ValueProperty, doubleanimaiton);
                //progressGrid.Visibility = Visibility.Visible;
            }

        }
        public void HideProgressBar()
        {
            bsyindicator.IsBusy = false;
            //progressGrid.Visibility = Visibility.Collapsed;
            tbLiabality.Visibility = Visibility.Visible;
        }
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (LoggedinUserDetail.isInserting == true)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Please wait for other bet completion.");
                    return;
                }
                if (Clickedlocation != 8)
                {
                    if (CurrentMarketBookId != currmarketbookforbet.MarketId)
                    {
                        popupBetslip.IsOpen = false;
                        Xceed.Wpf.Toolkit.MessageBox.Show("Please open bet slip again.");
                        return;
                    }
                }
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    if (currmarketbookforbet.IsMarketDataDelayed == true)
                    {
                        PlayMessage("Betfair data is delay, Please bet later.");
                        Xceed.Wpf.Toolkit.MessageBox.Show("Betfair data is delay, Please bet later.");
                        return;
                    }
                    //calculateProfitandLossonBetSlip();


                    SetBetPlacewaitTimerandInterval(currmarketbookforbet.MainSportsname, currmarketbookforbet.MarketBookName, currmarketbookforbet.Runners.Count.ToString());


                    // ShowProgressBar();
                    if (Allowedbetting(currmarketbookforbet.BettingAllowed, currmarketbookforbet.MarketStatusstr, currmarketbookforbet.MarketBookName, currmarketbookforbet.MainSportsname, currmarketbookforbet.OpenDate, currmarketbookforbet.Runners.Count.ToString(), currmarketbookforbet.MarketId, true) == true)
                    {

                        popupBetslip.IsOpen = false;
                        LoggedinUserDetail.isInserting = true;

                        btnSubmitBetSlip.IsEnabled = false;
                        btnResetBetSlip.IsEnabled = false;
                        //ShowLoader;
                        decimal betamounttobeplaced = nupdnUserAmount.Value.Value;
                        decimal lowerbetlimit = 2000;
                        decimal betupperlimit = 500000;
                        string categoryname = currmarketbookforbet.MainSportsname;
                        string marketbookname = currmarketbookforbet.MarketBookName;
                        if (categoryname.Contains("Horse Racing") && !marketbookname.Contains("To Be Placed"))
                        {
                            lowerbetlimit = LoggedinUserDetail.GetBetLowerLimit();
                            betupperlimit = LoggedinUserDetail.GetBetUpperLimit();
                        }
                        else
                        {
                            if (categoryname.Contains("Horse Racing") && marketbookname.Contains("To Be Placed"))
                            {
                                lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitHorsePlace();
                                betupperlimit = LoggedinUserDetail.GetBetUpperLimitHorsePlace();
                            }
                            else
                            {
                                if (categoryname.Contains("Greyhound Racing") && marketbookname.Contains("To Be Placed"))
                                {
                                    lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitGrayHoundPlace();
                                    betupperlimit = LoggedinUserDetail.GetBetUpperLimitGrayHoundPlace();
                                }
                                else
                                {
                                    if (categoryname.Contains("Greyhound Racing") && !marketbookname.Contains("To Be Placed"))
                                    {
                                        lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitGrayHoundWin();
                                        betupperlimit = LoggedinUserDetail.GetBetUpperLimitGrayHoundWin();
                                    }
                                    else
                                    {
                                        if (marketbookname.Contains("Completed Match"))
                                        {
                                            lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitCompletedMatch();
                                            betupperlimit = LoggedinUserDetail.GetBetUpperLimitCompletedMatch();
                                        }
                                        else
                                        {
                                            if ((marketbookname.Contains("Innings Runs") || marketbookname.Contains("Inns Runs")) && currmarketbookforbet.Runners.Count > 1)
                                            {
                                                lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitInningsRuns();
                                                betupperlimit = LoggedinUserDetail.GetBetUpperLimitInningsRuns();
                                            }
                                            else
                                            {
                                                if (categoryname == "Tennis")
                                                {
                                                    lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitMatchOddsTennis();
                                                    betupperlimit = LoggedinUserDetail.GetBetUpperLimitMatchOddsTennis();
                                                }
                                                else
                                                {
                                                    if (categoryname == "Soccer")
                                                    {
                                                        lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitMatchOddsSoccer();
                                                        betupperlimit = LoggedinUserDetail.GetBetUpperLimitMatchOddsSoccer();
                                                    }
                                                    else
                                                    {
                                                        if (marketbookname.Contains("Tied Match"))
                                                        {
                                                            lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitTiedMatch();
                                                            betupperlimit = LoggedinUserDetail.GetBetUpperLimitTiedMatch();
                                                        }
                                                        else
                                                        {
                                                            if (marketbookname.Contains("Winner") || marketbookname.Contains("To Win the Toss"))
                                                            {
                                                                lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitWinner();
                                                                betupperlimit = LoggedinUserDetail.GetBetUpperLimitWinner();
                                                            }
                                                            else
                                                            {
                                                                if (currmarketbookforbet.Runners.Count() == 1)
                                                                {
                                                                    lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitFancy();
                                                                    betupperlimit = LoggedinUserDetail.GetBetUpperLimitFancy();
                                                                }
                                                                else
                                                                {
                                                                    lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitMatchOdds();
                                                                    betupperlimit = LoggedinUserDetail.GetBetUpperLimitMatchOdds();
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
                        }


                        decimal alreadypendingamount = 0;
                        foreach (UserBets dgvrow in DGVUnMatched.Items)
                        {
                            alreadypendingamount += Convert.ToDecimal(dgvrow.Amount);
                        }

                        if (alreadypendingamount >= betupperlimit)
                        {
                            HideProgressBar();
                            LoggedinUserDetail.isInserting = false;

                            PlayMessage("Already reached to limit.Please cancel any unmatch bet");
                            Xceed.Wpf.Toolkit.MessageBox.Show("Already reached to limit. Please cancel any unmatch bet.");

                            btnSubmitBetSlip.IsEnabled = true;
                            btnResetBetSlip.IsEnabled = true;

                            return;
                        }
                        if (betamounttobeplaced < lowerbetlimit || betamounttobeplaced > betupperlimit)
                        {
                            LoggedinUserDetail.isInserting = false;
                            HideProgressBar();
                            PlayMessage("Amount should not be less than " + lowerbetlimit + " and greater than " + betupperlimit + ".");
                            Xceed.Wpf.Toolkit.MessageBox.Show("Amount should not be less than " + lowerbetlimit + " and greater than " + betupperlimit + ".");

                            btnSubmitBetSlip.IsEnabled = true;
                            btnResetBetSlip.IsEnabled = true;

                            return;
                        }
                        List<string> lstSelectionIds = new List<string>();
                        lstSelectionIds.Add(SelectionID);
                        UserBetHelper objUserbethelper = new UserBetHelper();
                        string data = objUserbethelper.CheckforPlaceBet(betslipamountlabel.Content.ToString(), nupdownOdd.Value.ToString(), BetType, lstSelectionIds.ToArray(), currmarketbookforbet.MarketId, LoggedinUserDetail.CurrentUserBets, currmarketbookforbet.Runners, LoggedinUserDetail.CurrentAccountBalance);
                        if (data == "True")
                        {
                            isFirstClickonslip = true;
                            popupBetslip.IsOpen = false;
                            // Stoptimers();
                            ShowProgressBar();
                            backgroundWorkerInsertBet.RunWorkerAsync();
                        }
                        else
                        {
                            LoggedinUserDetail.isInserting = false;
                            HideProgressBar();
                            btnSubmitBetSlip.IsEnabled = true;
                            btnResetBetSlip.IsEnabled = true;
                            PlayMessage(data);
                            Xceed.Wpf.Toolkit.MessageBox.Show(data);
                        }
                    }
                    else
                    {
                        LoggedinUserDetail.isInserting = false;
                        popupBetslip.IsOpen = false;
                        PlayMessage("Betting Is Not Open Yet");
                        Xceed.Wpf.Toolkit.MessageBox.Show("Betting is not Open yet.", "Global Traders", MessageBoxButton.OK);

                    }

                }
                else
                {
                    LoggedinUserDetail.isInserting = false;
                    PlayMessage("Your are not allowed to perform this operation.");
                    Xceed.Wpf.Toolkit.MessageBox.Show("Your are not allowed to perform this operation.");
                }
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.isInserting = false;
                HideProgressBar();
                btnSubmitBetSlip.IsEnabled = true;
                btnResetBetSlip.IsEnabled = true;

                Xceed.Wpf.Toolkit.MessageBox.Show("Something went wrong. Please bet again or call the vendor.");
            }
        }
        public bool isFirstClickonslip = true;

        private void btnSubmitBetSlipin_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (LoggedinUserDetail.isInserting == true)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Please wait for other bet completion.");
                    return;
                }

                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {

                    calculateProfitandLossonBetSlipin();

                    BetPlaceWait = 200;
                    BetWaitTimerInterval = 200;

                    //SetBetPlacewaitTimerandInterval(MarketBook.MainSportsname, MarketBook.MarketBookName, MarketBook.Runners.Count.ToString());                   
                    if (Allowedbetting(BettingAllowed, MarketBook.MarketStatusstr, MarketBook.MarketBookName, MarketBook.MainSportsname, MarketBook.OpenDate, MarketBook.Runners.Count.ToString(), CurrentMarketBookId, true) == true)
                    {
                        popupBetslipforin.IsOpen = false;
                        LoggedinUserDetail.isInserting = true;
                        btnSubmitBetSlipin.IsEnabled = false;
                        btnResetBetSlipin.IsEnabled = false;
                        //ShowLoader;
                        decimal betamounttobeplaced = nupdnUserAmountin.Value.Value;
                        decimal lowerbetlimit = 2000;
                        decimal betupperlimit = 500000;
                        string categoryname = MarketBook.MainSportsname;
                        string marketbookname = MarketBook.MarketBookName;

                        lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitFancy();
                        betupperlimit = LoggedinUserDetail.GetBetUpperLimitFancy();


                        decimal alreadypendingamount = 0;
                        foreach (UserBets dgvrow in DGVUnMatched.Items)
                        {
                            alreadypendingamount += Convert.ToDecimal(dgvrow.Amount);
                        }

                        if (alreadypendingamount >= betupperlimit)
                        {
                            HideProgressBar();
                            LoggedinUserDetail.isInserting = false;

                            PlayMessage("Already reached to limit.Please cancel any unmatch bet");
                            Xceed.Wpf.Toolkit.MessageBox.Show("Already reached to limit. Please cancel any unmatch bet.");

                            btnSubmitBetSlipin.IsEnabled = true;
                            btnResetBetSlipin.IsEnabled = true;

                            return;
                        }
                        if (betamounttobeplaced < lowerbetlimit || betamounttobeplaced > betupperlimit)
                        {
                            LoggedinUserDetail.isInserting = false;
                            HideProgressBar();
                            PlayMessage("Amount should not be less than " + lowerbetlimit + " and greater than " + betupperlimit + ".");
                            Xceed.Wpf.Toolkit.MessageBox.Show("Amount should not be less than " + lowerbetlimit + " and greater than " + betupperlimit + ".");
                            btnSubmitBetSlipin.IsEnabled = true;
                            btnResetBetSlipin.IsEnabled = true;

                            return;
                        }
                        List<string> lstSelectionIds = new List<string>();
                        lstSelectionIds.Add(SelectionID);
                        UserBetHelper objUserbethelper = new UserBetHelper();
                        string data = objUserbethelper.CheckforPlaceBet(betslipamountlabelin.Content.ToString(), nupdownOddin.Value.ToString(), BetType, lstSelectionIds.ToArray(), CurrentMarketBookId, LoggedinUserDetail.CurrentUserBets, MarketBook.Runners, LoggedinUserDetail.CurrentAccountBalance);
                        if (data == "True")
                        {
                            isFirstClickonslip = true;
                            popupBetslipforin.IsOpen = false;
                            // Stoptimers();
                            ShowProgressBar();
                            backgroundWorkerInsertBetin.RunWorkerAsync();
                        }
                        else
                        {
                            LoggedinUserDetail.isInserting = false;
                            HideProgressBar();
                            btnSubmitBetSlipin.IsEnabled = true;
                            btnResetBetSlipin.IsEnabled = true;
                            PlayMessage(data);
                            Xceed.Wpf.Toolkit.MessageBox.Show(data);
                        }
                    }
                    else
                    {
                        LoggedinUserDetail.isInserting = false;
                        popupBetslipforin.IsOpen = false;
                        PlayMessage("Betting is not allowed");
                        Xceed.Wpf.Toolkit.MessageBox.Show("Betting is not allowed.", "Global Traders", MessageBoxButton.OK);

                    }

                }
                else
                {
                    LoggedinUserDetail.isInserting = false;

                    PlayMessage("Your are not allowed to perform this operation.");
                    Xceed.Wpf.Toolkit.MessageBox.Show("Your are not allowed to perform this operation.");
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                LoggedinUserDetail.isInserting = false;
                HideProgressBar();
                btnSubmitBetSlipin.IsEnabled = true;
                btnResetBetSlipin.IsEnabled = true;

                Xceed.Wpf.Toolkit.MessageBox.Show("Something went wrong. Please bet again or call the vendor.");
            }
        }

        private void btnSubmitBetSlipin1_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (LoggedinUserDetail.isInserting == true)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Please wait for other bet completion.");
                    return;
                }

                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {

                    calculateProfitandLossonBetSlipin();
                    BetPlaceWait = 200;
                    BetWaitTimerInterval = 200;
                    //SetBetPlacewaitTimerandInterval(MarketBook.MainSportsname, MarketBook.MarketBookName, MarketBook.Runners.Count.ToString());                   
                    //if (Allowedbetting(BettingAllowed, MarketBook.MarketStatusstr, MarketBook.MarketBookName, MarketBook.MainSportsname, MarketBook.OpenDate, MarketBook.Runners.Count.ToString(), CurrentMarketBookId, true) == true)
                    //{

                    popupBetslipforin1.IsOpen = false;
                    LoggedinUserDetail.isInserting = true;
                    btnSubmitBetSlipin1.IsEnabled = false;
                    btnResetBetSlipin1.IsEnabled = false;
                    //ShowLoader;
                    decimal betamounttobeplaced = nupdnUserAmountin1.Value.Value;
                    decimal lowerbetlimit = 2000;
                    decimal betupperlimit = 500000;
                    string categoryname = MarketBook.MainSportsname;
                    string marketbookname = MarketBook.MarketBookName;


                    lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitFancy();
                    betupperlimit = LoggedinUserDetail.GetBetUpperLimitFancy();


                    decimal alreadypendingamount = 0;
                    foreach (UserBets dgvrow in DGVUnMatched.Items)
                    {
                        alreadypendingamount += Convert.ToDecimal(dgvrow.Amount);
                    }

                    if (alreadypendingamount >= betupperlimit)
                    {
                        HideProgressBar();
                        LoggedinUserDetail.isInserting = false;

                        PlayMessage("Already reached to limit.Please cancel any unmatch bet");
                        Xceed.Wpf.Toolkit.MessageBox.Show("Already reached to limit. Please cancel any unmatch bet.");

                        btnSubmitBetSlipin1.IsEnabled = true;
                        btnResetBetSlipin1.IsEnabled = true;

                        return;
                    }
                    if (betamounttobeplaced < lowerbetlimit || betamounttobeplaced > betupperlimit)
                    {
                        LoggedinUserDetail.isInserting = false;
                        HideProgressBar();
                        PlayMessage("Amount should not be less than " + lowerbetlimit + " and greater than " + betupperlimit + ".");
                        Xceed.Wpf.Toolkit.MessageBox.Show("Amount should not be less than " + lowerbetlimit + " and greater than " + betupperlimit + ".");
                        btnSubmitBetSlipin1.IsEnabled = true;
                        btnResetBetSlipin1.IsEnabled = true;

                        return;
                    }
                    List<string> lstSelectionIds = new List<string>();
                    lstSelectionIds.Add(SelectionID);
                    UserBetHelper objUserbethelper = new UserBetHelper();
                    string data = objUserbethelper.CheckforPlaceBet(betslipamountlabelin1.Content.ToString(), nupdownOddin1.Value.ToString(), BetType, lstSelectionIds.ToArray(), CurrentMarketBookId, LoggedinUserDetail.CurrentUserBets, MarketBook.Runners, LoggedinUserDetail.CurrentAccountBalance);
                    if (data == "True")
                    {
                        isFirstClickonslip = true;
                        popupBetslipforin1.IsOpen = false;
                        // Stoptimers();
                        ShowProgressBar();
                        backgroundWorkerInsertBetin1.RunWorkerAsync();
                    }
                    else
                    {
                        LoggedinUserDetail.isInserting = false;
                        HideProgressBar();
                        btnSubmitBetSlipin1.IsEnabled = true;
                        btnResetBetSlipin1.IsEnabled = true;
                        PlayMessage(data);
                        Xceed.Wpf.Toolkit.MessageBox.Show(data);
                    }
                    //}
                    //else
                    //{
                    //    LoggedinUserDetail.isInserting = false;
                    //    popupBetslipforin1.IsOpen = false;
                    //    PlayMessage("Betting is not allowed");
                    //    Xceed.Wpf.Toolkit.MessageBox.Show("Betting is not allowed.", "Global Traders", MessageBoxButton.OK);

                    //}

                }
                else
                {
                    LoggedinUserDetail.isInserting = false;
                    PlayMessage("Your are not allowed to perform this operation.");
                    Xceed.Wpf.Toolkit.MessageBox.Show("Your are not allowed to perform this operation.");
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                LoggedinUserDetail.isInserting = false;
                HideProgressBar();
                btnSubmitBetSlipin1.IsEnabled = true;
                btnResetBetSlipin1.IsEnabled = true;

                Xceed.Wpf.Toolkit.MessageBox.Show("Something went wrong. Please bet again or call the vendor.");
            }
        }


        private void nupdnUserAmountMultiple_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            CalculateAmountsMultiple();
        }

        private void btnSubmitBetSlipMultiple_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnSubmitMultiple_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoggedinUserDetail.isInserting == true)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Please wait for other bet completion.");
                    return;
                }

                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    if (MarketBook.IsMarketDataDelayed == true)
                    {
                        PlayMessage("Betfair data is delay, Please bet later.");
                        Xceed.Wpf.Toolkit.MessageBox.Show("Betfair data is delay, Please bet later.");
                        return;
                    }
                    CalculateAmountsMultiple();

                    if (Allowedbetting(MarketBook.BettingAllowed, MarketBook.MarketStatusstr, MarketBook.MarketBookName, MarketBook.MainSportsname, MarketBook.OpenDate, MarketBook.Runners.Count.ToString(), MarketBook.MarketId, false) == true)
                    {

                        lblWaitTimer.Content = "Wait....";
                        popupBetslipMultiple.IsOpen = false;
                        btnSubmitBetSlipMultiple.IsEnabled = false;
                        btnResetBetSlipMultiple.IsEnabled = false;
                        //ShowLoader;
                        decimal betamounttobeplaced = nupdnUserAmountMultiple.Value.Value;
                        decimal lowerbetlimit = 2000;
                        decimal betupperlimit = 500000;
                        string categoryname = MarketBook.MainSportsname;
                        string marketbookname = MarketBook.MarketBookName;
                        if (categoryname.Contains("Horse Racing") && !marketbookname.Contains("To Be Placed"))
                        {
                            lowerbetlimit = LoggedinUserDetail.GetBetLowerLimit();
                            betupperlimit = LoggedinUserDetail.GetBetUpperLimit();
                        }
                        else
                        {
                            if (categoryname.Contains("Horse Racing") && marketbookname.Contains("To Be Placed"))
                            {
                                lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitHorsePlace();
                                betupperlimit = LoggedinUserDetail.GetBetUpperLimitHorsePlace();
                            }
                            else
                            {
                                if (categoryname.Contains("Greyhound Racing") && marketbookname.Contains("To Be Placed"))
                                {
                                    lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitGrayHoundPlace();
                                    betupperlimit = LoggedinUserDetail.GetBetUpperLimitGrayHoundPlace();
                                }
                                else
                                {
                                    if (categoryname.Contains("Greyhound Racing") && !marketbookname.Contains("To Be Placed"))
                                    {
                                        lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitGrayHoundWin();
                                        betupperlimit = LoggedinUserDetail.GetBetUpperLimitGrayHoundWin();
                                    }
                                    else
                                    {
                                        if (marketbookname.Contains("Completed Match"))
                                        {
                                            lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitCompletedMatch();
                                            betupperlimit = LoggedinUserDetail.GetBetUpperLimitCompletedMatch();
                                        }
                                        else
                                        {
                                            if (marketbookname.Contains("Innings Runs") || marketbookname.Contains("Inns Runs"))
                                            {
                                                lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitInningsRuns();
                                                betupperlimit = LoggedinUserDetail.GetBetUpperLimitInningsRuns();
                                            }
                                            else
                                            {
                                                if (categoryname == "Tennis")
                                                {
                                                    lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitMatchOddsTennis();
                                                    betupperlimit = LoggedinUserDetail.GetBetUpperLimitMatchOddsTennis();
                                                }
                                                else
                                                {
                                                    if (categoryname == "Soccer")
                                                    {
                                                        lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitMatchOddsSoccer();
                                                        betupperlimit = LoggedinUserDetail.GetBetUpperLimitMatchOddsSoccer();
                                                    }
                                                    else
                                                    {
                                                        if (marketbookname.Contains("Tied Match"))
                                                        {
                                                            lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitTiedMatch();
                                                            betupperlimit = LoggedinUserDetail.GetBetUpperLimitTiedMatch();
                                                        }
                                                        else
                                                        {
                                                            if (marketbookname.Contains("Winner"))
                                                            {
                                                                lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitWinner();
                                                                betupperlimit = LoggedinUserDetail.GetBetUpperLimitWinner();
                                                            }
                                                            else
                                                            {
                                                                lowerbetlimit = LoggedinUserDetail.GetBetLowerLimitMatchOdds();
                                                                betupperlimit = LoggedinUserDetail.GetBetUpperLimitMatchOdds();
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
                        lowerbetlimit = lowerbetlimit * 2;
                        betupperlimit = betupperlimit * 2;


                        decimal alreadypendingamount = 0;
                        foreach (UserBets dgvrow in DGVUnMatched.Items)
                        {
                            alreadypendingamount += Convert.ToDecimal(dgvrow.Amount);
                        }

                        //$(".unmatched-bets .bets-items").each(function(){
                        //        alreadypendingamount += parseFloat($(this).attr("data-amount"));
                        //    });
                        if (alreadypendingamount >= betupperlimit)
                        {

                            lblWaitTimer.Content = "";
                            PlayMessage("Already reached to limit. Please cancel any unmatch bet.");
                            Xceed.Wpf.Toolkit.MessageBox.Show("Already reached to limit. Please cancel any unmatch bet.");
                            btnSubmitBetSlipMultiple.IsEnabled = true;
                            btnResetBetSlipMultiple.IsEnabled = true;

                            return;
                        }
                        if (betamounttobeplaced < lowerbetlimit || betamounttobeplaced > betupperlimit)
                        {

                            lblWaitTimer.Content = "";
                            PlayMessage("Amount should not be less than " + lowerbetlimit + " and greater than " + betupperlimit + ".");
                            Xceed.Wpf.Toolkit.MessageBox.Show("Amount should not be less than " + lowerbetlimit + " and greater than " + betupperlimit + ".");
                            btnSubmitBetSlipMultiple.IsEnabled = true;
                            btnResetBetSlipMultiple.IsEnabled = true;

                            return;
                        }
                        List<string> lstSelectionIds = new List<string>();
                        List<string> lstClothnumbers = new List<string>();
                        List<string> lstSelectionnames = new List<string>();
                        foreach (MarketBookShow objSelection in DGVMultipleBetsSelection.Items)
                        {
                            lstSelectionIds.Add(objSelection.SelectionID);
                            lstSelectionnames.Add(objSelection.Selection);
                            if (objSelection.Clothnumber != null && objSelection.Clothnumber != "Not" && objSelection.Clothnumber != "")
                            {
                                lstClothnumbers.Add(objSelection.Clothnumber);
                            }
                            else
                            {
                                lstClothnumbers.Add("");
                            }

                        }

                        UserBetHelper objUserbethelper = new UserBetHelper();
                        string data = objUserbethelper.CheckforPlaceBet(betslipamountlabelMultiple.Content.ToString(), 0.ToString(), BetType, lstSelectionIds.ToArray(), MarketBook.MarketId, LoggedinUserDetail.CurrentUserBets, MarketBook.Runners, LoggedinUserDetail.CurrentAccountBalance);
                        if (data == "True")
                        {

                            LoggedinUserDetail.isInserting = true;
                            for (int i = 0; i <= lstSelectionIds.Count - 1; i++)
                            {

                                ExternalAPI.TO.Runner objRunner = MarketBook.Runners.Where(item1 => item1.SelectionId == lstSelectionIds[i]).FirstOrDefault();
                                SelectionID = objRunner.SelectionId;
                                Selectionname = lstSelectionnames[i];
                                if (lstClothnumbers[i] == "")
                                {
                                    var selectionnameactual = Selectionname.ToString().Split('.');
                                    Selectionname = selectionnameactual[0] + "-" + Selectionname;

                                }
                                else
                                {
                                    Selectionname = lstClothnumbers[i].ToString() + "-" + Selectionname.ToString();
                                }
                                if (BetType == "back")
                                {
                                    if (objRunner.ExchangePrices.AvailableToBack != null)
                                    {
                                        decimal odd = Convert.ToDecimal(betslipbackmultiple.Content);
                                       
                                        decimal amount = nupdnUserAmountMultiple.Value.Value;
                                        decimal totamount = odd * amount;
                                        totamount = totamount + amount;

                                        decimal currentoddbyselectionID = Convert.ToDecimal(objRunner.ExchangePrices.AvailableToBack[0].Price);
                                        decimal amountforcurrentodd = 100 / currentoddbyselectionID;
                                        amountforcurrentodd = amountforcurrentodd * totamount;
                                        amountforcurrentodd = amountforcurrentodd / 100;
                                        loadedlocation = -1;


                                        clickedbetsize = -1;
                                        clickedodd = 0;

                                        ParentID = 0;
                                        Clickedlocation = 0;
                                        lblBetSlipBack.Content = objRunner.ExchangePrices.AvailableToBack[0].Price.ToString();
                                        Betslipsize.Text = objRunner.ExchangePrices.AvailableToBack[0].Size.ToString();

                                        nupdnUserAmount.Value = Convert.ToInt32(amountforcurrentodd);
                                        nupdownOdd.Value = currentoddbyselectionID;


                                        insertbetslip();
                                    }
                                }
                                else
                                {
                                    if (objRunner.ExchangePrices.AvailableToLay != null)
                                    {
                                        decimal odd = Convert.ToDecimal(betsliplaymultiple.Content);
                                        decimal amount = nupdnUserAmountMultiple.Value.Value;
                                        decimal totamount = odd * amount;
                                        totamount = totamount + amount;

                                        decimal currentoddbyselectionID = Convert.ToDecimal(objRunner.ExchangePrices.AvailableToLay[0].Price);
                                        decimal amountforcurrentodd = 100 / currentoddbyselectionID;
                                        amountforcurrentodd = amountforcurrentodd * totamount;
                                        amountforcurrentodd = amountforcurrentodd / 100;
                                        loadedlocation = -1;


                                        clickedbetsize = -1;
                                        clickedodd = 0;
                                        Clickedlocation = 0;
                                        ParentID = 0;

                                        lblBetSlipBack.Content = objRunner.ExchangePrices.AvailableToLay[0].Price.ToString();
                                        Betslipsize.Text = objRunner.ExchangePrices.AvailableToLay[0].Size.ToString();

                                        nupdnUserAmount.Value = Convert.ToInt32(amountforcurrentodd);
                                        nupdownOdd.Value = currentoddbyselectionID;
                                        insertbetslip();
                                    }
                                }
                            }
                            lblWaitTimer.Content = "";

                            LoggedinUserDetail.isInserting = false;
                            isFirstClickonslip = true;
                            popupBetslipMultiple.IsOpen = false;
                            btnSubmitBetSlipMultiple.IsEnabled = true;
                            btnResetBetSlipMultiple.IsEnabled = true;


                        }
                        else
                        {

                            lblWaitTimer.Content = "";
                            btnSubmitBetSlipMultiple.IsEnabled = true;
                            btnResetBetSlipMultiple.IsEnabled = true;
                            PlayMessage(data);
                            Xceed.Wpf.Toolkit.MessageBox.Show(data);
                        }
                    }
                }
                else
                {
                    PlayMessage("Your are not allowed to perform this operation.");
                    Xceed.Wpf.Toolkit.MessageBox.Show("Your are not allowed to perform this operation.");
                }
            }
            catch (System.Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message.ToString());
                lblWaitTimer.Content = "";
                LoggedinUserDetail.isInserting = false;
                isFirstClickonslip = true;
                btnSubmitBetSlipMultiple.IsEnabled = true;
                btnResetBetSlipMultiple.IsEnabled = true;

                Xceed.Wpf.Toolkit.MessageBox.Show("Something went wrong. Please bet again or call the vendor.");
            }
        }
        private void lblFavoriteBack_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BetType = "back";
            ShowBetSlipMultiple();
        }

        private void lblFavoriteLay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            BetType = "lay";
            ShowBetSlipMultiple();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            popupBetslipMultiple.IsOpen = false;
        }

        private void btnCancelBetSlipMultiple_Click(object sender, RoutedEventArgs e)
        {
            popupBetslipMultiple.IsOpen = false;
            isFirstClickonslip = true;
        }

        private void btnResetBetSlip_Click(object sender, RoutedEventArgs e)
        {
            isFirstClickonslip = true;
            if (BetType == "back")
            {
                nupdnUserAmount.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeBack);
                nupdnUserAmountin.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeBack);
            }
            else
            {
                nupdnUserAmount.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeLay);
                nupdnUserAmountin.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeLay);
            }
            // nupdownAmount.Value = 2000;
            calculateProfitandLossonBetSlip();
            calculateProfitandLossonBetSlipin();
        }

        private void btnResetBetSlipMultiple_Click(object sender, RoutedEventArgs e)
        {
            isFirstClickonslip = true;
            if (BetType == "back")
            {
                nupdnUserAmountMultiple.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeBackMultiple);
            }
            else
            {
                nupdnUserAmountMultiple.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeLayMultiple);
            }
            CalculateAmountsMultiple();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            popupBetslipKeys.IsOpen = false;
        }

        private void btnUpdateBetSlipKeys_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popupBetslipKeys.IsOpen = false;
                objUsersServiceCleint.UpdateBetSlipKeys(LoggedinUserDetail.GetUserID(), txtSimpleBtn1.Text, txtSimpleBtn2.Text, txtSimpleBtn3.Text, txtSimpleBtn4.Text, txtSimpleBtn5.Text, txtSimpleBtn6.Text, txtSimpleBtn7.Text, txtSimpleBtn8.Text, txtSimpleBtn9.Text, "0", "0", "0", txtMultpleBtn1.Text, txtMultpleBtn2.Text, txtMultpleBtn3.Text, txtMultpleBtn4.Text, txtMultpleBtn5.Text, txtMultpleBtn6.Text, txtMultpleBtn7.Text, txtMultpleBtn8.Text, txtMultpleBtn9.Text, "0", "0", "0");
                Properties.Settings.Default.DefaultStakeBack = Convert.ToDouble(txtDefaultStakeBack.Text);
                Properties.Settings.Default.DefaultStakeBackMultiple = Convert.ToDouble(txtDefaultStakeBackMultiple.Text);
                Properties.Settings.Default.DefaultStakeLay = Convert.ToDouble(txtDefaultStakeLay.Text);
                Properties.Settings.Default.DefaultStakeLayMultiple = Convert.ToDouble(txtDefaultStakeLayMultiple.Text);
                Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
                LoggedinUserDetail.objBetSlipKeys = JsonConvert.DeserializeObject<HelperClasses.BetSlipKeys>(objUsersServiceCleint.GetBetSlipKeys(LoggedinUserDetail.GetUserID()));
                SetBetSlipKeys();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter correct values");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (btnShowHideMenu.Tag.ToString() == "1")
            { btnShowHideMenu.Tag = "0"; }
            else
            {
                btnShowHideMenu.Tag = "1";
            }
            Resizewindow();
        }

        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RulesWindow objrules = new RulesWindow();
            objrules.rules = SetRulesbyName();
            objrules.Show();
        }
        List<MarketRules> MarketRulesAll = new List<MarketRules>();
        public string SetRulesbyName()
        {
            string categoryname = MarketBook.MainSportsname;
            string marketbookname = MarketBook.MarketBookName;

            if (categoryname.Contains("Horse Racing") && !marketbookname.Contains("To Be Placed"))
            {

                return MarketRulesAll.Where(item => item.Category == "Horse Racing" && item.MarketType == "Win").Select(item => item.Rules).FirstOrDefault().ToString();
            }
            else
            {
                if (categoryname.Contains("Horse Racing") && marketbookname.Contains("To Be Placed"))
                {
                    return MarketRulesAll.Where(item => item.Category == "Horse Racing" && item.MarketType == "To Be Placed").Select(item => item.Rules).FirstOrDefault().ToString();

                }
                else
                {
                    if (categoryname.Contains("Greyhound Racing") && marketbookname.Contains("To Be Placed"))
                    {
                        return MarketRulesAll.Where(item => item.Category == "Greyhound Racing" && item.MarketType == "To Be Placed").Select(item => item.Rules).FirstOrDefault().ToString();
                    }
                    else
                    {
                        if (categoryname.Contains("Greyhound Racing") && !marketbookname.Contains("To Be Placed"))
                        {
                            return MarketRulesAll.Where(item => item.Category == "Greyhound Racing" && item.MarketType == "Win").Select(item => item.Rules).FirstOrDefault().ToString();

                        }
                        else
                        {
                            if (marketbookname.Contains("Completed Match"))
                            {
                                return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Completed Match").Select(item => item.Rules).FirstOrDefault().ToString();

                            }
                            else
                            {
                                if (marketbookname.Contains("Innings Runs") || marketbookname.Contains("Inns Runs"))
                                {
                                    return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Innings Runs").Select(item => item.Rules).FirstOrDefault().ToString();

                                }
                                else
                                {
                                    if (categoryname == "Tennis")
                                    {
                                        return MarketRulesAll.Where(item => item.Category == "Tennis").Select(item => item.Rules).FirstOrDefault().ToString();

                                    }
                                    else
                                    {
                                        if (categoryname == "Soccer")
                                        {
                                            return MarketRulesAll.Where(item => item.Category == "Soccer").Select(item => item.Rules).FirstOrDefault().ToString();

                                        }
                                        else
                                        {
                                            if (marketbookname.Contains("Tied Match"))
                                            {
                                                return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Tied Match").Select(item => item.Rules).FirstOrDefault().ToString();

                                            }
                                            else
                                            {
                                                if (marketbookname.Contains("Winner"))
                                                {
                                                    return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Match Odds").Select(item => item.Rules).FirstOrDefault().ToString();

                                                }
                                                else
                                                {
                                                    if (MarketBook.Runners.Count == 3)
                                                    {
                                                        return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Test Match").Select(item => item.Rules).FirstOrDefault().ToString();
                                                    }
                                                    else
                                                    {
                                                        string cricketrules = MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Match Odds").Select(item => item.Rules).FirstOrDefault().ToString();
                                                        cricketrules += Environment.NewLine + Environment.NewLine + MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Fancy").Select(item => item.Rules).FirstOrDefault().ToString();
                                                        return cricketrules;
                                                        // return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Match Odds").Select(item => item.Rules).FirstOrDefault().ToString();
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
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // popupBetslip1.IsOpen = false;
        }



        private void DGVUnMatched_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DGVUnMatched.Items.Count > 0)
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {

                        if (DGVUnMatched.CurrentCell.Column.DisplayIndex == 4)
                        {
                            UserBets objSelectedBet = (UserBets)DGVUnMatched.CurrentCell.Item;
                            string IDtoDelete = objSelectedBet.ID.ToString();
                            if (MarketBook.MainSportsname == "Cricket")
                            {
                                System.Threading.Thread.Sleep(LoggedinUserDetail.BetPlaceWaitInterval.CancelBetTime.Value);
                                List<string> lstIds = new List<string>();
                                lstIds.Add(IDtoDelete);
                                UserBetHelper objuserbets = new UserBetHelper();
                                objuserbets.UpdateUnMatchedStatustoComplete(lstIds.ToArray());

                            }
                            else
                            {
                                //  Thread.Sleep(2000);
                                List<string> lstIds = new List<string>();
                                lstIds.Add(IDtoDelete);
                                UserBetHelper objuserbets = new UserBetHelper();
                                objuserbets.UpdateUnMatchedStatustoComplete(lstIds.ToArray());
                            }


                        }
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void DeleteUnmatchedBet(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 3)
            {
                if (MarketBook.MainSportsname == "Cricket")
                {
                    if (DGVUnMatched.Items.Count > 0)
                    {
                        System.Threading.Thread.Sleep(LoggedinUserDetail.BetPlaceWaitInterval.CancelBetTime.Value);
                        List<string> lstIds = new List<string>();
                        foreach (UserBets dgrow in DGVUnMatched.Items)
                        {
                            lstIds.Add(dgrow.ID.ToString());
                        }
                        UserBetHelper objuserbets = new UserBetHelper();
                        objuserbets.UpdateUnMatchedStatustoComplete(lstIds.ToArray());
                    }


                }
                else
                {
                    // Thread.Sleep(2000);
                    List<string> lstIds = new List<string>();
                    foreach (UserBets dgrow in DGVUnMatched.Items)
                    {
                        lstIds.Add(dgrow.ID.ToString());
                    }
                    UserBetHelper objuserbets = new UserBetHelper();
                    objuserbets.UpdateUnMatchedStatustoComplete(lstIds.ToArray());
                }
            }
        }
        public void AssignUserstoCombobox()
        {
            cmbUsersCurrentPosition.ItemsSource = LoggedinUserDetail.AllUsers;
            cmbUsersCurrentPosition.DisplayMemberPath = "UserName";
            cmbUsersCurrentPosition.SelectedValuePath = "ID";


            cmbAgentsForProfitandLoss.IsSynchronizedWithCurrentItem = false;
            cmbAgentsForProfitandLoss.ItemsSource = LoggedinUserDetail.AllUsers.Where(u => new[] { "2" }.Contains(u.UserTypeID.ToString())).ToList();
            cmbAgentsForProfitandLoss.DisplayMemberPath = "UserName";
            cmbAgentsForProfitandLoss.SelectedValuePath = "ID";
            cmbAgentsForProfitandLoss.SelectedValue = 73;
            //cmbAgentsForProfitandLossFAncy.IsSynchronizedWithCurrentItem = false;
            //cmbAgentsForProfitandLossFAncy.ItemsSource = LoggedinUserDetail.AllUsers.Where(u => new[] { "2" }.Contains(u.UserTypeID.ToString())).ToList();
            //cmbAgentsForProfitandLossFAncy.DisplayMemberPath = "UserName";
            //cmbAgentsForProfitandLossFAncy.SelectedValuePath = "ID";
            //cmbAgentsForProfitandLossFAncy.SelectedValue = 73;
        }
        public void ShowaverageSectionclick()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {


                //popupAverageSection.IsOpen = true;
                AssignUserstoCombobox();


                var results = objUsersServiceCleint.GetSelectionNamesbyMarketID(MarketBook.MarketId);
                cmbRunner11.ItemsSource = null;
                cmbRunner11.ItemsSource = results;
                cmbRunner11.DisplayMemberPath = "SelectionName";
                cmbRunner11.SelectedValuePath = "SelectionID";
                cmbRunner11.IsSynchronizedWithCurrentItem = false;

                cmbRunner21.IsSynchronizedWithCurrentItem = false;
                //List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                //lstRunners = lastloadedmarket.Runners;
                cmbRunner21.ItemsSource = null;
                cmbRunner21.ItemsSource = results;
                cmbRunner21.DisplayMemberPath = "SelectionName";
                cmbRunner21.SelectedValuePath = "SelectionID";
                cmbRunnersCurrentPosition1.ItemsSource = null;
                cmbRunnersCurrentPosition1.IsSynchronizedWithCurrentItem = false;
                cmbRunnersCurrentPosition1.ItemsSource = results;
                cmbRunnersCurrentPosition1.DisplayMemberPath = "SelectionName";
                cmbRunnersCurrentPosition1.SelectedValuePath = "SelectionID";
                cmbAdminBetRunners1.ItemsSource = null;
                cmbAdminBetRunners1.IsSynchronizedWithCurrentItem = false;
                cmbAdminBetRunners1.ItemsSource = results;
                cmbAdminBetRunners1.DisplayMemberPath = "SelectionName";
                cmbAdminBetRunners1.SelectedValuePath = "SelectionID";
                cmbAdminBetRunners1.SelectedValue = MarketBook.FavoriteID;


                List<bftradeline.HelperClasses.CuttingUsers> lstCuttingUsers = JsonConvert.DeserializeObject<List<bftradeline.HelperClasses.CuttingUsers>>(objUsersServiceCleint.GetAllCuttingUsers(LoggedinUserDetail.PasswordForValidate));

                cmbAdminBetUsers1.ItemsSource = lstCuttingUsers;
                cmbAdminBetUsers1.DisplayMemberPath = "username";
                cmbAdminBetUsers1.SelectedValuePath = "ID";
                cmbAdminBetType1.SelectedIndex = 0;
                cmbAdminBetUsers1.SelectedIndex = 0;
              
                lblCurrentPostitionUserPL1.Content = "0";
                cmbUsersCurrentPosition.SelectedIndex = 0;

                txtAverageAmount11.Content = "0.00";
                txtAverageAmount21.Content = "0.00";
                cmbRunner21.SelectedIndex = 1;
                cmbRunner11.SelectedIndex = 0;
                CalculateAvearageforAdmin();

                txtAdminOdd1.Focus();
            }

        }
        public void CalculateAvearageforAdmin()
        {
            try
            {


                if (LoggedinUserDetail.GetUserTypeID() == 1 && popupAverageSection.IsOpen == true)
                {

                    if (1 == 1)
                    {
                        double runner1profit = 0;
                        double runner2profit = 0;
                        ExternalAPI.TO.Runner runner1 = CurrentMarketProfitandLoss[0].Runners.Where(item => item.SelectionId == cmbRunner11.SelectedValue.ToString()).FirstOrDefault();
                        ExternalAPI.TO.Runner runner2 = CurrentMarketProfitandLoss[0].Runners.Where(item => item.SelectionId == cmbRunner21.SelectedValue.ToString()).FirstOrDefault();
                        if (runner1 != null)
                        {
                            runner1profit = runner1.ProfitandLoss;
                            runner2profit = runner2.ProfitandLoss;
                        }

                        if ((runner1profit > 0 && runner2profit > 0) || (runner1profit < 0 && runner2profit < 0))
                        {
                            txtAverageAmount11.Content = "0.00";
                            txtAverageAmount21.Content = "0.00";
                            lblAverage11.Content = "";
                            lblAverage21.Content = "";
                            return;
                        }
                        if (runner1profit > 0)
                        {
                            lblAverage11.Content = "L";
                            lblAverage11.Foreground = Brushes.DarkGreen;
                            lblAverage21.Content = "K";
                            lblAverage21.Foreground = Brushes.Red;

                        }
                        else
                        {

                            if (runner2profit > 0)
                            {
                                lblAverage11.Content = "K";
                                lblAverage11.Foreground = Brushes.Red;
                                lblAverage21.Content = "L";
                                lblAverage21.Foreground = Brushes.DarkGreen;
                            }
                        }
                        if (runner1profit < 0) { runner1profit = runner1profit * -1; }
                        if (runner2profit < 0) { runner2profit = runner2profit * -1; }
                        if (runner1profit > 0 && runner2profit > 0)
                        {
                            double result = runner1profit / runner2profit;
                            double result2 = runner2profit / runner1profit;
                            txtAverageAmount11.Content = result.ToString("F2");
                            txtAverageAmount21.Content = result2.ToString("F2");
                        }

                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        public void CalculateAvearageforAllUsers()
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {

                    if (lstMarketBookRunners.Where(item => item.isSelectedForLK == true).Count() == 2)
                    {

                        if (1 == 1)
                        {
                            double runner1profit = 0;
                            double runner2profit = 0;
                            MarketBookShow Runner1ID = lstMarketBookRunners.Where(item => item.isSelectedForLK == true).First();
                            MarketBookShow Runner2ID = lstMarketBookRunners.Where(item => item.isSelectedForLK == true).Last();
                            ExternalAPI.TO.Runner runner1 = CurrentMarketProfitandLoss[0].Runners.Where(item => item.SelectionId == Runner1ID.SelectionID).FirstOrDefault();
                            ExternalAPI.TO.Runner runner2 = CurrentMarketProfitandLoss[0].Runners.Where(item => item.SelectionId == Runner2ID.SelectionID).FirstOrDefault();
                            if (runner1 != null)
                            {
                                runner1profit = runner1.ProfitandLoss;
                                runner2profit = runner2.ProfitandLoss;
                            }
                            if (runner1profit == 0 || runner2profit == 0)
                            {
                                Runner1ID.Average = "";
                                Runner2ID.Average = "";
                            }
                            if ((runner1profit > 0 && runner2profit > 0))
                            {
                                Runner1ID.Average = "0.00";
                                Runner2ID.Average = "0.00";

                                return;
                            }
                            if ((runner1profit < 0 && runner2profit < 0))
                            {
                                Runner1ID.Average = "-0.00";
                                Runner2ID.Average = "-0.00";

                                return;
                            }
                            if (runner1profit > 0)
                            {
                                Runner1ID.Average = " L";

                                Runner2ID.Average = " K";


                            }
                            else
                            {

                                if (runner2profit > 0)
                                {
                                    Runner1ID.Average = " K";

                                    Runner2ID.Average = " L";
                                }
                            }
                            if (runner1profit < 0) { runner1profit = runner1profit * -1; }
                            if (runner2profit < 0) { runner2profit = runner2profit * -1; }
                            if (runner1profit > 0 && runner2profit > 0)
                            {
                                double result = runner1profit / runner2profit;
                                double result2 = runner2profit / runner1profit;
                                Runner1ID.Average = result.ToString("F2") + Runner1ID.Average;
                                Runner2ID.Average = result2.ToString("F2") + Runner2ID.Average;
                            }

                        }
                    }
                }
                else
                {
                    return;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        public void CalculateAvearageforAllUsersTowinTheToss()
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {

                    if (lstMarketBookRunnersToWintheToss.Where(item => item.isSelectedForLK == true).Count() == 2)
                    {

                        if (1 == 1)
                        {
                            double runner1profit = 0;
                            double runner2profit = 0;
                            MarketBookShow Runner1ID = lstMarketBookRunnersToWintheToss.Where(item => item.isSelectedForLK == true).First();
                            MarketBookShow Runner2ID = lstMarketBookRunnersToWintheToss.Where(item => item.isSelectedForLK == true).Last();
                            ExternalAPI.TO.Runner runner1 = CurrentMarketProfitandLossToWinTheToss[0].Runners.Where(item => item.SelectionId == Runner1ID.SelectionID).FirstOrDefault();
                            ExternalAPI.TO.Runner runner2 = CurrentMarketProfitandLossToWinTheToss[0].Runners.Where(item => item.SelectionId == Runner2ID.SelectionID).FirstOrDefault();
                            if (runner1 != null)
                            {
                                runner1profit = runner1.ProfitandLoss;
                                runner2profit = runner2.ProfitandLoss;
                            }
                            if (runner1profit == 0 || runner2profit == 0)
                            {
                                Runner1ID.Average = "";
                                Runner2ID.Average = "";
                            }
                            if ((runner1profit > 0 && runner2profit > 0))
                            {
                                Runner1ID.Average = "0.00";
                                Runner2ID.Average = "0.00";

                                return;
                            }
                            if ((runner1profit < 0 && runner2profit < 0))
                            {
                                Runner1ID.Average = "-0.00";
                                Runner2ID.Average = "-0.00";

                                return;
                            }
                            if (runner1profit > 0)
                            {
                                Runner1ID.Average = " L";

                                Runner2ID.Average = " K";


                            }
                            else
                            {

                                if (runner2profit > 0)
                                {
                                    Runner1ID.Average = " K";

                                    Runner2ID.Average = " L";
                                }
                            }
                            if (runner1profit < 0) { runner1profit = runner1profit * -1; }
                            if (runner2profit < 0) { runner2profit = runner2profit * -1; }
                            if (runner1profit > 0 && runner2profit > 0)
                            {
                                double result = runner1profit / runner2profit;
                                double result2 = runner2profit / runner1profit;
                                Runner1ID.Average = result.ToString("F2") + Runner1ID.Average;
                                Runner2ID.Average = result2.ToString("F2") + Runner2ID.Average;
                            }

                        }
                    }
                }
                else {
                    return;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void cmbAdminBetRunners1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbAdminBetRunners1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtAdminOdd1.Focus();
        }

        private void txtAdminOdd1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtAdminAmount1.Focus();
            }
        }

        private void txtAdminAmount1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmbAdminBetType1.Focus();
            }
        }

        private void cmbAdminBetType1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmbAdminBetUsers1.Focus();
            }
        }

        private void cmbAdminBetUsers1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnAdminBetAdd1.Focus();
            }
        }

        private void btnAdminBetAdd1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnAdminBetAdd1.IsEnabled = false;
                if (Convert.ToDecimal(txtAdminOdd1.Text) > 1 && Convert.ToDecimal(txtAdminAmount1.Text) > 1)
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        objUsersServiceCleint.InsertUserBetAdminAsync(cmbAdminBetRunners1.SelectedValue.ToString(), Convert.ToInt32(cmbAdminBetUsers1.SelectedValue), txtAdminOdd1.Text, txtAdminAmount1.Text, cmbAdminBetType1.Text, txtAdminOdd1.Text, true, "In-Complete", MarketBook.MarketId, DateTime.Now, DateTime.Now, cmbAdminBetRunners1.Text, MarketBook.MarketBookName, "0", "0", 0, "0", 0, LoggedinUserDetail.PasswordForValidate);
                    }
                    btnAdminBetAdd1.IsEnabled = true;
                    txtAdminOdd1.Focus();
                }
                else
                {
                    btnAdminBetAdd1.IsEnabled = true;
                    MessageBox.Show("Please enter correct values.");
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                btnAdminBetAdd1.IsEnabled = true;
                MessageBox.Show("Please enter correct values.");
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (panelcurrentpos.Visibility == Visibility.Collapsed)
            {
                panelcurrentpos.Visibility = Visibility.Visible;
                btnShowCurrentPos.Content = "Hide Current Position";



            }
            else
            {
                panelcurrentpos.Visibility = Visibility.Collapsed;
                btnShowCurrentPos.Content = "Show Current Position";

            }
        }

        private void btnGetCurrentProfitandLoss1_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                if (cmbUsersCurrentPosition.SelectedIndex > 0 && cmbRunnersCurrentPosition1.SelectedIndex > -1)
                {
                    string userbets = objUsersServiceCleint.GetUserbetsbyUserID(Convert.ToInt32(cmbUsersCurrentPosition.SelectedValue), LoggedinUserDetail.PasswordForValidate);
                    List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(userbets);
                    if (lstUserBets.Count > 0)
                    {
                        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                        List<UserBets> lstUserBets1 = lstUserBets.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBook.MarketId).ToList();

                        var currentusermarketbook = MarketBook;
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
                        long currentprofitandloss = currentusermarketbook.Runners.Where(item => item.SelectionId == cmbRunnersCurrentPosition1.SelectedValue.ToString()).FirstOrDefault().ProfitandLoss;
                        if (currentprofitandloss >= 0)
                        {
                            lblCurrentPostitionUserPL1.Foreground = Brushes.Green;
                        }
                        else
                        {
                            lblCurrentPostitionUserPL1.Foreground = Brushes.Red;
                        }
                        lblCurrentPostitionUserPL1.Content = currentprofitandloss.ToString();

                    }
                    else
                    {
                        lblCurrentPostitionUserPL1.Content = "0";
                    }
                }
            }
        }

        private void imgAverageSection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {


        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            popupAverageSection.IsOpen = false;
        }


        private void txtAdminAmount1_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (txtAdminAmount1.Text.Length > 3)
            //{

            //    txtAdminAmount1.Text = Convert.ToDecimal(txtAdminAmount1.Text).ToString("N0");
            //}
        }

        private void DGVMatchedBetsAdmin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = Window.GetWindow(this) as MainWindow;
            try
            {


                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    DataGrid objSender = (DataGrid)sender;
                    if (objSender.Items.Count > 0)
                    {

                        UserBetsForAdmin objselectedmarket = (UserBetsForAdmin)objSender.SelectedItem;
                        //MarketBookFunc(objselectedmarket.MarketBookID);

                        window.bsyindicator.IsBusy = true;
                        window.MarketBook(objselectedmarket.MarketBookID);
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                window.bsyindicator.IsBusy = false;
            }
        }
        public bool isProfitLossbyAgentShown = false;
        public void ShowaverageSectionByAgentclick()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                lblProfitorLossRunnerAgent1.Content = 0;
                lblProfitandLossRunnerbyAgent2.Content = 0;
                lblProfitandLossRunnerbyAgent3.Content = 0;
                //popupProffitLossAgent.IsOpen = false;
                isProfitLossbyAgentShown = true;
                AssignUserstoCombobox();
                cmbRunnerForProfitandLoss1.IsSynchronizedWithCurrentItem = false;


                var results = objUsersServiceCleint.GetSelectionNamesbyMarketID(MarketBook.MarketId);

                cmbRunnerForProfitandLoss1.ItemsSource = results;
                cmbRunnerForProfitandLoss1.DisplayMemberPath = "SelectionName";
                cmbRunnerForProfitandLoss1.SelectedValuePath = "SelectionID";

                cmbRunnerForProfitandLoss2.IsSynchronizedWithCurrentItem = false;
                cmbRunnerForProfitandLoss1.SelectedIndex = 0;

                //List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                //lstRunners = lastloadedmarket.Runners;
                cmbRunnerForProfitandLoss2.ItemsSource = results;
                cmbRunnerForProfitandLoss2.DisplayMemberPath = "SelectionName";
                cmbRunnerForProfitandLoss2.SelectedValuePath = "SelectionID";
                txtAverageAmountbyAgnet1.Content = "0.00";
                txtAverageAmountbyAgent2.Content = "0.00";
                cmbRunnerForProfitandLoss2.SelectedIndex = 1;
                if (results.Count() == 3)
                {

                    cmbRunnerForProfitandLoss3.IsSynchronizedWithCurrentItem = false;
                    cmbRunnerForProfitandLoss3.ItemsSource = results;
                    cmbRunnerForProfitandLoss3.DisplayMemberPath = "SelectionName";
                    cmbRunnerForProfitandLoss3.SelectedValuePath = "SelectionID";
                    cmbRunnerForProfitandLoss3.SelectedIndex = 2;
                    lblProfitandLossRunnerbyAgent3.Content = "0";
                }
                else
                {
                    cmbRunnerForProfitandLoss3.Visibility = Visibility.Collapsed;
                    lblProfitandLossRunnerbyAgent3.Visibility = Visibility.Collapsed;
                }
                // CalculateAvearageforSelectedAgent();


            }

        }
        public void CalculateAvearageforSelectedAgent()
        {
            try
            {


                if (LoggedinUserDetail.GetUserTypeID() == 1 && isProfitLossbyAgentShown == true)
                {

                    if (1 == 1)
                    {
                        double runner1profit = 0;
                        double runner2profit = 0;
                        ExternalAPI.TO.Runner runner1 = CurrentMarketProfitandLossForAgent[0].Runners.Where(item => item.SelectionId == cmbRunnerForProfitandLoss1.SelectedValue.ToString()).FirstOrDefault();
                        ExternalAPI.TO.Runner runner2 = CurrentMarketProfitandLossForAgent[0].Runners.Where(item => item.SelectionId == cmbRunnerForProfitandLoss2.SelectedValue.ToString()).FirstOrDefault();
                        if (runner1 != null)
                        {
                            runner1profit = runner1.ProfitandLoss;
                            if (lblProfitorLossRunnerAgent1.Content.ToString() != runner1profit.ToString("N0"))
                            {
                                lblProfitorLossRunnerAgent1.Content = runner1profit.ToString("N0");
                            }

                            if (runner1profit >= 0)
                            {
                                if (lblProfitorLossRunnerAgent1.Foreground != Brushes.DarkGreen)
                                {
                                    lblProfitorLossRunnerAgent1.Foreground = Brushes.DarkGreen;
                                }

                            }
                            else
                            {
                                if (lblProfitorLossRunnerAgent1.Foreground != Brushes.Red)
                                {
                                    lblProfitorLossRunnerAgent1.Foreground = Brushes.Red;
                                }

                            }
                            runner2profit = runner2.ProfitandLoss;
                            if (lblProfitandLossRunnerbyAgent2.Content.ToString() != runner2profit.ToString("N0"))
                            {
                                lblProfitandLossRunnerbyAgent2.Content = runner2profit.ToString("N0");
                            }

                            if (runner2profit >= 0)
                            {
                                if (lblProfitandLossRunnerbyAgent2.Foreground != Brushes.DarkGreen)
                                {
                                    lblProfitandLossRunnerbyAgent2.Foreground = Brushes.DarkGreen;
                                }

                            }
                            else
                            {
                                if (lblProfitandLossRunnerbyAgent2.Foreground != Brushes.Red)
                                {
                                    lblProfitandLossRunnerbyAgent2.Foreground = Brushes.Red;
                                }

                            }

                        }
                        if (CurrentMarketProfitandLossForAgent[0].Runners.Count == 3)
                        {
                            ExternalAPI.TO.Runner runner3 = CurrentMarketProfitandLossForAgent[0].Runners.Where(item => item.SelectionId == cmbRunnerForProfitandLoss3.SelectedValue.ToString()).FirstOrDefault();
                            if (runner3 != null)
                            {
                                if (lblProfitandLossRunnerbyAgent3.Content.ToString() != runner3.ProfitandLoss.ToString("N0"))
                                {
                                    lblProfitandLossRunnerbyAgent3.Content = runner3.ProfitandLoss.ToString("N0");
                                }

                                if (runner3.ProfitandLoss >= 0)
                                {
                                    if (lblProfitandLossRunnerbyAgent3.Foreground != Brushes.DarkGreen)
                                    {
                                        lblProfitandLossRunnerbyAgent3.Foreground = Brushes.DarkGreen;
                                    }

                                }
                                else
                                {
                                    if (lblProfitandLossRunnerbyAgent3.Foreground != Brushes.Red)
                                    {
                                        lblProfitandLossRunnerbyAgent3.Foreground = Brushes.Red;
                                    }

                                }
                            }
                        }
                        if ((runner1profit > 0 && runner2profit > 0))
                        {
                            if (txtAverageAmountbyAgnet1.Content.ToString() != "0.00")
                            {
                                txtAverageAmountbyAgnet1.Content = "0.00";
                            }
                            if (txtAverageAmountbyAgent2.Content.ToString() != "0.00")
                            {
                                txtAverageAmountbyAgent2.Content = "0.00";
                            }

                            return;
                        }
                        if ((runner1profit < 0 && runner2profit < 0))
                        {
                            if (txtAverageAmountbyAgnet1.Content.ToString() != "-0.00")
                            {
                                txtAverageAmountbyAgnet1.Content = "-0.00";
                            }
                            if (txtAverageAmountbyAgent2.Content.ToString() != "-0.00")
                            {
                                txtAverageAmountbyAgent2.Content = "-0.00";
                            }

                            return;
                        }
                        //if ((runner1profit > 0 && runner2profit > 0) || (runner1profit < 0 && runner2profit < 0))
                        //{
                        //    txtAverageAmountbyAgnet1.Content = "0.00";
                        //    txtAverageAmountbyAgent2.Content = "0.00";
                        //    return;
                        //}
                        if (runner1profit > 0)
                        {
                            if (lblAveragebyAgent1.Content.ToString() != "L")
                            {
                                lblAveragebyAgent1.Content = "L";
                            }
                            if (lblAveragebyAgent2.Content.ToString() != "K")
                            {
                                lblAveragebyAgent2.Content = "K";
                            }


                            if (lblAveragebyAgent1.Foreground != Brushes.DarkGreen)
                            {
                                lblAveragebyAgent1.Foreground = Brushes.DarkGreen;

                            }
                            if (lblAveragebyAgent2.Foreground != Brushes.Red)
                            {
                                lblAveragebyAgent2.Foreground = Brushes.Red;
                            }



                        }
                        else
                        {
                            if (lblProfitorLossRunnerAgent1.Foreground != Brushes.Red)
                            {
                                lblProfitorLossRunnerAgent1.Foreground = Brushes.Red;
                            }

                            if (runner2profit > 0)
                            {
                                if (lblAveragebyAgent1.Content.ToString() != "K")
                                {
                                    lblAveragebyAgent1.Content = "K";
                                }
                                if (lblAveragebyAgent2.Content.ToString() != "L")
                                {
                                    lblAveragebyAgent2.Content = "L";
                                }


                                if (lblAveragebyAgent1.Foreground != Brushes.Red)
                                {
                                    lblAveragebyAgent1.Foreground = Brushes.Red;

                                }
                                if (lblAveragebyAgent2.Foreground != Brushes.DarkGreen)
                                {
                                    lblAveragebyAgent2.Foreground = Brushes.DarkGreen;
                                }
                                //lblAveragebyAgent1.Content = "K";
                                //lblAveragebyAgent1.Foreground = Brushes.Red;
                                //lblAveragebyAgent2.Content = "L";
                                //lblAveragebyAgent2.Foreground = Brushes.DarkGreen;

                            }

                        }

                        if (runner1profit < 0) { runner1profit = runner1profit * -1; }
                        if (runner2profit < 0) { runner2profit = runner2profit * -1; }
                        if (runner1profit > 0 && runner2profit > 0)
                        {
                            double result = runner1profit / runner2profit;
                            double result2 = runner2profit / runner1profit;
                            if (txtAverageAmountbyAgnet1.Content.ToString() != result.ToString("F2"))
                            {
                                txtAverageAmountbyAgnet1.Content = result.ToString("F2");
                            }
                            if (txtAverageAmountbyAgent2.Content.ToString() != result2.ToString("F2"))
                            {
                                txtAverageAmountbyAgent2.Content = result2.ToString("F2");
                            }

                        }
                        if (runner1profit == 0)
                        {
                            if (txtAverageAmountbyAgnet1.Content.ToString() != "0.00")
                            {
                                txtAverageAmountbyAgnet1.Content = "0.00";
                            }
                            if (lblAveragebyAgent1.Content.ToString() != "")
                            {
                                lblAveragebyAgent1.Content = "";
                            }

                        }
                        if (runner2profit == 0)
                        {
                            if (txtAverageAmountbyAgent2.Content.ToString() != "0.00")
                            {
                                txtAverageAmountbyAgent2.Content = "0.00";
                            }
                            if (lblAveragebyAgent2.Content.ToString() != "")
                            {
                                lblAveragebyAgent2.Content = "";
                            }
                            //txtAverageAmountbyAgent2.Content = "0.00";
                            //lblAveragebyAgent2.Content = "";
                        }

                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        //public void CalculateAvearageforSelectedAgent()
        //{
        //    try
        //    {


        //        if (LoggedinUserDetail.GetUserTypeID() == 1 && isProfitLossbyAgentShown == true)
        //        {

        //            if (1 == 1)
        //            {
        //                double runner1profit = 0;
        //                double runner2profit = 0;
        //                ExternalAPI.TO.Runner runner1 = CurrentMarketProfitandLossForAgent[0].Runners.Where(item => item.SelectionId == cmbRunnerForProfitandLoss1.SelectedValue.ToString()).FirstOrDefault();
        //                ExternalAPI.TO.Runner runner2 = CurrentMarketProfitandLossForAgent[0].Runners.Where(item => item.SelectionId == cmbRunnerForProfitandLoss2.SelectedValue.ToString()).FirstOrDefault();
        //                if (runner1 != null)
        //                {
        //                    runner1profit = runner1.ProfitandLoss;
        //                    lblProfitorLossRunnerAgent1.Content = runner1profit.ToString("N0");
        //                    if (runner1profit >= 0)
        //                    {
        //                        lblProfitorLossRunnerAgent1.Foreground = Brushes.DarkGreen;
        //                    }
        //                    else
        //                    {
        //                        lblProfitorLossRunnerAgent1.Foreground = Brushes.Red;
        //                    }
        //                    runner2profit = runner2.ProfitandLoss;
        //                    lblProfitandLossRunnerbyAgent2.Content = runner2profit.ToString("N0");
        //                    if (runner2profit >= 0)
        //                    {
        //                        lblProfitandLossRunnerbyAgent2.Foreground = Brushes.DarkGreen;
        //                    }
        //                    else
        //                    {
        //                        lblProfitandLossRunnerbyAgent2.Foreground = Brushes.Red;
        //                    }

        //                }
        //                if (CurrentMarketProfitandLossForAgent[0].Runners.Count == 3)
        //                {
        //                    ExternalAPI.TO.Runner runner3 = CurrentMarketProfitandLossForAgent[0].Runners.Where(item => item.SelectionId == cmbRunnerForProfitandLoss3.SelectedValue.ToString()).FirstOrDefault();
        //                    if (runner3 != null)
        //                    {
        //                        lblProfitandLossRunnerbyAgent3.Content = runner3.ProfitandLoss.ToString("N0");
        //                        if (runner3.ProfitandLoss >= 0)
        //                        {
        //                            lblProfitandLossRunnerbyAgent3.Foreground = Brushes.DarkGreen;
        //                        }
        //                        else
        //                        {
        //                            lblProfitandLossRunnerbyAgent3.Foreground = Brushes.Red;
        //                        }
        //                    }
        //                }
        //                if ((runner1profit > 0 && runner2profit > 0))
        //                {
        //                    txtAverageAmountbyAgnet1.Content = "0.00";
        //                    txtAverageAmountbyAgent2.Content = "0.00";
        //                    return;
        //                }
        //                if ((runner1profit < 0 && runner2profit < 0))
        //                {
        //                    txtAverageAmountbyAgnet1.Content = "-0.00";
        //                    txtAverageAmountbyAgent2.Content = "-0.00";
        //                    return;
        //                }
        //                if (runner1profit > 0)
        //                {
        //                    lblAveragebyAgent1.Content = "L";
        //                    lblAveragebyAgent1.Foreground = Brushes.DarkGreen;
        //                    lblAveragebyAgent2.Content = "K";
        //                    lblAveragebyAgent2.Foreground = Brushes.Red;


        //                }
        //                else
        //                {
        //                    lblProfitorLossRunnerAgent1.Foreground = Brushes.Red;
        //                    if (runner2profit > 0)
        //                    {
        //                        lblAveragebyAgent1.Content = "K";
        //                        lblAveragebyAgent1.Foreground = Brushes.Red;
        //                        lblAveragebyAgent2.Content = "L";
        //                        lblAveragebyAgent2.Foreground = Brushes.DarkGreen;

        //                    }

        //                }

        //                if (runner1profit < 0) { runner1profit = runner1profit * -1; }
        //                if (runner2profit < 0) { runner2profit = runner2profit * -1; }
        //                if (runner1profit > 0 && runner2profit > 0)
        //                {
        //                    double result = runner1profit / runner2profit;
        //                    double result2 = runner2profit / runner1profit;
        //                    txtAverageAmountbyAgnet1.Content = result.ToString("F2");
        //                    txtAverageAmountbyAgent2.Content = result2.ToString("F2");
        //                }
        //                if (runner1profit == 0)
        //                {
        //                    txtAverageAmountbyAgnet1.Content = "0.00";
        //                    lblAveragebyAgent1.Content = "";
        //                }
        //                if (runner2profit == 0)
        //                {
        //                    txtAverageAmountbyAgent2.Content = "0.00";
        //                    lblAveragebyAgent2.Content = "";
        //                }

        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {

        //    }
        //}

        public void ShowaverageSectionclickFancy()
        {

            if (MarketBook.LineVMarkets != null)
            {


                popupFancyCutting.IsOpen = true;

                //  var results = LastloadedLinMarkets.Where(item => item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 0 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 0).ToList();
                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                foreach (var item in MarketBook.LineVMarkets)
                {
                    ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                    objRunner.SelectionId = item.SelectionID + "|" + item.MarketCatalogueID + "|" + item.MarketCatalogueName;
                    objRunner.RunnerName = item.MarketCatalogueName;
                    lstRunners.Add(objRunner);
                }


                cmbRunnersFancy.IsSynchronizedWithCurrentItem = false;
                cmbRunnersFancy.ItemsSource = lstRunners;
                cmbRunnersFancy.DisplayMemberPath = "RunnerName";
                cmbRunnersFancy.SelectedValuePath = "SelectionId";
                cmbRunnersFancy.SelectedIndex = 0;
                List<bftradeline.HelperClasses.CuttingUsers> lstCuttingUsers = JsonConvert.DeserializeObject<List<bftradeline.HelperClasses.CuttingUsers>>(objUsersServiceCleint.GetAllCuttingUsers(LoggedinUserDetail.PasswordForValidate));
                cmbCuttingUserFancy.IsSynchronizedWithCurrentItem = false;
                cmbCuttingUserFancy.ItemsSource = lstCuttingUsers;
                cmbCuttingUserFancy.DisplayMemberPath = "username";
                cmbCuttingUserFancy.SelectedValuePath = "ID";
                cmbCuttingUserFancy.SelectedIndex = 0;
                cmbBetTypeFancy.SelectedIndex = 0;





                txtOddFancy.Focus();
            }


        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

        }


        public void ShowFancyResultsPostingPanelKJ()
        {

            if (MarketBook.LineVMarkets != null)
            {


                popupFancyResultPostingKJ.IsOpen = true;
                MarketBook.LineVMarkets = MarketBook.LineVMarkets.Where(item => item.EventName == "Kali v Jut").ToList();
                //  var results = LastloadedLinMarkets.Where(item => item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 0 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 0).ToList();
                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                foreach (var item in MarketBook.LineVMarkets)
                {
                    ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                    objRunner.SelectionId = item.MarketCatalogueID;
                    objRunner.RunnerName = item.MarketCatalogueName;
                    lstRunners.Add(objRunner);
                }


                cmbRunnersFancyResultsKJ.IsSynchronizedWithCurrentItem = false;
                cmbRunnersFancyResultsKJ.ItemsSource = lstRunners;
                cmbRunnersFancyResultsKJ.DisplayMemberPath = "RunnerName";
                cmbRunnersFancyResultsKJ.SelectedValuePath = "SelectionId";
                cmbRunnersFancyResultsKJ.SelectedIndex = 0;


            }


        }

        public void ShowFancyResultsPostingPanelSFig()
        {

            if (MarketBook.LineVMarkets != null)
            {
                popupFancyResultPostingSFig.IsOpen = true;
                //  var results = LastloadedLinMarkets.Where(item => item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 0 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 0).ToList();
                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                MarketBook.LineVMarkets = MarketBook.LineVMarkets.Where(item => item.EventName == "SmallFig").ToList();
                foreach (var item in MarketBook.LineVMarkets)
                {
                    ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                    objRunner.SelectionId = item.MarketCatalogueID;
                    objRunner.RunnerName = item.MarketCatalogueName;
                    lstRunners.Add(objRunner);
                }


                cmbRunnersFancyResultsSFig.IsSynchronizedWithCurrentItem = false;
                cmbRunnersFancyResultsSFig.ItemsSource = lstRunners;
                cmbRunnersFancyResultsSFig.DisplayMemberPath = "RunnerName";
                cmbRunnersFancyResultsSFig.SelectedValuePath = "SelectionId";
                cmbRunnersFancyResultsSFig.SelectedIndex = 0;


            }


        }

        public void ShowFancyResultsPostingPanelFig()
        {

            if (MarketBook.LineVMarkets != null)
            {


                popupFancyResultPostingFig.IsOpen = true;

                //  var results = LastloadedLinMarkets.Where(item => item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 0 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 0).ToList();
                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                MarketBook.LineVMarkets = MarketBook.LineVMarkets.GroupBy(item => item.MarketCatalogueName).Select(g => g.First()).Where(item2 => item2.EventName == "Figure").ToList();

                foreach (var item in MarketBook.LineVMarkets)
                {
                    ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                    objRunner.SelectionId = item.MarketCatalogueID;
                    objRunner.RunnerName = item.MarketCatalogueName;
                    lstRunners.Add(objRunner);
                }


                cmbRunnersFancyResultsFig.IsSynchronizedWithCurrentItem = false;
                cmbRunnersFancyResultsFig.ItemsSource = lstRunners;
                cmbRunnersFancyResultsFig.DisplayMemberPath = "RunnerName";
                cmbRunnersFancyResultsFig.SelectedValuePath = "SelectionId";
                cmbRunnersFancyResultsFig.SelectedIndex = 0;


            }


        }

        public void ShowFancyResultsPostingPanelIN()
        {
            List<bftradeline.Models.UserBetsForAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.location == "9" && item.isMatched == true).ToList();
            if (lstCurrentBetsAdmin != null)
            {
                popupIndianFancyResultPosting.IsOpen = true;

                //  var results = LastloadedLinMarkets.Where(item => item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 0 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 0).ToList();
                List<ExternalAPI.TO.RunnerForIndianFancy> lstRunners = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                foreach (var item in lstCurrentBetsAdmin)
                {
                    ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner.SelectionId = item.SelectionID;
                    objRunner.RunnerName = item.SelectionName;
                    objRunner.MarketBookID = item.MarketBookID;
                    //  txtInningsResultI.Text = item.MarketBookID;
                    lstRunners.Add(objRunner);
                }

                cmbRunnersFancyResultsIN.IsSynchronizedWithCurrentItem = false;
                cmbRunnersFancyResultsIN.ItemsSource = lstRunners;
                cmbRunnersFancyResultsIN.DisplayMemberPath = "RunnerName";
                cmbRunnersFancyResultsIN.SelectedValuePath = "MarketBookID";

                cmbRunnersFancyResultsIN.SelectedIndex = 0;

            }
        }



        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            //   popupFancyResultPosting.IsOpen = false;
            popupFancyResultPostingKJ.IsOpen = false;
            popupFancyResultPostingFig.IsOpen = false;
            popupFancyResultPostingSFig.IsOpen = false;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            //popupFancyProffitLossAgent.IsOpen = false;
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            popupFancyCutting.IsOpen = false;
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            popupProffitLossAgent.IsOpen = false;
            isProfitLossbyAgentShown = false;
        }

        private void Image_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {

        }

        private void Image_MouseLeftButtonUp_3(object sender, MouseButtonEventArgs e)
        {

        }
      



        private void btnLoadScoresKJ_Click(object sender, RoutedEventArgs e)
        {
            LoadScoresKJ();
        }
        public void LoadScoresKJ()
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    var eventdetails = objUsersServiceCleint.GetEventDetailsbyMarketBook(cmbRunnersFancyResultsKJ.SelectedValue.ToString());

                    var inningsnameandover = eventdetails.MarketCatalogueName.Split(new string[] { " Innings " }, StringSplitOptions.None);
                    string innings = Regex.Match(inningsnameandover[0], @"\d+").Value;
                    string over = Regex.Match(inningsnameandover[1], @"\d+").Value;
                    var matchscores = "";//JsonConvert.DeserializeObject<ScoresForResultPosting>(objUsersServiceCleint.GetScorebyEventIDandInningsandOvers(eventdetails.associateeventID, eventdetails.EventOpenDate.Value, Convert.ToInt32(innings), Convert.ToInt32(over)));
                    if (matchscores != "")
                    {
                        //txtScoresResults.Text = matchscores.Scores.Value.ToString();
                        //txtOversResults.Text = matchscores.Overs.Value.ToString();
                        //txtInningsResult.Text = matchscores.Innings.Value.ToString();
                        //lblTeamname.Content = matchscores.TeamName.ToString();
                    }
                    else
                    {
                        txtScoresResultsKJ.Text = "";
                        txtOversResultsKJ.Text = over.ToString();
                        txtInningsResultKJ.Text = innings.ToString();
                        lblTeamnameKJ.Content = "";
                        txtScoresResultsKJ.Focus();
                    }
                    lblPostResultsForKJ.Content = cmbRunnersFancyResultsKJ.Text;


                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        public void LoadScoresFig()
        {
            try
            {


                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    var eventdetails = objUsersServiceCleint.GetEventDetailsbyMarketBook(cmbRunnersFancyResultsFig.SelectedValue.ToString());

                    var inningsnameandover = eventdetails.MarketCatalogueName.Split(new string[] { " Innings " }, StringSplitOptions.None);
                    string innings = Regex.Match(inningsnameandover[0], @"\d+").Value;
                    string over = Regex.Match(inningsnameandover[1], @"\d+").Value;
                    var matchscores = "";//JsonConvert.DeserializeObject<ScoresForResultPosting>(objUsersServiceCleint.GetScorebyEventIDandInningsandOvers(eventdetails.associateeventID, eventdetails.EventOpenDate.Value, Convert.ToInt32(innings), Convert.ToInt32(over)));
                    if (matchscores != "")
                    {
                        //txtScoresResults.Text = matchscores.Scores.Value.ToString();
                        //txtOversResults.Text = matchscores.Overs.Value.ToString();
                        //txtInningsResult.Text = matchscores.Innings.Value.ToString();
                        //lblTeamname.Content = matchscores.TeamName.ToString();
                    }
                    else
                    {
                        txtScoresResultsFig.Text = "";
                        txtOversResultsFig.Text = over.ToString();
                        txtInningsResultFig.Text = innings.ToString();
                        lblTeamnameFig.Content = "";
                        txtScoresResultsFig.Focus();
                    }
                    lblPostResultsForFig.Content = cmbRunnersFancyResultsFig.Text;


                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void btnLoadScoresFig_Click(object sender, RoutedEventArgs e)
        {
            LoadScoresFig();
        }
        public void LoadScoresSKJ()
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    var eventdetails = objUsersServiceCleint.GetEventDetailsbyMarketBook(cmbRunnersFancyResultsSFig.SelectedValue.ToString());

                    var inningsnameandover = eventdetails.MarketCatalogueName.Split(new string[] { " Innings " }, StringSplitOptions.None);
                    string innings = Regex.Match(inningsnameandover[0], @"\d+").Value;
                    string over = Regex.Match(inningsnameandover[1], @"\d+").Value;
                    var matchscores = "";//JsonConvert.DeserializeObject<ScoresForResultPosting>(objUsersServiceCleint.GetScorebyEventIDandInningsandOvers(eventdetails.associateeventID, eventdetails.EventOpenDate.Value, Convert.ToInt32(innings), Convert.ToInt32(over)));
                    if (matchscores != "")
                    {
                        //txtScoresResults.Text = matchscores.Scores.Value.ToString();
                        //txtOversResults.Text = matchscores.Overs.Value.ToString();
                        //txtInningsResult.Text = matchscores.Innings.Value.ToString();
                        //lblTeamname.Content = matchscores.TeamName.ToString();
                    }
                    else
                    {
                        txtScoresResultsSFig.Text = "";
                        txtOversResultsSFig.Text = over.ToString();
                        txtInningsResultSFig.Text = innings.ToString();
                        lblTeamnameKJ.Content = "";
                        txtScoresResultsSFig.Focus();
                    }
                    lblPostResultsForSFig.Content = cmbRunnersFancyResultsSFig.Text;


                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void btnLoadScoresSFig_Click(object sender, RoutedEventArgs e)
        {
            LoadScoresSKJ();
        }



        private void cmbRunnersFancy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtOddFancy.Focus();
            }

        }

        private void cmbRunnersFancy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtOddFancy.Focus();
        }

        private void txtOddFancy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtAmountFancy.Focus();
            }
        }

        private void txtAmountFancy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmbBetTypeFancy.Focus();
            }
        }

        private void cmbBetTypeFancy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmbCuttingUserFancy.Focus();
            }
        }

        private void cmbBetTypeFancy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbCuttingUserFancy.Focus();
            }
            catch (System.Exception ex)
            {

            }
        }

        private void cmbCuttingUserFancy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                btnAddFancyCutting.Focus();
            }
            catch (System.Exception ex)
            {

            }
        }

        private void cmbCuttingUserFancy_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnAddFancyCutting.Focus();
            }
        }

        private void btnAddFancyCutting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnAddFancyCutting.IsEnabled = false;
                if (Convert.ToDecimal(txtAmountFancy.Text) > 1 && Convert.ToDecimal(txtAmountFancy.Text) > 1)
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        if (cmbRunnersFancy.Items.Count > 0)
                        {
                            string selectionID = cmbRunnersFancy.SelectedValue.ToString();
                            string[] selectionIdandmarketbookID = selectionID.Split('|');
                            objUsersServiceCleint.InsertUserBetAdminAsync(selectionIdandmarketbookID[0], Convert.ToInt32(cmbCuttingUserFancy.SelectedValue), txtOddFancy.Text, txtAmountFancy.Text, cmbBetTypeFancy.Text, txtOddFancy.Text, true, "In-Complete", selectionIdandmarketbookID[1], DateTime.Now, DateTime.Now, cmbRunnersFancy.Text, selectionIdandmarketbookID[2], "0", "0", 0, "0", 0, LoggedinUserDetail.PasswordForValidate);
                        }

                    }
                    btnAddFancyCutting.IsEnabled = true;
                    txtOddFancy.Focus();
                }
                else
                {
                    btnAddFancyCutting.IsEnabled = true;
                    MessageBox.Show("Please enter correct values.");
                }

            }
            catch (System.Exception ex)
            {
                btnAddFancyCutting.IsEnabled = true;
                MessageBox.Show("Please enter correct values.");
            }
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            popupCuttingBets.IsOpen = false;
        }

        private void btnCuttingBets_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                popupCuttingBets.IsOpen = true;
                List<UserBetsForAdmin> lstCurrentBetsMatched = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true && item.UserTypeID == 4).OrderByDescending(item => item.ID).ToList();
                DGVAllCuttingBets.ItemsSource = lstCurrentBetsMatched;
            }
        }

        private void DGVAllCuttingBets_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                if (DGVAllCuttingBets.CurrentCell.Column.DisplayIndex == 7)
                {
                    UserBetsForAdmin objSelectedRow = (UserBetsForAdmin)DGVAllCuttingBets.CurrentCell.Item;
                    long BetID = objSelectedRow.ID;
                    objUsersServiceCleint.UpdateUserBetUnMatchedStatusTocompleteforCuttingUser(BetID, LoggedinUserDetail.PasswordForValidate);
                    //List<string> lstIds = new List<string>();
                    //lstIds.Add(dgvCuttingBets.Rows[e.RowIndex].Cells["ID"].Value.ToString());
                    //UserBetHelper objuserbets = new UserBetHelper();
                    //objuserbets.UpdateUnMatchedStatustoComplete(lstIds.ToArray());
                    List<UserBetsForAdmin> lstCurrentBetsMatched = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true && item.UserTypeID == 4 && item.ID != Convert.ToInt64(BetID)).ToList();
                    DGVAllCuttingBets.ItemsSource = lstCurrentBetsMatched;

                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            string selectionId = chk.Tag.ToString();
            lstMarketBookRunners.Where(item => item.SelectionID == selectionId).FirstOrDefault().isSelectedForLK = true;
        }


        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            string selectionId = chk.Tag.ToString();
            lstMarketBookRunners.Where(item => item.SelectionID == selectionId).FirstOrDefault().isSelectedForLK = false;
        }
        private void CheckBox_Checked_4(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            string MarketID = chk.Tag.ToString();
            try
            {
                if (chk.IsChecked == true)
                {
                    objUsersServiceCleint.UpdateBettingAllowed(MarketID, "0");
                    Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
                }
                else
                {

                }
            }
            catch (System.Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }
        public int UserIDforLoadMarket = 0;
        private void btnAllSports_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (plusminusAllSports.Content.ToString() == "+")
                {
                    plusminusAllSports.Content = "-";
                    stkpnlSports.Visibility = Visibility.Visible;
                    stkpnlSports.Children.Clear();
                    TreeView inplaytreeview = new TreeView();
                    List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(objUsersServiceCleint.GetAllMatches(UserIDforLoadMarket));

                    var lstEventTypes = lstInPlayMatches.Select(item => new { item.EventTypeID, item.EventTypeName }).Distinct().ToArray();
                    if (lstEventTypes.Count() > 0)
                    {

                        foreach (var eventtypeitem in lstEventTypes)
                        {

                            TreeViewItem NewNodeeventtype = new TreeViewItem();
                            StackPanel stack = new StackPanel();
                            stack.Orientation = Orientation.Horizontal;

                            // create Image
                            Image image = new Image();

                            image.Source = new BitmapImage(new Uri(eventtypeitem.EventTypeName.ToString() + ".png", UriKind.Relative));
                            image.Width = 15;
                            image.Height = 15;
                            // Label
                            Label lbl = new Label();
                            lbl.Content = eventtypeitem.EventTypeName.ToString().ToUpper();
                            lbl.Padding = new Thickness(2);
                            lbl.Margin = new Thickness(2, 0, 0, 0);
                            lbl.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF243875"));
                            // Add into stack
                            stack.Children.Add(image);
                            stack.Children.Add(lbl);
                            //
                            NewNodeeventtype.Tag = "0";
                            NewNodeeventtype.Header = stack;
                            //NewNodeeventtype.Tag = "0";
                            //NewNodeeventtype.Header = eventtypeitem.EventTypeName.ToString();
                            inplaytreeview.Items.Add(NewNodeeventtype);
                            var lstCompetitions = lstInPlayMatches.Where(item => item.EventTypeID == eventtypeitem.EventTypeID).Select(item => new { item.CompetitionID, item.CompetitionName, item.CountryCode }).Distinct().ToArray();
                            foreach (var competitionitem in lstCompetitions)
                            {
                                TreeViewItem newnodecompetition = new TreeViewItem();
                                // create Image
                                StackPanel stack1 = new StackPanel();
                                stack1.Orientation = Orientation.Horizontal;

                                Image image1 = new Image();
                                if (competitionitem.CountryCode != null)
                                {
                                    image1.Source = new BitmapImage(new Uri(@"/Resources/" + competitionitem.CountryCode.ToString() + ".gif", UriKind.Relative));
                                    image1.Width = 18;
                                    image1.Height = 12;
                                    stack1.Children.Add(image1);
                                }

                                // Label
                                Label lbl1 = new Label();
                                lbl1.Content = competitionitem.CompetitionName.ToString();
                                lbl1.Padding = new Thickness(2);
                                lbl1.Margin = new Thickness(2, 0, 0, 0);
                                lbl1.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF243875"));

                                // Add into stack

                                stack1.Children.Add(lbl1);
                                newnodecompetition.Tag = 0;
                                newnodecompetition.Header = stack1;
                                NewNodeeventtype.Items.Add(newnodecompetition);
                                var lstEvents = lstInPlayMatches.Where(item => item.CompetitionID == competitionitem.CompetitionID).Select(item => new { item.EventID, item.EventName }).Distinct().ToArray();
                                foreach (var eventitem in lstEvents)

                                {
                                    if (!(eventitem.EventName.Contains("Line v") || eventitem.EventName.Contains("Lines v")))
                                    {


                                        TreeViewItem newnodeevents = new TreeViewItem();
                                        newnodeevents.Tag = 0;
                                        newnodeevents.Header = eventitem.EventName;
                                        newnodecompetition.Items.Add(newnodeevents);
                                        var lstMarketCatalogues = lstInPlayMatches.Where(item => item.EventID == eventitem.EventID).Select(item => new { item.MarketCatalogueID, item.MarketCatalogueName }).Distinct().ToArray();

                                        foreach (var marketcatalogueitem in lstMarketCatalogues)
                                        {
                                            TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                                            newnodemarketcatalogue.Tag = marketcatalogueitem.MarketCatalogueID;
                                            newnodemarketcatalogue.Header = marketcatalogueitem.MarketCatalogueName;
                                            newnodeevents.Items.Add(newnodemarketcatalogue);
                                            newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;


                                        }
                                        var linevmaketbyEventID = lstInPlayMatches.Where(item => item.AssociateEventID == eventitem.EventID).Select(item => new { item.MarketCatalogueID, item.MarketCatalogueName }).Distinct().ToArray();
                                        if (linevmaketbyEventID.Count() > 0)
                                        {
                                            linevmaketbyEventID.OrderBy(item => item.MarketCatalogueName).ToList();
                                            TreeViewItem newnodeeventsline = new TreeViewItem();
                                            newnodeeventsline.Tag = 0;
                                            newnodeeventsline.Header = "Line v Markets";
                                            newnodeevents.Items.Add(newnodeeventsline);
                                            // var lstMarketCataloguesline = lstInPlayMatches.Where(item => item.EventID == eventitem.EventID).Select(item => new { item.MarketCatalogueID, item.MarketCatalogueName }).Distinct().ToArray();

                                            foreach (var marketcatalogueitem in linevmaketbyEventID)
                                            {
                                                TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                                                newnodemarketcatalogue.Tag = marketcatalogueitem.MarketCatalogueID;
                                                newnodemarketcatalogue.Header = marketcatalogueitem.MarketCatalogueName;
                                                newnodeeventsline.Items.Add(newnodemarketcatalogue);
                                                newnodeeventsline.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;


                                            }
                                        }
                                    }
                                }
                            }
                        }
                        inplaytreeview.Height = 300;
                        stkpnlSports.Children.Add(inplaytreeview);
                        stkpnlSports.UpdateLayout();
                    }
                    else
                    {

                    }
                }
                else
                {
                    plusminusAllSports.Content = "+";
                    stkpnlSports.Visibility = Visibility.Collapsed;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void btnInPlayEvents_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (plusminusInPlay.Content.ToString() == "+")
                {
                    plusminusInPlay.Content = "-";
                    stkpnlInPlayEvents.Visibility = Visibility.Visible;
                    stkpnlInPlayEvents.Children.Clear();
                    TreeView inplaytreeview = new TreeView();

                    List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(objUsersServiceCleint.GetInPlayMatches(UserIDforLoadMarket));

                    var lstEventTypes = lstInPlayMatches.Select(item => new { item.EventTypeID, item.EventTypeName }).Distinct().ToArray();
                    if (lstEventTypes.Count() > 0)
                    {

                        foreach (var eventtypeitem in lstEventTypes)
                        {

                            TreeViewItem NewNodeeventtype = new TreeViewItem();
                            //
                            StackPanel stack = new StackPanel();
                            stack.Orientation = Orientation.Horizontal;

                            // create Image
                            Image image = new Image();

                            image.Source = new BitmapImage(new Uri(eventtypeitem.EventTypeName.ToString() + ".png", UriKind.Relative));
                            image.Width = 15;
                            image.Height = 15;
                            // Label
                            Label lbl = new Label();
                            lbl.Content = eventtypeitem.EventTypeName.ToString().ToUpper();
                            lbl.Padding = new Thickness(2);
                            lbl.Margin = new Thickness(2, 0, 0, 0);
                            lbl.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF243875"));
                            lbl.Focusable = false;
                            // Add into stack
                            stack.Children.Add(image);
                            stack.Children.Add(lbl);
                            //
                            NewNodeeventtype.Tag = "0";
                            NewNodeeventtype.Header = stack;
                            inplaytreeview.Items.Add(NewNodeeventtype);
                            var lstCompetitions = lstInPlayMatches.Where(item => item.EventTypeID == eventtypeitem.EventTypeID).Select(item => new { item.CompetitionID, item.CompetitionName, item.CountryCode }).Distinct().ToArray();
                            foreach (var competitionitem in lstCompetitions)
                            {
                                TreeViewItem newnodecompetition = new TreeViewItem();
                                // create Image
                                StackPanel stack1 = new StackPanel();
                                stack1.Orientation = Orientation.Horizontal;

                                Image image1 = new Image();
                                if (competitionitem.CountryCode != null)
                                {
                                    image1.Source = new BitmapImage(new Uri(@"/Resources/" + competitionitem.CountryCode.ToString() + ".gif", UriKind.Relative));
                                    image1.Width = 18;
                                    image1.Height = 12;
                                    stack1.Children.Add(image1);
                                }

                                // Label
                                Label lbl1 = new Label();
                                lbl1.Content = competitionitem.CompetitionName.ToString();
                                lbl1.Padding = new Thickness(2);
                                lbl1.Margin = new Thickness(2, 0, 0, 0);
                                lbl1.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF243875"));
                                lbl1.Focusable = false;
                                // Add into stack

                                stack1.Children.Add(lbl1);
                                newnodecompetition.Tag = 0;
                                newnodecompetition.Header = stack1;
                                NewNodeeventtype.Items.Add(newnodecompetition);
                                var lstEvents = lstInPlayMatches.Where(item => item.CompetitionID == competitionitem.CompetitionID).Select(item => new { item.EventID, item.EventName }).Distinct().ToArray();
                                foreach (var eventitem in lstEvents)

                                {
                                    if (!(eventitem.EventName.Contains("Line v") || eventitem.EventName.Contains("Lines v")))
                                    {


                                        TreeViewItem newnodeevents = new TreeViewItem();
                                        newnodeevents.Tag = 0;
                                        newnodeevents.Header = eventitem.EventName;
                                        newnodecompetition.Items.Add(newnodeevents);
                                        var lstMarketCatalogues = lstInPlayMatches.Where(item => item.EventID == eventitem.EventID).Select(item => new { item.MarketCatalogueID, item.MarketCatalogueName }).Distinct().ToArray();

                                        foreach (var marketcatalogueitem in lstMarketCatalogues)
                                        {
                                            TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                                            newnodemarketcatalogue.Tag = marketcatalogueitem.MarketCatalogueID;
                                            newnodemarketcatalogue.Header = marketcatalogueitem.MarketCatalogueName;
                                            newnodeevents.Items.Add(newnodemarketcatalogue);
                                            newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;


                                        }
                                        var linevmaketbyEventID = lstInPlayMatches.Where(item => item.AssociateEventID == eventitem.EventID).Select(item => new { item.MarketCatalogueID, item.MarketCatalogueName }).Distinct().ToArray();
                                        if (linevmaketbyEventID.Count() > 0)
                                        {
                                            linevmaketbyEventID.OrderBy(item => item.MarketCatalogueName).ToList();
                                            TreeViewItem newnodeeventsline = new TreeViewItem();
                                            newnodeeventsline.Tag = 0;
                                            newnodeeventsline.Header = "Line v Markets";
                                            newnodeevents.Items.Add(newnodeeventsline);
                                            // var lstMarketCataloguesline = lstInPlayMatches.Where(item => item.EventID == eventitem.EventID).Select(item => new { item.MarketCatalogueID, item.MarketCatalogueName }).Distinct().ToArray();

                                            foreach (var marketcatalogueitem in linevmaketbyEventID)
                                            {
                                                TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                                                newnodemarketcatalogue.Tag = marketcatalogueitem.MarketCatalogueID;
                                                newnodemarketcatalogue.Header = marketcatalogueitem.MarketCatalogueName;
                                                newnodeeventsline.Items.Add(newnodemarketcatalogue);
                                                newnodeeventsline.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        inplaytreeview.Height = 300;
                        stkpnlInPlayEvents.Children.Add(inplaytreeview);
                        stkpnlInPlayEvents.UpdateLayout();
                    }
                    else
                    {

                    }
                }
                else
                {
                    plusminusInPlay.Content = "+";
                    stkpnlInPlayEvents.Visibility = Visibility.Collapsed;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        public Service123Client objBettingClient = new Service123Client();

        public ExternalAPI.TO.MarketBook GetCurrentMarketBook(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate, bool BettingAllowed)
        {
            try
            {
                string[] marketIds = new string[]
                      {
                    marketid
                      };
                var marketbook = objBettingClient.GetMarketDatabyID(marketIds, sheetname, marketopendate, MainSportsCategory, LoggedinUserDetail.PasswordForValidate);
                return marketbook[0];


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        //MarketBookFunc
        public void MarketBookFunc(string ID)
        {
            try
            {




                LoggedinUserDetail.CheckifUserLogin();
                if (ID != "" && LoggedinUserDetail.GetUserTypeID() != 1)
                {
                    if (LoggedinUserDetail.MarketBooks != null)

                    {
                        if (LoggedinUserDetail.MarketBooks.Count > 10)
                        {
                            MessageBox.Show("Limit exceed");
                            return;
                        }

                    }

                }
                if (ID != "" && LoggedinUserDetail.GetUserTypeID() == 1)
                {

                    objUsersServiceCleint.SetMarketBookOpenbyUSer(73, ID);
                }
                var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 1 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
                    {
                        if (ID == "")
                        {
                            var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));

                            if (results != null)
                            {

                                List<string> lstIDs = new List<string>();

                                foreach (var item in results)
                                {
                                    var marketbook = GetCurrentMarketBook(item.ID, item.Name, item.EventTypeName, item.EventOpenDate, item.BettingAllowed);
                                    marketbooks.Add(marketbook);
                                }

                                foreach (var item in results)
                                {
                                    foreach (var item2 in marketbooks)
                                    {
                                        if (item.ID == item2.MarketId)
                                        {
                                            item2.MarketBookName = item.Name + " / " + item.EventName;
                                            item2.OrignalOpenDate = item.EventOpenDate;
                                            item2.MainSportsname = item.EventTypeName;
                                            item2.BettingAllowed = item.BettingAllowed;
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


                            }
                            if (ID == "")
                            {

                            }
                            else
                            {
                                string tabpagename = "marketwin" + ID.Replace(".", "").ToString();
                                var marketwindowname = LoggedinUserDetail.OpenMarkets.Where(item => item == tabpagename).FirstOrDefault();

                                if (marketwindowname != null)
                                {
                                    foreach (Window win in App.Current.Windows)
                                    {
                                        if (win.Name == "marketwin" + ID.Replace(".", ""))
                                        {
                                            win.Activate();
                                        }
                                    }

                                }
                                else
                                {
                                    ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
                                    objmarketbook = marketbooks.Where(item => item.MarketId == ID).FirstOrDefault();
                                    if (objmarketbook != null)
                                    {
                                        LoggedinUserDetail.MarketBooks.Add(objmarketbook);
                                        LoadGridMarket(objmarketbook.MarketId);
                                    }

                                }

                            }


                        }
                        else
                        {
                            if (1 == 1)
                            {
                                string tabpagename = "marketwin" + ID.Replace(".", "").ToString();
                                var marketwindowname = LoggedinUserDetail.OpenMarkets.Where(item => item == tabpagename).FirstOrDefault();

                                if (marketwindowname != null)
                                {
                                    bool windowfound = false;
                                    foreach (Window win in App.Current.Windows)
                                    {
                                        if (win.Name == "marketwin" + ID.Replace(".", ""))
                                        {
                                            windowfound = true;
                                            win.Activate();

                                        }
                                    }
                                    if (windowfound == false)
                                    {

                                    }

                                }
                                else
                                {
                                    // var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                                    var results = JsonConvert.DeserializeObject<List<MarketCatalogueandSelectionNames>>(objUsersServiceCleint.SetMarketBookOpenbyUSerandGet(UserIDforLoadMarket, ID));
                                    var newmarkettobeopened = results;
                                    List<string> lstIDs = new List<string>();

                                    var newmarketbook = new ExternalAPI.TO.MarketBook();

                                    ExternalAPI.TO.MarketBook marketbook1;
                                    marketbook1 = GetCurrentMarketBook(newmarkettobeopened[0].ID, newmarkettobeopened[0].Name, newmarkettobeopened[0].EventTypeName, newmarkettobeopened[0].EventOpenDate, newmarkettobeopened[0].BettingAllowed);
                                    if (1 == 1)
                                    {
                                        newmarketbook = (marketbook1);
                                        newmarketbook.MarketBookName = newmarkettobeopened[0].Name + " / " + newmarkettobeopened[0].EventName;
                                        newmarketbook.OrignalOpenDate = newmarkettobeopened[0].EventOpenDate;
                                        newmarketbook.MainSportsname = newmarkettobeopened[0].EventTypeName;
                                        newmarketbook.BettingAllowed = newmarkettobeopened[0].BettingAllowed;
                                        if (1 == 1)
                                        {

                                            if (1 == 2)
                                            {

                                            }
                                            else
                                            {
                                                foreach (var runnermarketitem in newmarkettobeopened)
                                                {
                                                    var runneritem = newmarketbook.Runners.Where(item => item.SelectionId == runnermarketitem.SelectionID).First();
                                                    runneritem.RunnerName = runnermarketitem.SelectionName;
                                                    runneritem.JockeyName = runnermarketitem.JockeyName;
                                                    runneritem.WearingURL = runnermarketitem.Wearing;
                                                    runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                    runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                    runneritem.StallDraw = runnermarketitem.StallDraw;


                                                }
                                            }


                                        }
                                        ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
                                        objmarketbook = newmarketbook;
                                        if (objmarketbook != null)
                                        {
                                            LoggedinUserDetail.MarketBooks.Add(objmarketbook);
                                            LoadGridMarket(objmarketbook.MarketId);





                                        }

                                    }
                                }
                            }
                            else
                            {
#pragma warning disable CS0162 // Unreachable code detected
                                string tabpagename = "markettab" + ID.Replace(".", "").ToString();
#pragma warning restore CS0162 // Unreachable code detected
                                var marketwindowname = LoggedinUserDetail.OpenMarkets.Where(item => item == tabpagename).FirstOrDefault();

                                if (marketwindowname != null)
                                {




                                }
                                else
                                {
                                    // var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                                    var results = JsonConvert.DeserializeObject<List<MarketCatalogueandSelectionNames>>(objUsersServiceCleint.SetMarketBookOpenbyUSerandGet(UserIDforLoadMarket, ID));
                                    var newmarkettobeopened = results;
                                    List<string> lstIDs = new List<string>();

                                    var newmarketbook = new ExternalAPI.TO.MarketBook();

                                    ExternalAPI.TO.MarketBook marketbook1;
                                    marketbook1 = GetCurrentMarketBook(newmarkettobeopened[0].ID, newmarkettobeopened[0].Name, newmarkettobeopened[0].EventTypeName, newmarkettobeopened[0].EventOpenDate, newmarkettobeopened[0].BettingAllowed);
                                    if (1 == 1)
                                    {
                                        newmarketbook = (marketbook1);
                                        newmarketbook.MarketBookName = newmarkettobeopened[0].Name + " / " + newmarkettobeopened[0].EventName;
                                        newmarketbook.OrignalOpenDate = newmarkettobeopened[0].EventOpenDate;
                                        newmarketbook.MainSportsname = newmarkettobeopened[0].EventTypeName;
                                        newmarketbook.BettingAllowed = newmarkettobeopened[0].BettingAllowed;
                                        if (1 == 1)
                                        {

                                            if (1 == 2)
                                            {

                                            }
                                            else
                                            {
                                                foreach (var runnermarketitem in newmarkettobeopened)
                                                {
                                                    var runneritem = newmarketbook.Runners.Where(item => item.SelectionId == runnermarketitem.SelectionID).First();
                                                    runneritem.RunnerName = runnermarketitem.SelectionName;
                                                    runneritem.JockeyName = runnermarketitem.JockeyName;
                                                    runneritem.WearingURL = runnermarketitem.Wearing;
                                                    runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                                    runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                                    runneritem.StallDraw = runnermarketitem.StallDraw;


                                                }
                                            }


                                        }
                                        ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
                                        objmarketbook = newmarketbook;
                                        if (objmarketbook != null)
                                        {
                                            LoggedinUserDetail.MarketBooks.Add(objmarketbook);
                                            LoadGridMarket(objmarketbook.MarketId);





                                        }

                                    }
                                }
                            }


                        }
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            if (ID == "")
                            {



                                var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUsersForAdmin());
                                if (results.Count > 0)
                                {

                                    List<string> lstIDs = new List<string>();
                                    foreach (var item in results)
                                    {
                                        lstIDs = new List<string>();

                                        lstIDs.Add(item.ID);
                                        var marketbook = GetCurrentMarketBook(item.ID, item.Name, item.EventTypeName, item.EventOpenDate, false);
                                        if (marketbook.MarketId != null)
                                        {
                                            marketbooks.Add(marketbook);
                                        }

                                    }

                                    foreach (var item in results)
                                    {
                                        foreach (var item2 in marketbooks)
                                        {
                                            if (item.ID == item2.MarketId)
                                            {
                                                item2.MarketBookName = item.Name + " / " + item.EventName;
                                                item2.OrignalOpenDate = item.EventOpenDate;
                                                item2.MainSportsname = item.EventTypeName;
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


                                }
                            }
                            else
                            {
                                var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUsersForAdmin());
                                if (results.Count > 0)
                                {
                                    var newmarkettobeopened = results.Where(item3 => item3.ID == ID).FirstOrDefault();
                                    List<string> lstIDs = new List<string>();


                                    lstIDs.Add(newmarkettobeopened.ID);
                                    var marketbook = GetCurrentMarketBook(newmarkettobeopened.ID, newmarkettobeopened.Name, newmarkettobeopened.EventTypeName, newmarkettobeopened.EventOpenDate, newmarkettobeopened.BettingAllowed);
                                    if (marketbook != null)
                                    {
                                        marketbooks.Add(marketbook);
                                        marketbooks[0].MarketBookName = newmarkettobeopened.Name + " / " + newmarkettobeopened.EventName;
                                        marketbooks[0].OrignalOpenDate = newmarkettobeopened.EventOpenDate;
                                        marketbooks[0].MainSportsname = newmarkettobeopened.EventTypeName;
                                        var runnerdesc = objUsersServiceCleint.GetSelectionNamesbyMarketID(ID);
                                        foreach (var runnermarketitem in runnerdesc)
                                        {
                                            foreach (var runneritem in marketbooks[0].Runners)
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



                                    // List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin());
                                }
                            }
                            // Session["userbets"] = lstUserBet;
                            if (ID == "")
                            {

                            }
                            else
                            {
                                string tabpagename = "marketwin" + ID.Replace(".", "").ToString();
                                var marketwindowname = LoggedinUserDetail.OpenMarkets.Where(item => item == ID).FirstOrDefault();

                                if (marketwindowname != null)
                                {
                                    foreach (Window win in App.Current.Windows)
                                    {
                                        if (win.Name == "marketwin" + ID.Replace(".", ""))
                                        {
                                            win.Activate();
                                        }
                                    }

                                }
                                else
                                {
                                    ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
                                    objmarketbook = marketbooks.Where(item => item.MarketId == ID).FirstOrDefault();
                                    if (objmarketbook != null)
                                    {
                                        LoggedinUserDetail.MarketBooks.Add(objmarketbook);
                                        LoadGridMarket(objmarketbook.MarketId);
                                    }

                                }

                            }


                        }
                        else
                        {


                        }


                    }


                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        public void LoadGridMarket(string MarketBookID)
        {
            //WindowState = WindowState.Normal;
            if (LoggedinUserDetail.OpenMarkets != null)
            {
                var marketbook = LoggedinUserDetail.OpenMarkets.Where(item => item == MarketBookID).FirstOrDefault();
                if (marketbook != null)
                {

                }
                else
                {


                    if (1 == 1)
                    {
                        MarketWindow objmarketwindow = new MarketWindow();
                        objmarketwindow.Name = "marketwin" + MarketBookID.Replace(".", "");
                        LoggedinUserDetail.OpenMarkets.Add(objmarketwindow.Name);
                        objmarketwindow.MarketBook = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == MarketBookID).FirstOrDefault();
                        //objmarketwindow.MarketBookForProfitandloss = objmarketwindow.MarketBook;
                        objmarketwindow.Top = 50;
                        objmarketwindow.Left = 100;
                        objmarketwindow.UserIDforLoadMarket = UserIDforLoadMarket;
                        objmarketwindow.Show();
                    }
                    else
                    {

                    }

                }
            }
        }

        private void Inplaytreeview_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                TreeViewItem objSender = (TreeViewItem)sender;
                if (objSender.Tag != null)
                {
                    if (objSender.Tag.ToString() != "0")
                    {

                        MarketBookFunc(objSender.Tag.ToString());

                    }
                }


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void btnHorseRace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (1 == 1)
                {
                    TreeView inplaytreeview = new TreeView();
                    stkpnlHorseRace.Visibility = Visibility.Visible;
                    stkpnlHorseRace.Children.Clear();
                    if (plusminusHorseRace.Content.ToString() == "+")
                    {
                        plusminusHorseRace.Content = "-";
                    }
                    else
                    {
                        plusminusHorseRace.Content = "+";
                        stkpnlHorseRace.Visibility = Visibility.Collapsed;
                        return;
                    }
                    if (LoggedinUserDetail.user.isHorseRaceAllowed == true || LoggedinUserDetail.GetUserTypeID() == 1)
                    {

                        List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(UserIDforLoadMarket, "7"));



                        if (lstTodayHorseRacing.Count > 0)
                        {

                            var lstEvents = lstTodayHorseRacing.Select(item => new { item.TodayHorseRace, item.CountryCode }).Distinct().ToArray();
                            foreach (var eventitem in lstEvents)
                            {
                                TreeViewItem newnodeevent = new TreeViewItem();
                                StackPanel stack = new StackPanel();
                                stack.Orientation = Orientation.Horizontal;

                                // create Image
                                Image image = new Image();
                                if (eventitem.CountryCode != null)
                                {
                                    image.Source = new BitmapImage(new Uri(@"/Resources/" + eventitem.CountryCode.ToString() + ".gif", UriKind.Relative));
                                    image.Width = 18;
                                    image.Height = 12;
                                    stack.Children.Add(image);
                                }

                                // Label
                                Label lbl = new Label();
                                lbl.Content = eventitem.TodayHorseRace;
                                lbl.Padding = new Thickness(0);
                                lbl.Margin = new Thickness(5, 0, 0, 0);
                                lbl.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF243875"));

                                // Add into stack

                                stack.Children.Add(lbl);
                                newnodeevent.Tag = "0";
                                newnodeevent.Header = stack;
                                inplaytreeview.Items.Add(newnodeevent);
                                var lstmarketcatalogues = lstTodayHorseRacing.Where(item => item.TodayHorseRace == eventitem.TodayHorseRace).ToList();
                                foreach (var item in lstmarketcatalogues)
                                {
                                    TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                                    newnodemarketcatalogue.Tag = item.MarketCatalogueID;
                                    newnodemarketcatalogue.Header = item.MarketCatalogueName;
                                    newnodeevent.Items.Add(newnodemarketcatalogue);
                                    newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;
                                }
                                //foreach (var item in lstTodayHorseRacing)
                                //{
                                //    TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                                //    newnodemarketcatalogue.Tag = item.MarketCatalogueID;
                                //    newnodemarketcatalogue.Header = item.TodayHorseRace;
                                //    inplaytreeview.Items.Add(newnodemarketcatalogue);
                                //    newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;
                                //}
                            }

                        }


                        else
                        {
                            TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                            newnodemarketcatalogue.Tag = "0";
                            newnodemarketcatalogue.Header = "No Race found";
                            inplaytreeview.Items.Add(newnodemarketcatalogue);
                        }
                    }
                    else
                    {
                        TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                        newnodemarketcatalogue.Tag = "0";
                        newnodemarketcatalogue.Header = "Race not Allowed";
                        inplaytreeview.Items.Add(newnodemarketcatalogue);
                    }
                    inplaytreeview.Height = 300;
                    //  inplaytreeview.FontSize = 16;
                    stkpnlHorseRace.Children.Add(inplaytreeview);
                    stkpnlHorseRace.UpdateLayout();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void btnGreyhound_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (1 == 1)
                {
                    TreeView inplaytreeview = new TreeView();
                    stkpnlGreyHoundRace.Visibility = Visibility.Visible;
                    stkpnlGreyHoundRace.Children.Clear();
                    if (plusminusGrayhound.Content.ToString() == "+")
                    {
                        plusminusGrayhound.Content = "-";
                    }
                    else
                    {
                        plusminusGrayhound.Content = "+";
                        stkpnlGreyHoundRace.Visibility = Visibility.Collapsed;
                        return;
                    }
                    if (LoggedinUserDetail.user.isGrayHoundRaceAllowed == true || LoggedinUserDetail.GetUserTypeID() == 1)
                    {

                        List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(UserIDforLoadMarket, "4339"));



                        if (lstTodayHorseRacing.Count > 0)
                        {


                            var lstEvents = lstTodayHorseRacing.Select(item => new { item.TodayHorseRace, item.CountryCode }).Distinct().ToArray();
                            foreach (var eventitem in lstEvents)
                            {
                                TreeViewItem newnodeevent = new TreeViewItem();
                                StackPanel stack = new StackPanel();
                                stack.Orientation = Orientation.Horizontal;

                                // create Image
                                Image image = new Image();
                                if (eventitem.CountryCode != null)
                                {
                                    image.Source = new BitmapImage(new Uri(@"/Resources/" + eventitem.CountryCode.ToString() + ".gif", UriKind.Relative));
                                    image.Width = 18;
                                    image.Height = 12;
                                    stack.Children.Add(image);
                                }

                                // Label
                                Label lbl = new Label();
                                lbl.Content = eventitem.TodayHorseRace;
                                lbl.Padding = new Thickness(0);
                                lbl.Margin = new Thickness(5, 0, 0, 0);
                                lbl.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF243875"));
                                // Add into stack

                                stack.Children.Add(lbl);
                                newnodeevent.Tag = "0";
                                newnodeevent.Header = stack;

                                inplaytreeview.Items.Add(newnodeevent);
                                var lstmarketcatalogues = lstTodayHorseRacing.Where(item => item.TodayHorseRace == eventitem.TodayHorseRace).ToList();
                                foreach (var item in lstmarketcatalogues)
                                {
                                    TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                                    newnodemarketcatalogue.Tag = item.MarketCatalogueID;
                                    newnodemarketcatalogue.Header = item.MarketCatalogueName;
                                    newnodeevent.Items.Add(newnodemarketcatalogue);
                                    newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;
                                }

                            }
                        }


                        else
                        {
                            TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                            newnodemarketcatalogue.Tag = "0";
                            newnodemarketcatalogue.Header = "No Race found";
                            inplaytreeview.Items.Add(newnodemarketcatalogue);
                        }
                    }
                    else
                    {
                        TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                        newnodemarketcatalogue.Tag = "0";
                        newnodemarketcatalogue.Header = "Race not Allowed";
                        inplaytreeview.Items.Add(newnodemarketcatalogue);
                    }
                    inplaytreeview.Height = 300;
                    stkpnlGreyHoundRace.Children.Add(inplaytreeview);
                    stkpnlGreyHoundRace.UpdateLayout();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (plusminussettings.Content.ToString() == "+")
            {
                plusminussettings.Content = "-";
                stkpnlBetSlipKeys.Visibility = Visibility.Visible;
            }
            else
            {
                plusminussettings.Content = "+";
                stkpnlBetSlipKeys.Visibility = Visibility.Collapsed;
                return;
            }
        }

        VideoWindow objVideo = new VideoWindow();
        private void imgShowTV_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //objVideo = new VideoWindow();
            //objVideo.Top = 10;
            //objVideo.Left = 10;
            //objVideo.Show();
            if (LoggedinUserDetail.isTVON == false)
            {
                LoggedinUserDetail.isTVON = true;
                objVideo = new VideoWindow();
                objVideo.Top = 10;
                objVideo.Left = 10;
                objVideo.Show();
            }
            else
            {
                objVideo.Activate();
            }
        }

        private void ComboBoxItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void imgAverageSection_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {


                popupAverageSection.IsOpen = true;
                ShowaverageSectionclick();
            }
        }
        private void btnCancelBetSlipin1_Click(object sender, RoutedEventArgs e)
        {
            popupBetslipforin1.IsOpen = false;

        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // SelectedAgentForProfitandLoss = 73;
            popupProffitLossAgent.IsOpen = true;

            ShowaverageSectionByAgentclick();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            popupFancyCutting.IsOpen = true;
            ShowaverageSectionclickFancy();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                //  popupFancyResultPosting.IsOpen = true;
                //  ShowFancyResultsPostingPanel();
            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            //ShowaverageSectionByAgentclickFancy();
        }

        private void TextBlock_MouseLeftButtonUp_4(object sender, MouseButtonEventArgs e)
        {
            if (BetType == "back")
            {
                nupdownOdd.Value = Convert.ToDecimal(lblBetSlipBack.Content);
            }
            else
            {
                nupdownOdd.Value = Convert.ToDecimal(lblBetSlipLay.Content);
            }
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            bool windowfound = false;
            foreach (Window win in App.Current.Windows)
            {
                if (win.Name == "AdminBetsWin")
                {
                    windowfound = true;
                    win.Activate();

                }
            }
            if (windowfound == false)
            {
                AdminBetsWindow objWind = new AdminBetsWindow();

                objWind.Top = 0;
                objWind.Left = this.Width - objWind.Width;
                objWind.Show();
            }
        }
        AccountsReceiveableWindow objAccReceWind = new AccountsReceiveableWindow();
        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            objAccReceWind = new globaltraders.AccountsReceiveableWindow();
            objAccReceWind.Show();
        }




        string currentauthkey = "";

        public class Auth
        {
            public string access_token { get; set; }
            public double expires { get; set; }
        }

        public class AuthDescription
        {
            public bool status { get; set; }
            public string version { get; set; }
            public int status_code { get; set; }
            public string expires { get; set; }
            public Auth auth { get; set; }
            public object Etag { get; set; }
            public string cache_key { get; set; }
        }


        //class LitzscoreStorageHandler : ILitzscoreLocalStorage
        //{
        //    Dictionary<string, string> store { get; set; }
        //    public LitzscoreStorageHandler()
        //    {
        //        store = new Dictionary<string, string>();
        //    }

        //    public string GetValue(string key)
        //    {
        //        return store[key];
        //    }

        //    public void SetValue(string key, string value)
        //    {
        //        store.Add(key, value);
        //    }

        //    public bool HasValue(string key)
        //    {
        //        return store.ContainsKey(key);
        //    }

        //    public string NewDeviceID()
        //    {
        //        string val = System.Guid.NewGuid().ToString("B").ToLower().Replace('{', '_').Replace('-', '_').Replace('}', '_');
        //        return val.Replace("_", "");
        //    }
        //}
        //void pushClientExample(LZApp app)
        //{
        //    app.initPushHandler();

        //    app.pushHandler.onMatchUpdate = delegate (String card)
        //    {
        //        dynamic cardObject = JsonConvert.DeserializeObject(card);
        //        cardObject = cardObject.args[0];
        //        try
        //        {
        //            match = cardObject;


        //        }
        //        catch (System.Exception ex)
        //        {

        //        }



        //    };


        //    app.pushHandler.listenMatch(matchCricketAPIKey);
        //    //double d = Convert.ToDouble(app.token.expires);
        //    //DateTime expiresat =DateTime.FromOADate(d);

        //}


        // List<Sp_GetKalijut_Result> KJ = new List<Sp_GetKalijut_Result>();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //getfancy = false;

                time = false;


                //this.LayoutTransform = new ScaleTransform(nWidth / 1920, nHieght / 1080);

                objUsersServiceCleint.GetUserBetsbyAgentIDwithZeroRefererCompleted += ObjUsersServiceCleint_GetUserBetsbyAgentIDwithZeroRefererCompleted1;
                objUsersServiceCleint.GetScoresbyEventIDandDateCompleted += ObjUsersServiceCleint_GetScoresbyEventIDandDateCompleted;
                //wsFancy.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Fancy").FirstOrDefault().URLForData;
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {

                    ShowaverageSectionclick();
                    ShowaverageSectionByAgentclick();
                    SelectedAgentForProfitandLoss = 73;
                }
                if (MarketBookForProfitandloss.Runners != null)
                {
                    return;
                }
                if (MarketBook.MarketBookName.Contains("To Be Placed"))
                {
                    txtToBePlaced.Text = MarketBook.NumberOfWinners.ToString() + " - TO BE PLACED";
                    txtToBePlaced.Visibility = Visibility.Visible;
                }
                if (LoggedinUserDetail.isTVShown == false)
                {
                    imgShowTV.Visibility = Visibility.Collapsed;
                }
                MarketBookForProfitandloss.MarketBookName = MarketBook.MarketBookName;
                MarketBookForProfitandloss.MarketId = MarketBook.MarketId;
                MarketBookForProfitandloss.Runners = new List<ExternalAPI.TO.Runner>();
                foreach (var runneritem in MarketBook.Runners)
                {
                    ExternalAPI.TO.Runner objNewRunner = new ExternalAPI.TO.Runner();
                    objNewRunner = runneritem;
                    MarketBookForProfitandloss.Runners.Add(objNewRunner);
                }
                MarketBookForProfitandlossAgent.MarketBookName = MarketBook.MarketBookName;
                MarketBookForProfitandlossAgent.MarketId = MarketBook.MarketId;
                MarketBookForProfitandlossAgent.Runners = new List<ExternalAPI.TO.Runner>();
                foreach (var runneritem in MarketBook.Runners)
                {
                    ExternalAPI.TO.Runner objNewRunner = new ExternalAPI.TO.Runner();
                    objNewRunner.SelectionId = runneritem.SelectionId;
                    objNewRunner.Handicap = runneritem.Handicap;
                    MarketBookForProfitandlossAgent.Runners.Add(objNewRunner);
                }
                // this.Title = MarketBook.MarketBookName;
                //  lblMarketName.Content = MarketBook.MarketBookName;
                if (MarketBook.MarketStatusstr == "ACTIVE")
                {
                    lblMarketStatus.Content = "GOING LIVE";
                }
                else
                {
                    lblMarketStatus.Content = MarketBook.MarketStatusstr;
                }
                lblMarketTime.Content = MarketBook.OpenDate;
                GetRunnersDataSource(MarketBook.Runners, MarketBook);
                DGVMarket.ItemsSource = lstMarketBookRunners;

                if (!MarketBook.MainSportsname.Contains("Racing"))
                {
                    DGVMarket.Columns[2].Visibility = Visibility.Collapsed;
                    DGVMarket.Columns[4].Visibility = Visibility.Collapsed;
                    if (MarketBook.Runners.Count == 2)
                    {
                        DGVMarket.Columns[1].Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    if (MarketBook.MainSportsname != "Horse Racing")
                    {
                        DGVMarket.Columns[2].Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        if (MarketBook.Runners[0].Clothnumber == "" || MarketBook.Runners[0].Clothnumber == "Not" || MarketBook.Runners[0].Clothnumber == null)
                        {
                            DGVMarket.Columns[2].Visibility = Visibility.Collapsed;
                        }
                    }
                    DGVMarket.Columns[1].Visibility = Visibility.Collapsed;
                    DGVMarket.Columns[6].Visibility = Visibility.Collapsed;
                }
               
                string[] marketbooknameandtype = MarketBook.MarketBookName.Split('/');

                if (MarketBook.MainSportsname == "Horse Racing")
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                        sktrace.Visibility = Visibility.Visible;
                    marketimg.Source = new BitmapImage(new Uri("ws7.png", UriKind.Relative));
                    marketType.Content = "Horse Race".ToUpper();
                    lblMarketName.Content = MarketBook.MarketBookName.ToUpper();
                    //backgroundWorkerGetScoreSCT.CancelAsync();
                    // backgroundWorkerGetScoreSCT.Dispose();
                    // backgroundWorkerGetScoreSCT = null;
                }
                else
                {

                    if (MarketBook.MainSportsname == "Greyhound Racing")
                    {
                        marketimg.Source = new BitmapImage(new Uri("ws4339.png", UriKind.Relative));
                        marketType.Content = "Greyhound Race".ToUpper();
                        lblMarketName.Content = MarketBook.MarketBookName.ToUpper();
                    }
                    else
                    {
                        if (MarketBook.MainSportsname == "Cricket")
                        {
                           // chkupdate = objUsersServiceCleint.GetBettingAllowedbyMarketIDandUserIDInplay(73);
                            marketimg.Source = new BitmapImage(new Uri("ws4.png", UriKind.Relative));
                            if (MarketBook.Runners.Count > 1)
                            {
                                marketType.Content = marketbooknameandtype[0].ToUpper();
                                lblMarketName.Content = marketbooknameandtype[1].ToUpper(); //+ "   V   " + marketbooknameandtype[2].ToUpper();
                            }
                            else
                            {
                                marketType.Content = "Line Market".ToUpper();
                                lblMarketName.Content = MarketBook.MarketBookName.ToUpper();
                            }
                        }
                        else
                        {
                            if (MarketBook.MainSportsname == "Soccer")
                            {
                                marketimg.Source = new BitmapImage(new Uri("ws1.png", UriKind.Relative));
                                marketType.Content = "Match Odds".ToUpper();
                                lblMarketName.Content = marketbooknameandtype[1].ToUpper(); //+ "   V   " + marketbooknameandtype[2].ToUpper();
                                GetToSoccerGoalMarket();
                                //backgroundWorkerGetScoreSCT.RunWorkerAsync();
                            }
                            else
                            {
                                if (MarketBook.MainSportsname == "Tennis")
                                {
                                    marketimg.Source = new BitmapImage(new Uri("ws2.png", UriKind.Relative));
                                    marketType.Content = "Match Odds".ToUpper();
                                    lblMarketName.Content = marketbooknameandtype[1].ToUpper(); //+ "   V   " + marketbooknameandtype[2].ToUpper();
                                    //backgroundWorkerGetScoreSCT.RunWorkerAsync();
                                }
                            }
                        }
                    }
                }


                if (MarketBook.MainSportsname == "Cricket" && MarketBook.MarketBookName.Contains("Match Odds"))
                {
                    SPMain.Visibility = Visibility.Visible;
                    //     matchCricketAPIKey = objUsersServiceCleint.GetCricketMatchKey(MarketBook.MarketId);
                    DGVMarketIndianFancy.ItemsSource = lstMarketBookRunnersFancyin;
                }


                MarketRulesAll = LoggedinUserDetail.MarketRulesAll;
                tmrUpdateMarket.Tick += TmrUpdateMarket_Tick;
                tmrUpdateMarket.Interval = TimeSpan.FromMilliseconds(200);

               

                if (MarketBook.MainSportsname.Contains("Cricket") && MarketBook.MarketBookName.Contains("Match Odds"))
                {
                    GetFancyMarkets();
                    backgroundWorkerLiabalityandScore.RunWorkerAsync();

                    // backgroundWorkerGetScore.RunWorkerAsync();
                    GetTowWinTheTossMarket();
                    if (LoggedinUserDetail.GetUserTypeID() != 1)
                    {
                        //backgroundWorkertotalmatched.RunWorkerAsync();
                    }

                   // if (MarketBook.LineVMarkets != null && LoggedinUserDetail.isTVShown == true)
                    //{
                        tmrUpdateLiabalities.Tick += TmrUpdateLiabalities_Tick;
                        tmrUpdateLiabalities.Interval = TimeSpan.FromMilliseconds(2000);
                        tmrUpdateLiabalities.Start();
                         backgroundWorkerUpdateFigData.RunWorkerAsync();
                        //backgroundWorkertotalmatched.RunWorkerAsync();
                  // }

                    //backgroundWorkerUpdateFigData.RunWorkerAsync();
                }
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    // ShowaverageSectionclick();
                    // ShowaverageSectionByAgentclick();

                    backgroundWorkerProfitandlossbyAgent.RunWorkerAsync();
                    //backgroundWorkertotalmatched.RunWorkerAsync();
                }
                tmrUpdateMarket.Start();
              
                //  backgroundWorkerUpdateData.RunWorkerAsync();
                //

                // backgroundWorkertotalmatched.RunWorkerAsync();


            }
            catch (System.Exception ex)
            {

            }
        }

        public bool chkupdate;
        private void ObjUsersServiceCleint_GetCricketMatchKeyCompleted(object sender, GetCricketMatchKeyCompletedEventArgs e)
        {

        }

        public void sizechanged()
        {
            try
            {
                pnlMenuBorder.Height = this.Height - 140;
                txtToBePlaced.HorizontalAlignment = HorizontalAlignment.Left;
                //scrlviewrBets.Height = this.Height - 150;
                if (LoggedinUserDetail.GetUserTypeID() == 1 || LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    //stkpnlmarketname.Width = this.Width - 185;
                    stkpnlmarketname.Margin = new Thickness(0, 0, 20, 0);
                    // txtBlockMarkettimeandstatus.Width = this.Width - 314;

                    txtToBePlaced.Width = this.Width - 70;
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        // btnFancyPL.Visibility = Visibility.Visible;
                        chkBettingAllowed.Visibility = Visibility.Visible;
                        stkpnlbetsandmenuheading.Width = this.Width - 232;
                    }
                    else
                    {
                        // btnFancyPL.Visibility = Visibility.Collapsed;
                        chkBettingAllowed.Visibility = Visibility.Collapsed;
                        stkpnlbetsandmenuheading.Width = this.Width - 157;
                    }
                }
                else
                {
                    //btnFancyPL.Visibility = Visibility.Collapsed;
                    chkBettingAllowed.Visibility = Visibility.Collapsed;
                    // stkpnlmarketname.Width = this.Width - 195;
                    stkpnlmarketname.Margin = new Thickness(80, 0, 0, 0);
                    //txtBlockMarkettimeandstatus.Width = this.Width - 284;
                    // stkpnlbetsandmenuheading.Width = this.Width - 157;
                    txtToBePlaced.Width = this.Width - 30;
                }

                //  stkpnlScores.Width = this.Width;

                if (MarketBook.MainSportsname.Contains("Racing"))
                {
                    DGVMarket.Height = this.Height - 170;
                    pnlMenuBorder.Height = this.Height - 100;
                    DGVMatchedBets.Height = this.Height - 325;
                    DGVMatchedBetsAdmin.Height = this.Height - 325;
                    DGVMatchedBetsaGENT.Height = this.Height - 325;
                }
                else
                {
                    if (MarketBook.Runners.Count > 10)
                    {
                        DGVMarket.Height = this.Height - 170;
                    }
                    pnlMenuBorder.Height = this.Height - 120;
                    DGVMatchedBets.Height = this.Height - 325;
                    DGVMatchedBetsAdmin.Height = this.Height - 325;
                    DGVMatchedBetsaGENT.Height = this.Height - 325;
                }
            }
            catch (System.Exception ex)
            {

            }
            Resizewindow();
        }
        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            sizechanged();
        }
        private void TextBlock_MouseLeftButtonUp_5(object sender, MouseButtonEventArgs e)
        {
            popupScoreCard.IsOpen = true;
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            popupScoreCard.IsOpen = false;
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            try
            {


                TeamASquad.Visibility = Visibility.Visible;
                TeamAScoreCard.Visibility = Visibility.Collapsed;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            try
            {

                TeamASquad.Visibility = Visibility.Collapsed;
                TeamAScoreCard.Visibility = Visibility.Visible;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            TeamBSquad.Visibility = Visibility.Visible;
            TeamBScoreCard.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            TeamBSquad.Visibility = Visibility.Collapsed;
            TeamBScoreCard.Visibility = Visibility.Visible;
        }

        private void TeamAName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void TeamBName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void TeamAName_Click(object sender, RoutedEventArgs e)
        {
            scorecardTeamA.Visibility = Visibility.Visible;
            TeamAName.Background = Brushes.LightGray;
            TeamBName.Background = Brushes.Transparent;
            scorecardTeamB.Visibility = Visibility.Collapsed;
        }

        private void TeamBName_Click(object sender, RoutedEventArgs e)
        {
            scorecardTeamA.Visibility = Visibility.Collapsed;
            scorecardTeamB.Visibility = Visibility.Visible;
            TeamAName.Background = Brushes.Transparent;
            TeamBName.Background = Brushes.LightGray;
        }

        private void cmbAgentsForProfitandLoss_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {


                SelectedAgentForProfitandLoss = Convert.ToInt32(cmbAgentsForProfitandLoss.SelectedValue);
                if (SelectedAgentForProfitandLoss == 0)
                {
                    SelectedAgentForProfitandLoss = 73;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

            txtAverageAmountbyAgent2.Content = "0";
            txtAverageAmountbyAgnet1.Content = "0";
            lblAveragebyAgent1.Content = "";
            lblAveragebyAgent2.Content = "";
            lblProfitandLossRunnerbyAgent2.Content = "0";
            lblProfitorLossRunnerAgent1.Content = "0";


        }

        private void btnFancyPL_Click(object sender, RoutedEventArgs e)
        {
            ProfitLossWindow objPLWin = new ProfitLossWindow();
            objPLWin.chkOnlyFancy.IsChecked = true;
            objPLWin.ShowDialog();

        }

        private void txtRulesNew_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RulesWindow objrules = new RulesWindow();
            objrules.rules = SetRulesbyName();
            objrules.Show();
        }
        private void lblBetSlipBack_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void lblBetSlipLay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (BetType == "back")
            {
                nupdownOdd.Value = Convert.ToDecimal(lblBetSlipBack.Content);
            }
            else
            {
                nupdownOdd.Value = Convert.ToDecimal(lblBetSlipLay.Content);
            }
        }

        private void lblBetSlipBack_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

        }

        private void lblBetSlipLay_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            if (BetType == "back")
            {
                nupdownOdd.Value = Convert.ToDecimal(lblBetSlipBack.Content);
            }
            else
            {
                nupdownOdd.Value = Convert.ToDecimal(lblBetSlipLay.Content);
            }
        }
        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            popupAllBets.IsOpen = false;
        }

        private void btnAllBets_Click(object sender, RoutedEventArgs e)
        {
            popupAllBets.IsOpen = true;
            UpdateUserBetsDataAll();

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            //MarketBook.LineVMarkets.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().isOpenedbyUser = true;
            int UserIDforLinevmarkets = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLinevmarkets = 73;
            }
            else
            {
                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            }
            objUsersServiceCleint.UpdateFancySyncONorOFFbyMarketIDAsync(UserIDforLinevmarkets, chkbox.Tag.ToString(), true);
        }
        private void btnAllBetsRefresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateUserBetsDataAll();
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            popupFancySyncONOFF.IsOpen = false;
            popupKalijuttSyncONOFF.IsOpen = false;
            popupSFigSyncONOFF.IsOpen = false;
            popupFigureSyncONOFF.IsOpen = false;
        }


        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            //MarketBook.LineVMarkets.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().isOpenedbyUser = true;
            int UserIDforLinevmarkets = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLinevmarkets = 73;
            }
            else
            {
                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            }
            objUsersServiceCleint.UpdateKJSyncONorOFFbyMarketIDAsync(UserIDforLinevmarkets, chkbox.Tag.ToString(), true);

        }
        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            //MarketBook.LineVMarkets.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().isOpenedbyUser = false;
            int UserIDforLinevmarkets = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLinevmarkets = 73;
            }
            else
            {
                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            }
            objUsersServiceCleint.UpdateKJSyncONorOFFbyMarketIDAsync(UserIDforLinevmarkets, chkbox.Tag.ToString(), false);
        }
        private void CheckBox_Checked_5(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            // MarketBook.LineVMarkets.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().isOpenedbyUser = true;
            int UserIDforLinevmarkets = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLinevmarkets = 73;
            }
            else
            {
                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            }
            objUsersServiceCleint.UpdateKJSyncONorOFFbyMarketIDAsync(UserIDforLinevmarkets, chkbox.Tag.ToString(), true);

        }

        private void CheckBox_Unchecked_3(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            //MarketBook.LineVMarkets.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().isOpenedbyUser = false;
            int UserIDforLinevmarkets = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLinevmarkets = 73;
            }
            else
            {
                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            }
            objUsersServiceCleint.UpdateKJSyncONorOFFbyMarketIDAsync(UserIDforLinevmarkets, chkbox.Tag.ToString(), false);
        }

        private void CheckBox_Checked_3(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            //MarketBook.LineVMarkets.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().isOpenedbyUser = true;
            int UserIDforLinevmarkets = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLinevmarkets = 73;
            }
            else
            {
                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            }
            objUsersServiceCleint.UpdateKJSyncONorOFFbyMarketIDAsync(UserIDforLinevmarkets, chkbox.Tag.ToString(), true);

            time = true;
        }
        private void CheckBox_Unchecked_2(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            //MarketBook.LineVMarkets.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().isOpenedbyUser = false;
            int UserIDforLinevmarkets = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLinevmarkets = 73;
            }
            else
            {
                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            }
            objUsersServiceCleint.UpdateKJSyncONorOFFbyMarketIDAsync(UserIDforLinevmarkets, chkbox.Tag.ToString(), false);

            time = true;
        }
        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            try
            {
                popupKalijuttSyncONOFF.IsOpen = true;
                lblKalijuttSyncONOFFFor.Content = MarketBook.MarketBookName;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {


            }
        }
        private void MenuItem_Click_14(object sender, RoutedEventArgs e)
        {
            try
            {
                popupSFigSyncONOFF.IsOpen = true;
                lblSFigSyncONOFFFor.Content = MarketBook.MarketBookName;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {


            }
        }
        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFigureSyncONOFF.IsOpen = true;
                lblFGSyncONOFFFor.Content = MarketBook.MarketBookName;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {


            }
        }
        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFancySyncONOFF.IsOpen = true;
                lblFancySyncONOFFFor.Content = MarketBook.MarketBookName;
            }
            catch (System.Exception ex)
            {

            }
        }

        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            popupCurrentPOSAgent.IsOpen = false;
        }

        private void btnGetPOSbyUserAgent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    if (cmbUsersCurrentPositionAgent.SelectedIndex > 0 && cmbRunnersCurrentPosition1Agent.SelectedIndex > -1)
                    {
                        string userbets = objUsersServiceCleint.GetUserbetsbyUserID(Convert.ToInt32(cmbUsersCurrentPositionAgent.SelectedValue), LoggedinUserDetail.PasswordForValidate);
                        List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(userbets);
                        if (lstUserBets.Count > 0)
                        {
                            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                            List<UserBets> lstUserBets1 = lstUserBets.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBook.MarketId).ToList();

                            var currentusermarketbook = MarketBook;
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
                            long currentprofitandloss = currentusermarketbook.Runners.Where(item => item.SelectionId == cmbRunnersCurrentPosition1Agent.SelectedValue.ToString()).FirstOrDefault().ProfitandLoss;
                            if (currentprofitandloss >= 0)
                            {
                                lblCurrentPostitionUserPL1Agent.Foreground = Brushes.Green;
                            }
                            else
                            {
                                lblCurrentPostitionUserPL1Agent.Foreground = Brushes.Red;
                            }
                            lblCurrentPostitionUserPL1Agent.Content = currentprofitandloss.ToString();

                        }
                        else
                        {
                            lblCurrentPostitionUserPL1Agent.Content = "0";
                        }
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {


            }
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            try
            {
                popupCurrentPOSAgent.IsOpen = true;

                var results = objUsersServiceCleint.GetSelectionNamesbyMarketID(MarketBook.MarketId);
                cmbRunnersCurrentPosition1Agent.ItemsSource = null;
                cmbRunnersCurrentPosition1Agent.IsSynchronizedWithCurrentItem = false;
                cmbRunnersCurrentPosition1Agent.ItemsSource = results;
                cmbRunnersCurrentPosition1Agent.DisplayMemberPath = "SelectionName";
                cmbRunnersCurrentPosition1Agent.SelectedValuePath = "SelectionID";

                AssignUSerstoComboboxAgent();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {


            }
        }
        public void AssignUSerstoComboboxAgent()
        {
            //For Agent
            cmbUsersCurrentPositionAgent.IsSynchronizedWithCurrentItem = false;
            cmbUsersCurrentPositionAgent.ItemsSource = LoggedinUserDetail.AllUsers;
            cmbUsersCurrentPositionAgent.DisplayMemberPath = "UserName";
            cmbUsersCurrentPositionAgent.SelectedValuePath = "ID";
            //
        }

        private void btnRules_Click(object sender, RoutedEventArgs e)
        {
            RulesWindow objrules = new RulesWindow();
            objrules.rules = SetRulesbyName();
            objrules.Show();
        }
        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Left)
        //        this.DragMove();
        //}

        private void chkUpdateBettingAllowed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chkBettingAllowed.IsChecked == true)
                {
                    objUsersServiceCleint.UpdateBettingAllowed(MarketBook.MarketId, "0");
                    Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
                }
                else
                {

                }
            }
            catch (System.Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                popupIndianFancyResultPosting.IsOpen = true;
                //ShowFancyResultsPostingPanel();
                ShowFancyResultsPostingPanelIN();
            }
        }
        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                popupFancyResultPostingKJ.IsOpen = true;
                ShowFancyResultsPostingPanelKJ();

            }
        }
        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                popupFancyResultPostingFig.IsOpen = true;
                ShowFancyResultsPostingPanelFig();

            }
        }
        private void MenuItem_Click_13(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                popupFancyResultPostingSFig.IsOpen = true;
                ShowFancyResultsPostingPanelSFig();

            }
        }

        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            popupIndianFancyResultPosting.IsOpen = false;
        }



        private void btnPostScoresI_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRunnersFancyResultsIN.SelectedIndex > -1)
            {
                MessageBoxResult messgeresult = Xceed.Wpf.Toolkit.MessageBox.Show("Post Results ?", "Confirm Post Results", MessageBoxButton.YesNo);
                if (messgeresult == MessageBoxResult.Yes)
                {
                    
                    objUsersServiceCleint.CheckforMatchCompletedFancyIN(cmbRunnersFancyResultsIN.SelectedValue.ToString(), cmbRunnersFancyResultsIN.Text, Convert.ToInt32(txtScoresResultsIN.Text));
                    popupIndianFancyResultPosting.IsOpen = false;
                    Xceed.Wpf.Toolkit.MessageBox.Show("Posted Succesfully.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        private void btnPostScoresKJ_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRunnersFancyResultsKJ.SelectedIndex > -1)
            {
                MessageBoxResult messgeresult = Xceed.Wpf.Toolkit.MessageBox.Show("Post Results ?", "Confirm Post Results", MessageBoxButton.YesNo);
                if (messgeresult == MessageBoxResult.Yes)
                {
                    string aa = cmbRunnersFancyResultsKJ.SelectedValue.ToString();
                    objUsersServiceCleint.CheckforMatchCompletedFancyKJ(cmbRunnersFancyResultsKJ.SelectedValue.ToString(), Convert.ToInt32(txtInningsResultKJ.Text), Convert.ToInt32(txtScoresResultsKJ.Text));
                    popupFancyResultPostingKJ.IsOpen = false;
                    Xceed.Wpf.Toolkit.MessageBox.Show("Posted Succesfully.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        private void btnPostScoresSFig_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRunnersFancyResultsSFig.SelectedIndex > -1)
            {
                MessageBoxResult messgeresult = Xceed.Wpf.Toolkit.MessageBox.Show("Post Results ?", "Confirm Post Results", MessageBoxButton.YesNo);
                if (messgeresult == MessageBoxResult.Yes)
                {
                    string aa = cmbRunnersFancyResultsSFig.SelectedValue.ToString();
                    objUsersServiceCleint.CheckforMatchCompletedSmallFig(cmbRunnersFancyResultsSFig.SelectedValue.ToString(), Convert.ToInt32(txtInningsResultSFig.Text), Convert.ToInt32(txtScoresResultsSFig.Text));
                    popupFancyResultPostingSFig.IsOpen = false;
                    Xceed.Wpf.Toolkit.MessageBox.Show("Posted Succesfully.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }


        private void btnPostScoresFig_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRunnersFancyResultsFig.SelectedIndex > -1)
            {
                MessageBoxResult messgeresult = Xceed.Wpf.Toolkit.MessageBox.Show("Post Results ?", "Confirm Post Results", MessageBoxButton.YesNo);
                if (messgeresult == MessageBoxResult.Yes)
                {
                    string aa = cmbRunnersFancyResultsFig.SelectedValue.ToString();
                    objUsersServiceCleint.CheckforMatchCompletedFancyFig(cmbRunnersFancyResultsFig.SelectedValue.ToString(), Convert.ToInt32(txtInningsResultFig.Text), Convert.ToInt32(txtScoresResultsFig.Text));
                    popupFancyResultPostingFig.IsOpen = false;
                    Xceed.Wpf.Toolkit.MessageBox.Show("Posted Succesfully.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }


        private void DGVMarketKalijut_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DGVMarketKalijut.Items.Count > 0)
                {
                    DataGrid objSender = (DataGrid)sender;
                    MarketBookShow objSelectedRow = (MarketBookShow)objSender.SelectedItem;
                    int currcellindx = objSender.CurrentCell.Column.DisplayIndex;
                    //marketcategory
                    CategoryName = objSelectedRow.CategoryName;
                    MarketbooknameBet = objSelectedRow.MarketbooknameBet;
                    Marketstatusstr = objSelectedRow.Marketstatusstr;
                    BettingAllowed = objSelectedRow.BettingAllowed;
                    OpenDate = objSelectedRow.OpenDate;
                    runnerscount = objSelectedRow.runnerscount;
                    CurrentMarketBookId = objSelectedRow.CurrentMarketBookId;
                    // MarketBook currmarketbookforbet1 = (MarketBook)grid.Tag;
                    ////
                    if (objSelectedRow.Backprice0 == "0" || objSelectedRow.Layprice0 == "0")
                    {
                        return;
                    }
                    if (objSelectedRow.RunnerStatusstr == "SUSPENDED" || objSelectedRow.RunnerStatusstr == "Ball Running")
                    {
                        return;
                    }
                    if ((currcellindx >= 4 && currcellindx <= 12))
                    {
                        //if (Allowedbetting(BettingAllowed, Marketstatusstr, MarketbooknameBet, CategoryName, OpenDate, runnerscount, CurrentMarketBookId, true) == true)
                        // {
                        Clickedlocationin = 8;
                        if (LoggedinUserDetail.GetUserTypeID() == 3 && LoggedinUserDetail.isInserting == false)
                        {
                           // loadedlocation = -1;
                           // clickedbetsize = -1;
                            clickedodd = 0;
                            ParentID = 0;
                            SelectionID = objSelectedRow.SelectionID;
                            Selectionname = objSelectedRow.Selection;
                            if (objSelectedRow.Clothnumber != null && objSelectedRow.Clothnumber != "Not")
                            {
                                Selectionname = objSelectedRow.Clothnumber + "-" + Selectionname;
                            }

                            if (currcellindx == 4)
                            {
                                //Clickedlocation = 9 - currcellindx;
                                betslipoddareain.Background = Brushes.LightSkyBlue;
                                lblBetSlipHeadingin.Content = "You are going to back " + objSelectedRow.Selection;
                                lblBetSelectionnamein.Content = objSelectedRow.Selection.ToString();
                                lblBetSlipBackin.Content = objSelectedRow.Backprice0;
                                lblBetslipBackOdd1in.Content = objSelectedRow.Backprice1;
                                lblBetslipBackOdd2in.Content = objSelectedRow.Backprice2;
                                lblBetslipBackSize0in.Content = objSelectedRow.Backsize0;
                                //  lblBetslipBackSize1.Content = objSelectedRow.Backsize1;
                                //  lblBetslipBackSize2.Content = objSelectedRow.Backsize2;
                                Betslipsizein.Text = objSelectedRow.Backsize0;
                                lblBetSlipLayin.Content = objSelectedRow.Layprice0;
                                //  lblBetslipLayOdd1.Content = objSelectedRow.Layprice1;
                                //  lblBetslipLayOdd2.Content = objSelectedRow.Layprice2;
                                lblBetslipLaySize0in.Content = objSelectedRow.Laysize0;
                                //  lblBetslipLaySize1.Content = objSelectedRow.Laysize1;
                                // lblBetslipLaySize2.Content = objSelectedRow.Laysize2;
                                nupdownOddin.Text = objSelectedRow.Backprice0;
                                nupdownsizein.Text = objSelectedRow.Backsize0;


                                BetSlipSelectionID = objSelectedRow.SelectionID;
                                BetType = "back";
                                popupBetslipforin.Visibility = Visibility.Visible;
                                popupBetslipforin.IsOpen = true;
                                calculateProfitandLossonBetSlipin();
                                nupdnUserAmountin.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeBack);
                            }
                            else
                            {

                                // Clickedlocation = currcellindx - 10;
                                betslipoddareain.Background = Brushes.LightPink;
                                lblBetSlipHeadingin.Content = "You are going to lay " + objSelectedRow.Selection;
                                lblBetSelectionnamein.Content = objSelectedRow.Selection.ToString();
                                lblBetSlipBackin.Content = objSelectedRow.Backprice0;
                                lblBetslipBackOdd1in.Content = objSelectedRow.Backprice1;
                                lblBetslipBackOdd2in.Content = objSelectedRow.Backprice2;
                                lblBetslipBackSize0in.Content = objSelectedRow.Backsize0;
                                lblBetslipBackSize1in.Content = objSelectedRow.Backsize1;
                                lblBetslipBackSize2in.Content = objSelectedRow.Backsize2;
                                Betslipsizein.Text = objSelectedRow.Laysize0;
                                lblBetSlipLay.Content = objSelectedRow.Layprice0;
                                lblBetslipLayOdd1in.Content = objSelectedRow.Layprice1;
                                lblBetslipLayOdd2in.Content = objSelectedRow.Layprice2;
                                lblBetslipLaySize0in.Content = objSelectedRow.Laysize0;
                                lblBetslipLaySize1in.Content = objSelectedRow.Laysize1;
                                lblBetslipLaySize2in.Content = objSelectedRow.Laysize2;
                                nupdownOddin.Text = objSelectedRow.Layprice0;
                                nupdownsizein.Text = objSelectedRow.Laysize0;
                                SetIncrementforNumericUpdown(Convert.ToDecimal(objSelectedRow.Layprice0));
                                BetSlipSelectionID = objSelectedRow.SelectionID;
                                BetType = "lay";
                                popupBetslipforin.Visibility = Visibility.Visible;
                                popupBetslipforin.IsOpen = true;
                                calculateProfitandLossonBetSlipin();
                                nupdnUserAmountin.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeLay);
                            }
                        }
                        // }
                        //else
                        //{
                        //    PlayMessage("Betting is not allowed");
                        //    Xceed.Wpf.Toolkit.MessageBox.Show("Betting is not allowed.", "Global Traders", MessageBoxButton.OK);
                        //    //    MessageBox.Show(this, "Betting is not allowed.");
                        //}

                    }

                    if ((currcellindx < 5 && currcellindx > 0))
                    {
                        // bool chkvalue = (bool)(grid[intRow, 1]);
                        if (Marketstatusstr == "Closed" || Marketstatusstr == "Suspended")
                        {
                            // grid[intRow, colSel] = false;
                            return;
                        }
                        if (!MarketBook.MainSportsname.Contains("Racing"))
                        {
                            // grid[intRow, colSel] = false;
                            return;
                        }
                        if (objSelectedRow.isSelected == true)
                        {
                            objSelectedRow.isSelected = false;
                        }
                        else
                        {
                            objSelectedRow.isSelected = true;
                        }
                    }
                    else
                    {

                    }

                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void DGVMarketSFig_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DGVMarketSFig.Items.Count > 0)
                {
                    DataGrid objSender = (DataGrid)sender;
                    MarketBookShow objSelectedRow = (MarketBookShow)objSender.SelectedItem;
                    int currcellindx = objSender.CurrentCell.Column.DisplayIndex;
                    //marketcategory
                    CategoryName = objSelectedRow.CategoryName;
                    MarketbooknameBet = objSelectedRow.MarketbooknameBet;
                    Marketstatusstr = objSelectedRow.Marketstatusstr;
                    BettingAllowed = objSelectedRow.BettingAllowed;
                    OpenDate = objSelectedRow.OpenDate;
                    runnerscount = objSelectedRow.runnerscount;
                    CurrentMarketBookId = objSelectedRow.CurrentMarketBookId;
                    Clickedlocationin = 8;


                    if (currcellindx == 0)
                    {
                        foreach (Window win in App.Current.Windows)
                        {
                            if (win.Name == "BookPostionForINwin" + CurrentMarketBookId.Replace(".", ""))
                            {

                                win.Close();

                            }
                        }
                        BookPostionForIN objbookpostion = new BookPostionForIN();
                        objbookpostion.Name = "BookPostionForINwin" + CurrentMarketBookId.Replace(".", "");
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            objbookpostion.CurrentUserbetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList();

                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 2)
                            {
                                objbookpostion.CurrentUserbetsAgent = LoggedinUserDetail.CurrentAgentBets;
                            }
                            else
                            {
                                objbookpostion.CurrentUserbets = LoggedinUserDetail.CurrentUserBets;
                            }
                        }
                        objbookpostion.marketBookID = MarketBook.EventID;
                        objbookpostion.eventID = objSelectedRow.SelectionID;

                        objbookpostion.marketbookName = MarketbooknameBet + "(" + lblMarketName.Content.ToString() + ")";
                        objbookpostion.UserTypeID = LoggedinUserDetail.GetUserTypeID();
                        objbookpostion.userID = LoggedinUserDetail.GetUserID();
                        objbookpostion.Show();
                        return;
                    }
                    if ((currcellindx >= 4 && currcellindx <= 12))
                    {
                        // if (Allowedbetting(BettingAllowed, Marketstatusstr, MarketbooknameBet, CategoryName, OpenDate, runnerscount, CurrentMarketBookId, true) == true)
                        //{
                        if (LoggedinUserDetail.GetUserTypeID() == 3 && LoggedinUserDetail.isInserting == false)
                        {
                           // loadedlocation = -1;
                           // clickedbetsize = -1;
                            clickedodd = 0;
                            ParentID = 0;

                            SelectionID = objSelectedRow.SelectionID;
                            Selectionname = objSelectedRow.Selection;
                            if (objSelectedRow.Clothnumber != null && objSelectedRow.Clothnumber != "Not")
                            {
                                Selectionname = objSelectedRow.Clothnumber + "-" + Selectionname;
                            }

                            if (currcellindx == 4)
                            {

                               // Clickedlocation = 9 - currcellindx;
                                betslipoddareain.Background = Brushes.LightSkyBlue;
                                lblBetSlipHeadingin.Content = "You are going to back " + objSelectedRow.Selection;
                                lblBetSelectionnamein.Content = objSelectedRow.Selection.ToString();
                                lblBetSlipBackin.Content = objSelectedRow.Backprice0;
                                lblBetslipBackOdd1in.Content = objSelectedRow.Backprice1;
                                lblBetslipBackOdd2in.Content = objSelectedRow.Backprice2;
                                lblBetslipBackSize0in.Content = objSelectedRow.Backsize0;
                                //  lblBetslipBackSize1.Content = objSelectedRow.Backsize1;
                                //  lblBetslipBackSize2.Content = objSelectedRow.Backsize2;
                                Betslipsizein.Text = objSelectedRow.Backsize0;
                                lblBetSlipLayin.Content = objSelectedRow.Layprice0;
                                //  lblBetslipLayOdd1.Content = objSelectedRow.Layprice1;
                                //  lblBetslipLayOdd2.Content = objSelectedRow.Layprice2;
                                lblBetslipLaySize0in.Content = objSelectedRow.Laysize0;
                                //  lblBetslipLaySize1.Content = objSelectedRow.Laysize1;
                                // lblBetslipLaySize2.Content = objSelectedRow.Laysize2;
                                nupdownOddin.Text = objSelectedRow.Backprice0;
                                nupdownsizein.Text = objSelectedRow.Backsize0;
                                SetIncrementforNumericUpdown(Convert.ToDecimal(objSelectedRow.Backprice0));
                                BetSlipSelectionID = objSelectedRow.SelectionID;
                                BetType = "back";
                                popupBetslipforin.Visibility = Visibility.Visible;
                                popupBetslipforin.IsOpen = true;
                                calculateProfitandLossonBetSlipin();
                                nupdnUserAmountin.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeBack);
                            }
                            else
                            {

                               // Clickedlocation = currcellindx - 10;
                                betslipoddareain.Background = Brushes.LightPink;
                                lblBetSlipHeadingin.Content = "You are going to lay " + objSelectedRow.Selection;
                                lblBetSelectionnamein.Content = objSelectedRow.Selection.ToString();
                                lblBetSlipBackin.Content = objSelectedRow.Backprice0;
                                lblBetslipBackOdd1in.Content = objSelectedRow.Backprice1;
                                lblBetslipBackOdd2in.Content = objSelectedRow.Backprice2;
                                lblBetslipBackSize0in.Content = objSelectedRow.Backsize0;
                                lblBetslipBackSize1in.Content = objSelectedRow.Backsize1;
                                lblBetslipBackSize2in.Content = objSelectedRow.Backsize2;
                                //
                                Betslipsizein.Text = objSelectedRow.Laysize0;
                                //
                                lblBetSlipLayin.Content = objSelectedRow.Layprice0;
                                lblBetslipLayOdd1in.Content = objSelectedRow.Layprice1;
                                lblBetslipLayOdd2in.Content = objSelectedRow.Layprice2;
                                lblBetslipLaySize0in.Content = objSelectedRow.Laysize0;
                                lblBetslipLaySize1in.Content = objSelectedRow.Laysize1;
                                lblBetslipLaySize2in.Content = objSelectedRow.Laysize2;
                                nupdownOddin.Text = objSelectedRow.Layprice0;
                                nupdownsizein.Text = objSelectedRow.Laysize0;
                                SetIncrementforNumericUpdown(Convert.ToDecimal(objSelectedRow.Layprice0));
                                BetSlipSelectionID = objSelectedRow.SelectionID;
                                BetType = "lay";
                                popupBetslipforin.Visibility = Visibility.Visible;
                                popupBetslipforin.IsOpen = true;
                                calculateProfitandLossonBetSlipin();
                                nupdnUserAmountin.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeLay);
                            }

                        }

                        //}
                        //else
                        //{
                        //    PlayMessage("Betting is not allowed");
                        //    Xceed.Wpf.Toolkit.MessageBox.Show("Betting is not allowed.", "Global Traders", MessageBoxButton.OK);
                        //    //    MessageBox.Show(this, "Betting is not allowed.");
                        //}

                    }
                    ////


                    if ((currcellindx < 5 && currcellindx > 0))
                    {
                        // bool chkvalue = (bool)(grid[intRow, 1]);
                        if (Marketstatusstr == "Closed" || Marketstatusstr == "Suspended")
                        {
                            // grid[intRow, colSel] = false;
                            return;
                        }
                        if (!MarketBook.MainSportsname.Contains("Racing"))
                        {
                            // grid[intRow, colSel] = false;
                            return;
                        }
                        if (objSelectedRow.isSelected == true)
                        {
                            objSelectedRow.isSelected = false;
                        }

                        else
                        {
                            objSelectedRow.isSelected = true;
                        }


                    }
                    else
                    {

                    }

                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void DGVMarketFigure_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DGVMarketFigure.Items.Count > 0)
                {
                    DataGrid objSender = (DataGrid)sender;
                    MarketBookShow objSelectedRow = (MarketBookShow)objSender.SelectedItem;
                    int currcellindx = objSender.CurrentCell.Column.DisplayIndex;
                    //marketcategory
                    CategoryName = objSelectedRow.CategoryName;
                    MarketbooknameBet = objSelectedRow.MarketbooknameBet;
                    Marketstatusstr = objSelectedRow.Marketstatusstr;
                    BettingAllowed = objSelectedRow.BettingAllowed;
                    OpenDate = objSelectedRow.OpenDate;
                    runnerscount = objSelectedRow.runnerscount;
                    CurrentMarketBookId = objSelectedRow.CurrentMarketBookId;
                    Clickedlocationin = 8;


                    if (currcellindx == 0)
                    {
                        foreach (Window win in App.Current.Windows)
                        {
                            if (win.Name == "BookPostionForINwin" + CurrentMarketBookId.Replace(".", ""))
                            {

                                win.Close();

                            }
                        }
                        BookPostionForIN objbookpostion = new BookPostionForIN();
                        objbookpostion.Name = "BookPostionForINwin" + CurrentMarketBookId.Replace(".", "");
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            objbookpostion.CurrentUserbetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList();
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 2)
                            {
                                objbookpostion.CurrentUserbetsAgent = LoggedinUserDetail.CurrentAgentBets;
                            }
                            else
                            {
                                objbookpostion.CurrentUserbets = LoggedinUserDetail.CurrentUserBets;
                            }
                        }
                        objbookpostion.marketBookID = MarketBook.EventID;
                        objbookpostion.eventID = objSelectedRow.SelectionID;

                        objbookpostion.marketbookName = MarketbooknameBet + "(" + lblMarketName.Content.ToString() + ")";
                        objbookpostion.UserTypeID = LoggedinUserDetail.GetUserTypeID();
                        objbookpostion.userID = LoggedinUserDetail.GetUserID();
                        objbookpostion.Show();
                        return;
                    }
                    if ((currcellindx >= 4 && currcellindx <= 12))
                    {
                        //if (Allowedbetting(BettingAllowed, Marketstatusstr, MarketbooknameBet, CategoryName, OpenDate, runnerscount, CurrentMarketBookId, true) == true)
                        //{
                        if (LoggedinUserDetail.GetUserTypeID() == 3 && LoggedinUserDetail.isInserting == false)
                        {
                            loadedlocation = -1;
                            clickedbetsize = -1;
                            clickedodd = 0;
                            ParentID = 0;

                            SelectionID = objSelectedRow.SelectionID;
                            Selectionname = objSelectedRow.Selection;
                            if (objSelectedRow.Clothnumber != null && objSelectedRow.Clothnumber != "Not")
                            {
                                Selectionname = objSelectedRow.Clothnumber + "-" + Selectionname;
                            }

                            if (currcellindx == 4)
                            {
                                //Clickedlocation = 9 - currcellindx;
                                betslipoddareain.Background = Brushes.LightSkyBlue;
                                lblBetSlipHeadingin.Content = "You are going to back " + objSelectedRow.Selection;
                                lblBetSelectionnamein.Content = objSelectedRow.Selection.ToString();
                                lblBetSlipBackin.Content = objSelectedRow.Backprice0;
                                lblBetslipBackOdd1in.Content = objSelectedRow.Backprice1;
                                lblBetslipBackOdd2in.Content = objSelectedRow.Backprice2;
                                lblBetslipBackSize0in.Content = objSelectedRow.Backsize0;
                                //  lblBetslipBackSize1.Content = objSelectedRow.Backsize1;
                                //  lblBetslipBackSize2.Content = objSelectedRow.Backsize2;
                                Betslipsizein.Text = objSelectedRow.Backsize0;
                                lblBetSlipLayin.Content = objSelectedRow.Layprice0;
                                //  lblBetslipLayOdd1.Content = objSelectedRow.Layprice1;
                                //  lblBetslipLayOdd2.Content = objSelectedRow.Layprice2;
                                lblBetslipLaySize0in.Content = objSelectedRow.Laysize0;
                                //  lblBetslipLaySize1.Content = objSelectedRow.Laysize1;
                                // lblBetslipLaySize2.Content = objSelectedRow.Laysize2;
                                nupdownOddin.Text = objSelectedRow.Backprice0;
                                nupdownsizein.Text = objSelectedRow.Backsize0;
                                SetIncrementforNumericUpdown(Convert.ToDecimal(objSelectedRow.Backprice0));
                                BetSlipSelectionID = objSelectedRow.SelectionID;
                                BetType = "back";
                                popupBetslipforin.Visibility = Visibility.Visible;
                                popupBetslipforin.IsOpen = true;
                                calculateProfitandLossonBetSlipin();
                                nupdnUserAmountin.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeBack);
                            }
                            else
                            {

                               // Clickedlocation = currcellindx - 10;
                                betslipoddareain.Background = Brushes.LightPink;
                                lblBetSlipHeadingin.Content = "You are going to lay " + objSelectedRow.Selection;
                                lblBetSelectionnamein.Content = objSelectedRow.Selection.ToString();
                                lblBetSlipBackin.Content = objSelectedRow.Backprice0;
                                lblBetslipBackOdd1in.Content = objSelectedRow.Backprice1;
                                lblBetslipBackOdd2in.Content = objSelectedRow.Backprice2;
                                lblBetslipBackSize0in.Content = objSelectedRow.Backsize0;
                                lblBetslipBackSize1in.Content = objSelectedRow.Backsize1;
                                lblBetslipBackSize2in.Content = objSelectedRow.Backsize2;
                                //
                                Betslipsizein.Text = objSelectedRow.Laysize0;
                                //
                                lblBetSlipLayin.Content = objSelectedRow.Layprice0;
                                lblBetslipLayOdd1in.Content = objSelectedRow.Layprice1;
                                lblBetslipLayOdd2in.Content = objSelectedRow.Layprice2;
                                lblBetslipLaySize0in.Content = objSelectedRow.Laysize0;
                                lblBetslipLaySize1in.Content = objSelectedRow.Laysize1;
                                lblBetslipLaySize2in.Content = objSelectedRow.Laysize2;
                                nupdownOddin.Text = objSelectedRow.Layprice0;
                                nupdownsizein.Text = objSelectedRow.Laysize0;
                                SetIncrementforNumericUpdown(Convert.ToDecimal(objSelectedRow.Layprice0));
                                BetSlipSelectionID = objSelectedRow.SelectionID;
                                BetType = "lay";
                                popupBetslipforin.Visibility = Visibility.Visible;
                                popupBetslipforin.IsOpen = true;
                                calculateProfitandLossonBetSlipin();
                                nupdnUserAmountin.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeLay);
                            }
                        }
                        //}
                        //else
                        //{
                        //    PlayMessage("Betting is not allowed");
                        //    Xceed.Wpf.Toolkit.MessageBox.Show("Betting is not allowed.", "Global Traders", MessageBoxButton.OK);
                        //    //    MessageBox.Show(this, "Betting is not allowed.");
                        //}
                    }

                    if ((currcellindx < 5 && currcellindx > 0))
                    {
                        // bool chkvalue = (bool)(grid[intRow, 1]);
                        if (Marketstatusstr == "Closed" || Marketstatusstr == "Suspended")
                        {
                            // grid[intRow, colSel] = false;
                            return;
                        }
                        if (!MarketBook.MainSportsname.Contains("Racing"))
                        {
                            // grid[intRow, colSel] = false;
                            return;
                        }
                        if (objSelectedRow.isSelected == true)
                        {
                            objSelectedRow.isSelected = false;
                        }

                        else
                        {
                            objSelectedRow.isSelected = true;
                        }
                    }
                    else
                    {

                    }

                }
            }
            catch (System.Exception ex)
            {

            }
        }
        
       
       private void DGVMarketIndianFancy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        try
            {
                if (DGVMarketIndianFancy.Items.Count > 0)
                {
                    DataGrid objSender = (DataGrid)sender;
                    MarketBookShow objSelectedRow = (MarketBookShow)objSender.SelectedItem;
                    int currcellindx = objSender.CurrentCell.Column.DisplayIndex;

                    CategoryName = objSelectedRow.CategoryName;
                    MarketbooknameBet = objSelectedRow.MarketbooknameBet;
                    Marketstatusstr = objSelectedRow.Marketstatusstr;
                    BettingAllowed = objSelectedRow.BettingAllowed;
                    OpenDate = objSelectedRow.OpenDate;
                    runnerscount = objSelectedRow.runnerscount;
                    CurrentMarketBookId = objSelectedRow.CurrentMarketBookId;
                    Clickedlocationin = 9;
                    if (currcellindx == 0)
                    {
                        foreach (Window win in App.Current.Windows)
                        {
                            if (win.Name == "BookPostionForINwin" + CurrentMarketBookId.Replace(".", ""))
                            {
                                win.Close();
                            }
                        }
                        BookPostionForIN objbookpostion = new BookPostionForIN();

                        objbookpostion.selectionID = objSelectedRow.SelectionID;

                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            objbookpostion.CurrentUserbetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList();
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 2)
                            {
                                objbookpostion.CurrentUserbetsAgent = LoggedinUserDetail.CurrentAgentBets;
                            }
                            else
                            {
                                objbookpostion.CurrentUserbets = LoggedinUserDetail.CurrentUserBets;
                            }
                        }
                        objbookpostion.marketBookID = objSelectedRow.CurrentMarketBookId;
                        objbookpostion.eventID = objSelectedRow.SelectionID;
                        objbookpostion.marketbookName = MarketbooknameBet + "(" + lblMarketName.Content.ToString() + ")";
                        objbookpostion.UserTypeID = LoggedinUserDetail.GetUserTypeID();
                        objbookpostion.userID = LoggedinUserDetail.GetUserID();
                        objbookpostion.Show();
                        return;
                    }
                    if (objSelectedRow.Backprice0 == "0" || objSelectedRow.Layprice0 == "0")
                    {
                        return;
                    }
                    //if (objSelectedRow.RunnerStatusstr == "SUSPENDED" || objSelectedRow.RunnerStatusstr == "Ball Running")
                    //{
                    //    return;
                    //}

                    if ((currcellindx >= 6 && currcellindx <= 12))
                    {

                        if (LoggedinUserDetail.GetUserTypeID() == 3 && LoggedinUserDetail.isInserting == false)
                        {
                            loadedlocation = -1;
                            clickedbetsize = -1;
                            clickedodd = 0;
                            ParentID = 0;
                            SelectionID = objSelectedRow.SelectionID;
                            Selectionname = objSelectedRow.Selection;
                            if (objSelectedRow.Clothnumber != null && objSelectedRow.Clothnumber != "Not")
                            {
                                Selectionname = objSelectedRow.Clothnumber + "-" + Selectionname;
                            }

                            if (currcellindx == 6)
                            {
                                Clickedlocation = 9 - currcellindx;
                                betslipoddareain1.Background = Brushes.LightSkyBlue;
                                lblBetSlipHeadingin1.Content = "You are going to back " + objSelectedRow.Selection;
                                lblBetSelectionnamein1.Content = objSelectedRow.Selection.ToString();
                                lblBetSlipBackin1.Content = objSelectedRow.Backprice0;
                                lblBetslipBackOdd1in1.Content = objSelectedRow.Backprice1;
                                lblBetslipBackOdd2in1.Content = objSelectedRow.Backprice2;
                                lblBetslipBackSize0in1.Content = objSelectedRow.Backsize0;
                                Betslipsizein1.Text = objSelectedRow.Backsize0;
                                lblBetSlipLayin1.Content = objSelectedRow.Layprice0;
                                lblBetslipLaySize0in1.Content = objSelectedRow.Laysize0;
                                nupdownsizein1.Text = objSelectedRow.Backsize0;
                                nupdownOddin1.Text = objSelectedRow.Backprice0;
                                SetIncrementforNumericUpdown(Convert.ToDecimal(objSelectedRow.Backprice0));
                                BetSlipSelectionID = objSelectedRow.SelectionID;
                                BetType = "back";
                                popupBetslipforin1.Visibility = Visibility.Visible;
                                popupBetslipforin1.IsOpen = true;
                                calculateProfitandLossonBetSlipin1();
                                nupdnUserAmountin1.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeBack);
                            }
                            else
                            {
                                Clickedlocation = currcellindx - 10;
                                betslipoddareain1.Background = Brushes.LightPink;
                                lblBetSlipHeadingin1.Content = "You are going to lay " + objSelectedRow.Selection;
                                lblBetSelectionnamein1.Content = objSelectedRow.Selection.ToString();
                                lblBetSlipBackin1.Content = objSelectedRow.Backprice0;
                                lblBetslipBackOdd1in1.Content = objSelectedRow.Backprice1;
                                lblBetslipBackOdd2in1.Content = objSelectedRow.Backprice2;
                                lblBetslipBackSize0in1.Content = objSelectedRow.Backsize0;
                                lblBetslipBackSize1in1.Content = objSelectedRow.Backsize1;
                                lblBetslipBackSize2in1.Content = objSelectedRow.Backsize2;
                                Betslipsizein1.Text = objSelectedRow.Laysize0;
                                lblBetSlipLayin1.Content = objSelectedRow.Layprice0;
                                lblBetslipLayOdd1in1.Content = objSelectedRow.Layprice1;
                                lblBetslipLayOdd2in1.Content = objSelectedRow.Layprice2;
                                lblBetslipLaySize0in1.Content = objSelectedRow.Laysize0;
                                lblBetslipLaySize1in1.Content = objSelectedRow.Laysize1;
                                lblBetslipLaySize2in1.Content = objSelectedRow.Laysize2;
                                nupdownsizein1.Text = objSelectedRow.Laysize0;
                                nupdownOddin1.Text = objSelectedRow.Layprice0;
                                SetIncrementforNumericUpdown(Convert.ToDecimal(objSelectedRow.Layprice0));
                                BetSlipSelectionID = objSelectedRow.SelectionID;
                                BetType = "lay";
                                popupBetslipforin1.Visibility = Visibility.Visible;
                                popupBetslipforin1.IsOpen = true;
                                calculateProfitandLossonBetSlipin1();
                                nupdnUserAmountin1.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeLay);
                            }

                        }



                    }

                    if ((currcellindx < 5 && currcellindx > 0))
                    {
                        if (Marketstatusstr == "Closed" || Marketstatusstr == "Suspended")
                        {
                            return;
                        }
                        if (!MarketBook.MainSportsname.Contains("Racing"))
                        {
                            return;
                        }
                        if (objSelectedRow.isSelected == true)
                        {
                            objSelectedRow.isSelected = false;
                        }

                        else
                        {
                            objSelectedRow.isSelected = true;
                        }
                    }
                    else
                    {

                    }

                }
            }
            catch (System.Exception ex)
            {

            }
        }


        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
            popupBetslipforin.Visibility = Visibility.Collapsed;
            popupBetslipforin.IsOpen = false;
        }
        private void Button_Click_26(object sender, RoutedEventArgs e)
        {
            popupBetslipforin1.Visibility = Visibility.Collapsed;
            popupBetslipforin1.IsOpen = false;
        }

        private void btnSimple1in_Click(object sender, RoutedEventArgs e)
        {
            var betlowerlimit = setBetslipamountlowerlimit();
            if (isFirstClickonslip == true)
            {
                isFirstClickonslip = false;

                nupdnUserAmountin.Value = 0;
            }

            if (LoggedinUserDetail.isInserting == true)
            {
                return;
            }
            string amount = (string)((Button)sender).Tag;
            if (amount.Contains("+"))
            {
                
                nupdnUserAmountin.Value = nupdnUserAmountin.Value + Convert.ToInt32(amount);
            }
            else
            {
               
                nupdnUserAmountin.Value = Convert.ToInt32(amount);
            }
            calculateProfitandLossonBetSlipin();
        }

        private void btnSimple1in1_Click(object sender, RoutedEventArgs e)
        {
            var betlowerlimit = setBetslipamountlowerlimit();
            if (isFirstClickonslip == true)
            {
                isFirstClickonslip = false;

                nupdnUserAmountin1.Value = 0;
            }

            if (LoggedinUserDetail.isInserting == true)
            {
                return;
            }
            string amount = (string)((Button)sender).Tag;
            if (amount.Contains("+"))
            {

                nupdnUserAmountin1.Value = nupdnUserAmountin1.Value + Convert.ToInt32(amount);
            }
            else
            {

                nupdnUserAmountin1.Value = Convert.ToInt32(amount);
            }
            calculateProfitandLossonBetSlipin1();
        }

        private void btnSubmitBetSlipin_Click_1(object sender, RoutedEventArgs e)
        {

        }        

        private void Button_Click_24(object sender, RoutedEventArgs e)
        {
            popupFigureSyncONOFF.IsOpen = false;
        }

        private void tv_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.isTVON == false)
            {
                LoggedinUserDetail.isTVON = true;
                objVideo = new VideoWindow();
                objVideo.eventID = MarketBook.EventID;
                objVideo.Top = 10;
                objVideo.Left = 10;
                objVideo.Show();
            }
            else
            {
                objVideo.Activate();
            }
            //stktv.Visibility = Visibility.Visible;
            //tv.Visibility = Visibility.Collapsed;
            //tvclose.Visibility = Visibility.Visible;
        }

        private void tvclose_Click(object sender, RoutedEventArgs e)
        {
            tvclose.Visibility = Visibility.Collapsed;
            tv.Visibility = Visibility.Visible;
            stktv.Visibility = Visibility.Collapsed;

        }
    
        private void CheckBox_Checked_6(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked_4(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            string MarketID = chk.Tag.ToString();
            try
            {
                if (chk.IsChecked == false)
                {
                    objUsersServiceCleint.UpdateBettingAllowed(MarketID, "0");
                    Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
                }
                else
                {
                   
                }
            }
            catch (System.Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_27(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;
            string MarketID = button.Tag.ToString();
            try
            {
               
                    objUsersServiceCleint.UpdateBettingAllowed(MarketID, "0");
                    Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
            
            }
            catch (System.Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message);
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Primitives.ToggleButton ss = new System.Windows.Controls.Primitives.ToggleButton();
            var a= ss.Tag.ToString();    
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //Removeallbackgroundworkers();
            //popupProffitLossAgent.IsOpen = false;
            //isProfitLossbyAgentShown = false;
        }

        private void CheckBox_Checked_7(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_15(object sender, RoutedEventArgs e)
        {

            popupFancyCuttingIN.IsOpen = true;
            ShowaverageSectionclickFancyIN();
           
        }

        public void ShowaverageSectionclickFancyIN()
        {

            List<bftradeline.Models.UserBetsForAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.location == "9" && item.isMatched == true && item.MarketBookID==MarketBook.EventID).ToList();

            if (lstCurrentBetsAdmin != null)
            {
                //  var results = LastloadedLinMarkets.Where(item => item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 0 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 0).ToList();
                List<ExternalAPI.TO.RunnerForIndianFancy> lstRunners = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                foreach (var item in lstCurrentBetsAdmin)
                {
                    ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner.SelectionId = item.SelectionID;
                    objRunner.RunnerName = item.SelectionName;
                    objRunner.MarketBookID = item.MarketBookID;
                    // txtInningsResultI.Text = item.MarketBookID;
                    lstRunners.Add(objRunner);
                }

                cmbRunnersFancyIN.IsSynchronizedWithCurrentItem = false;
                cmbRunnersFancyIN.ItemsSource = lstRunners;
                cmbRunnersFancyIN.DisplayMemberPath = "RunnerName";
                cmbRunnersFancyIN.SelectedValuePath = "SelectionId";              
                cmbRunnersFancyIN.SelectedIndex = 0;

                List<bftradeline.HelperClasses.CuttingUsers> lstCuttingUsers = JsonConvert.DeserializeObject<List<bftradeline.HelperClasses.CuttingUsers>>(objUsersServiceCleint.GetAllCuttingUsers(LoggedinUserDetail.PasswordForValidate));
                cmbCuttingUserFancyIN.IsSynchronizedWithCurrentItem = false;
                cmbCuttingUserFancyIN.ItemsSource = lstCuttingUsers;
                cmbCuttingUserFancyIN.DisplayMemberPath = "username";
                cmbCuttingUserFancyIN.SelectedValuePath = "ID";
                cmbCuttingUserFancyIN.SelectedIndex = 0;
                cmbBetTypeFancyIN.SelectedIndex = 0;

            }
        }

        private void cmbCuttingUserFancyIN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                btnAddFancyCuttingIN.Focus();
            }
            catch (System.Exception ex)
            {

            }
        }

        private void cmbRunnersFancyIN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtOddFancyIN.Focus();
            }
        }

        private void txtOddFancyIN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               txtsizeFancyIN.Focus();
            }
        }

        private void txtsizeFancyIN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txtAmountFancyIN.Focus();
            }

        }

        private void txtAmountFancyIN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmbBetTypeFancyIN.Focus();
            }
        }

        private void cmbBetTypeFancyIN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                cmbCuttingUserFancyIN.Focus();
            }
        }

        private void cmbBetTypeFancyIN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbCuttingUserFancyIN.Focus();
            }
            catch (System.Exception ex)
            {

            }
        }

        private void cmbCuttingUserFancyIN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnAddFancyCuttingIN.Focus();
            }
        }

        private void btnAddFancyCuttingIN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnAddFancyCuttingIN.IsEnabled = false;
                if (Convert.ToDecimal(txtAmountFancyIN.Text) > 1 && Convert.ToDecimal(txtAmountFancyIN.Text) > 1)
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        if (cmbRunnersFancyIN.Items.Count > 0)
                        {
                            string selectionID = cmbRunnersFancyIN.SelectedValue.ToString();
                            //string[] selectionIdandmarketbookID = selectionID.Split('|');
                           objUsersServiceCleint.InsertUserBetAdminAsync(selectionID, Convert.ToInt32(cmbCuttingUserFancyIN.SelectedValue), txtsizeFancyIN.Text, txtAmountFancyIN.Text, cmbBetTypeFancyIN.Text, txtsizeFancyIN.Text, true, "In-Complete", MarketBook.EventID, DateTime.Now, DateTime.Now, cmbRunnersFancyIN.Text, cmbRunnersFancyIN.Text, "0",txtOddFancyIN.Text, 0, "9", 0, LoggedinUserDetail.PasswordForValidate);
                        }

                    }
                    btnAddFancyCuttingIN.IsEnabled = true;
                    txtOddFancyIN.Focus();
                }
                else
                {
                    btnAddFancyCuttingIN.IsEnabled = true;
                    MessageBox.Show("Please enter correct values.");
                }

            }
            catch (System.Exception ex)
            {
                btnAddFancyCutting.IsEnabled = true;
                MessageBox.Show("Please enter correct values.");
            }
        }

        private void Button_Click_23(object sender, RoutedEventArgs e)
        {
            popupFancyCuttingIN.IsOpen = false;
        }
    }
    }

