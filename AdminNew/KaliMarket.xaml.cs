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

namespace globaltraders
{
    public partial class KaliMarket : Window
    {
        public KaliMarket()
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
        private ObservableCollection<MarketBookShow> _lstMarketBookRunnerKalijut;
        public List<MarketBook> LastloadedLinMarketsFig = new List<MarketBook>();
        public List<MarketBook> CurrentMarketProfitandLossToFigre = new List<MarketBook>();
        public MarketBook MarketBookFigure = new MarketBook();


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

                if (Fancymarket.LinevMarkets.Where(item => item.EventName == "Kali v Jut").ToList().Count > 0)
                {
                    AssignKalijutdata();
                    DGVMarketKalijut.ItemsSource = lstMarketBookRunnerKalijut;
                    if (lstMarketBookRunnerKalijut != null)
                    {
                        if (lstMarketBookRunnerKalijut.Count > 0)
                        {
                            DGVMarketKalijut.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            DGVMarketKalijut.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        DGVMarketKalijut.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    DGVMarketKalijut.Visibility = Visibility.Collapsed;
                }
                SetWindowHeight();

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
                backgroundWorkerUpdateFigData.RunWorkerAsync();
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

        public void AssignKalijutdata()
        {
            try
            {

                int UserIDforLinevmarkets = 0;              
                UserIDforLinevmarkets = 73;

                lstMarketBookRunnerKalijut = new ObservableCollection<MarketBookShow>();
                lstMarketBookRunnerKalijut.Clear();
                var KJMarkets = Fancymarket.LinevMarkets.Where(item => item.isOpenedbyUser == true && item.EventName == "Kali v Jut" && item.EventID == eventID).ToList();
                var distinct = KJMarkets.GroupBy(x => x.MarketCatalogueID, (key, group) => group.First());
                if (distinct.Count() > 0)
                {
                    foreach (var runners in KJMarkets)
                    {
                        MarketBookShow objmarketbookshow = new MarketBookShow();
                        objmarketbookshow.Selection = runners.MarketCatalogueName;
                        objmarketbookshow.SelectionID = "369646";
                        objmarketbookshow.CategoryName = "Line v Markets";
                        objmarketbookshow.MarketbooknameBet = lblMarketName.Content.ToString();
                        objmarketbookshow.RunnerStatusstr = MarketBook.MarketStatusstr;
                        
                        objmarketbookshow.BettingAllowed = runners.BettingAllowed;
                        objmarketbookshow.CurrentMarketBookId = runners.MarketCatalogueID;

                        objmarketbookshow.Laysize0 = "1.02";
                        objmarketbookshow.Layprice0 = "2.02";
                        objmarketbookshow.Backsize0 = "0.98";
                        objmarketbookshow.Backprice0 = "1.98";

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
                backgroundWorkerUpdateFigData.RunWorkerAsync();
            }
        }

        public MarketBookForindianFancy GetBookPositioninKJ(string marketBookID, string selectionID)
        {

            MarketBook objmarketbook = new MarketBook();
            MarketBookForindianFancy objmarketbookin = new MarketBookForindianFancy();
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
                    objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>();
                    RunnerForIndianFancy objRunner1 = new RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbookin.RunnersForindianFancy != null)
                        {
                            RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            RunnerForIndianFancy objRunner = new RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objmarketbookin.RunnersForindianFancy = new List<RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }



                    }
                    RunnerForIndianFancy objRunnerlast = new RunnerForIndianFancy();
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

        public void SetWindowHeight()
        {
            try
            {                
                    double kjygridheight = 0;
                    if (DGVMarketKalijut.Visibility == Visibility.Visible && lstMarketBookRunnerKalijut != null )
                    {
                        if (lstMarketBookRunnerKalijut.Count > 0)
                        {
                            var isshownitems = lstMarketBookRunnerKalijut.Where(item => item.isShow == true).ToList();
                            if (lstMarketBookRunnerKalijut.Count > 0)
                            {
                                kjygridheight = (lstMarketBookRunnerKalijut.Count * 48);
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
                    double newheight = 48 +  kjygridheight ;
                                    
                   this.Height = newheight;
          
            }
            catch (System.Exception ex)
            {
            }
        }
    }
}

