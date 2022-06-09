using bfnexchange.Models;
using bfnexchange.UsersServiceReference;
using ExternalAPI.TO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;

namespace bfnexchange.Controllers
{

    public class UserBetController : Controller
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();

        // GET: UserBet
        public ActionResult Index()
        {
            return View();
        }
        public string UserBets()
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"])); //(List<UserBets>)Session["userbets"];
                    Session["userbets"] = lstUserBets;
                    List<UserBets> lstAllUserBets = new List<Models.UserBets>();
                    lstAllUserBets.Clear();
                    if (Session["linevmarkets"] != null)
                    {
                        List<UserBets> lstUserBetAll = (List<UserBets>)Session["userbet"];
                        List<ExternalAPI.TO.LinevMarkets> linevmarketsfig = new List<LinevMarkets>();
                        List<ExternalAPI.TO.LinevMarkets> linevmarkets = (List<ExternalAPI.TO.LinevMarkets>)Session["linevmarkets"];
                        if (linevmarkets != null )
                        {
                            linevmarketsfig = linevmarkets.GroupBy(item => item.MarketCatalogueName).Select(g => g.First()).Where(item2 => item2.EventName == "Figure").ToList();
                            linevmarkets = linevmarkets.Where(item => item.EventName != "Figure").ToList();

                            List<UserBets> lstFancyBets = new List<UserBets>();
                            foreach (var lineitem in linevmarkets)
                            {
                                lstFancyBets = lstUserBetAll.Where(item =>  item.MarketBookID == lineitem.MarketCatalogueID).ToList();

                                lstAllUserBets.AddRange(lstFancyBets);
                            }
                            List<UserBets> lstFancyfigBets = new List<UserBets>();
                            foreach (var lineitem in linevmarketsfig)
                            {
                                lstFancyfigBets = lstUserBetAll.Where(item => item.MarketBookID == lineitem.MarketCatalogueID).ToList();

                                lstAllUserBets.AddRange(lstFancyfigBets);
                            }
                        }
                    }
                    if (Session["TWT"] != null)
                    {
                        List<UserBets> lstUserBetAll = (List<UserBets>)Session["userbet"];

                        string TWTMarket = (string)Session["TWT"];
                        if (TWTMarket != null)
                        {
                            List<UserBets> lstFancyBets = new List<UserBets>();
                            
                                lstFancyBets = lstUserBetAll.Where(item => item.MarketBookID == TWTMarket).ToList();

                                lstAllUserBets.AddRange(lstFancyBets);
                            
                        }
                    }
                    if (lstUserBets != null)
                    {
                        lstAllUserBets.AddRange(lstUserBets);
                         lstAllUserBets = lstAllUserBets.GroupBy(car => car.ID).Select(g => g.First()).ToList();
                        lstAllUserBets = lstAllUserBets.OrderByDescending(item => item.ID).ToList();

                    }
                 
                    //long Liabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBets);
                    ViewData["liabality"] = Session["liabality"];
                    ViewBag.totliabality = Session["totliabality"];

                    return RenderRazorViewToString("UserBets", lstAllUserBets);

                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                       // string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                     //   var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(userbets);
                       List<UserBetsforAgent> lstUserBets = (List<UserBetsforAgent>)Session["userbets"];
                        ViewData["liabality"] = Session["liabality"];
                        ViewData["totliabality"] = Session["totliabality"];

