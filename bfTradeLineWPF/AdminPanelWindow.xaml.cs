using bftradeline.Models;
using globaltraders.AccountsServiceReference;
using globaltraders.UserServiceReference;
using iTextSharp.text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using bftradeline.HelperClasses;
using globaltraders.Service123Reference;
using globaltraders.Models;

namespace globaltraders
{
    /// <summary>
    /// Interaction logic for AdminPanelWindow.xaml
    /// </summary>
    public partial class AdminPanelWindow : Window
    {
        public AdminPanelWindow()
        {
            InitializeComponent();
        }
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        AccountsServiceClient objAccountsService = new AccountsServiceClient();
        public void ClearAllTextFields()
        {
            txtAccountBalance.Text = "";
            txtAgentrate.Text = "0";
            txtAgentRateUpate.Text = "0";
            txtEmail.Text = "";
            txtLocation.Text = "";
            txtName.Text = "";

            txtPasswordAdd.Text = "";
            txtPasswordUpdate.Text = "";
            txtPhone.Text = "";
            txtUsername.Text = "";
            txtUsernameupdate.Text = "";
            txtUserPhoneUpdate.Text = "";

        }
        public void ClearAllLabelFields()
        {
            lblName.Content = "";
            lblUserName.Content = "";
            lblPhone.Content = "";
            lblAccountBalance.Content = "";
            lblLastLoginTime.Content = "";
            lblStatus.Content = "";
            lblProfitandLoss.Content = "";
            lblLastAmoundAdd.Content = "";
            lblLastAmountRemoved.Content = "";
            lblAgentRate.Content = "";
            lblcommissionRate.Content = "";
            lblLastLocation.Content = "";
            lblIpAddress.Content = "";
            lblCreatedBy.Content = "";
            lblCreatedDateDetails.Content = "";
            chkBlockUser.IsChecked = false;
            chkGrayHoundRacing.IsChecked = false;
            chkHorseRacing.IsChecked = false;
            chkLoggedIn.IsChecked = false;
            chkSoccer.IsChecked = false;
            chkTennis.IsChecked = false;
        }
        public void GetusersbyUserTypeReload()
        {
            //   var results = objUsersServiceCleint.GetAllUsersbyUserType(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.GetUserTypeID(), LoggedinUserDetail.PasswordForValidate);
            var results = objUsersServiceCleint.GetAllUsersbyUserTypeNew(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.GetUserTypeID(), LoggedinUserDetail.PasswordForValidate);
            if (results != "")
            {

                List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);

                foreach (UserIDandUserType objuser in lstUsers)
                {
                    //    objuser.UserName = Crypto.Decrypt(objuser.UserName);
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
        List<GetMarketForAllowedBetting_Result> lstMarketsdownload = new List<GetMarketForAllowedBetting_Result>();
        public void GetUsersbyUsersType()
        {
            if (1 == 1)
            {
                List<UserIDandUserType> lstUsers = LoggedinUserDetail.AllUsers;


                cmbUsers.ItemsSource = lstUsers;
                cmbUsers.DisplayMemberPath = "UserName";
                cmbUsers.SelectedValuePath = "ID";
                cmbUsersCredit.IsSynchronizedWithCurrentItem = false;
                cmbUsersCredit.ItemsSource = lstUsers;
                cmbUsersCredit.DisplayMemberPath = "UserName";
                cmbUsersCredit.SelectedValuePath = "ID";
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    ComboBoxItem matchtype = (ComboBoxItem)comboBox1.SelectedItem;
                    lstMarketsdownload = JsonConvert.DeserializeObject<List<GetMarketForAllowedBetting_Result>>(objUsersServiceCleint.GetMarketsforBettingAllowed(Convert.ToInt32(73))).ToList();
                    var lstdistincteventmarketsfordownload = lstMarketsdownload.Where(item => item.Market.Contains("Match Odds") && item.EventTypeName == matchtype.Content.ToString()).Distinct().ToList();
                    cmbEventss.ItemsSource = lstdistincteventmarketsfordownload;
                    cmbEventss.DisplayMemberPath = "Market";
                    cmbEventss.SelectedValuePath = "EventID";

                    cmbCustomerAllowedMarkets.IsSynchronizedWithCurrentItem = false;
                    cmbCustomerAllowedMarkets.ItemsSource = lstUsers;
                    cmbCustomerAllowedMarkets.DisplayMemberPath = "UserName";
                    cmbCustomerAllowedMarkets.SelectedValuePath = "ID";

                    cmbCustomersForhissa.ItemsSource = lstUsers;
                    cmbCustomersForhissa.DisplayMemberPath = "UserName";
                    cmbCustomersForhissa.SelectedValuePath = "ID";

                    cmbCustomerAllowedMarketskalijutt.ItemsSource = lstUsers;
                    cmbCustomerAllowedMarketskalijutt.DisplayMemberPath = "UserName";
                    cmbCustomerAllowedMarketskalijutt.ValueMemberPath = "ID";

                    cmbCustomerAllowedMarketsInFancy.ItemsSource = lstUsers;
                    cmbCustomerAllowedMarketsInFancy.DisplayMemberPath = "UserName";
                    cmbCustomerAllowedMarketsInFancy.ValueMemberPath = "ID";

                    cmbReferrerUserhissa.IsSynchronizedWithCurrentItem = false;
                    cmbReferrerUserhissa.ItemsSource = lstUsers;
                    cmbReferrerUserhissa.DisplayMemberPath = "UserName";
                    cmbReferrerUserhissa.SelectedValuePath = "ID";

                    cmbCustomerAllowedMarketsNew.ItemsSource = lstUsers;
                    cmbCustomerAllowedMarketsNew.DisplayMemberPath = "UserName";
                    cmbCustomerAllowedMarketsNew.ValueMemberPath = "ID";


                    //Accountscombobx
                    cmbCustomersForUpdateResultsAccounts.IsSynchronizedWithCurrentItem = false;
                    cmbCustomersForUpdateResultsAccounts.ItemsSource = lstUsers;
                    cmbCustomersForUpdateResultsAccounts.DisplayMemberPath = "UserName";
                    cmbCustomersForUpdateResultsAccounts.SelectedValuePath = "ID";
                    /////
                    //BetsCombobox
                    cmbCustomersForUpdateResultsBets.IsSynchronizedWithCurrentItem = false;
                    cmbCustomersForUpdateResultsBets.ItemsSource = lstUsers;
                    cmbCustomersForUpdateResultsBets.DisplayMemberPath = "UserName";
                    cmbCustomersForUpdateResultsBets.SelectedValuePath = "ID";
                    /////
                    cmbCustomersForLimits.IsSynchronizedWithCurrentItem = false;
                    cmbCustomersForLimits.ItemsSource = lstUsers;
                    cmbCustomersForLimits.DisplayMemberPath = "UserName";
                    cmbCustomersForLimits.SelectedValuePath = "ID";
                    cmbReferrerUser.IsSynchronizedWithCurrentItem = false;
                    cmbReferrerUser.ItemsSource = lstUsers;
                    cmbReferrerUser.DisplayMemberPath = "UserName";
                    cmbReferrerUser.SelectedValuePath = "ID";
                    cmbCustomersForTimers.IsSynchronizedWithCurrentItem = false;
                    cmbCustomersForTimers.ItemsSource = lstUsers.Where(u => new[] { "0", "3" }.Contains(u.UserTypeID.ToString())).ToList();
                    cmbCustomersForTimers.DisplayMemberPath = "UserName";
                    cmbCustomersForTimers.SelectedValuePath = "ID";
                    //cmb Agents TransferAdmin
                    cmbAgentForTransferAdmin.IsSynchronizedWithCurrentItem = false;
                    cmbAgentForTransferAdmin.ItemsSource = lstUsers.Where(u => new[] { "0", "2" }.Contains(u.UserTypeID.ToString())).ToList();
                    cmbAgentForTransferAdmin.DisplayMemberPath = "UserName";
                    cmbAgentForTransferAdmin.SelectedValuePath = "ID";
                    //
                    dgvUnBlockUsers.AutoGenerateColumns = false;
                    // var lstEndusersandAgent= lstUsers.Where(u => new[] { "2", "3" }.Contains(u.UserTypeID.ToString()));
                    dgvUnBlockUsers.ItemsSource = lstUsers.Where(u => new[] { "0", "2", "3" }.Contains(u.UserTypeID.ToString())).ToList();

                }

                cmbTransferBalanceTo.IsSynchronizedWithCurrentItem = false;
                cmbTransferBalanceTo.ItemsSource = lstUsers;
                cmbTransferBalanceTo.DisplayMemberPath = "UserName";
                cmbTransferBalanceTo.SelectedValuePath = "ID";
                cmbTransferBalanceFrom.IsSynchronizedWithCurrentItem = false;
                cmbTransferBalanceFrom.ItemsSource = lstUsers;
                cmbTransferBalanceFrom.DisplayMemberPath = "UserName";
                cmbTransferBalanceFrom.SelectedValuePath = "ID";
                cmbUsersforBalanceSheet.IsSynchronizedWithCurrentItem = false;
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    List<UserIDandUserType> lstonlyAgents = lstUsers.Where(item =>  item.UserTypeID == 2  && item.CreatedbyID == 1).ToList();
                    lstonlyAgentsAll.AddRange(lstonlyAgents);
                    List<UserIDandUserType> lstonlyAgents1 = lstUsers.Where(item => item.UserTypeID == 9 && item.CreatedbyID == 1).ToList();
                    lstonlyAgentsAll.AddRange( lstonlyAgents1);
                    List<UserIDandUserType> lstonlyAgents2= lstUsers.Where(item => item.UserTypeID == 8 && item.CreatedbyID == 1).ToList();
                    lstonlyAgentsAll.AddRange(lstonlyAgents2);
                    cmbUsersforBalanceSheet.ItemsSource = lstonlyAgents;
                    cmbUsersforBalanceSheet.DisplayMemberPath = "UserName";
                    cmbUsersforBalanceSheet.SelectedValuePath = "ID";
                   
                }
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {
                   
                    List<UserIDandUserType> lstonlyAgentsforsuper = lstUsers.Where(item => item.UserTypeID == 2 && item.CreatedbyID == LoggedinUserDetail.GetUserID()).ToList();
                    lstonlyAgentsAllforSuper = lstonlyAgentsforsuper;
                    cmbUsersforBalanceSheet.ItemsSource = lstonlyAgentsforsuper;
                    cmbUsersforBalanceSheet.DisplayMemberPath = "UserName";
                    cmbUsersforBalanceSheet.SelectedValuePath = "ID";
                }
                if (LoggedinUserDetail.GetUserTypeID() == 9)
                {
                    List<UserIDandUserType> lstonlySuperforSamiadmin = lstUsers.Where(item => item.UserTypeID == 8 && item.CreatedbyID == LoggedinUserDetail.GetUserID()).ToList();
                    lstonlySuperAllforSamiAdmin = lstonlySuperforSamiadmin;
                    cmbUsersforBalanceSheet.ItemsSource = lstonlySuperforSamiadmin;
                    cmbUsersforBalanceSheet.DisplayMemberPath = "UserName";
                    cmbUsersforBalanceSheet.SelectedValuePath = "ID";
                }
            }
            else
            {
                List<UserIDandUserType> lstUsers = new List<UserIDandUserType>();
            }
        }
        List<UserIDandUserType> lstonlyAgentsAllforSuper = new List<UserIDandUserType>();
        List<UserIDandUserType> lstonlySuperAllforSamiAdmin = new List<UserIDandUserType>();
        List<UserIDandUserType> lstonlyAgentsAll = new List<UserIDandUserType>();
        List<UserIDandUserType> lstonlysuperAll = new List<UserIDandUserType>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            txtMarquee.Text = objUsersServiceCleint.GetMarqueeText();

            var results = JsonConvert.DeserializeObject<UserDetails>(objUsersServiceCleint.GetUserDetailsbyID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate));
            UserDetails objuserDetails = new UserDetails();
            objuserDetails = results;
            string[] names = objuserDetails.Name.Split('(');
            txtUsernameupdate.Text = names[0].Trim();
            txtUserPhoneUpdate.Text = objuserDetails.PhoneNumber;

