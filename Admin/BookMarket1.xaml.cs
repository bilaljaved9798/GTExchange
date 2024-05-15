using bftradeline.Models;
using ExternalAPI.TO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for BookMarket.xaml
    /// </summary>
    public partial class BookMarket1 : Window
    {
        WindowResizer ob;
        public BookMarket1()
        {
            InitializeComponent();
            init();
        }
        public string marketBookID = "";
        public string eventID = "";
        public string selectionID = "";

        public string marketbookName = "";
        public int UserTypeID = 0;
        public int userID = 0;
        private ObservableCollection<MarketBookShow> _lstMarketBookRunnerKalijut;
        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersToWintheToss;
        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersFancyin;
        private ObservableCollection<MarketBookShow> _lstMarketBookRunnerSFigure;
        private ObservableCollection<MarketBookShow> _lstMarketBookRunnersFigure;
        private ObservableCollection<UserBetsForAdmin> lstCurrentBetsAdmin;
        public BackgroundWorker backgroundWorkerLiabalityandScore;
        public void init()
        {
            try
            {
                ob = new WindowResizer(this);
                this.lstCurrentBetsAdmin = new ObservableCollection<UserBetsForAdmin>();
                DGVMarketIndianFancy.DataContext = lstMarketBookRunnersFancyin;

                backgroundWorkerLiabalityandScore = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                backgroundWorkerLiabalityandScore.DoWork += BackgroundWorkerLiabalityandScore_DoWork;
                backgroundWorkerLiabalityandScore.RunWorkerCompleted += BackgroundWorkerLiabalityandScore_RunWorkerCompleted;


                //timerCountdown.Tick += TimerCountdown_Tick;

            }
            catch (System.Exception ex)
            {
                return;
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
                GetDataForFancy(eventID, marketBookID);
            }
            catch (System.Exception ex)
            {
                // scores = new List<MatchScores>();
            }
        }
        private void BackgroundWorkerLiabalityandScore_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                UpdateLineMarketsDataIN(false);

                if (backgroundWorkerLiabalityandScore != null)
                    backgroundWorkerLiabalityandScore.RunWorkerAsync();
            }
            catch (System.Exception ex)
            {}
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
        public DispatcherTimer tmrUpdateMarket = new DispatcherTimer();
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            tmrUpdateMarket.Stop();
            //    ExternalAPI.TO.MarketBook markettoremove = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == MarketBook.MarketId).First();
            //    LoggedinUserDetail.MarketBooks.Remove(markettoremove);
            //    LoggedinUserDetail.OpenMarkets.Remove("marketwin" + markettoremove.MarketId.Replace(".", ""));
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

        private void TmrUpdateMarket_Tick(object sender, EventArgs e)
        {
            try
            {
                UpdateLineMarketsDataIN(false);
            }
            catch (System.Exception ex)
            {

            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                backgroundWorkerLiabalityandScore.RunWorkerAsync();
                lblMarketName.Content = marketbookName;
                DGVMarketIndianFancy.ItemsSource = lstMarketBookRunnersFancyin;
                tmrUpdateMarket.Tick += TmrUpdateMarket_Tick;
                tmrUpdateMarket.Interval = TimeSpan.FromMilliseconds(200);

                tmrUpdateMarket.Start();

            }
            catch (System.Exception ex)
            {

            }

        }
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {          
                backgroundWorkerLiabalityandScore.CancelAsync();
                backgroundWorkerLiabalityandScore.Dispose();
                backgroundWorkerLiabalityandScore = null;             
                //bgdata = true;
                //data = true;
                //getfancy = true;
                GC.Collect();

            }
            catch (System.Exception ex)
            {
                tmrUpdateMarket.Stop();
            }
            tmrUpdateMarket.Stop();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (DGVMarketIndianFancy.Items.Count > 0)
            //    {
            //        if (sender is Button btn && btn.DataContext is MarketBookShow item)
            //        {
            //            string selection = item.Selection;
            //            int currcellindx = 1;
            //            CategoryName = item.CategoryName;
            //            MarketbooknameBet = item.MarketbooknameBet;
            //            Marketstatusstr = item.Marketstatusstr;
            //            BettingAllowed = item.BettingAllowed;
            //            OpenDate = item.OpenDate;
            //            runnerscount = item.runnerscount;
            //            CurrentMarketBookId = item.CurrentMarketBookId;
            //            if (currcellindx == 1)
            //            {
            //                foreach (Window win in App.Current.Windows)
            //                {
            //                    if (win.Name == "BookPostionForINwin" + CurrentMarketBookId.Replace(".", ""))
            //                    {
            //                        win.Close();
            //                    }
            //                }
            //                BookPostionForIN objbookpostion = new BookPostionForIN();
            //                objbookpostion.selectionID = item.SelectionID;

            //                if (LoggedinUserDetail.GetUserTypeID() == 1)
            //                {
            //                    objbookpostion.CurrentUserbetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList();
            //                }
            //                else
            //                {
            //                    if (LoggedinUserDetail.GetUserTypeID() == 2)
            //                    {
            //                        objbookpostion.CurrentUserbetsAgent = LoggedinUserDetail.CurrentAgentBets;
            //                    }
            //                    else
            //                    {
            //                        objbookpostion.CurrentUserbets = LoggedinUserDetail.CurrentUserBets;
            //                    }
            //                }
            //                objbookpostion.marketBookID = item.CurrentMarketBookId;
            //                objbookpostion.eventID = item.SelectionID;
            //                objbookpostion.marketbookName = MarketbooknameBet + "(" + lblMarketName.Content.ToString() + ")";
            //                objbookpostion.UserTypeID = LoggedinUserDetail.GetUserTypeID();
            //                objbookpostion.userID = LoggedinUserDetail.GetUserID();
            //                objbookpostion.Show();
            //                return;
            //            }
            //            if (item.Backprice0 == "0" || item.Layprice0 == "0")
            //            {
            //                return;
            //            }
            //            if (item.RunnerStatusstr == "SUSPENDED" || item.RunnerStatusstr == "Ball Running")
            //            {
            //                return;
            //            }

            //            if ((currcellindx >= 6 && currcellindx <= 12))
            //            {
            //                if (LoggedinUserDetail.GetUserTypeID() == 3 && LoggedinUserDetail.isInserting == false)
            //                {
            //                    loadedlocation = -1;
            //                    clickedbetsize = -1;
            //                    clickedodd = 0;
            //                    ParentID = 0;
            //                    SelectionID = item.SelectionID;
            //                    Selectionname = item.Selection;
            //                    if (item.Clothnumber != null && item.Clothnumber != "Not")
            //                    {
            //                        Selectionname = item.Clothnumber + "-" + Selectionname;
            //                    }
            //                }
            //            }

            //            if ((currcellindx < 5 && currcellindx > 0))
            //            {
            //                if (Marketstatusstr == "Closed" || Marketstatusstr == "Suspended")
            //                {
            //                    return;
            //                }
            //                if (!MarketBook.MainSportsname.Contains("Racing"))
            //                {
            //                    return;
            //                }
            //                if (item.isSelected == true)
            //                {
            //                    item.isSelected = false;
            //                }

            //                else
            //                {
            //                    item.isSelected = true;
            //                }
            //            }
            //            else
            //            {

            //            }
            //        }
            //    }
            //}
            //catch (System.Exception ex)
            //{

            //}
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        ExternalAPI.TO.GetDataFancy test = new ExternalAPI.TO.GetDataFancy();
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
                test = JsonConvert.DeserializeObject<GetDataFancy>(test1);
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
                if (runners != null)
                {
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
                        objmarketbookshow.Marketstatusstr = "IN-Play";
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
                            objmarketbookshow.Price = "0,1,0,0";
                        }
                        else
                        {
                            if (objmarketbookshow.RunnerStatusstr == "Ball Running")
                            {

                                objmarketbookshow.StatusStr = "Collapsed";
                                objmarketbookshow.JockeyName = "BALLRU";
                                objmarketbookshow.JockeyHeading = "NNING";
                                objmarketbookshow.StallDraw = "Visible";
                                objmarketbookshow.Price = "0,1,0,0";
                            }
                            else
                            {
                                objmarketbookshow.StallDraw = "Collapsed";
                                objmarketbookshow.Laysize0 = item.LaySize1.ToString();
                                objmarketbookshow.Layprice0 = item.LayPrice1.ToString();
                                objmarketbookshow.Backsize0 = item.BackSize1.ToString();
                                objmarketbookshow.Backprice0 = item.BackPrice1.ToString();
                                objmarketbookshow.StatusStr = "Visible";
                                objmarketbookshow.Price = "1,1,0,0";
                                objmarketbookshow.isShow = true;
                            }
                        }

                        objmarketbookshow.CurrentMarketBookId = eventID; //a[0].MarketCatalogueID;

                        objmarketbookshow.OpenDate = DateTime.Now.ToString();

                        lstMarketBookRunnersFancyin.Add(objmarketbookshow);

                        //}
                    }
                }

            }
            catch (System.Exception ex)
            {

            }

            //  return lstMArketbookshow;
        }
        public ExternalAPI.TO.MarketBookForindianFancy GetBookPositioninNew(string selectionID)
        {

            ExternalAPI.TO.MarketBookForindianFancy objmarketbookin = new ExternalAPI.TO.MarketBookForindianFancy();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();

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
                    }
                }

                objmarketbookin.DebitCredit = lstDebitCredit;
                foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                {

                    runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                    runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                }

            }


            return objmarketbookin;
        }
    }
}
