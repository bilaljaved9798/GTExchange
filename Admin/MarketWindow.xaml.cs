using bftradeline;
using globaltraders.Service123Reference;
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


using globaltraders.UserServiceReference;
using System.Windows.Media.Animation;
using globaltraders.HelperClasses;
using bftradeline.Models;
using System.Text.RegularExpressions;

using System.Runtime.InteropServices;
using WinInterop = System.Windows.Interop;
using bftradeline.HelperClasses;
using System.Threading;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Collections.Specialized;
using globaltraders.CricketScoreServiceReference;
using System.IO;


namespace globaltraders
{
    /// <summary>
    /// Interaction logic for MarketWindow.xaml
    /// </summary>
    public partial class MarketWindow : Window, INotifyPropertyChanged
    {
        WindowResizer ob;
        private void Resize(object sender, MouseButtonEventArgs e)
        {
            isMaximizedWindow = false;
            ob.resizeWindow(sender);
           
        }

        private void DisplayResizeCursor(object sender, MouseEventArgs e)
        {
            ob.displayResizeCursor(sender);
        }

        private void ResetCursor(object sender, MouseEventArgs e)
        {
            ob.resetCursor();
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            ob.dragWindow();
        }
    

        public MarketWindow()
        {

            InitializeComponent();
            init();        
        }


        public void init()
        {
            try
            {
                //if (isMaximizedWindow == true)
                //{
                //    isMaximizedWindow = false;
                    
                //    //if (MarketBook.MainSportsname == "Cricket" && MarketBook.Runners.Count == 3 && DGVMarketFancy.Visibility == Visibility.Collapsed)
                //   if (MarketBook.MainSportsname == "Cricket" && MarketBook.Runners.Count == 3 && DGVMarketIndianFancy.Visibility == Visibility.Collapsed)
                //    {
                //        this.Height = 350;
                //    }
                //    else
                //    {

                //        this.Height = 550;
                //        this.Width = 685;


                //    }

                //    this.Width = 680;
                //}
                //else
                //{
                //    isMaximizedWindow = true;
                //    this.Width = System.Windows.SystemParameters.WorkArea.Width;
                //    this.Height = System.Windows.SystemParameters.WorkArea.Height;
                //    this.Left = 0;
                //    this.Top = 0;
                //}


                ob = new WindowResizer(this);

                this.lstCurrentBetsAdmin = new ObservableCollection<UserBetsForAdmin>();
                this.lstCurrentBets = new ObservableCollection<UserBets>();
                this.lstCurrentBetsAgent = new ObservableCollection<UserBetsforAgent>();
                this.lstCurrentBetsAdminUnMAtched = new ObservableCollection<UserBetsForAdmin>();
                this.lstCurrentBetsUnMatched = new ObservableCollection<UserBets>();
                this.lstCurrentBetsAgentUnMatched = new ObservableCollection<UserBetsforAgent>();
                DGVMarket.DataContext = lstMarketBookRunners;
                DGVMarketIndianFancy.DataContext = lstMarketBookRunnersFancyin;
                DGVMarketFancy.DataContext = lstMarketBookRunnersFancy;              
                DGVMarketToWintheToss.DataContext = lstMarketBookRunnersToWintheToss;


                backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                backgroundWorkerUpdateData = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                backgroundWorkerInsertBet = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                //backgroundWorkerInsertBet.DoWork += BackgroundWorkerInsertBet_DoWork;
                //backgroundWorkerInsertBet.RunWorkerCompleted += BackgroundWorkerInsertBet_RunWorkerCompleted;
               
                backgroundWorker.DoWork += backgroundWorker_DoWork;
                backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
                //
                backgroundWorkerUpdateData.DoWork += BackgroundWorkerUpdateData_DoWork;
                backgroundWorkerUpdateData.RunWorkerCompleted += BackgroundWorkerUpdateData_RunWorkerCompleted;
                //
                backgroundWorkerProfitandlossbyAgent = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                backgroundWorkerProfitandlossbyAgent.DoWork += BackgroundWorkerProfitandlossbyAgent_DoWork;
                backgroundWorkerProfitandlossbyAgent.RunWorkerCompleted += BackgroundWorkerProfitandlossbyAgent_RunWorkerCompleted;
                //
                backgroundWorkerUpdateAllotherData = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                backgroundWorkerUpdateAllotherData.DoWork += BackgroundWorkerUpdateAllotherData_DoWork;
                backgroundWorkerUpdateAllotherData.RunWorkerCompleted += BackgroundWorkerUpdateAllotherData_RunWorkerCompleted;
                //
                backgroundWorkerUpdateFigData = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                backgroundWorkerUpdateFigData.DoWork += BackgroundWorkerUpdateFigData_DoWork;
                backgroundWorkerUpdateFigData.RunWorkerCompleted += BackgroundWorkerUpdateFigData_RunWorkerCompleted;

                //
                backgroundWorkerLiabalityandScore = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                backgroundWorkerLiabalityandScore.DoWork += BackgroundWorkerLiabalityandScore_DoWork;
                backgroundWorkerLiabalityandScore.RunWorkerCompleted += BackgroundWorkerLiabalityandScore_RunWorkerCompleted;
                //
                timerCountdown.Tick += TimerCountdown_Tick;
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {

                    //DGVMatchedBetsAll.Visibility = Visibility.Collapsed;
                    //  DGVMatchedBetsAdminAll.Visibility = Visibility.Visible;
                    //  DGVMatchedBetsaGENTAll.Visibility = Visibility.Collapsed;

                    stkpnlAdminArea.Visibility = Visibility.Visible;

                }
                else
                {

                    // DGVMatchedBetsAll.Visibility = Visibility.Visible;
                    //  DGVMatchedBetsAdminAll.Visibility = Visibility.Collapsed;
                    //   DGVMatchedBetsaGENTAll.Visibility = Visibility.Collapsed;

                }
                // SetBetSlipKeys();              
            }
            catch (System.Exception ex)
            {
                return;
            }
        }

