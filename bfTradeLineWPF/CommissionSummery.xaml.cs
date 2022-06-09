using bftradeline.HelperClasses;
using bftradeline.Models;
using globaltraders.AccountsServiceReference;
using globaltraders.Models;
using globaltraders.UserServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for CommissionSummery.xaml
    /// </summary>
    public partial class CommissionSummery : Window
    {

        AccountsServiceClient objAccountsServiceclient = new AccountsServiceClient();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        public DateTime FromDate;
        public DateTime ToDate;
        public string EventTypeforaccounts = "";
        List<ProfitandLossEventType> lstProfitandLossAll = new List<ProfitandLossEventType>();
        public CommissionSummery()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dtpFrom.SelectedDate = DateTime.Now;
            dtpTo.SelectedDate = DateTime.Now;
            GetEventTypeFromAccounts();
            button1_Click(this, e);
        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {

            //   Getdataforfancybysession();
            //  GetDistinctMatchesfromResults();
          //  return;


            string DateFrom = dtpFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
            string DateTo = dtpTo.SelectedDate.Value.ToString("yyyy-MM-dd");
            List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();

            if (cmbEventTypeforLedger.SelectedIndex > 0)
            {

            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {
                    List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                    lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetDatabyAgentIDForCommisionandDateRangeByEventtype(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                    objProfitandLossCommission.EventType = "Commission";
                    objProfitandLossCommission.NetProfitandLoss = lstProfitandlossEventtypeCommission.Sum(item => item.NetProfitandLoss);
                    lstProfitandlossEventtype.Add(objProfitandLossCommission);
                }
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                    lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetDatabyAgentIDForCommisionandDateRangeByEventtype(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    //lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetDatabyAgentIDForCommisionandDateRangeByEventtype(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    // var data = JsonConvert.DeserializeObject(objUsersServiceCleint.GetDatabyAgentIDForCommisionandDateRangeByEventtype(Convert.ToInt32(LoggedinUserDetail.GetUserID()), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                    //objProfitandLossCommission.EventType =data.;
                    //objProfitandLossCommission.NetProfitandLoss = lstProfitandlossEventtypeCommission.Sum(item => item.NetProfitandLoss);
                    //lstProfitandlossEventtype.Add(objProfitandLossCommission);
                    lstProfitandLossAll = lstProfitandlossEventtype;
                    dgvProfitandLoss.ItemsSource = lstProfitandLossAll;
                }



                else
                {
                    dgvProfitandLoss.ItemsSource = new List<ProfitandLossEventType>();

                    MessageBox.Show("No data found.");
                }
            }
            lstProfitandLossAll = lstProfitandlossEventtype;



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
        private void dgvProfitandLoss_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
            catch (System.Exception ex)
            {

            }
        }

    }
}
