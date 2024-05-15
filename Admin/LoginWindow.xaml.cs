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
using System.Windows.Shapes;
using globaltraders.UserServiceReference;
using globaltraders.APIConfigServiceReference;
using Newtonsoft.Json;
using bftradeline.Models;
using Xceed.Wpf.Toolkit;
using System.Runtime.CompilerServices;

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        UserServicesClient objUserServiceClient = new UserServicesClient();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var spinner = FindVisualChild<Image>(button);
            try
            {

                if (txtPassword.Password == "" || txtUsername.Text == "")
                {
                    lblError.Text = "Please enter username and password.";
                    lblError.Visibility = Visibility.Visible;
                }
                else
                {
                    // Show spinner
                    lblError.Visibility = Visibility.Hidden;
                    spinner.Visibility = Visibility.Visible;
                    button.IsEnabled = false;


                    objUserServiceClient.GetUserbyUsernameandPasswordAsync(Crypto.Encrypt(txtUsername.Text), Crypto.Encrypt(txtPassword.Password));
                    objUserServiceClient.GetUserbyUsernameandPasswordCompleted += ObjUserServiceClient_GetUserbyUsernameandPasswordCompleted;


                }
            }
            catch (System.Exception ex)
            {
                spinner.Visibility = Visibility.Collapsed;
                lblError.Text = "Error occured pleae try again.";
                lblError.Visibility = Visibility.Visible;
                //bsyindicator.IsBusy = false;
                button.IsEnabled = true;

            }
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private async void ObjUserServiceClient_GetUserbyUsernameandPasswordCompleted(object sender, GetUserbyUsernameandPasswordCompletedEventArgs e)
        {
            var spinner = FindVisualChild<Image>(button);
            try
            {              
                if (e.Result != "")
                {
                    var result = JsonConvert.DeserializeObject<UserIDandUserType>(e.Result);
                    if (result.isBlocked == true)
                    {
                        spinner.Visibility = Visibility.Collapsed;
                        lblError.Text = "Account is blocked.";
                        lblError.Visibility = Visibility.Visible;
                        //bsyindicator.IsBusy = false;
                        button.IsEnabled = true;
                        return;
                    }
                    if (result.isDeleted == true)
                    {
                        spinner.Visibility = Visibility.Collapsed;
                        lblError.Text = "Account is deleted.";
                        lblError.Visibility = Visibility.Visible;
                        //bsyindicator.IsBusy = false;
                        button.IsEnabled = true;
                        return;
                    }
                    LoggedinUserDetail.PasswordForValidate = result.PasswordforValidate;
                    LoggedinUserDetail.PasswordForValidateS = result.PasswordforValidateS;
                    if (result.UserTypeID != 1)
                    {
                        if (result.Loggedin == true)
                        {
                            objUserServiceClient.SetLoggedinStatusAsync(result.ID, false);
                            await Task.Delay(10000);
                            //System.Threading.Thread.Sleep(10000);
                            LoggedinUserDetail.user = result;
                            objUserServiceClient.SetLoggedinStatusAsync(result.ID, true);
                            LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(LoggedinUserDetail.GetUserID()));
                            LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                            // button.IsEnabled = true;
                            LoggedinUserDetail.InsertActivityLog(result.ID, "Logged In");
                            Properties.Settings.Default.Username = txtUsername.Text;
                            Properties.Settings.Default.Password = txtPassword.Password;
                            Properties.Settings.Default.Save();
                            this.Hide();
                            MainWindow objmainwindow = new MainWindow();
                            objmainwindow.ShowDialog();
                        }
                        else
                        {


                            LoggedinUserDetail.user = result;
                            objUserServiceClient.SetLoggedinStatus(result.ID, true);
                            LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(LoggedinUserDetail.GetUserID()));
                            LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                            //button.IsEnabled = true;
                            LoggedinUserDetail.InsertActivityLog(result.ID, "Logged In");
                            Properties.Settings.Default.Username = txtUsername.Text;
                            Properties.Settings.Default.Password = txtPassword.Password;
                            Properties.Settings.Default.Save();
                            this.Hide();

                            MainWindow objmainwindow = new MainWindow();
                            objmainwindow.ShowDialog();
                        }
                    }
                    else
                    {
                        LoggedinUserDetail.user = result;
                        objUserServiceClient.SetLoggedinStatus(result.ID, true);
                        APIConfigServiceClient objAPIConfigCleint = new APIConfigServiceClient();

                        LoggedinUserDetail.PoundRate = Convert.ToDecimal(Crypto.Decrypt(objAPIConfigCleint.GetPoundRate()));
                        //button.IsEnabled = true;
                        LoggedinUserDetail.InsertActivityLog(result.ID, "Logged In");
                        Properties.Settings.Default.Username = txtUsername.Text;
                        Properties.Settings.Default.Password = txtPassword.Password;
                        Properties.Settings.Default.Save();
                        this.Hide();
                        MainWindow objmainwindow = new MainWindow();
                        objmainwindow.ShowDialog();
                    }



                }
                else
                {
                    spinner.Visibility = Visibility.Collapsed;
                    lblError.Text = "Invalid Username and password.";
                    lblError.Visibility = Visibility.Visible;
                    //bsyindicator.IsBusy = false;
                    button.IsEnabled = true;

                }
            }
            catch (System.Exception ex)
            {
                spinner.Visibility = Visibility.Collapsed;
                lblError.Text = "Error occured pleae try again.";
                lblError.Visibility = Visibility.Visible;
                //bsyindicator.IsBusy = false;
                button.IsEnabled = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Username != string.Empty)
            {
                txtUsername.Text = Properties.Settings.Default.Username;
                txtPassword.Password = Properties.Settings.Default.Password;
            }
        }
        //private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        //{
        //    MediaPlayer.Visibility = Visibility.Visible;
        //    // MediaPlayer.Source = new Uri(@"../Loader.gif");
        //    MediaPlayer.Position = new TimeSpan(0, 0, 2);
        //    MediaPlayer.Play();
        //}
        //private void MediaPlayer_MediaStart(object sender, RoutedEventArgs e)
        //{
        //    MediaPlayer.Visibility = Visibility.Collapsed;

        //    MediaPlayer.Stop();
        //}
    }
}
