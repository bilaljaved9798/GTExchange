using bftradeline.Models;
using globaltraders;
using globaltraders.UserServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bftradeline.HelperClasses
{
    public class UserBetHelper
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        //public List<UserBets> UserBets()
        //{
        //    try
        //    {
        //        if (LoggedinUserDetail.GetUserTypeID() == 3)
        //        {

        //            List<UserBets> lstUserBets = (List<UserBets>)Session["userbets"];
        //            //long Liabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBets);
        //            //  ViewData["liabality"] = Session["liabality"];
        //            //  ViewBag.totliabality = Session["totliabality"];

        //            //  return RenderRazorViewToString("UserBets", lstUserBets);

        //        }
        //        else
        //        {
        //            if (LoggedinUserDetail.GetUserTypeID() == 2)
        //            {
        //                string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID());
        //                var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(userbets);
        //                //  List<UserBetsforAgent> lstUserBets = (List<UserBetsforAgent>)Session["userbets"];
        //                ViewData["liabality"] = Session["liabality"];
        //                ViewData["totliabality"] = Session["totliabality"];

        //                return RenderRazorViewToString("UserBetsAgent", lstUserBet);
        //            }
        //            else
        //            {
        //                if (LoggedinUserDetail.GetUserTypeID() == 1)
        //                {
        //                    string userbets = objUsersServiceCleint.GetUserbetsForAdmin();
        //                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(userbets);

        //                    //  List<UserBetsForAdmin> lstUserBets = (List<UserBetsForAdmin>)Session["userbets"];
        //                    ViewData["liabality"] = Session["liabality"];
        //                    ViewData["totliabality"] = Session["totliabality"];

        //                    return RenderRazorViewToString("UserBetsForAdmin", lstUserBet);
        //                }
        //                else
        //                {
        //                    List<UserBets> lstUserBets = new List<UserBets>();
        //                    return RenderRazorViewToString("UserBets", lstUserBets);
        //                }

        //            }
        //        }

        //    }
        //    catch (System.Exception ex)
        //    {
        //        List<UserBets> lstUserBets = new List<UserBets>();
        //        return RenderRazorViewToString("UserBets", lstUserBets);
        //    }
        //}

        public string CheckforPlaceBet(string Amount, string Odd, string BetType, string[] SelectionID, string marketbookID, List<UserBets> lstUserBets, List<ExternalAPI.TO.Runner> lstrunners, double CurrentAccountbalance)
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {
                    
                    List<UserBets> UnlstUserBets = lstUserBets.Where(item => item.isMatched == false).ToList();
                    List<UserBets> MlstUserBets = lstUserBets.Where(item => item.isMatched == true && item.location !="9").ToList();
                   
                    decimal TotBalance = 0;
                    decimal TotLiabality = 0;
                    

                    if (UnlstUserBets.Count == 1)
                    {
                        List<UserLiabality> lstUserLiabality = JsonConvert.DeserializeObject<List<Models.UserLiabality>>(objUsersServiceCleint.GetCurrentLiabality(LoggedinUserDetail.GetUserID()));
                      
                        if (lstrunners.Count > 1)
                        {
                            foreach (var selectionIDitem in SelectionID)
                            {

                                TotLiabality += objUserBets.GetLiabalityofCurrentUserActual(LoggedinUserDetail.GetUserID(), selectionIDitem, BetType, marketbookID, lstUserBets, lstrunners);
                            }
                        }
                        else
                        {
                            TotLiabality += objUserBets.GetLiabalityofCurrentUserActual(LoggedinUserDetail.GetUserID(), Odd, BetType, marketbookID, MlstUserBets, lstrunners);
                        }

                       
                        TotLiabality += objUserBets.GetLiabalityofCurrentUserActualforOtherMarkets(LoggedinUserDetail.GetUserID(), "", BetType, marketbookID, MlstUserBets);
                     
                        decimal CurrentBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate));

                      

                        TotBalance = CurrentBalance + TotLiabality;
                    }
                    else
                    {
                        List<LiabalitybyMarket> lstLibalitybymraketsfancy = new List<LiabalitybyMarket>();
                        decimal fancylab = 0;
                        decimal fancylabkj = 0;
                        List<UserLiabality> lstUserLiabality = JsonConvert.DeserializeObject<List<Models.UserLiabality>>(objUsersServiceCleint.GetCurrentLiabality(LoggedinUserDetail.GetUserID()));
                        List<UserBets> lstuserbetfancy = lstUserBets.Where(item => item.location == "9" || item.location == "8").ToList();
                        
                        lstUserBets = lstUserBets.Where(item => item.isMatched  && item.location != "9" ).ToList();
                        lstUserBets = lstUserBets.Where(item => item.isMatched && item.location != "8").ToList();
                        if (lstrunners.Count > 1)
                        {
                            foreach (var selectionIDitem in SelectionID)
                            {
                                TotLiabality += objUserBets.GetLiabalityofCurrentUserActual(LoggedinUserDetail.GetUserID(), selectionIDitem, BetType, marketbookID, lstUserBets, lstrunners);
                            }
                        }
                        else
                        {
                            TotLiabality += objUserBets.GetLiabalityofCurrentUserActual(LoggedinUserDetail.GetUserID(), Odd, BetType, marketbookID, lstUserBets, lstrunners);
                        }

                        TotLiabality += objUserBets.GetLiabalityofCurrentUserActualforOtherMarkets(LoggedinUserDetail.GetUserID(), "", BetType, marketbookID, lstUserBets);

                        fancylab = objUserBets.GetLiabalityofCurrentUserfancy(LoggedinUserDetail.GetUserID(), lstuserbetfancy.Where(item => item.isMatched == true).ToList());
                       
                        decimal CurrentBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate));

                        TotBalance = CurrentBalance + TotLiabality+ (fancylab);
                    }

                    if (TotBalance >= Convert.ToDecimal(Amount))
                    {
                        return "True";
                    }
                    else
                    {
                        return "Available balance is less then your amount";
                        //Fancy
                        if (lstrunners.Count == 1)
                        {
                           
                            long ProfitandLoss = 0;
                            var lstcurrentmarketbets = lstUserBets.Where(item=> item.MarketBookID == marketbookID).ToList();
                            if (lstcurrentmarketbets.Count > 0)
                            {
                                var lstUserbetslay = lstcurrentmarketbets.Where(item => item.BetType == "lay" && item.isMatched==true).ToList();
                                var lstuserbetsback = lstcurrentmarketbets.Where(item => item.BetType == "back" && item.isMatched == true).ToList();
                                var maxlayodd = 0;
                                var maxbackodd = 0;
                              
                                if (lstUserbetslay.Count > 0)
                                {
                                    maxlayodd=lstUserbetslay.Min(item => Convert.ToInt32(item.UserOdd));
                                   
                                }
                                if (lstuserbetsback.Count>0)
                                {
                                    maxbackodd=lstuserbetsback.Max(item => Convert.ToInt32(item.UserOdd));
                                  
                                }
                              
                                lstcurrentmarketbets.Where(item => item.BetType == "back" ).ToList().Sum(item => Convert.ToInt64(item.Amount));
                                ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = objUserBets.GetBookPosition(marketbookID, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), lstUserBets);
                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                }
                                 TotLiabality = TotLiabality - ProfitandLoss;
                                if (BetType == "back")
                                {

                                    if (Convert.ToInt32(Odd) <= maxlayodd && maxlayodd > 0 && Convert.ToDecimal(Amount) <= Convert.ToDecimal(-1 * ProfitandLoss))
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
                                    if (Convert.ToInt32(Odd) >= maxbackodd && maxbackodd > 0 && Convert.ToDecimal(Amount) <= Convert.ToDecimal(-1 * ProfitandLoss))
                                    {
                                        return "True";
                                    }
                                    else
                                    {
                                        return "Available balance is less then your amount";
                                    }
                                }
                            }
                            else
                            {
                                return "Available balance is less then your amount";
                            }                           
                        }
                        else
                        {
                            return "Available balance is less then your amount";
                        }
                        //                       
                    }
                }
                else
                {
                    return "You are not allowed to perform this operation.";
                }
            }
            catch (System.Exception ex)
            {
               // LoggedinUserDetail.LogError(ex);
                return "False";
            }
        }
        public ExternalAPI.TO.MarketBookForindianFancy GetBookPositionin(string marketBookID, string selectionID)
        {

            ExternalAPI.TO.MarketBookForindianFancy objmarketbookin = new ExternalAPI.TO.MarketBookForindianFancy();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            
                
                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {

                        List<bftradeline.Models.UserBets> lstCurrentBets = LoggedinUserDetail.CurrentUserBets.Where(item => item.SelectionID == selectionID && item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                        if (lstCurrentBets.Count > 0)
                        {
                            lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                            objmarketbookin.MarketId = marketBookID;
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBets[0].UserOdd) - 1).ToString();
                            objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
                            objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                            foreach (var userbet in lstCurrentBets)
                            {
                                if (objmarketbookin.RunnersForindianFancy != null)
                                {
                                    ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                    if (objexistingrunner == null)
                                    {
                                        ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                        objRunner.SelectionId = userbet.UserOdd;
                                        objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                        objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                    }
                                }
                                else
                                {
                                    ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                                    objmarketbookin.RunnersForindianFancy.Add(objRunner);
                                }
                            }
                            ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBets.Last().UserOdd) + 1).ToString();
                            objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                            objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                            ///calculation
                            foreach (var userbet in lstCurrentBets)
                            {
                                decimal num = Convert.ToDecimal(userbet.BetSize) / 100;
                                var totamount = (Convert.ToDecimal(userbet.Amount) * num);
                                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                if (userbet.BetType == "back")
                                {
                                    double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;

                                    objDebitCredit.Debit = totamount;//Convert.ToDecimal(userbet.Amount);
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = totamount;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }

                                }
                                else
                                {
                                    double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = totamount;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            //decimal num = Convert.ToDecimal(userbet.BetSize) / 100;
                                            objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }
                                }
                            }
                            objmarketbookin.DebitCredit = lstDebitCredit;
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                            }
                        }           
                }
            return objmarketbookin;
        }

        public string InsertUserBetFancy(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string marketbookid, string marketbookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {

                    userOdd = Convert.ToDecimal(userOdd).ToString("G29");
                    selecitonname = selecitonname.Trim();
                  var objMaxOddBack = objUsersServiceCleint.GetMaxOddBackandLay(LoggedinUserDetail.GetUserID());
                    var ID = objUsersServiceCleint.InsertUserBet(SelectionID, LoggedinUserDetail.GetUserID(), userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, selecitonname, marketbookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), LoggedinUserDetail.PasswordForValidate);
                    return "True";

                }
                else
                {
                    return "You are not allowed to perform this operation";
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return "False";
            }

        }
        public string InsertUserBet(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string marketbookid, string marketbookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay)
        {
            try
            {
                if (LoggedinUserDetail.GetUserTypeID() == 3)
                {

                    //if(PendingAmount < 0)
                    //{
                    //    double Amount =Convert.ToDouble( BetSize + PendingAmount);
                    //    amount = Amount.ToString();
                    //}
                    selecitonname = selecitonname.Trim();
                    userOdd = Convert.ToDecimal(userOdd).ToString("G29");
                    var ID = objUsersServiceCleint.InsertUserBet(SelectionID, LoggedinUserDetail.GetUserID(), userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, selecitonname, marketbookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, LoggedinUserDetail.PasswordForValidate);
                    if (ID != "")
                    {
                        return "True" + "|" + ID.ToString();
                    }
                    else
                    {
                        return "False";
                    }


                }
                else
                {
                    return "You are not allowed to perform this operation";
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                    var ID = objUsersServiceCleint.InsertUserBet(SelectionID, UserID, userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, selecitonname, marketbookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID, LoggedinUserDetail.user.MaxOddBack, LoggedinUserDetail.user.MaxOddLay, LoggedinUserDetail.user.CheckforMaxOddBack, LoggedinUserDetail.user.CheckforMaxOddLay, LoggedinUserDetail.PasswordForValidate);
                    return "True" + "|" + ID.ToString();
                }
                else
                {
                    return "You are not allowed to perform this operation";
                }
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return "False";
            }


        }
        public string UpdateUnMatchedStatustoComplete(string[] userbetsIDs)
        {

            long[] lstUserBetIDs = Array.ConvertAll(userbetsIDs, long.Parse);
            objUsersServiceCleint.UpdateUserBetUnMatchedStatusTocomplete(lstUserBetIDs, LoggedinUserDetail.PasswordForValidate);
            return "True";

        }

    }
}
