using bftradeline.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                System.Threading.Thread.Sleep(100);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        public DispatcherTimer timerCountdown = new DispatcherTimer();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                backgroundWorkerUpdateData.RunWorkerAsync();
                // this.Height = App.Current.Windows[0].Height;
             //  timerCountdown.Start();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                //  timerCountdown.Start();
            }
        }
        public void UpdateUserBetsData()
        {
            try
            {

                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    //  UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
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
                            if (item.location == "9")
                            {
                                lstCurrentBetsAdmin.Add(new UserBetsForAdmin() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.BetSize, Amount = item.Amount, LiveOdd = item.LiveOdd, CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID });
                            }
                            else
                            {
                                lstCurrentBetsAdmin.Add(new UserBetsForAdmin() { ID = item.ID, SelectionID = item.SelectionID, SelectionName = item.SelectionName, UserOdd = item.UserOdd, Amount = item.Amount, LiveOdd = item.LiveOdd, CustomerName = item.CustomerName, DealerName = item.DealerName, BetType = item.BetType, MarketBookID = item.MarketBookID });
                            }
                        }

                    }
                    lblMatchBetsCount.Content = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true).Count().ToString();

                }


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

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
        public void UpdateUserBetsDataUnMatched()
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                   // UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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

                        //MarketBookFunc(objselectedmarket.MarketBookID);
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                //  window.bsyindicator.IsBusy = false;
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
                    DGVMatchedBetsAdminAll.ItemsSource = lstMatchbets;

                    lblAllMatchedBets.Content = "Matched Bets " + LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.isMatched == true).Count().ToString();

                }



            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {


            }
        }
    }
}
