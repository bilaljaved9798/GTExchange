using bfnexchange.Models;
using bfnexchange.UsersServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bfnexchange.Controllers
{
    public class LiabalityController : Controller
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        AccessRightsbyUserType objAccessrightsbyUserType;
        // GET: Liabality
        public ActionResult Index()
        {
            ViewBag.backgrod = "-webkit-linear-gradient(bottom, #1D9BF0, #0a0a0a) !important;";
            ViewBag.color = "white";
            return View();
        }

        //public PartialViewResult LoadLiabalitybyMarket()
       // public string LoadLiabalitybyMarket()
         public PartialViewResult LoadLiabalitybyMarket()
        {
            objAccessrightsbyUserType = new AccessRightsbyUserType();
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            try
            {
                List<LiabalitybyMarket> lstLibalitybymrakets = new List<LiabalitybyMarket>();
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    //List<Models.TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "4339"));

                    // List<UserBets> lstUserBets = (List<UserBets>)Session["userbet"];
                    var lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                    UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                    lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentUserbyMarkets(LoggedinUserDetail.GetUserID(), lstUserBets);
                    decimal liab=Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                    objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");
                    //return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
                    return PartialView("LiabalitybyMarket", lstLibalitybymrakets);

                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {

                        string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                        var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(userbets);
                        UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                        lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentAgentbyMarkets(lstUserBet);
                        decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                        objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");
                        //return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
                        return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {


                            string userbets = objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]);
                            List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(userbets);
                            UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                            lstLibalitybymrakets = objUserBets.GetLiabalityofAdminbyMarkets(lstUserBet);
                            decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                            objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");
                            // return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);

                            return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                string userbets = objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(),ConfigurationManager.AppSettings["PasswordForValidate"]);
                                List<UserBetsforSuper> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforSuper>>(userbets);
                                UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                                lstLibalitybymrakets = objUserBets.GetLiabalityofSuperbyMarkets(lstUserBet);
                                decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                                objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");
                                //return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
                                return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                            }
                            else
                            {
                               // return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
                                return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
                            }
                        }

                    }
                }

            }
            catch (System.Exception ex)
            {
                List<LiabalitybyMarket> lstLibalitybymrakets = new List<LiabalitybyMarket>();
                //return RenderRazorViewToString("LiabalitybyMarket", lstLibalitybymrakets);
                return PartialView("LiabalitybyMarket", lstLibalitybymrakets);
            }


        }


        public ActionResult LoadLiabalitybyMarketall()
        {
            objAccessrightsbyUserType = new AccessRightsbyUserType();
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            try
            {
                List<LiabalitybyMarket> lstLibalitybymrakets = new List<LiabalitybyMarket>();
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    //List<Models.TodayHorseRacing> lstTodayHorseRacing = JsonConvert.DeserializeObject<List<Models.TodayHorseRacing>>(objUsersServiceCleint.GetTodayHorseRacing(LoggedinUserDetail.GetUserID(), "4339"));

                    // List<UserBets> lstUserBets = (List<UserBets>)Session["userbet"];
                    var lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]));
                    UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                    lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentUserbyMarkets(LoggedinUserDetail.GetUserID(), lstUserBets);
                    decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                    objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");

                    return View(objAccessrightsbyUserType);

                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {

                        string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                        var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(userbets);
                        UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                        lstLibalitybymrakets = objUserBets.GetLiabalityofCurrentAgentbyMarkets(lstUserBet);
                        decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                        objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");
                        return View(objAccessrightsbyUserType);
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 1)
                        {
                            string userbets = objUsersServiceCleint.GetUserbetsForAdmin(ConfigurationManager.AppSettings["PasswordForValidate"]);
                            List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(userbets);
                            UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                            lstLibalitybymrakets = objUserBets.GetLiabalityofAdminbyMarkets(lstUserBet);
                            decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                            objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");

                            return View(objAccessrightsbyUserType);
                        }
                        else
                        {
                            if (LoggedinUserDetail.GetUserTypeID() == 8)
                            {
                                string userbets = objUsersServiceCleint.GetUserBetsbySuperID(LoggedinUserDetail.GetUserID(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                List<UserBetsforSuper> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforSuper>>(userbets);
                                UserBetsUpdateUnmatcedBets objUserbet = new UserBetsUpdateUnmatcedBets();
                                lstLibalitybymrakets = objUserBets.GetLiabalityofSuperbyMarkets(lstUserBet);
                                decimal liab = Convert.ToDecimal(lstLibalitybymrakets.Sum(item => Convert.ToDecimal((item.Liabality))));
                                objAccessrightsbyUserType.CurrentLiabality = "Liab: " + liab.ToString("F2");

                                return View(objAccessrightsbyUserType);
                            }
                            else
                            {                              
                                return View(objAccessrightsbyUserType);
                            }
                        }

                    }
                }

            }
            catch (System.Exception ex)
            {
             
                return View(objAccessrightsbyUserType);
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

    }
}