
using bftradeline.HelperClasses;
using bftradeline.Models;
using bftradeline.Models123;
using globaltraders.AccountsServiceReference;
using globaltraders.HelperClasses;
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
    /// Interaction logic for PlusMinusSummaryWindow.xaml
    /// </summary>
    public partial class PlusMinusSummaryWindow : Window
    {
        AccountsServiceClient objAccountsServiceclient = new AccountsServiceClient();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        public DateTime FromDate;
        public DateTime ToDate;
        public PlusMinusSummaryWindow()
        {
            InitializeComponent();
        }
        public string EventTypeforaccounts = "";


        private void btnLoadLedger_Click(object sender, RoutedEventArgs e)
        {
            string DateFrom = dtpFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
            string DateTo = dtpTo.SelectedDate.Value.ToString("yyyy-MM-dd");
            Decimal TotAdminAmount = 0;
            List<UserAccounts> lstUserAccounts = new List<UserAccounts>();

            if (EventTypeforaccounts != "")
            {
                List<AccountsServiceReference. SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result> results=new List<AccountsServiceReference.SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result>();
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    results = objAccountsServiceclient.GetAccountsDatabyUserIdDateRangeandEventType(Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue), DateFrom, DateTo, false, EventTypeforaccounts).ToList();
                   
                }
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {
                    results = objAccountsServiceclient.GetAccountsDatabyUserIdDateRangeandEventType(Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue), DateFrom, DateTo, false, EventTypeforaccounts).ToList();

                }
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                     results = objAccountsServiceclient.GetAccountsDatabyUserIdDateRangeandEventType(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, false, EventTypeforaccounts).ToList();
                    
                }

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
                    objUserAccounts.AgentRate = item.AgentRate;
                    lstUserAccounts.Add(objUserAccounts);
                }
                ///Data For Agents
                if (Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue) == 73)
                {
                    foreach (UserIDandUserType item in cmbUsersforBalanceSheet.Items)
                    {
                        if (item.ID > 0 && item.ID != 73 && item.ID != 3)
                        {

                            var resultsAgents = objAccountsServiceclient.GetAccountsDatabyUserIdDateRangeandEventType(Convert.ToInt32(item.ID), DateFrom, DateTo, false, EventTypeforaccounts).ToList();
                            List<UserAccounts> lstUserAccountsForAgent = new List<UserAccounts>();
                            foreach (var item1 in resultsAgents)
                            {
                                UserAccounts objUserAccounts = new UserAccounts();
                                objUserAccounts.AccountsTitle = item1.AccountsTitle;
                                objUserAccounts.CreatedDate = item1.CreatedDate;
                                objUserAccounts.Credit = item1.Credit;
                                objUserAccounts.Debit = item1.Debit;
                                objUserAccounts.MarketBookID = item1.MarketBookID;
                                objUserAccounts.OpeningBalance = (decimal)item1.OpeningBalance;
                                objUserAccounts.UserID = item1.UserID;
                                objUserAccounts.UserName = item1.UserName;
                                objUserAccounts.AgentRate = item1.AgentRate;
                                lstUserAccountsForAgent.Add(objUserAccounts);
                            }

                            if (lstUserAccountsForAgent.Count > 0)
                            {
                                lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                                foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                                {
                                    if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                                    {
                                        int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                        int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                        decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);

                                        if (ActualAmount > 0)
                                        {
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

                                            TotAdminAmount += -1 * (ActualAmount - (AgentAmount) - Comissionamount);
                                        }
                                        else
                                        {
                                            ActualAmount = -1 * ActualAmount;
                                            decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                            TotAdminAmount += ActualAmount - AgentAmount;
                                        }


                                    }
                                }
                                UserAccounts objNewUseAccount = new UserAccounts();
                                objNewUseAccount.UserName = Crypto.Encrypt(item.UserName);
                                objNewUseAccount.UserType = "Agent";
                                objNewUseAccount.UserID = item.ID;
                                objNewUseAccount.OpeningBalance = objUsersServiceCleint.GetStartingBalance(item.ID, LoggedinUserDetail.PasswordForValidate);
                                TotAdminAmount = -1 * TotAdminAmount;
                                if (TotAdminAmount >= 0)
                                {
                                    objNewUseAccount.Debit = TotAdminAmount.ToString();
                                    objNewUseAccount.Credit = "0.00";
                                }
                                else
                                {
                                    objNewUseAccount.Credit = (-1 * TotAdminAmount).ToString();
                                    objNewUseAccount.Debit = "0.00";

                                }
                                lstUserAccounts.Add(objNewUseAccount);
                                TotAdminAmount = 0;
                            }


                        }

                    }
                }


            }
            ///
            if (lstUserAccounts.Count > 0)
            {
                var lstUsers = lstUserAccounts.Select(item => item.UserID).Distinct().ToList();
                var lstUsersSummaryEventtype = new List<PlusMinussummarybyEventTypeandUser>();
                foreach (var userid in lstUsers)
                {
                    PlusMinussummarybyEventTypeandUser objplusminus = new PlusMinussummarybyEventTypeandUser();
                    var lstUserAccountsbyUser = lstUserAccounts.Where(item => item.UserID == userid).ToList();
                    decimal nettotalbyuser = lstUserAccountsbyUser.Sum(item => Convert.ToDecimal(item.Debit)) - lstUserAccountsbyUser.Sum(item => Convert.ToDecimal(item.Credit));
                    objplusminus.Username = Crypto.Decrypt(lstUserAccountsbyUser[0].UserName);
                    if (nettotalbyuser >= 0)
                    {
                        objplusminus.Plus = nettotalbyuser;
                        objplusminus.Minus = 0;

                    }
                    else
                    {
                        objplusminus.Plus = 0;
                        objplusminus.Minus = nettotalbyuser;
                    }
                    lstUsersSummaryEventtype.Add(objplusminus);
                }
                List<PlusMinussummarybyEventTypeandUser> lstPlusCustomers = lstUsersSummaryEventtype.Where(item => item.Plus > 0).ToList();
                List<PlusMinussummarybyEventTypeandUser> lstMinusCustomers = lstUsersSummaryEventtype.Where(item => item.Minus < 0).ToList();
                dgvPlus.ItemsSource = lstPlusCustomers;
                DGVMinus.ItemsSource = lstMinusCustomers;
            }
            else
            {
                List<PlusMinussummarybyEventTypeandUser> lstPlusCustomers = new List<PlusMinussummarybyEventTypeandUser>();
                List<PlusMinussummarybyEventTypeandUser> lstMinusCustomers = new List<PlusMinussummarybyEventTypeandUser>();
                dgvPlus.ItemsSource = lstPlusCustomers;
                DGVMinus.ItemsSource = lstMinusCustomers;
            }

        }


        public void GetUsersbyUsersType()
        {
            LoggedinUserDetail.CheckifUserLogin();
            var results = objUsersServiceCleint.GetAllUsersbyUserTypeNew(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.GetUserTypeID(), LoggedinUserDetail.PasswordForValidate);
            if (results != "")
            {
                List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);

                foreach (UserIDandUserType objuser in lstUsers)
                {
                  // objuser.UserName = Crypto.Decrypt(objuser.UserName);
                    objuser.UserName = objuser.UserName + " (" + objuser.UserType + ")";
                }
                UserIDandUserType userdefult = new UserIDandUserType();
                userdefult.ID = 0;
                userdefult.UserTypeID = 0;
                userdefult.UserName = "Please Select";
                lstUsers.Insert(0, userdefult);

                List<UserIDandUserType> lstonlyAgents = lstUsers.Where(item => item.UserTypeID == 2 || item.UserTypeID == 0).ToList();
                cmbUsersforBalanceSheet.ItemsSource = lstonlyAgents;
                cmbUsersforBalanceSheet.DisplayMemberPath = "UserName";
                cmbUsersforBalanceSheet.SelectedValuePath = "ID";



            }
            else
            {
                List<UserIDandUserType> lstUsers = new List<UserIDandUserType>();

            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFrom.SelectedDate = FromDate;
            dtpTo.SelectedDate = ToDate;
            dgvPlus.AutoGenerateColumns = false;
            DGVMinus.AutoGenerateColumns = false;
            //GetEventTypeFromAccounts();
            if (EventTypeforaccounts != "")
            {
                lblHeading.Content = lblHeading.Content + " ( " + EventTypeforaccounts + " )";
            }
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                GetUsersbyUsersType();
                lblBalanceSheet.Visibility = Visibility.Visible;
                cmbUsersforBalanceSheet.Visibility = Visibility.Visible;
                cmbUsersforBalanceSheet.SelectedValue = 73;
            }
            btnLoadLedger_Click(this, e);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
