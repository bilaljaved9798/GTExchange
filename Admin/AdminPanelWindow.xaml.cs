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
           
            var results = objUsersServiceCleint.GetAllUsersbyUserTypeNew(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.GetUserTypeID(), LoggedinUserDetail.PasswordForValidate);
            if (results != "")
            {
                List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);

                foreach (UserIDandUserType objuser in lstUsers)
                {
                   
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
                    cmbCustomerAllowedMarkets.IsSynchronizedWithCurrentItem = false;
                    cmbCustomerAllowedMarkets.ItemsSource = lstUsers;
                    cmbCustomerAllowedMarkets.DisplayMemberPath = "UserName";
                    cmbCustomerAllowedMarkets.SelectedValuePath = "ID";


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
                List<UserIDandUserType> lstonlyAgents = lstUsers.Where(item => item.UserTypeID == 2 || item.UserTypeID == 0).ToList();
                lstonlyAgentsAll = lstonlyAgents;
                cmbUsersforBalanceSheet.ItemsSource = lstonlyAgents;
                cmbUsersforBalanceSheet.DisplayMemberPath = "UserName";
                cmbUsersforBalanceSheet.SelectedValuePath = "ID";



            }
            else
            {
                List<UserIDandUserType> lstUsers = new List<UserIDandUserType>();

            }
        }
        List<UserIDandUserType> lstonlyAgentsAll = new List<UserIDandUserType>();
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
           // }
        }
        public List<BalanceSheet> BalanceSheet(int UserID, bool isCredit)
        {
            LoggedinUserDetail.CheckifUserLogin();
            try
            {

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
                        foreach (var item in lstonlyAgentsAll)
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
                                List<UserAccounts> lstAccountsDonebyAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(1, false, LoggedinUserDetail.PasswordForValidate));
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
                    if (lstUserAccounts.Count > 0)
                    {

                        var lstUserIDs = lstUserAccounts.Select(item1 => new { item1.UserID }).Distinct().ToArray();

                        foreach (var useritem in lstUserIDs)
                        {
                            List<UserAccounts> lstUseraccountsbyUser = lstUserAccounts.Where(item2 => item2.UserID == useritem.UserID).ToList();
                            if (lstUseraccountsbyUser.Count > 0)
                            {
                                BalanceSheet objBalanceSheet = new BalanceSheet();
                                if (lstUseraccountsbyUser[0].UserType == null)
                                {
                                    objBalanceSheet.Username = Crypto.Decrypt(lstUseraccountsbyUser[0].UserName);
                                }
                                else
                                {
                                    objBalanceSheet.Username = lstUseraccountsbyUser[0].UserName;
                                }
                                objBalanceSheet.StartingBalance = lstUseraccountsbyUser[0].OpeningBalance;
                                objBalanceSheet.UserID = Convert.ToInt32(lstUseraccountsbyUser[0].UserID);
                                decimal Profitorloss = lstUseraccountsbyUser.Sum(item3 => Convert.ToDecimal(item3.Debit)) - lstUseraccountsbyUser.Sum(item3 => Convert.ToDecimal(item3.Credit));
                                if (Profitorloss >= 0)
                                {
                                    objBalanceSheet.Profit = Profitorloss;

                                }
                                else
                                {
                                    objBalanceSheet.Loss = Profitorloss;
                                }
                                lstBalanceSheet.Add(objBalanceSheet);
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
                        lstUserAccountsAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(UserID, isCredit, LoggedinUserDetail.PasswordForValidate));
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
                   
                }
                //HawalaAccount
                if (UserID != 73)
                {


                    int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
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
                   
                }

              
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


            }
            catch (Exception ex)
            {
                // Response.Write(ex.ToString());
            }
        }
        private void TabItem_Loaded(object sender, RoutedEventArgs e)
        {
           
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
                   
                }
                else
                {
                   
                }
            }
            catch (System.Exception ex)
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
                       
                    }
                    catch (System.Exception ex)
                    {

                    }

                    chkAutomaticResultPostFancy.IsChecked = objUsersServiceCleint.GetFancyResultPostSetting();

                }
                catch (System.Exception ex)
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
                        objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + txtUsername.Text + ")", "0.00", txtAccountBalance.Text.ToString(), HawalaID, "", DateTime.Now, "", "", Newaccountbalance, false, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, Convert.ToDecimal(txtAccountBalance.Text), LoggedinUserDetail.PasswordForValidate);
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
                     
                    }
                  
                    objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + txtUsername.Text.ToString() + ")", "0.00", txtAccountBalance.Text.ToString(), CreatedbyID, "", DateTime.Now, "", "", Newaccountbalance, false, "", "", "", "", "");
                    objUsersServiceCleint.UpdateAccountBalacnebyUser(CreatedbyID, Convert.ToDecimal(txtAccountBalance.Text), LoggedinUserDetail.PasswordForValidate);

                    LoggedinUserDetail.InsertActivityLog(CreatedbyID, "Created new user (" + txtUsername.Text.ToString() + ")");
                    Xceed.Wpf.Toolkit.MessageBox.Show("Added successfully");
                    GetusersbyUserTypeReload();
                    GetUsersbyUsersType();
                    ClearAllTextFields();
                    // return "True" + "|" + userid.ToString();
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
                            if (objSelectedUser.UserTypeID == 2)
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
                                catch (Exception ex)
                                {


                                }
                            }
                        }
                        catch (System.Exception ex)
                        {

                        }
                    }


                }
            }
            catch (System.Exception ex)
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
                        if (LoggedinUserDetail.GetUserTypeID() == 1 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
                        {

                            
                        }
                        txtAgentRateUpate.Text = Crypto.Encrypt(txtAgentRateUpate.Text);
                        DateTime updatedtime = DateTime.Now;

                        objUsersServiceCleint.SetBlockedStatusofUser(Convert.ToInt32(cmbUsers.SelectedValue), chkBlockUser.IsChecked.Value, LoggedinUserDetail.PasswordForValidate);
                        LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Block Status");
                      
                        if (txtPasswordUpdate.Text.Length >= 6)
                        {
                            objUsersServiceCleint.ResetPasswordofUser(Convert.ToInt32(cmbUsers.SelectedValue), Crypto.Encrypt(txtPasswordUpdate.Text), UpdatedBy, updatedtime, LoggedinUserDetail.PasswordForValidate);
                            LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Password");
                        }
                        objUsersServiceCleint.SetAgentRateofUser(Convert.ToInt32(cmbUsers.SelectedValue), txtAgentRateUpate.Text, LoggedinUserDetail.PasswordForValidate);
                        LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Set Rate of User " + Convert.ToInt32(cmbUsers.SelectedValue).ToString() + " " + Crypto.Decrypt(txtAgentRateUpate.Text).ToString());
                        objUsersServiceCleint.SetLoggedinStatus(Convert.ToInt32(cmbUsers.SelectedValue), chkLoggedIn.IsChecked.Value);
                        
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
            catch (System.Exception ex)
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
                       
                    }
                    objAccountsService.AddtoUsersAccounts("Amount removed from your account( " + txtAccountsTitle.Text + " for " + cmbUsersCredit.Text + "(UserID=" + UserID.ToString() + ") )", "0.00", Newaccountbalance.ToString(), AddedbyID, "", DateTime.Now, "", "", CurrentAccountBalance, chkIsCredit.IsChecked.Value, "", "", "", "", "");
                    objUsersServiceCleint.UpdateAccountBalacnebyUser(AddedbyID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                    LoggedinUserDetail.InsertActivityLog(AddedbyID, "Added Balance " + Newaccountbalance.ToString() + " to user ( " + cmbUsersCredit.Text + " )");
                    btnAddCredit.IsEnabled = true;
                    Xceed.Wpf.Toolkit.MessageBox.Show("Successfully Added.");
                    CreditUsersLoad();
                    txtAccountsTitle.Text = "";
                    txtBalanceAdd.Text = "";
                }
           // }
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
                        objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + txtAccountsTitle.Text + " for " + cmbUsersCredit.Text + "(UserID=" + UserID.ToString() + ") )", Newaccountbalance.ToString(), "0.00", AddedbyID, "", DateTime.Now, "", "", CurrentAccountBalance, chkIsCredit.IsChecked.Value, "", "", "", "", "");
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
                           
                        }
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(AddedbyID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                        LoggedinUserDetail.InsertActivityLog(AddedbyID, "Removed Balance " + Newaccountbalance.ToString() + "  from user ( " + cmbUsersCredit.Text + " )");
                        btnRemovecredit.IsEnabled = true;
                        MessageBox.Show("Successfully Removed");
                        CreditUsersLoad();
                        txtAccountsTitle.Text = "";
                        txtBalanceAdd.Text = "";
                   // }

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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
                objAccountsService.AddtoUsersAccounts(AccountsTitle, AccountBalance.ToString(), "0.00", UserIDTo, "", DateTime.Now, "", "", UserToBalance, false, "", "", "", "", "");
                objUsersServiceCleint.AddCredittoUser(AccountBalance, UserIDTo, LoggedinUserDetail.GetUserID(), DateTime.Now, 0, false, AccountsTitle, false, LoggedinUserDetail.PasswordForValidate);
                objAccountsService.AddtoUsersAccounts(AccountsTitle, "0.00", AccountBalance.ToString(), UserIDFrom, "", DateTime.Now, "", "", UserFromBalance, false, "", "", "", "", "");
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
                catch (System.Exception ex)
                {

                }
               

                Xceed.Wpf.Toolkit.MessageBox.Show("Updated Successfully");
                CustomerAllowedMarketsLoad();
                FilterMarkets();
                stkpnlLoaderHorse.Visibility = Visibility.Collapsed;
            }
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
            {

            }
        }

        private void Button_Click_21(object sender, RoutedEventArgs e)
        {
            if (cmbEvents.SelectedIndex > -1)
            {
               
                objUsersServiceCleint.UpdateTotalOversbyMarket(cmbEvents.SelectedValue.ToString(), txtTotalOvers.Text.ToString());
                Xceed.Wpf.Toolkit.MessageBox.Show("Updated successfully.");
            }
        }

        private void Button_Click_22(object sender, RoutedEventArgs e)
        {
            if (cmbEvents.SelectedIndex > -1)
            {

                objUsersServiceCleint.UpdateGetDataFromForLoggingData(cmbEvents.SelectedValue.ToString(), cmbGetUpdateFrom.Text);
               
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
                catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (System.Exception ex)
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
            catch (Exception ex)
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
            catch (Exception ex)
            {


            }
        }
    }
}
