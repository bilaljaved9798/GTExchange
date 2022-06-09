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
using globaltraders.Service123Reference;
using System.Net.Http;
using RestSharp;
using globaltraders.Models;

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
                backgroundWorkerGetStatus.DoWork += backgroundWorkerGetStatus_DoWork;
                backgroundWorkerGetStatus.RunWorkerCompleted += BackgroundWorkerGetStatus_RunWorkerCompleted;

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
#pragma warning disable CS0169 // The field 'MainWindow.cancellationToken' is never used
        CancellationToken cancellationToken;
#pragma warning restore CS0169 // The field 'MainWindow.cancellationToken' is never used
        //public async Task DoOperationsConcurrentlyAsync(CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        await Task.Run(async () =>
        //        {
        //            while (true)
        //            {
        //                Task[] tasks = new Task[3];
        //                tasks[0] = GetAllMarketsOpenedbyUser();
        //                tasks[1] = GetBetsData();
        //                tasks[2] = GetLiabalities();
        //                Task.WaitAll(tasks);
        //                await Task.Delay(50, cancellationToken);

        //            }
        //        });
        //    }

        //    catch (System.Exception ex)
        //    {

        //    }
        //}
        //public async Task DoOperationsConcurrentlyAsyncLiabalites(CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        await Task.Run(async () =>
        //        {
        //            while (true)
        //            {
        //                Task[] tasks = new Task[1];

        //                  tasks[0] = GetLiabalities();
        //                await Task.Delay(500, cancellationToken);

        //            }
        //        });
        //    }

        //    catch (System.Exception ex)
        //    {

        //    }
        //}
        //public async Task DoOperationsConcurrentlyAsyncBets(CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        await Task.Run(async () =>
        //        {
        //            while (true)
        //            {
        //                Task[] tasks = new Task[1];

        //                tasks[0] = GetBetsData();
        //                await Task.Delay(200, cancellationToken);

        //            }
        //        });
        //    }

        //    catch (System.Exception ex)
        //    {

        //    }
        //}
        //private async Task DoWorkAsyncInfiniteLoop()
        //{
        //    while (true)
        //    {
        //        // do the work in the loop
        //        try
        //        {
        //            if (LoggedinUserDetail.GetUserTypeID() == 3)
        //            {



        //                bool isstatustrue = GetStatus();
        //                if (isstatustrue == false)
        //                {
        //                    PlayMessage("Some changes made on server, please restart the application.");
        //                    Xceed.Wpf.Toolkit.MessageBox.Show(this, "Some changes made on server, please restart the application.");
        //                    Application.Current.Shutdown();



        //                }
        //                else
        //                {
        //                    await Task.Delay(5000);
        //                    // backgroundWorkerGetStatus.RunWorkerAsync();
        //                }
        //            }

        //        }
        //        catch (System.Exception ex)
        //        {

        //            Application.Current.Shutdown();
        //        }

        //        // don't run again for at least 200 milliseconds

        //    }
        //}

     
        private void BackgroundWorkerLiabality_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {


                txtAvailBalance.Content = ((long)LoggedinUserDetail.CurrentAvailableBalance).ToString("N0");
                txtTotalLiabality.Content = LoggedinUserDetail.TotalLiabality.ToString("N0");
                // txtCurrentLiabality.Text = CurrentLiabality.ToString();
                txtBalance.Content = LoggedinUserDetail.NetBalance.ToString("N0");
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
        //public async Task GetLiabalities()
        //{
        //    UpdateUserLiablity();

        //}
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
                        //  Getamountreceivablebydate(DateTime.Now);
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
                //backgroundWorkerLiabality.RunWorkerAsync();
            }

        }

     
        private void BackgroundWorkerGetBets_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                backgroundWorkerGetBets.RunWorkerAsync();

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                //   backgroundWorkerGetBets.RunWorkerAsync();
            }

        }
        System.Net.WebClient RESTProxy = new System.Net.WebClient();
       
        
        public void GetBetsData()
        {
            try
            {
               // GetDataForFancy("31304669", "1.196394285");

                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    var results = objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate);
                    List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(results);

                    LoggedinUserDetail.CurrentUserBets = lstUserBets;

                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                        var results = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate);
                        var lstUserBet = JsonConvert.DeserializeObject<List<UserBetsforAgent>>(results);
                        LoggedinUserDetail.CurrentAgentBets = lstUserBet;
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            // var results = objUsersServiceCleint.GetUserbetsForAdmin(LoggedinUserDetail.PasswordForValidate);
                            LoggedinUserDetail.CurrentAdminBets = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(LoggedinUserDetail.PasswordForValidate));
                            //string RestAPIPath = ConfigurationManager.AppSettings["RestAPIPath"];

                            //byte[] Data = RESTProxy.DownloadData(new Uri(RestAPIPath + "Services/BettingServiceRest.svc/GetUserbetsForAdmin"));

                            //System.IO.Stream stream = new System.IO.MemoryStream(Data);
                            //System.Runtime.Serialization.Json.DataContractJsonSerializer obj = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(string));
                            //string userbets = obj.ReadObject(stream).ToString();

                            //List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(userbets);


                            //LoggedinUserDetail.CurrentAdminBets = lstUserBet;


                            ///

                            //string RestAPIPath1 = ConfigurationManager.AppSettings["RestAPIPath"] + "Services/BettingServiceRest.svc/GetUserbetsForAdmin";
                            //////var client1 = new RestClient(RestAPIPath1);
                            //////client1.CookieContainer = new CookieContainer();
                            //////var request1 = new RestRequest(Method.GET);
                            //////request1.AddHeader("Accept-Encoding", "gzip");
                            //////var response1 = client1.Execute(request1);
                            //////var arr = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JValue>(response1.Content);
                            //////var arr1 = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(arr.Value.ToString());
                            //////var results = arr1;
                            //////LoggedinUserDetail.CurrentAdminBets = results;
                            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(RestAPIPath1);
                            //request.Method = "GET";
                            //request.Proxy = null;
                            //request.Timeout = 5000;
                            //request.KeepAlive = false;
                            //request.ServicePoint.ConnectionLeaseTimeout = 5000;
                            //request.ServicePoint.MaxIdleTime = 5000;
                            //request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            //String test = String.Empty;
                            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                            //{
                            //    using (Stream dataStream = response.GetResponseStream())
                            //    {
                            //        System.Runtime.Serialization.Json.DataContractJsonSerializer obj1 = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(string));
                            //        test = obj1.ReadObject(dataStream).ToString();

                            //        dataStream.Close();
                            //    }



                            //}
                            //List<UserBetsForAdmin> lstUserBet1 = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(test);
                            ////////// List<UserBetsForAdmin> lstUserBet1 = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(objUsersServiceCleint.GetUserbetsForAdmin(LoggedinUserDetail.PasswordForValidate));

                            //LoggedinUserDetail.CurrentAdminBets = lstUserBet1;
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 9 )
                            {
                                var results = objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate);
                                var lstUserBet = JsonConvert.DeserializeObject<List<UserBetsforSamiAdmin>>(results);
                                LoggedinUserDetail.CurrentsamiadminBets = lstUserBet;
                            }
                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 8)
                                {
                                    var results = objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate);
                                    var lstUserBet = JsonConvert.DeserializeObject<List<UserBetsforSuper>>(results);
                                    LoggedinUserDetail.CurrentSuperBets = lstUserBet;
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

        MarketBookShow runner1 = new MarketBookShow();
        ExternalAPI.TO.MarketBookForindianFancy currentmarketsfancyPL = new ExternalAPI.TO.MarketBookForindianFancy();
        ObservableCollection<ExternalAPI.TO.MarketBookForindianFancy> allmarkets = new ObservableCollection<ExternalAPI.TO.MarketBookForindianFancy>();
        ExternalAPI.TO.GetDataFancy GetDataFancy = new ExternalAPI.TO.GetDataFancy();
       


        private void RESTProxy_DownloadDataCompleted(object sender, System.Net.DownloadDataCompletedEventArgs e)
        {
            try
            {
                System.IO.Stream stream = new System.IO.MemoryStream(e.Result);
                System.Runtime.Serialization.Json.DataContractJsonSerializer obj = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(string));
                string userbets = obj.ReadObject(stream).ToString();
                //string userbets = objUsersServiceCleint.GetUserbetsForAdmin();
                List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<UserBetsForAdmin>>(userbets);

                // LoggedinUserDetail.CurrentAdminBetsNew = JsonConvert.DeserializeObject<ObservableCollection<UserBetsForAdmin>>(userbets);
                //LoggedinUserDetail.CurrentAdminBets = lstUserBet;

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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        public void GetusersbyUserType()
        {
            //  objUsersServiceCleint.GetAllUsersbyUserTypeAsync(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.GetUserTypeID(), LoggedinUserDetail.PasswordForValidate);
            //   objUsersServiceCleint.GetAllUsersbyUserTypeCompleted += ObjUsersServiceCleint_GetAllUsersbyUserTypeCompleted;

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

        private void BackgroundWorkerGetStatus_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {


                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {


                    //try
                    //{
                    //    var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUsersForAdmin());
                    //    if (results.Count > 0)
                    //    {
                    //        foreach (var objmarket in results)
                    //        {
                    //            objmarket.Name = objmarket.Name + "/" + objmarket.EventName + " (" + objmarket.EventTypeName + ")";
                    //        }
                    //        DGVMarketOpenbyUser.ItemsSource = results;
                    //    }
                    //    else
                    //    {
                    //        DGVMarketOpenbyUser.ItemsSource = null;
                    //        DGVMarketOpenbyUser.Items.Clear();
                    //    }
                    //}
                    //catch (System.Exception ex)
                    //{

                    //}

                }
                else
                {
                    TabItem tbitem3 = (TabItem)tbctrlLiabalities.Items[0];
                    tbitem3.Visibility = Visibility.Collapsed;
                    tbctrlLiabalities.SelectedIndex = 1;
                }
                backgroundWorkerGetStatus.RunWorkerAsync();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
            // GetLiabalityofAllMarkets();
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
        private void backgroundWorkerGetStatus_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() != 1 && LoggedinUserDetail.GetUserTypeID() != 8)
                {


                    BackgroundWorker worker = sender as BackgroundWorker;
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;

                        return;
                    }

                    bool isstatustrue = GetStatus();
                    if (isstatustrue == false)
                    {

                        if (!CheckAccess())
                        {
                            Dispatcher.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                            {

                                PlayMessage("Some changes made on server, please restart the application.");
                                Xceed.Wpf.Toolkit.MessageBox.Show(this, "Some changes made on server, please restart the application.");
                                Application.Current.Shutdown();
                            }));
                        }
                        else
                        {

                            PlayMessage("Some changes made on server, please restart the application.");
                            Xceed.Wpf.Toolkit.MessageBox.Show(this, "Some changes made on server, please restart the application.");
                            Application.Current.Shutdown();
                        }


                    }
                    else
                    {
                        System.Threading.Thread.Sleep(5000);
                        // backgroundWorkerGetStatus.RunWorkerAsync();
                    }
                }
                else
                {
                    System.Threading.Thread.Sleep(200);
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            catch (System.Exception ex)
            {
                //backgroundWorkerGetStatus.CancelAsync();
                if (!CheckAccess())
                {
                    Dispatcher.Invoke(new System.Windows.Forms.MethodInvoker(delegate ()
                    {

                        //   PlayMessage("Something wrong with internet. Please restart the application");
                        Xceed.Wpf.Toolkit.MessageBox.Show(this, "Something wrong with internet. Please check your internet.");
                        // Application.Current.Shutdown();
                    }));
                }
                else
                {

                    // PlayMessage("Something wrong with internet. Please restart the application.");
                    Xceed.Wpf.Toolkit.MessageBox.Show(this, "Something wrong with internet. Please check your internet.");
                    // Application.Current.Shutdown();
                }

            }
        }

        public bool GetStatus()
        {
            if (LoggedinUserDetail.GetUserID() == 0)
            {
                return false;
            }
            UserStatus objUserStatus = JsonConvert.DeserializeObject<UserStatus>(objUsersServiceCleint.GetUserStatus(LoggedinUserDetail.GetUserID()));


            if (objUserStatus.isBlocked == true || objUserStatus.isDeleted == true || objUserStatus.Loggedin == false || objUserStatus.CurrentLoggedInID != LoggedinUserDetail.user.CurrentLoggedInID)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        int BetPlaceWait = 0;

        int BetWaitTimerInterval = 0;

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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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


                    // lstAllMarkets = GetAllMarketsInPlay();
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                                        newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;
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


                    if (chkShowinMultipleWindows.IsChecked == true)
                    {
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
                                objmarketwindow.Height = 400;
                            }
                            else
                            {
                                objmarketwindow.Height = 400;
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
                        //objmarketwindow.MarketBookForProfitandloss = objmarketwindow.MarketBook;


                        objmarketwindow.UserIDforLoadMarket = UserIDforLoadMarket;
                        objmarketwindow.Show();
                    }
                    else
                    {
                        //MarketWindowUserControl1 objmarketwindow = new MarketWindowUserControl1();
                        //objmarketwindow.Name = "markettab" + MarketBookID.Replace(".", "");
                        //ClosableTab newtabitem = new ClosableTab();
                        //newtabitem.Title = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == MarketBookID).FirstOrDefault().MarketBookName;
                        //newtabitem.Content = objmarketwindow;
                        //newtabitem.Name = objmarketwindow.Name;
                        //newtabitem.Tag = MarketBookID;

                        //tabctrlMarketsAll.Items.Add(newtabitem);
                        //tabctrlMarketsAll.SelectedIndex = tabctrlMarketsAll.Items.Count - 1;
                        MarketWindowUserControl objmarketwindow = new MarketWindowUserControl();
                        objmarketwindow.Name = "tabctrlMarketsAll_MouseDoubleClick" + MarketBookID.Replace(".", "");
                        LoggedinUserDetail.OpenMarkets.Add(objmarketwindow.Name);
                        objmarketwindow.MarketBook = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == MarketBookID).FirstOrDefault();
                        ClosableTab newtabitem = new ClosableTab();
                        newtabitem.Title = LoggedinUserDetail.MarketBooks.Where(item => item.MarketId == MarketBookID).FirstOrDefault().MarketBookName;
                        newtabitem.Content = objmarketwindow;
                        newtabitem.Name = objmarketwindow.Name;
                        objmarketwindow.Foreground = Brushes.Black;                                 
                        newtabitem.Foreground =Brushes.Black;
                        objmarketwindow.Width = pnlMarkets.Width;
                        objmarketwindow.Height = pnlMarkets.Height;
                        marketbookIDselected = MarketBookID;
                        newtabitem.Tag = MarketBookID;                     
                        //  newtabitem.Style = Resources["customtabitem"] as Style;
                        tabctrlMarketsAll.Items.Add(newtabitem);
                        objmarketwindow.stkpnlMarketGrid.Width = pnlMarkets.Width;
                        tabctrlMarketsAll.SelectedIndex = tabctrlMarketsAll.Items.Count - 1;
                      // newtabitem.Header = new FontFamily("Times New Roman");
                        //newtabitem.FontSize = 22;




                    }

                }
            }
        }

        public void GetAllMarketBooks()
        {
            IList<bftradeline.wrBF.MarketBook> list;
            try
            {

                List<string> marketIdsNew = LoggedinUserDetail.MarketBooks.Where(item =>item.isOpenExternally==true|| item.isWinTheTossMarket == true).Select(item => item.MarketId).Distinct().ToList();
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                                    currentmarketobject = ConvertJsontoMarketObjectBFNewSource(item, currentmarketobject.MarketId, currentmarketobject.OrignalOpenDate.Value, currentmarketobject.MarketBookName, currentmarketobject.MainSportsname, false, currentmarketobject.Runners, currentmarketobject.isOpenExternally,currentmarketobject);
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {

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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0162 // Unreachable code detected
                    return new MarketBook();
#pragma warning restore CS0162 // Unreachable code detected
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                    // var marketbook = new MarketBook();

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
                                catch (System.Exception ex)
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
#pragma warning disable CS0162 // Unreachable code detected
                    return new MarketBook();
#pragma warning restore CS0162 // Unreachable code detected
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new ExternalAPI.TO.MarketBook();
            }
        }
        public MarketBook ConvertJsontoMarketObjectBFNewSource(ExternalAPI.TO.MarketBookString BFMarketbook, string marketid, DateTime marketopendate, string sheetname, string MainSportsCategory, bool ConvertRates, List<ExternalAPI.TO.Runner> OldRunners, bool isopenexternally,MarketBook marketbook)
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
                    //&& newres[i] != "Æ  Æ UnitedBet Æ 1 Æ 1 Æ" && newres[i] != "Æ  Æ UnitedBet Æ 2 Æ 2 Æ"
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0162 // Unreachable code detected
                return new MarketBook();
#pragma warning restore CS0162 // Unreachable code detected
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
#pragma warning disable CS0162 // Unreachable code detected
                    return new MarketBook();
#pragma warning restore CS0162 // Unreachable code detected
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
        public void SetBetSlipKeys()
        {
            try
            {


                BetSlipKeys objBetSlipKeys = LoggedinUserDetail.objBetSlipKeys;


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
        public bool isOpeningWindow = false;
        public void MarketBook(string ID)
        {
            try
            {

                //Dispatcher.BeginInvoke(new Action(() =>
                //{


                LoggedinUserDetail.CheckifUserLogin();
                if (ID != "" && LoggedinUserDetail.GetUserTypeID() != 1)
                {
                    if (LoggedinUserDetail.MarketBooks != null)

                    {
                        if (LoggedinUserDetail.MarketBooks.Count > 10)
                        {
                            bsyindicator.IsBusy = false;
                            MessageBox.Show(this, "Limit exceed");
                            return;
                        }

                    }

                }
                if (ID != "" && LoggedinUserDetail.GetUserTypeID() == 1)
                {

                    objUsersServiceCleint.SetMarketBookOpenbyUSerAsync(73, ID);
                }
                var marketbooks = new List<ExternalAPI.TO.MarketBook>();
                if (LoggedinUserDetail.GetUserTypeID() == 3 || LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 1 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
                {
                    if (ID == "")
                    {
                    }
                    else
                    {
                        if (chkShowinMultipleWindows.IsChecked == true)
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
                                // var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));
                                isOpeningWindow = true;
                                //var results= objUsersServiceCleint.SetMarketBookOpenbyUSerandGet(UserIDforLoadMarket, ID);
                                //intializemarketdata(results);
                                objUsersServiceCleint.SetMarketBookOpenbyUSerandGetAsync(UserIDforLoadMarket, ID);
                                objUsersServiceCleint.SetMarketBookOpenbyUSerandGetCompleted += ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGetCompleted;
                            }
                        }
                        else
                        {
                            string tabpagename = "markettab" + ID.Replace(".", "").ToString();
                            var marketwindowname = LoggedinUserDetail.OpenMarkets.Where(item => item == tabpagename).FirstOrDefault();

                            if (marketwindowname != null)
                            {
                                foreach (TabItem win in tabctrlMarketsAll.Items)
                                {
                                    if (win.Name == "markettab" + ID.Replace(".", ""))
                                    {
                                        win.FontFamily=new FontFamily("Times New Roman");
                                        win.Focus();
                                    }
                                }
                                isOpeningWindow = false;
                                bsyindicator.IsBusy = false;

                            }
                            else
                            {
                                //isOpeningWindow = true;
                                //try
                                //{

                                //    var results = JsonConvert.DeserializeObject<List<MarketCatalogueandSelectionNames>>(objUsersServiceCleint.SetMarketBookOpenbyUSerandGet(UserIDforLoadMarket, ID));
                                //    var newmarkettobeopened = results;
                                //    List<string> lstIDs = new List<string>();

                                //    var newmarketbook = new ExternalAPI.TO.MarketBook();

                                //    ExternalAPI.TO.MarketBook marketbook1;
                                //    marketbook1 = GetCurrentMarketBook(newmarkettobeopened[0].ID, newmarkettobeopened[0].Name, newmarkettobeopened[0].EventTypeName, newmarkettobeopened[0].EventOpenDate, newmarkettobeopened[0].BettingAllowed, new List<ExternalAPI.TO.Runner>());

                                //    if (marketbook1.MarketId != null)
                                //    {
                                //        newmarketbook = (marketbook1);
                                //        newmarketbook.MarketBookName = newmarkettobeopened[0].Name + " / " + newmarkettobeopened[0].EventName;
                                //        newmarketbook.OrignalOpenDate = newmarkettobeopened[0].EventOpenDate;
                                //        newmarketbook.MainSportsname = newmarkettobeopened[0].EventTypeName;
                                //        newmarketbook.BettingAllowed = newmarkettobeopened[0].BettingAllowed;

                                //        if (1 == 1)
                                //        {

                                //            if (1 == 2)
                                //            {

                                //            }
                                //            else
                                //            {
                                //                foreach (var runnermarketitem in newmarkettobeopened)
                                //                {

                                //                    var runneritem = newmarketbook.Runners.Where(item => item.SelectionId == runnermarketitem.SelectionID).First();

                                //                    runneritem.SelectionId = runnermarketitem.SelectionID;
                                //                    runneritem.RunnerName = runnermarketitem.SelectionName;
                                //                    runneritem.JockeyName = runnermarketitem.JockeyName;
                                //                    runneritem.WearingURL = runnermarketitem.Wearing;
                                //                    runneritem.WearingDesc = runnermarketitem.WearingDesc;
                                //                    runneritem.Clothnumber = runnermarketitem.ClothNumber;
                                //                    runneritem.StallDraw = runnermarketitem.StallDraw;



                                //                }
                                //            }


                                //        }
                                //        ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
                                //        objmarketbook = newmarketbook;
                                //        if (objmarketbook != null)
                                //        {
                                //            LoggedinUserDetail.MarketBooks.Add(objmarketbook);
                                //            LoadGridMarket(objmarketbook.MarketId);





                                //        }
                                //        isOpeningWindow = false;

                                //    }
                                //    else
                                //    {
                                //        isOpeningWindow = false;
                                //    }
                                //    bsyindicator.IsBusy = false;
                                //}
                                //catch (System.Exception ex)
                                //{
                                //    isOpeningWindow = false;
                                //    bsyindicator.IsBusy = false;
                                //}
                                //var results = objUsersServiceCleint.SetMarketBookOpenbyUSerandGet(UserIDforLoadMarket, ID);
                                //intializemarketdata(results);
                                isOpeningWindow = true;
                                objUsersServiceCleint.SetMarketBookOpenbyUSerandGetAsync(UserIDforLoadMarket, ID);
                                objUsersServiceCleint.SetMarketBookOpenbyUSerandGetCompleted += ObjUsersServiceCleint_SetMarketBookOpenbyUSerandGetCompleted;
                                // var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUser(LoggedinUserDetail.GetUserID()));

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



                //}));
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                        // Task.Factory.StartNew(() =>
                        //{



                        //}).ContinueWith(task =>
                        //{
                        //      //this code runs back on the UI thread
                        //     // bsyindicator.IsBusy = false;
                        //}, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
                        //await MarketBook(objSender.Tag.ToString()).ContinueWith((x) =>
                        //{

                        //});

                    }
                }


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                        //TodayRace root = new TodayRace();
                        //  root.TodayRaces = new ObservableCollection<TodayRace>();
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
                        // lstRaces.Add(root);
                        //foreach (var item in lstTodayHorseRacing)
                        //{
                        //    TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                        //    newnodemarketcatalogue.Tag = item.MarketCatalogueID;
                        //    newnodemarketcatalogue.Header = item.TodayHorseRace;
                        //    inplaytreeview.Items.Add(newnodemarketcatalogue);
                        //    newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;
                        //}
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
           // lstEventTypes= lstEventTypes.Where(item=>item.EventTypeID=="4").ToArray();
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


                    //if (lbl.ToString()!= "")

                    //{
                    //lbl.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF"));
                    //}

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


                        //if (lbl1.ToString() != "")
                        //{
                        //    lbl1.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF"));
                        //}
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
                                        newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;


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

        private void btnCricket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (plusminusAllCricket.Content.ToString() == "+")
                {
                    plusminusAllCricket.Content = "-";
                    stkpnlCricket.Visibility = Visibility.Visible;
                    stkpnlCricket.Children.Clear();
                    bsyindicator.IsBusy = true;
                    objUsersServiceCleint.GetAllCricketMatchesAsync(UserIDforLoadMarket);
                    objUsersServiceCleint.GetAllCricketMatchesCompleted += ObjUsersServiceCleint_GetAllCricketMatchesCompleted;

                }
                else
                {
                    plusminusAllCricket.Content = "+";
                    stkpnlCricket.Visibility = Visibility.Collapsed;
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                bsyindicator.IsBusy = false;
            }
        }

        private void ObjUsersServiceCleint_GetAllCricketMatchesCompleted(object sender, GetAllCricketMatchesCompletedEventArgs e)
        {
            objUsersServiceCleint.GetAllCricketMatchesCompleted -= ObjUsersServiceCleint_GetAllCricketMatchesCompleted;
            List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(e.Result);
            TreeView inplaytreeview = new TreeView();
            var lstEventTypes = lstInPlayMatches.Select(item => new { item.EventTypeID, item.EventTypeName }).Distinct().ToArray();
           // lstEventTypes = lstEventTypes.Where(item => item.EventTypeName != "Line v Markets" && item.EventTypeName != "Kali v Jut" && item.EventTypeName != "Figure" && item.EventTypeName != "SmallFig").ToArray();
           // lstEventTypes = lstEventTypes.Where(item => item.EventTypeName != "" ).ToArray();
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


                    //if (lbl.ToString()!= "")

                    //{
                    //lbl.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF"));
                    //}

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


                        //if (lbl1.ToString() != "")
                        //{
                        //    lbl1.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF"));
                        //}
                        // Add into stack

                        stack1.Children.Add(lbl1);
                        newnodecompetition.Tag = 0;
                        newnodecompetition.Header = stack1;
                        NewNodeeventtype.Items.Add(newnodecompetition);
                        var lstEvents = lstInPlayMatches.Where(item => item.CompetitionID == competitionitem.CompetitionID).Select(item => new { item.EventID, item.EventName }).Distinct().ToArray();
                        lstEvents= lstEvents.Where(item => item.EventName != "Line v Markets" && item.EventName != "Kali v Jut" && item.EventName != "Figure" && item.EventName != "SmallFig").ToArray();
                        foreach (var eventitem in lstEvents)

                        {
                            if (!(eventitem.EventName.Contains("Kali v Jut") || eventitem.EventName.Contains("Lines v")))
                            {


                                TreeViewItem newnodeevents = new TreeViewItem();
                                newnodeevents.Tag = 0;
                                newnodeevents.Header = eventitem.EventName;
                                newnodecompetition.Items.Add(newnodeevents);
                                var lstMarketCatalogues = lstInPlayMatches.Where(item => item.EventID == eventitem.EventID && item.EventName != "Line v Markets" && item.EventName != "Kali v Jut" && item.EventName != "Figure" && item.EventName != "SmallFig").Select(item => new { item.MarketCatalogueID, item.MarketCatalogueName }).Distinct().ToArray();

                                foreach (var marketcatalogueitem in lstMarketCatalogues)
                                {
                                    TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                                    newnodemarketcatalogue.Tag = marketcatalogueitem.MarketCatalogueID;
                                    newnodemarketcatalogue.Header = marketcatalogueitem.MarketCatalogueName;
                                    newnodeevents.Items.Add(newnodemarketcatalogue);
                                    newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;


                                }
                                //var linevmaketbyEventID = lstInPlayMatches.Where(item => item.AssociateEventID == eventitem.EventID && item.EventName == "Line v Markets").Select(item => new { item.MarketCatalogueID, item.MarketCatalogueName }).Distinct().ToArray();
                                //if (linevmaketbyEventID.Count() > 0)
                                //{
                                //    linevmaketbyEventID.OrderBy(item => item.MarketCatalogueName).ToList();
                                //    TreeViewItem newnodeeventsline = new TreeViewItem();
                                //    newnodeeventsline.Tag = 0;
                                //    newnodeeventsline.Header = "Line v Markets";
                                //    newnodeevents.Items.Add(newnodeeventsline);
                                   
                                    //foreach (var marketcatalogueitem in linevmaketbyEventID)
                                    //{
                                    //    TreeViewItem newnodemarketcatalogue = new TreeViewItem();
                                    //    newnodemarketcatalogue.Tag = marketcatalogueitem.MarketCatalogueID;
                                    //    newnodemarketcatalogue.Header = marketcatalogueitem.MarketCatalogueName;
                                    //    newnodeeventsline.Items.Add(newnodemarketcatalogue);
                                    //    newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;


                                    //}
                              //  }
                            }
                        }
                    }
                }
                inplaytreeview.Height = 300;
                stkpnlCricket.Children.Add(inplaytreeview);
                stkpnlCricket.UpdateLayout();
            }
            else
            {

            }
            bsyindicator.IsBusy = false;
        }

    
        private void btnSoccer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (plusminusAllSoccer.Content.ToString() == "+")
                {
                    plusminusAllSoccer.Content = "-";
                    stkpnlSoccer.Visibility = Visibility.Visible;
                    stkpnlSoccer.Children.Clear();
                    bsyindicator.IsBusy = true;
                    objUsersServiceCleint.GetAllSoccerMatchesAsync(UserIDforLoadMarket);
                    objUsersServiceCleint.GetAllSoccerMatchesCompleted += ObjUsersServiceCleint_GetAllSoccerMatchesCompleted;
                }
                else
                {
                    plusminusAllSoccer.Content = "+";
                    stkpnlSoccer.Visibility = Visibility.Collapsed;
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                bsyindicator.IsBusy = false;
            }
        }

        private void ObjUsersServiceCleint_GetAllSoccerMatchesCompleted(object sender, GetAllSoccerMatchesCompletedEventArgs e)
        {
            objUsersServiceCleint.GetAllSoccerMatchesCompleted -= ObjUsersServiceCleint_GetAllSoccerMatchesCompleted;
            List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(e.Result);
            TreeView inplaytreeview = new TreeView();
            var lstEventTypes = lstInPlayMatches.Select(item => new { item.EventTypeID, item.EventTypeName }).Distinct().ToArray();
            lstEventTypes = lstEventTypes.Where(item => item.EventTypeID == "4").ToArray();
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


                    //if (lbl.ToString()!= "")

                    //{
                    //lbl.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF"));
                    //}

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


                        //if (lbl1.ToString() != "")
                        //{
                        //    lbl1.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF"));
                        //}
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
                                        newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;


                                    }
                                }
                            }
                        }
                    }
                }
                inplaytreeview.Height = 300;
                stkpnlSoccer.Children.Add(inplaytreeview);
                stkpnlSoccer.UpdateLayout();
            }
            else
            {

            }
            bsyindicator.IsBusy = false;
        }

        private void btnTennis_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (plusminusAllTennis.Content.ToString() == "+")
                {
                    plusminusAllTennis.Content = "-";
                    stkpnlTennis.Visibility = Visibility.Visible;
                    stkpnlTennis.Children.Clear();
                    bsyindicator.IsBusy = true;
                    objUsersServiceCleint.GetAllTennisMatchesAsync(UserIDforLoadMarket);
                    objUsersServiceCleint.GetAllTennisMatchesCompleted += ObjUsersServiceCleint_GetAllTennisMatchesCompleted;
                }
                else
                {
                    plusminusAllTennis.Content = "+";
                    stkpnlTennis.Visibility = Visibility.Collapsed;
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                bsyindicator.IsBusy = false;
            }
        }

        private void ObjUsersServiceCleint_GetAllTennisMatchesCompleted(object sender, GetAllTennisMatchesCompletedEventArgs e)
        {
            objUsersServiceCleint.GetAllTennisMatchesCompleted -= ObjUsersServiceCleint_GetAllTennisMatchesCompleted;
            List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(e.Result);
            TreeView inplaytreeview = new TreeView();
            var lstEventTypes = lstInPlayMatches.Select(item => new { item.EventTypeID, item.EventTypeName }).Distinct().ToArray();
           // lstEventTypes = lstEventTypes.Where(item => item.EventTypeID == "2").ToArray();
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


                    //if (lbl.ToString()!= "")

                    //{
                    //lbl.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF"));
                    //}

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


                        //if (lbl1.ToString() != "")
                        //{
                        //    lbl1.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF"));
                        //}
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
                                        newnodemarketcatalogue.MouseLeftButtonUp += Inplaytreeview_MouseLeftButtonDown;


                                    }
                                }
                            }
                        }
                    }
                }
                inplaytreeview.Height = 300;
                stkpnlTennis.Children.Add(inplaytreeview);
                stkpnlTennis.UpdateLayout();
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
                //DGVInPlaymarketsall.ItemsSource = new;
                //DGVInPlaymarketsall.Items.Refresh();
                objUsersServiceCleint.GetInPlayMatcheswithRunners1Async(UserIDforLoadMarket);
                objUsersServiceCleint.GetInPlayMatcheswithRunners1Completed += ObjUsersServiceCleint_GetInPlayMatcheswithRunners1Completed;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void ObjUsersServiceCleint_GetInPlayMatcheswithRunners1Completed(object sender, GetInPlayMatcheswithRunners1CompletedEventArgs e)
        {
            objUsersServiceCleint.GetInPlayMatcheswithRunners1Completed -= ObjUsersServiceCleint_GetInPlayMatcheswithRunners1Completed;
            List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(e.Result);
            List<string> lstIds = lstInPlayMatches.Where(item => item.EventTypeName == "Cricket").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList();
            lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Soccer" ).Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
            lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Tennis").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
            //lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Tennis" && item.EventOpenDate.Value.AddHours(5) >= DateTime.Now).Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());

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
                    objGridMarket.MarketStatus = objMarketLocal.MarketStatus;
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                    {

                    }
                    lstGridMarkets.Add(objGridMarket);

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    bsyindicator.IsBusy = false;
                }
            }
            DGVInPlaymarketsall.ItemsSource = lstGridMarkets;
            bsyindicator.IsBusy = false;
        }


        public void GetLiabalityofAllMarkets()
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            try
            {
                List<LiabalitybyMarket> lstLibalitybymrakets = new List<LiabalitybyMarket>();
                List<LiabalitybyMarket> lstLibalitybymraketsfancy = new List<LiabalitybyMarket>();
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    List<UserBets> lstuserbetfancy = LoggedinUserDetail.CurrentUserBets.Where(item => item.location == "9" || item.location=="8").ToList();
                    List <UserBets> lstuserbet = LoggedinUserDetail.CurrentUserBets.Where(item=>item.location !="9" && item.location !="8").ToList();
                    lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentUserbyMarkets(LoggedinUserDetail.GetUserID(), lstuserbet);
                    objUserBets.GetLiabalityofCurrentUserfancy(LoggedinUserDetail.GetUserID(), lstuserbetfancy);
                    lstLibalitybymrakets.AddRange(objUserBets.GetLiabalityofCurrentUserbyMarketsfancy(LoggedinUserDetail.GetUserID(), lstuserbetfancy));
                    DGVLiabalites.ItemsSource = lstLibalitybymrakets;
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {

                        lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentAgentbyMarkets(LoggedinUserDetail.CurrentAgentBets);

                        DGVLiabalites.ItemsSource = lstLibalitybymrakets;

                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {

                            lstLibalitybymrakets = objUserBets.GetLiabalityofAdminbyMarkets(LoggedinUserDetail.CurrentAdminBets.ToList());
                            DGVLiabalites.ItemsSource = lstLibalitybymrakets;


                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                lstLibalitybymrakets = objUserBets.GetLiabalityofSuperbyMarkets(LoggedinUserDetail.CurrentSuperBets.ToList());
                                DGVLiabalites.ItemsSource = lstLibalitybymrakets;
                            }
                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 9)
                                {
                                    lstLibalitybymrakets = objUserBets.GetLiabalityofSamiadminbyMarkets(LoggedinUserDetail.CurrentsamiadminBets.ToList());
                                    DGVLiabalites.ItemsSource = lstLibalitybymrakets;
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
                pnlMarketsBorder.Height = containerheight;
                // pnlEvents.Width = containerwidth;
                pnlMarkets.Width = containerwidth - 200;
                DGVInPlaymarketsall.Width = pnlMarkets.Width - 10;
                DGVInPlaymarketsall.Height = containerheight - 60;
                txtMarketHeading.Width = containerwidth - 500;
                pnlMarkets.Height = containerheight;
                // pnlBets.Width = containerwidth;
                //  pnlBets.Height = containerheight;
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
                    if (LoggedinUserDetail.GetUserTypeID() != 1)
                    {
                        objUsersServiceCleint.SetLoggedinStatus(LoggedinUserDetail.GetUserID(), false);
                    }

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {

                }
                Environment.Exit(0);
                //  Application.Current.Shutdown();
            }

        }
        public void SetURLsData()
        {
            LoggedinUserDetail.URLsData = JsonConvert.DeserializeObject<List<SP_URLsData_GetAllData_Result>>(objUsersServiceCleint.GetURLsData());
            //ws1.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Soccer").FirstOrDefault().URLForData;
            //ws2.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Tennis").FirstOrDefault().URLForData;
            //ws4.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().URLForData;
            //ws7.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Horse Racing").FirstOrDefault().URLForData;
            //ws4339.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "GreyHound Racing").FirstOrDefault().URLForData;
            //wsFancy.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Fancy").FirstOrDefault().URLForData;
            //wsAllData.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "AllData").FirstOrDefault().URLForData;
            //ws0.Url = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Other").FirstOrDefault().URLForData;
            LoggedinUserDetail.SecurityCode = LoggedinUserDetail.URLsData.FirstOrDefault().Scd;
            LoggedinUserDetail.GetCricketDataFrom = LoggedinUserDetail.URLsData.Where(item => item.EventType == "Cricket").FirstOrDefault().GetDataFrom;
            if (LoggedinUserDetail.GetCricketDataFrom == "Live")
            {
                //  chkShowinMultipleWindows.IsEnabled = false;
            }
        }
        public static class Kalijut
        {
            public static int ID { get; set; }
            public static Nullable<double> KaliSizeBack { get; set; }
            public static Nullable<double> KaliPriceBack { get; set; }
            public static Nullable<double> KaliSizeLay { get; set; }
            public static Nullable<double> KaliPriceLay { get; set; }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            List<string> s = new List<string>();

            RESTProxy.DownloadDataCompleted += RESTProxy_DownloadDataCompleted;
            //  await  DoWorkAsyncInfiniteLoop();
            //if (LoggedinUserDetail.GetUserTypeID() == 3)
            //{


            //    List<bftradeline.Models.LastLoginTimes> lstLastLoginTimes = JsonConvert.DeserializeObject<List<LastLoginTimes>>(objUsersServiceCleint.GetLastLoginTimes(LoggedinUserDetail.user.ID));
            //    if (lstLastLoginTimes.Count() == 2)
            //    {
            //        TimeSpan timediff = lstLastLoginTimes[0].Activitytime - lstLastLoginTimes[1].Activitytime;
            //        if (timediff.TotalSeconds <= 10)
            //        {
            //            objUsersServiceCleint.SetBlockedStatusofUserAsync(LoggedinUserDetail.user.ID, true, LoggedinUserDetail.PasswordForValidate);
            //            LoggedinUserDetail.InsertActivityLog(LoggedinUserDetail.user.ID, "Account Blocked");
            //            Xceed.Wpf.Toolkit.MessageBox.Show(this, "Account is Blocked.");
            //            int UserID = LoggedinUserDetail.user.ID;
            //            objUsersServiceCleint.SetLoggedinStatusAsync(UserID, false);
            //            LoggedinUserDetail.InsertActivityLog(UserID, "Logged Out");
            //            Application.Current.Shutdown();
            //            return;
            //            LoggedinUserDetail.user = new UserIDandUserType();
            //        }
            //    }
            //}
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
                btnAccountRece.Visibility = Visibility.Collapsed;
                objUsersServiceCleint.GetAllowedMarketsbyUserIDAsync(Convert.ToInt32(LoggedinUserDetail.GetUserID()));
                objUsersServiceCleint.GetAllowedMarketsbyUserIDCompleted += ObjUsersServiceCleint_GetAllowedMarketsbyUserIDCompleted;
            }
            if (LoggedinUserDetail.GetUserTypeID() == 2)
            {
                GetusersbyUserType();
                try
                {
                    LoggedinUserDetail.MaxBalanceTransferLimit = objUsersServiceCleint.GetMaxBalanceTransferLimit(LoggedinUserDetail.GetUserID());
                    LoggedinUserDetail.MaxAgentRateLimit = objUsersServiceCleint.GetMaxAgentRate(LoggedinUserDetail.GetUserID());
                }
                catch (System.Exception ex)
                {
                }
            }
            if (LoggedinUserDetail.GetUserTypeID() == 8)
            {
                GetusersbyUserType();
                try
                {
                    LoggedinUserDetail.MaxBalanceTransferLimit = objUsersServiceCleint.GetMaxBalanceTransferLimit(LoggedinUserDetail.GetUserID());
                    LoggedinUserDetail.MaxAgentRateLimit = objUsersServiceCleint.GetMaxAgentRate(LoggedinUserDetail.GetUserID());
                }
                catch (System.Exception ex)
                {
                }
            }
            if (LoggedinUserDetail.GetUserTypeID() == 9)
            {
                GetusersbyUserType();
                try
                {
                    LoggedinUserDetail.MaxBalanceTransferLimit = objUsersServiceCleint.GetMaxBalanceTransferLimit(LoggedinUserDetail.GetUserID());
                    LoggedinUserDetail.MaxAgentRateLimit = objUsersServiceCleint.GetMaxAgentRate(LoggedinUserDetail.GetUserID());
                }
                catch (System.Exception ex)
                {
                }
            }
            DGVInPlaymarketsall.AutoGenerateColumns = false;

            GetAllMarketsInPlay();

            LoggedinUserDetail.user.CurrentLoggedInID = LoggedinUserDetail.user.CurrentLoggedInID + 1;
            objUsersServiceCleint.UpdateCurrentLoggedInIDbyUserIDAsync(LoggedinUserDetail.user.ID);
            objUsersServiceCleint.GetCurrentBalancebyUserAsync(LoggedinUserDetail.user.ID, LoggedinUserDetail.PasswordForValidate);
            objUsersServiceCleint.GetCurrentBalancebyUserCompleted += ObjUsersServiceCleint_GetCurrentBalancebyUserCompleted;
            objUsersServiceCleint.GetStartingBalanceAsync(LoggedinUserDetail.user.ID, LoggedinUserDetail.PasswordForValidate);
            objUsersServiceCleint.GetStartingBalanceCompleted += ObjUsersServiceCleint_GetStartingBalanceCompleted;


            lblUSerName.Content = LoggedinUserDetail.user.UserName;

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
            //   UpdateUserLiablity();
            if (LoggedinUserDetail.GetUserTypeID() == 3)
            {
              //  backgroundWorkerLiabality.RunWorkerAsync();
            }
            if (LoggedinUserDetail.GetUserTypeID() != 1)
            {
                backgroundWorkerGetStatus.RunWorkerAsync();
            }

            //  Parallel.Invoke(GetBetsData, GetLiabalities, GetAllMarketsOpenedbyUser);
            // await DoOperationsConcurrentlyAsync(cancellationToken);
            //   await DoOperationsConcurrentlyAsyncBets(cancellationToken);
            //    await DoOperationsConcurrentlyAsyncLiabalites(cancellationToken);
            bsyindicator.IsBusy = false;

        }

        private void ObjUsersServiceCleint_GetLiveTVChanelsCompleted(object sender, GetLiveTVChanelsCompletedEventArgs e)
        {
            List<LiveTVChannels> lstTVChannels = JsonConvert.DeserializeObject<List<LiveTVChannels>>(e.Result);
           LoggedinUserDetail.TvChannels = lstTVChannels;
        }

        private void ObjUsersServiceCleint_GetMarketRulesCompleted(object sender, GetMarketRulesCompletedEventArgs e)
        {
            try
            {
                LoggedinUserDetail.MarketRulesAll = JsonConvert.DeserializeObject<List<MarketRules>>(e.Result);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch(System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
            }

        private void ObjUsersServiceCleint_GetStartingBalanceCompleted(object sender, GetStartingBalanceCompletedEventArgs e)
        {
            try
            {
                txtStartBalance.Content = Convert.ToInt64(e.Result).ToString();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void ObjUsersServiceCleint_GetCurrentBalancebyUserCompleted(object sender, GetCurrentBalancebyUserCompletedEventArgs e)
        {

            Decimal CurrentAccountBalance = Convert.ToDecimal(e.Result);
            txtAvailBalance.Content = Convert.ToInt64(CurrentAccountBalance).ToString();
        }

        private void ObjUsersServiceCleint_GetAllowedMarketsbyUserIDCompleted(object sender, GetAllowedMarketsbyUserIDCompletedEventArgs e)
        {
            LoggedinUserDetail.AllowedMarketsForUser = JsonConvert.DeserializeObject<AllowedMarkets>(e.Result);
        }

        private void btnUpdateBetSlipKeys_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                objUsersServiceCleint.UpdateBetSlipKeys(LoggedinUserDetail.GetUserID(), txtSimpleBtn1.Text, txtSimpleBtn2.Text, txtSimpleBtn3.Text, txtSimpleBtn4.Text, txtSimpleBtn5.Text, txtSimpleBtn6.Text, txtSimpleBtn7.Text, txtSimpleBtn8.Text, txtSimpleBtn9.Text, "0", "0", "0", txtMultpleBtn1.Text, txtMultpleBtn2.Text, txtMultpleBtn3.Text, txtMultpleBtn4.Text, txtMultpleBtn5.Text, txtMultpleBtn6.Text, txtMultpleBtn7.Text, txtMultpleBtn8.Text, txtMultpleBtn9.Text, "0", "0", "0");
                Properties.Settings.Default.DefaultStakeBack = Convert.ToDouble(txtDefaultStakeBack.Text);
                Properties.Settings.Default.DefaultStakeBackMultiple = Convert.ToDouble(txtDefaultStakeBackMultiple.Text);
                Properties.Settings.Default.DefaultStakeLay = Convert.ToDouble(txtDefaultStakeLay.Text);
                Properties.Settings.Default.DefaultStakeLayMultiple = Convert.ToDouble(txtDefaultStakeLayMultiple.Text);
                Xceed.Wpf.Toolkit.MessageBox.Show(this, "Updated Successfully");
                LoggedinUserDetail.objBetSlipKeys = JsonConvert.DeserializeObject<HelperClasses.BetSlipKeys>(objUsersServiceCleint.GetBetSlipKeys(LoggedinUserDetail.GetUserID()));
                SetBetSlipKeys();
            }
            catch (System.Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(this, "Please enter correct values");
            }
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
              

                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    double laboddmarket = 0;
                    double othermarket = 0;
                    laboddmarket = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.CurrentUserBets);
                    othermarket = objUserBets.GetLiabalityofCurrentUserfancy(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.CurrentUserBets);
                    LoggedinUserDetail.TotalLiabality = laboddmarket + othermarket;
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {

                        LoggedinUserDetail.TotalLiabality = objUserBets.GetLiabalityofCurrentAgent(LoggedinUserDetail.CurrentAgentBets);

                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {

                            LoggedinUserDetail.TotalLiabality = objUserBets.GetLiabalityofAdmin(LoggedinUserDetail.CurrentAdminBets.ToList());

                        }
                        else
                        {
                            if(LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                
                                LoggedinUserDetail.TotalLiabality = objUserBets.GetLiabalityofSuper(LoggedinUserDetail.CurrentSuperBets.ToList());
                            }
                            if (LoggedinUserDetail.GetUserTypeID() == 9)
                            {

                                LoggedinUserDetail.TotalLiabality = objUserBets.GetLiabalityofSamiadmin(LoggedinUserDetail.CurrentsamiadminBets.ToList());
                            }
                        }
                    }
                }
                string accountbalance = objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.user.ID, LoggedinUserDetail.PasswordForValidate);
                LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(accountbalance);
                
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    UserServiceReference.SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                    LoggedinUserDetail.NetBalance = objUsersServiceCleint.GetProfitorLossbyUserID(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), false, LoggedinUserDetail.PasswordForValidate);
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 8)
                    {
                        decimal TotAdminAmount = 0;
                        decimal TotAdmincommession = 0;
                        decimal TotalAdminAmountWithoutMarkets = 0;
                        List<UserAccounts> AgentCommission = new List<UserAccounts>();


                        List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByIDForSuper(LoggedinUserDetail.GetUserID(), false, LoggedinUserDetail.PasswordForValidate));
                        if (lstUserAccountsForAgent.Count > 0) 
                        {
                            AgentCommission = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle == "Commission").ToList();
                            lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                            foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                            {
                                if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                                {
                                    int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                    int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                    int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                    int SamiadminRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                    decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                    decimal superpercent = SuperRate - AgentRate;
                                    decimal superpercentfinal = superpercent + AgentRate;

                                    if (ActualAmount > 0)
                                    {
                                        decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                        decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                        decimal Comissionamount = 0;
                                        if (AgentRate == 100)
                                        {
                                            Comissionamount = 0;
                                        }
                                        else
                                        {
                                            Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                                        }

                                        TotAdminAmount += -1 * (ActualAmount - SuperAmount - AgentAmount );
                                    }
                                    else
                                    {
                                        ActualAmount = -1 * ActualAmount;
                                        decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                        decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                        TotAdminAmount += ActualAmount - (AgentAmount + SuperAmount);
                                    }


                                }

                            }
                            // NetBalance =;
                        }
                     
                     
                       // decimal AgentCommission = 0;

                        try
                        {
                            foreach (UserAccounts objuserAccounts in AgentCommission)
                            {
                                int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                decimal superpercent = SuperRate - AgentRate;
                                ActualAmount = -1 * ActualAmount;
                                decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                TotAdmincommession += ActualAmount - AgentAmount - SuperAmount;
                            }
                                //AgentCommission = objUsersServiceCleint.GetTotalAgentCommissionbyAgentID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate);
                        }
                        catch (System.Exception ex)
                        {


                        }

                        LoggedinUserDetail.NetBalance = (-1 * (TotAdminAmount) + (-1 * TotAdmincommession)) ;
                        LoggedinUserDetail.CurrentAvailableBalance = Convert.ToDouble( LoggedinUserDetail.NetBalance) + LoggedinUserDetail.TotalLiabality + Convert.ToDouble(txtStartBalance.Content);

                    }

                    if (LoggedinUserDetail.GetUserTypeID() == 9)
                    {
                        decimal TotAdminAmount = 0;
                        decimal TotAdmincommession = 0;
                        decimal TotalAdminAmountWithoutMarkets = 0;
                        List<UserAccounts> AgentCommission = new List<UserAccounts>();


                        List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByIDForSamiAdmin(LoggedinUserDetail.GetUserID(), false, LoggedinUserDetail.PasswordForValidate));
                        if (lstUserAccountsForAgent.Count > 0)
                        {
                            AgentCommission = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle == "Commission").ToList();
                            lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                            foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                            {
                                if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                                {
                                    int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                    int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                    int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                    int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiAdminRate);
                                    decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                    decimal superpercent = SuperRate - AgentRate;
                                    decimal samiadminpercent = SamiadminRate -(superpercent + AgentRate);
                                    if (ActualAmount > 0)
                                    {
                                        decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                        decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                        decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                        decimal Comissionamount = 0;
                                        if (AgentRate == 100)
                                        {
                                            Comissionamount = 0;
                                        }
                                        else
                                        {
                                            Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                                        }

                                        TotAdminAmount += -1 * (ActualAmount -( SuperAmount + AgentAmount + SamiadminAmount));
                                    }
                                    else
                                    {
                                        ActualAmount = -1 * ActualAmount;
                                        decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                        decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                        decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                        TotAdminAmount += ActualAmount - (AgentAmount + SuperAmount + SamiadminAmount);
                                    }


                                }

                            }
                            // NetBalance =;
                        }


                        // decimal AgentCommission = 0;

                        try
                        {
                            foreach (UserAccounts objuserAccounts in AgentCommission)
                            {
                                int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiAdminRate);
                                decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                decimal superpercent = SuperRate - AgentRate;
                                decimal samiadminpercent = SamiadminRate - (superpercent + AgentRate);
                                ActualAmount = -1 * ActualAmount;
                                decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                TotAdmincommession += ActualAmount - (AgentAmount + SuperAmount + SamiadminAmount);
                            }
                            //AgentCommission = objUsersServiceCleint.GetTotalAgentCommissionbyAgentID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate);
                        }
                        catch (System.Exception ex)
                        {


                        }

                        LoggedinUserDetail.NetBalance = (-1 * (TotAdminAmount) + (-1 * TotAdmincommession));
                        LoggedinUserDetail.CurrentAvailableBalance = Convert.ToDouble(LoggedinUserDetail.NetBalance) + LoggedinUserDetail.TotalLiabality + Convert.ToDouble(txtStartBalance.Content);

                    }

                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 3)
                        {
                            LoggedinUserDetail.NetBalance = objUsersServiceCleint.GetProfitorLossbyUserID(LoggedinUserDetail.user.ID, false, LoggedinUserDetail.PasswordForValidate);
                            LoggedinUserDetail.CurrentAvailableBalance = Convert.ToDouble(LoggedinUserDetail.NetBalance) + LoggedinUserDetail.TotalLiabality + Convert.ToDouble(txtStartBalance.Content);

                        }
                        else

                        {
                            // LoggedinUserDetail.NetBalance = objUsersServiceCleint.GetProfitorLossbyUserID(LoggedinUserDetail.user.ID, false, LoggedinUserDetail.PasswordForValidate);
                            if (LoggedinUserDetail.GetUserTypeID() == 2)
                            {
                                decimal TotAdminAmount = 0;
                                decimal SuperAmount = 0;
                                decimal SuperAmount1 = 0;
                                decimal TotalAdminAmountWithoutMarkets = 0;
                                List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(LoggedinUserDetail.GetUserID(), false, LoggedinUserDetail.PasswordForValidate));
                                if (lstUserAccountsForAgent.Count > 0)
                                {
                                    lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                                    foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                                    {
                                        if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                                        {

                                            int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                            int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                            int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                            decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                            decimal superpercent = 0;
                                            if (SuperRate > 0)
                                            {
                                                superpercent = SuperRate - AgentRate;
                                            }
                                            else
                                            {
                                                superpercent = 0;
                                            }
                                            if (ActualAmount > 0)
                                            {
                                                SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                decimal Comissionamount = 0;
                                                if (AgentRate == 100)
                                                {
                                                    Comissionamount = 0;
                                                }
                                                else
                                                {
                                                    Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                                                }

                                                TotAdminAmount += -1 * (ActualAmount - (AgentAmount + SuperAmount) - Comissionamount);
                                                SuperAmount1 += -1 * SuperAmount;
                                            }
                                            else
                                            {
                                                ActualAmount = -1 * ActualAmount;
                                                SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                TotAdminAmount += ActualAmount - (AgentAmount + SuperAmount);
                                                SuperAmount1 += SuperAmount;
                                            }


                                        }

                                    }

                                }
                                //int cretedbyid = objUsersServiceCleint.GetCreatedbyid(LoggedinUserDetail.GetUserID());
                                int createdbyid = objUsersServiceCleint.GetCreatedbyID(LoggedinUserDetail.GetUserID());


                                List<UserAccounts> lstAccountsDonebyAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(createdbyid, false, LoggedinUserDetail.PasswordForValidate));
                                if (lstAccountsDonebyAdmin.Count > 0)
                                {
                                    List<UserAccounts> lstAccountsDonebyAdminagainstthisAgent = lstAccountsDonebyAdmin.Where(item => item.AccountsTitle.Contains("(UserID=" + LoggedinUserDetail.GetUserID().ToString() + ")")).ToList();
                                    if (lstAccountsDonebyAdminagainstthisAgent.Count > 0)
                                    {
                                        TotalAdminAmountWithoutMarkets = lstAccountsDonebyAdminagainstthisAgent.Sum(item => Convert.ToDecimal(item.Debit)) - lstAccountsDonebyAdminagainstthisAgent.Sum(item => Convert.ToDecimal(item.Credit));
                                    }
                                }
                                decimal AgentCommission = 0;
                                decimal superCommission = 0;

                                try
                                {
                                    AgentCommission = objUsersServiceCleint.GetTotalAgentCommissionbyAgentID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate);
                                }
                                catch (System.Exception ex)
                                {
                                }

                                LoggedinUserDetail.NetBalance = (-1 * (TotAdminAmount) + (-1 * TotalAdminAmountWithoutMarkets) + (-1 * (SuperAmount1))) + AgentCommission;
                                LoggedinUserDetail.CurrentAvailableBalance = Convert.ToDouble(LoggedinUserDetail.NetBalance) + LoggedinUserDetail.TotalLiabality + Convert.ToDouble(txtStartBalance.Content);

                            }
                        }
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


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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


                if (DGVInPlaymarketsall.Items.Count > 0)
                {
                    DataGrid objSender = (DataGrid)sender;
                    AllMarketsInPlay objSelectedRow = (AllMarketsInPlay)DGVInPlaymarketsall.SelectedItem;
                    string MarketBookID = objSelectedRow.MarketBookID;
                    bsyindicator.IsBusy = true;
                    MarketBook(MarketBookID);


                    //await MarketBook(objSelectedRow.MarketBookID).ContinueWith((x) =>
                    //{
                    //    bsyindicator.IsBusy = false;
                    //});

                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {

                }
                Environment.Exit(0);
                //this.Close();
               // Application.Current.Shutdown();
               
            }
        }

        private void btnUserDetails_Click(object sender, RoutedEventArgs e)
        {
            if (plusminusUser.Content.ToString() == "+")
            {
                plusminusUser.Content = "-";
                stkpnlUserDetails.Visibility = Visibility.Visible;
                if (1==1)
                {
                    UpdateUserLiablity();
                    txtAvailBalance.Content = ((long)LoggedinUserDetail.CurrentAvailableBalance).ToString("N0");
                    txtTotalLiabality.Content = LoggedinUserDetail.TotalLiabality.ToString("N0");
                    // txtCurrentLiabality.Text = CurrentLiabality.ToString();
                    txtBalance.Content = LoggedinUserDetail.NetBalance.ToString("N0");
                    if (showpendingamountlabel == true)
                    {
                        lblShowPaymentPending.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lblShowPaymentPending.Visibility = Visibility.Collapsed;
                    }
                }
            }
            else
            {
                plusminusUser.Content = "+";
                stkpnlUserDetails.Visibility = Visibility.Collapsed;
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
                //  PlayMessage("Some changes made on server, please restart the application.");
                // MessageBox.Show("Some changes made on server, please restart the application.");

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
            lastselectedindex = tabctrlMarketsAll.SelectedIndex;
            tabctrlMarketsAll.SelectedIndex = 0;
            LedgerWindow objFrmLedger = new LedgerWindow();
            objFrmLedger.UserIDforLedger = LoggedinUserDetail.GetUserID();
            objFrmLedger.ShowDialog();
            tabctrlMarketsAll.SelectedIndex = lastselectedindex;
        }

        private void btnProfitLoss_Click(object sender, RoutedEventArgs e)
        {
            lastselectedindex = tabctrlMarketsAll.SelectedIndex;
            tabctrlMarketsAll.SelectedIndex = 0;
            ProfitLossWindow objPLWin = new ProfitLossWindow();
            objPLWin.ShowDialog();
            tabctrlMarketsAll.SelectedIndex = lastselectedindex;
        }
        public int lastselectedindex = 0;
        private void btnAdminpanel_Click(object sender, RoutedEventArgs e)
        {
            lastselectedindex = tabctrlMarketsAll.SelectedIndex;
            tabctrlMarketsAll.SelectedIndex = 0;
            AdminPanelWindow objAdmin = new AdminPanelWindow();
            objAdmin.ShowDialog();
            tabctrlMarketsAll.SelectedIndex = lastselectedindex;
        }

        private void btnAccountRece_Click(object sender, RoutedEventArgs e)
        {
            lastselectedindex = tabctrlMarketsAll.SelectedIndex;
            tabctrlMarketsAll.SelectedIndex = 0;
            AccountsReceiveableWindow objAccWindow = new AccountsReceiveableWindow();
            objAccWindow.ShowDialog();
            tabctrlMarketsAll.SelectedIndex = lastselectedindex;
        }

        private void lblShowPaymentPending_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lastselectedindex = tabctrlMarketsAll.SelectedIndex;
            tabctrlMarketsAll.SelectedIndex = 0;
            AccountsReceiveableWindow objAccWindow = new AccountsReceiveableWindow();
            objAccWindow.ShowDialog();
            tabctrlMarketsAll.SelectedIndex = lastselectedindex;
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


                    //await MarketBook(objSelectedRow.ID).ContinueWith((x) =>
                    //{
                    //    bsyindicator.IsBusy = false;
                    //});

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
            //bool isAgentcom = objUsersServiceCleint.GetIsComAllowbyUserID(CreatedbyID);  
            // GetUpdate("1");
            // GetINFancy("1.192514871");
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

        private void tabctrlMarketsAll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (tabctrlMarketsAll.SelectedIndex > 0)
                {
                    var selecteditem = (ClosableTab)tabctrlMarketsAll.SelectedItem;
                    marketbookIDselected = selecteditem.Tag.ToString();
                }


            }
            catch (System.Exception ex)
            {

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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                bsyindicator.IsBusy = false;

            }
        }

    
    }
}

