using GTWeb.BettingServiceReference;
using GTWeb.Models;
using GTWeb.UsersServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GTWeb.Controllers
{
    //[Route("api/[controller]/[action]")]
    public class AccountsController : ApiController
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceClient objBettingClient = new BettingServiceClient();

        List<ProfitandLossEventType> lstProfitandLossAll = new List<ProfitandLossEventType>();
        public string ConvertDateFormat(string datetoconvert)
        {
            string[] datearr = datetoconvert.Split('-');
            datetoconvert = datearr[2].ToString() + "-" + datearr[1].ToString() + "-" + datearr[0].ToString();
            return datetoconvert;
        }

        public void Getdataforfancybysession()
        {
            try
            {


                //  string DateFrom = dtpFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
                // string DateTo = dtpTo.SelectedDate.Value.ToString("yyyy-MM-dd");
                List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    //  SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                    //   lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRangeFancywithMArketName(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    //List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                    //lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.CommisionAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    //  lstProfitandlossEventtypeCommission = lstProfitandlossEventtypeCommission.Where(item => item.EventType.Contains("Fancy")).ToList();
                    //  ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                    //foreach (var item in lstProfitandlossEventtypeCommission)
                    //{
                    //    item.EventType = item.EventType + " (Commission)";
                    //}

                    //  lstProfitandlossEventtype.AddRange(lstProfitandlossEventtypeCommission);
                }
                else
                {
                    // lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<HelperClasses.ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(LoggedinUserDetail.GetUserID(), DateFrom, DateTo));
                    //   lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRangeFancywithMArketName(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                }


                if (lstProfitandlossEventtype.Count > 0)
                {
                    //if (chkOnlyFancy.IsChecked == true)
                    //{
                    //    lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType.Contains("Fancy")).ToList();
                    //}
                    lstProfitandlossEventtype = lstProfitandlossEventtype.OrderBy(o => o.EventID).ThenBy(o => o.EventType).ToList();
                    // dgvProfitandLoss.ItemsSource = lstProfitandlossEventtype;
                    // txtTotProfiltandLoss.Content = (lstProfitandlossEventtype.Where(item => !item.EventType.Contains("Commission")).Sum(item => item.NetProfitandLoss)+(-1*(lstProfitandlossEventtype.Where(item => item.EventType.Contains("Commission")).Sum(item => item.NetProfitandLoss)))).ToString("N0");
                    //txtTotProfiltandLoss.Content = lstProfitandlossEventtype.Where(item => !item.EventType.Contains("Commission")).Sum(item => item.NetProfitandLoss).ToString("N0");
                    // if (Convert.ToDecimal(txtTotProfiltandLoss.Content) >= 0)
                    //{
                    //    txtTotProfiltandLoss.Background = Brushes.Green;
                    //    txtTotProfiltandLoss.Foreground = Brushes.White;
                    //}
                    //else
                    //{
                    //    txtTotProfiltandLoss.Background = Brushes.Red;
                    //    txtTotProfiltandLoss.Foreground = Brushes.White;
                    //}
                    lstProfitandLossAll = lstProfitandlossEventtype;
                }
                else
                {
                    //dgvProfitandLoss.ItemsSource = new List<ProfitandLossEventType>();
                    //txtTotProfiltandLoss.Content = "0.00";
                    //txtTotProfiltandLoss.Background = Brushes.Green;
                    //txtTotProfiltandLoss.Foreground = Brushes.White;
                    //MessageBox.Show("No data found.");
                }
            }
            catch (System.Exception ex)
            {

            }
        }


    }
}
