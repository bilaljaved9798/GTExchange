using bftradeline.Models;
using ExternalAPI.TO;
using globaltraders.Service123Reference;
using globaltraders.UserServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for AdminBetsWindow.xaml
    /// </summary>
    public partial class AdminBetsWindow : Window
    {
        public AdminBetsWindow()
        {
            InitializeComponent();
            DGVMatchedBetsAdmin.Height = 135;
            timerCountdown.Interval = TimeSpan.FromMilliseconds(100);

            timerCountdown.Tick += TimerCountdown_Tick;
            lstCurrentBetsAdmin = new ObservableCollection<UserBetsForAdmin>();
            lstCurrentBetsAdminUnMAtched = new ObservableCollection<UserBetsForAdmin>();
            this.DGVMatchedBetsAdmin.DataContext = lstCurrentBetsAdmin;
            this.DGVUnMatchedAdmin.DataContext = lstCurrentBetsAdminUnMAtched;
            backgroundWorkerUpdateData = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            backgroundWorkerUpdateData.DoWork += BackgroundWorkerUpdateData_DoWork;
            backgroundWorkerUpdateData.RunWorkerCompleted += BackgroundWorkerUpdateData_RunWorkerCompleted;
        }

        private void BackgroundWorkerUpdateData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {


                UpdateUserBetsData();
                UpdateUserBetsDataUnMatched();
                BackgroundWorker worker = sender as BackgroundWorker;
                if (worker.CancellationPending == true)
                {
                    return;
                }
                backgroundWorkerUpdateData.RunWorkerAsync();
            }
            catch (System.Exception ex)
            {

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

                popupCuttingBets.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(placePopup);
                List<UserBetsForAdmin> lstCurrentBetsMatched = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true && item.UserTypeID == 4).OrderByDescending(item => item.ID).ToList();
                DGVAllCuttingBets.ItemsSource = lstCurrentBetsMatched;

            }
        }
        public CustomPopupPlacement[] placePopup(Size popupsize, Size targetSize, Point offset)
        {
            CustomPopupPlacement placment1 = new CustomPopupPlacement(new Point(0, 800), PopupPrimaryAxis.Vertical);
            CustomPopupPlacement placment2 = new CustomPopupPlacement(new Point(10, 20), PopupPrimaryAxis.Horizontal);
            CustomPopupPlacement[] ttplaces = new CustomPopupPlacement[] { placment1, placment2 };
            return ttplaces;
        }

        public UserServicesClient objUsersServiceCleint = new UserServicesClient();
        private void DGVAllCuttingBets_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {


            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                if (DGVAllCuttingBets.CurrentCell.Column.DisplayIndex == 7)

                {

                    UserBetsForAdmin objSelectedRow = (UserBetsForAdmin)DGVAllCuttingBets.CurrentCell.Item;
                    long BetID = objSelectedRow.ID;

                    MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show(Application.Current.MainWindow, "Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        objUsersServiceCleint.UpdateUserBetUnMatchedStatusTocompleteforCuttingUser(BetID, LoggedinUserDetail.PasswordForValidate);

                        List<UserBetsForAdmin> lstCurrentBetsMatched = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true && item.UserTypeID == 4 && item.ID != Convert.ToInt64(BetID)).ToList();
                        DGVAllCuttingBets.ItemsSource = lstCurrentBetsMatched;

                    }
                }
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
                System.Threading.Thread.Sleep(100);
            }
            catch (System.Exception ex)
            {

            }
        }

        private ObservableCollection<UserBetsForAdmin> lstCurrentBetsAdmin;
        private ObservableCollection<UserBetsForAdmin> lstCurrentBetsAdminUnMAtched;
        public BackgroundWorker backgroundWorkerUpdateData;
        private void TimerCountdown_Tick(object sender, EventArgs e)
        {
            try
            {


                UpdateUserBetsData();
                UpdateUserBetsDataUnMatched();
            }
            catch (System.Exception ex)
            {

            }
        }

        public DispatcherTimer timerCountdown = new DispatcherTimer();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                backgroundWorkerUpdateData.RunWorkerAsync();

            }
            catch (System.Exception ex)
            {

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
                    var result = lstMatchbets.Take(12).Where(p => !lstCurrentBetsAdmin.Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));

                    var result1 = lstCurrentBetsAdmin.Where(p => !lstMatchbets.Take(12).Any(p2 => p2.ID == p.ID && p2.Amount == p.Amount));
                    if (result.Count() > 0 || result1.Count() > 0)
                    {
                        lstCurrentBetsAdmin.Clear();
                        foreach (var item in lstMatchbets.Take(12))
                        {
                            if (item.DealerName == "Admin")
                            {
                                item.DealerName = item.AgentName;
                            }
                            if (item.location == "8")
                            {
                                lstCurrentBetsAdmin.Add(new UserBetsForAdmin() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = (Convert.ToDecimal(item.BetSize) / 100).ToString(), Amount = item.Amount, LiveOdd = item.UserOdd.ToString(), CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID, location = item.location }); ;
                            }
                            else
                            {
                                if (item.location == "9")
                                {
                                    lstCurrentBetsAdmin.Add(new UserBetsForAdmin() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.BetSize, Amount = item.Amount, LiveOdd = item.LiveOdd, CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID, location = item.location }); ;
                                }
                                else
                                {
                                    lstCurrentBetsAdmin.Add(new UserBetsForAdmin() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID, UserID = item.UserID });
                                }
                            }
                           
                        }
                    }
                    lblMatchBetsCount.Content = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true).Count().ToString();
                }
            }
            catch (System.Exception ex)
            {

            }
        }
        public void PlayMessage(string msg)
        {
            System.Speech.Synthesis.SpeechSynthesizer synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = 1;     // -10...10


            synthesizer.SelectVoiceByHints(System.Speech.Synthesis.VoiceGender.Neutral, System.Speech.Synthesis.VoiceAge.Adult);

            synthesizer.SpeakAsync(msg);
        }
        public void UpdateUserBetsDataUnMatched()
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {

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

            }
            catch (System.Exception ex)
            {

            }
        }

        //private void TextBlock_MouseLeftButtonUp_3(object sender, MouseButtonEventArgs e)
        //{
        //    if (txtTopMost.Tag.ToString() == "0")
        //    {
        //        txtTopMost.Tag = "1";
        //        this.Topmost = true;
        //        RotateTransform rotateTransform = new RotateTransform(0, 0.5, 0.5);


        //        pinTopMost.RenderTransform = rotateTransform;
        //    }
        //    else
        //    {
        //        txtTopMost.Tag = "0";
        //        this.Topmost = false;
        //        RotateTransform rotateTransform = new RotateTransform(90, 0.5, 0.5);

        //        pinTopMost.RenderTransform = rotateTransform;
        //    }
        //}

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void TextBlock_MouseLeftButtonUp_2(object sender, MouseButtonEventArgs e)
        {
            this.Close();
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


                        foreach (Window win in App.Current.Windows)
                        {

                            if (win.Name == "mainwindow")
                            {
                                MainWindow window = win as MainWindow;
                                window.bsyindicator.IsBusy = true;

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




        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            popupAllBets.IsOpen = false;
        }

        private void btnAllBets_Click(object sender, RoutedEventArgs e)
        {
            popupAllBets.IsOpen = true;
            UpdateUserBetsDataAll();

        }

        private void btnAllBetsRefresh_Click(object sender, RoutedEventArgs e)
        {
            UpdateUserBetsDataAll();
        }
        public void UpdateUserBetsDataAll()
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    List<UserBetsForAdmin> lstMatchbets = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true).ToList();
                    lstMatchbets = lstMatchbets.OrderByDescending(item => item.ID).ToList();
                    
                    lstMatchbets.Where(w => w.location == "8").ToList().ForEach(i =>
                    {
                        i.UserOdd = (Convert.ToDouble(i.BetSize) / 100).ToString();
                        i.LiveOdd = i.LiveOdd;
                    });
                    lstMatchbets.Where(w => w.location == "9").ToList().ForEach(i =>
                    {
                        i.UserOdd = i.BetSize;
                        i.LiveOdd = i.LiveOdd;                       
                    });

                    DGVMatchedBetsAdminAll.ItemsSource = lstMatchbets;

                    lblAllMatchedBets.Content = "Matched Bets " + LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true).Count().ToString();

                }

            }
            catch (System.Exception ex)
            {

            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                backgroundWorkerUpdateData.CancelAsync();
                backgroundWorkerUpdateData.Dispose();
                backgroundWorkerUpdateData = null;
                GC.Collect();

                timerCountdown.Tick -= TimerCountdown_Tick;
                timerCountdown.Stop();
            }
            catch (System.Exception ex)
            {

            }
        }

        private void AdminBetsWin_LocationChanged(object sender, EventArgs e)
        {
            try
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
            catch (System.Exception ex)
            {


            }
        }
        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            popupProffitLossAgent.IsOpen = false;
            //  isProfitLossbyAgentShown = false;
        }
        public Service123Client objBettingClient = new Service123Client();
        private void DGVMatchedBetsAdmin_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {

                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    DataGrid objSender = (DataGrid)sender;
                    int currcellindx = objSender.CurrentCell.Column.DisplayIndex;
                    if (currcellindx == 0)
                    {
                        UserBetsForAdmin objselectedmarket = new UserBetsForAdmin();
                        objselectedmarket = null;
                        objselectedmarket = (UserBetsForAdmin)objSender.SelectedItem;
                        if (objselectedmarket.location == "9")
                        {
                            var result = JsonConvert.DeserializeObject(objUsersServiceCleint.GetMarketIDbyEventID(objselectedmarket.MarketBookID));
                            if (result != null)
                            {
                                objselectedmarket.MarketBookID = result.ToString();
                            }
                        }

                        foreach (Window win in App.Current.Windows)
                        {

                            if (win.Name == "mainwindow")
                            {
                                MainWindow window = win as MainWindow;
                                window.bsyindicator.IsBusy = true;

                                window.MarketBook(objselectedmarket.MarketBookID);
                                return;


                            }

                        }
                    }
                    else
                    {

                        UserBetsForAdmin objselectedmarket = (UserBetsForAdmin)objSender.SelectedItem;
                        var results = JsonConvert.DeserializeObject<List<MarketCatalgoue>>(objUsersServiceCleint.GetMarketsOpenedbyUsersForAdmin());
                        var newmarkettobeopened = results.Where(item3 => item3.ID == objselectedmarket.MarketBookID).FirstOrDefault();
                        string[] marketIds = new string[]
                     {
                    objselectedmarket.MarketBookID
                     };

                        var marketbook = objBettingClient.GetMarketDatabyID(marketIds, newmarkettobeopened.SheetName, newmarkettobeopened.EventOpenDate, "Cricket", LoggedinUserDetail.PasswordForValidate);
                        mb = marketbook[0];
                        user.Content = objselectedmarket.UserID;
                       // Runner0.Content = mb.Runners[0].SelectionId;
                        //Runner1.Content = mb.Runners[1].SelectionId;
                        //if (mb.Runners.Count == 3)
                        //{
                        //    Runner2.Content = mb.Runners[2].SelectionId;
                        //}
                        popupProffitLossAgent.IsOpen = true;
                        var results2 = objUsersServiceCleint.GetSelectionNamesbyMarketID(objselectedmarket.MarketBookID);


                        Runner0.ItemsSource = results2;
                        Runner0.DisplayMemberPath = "SelectionName";
                        Runner0.SelectedValuePath = "SelectionID";

                        Runner0.IsSynchronizedWithCurrentItem = false;
                        Runner0.SelectedIndex = 0;

                        //List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
                        //lstRunners = lastloadedmarket.Runners;
                        Runner1.ItemsSource = results2;
                        Runner1.DisplayMemberPath = "SelectionName";
                        Runner1.SelectedValuePath = "SelectionID";
                        Runner1.SelectedIndex = 1;

                        if (results2.Count() == 3)
                        {

                            Runner2.IsSynchronizedWithCurrentItem = false;
                            Runner2.ItemsSource = results2;
                            Runner2.DisplayMemberPath = "SelectionName";
                            Runner2.SelectedValuePath = "SelectionID";
                            Runner2.SelectedIndex = 2;
                            lblProfitandLossRunnerbyAgent3.Content = "0";
                        }
                        else
                        {
                            Runner2.Visibility = Visibility.Collapsed;
                            lblProfitandLossRunnerbyAgent3.Visibility = Visibility.Collapsed;
                        }
                    }
                    
                    data();

                }


            }
            catch (System.Exception ex)
            {

            }
        }
        MarketBook mb = new MarketBook();
        public void data()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                string userbets = objUsersServiceCleint.GetUserbetsbyUserID(Convert.ToInt32(user.Content), LoggedinUserDetail.PasswordForValidate);
                List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(userbets);
                if (lstUserBets.Count > 0)
                {
                    UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                    List<UserBets> lstUserBets1 = lstUserBets.Where(item2 => item2.isMatched == true && item2.MarketBookID == mb.MarketId).ToList();

                    var currentusermarketbook = mb;
                    currentusermarketbook.DebitCredit = objUserBets.ceckProfitandLoss(currentusermarketbook, lstUserBets1);

                    foreach (var runner in currentusermarketbook.Runners)
                    {
                        runner.ProfitandLoss = Convert.ToInt64(currentusermarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - currentusermarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                    }

                    double runner1profit = 0;
                    double runner2profit = 0;
                    ExternalAPI.TO.Runner runner1 = currentusermarketbook.Runners.Where(item => item.SelectionId == Runner0.SelectedValue.ToString()).FirstOrDefault();
                    ExternalAPI.TO.Runner runner2 = currentusermarketbook.Runners.Where(item => item.SelectionId == Runner1.SelectedValue.ToString()).FirstOrDefault();
                    if (runner1 != null)
                    {
                        runner1profit = runner1.ProfitandLoss;
                        if (lblProfitorLossRunnerAgent1.Content.ToString() != runner1profit.ToString("N0"))
                        {
                           // Runner0name.Content = currentusermarketbook.Runners[0].RunnerName;
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
                            //Runner1name.Content = currentusermarketbook.Runners[1].RunnerName;
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
                    if (currentusermarketbook.Runners.Count == 3)
                    {
                        ExternalAPI.TO.Runner runner3 = currentusermarketbook.Runners.Where(item => item.SelectionId == Runner2.SelectedValue.ToString()).FirstOrDefault();
                        if (runner3 != null)
                        {
                            if (lblProfitandLossRunnerbyAgent3.Content.ToString() != runner3.ProfitandLoss.ToString("N0"))
                            {
                                //Runner2name.Content = currentusermarketbook.Runners[2].RunnerName;
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
                }
                else
                {
                    //lblCurrentPostitionUserPL1.Content = "0";
                }

            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
    }