            if (LoggedinUserDetail.GetUserTypeID() != 3)
            {

            }
            else
            {
                foreach (TabItem win in tabControlAdminpanel.Items)
                {
                    if (win.Header.ToString() == "Profile")
                    {

                        win.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        win.Visibility = Visibility.Collapsed;
                    }
                }

            }
            if (LoggedinUserDetail.GetUserTypeID() == 2)
            {
                GetusersbyUserTypeReload();
                // GetUsersbyUsersType();
                GetUsersbyUsersType();
                List<CuttingUsers> lstCuttingUsers = new List<CuttingUsers>();

                CuttingUsers objCuttingUser2 = new CuttingUsers();
                objCuttingUser2.ID = 3;
                objCuttingUser2.username = "End User";
                lstCuttingUsers.Add(objCuttingUser2);
                cmbUserType.ItemsSource = lstCuttingUsers;
                cmbUserType.DisplayMemberPath = "username";
                cmbUserType.SelectedValuePath = "ID";

                TabItem tbitem3 = (TabItem)tabControl2.Items[3];
                tbitem3.Visibility = Visibility.Collapsed;
                TabItem tbitem4 = (TabItem)tabControl2.Items[4];
                tbitem4.Visibility = Visibility.Collapsed;
                TabItem tbitem5 = (TabItem)tabControl2.Items[5];
                tbitem5.Visibility = Visibility.Collapsed;
                TabItem tbitem6 = (TabItem)tabControl2.Items[6];
                tbitem6.Visibility = Visibility.Collapsed;
                TabItem tbitem7 = (TabItem)tabControl2.Items[7];
                tbitem7.Visibility = Visibility.Collapsed;
                TabItem tbitem8 = (TabItem)tabControl2.Items[8];
                tbitem8.Visibility = Visibility.Collapsed;
                TabItem tbitem9 = (TabItem)tabControl2.Items[9];
                tbitem9.Visibility = Visibility.Collapsed;
                TabItem tbitem10 = (TabItem)tabControl2.Items[10];
                tbitem10.Visibility = Visibility.Collapsed;
               
                TabItem tbitem12 = (TabItem)tabControlAdminpanel.Items[3];
                tbitem12.Visibility = Visibility.Collapsed;

            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {
                    GetusersbyUserTypeReload();
                    // GetUsersbyUsersType();
                    GetUsersbyUsersType();
                    List<CuttingUsers> lstCuttingUsers = new List<CuttingUsers>();

                    CuttingUsers objCuttingUser1 = new CuttingUsers();
                    objCuttingUser1.ID = 2;
                    objCuttingUser1.username = "Agent";
                    lstCuttingUsers.Add(objCuttingUser1);
                    if (LoggedinUserDetail.user.CheckforMaxOddLay == true)
                    {
                        CuttingUsers objCuttingUser4 = new CuttingUsers();
                        objCuttingUser4.ID = 8;
                        objCuttingUser4.username = "Super Master";
                        lstCuttingUsers.Add(objCuttingUser4);
                    }
                    cmbUserType.ItemsSource = lstCuttingUsers;
                    cmbUserType.DisplayMemberPath = "username";
                    cmbUserType.SelectedValuePath = "ID";
                    TabItem tbitem4 = (TabItem)tabControl2.Items[4];
                    tbitem4.Visibility = Visibility.Collapsed;
                    TabItem tbitem5 = (TabItem)tabControl2.Items[5];
                    tbitem5.Visibility = Visibility.Collapsed;
                    TabItem tbitem6 = (TabItem)tabControl2.Items[6];
                    tbitem6.Visibility = Visibility.Collapsed;
                    TabItem tbitem7 = (TabItem)tabControl2.Items[7];
                    tbitem7.Visibility = Visibility.Collapsed;
                    TabItem tbitem8 = (TabItem)tabControl2.Items[8];
                    tbitem8.Visibility = Visibility.Collapsed;
                    TabItem tbitem9 = (TabItem)tabControlAdminpanel.Items[3];
                    tbitem9.Visibility = Visibility.Collapsed;

                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        //GetBetsIntervalandPlaceBetTimings();
                        GetusersbyUserTypeReload();

                        GetUsersbyUsersType();
                        List<CuttingUsers> lstCuttingUsers = new List<CuttingUsers>();
                        CuttingUsers objCuttingUser = new CuttingUsers();
                        objCuttingUser.ID = 1;
                        objCuttingUser.username = "Admin";
                        lstCuttingUsers.Add(objCuttingUser);
                        CuttingUsers objCuttingUser4 = new CuttingUsers();
                        objCuttingUser4.ID = 8;
                        objCuttingUser4.username = "Super Master";
                        lstCuttingUsers.Add(objCuttingUser4);
                        CuttingUsers objCuttingUser1 = new CuttingUsers();
                        objCuttingUser1.ID = 2;
                        objCuttingUser1.username = "Agent";
                        lstCuttingUsers.Add(objCuttingUser1);
                        CuttingUsers objCuttingUser2 = new CuttingUsers();
                        objCuttingUser2.ID = 3;
                        objCuttingUser2.username = "End User";
                        lstCuttingUsers.Add(objCuttingUser2);
                        CuttingUsers objCuttingUser3 = new CuttingUsers();
                        objCuttingUser3.ID = 4;
                        objCuttingUser3.username = "Cutting User";
                        lstCuttingUsers.Add(objCuttingUser3);
                        cmbUserType.ItemsSource = lstCuttingUsers;
                        cmbUserType.DisplayMemberPath = "username";
                        cmbUserType.SelectedValuePath = "ID";

                    }
                    if (LoggedinUserDetail.GetUserTypeID() == 9)
                    {
                        GetusersbyUserTypeReload();
                        // GetUsersbyUsersType();
                        GetUsersbyUsersType();
                        List<CuttingUsers> lstCuttingUsers = new List<CuttingUsers>();

                       
                       
                            CuttingUsers objCuttingUser4 = new CuttingUsers();
                            objCuttingUser4.ID = 8;
                            objCuttingUser4.username = "Super Master";
                            lstCuttingUsers.Add(objCuttingUser4);
                        
                        cmbUserType.ItemsSource = lstCuttingUsers;
                        cmbUserType.DisplayMemberPath = "username";
                        cmbUserType.SelectedValuePath = "ID";
                        TabItem tbitem4 = (TabItem)tabControl2.Items[4];
                        tbitem4.Visibility = Visibility.Collapsed;
                        TabItem tbitem5 = (TabItem)tabControl2.Items[5];
                        tbitem5.Visibility = Visibility.Collapsed;
                        TabItem tbitem6 = (TabItem)tabControl2.Items[6];
                        tbitem6.Visibility = Visibility.Collapsed;
                        TabItem tbitem7 = (TabItem)tabControl2.Items[7];
                        tbitem7.Visibility = Visibility.Collapsed;
                        TabItem tbitem8 = (TabItem)tabControl2.Items[8];
                        tbitem8.Visibility = Visibility.Collapsed;
                        TabItem tbitem9 = (TabItem)tabControlAdminpanel.Items[3];
                        tbitem9.Visibility = Visibility.Collapsed;

                    }

                }
            }


        }


        public List<BalanceSheet> BalanceSheet(int UserID, bool isCredit)
        {
            LoggedinUserDetail.CheckifUserLogin();
            try
            {
                List<UserAccounts> lstAccountsDonebyAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(1, false, LoggedinUserDetail.PasswordForValidate));

                if (UserID == 0)
                {
                    UserID = LoggedinUserDetail.GetUserID();
                }
                List<BalanceSheet> lstBalanceSheet = new List<BalanceSheet>();
                Decimal TotAdminAmount = 0;
               
                decimal TotalAdminAmountWithoutMarkets = 0;
               
                if (LoggedinUserDetail.GetUserTypeID() != 3)
                {

                    List<UserAccounts> lstUserAccounts = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(UserID, isCredit, LoggedinUserDetail.PasswordForValidate));
                    if (UserID == 73)
                    {
                        List<UserIDandUserType> lstonlyAgentsAllforsheet = new List<UserIDandUserType>();
                        lstonlyAgentsAllforsheet = lstonlyAgentsAll.Where(s => s.UserTypeID != 8 && s.UserTypeID != 9).ToList();
                        foreach (var item in lstonlyAgentsAllforsheet)
                        {
                            if (item.ID > 0 && item.ID != 73 && item.ID != 3)
                            {
                                List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(item.ID, isCredit, LoggedinUserDetail.PasswordForValidate));
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
                                    objNewUseAccount.UserName = item.UserName;
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
                                //
                                //List<UserAccounts> lstAccountsDonebyAdmin = lstAccountsDonebyAdmin; //JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(LoggedinUserDetail.GetUserID(), false, LoggedinUserDetail.PasswordForValidate));
                                if (lstAccountsDonebyAdmin.Count > 0)
                                {
                                    List<UserAccounts> lstAccountsDonebyAdminagainstthisAgent = lstAccountsDonebyAdmin.Where(item1 => item1.AccountsTitle.Contains("(UserID=" + item.ID.ToString() + ")")).ToList();
                                    if (lstAccountsDonebyAdminagainstthisAgent.Count > 0)
                                    {
                                        TotalAdminAmountWithoutMarkets = lstAccountsDonebyAdminagainstthisAgent.Sum(item1 => Convert.ToDecimal(item1.Debit)) - lstAccountsDonebyAdminagainstthisAgent.Sum(item1 => Convert.ToDecimal(item1.Credit));

                                    }
                                }
                                List<UserAccounts> objCurrentUserAccountlst = lstUserAccounts.Where(itemagent => itemagent.UserID.ToString() == item.ID.ToString()).ToList();
                                if (objCurrentUserAccountlst.Count > 0)
                                {
                                    UserAccounts objCurrentUserAccount = objCurrentUserAccountlst[0];
                                    if (TotalAdminAmountWithoutMarkets < 0)
                                    {
                                        objCurrentUserAccount.Debit = (Convert.ToDecimal(objCurrentUserAccount.Debit) + (-1 * TotalAdminAmountWithoutMarkets)).ToString();

                                    }
                                    else
                                    {
                                        objCurrentUserAccount.Credit = (Convert.ToDecimal(objCurrentUserAccount.Credit) + TotalAdminAmountWithoutMarkets).ToString();
                                        //if (Convert.ToDecimal(objCurrentUserAccount.Credit)<0) {
                                        //    objCurrentUserAccount.Credit = (-1 * Convert.ToDecimal(objCurrentUserAccount.Credit)).ToString();
                                        //}
                                    }
                                    if (Convert.ToDecimal(objCurrentUserAccount.Debit) < 0)
                                    {
                                        objCurrentUserAccount.Debit = (-1 * Convert.ToDecimal(objCurrentUserAccount.Debit)).ToString();
                                    }
                                    if (Convert.ToDecimal(objCurrentUserAccount.Credit) < 0)
                                    {
                                        objCurrentUserAccount.Credit = (-1 * Convert.ToDecimal(objCurrentUserAccount.Credit)).ToString();
                                    }
                                    TotalAdminAmountWithoutMarkets = 0;
                                }

                                else
                                {
                                    UserAccounts objNewUseAccount = new UserAccounts();
                                    objNewUseAccount.UserName = item.UserName;
                                    objNewUseAccount.UserType = "Agent";
                                    objNewUseAccount.UserID = item.ID;
                                    objNewUseAccount.OpeningBalance = objUsersServiceCleint.GetStartingBalance(item.ID, LoggedinUserDetail.PasswordForValidate);
                                    TotalAdminAmountWithoutMarkets = -1 * TotalAdminAmountWithoutMarkets;
                                    if (TotalAdminAmountWithoutMarkets >= 0)
                                    {
                                        objNewUseAccount.Debit = TotalAdminAmountWithoutMarkets.ToString();
                                        objNewUseAccount.Credit = "0.00";
                                    }
                                    else
                                    {
                                        objNewUseAccount.Credit = (-1 * TotalAdminAmountWithoutMarkets).ToString();
                                        objNewUseAccount.Debit = "0.00";

                                    }
                                    lstUserAccounts.Add(objNewUseAccount);
                                    TotalAdminAmountWithoutMarkets = 0;
                                }
                                //commisionagent
                                decimal AgentCommission = 0;
                                try
                                {
                                    AgentCommission = objUsersServiceCleint.GetTotalAgentCommissionbyAgentID(item.ID, LoggedinUserDetail.PasswordForValidate);
                                    UserAccounts objNewUseAccount = new UserAccounts();
                                    objNewUseAccount.UserName = item.UserName;
                                    objNewUseAccount.UserType = "Agent";
                                    objNewUseAccount.UserID = item.ID;
                                    objNewUseAccount.OpeningBalance = objUsersServiceCleint.GetStartingBalance(item.ID, LoggedinUserDetail.PasswordForValidate);

                                    if (AgentCommission >= 0)
                                    {
                                        objNewUseAccount.Debit = AgentCommission.ToString();
                                        objNewUseAccount.Credit = "0.00";
                                    }
                                    else
                                    {
                                        objNewUseAccount.Credit = (-1 * AgentCommission).ToString();
                                        objNewUseAccount.Debit = "0.00";

                                    }
                                    lstUserAccounts.Add(objNewUseAccount);

                                    AgentCommission = 0;
                                }

                                catch (System.Exception ex)
                                {

                                }


                            }
                        }


                        //List<UserAccounts> lstUserAccounts1 = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(LoggedinUserDetail.GetUserID(), isCredit));
                        //foreach(UserAccounts objusrAccounts in lstUserAccounts1)
                        //{
                        //    if(objusrAccounts.UserID != 73  )
                        //    {
                        //        objusrAccounts.UserName = Crypto.Decrypt(objusrAccounts.UserName)+ "(Agent)";
                        //        objusrAccounts.UserType = "Agent";
                        //        lstUserAccounts.Add(objusrAccounts);
                        //    }
                        //}

                    }


                    if (UserID == 59999)
                    {
                        foreach (var item in lstonlyAgentsAllforSuper)
                        {
                            //decimal TotAdminAmount = 0;
                            decimal SuperAmount = 0;
                            decimal SuperAmount1 = 0;
                            //decimal TotalAdminAmountWithoutMarkets = 0;
                            if (item.ID > 0 && item.ID != 73 && item.ID != 3)
                            {
                                List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(item.ID, isCredit, LoggedinUserDetail.PasswordForValidate));
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
                                    TotAdminAmount = (TotAdminAmount) + (SuperAmount1);
                                    UserAccounts objNewUseAccount = new UserAccounts();
                                    objNewUseAccount.UserName = item.UserName;
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
                                // List<UserAccounts> lstAccountsDonebyAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(LoggedinUserDetail.GetUserID(), false, LoggedinUserDetail.PasswordForValidate));
                                int createdbyid = objUsersServiceCleint.GetCreatedbyID(item.ID);


                                List<UserAccounts> lstAccountsDonebyAdmin1 = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(createdbyid, false, LoggedinUserDetail.PasswordForValidate));
                                if (lstAccountsDonebyAdmin.Count > 0)
                                {
                                    List<UserAccounts> lstAccountsDonebyAdminagainstthisAgent = lstAccountsDonebyAdmin1.Where(item1 => item1.AccountsTitle.Contains("(UserID=" + item.ID.ToString() + ")")).ToList();
                                    if (lstAccountsDonebyAdminagainstthisAgent.Count > 0)
                                    {
                                        TotalAdminAmountWithoutMarkets = lstAccountsDonebyAdminagainstthisAgent.Sum(item1 => Convert.ToDecimal(item1.Debit)) - lstAccountsDonebyAdminagainstthisAgent.Sum(item1 => Convert.ToDecimal(item1.Credit));

                                    }
                                    //List<UserAccounts> lstAccountsDonebyAdminagainstthisAgent = lstAccountsDonebyAdmin.Where(item1 => item1.AccountsTitle.Contains("(UserID=" + item1.UserID.ToString() + ")")).ToList();
                                    //if (lstAccountsDonebyAdminagainstthisAgent.Count > 0)
                                    //{
                                    //    TotalAdminAmountWithoutMarkets = lstAccountsDonebyAdminagainstthisAgent.Sum(item1 => Convert.ToDecimal(item1.Debit)) - lstAccountsDonebyAdminagainstthisAgent.Sum(item1 => Convert.ToDecimal(item1.Credit));
                                    //}
                                }
                                List<UserAccounts> objCurrentUserAccountlst = lstUserAccounts.Where(itemagent => itemagent.UserID.ToString() == item.ID.ToString()).ToList();
                                if (objCurrentUserAccountlst.Count > 0)
                                {
                                    UserAccounts objCurrentUserAccount = objCurrentUserAccountlst[0];
                                    if (TotalAdminAmountWithoutMarkets < 0)
                                    {
                                        objCurrentUserAccount.Debit = (Convert.ToDecimal(objCurrentUserAccount.Debit) + TotalAdminAmountWithoutMarkets).ToString();

                                    }
                                    else
                                    {
                                        objCurrentUserAccount.Credit = (Convert.ToDecimal(objCurrentUserAccount.Credit) + TotalAdminAmountWithoutMarkets).ToString();
                                        //if (Convert.ToDecimal(objCurrentUserAccount.Credit)<0) {
                                        //    objCurrentUserAccount.Credit = (-1 * Convert.ToDecimal(objCurrentUserAccount.Credit)).ToString();
                                        //}
                                    }
                                    if (Convert.ToDecimal(objCurrentUserAccount.Debit) < 0)
                                    {
                                        objCurrentUserAccount.Debit = (-1 * Convert.ToDecimal(objCurrentUserAccount.Debit)).ToString();
                                    }
                                    if (Convert.ToDecimal(objCurrentUserAccount.Credit) < 0)
                                    {
                                        objCurrentUserAccount.Credit = (-1 * Convert.ToDecimal(objCurrentUserAccount.Credit)).ToString();
                                    }
                                    TotalAdminAmountWithoutMarkets = 0;
                                }
                                else
                                {
                                    UserAccounts objNewUseAccount = new UserAccounts();
                                    objNewUseAccount.UserName = item.UserName;
                                    objNewUseAccount.UserType = "Agent";
                                    objNewUseAccount.UserID = item.ID;
                                    objNewUseAccount.OpeningBalance = objUsersServiceCleint.GetStartingBalance(item.ID, LoggedinUserDetail.PasswordForValidate);
                                    TotalAdminAmountWithoutMarkets = -1 * TotalAdminAmountWithoutMarkets;
                                    if (TotalAdminAmountWithoutMarkets >= 0)
                                    {
                                        objNewUseAccount.Debit = TotalAdminAmountWithoutMarkets.ToString();
                                        objNewUseAccount.Credit = "0.00";
                                    }
                                    else
                                    {
                                        objNewUseAccount.Credit = (-1 * TotalAdminAmountWithoutMarkets).ToString();
                                        objNewUseAccount.Debit = "0.00";

                                    }
                                    lstUserAccounts.Add(objNewUseAccount);
                                    TotalAdminAmountWithoutMarkets = 0;
                                }
                                //commisionagent
                                decimal AgentCommission = 0;
                                try
                                {
                                    AgentCommission = objUsersServiceCleint.GetTotalAgentCommissionbyAgentID(item.ID, LoggedinUserDetail.PasswordForValidate);
                                    UserAccounts objNewUseAccount = new UserAccounts();
                                    objNewUseAccount.UserName = item.UserName;
                                    objNewUseAccount.UserType = "Agent";
                                    objNewUseAccount.UserID = item.ID;
                                    objNewUseAccount.OpeningBalance = objUsersServiceCleint.GetStartingBalance(item.ID, LoggedinUserDetail.PasswordForValidate);

                                    if (AgentCommission >= 0)
                                    {
                                        objNewUseAccount.Debit = AgentCommission.ToString();
                                        objNewUseAccount.Credit = "0.00";
                                    }
                                    else
                                    {
                                        objNewUseAccount.Credit = (-1 * AgentCommission).ToString();
                                        objNewUseAccount.Debit = "0.00";

                                    }
                                    lstUserAccounts.Add(objNewUseAccount);
                                    AgentCommission = 0;
                                }
                                catch (System.Exception ex)
                                {


                                }
                            }
                        }


                    }

                    if (UserID==999999)
                    {
                        foreach (var item in lstonlySuperAllforSamiAdmin)
                        {
                            //decimal TotAdminAmount = 0;
                            decimal SuperAmount = 0;
                            decimal SuperAmount1 = 0;
                            //decimal TotalAdminAmountWithoutMarkets = 0;
                            if (item.ID > 0 && item.ID != 73 && item.ID != 3)
                            {
                                List<UserAccounts> lstUserAccountsAgentForsuper = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(item.ID, isCredit, LoggedinUserDetail.PasswordForValidate));

                                 lstUserAccountsAgentForsuper = lstUserAccountsAgentForsuper.GroupBy(x => x.UserID).Select(g => g.First()).ToList();

                                foreach (var item2 in lstUserAccountsAgentForsuper)
                                {
                                    if (item2.UserID > 0 && item2.UserID != 73 && item2.UserID != 3)
                                    {
                                        List<UserAccounts> lstUserAccountsusersForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(Convert.ToInt32(item2.UserID), isCredit, LoggedinUserDetail.PasswordForValidate));

                                        if (lstUserAccountsusersForAgent.Count > 0)
                                        {
                                            lstUserAccountsusersForAgent = lstUserAccountsusersForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                                            foreach (UserAccounts objuserAccounts in lstUserAccountsusersForAgent)
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
                                                            decimal samiadmincommssion =100- (superpercent + AgentRate);
                                                            Comissionamount = (samiadmincommssion / 100) * Comissionamount;
                                                        }

                                                        TotAdminAmount += -1 * (ActualAmount  - (AgentAmount + SuperAmount) );
                                                        TotAdminAmount += Comissionamount;
                                                        //SuperAmount1 += -1 * SuperAmount;
                                                    }
                                                    else
                                                    {
                                                        ActualAmount = -1 * ActualAmount;
                                                        SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                        decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                        TotAdminAmount += ActualAmount - (AgentAmount + SuperAmount);
                                                        //SuperAmount1 += SuperAmount;

                                                    }

                                                }
                                            }
                                           // TotAdminAmount = (TotAdminAmount) + (SuperAmount1);
                                            UserAccounts objNewUseAccount = new UserAccounts();
                                            objNewUseAccount.UserName = item.UserName;
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
                                 
                                // List<UserAccounts> lstAccountsDonebyAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(LoggedinUserDetail.GetUserID(), false, LoggedinUserDetail.PasswordForValidate));
                                int createdbyid = objUsersServiceCleint.GetCreatedbyID(item.ID);


                                List<UserAccounts> lstAccountsDonebyAdmin1 = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(createdbyid, false, LoggedinUserDetail.PasswordForValidate));
                                if (lstAccountsDonebyAdmin.Count > 0)
                                {
                                    List<UserAccounts> lstAccountsDonebyAdminagainstthisAgent = lstAccountsDonebyAdmin1.Where(item1 => item1.AccountsTitle.Contains("(UserID=" + item.ID.ToString() + ")")).ToList();
                                    if (lstAccountsDonebyAdminagainstthisAgent.Count > 0)
                                    {
                                        TotalAdminAmountWithoutMarkets = lstAccountsDonebyAdminagainstthisAgent.Sum(item1 => Convert.ToDecimal(item1.Debit)) - lstAccountsDonebyAdminagainstthisAgent.Sum(item1 => Convert.ToDecimal(item1.Credit));

                                    }
                                    //List<UserAccounts> lstAccountsDonebyAdminagainstthisAgent = lstAccountsDonebyAdmin.Where(item1 => item1.AccountsTitle.Contains("(UserID=" + item1.UserID.ToString() + ")")).ToList();
                                    //if (lstAccountsDonebyAdminagainstthisAgent.Count > 0)
                                    //{
                                    //    TotalAdminAmountWithoutMarkets = lstAccountsDonebyAdminagainstthisAgent.Sum(item1 => Convert.ToDecimal(item1.Debit)) - lstAccountsDonebyAdminagainstthisAgent.Sum(item1 => Convert.ToDecimal(item1.Credit));
                                    //}
                                }
                                List<UserAccounts> objCurrentUserAccountlst = lstUserAccounts.Where(itemagent => itemagent.UserID.ToString() == item.ID.ToString()).ToList();
                                if (objCurrentUserAccountlst.Count > 0)
                                {
                                    UserAccounts objCurrentUserAccount = objCurrentUserAccountlst[0];
                                    if (TotalAdminAmountWithoutMarkets < 0)
                                    {
                                        objCurrentUserAccount.Debit = (Convert.ToDecimal(objCurrentUserAccount.Debit) + TotalAdminAmountWithoutMarkets).ToString();

                                    }
                                    else
                                    {
                                        objCurrentUserAccount.Credit = (Convert.ToDecimal(objCurrentUserAccount.Credit) + TotalAdminAmountWithoutMarkets).ToString();
                                        //if (Convert.ToDecimal(objCurrentUserAccount.Credit)<0) {
                                        //    objCurrentUserAccount.Credit = (-1 * Convert.ToDecimal(objCurrentUserAccount.Credit)).ToString();
                                        //}
                                    }
                                    if (Convert.ToDecimal(objCurrentUserAccount.Debit) < 0)
                                    {
                                        objCurrentUserAccount.Debit = (-1 * Convert.ToDecimal(objCurrentUserAccount.Debit)).ToString();
                                    }
                                    if (Convert.ToDecimal(objCurrentUserAccount.Credit) < 0)
                                    {
                                        objCurrentUserAccount.Credit = (-1 * Convert.ToDecimal(objCurrentUserAccount.Credit)).ToString();
                                    }
                                    TotalAdminAmountWithoutMarkets = 0;
                                }
                                else
                                {
                                    UserAccounts objNewUseAccount = new UserAccounts();
                                    objNewUseAccount.UserName = item.UserName;
                                    objNewUseAccount.UserType = "Agent";
                                    objNewUseAccount.UserID = item.ID;
                                    objNewUseAccount.OpeningBalance = objUsersServiceCleint.GetStartingBalance(item.ID, LoggedinUserDetail.PasswordForValidate);
                                    TotalAdminAmountWithoutMarkets = -1 * TotalAdminAmountWithoutMarkets;
                                    if (TotalAdminAmountWithoutMarkets >= 0)
                                    {
                                        objNewUseAccount.Debit = TotalAdminAmountWithoutMarkets.ToString();
                                        objNewUseAccount.Credit = "0.00";
                                    }
                                    else
                                    {
                                        objNewUseAccount.Credit = (-1 * TotalAdminAmountWithoutMarkets).ToString();
                                        objNewUseAccount.Debit = "0.00";

                                    }
                                    lstUserAccounts.Add(objNewUseAccount);
                                    TotalAdminAmountWithoutMarkets = 0;
                                }
                                //commisionagent
                               
                            }
                        }


                    }




                    if (lstUserAccounts.Count > 0)
                    {

                        var lstUserIDs = lstUserAccounts.Select(item1 => new { item1.UserID }).Distinct().ToArray();

                        foreach (var useritem in lstUserIDs)
                        {
                            List<UserAccounts> lstUseraccountsbyUser = lstUserAccounts.Where(item2 => item2.UserID == useritem.UserID).ToList();
                            if (lstUseraccountsbyUser.Count > 0)
                            {
                                BalanceSheet objBalanceSheet3 = new BalanceSheet();
                                if (lstUseraccountsbyUser[0].UserType == null)
                                {
                                    objBalanceSheet3.Username = Crypto.Decrypt(lstUseraccountsbyUser[0].UserName);
                                }
                                else
                                {
                                    objBalanceSheet3.Username = lstUseraccountsbyUser[0].UserName;
                                }
                                objBalanceSheet3.StartingBalance = lstUseraccountsbyUser[0].OpeningBalance;
                                objBalanceSheet3.UserID = Convert.ToInt32(lstUseraccountsbyUser[0].UserID);
                                decimal Profitorloss = lstUseraccountsbyUser.Sum(item3 => Convert.ToDecimal(item3.Debit)) - lstUseraccountsbyUser.Sum(item3 => Convert.ToDecimal(item3.Credit));
                                if (Profitorloss >= 0)
                                {
                                    objBalanceSheet3.Profit = Profitorloss;

                                }
                                else
                                {
                                    objBalanceSheet3.Loss = Profitorloss;
                                }
                                lstBalanceSheet.Add(objBalanceSheet3);
                            }
                        }

                    }
                    List<UserAccounts> lstUserAccountsAdmin = new List<UserAccounts>();
                    if (LoggedinUserDetail.GetUserTypeID() == 1 && Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue) == 73)
                    {
                        SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                        //  lstUserAccountsAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), isCredit));
                    }
                    else
                    {
                        lstUserAccountsAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(LoggedinUserDetail.GetUserID(), isCredit, LoggedinUserDetail.PasswordForValidate));
                    }
                    if (lstUserAccountsAdmin.Count > 0)
                    {
                        BalanceSheet objBalanceSheet1 = new BalanceSheet();
                        objBalanceSheet1.Username = "Book account";
                        objBalanceSheet1.StartingBalance = lstUserAccountsAdmin[0].OpeningBalance;
                        if (UserID > 0 && UserID != 73)
                        {
                            lstUserAccountsAdmin = lstUserAccountsAdmin.Where(item => item.MarketBookID != "").ToList();
                        }
                        //
                        decimal Profitorloss1 = lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Debit)) - lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Credit));
                        if (Profitorloss1 >= 0)
                        {
                            objBalanceSheet1.Profit = Profitorloss1;

                        }
                        else
                        {
                            objBalanceSheet1.Loss = Profitorloss1;
                        }

                        lstBalanceSheet.Add(objBalanceSheet1);
                    }
                }
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {

                    List<UserAccounts> lstUserAccounts = new List<UserAccounts>(); //JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(UserID, isCredit, LoggedinUserDetail.PasswordForValidate));
                    List<UserIDandUserType> lstonlysuperForadmin = new List<UserIDandUserType>();
                    lstonlysuperForadmin = lstonlyAgentsAll.Where(s => s.UserTypeID == 8).ToList();

                    decimal TotalAdminAmountWithoutMarkets1 = 0;
                    foreach (var item in lstonlysuperForadmin)
                    {
                        decimal TotAdmincommession = 0;
                      
                        List<UserAccounts> AgentCommission = new List<UserAccounts>();
                        decimal TotAdminAmountforsuper = 0;
                        UserAccounts objNewUseAccount = new UserAccounts();

                        List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByIDForSuper(item.ID, false, LoggedinUserDetail.PasswordForValidate));
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
                                    decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                    decimal superpercent = SuperRate - AgentRate;
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

                                        TotAdminAmountforsuper += -1 * (ActualAmount - (SuperAmount + AgentAmount) );
                                    }
                                    else
                                    {
                                        ActualAmount = -1 * ActualAmount;
                                        decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                        decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                        TotAdminAmountforsuper += ActualAmount - (AgentAmount + SuperAmount);
                                    }
                                }
                            }
                            // NetBalance =;
                        }
                        //objNewUseAccount.UserName = item.UserName;
                        //objNewUseAccount.UserType = "Super";
                        //objNewUseAccount.UserID = item.ID;


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
                        }
                        catch (System.Exception ex)
                        {
                        }

                          objNewUseAccount.NetProfitorLoss = (-1 * (TotAdminAmountforsuper) + (-1 * TotAdmincommession));
                        //UserAccounts objNewUseAccount = new UserAccounts();
                        //objNewUseAccount.UserName = item.UserName;
                        //objNewUseAccount.UserType = "Super";
                        //objNewUseAccount.UserID = item.ID;
                        //objNewUseAccount.OpeningBalance = objUsersServiceCleint.GetStartingBalance(item.ID, LoggedinUserDetail.PasswordForValidate);
                        //TotAdminAmountforsuper = (-1 * (TotAdminAmountforsuper) + (-1 * TotAdmincommession));
                        //if (TotAdminAmountforsuper >= 0)
                        //{
                        //    objNewUseAccount.Debit = TotAdminAmountforsuper.ToString();
                        //    objNewUseAccount.Credit = "0.00";
                        //}
                        //else
                        //{
                        //    objNewUseAccount.Credit = (-1 * TotAdminAmountforsuper).ToString();
                        //    objNewUseAccount.Debit = "0.00";

                        //}
                        BalanceSheet objBalanceSheet5 = new BalanceSheet();    
                        objBalanceSheet5.Username = item.UserName;
                        objBalanceSheet5.StartingBalance = objUsersServiceCleint.GetStartingBalance(item.ID, LoggedinUserDetail.PasswordForValidate);
                        objBalanceSheet5.UserID = item.ID;
                        decimal Profitorloss = (-1 * (TotAdminAmountforsuper) + (-1 * TotAdmincommession));
                        if (Profitorloss >= 0)
                        {
                            objBalanceSheet5.Profit = Profitorloss;

                        }
                        else
                        {
                            objBalanceSheet5.Loss = Profitorloss;
                        }

                        lstBalanceSheet.Add(objBalanceSheet5);

                    }

                    ///////////////
                    List<UserIDandUserType> lstonlysamiadminForadmin = new List<UserIDandUserType>();
                    lstonlysamiadminForadmin = lstonlyAgentsAll.Where(s => s.UserTypeID == 9).ToList();

                    foreach (var item in lstonlysamiadminForadmin)
                    {
                        //decimal TotAdminAmount = 0;
                        decimal SuperAmount = 0;
                        decimal SuperAmount1 = 0;
                        //decimal TotalAdminAmountWithoutMarkets = 0;
                        if (item.ID > 0 && item.ID != 73 && item.ID != 3)
                        {
                            List<UserAccounts> lstUserAccountsAgentForSamiadmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(item.ID, isCredit, LoggedinUserDetail.PasswordForValidate));
                            lstUserAccountsAgentForSamiadmin = lstUserAccountsAgentForSamiadmin.GroupBy(x => x.UserID).Select(g => g.First()).ToList();

                            foreach (var item2 in lstUserAccountsAgentForSamiadmin)
                            {
                                if (item2.UserID > 0 && item2.UserID != 73 && item2.UserID != 3)
                                {
                                    List<UserAccounts> samiadminCommission = new List<UserAccounts>();
                                    List<UserAccounts> lstUserAccountsusersForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(Convert.ToInt32(item2.UserID), isCredit, LoggedinUserDetail.PasswordForValidate));
                                    lstUserAccountsusersForAgent = lstUserAccountsusersForAgent.GroupBy(x => x.UserID).Select(g => g.First()).ToList();

                                    foreach (var item3 in lstUserAccountsusersForAgent)
                                    {
                                        List<UserAccounts> lstUserAccountsusersForAgentforsuper = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(Convert.ToInt32(item3.UserID), isCredit, LoggedinUserDetail.PasswordForValidate));
                                        decimal TotAdmincommession = 0;
                                        if (lstUserAccountsusersForAgentforsuper.Count > 0)
                                        {
                                            samiadminCommission = lstUserAccountsusersForAgentforsuper.Where(item1 => item1.AccountsTitle == "Commission").ToList();
                                            lstUserAccountsusersForAgent = lstUserAccountsusersForAgentforsuper.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                                            foreach (UserAccounts objuserAccounts in lstUserAccountsusersForAgentforsuper)
                                            {
                                                if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                                                {
                                                    int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                                    int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                                    int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                                    int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiAdminRate);
                                                    decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                                    decimal superpercent = SuperRate - AgentRate;
                                                    decimal samiadminpercent = SamiadminRate - (superpercent + AgentRate);
                                                    if (ActualAmount > 0)
                                                    {
                                                        decimal SuperAmount11 = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
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

                                                        TotAdminAmount += -1 * (ActualAmount - (SuperAmount11 + AgentAmount + SamiadminAmount));
                                                    }
                                                    else
                                                    {
                                                        ActualAmount = -1 * ActualAmount;
                                                        decimal SuperAmount11 = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                        decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                        decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                                        TotAdminAmount += ActualAmount - (AgentAmount + SuperAmount11 + SamiadminAmount);
                                                    }
                                                }
                                            }
                                            try
                                            {
                                                foreach (UserAccounts objuserAccounts in samiadminCommission)
                                                {
                                                    int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                                    int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                                    int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                                    int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiAdminRate);
                                                    decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                                    decimal superpercent = SuperRate - AgentRate;
                                                    decimal samiadminpercent = SamiadminRate - (superpercent + AgentRate);
                                                    ActualAmount = -1 * ActualAmount;
                                                    decimal SuperAmount12 = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                    decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                    decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                                    TotAdmincommession += ActualAmount - (AgentAmount + SuperAmount12 + SamiadminAmount);
                                                }
                                                //AgentCommission = objUsersServiceCleint.GetTotalAgentCommissionbyAgentID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate);
                                            }
                                            catch (System.Exception ex)
                                            {


                                            }

                                            BalanceSheet objBalanceSheet6 = new BalanceSheet();
                                            objBalanceSheet6.Username = item.UserName;
                                            objBalanceSheet6.StartingBalance = objUsersServiceCleint.GetStartingBalance(item.ID, LoggedinUserDetail.PasswordForValidate);
                                            objBalanceSheet6.UserID = item.ID;
                                            decimal Profitorloss = (-1 * (TotAdminAmount) + (-1 * TotAdmincommession));
                                            if (Profitorloss >= 0)
                                            {
                                                objBalanceSheet6.Profit = Profitorloss;

                                            }
                                            else
                                            {
                                                objBalanceSheet6.Loss = Profitorloss;
                                            }

                                            lstBalanceSheet.Add(objBalanceSheet6);
                                            // TotAdminAmount = (TotAdminAmount) + (SuperAmount1);
                                                                   }
                                    }

                                }
                            }


                        }
                    }

                    return lstBalanceSheet;

                }
                if ( LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    //samiadminhisab
                    List<UserAccounts> lstUserAccounts = new List<UserAccounts>();
                   
                }
                    //HawalaAccount
                    if (UserID != 73 && LoggedinUserDetail.GetUserTypeID() == 8)
                {
                    int HawalaID;
                    if (LoggedinUserDetail.GetUserTypeID() == 8)
                    {
                        HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(LoggedinUserDetail.GetUserID());
                    }
                    else
                    {
                        HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                    }


                    decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, LoggedinUserDetail.PasswordForValidate));

                    decimal StartingBalance = Convert.ToDecimal(objUsersServiceCleint.GetStartingBalance(HawalaID, LoggedinUserDetail.PasswordForValidate));
                    BalanceSheet objBalanceSheetHawala = new BalanceSheet();
                    objBalanceSheetHawala.Username = "Hawala account";
                    objBalanceSheetHawala.StartingBalance = StartingBalance;

                    // lstHawalEntries = lstHawalEntries.Where(item => item.MarketBookID != "").ToList();

                    if (CurrentAccountBalance >= 0)
                    {
                        objBalanceSheetHawala.Profit = CurrentAccountBalance;

                    }
                    else
                    {
                        objBalanceSheetHawala.Loss = CurrentAccountBalance;
                    }

                    lstBalanceSheet.Add(objBalanceSheetHawala);
                }
                //
                lstBalanceSheet = lstBalanceSheet.OrderBy(item => item.Username).ToList();
                return lstBalanceSheet;
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                List<BalanceSheet> lstBalanceSheet = new List<BalanceSheet>();
                return lstBalanceSheet;
            }

        }
        public void ExportToPdf() //Datatable 
        {
            //Here set page size as A4

            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);

            try
            {
                string currenttime = string.Format("{0:hh:mm:ss tt}", DateTime.Now);
                currenttime = currenttime.Replace(":", "-");
                PdfWriter wri = PdfWriter.GetInstance(pdfDoc, new FileStream("d:\\Balance Sheet of " + DateTime.Now.Date.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + " " + currenttime + ".pdf", FileMode.Create));
                //PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();

                //Set Font Properties for PDF File
                var FontColour = new BaseColor(0, 0, 0);
                iTextSharp.text.Font fnt = FontFactory.GetFont("Times New Roman", 10, FontColour);
                var FontColour1 = new BaseColor(26, 178, 41);
                iTextSharp.text.Font fntgreen = FontFactory.GetFont("Times New Roman", 10, FontColour1);
                var FontColour2 = new BaseColor(244, 66, 66);
                iTextSharp.text.Font fntred = FontFactory.GetFont("Times New Roman", 10, FontColour2);
                Paragraph paragraphheading = new Paragraph("Balance Sheet of " + DateTime.Now.Date.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + " ( " + cmbUsersforBalanceSheet.Text + " )");
                paragraphheading.Alignment = Element.ALIGN_CENTER;
                Paragraph paragraphspace = new Paragraph("\n");
                Paragraph paragraphPlusCustomers = new Paragraph("Plus Customers");
                paragraphPlusCustomers.Alignment = Element.ALIGN_CENTER;
                Paragraph paragraphMinusCustomers = new Paragraph("Minus Customers");
                paragraphMinusCustomers.Alignment = Element.ALIGN_CENTER;
                DataGrid dt = DGVPlusCustomers;

                pdfDoc.Add(paragraphheading);
                pdfDoc.Add(paragraphspace);


                //  pdfDoc.Add(paragraphPlusCustomers);
                // pdfDoc.Add(paragraphspace);

                if (dt != null)
                {

                    PdfPTable PdfTable = new PdfPTable(3);
                    PdfTable.TotalWidth = 200f;
                    PdfTable.LockedWidth = true;
                    PdfPCell PdfPCell = null;

                    //Here we create PDF file tables

                    for (int rows = 0; rows < dt.Items.Count; rows++)
                    {
                        if (rows == 0)
                        {
                            PdfPCell = new PdfPCell(new Phrase(new Chunk("Customers", fnt)));
                            PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            PdfTable.AddCell(PdfPCell);
                            PdfPCell = new PdfPCell(new Phrase(new Chunk("Plus", fnt)));
                            PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            PdfTable.AddCell(PdfPCell);
                            PdfPCell = new PdfPCell(new Phrase(new Chunk("A/C", fnt)));
                            PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            PdfTable.AddCell(PdfPCell);

                        }
                        BalanceSheet currrow = (BalanceSheet)dt.Items[rows];
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(currrow.Username, fnt)));
                        PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        PdfTable.AddCell(PdfPCell);
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(currrow.Profit.ToString("N0").Replace(",", ""), fntgreen)));
                        PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        PdfTable.AddCell(PdfPCell);

                        PdfPCell = new PdfPCell(new Phrase(new Chunk(currrow.StartingBalance.ToString("N0").Replace(",", "") == "0" ? "A" : "C", fnt)));
                        PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        PdfTable.AddCell(PdfPCell);
                        //for (int column = 0; column < 2; column++)
                        //{
                        //    if (dt.Columns[column].Visible == true)
                        //    {
                        //        PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows].Cells[column].Value.ToString(), fnt)));
                        //        PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        //        PdfTable.AddCell(PdfPCell);
                        //    }
                        //}
                    }
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("-", fnt)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("-", fntgreen)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("-", fntgreen)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);

                    PdfPCell = new PdfPCell(new Phrase(new Chunk("Total", fnt)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk(txtTotalPlus.Text.ToString(), fntgreen)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("-", fntgreen)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    PdfTable.WriteSelectedRows(0, -1, pdfDoc.Left + 100, pdfDoc.Top - 40, wri.DirectContent);
                    // Finally Add pdf table to the document 
                    //pdfDoc.Add(PdfTable);
                }

                //   pdfDoc.Add(paragraphMinusCustomers);
                //  pdfDoc.Add(paragraphspace);

                dt = dgvMinusCustomers;

                if (dt != null)
                {

                    PdfPTable PdfTable = new PdfPTable(2);
                    PdfTable.TotalWidth = 200f;
                    PdfTable.LockedWidth = true;
                    PdfPCell PdfPCell = null;

                    //Here we create PDF file tables

                    for (int rows = 0; rows < dt.Items.Count; rows++)
                    {
                        if (rows == 0)
                        {
                            PdfPCell = new PdfPCell(new Phrase(new Chunk("Customers", fnt)));
                            PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            PdfTable.AddCell(PdfPCell);
                            PdfPCell = new PdfPCell(new Phrase(new Chunk("Minus", fnt)));
                            PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            PdfTable.AddCell(PdfPCell);

                        }
                        BalanceSheet currrow = (BalanceSheet)dt.Items[rows];
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(currrow.Username, fnt)));
                        PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                        PdfTable.AddCell(PdfPCell);
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(currrow.Loss.ToString("N0").Replace(",", ""), fntred)));
                        PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        PdfTable.AddCell(PdfPCell);

                    }
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("-", fnt)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("-", fntred)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("Total", fnt)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk(txtTotalMinus.Text.ToString(), fntred)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    PdfTable.WriteSelectedRows(0, -1, pdfDoc.Left + 320, pdfDoc.Top - 40, wri.DirectContent);
                }
                pdfDoc.Close();

                //Response.ContentType = "application/pdf";

                ////Set default file Name as current datetime
                //Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMdd") + ".pdf");

                //System.Web.HttpContext.Current.Response.Write(pdfDoc);

                //Response.Flush();
                //Response.End();

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                // Response.Write(ex.ToString());
            }
        }
        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 2)
            {
                cmbUsersforBalanceSheet.Visibility = Visibility.Collapsed;
                lblBalanceSheet.Visibility = Visibility.Collapsed;
                btnLoadBalanceSheet.Visibility = Visibility.Collapsed;
                List<BalanceSheet> lstBalanceSheet = BalanceSheet(LoggedinUserDetail.GetUserID(), false);
                List<BalanceSheet> lstPlusCustomers = lstBalanceSheet.Where(item => item.Profit > 0).ToList();
                List<BalanceSheet> lstMinusCustomers = lstBalanceSheet.Where(item => item.Loss < 0).ToList();
                DGVPlusCustomers.ItemsSource = lstPlusCustomers;
                dgvMinusCustomers.ItemsSource = lstMinusCustomers;

                decimal totPlus = lstPlusCustomers.Sum(item => item.Profit);
                decimal totminus = lstMinusCustomers.Sum(item => item.Loss);
                txtTotalPlus.Text = totPlus.ToString("N0").Replace(",", "");
                txtTotalMinus.Text = totminus.ToString("N0").Replace(",", "");
            }
            if (LoggedinUserDetail.GetUserTypeID() == 8)
            {
                if (Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue) > 0)
                {
                    int id = 59999;
                    List<BalanceSheet> lstBalanceSheet = BalanceSheet(Convert.ToInt32(id), false);
                    List<BalanceSheet> lstPlusCustomers = lstBalanceSheet.Where(item => item.Profit > 0).ToList();
                    List<BalanceSheet> lstMinusCustomers = lstBalanceSheet.Where(item => item.Loss < 0).ToList();
                    decimal totPlus = lstPlusCustomers.Sum(item => item.Profit);
                    decimal totminus = lstMinusCustomers.Sum(item => item.Loss);
                    txtTotalPlus.Text = totPlus.ToString("N0").Replace(",", "");
                    txtTotalMinus.Text = totminus.ToString("N0").Replace(",", "");

                    DGVPlusCustomers.ItemsSource = lstPlusCustomers;
                    dgvMinusCustomers.ItemsSource = lstMinusCustomers;
                }
                if (Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue) == 0)
                {
                    int id = 59999;
                    List<BalanceSheet> lstBalanceSheet = BalanceSheet(id, false);
                    List<BalanceSheet> lstPlusCustomers = lstBalanceSheet.Where(item => item.Profit > 0).ToList();
                    List<BalanceSheet> lstMinusCustomers = lstBalanceSheet.Where(item => item.Loss < 0).ToList();
                    decimal totPlus = lstPlusCustomers.Sum(item => item.Profit);
                    decimal totminus = lstMinusCustomers.Sum(item => item.Loss);
                    txtTotalPlus.Text = totPlus.ToString("N0").Replace(",", "");
                    txtTotalMinus.Text = totminus.ToString("N0").Replace(",", "");
                    DGVPlusCustomers.ItemsSource = lstPlusCustomers;
                    dgvMinusCustomers.ItemsSource = lstMinusCustomers;
                }
            }
            if (LoggedinUserDetail.GetUserTypeID() == 9)
            {
                if (Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue) > 0)
                {
                    int id = 999999;
                    List<BalanceSheet> lstBalanceSheet = BalanceSheet(Convert.ToInt32(id), false);
                    List<BalanceSheet> lstPlusCustomers = lstBalanceSheet.Where(item => item.Profit > 0).ToList();
                    List<BalanceSheet> lstMinusCustomers = lstBalanceSheet.Where(item => item.Loss < 0).ToList();
                    decimal totPlus = lstPlusCustomers.Sum(item => item.Profit);
                    decimal totminus = lstMinusCustomers.Sum(item => item.Loss);
                    txtTotalPlus.Text = totPlus.ToString("N0").Replace(",", "");
                    txtTotalMinus.Text = totminus.ToString("N0").Replace(",", "");

                    DGVPlusCustomers.ItemsSource = lstPlusCustomers;
                    dgvMinusCustomers.ItemsSource = lstMinusCustomers;
                }
                if (Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue) == 0)
                {
                    int id = 999999;
                    List<BalanceSheet> lstBalanceSheet = BalanceSheet(id, false);
                    List<BalanceSheet> lstPlusCustomers = lstBalanceSheet.Where(item => item.Profit > 0).ToList();
                    List<BalanceSheet> lstMinusCustomers = lstBalanceSheet.Where(item => item.Loss < 0).ToList();
                    decimal totPlus = lstPlusCustomers.Sum(item => item.Profit);
                    decimal totminus = lstMinusCustomers.Sum(item => item.Loss);
                    txtTotalPlus.Text = totPlus.ToString("N0").Replace(",", "");
                    txtTotalMinus.Text = totminus.ToString("N0").Replace(",", "");
                    DGVPlusCustomers.ItemsSource = lstPlusCustomers;
                    dgvMinusCustomers.ItemsSource = lstMinusCustomers;
                }
            }
        }

        private void btnLoadBalanceSheet_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    if (Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue) > 0)
                    {

                        List<BalanceSheet> lstBalanceSheet = BalanceSheet(Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue), false);
                        List<BalanceSheet> lstPlusCustomers = lstBalanceSheet.Where(item => item.Profit > 0).ToList();
                        List<BalanceSheet> lstMinusCustomers = lstBalanceSheet.Where(item => item.Loss < 0).ToList();
                        decimal totPlus = lstPlusCustomers.Sum(item => item.Profit);
                        decimal totminus = lstMinusCustomers.Sum(item => item.Loss);
                        txtTotalPlus.Text = totPlus.ToString("N0").Replace(",", "");
                        txtTotalMinus.Text = totminus.ToString("N0").Replace(",", "");

                        DGVPlusCustomers.ItemsSource = lstPlusCustomers;
                        dgvMinusCustomers.ItemsSource = lstMinusCustomers;
                    }
                    else
                    {
                        List<BalanceSheet> lstBalanceSheet = BalanceSheet(LoggedinUserDetail.GetUserID(), false);
                        List<BalanceSheet> lstPlusCustomers = lstBalanceSheet.Where(item => item.Profit > 0).ToList();
                        List<BalanceSheet> lstMinusCustomers = lstBalanceSheet.Where(item => item.Loss < 0).ToList();
                        decimal totPlus = lstPlusCustomers.Sum(item => item.Profit);
                        decimal totminus = lstMinusCustomers.Sum(item => item.Loss);
                        txtTotalPlus.Text = totPlus.ToString("N0").Replace(",", "");
                        txtTotalMinus.Text = totminus.ToString("N0").Replace(",", "");
                        DGVPlusCustomers.ItemsSource = lstPlusCustomers;
                        dgvMinusCustomers.ItemsSource = lstMinusCustomers;
                    }

                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 8)
                    {
                        if (Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue) > 0)
                        {

                            List<BalanceSheet> lstBalanceSheet = BalanceSheet(Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue), false);
                            List<BalanceSheet> lstPlusCustomers = lstBalanceSheet.Where(item => item.Profit > 0).ToList();
                            List<BalanceSheet> lstMinusCustomers = lstBalanceSheet.Where(item => item.Loss < 0).ToList();
                            decimal totPlus = lstPlusCustomers.Sum(item => item.Profit);
                            decimal totminus = lstMinusCustomers.Sum(item => item.Loss);
                            txtTotalPlus.Text = totPlus.ToString("N0").Replace(",", "");
                            txtTotalMinus.Text = totminus.ToString("N0").Replace(",", "");

                            DGVPlusCustomers.ItemsSource = lstPlusCustomers;
                            dgvMinusCustomers.ItemsSource = lstMinusCustomers;
                        }
                        if (Convert.ToInt32(cmbUsersforBalanceSheet.SelectedValue) == 0)
                        {
                            int id = 59999;
                            List<BalanceSheet> lstBalanceSheet = BalanceSheet(id, false);
                            List<BalanceSheet> lstPlusCustomers = lstBalanceSheet.Where(item => item.Profit > 0).ToList();
                            List<BalanceSheet> lstMinusCustomers = lstBalanceSheet.Where(item => item.Loss < 0).ToList();
                            decimal totPlus = lstPlusCustomers.Sum(item => item.Profit);
                            decimal totminus = lstMinusCustomers.Sum(item => item.Loss);
                            txtTotalPlus.Text = totPlus.ToString("N0").Replace(",", "");
                            txtTotalMinus.Text = totminus.ToString("N0").Replace(",", "");
                            DGVPlusCustomers.ItemsSource = lstPlusCustomers;
                            dgvMinusCustomers.ItemsSource = lstMinusCustomers;
                        }

                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            List<BalanceSheet> lstBalanceSheet = BalanceSheet(LoggedinUserDetail.GetUserID(), false);
                            List<BalanceSheet> lstPlusCustomers = lstBalanceSheet.Where(item => item.Profit > 0).ToList();
                            List<BalanceSheet> lstMinusCustomers = lstBalanceSheet.Where(item => item.Loss < 0).ToList();
                            decimal totPlus = lstPlusCustomers.Sum(item => item.Profit);
                            decimal totminus = lstMinusCustomers.Sum(item => item.Loss);
                            txtTotalPlus.Text = totPlus.ToString("N0").Replace(",", "");
                            txtTotalMinus.Text = totminus.ToString("N0").Replace(",", "");
                            DGVPlusCustomers.ItemsSource = lstPlusCustomers;
                            dgvMinusCustomers.ItemsSource = lstMinusCustomers;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExportToPdf();
            Xceed.Wpf.Toolkit.MessageBox.Show("Successfully saved to pdf.");
        }

        private void DGVPlusCustomers_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DGVPlusCustomers.Items.Count > 0)
            {
                LedgerWindow objFrmLedger = new LedgerWindow();
                objFrmLedger.fromBalanceSheet = 1;
                BalanceSheet currrow = (BalanceSheet)DGVPlusCustomers.SelectedItem;
                objFrmLedger.UserIDforLedger = Convert.ToInt32(currrow.UserID);
                objFrmLedger.ShowDialog();
            }
        }

        private void dgvMinusCustomers_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (dgvMinusCustomers.Items.Count > 0)
            {
                LedgerWindow objFrmLedger = new LedgerWindow();
                objFrmLedger.fromBalanceSheet = 1;
                BalanceSheet currrow = (BalanceSheet)dgvMinusCustomers.SelectedItem;
                objFrmLedger.UserIDforLedger = Convert.ToInt32(currrow.UserID);
                objFrmLedger.ShowDialog();
            }
        }

        private void tabControl2_Loaded(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                try
                {
                    List<EventLineMarketandOddsForAssociation_Result> lstLinevmarketsandEvents = JsonConvert.DeserializeObject<List<EventLineMarketandOddsForAssociation_Result>>(objUsersServiceCleint.GetLineandMatchOddsforAssociation());
                    if (lstLinevmarketsandEvents.Count > 0)
                    {
                        var lstLinevmarkets = lstLinevmarketsandEvents.Where(item => item.isLineMarket == 1).ToList();
                        var lstEvents = lstLinevmarketsandEvents.Where(item => item.isLineMarket == 0).ToList();
                        if (lstLinevmarkets.Count > 0)
                        {

                            cmbLinevMarkets.ItemsSource = lstLinevmarkets;
                            cmbLinevMarkets.DisplayMemberPath = "EventName";
                            cmbLinevMarkets.SelectedValuePath = "EventID";
                        }
                        if (lstEvents.Count > 0)
                        {

                            cmbEvents.ItemsSource = lstEvents;
                            cmbEvents.DisplayMemberPath = "EventName";
                            cmbEvents.SelectedValuePath = "EventID";
                        }
                    }
                    try
                    {
                        //List<RecentMatches> lstREcentMatches = JsonConvert.DeserializeObject<List<RecentMatches>>(objUsersServiceCleint.GetRecentMatchesFromCricketAPI());
                        //if (lstREcentMatches.Count > 0)
                        //{
                        //    RecentMatches objRecetMatches = new RecentMatches();
                        //    objRecetMatches.Key = "";
                        //    objRecetMatches.MatchName = "Please Select";
                        //    lstREcentMatches.Insert(0, objRecetMatches);
                        //    cmbCricketAPIKeys.ItemsSource = null;
                        //    cmbCricketAPIKeys.Items.Clear();
                        //    cmbCricketAPIKeys.ItemsSource = lstREcentMatches;
                        //    cmbCricketAPIKeys.DisplayMemberPath = "MatchName";
                        //    cmbCricketAPIKeys.SelectedValuePath = "Key";
                        //}
                    }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                    {

                    }

                    chkAutomaticResultPostFancy.IsChecked = objUsersServiceCleint.GetFancyResultPostSetting();

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {

                }
            }
        }
        public class RecentMatches
        {
            public string Key { get; set; }
            public string MatchName { get; set; }
        }
        private void btnUpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            if (txtUsernameupdate.Text.Length > 0 && txtUserPhoneUpdate.Text.Length > 0)
            {
                objUsersServiceCleint.UpdateUserPhoneandNamebyUserId(LoggedinUserDetail.GetUserID(), txtUsernameupdate.Text, txtUserPhoneUpdate.Text);
                if (txtPassword.Text.Length >= 6)
                {
                    if (LoggedinUserDetail.GetUserID() > 0)
                    {
                        objUsersServiceCleint.ResetPasswordofUser(LoggedinUserDetail.GetUserID(), Crypto.Encrypt(txtPassword.Text), LoggedinUserDetail.GetUserID(), DateTime.Now, LoggedinUserDetail.PasswordForValidate);
                    }

                    MessageBox.Show("Updated successfully");
                }
                else
                {
                    MessageBox.Show("Minimum length is 6 digits.");
                }
                Xceed.Wpf.Toolkit.MessageBox.Show("Updated successfully");

            }
        }

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            if ((txtUsername.Text.Length == 0))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Enter Username");

                return;
            }
            if ((txtName.Text.Length == 0))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Enter Name");

                return;
            }
            if ((txtPasswordAdd.Text.Length < 6))
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Enter password minimum 6 characters");

                return;
            }
            LoggedinUserDetail.CheckifUserLogin();
            var result = objUsersServiceCleint.CheckifUserExists(Crypto.Encrypt(txtUsername.Text.ToLower()));
            if (result == "0")
            {
                int CreatedbyID = LoggedinUserDetail.GetUserID();
                string AccountBalance = objUsersServiceCleint.GetCurrentBalancebyUser(CreatedbyID, LoggedinUserDetail.PasswordForValidate);
                Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(CreatedbyID);

                    AccountBalance = objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, LoggedinUserDetail.PasswordForValidate);
                    Newaccountbalance = Convert.ToDecimal(AccountBalance);
                    if (Convert.ToInt32(txtAgentrate.Text) > LoggedinUserDetail.MaxAgentRateLimit)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Agent Rate cannot greater than " + LoggedinUserDetail.MaxAgentRateLimit.ToString() + " %.");
                        return;
                    }

                    if (Convert.ToInt32(txtAccountBalance.Text) > LoggedinUserDetail.MaxBalanceTransferLimit && LoggedinUserDetail.GetUserID() != 73)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Account Balance should be not be greater than " + LoggedinUserDetail.MaxBalanceTransferLimit.ToString());
                        return;
                    }
                    if (Newaccountbalance < Convert.ToInt32(txtAccountBalance.Text))
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Account Balance is more than available balance");
                        return;
                    }
                    else
                    {
                        string userid = objUsersServiceCleint.AddUser(txtName.Text, txtPhone.Text, txtEmail.Text, Crypto.Encrypt(txtUsername.Text.ToLower()), Crypto.Encrypt(txtPasswordAdd.Text), txtLocation.Text, Convert.ToDecimal(txtAccountBalance.Text), Convert.ToInt32(cmbUserType.SelectedValue), CreatedbyID, Crypto.Encrypt(txtAgentrate.Text), 2000, 15000, true, 2000, 15000, 2000, 15000, 2000, 15000, 2000, 500000, 2000, 150000, 2000, 150000, 2000, 15000, 2000, 15000, 15000, 2000, 15000, 2000, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                        // objAccountsService.AddtoUsersAccounts("Amount Credit to your account", User.AccountBalance.ToString(), "0.00", Convert.ToInt32(userid), "", DateTime.Now, Crypto.Encrypt(User.AgentRateC), "", 0);
                        objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + txtUsername.Text + ")", "0.00", txtAccountBalance.Text.ToString(), HawalaID, "", DateTime.Now, "","","", "", Newaccountbalance, false, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, Convert.ToDecimal(txtAccountBalance.Text), LoggedinUserDetail.PasswordForValidate);
                        int AhmadRate=objUsersServiceCleint.GetAhmadRate(LoggedinUserDetail.GetUserID());
                        objUsersServiceCleint.UpdateAhmadRate(Convert.ToInt32(userid), AhmadRate);
                        objUsersServiceCleint.UpdateMaxAgentRate(Convert.ToInt32(userid), Convert.ToInt32(txtAgentrate.Text));
                        try
                        {
                            List<AllUserMarkets> lstUserMarket = JsonConvert.DeserializeObject<List<AllUserMarkets>>(objUsersServiceCleint.GetAllUserMarketbyUserID(CreatedbyID));
                            if (lstUserMarket.Count > 0)
                            {
                                List<string> allusersmarket = new List<string>();
                                foreach (var usermarket in lstUserMarket)
                                {
                                    var orignalopendate = Convert.ToDateTime(usermarket.EventOpenDate).ToString("s");
                                    var objusermarket = usermarket.EventTypeID.ToString() + "#" + usermarket.EventTypeName.ToString() + "#" + usermarket.CompetitionID.ToString() + "#" + usermarket.CompetitionName.ToString() + "#" + usermarket.EventID.ToString() + "#" + usermarket.EventName.ToString() + "#" + usermarket.MarketCatalogueID.ToString() + "#" + usermarket.MarketCatalogueName + "#NotInsert#" + orignalopendate + "#" + usermarket.AssociateEventID + "#" + usermarket.GetMatchUpdatesFrom + "#" + usermarket.TotalOvers + "#" + usermarket.CountryCode;
                                    allusersmarket.Add(objusermarket);
                                }
                                objUsersServiceCleint.InsertUserMarket(allusersmarket.ToArray(), Convert.ToInt32(userid), CreatedbyID, false);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        LoggedinUserDetail.InsertActivityLog(CreatedbyID, "Created new user (" + txtUsername.Text.ToString() + ")");
                        Xceed.Wpf.Toolkit.MessageBox.Show("Added successfully");
                        GetusersbyUserTypeReload();
                        GetUsersbyUsersType();
                        ClearAllTextFields();
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 9)
                    {
                        CreatedbyID = LoggedinUserDetail.GetUserID();
                        int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(CreatedbyID);
                        AccountBalance = objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, LoggedinUserDetail.PasswordForValidate);
                        Newaccountbalance = Convert.ToDecimal(AccountBalance);
                        if (Convert.ToInt32(txtAgentrate.Text) > LoggedinUserDetail.MaxAgentRateLimit)
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Agent Rate cannot greater than " + LoggedinUserDetail.MaxAgentRateLimit.ToString() + " %.");
                            return;
                        }

                        if (Convert.ToInt32(txtAccountBalance.Text) > LoggedinUserDetail.MaxBalanceTransferLimit && LoggedinUserDetail.GetUserID() != 73)
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Account Balance should be not be greater than " + LoggedinUserDetail.MaxBalanceTransferLimit.ToString());
                            return;
                        }
                        if (Newaccountbalance < Convert.ToInt32(txtAccountBalance.Text))
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Account Balance is more than available balance");
                            return;
                        }
                        string userid = "0";
                       
                            userid = objUsersServiceCleint.AddUser(txtName.Text, txtPhone.Text, txtEmail.Text, Crypto.Encrypt(txtUsername.Text.ToLower()), Crypto.Encrypt(txtPasswordAdd.Text), txtLocation.Text, Convert.ToDecimal(txtAccountBalance.Text), Convert.ToInt32(cmbUserType.SelectedValue), CreatedbyID, Crypto.Encrypt(txtAgentrate.Text), 2000, 25000, true, 2000, 25000, 2000, 25000, 2000, 25000, 2000, 500000, 2000, 500000, 2000, 500000, 2000, 25000, 2000, 25000, 500000, 2000, 500000, 2000, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                      
                        if (Convert.ToInt32(cmbUserType.SelectedValue) == 2 || Convert.ToInt32(cmbUserType.SelectedValue) == 8)
                        {
                            try
                            {
                                List<AllUserMarkets> lstUserMarket = JsonConvert.DeserializeObject<List<AllUserMarkets>>(objUsersServiceCleint.GetAllUserMarketbyUserID(73));
                                if (lstUserMarket.Count > 0)
                                {
                                    List<string> allusersmarket = new List<string>();
                                    foreach (var usermarket in lstUserMarket)
                                    {
                                        var orignalopendate = Convert.ToDateTime(usermarket.EventOpenDate).ToString("s");
                                        var objusermarket = usermarket.EventTypeID.ToString() + "#" + usermarket.EventTypeName.ToString() + "#" + usermarket.CompetitionID.ToString() + "#" + usermarket.CompetitionName.ToString() + "#" + usermarket.EventID.ToString() + "#" + usermarket.EventName.ToString() + "#" + usermarket.MarketCatalogueID.ToString() + "#" + usermarket.MarketCatalogueName + "#NotInsert#" + orignalopendate + "#" + usermarket.AssociateEventID + "#" + usermarket.GetMatchUpdatesFrom + "#" + usermarket.TotalOvers + "#" + usermarket.CountryCode;
                                        allusersmarket.Add(objusermarket);
                                    }
                                    objUsersServiceCleint.InsertUserMarket(allusersmarket.ToArray(), Convert.ToInt32(userid), CreatedbyID, false);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            string useridHawala = objUsersServiceCleint.AddUser("Hawala", txtPhone.Text, txtEmail.Text, Crypto.Encrypt("Hawala" + txtUsername.Text.ToLower()), Crypto.Encrypt(txtPasswordAdd.Text), txtLocation.Text, Convert.ToDecimal(txtAccountBalance.Text), Convert.ToInt32(7), CreatedbyID, Crypto.Encrypt(txtAgentrate.Text), 2000, 25000, true, 2000, 25000, 2000, 25000, 2000, 25000, 2000, 500000, 2000, 500000, 2000, 500000, 2000, 25000, 2000, 25000, 500000, 2000, 500000, 2000, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                            objUsersServiceCleint.UpdateHawalaIDbyUserID(Convert.ToInt32(useridHawala), Convert.ToInt32(userid));
                        }
                        objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + txtUsername.Text.ToString() + ")", "0.00", txtAccountBalance.Text.ToString(), CreatedbyID, "", DateTime.Now, "","", "", "", Newaccountbalance, false, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, Convert.ToDecimal(txtAccountBalance.Text), LoggedinUserDetail.PasswordForValidate);

                        LoggedinUserDetail.InsertActivityLog(CreatedbyID, "Created new user (" + txtUsername.Text.ToString() + ")");
                        Xceed.Wpf.Toolkit.MessageBox.Show("Added successfully");
                        GetusersbyUserTypeReload();
                        GetUsersbyUsersType();
                        ClearAllTextFields();
                    }
                    else
                    { 
                    if (LoggedinUserDetail.GetUserTypeID() == 8)
                    {
                        CreatedbyID = LoggedinUserDetail.GetUserID();
                        int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(CreatedbyID);

                        AccountBalance = objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, LoggedinUserDetail.PasswordForValidate);
                        Newaccountbalance = Convert.ToDecimal(AccountBalance);
                        if (Convert.ToInt32(txtAgentrate.Text) > LoggedinUserDetail.MaxAgentRateLimit)
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Agent Rate cannot greater than " + LoggedinUserDetail.MaxAgentRateLimit.ToString() + " %.");
                            return;
                        }

                        if (Convert.ToInt32(txtAccountBalance.Text) > LoggedinUserDetail.MaxBalanceTransferLimit && LoggedinUserDetail.GetUserID() != 73)
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Account Balance should be not be greater than " + LoggedinUserDetail.MaxBalanceTransferLimit.ToString());
                            return;
                        }
                        if (Newaccountbalance < Convert.ToInt32(txtAccountBalance.Text))
                        {
                            Xceed.Wpf.Toolkit.MessageBox.Show("Account Balance is more than available balance");
                            return;
                        }
                        string userid = "0";
                        if (Convert.ToInt32(cmbUserType.SelectedValue) == 4)
                        {
                            userid = objUsersServiceCleint.AddUser(txtName.Text, txtPhone.Text, txtEmail.Text, Crypto.Encrypt(txtUsername.Text.ToLower()), Crypto.Encrypt(txtPasswordAdd.Text), txtLocation.Text, Convert.ToDecimal(txtAccountBalance.Text), Convert.ToInt32(cmbUserType.SelectedValue), 73, Crypto.Encrypt(txtAgentrate.Text), 2000, 25000, true, 2000, 25000, 2000, 25000, 2000, 25000, 2000, 500000, 2000, 500000, 2000, 500000, 2000, 25000, 2000, 25000, 500000, 2000, 500000, 2000, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                        }
                        else
                        {
                            userid = objUsersServiceCleint.AddUser(txtName.Text, txtPhone.Text, txtEmail.Text, Crypto.Encrypt(txtUsername.Text.ToLower()), Crypto.Encrypt(txtPasswordAdd.Text), txtLocation.Text, Convert.ToDecimal(txtAccountBalance.Text), Convert.ToInt32(cmbUserType.SelectedValue), CreatedbyID, Crypto.Encrypt(txtAgentrate.Text), 2000, 25000, true, 2000, 25000, 2000, 25000, 2000, 25000, 2000, 500000, 2000, 500000, 2000, 500000, 2000, 25000, 2000, 25000, 500000, 2000, 500000, 2000, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                        }
                        if (Convert.ToInt32(cmbUserType.SelectedValue) == 2)
                        {
                            try
                            {
                                List<AllUserMarkets> lstUserMarket = JsonConvert.DeserializeObject<List<AllUserMarkets>>(objUsersServiceCleint.GetAllUserMarketbyUserID(73));
                                if (lstUserMarket.Count > 0)
                                {
                                    List<string> allusersmarket = new List<string>();
                                    foreach (var usermarket in lstUserMarket)
                                    {
                                        var orignalopendate = Convert.ToDateTime(usermarket.EventOpenDate).ToString("s");
                                        var objusermarket = usermarket.EventTypeID.ToString() + "#" + usermarket.EventTypeName.ToString() + "#" + usermarket.CompetitionID.ToString() + "#" + usermarket.CompetitionName.ToString() + "#" + usermarket.EventID.ToString() + "#" + usermarket.EventName.ToString() + "#" + usermarket.MarketCatalogueID.ToString() + "#" + usermarket.MarketCatalogueName + "#NotInsert#" + orignalopendate + "#" + usermarket.AssociateEventID + "#" + usermarket.GetMatchUpdatesFrom + "#" + usermarket.TotalOvers + "#" + usermarket.CountryCode;
                                        allusersmarket.Add(objusermarket);
                                    }
                                    objUsersServiceCleint.InsertUserMarket(allusersmarket.ToArray(), Convert.ToInt32(userid), CreatedbyID, false);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            string useridHawala = objUsersServiceCleint.AddUser("Hawala", txtPhone.Text, txtEmail.Text, Crypto.Encrypt("Hawala" + txtUsername.Text.ToLower()), Crypto.Encrypt(txtPasswordAdd.Text), txtLocation.Text, Convert.ToDecimal(txtAccountBalance.Text), Convert.ToInt32(7), CreatedbyID, Crypto.Encrypt(txtAgentrate.Text), 2000, 25000, true, 2000, 25000, 2000, 25000, 2000, 25000, 2000, 500000, 2000, 500000, 2000, 500000, 2000, 25000, 2000, 25000, 500000, 2000, 500000, 2000, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                            objUsersServiceCleint.UpdateHawalaIDbyUserID(Convert.ToInt32(useridHawala), Convert.ToInt32(userid));
                                int AhmadRate = objUsersServiceCleint.GetAhmadRate(LoggedinUserDetail.GetUserID());
                                objUsersServiceCleint.UpdateAhmadRate(Convert.ToInt32(userid), AhmadRate);
                                objUsersServiceCleint.UpdateMaxAgentRate(Convert.ToInt32(userid), Convert.ToInt32(txtAgentrate.Text));
                               // objUsersServiceCleint.UpdateSuperRate(Convert.ToInt32(userid), Convert.ToInt32(LoggedinUserDetail.AgentRate));
                            }
                        objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + txtUsername.Text.ToString() + ")", "0.00", txtAccountBalance.Text.ToString(), CreatedbyID, "", DateTime.Now, "","", "", "", Newaccountbalance, false, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, Convert.ToDecimal(txtAccountBalance.Text), LoggedinUserDetail.PasswordForValidate);

                        LoggedinUserDetail.InsertActivityLog(CreatedbyID, "Created new user (" + txtUsername.Text.ToString() + ")");
                        Xceed.Wpf.Toolkit.MessageBox.Show("Added successfully");
                        GetusersbyUserTypeReload();
                        GetUsersbyUsersType();
                        ClearAllTextFields();
                    }

                    else
                    {
                        string userid = "0";
                        if (Convert.ToInt32(cmbUserType.SelectedValue) == 4)
                        {
                            userid = objUsersServiceCleint.AddUser(txtName.Text, txtPhone.Text, txtEmail.Text, Crypto.Encrypt(txtUsername.Text.ToLower()), Crypto.Encrypt(txtPasswordAdd.Text), txtLocation.Text, Convert.ToDecimal(txtAccountBalance.Text), Convert.ToInt32(cmbUserType.SelectedValue), 73, Crypto.Encrypt(txtAgentrate.Text), 2000, 25000, true, 2000, 25000, 2000, 25000, 2000, 25000, 2000, 500000, 2000, 500000, 2000, 500000, 2000, 25000, 2000, 25000, 500000, 2000, 500000, 2000, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                        }
                        else
                        {
                            userid = objUsersServiceCleint.AddUser(txtName.Text, txtPhone.Text, txtEmail.Text, Crypto.Encrypt(txtUsername.Text.ToLower()), Crypto.Encrypt(txtPasswordAdd.Text), txtLocation.Text, Convert.ToDecimal(txtAccountBalance.Text), Convert.ToInt32(cmbUserType.SelectedValue), CreatedbyID, Crypto.Encrypt(txtAgentrate.Text), 2000, 25000, true, 2000, 25000, 2000, 25000, 2000, 25000, 2000, 500000, 2000, 500000, 2000, 500000, 2000, 25000, 2000, 25000, 500000, 2000, 500000, 2000, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                        }

                        if (Convert.ToInt32(cmbUserType.SelectedValue) == 2)
                        {
                            try
                            {
                                List<AllUserMarkets> lstUserMarket = JsonConvert.DeserializeObject<List<AllUserMarkets>>(objUsersServiceCleint.GetAllUserMarketbyUserID(73));
                                if (lstUserMarket.Count > 0)
                                {
                                    List<string> allusersmarket = new List<string>();
                                    foreach (var usermarket in lstUserMarket)
                                    {
                                        //  var datearr = usermarket.EventOpenDate.ToString().Split(' ');
                                        //  var datearr2 = datearr[0].Split('/');
                                        //  var orignalopendate = datearr2[1] + "/" + datearr2[0] + "/" + datearr2[2] + " " + datearr[1];
                                        var orignalopendate = Convert.ToDateTime(usermarket.EventOpenDate).ToString("s");
                                        var objusermarket = usermarket.EventTypeID.ToString() + "#" + usermarket.EventTypeName.ToString() + "#" + usermarket.CompetitionID.ToString() + "#" + usermarket.CompetitionName.ToString() + "#" + usermarket.EventID.ToString() + "#" + usermarket.EventName.ToString() + "#" + usermarket.MarketCatalogueID.ToString() + "#" + usermarket.MarketCatalogueName + "#NotInsert#" + orignalopendate + "#" + usermarket.AssociateEventID + "#" + usermarket.GetMatchUpdatesFrom + "#" + usermarket.TotalOvers + "#" + usermarket.CountryCode;
                                        allusersmarket.Add(objusermarket);
                                    }
                                    objUsersServiceCleint.InsertUserMarket(allusersmarket.ToArray(), Convert.ToInt32(userid), CreatedbyID, false);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            string useridHawala = objUsersServiceCleint.AddUser("Hawala", txtPhone.Text, txtEmail.Text, Crypto.Encrypt("Hawala" + txtUsername.Text.ToLower()), Crypto.Encrypt(txtPasswordAdd.Text), txtLocation.Text, Convert.ToDecimal(txtAccountBalance.Text), Convert.ToInt32(7), CreatedbyID, Crypto.Encrypt(txtAgentrate.Text), 2000, 25000, true, 2000, 25000, 2000, 25000, 2000, 25000, 2000, 500000, 2000, 500000, 2000, 500000, 2000, 25000, 2000, 25000, 500000, 2000, 500000, 2000, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                            objUsersServiceCleint.UpdateHawalaIDbyUserID(Convert.ToInt32(useridHawala), Convert.ToInt32(userid));
                                objUsersServiceCleint.UpdateMaxAgentRate(Convert.ToInt32(userid), Convert.ToInt32(txtAgentrate.Text));
                                objUsersServiceCleint.UpdateSuperRate(Convert.ToInt32(userid), Convert.ToInt32(txtAgentrate.Text));
                            }
                        else
                        {
                            if (Convert.ToInt32(cmbUserType.SelectedValue) == 8)
                            {
                                try
                                {
                                    List<AllUserMarkets> lstUserMarket = JsonConvert.DeserializeObject<List<AllUserMarkets>>(objUsersServiceCleint.GetAllUserMarketbyUserID(73));
                                    if (lstUserMarket.Count > 0)
                                    {
                                        List<string> allusersmarket = new List<string>();
                                        foreach (var usermarket in lstUserMarket)
                                        {
                                            //  var datearr = usermarket.EventOpenDate.ToString().Split(' ');
                                            //  var datearr2 = datearr[0].Split('/');
                                            //  var orignalopendate = datearr2[1] + "/" + datearr2[0] + "/" + datearr2[2] + " " + datearr[1];
                                            var orignalopendate = Convert.ToDateTime(usermarket.EventOpenDate).ToString("s");
                                            var objusermarket = usermarket.EventTypeID.ToString() + "#" + usermarket.EventTypeName.ToString() + "#" + usermarket.CompetitionID.ToString() + "#" + usermarket.CompetitionName.ToString() + "#" + usermarket.EventID.ToString() + "#" + usermarket.EventName.ToString() + "#" + usermarket.MarketCatalogueID.ToString() + "#" + usermarket.MarketCatalogueName + "#NotInsert#" + orignalopendate + "#" + usermarket.AssociateEventID + "#" + usermarket.GetMatchUpdatesFrom + "#" + usermarket.TotalOvers + "#" + usermarket.CountryCode;
                                            allusersmarket.Add(objusermarket);
                                        }
                                        objUsersServiceCleint.InsertUserMarket(allusersmarket.ToArray(), Convert.ToInt32(userid), CreatedbyID, false);
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                                string useridHawala = objUsersServiceCleint.AddUser("Hawala", txtPhone.Text, txtEmail.Text, Crypto.Encrypt("Hawala" + txtUsername.Text.ToLower()), Crypto.Encrypt(txtPasswordAdd.Text), txtLocation.Text, Convert.ToDecimal(txtAccountBalance.Text), Convert.ToInt32(7), CreatedbyID, Crypto.Encrypt(txtAgentrate.Text), 2000, 25000, true, 2000, 25000, 2000, 25000, 2000, 25000, 2000, 500000, 2000, 500000, 2000, 500000, 2000, 25000, 2000, 25000, 500000, 2000, 500000, 2000, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                                objUsersServiceCleint.UpdateHawalaIDbyUserID(Convert.ToInt32(useridHawala), Convert.ToInt32(userid));
                                //  objUsersServiceCleint.UpdateStartBalancebyUserID(Convert.ToInt32(useridHawala), Convert.ToDecimal(txtAccountBalance.Text));
                            }
                        }

                        objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + txtUsername.Text.ToString() + ")", "0.00", txtAccountBalance.Text.ToString(), CreatedbyID, "", DateTime.Now, "", "","", "", Newaccountbalance, false, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(CreatedbyID, Convert.ToDecimal(txtAccountBalance.Text), LoggedinUserDetail.PasswordForValidate);

                        LoggedinUserDetail.InsertActivityLog(CreatedbyID, "Created new user (" + txtUsername.Text.ToString() + ")");
                        Xceed.Wpf.Toolkit.MessageBox.Show("Added successfully");
                        GetusersbyUserTypeReload();
                        GetUsersbyUsersType();
                        ClearAllTextFields();
                        // return "True" + "|" + userid.ToString();
                    }
                }
            }
                
            }
            else
            {
                MessageBox.Show("Username already exists.");
            }
        }
       
        public UserDetails getuserdetails(int UserID)
        {
            LoggedinUserDetail.CheckifUserLogin();
            var results = JsonConvert.DeserializeObject<UserDetails>(objUsersServiceCleint.GetUserDetailsbyID(UserID, LoggedinUserDetail.PasswordForValidate));
            UserDetails objuserDetails = new UserDetails();
            objuserDetails = results;
            objuserDetails.Username = Crypto.Decrypt(objuserDetails.Username);
            objuserDetails.Password = Crypto.Decrypt(objuserDetails.Password);
            objuserDetails.RatePercent = Crypto.Decrypt(objuserDetails.RatePercent);

            if (LoggedinUserDetail.GetUserTypeID() != 1)
            {
                objuserDetails.Password = "";
            }
            if (objuserDetails.isBlocked == true)
            {
                objuserDetails.Status = "Blocked";
            }
            else
            {
                if (objuserDetails.isDeleted == true)
                {
                    objuserDetails.Status = "Deleted";
                }
                else
                {
                    objuserDetails.Status = "Active";
                }
            }
            return objuserDetails;



        }
        public void CustomerDetailsLoad()
        {
            try
            {
                ClearAllLabelFields();
                if (cmbUsers.SelectedValue is int)
                {
                    UserDetails objuserDetails = getuserdetails(Convert.ToInt32(cmbUsers.SelectedValue));
                    lblName.Content = objuserDetails.Name;
                    lblUserName.Content = objuserDetails.Username;
                    lblPhone.Content = objuserDetails.PhoneNumber;
                    lblAccountBalance.Content = objuserDetails.AccountBalance.ToString();
                    lblLastLoginTime.Content = objuserDetails.LastLoginTime;
                    lblStatus.Content = objuserDetails.Status;
                    lblProfitandLoss.Content = objuserDetails.ProfitandLoss;
                    lblLastAmoundAdd.Content = objuserDetails.LastAmountAdded;
                    lblLastAmountRemoved.Content = objuserDetails.LastAmountRemoved;
                    lblAgentRate.Content = objuserDetails.RatePercent;
                    lblcommissionRate.Content = objuserDetails.CommissionRate.ToString();
                    lblLastLocation.Content = objuserDetails.LastLocation;
                    lblIpAddress.Content = objuserDetails.LastIPAddress;
                    lblCreatedBy.Content = objuserDetails.CreatedBy;
                    lblCreatedDateDetails.Content = objuserDetails.CreatedDate;
                    txtAgentRateUpate.Text = objuserDetails.RatePercent;
                    chkBlockUser.IsChecked = objuserDetails.isBlocked;

                    chkGrayHoundRacing.IsChecked = objuserDetails.isGrayHoundRaceAllowed;
                    chkHorseRacing.IsChecked = objuserDetails.isHorseRaceAllowed;
                    chkLoggedIn.IsChecked = objuserDetails.Loggedin;
                    chkSoccer.IsChecked = (bool)objuserDetails.isSoccerAllowed;
                    chkTennis.IsChecked = (bool)objuserDetails.isTennisAllowed;
                    chkBlockUserBMS.IsChecked = objuserDetails.isBMSBlocked;
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        txtPasswordUpdate.Text = objuserDetails.Password;
                        try
                        {
                            UserIDandUserType objSelectedUser = (UserIDandUserType)cmbUsers.SelectedItem;
                            if (objSelectedUser.UserTypeID == 2 || objSelectedUser.UserTypeID == 9 || objSelectedUser.UserTypeID == 8)
                            {
                                txtblocktransferagentcommision.Visibility = Visibility.Visible;
                                chkTransferAgentCommision.IsChecked = (bool)objUsersServiceCleint.GetTransferAgnetCommision(Convert.ToInt32(cmbUsers.SelectedValue));
                                try
                                {
                                    txtblockMaxBalanceTransferLimit.Visibility = Visibility.Visible;
                                    txtMaxBalanceTransferLimit.Text = objUsersServiceCleint.GetMaxBalanceTransferLimit(Convert.ToInt32(cmbUsers.SelectedValue)).ToString();
                                    txtblockMaxAgentRate.Visibility = Visibility.Visible;
                                    txtMaxAgentRate.Text = objUsersServiceCleint.GetMaxAgentRate(Convert.ToInt32(cmbUsers.SelectedValue)).ToString();
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else
                            {
                                txtblocktransferagentcommision.Visibility = Visibility.Collapsed;
                                chkTransferAgentCommision.IsChecked = false;
                                try
                                {
                                    txtblockMaxBalanceTransferLimit.Visibility = Visibility.Collapsed;
                                    txtMaxBalanceTransferLimit.Text = "0";
                                    txtblockMaxAgentRate.Visibility = Visibility.Collapsed;
                                    txtMaxAgentRate.Text = "0";
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {


                                }
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void cmbUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomerDetailsLoad();
        }
        List<GetMarketForAllowedBetting_Result> lstMarketsAllowedforBet = new List<GetMarketForAllowedBetting_Result>();
        public void CustomerAllowedMarketsLoad()
        {
            chkAllowdHorseWin.IsChecked = false;
            chkAllowdSoccer.IsChecked = false;
            chkAllowedCricketOdds.IsChecked = false;
            chkAllowedInningsRuns.IsChecked = false;
            chkAllowedTenis.IsChecked = false;
            chkCompletedMatchAllowed.IsChecked = false;
            chkGrayHoundAllowedWin.IsChecked = false;
            chkGrayHoundPlaceAllowed.IsChecked = false;
            chkHorseRacePlaceAllowed.IsChecked = false;
            chkTiedMatchAllowed.IsChecked = false;
            chkCheckforMaxOddBack.IsChecked = false;
            chkCheckForMaxOddLay.IsChecked = false;
            chkWinnerForAllowedBet.IsChecked = false;
            chkFancyForAllowedBet.IsChecked = false;
            txtMaxOddBack.Text = "0";
            txtMaxOddLay.Text = "0";
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                if (cmbCustomerAllowedMarkets.SelectedValue is int)
                {
                    AllowedMarkets objAllowedMarkets = JsonConvert.DeserializeObject<AllowedMarkets>(objUsersServiceCleint.GetAllowedMarketsbyUserID(Convert.ToInt32(cmbCustomerAllowedMarkets.SelectedValue)));
                    chkAllowdHorseWin.IsChecked = objAllowedMarkets.isHorseRaceWinAllowedForBet;
                    chkAllowdSoccer.IsChecked = objAllowedMarkets.isSoccerAllowedForBet;
                    chkAllowedCricketOdds.IsChecked = objAllowedMarkets.isCricketMatchOddsAllowedForBet;
                    chkAllowedInningsRuns.IsChecked = objAllowedMarkets.isCricketInningsRunsAllowedForBet;
                    chkAllowedTenis.IsChecked = objAllowedMarkets.isTennisAllowedForBet;
                    chkCompletedMatchAllowed.IsChecked = objAllowedMarkets.isCricketCompletedMatchAllowedForBet;
                    chkGrayHoundAllowedWin.IsChecked = objAllowedMarkets.isGrayHoundRaceWinAllowedForBet;
                    chkGrayHoundPlaceAllowed.IsChecked = objAllowedMarkets.isGrayHoundRacePlaceAllowedForBet;
                    chkHorseRacePlaceAllowed.IsChecked = objAllowedMarkets.isHorseRacePlaceAllowedForBet;
                    chkTiedMatchAllowed.IsChecked = objAllowedMarkets.isCricketTiedMatchAllowedForBet;
                    chkWinnerForAllowedBet.IsChecked = objAllowedMarkets.isWinnerMarketAllowedForBet;
                    chkFancyForAllowedBet.IsChecked = objAllowedMarkets.isFancyMarketAllowed;
                    SP_Users_GetMaxOddBackandLay_Result objMaxOddBackandLay = objUsersServiceCleint.GetMaxOddBackandLay(Convert.ToInt32(cmbCustomerAllowedMarkets.SelectedValue));
                    chkCheckforMaxOddBack.IsChecked = Convert.ToBoolean(objMaxOddBackandLay.CheckforMaxOddBack);
                    chkCheckForMaxOddLay.IsChecked = Convert.ToBoolean(objMaxOddBackandLay.CheckforMaxOddLay);
                    txtMaxOddBack.Text = objMaxOddBackandLay.MaxOddBack.ToString();
                    txtMaxOddLay.Text = objMaxOddBackandLay.MaxOddLay.ToString();
                    lstMarketsAllowedforBet = JsonConvert.DeserializeObject<List<GetMarketForAllowedBetting_Result>>(objUsersServiceCleint.GetMarketsforBettingAllowed(Convert.ToInt32(cmbCustomerAllowedMarkets.SelectedValue))).ToList();
                    if (lstMarketsAllowedforBet.Count > 0)
                    {
                        dgvAllowedMarkets.AutoGenerateColumns = false;
                        dgvAllowedMarkets.ItemsSource = lstMarketsAllowedforBet;
                    }
                }
            }
        }
        private void btnUpdateUser_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() != 3)
            {
                UserIDandUserType objSelectedUser = (UserIDandUserType)cmbUsers.SelectedItem;
                if (Convert.ToInt32(cmbUsers.SelectedValue) > 0)
                {
                    int UpdatedBy = LoggedinUserDetail.GetUserID();
                    if (UpdatedBy > 0)
                    {
                        if (txtAgentRateUpate.Text == "")
                        {
                            txtAgentRateUpate.Text = "0";

                        }
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            if (Convert.ToInt32(txtAgentRateUpate.Text) > LoggedinUserDetail.MaxAgentRateLimit)
                            {
                                Xceed.Wpf.Toolkit.MessageBox.Show("Agent Rate cannot greater than " + LoggedinUserDetail.MaxAgentRateLimit.ToString() + " %.");
                                return;
                            }
                        }
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            try
                            {
                                objUsersServiceCleint.SetBlockedStatusofUserBMS(Convert.ToInt32(cmbUsers.SelectedValue), chkBlockUserBMS.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        txtAgentRateUpate.Text = Crypto.Encrypt(txtAgentRateUpate.Text);
                        DateTime updatedtime = DateTime.Now;

                        if(LoggedinUserDetail.GetUserTypeID() == 9 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            if(objSelectedUser.UserTypeID==2)
                            {
                                var results = objUsersServiceCleint.GetAllUsersbyUserTypeNew(objSelectedUser.ID, Convert.ToInt32(objSelectedUser.UserTypeID), LoggedinUserDetail.PasswordForValidate);
                                if (results != "")
                                {
                                    List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);

                                    foreach (UserIDandUserType objuser in lstUsers)
                                    {

                                        objUsersServiceCleint.SetBlockedStatusofUser(Convert.ToInt32(objuser.ID), true, LoggedinUserDetail.PasswordForValidate);
                                    }
                                    //LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Block Status");
                                }
                            }
                            if (objSelectedUser.UserTypeID == 8)
                            {
                                var results = objUsersServiceCleint.GetAllUsersbyUserTypeNew(objSelectedUser.ID, Convert.ToInt32(objSelectedUser.UserTypeID), LoggedinUserDetail.PasswordForValidate);
                                if (results != "")
                                {
                                    List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);

                                    foreach (UserIDandUserType objuser in lstUsers)
                                    {

                                        objUsersServiceCleint.SetBlockedStatusofUser(Convert.ToInt32(objuser.ID), true, LoggedinUserDetail.PasswordForValidate);
                                    }
                                    //LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Block Status");
                                }
                            }
                            if (objSelectedUser.UserTypeID == 9)
                            {
                                var results = objUsersServiceCleint.GetAllUsersbyUserTypeNew(objSelectedUser.ID, Convert.ToInt32(objSelectedUser.UserTypeID), LoggedinUserDetail.PasswordForValidate);
                                if (results != "")
                                {
                                    List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);

                                    foreach (UserIDandUserType objuser in lstUsers)
                                    {

                                        objUsersServiceCleint.SetBlockedStatusofUser(Convert.ToInt32(objuser.ID), true, LoggedinUserDetail.PasswordForValidate);
                                    }
                                    //LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Block Status");
                                }
                            }
                        }
                        objUsersServiceCleint.SetBlockedStatusofUser(Convert.ToInt32(cmbUsers.SelectedValue), chkBlockUser.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                        LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Block Status");
                        //objUsersServiceCleint.SetDeleteStatusofUser(Convert.ToInt32(cmbUsers.SelectedValue), chk);
                        //LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Delete Status");
                        if (txtPasswordUpdate.Text.Length >= 6)
                        {
                            objUsersServiceCleint.ResetPasswordofUser(Convert.ToInt32(cmbUsers.SelectedValue), Crypto.Encrypt(txtPasswordUpdate.Text), UpdatedBy, updatedtime, LoggedinUserDetail.PasswordForValidate);
                            LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Password");
                        }
                        objUsersServiceCleint.SetAgentRateofUser(Convert.ToInt32(cmbUsers.SelectedValue), txtAgentRateUpate.Text, LoggedinUserDetail.PasswordForValidate);
                        LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Set Rate of User " + Convert.ToInt32(cmbUsers.SelectedValue).ToString() + " " + Crypto.Decrypt(txtAgentRateUpate.Text).ToString());
                        objUsersServiceCleint.SetLoggedinStatus(Convert.ToInt32(cmbUsers.SelectedValue), chkLoggedIn.IsChecked.Value);
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            //  objUsersServiceCleint.UpdateMarketsForView(Convert.ToInt32(cmbUsers.SelectedValue), chkGrayHoundForView, BetUpperLimit, isAllowedGrayHound, isAllowedHorse, BetLowerLimitHorsePlace, BetUpperLimitHorsePlace, BetLowerLimitGrayHoundWin, BetUpperLimitGrayHoundWin, BetLowerLimitGrayHoundPlace, BetUpperLimitGrayHoundPlace, BetLowerLimitMatchOdds, BetUpperLimitMatchOdds, BetLowerLimitInningsRunns, BetUpperLimitInningsRunns, BetLowerLimitCompletedMatch, BetUpperLimitCompletedMatch, isTennisAllowed, isSoccerAllowed, CommissionRate);
                        }
                        txtAgentRateUpate.Text = Crypto.Decrypt(txtAgentRateUpate.Text);
                        Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
                        CustomerDetailsLoad();

                    }
                    else
                    { }
                }

            }
        }
        public void CreditUsersLoad()
        {
            try
            {
                if (cmbUsersCredit.SelectedValue is int)
                {
                    UserDetails objuserdetails = getuserdetails(Convert.ToInt32(cmbUsersCredit.SelectedValue));
                    lblAccountBalanceAddRemove.Content = objuserdetails.AccountBalance.ToString("N0");
                    lblCredit.Content = Convert.ToInt64(objUsersServiceCleint.GetStartingBalance(Convert.ToInt32(cmbUsersCredit.SelectedValue), LoggedinUserDetail.PasswordForValidate)).ToString("N0");

                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void cmbUsersCredit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CreditUsersLoad();
        }

        private void btnAddCredit_Click(object sender, RoutedEventArgs e)
        {

            if (txtAccountsTitle.Text.Length == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Enter Title");
                return;
            }
            if (txtBalanceAdd.Text.Length == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Enter Balance");
                return;
            }
            btnAddCredit.IsEnabled = false;
            if (LoggedinUserDetail.GetUserTypeID() != 3 && (Convert.ToInt32(cmbUsersCredit.SelectedValue) > 0))
            {
                LoggedinUserDetail.CheckifUserLogin();

                // if (LoggedinUserDetail.GetUserTypeID()==2 && AddedbyID != 73)
                // {
                //  HawalaID   = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                // }
                //else
                // {
                //     if (LoggedinUserDetail.GetUserTypeID() == 1)
                //     {
                //         HawalaID= objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                //     }
                // }

                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    int UserID = Convert.ToInt32(cmbUsersCredit.SelectedValue);


                    int AddedbyID = LoggedinUserDetail.GetUserID();
                    int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                    Decimal Newaccountbalance = Convert.ToDecimal(txtBalanceAdd.Text);
                    Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, LoggedinUserDetail.PasswordForValidate));
                    decimal UserCurrentBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserID, LoggedinUserDetail.PasswordForValidate));

                    if (chkIsCredit.IsChecked == true)
                    {
                        decimal UserStartBalance = Convert.ToDecimal(objUsersServiceCleint.GetStartingBalance(UserID, LoggedinUserDetail.PasswordForValidate));

                        if ((Newaccountbalance + UserStartBalance) > LoggedinUserDetail.MaxBalanceTransferLimit && LoggedinUserDetail.GetUserID() != 73)
                        {
                            btnAddCredit.IsEnabled = true;
                            Xceed.Wpf.Toolkit.MessageBox.Show("You are not allowed to add more than " + LoggedinUserDetail.MaxBalanceTransferLimit.ToString() + " to credit limit.");
                            return;
                        }
                    }
                    if ((UserCurrentBalance + Newaccountbalance) > LoggedinUserDetail.MaxBalanceTransferLimit && LoggedinUserDetail.GetUserID() != 73)
                    {
                        btnAddCredit.IsEnabled = true;
                        Xceed.Wpf.Toolkit.MessageBox.Show("You are not allowed to add more than " + LoggedinUserDetail.MaxBalanceTransferLimit.ToString());
                        return;
                    }
                    if (Newaccountbalance > CurrentAccountBalance)
                    {
                        btnAddCredit.IsEnabled = true;
                        Xceed.Wpf.Toolkit.MessageBox.Show("Account Balance is more than available balance.");
                        return;
                    }
                    else
                    {
                        objUsersServiceCleint.AddCredittoUser(Newaccountbalance, UserID, AddedbyID, DateTime.Now, 0, true, txtAccountsTitle.Text, chkIsCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                        if (chkIsCredit.IsChecked == true)
                        {

                            objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            // objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID, Newaccountbalance);
                        }


                        objAccountsService.AddtoUsersAccounts("Amount removed from your account ( " + txtAccountsTitle.Text + " for " + cmbUsersCredit.Text + " )", "0.00", Newaccountbalance.ToString(), HawalaID, "", DateTime.Now, "", "", "","", CurrentAccountBalance, chkIsCredit.IsChecked.Value, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);

                        LoggedinUserDetail.InsertActivityLog(HawalaID, "Added Balance " + Newaccountbalance.ToString() + "  to user ( " + cmbUsersCredit.Text + " )");
                        btnAddCredit.IsEnabled = true;
                        Xceed.Wpf.Toolkit.MessageBox.Show("Successfully Added.");
                        CreditUsersLoad();
                        txtAccountsTitle.Text = "";
                        txtBalanceAdd.Text = "";
                    }


                }
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {
                    LoggedinUserDetail.GetUserID();

                    int UserID = Convert.ToInt32(cmbUsersCredit.SelectedValue);
                    int AddedbyID = LoggedinUserDetail.GetUserID();
                    int HawalaIDSuper = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                    int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                    Decimal Newaccountbalance = Convert.ToDecimal(txtBalanceAdd.Text);
                    Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(AddedbyID, LoggedinUserDetail.PasswordForValidate));
                    objUsersServiceCleint.AddCredittoUser(Newaccountbalance, UserID, AddedbyID, DateTime.Now, 0, true, txtAccountsTitle.Text, chkIsCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                    if (HawalaID > 0)
                    {
                        objUsersServiceCleint.AddCredittoUser(Newaccountbalance, HawalaID, AddedbyID, DateTime.Now, 0, true, txtAccountsTitle.Text, chkIsCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                    }
                    if (chkIsCredit.IsChecked == true)
                    {

                        objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                        if (HawalaID > 0)
                        {
                            objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                        }
                        //  objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID, Newaccountbalance);
                    }
                    objAccountsService.AddtoUsersAccounts("Amount removed from your account( " + txtAccountsTitle.Text + " for " + cmbUsersCredit.Text + "(UserID=" + UserID.ToString() + ") )", "0.00", Newaccountbalance.ToString(), AddedbyID, "", DateTime.Now, "","", "", "", CurrentAccountBalance, chkIsCredit.IsChecked.Value, "", "", "", "", "");
                    objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaIDSuper, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                    LoggedinUserDetail.InsertActivityLog(AddedbyID, "Added Balance " + Newaccountbalance.ToString() + " to user ( " + cmbUsersCredit.Text + " )");
                    btnAddCredit.IsEnabled = true;
                    Xceed.Wpf.Toolkit.MessageBox.Show("Successfully Added.");
                    CreditUsersLoad();
                    txtAccountsTitle.Text = "";
                    txtBalanceAdd.Text = "";
                }
                if (LoggedinUserDetail.GetUserTypeID() == 9)
                {
                    LoggedinUserDetail.GetUserID();

                    int UserID = Convert.ToInt32(cmbUsersCredit.SelectedValue);
                    int AddedbyID = LoggedinUserDetail.GetUserID();
                    int HawalaIDSuper = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                    int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                    Decimal Newaccountbalance = Convert.ToDecimal(txtBalanceAdd.Text);
                    Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(AddedbyID, LoggedinUserDetail.PasswordForValidate));
                    objUsersServiceCleint.AddCredittoUser(Newaccountbalance, UserID, AddedbyID, DateTime.Now, 0, true, txtAccountsTitle.Text, chkIsCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                    if (HawalaID > 0)
                    {
                        objUsersServiceCleint.AddCredittoUser(Newaccountbalance, HawalaID, AddedbyID, DateTime.Now, 0, true, txtAccountsTitle.Text, chkIsCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                    }
                    if (chkIsCredit.IsChecked == true)
                    {

                        objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                        if (HawalaID > 0)
                        {
                            objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                        }
                        //  objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID, Newaccountbalance);
                    }
                    objAccountsService.AddtoUsersAccounts("Amount removed from your account( " + txtAccountsTitle.Text + " for " + cmbUsersCredit.Text + "(UserID=" + UserID.ToString() + ") )", "0.00", Newaccountbalance.ToString(), AddedbyID, "", DateTime.Now, "", "","", "", CurrentAccountBalance, chkIsCredit.IsChecked.Value, "", "", "", "", "");
                    objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaIDSuper, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                    LoggedinUserDetail.InsertActivityLog(AddedbyID, "Added Balance " + Newaccountbalance.ToString() + " to user ( " + cmbUsersCredit.Text + " )");
                    btnAddCredit.IsEnabled = true;
                    Xceed.Wpf.Toolkit.MessageBox.Show("Successfully Added.");
                    CreditUsersLoad();
                    txtAccountsTitle.Text = "";
                    txtBalanceAdd.Text = "";
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        int UserID = Convert.ToInt32(cmbUsersCredit.SelectedValue);
                        int AddedbyID = LoggedinUserDetail.GetUserID();


                        int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                        Decimal Newaccountbalance = Convert.ToDecimal(txtBalanceAdd.Text);
                        Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(AddedbyID, LoggedinUserDetail.PasswordForValidate));
                        objUsersServiceCleint.AddCredittoUser(Newaccountbalance, UserID, AddedbyID, DateTime.Now, 0, true, txtAccountsTitle.Text, chkIsCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                        if (HawalaID > 0)
                        {
                            objUsersServiceCleint.AddCredittoUser(Newaccountbalance, HawalaID, AddedbyID, DateTime.Now, 0, true, txtAccountsTitle.Text, chkIsCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                        }
                        if (chkIsCredit.IsChecked == true)
                        {

                            objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            if (HawalaID > 0)
                            {
                                objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            }
                            //  objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID, Newaccountbalance);
                        }
                        objAccountsService.AddtoUsersAccounts("Amount removed from your account( " + txtAccountsTitle.Text + " for " + cmbUsersCredit.Text + "(UserID=" + UserID.ToString() + ") )", "0.00", Newaccountbalance.ToString(), AddedbyID, "", DateTime.Now, "","","", "", CurrentAccountBalance, chkIsCredit.IsChecked.Value, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(AddedbyID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                        LoggedinUserDetail.InsertActivityLog(AddedbyID, "Added Balance " + Newaccountbalance.ToString() + " to user ( " + cmbUsersCredit.Text + " )");
                        btnAddCredit.IsEnabled = true;
                        Xceed.Wpf.Toolkit.MessageBox.Show("Successfully Added.");
                        CreditUsersLoad();
                        txtAccountsTitle.Text = "";
                        txtBalanceAdd.Text = "";
                    }
                }
            }
        }

        private void btnRemovecredit_Click(object sender, RoutedEventArgs e)
        {
            if (txtAccountsTitle.Text.Length == 0)
            {
                MessageBox.Show("Enter Title");
                return;
            }
            if (txtBalanceAdd.Text.Length == 0)
            {
                MessageBox.Show("Enter Balance");
                return;
            }
            btnRemovecredit.IsEnabled = false;
            LoggedinUserDetail.CheckifUserLogin();
            if (txtBalanceAdd.Text.Length > 0)
            {
                if (LoggedinUserDetail.GetUserTypeID() != 3 && (Convert.ToInt32(cmbUsersCredit.SelectedValue) > 0))
                {
                    int UserID = Convert.ToInt32(cmbUsersCredit.SelectedValue);
                    int AddedbyID = LoggedinUserDetail.GetUserID();

                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                        int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                        Decimal Newaccountbalance = Convert.ToDecimal(txtBalanceAdd.Text);
                        Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, LoggedinUserDetail.PasswordForValidate));
                        Decimal AlreadyBalance = Convert.ToDecimal(lblAccountBalanceAddRemove.Content);
                        if (Newaccountbalance > AlreadyBalance)
                        {
                            btnRemovecredit.IsEnabled = true;
                            Xceed.Wpf.Toolkit.MessageBox.Show("Amount is greater than current balance.");
                            return;
                        }
                        objUsersServiceCleint.AddCredittoUser(0, UserID, AddedbyID, DateTime.Now, Newaccountbalance, true, txtAccountsTitle.Text, chkIsCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);

                        objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + txtAccountsTitle.Text + " for " + cmbUsersCredit.Text + " )", Newaccountbalance.ToString(), "0.00", HawalaID, "", DateTime.Now, "","", "", "", CurrentAccountBalance, chkIsCredit.IsChecked.Value, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                        if (chkIsCredit.IsChecked == true)
                        {

                            objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            // objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID,-1* Newaccountbalance);
                        }
                        LoggedinUserDetail.InsertActivityLog(AddedbyID, "Removed Balance " + Newaccountbalance.ToString() + "  from user ( " + cmbUsersCredit.Text + " )");
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            LoggedinUserDetail.InsertActivityLog(HawalaID, "Added Balance " + Newaccountbalance.ToString() + "  to user ( " + LoggedinUserDetail.GetUserName().ToString() + " )");
                        }
                        btnRemovecredit.IsEnabled = true;
                        Xceed.Wpf.Toolkit.MessageBox.Show("Successfully Removed");
                        CreditUsersLoad();
                        txtAccountsTitle.Text = "";
                        txtBalanceAdd.Text = "";

                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                        {

                            int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                            int HawalaIDSuper = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                            Decimal Newaccountbalance = Convert.ToDecimal(txtBalanceAdd.Text);
                            Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserID, LoggedinUserDetail.PasswordForValidate));
                            //  Decimal CurrentAccountBalanceHawala = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID));
                            Decimal AlreadyBalance = Convert.ToDecimal(lblAccountBalanceAddRemove.Content);
                            if (Newaccountbalance > AlreadyBalance)
                            {
                                btnRemovecredit.IsEnabled = true;
                                Xceed.Wpf.Toolkit.MessageBox.Show("Amount is greater than current balance.");
                                return;
                            }
                            objUsersServiceCleint.AddCredittoUser(0, UserID, AddedbyID, DateTime.Now, Newaccountbalance, true, txtAccountsTitle.Text, chkIsCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                            objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + txtAccountsTitle.Text + " for " + cmbUsersCredit.Text + "(UserID=" + UserID.ToString() + ") )", Newaccountbalance.ToString(), "0.00", AddedbyID, "", DateTime.Now, "","" ,"", "", CurrentAccountBalance, chkIsCredit.IsChecked.Value, "", "", "", "", "");
                            objUsersServiceCleint.UpdateAccountBalacnebyUser(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            if (HawalaID > 0)
                            {
                                objUsersServiceCleint.AddCredittoUser(0, HawalaID, AddedbyID, DateTime.Now, Newaccountbalance, true, txtAccountsTitle.Text, chkIsCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                                // objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + txtAccountsTitle.Text + " for " + cmbUsersCredit.Text + " )", Newaccountbalance.ToString(), "0.00", HawalaID, "", DateTime.Now, "", "", CurrentAccountBalanceHawala, chkIsCredit.Checked, "", "");
                                objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            }
                            if (chkIsCredit.IsChecked == true)
                            {

                                objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                                if (HawalaID > 0)
                                {
                                    objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                                }
                                // objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID,-1* Newaccountbalance);
                            }
                            objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaIDSuper, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            LoggedinUserDetail.InsertActivityLog(AddedbyID, "Removed Balance " + Newaccountbalance.ToString() + "  from user ( " + cmbUsersCredit.Text + " )");
                            btnRemovecredit.IsEnabled = true;
                            MessageBox.Show("Successfully Removed");
                            CreditUsersLoad();
                            txtAccountsTitle.Text = "";
                            txtBalanceAdd.Text = "";
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                            {
                                int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                                Decimal Newaccountbalance = Convert.ToDecimal(txtBalanceAdd.Text);
                                Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserID, LoggedinUserDetail.PasswordForValidate));
                                //  Decimal CurrentAccountBalanceHawala = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID));
                                Decimal AlreadyBalance = Convert.ToDecimal(lblAccountBalanceAddRemove.Content);
                                if (Newaccountbalance > AlreadyBalance)
                                {
                                    btnRemovecredit.IsEnabled = true;
                                    Xceed.Wpf.Toolkit.MessageBox.Show("Amount is greater than current balance.");
                                    return;
                                }
                                objUsersServiceCleint.AddCredittoUser(0, UserID, AddedbyID, DateTime.Now, Newaccountbalance, true, txtAccountsTitle.Text, chkIsCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                                objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + txtAccountsTitle.Text + " for " + cmbUsersCredit.Text + "(UserID=" + UserID.ToString() + ") )", Newaccountbalance.ToString(), "0.00", AddedbyID, "", DateTime.Now, "","", "", "", CurrentAccountBalance, chkIsCredit.IsChecked.Value, "", "", "", "", "");
                                objUsersServiceCleint.UpdateAccountBalacnebyUser(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                                if (HawalaID > 0)
                                {
                                    objUsersServiceCleint.AddCredittoUser(0, HawalaID, AddedbyID, DateTime.Now, Newaccountbalance, true, txtAccountsTitle.Text, chkIsCredit.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                                    // objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + txtAccountsTitle.Text + " for " + cmbUsersCredit.Text + " )", Newaccountbalance.ToString(), "0.00", HawalaID, "", DateTime.Now, "", "", CurrentAccountBalanceHawala, chkIsCredit.Checked, "", "");
                                    objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                                }
                                if (chkIsCredit.IsChecked == true)
                                {

                                    objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                                    if (HawalaID > 0)
                                    {
                                        objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                                    }
                                    // objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID,-1* Newaccountbalance);
                                }
                                objUsersServiceCleint.UpdateAccountBalacnebyUser(AddedbyID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                                LoggedinUserDetail.InsertActivityLog(AddedbyID, "Removed Balance " + Newaccountbalance.ToString() + "  from user ( " + cmbUsersCredit.Text + " )");
                                btnRemovecredit.IsEnabled = true;
                                MessageBox.Show("Successfully Removed");
                                CreditUsersLoad();
                                txtAccountsTitle.Text = "";
                                txtBalanceAdd.Text = "";
                            }
                        }
                    }
                }
                else
                {

                }

            }
            else
            {
                MessageBox.Show("Please enter amount");
            }
        }
        public void LoadBalanceofTransferTo()
        {
            try
            {
                if (Convert.ToInt32(cmbTransferBalanceTo.SelectedValue) > 0)
                {
                    lblCurrentBalanceTo.Content = objUsersServiceCleint.GetCurrentBalancebyUser(Convert.ToInt32(cmbTransferBalanceTo.SelectedValue), LoggedinUserDetail.PasswordForValidate).ToString();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void cmbTransferBalanceTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadBalanceofTransferTo();
        }
        public void LoadTransferBalanceFrom()
        {
            try
            {
                if (Convert.ToInt32(cmbTransferBalanceFrom.SelectedValue) > 0)
                {
                    lblCurrentbalanceFrom.Content = objUsersServiceCleint.GetCurrentBalancebyUser(Convert.ToInt32(cmbTransferBalanceFrom.SelectedValue), LoggedinUserDetail.PasswordForValidate).ToString();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void cmbTransferBalanceFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadTransferBalanceFrom();
        }
        public string TransferBalanceFromOneUsertoAnother(decimal AccountBalance, int UserIDFrom, string UsernameFrom, string AccountsTitle, int UserIDTo, string UsernameTo)
        {
            LoggedinUserDetail.CheckifUserLogin();
            if (LoggedinUserDetail.GetUserTypeID() != 3)
            {
                decimal UserFromBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserIDFrom, LoggedinUserDetail.PasswordForValidate));
                decimal UserToBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserIDTo, LoggedinUserDetail.PasswordForValidate));
                if (UserFromBalance < AccountBalance)
                {
                    return "Amount is less than available balance";
                }
                objAccountsService.AddtoUsersAccounts(AccountsTitle, AccountBalance.ToString(), "0.00", UserIDTo, "", DateTime.Now, "","", "", "", UserToBalance, false, "", "", "", "", "");
                objUsersServiceCleint.AddCredittoUser(AccountBalance, UserIDTo, LoggedinUserDetail.GetUserID(), DateTime.Now, 0, false, AccountsTitle, false, LoggedinUserDetail.PasswordForValidate);
                objAccountsService.AddtoUsersAccounts(AccountsTitle, "0.00", AccountBalance.ToString(), UserIDFrom, "", DateTime.Now, "", "","", "", UserFromBalance, false, "", "", "", "", "");
                objUsersServiceCleint.AddCredittoUser(-1 * AccountBalance, UserIDFrom, LoggedinUserDetail.GetUserID(), DateTime.Now, 0, false, AccountsTitle, false, LoggedinUserDetail.PasswordForValidate);
                objUsersServiceCleint.AddUserActivity("Transfer Balance From " + UsernameFrom + " To " + UsernameTo + " ( " + AccountBalance.ToString() + " ).", DateTime.Now, LoggedinUserDetail.GetIPAddress(), "", "", LoggedinUserDetail.GetUserID());
            }
            return "True";
        }
        private void btnTransferamount_Click(object sender, RoutedEventArgs e)
        {
            if (txtAccounttitleTransferBalance.Text.Length == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Enter accounts title");
                return;

            }
            if (txtAmountTransfer.Text.Length == 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Enter amount.");
                return;

            }
            btnTransferamount.IsEnabled = false;
            if (Convert.ToInt32(cmbTransferBalanceFrom.SelectedValue) > 0 && (Convert.ToInt32(cmbTransferBalanceTo.SelectedValue) > 0))
            {
                int useridtocreatedby = LoggedinUserDetail.AllUsers.Where(item => item.ID == Convert.ToInt32(cmbTransferBalanceTo.SelectedValue)).FirstOrDefault().CreatedbyID;
                int useridfromcreatedby = LoggedinUserDetail.AllUsers.Where(item => item.ID == Convert.ToInt32(cmbTransferBalanceFrom.SelectedValue)).FirstOrDefault().CreatedbyID;
                if (useridtocreatedby != useridfromcreatedby)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Cannot perform this operation. Please select correct users.");
                    btnTransferamount.IsEnabled = true;
                    return;
                }
                string message = TransferBalanceFromOneUsertoAnother(Convert.ToDecimal(txtAmountTransfer.Text), Convert.ToInt32(cmbTransferBalanceFrom.SelectedValue), cmbTransferBalanceFrom.Text, txtAccounttitleTransferBalance.Text, Convert.ToInt32(cmbTransferBalanceTo.SelectedValue), cmbTransferBalanceTo.Text);
                if (message == "True")
                {
                    btnTransferamount.IsEnabled = true;
                    Xceed.Wpf.Toolkit.MessageBox.Show("Successfully Transfered");
                    LoadTransferBalanceFrom();
                    LoadBalanceofTransferTo();
                    txtAccounttitleTransferBalance.Text = "";
                    txtAmountTransfer.Text = "";
                }
                else
                {
                    btnTransferamount.IsEnabled = true;
                    Xceed.Wpf.Toolkit.MessageBox.Show(message);
                }
            }
        }

        private void cmbCustomerAllowedMarkets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CustomerAllowedMarketsLoad();
            FilterMarkets();
            GetDistinctEventsbyMArketBookName();
        }
        public void GetDistinctEventsbyMArketBookName()
        {
            try
            {
                cmbEventsforMarketAllowed.ItemsSource = null;
                if (comboBox1.SelectedIndex > 0 && lstMarketsAllowedforBet.Count > 0)
                {
                    ComboBoxItem matchtype = (ComboBoxItem)comboBox1.SelectedItem;
                    var lstdistincteventmarkets = lstMarketsAllowedforBet.Where(item => item.Market.Contains("Match Odds") && item.EventTypeName == matchtype.Content.ToString()).Distinct().ToList();
                    if (lstdistincteventmarkets.Count > 0)
                    {
                        GetMarketForAllowedBetting_Result objPleaseselect = new GetMarketForAllowedBetting_Result();
                        objPleaseselect.EventID = "0";
                        objPleaseselect.Market = "Please Select";
                        lstdistincteventmarkets.Insert(0, objPleaseselect);
                        cmbEventsforMarketAllowed.ItemsSource = lstdistincteventmarkets;
                        cmbEventsforMarketAllowed.DisplayMemberPath = "Market";
                        cmbEventsforMarketAllowed.SelectedValuePath = "EventID";
                        cmbEventsforMarketAllowed.SelectedIndex = -1;
                        //List<GetMarketForAllowedBetting_Result> lstMarketsAllowedforBetSelected = lstMarketsAllowedforBet.Where(item => item.EventID == lstdistincteventmarkets.FirstOrDefault().EventID).ToList();
                        //dgvAllowedMarkets.ItemsSource = lstMarketsAllowedforBetSelected;
                    }
                    else
                    {
                        cmbEventsforMarketAllowed.ItemsSource = null;
                    }

                }
                else
                {
                    cmbEventsforMarketAllowed.ItemsSource = null;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }


        public void FilterMarkets()
        {
            try
            {


                if (comboBox1.SelectedIndex > 0 && lstMarketsAllowedforBet.Count > 0)
                {
                    ComboBoxItem matchtype = (ComboBoxItem)comboBox1.SelectedItem;
                    if (chkDateFilter.IsChecked == true)
                    {
                        List<GetMarketForAllowedBetting_Result> lstMarketsAllowedforBetSelected = lstMarketsAllowedforBet.Where(item => item.EventTypeName == matchtype.Content.ToString() && item.EventOpenDate.Value.Date >= DateTime.Now.AddHours(-5).Date).ToList();
                        dgvAllowedMarkets.ItemsSource = lstMarketsAllowedforBetSelected;
                    }
                    else
                    {
                        List<GetMarketForAllowedBetting_Result> lstMarketsAllowedforBetSelected = lstMarketsAllowedforBet.Where(item => item.EventTypeName == matchtype.Content.ToString()).ToList();
                        dgvAllowedMarkets.ItemsSource = lstMarketsAllowedforBetSelected;
                    }

                }
                else
                {
                    if (chkDateFilter != null)
                    {


                        if (chkDateFilter.IsChecked == true)
                        {
                            List<GetMarketForAllowedBetting_Result> lstMarketsAllowedforBetSelected = lstMarketsAllowedforBet.Where(item => item.EventOpenDate.Value.Date >= DateTime.Now.AddHours(-5).Date).ToList();
                            dgvAllowedMarkets.ItemsSource = lstMarketsAllowedforBetSelected;
                        }
                        else
                        {
                            dgvAllowedMarkets.ItemsSource = lstMarketsAllowedforBet;
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
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterMarkets();
            GetDistinctEventsbyMArketBookName();
        }

        private void chkDateFilter_Checked(object sender, RoutedEventArgs e)
        {
            FilterMarkets();
            GetDistinctEventsbyMArketBookName();
        }

        private void chkDateFilter_Unchecked(object sender, RoutedEventArgs e)
        {
            FilterMarkets();
            GetDistinctEventsbyMArketBookName();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                objUsersServiceCleint.UpdateAllowedMarketsbyUserID(chkAllowedCricketOdds.IsChecked.Value, chkTiedMatchAllowed.IsChecked.Value, chkCompletedMatchAllowed.IsChecked.Value, chkAllowedInningsRuns.IsChecked.Value, chkAllowdSoccer.IsChecked.Value, chkAllowedTenis.IsChecked.Value, chkAllowdHorseWin.IsChecked.Value, chkHorseRacePlaceAllowed.IsChecked.Value, chkGrayHoundAllowedWin.IsChecked.Value, chkGrayHoundPlaceAllowed.IsChecked.Value, Convert.ToInt32(cmbCustomerAllowedMarkets.SelectedValue), chkWinnerForAllowedBet.IsChecked.Value, LoggedinUserDetail.PasswordForValidate, chkFancyForAllowedBet.IsChecked.Value);
                objUsersServiceCleint.UpdateMaxOddBackandLay(Convert.ToInt32(cmbCustomerAllowedMarkets.SelectedValue), Convert.ToDecimal(txtMaxOddBack.Text), chkCheckforMaxOddBack.IsChecked.Value, Convert.ToDecimal(txtMaxOddLay.Text), chkCheckForMaxOddLay.IsChecked.Value);
                objUsersServiceCleint.SetLoggedinStatus(Convert.ToInt32(cmbCustomerAllowedMarkets.SelectedValue), false);
                Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1 && dgvAllowedMarkets.Items.Count > 0)

                {
                    MessageBoxResult result = MessageBox.Show("Do you want to update? There are " + dgvAllowedMarkets.Items.Count.ToString() + " rows and if agent is selected then it will update its End users also", "Confirmation", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        stkpnlLoaderHorse.Visibility = Visibility.Visible;
                        if (cmbCustomerAllowedMarketsNew.SelectedValue.Count() > 0)
                        {
                            List<int> userIDs = cmbCustomerAllowedMarketsNew.SelectedValue.ToString().Split(',').Select(int.Parse).ToList();
                            var selecteditems = (List<GetMarketForAllowedBetting_Result>)dgvAllowedMarkets.ItemsSource;
                            var targetList = selecteditems
  .Select(x => new SP_UserMarket_GetMarketForAllowedBetting_Result() { MarketCatalogueID = x.MarketCatalogueID, BettingAllowed = x.BettingAllowed })
  .ToList();
                            objUsersServiceCleint.UpdateMarketAllowedBettingForAllAgentsAsync(userIDs.ToArray(), targetList.ToArray());
                            objUsersServiceCleint.UpdateMarketAllowedBettingForAllAgentsCompleted += ObjUsersServiceCleint_UpdateMarketAllowedBettingForAllAgentsCompleted;


                        }

                        //foreach (GetMarketForAllowedBetting_Result dgvRow in dgvAllowedMarkets.Items)
                        //{
                        //    objUsersServiceCleint.UpdateMarketAllowedBetting(Convert.ToInt32(cmbCustomerAllowedMarkets.SelectedValue), dgvRow.MarketCatalogueID, Convert.ToBoolean(dgvRow.BettingAllowed));

                        //}

                    }


                }
            }
            catch (System.Exception ex)
            {
                stkpnlLoaderHorse.Visibility = Visibility.Collapsed;
                MessageBox.Show(ex.Message);
            }
        }

        private void ObjUsersServiceCleint_UpdateMarketAllowedBettingForAllAgentsCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                List<int> userIDs = cmbCustomerAllowedMarketsNew.SelectedValue.ToString().Split(',').Select(int.Parse).ToList();
                try
                {

                    var selecteditems = (List<GetMarketForAllowedBetting_Result>)dgvAllowedMarkets.ItemsSource;
                    var cricketmatchoddslist = selecteditems.Where(item => item.Market.Contains("Match Odds") && item.EventTypeName == "Cricket" && item.BettingAllowed == true).ToList();
                    if (cricketmatchoddslist.Count > 0)
                    {
                        foreach (var cricketitem in cricketmatchoddslist)
                        {
                            objUsersServiceCleint.UpdateFancySyncONorOFFAsync(73, cricketitem.EventID, chkSTartFancySyncAllowed.IsChecked.Value);
                        }
                    }
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {

                }
                //foreach (var userid in userIDs)
                //{
                //    objUsersServiceCleint.SetLoggedinStatusAsync(userid, false);
                //}

                Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
                CustomerAllowedMarketsLoad();
                FilterMarkets();
                stkpnlLoaderHorse.Visibility = Visibility.Collapsed;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                stkpnlLoaderHorse.Visibility = Visibility.Collapsed;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            lstMarketsAllowedforBet.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().BettingAllowed = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = (CheckBox)sender;
            lstMarketsAllowedforBet.Where(item => item.MarketCatalogueID == chkbox.Tag.ToString()).FirstOrDefault().BettingAllowed = false;
        }
        List<GetReferrerRateandReferrerIDbyUserID_Result> lstNewRefferes = new List<GetReferrerRateandReferrerIDbyUserID_Result>();
        private void cmbCustomersForLimitsLoad()
        {
            try
            {


                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    if (cmbCustomersForLimits.SelectedValue is int)
                    {

                        var lstRefferrers = objUsersServiceCleint.GetReferrerRatesbyUserID(Convert.ToInt32(cmbCustomersForLimits.SelectedValue));
                        DGVHisaPunters.ItemsSource = null;
                        lstNewRefferes = new List<GetReferrerRateandReferrerIDbyUserID_Result>();
                        if (lstRefferrers.Count() > 0)
                        {
                            foreach (var item in lstRefferrers)
                            {
                                cmbReferrerUser.SelectedValue = item.ReferrerID;
                                GetReferrerRateandReferrerIDbyUserID_Result objReffere = new GetReferrerRateandReferrerIDbyUserID_Result();
                                objReffere.ReferrerID = item.ReferrerID;
                                objReffere.RefferrerName = LoggedinUserDetail.AllUsers.Where(item1 => item1.ID == item.ReferrerID).Select(item1 => new { item1.UserName }).FirstOrDefault().UserName.ToString();
                                objReffere.ReferrerRate = item.ReferrerRate;
                                lstNewRefferes.Add(objReffere);

                            }
                            DGVHisaPunters.ItemsSource = lstNewRefferes;

                        }
                        else
                        {

                        }
                        UserDetails objuserDetails = getuserdetails(Convert.ToInt32(cmbCustomersForLimits.SelectedValue));
                        txtCommissionRateUpdate.Text = objuserDetails.CommissionRate.ToString();
                        txtCommissionRateFancyUpdate.Text = objUsersServiceCleint.GetCommissionRatebyUserIDFancy(Convert.ToInt32(cmbCustomersForLimits.SelectedValue)).ToString();
                        chkSoccerForView.IsChecked = objuserDetails.isSoccerAllowed;
                        chkTennisForView.IsChecked = objuserDetails.isTennisAllowed;
                        chkGrayHoundForView.IsChecked = objuserDetails.isGrayHoundRaceAllowed;
                        chkHorseRaceAllowedForViewMarket.IsChecked = objuserDetails.isHorseRaceAllowed;
                        //Cricket
                        txtMatchOddsLower.Text = objuserDetails.BetLowerLimitMatchOdds.ToString("N0");
                        txtMatchOddsUpper.Text = objuserDetails.BetUpperLimitMatchOdds.ToString("N0");
                        txtTiedMatchLower.Text = objuserDetails.BetLowerLimitTiedMatch.ToString("N0");
                        txtTiedMatchUpper.Text = objuserDetails.BetUpperLimitTiedMatch.ToString("N0");
                        txtCompletedLower.Text = objuserDetails.BetLowerLimitCompletedMatch.ToString("N0");
                        txtCompletedUpper.Text = objuserDetails.BetUpperLimitCompletedMatch.ToString("N0");
                        txtInnsRunsLower.Text = objuserDetails.BetLowerLimitInningRuns.ToString("N0");
                        txtInnsRunsUpper.Text = objuserDetails.BetUpperLimitInningRuns.ToString("N0");
                        txtWinnerLower.Text = objuserDetails.BetLowerLimitWinner.ToString("N0");
                        txtWinnerUpper.Text = objuserDetails.BetUpperLimitWinner.ToString("N0");
                        txtFancyLower.Text = objuserDetails.BetLowerLimitFancy.ToString("N0");
                        txtFancyUpper.Text = objuserDetails.BetUpperLimitFancy.ToString("N0");
                        //Horse Racing
                        txtHorsePlaceLower.Text = objuserDetails.BetLowerLimitHorsePlace.ToString("N0");
                        txtHorsePlaceUpper.Text = objuserDetails.BetUpperLimitHorsePlace.ToString("N0");
                        txtHorseWinLower.Text = objuserDetails.BetLowerLimit.ToString("N0");
                        txtHorseWinUpper.Text = objuserDetails.BetUpperLimit.ToString("N0");
                        //GrayHound Racing
                        txtGrayHoundPlaceLower.Text = objuserDetails.BetLowerLimitGrayHoundPlace.ToString("N0");
                        txtGrayHoundPlaceUpper.Text = objuserDetails.BetUpperLimitGrayHoundPlace.ToString("N0");
                        txtGrayHoundWinLower.Text = objuserDetails.BetLowerLimitGrayHoundWin.ToString("N0");
                        txtGrayHoundWinUpper.Text = objuserDetails.BetUpperLimitGrayHoundWin.ToString("N0");
                        //Soccer & Tennis
                        txtSoccerLower.Text = objuserDetails.BetLowerLimitMatchOddsSoccer.ToString("N0");
                        txtSoccerUpper.Text = objuserDetails.BetUpperLimitMatchOddsSoccer.ToString("N0");
                        txtTennisLower.Text = objuserDetails.BetLowerLimitMatchOddsTennis.ToString("N0");
                        txtTennisUpper.Text = objuserDetails.BetUpperLimitMatchOddsTennis.ToString("N0");
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void cmbCustomersForLimits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbCustomersForLimitsLoad();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                if (Convert.ToInt32(cmbCustomersForLimits.SelectedValue) > 0)
                {


                    //Cricket
                    UserDetails objuserDetails = new UserDetails();
                    objuserDetails.BetLowerLimitMatchOdds = Convert.ToDecimal(txtMatchOddsLower.Text);
                    objuserDetails.BetUpperLimitMatchOdds = Convert.ToDecimal(txtMatchOddsUpper.Text.ToString());
                    objuserDetails.BetLowerLimitTiedMatch = Convert.ToDecimal(txtTiedMatchLower.Text.ToString());
                    objuserDetails.BetUpperLimitTiedMatch = Convert.ToDecimal(txtTiedMatchUpper.Text.ToString());
                    objuserDetails.BetLowerLimitCompletedMatch = Convert.ToDecimal(txtCompletedLower.Text.ToString());
                    objuserDetails.BetUpperLimitCompletedMatch = Convert.ToDecimal(txtCompletedUpper.Text.ToString());
                    objuserDetails.BetLowerLimitInningRuns = Convert.ToDecimal(txtInnsRunsLower.Text.ToString());
                    objuserDetails.BetUpperLimitInningRuns = Convert.ToDecimal(txtInnsRunsUpper.Text.ToString());
                    objuserDetails.BetLowerLimitWinner = Convert.ToDecimal(txtWinnerLower.Text.ToString());
                    objuserDetails.BetUpperLimitWinner = Convert.ToDecimal(txtWinnerUpper.Text.ToString());
                    objuserDetails.BetLowerLimitFancy = Convert.ToDecimal(txtFancyLower.Text.ToString());
                    objuserDetails.BetUpperLimitFancy = Convert.ToDecimal(txtFancyUpper.Text.ToString());
                    //Horse Racing
                    objuserDetails.BetLowerLimitHorsePlace = Convert.ToDecimal(txtHorsePlaceLower.Text.ToString());
                    objuserDetails.BetUpperLimitHorsePlace = Convert.ToDecimal(txtHorsePlaceUpper.Text.ToString());
                    objuserDetails.BetLowerLimit = Convert.ToDecimal(txtHorseWinLower.Text.ToString());
                    objuserDetails.BetUpperLimit = Convert.ToDecimal(txtHorseWinUpper.Text.ToString());
                    //GrayHound Racing
                    objuserDetails.BetLowerLimitGrayHoundPlace = Convert.ToDecimal(txtGrayHoundPlaceLower.Text.ToString());
                    objuserDetails.BetUpperLimitGrayHoundPlace = Convert.ToDecimal(txtGrayHoundPlaceUpper.Text.ToString());
                    objuserDetails.BetLowerLimitGrayHoundWin = Convert.ToDecimal(txtGrayHoundWinLower.Text.ToString());
                    objuserDetails.BetUpperLimitGrayHoundWin = Convert.ToDecimal(txtGrayHoundWinUpper.Text.ToString());
                    //Soccer & Tennis
                    objuserDetails.BetLowerLimitMatchOddsSoccer = Convert.ToDecimal(txtSoccerLower.Text.ToString());
                    objuserDetails.BetUpperLimitMatchOddsSoccer = Convert.ToDecimal(txtSoccerUpper.Text.ToString());
                    objuserDetails.BetLowerLimitMatchOddsTennis = Convert.ToDecimal(txtTennisLower.Text.ToString());
                    objuserDetails.BetUpperLimitMatchOddsTennis = Convert.ToDecimal(txtTennisUpper.Text.ToString());

                    objUsersServiceCleint.UpdateBetLowerLimit(Convert.ToInt32(cmbCustomersForLimits.SelectedValue), objuserDetails.BetLowerLimit, objuserDetails.BetUpperLimit, chkGrayHoundForView.IsChecked.Value, chkHorseRaceAllowedForViewMarket.IsChecked.Value, objuserDetails.BetLowerLimitHorsePlace, objuserDetails.BetUpperLimitHorsePlace, objuserDetails.BetLowerLimitGrayHoundWin, objuserDetails.BetUpperLimitGrayHoundWin, objuserDetails.BetLowerLimitGrayHoundPlace, objuserDetails.BetUpperLimitGrayHoundPlace, objuserDetails.BetLowerLimitMatchOdds, objuserDetails.BetUpperLimitMatchOdds, objuserDetails.BetLowerLimitInningRuns, objuserDetails.BetUpperLimitInningRuns, objuserDetails.BetLowerLimitCompletedMatch, objuserDetails.BetUpperLimitCompletedMatch, chkTennisForView.IsChecked.Value, chkSoccerForView.IsChecked.Value, Convert.ToInt32(txtCommissionRateUpdate.Text), objuserDetails.BetLowerLimitMatchOddsSoccer, objuserDetails.BetUpperLimitMatchOddsSoccer, objuserDetails.BetLowerLimitMatchOddsTennis, objuserDetails.BetUpperLimitMatchOddsTennis, objuserDetails.BetUpperLimitTiedMatch, objuserDetails.BetLowerLimitTiedMatch, objuserDetails.BetUpperLimitWinner, objuserDetails.BetLowerLimitWinner, LoggedinUserDetail.PasswordForValidate, objuserDetails.BetUpperLimitFancy, objuserDetails.BetLowerLimitFancy);
                    objUsersServiceCleint.UpdateCommissionRatebyUserID(Convert.ToInt32(cmbCustomersForLimits.SelectedValue), Convert.ToInt32(txtCommissionRateFancyUpdate.Text));
                    objUsersServiceCleint.SetLoggedinStatus(Convert.ToInt32(cmbCustomersForLimits.SelectedValue), false);

                    MessageBox.Show("Updated Successfully");
                    cmbCustomersForLimitsLoad();
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbCustomersForLimits.SelectedIndex > 0 && cmbReferrerUser.SelectedIndex > 0)
                {
                    var refferer = lstNewRefferes.Where(item => item.ReferrerID == Convert.ToInt32(cmbReferrerUser.SelectedValue)).FirstOrDefault();
                    if (refferer != null)
                    {
                        refferer.ReferrerRate = Convert.ToInt32(txtReferreRate.Text);
                        DGVHisaPunters.ItemsSource = null;
                        DGVHisaPunters.Items.Clear();
                        DGVHisaPunters.ItemsSource = lstNewRefferes;
                        return;
                    }
                    else
                    {
                        GetReferrerRateandReferrerIDbyUserID_Result objReffere = new GetReferrerRateandReferrerIDbyUserID_Result();
                        objReffere.ReferrerID = Convert.ToInt32(cmbReferrerUser.SelectedValue.ToString());
                        objReffere.RefferrerName = cmbReferrerUser.Text;
                        objReffere.ReferrerRate = Convert.ToInt32(txtReferreRate.Text);
                        lstNewRefferes.Add(objReffere);
                        DGVHisaPunters.ItemsSource = null;
                        DGVHisaPunters.Items.Clear();
                        DGVHisaPunters.ItemsSource = lstNewRefferes;
                    }

                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                if (Convert.ToInt32(cmbCustomersForLimits.SelectedValue) > 0)
                {
                    objUsersServiceCleint.DeletReffererUSers(Convert.ToInt32(cmbCustomersForLimits.SelectedValue));
                }

                if (DGVHisaPunters.Items.Count > 0)
                {

                    foreach (GetReferrerRateandReferrerIDbyUserID_Result dgRow in DGVHisaPunters.Items)
                    {
                        objUsersServiceCleint.AddReferrerUsers(Convert.ToInt32(cmbCustomersForLimits.SelectedValue), Convert.ToInt32(dgRow.ReferrerID), Convert.ToInt32(dgRow.ReferrerRate));
                    }

                }
                Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
                cmbCustomersForLimitsLoad();
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            GetusersbyUserTypeReload();
            GetUsersbyUsersType();

        }

        private void btnBlockAllUsers_Click(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Do you want to update?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    objUsersServiceCleint.UpdateUsersAllBlock();
                    Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully.");
                    btnRefresh_Click(this, e);
                }



            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                MessageBoxResult result = Xceed.Wpf.Toolkit.MessageBox.Show("Do you want to update?", "Confirmation", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    objUsersServiceCleint.UpdateUsersAllLoggedOut();
                    MessageBox.Show("Updated Successfully.");
                    btnRefresh_Click(this, e);
                }



            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                if (dgvUnBlockUsers.Items.Count > 0)
                {
                    foreach (UserIDandUserType dgvRow in LoggedinUserDetail.AllUsers)
                    {
                        int UserID = Convert.ToInt32(dgvRow.ID);
                        bool isBlocked = Convert.ToBoolean(dgvRow.isBlocked);
                        objUsersServiceCleint.SetBlockedStatusofUser(UserID, isBlocked, LoggedinUserDetail.PasswordForValidate);

                    }
                    btnRefresh_Click(this, e);
                    Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully.");

                }
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                if (dgvUnBlockUsers.Items.Count > 0)
                {
                    foreach (UserIDandUserType dgvRow in LoggedinUserDetail.AllUsers)
                    {
                        int UserID = Convert.ToInt32(dgvRow.ID);
                        bool loggedin = Convert.ToBoolean(dgvRow.Loggedin);
                        objUsersServiceCleint.SetLoggedinStatus(UserID, loggedin);
                    }
                    btnRefresh_Click(this, e);
                    Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully.");

                }
            }
        }
        public void GetBetsIntervalandPlaceBetTimings(int UserID)
        {
            try
            {


                SP_BetPlaceWaitandInterval_GetAllData_Result BetPlaceWaitandInterval = new SP_BetPlaceWaitandInterval_GetAllData_Result();
                BetPlaceWaitandInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUsersServiceCleint.GetIntervalandBetPlaceTimings(UserID));
                txtMatchOddsBetPlace.Text = BetPlaceWaitandInterval.CricketMatchOddsBetPlaceWait.ToString();
                txtMatchOddsInterval.Text = BetPlaceWaitandInterval.CricketMatchOddsTimerInterval.ToString();
                txtHorseRacingInterval.Text = BetPlaceWaitandInterval.HorseRaceTimerInterval.ToString();
                txtHorseRacingBetPlace.Text = BetPlaceWaitandInterval.HorseRaceBetPlaceWait.ToString();
                txtGrayHoundBetPlace.Text = BetPlaceWaitandInterval.GrayHoundBetPlaceWait.ToString();
                txtGrayHoundInterval.Text = BetPlaceWaitandInterval.GrayHoundTimerInterval.ToString();
                txtCompletedBetInterval.Text = BetPlaceWaitandInterval.CompletedMatchTimerInterval.ToString();
                txtCompletedBetPlace.Text = BetPlaceWaitandInterval.CompletedMatchBetPlaceWait.ToString();
                txtTiedMatchInterval.Text = BetPlaceWaitandInterval.TiedMatchTimerInterval.ToString();
                txtTiedMatchBetPlace.Text = BetPlaceWaitandInterval.TiedMatchBetPlaceWait.ToString();
                txtInnsrunsInterval.Text = BetPlaceWaitandInterval.InningsRunsTimerInterval.ToString();
                txtInnsRunsBetPlace.Text = BetPlaceWaitandInterval.InningsRunsBetPlaceWait.ToString();
                txtWinnerInterval.Text = BetPlaceWaitandInterval.WinnerTimerInterval.ToString();
                txtWinnerBetPlace.Text = BetPlaceWaitandInterval.WinnerBetPlaceWait.ToString();
                txtTennisInterval.Text = BetPlaceWaitandInterval.TennisTimerInterval.ToString();
                txtTennisBetPlace.Text = BetPlaceWaitandInterval.TennisBetPlaceWait.ToString();
                txtSoccerInterval.Text = BetPlaceWaitandInterval.SoccerTimerInterval.ToString();
                txtSoccerBetPlace.Text = BetPlaceWaitandInterval.SoccerBetPlaceWait.ToString();
                txtPoundRateUser.Text = BetPlaceWaitandInterval.PoundRate.ToString();
                txtFancyBetPlace.Text = BetPlaceWaitandInterval.FancyBetPlaceWait.Value.ToString();
                txtFancyInterval.Text = BetPlaceWaitandInterval.FancyTimerInterval.Value.ToString();
                txtRaceBeforeAllowedMinute.Text = BetPlaceWaitandInterval.RaceMinutesBeforeStart.Value.ToString();
                txtCancelBetTime.Text = BetPlaceWaitandInterval.CancelBetTime.Value.ToString();
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

        }
        private void cmbCustomersForTimersLoad()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                if (cmbCustomersForTimers.SelectedValue is int)
                {
                    GetBetsIntervalandPlaceBetTimings(Convert.ToInt32(cmbCustomersForTimers.SelectedValue));
                    chkShowTV.IsChecked = objUsersServiceCleint.GetShowTV(Convert.ToInt32(cmbCustomersForTimers.SelectedValue));
                    var transferadminamountresults = objUsersServiceCleint.GetTransferAdminAmount(Convert.ToInt32(cmbCustomersForTimers.SelectedValue));
                    chkTrsnferAdminamount.IsChecked = transferadminamountresults.TransferAdminAmount.Value;
                    //soccer
                    chkTrsnferAdminamountSoccer.IsChecked = transferadminamountresults.TransferAdminAmountSoccer.Value;
                    //tennis
                    chkTrsnferAdminamountTennis.IsChecked = transferadminamountresults.TransferAdminAmountTennis.Value;
                    //horse
                    chkTrsnferAdminamountHorse.IsChecked = transferadminamountresults.TransferAdminAmountHorse.Value;
                    //greyhound
                    chkTrsnferAdminamountGreyHound.IsChecked = transferadminamountresults.TransferAdminAmountGreyHound.Value;
                    //selected agent
                    cmbAgentForTransferAdmin.SelectedValue = transferadminamountresults.TransferAgentID.Value;
                }
            }
        }
        private void cmbCustomersForTimers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbCustomersForTimersLoad();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            if (cmbCustomersForTimers.SelectedIndex > 0)
            {
                objUsersServiceCleint.UpdateShowTV(Convert.ToInt32(cmbCustomersForTimers.SelectedValue), chkShowTV.IsChecked.Value);
                objUsersServiceCleint.UpdateTransferAdminAmount(Convert.ToInt32(cmbCustomersForTimers.SelectedValue), chkTrsnferAdminamount.IsChecked.Value, Convert.ToInt32(cmbAgentForTransferAdmin.SelectedValue), chkTrsnferAdminamountSoccer.IsChecked.Value, chkTrsnferAdminamountTennis.IsChecked.Value, chkTrsnferAdminamountHorse.IsChecked.Value, chkTrsnferAdminamountGreyHound.IsChecked.Value);
                objUsersServiceCleint.SetLoggedinStatus(Convert.ToInt32(cmbCustomersForTimers.SelectedValue), false);
                Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully.");
            }
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                SP_BetPlaceWaitandInterval_GetAllData_Result BetPlaceWaitandInterval = new SP_BetPlaceWaitandInterval_GetAllData_Result();

                int CricketMatchOddsBetPlaceWait = Convert.ToInt32(txtMatchOddsBetPlace.Text);
                int CricketMatchOddsTimerInterval = Convert.ToInt32(txtMatchOddsInterval.Text.ToString());
                int HorseRaceTimerInterval = Convert.ToInt32(txtHorseRacingInterval.Text.ToString());
                int HorseRaceBetPlaceWait = Convert.ToInt32(txtHorseRacingBetPlace.Text.ToString());
                int GrayHoundBetPlaceWait = Convert.ToInt32(txtGrayHoundBetPlace.Text.ToString());
                int GrayHoundTimerInterval = Convert.ToInt32(txtGrayHoundInterval.Text.ToString());
                int CompletedMatchTimerInterval = Convert.ToInt32(txtCompletedBetInterval.Text.ToString());
                int CompletedMatchBetPlaceWait = Convert.ToInt32(txtCompletedBetPlace.Text.ToString());
                int TiedMatchTimerInterval = Convert.ToInt32(txtTiedMatchInterval.Text.ToString());
                int TiedMatchBetPlaceWait = Convert.ToInt32(txtTiedMatchBetPlace.Text.ToString());
                int InningsRunsTimerInterval = Convert.ToInt32(txtInnsrunsInterval.Text.ToString());
                int InningsRunsBetPlaceWait = Convert.ToInt32(txtInnsRunsBetPlace.Text.ToString());
                int WinnerTimerInterval = Convert.ToInt32(txtWinnerInterval.Text.ToString());
                int WinnerBetPlaceWait = Convert.ToInt32(txtWinnerBetPlace.Text.ToString());
                int TennisTimerInterval = Convert.ToInt32(txtTennisInterval.Text.ToString());
                int TennisBetPlaceWait = Convert.ToInt32(txtTennisBetPlace.Text.ToString());
                int SoccerTimerInterval = Convert.ToInt32(txtSoccerInterval.Text.ToString());
                int SoccerBetPlaceWait = Convert.ToInt32(txtSoccerBetPlace.Text.ToString());
                int FancyTimerInterval = Convert.ToInt32(txtFancyInterval.Text.ToString());
                int FancyBetPlaceWait = Convert.ToInt32(txtFancyBetPlace.Text.ToString());
                decimal PoundRate = Convert.ToDecimal(txtPoundRateUser.Text.ToString());
                int RaceBeforeAllowedMinutes = Convert.ToInt32(txtRaceBeforeAllowedMinute.Text);
                int CancelBetTime = Convert.ToInt32(txtCancelBetTime.Text);
                objUsersServiceCleint.UpdateIntervalandBetPlaceTimings(HorseRaceTimerInterval, HorseRaceBetPlaceWait, GrayHoundTimerInterval, GrayHoundBetPlaceWait, CricketMatchOddsTimerInterval, CricketMatchOddsBetPlaceWait, CompletedMatchTimerInterval, CompletedMatchBetPlaceWait, TiedMatchTimerInterval, TiedMatchBetPlaceWait, InningsRunsTimerInterval, InningsRunsBetPlaceWait, WinnerTimerInterval, WinnerBetPlaceWait, TennisTimerInterval, TennisBetPlaceWait, SoccerTimerInterval, SoccerBetPlaceWait, PoundRate, Convert.ToInt32(cmbCustomersForTimers.SelectedValue), FancyTimerInterval, FancyBetPlaceWait, RaceBeforeAllowedMinutes, CancelBetTime);
                objUsersServiceCleint.SetLoggedinStatus(Convert.ToInt32(cmbCustomersForTimers.SelectedValue), false);
                Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
                GetBetsIntervalandPlaceBetTimings(Convert.ToInt32(cmbCustomersForTimers.SelectedValue));
            }
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            objUsersServiceCleint.UpdateMarqueeText(txtMarquee.Text);
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            if (cmbLinevMarkets.SelectedIndex > -1 && cmbEvents.SelectedIndex > -1)
            {
                objUsersServiceCleint.UpdateAssociateEventID(cmbEvents.SelectedValue.ToString(), cmbLinevMarkets.SelectedValue.ToString());
                objUsersServiceCleint.UpdateGetDataFromForLoggingData(cmbEvents.SelectedValue.ToString(), cmbGetUpdateFrom.Text);
                objUsersServiceCleint.UpdateTotalOversbyMarket(cmbEvents.SelectedValue.ToString(), txtTotalOvers.Text.ToString());
                Xceed.Wpf.Toolkit.MessageBox.Show("Updated successfully.");
            }
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                objUsersServiceCleint.UpdateFancyResultPostSetting(chkAutomaticResultPostFancy.IsChecked.Value);
                // objUsersServiceCleint.UpdateGetFancyResultsFrom(cmbGetFancyResultFrom.Text);
                Xceed.Wpf.Toolkit.MessageBox.Show("Updated successfully.");
            }
        }

        private void TabItem_Loaded_1(object sender, RoutedEventArgs e)
        {
            dtpFromAccounts.SelectedDate = DateTime.Now.Date;
            dtpToAccounts.SelectedDate = DateTime.Now.Date;
            dtpFromBets.SelectedDate = DateTime.Now.Date;
            dtpToBets.SelectedDate = DateTime.Now.Date;
        }
        public class MarketBookandMarketCatatlogue
        {
            public string MarketName { get; set; }
            public string MarketBookID { get; set; }
        }
        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            try
            {
                string DateFrom = dtpFromBets.SelectedDate.Value.ToString("yyyy-MM-dd");
                string DateTo = dtpToBets.SelectedDate.Value.ToString("yyyy-MM-dd");
                List<MarketBookandMarketCatatlogue> lstMarkets = new List<MarketBookandMarketCatatlogue>();
                lstMarkets = JsonConvert.DeserializeObject<List<MarketBookandMarketCatatlogue>>(objUsersServiceCleint.GetDistinctMarketsFromBets(DateFrom, DateTo));
                if (lstMarkets.Count > 0)
                {
                    cmbMarketsbyBets.ItemsSource = lstMarkets;
                    cmbMarketsbyBets.DisplayMemberPath = "MarketName";
                    cmbMarketsbyBets.SelectedValuePath = "MarketBookID";
                }
                else
                {
                    cmbMarketsbyBets.ItemsSource = null;
                    cmbMarketsbyBets.Items.Clear();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            try
            {
                string DateFrom = dtpFromAccounts.SelectedDate.Value.ToString("yyyy-MM-dd");
                string DateTo = dtpToAccounts.SelectedDate.Value.ToString("yyyy-MM-dd");
                List<MarketBookandMarketCatatlogue> lstMarkets = new List<MarketBookandMarketCatatlogue>();
                lstMarkets = JsonConvert.DeserializeObject<List<MarketBookandMarketCatatlogue>>(objUsersServiceCleint.GetDistinctMarketsFromAccounts(DateFrom, DateTo));
                if (lstMarkets.Count > 0)
                {
                    cmbMarketsbyAccounts.ItemsSource = lstMarkets;
                    cmbMarketsbyAccounts.DisplayMemberPath = "MarketName";
                    cmbMarketsbyAccounts.SelectedValuePath = "MarketBookID";
                }
                else
                {
                    cmbMarketsbyAccounts.ItemsSource = null;
                    cmbMarketsbyAccounts.Items.Clear();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult msgres = Xceed.Wpf.Toolkit.MessageBox.Show(this, "Are you Sure to UnPost Results?", "Confirm", MessageBoxButton.YesNo);
                if (msgres == MessageBoxResult.Yes)
                {

                    var result = objUsersServiceCleint.UnPostUserAccountsbyUserIDandMarketID(cmbMarketsbyBets.SelectedValue.ToString(), Convert.ToInt32(cmbCustomersForUpdateResultsBets.SelectedValue), LoggedinUserDetail.PasswordForValidate);
                    if (result == true)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Successfully UnPosted Accounts");
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult msgres = Xceed.Wpf.Toolkit.MessageBox.Show(this, "Are you Sure to change status of Bets?", "Confirm", MessageBoxButton.YesNo);
                if (msgres == MessageBoxResult.Yes)
                {
                    var result = objUsersServiceCleint.UpdateUserBetsStatusbyMarketIDandUserID(cmbMarketsbyBets.SelectedValue.ToString(), Convert.ToInt32(cmbCustomersForUpdateResultsBets.SelectedValue), LoggedinUserDetail.PasswordForValidate);
                    if (result == true)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Successfully Updated");
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult msgres = Xceed.Wpf.Toolkit.MessageBox.Show(this, "Are you Sure to UnPost Results?", "Confirm", MessageBoxButton.YesNo);
                if (msgres == MessageBoxResult.Yes)
                {
                    var result = objUsersServiceCleint.UnPostUserAccountsbyUserIDandMarketID(cmbMarketsbyAccounts.SelectedValue.ToString(), Convert.ToInt32(cmbCustomersForUpdateResultsAccounts.SelectedValue), LoggedinUserDetail.PasswordForValidate);
                    if (result == true)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Successfully UnPosted Accounts");
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult msgres = Xceed.Wpf.Toolkit.MessageBox.Show(this, "Are you Sure to change status of Bets?", "Confirm", MessageBoxButton.YesNo);
                if (msgres == MessageBoxResult.Yes)
                {
                    var result = objUsersServiceCleint.UpdateUserBetsStatusbyMarketIDandUserID(cmbMarketsbyAccounts.SelectedValue.ToString(), Convert.ToInt32(cmbCustomersForUpdateResultsAccounts.SelectedValue), LoggedinUserDetail.PasswordForValidate);
                    if (result == true)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Successfully Updated");
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void DGVHisaPunters_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DGVHisaPunters.CurrentCell.Column.DisplayIndex == 3)
                {
                    GetReferrerRateandReferrerIDbyUserID_Result objReffer = (GetReferrerRateandReferrerIDbyUserID_Result)DGVHisaPunters.SelectedItem;
                    lstNewRefferes.Remove(objReffer);
                    DGVHisaPunters.ItemsSource = null;
                    DGVHisaPunters.Items.Clear();
                    DGVHisaPunters.ItemsSource = lstNewRefferes;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            try
            {

                CheckBox chkbox = (CheckBox)sender;

                LoggedinUserDetail.AllUsers.Where(item => item.ID.ToString() == chkbox.Tag.ToString()).First().isBlocked = true;


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox chkbox = (CheckBox)sender;

                LoggedinUserDetail.AllUsers.Where(item => item.ID.ToString() == chkbox.Tag.ToString()).First().isBlocked = false;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void CheckBox_Unchecked_2(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox chkbox = (CheckBox)sender;

                LoggedinUserDetail.AllUsers.Where(item => item.ID.ToString() == chkbox.Tag.ToString()).First().Loggedin = false;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckBox chkbox = (CheckBox)sender;

                LoggedinUserDetail.AllUsers.Where(item => item.ID.ToString() == chkbox.Tag.ToString()).First().Loggedin = true;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            if (cmbEvents.SelectedIndex > -1)
            {
                //  objUsersServiceCleint.UpdateAssociateEventID(cmbEvents.SelectedValue.ToString(), cmbLinevMarkets.SelectedValue.ToString());
                // objUsersServiceCleint.UpdateGetDataFromForLoggingData(cmbEvents.SelectedValue.ToString(), "Local");
                objUsersServiceCleint.UpdateTotalOversbyMarket(cmbEvents.SelectedValue.ToString(), txtTotalOvers.Text.ToString());
                Xceed.Wpf.Toolkit.MessageBox.Show("Updated successfully.");
            }
        }

        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
            if (cmbEvents.SelectedIndex > -1)
            {
                //  objUsersServiceCleint.UpdateAssociateEventID(cmbEvents.SelectedValue.ToString(), cmbLinevMarkets.SelectedValue.ToString());
                objUsersServiceCleint.UpdateGetDataFromForLoggingData(cmbEvents.SelectedValue.ToString(), cmbGetUpdateFrom.Text);
                // objUsersServiceCleint.UpdateTotalOversbyMarket(cmbEvents.SelectedValue.ToString(), txtTotalOvers.Text.ToString());
                Xceed.Wpf.Toolkit.MessageBox.Show("Updated successfully.");
            }
        }

        private void Button_Click_23(object sender, RoutedEventArgs e)
        {
            if (cmbCricketAPIKeys.SelectedIndex > -1 && cmbEvents.SelectedIndex > -1)
            {
                objUsersServiceCleint.UpdateCricketAPIMatchKey(cmbEvents.SelectedValue.ToString(), cmbCricketAPIKeys.SelectedValue.ToString());
                try
                {
                    CricketScoreServiceReference.Service1Client objCricektDataVlient = new CricketScoreServiceReference.Service1Client();
                    objCricektDataVlient.AddCricketMatchKey(cmbCricketAPIKeys.SelectedValue.ToString(), LoggedinUserDetail.PasswordForValidate);
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {

                }


                MessageBox.Show("Updated successfully.");
            }
        }

        private void btnGetDatabyCashRecieved_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    string DateFrom = dtpFromCash.SelectedDate.Value.ToString("yyyy-MM-dd");
                    string DateTo = dtpToCash.SelectedDate.Value.ToString("yyyy-MM-dd");
                    var results = objAccountsService.GetAccountsCashReceivedorPaidbyDataRange(LoggedinUserDetail.GetUserID(), DateFrom, DateTo);
                    txtTotalCashReceived.Text = results.TotDebit.ToString();
                    txtTotalCashPaid.Text = results.TotCredit.ToString();
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void cmbCustomerAllowedMarketsNew_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCustomerAllowedMarketsNew.SelectedValue.Count() > 0)
                {
                    string[] selectedagents = cmbCustomerAllowedMarketsNew.SelectedValue.ToString().Split(',');

                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void cmbCustomerAllowedMarketskalijutt_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCustomerAllowedMarketskalijutt.SelectedValue.Count() > 0)
                {
                    string[] selectedagents = cmbCustomerAllowedMarketskalijutt.SelectedValue.ToString().Split(',');

                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void cmbCustomerAllowedMarketsInFancy_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            try
            {
                if (cmbCustomerAllowedMarketsInFancy.SelectedValue.Count() > 0)
                {
                    string[] selectedagents = cmbCustomerAllowedMarketsInFancy.SelectedValue.ToString().Split(',');

                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void Button_Click_29(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> userIDs = cmbCustomerAllowedMarketskalijutt.SelectedValue.ToString().Split(',').Select(int.Parse).ToList();
                List<SP_Users_GetUserbyCreatedbyID_Result> lstUSersAll = new List<SP_Users_GetUserbyCreatedbyID_Result>();
                foreach (int id in userIDs)
                {
                    SP_Users_GetUserbyCreatedbyID_Result s = new SP_Users_GetUserbyCreatedbyID_Result();
                    int r = Convert.ToInt32(id);
                    s.ID = r;
                    lstUSersAll.AddRange(JsonConvert.DeserializeObject<List<SP_Users_GetUserbyCreatedbyID_Result>>(objUsersServiceCleint.getlistuserids(r)).ToList());
                    lstUSersAll.Add(s);
                }

                List<string> allusersmarket = new List<string>();
                //  List<int> cardValue = lstUSers.ToString().Select().to;

                string EventID = cmbEvents.SelectedValue.ToString();
                List<SP_UserMarket_GetUserMarketbyEventID1_Result> lstkalijuttmarket = new List<SP_UserMarket_GetUserMarketbyEventID1_Result>();
                lstkalijuttmarket = JsonConvert.DeserializeObject<List<SP_UserMarket_GetUserMarketbyEventID1_Result>>(objUsersServiceCleint.GetMarketbyEventID1(cmbEvents.SelectedValue.ToString()));

                List<string> Figure = new List<string>();

                // Add items using Add method      
                if (cmbkalijut.SelectedIndex == 0)
                {
                    Figure.Add("1st Innings 5 Over Figure");
                    Figure.Add("1st Innings 10 Over Figure");
                    Figure.Add("1st Innings 15 Over Figure");
                    Figure.Add("1st Innings 20 Over Figure");
                    Figure.Add("2st Innings 5 Over Figure");
                    Figure.Add("2st Innings 10 Over Figure");
                }

                if (cmbkalijut.SelectedIndex == 1)
                {
                    Figure.Add("1st Innings 5 Over Figure");
                    Figure.Add("1st Innings 10 Over Figure");
                    Figure.Add("1st Innings 15 Over Figure");
                    Figure.Add("1st Innings 20 Over Figure");
                    Figure.Add("1st Innings 25 Over Figure");
                    Figure.Add("1st Innings 30 Over Figure");
                    Figure.Add("1st Innings 35 Over Figure");
                    Figure.Add("1st Innings 40 Over Figure");
                    Figure.Add("1st Innings 45 Over Figure");
                    Figure.Add("1st Innings 50 Over Figure");
                    Figure.Add("2st Innings 5 Over Figure");
                    Figure.Add("2st Innings 10 Over Figure");
                    Figure.Add("2st Innings 15 Over Figure");
                    Figure.Add("2st Innings 20 Over Figure");
                    Figure.Add("2st Innings 25 Over Figure");
                    Figure.Add("2st Innings 30 Over Figure");
                    Figure.Add("2st Innings 35 Over Figure");
                    Figure.Add("2st Innings 40 Over Figure");
                }

                if (cmbkalijut.SelectedIndex == 2)
                {
                    Figure.Add("1st Innings Team A 10 Over Figure");
                    Figure.Add("1st Innings Team A 15 Over Figure");
                    Figure.Add("1st Innings Team A 20 Over Figure");
                    Figure.Add("1st Innings Team A 25 Over Figure");
                    Figure.Add("1st Innings Team A 30 Over Figure");
                    Figure.Add("1st Innings Team A 35 Over Figure");
                    Figure.Add("1st Innings Team A 40 Over Figure");
                    Figure.Add("1st Innings Team A 45 Over Figure");
                    Figure.Add("1st Innings Team A 50 Over Figure");
                    Figure.Add("1st Innings Team A 55 Over Figure");
                    Figure.Add("1st Innings Team A 60 Over Figure");
                    Figure.Add("1st Innings Team A 70 Over Figure");
                    Figure.Add("1st Innings Team A 75 Over Figure");
                    Figure.Add("1st Innings Team A 80 Over Figure");
                    Figure.Add("1st Innings Team A 85 Over Figure");
                    Figure.Add("1st Innings Team A 90 Over Figure");
                    Figure.Add("1st Innings Team A 95 Over Figure");
                    Figure.Add("1st Innings Team A 100 Over Figure");
                    Figure.Add("1st Innings Team B 10 Over Figure");
                    Figure.Add("1st Innings Team B 15 Over Figure");
                    Figure.Add("1st Innings Team B 20 Over Figure");
                    Figure.Add("1st Innings Team B 25 Over Figure");
                    Figure.Add("1st Innings Team B 30 Over Figure");
                    Figure.Add("1st Innings Team B 35 Over Figure");
                    Figure.Add("1st Innings Team B 40 Over Figure");
                    Figure.Add("1st Innings Team B 45 Over Figure");
                    Figure.Add("1st Innings Team B 50 Over Figure");
                    Figure.Add("1st Innings Team B 55 Over Figure");
                    Figure.Add("1st Innings Team B 60 Over Figure");
                    Figure.Add("1st Innings Team B 65 Over Figure");
                    Figure.Add("1st Innings Team B 70 Over Figure");
                    Figure.Add("1st Innings Team B 75 Over Figure");
                    Figure.Add("1st Innings Team B 80 Over Figure");
                    Figure.Add("1st Innings Team B 85 Over Figure");
                    Figure.Add("1st Innings Team B 90 Over Figure");
                    Figure.Add("1st Innings Team B 95 Over Figure");
                    Figure.Add("1st Innings Team B 100 Over Figure");
                    Figure.Add("2st Innings Team A 10 Over Figure");
                    Figure.Add("2st Innings Team A 15 Over Figure");
                    Figure.Add("2st Innings Team A 20 Over Figure");
                    Figure.Add("2st Innings Team A 25 Over Figure");
                    Figure.Add("2st Innings Team A 30 Over Figure");
                    Figure.Add("2st Innings Team A 35 Over Figure");
                    Figure.Add("2st Innings Team A 40 Over Figure");
                    Figure.Add("2st Innings Team A 45 Over Figure");
                    Figure.Add("2st Innings Team A 50 Over Figure");
                    Figure.Add("2st Innings Team A 55 Over Figure");
                    Figure.Add("2st Innings Team A 60 Over Figure");
                    Figure.Add("2st Innings Team A 65 Over Figure");
                    Figure.Add("2st Innings Team A 70 Over Figure");
                    Figure.Add("2st Innings Team A 75 Over Figure");
                    Figure.Add("2st Innings Team A 80 Over Figure");
                    Figure.Add("2st Innings Team A 85 Over Figure");
                    Figure.Add("2st Innings Team A 90 Over Figure");
                    Figure.Add("2st Innings Team A 95 Over Figure");
                    Figure.Add("2st Innings Team A 100 Over Figure");
                    Figure.Add("2st Innings Team B 10 Over Figure");
                    Figure.Add("2st Innings Team B 15 Over Figure");
                    Figure.Add("2st Innings Team B 20 Over Figure");
                    Figure.Add("2st Innings Team B 25 Over Figure");
                    Figure.Add("2st Innings Team B 30 Over Figure");
                    Figure.Add("2st Innings Team B 35 Over Figure");
                    Figure.Add("2st Innings Team B 40 Over Figure");
                    Figure.Add("2st Innings Team B 45 Over Figure");
                    Figure.Add("2st Innings Team B 50 Over Figure");
                    Figure.Add("2st Innings Team B 55 Over Figure");
                    Figure.Add("2st Innings Team B 60 Over Figure");
                    Figure.Add("2st Innings Team B 65 Over Figure");
                    Figure.Add("2st Innings Team B 70 Over Figure");
                    Figure.Add("2st Innings Team B 75 Over Figure");
                    Figure.Add("2st Innings Team B 80 Over Figure");
                    Figure.Add("2st Innings Team B 85 Over Figure");
                    Figure.Add("2st Innings Team B 90 Over Figure");
                    Figure.Add("2st Innings Team B 95 Over Figure");
                    Figure.Add("2st Innings Team B 100 Over Figure");
                }

                List<string> FigureCatelogIDs = new List<string>();
                if (Figure.Count == 6)
                {
                    FigureCatelogIDs.Add("3." + (EventID + 05));
                    FigureCatelogIDs.Add("3." + (EventID + 15));
                    FigureCatelogIDs.Add("3." + (EventID + 25));
                    FigureCatelogIDs.Add("3." + (EventID + 35));
                    FigureCatelogIDs.Add("3." + (EventID + 45));
                    FigureCatelogIDs.Add("3." + (EventID + 55));

                }
                if (Figure.Count == 18)
                {
                    FigureCatelogIDs.Add("3." + (EventID + 05));
                    FigureCatelogIDs.Add("3." + (EventID + 15));
                    FigureCatelogIDs.Add("3." + (EventID + 25));
                    FigureCatelogIDs.Add("3." + (EventID + 35));
                    FigureCatelogIDs.Add("3." + (EventID + 45));
                    FigureCatelogIDs.Add("3." + (EventID + 55));
                    FigureCatelogIDs.Add("3." + (EventID + 65));
                    FigureCatelogIDs.Add("3." + (EventID + 75));
                    FigureCatelogIDs.Add("3." + (EventID + 85));
                    FigureCatelogIDs.Add("3." + (EventID + 95));
                    FigureCatelogIDs.Add("3." + (EventID + 105));
                    FigureCatelogIDs.Add("3." + (EventID + 115));
                    FigureCatelogIDs.Add("3." + (EventID + 125));
                    FigureCatelogIDs.Add("3." + (EventID + 135));
                    FigureCatelogIDs.Add("3." + (EventID + 145));
                    FigureCatelogIDs.Add("3." + (EventID + 155));

                }
                if (Figure.Count == 75)
                {
                    FigureCatelogIDs.Add("3." + (EventID + 05));
                    FigureCatelogIDs.Add("3." + (EventID + 15));
                    FigureCatelogIDs.Add("3." + (EventID + 25));
                    FigureCatelogIDs.Add("3." + (EventID + 35));
                    FigureCatelogIDs.Add("3." + (EventID + 45));
                    FigureCatelogIDs.Add("3." + (EventID + 55));
                    FigureCatelogIDs.Add("3." + (EventID + 65));
                    FigureCatelogIDs.Add("3." + (EventID + 75));
                    FigureCatelogIDs.Add("3." + (EventID + 85));
                    FigureCatelogIDs.Add("3." + (EventID + 95));
                    FigureCatelogIDs.Add("3." + (EventID + 105));
                    FigureCatelogIDs.Add("3." + (EventID + 115));
                    FigureCatelogIDs.Add("3." + (EventID + 125));
                    FigureCatelogIDs.Add("3." + (EventID + 135));
                    FigureCatelogIDs.Add("3." + (EventID + 145));
                    FigureCatelogIDs.Add("3." + (EventID + 155));
                    FigureCatelogIDs.Add("3." + (EventID + 165));
                    FigureCatelogIDs.Add("3." + (EventID + 175));
                    FigureCatelogIDs.Add("3." + (EventID + 185));
                    FigureCatelogIDs.Add("3." + (EventID + 195));
                    FigureCatelogIDs.Add("3." + (EventID + 205));
                    FigureCatelogIDs.Add("3." + (EventID + 215));
                    FigureCatelogIDs.Add("3." + (EventID + 225));
                    FigureCatelogIDs.Add("3." + (EventID + 235));
                    FigureCatelogIDs.Add("3." + (EventID + 245));
                    FigureCatelogIDs.Add("3." + (EventID + 255));
                    FigureCatelogIDs.Add("3." + (EventID + 265));
                    FigureCatelogIDs.Add("3." + (EventID + 275));
                    FigureCatelogIDs.Add("3." + (EventID + 285));
                    FigureCatelogIDs.Add("3." + (EventID + 295));
                    FigureCatelogIDs.Add("3." + (EventID + 305));
                    FigureCatelogIDs.Add("3." + (EventID + 315));
                    FigureCatelogIDs.Add("3." + (EventID + 325));
                    FigureCatelogIDs.Add("3." + (EventID + 335));
                    FigureCatelogIDs.Add("3." + (EventID + 345));
                    FigureCatelogIDs.Add("3." + (EventID + 355));
                    FigureCatelogIDs.Add("3." + (EventID + 365));
                    FigureCatelogIDs.Add("3." + (EventID + 375));
                    FigureCatelogIDs.Add("3." + (EventID + 385));
                    FigureCatelogIDs.Add("3." + (EventID + 395));
                    FigureCatelogIDs.Add("3." + (EventID + 405));
                    FigureCatelogIDs.Add("3." + (EventID + 415));
                    FigureCatelogIDs.Add("3." + (EventID + 425));
                    FigureCatelogIDs.Add("3." + (EventID + 435));
                    FigureCatelogIDs.Add("3." + (EventID + 445));
                    FigureCatelogIDs.Add("3." + (EventID + 455));
                    FigureCatelogIDs.Add("3." + (EventID + 465));
                    FigureCatelogIDs.Add("3." + (EventID + 475));
                    FigureCatelogIDs.Add("3." + (EventID + 485));
                    FigureCatelogIDs.Add("3." + (EventID + 495));
                    FigureCatelogIDs.Add("3." + (EventID + 505));
                    FigureCatelogIDs.Add("3." + (EventID + 515));
                    FigureCatelogIDs.Add("3." + (EventID + 525));
                    FigureCatelogIDs.Add("3." + (EventID + 535));
                    FigureCatelogIDs.Add("3." + (EventID + 545));
                    FigureCatelogIDs.Add("3." + (EventID + 555));
                    FigureCatelogIDs.Add("3." + (EventID + 565));
                    FigureCatelogIDs.Add("3." + (EventID + 575));
                    FigureCatelogIDs.Add("3." + (EventID + 585));
                    FigureCatelogIDs.Add("3." + (EventID + 595));
                    FigureCatelogIDs.Add("3." + (EventID + 605));
                    FigureCatelogIDs.Add("3." + (EventID + 615));
                    FigureCatelogIDs.Add("3." + (EventID + 625));
                    FigureCatelogIDs.Add("3." + (EventID + 635));
                    FigureCatelogIDs.Add("3." + (EventID + 645));
                    FigureCatelogIDs.Add("3." + (EventID + 655));
                    FigureCatelogIDs.Add("3." + (EventID + 665));
                    FigureCatelogIDs.Add("3." + (EventID + 675));
                    FigureCatelogIDs.Add("3." + (EventID + 685));
                    FigureCatelogIDs.Add("3." + (EventID + 695));
                    FigureCatelogIDs.Add("3." + (EventID + 705));
                    FigureCatelogIDs.Add("3." + (EventID + 715));
                    FigureCatelogIDs.Add("3." + (EventID + 725));
                    FigureCatelogIDs.Add("3." + (EventID + 735));
                    FigureCatelogIDs.Add("3." + (EventID + 745));
                }


                List<string> Figureselections = new List<string>();
                {
                    Figureselections.Add("0001");
                    Figureselections.Add("0002");
                    Figureselections.Add("0003");
                    Figureselections.Add("0004");
                    Figureselections.Add("0005");
                    Figureselections.Add("0006");
                    Figureselections.Add("0007");
                    Figureselections.Add("0008");
                    Figureselections.Add("0009");
                    Figureselections.Add("0011");
                }
                List<string> FigureselectionsName = new List<string>();
                {
                    FigureselectionsName.Add("0");
                    FigureselectionsName.Add("1");
                    FigureselectionsName.Add("2");
                    FigureselectionsName.Add("3");
                    FigureselectionsName.Add("4");
                    FigureselectionsName.Add("5");
                    FigureselectionsName.Add("6");
                    FigureselectionsName.Add("7");
                    FigureselectionsName.Add("8");
                    FigureselectionsName.Add("9");
                }
                List<int> lstUSers = new List<int>();
                foreach (var id in lstUSersAll)
                {
                    int IDs = id.ID;
                    lstUSers.Add(IDs);
                }

                objUsersServiceCleint.InsertUserMarketFigure(Figure.ToArray(), FigureCatelogIDs.ToArray(), Figureselections.ToArray(), FigureselectionsName.ToArray(), lstUSers.ToArray(), EventID, LoggedinUserDetail.GetUserID());

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        //public void Getmarket()
        //{
        //    List<string> kalijutt = new List<string>();
        //    List<string> KaliCatelogIDs = new List<string>();

        //    kalijutt.Add("1st Innings 5 Over Kali");
        //    kalijutt.Add("1st Innings 10 Over Kali");
        //    kalijutt.Add("1st Innings 15 Over Kali");
        //    kalijutt.Add("1st Innings 20 Over Kali");
        //    kalijutt.Add("1st Innings 25 Over Kali");
        //    kalijutt.Add("1st Innings 30 Over Kali");
        //    kalijutt.Add("1st Innings 35 Over Kali");
        //    kalijutt.Add("1st Innings 40 Over Kali");
        //    kalijutt.Add("1st Innings 45 Over Kali");
        //    kalijutt.Add("1st Innings 50 Over Kali");
        //    kalijutt.Add("2st Innings 5 Over Kali");
        //    kalijutt.Add("2st Innings 10 Over Kali");
        //    kalijutt.Add("2st Innings 15 Over Kali");
        //    kalijutt.Add("2st Innings 20Over Kali");
        //    kalijutt.Add("2st Innings 25 Over Kali");
        //    kalijutt.Add("2st Innings 30Over Kali");
        //    kalijutt.Add("2st Innings 35 Over Kali");
        //    kalijutt.Add("2st Innings 40Over Kali");

        //    //

        //    KaliCatelogIDs.Add("3." + (EventID + 10));
        //    KaliCatelogIDs.Add("3." + (EventID + 20));
        //    KaliCatelogIDs.Add("3." + (EventID + 30));
        //    KaliCatelogIDs.Add("3." + (EventID + 40));
        //    KaliCatelogIDs.Add("3." + (EventID + 50));
        //    KaliCatelogIDs.Add("3." + (EventID + 60));
        //    KaliCatelogIDs.Add("3." + (EventID + 70));
        //    KaliCatelogIDs.Add("3." + (EventID + 80));
        //    KaliCatelogIDs.Add("3." + (EventID + 90));
        //    KaliCatelogIDs.Add("3." + (EventID + 100));
        //    KaliCatelogIDs.Add("3." + (EventID + 110));
        //    KaliCatelogIDs.Add("3." + (EventID + 120));
        //    KaliCatelogIDs.Add("3." + (EventID + 130));
        //    KaliCatelogIDs.Add("3." + (EventID + 140));
        //    KaliCatelogIDs.Add("3." + (EventID + 150));
        //    KaliCatelogIDs.Add("3." + (EventID + 160));

        //}
        private void Button_Click_28(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> userIDs = cmbCustomerAllowedMarketskalijutt.SelectedValue.ToString().Split(',').Select(int.Parse).ToList();

                List<SP_Users_GetUserbyCreatedbyID_Result> lstUSersAll = new List<SP_Users_GetUserbyCreatedbyID_Result>();
                foreach (int id in userIDs)
                {
                    SP_Users_GetUserbyCreatedbyID_Result s = new SP_Users_GetUserbyCreatedbyID_Result();
                    int r = Convert.ToInt32(id);
                    s.ID = r;
                    lstUSersAll.AddRange(JsonConvert.DeserializeObject<List<SP_Users_GetUserbyCreatedbyID_Result>>(objUsersServiceCleint.getlistuserids(r)).ToList());
                    lstUSersAll.Add(s);
                }

                List<string> allusersmarket = new List<string>();
                //  List<int> cardValue = lstUSers.ToString().Select().to;

                string EventID = cmbEvents.SelectedValue.ToString();
                List<SP_UserMarket_GetUserMarketbyEventID1_Result> lstkalijuttmarket = new List<SP_UserMarket_GetUserMarketbyEventID1_Result>();
                lstkalijuttmarket = JsonConvert.DeserializeObject<List<SP_UserMarket_GetUserMarketbyEventID1_Result>>(objUsersServiceCleint.GetMarketbyEventID1(cmbEvents.SelectedValue.ToString()));

                List<string> kalijutt = new List<string>();
                // Add items using Add method      
                if (cmbkalijut.SelectedIndex == 0)
                {
                    kalijutt.Add("1st Innings 5 Over Kali");
                    kalijutt.Add("1st Innings 10 Over Kali");
                    kalijutt.Add("1st Innings 15 Over Kali");
                    kalijutt.Add("1st Innings 20 Over Kali");
                    kalijutt.Add("2st Innings 5 Over Kali");
                    kalijutt.Add("2st Innings 10 Over Kali");
                }

                if (cmbkalijut.SelectedIndex == 1)
                {
                    kalijutt.Add("1st Innings 5 Over Kali");
                    kalijutt.Add("1st Innings 10 Over Kali");
                    kalijutt.Add("1st Innings 15 Over Kali");
                    kalijutt.Add("1st Innings 20 Over Kali");
                    kalijutt.Add("1st Innings 25 Over Kali");
                    kalijutt.Add("1st Innings 30 Over Kali");
                    kalijutt.Add("1st Innings 35 Over Kali");
                    kalijutt.Add("1st Innings 40 Over Kali");
                    kalijutt.Add("1st Innings 45 Over Kali");
                    kalijutt.Add("1st Innings 50 Over Kali");
                    kalijutt.Add("2st Innings 5 Over Kali");
                    kalijutt.Add("2st Innings 10 Over Kali");
                    kalijutt.Add("2st Innings 15 Over Kali");
                    kalijutt.Add("2st Innings 20Over Kali");
                    kalijutt.Add("2st Innings 25 Over Kali");
                    kalijutt.Add("2st Innings 30Over Kali");
                    kalijutt.Add("2st Innings 35 Over Kali");
                    kalijutt.Add("2st Innings 40Over Kali");
                }

                if (cmbkalijut.SelectedIndex == 2)
                {
                    kalijutt.Add("1st Innings Team A 10 Over Kali");
                    kalijutt.Add("1st Innings Team A 15 Over Kali");
                    kalijutt.Add("1st Innings Team A 20 Over Kali");
                    kalijutt.Add("1st Innings Team A 25 Over Kali");
                    kalijutt.Add("1st Innings Team A 30 Over Kali");
                    kalijutt.Add("1st Innings Team A 35 Over Kali");
                    kalijutt.Add("1st Innings Team A 40 Over Kali");
                    kalijutt.Add("1st Innings Team A 45 Over Kali");
                    kalijutt.Add("1st Innings Team A 50 Over Kali");
                    kalijutt.Add("1st Innings Team A 55 Over Kali");
                    kalijutt.Add("1st Innings Team A 60 Over Kali");
                    kalijutt.Add("1st Innings Team A 70 Over Kali");
                    kalijutt.Add("1st Innings Team A 75 Over Kali");
                    kalijutt.Add("1st Innings Team A 80 Over Kali");
                    kalijutt.Add("1st Innings Team A 85 Over Kali");
                    kalijutt.Add("1st Innings Team A 90 Over Kali");
                    kalijutt.Add("1st Innings Team A 95 Over Kali");
                    kalijutt.Add("1st Innings Team A 100 Over Kali");
                    kalijutt.Add("1st Innings Team B 10 Over Kali");
                    kalijutt.Add("1st Innings Team B 15 Over Kali");
                    kalijutt.Add("1st Innings Team B 20 Over Kali");
                    kalijutt.Add("1st Innings Team B 25 Over Kali");
                    kalijutt.Add("1st Innings Team B 30 Over Kali");
                    kalijutt.Add("1st Innings Team B 35 Over Kalie");
                    kalijutt.Add("1st Innings Team B 40 Over Kali");
                    kalijutt.Add("1st Innings Team B 45 Over Kali");
                    kalijutt.Add("1st Innings Team B 50 Over Kali");
                    kalijutt.Add("1st Innings Team B 55 Over Kali");
                    kalijutt.Add("1st Innings Team B 60 Over Kali");
                    kalijutt.Add("1st Innings Team B 65 Over Kali");
                    kalijutt.Add("1st Innings Team B 70 Over Kali");
                    kalijutt.Add("1st Innings Team B 75 Over Kali");
                    kalijutt.Add("1st Innings Team B 80 Over Kali");
                    kalijutt.Add("1st Innings Team B 85 Over Kali");
                    kalijutt.Add("1st Innings Team B 90 Over Kali");
                    kalijutt.Add("1st Innings Team B 95 Over Kali");
                    kalijutt.Add("1st Innings Team B 100 Over Kali");
                    kalijutt.Add("2st Innings Team A 10 Over Kali");
                    kalijutt.Add("2st Innings Team A 15 Over Kali");
                    kalijutt.Add("2st Innings Team A 20 Over Kali");
                    kalijutt.Add("2st Innings Team A 25 Over Kali");
                    kalijutt.Add("2st Innings Team A 30 Over Kali");
                    kalijutt.Add("2st Innings Team A 35 Over Kali");
                    kalijutt.Add("2st Innings Team A 40 Over Kali");
                    kalijutt.Add("2st Innings Team A 45 Over Kali");
                    kalijutt.Add("2st Innings Team A 50 Over Kali");
                    kalijutt.Add("2st Innings Team A 55 Over Kali");
                    kalijutt.Add("2st Innings Team A 60 Over Kalie");
                    kalijutt.Add("2st Innings Team A 65 Over Kali");
                    kalijutt.Add("2st Innings Team A 70 Over Kali");
                    kalijutt.Add("2st Innings Team A 75 Over Kali");
                    kalijutt.Add("2st Innings Team A 80 Over Kali");
                    kalijutt.Add("2st Innings Team A 85 Over Kali");
                    kalijutt.Add("2st Innings Team A 90 Over Kali");
                    kalijutt.Add("2st Innings Team A 95 Over Kali");
                    kalijutt.Add("2st Innings Team A 100 Over Kali");
                    kalijutt.Add("2st Innings Team B 10 Over Kali");
                    kalijutt.Add("2st Innings Team B 15 Over Kali");
                    kalijutt.Add("2st Innings Team B 20 Over Kali");
                    kalijutt.Add("2st Innings Team B 25 Over Kali");
                    kalijutt.Add("2st Innings Team B 30 Over Kali");
                    kalijutt.Add("2st Innings Team B 35 Over Kali");
                    kalijutt.Add("2st Innings Team B 40 Over Kali");
                    kalijutt.Add("2st Innings Team B 45 Over Kali");
                    kalijutt.Add("2st Innings Team B 50 Over Kali");
                    kalijutt.Add("2st Innings Team B 55 Over Kali");
                    kalijutt.Add("2st Innings Team B 60 Over Kali");
                    kalijutt.Add("2st Innings Team B 65 Over Kali");
                    kalijutt.Add("2st Innings Team B 70 Over Kali");
                    kalijutt.Add("2st Innings Team B 75 Over Kali");
                    kalijutt.Add("2st Innings Team B 80 Over Kali");
                    kalijutt.Add("2st Innings Team B 85 Over Kali");
                    kalijutt.Add("2st Innings Team B 90 Over Kali");
                    kalijutt.Add("2st Innings Team B 95 Over Kali");
                    kalijutt.Add("2st Innings Team B 100 Over Kali");
                }

                List<string> KaliCatelogIDs = new List<string>();

                if (kalijutt.Count == 6)
                {
                    KaliCatelogIDs.Add("3." + (EventID + 10));
                    KaliCatelogIDs.Add("3." + (EventID + 20));
                    KaliCatelogIDs.Add("3." + (EventID + 30));
                    KaliCatelogIDs.Add("3." + (EventID + 40));
                    KaliCatelogIDs.Add("3." + (EventID + 50));
                    KaliCatelogIDs.Add("3." + (EventID + 60));
                }
                if (kalijutt.Count == 18)
                {
                    KaliCatelogIDs.Add("3." + (EventID + 10));
                    KaliCatelogIDs.Add("3." + (EventID + 20));
                    KaliCatelogIDs.Add("3." + (EventID + 30));
                    KaliCatelogIDs.Add("3." + (EventID + 40));
                    KaliCatelogIDs.Add("3." + (EventID + 50));
                    KaliCatelogIDs.Add("3." + (EventID + 60));
                    KaliCatelogIDs.Add("3." + (EventID + 70));
                    KaliCatelogIDs.Add("3." + (EventID + 80));
                    KaliCatelogIDs.Add("3." + (EventID + 90));
                    KaliCatelogIDs.Add("3." + (EventID + 100));
                    KaliCatelogIDs.Add("3." + (EventID + 110));
                    KaliCatelogIDs.Add("3." + (EventID + 120));
                    KaliCatelogIDs.Add("3." + (EventID + 130));
                    KaliCatelogIDs.Add("3." + (EventID + 140));
                    KaliCatelogIDs.Add("3." + (EventID + 150));
                    KaliCatelogIDs.Add("3." + (EventID + 160));
                }
                if (kalijutt.Count == 75)
                {
                    KaliCatelogIDs.Add("3." + (EventID + 10));
                    KaliCatelogIDs.Add("3." + (EventID + 20));
                    KaliCatelogIDs.Add("3." + (EventID + 30));
                    KaliCatelogIDs.Add("3." + (EventID + 40));
                    KaliCatelogIDs.Add("3." + (EventID + 50));
                    KaliCatelogIDs.Add("3." + (EventID + 60));
                    KaliCatelogIDs.Add("3." + (EventID + 70));
                    KaliCatelogIDs.Add("3." + (EventID + 80));
                    KaliCatelogIDs.Add("3." + (EventID + 90));
                    KaliCatelogIDs.Add("3." + (EventID + 100));
                    KaliCatelogIDs.Add("3." + (EventID + 110));
                    KaliCatelogIDs.Add("3." + (EventID + 120));
                    KaliCatelogIDs.Add("3." + (EventID + 130));
                    KaliCatelogIDs.Add("3." + (EventID + 140));
                    KaliCatelogIDs.Add("3." + (EventID + 150));
                    KaliCatelogIDs.Add("3." + (EventID + 160));
                    KaliCatelogIDs.Add("3." + (EventID + 170));
                    KaliCatelogIDs.Add("3." + (EventID + 180));
                    KaliCatelogIDs.Add("3." + (EventID + 190));
                    KaliCatelogIDs.Add("3." + (EventID + 200));
                    KaliCatelogIDs.Add("3." + (EventID + 210));
                    KaliCatelogIDs.Add("3." + (EventID + 220));
                    KaliCatelogIDs.Add("3." + (EventID + 230));
                    KaliCatelogIDs.Add("3." + (EventID + 240));
                    KaliCatelogIDs.Add("3." + (EventID + 250));
                    KaliCatelogIDs.Add("3." + (EventID + 260));
                    KaliCatelogIDs.Add("3." + (EventID + 270));
                    KaliCatelogIDs.Add("3." + (EventID + 280));
                    KaliCatelogIDs.Add("3." + (EventID + 290));
                    KaliCatelogIDs.Add("3." + (EventID + 300));
                    KaliCatelogIDs.Add("3." + (EventID + 310));
                    KaliCatelogIDs.Add("3." + (EventID + 320));
                    KaliCatelogIDs.Add("3." + (EventID + 330));
                    KaliCatelogIDs.Add("3." + (EventID + 340));
                    KaliCatelogIDs.Add("3." + (EventID + 350));
                    KaliCatelogIDs.Add("3." + (EventID + 360));
                    KaliCatelogIDs.Add("3." + (EventID + 370));
                    KaliCatelogIDs.Add("3." + (EventID + 380));
                    KaliCatelogIDs.Add("3." + (EventID + 390));
                    KaliCatelogIDs.Add("3." + (EventID + 400));
                    KaliCatelogIDs.Add("3." + (EventID + 410));
                    KaliCatelogIDs.Add("3." + (EventID + 420));
                    KaliCatelogIDs.Add("3." + (EventID + 330));
                    KaliCatelogIDs.Add("3." + (EventID + 440));
                    KaliCatelogIDs.Add("3." + (EventID + 450));
                    KaliCatelogIDs.Add("3." + (EventID + 460));
                    KaliCatelogIDs.Add("3." + (EventID + 470));
                    KaliCatelogIDs.Add("3." + (EventID + 480));
                    KaliCatelogIDs.Add("3." + (EventID + 490));
                    KaliCatelogIDs.Add("3." + (EventID + 500));
                    KaliCatelogIDs.Add("3." + (EventID + 510));
                    KaliCatelogIDs.Add("3." + (EventID + 520));
                    KaliCatelogIDs.Add("3." + (EventID + 530));
                    KaliCatelogIDs.Add("3." + (EventID + 540));
                    KaliCatelogIDs.Add("3." + (EventID + 550));
                    KaliCatelogIDs.Add("3." + (EventID + 560));
                    KaliCatelogIDs.Add("3." + (EventID + 570));
                    KaliCatelogIDs.Add("3." + (EventID + 580));
                    KaliCatelogIDs.Add("3." + (EventID + 590));
                    KaliCatelogIDs.Add("3." + (EventID + 600));
                    KaliCatelogIDs.Add("3." + (EventID + 610));
                    KaliCatelogIDs.Add("3." + (EventID + 620));
                    KaliCatelogIDs.Add("3." + (EventID + 630));
                    KaliCatelogIDs.Add("3." + (EventID + 640));
                    KaliCatelogIDs.Add("3." + (EventID + 650));
                    KaliCatelogIDs.Add("3." + (EventID + 660));
                    KaliCatelogIDs.Add("3." + (EventID + 670));
                    KaliCatelogIDs.Add("3." + (EventID + 680));
                    KaliCatelogIDs.Add("3." + (EventID + 690));
                    KaliCatelogIDs.Add("3." + (EventID + 700));
                    KaliCatelogIDs.Add("3." + (EventID + 710));
                    KaliCatelogIDs.Add("3." + (EventID + 720));
                    KaliCatelogIDs.Add("3." + (EventID + 730));
                    KaliCatelogIDs.Add("3." + (EventID + 740));
                    KaliCatelogIDs.Add("3." + (EventID + 750));
                }

                List<string> selections = new List<string>();
                {
                    selections.Add("369646");
                    selections.Add("Kali");
                    //selectionIDs.Add("543555");
                }

                List<int> lstUSers = new List<int>();
                foreach (var id in lstUSersAll)
                {
                    int IDs = id.ID;
                    lstUSers.Add(IDs);
                }
                objUsersServiceCleint.InsertUserMarketKJ(kalijutt.ToArray(), KaliCatelogIDs.ToArray(), selections.ToArray(), lstUSers.ToArray(), EventID, LoggedinUserDetail.GetUserID());
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void Button_Click_30(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> userIDs = cmbCustomerAllowedMarketskalijutt.SelectedValue.ToString().Split(',').Select(int.Parse).ToList();

                List<SP_Users_GetUserbyCreatedbyID_Result> lstUSersAll = new List<SP_Users_GetUserbyCreatedbyID_Result>();
                foreach (int id in userIDs)
                {
                    SP_Users_GetUserbyCreatedbyID_Result s = new SP_Users_GetUserbyCreatedbyID_Result();
                    int r = Convert.ToInt32(id);
                    s.ID = r;
                    lstUSersAll.AddRange(JsonConvert.DeserializeObject<List<SP_Users_GetUserbyCreatedbyID_Result>>(objUsersServiceCleint.getlistuserids(r)).ToList());
                    lstUSersAll.Add(s);
                }

                List<string> allusersmarket = new List<string>();
                //  List<int> cardValue = lstUSers.ToString().Select().to;

                string EventID = cmbEvents.SelectedValue.ToString();
                List<SP_UserMarket_GetUserMarketbyEventID1_Result> lstkalijuttmarket = new List<SP_UserMarket_GetUserMarketbyEventID1_Result>();
                lstkalijuttmarket = JsonConvert.DeserializeObject<List<SP_UserMarket_GetUserMarketbyEventID1_Result>>(objUsersServiceCleint.GetMarketbyEventID1(cmbEvents.SelectedValue.ToString()));

                List<string> SmallFig = new List<string>();
                // Add items using Add method      
                if (cmbkalijut.SelectedIndex == 0)
                {
                    SmallFig.Add("1st Innings 5 Over 0-to-4");
                    SmallFig.Add("1st Innings 10 Over 0-to-4");
                    SmallFig.Add("1st Innings 15 Over 0-to-4");
                    SmallFig.Add("1st Innings 20 Over 0-to-4");
                    SmallFig.Add("2st Innings 5 Over 0-to-4");
                    SmallFig.Add("2st Innings 10 Over 0-to-4");
                }

                if (cmbkalijut.SelectedIndex == 1)
                {
                    SmallFig.Add("1st Innings 5 Over 0-to-4");
                    SmallFig.Add("1st Innings 10 Over 0-to-4");
                    SmallFig.Add("1st Innings 15 Over 0-to-4");
                    SmallFig.Add("1st Innings 20 Over 0-to-4");
                    SmallFig.Add("1st Innings 25 Over 0-to-4");
                    SmallFig.Add("1st Innings 30 Over 0-to-4");
                    SmallFig.Add("1st Innings 35 Over 0-to-4");
                    SmallFig.Add("1st Innings 40 Over 0-to-4");
                    SmallFig.Add("1st Innings 45 Over 0-to-4");
                    SmallFig.Add("1st Innings 50 Over 0-to-4");
                    SmallFig.Add("2st Innings 5 Over 0-to-4");
                    SmallFig.Add("2st Innings 10 Over 0-to-4");
                    SmallFig.Add("2st Innings 15 Over 0-to-4");
                    SmallFig.Add("2st Innings 20Over 0-to-4");
                    SmallFig.Add("2st Innings 25 Over 0-to-4");
                    SmallFig.Add("2st Innings 30Over 0-to-4");
                    SmallFig.Add("2st Innings 35 Over 0-to-4");
                    SmallFig.Add("2st Innings 40Over 0-to-4");
                }

                List<string> smallFigCatelogIDs = new List<string>();

                if (SmallFig.Count == 6)
                {
                    smallFigCatelogIDs.Add("3." + (EventID + 3));
                    smallFigCatelogIDs.Add("3." + (EventID + 6));
                    smallFigCatelogIDs.Add("3." + (EventID + 9));
                    smallFigCatelogIDs.Add("3." + (EventID + 12));
                    smallFigCatelogIDs.Add("3." + (EventID + 16));
                    smallFigCatelogIDs.Add("3." + (EventID + 19));
                }
                if (SmallFig.Count == 18)
                {
                    smallFigCatelogIDs.Add("3." + (EventID + 3));
                    smallFigCatelogIDs.Add("3." + (EventID + 6));
                    smallFigCatelogIDs.Add("3." + (EventID + 9));
                    smallFigCatelogIDs.Add("3." + (EventID + 12));
                    smallFigCatelogIDs.Add("3." + (EventID + 16));
                    smallFigCatelogIDs.Add("3." + (EventID + 19));
                    smallFigCatelogIDs.Add("3." + (EventID + 21));
                    smallFigCatelogIDs.Add("3." + (EventID + 23));
                    smallFigCatelogIDs.Add("3." + (EventID + 26));
                    smallFigCatelogIDs.Add("3." + (EventID + 29));
                    smallFigCatelogIDs.Add("3." + (EventID + 31));
                    smallFigCatelogIDs.Add("3." + (EventID + 33));
                    smallFigCatelogIDs.Add("3." + (EventID + 36));
                    smallFigCatelogIDs.Add("3." + (EventID + 39));
                    smallFigCatelogIDs.Add("3." + (EventID + 41));
                    smallFigCatelogIDs.Add("3." + (EventID + 43));
                }


                List<string> selections = new List<string>();
                {
                    selections.Add("3121");
                    selections.Add("SmallFig");
                }

                List<int> lstUSers = new List<int>();
                foreach (var id in lstUSersAll)
                {
                    int IDs = id.ID;
                    lstUSers.Add(IDs);
                }
                objUsersServiceCleint.InsertUserMarketSFig(SmallFig.ToArray(), smallFigCatelogIDs.ToArray(), selections.ToArray(), lstUSers.ToArray(), EventID, LoggedinUserDetail.GetUserID());
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

        private void btnUpdateTransferAgentCommision_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(cmbUsers.SelectedValue) > 0 && LoggedinUserDetail.GetUserTypeID() == 1)
                {


                    UserIDandUserType objSelectedUser = (UserIDandUserType)cmbUsers.SelectedItem;
                    if (objSelectedUser.UserTypeID == 2)
                    {
                        objUsersServiceCleint.UpdateTransferAgnetCommision(Convert.ToInt32(cmbUsers.SelectedValue), chkTransferAgentCommision.IsChecked.Value);
                        Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
                    }
                    else
                    {

                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void Button_Click_24(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbEvents.SelectedIndex > -1)
                {
                    objUsersServiceCleint.UpdateFancySyncONorOFF(73, cmbEvents.SelectedValue.ToString(), chkSTartFancySync.IsChecked.Value);
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void cmbEventsforMarketAllowed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbEventsforMarketAllowed.SelectedValue is string)
                {
                    if (cmbEventsforMarketAllowed.SelectedIndex > 0)
                    {


                        List<GetMarketForAllowedBetting_Result> lstMarketsAllowedforBetSelected = lstMarketsAllowedforBet.Where(item => item.EventID == cmbEventsforMarketAllowed.SelectedValue.ToString()).ToList();
                        dgvAllowedMarkets.ItemsSource = lstMarketsAllowedforBetSelected;
                    }
                    else
                    {
                        FilterMarkets();
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void btnUpdateMaxBalanceTransferLimit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    if (txtMaxBalanceTransferLimit.Text.Length > 0)
                    {
                        objUsersServiceCleint.UpdateMaxBalanceTransferLimit(Convert.ToInt32(cmbUsers.SelectedValue), Convert.ToInt32(txtMaxBalanceTransferLimit.Text));
                        Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully.");
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {


            }
        }

        private void btnUpdateMaxAgentRate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    if (txtMaxAgentRate.Text.Length > 0)
                    {
                        objUsersServiceCleint.UpdateMaxAgentRate(Convert.ToInt32(cmbUsers.SelectedValue), Convert.ToInt32(txtMaxAgentRate.Text));
                        Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully.");
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {


            }
        }

        private void Button_Click_25(object sender, RoutedEventArgs e)
        {
            try
            {
                var a = lstNewRefferes.Sum(x => x.ReferrerRate);
                UserDetails objuserDetails = getuserdetails(Convert.ToInt32(cmbCustomersForhissa.SelectedValue));
                int RatePercent = Convert.ToInt32(objuserDetails.RatePercent);
                int inputrate = Convert.ToInt32(txtReferrehisasa.Text);
                int totalgivereffer = Convert.ToInt32(a) + inputrate;

                if (inputrate <= RatePercent && txtReferrehisasa.Text != "" && totalgivereffer <= RatePercent)
                {
                    if (cmbCustomersForhissa.SelectedIndex > 0 && cmbReferrerUserhissa.SelectedIndex > 0)
                    {
                        GetReferrerRateandReferrerIDbyUserID_Result objReffereall = new GetReferrerRateandReferrerIDbyUserID_Result();

                        var refferer = lstNewRefferes.Where(item => item.ReferrerID == Convert.ToInt32(cmbReferrerUserhissa.SelectedValue)).FirstOrDefault();
                        if (refferer != null)
                        {
                            refferer.ReferrerRate = Convert.ToInt32(txtReferrehisasa.Text);
                            DGVHisaPuntershissa.ItemsSource = null;
                            DGVHisaPuntershissa.Items.Clear();
                            DGVHisaPuntershissa.ItemsSource = lstNewRefferes;
                            return;
                        }
                        else
                        {
                            GetReferrerRateandReferrerIDbyUserID_Result objReffere = new GetReferrerRateandReferrerIDbyUserID_Result();
                            objReffere.ReferrerID = Convert.ToInt32(cmbReferrerUserhissa.SelectedValue.ToString());
                            objReffere.RefferrerName = cmbReferrerUserhissa.Text;
                            objReffere.ReferrerRate = Convert.ToInt32(txtReferrehisasa.Text);
                            lstNewRefferes.Add(objReffere);
                            DGVHisaPuntershissa.ItemsSource = null;
                            DGVHisaPuntershissa.Items.Clear();
                            DGVHisaPuntershissa.ItemsSource = lstNewRefferes;

                        }
                    }
                }
                else
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Hissa Limts Increase Max Limt Is your Agent Rate ");
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

        }

        private void DGVHisaPuntersagent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
        private void cmbCustomersForHissa()
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 1 || LoggedinUserDetail.GetUserTypeID() == 2)
                {

                    // lblcount.Content = 0;
                    if (cmbCustomersForhissa.SelectedValue is int)
                    {
                        //bfnexchange.Services.DBModel.SP_Users_GetReferrerRateandReferrerIDbyUserID_Result objRefferIDandRate = objUsersServiceCleint.GetReferrerRateandIDbyUserID(Convert.ToInt32(cmbCustomersForLimits.SelectedValue));
                        //if (objRefferIDandRate.ReferrerID > 0)
                        //{
                        //    txtReferreRate.Text = objRefferIDandRate.ReferrerRate.ToString();
                        //    cmbReferrerUser.SelectedValue = objRefferIDandRate.ReferrerID;
                        //}
                        //else
                        //{
                        //    txtReferreRate.Text = "0";
                        //    cmbReferrerUser.SelectedValue = 0;
                        //}
                        var lstRefferrers = objUsersServiceCleint.GetReferrerRatesbyUserID(Convert.ToInt32(cmbCustomersForhissa.SelectedValue));

                        DGVHisaPuntershissa.ItemsSource = null;
                        lstNewRefferes = new List<GetReferrerRateandReferrerIDbyUserID_Result>();
                        if (lstRefferrers.Count() > 0)
                        {
                            foreach (var item in lstRefferrers)
                            {
                                cmbReferrerUserhissa.SelectedValue = item.ReferrerID;
                                GetReferrerRateandReferrerIDbyUserID_Result objReffere = new GetReferrerRateandReferrerIDbyUserID_Result();
                                objReffere.ReferrerID = item.ReferrerID;
                                objReffere.RefferrerName = LoggedinUserDetail.AllUsers.Where(item1 => item1.ID == item.ReferrerID).Select(item1 => new { item1.UserName }).FirstOrDefault().UserName.ToString();
                                objReffere.ReferrerRate = item.ReferrerRate;
                                lstNewRefferes.Add(objReffere);
                                // lstNewRefferes.Sum(x => x.ReferrerRate);



                            }
                            DGVHisaPuntershissa.ItemsSource = lstNewRefferes;
                            //lblcount.Content=lstNewRefferes.Sum(x => x.ReferrerRate);                           
                        }
                        else
                        {

                        }
                        UserDetails objuserDetails = getuserdetails(Convert.ToInt32(cmbCustomersForhissa.SelectedValue));
                        //   lblmaxrate.Content = objuserDetails.RatePercent;
                        // lblmaxrate.Content = objUsersServiceCleint.GetMaxAgentRate(Convert.ToInt32(cmbUsers.SelectedValue)).ToString();


                        //txtCommissionRateUpdate.Text = objuserDetails.CommissionRate.ToString();
                        //txtCommissionRateFancyUpdate.Text = objUsersServiceCleint.GetCommissionRatebyUserIDFancy(Convert.ToInt32(cmbCustomersForLimitsagent.SelectedValue)).ToString();
                        //chkSoccerForView.IsChecked = objuserDetails.isSoccerAllowed;
                        //chkTennisForView.IsChecked = objuserDetails.isTennisAllowed;
                        //chkGrayHoundForView.IsChecked = objuserDetails.isGrayHoundRaceAllowed;
                        //chkHorseRaceAllowedForViewMarket.IsChecked = objuserDetails.isHorseRaceAllowed;
                        ////Cricket
                        //txtMatchOddsLower.Text = objuserDetails.BetLowerLimitMatchOdds.ToString("N0");
                        //txtMatchOddsUpper.Text = objuserDetails.BetUpperLimitMatchOdds.ToString("N0");
                        //txtTiedMatchLower.Text = objuserDetails.BetLowerLimitTiedMatch.ToString("N0");
                        //txtTiedMatchUpper.Text = objuserDetails.BetUpperLimitTiedMatch.ToString("N0");
                        //txtCompletedLower.Text = objuserDetails.BetLowerLimitCompletedMatch.ToString("N0");
                        //txtCompletedUpper.Text = objuserDetails.BetUpperLimitCompletedMatch.ToString("N0");
                        //txtInnsRunsLower.Text = objuserDetails.BetLowerLimitInningRuns.ToString("N0");
                        //txtInnsRunsUpper.Text = objuserDetails.BetUpperLimitInningRuns.ToString("N0");
                        //txtWinnerLower.Text = objuserDetails.BetLowerLimitWinner.ToString("N0");
                        //txtWinnerUpper.Text = objuserDetails.BetUpperLimitWinner.ToString("N0");
                        //txtFancyLower.Text = objuserDetails.BetLowerLimitFancy.ToString("N0");
                        //txtFancyUpper.Text = objuserDetails.BetUpperLimitFancy.ToString("N0");
                        ////Horse Racing
                        //txtHorsePlaceLower.Text = objuserDetails.BetLowerLimitHorsePlace.ToString("N0");
                        //txtHorsePlaceUpper.Text = objuserDetails.BetUpperLimitHorsePlace.ToString("N0");
                        //txtHorseWinLower.Text = objuserDetails.BetLowerLimit.ToString("N0");
                        //txtHorseWinUpper.Text = objuserDetails.BetUpperLimit.ToString("N0");
                        ////GrayHound Racing
                        //txtGrayHoundPlaceLower.Text = objuserDetails.BetLowerLimitGrayHoundPlace.ToString("N0");
                        //txtGrayHoundPlaceUpper.Text = objuserDetails.BetUpperLimitGrayHoundPlace.ToString("N0");
                        //txtGrayHoundWinLower.Text = objuserDetails.BetLowerLimitGrayHoundWin.ToString("N0");
                        //txtGrayHoundWinUpper.Text = objuserDetails.BetUpperLimitGrayHoundWin.ToString("N0");
                        ////Soccer & Tennis
                        //txtSoccerLower.Text = objuserDetails.BetLowerLimitMatchOddsSoccer.ToString("N0");
                        //txtSoccerUpper.Text = objuserDetails.BetUpperLimitMatchOddsSoccer.ToString("N0");
                        //txtTennisLower.Text = objuserDetails.BetLowerLimitMatchOddsTennis.ToString("N0");
                        //txtTennisUpper.Text = objuserDetails.BetUpperLimitMatchOddsTennis.ToString("N0");
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }






        private void Button_Click_26(object sender, RoutedEventArgs e)
        {
            UserDetails objuserDetails = getuserdetails(Convert.ToInt32(cmbCustomersForhissa.SelectedValue));
            var agentrate1 = objuserDetails.RatePercent;
            var lstRefferrers = objUsersServiceCleint.GetReferrerRatesbyUserID(Convert.ToInt32(cmbCustomersForhissa.SelectedValue));
            //decimal sumofrafrates = 0;
            //foreach (dgrow row in DGVHisaPuntershissa.ItemsSource)
            //{
            //    sumofrafrates += (decimal)row["tutar"];
            //}
            if (LoggedinUserDetail.GetUserTypeID() == 1 || LoggedinUserDetail.GetUserTypeID() == 2)
            {

                if (Convert.ToInt32(cmbCustomersForhissa.SelectedValue) > 0)
                {
                    objUsersServiceCleint.DeletReffererUSers(Convert.ToInt32(cmbCustomersForhissa.SelectedValue));
                }

                if (DGVHisaPuntershissa.Items.Count > 0)
                {

                    foreach (GetReferrerRateandReferrerIDbyUserID_Result dgRow in DGVHisaPuntershissa.Items)
                    {
                        objUsersServiceCleint.AddReferrerUsers(Convert.ToInt32(cmbCustomersForhissa.SelectedValue), Convert.ToInt32(dgRow.ReferrerID), Convert.ToInt32(dgRow.ReferrerRate));
                    }

                }
                Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
                cmbCustomersForLimitsLoad();
            }
        }

        private void DGVHisaPuntershissa_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (DGVHisaPuntershissa.CurrentCell.Column.DisplayIndex == 3)
                {
                    GetReferrerRateandReferrerIDbyUserID_Result objReffer = (GetReferrerRateandReferrerIDbyUserID_Result)DGVHisaPuntershissa.SelectedItem;
                    lstNewRefferes.Remove(objReffer);
                    DGVHisaPuntershissa.ItemsSource = null;
                    DGVHisaPuntershissa.Items.Clear();
                    DGVHisaPuntershissa.ItemsSource = lstNewRefferes;
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }
        }

        private void cmbCustomersForhissa_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            cmbCustomersForHissa();
        }
        public Service123Client objBettingClient = new Service123Client();

        private void Button_Click_32(object sender, RoutedEventArgs e)
        {
            try
            {
                List<ExternalAPI.TO.MarketBookForindianFancy> allmarkets = new List<ExternalAPI.TO.MarketBookForindianFancy>();
                List<ExternalAPI.TO.RunnerForIndianFancy> runners = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                allmarkets.Clear();
                allmarkets.Add(objBettingClient.GetMarketDatabyIDIndianFancy(EventID, marketbookid));
                foreach (var item in allmarkets)
                {
                    foreach (var runner in item.RunnersForindianFancy)
                    {
                        runners.Add(runner);
                    }
                }
                cmbfancy.ItemsSource = runners;
                cmbfancy.DisplayMemberPath = "RunnerName";
                cmbfancy.ValueMemberPath = "RunnerName";
            }
            catch(Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Fancy are not Comming..");
            }
            }
        public string Insertfancy(string EventID, string MarketBookID)
        {

            List<int> userIDs = cmbCustomerAllowedMarketsInFancy.SelectedValue.ToString().Split(',').Select(int.Parse).ToList();
            List<SP_Users_GetUserbyCreatedbyID_Result> lstUSersAll = new List<SP_Users_GetUserbyCreatedbyID_Result>();
            foreach (int id in userIDs)
            {
                SP_Users_GetUserbyCreatedbyID_Result s = new SP_Users_GetUserbyCreatedbyID_Result();
                int r = Convert.ToInt32(id);
                s.ID = r;
                lstUSersAll.AddRange( JsonConvert.DeserializeObject<List<SP_Users_GetUserbyCreatedbyID_Result>>(objUsersServiceCleint.getlistuserids(r)).ToList());
                lstUSersAll.Add(s);
            }

            List<int> lstUSers = new List<int>();
            foreach (var id in lstUSersAll)
            {
                int IDs = id.ID;
                lstUSers.Add(IDs);
            }

            List<ExternalAPI.TO.MarketBookForindianFancy> allmarkets = new List<ExternalAPI.TO.MarketBookForindianFancy>();
            List<ExternalAPI.TO.RunnerForIndianFancy> runners = new List<ExternalAPI.TO.RunnerForIndianFancy>();
            List<ExternalAPI.TO.RunnerForIndianFancy> runners2 = new List<ExternalAPI.TO.RunnerForIndianFancy>();
            ExternalAPI.TO.RunnerForIndianFancy runners3 = new ExternalAPI.TO.RunnerForIndianFancy();
            allmarkets.Clear();
            allmarkets.Add(objBettingClient.GetMarketDatabyIDIndianFancy(EventID, MarketBookID));
            foreach(var item in allmarkets)
            {
                foreach(var runner in item.RunnersForindianFancy)
                {
                    runners.Add(runner);
                }
            }
            List<string> fancy = cmbfancy.SelectedValue.ToString().Split(',').ToList();
            foreach(var fan in fancy)
            {
                runners3 = runners.Where(item => item.RunnerName == fan).FirstOrDefault();

                runners2.Add(runners3);
            }
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                //   cmbfancy.DisplayMemberPath= allmarkets[0].
                objUsersServiceCleint.InsertIndainFancy(allmarkets.ToArray(), runners2.ToArray(), lstUSers.ToArray(), EventID, LoggedinUserDetail.PasswordForValidate);               
            }
            return "";
        }
        private void Button_Click_27(object sender, RoutedEventArgs e)
        {
            Insertfancy(EventID, marketbookid);
        }
        public string EventID;
        public string marketbookid;
        private void cmbEventss_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lbleventid.Content = "";
                marketbookid = "";
                EventID = (cmbEventss.SelectedValue).ToString();

                var lstdistincteventmarketsfordownload = lstMarketsdownload.Where(item => item.Market.Contains("Match Odds") && item.EventID == EventID).Distinct().ToList();
                marketbookid = lstdistincteventmarketsfordownload[0].MarketCatalogueID;

                ShowFancyResultsPostingPanelINPost();
            }
            catch (Exception ex)
            {

            }
        }

        public void ShowFancyResultsPostingPanelINPost()
        {
            List<bftradeline.Models.UserBetsForAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.location == "9" && item.isMatched == true).ToList();
            if (lstCurrentBetsAdmin != null)
            {
                //  var results = LastloadedLinMarkets.Where(item => item.Runners[0].ExchangePrices.AvailableToBack[0].Price > 0 || item.Runners[0].ExchangePrices.AvailableToLay[0].Price > 0).ToList();
                List<ExternalAPI.TO.RunnerForIndianFancy> lstRunners = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                foreach (var item in lstCurrentBetsAdmin)
                {
                    ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner.SelectionId = item.SelectionID;
                    objRunner.RunnerName = item.SelectionName;
                    objRunner.MarketBookID = item.MarketBookID;
                    // txtInningsResultI.Text = item.MarketBookID;
                    lstRunners.Add(objRunner);
                }

                cmbINFancy.IsSynchronizedWithCurrentItem = false;
                cmbINFancy.ItemsSource = lstRunners;
                cmbINFancy.DisplayMemberPath = "RunnerName";
                cmbINFancy.SelectedValuePath = "MarketBookID";
                cmbINFancy.SelectedIndex = 0;

            }
        }

        private void Button_Click_31(object sender, RoutedEventArgs e)
        {
            if (cmbINFancy.SelectedIndex > -1)
            {
                MessageBoxResult messgeresult = Xceed.Wpf.Toolkit.MessageBox.Show("Post Results ?", "Confirm Post Results", MessageBoxButton.YesNo);
                if (messgeresult == MessageBoxResult.Yes)
                {
                    string aa = cmbINFancy.SelectedValue.ToString();
                    objUsersServiceCleint.CheckforMatchCompletedFancyIN(cmbINFancy.SelectedValue.ToString(), cmbINFancy.Text, Convert.ToInt32(txtscore.Text));
                    //  popupIndianFancyResultPosting.IsOpen = false;
                    Xceed.Wpf.Toolkit.MessageBox.Show("Posted Succesfully.");
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        private void cmbINFancy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbINFancy.SelectedIndex > -1)
            {
                string aa = cmbINFancy.SelectedValue.ToString();
                List<bftradeline.Models.UserBetsForAdmin> lstCurrentBetsAdmin = LoggedinUserDetail.CurrentAdminBets.ToList().Where(item => item.location == "9" && item.isMatched == true).ToList();
                var a = lstCurrentBetsAdmin.Where(item => item.SelectionID == aa).FirstOrDefault();
                lbleventid.Content = a.MarketBookID;
            }
        }

        private void cmbmarket_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {

        }

       
    } }
