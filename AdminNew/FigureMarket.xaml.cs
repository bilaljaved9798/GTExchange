using bftradeline.HelperClasses;
using bftradeline.Models;
using ExternalAPI.TO;
using globaltraders.UserServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for AdminBetsWindow.xaml
    /// </summary>
    public partial class FigureMarket : Window
    {
        public FigureMarket()
        {
            InitializeComponent();
            backgroundWorkerUpdateFigData = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerUpdateFigData.DoWork += BackgroundWorkerUpdateFigData_DoWork;
            backgroundWorkerUpdateFigData.RunWorkerCompleted += BackgroundWorkerUpdateFigData_RunWorkerCompleted;
        }

        public string marketBookID = "";
        public string eventID = "";
        public string selectionID = "";

        public string marketbookName = "";
        public int UserTypeID = 0;
        public int userID = 0;
        bool time = true;
        public MarketBook MarketBook = new MarketBook();
        public GetDataFancy Fancymarket = new GetDataFancy();
        private ObservableCollection<UserBetsForAdmin> lstCurrentBetsAdmin;
        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersFigure;
        public List<MarketBook> LastloadedLinMarketsFig = new List<MarketBook>();
        public List<MarketBook> CurrentMarketProfitandLossToFigre = new List<MarketBook>();
        public MarketBook MarketBookFigure = new MarketBook();
        
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
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }


        public UserServicesClient objUsersServiceCleint = new UserServicesClient();

       
        private void BackgroundWorkerUpdateFigData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                if (Fancymarket.LinevMarkets.GroupBy(item => item.MarketCatalogueName).Select(g => g.First()).Where(item2 => item2.EventName == "Figure").ToList().Count > 0)
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


                if (backgroundWorkerUpdateFigData != null)
                    backgroundWorkerUpdateFigData.RunWorkerAsync();

            }
            catch (System.Exception ex)
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
                GetDataForFancy(eventID, marketBookID);
            }
            catch (System.Exception ex)
            {

            }

        }

        public BackgroundWorker backgroundWorkerUpdateFigData;


        public DispatcherTimer timerCountdown = new DispatcherTimer();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lblMarketName.Content = marketbookName;
                backgroundWorkerUpdateFigData.RunWorkerAsync();

            }
            catch (System.Exception ex)
            {

            }
        }


        private void stkpnlmarketname_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                backgroundWorkerUpdateFigData.CancelAsync();
                backgroundWorkerUpdateFigData.Dispose();
                backgroundWorkerUpdateFigData = null;
                GC.Collect();
            }
            catch (System.Exception ex)
            {

            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
       

        public MarketBook currmarketbookforbet = new MarketBook();
        string CategoryName = "";
        string MarketbooknameBet = "";
        string Marketstatusstr = "";
        bool BettingAllowed = false;
        string OpenDate = "";
        string runnerscount = "";
        string CurrentMarketBookId = "";

        public void AssignFiguredata()
        {
            lstMarketBookRunnersFigure = new ObservableCollection<MarketBookShow>();
            lstMarketBookRunnersFigure.Clear();

            var FigureMarkets = Fancymarket.LinevMarkets.GroupBy(item => item.MarketCatalogueName).Select(g => g.First()).Where(item2 => item2.EventName == "Figure").ToList(); //MarketBook.LineVMarkets.Where(item => item.isOpenedbyUser == true && item.EventName== "Figure").ToList();
            FigureMarkets = FigureMarkets.Where(item => item.isOpenedbyUser == true && item.EventID == eventID).ToList();
            if (FigureMarkets.Count > 0)
            {
                foreach (var bfobject in FigureMarkets)
                {
                    try
                    {
                        if (bfobject.MarketCatalogueID != MarketBookFigure.MarketId && MarketBookFigure.MarketId != null)
                        {
                            LastloadedLinMarketsFig.Clear();
                            time = true;
                        }
                        if (time == true)
                        {
                            LastloadedLinMarketsFig.Add(ConvertJsontoMarketObjectBFNewSourceFigure(bfobject.MarketCatalogueID, bfobject.MarketCatalogueName, "Cricket", bfobject.BettingAllowed));
                            MarketBookFigure = LastloadedLinMarketsFig.FirstOrDefault();
                            time = false;
                        }
                        GetRunnersDataSourceFigure(MarketBookFigure.Runners, MarketBookFigure);
                        UpdateLiabaliteies();
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

        public MarketBook ConvertJsontoMarketObjectBFNewSourceFigure(string marketid, string sheetname, string MainSportsCategory, bool BettingAllowed)
        {
            if (1 == 1)
            {
                var marketbook = new MarketBook();
                marketbook.MarketId = marketid;
                marketbook.BettingAllowed = BettingAllowed;
                marketbook.MainSportsname = "Cricket";
                marketbook.MarketBookName = sheetname;
                //marketbook.MarketStatusstr = lblMarketStatus.Content.ToString();

                int seletionID = 001;

                int RunnerName = 0;
                List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                for (int i = 0; i <= 9; i++)
                {


                    var runner = new ExternalAPI.TO.Runner();
                    runner.SelectionId = seletionID + i.ToString();
                    runner.RunnerName = i.ToString();

                    var lstpricelist = new List<PriceSize>();

                    var pricesize = new PriceSize();
                    pricesize.Size = 9;
                    pricesize.Price = i;
                    lstpricelist.Add(pricesize);
                    runner.ExchangePrices = new ExchangePrices();
                    runner.ExchangePrices.AvailableToBack = lstpricelist;
                    lstpricelist = new List<PriceSize>();

                    var pricesize1 = new PriceSize();
                    pricesize1.Size = 10.25;
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

        public void GetRunnersDataSourceFigure(List<ExternalAPI.TO.Runner> runners, MarketBook obj)
        {
            try
            {
                if (runners != null)
                {
                    lstMarketBookRunnersFigure = new ObservableCollection<MarketBookShow>();
                    lstMarketBookRunnersFigure.Clear();
                    foreach (var item in runners)
                    {
                        MarketBookShow objmarketbookshow = new MarketBookShow();
                        objmarketbookshow.Selection = obj.MarketBookName + item.RunnerName.ToString().ToUpper();
                        objmarketbookshow.SelectionID = item.SelectionId;

                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            List<UserBetsForAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item1 => item1.MarketBookID == obj.MarketId).ToList();
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
                        objmarketbookshow.runnerscount = runners.Count.ToString();
                        objmarketbookshow.CurrentMarketBookId = obj.MarketId;
                        if (objmarketbookshow.StallDraw != "Not" && objmarketbookshow.StallDraw != "" && objmarketbookshow.StallDraw != null)
                        {
                            objmarketbookshow.StallDraw = "(" + objmarketbookshow.StallDraw + ")";
                        }
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
                ProfitandLoss objProfitandloss = new ProfitandLoss();
                try
                {
                    List<UserBetsForAdmin> listfigurebets = LoggedinUserDetail.CurrentAdminBets.Where(x => x.location == "8").ToList();
                    if (MarketBookFigure.Runners != null && listfigurebets.Count > 0)
                    {
                        CurrentMarketProfitandLossToFigre = objProfitandloss.CalculateProfitandLossAdminFig(MarketBookFigure, LoggedinUserDetail.CurrentAdminBets.ToList());
                    }
                }
                catch (System.Exception ex)
                {
                }

            }
            catch (System.Exception ex)
            {

            }
        }

        public void GetDataForFancy(string EventID, string marketIds)
        {
            try
            {
                string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataIndiaFancy/?marketID=" + string.Join(",", marketIds);
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
                }
                Fancymarket = JsonConvert.DeserializeObject<GetDataFancy>(test1);
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}

