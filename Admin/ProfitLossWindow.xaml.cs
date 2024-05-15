
using bftradeline.HelperClasses;
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
    /// Interaction logic for ProfitLossWindow.xaml
    /// </summary>
    public partial class ProfitLossWindow : Window
    {
        public ProfitLossWindow()
        {
            InitializeComponent();
        }
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        List<ProfitandLossEventType> lstProfitandLossAll = new List<ProfitandLossEventType>();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFrom.SelectedDate = DateTime.Now;
            dtpTo.SelectedDate = DateTime.Now;
            button1_Click(this, e);
        }
        public void Getdataforfancybysession()
        {
            try
            {


                string DateFrom = dtpFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
                string DateTo = dtpTo.SelectedDate.Value.ToString("yyyy-MM-dd");
                List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                    lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRangeFancywithMArketName(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                    lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.CommisionAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    lstProfitandlossEventtypeCommission = lstProfitandlossEventtypeCommission.Where(item => item.EventType.Contains("Fancy")).ToList();
                    ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                    foreach (var item in lstProfitandlossEventtypeCommission)
                    {
                        item.EventType = item.EventType + " (Commission)";
                    }

                    lstProfitandlossEventtype.AddRange(lstProfitandlossEventtypeCommission);
                }
               


                if (lstProfitandlossEventtype.Count > 0)
                {
                   
                    lstProfitandlossEventtype = lstProfitandlossEventtype.OrderBy(o => o.EventID).ThenBy(o => o.EventType).ToList();
                    dgvProfitandLoss.ItemsSource = lstProfitandlossEventtype;
                    
                    txtTotProfiltandLoss.Content = lstProfitandlossEventtype.Where(item => !item.EventType.Contains("Commission")).Sum(item => item.NetProfitandLoss).ToString("N0");
                    if (Convert.ToDecimal(txtTotProfiltandLoss.Content) >= 0)
                    {
                        txtTotProfiltandLoss.Background = Brushes.Green;
                        txtTotProfiltandLoss.Foreground = Brushes.White;
                    }
                    else
                    {
                        txtTotProfiltandLoss.Background = Brushes.Red;
                        txtTotProfiltandLoss.Foreground = Brushes.White;
                    }
                    lstProfitandLossAll = lstProfitandlossEventtype;
                }
                else
                {
                    dgvProfitandLoss.ItemsSource = new List<ProfitandLossEventType>();
                    txtTotProfiltandLoss.Content = "0.00";
                    txtTotProfiltandLoss.Background = Brushes.Green;
                    txtTotProfiltandLoss.Foreground = Brushes.White;
                    MessageBox.Show("No data found.");
                }
            }
            catch (System.Exception ex)
            {

            }
        }
      
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if(chkOnlyFancy.IsChecked.Value==true && chkOnlyFancySession.IsChecked.Value == true)
            {
                Getdataforfancybysession();
                return;
            }
            if (chkByMarket.IsChecked.Value == false)
            {
                string DateFrom = dtpFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
                string DateTo = dtpTo.SelectedDate.Value.ToString("yyyy-MM-dd");
                List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();

                SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));       
                List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.CommisionAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                    objProfitandLossCommission.EventType = "Commission";
                    objProfitandLossCommission.NetProfitandLoss = lstProfitandlossEventtypeCommission.Sum(item => item.NetProfitandLoss);
                    lstProfitandlossEventtype.Add(objProfitandLossCommission);
              

                if (lstProfitandlossEventtype.Count > 0)
                {
                    if (chkOnlyFancy.IsChecked == true)
                    {
                        lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType.Contains("Fancy")).ToList();
                    }
                    if (chkByMarketCricket.IsChecked.Value == true)
                    {
                        lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType==("Cricket") || item.EventType==("Fancy")).ToList();
                    }
                    dgvProfitandLoss.ItemsSource = lstProfitandlossEventtype;
              
                    txtTotProfiltandLoss.Content = lstProfitandlossEventtype.Where(item => item.EventType != "Commission").Sum(item => item.NetProfitandLoss).ToString("N0");
                    if (Convert.ToDecimal(txtTotProfiltandLoss.Content) >= 0)
                    {
                        txtTotProfiltandLoss.Background = Brushes.Green;
                        txtTotProfiltandLoss.Foreground = Brushes.White;
                    }
                    else
                    {
                        txtTotProfiltandLoss.Background = Brushes.Red;
                        txtTotProfiltandLoss.Foreground = Brushes.White;
                    }
                }
                else
                {
                    dgvProfitandLoss.ItemsSource = new List<ProfitandLossEventType>();
                    txtTotProfiltandLoss.Content = "0.00";
                    txtTotProfiltandLoss.Background = Brushes.Green;
                    txtTotProfiltandLoss.Foreground = Brushes.White;
                    MessageBox.Show("No data found.");
                }
                lstProfitandLossAll = lstProfitandlossEventtype;
            }
            else
            {
                try
                {


                    string DateFrom = dtpFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
                    string DateTo = dtpTo.SelectedDate.Value.ToString("yyyy-MM-dd");
                    List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                       SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                        List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                        lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.CommisionAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                       ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                        foreach (var item in lstProfitandlossEventtypeCommission)
                        {
                            item.EventType = item.EventType + " (Commission)";
                        }

                        lstProfitandlossEventtype.AddRange(lstProfitandlossEventtypeCommission);
                    }
                   


                    if (lstProfitandlossEventtype.Count > 0)
                    {
                        if (chkOnlyFancy.IsChecked == true)
                        {
                            lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType.Contains("Fancy")).ToList();
                        }
                        if (chkByMarketCricket.IsChecked.Value == true)
                        {
                            lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.Eventtype1==("Cricket") ||  item.Eventtype1==("Fancy")).ToList();
                        }
                        dgvProfitandLoss.ItemsSource = lstProfitandlossEventtype;
                       
                        txtTotProfiltandLoss.Content = lstProfitandlossEventtype.Where(item => !item.EventType.Contains("Commission")).Sum(item => item.NetProfitandLoss).ToString("N0");
                        if (Convert.ToDecimal(txtTotProfiltandLoss.Content) >= 0)
                        {
                            txtTotProfiltandLoss.Background = Brushes.Green;
                            txtTotProfiltandLoss.Foreground = Brushes.White;
                        }
                        else
                        {
                            txtTotProfiltandLoss.Background = Brushes.Red;
                            txtTotProfiltandLoss.Foreground = Brushes.White;
                        }
                    }
                    else
                    {
                        dgvProfitandLoss.ItemsSource = new List<ProfitandLossEventType>();
                        txtTotProfiltandLoss.Content = "0.00";
                        txtTotProfiltandLoss.Background = Brushes.Green;
                        txtTotProfiltandLoss.Foreground = Brushes.White;
                        MessageBox.Show("No data found.");
                    }
                    lstProfitandLossAll = lstProfitandlossEventtype;
                 
                }
                
                catch (System.Exception ex)
                {

                }
               
               
            }
            chkSelectAll.IsChecked = true;
        }

        private void dgvProfitandLoss_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (LoggedinUserDetail.GetUserTypeID() != 3)
            {
                if (dgvProfitandLoss.Items.Count > 0 && chkByMarket.IsChecked == false)
                {
                   ProfitandLossEventType objSelectedRow = (ProfitandLossEventType)dgvProfitandLoss.SelectedItem;
                    PlusMinusSummaryWindow objfrmledger = new PlusMinusSummaryWindow();
                    objfrmledger.EventTypeforaccounts = objSelectedRow.EventType;
                    objfrmledger.FromDate = dtpFrom.SelectedDate.Value;
                    objfrmledger.ToDate = dtpTo.SelectedDate.Value;
                    objfrmledger.ShowDialog();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }      
        public void GetTotalofAll()
        {
            try
            {
                List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();
               lstProfitandlossEventtype= lstProfitandLossAll.Where(item => item.EventType != "Commission" && !item.EventType.Contains("Commission") && item.isCheckedforTotal==true).ToList();
                txtTotProfiltandLoss.Content = lstProfitandlossEventtype.Where(item => item.EventType != "Commission").Sum(item => item.NetProfitandLoss).ToString("N0");
                if (Convert.ToDecimal(txtTotProfiltandLoss.Content) >= 0)
                {
                    txtTotProfiltandLoss.Background = Brushes.Green;
                    txtTotProfiltandLoss.Foreground = Brushes.White;
                }
                else
                {
                    txtTotProfiltandLoss.Background = Brushes.Red;
                  txtTotProfiltandLoss.Foreground = Brushes.White;
                }
            }
            catch (System.Exception ex)
            {

            }
        }
        private void chkSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvProfitandLoss != null)
                {
                    foreach (var item in lstProfitandLossAll)
                    {
                        item.isCheckedforTotal = true;
                    }
                    dgvProfitandLoss.ItemsSource = null;
                    dgvProfitandLoss.ItemsSource = lstProfitandLossAll;
                    GetTotalofAll();
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private void chkSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvProfitandLoss != null)
                {
                    foreach (var item in lstProfitandLossAll)
                    {
                        item.isCheckedforTotal = false;
                    }
                    dgvProfitandLoss.ItemsSource = null;
                    dgvProfitandLoss.ItemsSource = lstProfitandLossAll;
                    GetTotalofAll();
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetTotalofAll();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvProfitandLoss.SelectedItem != null)
                {

                
                ProfitandLossEventType selecteditem = (ProfitandLossEventType)dgvProfitandLoss.SelectedItem;
                selecteditem.isCheckedforTotal = true;
                GetTotalofAll();
                }
            }
            catch(System.Exception ex)
            {

            }
          
         
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgvProfitandLoss.SelectedItem != null)
                {
                    ProfitandLossEventType selecteditem = (ProfitandLossEventType)dgvProfitandLoss.SelectedItem;
                    selecteditem.isCheckedforTotal = false;
                    GetTotalofAll();
                }
            }
            catch (System.Exception ex)
            {

            }
        }

    }
}
