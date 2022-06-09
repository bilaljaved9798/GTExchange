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
using globaltraders.Models;

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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {



                if (txtPassword.Password == "" || txtUsername.Text == "")
                {
                    txtPassword.BorderBrush =Brushes.Red;
                    txtUsername.BorderBrush = Brushes.Red;
                    lblError.Text = "Please enter username and password.";
                    lblError.Visibility = Visibility.Visible;
                }
                else
                {
                    //stkpnlLoader.Visibility = Visibility.Visible;
                    //stkpnlmaincontent.Visibility = Visibility.Hidden;
                    //System.Threading.Thread.Sleep(200);
                    lblError.Visibility = Visibility.Hidden;
                    bsyindicator.IsBusy = true;
                    button.IsEnabled = false;
                   //var results= objUserServiceClient.GetUserbyUsernameandPassword(Crypto.Encrypt(txtUsername.Text), Crypto.Encrypt(txtPassword.Password));
                   // try
                   // {


                   //     if (results != "")
                   //     {

                   //         var result = JsonConvert.DeserializeObject<UserIDandUserType>(results);
                   //         if (result.isBlocked == true)
                   //         {
                   //             lblError.Text = "Account is blocked.";
                   //             lblError.Visibility = Visibility.Visible;
                   //             bsyindicator.IsBusy = false;
                   //             button.IsEnabled = true;
                   //             //stkpnlLoader.Visibility = Visibility.Hidden;
                   //             //stkpnlmaincontent.Visibility = Visibility.Visible;
                   //             return;

                   //         }
                   //         if (result.isDeleted == true)
                   //         {
                   //             lblError.Text = "Account is deleted.";
                   //             lblError.Visibility = Visibility.Visible;
                   //             bsyindicator.IsBusy = false;
                   //             button.IsEnabled = true;
                   //             //stkpnlLoader.Visibility = Visibility.Hidden;
                   //             //stkpnlmaincontent.Visibility = Visibility.Visible;
                   //             return;
                   //         }
                   //         LoggedinUserDetail.PasswordForValidate = result.PasswordforValidate;

                   //         if (result.UserTypeID != 1)
                   //         {
                   //             if (result.Loggedin == true)
                   //             {
                   //                 objUserServiceClient.SetLoggedinStatusAsync(result.ID, false);
                   //                 //await Task.(10000);
                   //                 System.Threading.Thread.Sleep(10000);
                   //                 LoggedinUserDetail.user = result;
                   //                 objUserServiceClient.SetLoggedinStatusAsync(result.ID, true);

                   //                 //PoundRates configData = JsonConvert.DeserializeObject<PoundRates>(objAPIConfigCleint.GetPoundRate());
                   //                 LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(LoggedinUserDetail.GetUserID()));
                   //                 LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                   //                 button.IsEnabled = true;
                   //                 LoggedinUserDetail.InsertActivityLog(result.ID, "Logged In");
                   //                 Properties.Settings.Default.Username = txtUsername.Text;
                   //                 Properties.Settings.Default.Password = txtPassword.Password;
                   //                 Properties.Settings.Default.Save();
                   //                 this.Hide();

                   //                 MainWindow objmainwindow = new MainWindow();
                   //                 objmainwindow.ShowDialog();
                   //             }
                   //             else
                   //             {


                   //                 LoggedinUserDetail.user = result;
                   //                 objUserServiceClient.SetLoggedinStatus(result.ID, true);
                   //                 LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(LoggedinUserDetail.GetUserID()));
                   //                 LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                   //                 button.IsEnabled = true;
                   //                 LoggedinUserDetail.InsertActivityLog(result.ID, "Logged In");
                   //                 Properties.Settings.Default.Username = txtUsername.Text;
                   //                 Properties.Settings.Default.Password = txtPassword.Password;
                   //                 Properties.Settings.Default.Save();
                   //                 this.Hide();

                   //                 MainWindow objmainwindow = new MainWindow();
                   //                 objmainwindow.ShowDialog();
                   //             }
                   //         }
                   //         else
                   //         {
                   //             LoggedinUserDetail.user = result;
                   //             objUserServiceClient.SetLoggedinStatus(result.ID, true);
                   //             APIConfigServiceClient objAPIConfigCleint = new APIConfigServiceClient();

                   //             LoggedinUserDetail.PoundRate = Convert.ToDecimal(Crypto.Decrypt(objAPIConfigCleint.GetPoundRate()));
                   //             button.IsEnabled = true;
                   //             LoggedinUserDetail.InsertActivityLog(result.ID, "Logged In");
                   //             Properties.Settings.Default.Username = txtUsername.Text;
                   //             Properties.Settings.Default.Password = txtPassword.Password;
                   //             Properties.Settings.Default.Save();
                   //             this.Hide();
                   //             MainWindow objmainwindow = new MainWindow();
                   //             objmainwindow.ShowDialog();
                   //         }



                   //     }
                   //     else
                   //     {
                   //         lblError.Text = "Invalid Username and password.";
                   //         lblError.Visibility = Visibility.Visible;
                   //         bsyindicator.IsBusy = false;
                   //         button.IsEnabled = true;
                   //         //stkpnlLoader.Visibility = Visibility.Hidden;
                   //         //stkpnlmaincontent.Visibility = Visibility.Visible;
                   //         //  lblError.Visible = false;
                   //     }
                   // }
                   // catch (System.Exception ex)
                   // {
                   //     lblError.Text = "Error occured pleae try again.";
                   //     lblError.Visibility = Visibility.Visible;
                   //     bsyindicator.IsBusy = false;
                   //     button.IsEnabled = false;
                   // }


                    objUserServiceClient.GetUserbyUsernameandPasswordAsync(Crypto.Encrypt(txtUsername.Text), Crypto.Encrypt(txtPassword.Password));
                    objUserServiceClient.GetUserbyUsernameandPasswordCompleted += ObjUserServiceClient_GetUserbyUsernameandPasswordCompleted;
                    //  var results = objUserServiceClient.GetUserbyUsernameandPassword(Crypto.Encrypt(txtUsername.Text), Crypto.Encrypt(txtPassword.Password));

                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                lblError.Text = "Error occured pleae try again.";
                lblError.Visibility = Visibility.Visible;
                bsyindicator.IsBusy = false;
                button.IsEnabled = true;
                //stkpnlLoader.Visibility = Visibility.Hidden;
                //stkpnlmaincontent.Visibility = Visibility.Visible;
            }
        }

        private void ObjUserServiceClient_GetUserbyUsernameandPasswordCompleted(object sender, GetUserbyUsernameandPasswordCompletedEventArgs e)
        {
            try
            {


                if (e.Result != "")
                {

                    var result = JsonConvert.DeserializeObject<UserIDandUserType>(e.Result);
                    if (result.isBlocked == true)
                    {
                        lblError.Text = "Account is blocked.";
                        lblError.Visibility = Visibility.Visible;
                        bsyindicator.IsBusy = false;
                        button.IsEnabled = true;
                        //stkpnlLoader.Visibility = Visibility.Hidden;
                        //stkpnlmaincontent.Visibility = Visibility.Visible;
                        return;

                    }

                    if (result.isDeleted == true)
                    {
                        lblError.Text = "Account is deleted.";
                        lblError.Visibility = Visibility.Visible;
                        bsyindicator.IsBusy = false;
                        button.IsEnabled = true;
                        //stkpnlLoader.Visibility = Visibility.Hidden;
                        //stkpnlmaincontent.Visibility = Visibility.Visible;
                        return;
                    }
                     LoggedinUserDetail.PasswordForValidate = result.PasswordforValidate;
                     LoggedinUserDetail.PasswordForValidateS = result.PasswordforValidateS;
                    if (result.UserTypeID != 1)
                    {
                        if (result.Loggedin == true)
                        {
                            objUserServiceClient.SetLoggedinStatusAsync(result.ID, false);
                            //await Task.(10000);
                            System.Threading.Thread.Sleep(10000);
                            LoggedinUserDetail.user = result;
                            objUserServiceClient.SetLoggedinStatusAsync(result.ID, true);

                            //PoundRates configData = JsonConvert.DeserializeObject<PoundRates>(objAPIConfigCleint.GetPoundRate());
                            LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(LoggedinUserDetail.GetUserID()));
                            LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                            button.IsEnabled = true;
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
                            button.IsEnabled = true;
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
                        button.IsEnabled = true;
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
                    lblError.Text = "Invalid Username and password.";
                    lblError.Visibility = Visibility.Visible;
                    bsyindicator.IsBusy = false;
                    button.IsEnabled = true;
                    //stkpnlLoader.Visibility = Visibility.Hidden;
                    //stkpnlmaincontent.Visibility = Visibility.Visible;
                    //  lblError.Visible = false;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                lblError.Text = "Error occured pleae try again.";
                lblError.Visibility = Visibility.Visible;
                bsyindicator.IsBusy = false;
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
        private void MediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Visibility = Visibility.Visible;
            // MediaPlayer.Source = new Uri(@"../Loader.gif");
            MediaPlayer.Position = new TimeSpan(0, 0, 2);
            MediaPlayer.Play();
        }
        private void MediaPlayer_MediaStart(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Visibility = Visibility.Collapsed;

            MediaPlayer.Stop();
        }
    }
}