        private void BackgroundWorkerUpdateAllotherData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
               updatedatallnew();
            }
            catch (System.Exception ex)
            {

            }
            backgroundWorkerUpdateAllotherData.RunWorkerAsync();
        }

        private void BackgroundWorkerUpdateAllotherData_DoWork(object sender, DoWorkEventArgs e)
        {
            OddsData objOddsData = new OddsData();
            MarketBook currentmarketobject = objOddsData.GetAllMarketBooks(MarketBook.MarketId);
            if (currentmarketobject.MarketId != null)
            {
                MarketBook.Runners = currentmarketobject.Runners;
                MarketBook.MarketStatusstr = currentmarketobject.MarketStatusstr;
                MarketBook.OpenDate = currentmarketobject.OpenDate;
                MarketBook.FavoriteBack = currentmarketobject.FavoriteBack;
                MarketBook.FavoriteLay = currentmarketobject.FavoriteLay;
                MarketBook.FavoriteID = currentmarketobject.FavoriteID;
            }

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
            catch (System.Exception ex)
            {

            }
        }

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
            catch (System.Exception ex)
            {

            }
        }

        //public MatchScoreCard objMatchScoreCard = new MatchScoreCard();
        //public class LZWebClient : WebClient
        //{
        //    protected override WebRequest GetWebRequest(Uri address)
        //    {
        //        HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
        //        request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
        //        return request;
        //    }
        //}
          private void BackgroundWorkerUpdateData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {


                UpdateAllData();
                if (MarketBook.MainSportsname == "Cricket" && MarketBook.MarketBookName.Contains("Match Odds"))
                {
                    UpdateLineMarketsData(false);
                    
                }
                SetWindowHeight();
                //if (popupBetslip.IsOpen == true && runnerscount == "1")
                //{
                //    var currmarketbook = LastloadedLinMarkets.Where(item => item.MarketId == CurrentMarketBookId).FirstOrDefault();
                //    currmarketbookforbet = currmarketbook;
                //    lblBetSlipBack.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[0].Price.ToString();
                   
                //    lblBetslipBackSize0.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[0].Size.ToString();
                   

                //    lblBetSlipLay.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[0].Price.ToString();
                  
                //    lblBetslipLaySize0.Content = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[0].Size.ToString();
                   
                //    if (BetType == "back")
                //    {
                //        Betslipsize.Text = currmarketbook.Runners[0].ExchangePrices.AvailableToBack[0].Size.ToString();
                //    }
                //    else
                //    {
                //        Betslipsize.Text = currmarketbook.Runners[0].ExchangePrices.AvailableToLay[0].Size.ToString();
                //    }
                //}
                if (isProfitLossbyAgentShown == true)
                {
                    CalculateAvearageforSelectedAgent();
                }
                backgroundWorkerUpdateData.RunWorkerAsync();
            }
            catch (System.Exception ex)
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
                System.Threading.Thread.Sleep(200);
            }
            catch (System.Exception ex)
            {

            }

        }

       
        public double CurrentLiabality = 0;

        public void SetWindowHeight()
        {
            try
            {
               
                if (isMaximizedWindow == false && MarketBook.Runners.Count <= 3)
                {
                    double fancygridheight = 0;
                    double fancygridheightmain = 0;
                    if (DGVMarketFancy.Visibility == Visibility.Visible && lstMarketBookRunnersFancy != null)
                    {
                        if (lstMarketBookRunnersFancy.Count > 0)
                        {

                            fancygridheightmain = 50;//DGVMarketFancy.ActualHeight;
                        }
                        else
                        {
                            fancygridheightmain = 0;
                        }
                    }
                    if (DGVMarketIndianFancy.Visibility == Visibility.Visible && lstMarketBookRunnersFancyin != null)
                    {
                        if (lstMarketBookRunnersFancyin.Count > 0)
                        {
                            var isshownitems = lstMarketBookRunnersFancyin.Where(item => item.isShow == true).ToList();

                            fancygridheight = (lstMarketBookRunnersFancyin.Count * 50) + 10;

                            if (fancygridheight > 300)
                            {
                                // fancygridheight = 300;
                            }
                            else
                            {
                                // fancygridheight = DGVMarketFancy.ActualHeight;
                            }


                        }
                        else
                        {
                            fancygridheight = 0;
                        }

                    }
                    double kjygridheight = 0;
                    if (DGVMarketKalijut.Visibility == Visibility.Visible && lstMarketBookRunnerKalijut != null)
                    {
                        if (lstMarketBookRunnerKalijut.Count > 0)
                        {
                            var isshownitems = lstMarketBookRunnerKalijut.Where(item => item.isShow == true).ToList();
                            if (isshownitems.Count > 0)
                            {
                                kjygridheight = (isshownitems.Count * 50) + 45;

                            }
                            else
                            {
                                kjygridheight = 0;
                            }

                        }
                        else
                        {
                            kjygridheight = 0;
                        }

                    }
                    double FigSgridheight = 0;
                    if (DGVMarketSFig.Visibility == Visibility.Visible && lstMarketBookRunnersSFIg != null)
                    {
                        if (lstMarketBookRunnersSFIg.Count > 0)
                        {
                            var isshownitems = lstMarketBookRunnersSFIg.Where(item => item.isShow == true).ToList();
                            if (isshownitems.Count > 0)
                            {
                                FigSgridheight = (isshownitems.Count * 50) + 45;

                            }
                            else
                            {
                                FigSgridheight = 0;
                            }

                        }
                        else
                        {
                            FigSgridheight = 0;
                        }

                    }
                    double Figgridheight = 0;
                    if (DGVMarketFigure.Visibility == Visibility.Visible && lstMarketBookRunnersFigure != null)
                    {
                        if (lstMarketBookRunnersFigure.Count > 0)
                        {
                            Figgridheight = 600;
                        }
                        else
                        {
                            Figgridheight = 0;
                        }
                    }

                    double newheight = upperportion.ActualHeight + DGVMarket.ActualHeight + fancygridheight+ fancygridheightmain + 40 + kjygridheight + FigSgridheight + Figgridheight;
                    if (stkpnlTowintheToss.Visibility == Visibility.Visible)
                    {
                        newheight += DGVMarketToWintheToss.ActualHeight + 35;
                    }

                    if (newheight > 650 && sethieght == false)
                    {
                        if (this.Height != newheight)
                        {
                            this.Height = 600;
                        }

                    }
                    else
                    {
                        if (sethieght == true)
                        {
                            this.Height = 240;
                        }
                        else
                        {
                            this.Height = newheight;
                            //this.Height = 240;
                        }
                    }
                    
                }
                else
                {
                    this.Height = (lstMarketBookRunners.Count * 50) + 50;
                    
                }

            }
            catch (System.Exception ex)
            {
            }
        }


        public List<MatchScores> scores = new List<MatchScores>();
        public List<ExternalAPI.TO.MarketBook> CurrentMarketProfitandLoss = new List<ExternalAPI.TO.MarketBook>();
        public List<ExternalAPI.TO.MarketBook> CurrentMarketProfitandLossForAgent = new List<ExternalAPI.TO.MarketBook>();
        public List<ExternalAPI.TO.MarketBook> CurrentMarketProfitandLossToFigre = new List<ExternalAPI.TO.MarketBook>();
        public int SelectedAgentForProfitandLoss = 73;

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
                            var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (adminamunt * transadpercen);
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
        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
        public void UpdateUserLiablity()
        {
            try
            {
                       if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            List<UserBetsForAdmin> lstCurrentmarketbets = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.MarketBookID == MarketBook.MarketId).ToList();
                            CurrentLiabality = objUserBets.GetLiabalityofAdmin(lstCurrentmarketbets, CurrentMarketProfitandLoss[0]);
                        }
                   
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
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                  
                }

            }
            catch (System.Exception ex)
            {

            }
        }
        private void ObjUsersServiceCleint_GetUserBetsbyAgentIDwithZeroRefererCompleted(object sender, GetUserBetsbyAgentIDwithZeroRefererCompletedEventArgs e)
        {
            objUsersServiceCleint.GetUserBetsbyAgentIDwithZeroRefererCompleted -= ObjUsersServiceCleint_GetUserBetsbyAgentIDwithZeroRefererCompleted;
            ProfitandLoss objProfitandloss = new ProfitandLoss();
            var lstUserBetAgent = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(e.Result);
            CurrentMarketProfitandLossForAgent = objProfitandloss.CalculateProfitandLossAgent(MarketBookForProfitandlossAgent, lstUserBetAgent);
            if (isProfitLossbyAgentShown == true)
            {
                CalculateAvearageforSelectedAgent();
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
                            List<UserBetsForAdmin> listfigurebets = LoggedinUserDetail.CurrentAdminBets.Where(x => x.location == "8").ToList();
                            if (listfigurebets.Count > 0)
                            {
                                CurrentMarketProfitandLossToFigre = objProfitandloss.CalculateProfitandLossAdminFig(MarketBookFigure, listfigurebets);
                            }
                        }
                        catch (System.Exception ex)
                        {
                        }
                        }

                    CalculateAvearageforAllUsers();
                }
                UpdateUserLiablity();
               
            }
            catch (System.Exception ex)
            {

            }
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
                GetDataForFancy(false);
                GetDataForFancy(MarketBook.EventID, MarketBook.MarketId);

            }
            catch (System.Exception ex)
            {
               // scores = new List<MatchScores>();
            }
        }
        public void UpdateUserBetsData()
        {
            try
            {

             
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {

                            List<UserBetsForAdmin> lstMatchbets = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true).ToList();
                            lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
                            var result = lstMatchbets.Take(20).Where(p => !lstCurrentBetsAdmin.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));
                            var result1 = lstCurrentBetsAdmin.Where(p => !lstMatchbets.Take(20).Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                            if (result.Count() > 0 || result1.Count() > 0)
                            {

                                lstCurrentBetsAdmin.Clear();
                                foreach (var item in lstMatchbets.Take(20))
                                {
                            //AgentName
                            objUsersServiceCleint.GetSuperName(Convert.ToInt32(item.ID));
                            lstCurrentBetsAdmin.Add(new UserBetsForAdmin() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID });
                                }
                                LoggedinUserDetail.RefreshLiabality = true;
                            }
                          
                            return;

                        }
            }
            catch (System.Exception ex)
            {

            }
        }
        private void CheckBox_Checked_3(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            MarketBook.FigureMarkets.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().isOpenedbyUser = true;
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

        private void Button_Click_23(object sender, RoutedEventArgs e)
        {
            popupKalijuttSyncONOFF.IsOpen = false;
        }
        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            MarketBook.KJMarkets.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().isOpenedbyUser = true;
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
            MarketBook.KJMarkets.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().isOpenedbyUser = false;
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
    

        private void CheckBox_Unchecked_2(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            MarketBook.FigureMarkets.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().isOpenedbyUser = false;
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
        public void UpdateUserBetsDataAll()
        {
            try
            {

               
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            List<UserBetsForAdmin> lstMatchbets = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true).ToList();
                            lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
                            //DGVMatchedBetsAdminAll.ItemsSource = lstMatchbets;

                          //  lblAllMatchedBets.Content = "Matched Bets " + LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true).Count().ToString();

                        }
                   // }
                //}

            }
            catch (System.Exception ex)
            {

            }
        }
        public void UpdateUserBetsDataUnMatched()
        {
            try
            {

               
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                           
                            List<UserBetsForAdmin> lstUnMAtchBets = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == false).ToList();
                            lstUnMAtchBets = lstUnMAtchBets.OrderByDescending(item => item.ID).ToList();
                           
                            var result = lstUnMAtchBets.Where(p => !lstCurrentBetsAdminUnMAtched.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));
                            var result1 = lstCurrentBetsAdminUnMAtched.Where(p => !lstUnMAtchBets.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                            if (result.Count() > 0 || result1.Count() > 0)
                            {

                                lstCurrentBetsAdminUnMAtched.Clear();
                                foreach (var item in lstUnMAtchBets)
                                {
                                    lstCurrentBetsAdminUnMAtched.Add(new UserBetsForAdmin() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID });
                                }

                            }

                        }
                 

            }
            catch (System.Exception ex)
            {

            }
        }
      
        private void TimerCountdown_Tick(object sender, EventArgs e)
        {
           
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
        
     

        public DispatcherTimer timerCountdown = new DispatcherTimer();
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                backgroundWorker.RunWorkerAsync();
            }
            catch (System.Exception ex)
            {

            }


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

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
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

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
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
                                foreach (var backitems in runneritem.Ex.AvailableToBack)
                                {
                                    var pricesize = new PriceSize();

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
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

                                        pricesize.Size = Convert.ToInt64((backitems.Size));
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

                                        pricesize.Size = Convert.ToInt64(backitems.Size);
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

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
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
            catch (System.Exception ex)
            {
                return new MarketBook();
            }
        }
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

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
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

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
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

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
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

                                        pricesize.Size = Convert.ToInt64((backitems.Size));
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

                                        pricesize.Size = Convert.ToInt64(backitems.Size);
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

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
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
            catch (System.Exception ex)
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

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
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

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
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

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
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

                                        pricesize.Size = Convert.ToInt64((backitems.Size));
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

                                        pricesize.Size = Convert.ToInt64(backitems.Size);
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

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
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
            catch (System.Exception ex)
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

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
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

                                            pricesize.Size = Convert.ToInt64(backitems.Size);
                                            pricesize.SizeStr = FormatNumber(pricesize.Size);
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

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
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

                                        pricesize.Size = Convert.ToInt64((backitems.Size));
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

                                        pricesize.Size = Convert.ToInt64(backitems.Size);
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

                                    pricesize.Size = Convert.ToInt64(backitems.Size);
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
            catch (System.Exception ex)
            {
                return new MarketBook();
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
                    if (objFancyMarketBook.Runners.Where(item => item.isShow == false).Count() > 0)
                    {
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
        public void GetRunnersDataSourceToWintheToss(List<ExternalAPI.TO.Runner> runners, MarketBook objMarketBook)
        {
            try
            {
                if (objMarketBook.MarketId == null)
                {
                    return;
                }

                    if (lstMarketBookRunnersToWintheToss == null || lstMarketBookRunnersToWintheToss.Count == 0)
                {

                    lstMarketBookRunnersToWintheToss = new ObservableCollection<MarketBookShow>();
                    lstMarketBookRunnersToWintheToss.Clear();

                    foreach (var item in runners)
                    {
                        
                        MarketBookShow objmarketbookshow = new MarketBookShow();
                        objmarketbookshow.Selection = item.RunnerName.ToString().ToUpper();
                        objmarketbookshow.SelectionID = item.SelectionId;
                        objmarketbookshow.Price = item.LastPriceTraded.ToString();
                        objmarketbookshow.PL = item.ProfitandLoss.ToString();
                        objmarketbookshow.RunnerStatusstr = item.StatusStr;
                      

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
                        lstMarketBookRunnersToWintheToss.Add(objmarketbookshow);
                

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
                      
                    }
                }

            }
            catch (System.Exception ex)
            {

            }

         
        }
        public List<ExternalAPI.TO.MarketBook> CurrentMarketProfitandLossToWinTheToss = new List<ExternalAPI.TO.MarketBook>();
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
                        GetRunnersDataSourceToWintheToss(MarketBookWintheToss.Runners, MarketBookWintheToss);
                        DGVMarketToWintheToss.ItemsSource = lstMarketBookRunnersToWintheToss;
                     
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

        ExternalAPI.TO.Home root = new ExternalAPI.TO.Home();
        //public void GetUpdate(string EventID)
        //{
        //    try
        //    {
        //        try
        //        {
        //            root = objBettingClient.GetUpdate(MarketBook.EventID);
        //            if (root.i2.tr == 0)
        //            {
        //                lblteamA.Text = root.t1.n;
        //                lblTeamAScore1.Text = root.i1.sc.ToString();
        //                lblw.Text = root.i1.wk.ToString();
        //                lblovs.Text = root.i1.ov.ToString();
        //                lblRscore.Text = root.i2.tr.ToString();
        //                string over = root.i1.ov.ToString();
        //                string[] over1 = over.Split('.');
        //                int of = Convert.ToInt32(over1[0]) * 6;
        //                int os = 0;
        //                if (over1.Length > 1)
        //                {
        //                    os = Convert.ToInt32(over1[1]);
        //                }
        //                decimal tball = of + os;
        //                string CR = ((Convert.ToDecimal(root.i1.sc) / tball) * 6).ToString("F2");
        //                lblCRR.Text = CR;
        //                lblStricker1.Text = root.cs.msg;
        //            }
        //            else
        //            {
        //                lblteamA.Text = root.t2.n;
        //                lblTeamAScore1.Text = root.i2.sc.ToString();
        //                lblw.Text = root.i2.wk.ToString();
        //                lblovs.Text = root.i2.ov.ToString();
        //                lblRscore.Text = root.i2.tr.ToString();
        //                string over = root.i2.ov.ToString();
        //                string[] over1 = over.Split('.');
        //                int of = Convert.ToInt32(over1[0]) * 6;
        //                int os = 0;
        //                if (over1.Length > 1)
        //                {
        //                    os = Convert.ToInt32(over1[1]);
        //                }
        //                decimal tball = of + os;

        //                string CR = ((Convert.ToDecimal(root.i2.sc) / tball) * 6).ToString("F2");


        //                lblCRR.Text = CR;
        //                decimal overs = Convert.ToDecimal(root.iov) * 6;//- root.i2.ov;
        //                decimal ball = overs - tball;
        //                //int ball =Convert.ToInt32(overs * 6);                     
        //                int Rscore = root.i2.tr - root.i2.sc;
        //                string msg = "need " + Rscore + " from " + ball + "";
        //                lblRRR.Text = (Rscore / (ball / 6)).ToString("F2");
        //                lblLastBallComment1.Text = msg;
        //            }
        //            lblballs.Text = root.pb.Substring(root.pb.Length - 11);
        //            string status;
        //            if (root.cs.msg == "WD")
        //            {
        //                status = "WIDE";
        //            }
        //            else
        //            {
        //                if (root.cs.msg == "B")
        //                {
        //                    status = "Ball";
        //                }
        //                else
        //                {
        //                    if (root.cs.msg == "OC")
        //                    {
        //                        status = "Over";
        //                    }
        //                    else
        //                    {
        //                        if (root.cs.msg == "BS")
        //                        {
        //                            status = "Bowler Stop";

        //                        }
        //                        else
        //                        {
        //                            status = root.cs.msg;
        //                        }
        //                    }

        //                }
        //            }
        //            lblStricker1.Text = status;

        //        }

        //        catch (System.Exception EX)
        //        {
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {

        //    }
        //}


        OddsData objOddsData = new OddsData();
        string matchCricketAPIKey = "";
        Service1Client objCricketScoreClient = new Service1Client();
     
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
                 
                    System.Threading.Thread.Sleep(3000);
               

                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (System.Exception ex)
            {

            }
        

        }
        public List<MarketBook> LastloadedLinMarkets = new List<ExternalAPI.TO.MarketBook>();
        public List<MarketBook> LastloadedLinMarketsFig = new List<ExternalAPI.TO.MarketBook>();
        
        public BackgroundWorker backgroundWorker;
        public BackgroundWorker backgroundWorkerUpdateData;
        public BackgroundWorker backgroundWorkerUpdateFigData;
        public BackgroundWorker backgroundWorkerInsertBet;
        public BackgroundWorker backgroundWorkerLiabalityandScore;
        public BackgroundWorker backgroundWorkerProfitandlossbyAgent;
        public BackgroundWorker backgroundWorkerUpdateAllotherData;
        private ObservableCollection<MarketBookShow> _lstMarketBookRunners;
        private ObservableCollection<UserBets> lstCurrentBets = new ObservableCollection<UserBets>();
        private ObservableCollection<UserBetsforAgent> lstCurrentBetsAgent = new ObservableCollection<UserBetsforAgent>();
        private ObservableCollection<UserBetsForAdmin> lstCurrentBetsAdmin;
        private ObservableCollection<UserBets> lstCurrentBetsUnMatched = new ObservableCollection<UserBets>();
        private ObservableCollection<UserBetsforAgent> lstCurrentBetsAgentUnMatched = new ObservableCollection<UserBetsforAgent>();
        private ObservableCollection<UserBetsForAdmin> lstCurrentBetsAdminUnMAtched = new ObservableCollection<UserBetsForAdmin>();
        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersFancy;
        private ObservableCollection<MarketBookShow> _lstMarketBookRunnerKalijut;

        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersToWintheToss;
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
        public ExternalAPI.TO.MarketBook MarketBookForProfitandlossToFigure = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookForProfitandloss = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookForProfitandlossAgent = new ExternalAPI.TO.MarketBook();
        public ExternalAPI.TO.MarketBook MarketBookForProfitandlossToWinTheToss = new ExternalAPI.TO.MarketBook();





        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            try
            {
                if (isMaximizedWindow == true)
                {
                    isMaximizedWindow = false;
                    //  && DGVMarketFancy.Visibility == Visibility.Collapsed
                    if (MarketBook.MainSportsname == "Cricket" && MarketBook.Runners.Count == 3)
                    {
                        this.Height = 400;
                    }
                    else
                    {
                        this.Height = 400;
                    }

                    this.Width = 676;


                }


                time = false;
                //    wsFancy.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Fancy").FirstOrDefault().URLForData;
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {

                    AssignUserstoCombobox();
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
                else
                {
                    txtToBePlaced.Visibility = Visibility.Collapsed;
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
                    var newrunner = runneritem;
                    MarketBookForProfitandloss.Runners.Add(newrunner);
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
                lblMarketName.Content = MarketBook.MarketBookName;
                if (MarketBook.MarketStatusstr.ToUpper() == "ACTIVE")
                {
                    lblMarketStatus.Content = "GOING LIVE";
                }
                else
                {
                    lblMarketStatus.Content = MarketBook.MarketStatusstr;
                }
                // lblMarketStatus.Content = MarketBook.MarketStatusstr;
                lblMarketTime.Content = MarketBook.OpenDate;
                GetRunnersDataSource(MarketBook.Runners, MarketBook);
                DGVMarket.ItemsSource = lstMarketBookRunners;
                //lstCurrentAdminBets = new ObservableCollection<UserBetsForAdmin>();
                //lstCurrentAdminBets = LoggedinUserDetail.CurrentAdminBetsNew;
                //  lstCurrentBetsAdmin = new ObservableCollection<UserBetsForAdmin>();

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

               
                string[] marketbooknameandtype = MarketBook.MarketBookName.Split('/', 'v');
                if (MarketBook.MainSportsname == "Horse Racing")
                {
                    marketimg.Source = new BitmapImage(new Uri("ws7.png", UriKind.Relative));
                    marketType.Content = "Horse Race".ToUpper();
                    lblMarketName.Content = MarketBook.MarketBookName;
                }
                else
                {

                    if (MarketBook.MainSportsname == "Greyhound Racing")
                    {
                        marketimg.Source = new BitmapImage(new Uri("ws4339.png", UriKind.Relative));
                        marketType.Content = "Greyhound Race".ToUpper();
                        lblMarketName.Content = MarketBook.MarketBookName;
                    }
                    else
                    {
                        if (MarketBook.MainSportsname == "Cricket")
                        {
                            marketimg.Source = new BitmapImage(new Uri("ws4.png", UriKind.Relative));

                            if (MarketBook.Runners.Count > 1)
                            {
                                marketType.Content = marketbooknameandtype[0].ToUpper();

                                // lblMarketName.Content = MarketBookForProfitandloss.Runners[0].RunnerName.ToString() + "         V         " + MarketBookForProfitandloss.Runners[1].RunnerName.ToString();
                                //lblMarketName.Content = marketbooknameandtype[1];
                                lblMarketName.Content = marketbooknameandtype[1].ToUpper() + "   V   " + marketbooknameandtype[2].ToUpper();
                            }
                            else
                            {
                                marketType.Content = "Line Market".ToUpper();
                                lblMarketName.Content = MarketBook.MarketBookName;
                            }

                        }
                        else
                        {
                            if (MarketBook.MainSportsname == "Soccer")
                            {
                                marketimg.Source = new BitmapImage(new Uri("ws1.png", UriKind.Relative));
                                marketType.Content = "Match Odds".ToUpper();
                                lblMarketName.Content = marketbooknameandtype[1].ToUpper() + "   V   " + marketbooknameandtype[2].ToUpper();
                                //lblMarketName.Content = MarketBookForProfitandloss.Runners[0].RunnerName.ToString() + "         V         " + MarketBookForProfitandloss.Runners[1].RunnerName.ToString();
                                GetToSoccerGoalMarket();
                            }
                            else
                            {
                                if (MarketBook.MainSportsname == "Tennis")
                                {
                                    marketimg.Source = new BitmapImage(new Uri("ws2.png", UriKind.Relative));
                                    marketType.Content = "Match Odds".ToUpper();
                                    lblMarketName.Content = marketbooknameandtype[1].ToUpper() + "   V   " + marketbooknameandtype[2].ToUpper();
                                    //lblMarketName.Content = MarketBookForProfitandloss.Runners[0].RunnerName.ToString() + "         V         " + MarketBookForProfitandloss.Runners[1].RunnerName.ToString();
                                }
                            }
                        }
                    }
                }


                if (MarketBook.MainSportsname == "Cricket" && MarketBook.MarketBookName.Contains("Match Odds"))
                {
                    //   matchCricketAPIKey = objUsersServiceCleint.GetCricketMatchKey(MarketBook.MarketId);
                    GetFancyMarkets();
                    DGVMarketFancy.ItemsSource = lstMarketBookRunnersFancy;
                }
                // GetFancyMarkets();
               // MarketRulesAll = LoggedinUserDetail.MarketRulesAll;
                tmrUpdateMarket.Tick += TmrUpdateMarket_Tick;
                tmrUpdateMarket.Interval = TimeSpan.FromMilliseconds(200);



                if (MarketBook.MainSportsname.Contains("Cricket") && MarketBook.MarketBookName.Contains("Match Odds"))
                {
                    backgroundWorkerLiabalityandScore.RunWorkerAsync();
                    GetTowWinTheTossMarket();
                    if (LoggedinUserDetail.GetUserTypeID() != 1)
                    {
                        backgroundWorker.RunWorkerAsync();
                    }

                    //if (MarketBook.LineVMarkets != null)
                    //{
                        tmrUpdateLiabalities.Tick += TmrUpdateLiabalities_Tick;
                        tmrUpdateLiabalities.Interval = TimeSpan.FromMilliseconds(2000);
                        backgroundWorkerUpdateFigData.RunWorkerAsync();
                        tmrUpdateLiabalities.Start();

                    //}
                    //tmrUpdateMarketKJ.Tick += new EventHandler(timer1_Tick);
                    //tmrUpdateMarketKJ.Interval = TimeSpan.FromMilliseconds(10000);  // in miliseconds
                    //tmrUpdateMarketKJ.Start();

                }

                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    backgroundWorkerProfitandlossbyAgent.RunWorkerAsync();
                }
                tmrUpdateMarket.Start();
                //  backgroundWorkerUpdateData.RunWorkerAsync();
                // backgroundWorkerUpdateAllotherData.RunWorkerAsync();

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

        }

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
                                //Getfirst(Soccergoalmarket[1].MarketCatalogueID); 
                                objUsersServiceCleint.SetMarketBookOpenbyUSerandGetAsync(UserIDforLinevmarkets, Soccergoalmarket[0].MarketCatalogueID);
                                objUsersServiceCleint.SetMarketBookOpenbyUSerandGetCompleted += ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGetCompleted;

                            }
                            DGVMarketToWintheToss.ItemsSource = lstMarketBookRunnersToWintheToss;
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

        private void TmrUpdateLiabalities_Tick(object sender, EventArgs e)
        {
            //UpdateLiabaliteies();
            UpdateLineMarketsDataIN(false);
        }

        CancellationToken cancellationToken;
      
        public string scoretxt = "";
     
        public void updatelabelstimerandstatus()
        {
           
            if (MarketBook.MarketStatusstr == "ACTIVE")
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
                stkpnlTowintheToss.Visibility = Visibility.Collapsed;
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

            
            if (lblMarketStatus.Content.ToString() == "IN-PLAY")
            {
                
            }
            else
            {

                if (lblMarketStatus.Content.ToString() == "SUSPENDED")
                {
                   
                }
                else
                {
                  
                }
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
            catch (System.Exception ex)
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
                  
                    CalculateAvearageforAllUsersTowinTheToss();
                  
                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                            {
                                List<UserBetsForAdmin> lstCurrentmarketbets = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.MarketBookID == MarketBookForProfitandlossToWinTheToss.MarketId).ToList();
                                CurrentLiabality += objUserBets.GetLiabalityofAdmin(lstCurrentmarketbets, CurrentMarketProfitandLossToWinTheToss[0]);
                            }                     
                }
             
            }
            catch (System.Exception ex)
            {

            }
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

        public ExternalAPI.TO.MarketBookForindianFancy GetBookPositionin(string marketBookID, string selectionID)
        {

            ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
            ExternalAPI.TO.MarketBookForindianFancy objmarketbookin = new ExternalAPI.TO.MarketBookForindianFancy();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                int a, b;
                List<bftradeline.Models.UserBetsForAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.SelectionID == selectionID && item.MarketBookID == marketBookID && item.isMatched == true).ToList();
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
                        agentrate = agentrate + superpercent;
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
           
            return objmarketbookin;
        }
        MarketBookShow runner1 = new MarketBookShow();
        ExternalAPI.TO.MarketBookForindianFancy currentmarketsfancyPL = new ExternalAPI.TO.MarketBookForindianFancy();
        ObservableCollection<ExternalAPI.TO.MarketBookForindianFancy> allmarkets = new ObservableCollection<ExternalAPI.TO.MarketBookForindianFancy>();
        public void GetINFancy(string EventID, string MarketBookID)
        {
            try
            {
                lstMarketBookRunnersFancyin = new ObservableCollection<MarketBookShow>();
                lstMarketBookRunnersFancyin.Clear();
                allmarkets.Clear();
                try
                {
                    allmarkets.Add(objBettingClient.GetMarketDatabyIDIndianFancy(EventID, MarketBookID));
                }

                catch (System.Exception EX)
                {

                }
                foreach (var usermarket in allmarkets.Take(4))
                {
                    if (usermarket.RunnersForindianFancy != null)
                    {

                        foreach (var runners in usermarket.RunnersForindianFancy)
                        {
                            var a = MarketBook.LineVMarkets.Where(item => item.SelectionID == runners.SelectionId).ToList();
                            if (a.Count > 0)
                            {
                                var runner = new MarketBookShow();
                                runner.CategoryName = "Cricket";
                                runner.MarketbooknameBet = runners.RunnerName;
                                runner.RunnerStatusstr = runners.MarketStatusStr;
                                runner.Marketstatusstr = lblMarketStatus.Content.ToString();
                                runner.BettingAllowed = true;
                                runner.Selection = runners.RunnerName;
                                runner.SelectionID = runners.SelectionId;

                                if (runner.RunnerStatusstr == "SUSPENDED")
                                {
                                    runner.StatusStr = "Collapsed";
                                    runner.JockeyName = "SUSPE";
                                    runner.JockeyHeading = "NDED";
                                    runner.StallDraw = "Visible";
                                    runner.Price = "0,0,0,0";
                                }
                                else
                                {
                                    if (runner.RunnerStatusstr == "Ball Running")
                                    {
                                        runner.StatusStr = "Collapsed";
                                        runner.JockeyName = "BALLRU";
                                        runner.JockeyHeading = "NNING";
                                        runner.StallDraw = "Visible";
                                        runner.Price = "0,0,0,0";
                                    }
                                    else
                                    {
                                        runner.StallDraw = "Collapsed";
                                        runner.Laysize0 = runners.LaySize.ToString();
                                        runner.Layprice0 = runners.Layprice.ToString();
                                        runner.Backsize0 = runners.BackSize.ToString();
                                        runner.Backprice0 = runners.Backprice.ToString();
                                        runner.StatusStr = "Visible";
                                        runner.Price = "1,0,0,0";
                                        runner.isShow = true;
                                    }
                                }

                                currentmarketsfancyPL = GetBookPositionin(a[0].MarketCatalogueID, runner.SelectionID);
                                double TotalProfit = 0;
                                double TotalLoss = 0;
                                if (currentmarketsfancyPL.RunnersForindianFancy != null)
                                {
                                    TotalProfit = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Max(t => t.ProfitandLoss));
                                    TotalLoss = Convert.ToDouble(currentmarketsfancyPL.RunnersForindianFancy.Min(t => t.ProfitandLoss));
                                    runner.PL = TotalProfit.ToString();
                                    runner.Loss = TotalLoss.ToString();
                                }
                                else
                                {
                                    runner.PL = TotalLoss.ToString();
                                    runner.Loss = TotalProfit.ToString();
                                }

                                runner.CurrentMarketBookId = a[0].MarketCatalogueID;
                                runner.runnerscount = usermarket.RunnersForindianFancy.Count.ToString();
                                runner.OpenDate = MarketBook.OpenDate;
                                lstMarketBookRunnersFancyin.Add(runner);
                                //  GetRunnersDataSourceFancyin(usermarket.RunnersForindianFancy);
                            }

                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                //  DGVMarketIndianFancy.Visibility = Visibility.Collapsed;
                // AssignKalijutdata();
                // return ExternalAPI.TO.MarketBookForindianFancy;
            }
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
                        agentrate = agentrate + superpercent;
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
            
            return objmarketbookin;
        }

      
        ObservableCollection<MarketBookShow> lstMarketBookRunnersKJ = new ObservableCollection<MarketBookShow>();
        bool data = true;
        public void AssignKalijutdata()
        {
            try
            {

                // var results = objUsersServiceCleint.GetEventDetailsbyMarketBook(MarketBook.MarketId);
                int UserIDforLinevmarkets = 0;
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    UserIDforLinevmarkets = 73;
                }
                else
                {
                    UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
                }
                var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(MarketBook.EventID, MarketBook.OrignalOpenDate.Value, UserIDforLinevmarkets));

                lstMarketBookRunnerKalijut = new ObservableCollection<MarketBookShow>();
                lstMarketBookRunnerKalijut.Clear();
                var KJMarkets = MarketBook.LineVMarkets.Where(item => item.isOpenedbyUser == true && item.EventName == "Kali v Jut").ToList();

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

                var KJMarkets = MarketBook.LineVMarkets.Where(item => item.isOpenedbyUser == true && item.EventName == "SmallFig").ToList();

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


        bool time = true;
        List<MarketBook> LastloadedLinMarkets1 = new List<MarketBook>();

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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                        //
                        if (objmarketbookshow.StallDraw != "Not" && objmarketbookshow.StallDraw != "" && objmarketbookshow.StallDraw != null)
                        {
                            objmarketbookshow.StallDraw = "(" + objmarketbookshow.StallDraw + ")";
                        }


                        if (item.ExchangePrices.AvailableToBack.Count == 1)
                        {
                            //== "0" ? "" : item.ExchangePrices.AvailableToBack[0].Price.ToString()
                            objmarketbookshow.Backprice0 = item.ExchangePrices.AvailableToBack[0].Price.ToString();
                            objmarketbookshow.Backsize0 = item.ExchangePrices.AvailableToBack[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToBack[0].Size.ToString();
                        }

                        if (item.ExchangePrices.AvailableToLay.Count == 1)
                        {
                            //== "0" ? "" : item.ExchangePrices.AvailableToLay[0].Price.ToString()
                            objmarketbookshow.Layprice0 = item.ExchangePrices.AvailableToLay[0].Price.ToString();
                            objmarketbookshow.Laysize0 = item.ExchangePrices.AvailableToLay[0].Size.ToString() == "0" ? "" : item.ExchangePrices.AvailableToLay[0].Size.ToString();
                        }

                        lstMarketBookRunnersFigure.Add(objmarketbookshow);
                        // lstMArketbookshow.Add(objmarketbookshow);

                    }
                    if (MarketBook.Runners.Count > 1)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            lstMarketBookRunnersFigure[i].isSelectedForLK = true;
                        }
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
                }
            catch (System.Exception ex)
            {

            }

        }
        ExternalAPI.TO.Root root2 = new ExternalAPI.TO.Root();
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
                backgroundWorkerUpdateFigData.Dispose();
               

               // backgroundWorkerUpdateFigData.RunWorkerAsync();
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

            }


        }



        bool bgdata = true;
        bool getfancy = true;

       


        public void UpdateAllData()
        {
            try
            {
                        
                UpdateLiabaliteies();
                GetRunnersDataSource(MarketBook.Runners.ToList(), MarketBook);
                if (MarketBookWintheToss.Runners != null)
                {
                    UpdateLiabaliteiesToWintheToss();
                    GetRunnersDataSourceToWintheToss(MarketBookWintheToss.Runners, MarketBookWintheToss);
                }
                if (MarketBook.MainSportsname == "Soccer")
                {
                    GetRunnersDataSourceToWintheToss(MarketBookWintheToss.Runners, MarketBookWintheToss);
                }
                if (MarketBook.MainSportsname == "Cricket")
                {                  
                    //GetUpdate(MarketBook.EventID);                                       
                }
                updatelabelstimerandstatus();      
            }
            catch (System.Exception ex)
            {

            }
        }
        public void updatedatallnew()
        {
            try
            {
                UpdateAllData();
                SetWindowHeight();
               
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
                SetWindowHeight();
                if (MarketBook.MainSportsname == "Cricket" && MarketBook.MarketBookName.Contains("Match Odds"))
                {                    
                   CreateScoreCard(MarketBook.EventID);

                }
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

                   
                    foreach (var item in runners)
                    {
                        MarketBookShow objmarketbookshow = new MarketBookShow();
                        objmarketbookshow.Selection = item.RunnerName.ToString().ToUpper();
                        objmarketbookshow.SelectionID = item.SelectionId;
                        objmarketbookshow.Price = item.LastPriceTraded.ToString();
                        objmarketbookshow.PL = item.ProfitandLoss.ToString();
                        objmarketbookshow.RunnerStatusstr = item.StatusStr;
                        objmarketbookshow.JockeyName = item.JockeyName;
                        objmarketbookshow.ImageURL = item.WearingURL.ToString().Replace(".betfair.com", ".cdnbf.net");
                        objmarketbookshow.Clothnumber = item.Clothnumber;
                        objmarketbookshow.StallDraw = item.StallDraw;
                        objmarketbookshow.JockeyHeading = "";
                       
                        if (MarketBook.MainSportsname.Contains("Horse Racing"))
                        {
                            DGVMarket.Columns[4].Header = "JOCKEY";
                            objmarketbookshow.JockeyHeading = "JOCKEY";
                        }


                        objmarketbookshow.CategoryName = objMarketBook.MainSportsname;
                        objmarketbookshow.MarketbooknameBet = objMarketBook.MarketBookName;
                        objmarketbookshow.Marketstatusstr = objMarketBook.MarketStatusstr;
                        objmarketbookshow.BettingAllowed = objMarketBook.BettingAllowed;
                        objmarketbookshow.OpenDate = objMarketBook.OpenDate;
                        objmarketbookshow.runnerscount = objMarketBook.Runners.Count.ToString();
                        objmarketbookshow.CurrentMarketBookId = objMarketBook.MarketId;
                        
                        
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


                            objmarketbookshow.PL = CurrentMarketProfitandLoss[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().ProfitandLoss.ToString();
                            objmarketbookshow.Loss = CurrentMarketProfitandLoss[0].Runners.Where(item1 => item1.SelectionId == item.SelectionId).FirstOrDefault().Loss.ToString();
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
                      
                    }

                }

            }
            catch (System.Exception ex)
            {

            }
;
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
        public void GetRunnersDataSourceFancy(ExternalAPI.TO.GetDataFancy runners)
        {
            try
            {
                lstMarketBookRunnersFancyin = new ObservableCollection<MarketBookShow>();
                lstMarketBookRunnersFancyin.Clear();
                runners.session= runners.session.Take(40).OrderBy(s => s.SelectionId).ToList();
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

                    currentmarketsfancyPL = GetBookPositioninNew(item.SelectionId);

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
               

            }
            catch (System.Exception ex)
            {
                
            }

            //  return lstMArketbookshow;
        }
        public void GetRunnersDataSourceFancy(List<ExternalAPI.TO.Runner> runners)
        {
            if (lstMarketBookRunnersFancy == null || lstMarketBookRunnersFancy.Count == 0)
            {


            }
            else
            {
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
        private void TmrUpdateMarket_Elapsed(object sender, ElapsedEventArgs e)
        {

        }
        public double lastwindowsize = 0;
        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
            lastwindowsize = this.Width;
            this.WindowState = WindowState.Minimized;

        }
        public bool isMaximizedWindow = true;
        private void TextBlock_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

            if (isMaximizedWindow == true)
            {
                isMaximizedWindow = false;
                
                //if (MarketBook.MainSportsname == "Cricket" && MarketBook.Runners.Count == 3 && DGVMarketFancy.Visibility == Visibility.Collapsed)
              if (MarketBook.MainSportsname == "Cricket" && MarketBook.Runners.Count == 3 && DGVMarketIndianFancy.Visibility == Visibility.Collapsed)
                {
                    this.Height = 400;
                }
                else
                {
                    this.Height = 400;
                }

                this.Width = 676;


            }
            else
            {
                isMaximizedWindow = true;
                this.Width = System.Windows.SystemParameters.WorkArea.Width;
                this.Height = System.Windows.SystemParameters.WorkArea.Height;
                this.Left = 0;
                this.Top = 0;

            }
            SetWindowHeight();

        }

        private void TextBlock_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {

            this.Close();
           
        }
        public void Resizewindow()
        {

            DGVMarket.Columns[4].Visibility = Visibility.Collapsed;
            if ((this.Width < 683) || (this.Width < 684))
            {
                DGVMarket.Columns[7].Visibility = Visibility.Collapsed;
                DGVMarket.Columns[8].Visibility = Visibility.Visible;
                DGVMarket.Columns[11].Visibility = Visibility.Visible;
                DGVMarket.Columns[12].Visibility = Visibility.Collapsed;
                DGVMarket.Columns[0].Visibility = Visibility.Collapsed;
                DGVMarket.Columns[4].Visibility = Visibility.Collapsed;
                DGVMarketToWintheToss.Columns[7].Visibility = Visibility.Collapsed;
                DGVMarketToWintheToss.Columns[8].Visibility = Visibility.Visible;
                DGVMarketToWintheToss.Columns[11].Visibility = Visibility.Visible;
                DGVMarketToWintheToss.Columns[12].Visibility = Visibility.Collapsed;
                DGVMarketToWintheToss.Columns[0].Visibility = Visibility.Collapsed;
                DGVMarketToWintheToss.Columns[4].Visibility = Visibility.Collapsed;

                DGVMarketFancy.Columns[7].Visibility = Visibility.Collapsed;
                DGVMarketFancy.Columns[8].Visibility = Visibility.Visible;
                DGVMarketFancy.Columns[11].Visibility = Visibility.Visible;
                DGVMarketFancy.Columns[12].Visibility = Visibility.Collapsed;
            }
            else
            {
                if (this.Width < 684)
                {
                    DGVMarket.Columns[7].Visibility = Visibility.Collapsed;
                    DGVMarket.Columns[8].Visibility = Visibility.Collapsed;
                    DGVMarket.Columns[11].Visibility = Visibility.Collapsed;
                    DGVMarket.Columns[12].Visibility = Visibility.Collapsed;
                    DGVMarket.Columns[0].Visibility = Visibility.Collapsed;
                    DGVMarket.Columns[4].Visibility = Visibility.Collapsed;
                    DGVMarketToWintheToss.Columns[7].Visibility = Visibility.Collapsed;
                    DGVMarketToWintheToss.Columns[8].Visibility = Visibility.Collapsed;
                    DGVMarketToWintheToss.Columns[11].Visibility = Visibility.Collapsed;
                    DGVMarketToWintheToss.Columns[12].Visibility = Visibility.Collapsed;
                    DGVMarketToWintheToss.Columns[0].Visibility = Visibility.Collapsed;
                    DGVMarketToWintheToss.Columns[4].Visibility = Visibility.Collapsed;

                    DGVMarketFancy.Columns[7].Visibility = Visibility.Collapsed;
                    DGVMarketFancy.Columns[8].Visibility = Visibility.Collapsed;
                    DGVMarketFancy.Columns[11].Visibility = Visibility.Collapsed;
                    DGVMarketFancy.Columns[12].Visibility = Visibility.Collapsed;
                }
                else
                {
                    DGVMarket.Columns[7].Visibility = Visibility.Visible;
                    DGVMarket.Columns[8].Visibility = Visibility.Visible;
                    DGVMarket.Columns[11].Visibility = Visibility.Visible;
                    DGVMarket.Columns[12].Visibility = Visibility.Visible;
                    DGVMarket.Columns[0].Visibility = Visibility.Visible;

                    if (this.Width > 880)
                    {
                        if (MarketBook.MainSportsname.Contains("Racing"))
                        {
                            DGVMarket.Columns[4].Visibility = Visibility.Visible;
                        }
                    }
                    DGVMarketToWintheToss.Columns[7].Visibility = Visibility.Visible;
                    DGVMarketToWintheToss.Columns[8].Visibility = Visibility.Visible;
                    DGVMarketToWintheToss.Columns[11].Visibility = Visibility.Visible;
                    DGVMarketToWintheToss.Columns[12].Visibility = Visibility.Visible;
                    DGVMarketToWintheToss.Columns[0].Visibility = Visibility.Visible;

                    DGVMarketFancy.Columns[7].Visibility = Visibility.Visible;
                    DGVMarketFancy.Columns[8].Visibility = Visibility.Visible;
                    DGVMarketFancy.Columns[11].Visibility = Visibility.Visible;
                    DGVMarketFancy.Columns[12].Visibility = Visibility.Visible;
                }

            }
            //stkpnlBets.Visibility = Visibility.Collapsed;
            //stkpnlMenu.Visibility = Visibility.Collapsed;
            stkpnlMarketGrid.Width = this.Width - 12;
            DGVMarket.Width = this.Width - 12;
            DGVMarketToWintheToss.Width = this.Width - 12;
            try
            {

            }
            catch (System.Exception ex)
            {

            }



            try
            {

            }
            catch (System.Exception ex)
            {

            }

        }
        private void txtRulesNew_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
           
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                lastwindowsize = this.Width;
                if (isMaximizedWindow == true)
                {
                    this.Width = System.Windows.SystemParameters.WorkArea.Width;
                    this.Height = System.Windows.SystemParameters.WorkArea.Height;

                }
                else
                {

                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        stkpnlmarketname.Width = this.Width - 290;
                        stkpnlmarketname.Margin = new Thickness(0, 0, 20, 0);
                        txtBlockMarkettimeandstatus.Width = this.Width - 314;

                        txtToBePlaced.Width = this.Width - 70;
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            //btnFancyPL.Visibility = Visibility.Visible;
                            //chkBettingAllowed.Visibility = Visibility.Visible;
                            //stkpnlbetsandmenuheading.Width = this.Width - 232;
                        }
                        else
                        {
                            //btnFancyPL.Visibility = Visibility.Collapsed;
                            //chkBettingAllowed.Visibility = Visibility.Collapsed;
                            //stkpnlbetsandmenuheading.Width = this.Width - 157;
                        }
                    }
                    else
                    {

                        stkpnlmarketname.Width = this.Width - 195;
                        stkpnlmarketname.Margin = new Thickness(80, 0, 0, 0);
                        txtBlockMarkettimeandstatus.Width = this.Width - 284;
                        //stkpnlbetsandmenuheading.Width = this.Width - 157;
                        txtToBePlaced.Width = this.Width - 30;
                    }



                    if (MarketBook.MainSportsname.Contains("Racing"))
                    {
                        if (MarketBook.Runners.Count >= 10)
                        {

                            DGVMarket.Height = this.Height - 65;
                        }
                        else
                        {
                            DGVMarket.Height = 750;

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



            if (isMaximizedWindow == true)
            {


            }
            else
            {
                if (this.WindowState == WindowState.Normal)
                {

                }

            }

            DGVMarket.Columns[7].Visibility = Visibility.Visible;
            DGVMarket.Columns[8].Visibility = Visibility.Visible;
            DGVMarket.Columns[11].Visibility = Visibility.Visible;
            DGVMarket.Columns[12].Visibility = Visibility.Visible;
            DGVMarket.Columns[0].Visibility = Visibility.Collapsed;
            DGVMarket.Columns[1].Visibility = Visibility.Visible;
            DGVMarket.Columns[2].Visibility = Visibility.Collapsed;
            //DGVMarket.Columns[3].Visibility = Visibility.Visible;

            if (MarketBook.MainSportsname.Contains("Racing"))
            {
                DGVMarket.Columns[4].Visibility = Visibility.Collapsed;
            }

            DGVMarketToWintheToss.Columns[7].Visibility = Visibility.Visible;
            DGVMarketToWintheToss.Columns[8].Visibility = Visibility.Visible;
            DGVMarketToWintheToss.Columns[11].Visibility = Visibility.Visible;
            DGVMarketToWintheToss.Columns[12].Visibility = Visibility.Visible;
            DGVMarketToWintheToss.Columns[0].Visibility = Visibility.Visible;

            DGVMarketFancy.Columns[7].Visibility = Visibility.Visible;
            DGVMarketFancy.Columns[8].Visibility = Visibility.Visible;
            DGVMarketFancy.Columns[11].Visibility = Visibility.Visible;
            DGVMarketFancy.Columns[12].Visibility = Visibility.Visible;



            // stkpnlBets.Visibility = Visibility.Collapsed;
            //stkpnlMenu.Visibility = Visibility.Collapsed;
            stkpnlMarketGrid.Width = this.Width - 12;
            DGVMarket.Width = this.Width - 12;
            DGVMarketToWintheToss.Width = this.Width - 12;
            try
            {

            }
            catch (System.Exception ex)
            {

            }
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void DGVMarket_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void stkpnlmarketname_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
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
                    this.DragMove();
                }
            }
            catch (System.Exception ex)
            {

            }


        }

        private void stkpnlmarketname_MouseMove(object sender, MouseEventArgs e)
        {

        }

      

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            tmrUpdateMarket.Stop();
            ExternalAPI.TO.MarketBook markettoremove = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == MarketBook.MarketId).First();
            LoggedinUserDetail.MarketBooks.Remove(markettoremove);
            LoggedinUserDetail.OpenMarkets.Remove("marketwin" + markettoremove.MarketId.Replace(".", ""));
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                objUsersServiceCleint.SetMarketBookClosedbyUserAsync(73, MarketBook.MarketId);
            }
            else
            {
                objUsersServiceCleint.SetMarketBookClosedbyUserAsync(LoggedinUserDetail.GetUserID(), MarketBook.MarketId);
            }
            if (MarketBookWintheToss.MarketId != null)
            {
                ExternalAPI.TO.MarketBook markettoremove1 = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == MarketBookWintheToss.MarketId).First();
                LoggedinUserDetail.MarketBooks.Remove(markettoremove1);

                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    objUsersServiceCleint.SetMarketBookClosedbyUserAsync(73, MarketBookWintheToss.MarketId);
                }
                else
                {
                    objUsersServiceCleint.SetMarketBookClosedbyUserAsync(LoggedinUserDetail.GetUserID(), MarketBookWintheToss.MarketId);
                }
            }

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
            //popupBetslip.Visibility = Visibility.Collapsed;
            //popupBetslip.IsOpen = false;
        }
        public string BetSlipSelectionID = "";
        public string BetType = "";
        AllowedMarkets AllowedMarketsForUser = new AllowedMarkets();
        public bool CheckForAllowedBettingOverAll(string categoryname, string marketbookname, string RunnersCount)
        {

            bool AllowedBet = false;
           
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
                if (MarketStatusstr == "IN-PLAY" || MarketStatusstr == "In Play")
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

                }

            }
            catch (System.Exception ex)
            {

            }
            }

        Sp_GetKalijut_Result KJs = new Sp_GetKalijut_Result();
        Sp_GetFigureOdds_Result FigureOdds = new Sp_GetFigureOdds_Result();
        public UserServicesClient objUsersServiceCleint = new UserServicesClient();
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
                            GetDataForFancy(MarketBook.EventID, MarketBook.MarketId);
                            DGVMarketIndianFancy.ItemsSource = lstMarketBookRunnersFancyin;
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
                                           }
                                 }
                            catch (System.Exception ex)
                            {
                            }
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
            catch (System.Exception ex)
            {

            }
        }
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

        int BetPlaceWait = 0;
        int BetWaitTimerInterval = 0;
        public void SetBetPlacewaitTimerandInterval(string categoryname, string marketbookname, string runnerscount)
        {
            if (runnerscount == "1")
            {
                BetPlaceWait = LoggedinUserDetail.BetPlaceWaitInterval.FancyBetPlaceWait.Value;
                BetWaitTimerInterval = LoggedinUserDetail.BetPlaceWaitInterval.FancyTimerInterval.Value;
            }
            else
            {
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
                    }
                }
            }

        }
        
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
             
                if (LoggedinUserDetail.isInserting == true)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show(this, "Please wait for other bet completion.");
                    return;
                }

                if (CurrentMarketBookId != currmarketbookforbet.MarketId)
                {
                    //popupBetslip.IsOpen = false;
                    Xceed.Wpf.Toolkit.MessageBox.Show("Please open bet slip again.");
                    return;
                }
            
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.isInserting = false;

                // btnSubmitBetSlip.IsEnabled = true;
              //  btnResetBetSlip.IsEnabled = true;

                Xceed.Wpf.Toolkit.MessageBox.Show("Something went wrong. Please bet again or call the vendor.");
            }
        }
        public bool isFirstClickonslip = true;
        private void btnSubmitBetSlip_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nupdnUserAmountMultiple_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
          //  CalculateAmountsMultiple();
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
                    Xceed.Wpf.Toolkit.MessageBox.Show(this, "Please wait for other bet completion.");
                    return;
                }

               
            }
            catch (System.Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message.ToString());
                //lblWaitTimer.Content = "";
                LoggedinUserDetail.isInserting = false;
                isFirstClickonslip = true;
               // btnSubmitBetSlipMultiple.IsEnabled = true;
                //btnResetBetSlipMultiple.IsEnabled = true;

                Xceed.Wpf.Toolkit.MessageBox.Show("Something went wrong. Please bet again or call the vendor.");
            }
        }
      

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    //popupBetslipMultiple.IsOpen = false;
        //}

        //private void btnCancelBetSlipMultiple_Click(object sender, RoutedEventArgs e)
        //{
        //    popupBetslipMultiple.IsOpen = false;
        //    isFirstClickonslip = true;
        //}

        //private void btnResetBetSlip_Click(object sender, RoutedEventArgs e)
        //{
        //    isFirstClickonslip = true;
        //    if (BetType == "back")
        //    {
        //        nupdnUserAmount.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeBack);
        //    }
        //    else
        //    {
        //        nupdnUserAmount.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeLay);
        //    }
           
        //    calculateProfitandLossonBetSlip();
        //}

        //private void btnResetBetSlipMultiple_Click(object sender, RoutedEventArgs e)
        //{
        //    isFirstClickonslip = true;
        //    if (BetType == "back")
        //    {
        //        nupdnUserAmountMultiple.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeBackMultiple);
        //    }
        //    else
        //    {
        //        nupdnUserAmountMultiple.Value = Convert.ToInt32(Properties.Settings.Default.DefaultStakeLayMultiple);
        //    }
        //    CalculateAmountsMultiple();
        //}

        //private void Button_Click_2(object sender, RoutedEventArgs e)
        //{
        //    popupBetslipKeys.IsOpen = false;
        //}

      

        private void chkUpdateBettingAllowed_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (SPMain.Visibility == Visibility.Visible)
                {
                    SPMain.Visibility = Visibility.Collapsed;
                    tmrUpdateLiabalities.Stop();
                    lstMarketBookRunnersFancyin.Clear();
                }
                else
                {                    
                        SPMain.Visibility = Visibility.Visible;
                    tmrUpdateLiabalities.Start();
                }

            }
            catch (System.Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(this, ex.Message);
            }
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
          
        }

        //private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    RulesWindow objrules = new RulesWindow();
        //    objrules.rules = SetRulesbyName();
        //    objrules.Show();
        //}
        //List<MarketRules> MarketRulesAll = new List<MarketRules>();
        //public string SetRulesbyName()
        //{
        //    string categoryname = MarketBook.MainSportsname;
        //    string marketbookname = MarketBook.MarketBookName;

        //    if (categoryname.Contains("Horse Racing") && !marketbookname.Contains("To Be Placed"))
        //    {

        //        return MarketRulesAll.Where(item => item.Category == "Horse Racing" && item.MarketType == "Win").Select(item => item.Rules).FirstOrDefault().ToString();
        //    }
        //    else
        //    {
        //        if (categoryname.Contains("Horse Racing") && marketbookname.Contains("To Be Placed"))
        //        {
        //            return MarketRulesAll.Where(item => item.Category == "Horse Racing" && item.MarketType == "To Be Placed").Select(item => item.Rules).FirstOrDefault().ToString();

        //        }
        //        else
        //        {
        //            if (categoryname.Contains("Greyhound Racing") && marketbookname.Contains("To Be Placed"))
        //            {
        //                return MarketRulesAll.Where(item => item.Category == "Greyhound Racing" && item.MarketType == "To Be Placed").Select(item => item.Rules).FirstOrDefault().ToString();
        //            }
        //            else
        //            {
        //                if (categoryname.Contains("Greyhound Racing") && !marketbookname.Contains("To Be Placed"))
        //                {
        //                    return MarketRulesAll.Where(item => item.Category == "Greyhound Racing" && item.MarketType == "Win").Select(item => item.Rules).FirstOrDefault().ToString();

        //                }
        //                else
        //                {
        //                    if (marketbookname.Contains("Completed Match"))
        //                    {
        //                        return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Completed Match").Select(item => item.Rules).FirstOrDefault().ToString();

        //                    }
        //                    else
        //                    {
        //                        if (marketbookname.Contains("Innings Runs") || marketbookname.Contains("Inns Runs"))
        //                        {
        //                            return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Innings Runs").Select(item => item.Rules).FirstOrDefault().ToString();

        //                        }
        //                        else
        //                        {
        //                            if (categoryname == "Tennis")
        //                            {
        //                                return MarketRulesAll.Where(item => item.Category == "Tennis").Select(item => item.Rules).FirstOrDefault().ToString();

        //                            }
        //                            else
        //                            {
        //                                if (categoryname == "Soccer")
        //                                {
        //                                    return MarketRulesAll.Where(item => item.Category == "Soccer").Select(item => item.Rules).FirstOrDefault().ToString();

        //                                }
        //                                else
        //                                {
        //                                    if (marketbookname.Contains("Tied Match"))
        //                                    {
        //                                        return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Tied Match").Select(item => item.Rules).FirstOrDefault().ToString();

        //                                    }
        //                                    else
        //                                    {
        //                                        if (marketbookname.Contains("Winner"))
        //                                        {
        //                                            return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Match Odds").Select(item => item.Rules).FirstOrDefault().ToString();

        //                                        }
        //                                        else
        //                                        {
        //                                            if (MarketBook.Runners.Count == 3)
        //                                            {
        //                                                return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Test Match").Select(item => item.Rules).FirstOrDefault().ToString();
        //                                            }
        //                                            else
        //                                            {
        //                                                string cricketrules = MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Match Odds").Select(item => item.Rules).FirstOrDefault().ToString();
        //                                                cricketrules += Environment.NewLine + Environment.NewLine + MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Fancy").Select(item => item.Rules).FirstOrDefault().ToString();
        //                                                return cricketrules;
        //                                            }


        //                                        }
        //                                    }

        //                                }
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

    
        private void cmbAgentsForProfitandLoss_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {


                SelectedAgentForProfitandLoss = Convert.ToInt32(cmbAgentsForProfitandLoss.SelectedValue);
                if (SelectedAgentForProfitandLoss == 0)
                {
                    SelectedAgentForProfitandLoss = 535;
                }
            }
            catch (System.Exception ex)
            {

            }
            txtAverageAmountbyAgent2.Content = "0";
            txtAverageAmountbyAgnet1.Content = "0";
            lblAveragebyAgent1.Content = "";
            lblAveragebyAgent2.Content = "";
            lblProfitandLossRunnerbyAgent2.Content = "0";
            lblProfitorLossRunnerAgent1.Content = "0";


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
            cmbAgentsForProfitandLoss.SelectedValue = 535;
            cmbAgentsForProfitandLossFAncy.IsSynchronizedWithCurrentItem = false;
            cmbAgentsForProfitandLossFAncy.ItemsSource = LoggedinUserDetail.AllUsers.Where(u => new[] { "2" }.Contains(u.UserTypeID.ToString())).ToList();
            cmbAgentsForProfitandLossFAncy.DisplayMemberPath = "UserName";
            cmbAgentsForProfitandLossFAncy.SelectedValuePath = "ID";
            cmbAgentsForProfitandLossFAncy.SelectedValue = 73;
        }
        public void ShowaverageSectionclick()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {


                popupAverageSection.IsOpen = true;
                AssignUserstoCombobox();


                var results = objUsersServiceCleint.GetSelectionNamesbyMarketID(MarketBook.MarketId);
                cmbRunner11.ItemsSource = null;
                cmbRunner11.ItemsSource = results;
                cmbRunner11.DisplayMemberPath = "SelectionName";
                cmbRunner11.SelectedValuePath = "SelectionID";
                cmbRunner11.IsSynchronizedWithCurrentItem = false;

                cmbRunner21.IsSynchronizedWithCurrentItem = false;
             
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
                            lblAverage11.Foreground = Brushes.Green;
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
                                lblAverage21.Foreground = Brushes.Green;
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
                    MessageBox.Show(this, "Please enter correct values.");
                }

            }
            catch (System.Exception ex)
            {
                btnAdminBetAdd1.IsEnabled = true;
                MessageBox.Show(this, "Please enter correct values.");
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

     

        private void DGVMatchedBetsAdmin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            try
            {


                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    DataGrid objSender = (DataGrid)sender;
                    if (objSender.Items.Count > 0)
                    {

                        UserBetsForAdmin objselectedmarket = (UserBetsForAdmin)objSender.SelectedItem;
                        isMaximizedWindow = false;
                        this.WindowState = WindowState.Normal;
                        
                        //if (MarketBook.MainSportsname == "Cricket" && MarketBook.Runners.Count == 3 && DGVMarketFancy.Visibility == Visibility.Collapsed)
                        if (MarketBook.MainSportsname == "Cricket" && MarketBook.Runners.Count == 3 && DGVMarketIndianFancy.Visibility == Visibility.Collapsed && DGVMarketFancy.Visibility == Visibility.Collapsed)
                        {
                            this.Height = 400;
                        }
                        else
                        {
                            this.Height = 400;
                        }
                        // this.Height = 400;
                        this.Width = 676;
                        this.Left = 0;
                     
                        foreach (Window win in App.Current.Windows)
                        {
                            if (win.Name == "mainwindow")
                            {
                                MainWindow window = win as MainWindow;
                               
                                window.MarketBook(objselectedmarket.MarketBookID);
                                return;
                            }
                        }


                    }
                }
            }
            catch (System.Exception ex)
            {
              
            }
        }
        public bool isProfitLossbyAgentShown = false;
        public void ShowaverageSectionByAgentclick()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {


                popupProffitLossAgent.IsOpen = true;
                isProfitLossbyAgentShown = true;
                AssignUserstoCombobox();
                cmbRunnerForProfitandLoss1.IsSynchronizedWithCurrentItem = false;


                var results = objUsersServiceCleint.GetSelectionNamesbyMarketID(MarketBook.MarketId);

                cmbRunnerForProfitandLoss1.ItemsSource = results;
                cmbRunnerForProfitandLoss1.DisplayMemberPath = "SelectionName";
                cmbRunnerForProfitandLoss1.SelectedValuePath = "SelectionID";
                cmbRunnerForProfitandLoss1.SelectedIndex = 0;
                cmbRunnerForProfitandLoss2.IsSynchronizedWithCurrentItem = false;
                cmbRunnerForProfitandLoss2.ItemsSource = results;
                cmbRunnerForProfitandLoss2.DisplayMemberPath = "SelectionName";
                cmbRunnerForProfitandLoss2.SelectedValuePath = "SelectionID";
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

                txtAverageAmountbyAgnet1.Content = "0.00";
                txtAverageAmountbyAgent2.Content = "0.00";
                cmbRunnerForProfitandLoss2.SelectedIndex = 1;

                CalculateAvearageforSelectedAgent();


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
                          
                        }

                    }
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        public void ShowaverageSectionclickFancy()
        {

            if (MarketBook.LineVMarkets != null)
            {


                popupFancyCutting.IsOpen = true;

              
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
        public void ShowFancyResultsPostingPanel()
        {

            if (MarketBook.LineVMarkets != null)
            {


                popupFancyResultPosting.IsOpen = true;

              
                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                foreach (var item in MarketBook.LineVMarkets)
                {
                    ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                    objRunner.SelectionId = item.MarketCatalogueID;
                    objRunner.RunnerName = item.MarketCatalogueName;
                    lstRunners.Add(objRunner);
                }


                cmbRunnersFancyResults.IsSynchronizedWithCurrentItem = false;
                cmbRunnersFancyResults.ItemsSource = lstRunners;
                cmbRunnersFancyResults.DisplayMemberPath = "RunnerName";
                cmbRunnersFancyResults.SelectedValuePath = "SelectionId";
                cmbRunnersFancyResults.SelectedIndex = 0;

            }


        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            popupFancyResultPosting.IsOpen = false;
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            popupFancyProffitLossAgent.IsOpen = false;
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
        public void ShowaverageSectionByAgentclickFancy()
        {
            if (1 == 1)
            {

                if (MarketBook.LineVMarkets != null)
                {

                    popupFancyProffitLossAgent.IsOpen = true;

                    List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                    foreach (var item in MarketBook.LineVMarkets)
                    {
                        ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                        objRunner.SelectionId = item.SelectionID + "|" + item.MarketCatalogueID + "|" + item.MarketCatalogueName;
                        objRunner.RunnerName = item.MarketCatalogueName;
                        lstRunners.Add(objRunner);
                    }
                    cmbRunnerForProfitandLoss1FAncy.IsSynchronizedWithCurrentItem = false;
                    cmbRunnerForProfitandLoss1FAncy.ItemsSource = lstRunners;
                    cmbRunnerForProfitandLoss1FAncy.DisplayMemberPath = "RunnerName";
                    cmbRunnerForProfitandLoss1FAncy.SelectedValuePath = "SelectionId";

                }

            }

        }

        private void btnBookbyAgent_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRunnerForProfitandLoss1FAncy.SelectedIndex >= 0)
            {
                List<UserBetsforAgent> lstSelectedAgentBets = new List<UserBetsforAgent>();
                string userbets = objUsersServiceCleint.GetUserBetsbyAgentIDwithZeroReferer(Convert.ToInt32(cmbAgentsForProfitandLossFAncy.SelectedValue), LoggedinUserDetail.PasswordForValidate);
                var lstUserBetAgent = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(userbets);
                string[] MarketIDandname = cmbRunnerForProfitandLoss1FAncy.SelectedValue.ToString().Split('|');
                foreach (Window win in App.Current.Windows)
                {
                    if (win.Name.Contains("BookPositionWinAgent"))
                    {

                        win.Close();

                    }
                }
                BookPosition objbookpostion = new BookPosition();
                objbookpostion.Name = "BookPositionWinAgent" + MarketIDandname[1].Replace(".", "");
                objbookpostion.CurrentUserbetsAgent = lstUserBetAgent;
                objbookpostion.marketBookID = MarketIDandname[1];
                objbookpostion.isopenedbyselecedagentfromadmin = true;
                objbookpostion.UserTypeID = 2;
                objbookpostion.marketbookName = MarketIDandname[2] + "(" + lblMarketName.Content.ToString() + ")";
                objbookpostion.userID = Convert.ToInt32(cmbAgentsForProfitandLossFAncy.SelectedValue);
                objbookpostion.Show();
            }
        }

        private void btnLoadScores_Click(object sender, RoutedEventArgs e)
        {
            LoadScores();
        }
        public void LoadScores()
        {
            try
            {


                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    var eventdetails = objUsersServiceCleint.GetEventDetailsbyMarketBook(cmbRunnersFancyResults.SelectedValue.ToString());

                    var inningsnameandover = eventdetails.MarketCatalogueName.Split(new string[] { " Innings " }, StringSplitOptions.None);
                    string innings = Regex.Match(inningsnameandover[0], @"\d+").Value;
                    string over = Regex.Match(inningsnameandover[1], @"\d+").Value;
                    var matchscores = JsonConvert.DeserializeObject<ScoresForResultPosting>(objUsersServiceCleint.GetScorebyEventIDandInningsandOvers(eventdetails.associateeventID, eventdetails.EventOpenDate.Value, Convert.ToInt32(innings), Convert.ToInt32(over)));
                    if (matchscores != null)
                    {
                        txtScoresResults.Text = matchscores.Scores.Value.ToString();
                        txtOversResults.Text = matchscores.Overs.Value.ToString();
                        txtInningsResult.Text = matchscores.Innings.Value.ToString();
                        lblTeamname.Content = matchscores.TeamName.ToString();
                    }
                    else
                    {
                        txtScoresResults.Text = "";
                        txtOversResults.Text = over.ToString();
                        txtInningsResult.Text = innings.ToString();
                        lblTeamname.Content = "";
                        txtScoresResults.Focus();
                    }
                    lblPostResultsFor.Content = cmbRunnersFancyResults.Text;


                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private void btnPostScores_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRunnersFancyResults.SelectedIndex > -1)
            {
                MessageBoxResult messgeresult = Xceed.Wpf.Toolkit.MessageBox.Show("Post Results ?", "Confirm Post Results", MessageBoxButton.YesNo);
                if (messgeresult == MessageBoxResult.Yes)
                {
                    objUsersServiceCleint.CheckforMatchCompletedFancy(cmbRunnersFancyResults.SelectedValue.ToString(), Convert.ToInt32(txtScoresResults.Text));
                    popupFancyResultPosting.IsOpen = false;
                    Xceed.Wpf.Toolkit.MessageBox.Show("Posted Succesfully.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
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
            //try
            //{
            //    cmbCuttingUserFancy.Focus();
            //}
            //catch (System.Exception ex)
            //{

            //}


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
                    MessageBox.Show(this, "Please enter correct values.");
                }

            }
            catch (System.Exception ex)
            {
                btnAddFancyCutting.IsEnabled = true;
                MessageBox.Show(this, "Please enter correct values.");
            }
        }

        //private void Button_Click_12(object sender, RoutedEventArgs e)
        //{
        //    popupCuttingBets.IsOpen = false;
        //}

        //private void btnCuttingBets_Click(object sender, RoutedEventArgs e)
        //{
        //    if (LoggedinUserDetail.GetUserTypeID() == 1)
        //    {
        //        popupCuttingBets.IsOpen = true;
        //        List<UserBetsForAdmin> lstCurrentBetsMatched = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true && item.UserTypeID == 4).OrderByDescending(item => item.ID).ToList();
        //        DGVAllCuttingBets.ItemsSource = lstCurrentBetsMatched;
        //    }
        //}

        //private void DGVAllCuttingBets_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (LoggedinUserDetail.GetUserTypeID() == 1)
        //    {
        //        if (DGVAllCuttingBets.CurrentCell.Column.DisplayIndex == 7)
        //        {
        //            UserBetsForAdmin objSelectedRow = (UserBetsForAdmin)DGVAllCuttingBets.CurrentCell.Item;
        //            long BetID = objSelectedRow.ID;
        //            objUsersServiceCleint.UpdateUserBetUnMatchedStatusTocompleteforCuttingUser(BetID, LoggedinUserDetail.PasswordForValidate);
                 
        //            List<UserBetsForAdmin> lstCurrentBetsMatched = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true && item.UserTypeID == 4 && item.ID != Convert.ToInt64(BetID)).ToList();
        //            DGVAllCuttingBets.ItemsSource = lstCurrentBetsMatched;

        //        }
        //    }
        //}

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
        public int UserIDforLoadMarket = 0;
     
        public Service123Client objBettingClient = new Service123Client();

        public ExternalAPI.TO.MarketBook GetCurrentMarketBook(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate, bool BettingAllowed)
        {
            try
            {
                string[] marketIds = new string[]
                      {
                    marketid
                      };
                Service123Client objBettingClientOld = new Service123Client();
                var marketbook = objBettingClientOld.GetMarketDatabyID(marketIds, sheetname, marketopendate, MainSportsCategory, LoggedinUserDetail.PasswordForValidate);
                return marketbook[0];


            }
            catch (System.Exception ex)
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public bool isOpeningWindow = false;
        public void MarketBookFunc(string ID)
        {
            try
            {

                isOpeningWindow = true;
                LoggedinUserDetail.CheckifUserLogin();
              
                if (ID != "" && LoggedinUserDetail.GetUserTypeID() == 1)
                {

                    objUsersServiceCleint.SetMarketBookOpenbyUSer(73, ID);
                }
                var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                if ( LoggedinUserDetail.GetUserTypeID() == 1) //LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2 ||
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
                            string tabpagename = "markettab" + ID.Replace(".", "").ToString();
                            var marketwindowname = LoggedinUserDetail.OpenMarkets.Where(item => item == tabpagename).FirstOrDefault();

                            if (marketwindowname != null)
                            {




                            }
                            else
                            {
                              
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
                      
                            }
                        }
                       
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

                isOpeningWindow = false;
            }
            catch (System.Exception ex)
            {
                isOpeningWindow = false;
            }
        }
        public void LoadGridMarket(string MarketBookID)
        {
          
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
                        objmarketwindow.MarketBook.isOpenExternally = true;
                       
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
                        isMaximizedWindow = false;
                        
                        //if (MarketBook.MainSportsname == "Cricket" && MarketBook.Runners.Count == 3 && DGVMarketFancy.Visibility == Visibility.Collapsed)
                        if (MarketBook.MainSportsname == "Cricket" && MarketBook.Runners.Count == 3 && DGVMarketIndianFancy.Visibility == Visibility.Collapsed)
                        {
                            this.Height = 400;
                        }
                        else
                        {
                            this.Height = 400;
                        }
                       
                        this.Width = 676;
                        this.Left = 0;
                     
                        foreach (Window win in App.Current.Windows)
                        {
                            if (win.Name == "mainwindow")
                            {
                                MainWindow window = win as MainWindow;
                                window.bsyindicator.IsBusy = true;
                                window.MarketBook(objSender.Tag.ToString());
                                return;
                            }
                        }
                      
                    }
                }


            }
            catch (System.Exception ex)
            {

            }
        }
  
        void win_SourceInitialized(object sender, EventArgs e)
        {
            System.IntPtr handle = (new WinInterop.WindowInteropHelper(this)).Handle;
            WinInterop.HwndSource.FromHwnd(handle).AddHook(new WinInterop.HwndSourceHook(WindowProc));
          
        }
        private static System.IntPtr WindowProc(
             System.IntPtr hwnd,
             int msg,
             System.IntPtr wParam,
             System.IntPtr lParam,
             ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    WmGetMinMaxInfo(hwnd, lParam);
                    handled = true;
                    break;
            }

            return (System.IntPtr)0;
        }
        private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        {

            MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

           
            int MONITOR_DEFAULTTONEAREST = 0x00000002;
            System.IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            if (monitor != System.IntPtr.Zero)
            {

                MONITORINFO monitorInfo = new MONITORINFO();
                GetMonitorInfo(monitor, monitorInfo);
                RECT rcWorkArea = monitorInfo.rcWork;
                RECT rcMonitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }


        /// <summary>
        /// POINT aka POINTAPI
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            /// x coordinate of point.
            /// </summary>
            public int x;
            /// <summary>
            /// y coordinate of point.
            /// </summary>
            public int y;

            /// <summary>
            /// Construct a point of coordinates (x,y).
            /// </summary>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };




        /// <summary>
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            /// <summary>
            /// </summary>            
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

            /// <summary>
            /// </summary>            
            public RECT rcMonitor = new RECT();

            /// <summary>
            /// </summary>            
            public RECT rcWork = new RECT();

            /// <summary>
            /// </summary>            
            public int dwFlags = 0;
        }


        /// <summary> Win32 </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            /// <summary> Win32 </summary>
            public int left;
            /// <summary> Win32 </summary>
            public int top;
            /// <summary> Win32 </summary>
            public int right;
            /// <summary> Win32 </summary>
            public int bottom;

            /// <summary> Win32 </summary>
            public static readonly RECT Empty = new RECT();

            /// <summary> Win32 </summary>
            public int Width
            {
                get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
            }
            /// <summary> Win32 </summary>
            public int Height
            {
                get { return bottom - top; }
            }

            /// <summary> Win32 </summary>
            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }


            /// <summary> Win32 </summary>
            public RECT(RECT rcSrc)
            {
                this.left = rcSrc.left;
                this.top = rcSrc.top;
                this.right = rcSrc.right;
                this.bottom = rcSrc.bottom;
            }

            /// <summary> Win32 </summary>
            public bool IsEmpty
            {
                get
                {
                    // BUGBUG : On Bidi OS (hebrew arabic) left > right
                    return left >= right || top >= bottom;
                }
            }
            /// <summary> Return a user friendly representation of this struct </summary>
            public override string ToString()
            {
                if (this == RECT.Empty) { return "RECT {Empty}"; }
                return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
            }

            /// <summary> Determine if 2 RECT are equal (deep compare) </summary>
            public override bool Equals(object obj)
            {
                if (!(obj is Rect)) { return false; }
                return (this == (RECT)obj);
            }

            /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
            public override int GetHashCode()
            {
                return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
            }


            /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
            public static bool operator ==(RECT rect1, RECT rect2)
            {
                return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
            }

            /// <summary> Determine if 2 RECT are different(deep compare)</summary>
            public static bool operator !=(RECT rect1, RECT rect2)
            {
                return !(rect1 == rect2);
            }


        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        /// <summary>
        /// 
        /// </summary>
        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        private void TextBlock_MouseLeftButtonUp_3(object sender, MouseButtonEventArgs e)
        {
            if (txtTopMost.Tag.ToString() == "0")
            {
                txtTopMost.Tag = "1";
                this.Topmost = true;
                RotateTransform rotateTransform = new RotateTransform(0, 0.5, 0.5);


                pinTopMost.RenderTransform = rotateTransform;
            }
            else
            {
                txtTopMost.Tag = "0";
                this.Topmost = false;
                RotateTransform rotateTransform = new RotateTransform(90, 0.5, 0.5);

                pinTopMost.RenderTransform = rotateTransform;
            }
        }
        VideoWindow objVideo = new VideoWindow();
        private void imgShowTV_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowaverageSectionByAgentclick();
        }
        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            try
            {
                popupFigureSyncONOFF.IsOpen = true;
                lblFGSyncONOFFFor.Content = MarketBook.MarketBookName;
            }
            catch (System.Exception ex)
            {


            }
        }
        private void btnLoadScoresI_Click(object sender, RoutedEventArgs e)
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

                    var inningsnameandover = eventdetails.MarketCatalogueName.Split(new string[] { " Inning " }, StringSplitOptions.None);
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
                    lblPostResultsForKJ.Content = cmbRunnersFancyResults.Text;


                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private void btnPostScoresI_Click(object sender, RoutedEventArgs e)
        {
            if (cmbRunnersFancyResultsIN.SelectedIndex > -1)
            {
                MessageBoxResult messgeresult = Xceed.Wpf.Toolkit.MessageBox.Show("Post Results ?", "Confirm Post Results", MessageBoxButton.YesNo);
                if (messgeresult == MessageBoxResult.Yes)
                {
                    string aa = cmbRunnersFancyResultsIN.SelectedValue.ToString();
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
        private void Button_Click_24(object sender, RoutedEventArgs e)
        {
            popupFigureSyncONOFF.IsOpen = false;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            ShowaverageSectionclickFancy();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                ShowFancyResultsPostingPanel();
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
        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            popupIndianFancyResultPosting.IsOpen = false;
        }

        public void ShowFancyResultsPostingPanelKJ()
        {

            if (MarketBook.KJMarkets != null)
            {


                popupFancyResultPostingKJ.IsOpen = true;

                //  var results = LastloadedLinMarkets.Where(item => item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 0 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 0).ToList();
                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                foreach (var item in MarketBook.KJMarkets)
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

        public void ShowFancyResultsPostingPanelFig()
        {

            if (MarketBook.FigureMarkets != null)
            {


                popupFancyResultPostingFig.IsOpen = true;

                //  var results = LastloadedLinMarkets.Where(item => item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 0 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 0).ToList();
                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                foreach (var item in MarketBook.FigureMarkets)
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

            if (MarketBook.LineVMarketsIN != null)
            {
                popupIndianFancyResultPosting.IsOpen = true;

                //  var results = LastloadedLinMarkets.Where(item => item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 0 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 0).ToList();
                List<ExternalAPI.TO.RunnerForIndianFancy> lstRunners = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                foreach (var item in MarketBook.LineVMarketsIN)
                {
                    ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner.SelectionId = item.SelectionID;
                    objRunner.RunnerName = item.SelectionName;
                    objRunner.MarketBookID = item.MarketBookID;
                    txtInningsResultI.Text = item.MarketBookID;
                    lstRunners.Add(objRunner);
                }

                cmbRunnersFancyResultsIN.IsSynchronizedWithCurrentItem = false;
                cmbRunnersFancyResultsIN.ItemsSource = lstRunners;
                cmbRunnersFancyResultsIN.DisplayMemberPath = "RunnerName";
                cmbRunnersFancyResultsIN.SelectedValuePath = "SelectionId";

                cmbRunnersFancyResultsIN.SelectedIndex = 0;

            }
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            ShowaverageSectionByAgentclickFancy();
        }

        //private void TextBlock_MouseLeftButtonUp_4(object sender, MouseButtonEventArgs e)
        //{
        //    if (BetType == "back")
        //    {
        //        nupdownOdd.Value = Convert.ToDecimal(lblBetSlipBack.Content);
        //    }
        //    else
        //    {
        //        nupdownOdd.Value = Convert.ToDecimal(lblBetSlipLay.Content);
        //    }
        //}

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
       // AccountsReceiveableWindow objAccReceWind = new AccountsReceiveableWindow();
        //private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        //{
        //    objAccReceWind = new globaltraders.AccountsReceiveableWindow();
        //    objAccReceWind.Show();
        //}



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


     

       
        private void btnFancyPL_Click(object sender, RoutedEventArgs e)
        {
            ProfitLossWindow objPLWin = new ProfitLossWindow();
            objPLWin.chkOnlyFancy.IsChecked = true;
            objPLWin.chkByMarketCricket.IsChecked = true;
            objPLWin.ShowDialog();
        }

        //private void lblBetSlipBack_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (BetType == "back")
        //    {
        //        nupdownOdd.Value = Convert.ToDecimal(lblBetSlipBack.Content);
        //    }
        //    else
        //    {
        //        nupdownOdd.Value = Convert.ToDecimal(lblBetSlipLay.Content);
        //    }
        //}

        //private void lblBetSlipLay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    if (BetType == "back")
        //    {
        //        nupdownOdd.Value = Convert.ToDecimal(lblBetSlipBack.Content);
        //    }
        //    else
        //    {
        //        nupdownOdd.Value = Convert.ToDecimal(lblBetSlipLay.Content);
        //    }
        //}

        //private void Button_Click_18(object sender, RoutedEventArgs e)
        //{
        //    popupAllBets.IsOpen = false;
        //}

        //private void btnAllBets_Click(object sender, RoutedEventArgs e)
        //{
        //    popupAllBets.IsOpen = true;
        //    UpdateUserBetsDataAll();

        //}

        //private void btnAllBetsRefresh_Click(object sender, RoutedEventArgs e)
        //{
        //    UpdateUserBetsDataAll();
        //}

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                backgroundWorker.CancelAsync();
                backgroundWorker.Dispose();
                backgroundWorker = null;
                backgroundWorkerLiabalityandScore.CancelAsync();
                backgroundWorkerLiabalityandScore.Dispose();
                backgroundWorkerLiabalityandScore = null;
                backgroundWorkerUpdateFigData.CancelAsync();
                backgroundWorkerUpdateFigData.Dispose();
                backgroundWorkerUpdateFigData = null;
                backgroundWorkerProfitandlossbyAgent.CancelAsync();
                backgroundWorkerProfitandlossbyAgent.Dispose();
                backgroundWorkerProfitandlossbyAgent = null;
                backgroundWorkerUpdateData.CancelAsync();
                backgroundWorkerUpdateData.Dispose();
                backgroundWorkerUpdateData = null;
                bgdata = true;
                data = true;
                getfancy = true;
                GC.Collect();
              
            }
            catch (System.Exception ex)
            {
                tmrUpdateMarket.Stop();
            }
            tmrUpdateMarket.Stop();

        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            popupFancySyncONOFF.IsOpen = false;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            MarketBook.LineVMarkets.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().isOpenedbyUser = true;
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

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            try
            {
                popupKalijuttSyncONOFF.IsOpen = true;
                lblKalijuttSyncONOFFFor.Content = MarketBook.MarketBookName;
            }
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
           
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            try
            {
                if (isMaximizedWindow == false)
                {
                    var height = System.Windows.SystemParameters.PrimaryScreenHeight - 40;
                    var width = System.Windows.SystemParameters.PrimaryScreenWidth;

                    if (this.Left < 0)
                        this.Left = 0;
                    if (this.Top < 0)
                        this.Top = 0;
                    if (this.Top + this.Height > height)
                        this.Top = height - this.Height;
                    if (this.Left + this.Width > width)
                        this.Left = width - this.Width;
                }

            }
            catch (System.Exception ex)
            {


            }


        }

        private void DGVMarketIndianFancy_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {

            try
            {
                if (DGVMarketIndianFancy.Items.Count > 0)
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
                    //Clickedlocationin = 9;
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
                        //objbookpostion.Name = "BookPostionForINwin" + CurrentMarketBookId.Replace(".", "");
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
                    if (objSelectedRow.RunnerStatusstr == "SUSPENDED" || objSelectedRow.RunnerStatusstr == "Ball Running")
                    {
                        return;
                    }

                    if ((currcellindx >= 6 && currcellindx <= 12))
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }


        bool sethieght = false;
        private void CheckBox_Checked_4(object sender, RoutedEventArgs e)
        {
            sethieght = true;
            tmrUpdateLiabalities.Stop();
            lstMarketBookRunnersFancyin.Clear();
            SPMain.Visibility = Visibility.Collapsed;
            SetWindowHeight();
        }

        private void CheckBox_Unchecked_3(object sender, RoutedEventArgs e)
        {
            sethieght = false;
            tmrUpdateLiabalities.Start();
            //lstMarketBookRunnersFancyin.Clear();
            SPMain.Visibility = Visibility.Visible;
            SetWindowHeight();
        }
    }
}