                        return RenderRazorViewToString("UserBetsAgent", lstUserBets);
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                        {
                            List<UserBetsforSuper> lstUserBets = (List<UserBetsforSuper>)Session["userbets"];
                            ViewData["liabality"] = Session["liabality"];
                            ViewData["totliabality"] = Session["totliabality"];

                            return RenderRazorViewToString("UserBetsForSuper", lstUserBets);
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                            {
                                // string userbets = objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]);
                                // List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(userbets);

                                List<UserBetsForAdmin> lstUserBets = (List<UserBetsForAdmin>)Session["userbets"];
                                ViewData["liabality"] = Session["liabality"];
                                ViewData["totliabality"] = Session["totliabality"];

                                return RenderRazorViewToString("UserBetsForAdmin", lstUserBets);
                            }
                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 9)
                                {
                                    List<UserBetsforSamiadmin> lstUserBets = (List<UserBetsforSamiadmin>)Session["userbets"];
                                    ViewData["liabality"] = Session["liabality"];
                                    ViewData["totliabality"] = Session["totliabality"];

                                    return RenderRazorViewToString("UserBetsForSamiadmin", lstUserBets);
                                }
                                else
                                {
                                    List<UserBets> lstUserBets = new List<UserBets>();
                                    return RenderRazorViewToString("UserBets", lstUserBets);
                                }
                            }
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                List<UserBets> lstUserBets = new List<UserBets>();
                return RenderRazorViewToString("UserBets", lstUserBets);
            }
        }
        public string UserBetsAll()
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {

                    List<UserBets> lstUserBets = (List<UserBets>)Session["userbets"];
                    List<UserBets> lstAllUserBets = new List<Models.UserBets>();
                    if (Session["linevmarkets"] != null)
                    {
                        List<UserBets> lstUserBetAll = (List<UserBets>)Session["userbet"];
                        List<ExternalAPI.TO.LinevMarkets> linevmarketsfig = new List<LinevMarkets>();
                        List<ExternalAPI.TO.LinevMarkets> linevmarkets = (List<ExternalAPI.TO.LinevMarkets>)Session["linevmarkets"];
                        if (linevmarkets != null)
                        {
                           
                            linevmarketsfig = linevmarkets.GroupBy(item => item.MarketCatalogueName).Select(g => g.First()).Where(item2 => item2.EventName == "Figure").ToList();
                            linevmarkets = linevmarkets.Where(item => item.EventName != "Figure").ToList();
                            List<UserBets> lstFancyBets = new List<UserBets>();
                            foreach (var lineitem in linevmarkets)
                            {
                                lstFancyBets = lstUserBetAll.Where(item => item.MarketBookID == lineitem.MarketCatalogueID).ToList();
                                lstAllUserBets.AddRange(lstFancyBets);
                            }
                            List<UserBets> lstFancyfigBets = new List<UserBets>();
                            foreach (var lineitem in linevmarketsfig)
                            {
                                lstFancyfigBets = lstUserBetAll.Where(item => item.MarketBookID == lineitem.MarketCatalogueID).ToList();
                                lstAllUserBets.AddRange(lstFancyfigBets);
                            }
                        }
                    }
                    if (lstUserBets != null)
                    {
                        lstAllUserBets.AddRange(lstUserBets);
                        lstAllUserBets = lstAllUserBets.OrderByDescending(item => item.ID).ToList();

                    }

                    //long Liabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBets);
                    ViewData["liabality"] = Session["liabality"];
                    ViewBag.totliabality = Session["totliabality"];

                    return RenderRazorViewToString("UserBetsAll", lstAllUserBets);

                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                       
                        List<UserBetsforAgent> lstUserBets = (List<UserBetsforAgent>)Session["userbets"];
                        ViewData["liabality"] = Session["liabality"];
                        ViewData["totliabality"] = Session["totliabality"];

                        return RenderRazorViewToString("UserBetsAgentAll", lstUserBets);
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 8)
                        {
                            List<UserBetsforSuper> lstUserBets = (List<UserBetsforSuper>)Session["userbets"];
                            ViewData["liabality"] = Session["liabality"];
                            ViewData["totliabality"] = Session["totliabality"];

                            return RenderRazorViewToString("UserBetsForSuperAll", lstUserBets);
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 1)
                            {

                                List<UserBetsForAdmin> lstUserBets = (List<UserBetsForAdmin>)Session["userbets"];
                                ViewData["liabality"] = Session["liabality"];
                                ViewData["totliabality"] = Session["totliabality"];

                                return RenderRazorViewToString("UserBetsForAdminAll", lstUserBets);
                            }
                            else
                            {
                                if (LoggedinUserDetail.GetUserTypeID() == 9)
                                {

                                    List<UserBetsforSamiadmin> lstUserBets = (List<UserBetsforSamiadmin>)Session["userbets"];
                                    ViewData["liabality"] = Session["liabality"];
                                    ViewData["totliabality"] = Session["totliabality"];
                                    return RenderRazorViewToString("UserBetsForSamiadminAll", lstUserBets);
                                }

                                else
                                {
                                    List<UserBets> lstUserBets = new List<UserBets>();
                                    return RenderRazorViewToString("UserBetsAll", lstUserBets);
                                }
                            }
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                List<UserBets> lstUserBets = new List<UserBets>();
                return RenderRazorViewToString("UserBetsAll", lstUserBets);
            }
        }
       
        public bool GetBettingAllowedbyMarketIDandUserID(string MarketBookID)
        {
            UserServicesClient objUserServiceClient = new UserServicesClient();
            return objUserServiceClient.GetBettingAllowedbyMarketIDandUserID(LoggedinUserDetail.GetUserID(), MarketBookID);
        }
        public string CheckforPlaceBet(string Amount, string Odd, string BetType, string[] SelectionID, string marketbookID,string MarketbookName,string Runnerscount)
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            try
            {
                List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                //List<UserBets> lstuserbetfancy = lstUserBets.Where(item => item.location == "9" || item.location == "8").ToList();
                //lstUserBets = lstUserBets.Where(item => item.isMatched && item.location != "9" && item.location != "8").ToList();
                decimal TotLiabality = 0;
                decimal fancylab = 0;
                decimal TotBalance = 0;
                List<UserBets> UnlstUserBets = lstUserBets.Where(item => item.isMatched == false).ToList();
                List<UserBets> MlstUserBets = lstUserBets.Where(item => item.isMatched == true && item.location != "9").ToList();

                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    if (UnlstUserBets.Count == 1)
                    {
                        List<UserLiabality> lstUserLiabality = JsonConvert.DeserializeObject<List<Models.UserLiabality>>(objUsersServiceCleint.GetCurrentLiabality(LoggedinUserDetail.GetUserID()));

                        TotLiabality += objUserBets.GetLiabalityofCurrentUserActual(LoggedinUserDetail.GetUserID(), Odd, BetType, marketbookID, MarketbookName, MlstUserBets);

                        TotLiabality += objUserBets.GetLiabalityofCurrentUserActualforOtherMarkets(LoggedinUserDetail.GetUserID(), "", BetType, marketbookID, MarketbookName, lstUserBets);

                        decimal CurrentBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));

                        TotBalance = CurrentBalance + TotLiabality;
                    }
                    else
                    {                      
                            lstUserBets = lstUserBets.Where(item => item.isMatched && item.location != "9").ToList();
                            lstUserBets = lstUserBets.Where(item => item.isMatched && item.location != "8").ToList();

                            //TotLiabality += objUserBets.GetLiabalityofCurrentUserActual(LoggedinUserDetail.GetUserID(), Odd, BetType, marketbookID, MarketbookName, lstUserBets);

                            TotLiabality += objUserBets.GetLiabalityofCurrentUserActualforOtherMarkets(LoggedinUserDetail.GetUserID(), "", BetType, marketbookID, MarketbookName, lstUserBets);

                            //fancylab = objUserBets.GetLiabalityofCurrentUserfancy(LoggedinUserDetail.GetUserID(), lstuserbetfancy.Where(item => item.isMatched == true).ToList());

                            decimal CurrentBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));

                            TotBalance = CurrentBalance + TotLiabality + (fancylab);
                    }
                    
                    
                    if (TotBalance >= Convert.ToDecimal(Amount))
                    {
                        return "True";
                    }
                    else
                    {
                        return "Available balance is less then your amount";
                    }

                }
                else
                {
                    return "You are not allowed to perform this operation.";
                }
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                return "False";
            }
        }
        public string InsertUserBet(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string marketbookid, string marketbookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    UserStatus objUserStatus = JsonConvert.DeserializeObject<UserStatus>(objUsersServiceCleint.GetUserStatus(LoggedinUserDetail.GetUserID()));


                    if (objUserStatus.isBlocked == true || objUserStatus.isDeleted == true || objUserStatus.Loggedin == false)
                    {
                        return "False";
                    }
                    userOdd = Convert.ToDecimal(userOdd).ToString("G29");
                    selecitonname = selecitonname.Trim();
                    bfnexchange.Services.DBModel.SP_Users_GetMaxOddBackandLay_Result objMaxOddBack = objUsersServiceCleint.GetMaxOddBackandLay(LoggedinUserDetail.GetUserID());
                    var ID = objUsersServiceCleint.InsertUserBet(SelectionID, LoggedinUserDetail.GetUserID(), userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, selecitonname, marketbookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID,Convert.ToDecimal(objMaxOddBack.MaxOddBack),Convert.ToDecimal(objMaxOddBack.MaxOddLay),Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack),Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), ConfigurationManager.AppSettings["PasswordForValidate"]);
                    return "True" + "|" + ID.ToString();
                   
                }
                else
                {
                    return "You are not allowed to perform this operation";
                }
            }
            catch (System.Exception ex)
            {
                return "False";
            }


        }

        public string InsertUserBetAdmin(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string marketbookid, string marketbookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID, int UserID)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() != 3)
                {
                    selecitonname = selecitonname.Trim();
                    var ID = objUsersServiceCleint.InsertUserBet(SelectionID, UserID, userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, selecitonname, marketbookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID,0,0,false,false, ConfigurationManager.AppSettings["PasswordForValidate"]);
                    return "True" + "|" + ID.ToString();
                }
                else
                {
                    return "You are not allowed to perform this operation";
                }
            }
            catch (System.Exception ex)
            {
                return "False";
            }


        }
        public string InsertUserBetFancy(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string marketbookid, string marketbookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    UserStatus objUserStatus = JsonConvert.DeserializeObject<UserStatus>(objUsersServiceCleint.GetUserStatus(LoggedinUserDetail.GetUserID()));


                    if (objUserStatus.isBlocked == true || objUserStatus.isDeleted == true || objUserStatus.Loggedin == false)
                    {
                        return "False";
                    }
                    userOdd = Convert.ToDecimal(userOdd).ToString("G29");
                    selecitonname = selecitonname.Trim();
                    bfnexchange.Services.DBModel.SP_Users_GetMaxOddBackandLay_Result objMaxOddBack = objUsersServiceCleint.GetMaxOddBackandLay(LoggedinUserDetail.GetUserID());
                    var ID = objUsersServiceCleint.InsertUserBet(SelectionID, LoggedinUserDetail.GetUserID(), userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, selecitonname, marketbookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), ConfigurationManager.AppSettings["PasswordForValidate"]);
                    return "True";

                }
                else
                {
                    return "You are not allowed to perform this operation";
                }
            }
            catch (System.Exception ex)
            {
                return "False";
            }
            return "True";
        }
        public string UpdateUnMatchedStatustoComplete(string[] userbetsIDs)
        {

            long[] lstUserBetIDs = Array.ConvertAll(userbetsIDs, long.Parse);
            objUsersServiceCleint.UpdateUserBetUnMatchedStatusTocomplete(lstUserBetIDs, ConfigurationManager.AppSettings["PasswordForValidate"]);
            return "True";

        }
        public void InsertUserBetNew(decimal userodd, string SelectionID, string Selectionname, string BetType, string nupdownAmount, string betslipamountlabel, int Clickedlocation, string Betslipsize, string Password, string marketbookId, string Marketbookname)
        {
            try
            {
                if (Clickedlocation == 8 || Clickedlocation == 9)
                {
                    if (Clickedlocation == 9)
                    {
                        bfnexchange.Services.DBModel.SP_Users_GetMaxOddBackandLay_Result objMaxOddBack = objUsersServiceCleint.GetMaxOddBackandLay(LoggedinUserDetail.GetUserID());
                        objUsersServiceCleint.InsertUserBetNew(userodd, SelectionID, Marketbookname , BetType, nupdownAmount, betslipamountlabel, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), Clickedlocation, LoggedinUserDetail.GetUserID(), Betslipsize, ConfigurationManager.AppSettings["PasswordForValidate"], marketbookId, Marketbookname, true);

                    }
                    else
                    {
                        bfnexchange.Services.DBModel.SP_Users_GetMaxOddBackandLay_Result objMaxOddBack = objUsersServiceCleint.GetMaxOddBackandLay(LoggedinUserDetail.GetUserID());
                        objUsersServiceCleint.InsertUserBetNew(userodd, SelectionID, '(' + Selectionname.ToLower() + ')', BetType, nupdownAmount, betslipamountlabel, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), Clickedlocation, LoggedinUserDetail.GetUserID(), Betslipsize, ConfigurationManager.AppSettings["PasswordForValidate"], marketbookId, Marketbookname, true);
                    }
                    }
                else
                {
                    bfnexchange.Services.DBModel.SP_Users_GetMaxOddBackandLay_Result objMaxOddBack = objUsersServiceCleint.GetMaxOddBackandLay(LoggedinUserDetail.GetUserID());
                    objUsersServiceCleint.InsertUserBetNew(userodd, SelectionID, '(' + Selectionname.ToLower() + ')', BetType, nupdownAmount, betslipamountlabel, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), Clickedlocation, LoggedinUserDetail.GetUserID(), Betslipsize, ConfigurationManager.AppSettings["PasswordForValidate"], marketbookId, Marketbookname, true);

                }
            }
            catch (System.Exception ex)
            {

            }
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        public string GetTotalLiabality()
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            decimal TotLiabality = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 3)
            {

                var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));

                List<UserBets> lstuserbetfancy = lstUserBet.Where(item => item.location == "9").ToList();
                List<UserBets> lstuserbet = lstUserBet.Where(item => item.location != "9").ToList();
                TotLiabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstuserbet.Where(item=>item.isMatched==true).ToList());
                TotLiabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstuserbetfancy.Where(item => item.isMatched == true).ToList());
                Session["userbets"] = lstUserBet;
                // Updateunmatchbets(marketbooks.ToArray(), lstUserBet);
                // marketbooks[0].UserBetsEndUser = ConverttoJSONString(lstUserBet);

            }
            if (LoggedinUserDetail.GetUserTypeID() == 2)
            {
                string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(userbets);

                List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true ).ToList();
                Session["userbets"] = lstUserBet;

                TotLiabality = objUserBets.GetLiabalityofCurrentAgent(lstUserBets);
            }
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                string userbets = objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]);
                List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(userbets);              
                List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true).ToList();
                Session["userbets"] = lstUserBet;
                TotLiabality = objUserBets.GetLiabalityofAdmin(lstUserBets);
            }

            if (LoggedinUserDetail.GetUserTypeID() == 8)
            {

                string userbets = objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforSuper>>(userbets);
                List<UserBetsforSuper> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true).ToList();
                Session["userbets"] = lstUserBet;

                TotLiabality = objUserBets.GetLiabalityofSuper(lstUserBets);
            }
            if (LoggedinUserDetail.GetUserTypeID() == 9)
            {

                string userbets = objUsersServiceCleint.GetUserBetsbySamiAdmin(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforSamiadmin>>(userbets);
                List<UserBetsforSamiadmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true).ToList();
                Session["userbets"] = lstUserBet;

                TotLiabality = objUserBets.GetLiabalityofSamiadmin(lstUserBets);
            }

            Session["liabality"] = 0;
           
            Session["totliabality"] = TotLiabality;
            Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
            decimal startingbalance = objUsersServiceCleint.GetStartingBalance(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
            return CurrentAccountBalance.ToString()+"|"+startingbalance.ToString()+"|"+ TotLiabality.ToString();
        }
    }
}