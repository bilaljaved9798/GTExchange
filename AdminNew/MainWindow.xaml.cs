using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using globaltraders.UserServiceReference;
using bftradeline.Models;
using Newtonsoft.Json;

using System.Configuration;

using ExternalAPI.TO;
using System.ComponentModel;
using System.Threading;
using System.Runtime.InteropServices;
using WinInterop = System.Windows.Interop;

using globaltraders.HelperClasses;

using bftradeline.HelperClasses;
using System.Collections.ObjectModel;

using System.Net;
using System.IO;
using bftradeline;
using bftradeline.Models123;

using System.Net.Http;
using RestSharp;
using globaltraders.Service123Reference;

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class TodayRace
    {
        public TodayRace()
        {
            this.TodayRaces = new ObservableCollection<TodayRace>();
        }

        public string Title { get; set; }
        public string MarketBookID { get; set; }
        public ObservableCollection<TodayRace> TodayRaces { get; set; }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                var timer = new System.Timers.Timer();
                timer.Interval = 1000 * 60 * 1;
                //timer.Interval = 1000;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                timer.Start();
                LoggedinUserDetail.MarketBooks = new List<ExternalAPI.TO.MarketBook>();
                LoggedinUserDetail.OpenMarkets = new List<string>();
                BackgroundWorker backgroundworkerloaddata = new BackgroundWorker();
                backgroundWorker = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                backgroundWorkerGetStatus = new BackgroundWorker
                {
                    WorkerReportsProgress = true,
                    WorkerSupportsCancellation = true
                };
                backgroundWorkerLiabality = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                backgroundWorkerGetBets = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                //backgroundWorkerGetStatus.DoWork += backgroundWorkerGetStatus_DoWork;
                //backgroundWorkerGetStatus.RunWorkerCompleted += BackgroundWorkerGetStatus_RunWorkerCompleted;

                backgroundWorker.DoWork += backgroundWorker_DoWork;
                //For the display of operation progress to UI.    

                //After the completation of operation.    
                backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

                backgroundWorkerGetBets.DoWork += BackgroundWorkerGetBets_DoWork;
                backgroundWorkerGetBets.RunWorkerCompleted += BackgroundWorkerGetBets_RunWorkerCompleted;
                backgroundWorkerLiabality.DoWork += BackgroundWorkerLiabality_DoWork;
                backgroundWorkerLiabality.RunWorkerCompleted += BackgroundWorkerLiabality_RunWorkerCompleted;
                //  this.SourceInitialized += new EventHandler(win_SourceInitialized);
            }
            catch (System.Exception ex)
            {

            }
        }
        CancellationToken cancellationToken;

        private void BackgroundWorkerLiabality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (showpendingamountlabel == true)
                {
                    lblShowPaymentPending.Visibility = Visibility.Visible;
                }
                else
                {
                    lblShowPaymentPending.Visibility = Visibility.Collapsed;
                }
                backgroundWorkerLiabality.RunWorkerAsync();
            }
            catch (System.Exception ex)
            {

            }
        }

        private void BackgroundWorkerLiabality_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;

                    return;
                }
                if (isOpeningWindow == false)
                {
                    Thread.Sleep(200);
                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {
                        UpdateUserLiablity();
                        LoggedinUserDetail.RefreshLiabality = false;
                    }

                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {

                    }
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.RefreshLiabality = false;

            }

        }

        private void BackgroundWorkerGetBets_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                backgroundWorkerGetBets.RunWorkerAsync();
            }
            catch (System.Exception ex)
            {

            }
        }
        System.Net.WebClient RESTProxy = new System.Net.WebClient();
        public void GetBetsData()
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    LoggedinUserDetail.CurrentAdminBets = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(LoggedinUserDetail.PasswordForValidate));
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void RESTProxy_DownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            try
            {
                System.IO.Stream stream = new System.IO.MemoryStream(e.Result);
                System.Runtime.Serialization.Json.DataContractJsonSerializer obj = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(string));
                string userbets = obj.ReadObject(stream).ToString();
                List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(userbets);
                LoggedinUserDetail.CurrentAdminBets = lstUserBet;
            }
            catch (System.Exception ex)
            {

            }
        }

        private void ObjUsersServiceCleint_GetUserBetsbyAgentIDCompleted(object sender, GetUserBetsbyAgentIDCompletedEventArgs e)
        {
            objUsersServiceCleint.GetUserBetsbyAgentIDCompleted -= ObjUsersServiceCleint_GetUserBetsbyAgentIDCompleted;
        }

        private void ObjUsersServiceCleint_GetUserbetsbyUserIDCompleted(object sender, GetUserbetsbyUserIDCompletedEventArgs e)
        {
            objUsersServiceCleint.GetUserbetsbyUserIDCompleted -= ObjUsersServiceCleint_GetUserbetsbyUserIDCompleted;

        }

        private void BackgroundWorkerGetBets_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;

                    return;
                }

                Thread.Sleep(100);
                GetBetsData();

            }
            catch (System.Exception ex)
            {

            }
        }
        public void GetusersbyUserType()
        {

            objUsersServiceCleint.GetAllUsersbyUserTypeNewAsync(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.GetUserTypeID(), LoggedinUserDetail.PasswordForValidate);
            objUsersServiceCleint.GetAllUsersbyUserTypeNewCompleted += ObjUsersServiceCleint_GetAllUsersbyUserTypeNewCompleted;

        }

        private void ObjUsersServiceCleint_GetAllUsersbyUserTypeNewCompleted(object sender, GetAllUsersbyUserTypeNewCompletedEventArgs e)
        {
            if (e.Result != "")
            {
                List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(e.Result);

                foreach (UserIDandUserType objuser in lstUsers)
                {
                    //  objuser.UserName = Crypto.Decrypt(objuser.UserName);
                    objuser.UserName = objuser.UserName + " (" + objuser.UserType + ")";
                }
                UserIDandUserType userdefult = new UserIDandUserType();
                userdefult.ID = 0;
                userdefult.UserTypeID = 0;
                userdefult.UserName = "Please Select";
                lstUsers.Insert(0, userdefult);
                LoggedinUserDetail.AllUsers = lstUsers;


            }
        }

        private void ObjUsersServiceCleint_GetAllUsersbyUserTypeCompleted(object sender, GetAllUsersbyUserTypeCompletedEventArgs e)
        {
            if (e.Result != "")
            {
                List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(e.Result);

                foreach (UserIDandUserType objuser in lstUsers)
                {
                    objuser.UserName = Crypto.Decrypt(objuser.UserName);
                    objuser.UserName = objuser.UserName + " (" + objuser.UserType + ")";
                }
                UserIDandUserType userdefult = new UserIDandUserType();
                userdefult.ID = 0;
                userdefult.UserTypeID = 0;
                userdefult.UserName = "Please Select";
                lstUsers.Insert(0, userdefult);
                LoggedinUserDetail.AllUsers = lstUsers;


            }
        }

        //private void BackgroundWorkerGetStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    try
        //    {


        //        if (LoggedinUserDetail.GetUserTypeID() == 1)
        //        {



        //        }
        //        else
        //        {

        //        }
        //        backgroundWorkerGetStatus.RunWorkerAsync();
        //    }
        //    catch (System.Exception ex)
        //    {

        //    }

        //}
        //public void PlayMessage(string msg)
        //{
        //    System.Speech.Synthesis.SpeechSynthesizer synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();
        //    synthesizer.Volume = 100;  // 0...100
        //    synthesizer.Rate = 1;     // -10...10


        //    synthesizer.SelectVoiceByHints(System.Speech.Synthesis.VoiceGender.Neutral, System.Speech.Synthesis.VoiceAge.Adult);

        //    synthesizer.SpeakAsync(msg);
        //}
        //private void backgroundWorkerGetStatus_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        if (LoggedinUserDetail.GetUserTypeID() != 1)
        //        {


        //            BackgroundWorker worker = sender as BackgroundWorker;
        //            if (worker.CancellationPending == true)
        //            {
        //                e.Cancel = true;

        //                return;
        //            }

        //            bool isstatustrue = GetStatus();
        //            if (isstatustrue == false)
        //            {

        //                if (!CheckAccess())
        //                {
        //                    Dispatcher.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
        //                    {

        //                        PlayMessage("Some changes made on server, please restart the application.");
        //                        Xceed.Wpf.Toolkit.MessageBox.Show(this, "Some changes made on server, please restart the application.");
        //                        Application.Current.Shutdown();
        //                    }));
        //                }
        //                else
        //                {

        //                    PlayMessage("Some changes made on server, please restart the application.");
        //                    Xceed.Wpf.Toolkit.MessageBox.Show(this, "Some changes made on server, please restart the application.");
        //                    Application.Current.Shutdown();
        //                }


        //            }
        //            else
        //            {
        //                System.Threading.Thread.Sleep(5000);

        //            }
        //        }
        //        else
        //        {
        //            System.Threading.Thread.Sleep(200);
        //        }
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //    }
        //    catch (System.Exception ex)
        //    {

        //        if (!CheckAccess())
        //        {
        //            Dispatcher.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
        //            {


        //                Xceed.Wpf.Toolkit.MessageBox.Show(this, "Something wrong with internet. Please check your internet.");

        //            }));
        //        }
        //        else
        //        {


        //            Xceed.Wpf.Toolkit.MessageBox.Show(this, "Something wrong with internet. Please check your internet.");

        //        }

        //    }
        //}

        //public bool GetStatus()
        //{
        //    if (LoggedinUserDetail.GetUserID() == 0)
        //    {
        //        return false;
        //    }
        //    UserStatus objUserStatus = JsonConvert.DeserializeObject<UserStatus>(objUsersServiceCleint.GetUserStatus(LoggedinUserDetail.GetUserID()));


        //    if (objUserStatus.isBlocked == true || objUserStatus.isDeleted == true || objUserStatus.Loggedin == false || objUserStatus.CurrentLoggedInID != LoggedinUserDetail.user.CurrentLoggedInID)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
        //int BetPlaceWait = 0;
        // int BetWaitTimerInterval = 0;

        public List<AllMarketsInPlay> lstAllMarkets = new List<AllMarketsInPlay>();
        public BackgroundWorker backgroundWorker;
        public BackgroundWorker backgroundWorkerGetStatus;
        public BackgroundWorker backgroundWorkerGetBets;
        public BackgroundWorker backgroundWorkerLiabality;
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

                if (isOpeningWindow == false)
                {



                    if (LoggedinUserDetail.OpenMarkets.Count > 0)
                    {
                        Thread.Sleep(200);
                        GetAllMarketsOpenedbyUser();
                    }
                    else
                    {
                        Thread.Sleep(200);
                    }
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        public UserServicesClient objUsersServiceCleint = new UserServicesClient();

        private void btnInPlayEvents_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (plusminusInPlay.Content.ToString() == "+")
                {

                    plusminusInPlay.Content = "-";
                    stkpnlInPlayEvents.Visibility = Visibility.Visible;
                    stkpnlInPlayEvents.Children.Clear();
                    bsyindicator.IsBusy = true;
                    objUsersServiceCleint.GetInPlayMatchesAsync(UserIDforLoadMarket);
                    objUsersServiceCleint.GetInPlayMatchesCompleted += ObjUsersServiceCleint_GetInPlayMatchesCompleted;

                }
                else
                {
                    plusminusInPlay.Content = "+";
                    stkpnlInPlayEvents.Visibility = Visibility.Collapsed;
                }

            }
            catch (System.Exception ex)
            {
                bsyindicator.IsBusy = false;
            }
        }

        private void ObjUsersServiceCleint_GetInPlayMatchesCompleted(object sender, GetInPlayMatchesCompletedEventArgs e)
        {
            objUsersServiceCleint.GetInPlayMatchesCompleted -= ObjUsersServiceCleint_GetInPlayMatchesCompleted;
            TreeView inplaytreeview = new TreeView();

            List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(e.Result);

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
                                var lstMarketCatalogues = lstInPlayMatches.Where(item => item.EventID == eventitem.EventID && item.EventName != "Line v Markets").Select(item => new { item.MarketCatalogueID, item.MarketCatalogueName }).Distinct().ToArray();

                                foreach (var marketcatalogueitem in lstMarketCatalogues)
                                {
                                    TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                                    newnodemarketcatalogue.Tag = marketcatalogueitem.MarketCatalogueID;
                                    newnodemarketcatalogue.Header = marketcatalogueitem.MarketCatalogueName;
                                    newnodeevents.Items.Add(newnodemarketcatalogue);
                                    newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;


                                }
                                var linevmaketbyEventID = lstInPlayMatches.Where(item => item.AssociateEventID == eventitem.EventID && item.EventName == "Line v Markets").Select(item => new { item.MarketCatalogueID, item.MarketCatalogueName }).Distinct().ToArray();
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
                // stkpnlInPlayEvents.UpdateLayout();
            }
            else
            {

            }
            bsyindicator.IsBusy = false;
        }
        string marketbookIDselected = "";



        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

           // LoadGridMarket();
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


                    //  if (chkShowinMultipleWindows.IsChecked == true)
                    //{
                    MarketWindow objmarketwindow = new MarketWindow();
                    objmarketwindow.Name = "marketwin" + MarketBookID.Replace(".", "");
                    LoggedinUserDetail.OpenMarkets.Add(objmarketwindow.Name);

                    objmarketwindow.MarketBook = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == MarketBookID).FirstOrDefault();
                    objmarketwindow.MarketBook.isOpenExternally = true;
                    var totcount = LoggedinUserDetail.MarketBooks.Where(item => item.isOpenExternally).ToList();
                    if (totcount.Count > 1)
                    {
                        objmarketwindow.isMaximizedWindow = false;
                        // this.WindowState = WindowState.Normal;
                        if (objmarketwindow.MarketBook.MainSportsname == "Cricket" && objmarketwindow.MarketBook.Runners.Count == 3)
                        {
                            objmarketwindow.Height = 350;
                        }
                        else
                        {
                            objmarketwindow.Height = 350;
                        }
                        //   objmarketwindow.Height = 400;
                        objmarketwindow.Width = 676;
                        objmarketwindow.Left = 680;
                        objmarketwindow.Top = 0;
                    }
                    else
                    {
                        objmarketwindow.Top = 0;
                        objmarketwindow.Left = 0;
                    }


                    objmarketwindow.UserIDforLoadMarket = UserIDforLoadMarket;
                    objmarketwindow.Show();

                }
            }
        }

        public void GetAllMarketBooks()
        {
            IList<bftradeline.wrBF.MarketBook> list;
            try
            {

                List<string> marketIdsNew = LoggedinUserDetail.MarketBooks.Where(item => item.isOpenExternally == true || item.isWinTheTossMarket == true).Select(item => item.MarketId).Distinct().ToList();
                if (marketbookIDselected != "")
                {
                    marketIdsNew.Add(marketbookIDselected);
                }

                if (marketIdsNew.Count == 0)
                {
                    return;
                }
                if (LoggedinUserDetail.GetCricketDataFrom == "Live")
                {


                    string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataL/?marketID=" + string.Join(",", marketIdsNew.Distinct().ToArray());
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RestAPIPath1);
                    request.Method = "GET";
                    request.Proxy = null;
                    request.Timeout = 5000;
                    request.KeepAlive = false;
                    request.ServicePoint.ConnectionLeaseTimeout = 5000;
                    request.ServicePoint.MaxIdleTime = 5000;
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                    List<ExternalAPI.TO.MarketBook> test = new List<ExternalAPI.TO.MarketBook>();
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream dataStream = response.GetResponseStream())
                        {

                            System.Runtime.Serialization.Json.DataContractJsonSerializer obj1 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(List<ExternalAPI.TO.MarketBook>));
                            test = (List<ExternalAPI.TO.MarketBook>)obj1.ReadObject(dataStream);

                            dataStream.Close();
                        }

                    }

                    //  var results1 = objBettingClient.GetAllMarketsBP(marketIdsNew.ToArray());
                    var results = (test);
                    foreach (var item in results)
                    {
                        try
                        {

                            if (item.MarketId != null)
                            {


                                MarketBook currentmarketobject = LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault();

                                currentmarketobject = ConvertJsontoMarketObjectBFLive(item, currentmarketobject.MarketId, currentmarketobject.OrignalOpenDate.Value, currentmarketobject.MarketBookName, currentmarketobject.MainSportsname, false, currentmarketobject.Runners, currentmarketobject.isOpenExternally);
                                LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().Runners = currentmarketobject.Runners;
                                LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().MarketStatusstr = currentmarketobject.MarketStatusstr;
                                LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().OpenDate = currentmarketobject.OpenDate;
                                LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().FavoriteBack = currentmarketobject.FavoriteBack;
                                LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().FavoriteLay = currentmarketobject.FavoriteLay;
                                LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().FavoriteID = currentmarketobject.FavoriteID;
                            }
                        }
                        catch (System.Exception ex)
                        {

                        }
                    }
                    return;
                }
                else
                {


                    if (LoggedinUserDetail.GetCricketDataFrom == "BP")
                    {


                        //var res = objBettingClient.GetAllMarketsBP(string.Join(",", marketIdsNew.Distinct().ToArray()));
                        //var results = JsonConvert.DeserializeObject<List<SampleResponse1>>(res);



                        string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataBP/?marketID=" + string.Join(",", marketIdsNew.Distinct().ToArray());
                        ////var client1 = new RestClient(RestAPIPath1);
                        ////var request1 = new RestRequest(Method.GET);
                        ////// request1.RequestFormat = RestSharp.DataFormat.Json;
                        //////request1.AddUrlSegment("marketID", string.Join(",", marketIdsNew.Distinct().ToArray()));

                        //////request1.JsonSerializer.ContentType = "application/json; charset=utf-8";
                        ////request1.AddHeader("Accept-Encoding", "gzip");

                        ////var response1 = client1.Execute(request1);
                        ////var arr = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JValue>(response1.Content);
                        ////var arr1 = JsonConvert.DeserializeObject<List<SampleResponse1>>(arr.Value.ToString());
                        ////var results = arr1;
                        ///
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RestAPIPath1);
                        request.Method = "GET";
                        request.Proxy = null;
                        request.Timeout = 5000;
                        request.KeepAlive = false;

                        //request.ServicePoint.ConnectionLeaseTimeout = 5000;
                        //request.ServicePoint.MaxIdleTime = 5000;
                        //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:24.0) Gecko/20100101 Firefox/24.0";
                        //request.ReadWriteTimeout = 30000;
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
                            // response.Close();


                        }
                        var results = JsonConvert.DeserializeObject<List<SampleResponse1>>(test);

                        foreach (var item in results)
                        {
                            try
                            {

                                if (item.MarketId != null)
                                {


                                    MarketBook currentmarketobject = LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault();

                                    currentmarketobject = ConvertJsontoMarketObjectBF123(item, currentmarketobject.MarketId, currentmarketobject.OrignalOpenDate.Value, currentmarketobject.MarketBookName, currentmarketobject.MainSportsname, false, currentmarketobject.Runners, currentmarketobject.isOpenExternally, currentmarketobject);



                                    //LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().Runners = currentmarketobject.Runners;
                                    //LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().MarketStatusstr = currentmarketobject.MarketStatusstr;
                                    //LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().OpenDate = currentmarketobject.OpenDate;
                                    //LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().FavoriteBack = currentmarketobject.FavoriteBack;
                                    //LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().FavoriteLay = currentmarketobject.FavoriteLay;
                                    //LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().FavoriteID = currentmarketobject.FavoriteID;
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }

                    }
                    else
                    {
                        string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetMarektDataOther/?marketID=" + string.Join(",", marketIdsNew.Distinct().ToArray());
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
                            response.Close();

                        }

                        // var results = (objBettingClient.GetAllMarketsOthers(marketIdsNew.ToArray()));
                        var results = test;
                        foreach (var item in results)
                        {
                            try
                            {

                                if (item.MarketBookId != null)
                                {


                                    MarketBook currentmarketobject = LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketBookId).FirstOrDefault();
                                    // var index = LoggedinUserDetail.MarketBooks.IndexOf(currentmarketobject);


                                    currentmarketobject = ConvertJsontoMarketObjectBFNewSource(item, currentmarketobject.MarketId, currentmarketobject.OrignalOpenDate.Value, currentmarketobject.MarketBookName, currentmarketobject.MainSportsname, false, currentmarketobject.Runners, currentmarketobject.isOpenExternally, currentmarketobject);
                                    //if (index != -1)
                                    //    LoggedinUserDetail.MarketBooks[index] = currentmarketobject;
                                    //LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketBookId).FirstOrDefault().Runners = currentmarketobject.Runners;
                                    //LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketBookId).FirstOrDefault().MarketStatusstr = currentmarketobject.MarketStatusstr;
                                    //LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketBookId).FirstOrDefault().OpenDate = currentmarketobject.OpenDate;
                                    //LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketBookId).FirstOrDefault().FavoriteBack = currentmarketobject.FavoriteBack;
                                    //LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketBookId).FirstOrDefault().FavoriteLay = currentmarketobject.FavoriteLay;
                                    //LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketBookId).FirstOrDefault().FavoriteID = currentmarketobject.FavoriteID;
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }

                    }
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private void ObjBettingClient_GetAllMarketsBPCompleted(object sender, GetAllMarketsBPCompletedEventArgs e)
        {
            objBettingClient.GetAllMarketsBPCompleted -= ObjBettingClient_GetAllMarketsBPCompleted;
            var results = JsonConvert.DeserializeObject<List<SampleResponse1>>(e.Result);
            foreach (var item in results)
            {
                try
                {

                    if (item.MarketId != null)
                    {


                        MarketBook currentmarketobject = LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault();

                        currentmarketobject = ConvertJsontoMarketObjectBF123(item, currentmarketobject.MarketId, currentmarketobject.OrignalOpenDate.Value, currentmarketobject.MarketBookName, currentmarketobject.MainSportsname, false, currentmarketobject.Runners, currentmarketobject.isOpenExternally, currentmarketobject);
                        LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().Runners = currentmarketobject.Runners;
                        LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().MarketStatusstr = currentmarketobject.MarketStatusstr;
                        LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().OpenDate = currentmarketobject.OpenDate;
                        LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().FavoriteBack = currentmarketobject.FavoriteBack;
                        LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().FavoriteLay = currentmarketobject.FavoriteLay;
                        LoggedinUserDetail.MarketBooks.Where(item1 => item1.MarketId == item.MarketId).FirstOrDefault().FavoriteID = currentmarketobject.FavoriteID;
                    }
                }
                catch (System.Exception ex)
                {

                }
            }
        }

        public void GetAllMarketsOpenedbyUser()
        {
            try
            {

                if (isOpeningWindow == false)
                {
                    if (LoggedinUserDetail.OpenMarkets.Count > 0)
                    {
                        GetAllMarketBooks();


                    }
                }

            }
            catch (System.Exception ex)
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
        public MarketBook ConvertJsontoMarketObjectBFLive(ExternalAPI.TO.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, List<ExternalAPI.TO.Runner> OldRunners, bool isopenexternally)
        {


            try
            {


                if (1 == 1)
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
                    marketbook.isOpenExternally = isopenexternally;
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
                        runner.Handicap = runneritem.Handicap;
                        runner.StatusStr = runneritem.Status.ToString();
                        runner.SelectionId = runneritem.SelectionId.ToString();
                        runner.LastPriceTraded = runneritem.LastPriceTraded;
                        try
                        {
                            var runnermarketitem = OldRunners.Where(item => item.SelectionId == runneritem.SelectionId.ToString()).First();
                            runner.RunnerName = runnermarketitem.RunnerName;

                            runner.JockeyName = runnermarketitem.JockeyName;
                            runner.WearingURL = runnermarketitem.WearingURL;
                            runner.WearingDesc = runnermarketitem.WearingDesc;
                            runner.Clothnumber = runnermarketitem.Clothnumber;
                            runner.StallDraw = runnermarketitem.StallDraw;
                        }
                        catch (System.Exception ex)
                        {

                        }

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

                    double lastback = 0;
                    double lastbackSize = 0;
                    double lastLaySize = 0;
                    double lastlay = 0;

                    if (marketbook.MarketStatusstr != "SUSPENDED")
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
                        if (marketbook.MarketStatusstr == "CLOSED")
                        {
                            var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER").ToList();
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
                    if (Convert.ToDouble(marketbook.FavoriteBack) < 0)
                    {
                        marketbook.FavoriteBack = "";
                        marketbook.FavoriteBackSize = "";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteLay) < 0)
                    {
                        marketbook.FavoriteLay = "";
                        marketbook.FavoriteLaySize = "";
                    }
                    return marketbook;

                }
                else
                {
                    return new MarketBook();
                }
            }
            catch (System.Exception ex)
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBF123(SampleResponse1 BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, List<ExternalAPI.TO.Runner> OldRunners, bool isopenexternally, MarketBook marketbook)
        {


            try
            {


                if (1 == 1)
                {


                    marketbook.MarketId = BFMarketbook.MarketId;
                    marketbook.SheetName = "";
                    marketbook.IsMarketDataDelayed = BFMarketbook.IsMarketDataDelayed;
                    marketbook.PoundRate = LoggedinUserDetail.PoundRate;
                    marketbook.NumberOfWinners = BFMarketbook.NumberOfWinners;
                    marketbook.MarketBookName = sheetname;
                    marketbook.MainSportsname = MainSportsCategory;
                    marketbook.OrignalOpenDate = marketopendate;
                    marketbook.isOpenExternally = isopenexternally;
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
                        runner.Handicap = runneritem.Handicap;
                        runner.StatusStr = runneritem.Status.ToString();
                        runner.SelectionId = runneritem.SelectionId.ToString();
                        runner.LastPriceTraded = runneritem.LastPriceTraded;
                        try
                        {
                            var runnermarketitem = OldRunners.Where(item => item.SelectionId == runneritem.SelectionId.ToString()).First();
                            runner.RunnerName = runnermarketitem.RunnerName;

                            runner.JockeyName = runnermarketitem.JockeyName;
                            runner.WearingURL = runnermarketitem.WearingURL;
                            runner.WearingDesc = runnermarketitem.WearingDesc;
                            runner.Clothnumber = runnermarketitem.Clothnumber;
                            runner.StallDraw = runnermarketitem.StallDraw;
                        }
                        catch (System.Exception ex)
                        {

                        }

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

                    double lastback = 0;
                    double lastbackSize = 0;
                    double lastLaySize = 0;
                    double lastlay = 0;

                    if (marketbook.MarketStatusstr != "SUSPENDED")
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
                        if (marketbook.MarketStatusstr == "CLOSED")
                        {
                            var resultsfav = marketbook.Runners.Where(item => item.StatusStr == "WINNER").ToList();
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
                    if (Convert.ToDouble(marketbook.FavoriteBack) < 0)
                    {
                        marketbook.FavoriteBack = "";
                        marketbook.FavoriteBackSize = "";
                    }
                    if (Convert.ToDouble(marketbook.FavoriteLay) < 0)
                    {
                        marketbook.FavoriteLay = "";
                        marketbook.FavoriteLaySize = "";
                    }
                    return marketbook;

                }
                else
                {
                    return new MarketBook();
                }
            }
            catch (System.Exception ex)
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public Service123Client objBettingClient = new Service123Client();

        public ExternalAPI.TO.MarketBook GetCurrentMarketBook(string marketid, string sheetname, string MainSportsCategory, DateTime marketopendate, bool BettingAllowed, List<ExternalAPI.TO.Runner> OldRunners)
        {
            try
            {
                string[] marketIds = new string[]
                      {
                    marketid
                      };
                if (MainSportsCategory == "Cricket" && sheetname.Contains("Match Odds") && LoggedinUserDetail.GetCricketDataFrom == "Live")
                {
                    var marketbook = objBettingClient.GetMarketDatabyID(marketIds, sheetname, marketopendate, MainSportsCategory, LoggedinUserDetail.PasswordForValidate);
                    if (marketbook.Count() > 0)
                    {
                        return marketbook[0];
                    }
                    else
                    {
                        return new ExternalAPI.TO.MarketBook();
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetCricketDataFrom == "BP")
                    {
                        var marketbook = objBettingClient.GetMarketDatabyID(marketIds, sheetname, marketopendate, MainSportsCategory, LoggedinUserDetail.PasswordForValidate);
                        if (marketbook.Count() > 0)
                        {
                            return marketbook[0];
                        }
                        else
                        {
                            return new ExternalAPI.TO.MarketBook();
                        }
                    }
                    else
                    {
                        var marketbook = objBettingClient.GetMarketDatabyID(marketIds, sheetname, marketopendate, MainSportsCategory, LoggedinUserDetail.PasswordForValidate);
                        if (marketbook.Count() > 0)
                        {
                            return marketbook[0];
                        }
                        else
                        {
                            return new ExternalAPI.TO.MarketBook();
                        }


                    }
                }
            }
            catch (System.Exception ex)
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBF(bftradeline.wrBF.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, List<ExternalAPI.TO.Runner> OldRunners)
        {


            try
            {


                if (1 == 1)
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
                        if (OldRunners.Count > 0)
                        {
                            var runnermarketitem = OldRunners.Where(item2 => item2.SelectionId == runneritem.SelectionId.ToString()).FirstOrDefault();
                            runner.RunnerName = runnermarketitem.RunnerName;
                            runner.JockeyName = runnermarketitem.JockeyName;
                            runner.WearingURL = runnermarketitem.WearingURL;
                            runner.WearingDesc = runnermarketitem.WearingDesc;
                            runner.Clothnumber = runnermarketitem.Clothnumber;
                            runner.StallDraw = runnermarketitem.StallDraw;
                        }

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

                    double lastback = 0;
                    double lastbackSize = 0;
                    double lastLaySize = 0;
                    double lastlay = 0;

                    if (marketbook.MarketStatusstr != "SUSPENDED")
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
                        if (marketbook.MarketStatusstr == "CLOSED")
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
                    return marketbook;

                }
                else
                {
                    return new MarketBook();
                }
            }
            catch (System.Exception ex)
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBFNewSource(ExternalAPI.TO.MarketBookString BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, List<ExternalAPI.TO.Runner> OldRunners, bool isopenexternally, MarketBook marketbook)
        {



            if (1 == 1)
            {
                //var marketbook = new MarketBook();
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
                marketbook.isOpenExternally = isopenexternally;
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
                    if (newres[i] != "Æ  Æ" && newres[i] != "Æ  Æ UnitedBet Æ 1 Æ 1 Æ" && newres[i] != "Æ  Æ UnitedBet Æ 2 Æ 2 Æ")
                    {
                        string[] runnerdetails = newres[i].Split(new string[] { "|" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray();
                        string[] runnerinfo = runnerdetails[0].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).ToArray(); ;
                        string[] runnerbackdata = runnerdetails[1].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                        string[] runnerlaydata = runnerdetails[2].Split(new string[] { "~" }, StringSplitOptions.None).Select(tag => tag.Trim()).Where(tag => !string.IsNullOrEmpty(tag)).ToArray();
                        var runner = new ExternalAPI.TO.Runner();
                        if (OldRunners.Count > 0)
                        {
                            var runnermarketitem = OldRunners.Where(item2 => item2.SelectionId == runnerinfo[0].Trim().ToString()).FirstOrDefault();
                            runner.RunnerName = runnermarketitem.RunnerName;
                            runner.JockeyName = runnermarketitem.JockeyName;
                            runner.WearingURL = runnermarketitem.WearingURL;
                            runner.WearingDesc = runnermarketitem.WearingDesc;
                            runner.Clothnumber = runnermarketitem.Clothnumber;
                            runner.StallDraw = runnermarketitem.StallDraw;
                        }
                        try
                        {
                            runner.Handicap = Convert.ToDouble(runnerinfo[4].Trim());
                        }
                        catch (System.Exception)
                        {
                            runner.Handicap = 0;

                        }

                        runner.StatusStr = runnerinfo[6].Trim();
                        runner.SelectionId = runnerinfo[0].Trim().ToString();

                        //runner.LastPriceTraded = runnerinfo[3];
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
                                                pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
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
                                                pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
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
                                        pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
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
                                                pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerbackdata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
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
                                                pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
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
                                        pricesize.Size = Convert.ToInt64(Convert.ToDouble(runnerlaydata[j + 1]) * Convert.ToDouble(marketbook.PoundRate));
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
                double lastback = 0;
                double lastbackSize = 0;
                double lastLaySize = 0;
                double lastlay = 0;

                if (marketbook.MarketStatusstr != "SUSPENDED")
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
                    if (marketbook.MarketStatusstr == "CLOSED")
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
                return marketbook;

            }
            else
            {
                return new MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBFOther(bftradeline.wrBF.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, List<ExternalAPI.TO.Runner> OldRunners, bool isopenexternally)
        {


            try
            {


                if (1 == 1)
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
                    marketbook.Version = BFMarketbook.Version;
                    marketbook.isOpenExternally = isopenexternally;
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
                        if (OldRunners.Count > 0)
                        {
                            var runnermarketitem = OldRunners.Where(item2 => item2.SelectionId == runneritem.SelectionId.ToString()).FirstOrDefault();
                            runner.RunnerName = runnermarketitem.RunnerName;
                            runner.JockeyName = runnermarketitem.JockeyName;
                            runner.WearingURL = runnermarketitem.WearingURL;
                            runner.WearingDesc = runnermarketitem.WearingDesc;
                            runner.Clothnumber = runnermarketitem.Clothnumber;
                            runner.StallDraw = runnermarketitem.StallDraw;
                        }

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

                    double lastback = 0;
                    double lastbackSize = 0;
                    double lastLaySize = 0;
                    double lastlay = 0;

                    if (marketbook.MarketStatusstr != "SUSPENDED")
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
                        if (marketbook.MarketStatusstr == "CLOSED")
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
                    return marketbook;

                }
                else
                {
                    return new MarketBook();
                }
            }
            catch (System.Exception ex)
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBFFancy(bftradeline.wrBF.MarketBook BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory)
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
        public void SetBetSlipKeys()
        {
            try
            {


                BetSlipKeys objBetSlipKeys = LoggedinUserDetail.objBetSlipKeys;



            }
            catch (System.Exception ex)
            {

            }

        }
        public bool isOpeningWindow = false;
        public void MarketBook(string ID)
        {

            try
            {

                LoggedinUserDetail.CheckifUserLogin();

                if (ID != "" && LoggedinUserDetail.GetUserTypeID() == 1)
                {

                    objUsersServiceCleint.SetMarketBookOpenbyUSerAsync(73, ID);
                }
                var marketbooks = new List<ExternalAPI.TO.MarketBook>();

                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    if (ID == "")
                    {

                    }
                    else
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
                            isOpeningWindow = false;
                            bsyindicator.IsBusy = false;
                        }
                        else
                        {

                            isOpeningWindow = true;

                            objUsersServiceCleint.SetMarketBookOpenbyUSerandGetAsync(UserIDforLoadMarket, ID);
                            objUsersServiceCleint.SetMarketBookOpenbyUSerandGetCompleted += ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGetCompleted;
                            
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
                                    var marketbook = GetCurrentMarketBook(item.ID, item.Name, item.EventTypeName, item.EventOpenDate, false, new List<ExternalAPI.TO.Runner>());
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
                                var marketbook = GetCurrentMarketBook(newmarkettobeopened.ID, newmarkettobeopened.Name, newmarkettobeopened.EventTypeName, newmarkettobeopened.EventOpenDate, newmarkettobeopened.BettingAllowed, new List<ExternalAPI.TO.Runner>());
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



                //}));
            }
            catch (System.Exception ex)
            {
                isOpeningWindow = false;
                bsyindicator.IsBusy = false;
            }
        }
        public void intializemarketdata(string result)
        {
            try
            {
                var results = JsonConvert.DeserializeObject<List<MarketCatalogueandSelectionNames>>(result);
                if (results.Count == 0)
                {
                    bsyindicator.IsBusy = false;
                    isOpeningWindow = false;
                    return;
                }
                var newmarkettobeopened = results;
                List<string> lstIDs = new List<string>();

                var newmarketbook = new ExternalAPI.TO.MarketBook();

                ExternalAPI.TO.MarketBook marketbook1;
                marketbook1 = GetCurrentMarketBook(newmarkettobeopened[0].ID, newmarkettobeopened[0].Name, newmarkettobeopened[0].EventTypeName, newmarkettobeopened[0].EventOpenDate, newmarkettobeopened[0].BettingAllowed, new List<ExternalAPI.TO.Runner>());

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
                        if (objmarketbook.MarketStatusstr == "Closed")
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show(this, "This market is already closed.");
                            isOpeningWindow = false;
                            bsyindicator.IsBusy = false;
                            return;
                        }
                        LoggedinUserDetail.MarketBooks.Add(objmarketbook);
                        LoadGridMarket(objmarketbook.MarketId);





                    }
                    isOpeningWindow = false;

                }
                else
                {
                    isOpeningWindow = false;
                }
                bsyindicator.IsBusy = false;
            }
            catch (System.Exception ex)
            {
                isOpeningWindow = false;
                bsyindicator.IsBusy = false;
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
                    bsyindicator.IsBusy = false;
                    isOpeningWindow = false;
                    return;
                }
                var newmarkettobeopened = results;
                List<string> lstIDs = new List<string>();

                var newmarketbook = new ExternalAPI.TO.MarketBook();

                ExternalAPI.TO.MarketBook marketbook1;
                marketbook1 = GetCurrentMarketBook(newmarkettobeopened[0].ID, newmarkettobeopened[0].Name, newmarkettobeopened[0].EventTypeName, newmarkettobeopened[0].EventOpenDate, newmarkettobeopened[0].BettingAllowed, new List<ExternalAPI.TO.Runner>());

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
                        if (objmarketbook.MarketStatusstr == "Closed")
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show(this, "This market is already closed.");
                            isOpeningWindow = false;
                            bsyindicator.IsBusy = false;
                            return;
                        }
                        LoggedinUserDetail.MarketBooks.Add(objmarketbook);
                        LoadGridMarket(objmarketbook.MarketId);

                    }
                    isOpeningWindow = false;

                }
                else
                {
                    isOpeningWindow = false;
                }
                bsyindicator.IsBusy = false;
            }
            catch (System.Exception ex)
            {
                isOpeningWindow = false;
                bsyindicator.IsBusy = false;
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
                        string MarketBookID = objSender.Tag.ToString();
                        bsyindicator.IsBusy = true;
                        MarketBook(MarketBookID);


                    }
                }


            }
            catch (System.Exception ex)
            {
                bsyindicator.IsBusy = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
                        stkpnlLoaderHorse.Visibility = Visibility.Visible;
                        objUsersServiceCleint.GetTodayHorseRacingAsync(UserIDforLoadMarket, "7");
                        objUsersServiceCleint.GetTodayHorseRacingCompleted += ObjUsersServiceCleint_GetTodayHorseRacingCompleted;
                    }
                    else
                    {
                        TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                        newnodemarketcatalogue.Tag = "0";
                        newnodemarketcatalogue.Header = "Race not Allowed";
                        inplaytreeview.Items.Add(newnodemarketcatalogue);
                    }


                    //  stkpnlHorseRace.UpdateLayout();
                }

            }
            catch (System.Exception ex)
            {
                stkpnlLoaderHorse.Visibility = Visibility.Collapsed;
                bsyindicator.IsBusy = false;
            }
        }

        private void ObjUsersServiceCleint_GetTodayHorseRacingCompleted(object sender, GetTodayHorseRacingCompletedEventArgs e)
        {
            try
            {


                objUsersServiceCleint.GetTodayHorseRacingCompleted -= ObjUsersServiceCleint_GetTodayHorseRacingCompleted;
                TreeView inplaytreeview = new TreeView();
                List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(e.Result);

                lstTodayHorseRacing = lstTodayHorseRacing.Take(50).ToList();
                ObservableCollection<TodayRace> lstRaces = new ObservableCollection<TodayRace>();
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
                        // root.Title = eventitem.TodayHorseRace;
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
                            TodayRace subitem = new TodayRace();
                            subitem.MarketBookID = item.MarketCatalogueID;
                            subitem.Title = item.MarketCatalogueName;
                            //  root.TodayRaces.Add(subitem);
                            TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                            newnodemarketcatalogue.Tag = item.MarketCatalogueID;
                            newnodemarketcatalogue.Header = item.MarketCatalogueName;
                            newnodeevent.Items.Add(newnodemarketcatalogue);
                            newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;
                        }

                    }
                    inplaytreeview.Height = 300;
                    //  inplaytreeview.FontSize = 16;
                    stkpnlHorseRace.Children.Add(inplaytreeview);
                    // horsetreeview.ItemsSource = lstRaces;
                }


                else
                {
                    TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                    newnodemarketcatalogue.Tag = "0";
                    newnodemarketcatalogue.Header = "No Race found";
                    inplaytreeview.Items.Add(newnodemarketcatalogue);
                }
                stkpnlLoaderHorse.Visibility = Visibility.Collapsed;
            }
            catch (System.Exception ex)
            {
                stkpnlLoaderHorse.Visibility = Visibility.Collapsed;
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
                    bsyindicator.IsBusy = true;
                    objUsersServiceCleint.GetAllMatchesAsync(UserIDforLoadMarket);
                    objUsersServiceCleint.GetAllMatchesCompleted += ObjUsersServiceCleint_GetAllMatchesCompleted;

                }
                else
                {
                    plusminusAllSports.Content = "+";
                    stkpnlSports.Visibility = Visibility.Collapsed;
                }

            }
            catch (System.Exception ex)
            {
                bsyindicator.IsBusy = false;
            }
        }

        private void ObjUsersServiceCleint_GetAllMatchesCompleted(object sender, GetAllMatchesCompletedEventArgs e)
        {
            objUsersServiceCleint.GetAllMatchesCompleted -= ObjUsersServiceCleint_GetAllMatchesCompleted;
            List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(e.Result);
            TreeView inplaytreeview = new TreeView();
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
                                var lstMarketCatalogues = lstInPlayMatches.Where(item => item.EventID == eventitem.EventID && item.EventName != "Line v Markets").Select(item => new { item.MarketCatalogueID, item.MarketCatalogueName }).Distinct().ToArray();

                                foreach (var marketcatalogueitem in lstMarketCatalogues)
                                {
                                    TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                                    newnodemarketcatalogue.Tag = marketcatalogueitem.MarketCatalogueID;
                                    newnodemarketcatalogue.Header = marketcatalogueitem.MarketCatalogueName;
                                    newnodeevents.Items.Add(newnodemarketcatalogue);
                                    newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;


                                }
                                var linevmaketbyEventID = lstInPlayMatches.Where(item => item.AssociateEventID == eventitem.EventID && item.EventName == "Line v Markets").Select(item => new { item.MarketCatalogueID, item.MarketCatalogueName }).Distinct().ToArray();
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
            bsyindicator.IsBusy = false;
        }


        public static string strWsMatch = ConfigurationManager.AppSettings["URLForData"];

        public void GetAllMarketsInPlay()
        {
            try
            {

                objUsersServiceCleint.GetInPlayMatcheswithRunnersAsync(UserIDforLoadMarket);
                objUsersServiceCleint.GetInPlayMatcheswithRunnersCompleted += ObjUsersServiceCleint_GetInPlayMatcheswithRunnersCompleted;

            }
            catch (System.Exception ex)
            {

            }
        }

        private void ObjUsersServiceCleint_GetInPlayMatcheswithRunnersCompleted(object sender, GetInPlayMatcheswithRunnersCompletedEventArgs e)
        {
            objUsersServiceCleint.GetInPlayMatcheswithRunnersCompleted -= ObjUsersServiceCleint_GetInPlayMatcheswithRunnersCompleted;
            List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(e.Result);
            List<string> lstIds = lstInPlayMatches.Where(item => item.EventTypeName == "Cricket").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList();
            lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Soccer" && item.EventOpenDate.Value.AddHours(5) >= DateTime.Now).Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
            lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Tennis" && item.EventOpenDate.Value.AddHours(5) >= DateTime.Now).Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());

            List<AllMarketsInPlay> lstGridMarkets = new List<AllMarketsInPlay>();


            foreach (var item in lstIds)
            {
                try
                {



                    InPlayMatches objMarketLocal = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).FirstOrDefault();
                    AllMarketsInPlay objGridMarket = new AllMarketsInPlay();
                    objGridMarket.MarketBookID = objMarketLocal.MarketCatalogueID;
                    objGridMarket.MarketBookName = objMarketLocal.MarketCatalogueName;
                    objGridMarket.MarketStartTime = objMarketLocal.EventOpenDate.Value.AddHours(5).ToString("dd-MM-yyyy hh:mm tt");
                    //  objGridMarket.MarketStatus = item.Status;
                    objGridMarket.MarketStatus = "";
                    objGridMarket.ImagePath = objMarketLocal.EventTypeName + ".png";
                    objGridMarket.CountryCode = objMarketLocal.CountryCode == null ? @"\Resources\0.png" : @"\Resources\" + objMarketLocal.CountryCode + ".gif";

                    List<InPlayMatches> lstRunnersID = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).ToList();
                    try
                    {
                        objGridMarket.Runner1 = lstRunnersID[0].SelectionName;
                        objGridMarket.Runner2 = lstRunnersID[1].SelectionName;

                        if (lstRunnersID.Count == 3)
                        {
                            objGridMarket.Runner3 = lstRunnersID[2].SelectionName;

                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                    lstGridMarkets.Add(objGridMarket);

                }
                catch (System.Exception ex)
                {
                    bsyindicator.IsBusy = false;
                }
            }
            //DGVInPlaymarketsall.ItemsSource = lstGridMarkets;
            bsyindicator.IsBusy = false;
        }


        public void GetLiabalityofAllMarkets()
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            try
            {
                List<LiabalitybyMarket> lstLibalitybymrakets = new List<LiabalitybyMarket>();

                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    List<UserBetsForAdmin> oddmarkets = LoggedinUserDetail.CurrentAdminBets.Where(x => x.location != "9").ToList();
                    List<UserBetsForAdmin> fancybet = LoggedinUserDetail.CurrentAdminBets.Where(x => x.location == "9").ToList();
                    lstLibalitybymrakets = objUserBets.GetLiabalityofAdminbyMarkets(oddmarkets);
                    lstLibalitybymrakets.AddRange(objUserBets.GetLiabalityofAdminbyMarketsFancy(fancybet));
                    DGVLiabalites.ItemsSource = lstLibalitybymrakets;


                }


            }
            catch (System.Exception ex)
            {

            }
        }





        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                //
                this.Width = System.Windows.SystemParameters.WorkArea.Width;
                this.Height = System.Windows.SystemParameters.WorkArea.Height;
                this.Left = 0;
                this.Top = 0;

                //
                double containerheight = this.Height;
                double containerwidth = this.Width;
                pnlEvents.Height = containerheight;
                pnlEventsBorder.Height = containerheight;

            }
            catch (System.Exception ex)
            {

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(this, "Are you sure to exit from this application?", "Confirm Quit", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {


                }
                catch (System.Exception ex)
                {

                }
                Environment.Exit(0);
                //  Application.Current.Shutdown();
            }

        }
        public void SetURLsData()
        {
            LoggedinUserDetail.URLsData = JsonConvert.DeserializeObject<List<SP_URLsData_GetAllData_Result>>(objUsersServiceCleint.GetURLsData());

            LoggedinUserDetail.SecurityCode = LoggedinUserDetail.URLsData.FirstOrDefault().Scd;
            LoggedinUserDetail.GetCricketDataFrom = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().GetDataFrom;
            if (LoggedinUserDetail.GetCricketDataFrom == "Live")
            {

            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RESTProxy.DownloadDataCompleted += RESTProxy_DownloadDataCompleted;

            SetURLsData();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLoadMarket = 73;
                var tabitem = tbctrlLiabalities.Items[1] as TabItem;
                tabitem.Visibility = Visibility.Visible;
                GetusersbyUserType();

            }
            else
            {
                UserIDforLoadMarket = LoggedinUserDetail.GetUserID();
                //btnAccountRece.Visibility = Visibility.Collapsed;
                objUsersServiceCleint.GetAllowedMarketsbyUserIDAsync(Convert.ToInt32(LoggedinUserDetail.GetUserID()));
                objUsersServiceCleint.GetAllowedMarketsbyUserIDCompleted += ObjUsersServiceCleint_GetAllowedMarketsbyUserIDCompleted;
            }


            GetAllMarketsInPlay();






            LoggedinUserDetail.user.CurrentLoggedInID = LoggedinUserDetail.user.CurrentLoggedInID + 1;
            objUsersServiceCleint.UpdateCurrentLoggedInIDbyUserIDAsync(LoggedinUserDetail.user.ID);
            objUsersServiceCleint.GetCurrentBalancebyUserAsync(LoggedinUserDetail.user.ID, LoggedinUserDetail.PasswordForValidate);
            objUsersServiceCleint.GetCurrentBalancebyUserCompleted += ObjUsersServiceCleint_GetCurrentBalancebyUserCompleted;
            objUsersServiceCleint.GetStartingBalanceAsync(LoggedinUserDetail.user.ID, LoggedinUserDetail.PasswordForValidate);
            objUsersServiceCleint.GetStartingBalanceCompleted += ObjUsersServiceCleint_GetStartingBalanceCompleted;


            GetBetSlipKeys();
            objUsersServiceCleint.GetMarketRulesAsync();
            objUsersServiceCleint.GetMarketRulesCompleted += ObjUsersServiceCleint_GetMarketRulesCompleted;
            GetLiabalityofAllMarkets();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                LoggedinUserDetail.isTVShown = true;
                objUsersServiceCleint.GetLiveTVChanelsAsync(LoggedinUserDetail.PasswordForValidate);
                objUsersServiceCleint.GetLiveTVChanelsCompleted += ObjUsersServiceCleint_GetLiveTVChanelsCompleted;
                AdminBetsWindow objWind = new AdminBetsWindow();
                objWind.Top = 0;
                objWind.Left = this.Width - objWind.Width;
                objWind.Show();


            }
            else
            {
                LoggedinUserDetail.isTVShown = objUsersServiceCleint.GetShowTV(LoggedinUserDetail.GetUserID());
                if (LoggedinUserDetail.isTVShown == true)
                {
                    objUsersServiceCleint.GetLiveTVChanelsAsync(LoggedinUserDetail.PasswordForValidate);
                    objUsersServiceCleint.GetLiveTVChanelsCompleted += ObjUsersServiceCleint_GetLiveTVChanelsCompleted;

                }

            }

            backgroundWorker.RunWorkerAsync();
            //
            backgroundWorkerGetBets.RunWorkerAsync();

            bsyindicator.IsBusy = false;

        }

        private void ObjUsersServiceCleint_GetLiveTVChanelsCompleted(object sender, GetLiveTVChanelsCompletedEventArgs e)
        {
            List<LiveTVChannels> lstTVChannels=new List<LiveTVChannels>(); //= JsonConvert.DeserializeObject<List<LiveTVChannels>>(e.Result);
            LiveTVChannels c1 = new LiveTVChannels();
            c1.ID = 1;
            c1.ChanelName = "TV 1";
            c1.ChanelURL = "https://livemediasystem.com/bf/c.php?ch=c3&user=zafar11";
            lstTVChannels.Add(c1);
            LiveTVChannels c2 = new LiveTVChannels();
            c2.ID = 2;
            c2.ChanelName = "TV 2";
            c2.ChanelURL = "https://livemediasystem.com/bf/c.php?ch=c2&user=zafar11";
            lstTVChannels.Add(c2);
            LiveTVChannels c3 = new LiveTVChannels();
            c3.ID = 3;
            c3.ChanelName = "TV 3";
            c3.ChanelURL = "https://livemediasystem.com/bf/c.php?ch=c1&user=zafar11";
            lstTVChannels.Add(c3);

            LoggedinUserDetail.TvChannels = lstTVChannels;
        }

        private void ObjUsersServiceCleint_GetMarketRulesCompleted(object sender, GetMarketRulesCompletedEventArgs e)
        {
            LoggedinUserDetail.MarketRulesAll = JsonConvert.DeserializeObject<List<MarketRules>>(e.Result);
        }

        private void ObjUsersServiceCleint_GetStartingBalanceCompleted(object sender, GetStartingBalanceCompletedEventArgs e)
        {

        }

        private void ObjUsersServiceCleint_GetCurrentBalancebyUserCompleted(object sender, GetCurrentBalancebyUserCompletedEventArgs e)
        {
            Decimal CurrentAccountBalance = Convert.ToDecimal(e.Result);



        }

        private void ObjUsersServiceCleint_GetAllowedMarketsbyUserIDCompleted(object sender, GetAllowedMarketsbyUserIDCompletedEventArgs e)
        {
            LoggedinUserDetail.AllowedMarketsForUser = JsonConvert.DeserializeObject<AllowedMarkets>(e.Result);
        }


        public void GetBetSlipKeys()
        {
            try
            {


                objUsersServiceCleint.GetBetSlipKeysAsync(LoggedinUserDetail.GetUserID());
                objUsersServiceCleint.GetBetSlipKeysCompleted += ObjUsersServiceCleint_GetBetSlipKeysCompleted;


            }
            catch (System.Exception ex)
            {

            }

        }

        private void ObjUsersServiceCleint_GetBetSlipKeysCompleted(object sender, GetBetSlipKeysCompletedEventArgs e)
        {
            LoggedinUserDetail.objBetSlipKeys = JsonConvert.DeserializeObject<HelperClasses.BetSlipKeys>(e.Result);
            SetBetSlipKeys();
        }

        public List<MatchScores> scores = new List<MatchScores>();
        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
        public void UpdateUserLiablity()
        {
            try
            {


                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {

                    LoggedinUserDetail.TotalLiabality = objUserBets.GetLiabalityofAdmin(LoggedinUserDetail.CurrentAdminBets.ToList());

                }

                string accountbalance = objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.user.ID, LoggedinUserDetail.PasswordForValidate);
                LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(accountbalance);
                LoggedinUserDetail.CurrentAvailableBalance = LoggedinUserDetail.CurrentAccountBalance + LoggedinUserDetail.TotalLiabality;

                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    UserServiceReference.SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                    LoggedinUserDetail.NetBalance = objUsersServiceCleint.GetProfitorLossbyUserID(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), false, LoggedinUserDetail.PasswordForValidate);
                }

                try
                {



                }
                catch (System.Exception ex)
                {
                    scores = new List<MatchScores>();
                }
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    //Getamountreceivablebydate(DateTime.Now);
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        public bool showpendingamountlabel = false;
        public void Getamountreceivablebydate(DateTime currdate)
        {
            var results = JsonConvert.DeserializeObject<List<AmountReceivables>>(objUsersServiceCleint.GetAllPendingAmountsbyDate(currdate));
            if (results != null)
            {

                if (results.Count > 0)
                {
                    var pendingresults = results.Where(item => item.Status == "Pending").ToList();
                    if (pendingresults.Count > 0)
                    {
                        showpendingamountlabel = true;
                    }
                    else
                    {
                        showpendingamountlabel = false;
                    }
                }
                else
                {
                    showpendingamountlabel = false;
                }
            }
            else
            {
                showpendingamountlabel = false;
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
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
                        bsyindicator.IsBusy = true;
                        objUsersServiceCleint.GetTodayHorseRacingAsync(UserIDforLoadMarket, "4339");
                        objUsersServiceCleint.GetTodayHorseRacingCompleted += ObjUsersServiceCleint_GetTodayHorseRacingCompleted1;

                    }
                    else
                    {
                        TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                        newnodemarketcatalogue.Tag = "0";
                        newnodemarketcatalogue.Header = "Race not Allowed";
                        inplaytreeview.Items.Add(newnodemarketcatalogue);
                        bsyindicator.IsBusy = false;
                    }


                }

            }
            catch (System.Exception ex)
            {
                bsyindicator.IsBusy = false;
            }

        }

        private void ObjUsersServiceCleint_GetTodayHorseRacingCompleted1(object sender, GetTodayHorseRacingCompletedEventArgs e)
        {
            objUsersServiceCleint.GetTodayHorseRacingCompleted -= ObjUsersServiceCleint_GetTodayHorseRacingCompleted1;
            TreeView inplaytreeview = new TreeView();
            List<TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<TodayHorseRacing>>(e.Result);
            lstTodayHorseRacing = lstTodayHorseRacing.Take(50).ToList();


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
                inplaytreeview.Height = 300;
                stkpnlGreyHoundRace.Children.Add(inplaytreeview);
                stkpnlGreyHoundRace.UpdateLayout();
            }


            else
            {
                TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                newnodemarketcatalogue.Tag = "0";
                newnodemarketcatalogue.Header = "No Race found";
                inplaytreeview.Items.Add(newnodemarketcatalogue);
                stkpnlGreyHoundRace.Children.Add(inplaytreeview);
                stkpnlGreyHoundRace.UpdateLayout();
            }
            bsyindicator.IsBusy = false;
        }

        private void DGVInPlaymarketsall_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {


            }
            catch (System.Exception ex)
            {
                bsyindicator.IsBusy = false;
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

            // Adjust the maximized size and position to fit the work area of the correct monitor
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




        private void TextBlock_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(this, "Are you sure to exit from this application?", "Confirm Quit", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (LoggedinUserDetail.GetUserTypeID() != 1)
                    {
                        objUsersServiceCleint.SetLoggedinStatus(LoggedinUserDetail.GetUserID(), false);
                    }

                }
                catch (System.Exception ex)
                {

                }
                Environment.Exit(0);
            }
        }



        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                this.Hide();
                int UserID = LoggedinUserDetail.GetUserID();
                objUsersServiceCleint.SetLoggedinStatus(UserID, false);
                LoggedinUserDetail.InsertActivityLog(UserID, "Logged Out");
                foreach (Window win in App.Current.Windows)
                {
                    if (win.Name == "AdminBetsWin")
                    {

                        win.Close();

                    }
                    if (win.Name.Contains("BookPositionWin"))
                    {

                        win.Close();

                    }

                }


            }
            catch (System.Exception ex)
            {

            }
        }

        private void btnRefreshLiabalites_Click(object sender, RoutedEventArgs e)
        {
            GetLiabalityofAllMarkets();
        }

        private void DGVLiabalites_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {


                if (DGVLiabalites.Items.Count > 0)
                {
                    LiabalitybyMarket objSelectedRow = (LiabalitybyMarket)DGVLiabalites.SelectedItem;
                    string MarketBookID = objSelectedRow.MarketBookID;
                    bsyindicator.IsBusy = true;
                    MarketBook(MarketBookID);



                }
            }
            catch (System.Exception ex)
            {
                bsyindicator.IsBusy = false;
            }
        }

        private void btnLedger_Click(object sender, RoutedEventArgs e)
        {

            LedgerWindow objFrmLedger = new LedgerWindow();
            objFrmLedger.UserIDforLedger = LoggedinUserDetail.GetUserID();
            objFrmLedger.ShowDialog();

        }

        private void btnProfitLoss_Click(object sender, RoutedEventArgs e)
        {
            ProfitLossWindow objPLWin = new ProfitLossWindow();
            objPLWin.ShowDialog();
        }
        public int lastselectedindex = 0;
        private void btnAdminpanel_Click(object sender, RoutedEventArgs e)
        {
            AdminPanelWindow objAdmin = new AdminPanelWindow();
            objAdmin.ShowDialog();
        }

        private void btnAccountRece_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lblShowPaymentPending_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void DGVMarketOpenbyUser_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {


                if (DGVMarketOpenbyUser.Items.Count > 0)
                {
                    MarketCatalgoue objSelectedRow = (MarketCatalgoue)DGVMarketOpenbyUser.SelectedItem;
                    string MarketBookID = objSelectedRow.ID;
                    bsyindicator.IsBusy = true;
                    MarketBook(MarketBookID);

                }
            }
            catch (System.Exception ex)
            {
                bsyindicator.IsBusy = false;
            }
        }

        private void tabctrlMarketsAll_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnLiabalites_Click(object sender, RoutedEventArgs e)
        {
            if (plusminusLiabaliteies.Content.ToString() == "+")
            {
                plusminusLiabaliteies.Content = "-";
                pnlBets.Visibility = Visibility.Visible;
                btnRefreshLiabalites_Click(this, e);
            }
            else
            {
                plusminusLiabaliteies.Content = "+";
                pnlBets.Visibility = Visibility.Collapsed;
            }
        }



        private void mainwindow_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                backgroundWorker.CancelAsync();
                backgroundWorker.Dispose();
                backgroundWorker = null;
                backgroundWorkerGetBets.CancelAsync();
                backgroundWorkerGetBets.Dispose();
                backgroundWorkerGetBets = null;
                backgroundWorkerGetStatus.CancelAsync();
                backgroundWorkerGetStatus.Dispose();
                backgroundWorkerGetStatus = null;
                backgroundWorkerLiabality.CancelAsync();
                backgroundWorkerLiabality.Dispose();
                backgroundWorkerLiabality = null;
                GC.Collect();

            }
            catch (System.Exception ex)
            {

            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {


                GetAllMarketsInPlay();

            }
            catch (System.Exception ex)
            {
                bsyindicator.IsBusy = false;

            }
        }

        private void mainwindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


    }
}

