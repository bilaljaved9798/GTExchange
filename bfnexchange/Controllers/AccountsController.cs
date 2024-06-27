using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bfnexchange.UsersServiceReference;
using bfnexchange.Models;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Configuration;
using ExternalAPI;
using ExternalAPI.TO;
using System.Net;
//using bfnexchange.Services;
using System.Globalization;

using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using bfnexchange.HelperClasses;
using System.Windows.Forms;
using bfnexchange.BettingServiceReference;

namespace bfnexchange.Controllers
{
    public class AccountsController : Controller
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceClient objBettingClient = new BettingServiceClient();


        List<ProfitandLossEventType> lstProfitandLossAll = new List<ProfitandLossEventType>();
        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AccStatement()
        {
            return View();
        }
        public ActionResult MyBets()
        {
            return View();
        }
        public ActionResult PlusMinusSummery()
        {
            return View();
        }
        public ActionResult pl()
        {
            return View();
        }
        public ActionResult Stack()
        {
            return View();
        }
        public ActionResult UserActivites()
        {
            return View();
        }
        public ActionResult test()
        {
            return View();
        }
        public string ConvertDateFormat(string datetoconvert)
        {
            string[] datearr = datetoconvert.Split('-');
            datetoconvert = datearr[2].ToString() + "-" + datearr[1].ToString() + "-" + datearr[0].ToString();
            return datetoconvert;
        }


      

        public PartialViewResult LedgerDetailsNew(string DateFrom, string DateTo, int UserID, bool isCredit)
        {
            LoggedinUserDetail.CheckifUserLogin();
            try
            {
                DateFrom = ConvertDateFormat(DateFrom);
                DateTo = ConvertDateFormat(DateTo);
                if (UserID == 0)
                {
                    UserID = LoggedinUserDetail.GetUserID();
                }
                List<UserAccounts> lstUserAccounts = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyUserIDandDateRange(UserID, DateFrom, DateTo, isCredit, ConfigurationManager.AppSettings["PasswordForValidate"]));
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
                            if (lstUserAccounts[i + 1].Debit == "" || lstUserAccounts[i + 1].Debit == "0.00")
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
                ViewBag.NetProfitorLoss = objUsersServiceCleint.GetProfitorLossbyUserID(UserID, isCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);
                return PartialView("LedgerDetailsNew", lstUserAccounts);
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                List<UserAccounts> lstUserAccounts = new List<UserAccounts>();
                return PartialView("LedgerDetailsNew", lstUserAccounts);
            }

        }
        public string UpdateBetSlipKeys(string SimpleBtn1, string SimpleBtn2, string SimpleBtn3, string SimpleBtn4, string SimpleBtn5, string SimpleBtn6, string SimpleBtn7, string SimpleBtn8, string SimpleBtn9, string SimpleBtn10, string SimpleBtn11, string SimpleBtn12, string MutipleBtn1, string MutipleBtn2, string MutipleBtn3, string MutipleBtn4, string MutipleBtn5, string MutipleBtn6, string MutipleBtn7, string MutipleBtn8, string MutipleBtn9, string MutipleBtn10, string MutipleBtn11, string MutipleBtn12)          
        {

            if (LoggedinUserDetail.GetUserID() > 0)
            {
                objUsersServiceCleint.UpdateBetSlipKeys(LoggedinUserDetail.GetUserID(), SimpleBtn1, SimpleBtn2, SimpleBtn3, SimpleBtn4, SimpleBtn5, SimpleBtn6, SimpleBtn7, SimpleBtn8, SimpleBtn9, SimpleBtn10, SimpleBtn11, SimpleBtn12, MutipleBtn1, MutipleBtn2, MutipleBtn3, MutipleBtn4, MutipleBtn5, MutipleBtn6, MutipleBtn7, MutipleBtn8, MutipleBtn9, MutipleBtn10, MutipleBtn11, MutipleBtn12);
                return "True";
            }
            else
            {
                return "False";
            }
        }


        [HttpPost]
        public ActionResult Stack(BetSlipKeys bt)
        {
            BetSlipKeys obj = new BetSlipKeys();

           // objUsersServiceCleint.UpdateBetSlipKeys(LoggedinUserDetail.GetUserID(), obj.SimpleBtn1, obj.SimpleBtn2, obj.SimpleBtn3, obj.SimpleBtn4, obj.SimpleBtn5, obj.SimpleBtn6, obj.SimpleBtn7, obj.SimpleBtn8, obj.SimpleBtn9, "0", "0", "0", obj.MutipleBtn1, obj.MutipleBtn2, obj.MutipleBtn3, obj.MutipleBtn4, obj.MutipleBtn5, obj.MutipleBtn6, obj.MutipleBtn7, obj.MutipleBtn8, obj.MutipleBtn9, "0", "0", "0");

            // MessageBox.Show("Updated Successfully");
            LoggedinUserDetail.objBetSlipKeys = JsonConvert.DeserializeObject<bfnexchange.Models.BetSlipKeys>(objUsersServiceCleint.GetBetSlipKeys(LoggedinUserDetail.GetUserID()));
            //SetBetSlipKeys();
            return View();

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

