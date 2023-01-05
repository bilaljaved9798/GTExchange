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

using System.Globalization;
using bfnexchange.AccountsServiceReference;
using System.ComponentModel;
using System.Data;
using bfnexchange.BettingServiceReference;
using System.Text.RegularExpressions;
using bfnexchange.HelperClasses;


//using bfnexchange.Services.DBModel;

namespace bfnexchange.Controllers
{

    public class DashBoardController : Controller
    {
        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        AccessRightsbyUserType objAccessrightsbyUserType;
        BettingServiceClient client = new BettingServiceClient();
        AccountsServiceClient objAccountsService = new AccountsServiceClient();
        List<ProfitandLossEventType> lstProfitandLossAll = new List<ProfitandLossEventType>();
        
        // GET: DashBoard
        public   ActionResult  Index()
        {
           
            if (LoggedinUserDetail.GetUserTypeID() == 3)
            {
                Session["userbets"] = new List<UserBets>();
                Session["userbet"] = new List<UserBets>();
            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    Session["userbets"] = new List<UserBetsforAgent>();
                    Session["userbet"] = new List<UserBetsforAgent>();
                    ViewBag.backgrod = "#1D9BF0";
                    ViewBag.color = "white";

                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 8)
                    {
                        Session["userbets"] = new List<UserBetsforSuper>();
                        Session["userbet"] = new List<UserBetsforSuper>();
                        ViewBag.backgrod = "#1D9BF0";
                        ViewBag.color = "white";
                    }

                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 9)
                        {
                            Session["userbets"] = new List<UserBetsforSamiadmin>();
                            Session["userbet"] = new List<UserBetsforSamiadmin>();
                            ViewBag.backgrod = "#1D9BF0";
                            ViewBag.color = "white";
                        }
                        else
                        {
                            Session["userbets"] = new List<UserBetsForAdmin>();
                            Session["userbet"] = new List<UserBetsForAdmin>();
                            ViewBag.backgrod = "#1D9BF0";
                            ViewBag.color = "white";
                        }
                    }
                }
            }

            LoggedinUserDetail.CheckifUserLogin();
         
        
            if (LoggedinUserDetail.GetUserID() > 0)
            {
               
                Decimal CurrentLiabality = 0;
              
               
                objAccessrightsbyUserType = new AccessRightsbyUserType();
                //objAccessrightsbyUserType = JsonConvert.DeserializeObject<AccessRightsbyUserType>(objUsersServiceCleint.GetAccessRightsbyUserType(LoggedinUserDetail.GetUserTypeID(), ConfigurationManager.AppSettings["PasswordForValidate"]));


                objAccessrightsbyUserType.CurrentAvailableBalance = LoggedinUserDetail.CurrentAccountBalance + Convert.ToDouble(CurrentLiabality);
                

                objAccessrightsbyUserType.Username = LoggedinUserDetail.GetUserName();

                if ((bool)Session["firsttimeload"] == true)
                {
                    Session["firsttimeload"] = false;
                    LoggedinUserDetail.UpdateCurrentLoggedInID();
                    //objUsersServiceCleint.UpdateCurrentLoggedInIDbyUserID(LoggedinUserDetail.GetUserID());
                }
                Session["linevmarkets"] = null;

                return View(objAccessrightsbyUserType);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public PartialViewResult GetBalnceDetails()
        {
            //objAccessrightsbyUserType = new AccessRightsbyUserType();
            if (LoggedinUserDetail.GetUserID() > 0)
            {
                double CurrentAccountBalance = 0;
                string CurrentLiabality = "";
                try
                {
                    LoggedinUserDetail.CurrentAccountBalance =Convert.ToDouble(objUsersServiceCleint.GetStartingBalance(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));

                    CurrentAccountBalance = Convert.ToDouble(objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                }
                catch (System.Exception ex)
                {

                }
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    //objAccessrightsbyUserType.CurrentLiabality = "";
                    double laboddmarket = 0;
                    double othermarket = 0;
                    List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                    List<UserBets> lstUserBetsF = lstUserBets.Where(x => x.location != "9").ToList();
                    List<UserBets> lstUserBetsfncy= lstUserBets.Where(x => x.location == "9").ToList();
                    laboddmarket = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBetsF);
                    othermarket = objUserBets.GetLiabalityofCurrentUserfancy(LoggedinUserDetail.GetUserID(), lstUserBetsfncy);
                    CurrentLiabality = (laboddmarket + othermarket).ToString("F2");
                    ViewBag.CurrentLiabality = CurrentLiabality;


                    LoggedinUserDetail.CurrentAvailableBalance= CurrentAccountBalance+Convert.ToDouble(CurrentLiabality);


                }
                List<UserLiabality> lstUserLiabality = JsonConvert.DeserializeObject<List<UserLiabality>>(objUsersServiceCleint.GetCurrentLiabality(LoggedinUserDetail.GetUserID()));

               

                objAccessrightsbyUserType = JsonConvert.DeserializeObject<AccessRightsbyUserType>(objUsersServiceCleint.GetAccessRightsbyUserType(LoggedinUserDetail.GetUserTypeID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                //if (LoggedinUserDetail.GetUserTypeID() != 1)
                //{
                //    LoggedinUserDetail.ShowTV = objUsersServiceCleint.GetShowTV(LoggedinUserDetail.GetUserID());
                //    List<HelperClasses.LiveTVChannels> lstTVChannels = JsonConvert.DeserializeObject<List<HelperClasses.LiveTVChannels>>(objUsersServiceCleint.GetLiveTVChanels(ConfigurationManager.AppSettings["PasswordForValidate"]));
                //    LoggedinUserDetail.TvChannels = lstTVChannels;
                //    if (lstUserLiabality.Count > 0)
                //    {
                //        CurrentLiabality = Convert.ToDecimal(lstUserLiabality.Sum(item => Convert.ToDecimal((item.Liabality))));
                //        objAccessrightsbyUserType.CurrentLiabality = CurrentLiabality.ToString("F2");
                //    }
                //    else
                //    {
                //        objAccessrightsbyUserType.CurrentLiabality = "0.00";
                //    }

                //}
                //string accountbalance = objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.UserID, ConfigurationManager.AppSettings["PasswordForValidate"]);
                //LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(accountbalance);

               // objAccessrightsbyUserType.CurrentAvailableBalance = LoggedinUserDetail.CurrentAccountBalance + Convert.ToDouble(CurrentLiabality);
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {
                    decimal TotAdminAmount = 0;
                    decimal TotAdmincommession = 0;
                    decimal TotalAdminAmountWithoutMarkets = 0;
                    List<UserAccounts> AgentCommission = new List<UserAccounts>();


                    List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByIDForSuper(LoggedinUserDetail.GetUserID(), false, ConfigurationManager.AppSettings["PasswordForValidate"]));
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

                                    TotAdminAmount += -1 * (ActualAmount - SuperAmount - AgentAmount);
                                }
                                else
                                {
                                    ActualAmount = -1 * ActualAmount;
                                    decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                    decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                    TotAdminAmount += ActualAmount - AgentAmount - SuperAmount;
                                }
                            }
                        }                   
                    }
                    decimal a = 0;
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
                        //AgentCommission = objUsersServiceCleint.GetTotalAgentCommissionbyAgentID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate);
                    }
                    catch (System.Exception ex)
                    {
                    }
                    if (LoggedinUserDetail.IsCom == true)
                    {
                        a = (-1 * (TotAdminAmount) );
                        ViewBag.commission = TotAdmincommession;
                    }
                    else
                    {
                         a = (-1 * (TotAdminAmount) + (-1 * TotAdmincommession));
                    }
                    objAccessrightsbyUserType.NetBalance = a.ToString();
                  }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {
                        ViewBag.NetBalance =  objUsersServiceCleint.GetProfitorLossbyUserID(LoggedinUserDetail.GetUserID(), false, ConfigurationManager.AppSettings["PasswordForValidate"]).ToString();
                    }

                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            ViewBag.backgrod = "#1D9BF0";
                           ViewBag.color = "white";
                            //LoggedinUserDetail.NetBalance = objUsersServiceCleint.GetProfitorLossbyUserID(LoggedinUserDetail.user.ID, false, LoggedinUserDetail.PasswordForValidate);

                            decimal TotAdminAmount = 0;
                            decimal TotalAdminAmountWithoutMarkets = 0;
                            decimal SuperAmount = 0;
                            decimal SuperAmount1 = 0;
                            List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(LoggedinUserDetail.GetUserID(), false, ConfigurationManager.AppSettings["PasswordForValidate"]));
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
                            }
                            int createdbyid = objUsersServiceCleint.GetCreatedbyID(LoggedinUserDetail.GetUserID());
                            string a = "";
                            List<UserAccounts> lstAccountsDonebyAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(createdbyid, false, ConfigurationManager.AppSettings["PasswordForValidate"]));
                            if (lstAccountsDonebyAdmin.Count > 0)
                            {
                                List<UserAccounts> lstAccountsDonebyAdminagainstthisAgent = lstAccountsDonebyAdmin.Where(item => item.AccountsTitle.Contains("(UserID=" + LoggedinUserDetail.GetUserID().ToString() + ")")).ToList();
                                if (lstAccountsDonebyAdminagainstthisAgent.Count > 0)
                                {
                                    TotalAdminAmountWithoutMarkets = lstAccountsDonebyAdminagainstthisAgent.Sum(item => Convert.ToDecimal(item.Debit)) - lstAccountsDonebyAdminagainstthisAgent.Sum(item => Convert.ToDecimal(item.Credit));
                                }
                            }
                            decimal AgentCommission = 0;
                            decimal superCommission = 0;
                            try
                            {
                                AgentCommission = objUsersServiceCleint.GetTotalAgentCommissionbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                            }
                            catch (System.Exception ex)
                            {
                            }
                            if (LoggedinUserDetail.IsCom == true)
                            {
                                a = ((-1 * (TotAdminAmount) + (-1 * TotalAdminAmountWithoutMarkets) + (-1 * (SuperAmount1))) ).ToString();
                                ViewBag.commission = AgentCommission;
                            }
                            else
                            {
                                a = ((-1 * (TotAdminAmount) + (-1 * TotalAdminAmountWithoutMarkets) + (-1 * (SuperAmount1))) + AgentCommission).ToString();
                            }

                            ViewBag.NetBalance =  a;   
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 9)
                            {
                                ViewBag.backgrod = "#1D9BF0";
                                ViewBag.color = "white";
                                decimal TotAdminAmount = 0;
                                decimal TotAdmincommession = 0;
                                decimal TotalAdminAmountWithoutMarkets = 0;
                                List<UserAccounts> AgentCommission = new List<UserAccounts>();

                                List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByIDForSamiAdmin(LoggedinUserDetail.GetUserID(), false, ConfigurationManager.AppSettings["PasswordForValidate"]));
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
                                            int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiadminRate);
                                            decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                            decimal superpercent = SuperRate - AgentRate;
                                            decimal samiadminpercent = SamiadminRate -( superpercent + AgentRate);
                                            if (ActualAmount > 0)
                                            {
                                                decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
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

                                                TotAdminAmount += -1 * (ActualAmount - SuperAmount - AgentAmount- SamiadminAmount);
                                            }
                                            else
                                            {
                                                ActualAmount = -1 * ActualAmount;
                                                decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                                decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                TotAdminAmount += ActualAmount - AgentAmount - SuperAmount- SamiadminAmount;
                                            }
                                        }
                                    }
                                }

                                try
                                {
                                    foreach (UserAccounts objuserAccounts in AgentCommission)
                                    {
                                        int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                        int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                        int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                        int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiadminRate);
                                        decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                        decimal superpercent = SuperRate - AgentRate;
                                        decimal samiadminpercent = SamiadminRate -(superpercent + AgentRate);
                                        ActualAmount = -1 * ActualAmount;
                                        decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                        decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                        decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                        TotAdmincommession += ActualAmount - AgentAmount - SuperAmount- SamiadminAmount;
                                    }
                                    //AgentCommission = objUsersServiceCleint.GetTotalAgentCommissionbyAgentID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate);
                                }
                                catch (System.Exception ex)
                                {
                                }
                                decimal a = (-1 * (TotAdminAmount) + (-1 * TotAdmincommession));

                                ViewBag.NetBalance = a.ToString();
                            }
                        }

                    }
                } 
            }
          
            return PartialView("BalanceDetails", objAccessrightsbyUserType);
        }
       
        public PartialViewResult GetRules()
        {
            return PartialView("Rules");
        }

        public PartialViewResult GetCompletedResult()
        {
            List<CompletedResult> lstResult = JsonConvert.DeserializeObject<List<CompletedResult>>(objUsersServiceCleint.GetCompletedResult()).ToList();
            // var  lstResulta = lstResult.GroupBy(u => u.Winnername).Select(grp => grp.ToList()).ToList();
            Session["CompletedResult"] = lstResult;
            return PartialView("ResultDetails", lstResult);
        }
            public int GetBetPlaceInterval(string categoryname, string Marketbookname,string Runnerscount)
        {
            return LoggedinUserDetail.GetBetPlacewaitTimerandInterval(categoryname, Marketbookname,Runnerscount);
        }

        public bool GetSession()
        {
            if (LoggedinUserDetail.GetUserID() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public PartialViewResult About()
        {
            return PartialView();
        }
        public bool GetStatus()
        {
            if (LoggedinUserDetail.GetUserID() == 0)
            {
                return false;
            }
            UserStatus objUserStatus = JsonConvert.DeserializeObject<UserStatus>(objUsersServiceCleint.GetUserStatus(LoggedinUserDetail.GetUserID()));
            if (objUserStatus.isBlocked == true || objUserStatus.GTLogin == true || objUserStatus.isDeleted == true || objUserStatus.Loggedin == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public string SendEmailToClient()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                objUsersServiceCleint.SendBalanceSheettoEmail(ConfigurationManager.AppSettings["PasswordForValidate"]);
                return "True";
            }
            else
            {
                return "False";
            }
        }
        public string UpdateHorseRace()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                objUsersServiceCleint.DownloadAllMarketHorseRace(ConfigurationManager.AppSettings["PasswordForValidate"]);

            }
            return "True";
        }
        public PartialViewResult UserDetails(int userID)
        {
            LoggedinUserDetail.CheckifUserLogin();
            if (userID > 0)
            {

                UserDetails objuserDetails = getDetails(userID);
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    List<UserMarketSelection> lstAssignedMarket = JsonConvert.DeserializeObject<List<UserMarketSelection>>(objUsersServiceCleint.GetUserMarketforSelection(userID));
                    string[] AssingedMarket = lstAssignedMarket.Select(i => i.Items).Distinct().ToArray();
                    foreach (var item in AssingedMarket)
                    {
                        if (objuserDetails.AssignedMarket == null)
                        {
                            objuserDetails.AssignedMarket = item;
                        }
                        else
                        {
                            objuserDetails.AssignedMarket = objuserDetails.AssignedMarket + "," + item;
                        }

                    }

                }

                return PartialView(objuserDetails);
            }
            else { return PartialView(new UserDetails()); }
        }
        public PartialViewResult FancysettingsandResultPost()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                List<bfnexchange.Services.DBModel.SP_UserMarket_GetEventLineMarketandOddsForAssociation_Result> lstLinevmarketsandEvents = JsonConvert.DeserializeObject<List<bfnexchange.Services.DBModel.SP_UserMarket_GetEventLineMarketandOddsForAssociation_Result>>(objUsersServiceCleint.GetLineandMatchOddsforAssociation());
                HelperClasses.MarketsForResultPost objMarkesforResultPost = new HelperClasses.MarketsForResultPost();
                if (lstLinevmarketsandEvents.Count > 0)
                {
                    bfnexchange.Services.DBModel.SP_UserMarket_GetEventLineMarketandOddsForAssociation_Result objPlease = new Services.DBModel.SP_UserMarket_GetEventLineMarketandOddsForAssociation_Result();
                    objPlease.EventID = "0";
                    objPlease.EventName = "Please Select";
                    

                    var lstEvents = lstLinevmarketsandEvents.Where(item => item.isLineMarket == 0).ToList();
                    lstEvents.Insert(0, objPlease);
                   
                    objMarkesforResultPost.lstEvents = lstEvents;
                    objMarkesforResultPost.FancyResultsAutomaticPost = objUsersServiceCleint.GetFancyResultPostSetting();
                }
                return PartialView(objMarkesforResultPost);
            }
            else
            {
                return PartialView(new HelperClasses.MarketsForResultPost());
            }
           
        }
        public PartialViewResult GetLinevMarketsbyMarketBookID(string MarketBookId)
        {
          //  var resultslinev = objUsersServiceCleint.GetEventDetailsbyMarketBook(MarketBookId);
            int UserIDforLinevmarkets = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLinevmarkets = 73;
            }
            else
            {
                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            }
            var linevmarkets = JsonConvert.DeserializeObject<List<ExternalAPI.TO.LinevMarkets>>(objUsersServiceCleint.GetLinevMarketsbyEventID(MarketBookId, DateTime.Now, UserIDforLinevmarkets));
            return PartialView("LinevMarkesbyEvent", linevmarkets);
        }
        public string GetScorebyEventID(string MarketBookID)
        {
            var eventdetails = objUsersServiceCleint.GetEventDetailsbyMarketBook(MarketBookID);
            string inningsoverandscore = "";
            var inningsnameandover = eventdetails.MarketCatalogueName.Split(new string[] { " Innings " }, StringSplitOptions.None);
            string innings = Regex.Match(inningsnameandover[0], @"\d+").Value;
            string over = Regex.Match(inningsnameandover[1], @"\d+").Value;
            var matchscores = JsonConvert.DeserializeObject<bfnexchange.Services.DBModel.SP_BallbyBallSummary_GetScoresbyEventIDbyInningsandOvers_Result>(objUsersServiceCleint.GetScorebyEventIDandInningsandOvers(eventdetails.associateeventID, eventdetails.EventOpenDate.Value, Convert.ToInt32(innings), Convert.ToInt32(over)));
            if (matchscores != null)
            {
                inningsoverandscore = innings + "|" + over + "|" + matchscores.Scores.Value.ToString() + "|"+matchscores.TeamName.ToString();
               
            }
            else
            {
                inningsoverandscore = innings + "|" + over + "|" + "0" + "|" + "";
            }
           
            return inningsoverandscore;
        }
        public string PostResultsForMarket(string MarketCatalogueId,int Scores)
        {
            objUsersServiceCleint.CheckforMatchCompletedFancy(MarketCatalogueId, Scores);
            return "True";
        }
        //public string PostResultsForMarketIN(string MarketCatalogueId, int Scores)
        //{
        //    objUsersServiceCleint.CheckforMatchCompletedFancyIN(MarketCatalogueId, Scores);
        //    return "True";
        //}
        public string PostResultsForMarketKJ(string MarketCatalogueId, int Scores)
        {
            objUsersServiceCleint.CheckforMatchCompletedFancyKJ(MarketCatalogueId,469676,Scores);
            return "True";
        }
        public string PostResultsForMarketFig(string MarketCatalogueId, int Scores)
        {
            objUsersServiceCleint.CheckforMatchCompletedFancyFig(MarketCatalogueId,3121,Scores);
            return "True";
        }
        public string PostResultsForMarketSFig(string MarketCatalogueId, int Scores)
        {
            objUsersServiceCleint.CheckforMatchCompletedSmallFig(MarketCatalogueId,1122,Scores);
            return "True";
        }
        public string UpdateAutomaticResultPostSettings(bool automaticresultsetting)
        {
            objUsersServiceCleint.UpdateFancyResultPostSetting(automaticresultsetting);
            return "True";
        }
        public string GetRulesbyMarketnameandCategory(string categoryname,string marketbookname,string Runnerscount)
        {
          List<bftradeline.Models.MarketRules> MarketRulesAll = JsonConvert.DeserializeObject<List<bftradeline.Models.MarketRules>>(objUsersServiceCleint.GetMarketRules());
            if (Runnerscount == "1")
            {
                return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Fancy").Select(item => item.Rules).FirstOrDefault().ToString();
            }
            if (categoryname.Contains("Horse Racing") && !marketbookname.Contains("To Be Placed"))
            {

                return MarketRulesAll.Where(item => item.Category == "Horse Racing" && item.MarketType == "Win").Select(item => item.Rules).FirstOrDefault().ToString();
            }
            else
            {
                if (categoryname.Contains("Horse Racing") && marketbookname.Contains("To Be Placed"))
                {
                    return MarketRulesAll.Where(item => item.Category == "Horse Racing" && item.MarketType == "To Be Placed").Select(item => item.Rules).FirstOrDefault().ToString();

                }
                else
                {
                    if (categoryname.Contains("Greyhound Racing") && marketbookname.Contains("To Be Placed"))
                    {
                        return MarketRulesAll.Where(item => item.Category == "Greyhound Racing" && item.MarketType == "To Be Placed").Select(item => item.Rules).FirstOrDefault().ToString();
                    }
                    else
                    {
                        if (categoryname.Contains("Greyhound Racing") && !marketbookname.Contains("To Be Placed"))
                        {
                            return MarketRulesAll.Where(item => item.Category == "Greyhound Racing" && item.MarketType == "Win").Select(item => item.Rules).FirstOrDefault().ToString();

                        }
                        else
                        {
                            if (marketbookname.Contains("Completed Match"))
                            {
                                return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Completed Match").Select(item => item.Rules).FirstOrDefault().ToString();

                            }
                            else
                            {
                                if (marketbookname.Contains("Innings Runs") || marketbookname.Contains("Inns Runs"))
                                {
                                    return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Innings Runs").Select(item => item.Rules).FirstOrDefault().ToString();

                                }
                                else
                                {
                                    if (categoryname == "Tennis")
                                    {
                                        return MarketRulesAll.Where(item => item.Category == "Tennis").Select(item => item.Rules).FirstOrDefault().ToString();

                                    }
                                    else
                                    {
                                        if (categoryname == "Soccer")
                                        {
                                            return MarketRulesAll.Where(item => item.Category == "Soccer").Select(item => item.Rules).FirstOrDefault().ToString();

                                        }
                                        else
                                        {
                                            if (marketbookname.Contains("Tied Match"))
                                            {
                                                return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Tied Match").Select(item => item.Rules).FirstOrDefault().ToString();

                                            }
                                            else
                                            {
                                                if (marketbookname.Contains("Winner"))
                                                {
                                                    return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Match Odds").Select(item => item.Rules).FirstOrDefault().ToString();

                                                }
                                                else
                                                {
                                                    if (Runnerscount == "3")
                                                    {
                                                        return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Test Match").Select(item => item.Rules).FirstOrDefault().ToString();
                                                    }
                                                    else
                                                    {
                                                        return MarketRulesAll.Where(item => item.Category == "Cricket" && item.MarketType == "Match Odds").Select(item => item.Rules).FirstOrDefault().ToString();
                                                    }


                                                }
                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
        }
        public string VerifyPassword(string password)
        {
            if (LoggedinUserDetail.GetUserPassword() == password)
            {
                return "True";
            }
            else
            {
                return "Incorrect Password";
            }

        }
        public PartialViewResult MyMarket()
        {

            LoggedinUserDetail.CheckifUserLogin();
            List<Models.Event> lstClientlist = JsonConvert.DeserializeObject<List<Models.Event>>(objUsersServiceCleint.GetEventsIDs("0", LoggedinUserDetail.GetUserID()));
            int srnum = 0;
            foreach (var item in lstClientlist)
            {
                if (item.isFavorite == true)
                {
                    item.SrNum = (srnum + 1);
                    srnum = srnum + 1;
                }

            }
            return PartialView("MyMarket", lstClientlist);




        }
        public string GetUserDetailsbyID(int userID)
        {
            LoggedinUserDetail.CheckifUserLogin();
            if (userID > 0)
            {
                var objuserDetails = getDetails(userID);
                var result = ConverttoJSONString(objuserDetails);
                return result;
            }
            else
            {
                return "";
            }
        }
        public UserDetails getDetails(int UserID)
        {
            try
            {

            LoggedinUserDetail.CheckifUserLogin();
            var results = JsonConvert.DeserializeObject<UserDetails>(objUsersServiceCleint.GetUserDetailsbyID(UserID, ConfigurationManager.AppSettings["PasswordForValidate"]));
            UserDetails objuserDetails = new Models.UserDetails();
            objuserDetails = results;
            objuserDetails.Username = Crypto.Decrypt(objuserDetails.Username);
            objuserDetails.Password = Crypto.Decrypt(objuserDetails.Password);
            objuserDetails.RatePercent = Crypto.Decrypt(objuserDetails.RatePercent);

            if (LoggedinUserDetail.GetUserTypeID() != 1)
            {
                //objuserDetails.Password = "";
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
            catch (System.Exception ex)
            {
                return new Models.UserDetails();
            }
        }
        public PartialViewResult AllUsers()
        {

            List<UserIDandUserType> lstUsers = GetUsersbyUsersType();
            return PartialView(lstUsers);
            // if (ViewBag.lstUsers != null)
            // {
            //     List<UserIDandUserType> lstUsers = ViewBag.lstUsers;
            //     return PartialView(lstUsers);
            // }
            //else
            // {

            // }
        }
        public PartialViewResult CuttingUsers()
        {
            List<CuttingUsers> lstusers = GetAllCuttingUsers();
            return PartialView(lstusers);
        }

       
        public string UpdateUserPasswordandStatus(int UserID, bool IsDeleted, bool isBlocked, string Password, string AgentRate, bool LoggedIn, decimal BetLowerLimit, decimal BetUpperLimit, bool isAllowedGrayHound, bool isAllowedHorse, decimal BetLowerLimitHorsePlace, decimal BetUpperLimitHorsePlace, decimal BetLowerLimitGrayHoundWin, decimal BetUpperLimitGrayHoundWin, decimal BetLowerLimitGrayHoundPlace, decimal BetUpperLimitGrayHoundPlace, decimal BetLowerLimitMatchOdds, decimal BetUpperLimitMatchOdds, decimal BetLowerLimitInningsRunns, decimal BetUpperLimitInningsRunns, decimal BetLowerLimitCompletedMatch, decimal BetUpperLimitCompletedMatch, bool isTennisAllowed, bool isSoccerAllowed, int CommissionRate, decimal BetLowerLimitMatchOddsSoccer, decimal BetUpperLimitMatchOddsSoccer, decimal BetLowerLimitMatchOddsTennis, decimal BetUpperLimitMatchOddsTennis, decimal BetUpperLimitTiedMatch, decimal BetLowerLimitTiedMatch, decimal BetUpperLimitWinner, decimal BetLowerLimitWinner)
        {
                            
            UserIDandUserType objSelectedUser =new  UserIDandUserType();
            if (UserID > 0)
            {

                int UpdatedBy = LoggedinUserDetail.GetUserID();
                if (UpdatedBy > 0)
                {
                    if (AgentRate == "")
                    {
                        AgentRate = "0";

                    }
                    if (LoggedinUserDetail.GetUserTypeID() == 2 || LoggedinUserDetail.GetUserTypeID() == 8 || LoggedinUserDetail.GetUserTypeID() == 9)
                    {
                        int CreatedbyID = LoggedinUserDetail.GetUserID();
                        int MaxagentrateLimit = objUsersServiceCleint.GetMaxAgentRate(CreatedbyID);
                        if (Convert.ToInt32(AgentRate) > MaxagentrateLimit)
                        {
                            return "Agent Rate cannot greater than  '"+ MaxagentrateLimit + "' %.";
                        }
                    }
                    AgentRate = Crypto.Encrypt(AgentRate);
                    DateTime updatedtime = DateTime.Now;
                    
                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                        {
                            var results = objUsersServiceCleint.GetAllUsersbyUserTypeNew(UserID,2, ConfigurationManager.AppSettings["PasswordForValidate"]);
                            if (results != "")
                            {
                                List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);

                                foreach (UserIDandUserType objuser in lstUsers)
                                {

                                    objUsersServiceCleint.SetBlockedStatusofUser(Convert.ToInt32(objuser.ID), isBlocked, LoggedinUserDetail.PasswordForValidate);
                                }
                                //LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Block Status");
                            }
                        }
                    if (LoggedinUserDetail.GetUserTypeID() == 9 )
                    {
                        var results = objUsersServiceCleint.GetAllUsersbyUserTypeNew(UserID,8, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        if (results != "")
                        {
                            List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);
                            if (isBlocked == true)
                            {
                                foreach (UserIDandUserType objuser in lstUsers)
                                {
                                    var results2 = objUsersServiceCleint.GetAllUsersbyUserTypeNew(objuser.ID, 2, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                    List<UserIDandUserType> lstUsers2 = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results2);
                                    foreach (UserIDandUserType objuser2 in lstUsers2)
                                    {
                                        objUsersServiceCleint.SetBlockedStatusofUser(Convert.ToInt32(objuser2.ID), isBlocked, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                    }
                                }
                            }
                            if(isBlocked == false)
                            {
                                foreach (UserIDandUserType objuser in lstUsers)
                                {
                                    objUsersServiceCleint.SetBlockedStatusofUser(Convert.ToInt32(objuser.ID), isBlocked, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                }
                                }
                            //LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Block Status");
                        }
                    }
                   
                   
                    
                    objUsersServiceCleint.SetBlockedStatusofUser(UserID, isBlocked, ConfigurationManager.AppSettings["PasswordForValidate"]);
                    LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Block Status");
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        objUsersServiceCleint.SetDeleteStatusofUser(UserID, IsDeleted, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Delete Status");
                    }
                        if (Password.Length >= 6)
                    {
                        objUsersServiceCleint.ResetPasswordofUser(UserID, Crypto.Encrypt(Password), UpdatedBy, updatedtime, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Password");
                    }
                    objUsersServiceCleint.SetAgentRateofUser(UserID, AgentRate, ConfigurationManager.AppSettings["PasswordForValidate"]);
                    LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Set Rate of User " + UserID.ToString() + " " + Crypto.Decrypt(AgentRate).ToString());
                    objUsersServiceCleint.SetLoggedinStatus(UserID, LoggedIn);
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        objUsersServiceCleint.UpdateBetLowerLimit(UserID, BetLowerLimit, BetUpperLimit, isAllowedGrayHound, isAllowedHorse, BetLowerLimitHorsePlace, BetUpperLimitHorsePlace, BetLowerLimitGrayHoundWin, BetUpperLimitGrayHoundWin, BetLowerLimitGrayHoundPlace, BetUpperLimitGrayHoundPlace, BetLowerLimitMatchOdds, BetUpperLimitMatchOdds, BetLowerLimitInningsRunns, BetUpperLimitInningsRunns, BetLowerLimitCompletedMatch, BetUpperLimitCompletedMatch, isTennisAllowed, isSoccerAllowed, CommissionRate, BetLowerLimitMatchOddsSoccer, BetUpperLimitMatchOddsSoccer, BetLowerLimitMatchOddsTennis, BetUpperLimitMatchOddsTennis, BetUpperLimitTiedMatch, BetLowerLimitTiedMatch, BetUpperLimitWinner, BetLowerLimitWinner, ConfigurationManager.AppSettings["PasswordForValidate"],5000,2000);
                    }
                    return "True";
                }
                else
                { return "False"; }
            }
            else { return "False"; }
        }

        public PartialViewResult PoundRate()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                CurrencryConverterService objcurrency = new CurrencryConverterService();
                APIConfigServiceReference.APIConfigServiceClient objAPIConfig = new APIConfigServiceReference.APIConfigServiceClient();
                ViewBag.PoundRate = Convert.ToDecimal(Crypto.Decrypt(objAPIConfig.GetPoundRate()));
                ViewBag.CurrentRate = objcurrency.ConvertCurrencyRates(1).ToString("F2");
                return PartialView("PoundRate");
            }
            else
            {
                ViewBag.PoundRate = "";
                return PartialView("PoundRate");
            }

        }
        public string ResetPoundRate(string Rate)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                if (Rate != "")
                {
                    APIConfigServiceReference.APIConfigServiceClient objAPIConfig = new APIConfigServiceReference.APIConfigServiceClient();
                    objAPIConfig.SetPoundRate(Crypto.Encrypt(Rate));
                    return "True";
                }
                else
                {
                    return "Please enter amount";
                }

            }
            else
            {
                return "False";
            }

        }
        public string ConverttoJSONString(UserDetails result)
        {

            if (result != null)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(result.GetType());
                MemoryStream memoryStream = new MemoryStream();
                serializer.WriteObject(memoryStream, result);

                // Return the results serialized as JSON
                string json = Encoding.Default.GetString(memoryStream.ToArray());
                return json;
            }
            else
            {
                return "";
            }
        }
        public PartialViewResult ManageUser()
        {
            LoggedinUserDetail.CheckifUserLogin();
            ManageUser user = new Models.ManageUser();
            objAccessrightsbyUserType = new AccessRightsbyUserType();
            objAccessrightsbyUserType = JsonConvert.DeserializeObject<AccessRightsbyUserType>(objUsersServiceCleint.GetAccessRightsbyUserType(LoggedinUserDetail.GetUserTypeID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
            user.CanBlockUser = objAccessrightsbyUserType.CanBlockUser;
            user.CanDeleteUser = objAccessrightsbyUserType.CanDeleteUser;
            user.CanChangeAgentRate = objAccessrightsbyUserType.CanChangeAgentRate;
            user.CanChangeLoggedIN = objAccessrightsbyUserType.CanChangeLoggedIN;

            return PartialView(user);
        }
        public PartialViewResult CreateUser()
        {
            LoggedinUserDetail.CheckifUserLogin();

            return PartialView();
        }
        //[HttpPost]
        //[NonAction]
        public string GetUser(CreateUser User)
        {
            if (!User.IsValid())
            {
                return "Enter correct fields.";
            }
            LoggedinUserDetail.CheckifUserLogin();
            var result = objUsersServiceCleint.CheckifUserExists(Crypto.Encrypt(User.UserName.ToLower()));
            if (result == "0")
            {
                int CreatedbyID = LoggedinUserDetail.GetUserID();
                string AccountBalance = objUsersServiceCleint.GetCurrentBalancebyUser(CreatedbyID, ConfigurationManager.AppSettings["PasswordForValidate"]);
                Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);

                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(CreatedbyID);
                    //string AccountBalance = objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, ConfigurationManager.AppSettings["PasswordForValidate"]);
                    // Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);
                    int MaxbalancetransferLimit = objUsersServiceCleint.GetMaxBalanceTransferLimit(CreatedbyID);
                    int MaxagentrateLimit = objUsersServiceCleint.GetMaxAgentRate(CreatedbyID);
                    if (Convert.ToInt32(User.AgentRateC) > MaxagentrateLimit)
                    {
                        return "Agent Rate cannot greater than " + MaxagentrateLimit.ToString() + " %.";
                    }

                    if (User.AccountBalance > MaxbalancetransferLimit && LoggedinUserDetail.GetUserID() != 73)
                    {
                        return "Account Balance should be not be greater than " + MaxbalancetransferLimit.ToString();
                    }
                    if (Newaccountbalance < User.AccountBalance)
                    {
                        return "Account Balance is more than available balance";
                    }
                    else
                    {
                        string userid = objUsersServiceCleint.AddUser(User.Name, User.PhoneNumber, User.EmailAddress, Crypto.Encrypt(User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(User.UserTypeID), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, ConfigurationManager.AppSettings["PasswordForValidate"], 5000, 1000);
                        // objAccountsService.AddtoUsersAccounts("Amount Credit to your account", User.AccountBalance.ToString(), "0.00", Convert.ToInt32(userid), "", DateTime.Now, Crypto.Encrypt(User.AgentRateC), "", 0);
                        objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + User.UserName.ToString() + ")", "0.00", User.AccountBalance.ToString(), HawalaID, "", DateTime.Now, "", "", "", "", Newaccountbalance, false, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, User.AccountBalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        int AhmadRate = objUsersServiceCleint.GetAhmadRate(LoggedinUserDetail.GetUserID());
                        objUsersServiceCleint.UpdateAhmadRate(Convert.ToInt32(userid), AhmadRate);
                        objUsersServiceCleint.UpdateMaxAgentRate(Convert.ToInt32(userid), Convert.ToInt32((User.AgentRateC)));
                        List<AllUserMarkets> lstUserMarket = JsonConvert.DeserializeObject<List<AllUserMarkets>>(objUsersServiceCleint.GetAllUserMarketbyUserID(CreatedbyID));
                        if (lstUserMarket.Count > 0)
                        {
                            List<string> allusersmarket = new List<string>();
                            lstUserMarket = lstUserMarket.Where(x => x.EventTypeID == "4").ToList();
                            foreach (var usermarket in lstUserMarket)
                            {
                                var objusermarket = usermarket.EventTypeID.ToString() + "#" + usermarket.EventTypeName.ToString() + "#" + usermarket.CompetitionID.ToString() + "#" + usermarket.CompetitionName.ToString() + "#" + usermarket.EventID.ToString() + "#" + usermarket.EventName.ToString() + "#" + usermarket.MarketCatalogueID.ToString() + "#" + usermarket.MarketCatalogueName + "#NotInsert#" + usermarket.EventOpenDate.ToString();
                                allusersmarket.Add(objusermarket);
                            }
                            objUsersServiceCleint.InsertUserMarket(allusersmarket.ToArray(), Convert.ToInt32(userid), CreatedbyID, false);
                        }
                        LoggedinUserDetail.InsertActivityLog(CreatedbyID, "Created new user (" + User.UserName.ToString() + ")");
                        return "True";
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 8)
                    {

                        int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(CreatedbyID);
                        //string AccountBalance = objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        // Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);
                        int MaxbalancetransferLimit = objUsersServiceCleint.GetMaxBalanceTransferLimit(CreatedbyID);
                        int MaxagentrateLimit = objUsersServiceCleint.GetMaxAgentRate(CreatedbyID);

                        if (Convert.ToInt32(User.AgentRateC) > MaxagentrateLimit)
                        {
                            return "Agent Rate cannot greater than " + MaxagentrateLimit.ToString() + " %.";
                        }

                        if (User.AccountBalance > MaxbalancetransferLimit && LoggedinUserDetail.GetUserID() != 73)
                        {
                            return "Account Balance should be not be greater than " + MaxbalancetransferLimit.ToString();
                        }
                        if (Newaccountbalance < User.AccountBalance)
                        {
                            return "Account Balance is more than available balance";
                        }

                        string userid = "0";
                        if (Convert.ToInt32(User.UserTypeID) == 4)
                        {
                            userid = objUsersServiceCleint.AddUser(User.Name, User.PhoneNumber, User.EmailAddress, Crypto.Encrypt(User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(User.UserTypeID), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, ConfigurationManager.AppSettings["PasswordForValidate"], 5000, 1000);
                        }
                        else
                        {
                            userid = objUsersServiceCleint.AddUser(User.Name, User.PhoneNumber, User.EmailAddress, Crypto.Encrypt(User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(User.UserTypeID), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, ConfigurationManager.AppSettings["PasswordForValidate"], 5000, 1000);
                        }
                        if (Convert.ToInt32(User.UserTypeID) == 2)
                        {
                            try
                            {
                                string useridHawala = objUsersServiceCleint.AddUser("Hawala", User.PhoneNumber, User.EmailAddress, Crypto.Encrypt("Hawala" + User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(7), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, ConfigurationManager.AppSettings["PasswordForValidate"], 5000, 1000);
                                objUsersServiceCleint.UpdateHawalaIDbyUserID(Convert.ToInt32(useridHawala), Convert.ToInt32(userid));
                                objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + User.UserName.ToString() + ")", "0.00", User.AccountBalance.ToString(), HawalaID, "", DateTime.Now, "", "", "", "", Newaccountbalance, false, "", "", "", "", "");
                                objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, User.AccountBalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                int AhmadRate = objUsersServiceCleint.GetAhmadRate(LoggedinUserDetail.GetUserID());
                                objUsersServiceCleint.UpdateAhmadRate(Convert.ToInt32(userid), AhmadRate);
                                objUsersServiceCleint.UpdateMaxAgentRate(Convert.ToInt32(userid), Convert.ToInt32(User.AgentRateC));
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
                                        var objusermarket = usermarket.EventTypeID.ToString() + "#" + usermarket.EventTypeName.ToString() + "#" + usermarket.CompetitionID.ToString() + "#" + usermarket.CompetitionName.ToString() + "#" + usermarket.EventID.ToString() + "#" + usermarket.EventName.ToString() + "#" + usermarket.MarketCatalogueID.ToString() + "#" + usermarket.MarketCatalogueName + "#NotInsert#" + usermarket.EventOpenDate.ToString();
                                        allusersmarket.Add(objusermarket);
                                    }
                                    objUsersServiceCleint.InsertUserMarket(allusersmarket.ToArray(), Convert.ToInt32(userid), CreatedbyID, false);
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }                   
                        }

                        LoggedinUserDetail.InsertActivityLog(CreatedbyID, "Created new user (" + User.UserName + ")");

                        GetUsersbyUsersType();

                        return "True" + "|" + userid.ToString();
                    }

                    else { 
                       
                            if (LoggedinUserDetail.GetUserTypeID() == 9)
                            {

                                int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(CreatedbyID);
                                //string AccountBalance = objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                // Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);
                                int MaxbalancetransferLimit = objUsersServiceCleint.GetMaxBalanceTransferLimit(CreatedbyID);
                                int MaxagentrateLimit = objUsersServiceCleint.GetMaxAgentRate(CreatedbyID);

                                if (Convert.ToInt32(User.AgentRateC) > MaxagentrateLimit)
                                {
                                    return "Agent Rate cannot greater than " + MaxagentrateLimit.ToString() + " %.";
                                }

                                if (User.AccountBalance > MaxbalancetransferLimit && LoggedinUserDetail.GetUserID() != 73)
                                {
                                    return "Account Balance should be not be greater than " + MaxbalancetransferLimit.ToString();
                                }
                                if (Newaccountbalance < User.AccountBalance)
                                {
                                    return "Account Balance is more than available balance";
                                }

                                string userid = "0";
                                if (Convert.ToInt32(User.UserTypeID) == 4)
                                {
                                    userid = objUsersServiceCleint.AddUser(User.Name, User.PhoneNumber, User.EmailAddress, Crypto.Encrypt(User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(User.UserTypeID), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, ConfigurationManager.AppSettings["PasswordForValidate"], 5000, 1000);
                                }
                                else
                                {
                                    userid = objUsersServiceCleint.AddUser(User.Name, User.PhoneNumber, User.EmailAddress, Crypto.Encrypt(User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(User.UserTypeID), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, ConfigurationManager.AppSettings["PasswordForValidate"], 5000, 1000);
                                }
                                if (Convert.ToInt32(User.UserTypeID) == 8)
                                {
                                    try
                                    {
                                    string useridHawala = objUsersServiceCleint.AddUser("Hawala", User.PhoneNumber, User.EmailAddress, Crypto.Encrypt("Hawala" + User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(7), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, ConfigurationManager.AppSettings["PasswordForValidate"], 5000, 1000);
                                    objUsersServiceCleint.UpdateHawalaIDbyUserID(Convert.ToInt32(useridHawala), Convert.ToInt32(userid));
                                    objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + User.UserName.ToString() + ")", "0.00", User.AccountBalance.ToString(), HawalaID, "", DateTime.Now, "", "", "", "", Newaccountbalance, false, "", "", "", "", "");
                                    objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, User.AccountBalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                    int AhmadRate = objUsersServiceCleint.GetAhmadRate(LoggedinUserDetail.GetUserID());
                                    objUsersServiceCleint.UpdateAhmadRate(Convert.ToInt32(userid), AhmadRate);
                                    objUsersServiceCleint.UpdateMaxAgentRate(Convert.ToInt32(userid), Convert.ToInt32(User.AgentRateC));
                                    objUsersServiceCleint.UpdateSuperRate(Convert.ToInt32(userid), Convert.ToInt32(User.AgentRateC));

                                    List<AllUserMarkets> lstUserMarket = JsonConvert.DeserializeObject<List<AllUserMarkets>>(objUsersServiceCleint.GetAllUserMarketbyUserID(73));
                                        if (lstUserMarket.Count > 0)
                                        {
                                            List<string> allusersmarket = new List<string>();
                                            foreach (var usermarket in lstUserMarket)
                                            {                                            
                                                var orignalopendate = Convert.ToDateTime(usermarket.EventOpenDate).ToString("s");
                                                var objusermarket = usermarket.EventTypeID.ToString() + "#" + usermarket.EventTypeName.ToString() + "#" + usermarket.CompetitionID.ToString() + "#" + usermarket.CompetitionName.ToString() + "#" + usermarket.EventID.ToString() + "#" + usermarket.EventName.ToString() + "#" + usermarket.MarketCatalogueID.ToString() + "#" + usermarket.MarketCatalogueName + "#NotInsert#" + usermarket.EventOpenDate.ToString();
                                                allusersmarket.Add(objusermarket);
                                            }
                                            objUsersServiceCleint.InsertUserMarket(allusersmarket.ToArray(), Convert.ToInt32(userid), CreatedbyID, false);
                                        }
                                    }
                                    catch (System.Exception ex)
                                    {

                                    }

                                   
                                }

                                LoggedinUserDetail.InsertActivityLog(CreatedbyID, "Created new user (" + User.UserName + ")");

                                GetUsersbyUsersType();

                                return "True" + "|" + userid.ToString();
                            }
                       
                    else
                    {
                        string userid = objUsersServiceCleint.AddUser(User.Name, User.PhoneNumber, User.EmailAddress, Crypto.Encrypt(User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(User.UserTypeID), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, ConfigurationManager.AppSettings["PasswordForValidate"], 5000, 1000);
                        //   objAccountsService.AddtoUsersAccounts("Amount Credit to your account", User.AccountBalance.ToString(), "0.00", Convert.ToInt32(userid), "", DateTime.Now, "", "", 0);
                        objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + User.UserName.ToString() + ")", "0.00", User.AccountBalance.ToString(), CreatedbyID, "", DateTime.Now, "", "", "", "", Newaccountbalance, false, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(CreatedbyID, User.AccountBalance, ConfigurationManager.AppSettings["PasswordForValidate"]);

                        LoggedinUserDetail.InsertActivityLog(CreatedbyID, "Created new user (" + User.UserName.ToString() + ")");
                        return "True" + "|" + userid.ToString();
                    }
                }

                }
            }
            else
            {
                return "Username already exists.";
            }


        }
        public bool CheckUsername(string UserName)
        {
            LoggedinUserDetail.CheckifUserLogin();
            var result = objUsersServiceCleint.CheckifUserExists(Crypto.Encrypt(UserName));
            if (result == "0")
            {
                return true;
            }
            else { return false; }
        }
        public List<CuttingUsers> GetAllCuttingUsers()
        {
            LoggedinUserDetail.CheckifUserLogin();
            var results = objUsersServiceCleint.GetAllCuttingUsers(ConfigurationManager.AppSettings["PasswordForValidate"]);
            if (results != "")
            {
                List<CuttingUsers> lstUsers = JsonConvert.DeserializeObject<List<CuttingUsers>>(results);

                foreach (CuttingUsers objuser in lstUsers)
                {
                    objuser.username = Crypto.Decrypt(objuser.username);

                }
                CuttingUsers userdefult = new CuttingUsers();
                userdefult.ID = 0;
                userdefult.username = "Please Select";
                lstUsers.Insert(0, userdefult);
                return lstUsers;

            }
            else
            {
                List<CuttingUsers> lstUsers = new List<CuttingUsers>();
                return lstUsers;
            }
        }
        public List<UserIDandUserType> GetUsersbyUsersType()
        {
            LoggedinUserDetail.CheckifUserLogin();

            var results = "";
           if ( LoggedinUserDetail.GetUserTypeID() != 3)
            {
                results = objUsersServiceCleint.GetAllUsersbyUserType(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.GetUserTypeID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
            }
               
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
                    return lstUsers;

                }         
            
            else
            {
                List<UserIDandUserType> lstUsers = new List<UserIDandUserType>();
                return lstUsers;
            }
        }
        public PartialViewResult AddCredit()
        {
            LoggedinUserDetail.CheckifUserLogin();

            return PartialView();
        }
        public string AddCredittoUser(string AccountBalance, int UserID, string Username, string AccountsTitle, bool IsCredit)
        {
            LoggedinUserDetail.CheckifUserLogin();
            int AddedbyID = LoggedinUserDetail.GetUserID();
            Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);
           
            int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
            int MaxbalancetransferLimit = objUsersServiceCleint.GetMaxBalanceTransferLimit(AddedbyID);
            Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, ConfigurationManager.AppSettings["PasswordForValidate"]));
            if (LoggedinUserDetail.GetUserTypeID() == 2)
            {
                decimal UserCurrentBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserID, ConfigurationManager.AppSettings["PasswordForValidate"]));
                if ((UserCurrentBalance + Newaccountbalance) > MaxbalancetransferLimit && LoggedinUserDetail.GetUserID() != 73)
                {
                    return "Account Balance should be not be greater than " + MaxbalancetransferLimit.ToString();
                }
                if (Newaccountbalance > CurrentAccountBalance)
                {
                    return "Account Balance is more than available balance.";
                }
                else
                {
                    objUsersServiceCleint.AddCredittoUser(Newaccountbalance, UserID, HawalaID, DateTime.Now, 0, true, AccountsTitle, IsCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);
                    if (IsCredit == true)
                    {
                        objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        // objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID, Newaccountbalance);
                    }

                    objAccountsService.AddtoUsersAccounts("Amount removed from your account ( " + AccountsTitle + " for " + Username + " )", "0.00", Newaccountbalance.ToString(), HawalaID, "", DateTime.Now, "", "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");
                    objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);

                    LoggedinUserDetail.InsertActivityLog(HawalaID, "Added Balance " + Newaccountbalance.ToString() + "  to user ( " + Username + " )");
                    return "True";
                }

            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {


                    int HawalaIDSuper = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                    HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                    Newaccountbalance = Convert.ToDecimal(AccountBalance);
                    CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(AddedbyID, ConfigurationManager.AppSettings["PasswordForValidate"]));
                    objUsersServiceCleint.AddCredittoUser(Newaccountbalance, UserID, AddedbyID, DateTime.Now, 0, true, AccountsTitle, IsCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);
                    if (HawalaID > 0)
                    {
                        objUsersServiceCleint.AddCredittoUser(Newaccountbalance, HawalaID, AddedbyID, DateTime.Now, 0, true, AccountsTitle, IsCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);
                    }
                    if (IsCredit == true)
                    {

                        objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        if (HawalaID > 0)
                        {
                            objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        }
                        //  objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID, Newaccountbalance);
                    }
                    objAccountsService.AddtoUsersAccounts("Amount removed from your account( " + AccountsTitle + " for " + Username + "(UserID=" + UserID.ToString() + ") )", "0.00", Newaccountbalance.ToString(), AddedbyID, "", DateTime.Now, "", "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");
                    objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaIDSuper, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                    LoggedinUserDetail.InsertActivityLog(AddedbyID, "Added Balance " + Newaccountbalance.ToString() + " to user ( " + Username + " )");
                    return "True";
                }


                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 9)
                    {


                        int HawalaIDSuper = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                        HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                        Newaccountbalance = Convert.ToDecimal(AccountBalance);
                        CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(AddedbyID, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        objUsersServiceCleint.AddCredittoUser(Newaccountbalance, UserID, AddedbyID, DateTime.Now, 0, true, AccountsTitle, IsCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        if (HawalaID > 0)
                        {
                            objUsersServiceCleint.AddCredittoUser(Newaccountbalance, HawalaID, AddedbyID, DateTime.Now, 0, true, AccountsTitle, IsCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        }
                        if (IsCredit == true)
                        {

                            objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                            if (HawalaID > 0)
                            {
                                objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                            }
                            //  objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID, Newaccountbalance);
                        }
                        objAccountsService.AddtoUsersAccounts("Amount removed from your account( " + AccountsTitle + " for " + Username + "(UserID=" + UserID.ToString() + ") )", "0.00", Newaccountbalance.ToString(), AddedbyID, "", DateTime.Now, "", "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaIDSuper, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        LoggedinUserDetail.InsertActivityLog(AddedbyID, "Added Balance " + Newaccountbalance.ToString() + " to user ( " + Username + " )");
                        return "True";
                    }
                    else
                    {

                        objUsersServiceCleint.AddCredittoUser(Newaccountbalance, UserID, AddedbyID, DateTime.Now, 0, true, AccountsTitle, IsCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        if (IsCredit == true)
                        {

                            objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                            //  objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID, Newaccountbalance);
                        }
                        objAccountsService.AddtoUsersAccounts("Amount removed from your account( " + AccountsTitle + " for " + Username + " )", "0.00", Newaccountbalance.ToString(), AddedbyID, "", DateTime.Now, "", "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(AddedbyID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        LoggedinUserDetail.InsertActivityLog(AddedbyID, "Added Balance " + Newaccountbalance.ToString() + " to user ( " + Username + " )");
                        return "True";

                    }
                }
            }
        }

        public string RemoveCredittoUser(string AccountBalance, int UserID, string Username, string AccountsTitle, bool IsCredit)
        {
            LoggedinUserDetail.CheckifUserLogin();
            if (AccountBalance != "")
            {
                if (UserID > 0)
                {
                    int AddedbyID = LoggedinUserDetail.GetUserID();
                  
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                        
                        Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);
                        int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                        Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        Decimal CurrentAccountBalanceofuser = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserID, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        Decimal AlreadyBalance = Convert.ToDecimal(CurrentAccountBalance);
                        if (Newaccountbalance > CurrentAccountBalanceofuser)
                        {
                            
                            return ("Amount is greater than current balance.");
                        }
                        objUsersServiceCleint.AddCredittoUser(0, UserID, AddedbyID, DateTime.Now, Newaccountbalance, true, AccountsTitle, IsCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);

                        objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + AccountsTitle + " for " + Username + " )", Newaccountbalance.ToString(), "0.00", HawalaID, "", DateTime.Now,"" ,"", "","", CurrentAccountBalance, IsCredit, "", "", "", "","");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(UserID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        if (IsCredit == true)
                        {

                            objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, -1 * Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                            // objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID,-1* Newaccountbalance);
                        }
                        LoggedinUserDetail.InsertActivityLog(AddedbyID, "Removed Balance " + Newaccountbalance.ToString() + "  from user ( " + Username + " )");
                        if (LoggedinUserDetail.GetUserTypeID() == 2)
                        {
                            objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, -1 * Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                            LoggedinUserDetail.InsertActivityLog(HawalaID, "Added Balance " + Newaccountbalance.ToString() + "  to user ( " + LoggedinUserDetail.GetUserName().ToString() + " )");
                        }
                        return "True";
                       
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                        {

                            int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                            int HawalaIDSuper = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                            Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);
                            Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserID, ConfigurationManager.AppSettings["PasswordForValidate"]));
                            //  Decimal CurrentAccountBalanceHawala = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID));
                            Decimal AlreadyBalance = Convert.ToDecimal(CurrentAccountBalance);
                            if (Newaccountbalance > AlreadyBalance)
                            {

                                return ("Amount is greater than current balance.");

                            }
                            objUsersServiceCleint.AddCredittoUser(0, UserID, AddedbyID, DateTime.Now, Newaccountbalance, true, AccountsTitle, IsCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);
                            objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + AccountsTitle + " for " + Username + "(UserID=" + UserID.ToString() + ") )", Newaccountbalance.ToString(), "0.00", AddedbyID, "", DateTime.Now, "", "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");
                            objUsersServiceCleint.UpdateAccountBalacnebyUser(UserID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                            if (HawalaID > 0)
                            {
                                objUsersServiceCleint.AddCredittoUser(0, HawalaID, AddedbyID, DateTime.Now, Newaccountbalance, true, AccountsTitle, IsCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);

                                objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                            }
                            if (IsCredit == true)
                            {

                                objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, -1 * Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                if (HawalaID > 0)
                                {
                                    objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, -1 * Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                }
                                // objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID,-1* Newaccountbalance);
                            }
                            objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaIDSuper, -1 * Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                            LoggedinUserDetail.InsertActivityLog(AddedbyID, "Removed Balance " + Newaccountbalance.ToString() + "  from user ( " + Username + " )");
                            return "True";
                        }


                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 9)
                            {

                                int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                                int HawalaIDSuper = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                                Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);
                                Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserID, ConfigurationManager.AppSettings["PasswordForValidate"]));
                                //  Decimal CurrentAccountBalanceHawala = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID));
                                Decimal AlreadyBalance = Convert.ToDecimal(CurrentAccountBalance);
                                if (Newaccountbalance > AlreadyBalance)
                                {

                                    return ("Amount is greater than current balance.");

                                }
                                objUsersServiceCleint.AddCredittoUser(0, UserID, AddedbyID, DateTime.Now, Newaccountbalance, true, AccountsTitle, IsCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + AccountsTitle + " for " + Username + "(UserID=" + UserID.ToString() + ") )", Newaccountbalance.ToString(), "0.00", AddedbyID, "", DateTime.Now, "", "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");
                                objUsersServiceCleint.UpdateAccountBalacnebyUser(UserID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                if (HawalaID > 0)
                                {
                                    objUsersServiceCleint.AddCredittoUser(0, HawalaID, AddedbyID, DateTime.Now, Newaccountbalance, true, AccountsTitle, IsCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);

                                    objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                }
                                if (IsCredit == true)
                                {

                                    objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, -1 * Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                    if (HawalaID > 0)
                                    {
                                        objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, -1 * Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                    }
                                    // objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID,-1* Newaccountbalance);
                                }
                                objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaIDSuper, -1 * Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                LoggedinUserDetail.InsertActivityLog(AddedbyID, "Removed Balance " + Newaccountbalance.ToString() + "  from user ( " + Username + " )");
                                return "True";
                            }
                            else
                            {
                                int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                                Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, ConfigurationManager.AppSettings["PasswordForValidate"]));
                                Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);
                                objUsersServiceCleint.AddCredittoUser(0, UserID, AddedbyID, DateTime.Now, Newaccountbalance, true, AccountsTitle, IsCredit, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + AccountsTitle + " for " + Username + " )", Newaccountbalance.ToString(), "0.00", AddedbyID, "", DateTime.Now, "", "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");

                                objUsersServiceCleint.UpdateAccountBalacnebyUser(UserID, Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                if (IsCredit == true)
                                {

                                    objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, -1 * Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                    if (HawalaID > 0)
                                    {
                                        objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                                    }
                                    // objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID,-1* Newaccountbalance);
                                }
                                objUsersServiceCleint.UpdateAccountBalacnebyUser(AddedbyID, -1 * Newaccountbalance, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                LoggedinUserDetail.InsertActivityLog(AddedbyID, "Removed Balance " + Newaccountbalance.ToString() + "  from user ( " + Username + " )");
                                return "True";
                            }
                        }
                    }
                }
                else
                {
                    return "False";
                }

            }
            else
            {
                return "Please enter amount.";
            }
        }

      

        public PartialViewResult ResetPasswordView()
        {
            LoggedinUserDetail.CheckifUserLogin();
            return PartialView();
        }
        public PartialViewResult UserBetsbyAgent()
        {
             ViewBag.backgrod = "#1D9BF0";
            ViewBag.color = "white";
                   
            if (LoggedinUserDetail.GetUserTypeID() != 3)
            {
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    List<UserBetsforAgent> lstUserbetsforagent = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                    return PartialView("UserBetsAgent", lstUserbetsforagent);
                }
                else
                {
                    List<UserBetsforAgent> lstUserbetsforagent = new List<UserBetsforAgent>();
                    return PartialView("UserBetsAgent", lstUserbetsforagent);
                }
            }
            else
            {
                List<UserBetsforAgent> lstUserbetsforagent = new List<UserBetsforAgent>();
                return PartialView("UserBetsAgent", lstUserbetsforagent);
            }
        }
        public string ResetPassword(string Password)
        {
            LoggedinUserDetail.CheckifUserLogin();
            if (Password.Length >= 6)
            {
                if (LoggedinUserDetail.GetUserID() > 0)
                {
                    objUsersServiceCleint.ResetPasswordofUser(LoggedinUserDetail.GetUserID(), Crypto.Encrypt(Password), LoggedinUserDetail.GetUserID(), DateTime.Now, ConfigurationManager.AppSettings["PasswordForValidate"]);
                }

                return "True";
            }
            else
            {
                return "False";
            }
        }
        public PartialViewResult Ledger()
        {
            LoggedinUserDetail.CheckifUserLogin();
            return PartialView();
        }

        public PartialViewResult MarketView()
        {
            LoggedinUserDetail.CheckifUserLogin();
            return PartialView();
        }
        public PartialViewResult showcompleteduserbets(string userid, string marektbookID)
        {
             ViewBag.backgrod = "#1D9BF0";
            ViewBag.color = "white";
                   
            LoggedinUserDetail.CheckifUserLogin();
            List<CompletedUserBets> lstCompletedUserBets = JsonConvert.DeserializeObject<List<CompletedUserBets>>(objUsersServiceCleint.GetCompletedMatchedBetsbyUserID(Convert.ToInt32(userid), marektbookID)).ToList();
            
                 //lstCompletedUserBets = lstCompletedUserBets.GroupBy(car => car.MarketBookname).Select(g => g.First()).ToList();
            return PartialView("CompletedUserBets", lstCompletedUserBets);
        }
        public string markid;
       
       
        public ExternalAPI.TO.MarketBook MarketBook = new ExternalAPI.TO.MarketBook();

        public PartialViewResult showcompleteduserbetsFancy(string marektbookID)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 2)
            {
                 ViewBag.backgrod = "#1D9BF0";
                 ViewBag.color = "white";
                   
                LoggedinUserDetail.CheckifUserLogin();

                UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
                List<Models.UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                List<Models.UserBetsforAgent> UserBetsforAgent = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                List<Models.UserBetsForAdmin> UserBetsForAdmin = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                List<Models.UserBetsforSuper> UserBetsForSuper = JsonConvert.DeserializeObject<List<Models.UserBetsforSuper>>(objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = objUserbets.GetBookPosition(marektbookID, UserBetsForAdmin, UserBetsForSuper, UserBetsforAgent, lstUserBets);
                if (CurrentMarketProfitandloss.Runners != null)
                {
                    var lstCurrentMarketBets = lstUserBets.Where(item => item.MarketBookID == marektbookID && item.isMatched == true).ToList();
                    if (lstCurrentMarketBets.Count > 0)
                    {
                        lstCurrentMarketBets = lstCurrentMarketBets.OrderBy(item => Convert.ToInt32(item.UserOdd)).ToList();
                        var maxuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[0].UserOdd) - 1);
                        var minuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[lstCurrentMarketBets.Count - 1].UserOdd) + 1);
                        CurrentMarketProfitandloss.Runners = CurrentMarketProfitandloss.Runners.Where(item => item.Handicap >= minuserodd && item.Handicap <= maxuserodd).ToList();
                    }
                }

                return PartialView("BookPostition", CurrentMarketProfitandloss);
            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {
                     ViewBag.backgrod = "#1D9BF0";
                     ViewBag.color = "white";
                   
                    LoggedinUserDetail.CheckifUserLogin();
                    UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
                    List<Models.UserBets> lstUserBets = (List<Models.UserBets>)Session["userbet"];
                    ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = objUserbets.GetBookPosition(marektbookID, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforAgent>(), lstUserBets);

                    if (CurrentMarketProfitandloss.Runners != null)
                    {
                        var lstCurrentMarketBets = lstUserBets.Where(item => item.MarketBookID == marektbookID && item.isMatched == true).ToList();
                        if (lstCurrentMarketBets.Count > 0)
                        {
                            lstCurrentMarketBets = lstCurrentMarketBets.OrderBy(item => Convert.ToInt32(item.UserOdd)).ToList();
                            var maxuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[0].UserOdd) - 1);
                            var minuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[lstCurrentMarketBets.Count - 1].UserOdd) + 1);
                            CurrentMarketProfitandloss.Runners = CurrentMarketProfitandloss.Runners.Where(item => item.Handicap >= minuserodd && item.Handicap <= maxuserodd).ToList();
                        }
                    }

                    return PartialView("BookPostition", CurrentMarketProfitandloss);


                }

                else
                {


                    LoggedinUserDetail.CheckifUserLogin();
                    UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
                    List<Models.UserBets> lstUserBets = (List<Models.UserBets>)Session["userbet"];
                    ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = objUserbets.GetBookPosition(marektbookID, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforAgent>(), lstUserBets);

                    if (CurrentMarketProfitandloss.Runners != null)
                    {
                        var lstCurrentMarketBets = lstUserBets.Where(item => item.MarketBookID == marektbookID && item.isMatched == true).ToList();
                        if (lstCurrentMarketBets.Count > 0)
                        {
                            lstCurrentMarketBets = lstCurrentMarketBets.OrderBy(item => Convert.ToInt32(item.UserOdd)).ToList();
                            var maxuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[0].UserOdd) - 1);
                            var minuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[lstCurrentMarketBets.Count - 1].UserOdd) + 1);
                            CurrentMarketProfitandloss.Runners = CurrentMarketProfitandloss.Runners.Where(item => item.Handicap >= minuserodd && item.Handicap <= maxuserodd).ToList();
                        }
                    }

                    return PartialView("BookPostition", CurrentMarketProfitandloss);

                }
            }
        
            
        }

        public PartialViewResult showcompleteduserbetsFancyIN(string marektbookID,string selectionID)
        {
            if (LoggedinUserDetail.GetUserTypeID() == 2)
            {
                ViewBag.backgrod = "#1D9BF0";
                ViewBag.color = "white";
                   
                LoggedinUserDetail.CheckifUserLogin();

                UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
                List<Models.UserBetsforAgent> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));               
                ExternalAPI.TO.MarketBookForindianFancy CurrentMarketProfitandloss = objUserbets.GetBookPositionIN(marektbookID, selectionID, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<Models.UserBetsforSamiadmin>(), lstUserBets, new List<Models.UserBets>());
                if (CurrentMarketProfitandloss.RunnersForindianFancy != null)
                {
                    var lstCurrentMarketBets = lstUserBets.Where(item => item.MarketBookID == marektbookID && item.isMatched == true).ToList();
                    if (lstCurrentMarketBets.Count > 0)
                    {
                        lstCurrentMarketBets = lstCurrentMarketBets.OrderBy(item => Convert.ToInt32(item.UserOdd)).ToList();
                        var maxuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[0].UserOdd) - 1);
                        var minuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[lstCurrentMarketBets.Count - 1].UserOdd) + 1);
                        CurrentMarketProfitandloss.RunnersForindianFancy = CurrentMarketProfitandloss.RunnersForindianFancy.Where(item => item.Handicap >= minuserodd && item.Handicap <= maxuserodd).ToList();
                    }
                }

                return PartialView("BookPositionIN", CurrentMarketProfitandloss);
            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {
                     ViewBag.backgrod = "#1D9BF0";
            ViewBag.color = "white";
                   
                    LoggedinUserDetail.CheckifUserLogin();
                    UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
                    List<Models.UserBetsforSuper> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsforSuper>>(objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                    //List<Models.UserBets> lstUserBets = (List<Models.UserBets>)Session["userbet"];
                    ExternalAPI.TO.MarketBookForindianFancy CurrentMarketProfitandloss = objUserbets.GetBookPositionIN(marektbookID, selectionID, new List<Models.UserBetsForAdmin>(), lstUserBets, new List<UserBetsforSamiadmin>(), new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());

                    if (CurrentMarketProfitandloss.RunnersForindianFancy != null)
                    {
                        var lstCurrentMarketBets = lstUserBets.Where(item => item.MarketBookID == marektbookID && item.isMatched == true).ToList();
                        if (lstCurrentMarketBets.Count > 0)
                        {
                            lstCurrentMarketBets = lstCurrentMarketBets.OrderBy(item => Convert.ToInt32(item.UserOdd)).ToList();
                            var maxuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[0].UserOdd) - 1);
                            var minuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[lstCurrentMarketBets.Count - 1].UserOdd) + 1);
                            CurrentMarketProfitandloss.RunnersForindianFancy = CurrentMarketProfitandloss.RunnersForindianFancy.Where(item => item.Handicap >= minuserodd && item.Handicap <= maxuserodd).ToList();
                        }
                    }

                    return PartialView("BookPositionIN", CurrentMarketProfitandloss);
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 9)
                    {
                         ViewBag.backgrod = "#1D9BF0";
            ViewBag.color = "white";
                   
                        LoggedinUserDetail.CheckifUserLogin();
                        UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
                        List<Models.UserBetsforSamiadmin> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBetsforSamiadmin>>(objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                        //List<Models.UserBets> lstUserBets = (List<Models.UserBets>)Session["userbet"];
                        ExternalAPI.TO.MarketBookForindianFancy CurrentMarketProfitandloss = objUserbets.GetBookPositionIN(marektbookID, selectionID, new List<Models.UserBetsForAdmin>(), new List<UserBetsforSuper>(), lstUserBets, new List<Models.UserBetsforAgent>(), new List<Models.UserBets>());

                        if (CurrentMarketProfitandloss.RunnersForindianFancy != null)
                        {
                            var lstCurrentMarketBets = lstUserBets.Where(item => item.MarketBookID == marektbookID && item.isMatched == true).ToList();
                            if (lstCurrentMarketBets.Count > 0)
                            {
                                lstCurrentMarketBets = lstCurrentMarketBets.OrderBy(item => Convert.ToInt32(item.UserOdd)).ToList();
                                var maxuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[0].UserOdd) - 1);
                                var minuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[lstCurrentMarketBets.Count - 1].UserOdd) + 1);
                                CurrentMarketProfitandloss.RunnersForindianFancy = CurrentMarketProfitandloss.RunnersForindianFancy.Where(item => item.Handicap >= minuserodd && item.Handicap <= maxuserodd).ToList();
                            }
                        }

                        return PartialView("BookPositionIN", CurrentMarketProfitandloss);
                    }
                    else
                    {
                        LoggedinUserDetail.CheckifUserLogin();
                        UserBetsUpdateUnmatcedBets objUserbets = new UserBetsUpdateUnmatcedBets();
                        List<Models.UserBets> lstUserBets = (List<Models.UserBets>)Session["userbet"];
                        ExternalAPI.TO.MarketBookForindianFancy CurrentMarketProfitandloss = objUserbets.GetBookPositionIN(marektbookID, selectionID, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(),new List<UserBetsforSamiadmin>(), new List<Models.UserBetsforAgent>(), lstUserBets);
                        if (CurrentMarketProfitandloss.RunnersForindianFancy != null)
                        {
                            var lstCurrentMarketBets = lstUserBets.Where(item => item.MarketBookID == marektbookID && item.isMatched == true).ToList();
                            if (lstCurrentMarketBets.Count > 0)
                            {
                                lstCurrentMarketBets = lstCurrentMarketBets.OrderBy(item => Convert.ToInt32(item.UserOdd)).ToList();
                                var maxuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[0].UserOdd) - 1);
                                var minuserodd = -1 * (Convert.ToInt32(lstCurrentMarketBets[lstCurrentMarketBets.Count - 1].UserOdd) + 1);
                                CurrentMarketProfitandloss.RunnersForindianFancy = CurrentMarketProfitandloss.RunnersForindianFancy.Where(item => item.Handicap >= minuserodd && item.Handicap <= maxuserodd).ToList();
                            }
                        }

                        return PartialView("BookPositionIN", CurrentMarketProfitandloss);

                    }
                }
            }
        }

        public void Getdataforfancybysession(string DateFrom, string DateTo, bool chkfancy)
        {
            try
            {
                //string DateFrom = dtpFrom.SelectedDate.Value.ToString("yyyy-MM-dd");
                //string DateTo = dtpTo.SelectedDate.Value.ToString("yyyy-MM-dd");
                List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    Services.DBModel.SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
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
                else
                {
                  //  lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(LoggedinUserDetail.GetUserID(), DateFrom, DateTo));
                    lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRangeFancywithMArketName(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                }


                if (lstProfitandlossEventtype.Count > 0)
                {
                    if (chkfancy == true)
                    {
                        lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType.Contains("Fancy")).ToList();
                    }
                    lstProfitandlossEventtype = lstProfitandlossEventtype.OrderBy(o => o.EventID).ThenBy(o => o.EventType).ToList();
                   // dgvProfitandLoss.ItemsSource = lstProfitandlossEventtype;
                    //txtTotProfiltandLoss.Content = (lstProfitandlossEventtype.Where(item => !item.EventType.Contains("Commission")).Sum(item => item.NetProfitandLoss) + (-1 * (lstProfitandlossEventtype.Where(item => item.EventType.Contains("Commission")).Sum(item => item.NetProfitandLoss)))).ToString("N0");
                    //txtTotProfiltandLoss.Content = lstProfitandlossEventtype.Where(item => !item.EventType.Contains("Commission")).Sum(item => item.NetProfitandLoss).ToString("N0");
                    //if (Convert.ToDecimal(txtTotProfiltandLoss.Content) >= 0)
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
       

        public PartialViewResult showcompletedevent(string DateFrom, string DateTo,string useridd, string eventtype)
        {
            Decimal TotAdminAmount = 0;
            List<UserAccounts> lstUserAccounts = new List<UserAccounts>();
            var lstUsersSummaryEventtype = new List<PlusMinussummarybyEventTypeandUser>();

            if (eventtype != "")
            {
                List<Services.DBModel.SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result> results = new List<Services.DBModel.SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result>();
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                     ViewBag.backgrod = "#1D9BF0";
                     ViewBag.color = "white";
                   
                    results = objAccountsService.GetAccountsDatabyUserIdDateRangeandEventType(Convert.ToInt32(useridd), DateFrom, DateTo, false, eventtype).ToList();

                }
                else
                {
                    results = objAccountsService.GetAccountsDatabyUserIdDateRangeandEventType(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, false, eventtype).ToList();

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
                //if (Convert.ToInt32(useridd) == 73)
                //{
                //    foreach (UserIDandUserType item in cmbUsersforBalanceSheet.Items)
                //    {
                //        if (item.ID > 0 && item.ID != 73 && item.ID != 3)
                //        {

                //            var resultsAgents = objAccountsService.GetAccountsDatabyUserIdDateRangeandEventType(Convert.ToInt32(item.ID), DateFrom, DateTo, false, EventTypeforaccounts).ToList();
                //            List<UserAccounts> lstUserAccountsForAgent = new List<UserAccounts>();
                //            foreach (var item1 in resultsAgents)
                //            {
                //                UserAccounts objUserAccounts = new UserAccounts();
                //                objUserAccounts.AccountsTitle = item1.AccountsTitle;
                //                objUserAccounts.CreatedDate = item1.CreatedDate;
                //                objUserAccounts.Credit = item1.Credit;
                //                objUserAccounts.Debit = item1.Debit;
                //                objUserAccounts.MarketBookID = item1.MarketBookID;
                //                objUserAccounts.OpeningBalance = (decimal)item1.OpeningBalance;
                //                objUserAccounts.UserID = item1.UserID;
                //                objUserAccounts.UserName = item1.UserName;
                //                objUserAccounts.AgentRate = item1.AgentRate;
                //                lstUserAccountsForAgent.Add(objUserAccounts);
                //            }

                //            if (lstUserAccountsForAgent.Count > 0)
                //            {
                //                lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                //                foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                //                {
                //                    if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                //                    {
                //                        int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                //                        int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                //                        decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);

                //                        if (ActualAmount > 0)
                //                        {
                //                            decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                //                            decimal Comissionamount = 0;
                //                            if (AgentRate == 100)
                //                            {
                //                                Comissionamount = 0;
                //                            }
                //                            else
                //                            {
                //                                Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                //                            }

                //                            TotAdminAmount += -1 * (ActualAmount - (AgentAmount) - Comissionamount);
                //                        }
                //                        else
                //                        {
                //                            ActualAmount = -1 * ActualAmount;
                //                            decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                //                            TotAdminAmount += ActualAmount - AgentAmount;
                //                        }


                //                    }
                //                }
                //                UserAccounts objNewUseAccount = new UserAccounts();
                //                objNewUseAccount.UserName = Crypto.Encrypt(item.UserName);
                //                objNewUseAccount.UserType = "Agent";
                //                objNewUseAccount.UserID = item.ID;
                //                objNewUseAccount.OpeningBalance = objUsersServiceCleint.GetStartingBalance(item.ID, LoggedinUserDetail.PasswordForValidate);
                //                TotAdminAmount = -1 * TotAdminAmount;
                //                if (TotAdminAmount >= 0)
                //                {
                //                    objNewUseAccount.Debit = TotAdminAmount.ToString();
                //                    objNewUseAccount.Credit = "0.00";
                //                }
                //                else
                //                {
                //                    objNewUseAccount.Credit = (-1 * TotAdminAmount).ToString();
                //                    objNewUseAccount.Debit = "0.00";

                //                }
                //                lstUserAccounts.Add(objNewUseAccount);
                //                TotAdminAmount = 0;
                //            }


                //        }

                //    }
                //}


            }
            ///
            if (lstUserAccounts.Count > 0)
            {
                var lstUsers = lstUserAccounts.Select(item => item.UserID).Distinct().ToList();
               
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
                //dgvPlus.ItemsSource = lstPlusCustomers;
               // DGVMinus.ItemsSource = lstMinusCustomers;
            }
            else
            {
                List<PlusMinussummarybyEventTypeandUser> lstPlusCustomers = new List<PlusMinussummarybyEventTypeandUser>();
                List<PlusMinussummarybyEventTypeandUser> lstMinusCustomers = new List<PlusMinussummarybyEventTypeandUser>();
                //dgvPlus.ItemsSource = lstPlusCustomers;
                //DGVMinus.ItemsSource = lstMinusCustomers;
            }
            return PartialView("PlusMinussummarybyEventTypeandUser", lstUsersSummaryEventtype);
        }

        public PartialViewResult ProfitandLoss(string DateFrom, string DateTo, int UserID,bool chkseassion,bool chkfancy,bool chkByMarket, bool chkByMarketCricket)
        {
            DateFrom = ConvertDateFormat(DateFrom);
            DateTo = ConvertDateFormat(DateTo);

            if (chkfancy == true && chkseassion == true)
            {
               Getdataforfancybysession(DateFrom, DateTo, chkfancy);
                //  GetDistinctMatchesfromResults();
                return PartialView("ProfitandLoss", lstProfitandLossAll);
            }
            LoggedinUserDetail.CheckifUserLogin();

            if (chkByMarket == false)
            {           
                List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();
                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                     ViewBag.backgrod = "#1D9BF0";
            ViewBag.color = "white";
                   
                    Services.DBModel.SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                    lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                    List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                    lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.CommisionAccountID), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                    ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                    objProfitandLossCommission.EventType = "Commission";
                    objProfitandLossCommission.NetProfitandLoss = lstProfitandlossEventtypeCommission.Sum(item => item.NetProfitandLoss);
                    lstProfitandlossEventtype.Add(objProfitandLossCommission);
                }
               
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {       
                     ViewBag.backgrod = "#1D9BF0";
                      ViewBag.color = "white";
                   
                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                        lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.UserAccountsGetCommission(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                        objProfitandLossCommission.EventType = "Commission";
                    objProfitandLossCommission.NetProfitandLoss = lstProfitandlossEventtypeCommission.Sum(item => item.NetProfitandLoss);
                    lstProfitandlossEventtype.Add(objProfitandLossCommission);
                }
                    if (LoggedinUserDetail.GetUserTypeID() == 9)
                    {
                     ViewBag.backgrod = "#1D9BF0";
                     ViewBag.color = "white";
                   
                        //Services.DBModel.SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                        lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.UserAccountsGetCommission(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                    //GetDatabyAgentIDForCommisionandDateRange
                        objProfitandLossCommission.EventType = "Commission";
                        objProfitandLossCommission.NetProfitandLoss = lstProfitandlossEventtypeCommission.Sum(item => item.NetProfitandLoss);
                        lstProfitandlossEventtype.Add(objProfitandLossCommission);
                    }
                    else
                {
                   
                    lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                    var data = JsonConvert.DeserializeObject(objUsersServiceCleint.GetDatabyAgentIDForCommisionandDateRange(Convert.ToInt32(LoggedinUserDetail.GetUserID()), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                    ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                    objProfitandLossCommission.EventType = "Commission";
                    objProfitandLossCommission.NetProfitandLoss = Convert.ToDecimal(data); //lstProfitandlossEventtypeCommission.Sum(item => item.NetProfitandLoss);
                    lstProfitandlossEventtype.Add(objProfitandLossCommission);
                }

                if (lstProfitandlossEventtype.Count > 0)
                {
                    if (chkfancy == true)
                    {
                        lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType.Contains("Fancy")).ToList();
                    }
                    if (chkByMarketCricket == true)
                    {
                        lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType == ("Cricket") || item.EventType == ("Fancy")).ToList();
                    }
                   

                    ViewBag.NetProfitorLoss1 = lstProfitandlossEventtype.Where(item => item.EventType != "Commission").Sum(item => item.NetProfitandLoss).ToString("N0");
                   
                }
                else
                {
                    //dgvProfitandLoss.ItemsSource = new List<ProfitandLossEventType>();
                    //txtTotProfiltandLoss.Content = "0.00";
                    //txtTotProfiltandLoss.Background = Brushes.Green;
                    //txtTotProfiltandLoss.Foreground = Brushes.White;
                    //MessageBox.Show("No data found.");
                }
                lstProfitandLossAll = lstProfitandlossEventtype;
            }
            else
            {
                try
                {
                
                    List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();
                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                         ViewBag.backgrod = "#1D9BF0";
            ViewBag.color = "white";
                   
                        Services.DBModel.SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                        lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.CommisionAccountID), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                        foreach (var item in lstProfitandlossEventtypeCommission)
                        {
                            item.EventType = item.EventType + " (Commission)";
                        }

                        lstProfitandlossEventtype.AddRange(lstProfitandlossEventtypeCommission);
                    }

                    if (LoggedinUserDetail.GetUserTypeID() == 8)
                    {
                         ViewBag.backgrod = "#1D9BF0";
            ViewBag.color = "white";
                   
                        //Services.DBModel.SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRange(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                        lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.UserAccountsGetCommission(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                        foreach (var item in lstProfitandlossEventtypeCommission)
                        {
                            item.EventType = item.EventType + " (Commission)";
                        }

                        lstProfitandlossEventtype.AddRange(lstProfitandlossEventtypeCommission);
                    }
                    if (LoggedinUserDetail.GetUserTypeID() == 9)
                    {
                         ViewBag.backgrod = "#1D9BF0";
            ViewBag.color = "white";
                   
                        //Services.DBModel.SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(LoggedinUserDetail.GetUserID());
                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRange(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                        lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.UserAccountsGetCommission(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                        ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                        foreach (var item in lstProfitandlossEventtypeCommission)
                        {
                            item.EventType = item.EventType + " (Commission)";
                        }

                        lstProfitandlossEventtype.AddRange(lstProfitandlossEventtypeCommission);
                    }

                    else
                    {
                        // lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<HelperClasses.ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(LoggedinUserDetail.GetUserID(), DateFrom, DateTo));
                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRange(LoggedinUserDetail.GetUserID(), DateFrom, DateTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                    }


                    if (lstProfitandlossEventtype.Count > 0)
                    {
                        if (chkfancy == true)
                        {
                            lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType.Contains("Fancy")).ToList();
                        }
                        if (chkByMarketCricket == true)
                        {
                            lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.Eventtype1 == ("Cricket") || item.Eventtype1 == ("Fancy")).ToList();
                        }
                     
                    }
                    else
                    {
                        //dgvProfitandLoss.ItemsSource = new List<ProfitandLossEventType>();
                        //txtTotProfiltandLoss.Content = "0.00";
                        //txtTotProfiltandLoss.Background = Brushes.Green;
                        //txtTotProfiltandLoss.Foreground = Brushes.White;
                        //MessageBox.Show("No data found.");
                    }
                    lstProfitandLossAll = lstProfitandlossEventtype;

                }

                catch (System.Exception ex)
                {

                }

                // GetDistinctMatchesfromResults();
            }

            return PartialView("ProfitandLoss", lstProfitandLossAll);
        }
        //private List<Models.EventType> GetMarkeData()
        //{
        //    List<Models.EventType> lstEventType = new List<Models.EventType>();
        //    var marketFilter = new MarketFilter();
        //    ISet<string> eventypeIds = new HashSet<string>();
        //    eventypeIds.Add("1");
        //    eventypeIds.Add("2");
        //    eventypeIds.Add("4");
        //    eventypeIds.Add("7");
        //    eventypeIds.Add("4339");
        //    marketFilter.EventTypeIds = eventypeIds;
        //    var eventTypes = client.listEventTypes(marketFilter);
        //    /////test/////
        //    marketFilter = new MarketFilter();
        //    var competitions = client.listCompetitions(marketFilter);
        //    var events = client.listEvents(marketFilter);
        //    ISet<MarketProjection> marketProjections = new HashSet<MarketProjection>();
        //    marketProjections.Add(MarketProjection.RUNNER_METADATA);
        //    var marketSort = MarketSort.FIRST_TO_START;
        //    var maxResults = "100";
        //    var marketCatalogues = client.listMarketCatalogue(marketFilter, marketProjections, marketSort, maxResults);
        //    return lstEventType;
        //    //////
        //    int eventtypecount = eventTypes.Count;



        //    for (int i = 0; i <= eventtypecount - 1; i++)
        //    {

        //        //Models.EventType objEventtype = new Models.EventType();
        //        //objEventtype.ID = eventTypes[i].EventType.Id;
        //        //objEventtype.Name = eventTypes[i].EventType.Name;

        //        eventypeIds = new HashSet<string>();
        //        eventypeIds.Add(eventTypes[i].EventType.Id);
        //        marketFilter = new MarketFilter();
        //        marketFilter.EventTypeIds = eventypeIds;

        //        // var competitions = client.listCompetitions(marketFilter);
        //        int competitionscount = competitions.Count;
        //        //  List<Models.Competition> lstCompetitions = new List<Models.Competition>();
        //        for (int j = 0; j <= competitionscount - 1; j++)
        //        {

        //            //Models.Competition objCompetition = new Models.Competition();
        //            //objCompetition.ID = competitions[j].Competition.Id;
        //            //objCompetition.Name = competitions[j].Competition.Name;
        //            //lstCompetitions.Add(objCompetition);
        //            ISet<string> competitionIDs = new HashSet<string>();
        //            competitionIDs.Add(competitions[j].Competition.Id);
        //            marketFilter = new MarketFilter();
        //            marketFilter.CompetitionIds = competitionIDs;

        //            //  var events = client.listEvents(marketFilter);
        //            int eventcount = events.Count;
        //            List<Models.Event> lstEvents = new List<Models.Event>();
        //            for (int k = 0; k <= eventcount - 1; k++)
        //            {
        //                //Models.Event objEvent = new Models.Event();
        //                //objEvent.ID = events[k].Event.Id;
        //                //objEvent.Name = events[k].Event.Name;
        //                //lstEvents.Add(objEvent);
        //                ISet<string> EventIDs = new HashSet<string>();
        //                EventIDs.Add(events[k].Event.Id);
        //                marketFilter = new MarketFilter();
        //                marketFilter.EventIds = EventIDs;

        //                //ISet<MarketProjection> marketProjections = new HashSet<MarketProjection>();
        //                //marketProjections.Add(MarketProjection.RUNNER_METADATA);
        //                //var marketSort = MarketSort.FIRST_TO_START;
        //                //var maxResults = "100";
        //                //var marketCatalogues = client.listMarketCatalogue(marketFilter, marketProjections, marketSort, maxResults);
        //                List<Models.MarketCatalgoue> lstMarketCatalogue = new List<MarketCatalgoue>();
        //                int marketcataloguecount = marketCatalogues.Count;
        //                for (int l = 0; l <= marketcataloguecount - 1; l++)
        //                {
        //                    //Models.MarketCatalgoue objMarketCatalogue = new Models.MarketCatalgoue();
        //                    //objMarketCatalogue.ID = marketCatalogues[l].MarketId;
        //                    //objMarketCatalogue.Name = marketCatalogues[l].MarketName;
        //                    //lstMarketCatalogue.Add(objMarketCatalogue);
        //                }
        //                //  objEvent.MarketCatalogue = lstMarketCatalogue;
        //            }
        //            // objCompetition.Event = lstEvents;
        //        }
        //        // objEventtype.Competition = lstCompetitions;
        //        //  lstEventType.Add(objEventtype);
        //    }
        //    // return lstEventType;
        //}
        public PartialViewResult ViewActivity()
        {
            //LoggedinUserDetail.CheckifUserLogin();
            //List<UserIDandUserType> lstUsers = GetUsersbyUsersType();

            //ViewBag.AllUsers = lstUsers;

            return PartialView();
        }
        public PartialViewResult LiabalitybyMarket()
        {
           
                  return PartialView();
        }
        public PartialViewResult EventType()
        {
            //List<Models.EventType> lstClientlist = JsonConvert.DeserializeObject<List<Models.EventType>>(objUsersServiceCleint.GetEventTypeIDs(LoggedinUserDetail.GetUserID()));
            //return PartialView("EventType", lstClientlist);
            if (LoggedinUserDetail.GetUserID() != 1)
            {
                 ViewBag.backgrod = "#1D9BF0";
                 ViewBag.color = "white";
                   
                List<UserMarket> lstUserMarket = new List<UserMarket>();
                lstUserMarket = JsonConvert.DeserializeObject<List<UserMarket>>(objUsersServiceCleint.GetUserMArket(LoggedinUserDetail.GetUserID()));
                Session["usermarket"] = lstUserMarket;
                var eventypeIDsbyuser = lstUserMarket.Select(i => i.EventTypeID).Distinct().ToArray();
                ISet<string> eventypeIds = new HashSet<string>();
                foreach (var item in eventypeIDsbyuser)
                {
                    eventypeIds.Add(item);
                }

            }
            try
            {
                // marketFilter.TextQuery = "Soccer, Tennis,Cricket,Horse Racing,Greyhound Racing";
                var eventTypes = client.listEventTypes(false, ConfigurationManager.AppSettings["PasswordForValidate"]);
                // List<Models.EventType> lstClientlist = JsonConvert.DeserializeObject<List<Models.EventType>>(objUsersServiceCleint.GetEventTypeIDs(LoggedinUserDetail.GetUserID()));
                return PartialView("EventType", eventTypes);
            }
            catch (System.Exception ex)
            {
                var eventTypes = client.listEventTypes(false, ConfigurationManager.AppSettings["PasswordForValidate"]);
                return PartialView("EventType", eventTypes);
            }

        }

        public PartialViewResult Competiion(string ID)
        {

            try
            {


                LoggedinUserDetail.CheckifUserLogin();


                if (LoggedinUserDetail.GetUserID() != 1)
                {

                    List<UserMarket> lstUserMarket = Session["usermarket"] as List<UserMarket>;
                    var competiotionIDsbyuser = lstUserMarket.Select(i => i.CompetitionID).Distinct().ToArray();
                    ISet<string> competitionIDs = new HashSet<string>();
                    foreach (var item in competiotionIDsbyuser)
                    {
                        competitionIDs.Add(item);
                    }


                }
                if (ID == "7")
                {

                }
                //List<Models.Competition> lstClientlist = JsonConvert.DeserializeObject<List<Models.Competition>>(objUsersServiceCleint.GetCompetitionIDs(ID.ToString(), LoggedinUserDetail.GetUserID()));
                //return PartialView("Competition", lstClientlist);

                var competitions = client.listCompetitions(ID, false, ConfigurationManager.AppSettings["PasswordForValidate"]);

                return PartialView("Competition", competitions);
            }
            catch (System.Exception ex)
            {
                var competitions = client.listCompetitions(ID, false, ConfigurationManager.AppSettings["PasswordForValidate"]);

                return PartialView("Competition", competitions);
            }
        }
        public PartialViewResult Events(string ID)
        {
            LoggedinUserDetail.CheckifUserLogin();
            //List<Models.Event> lstClientlist = JsonConvert.DeserializeObject<List<Models.Event>>(objUsersServiceCleint.GetEventsIDs(ID.ToString(), LoggedinUserDetail.GetUserID()));
            //return PartialView("Events", lstClientlist);
            if (LoggedinUserDetail.GetUserID() != 1)
            {

                List<UserMarket> lstUserMarket = Session["usermarket"] as List<UserMarket>;
                var EventIDsbyuser = lstUserMarket.Select(i => i.EventID).Distinct().ToArray();
                ISet<string> EventIDs = new HashSet<string>();
                foreach (var item in EventIDsbyuser)
                {
                    EventIDs.Add(item);
                }


            }

            var events = client.listEvents(ID, false, ConfigurationManager.AppSettings["PasswordForValidate"]);
            return PartialView("Events", events);
        }
        public PartialViewResult MarketCatalogue(string ID)
        {
            LoggedinUserDetail.CheckifUserLogin();
            //List<Models.MarketCatalgoue> lstCatalogueIDs = JsonConvert.DeserializeObject<List<Models.MarketCatalgoue>>(objUsersServiceCleint.GetMarketCatalogueIDs(ID.ToString(), LoggedinUserDetail.GetUserID()));
            //foreach (var item in lstCatalogueIDs)
            //{
            //    item.Runner = new List<Models.Runner>();
            //    var results = (objUsersServiceCleint.GetSelectionNamesbyMarketID(item.ID)).ToList();
            //    foreach(var items in results)
            //    {
            //        Models.Runner runneritem = new Models.Runner();
            //        runneritem.SelectionID = items.SelectionID.ToString();
            //        runneritem.SelectionName = items.SelectionName;
            //        item.Runner.Add(runneritem);
            //    }

            //}
            //return PartialView("MarketCatalogue", lstCatalogueIDs);

            if (LoggedinUserDetail.GetUserID() != 1)
            {

                List<UserMarket> lstUserMarket = Session["usermarket"] as List<UserMarket>;
                var marketCatalogueIDsbyuser = lstUserMarket.Select(i => i.MarketCatalogueID).Distinct().ToArray();
                ISet<string> MarketCatalogueIDs = new HashSet<string>();
                foreach (var item in marketCatalogueIDsbyuser)
                {
                    MarketCatalogueIDs.Add(item);
                }


            }
            List<string> lstMarketCatalogueIDs = new List<string>();

            var marketCatalogues = client.listMarketCatalogue(ID, lstMarketCatalogueIDs.ToArray(), false, ConfigurationManager.AppSettings["PasswordForValidate"]);

            return PartialView("MarketCatalogue", marketCatalogues);
        }

        public string ConvertDateFormat(string datetoconvert)
        {
            string[] datearr = datetoconvert.Split('-');
            datetoconvert = datearr[2].ToString() + "-" + datearr[1].ToString() + "-" + datearr[0].ToString();
            return datetoconvert;
        }
        public PartialViewResult LedgerDetails(string DateFrom, string DateTo, int UserID, bool isCredit)
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
                return PartialView("LedgerDetails", lstUserAccounts);
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                List<UserAccounts> lstUserAccounts = new List<UserAccounts>();
                return PartialView("LedgerDetails", lstUserAccounts);
            }

        }
        public PartialViewResult TransferBalance()
        {
            List<UserIDandUserType> lstUsers = GetUsersbyUsersType();
            return PartialView(lstUsers);
        }
        public string GetCurrentBalanceofUser(int userID)
        {
            LoggedinUserDetail.CheckifUserLogin();
            return objUsersServiceCleint.GetCurrentBalancebyUser(userID, ConfigurationManager.AppSettings["PasswordForValidate"]);
        }
        public string TransferBalanceFromOneUsertoAnother(decimal AccountBalance, int UserIDFrom, string UsernameFrom, string AccountsTitle, int UserIDTo, string UsernameTo)
        {
            LoggedinUserDetail.CheckifUserLogin();
            if (LoggedinUserDetail.GetUserTypeID() != 3)
            {
                decimal UserFromBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserIDFrom, ConfigurationManager.AppSettings["PasswordForValidate"]));
                decimal UserToBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserIDTo, ConfigurationManager.AppSettings["PasswordForValidate"]));
                if (UserFromBalance < AccountBalance)
                {
                    return "Amount is less than available balance";
                }
                objAccountsService.AddtoUsersAccounts(AccountsTitle, AccountBalance.ToString(), "0.00", UserIDTo, "", DateTime.Now,"","", "","", UserToBalance, false, "", "", "", "","");
                objUsersServiceCleint.AddCredittoUser(AccountBalance, UserIDTo, LoggedinUserDetail.GetUserID(), DateTime.Now, 0, false, AccountsTitle, false, ConfigurationManager.AppSettings["PasswordForValidate"]);
                objAccountsService.AddtoUsersAccounts(AccountsTitle, "0.00", AccountBalance.ToString(), UserIDFrom, "", DateTime.Now,"","", "","", UserFromBalance, false, "", "", "", "","");
                objUsersServiceCleint.AddCredittoUser(-1 * AccountBalance, UserIDFrom, LoggedinUserDetail.GetUserID(), DateTime.Now, 0, false, AccountsTitle, false, ConfigurationManager.AppSettings["PasswordForValidate"]);
                objUsersServiceCleint.AddUserActivity("Transfer Balance From " + UsernameFrom + " To " + UsernameTo + " ( " + AccountBalance.ToString() + " ).", DateTime.Now, LoggedinUserDetail.GetIPAddress(), "", "", LoggedinUserDetail.GetUserID());
            }
            return "True";
        }
        public PartialViewResult BalanceSheet( int UserID, bool isCredit)
        {
            LoggedinUserDetail.CheckifUserLogin();
            try
            {

                if (UserID == 0)
                {
                    UserID = LoggedinUserDetail.GetUserID();
                }
                List<BalanceSheet> lstBalanceSheet = new List<Models.BalanceSheet>();
                if (LoggedinUserDetail.GetUserTypeID() != 3 && LoggedinUserDetail.GetUserTypeID() != 8 && LoggedinUserDetail.GetUserTypeID() != 9)
                {
                    if (UserID == 73)
                    { }
                    List<UserAccounts> lstUserAccounts = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(UserID, isCredit, ConfigurationManager.AppSettings["PasswordForValidate"]));
                    if (lstUserAccounts.Count > 0)
                    {
                        var lstUserIDs = lstUserAccounts.Select(item1 => new { item1.UserID }).Distinct().ToArray();

                        foreach (var useritem in lstUserIDs)
                        {
                            List<UserAccounts> lstUseraccountsbyUser = lstUserAccounts.Where(item2 => item2.UserID == useritem.UserID).ToList();
                            if (lstUseraccountsbyUser.Count > 0)
                            {
                                BalanceSheet objBalanceSheet = new Models.BalanceSheet();
                                objBalanceSheet.Username = Crypto.Decrypt(lstUseraccountsbyUser[0].UserName);
                                objBalanceSheet.StartingBalance = lstUseraccountsbyUser[0].OpeningBalance;
                                decimal Profitorloss = lstUseraccountsbyUser.Sum(item3 => Convert.ToDecimal(item3.Debit)) - lstUseraccountsbyUser.Sum(item3 => Convert.ToDecimal(item3.Credit));
                                if (LoggedinUserDetail.GetUserTypeID() == 8)
                                {
                                    if (Profitorloss < 0)
                                    {
                                        decimal a = -1 * (Profitorloss);
                                        objBalanceSheet.Profit = a;
                                    }
                                    else
                                    {
                                        decimal a = -1 * (Profitorloss);
                                        objBalanceSheet.Loss = a;
                                    }
                                }
                                else
                                {
                                    if (Profitorloss >= 0)
                                    {
                                        objBalanceSheet.Profit = Profitorloss;
                                    }
                                    else
                                    {
                                        objBalanceSheet.Loss = Profitorloss;
                                    }
                                }
                                lstBalanceSheet.Add(objBalanceSheet);
                            }
                        }

                    }

                    if (LoggedinUserDetail.GetUserTypeID() != 8)
                    {

                        List<UserAccounts> lstUserAccountsAdmin = new List<UserAccounts>();

                        lstUserAccountsAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(UserID, isCredit, ConfigurationManager.AppSettings["PasswordForValidate"]));
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


                    int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                    decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, ConfigurationManager.AppSettings["PasswordForValidate"]));

                    decimal StartingBalance = Convert.ToDecimal(objUsersServiceCleint.GetStartingBalance(HawalaID, ConfigurationManager.AppSettings["PasswordForValidate"]));
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

                if (LoggedinUserDetail.GetUserTypeID() == 1)
                {
                    List<UserAccounts> lstUserAccountsAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(LoggedinUserDetail.GetUserID(), isCredit, ConfigurationManager.AppSettings["PasswordForValidate"]));
                    BalanceSheet objBalanceSheet1 = new Models.BalanceSheet();
                    objBalanceSheet1.Username = "Book account";
                    objBalanceSheet1.StartingBalance = lstUserAccountsAdmin[0].OpeningBalance;
                    lstUserAccountsAdmin = lstUserAccountsAdmin.Where(item => item.MarketBookID != "").ToList();
                    decimal Profitorloss = lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Debit)) - lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Credit));
                    if (Profitorloss >= 0)
                    {
                        objBalanceSheet1.Profit = Profitorloss;

                    }
                    else
                    {
                        objBalanceSheet1.Loss = Profitorloss;
                    }

                    lstBalanceSheet.Add(objBalanceSheet1);
                    List<UserAccounts> lstUserAccountsCommission = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForCommisionaccount(ConfigurationManager.AppSettings["PasswordForValidate"]));
                    BalanceSheet objBalanceSheet2 = new Models.BalanceSheet();
                    objBalanceSheet2.Username = "Commission";
                    objBalanceSheet2.StartingBalance = 0;
                    decimal Profitorloss1 = lstUserAccountsCommission.Sum(item3 => Convert.ToDecimal(item3.Credit));
                    if (Profitorloss1 >= 0)
                    {
                        objBalanceSheet2.Profit = Profitorloss1;

                    }
                    else
                    {
                        objBalanceSheet2.Loss = Profitorloss1;
                    }
                    lstBalanceSheet.Add(objBalanceSheet2);
                }

                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {
                     ViewBag.backgrod = "#1D9BF0";
            ViewBag.color = "white";
                   
                    List<BalanceSheet> objBalanceSheetforsuper = new List<Models.BalanceSheet>();
                    Decimal TotAdminAmount = 0;
                    var results = objUsersServiceCleint.GetAllUsersbyUserType(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.GetUserTypeID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                    if (results != "")
                    {
                        List<UserIDandUserType> lstonlyAgentsAllforSuper = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);
                        lstonlyAgentsAllforSuper = lstonlyAgentsAllforSuper.Where(x => x.UserTypeID == 2).ToList();
                        foreach (var item in lstonlyAgentsAllforSuper)
                        {
                            BalanceSheet objBalanceSheet = new Models.BalanceSheet();
                            decimal SuperAmount = 0;
                            decimal SuperAmount1 = 0;

                            if (item.ID > 0 && item.ID != 73 && item.ID != 3)
                            {
                                List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(item.ID, isCredit, ConfigurationManager.AppSettings["PasswordForValidate"]));
                                if (lstUserAccountsForAgent.Count > 0)
                                {
                                    List<UserAccounts> lstUserAccountsForAgentCommssion = new List<UserAccounts>();
                                    lstUserAccountsForAgentCommssion= lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle == "Commission").ToList();
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
                                                decimal ComissionamountAgent = 0;
                                                if (AgentRate == 100)
                                                {
                                                    Comissionamount = 0;
                                                }
                                                else
                                                {
                                                    Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                                                    ComissionamountAgent = Math.Round(((Convert.ToDecimal(AgentRate) / 100) * Comissionamount), 2);
                                                    Comissionamount = Comissionamount - ComissionamountAgent;
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
                                    BalanceSheet objBalanceSheet2 = new Models.BalanceSheet();
                                    
                                    objBalanceSheet2.Username = Crypto.Decrypt(item.UserName);
                                    //objBalanceSheet2. = "Agent";
                                    objBalanceSheet2.UserID = item.ID;
                                    objBalanceSheet2.StartingBalance = objUsersServiceCleint.GetStartingBalance(item.ID, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                    TotAdminAmount = -1 * TotAdminAmount;
                                    if (TotAdminAmount >= 0)
                                    {
                                        objBalanceSheet2.Profit = TotAdminAmount;                                    
                                    }
                                    else
                                    {
                                        objBalanceSheet2.Loss =  TotAdminAmount;                                     
                                    }
                                    lstBalanceSheet.Add(objBalanceSheet2);
                                    TotAdminAmount = 0;
                                }
                            }

                            List<UserAccounts> lstUserAccountsAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(LoggedinUserDetail.GetUserID(), isCredit, ConfigurationManager.AppSettings["PasswordForValidate"]));
                            
                            BalanceSheet objBalanceSheet1 = new Models.BalanceSheet();
                            objBalanceSheet1.Username = "Book account";
                            objBalanceSheet1.StartingBalance = lstUserAccountsAdmin[0].OpeningBalance;
                            lstUserAccountsAdmin = lstUserAccountsAdmin.Where(item4 => item4.MarketBookID != "").ToList();
                            decimal Profitorloss = lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Debit)) - lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Credit));
                            if (Profitorloss >= 0)
                            {
                                objBalanceSheet1.Profit = Profitorloss;
                            }
                            else
                            {
                                objBalanceSheet1.Loss = Profitorloss;
                            }

                            lstBalanceSheet.Add(objBalanceSheet1);
                        }
                    }
                }

                if (LoggedinUserDetail.GetUserTypeID() == 9)
                {
                     ViewBag.backgrod = "#1D9BF0";
            ViewBag.color = "white";
                   
                    List<BalanceSheet> objBalanceSheetforsuper = new List<Models.BalanceSheet>();
                    Decimal TotAdminAmount = 0;
                    var results = objUsersServiceCleint.GetAllUsersbyUserType(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.GetUserTypeID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                    if (results != "")
                    {
                        List<UserIDandUserType> lstonlySuperAllforSamiadmin = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);
                        lstonlySuperAllforSamiadmin = lstonlySuperAllforSamiadmin.Where(x => x.UserTypeID == 8).ToList();
                        foreach (var items in lstonlySuperAllforSamiadmin)
                        {
                            var results1 = objUsersServiceCleint.GetAllUsersbyUserType(items.ID,8, ConfigurationManager.AppSettings["PasswordForValidate"]);
                            List<UserIDandUserType> lstonlyAgentsAllforSuper = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results1);
                            lstonlyAgentsAllforSuper = lstonlyAgentsAllforSuper.Where(x => x.UserTypeID == 2).ToList();
                        
                        foreach (var item in lstonlyAgentsAllforSuper)
                        {
                            BalanceSheet objBalanceSheet = new Models.BalanceSheet();
                           

                            if (item.ID > 0 && item.ID != 73 && item.ID != 3)
                            {
                                List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(item.ID, isCredit, ConfigurationManager.AppSettings["PasswordForValidate"]));
                                if (lstUserAccountsForAgent.Count > 0)
                                {
                                        decimal SuperAmount1 = 0;
                                        List<UserAccounts> lstUserAccountsForAgentCommssion = new List<UserAccounts>();
                                    lstUserAccountsForAgentCommssion = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle == "Commission").ToList();
                                    lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                                    foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                                    {
                                           
                                            if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                                        {
                                            int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                            int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                            int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                            int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiadminRate);
                                            decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                            decimal superpercent = 0;                                            
                                            decimal samiadminercent = 0;
                                              
                                                if (SuperRate > 0)
                                            {
                                                superpercent = SuperRate - AgentRate;
                                            }
                                            else
                                            {
                                                superpercent = 0;
                                            }
                                                if (SamiadminRate > 0)
                                                {
                                                    samiadminercent = SamiadminRate -(superpercent + AgentRate);
                                                }
                                                else
                                                {
                                                    samiadminercent = 0;
                                                }

                                                if (ActualAmount > 0)
                                            {
                                                    decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                    decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminercent) / 100) * ActualAmount, 2);
                                                    decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                decimal Comissionamount = 0;
                                                decimal ComissionamountAgent = 0;
                                                    decimal ComissionamountSuper = 0;
                                                    if (AgentRate == 100)
                                                {
                                                    Comissionamount = 0;
                                                }
                                                else
                                                {
                                                    Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                                                    ComissionamountAgent = Math.Round((Convert.ToDecimal(AgentRate) / 100) * Comissionamount, 2);
                                                        ComissionamountSuper = Math.Round((Convert.ToDecimal(superpercent) / 100) * Comissionamount, 2);
                                                        Comissionamount = Comissionamount - ComissionamountAgent;
                                                        Comissionamount = Comissionamount - ComissionamountSuper;
                                                    }

                                                TotAdminAmount += -1 * (ActualAmount - (AgentAmount + SuperAmount + SamiadminAmount) - Comissionamount);

                                                SuperAmount1 += -1 * SamiadminAmount;
                                            }
                                            else
                                            {
                                                ActualAmount = -1 * ActualAmount;
                                               decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                    decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminercent) / 100) * ActualAmount, 2);
                                                    decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                TotAdminAmount += ActualAmount - (AgentAmount + SuperAmount + SamiadminAmount);
                                                SuperAmount1 += SamiadminAmount;

                                            }

                                        }
                                    }
                                    TotAdminAmount = (TotAdminAmount) + (SuperAmount1);
                                    BalanceSheet objBalanceSheet2 = new Models.BalanceSheet();
                                    //objBalanceSheet2.Username = item.UserName;
                                    objBalanceSheet2.Username = items.UserName;
                                    //objBalanceSheet2. = "Agent";
                                    objBalanceSheet2.UserID = items.ID;
                                    objBalanceSheet2.StartingBalance = objUsersServiceCleint.GetStartingBalance(items.ID, ConfigurationManager.AppSettings["PasswordForValidate"]);
                                    TotAdminAmount = -1 * TotAdminAmount;
                                    if (TotAdminAmount >= 0)
                                    {
                                        objBalanceSheet2.Profit = TotAdminAmount;
                                        //objNewUseAccount.Credit = "0.00";
                                    }
                                    else
                                    {
                                        objBalanceSheet2.Loss = TotAdminAmount;
                                        //objNewUseAccount.Debit = "0.00";

                                    }
                                    lstBalanceSheet.Add(objBalanceSheet2);
                                    TotAdminAmount = 0;
                                }
                            }
                        }
                    }
                    }

                            List<UserAccounts> lstUserAccountsAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(LoggedinUserDetail.GetUserID(), isCredit, ConfigurationManager.AppSettings["PasswordForValidate"]));

                            BalanceSheet objBalanceSheet1 = new Models.BalanceSheet();
                            objBalanceSheet1.Username = "Book account";
                            objBalanceSheet1.StartingBalance = lstUserAccountsAdmin[0].OpeningBalance;
                            lstUserAccountsAdmin = lstUserAccountsAdmin.Where(item4 => item4.MarketBookID != "").ToList();
                            decimal Profitorloss = lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Debit)) - lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Credit));
                            if (Profitorloss >= 0)
                            {
                                objBalanceSheet1.Profit = Profitorloss;
                            }
                            else
                            {
                                objBalanceSheet1.Loss = Profitorloss;
                            }

                            lstBalanceSheet.Add(objBalanceSheet1);
                        }
                    
                
                return PartialView("BalanceSheet", lstBalanceSheet);
                
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                List<BalanceSheet> lstBalanceSheet = new List<BalanceSheet>();
                return PartialView("BalanceSheet", lstBalanceSheet);
            }

        }
        public void DownloadMarket()
        {
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                // objUsersServiceCleint.DownloadAllMarket();
            }
        }
        public string UpdateUserMarket(string[] allitemsarr, int userID,bool DeleteOldMarket)
        {
            LoggedinUserDetail.CheckifUserLogin();
            try
            {
                if (userID > 0)
                {

                    if (LoggedinUserDetail.GetUserTypeID() == 1)
                    {
                        objUsersServiceCleint.InsertUserMarket(allitemsarr, userID, LoggedinUserDetail.GetUserID(),DeleteOldMarket);
                        return "True";
                    }
                }
            }
            catch (System.Exception ex)
            {
                return "False";
            }
            return "True";

        }
       
       
        public List<TodayHorserace> getmarkets()
        {
            List<TodayHorserace> market = new List<TodayHorserace>();
            for(int i=0;i<30;i++)
            {
                TodayHorserace g = new TodayHorserace();
                g.MarketBookID = "123535" + i;
                g.RaceName = "Deauville (FRA) 30th Nov" + i;
                market.Add(g);
            }
            return market;
        }
        BettingServiceClient objBettingClient = new BettingServiceClient();
        List<RootSCT> rootsct = new List<RootSCT>();
        public   PartialViewResult  GetDefaultPageData()
        {
            try
            {
                int userid = LoggedinUserDetail.GetUserID();
                if (userid == 1)
                {
                    userid = 73;
                }
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                var results = objUsersServiceCleint.GetInPlayMatcheswithRunners1(userid);
                List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(results);
                List<string> lstIds = lstInPlayMatches.Where(item => item.EventTypeName == "Cricket").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList();
                lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Soccer").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Tennis").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Horse Racing").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Greyhound Racing").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                List<AllMarketsInPlay> lstGridMarkets = new List<AllMarketsInPlay>();
                   

                    //https://ips.betfair.com/inplayservice/v1.0/eventsInPlay?locale=en&regionCode=UK&alt=json&maxResults=500&eventTypeIds=1


                    foreach (var item in lstIds)
                {
                    try
                    {
                        InPlayMatches objMarketLocal = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).FirstOrDefault();
                        AllMarketsInPlay objGridMarket = new AllMarketsInPlay();
                        objGridMarket.CategoryName = objMarketLocal.EventTypeName;
                        objGridMarket.MarketBookID = objMarketLocal.MarketCatalogueID;
                        objGridMarket.MarketBookName = objMarketLocal.MarketCatalogueName;
                        objGridMarket.EventName = objMarketLocal.EventName;
                        objGridMarket.CompetitionName = objMarketLocal.CompetitionName;
                        objGridMarket.MarketStartTime = objMarketLocal.EventOpenDate.Value.AddHours(5).ToString("dd-MM-yyyy hh:mm tt");
                        objGridMarket.MarketStatus = objMarketLocal.MarketStatus;

                        List<InPlayMatches> lstRunnersID = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).ToList();
                        try
                        {
                            objGridMarket.Runner1 = lstRunnersID.Where(x => !x.SelectionName.Contains("Draw")).FirstOrDefault().SelectionName;
                            objGridMarket.Runner2 = lstRunnersID.Where(x => !x.SelectionName.Contains("Draw")).LastOrDefault().SelectionName;
                            if (lstRunnersID.Count == 3)
                            {
                                objGridMarket.Runner3 = lstRunnersID.Where(x => x.SelectionName.Contains("Draw")).FirstOrDefault().SelectionName;
                            }
                        }
                        catch (System.Exception ex)
                        { }
                        lstGridMarkets.Add(objGridMarket);
                    }
                    catch (System.Exception ex)
                    { }
                }


                var model = new DefaultPageModel();
                //model.TodayHorseRacing = getmarkets();
                //model.TodayGreyRacing = getmarkets();
                 model.WelcomeMessage = "Please enjoy the non-stop intriguing betting experience only on www.gt-exch.com. Thanks";
                 model.WelcomeHeading = "Notice";
                 model.Rule = "Rule & Regs";
                 model.WelcomeMessage ="All bets apply to Full Time according to the match officials, plus any stoppage time. Extra - time / penalty shoot - outs are not included.If this market is re - opened for In - Play betting, unmatched bets will be cancelled at kick off and the market turned in play.The market will be suspended if it appears that a goal has been scored, a penalty will be given, or a red card will be shown.With the exception of bets for which the 'keep' option has been selected, unmatched bets will be cancelled in the event of a confirmed goal or sending off.Please note that should our data feeds fail we may be unable to manage this game in-play.Customers should be aware   that:Transmissions described as â€œliveâ€ by some broadcasters may actually be delayed.The extent of any such delay may vary, depending on the set-up through which they are receiving pictures or data.If this market is scheduled to go in-play, but due to unforeseen circumstances we are unable to offer the market in-play, then this market will be re-opened for the half-time interval and suspended again an hour after the scheduled kick-off time.";

                model.AllMarkets = lstGridMarkets;
                    //model.TodayHorseRacing = new List<TodayHorseRacing>();
                    //List<TodayHorseRacing> markets = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacingNew(LoggedinUserDetail.GetUserID()));

                    //model.TodayHorseRacing = markets.Where(item=>item.EventTypeName== "Horse Racing").ToList();
                    //model.TodayGreyRacing = markets.Where(item => item.EventTypeName == "Greyhound Racing").ToList();
                    //foreach (var item in todayhorserace)
                    //{
                    //    TodayHorserace obj = new TodayHorserace();
                    //    obj.MarketBookID = item.MarketCatalogueID;
                    //    obj.RaceName = item.TodayHorseRace;
                    //    model.TodayHorseRacing.Add(obj);

                    //}

                    model.TodayHorseRacing = new List<Models.TodayHorseRacing>();
                    var todaygreyrace = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "4339"));

                    foreach (var item in todaygreyrace.Take(15))
                    {
                        Models.TodayHorseRacing obj = new TodayHorseRacing();
                        obj.MarketCatalogueID = item.MarketCatalogueID;
                        obj.EventName = item.TodayHorseRace;
                        model.TodayHorseRacing.Add(obj);
                    }

                    model.ModalContent = new List<string>();
                string modalli1 = "Dummy text";
                string modalli2 = "Dummy text";
                model.ModalContent.Add(modalli1);
                model.ModalContent.Add(modalli2);
                
                return PartialView("AllInPayMatches", model);
                }
                else
                {
                    var results = objUsersServiceCleint.GetInPlayMatcheswithRunners1(userid);
                    List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(results);
                    List<string> lstIds = lstInPlayMatches.Where(item => item.EventTypeName == "Cricket").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList();
                     lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Soccer").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                     lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Tennis").Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                    List<AllMarketsInPlay> lstGridMarkets = new List<AllMarketsInPlay>();
                    ViewBag.backgrod = "#1D9BF0 !important";
                    ViewBag.color = "white";
                    foreach (var item in lstIds)
                    {
                        try
                        {
                            InPlayMatches objMarketLocal = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).FirstOrDefault();
                            AllMarketsInPlay objGridMarket = new AllMarketsInPlay();
                            objGridMarket.CategoryName = objMarketLocal.EventTypeName;
                            objGridMarket.MarketBookID = objMarketLocal.MarketCatalogueID;
                            objGridMarket.MarketBookName = objMarketLocal.MarketCatalogueName;
                            objGridMarket.EventName = objMarketLocal.EventName;
                            objGridMarket.CompetitionName = objMarketLocal.CompetitionName;
                            objGridMarket.MarketStartTime = objMarketLocal.EventOpenDate.Value.AddHours(5).ToString("dd-MM-yyyy hh:mm tt");
                            objGridMarket.MarketStatus = objMarketLocal.MarketStatus;

                            List<InPlayMatches> lstRunnersID = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).ToList();
                            try
                            {
                                objGridMarket.Runner1 = lstRunnersID.Where(x => !x.SelectionName.Contains("Draw")).FirstOrDefault().SelectionName;
                                objGridMarket.Runner2 = lstRunnersID.Where(x => !x.SelectionName.Contains("Draw")).LastOrDefault().SelectionName;
                                if (lstRunnersID.Count == 3)
                                {
                                    objGridMarket.Runner3 = lstRunnersID.Where(x => x.SelectionName.Contains("Draw")).FirstOrDefault().SelectionName;
                                }
                            }
                            catch (System.Exception ex)
                            { }
                            lstGridMarkets.Add(objGridMarket);
                        }
                        catch (System.Exception ex)
                        { }
                    }


                    var model = new DefaultPageModel();
                   
                    model.WelcomeMessage = "Please enjoy the non-stop intriguing betting experience only on www.gt-exch.com. Thanks";
                    model.WelcomeHeading = "Notice";
                    model.Rule = "Rule & Regs";
                    model.WelcomeMessage = "All bets apply to Full Time according to the match officials, plus any stoppage time. Extra - time / penalty shoot - outs are not included.If this market is re - opened for In - Play betting, unmatched bets will be cancelled at kick off and the market turned in play.The market will be suspended if it appears that a goal has been scored, a penalty will be given, or a red card will be shown.With the exception of bets for which the 'keep' option has been selected, unmatched bets will be cancelled in the event of a confirmed goal or sending off.Please note that should our data feeds fail we may be unable to manage this game in-play.Customers should be aware   that:Transmissions described as â€œliveâ€ by some broadcasters may actually be delayed.The extent of any such delay may vary, depending on the set-up through which they are receiving pictures or data.If this market is scheduled to go in-play, but due to unforeseen circumstances we are unable to offer the market in-play, then this market will be re-opened for the half-time interval and suspended again an hour after the scheduled kick-off time.";

                    model.AllMarkets = lstGridMarkets;
                  

                    model.ModalContent = new List<string>();
                    string modalli1 = "Dummy text";
                    string modalli2 = "Dummy text";
                    model.ModalContent.Add(modalli1);
                    model.ModalContent.Add(modalli2);

                    return PartialView("AllInPayMatches2", model);
                }
            }
            catch (System.Exception ex)
            {
                return PartialView("AllInPayMatches", new DefaultPageModel());
            }
        }
         

        public string GetReletedevent(string eventtype,string marketbookID)
        {
            try
            {
                 if (LoggedinUserDetail.GetUserTypeID() != 3) { 
                 ViewBag.backgrod = "#1D9BF0";
                     ViewBag.color = "white";
                }
                int userid = LoggedinUserDetail.GetUserID();
                if (userid == 1)
                {   
                    userid = 73;
                }
                var results = objUsersServiceCleint.GetInPlayMatcheswithRunners1(userid);
                List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(results);
                lstInPlayMatches = lstInPlayMatches.Where(item => item.EventTypeName == eventtype).ToList();
                lstInPlayMatches = lstInPlayMatches.GroupBy(car => car.MarketCatalogueID).Select(g => g.First()).ToList();
                lstInPlayMatches = lstInPlayMatches.Where(x => x.MarketCatalogueID != marketbookID).ToList();
               // var sd = lstInPlayMatches.GroupBy(x=>x.MarketCatalogueID).Select(b=>b.ToList()).ToList();
               //  lstInPlayMatches2 = dd.Skip(3).ToList();
                return RenderRazorViewToString("Relatedevent", lstInPlayMatches);
             
                }
               
            
            catch (System.Exception ex)
            {
                return RenderRazorViewToString("Relatedevent", new InPlayMatches());
               
            }
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);

                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);

                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public PartialViewResult GetDefaultPageDataInPlay(string ViewType)
        {
            try
            {
                //DGVInPlaymarketsall.ItemsSource = new;
                //DGVInPlaymarketsall.Items.Refresh();

                var results = objUsersServiceCleint.GetInPlayMatcheswithRunners(LoggedinUserDetail.GetUserID());
                List<InPlayMatches> lstInPlayMatches = JsonConvert.DeserializeObject<List<InPlayMatches>>(results);
                List<string> lstIds = new List<string>();
                if (ViewType == "Inplay") {
                    lstIds = lstInPlayMatches.Where(item => item.EventTypeName == "Cricket" && item.EventOpenDate.Value.Date == DateTime.Now.Date).Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList();
                    lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Soccer" && item.EventOpenDate.Value.Date == DateTime.Now.Date).Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                    lstIds.AddRange(lstInPlayMatches.Where(item => item.EventTypeName == "Tennis" && item.EventOpenDate.Value.Date == DateTime.Now.Date).Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList());
                }
                else
                {
                    lstIds = lstInPlayMatches.Where(item => item.EventTypeName == ViewType).Distinct().Select(item => item.MarketCatalogueID).Distinct().ToList();
                }
                

                List<AllMarketsInPlay> lstGridMarkets = new List<AllMarketsInPlay>();


                foreach (var item in lstIds)
                {
                    try
                    {
                        InPlayMatches objMarketLocal = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).FirstOrDefault();
                        AllMarketsInPlay objGridMarket = new AllMarketsInPlay();
                        objGridMarket.CategoryName = objMarketLocal.EventTypeName;
                        objGridMarket.MarketBookID = objMarketLocal.MarketCatalogueID;
                        objGridMarket.MarketBookName = objMarketLocal.MarketCatalogueName;
                        objGridMarket.EventName = objMarketLocal.EventName;
                        objGridMarket.CompetitionName = objMarketLocal.CompetitionName;
                        objGridMarket.MarketStartTime = objMarketLocal.EventOpenDate.Value.AddHours(5).ToString("dd-MM-yyyy hh:mm tt");
                        //  objGridMarket.MarketStatus = item.Status;
                        objGridMarket.MarketStatus = "";
                        //objGridMarket.ImagePath = objMarketLocal.EventTypeName + ".png";
                        // objGridMarket.CountryCode = objMarketLocal.CountryCode == null ? @"\Resources\0.png" : @"\Resources\" + objMarketLocal.CountryCode + ".gif";

                        List<InPlayMatches> lstRunnersID = lstInPlayMatches.Where(item2 => item2.MarketCatalogueID == item).ToList();
                        try
                        {
                            objGridMarket.Runner1 = lstRunnersID.Where(x => !x.SelectionName.Contains("Draw")).FirstOrDefault().SelectionName;
                            objGridMarket.Runner2 = lstRunnersID.Where(x => !x.SelectionName.Contains("Draw")).LastOrDefault().SelectionName;

                            if (lstRunnersID.Count == 3)
                            {
                                objGridMarket.Runner3 = lstRunnersID.Where(x => x.SelectionName.Contains("Draw")).FirstOrDefault().SelectionName;

                            }
                        }
                        catch (System.Exception ex)
                        {

                        }
                        lstGridMarkets.Add(objGridMarket);

                    }
                    catch (System.Exception ex)
                    {

                    }

                }
                var model = new DefaultPageModel();
              
                model.AllMarkets = lstGridMarkets;
                //model.TodayHorseRacing = new List<TodayHorserace>();
                model.ViewType = ViewType;
               
                return PartialView("MatchHighlights", model);
                //objUsersServiceCleint.GetInPlayMatcheswithRunnersCompleted += ObjUsersServiceCleint_GetInPlayMatcheswithRunnersCompleted;
            }
            catch (System.Exception ex)
            {
                return PartialView("MatchHighlights", new DefaultPageModel());
            }
        }

        //private void ObjUsersServiceCleint_GetInPlayMatcheswithRunnersCompleted(object sender, GetInPlayMatcheswithRunnersCompletedEventArgs e)
        //{
        //    objUsersServiceCleint.GetInPlayMatcheswithRunnersCompleted -= ObjUsersServiceCleint_GetInPlayMatcheswithRunnersCompleted;
           
           
        //}


    }
}