
using bftradeline.Models;
using bftradeline.Models123;
using globaltraders.AccountsServiceReference;
using globaltraders.Models;
using globaltraders.UserServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for LedgerWindow.xaml
    /// </summary>
    public partial class LedgerWindow : Window
    {
        public LedgerWindow()
        {
            InitializeComponent();
        }
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
       AccountsServiceClient objAccountsServiceclient = new AccountsServiceClient();
        public int UserIDforLedger;
        public string EventTypeforaccounts = "";
        public bool isGroupbyUser = false;
        public int fromBalanceSheet = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string DateFrom = dtpFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
                string DateTo = dtpTo.SelectedDate.Value.ToString("yyyy-MM-dd");
                List<UserAccounts> lstUserAccounts = new List<UserAccounts>();
                //if (LoggedinUserDetail.GetUserTypeID() == 1 && cmbUserForLedger.SelectedIndex==0)
                //{
                //    bfnexchange.Services.DBModel.SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                //    UserIDforLedger = Convert.ToInt32(objCommissionandBookAccountID.BookAccountID);
                //}
                if (EventTypeforaccounts != "")
                {
                    var results = objAccountsServiceclient.GetAccountsDatabyUserIdDateRangeandEventType(UserIDforLedger, DateFrom, DateTo, chkIncludeCredit.IsChecked.Value, EventTypeforaccounts).ToList();
                    foreach (var item in results)
                    {
                        UserAccounts objUserAccounts = new UserAccounts();
                        objUserAccounts.AccountsTitle = item.AccountsTitle;
                        objUserAccounts.CreatedDate = item.CreatedDate;
                        objUserAccounts.Credit = item.Credit;
                        objUserAccounts.Debit = item.Debit;
                        objUserAccounts.MarketBookID = item.MarketBookID;
                        objUserAccounts.OpeningBalance = (decimal)item.OpeningBalance;
                        objUserAccounts.UserID = item.UserID;
                        objUserAccounts.UserName = item.UserName;
                        lstUserAccounts.Add(objUserAccounts);
                    }


                }
                else
                {
                    lstUserAccounts = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyUserIDandDateRange(UserIDforLedger, DateFrom, DateTo, chkIncludeCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate));
                   
                }
                if (lstUserAccounts.Count > 0)
                {
                    UserAccounts objUseraccounts = new UserAccounts();
                    objUseraccounts.AccountsTitle = "Opening Balance";
                    objUseraccounts.Debit = lstUserAccounts[0].OpeningBalance.ToString("F2");
                    objUseraccounts.Credit = "0.00";
                    objUseraccounts.CreatedDate = lstUserAccounts[0].CreatedDate;
                    objUseraccounts.OpeningBalance = lstUserAccounts[0].OpeningBalance;
                    lstUserAccounts.Insert(0, objUseraccounts);
                    for (int i = 0; i <= lstUserAccounts.Count - 1; i++)
                    {
                        if (i + 1 < lstUserAccounts.Count)
                        {
                            if (lstUserAccounts[i + 1].Debit == "" || lstUserAccounts[i + 1].Debit == "0.00" || lstUserAccounts[i + 1].Debit == "0")
                            {
                                lstUserAccounts[i + 1].OpeningBalance = lstUserAccounts[i].OpeningBalance - Convert.ToDecimal(lstUserAccounts[i + 1].Credit);
                                lstUserAccounts[i + 1].Credit = (-1 * Convert.ToDecimal(lstUserAccounts[i + 1].Credit)).ToString();
                                lstUserAccounts[i + 1].Debit = "0.00";
                            }
                            else
                            {
                                lstUserAccounts[i + 1].OpeningBalance = lstUserAccounts[i].OpeningBalance + Convert.ToDecimal(lstUserAccounts[i + 1].Debit);
                                lstUserAccounts[i + 1].Credit = "0.00";
                            }
                        }
                    }


                }

                decimal profitorloss = objUsersServiceCleint.GetProfitorLossbyUserID(UserIDforLedger, chkIncludeCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                lblNetProfitandloss.Content = profitorloss.ToString("N0");
                if (profitorloss >= 0)
                {
                    lblNetProfitandloss.Foreground = Brushes.White;
                    lblNetProfitandloss.Background = Brushes.Green;
                }
                else
                {
                    lblNetProfitandloss.Foreground = Brushes.White;
                    lblNetProfitandloss.Background = Brushes.Red;
                }

                dgvLedger.AutoGenerateColumns = false;
                dgvLedger.ItemsSource = lstUserAccounts;
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);

            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (EventTypeforaccounts == "")
            {
                dtpFrom.SelectedDate = DateTime.Now.AddDays(-2);
                dtpTo.SelectedDate = DateTime.Now;
            }


            if (LoggedinUserDetail.GetUserTypeID() == 3)
            {
                cmbUserForLedger.Visibility = Visibility.Hidden;
                lblUserLedgerDetails.Visibility = Visibility.Hidden;
                cmbEventTypeforLedger.Visibility = Visibility.Hidden;
                lblEventType.Visibility = Visibility.Hidden;
            }
            else
            {

                GetUsersbyUsersType();
                GetEventTypeFromAccounts();


            }
            if (fromBalanceSheet == 1)
            {
                cmbUserForLedger.SelectedValue = UserIDforLedger;
                fromBalanceSheet = 0;
            }
            else
            {
                UserIDforLedger = LoggedinUserDetail.GetUserID();
            }
            Button_Click(this, e);
        }
        public void GetEventTypeFromAccounts()
        {
            var results = objAccountsServiceclient.GetDistinctEventTypesfromAccounts().ToList();
            AccountsServiceReference.SP_UserAccounts_GetDistinctEventTypes_Result objEventType = new AccountsServiceReference.SP_UserAccounts_GetDistinctEventTypes_Result();
            objEventType.Eventtype = "Please Select";
            results.Insert(0, objEventType);
            cmbEventTypeforLedger.ItemsSource = results;
            cmbEventTypeforLedger.DisplayMemberPath = "Eventtype";
            cmbEventTypeforLedger.SelectedValuePath = "Eventtype";
        }
        public void GetUsersbyUsersType()
        {
            cmbUserForLedger.ItemsSource = LoggedinUserDetail.AllUsers;
            cmbUserForLedger.DisplayMemberPath = "UserName";
            cmbUserForLedger.SelectedValuePath = "ID";
            return;
#pragma warning disable CS0162 // Unreachable code detected
            LoggedinUserDetail.CheckifUserLogin();
#pragma warning restore CS0162 // Unreachable code detected
            var results = objUsersServiceCleint.GetAllUsersbyUserType(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.GetUserTypeID(), LoggedinUserDetail.PasswordForValidate);
            if (results != "")
            {
                List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);

                foreach (UserIDandUserType objuser in lstUsers)
                {
                    objuser.UserName = Crypto.Decrypt(objuser.UserName);
                    objuser.UserName = objuser.UserName + " (" + objuser.UserType + ")";
                }
                UserIDandUserType userdefult = new UserIDandUserType();
                userdefult.ID = 0;
                userdefult.UserName = "Please Select";
                lstUsers.Insert(0, userdefult);

               
               




            }
            else
            {
                List<UserIDandUserType> lstUsers = new List<UserIDandUserType>();

            }
        }

        private void cmbEventTypeforLedger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

           
            if (cmbEventTypeforLedger.SelectedIndex > 0)
            {
                EventTypeforaccounts = cmbEventTypeforLedger.Text;
            }
            else
            {
                EventTypeforaccounts = "";
            }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void cmbUserForLedger_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (fromBalanceSheet == 0)
                {
                    UserIDforLedger = Convert.ToInt32(cmbUserForLedger.SelectedValue);
                    //

                }
                // fromBalanceSheet = 0;

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgvLedger_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (dgvLedger.Items.Count > 1)
            {
                UserAccounts objSelectedRow = (UserAccounts)dgvLedger.SelectedItem;
                if (objSelectedRow.MarketBookID != null)
                {
                    CompletedBetsWindow objfrmuserbetscompeleted = new CompletedBetsWindow();
                    objfrmuserbetscompeleted.UserID = objSelectedRow.UserID.Value;
                    objfrmuserbetscompeleted.MarketBookID = objSelectedRow.MarketBookID;
                    objfrmuserbetscompeleted.ShowDialog();
                }
               
            }
            
        }
    }
}
