
using GTExchNew.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTExchNew.HelperClasses
{
    //public class UserBetHelper
    //{
    //    UserServicesClient objUsersServiceCleint = new UserServicesClient();
    //    //public List<UserBets> UserBets()
    //    //{
    //    //    try
    //    //    {
    //    //        if (LoggedinUserDetail.GetUserTypeID() == 3)
    //    //        {

    //    //            List<UserBets> lstUserBets = (List<UserBets>)Session["userbets"];
    //    //            //long Liabality = objUserBets.GetLiabalityofCurrentUser(LoggedinUserDetail.GetUserID(), lstUserBets);
    //    //            //  ViewData["liabality"] = Session["liabality"];
    //    //            //  ViewBag.totliabality = Session["totliabality"];

    //    //            //  return RenderRazorViewToString("UserBets", lstUserBets);

    //    //        }
    //    //        else
    //    //        {
    //    //            if (LoggedinUserDetail.GetUserTypeID() == 2)
    //    //            {
    //    //                string userbets = objUsersServiceCleint.GetUserBetsbyAgentID(LoggedinUserDetail.GetUserID());
    //    //                var lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(userbets);
    //    //                //  List<UserBetsforAgent> lstUserBets = (List<UserBetsforAgent>)Session["userbets"];
    //    //                ViewData["liabality"] = Session["liabality"];
    //    //                ViewData["totliabality"] = Session["totliabality"];

    //    //                return RenderRazorViewToString("UserBetsAgent", lstUserBet);
    //    //            }
    //    //            else
    //    //            {
    //    //                if (LoggedinUserDetail.GetUserTypeID() == 1)
    //    //                {
    //    //                    string userbets = objUsersServiceCleint.GetUserbetsForAdmin();
    //    //                    List<UserBetsForAdmin> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBetsForAdmin>>(userbets);

    //    //                    //  List<UserBetsForAdmin> lstUserBets = (List<UserBetsForAdmin>)Session["userbets"];
    //    //                    ViewData["liabality"] = Session["liabality"];
    //    //                    ViewData["totliabality"] = Session["totliabality"];

    //    //                    return RenderRazorViewToString("UserBetsForAdmin", lstUserBet);
    //    //                }
    //    //                else
    //    //                {
    //    //                    List<UserBets> lstUserBets = new List<UserBets>();
    //    //                    return RenderRazorViewToString("UserBets", lstUserBets);
    //    //                }

    //    //            }
    //    //        }

    //    //    }
    //    //    catch (System.Exception ex)
    //    //    {
    //    //        List<UserBets> lstUserBets = new List<UserBets>();
    //    //        return RenderRazorViewToString("UserBets", lstUserBets);
    //    //    }
    //    //}

    //    public string CheckforPlaceBet(string Amount, string Odd, string BetType, string[] SelectionID, string MarketBookID, List<UserBets> lstUserBets, List<ExternalAPI.TO.Runner> lstrunners, double CurrentAccountbalance)
    //    {
    //        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
    //        try
    //        {
    //            if (LoggedinUserDetail.GetUserTypeID() == 3)
    //            {
    //               // decimal CurrentBalance = Convert.ToDecimal(CurrentAccountbalance);
    //                //List<UserLiabality> lstUserLiabality = JsonConvert.DeserializeObject<List<Models.UserLiabality>>(objUsersServiceCleint.GetCurrentLiabality(LoggedinUserDetail.GetUserID()));
    //                decimal TotLiabality = 0;
    //                if (lstrunners.Count > 1)
    //                {
    //                    foreach (var selectionIDitem in SelectionID)
    //                    {
    //                        TotLiabality += objUserBets.GetLiabalityofCurrentUserActual(LoggedinUserDetail.GetUserID(), selectionIDitem, BetType, MarketBookID, lstUserBets, lstrunners);

    //                    }
    //                }
    //                else
    //                {
    //                    TotLiabality += objUserBets.GetLiabalityofCurrentUserActual(LoggedinUserDetail.GetUserID(), Odd, BetType, MarketBookID, lstUserBets, lstrunners);
    //                }
                   
    //                //TotLiabality += objUserBets.GetLiabalityofCurrentUserActualforOtherMarkets(LoggedinUserDetail.GetUserID(), "", BetType, MarketBookID, lstUserBets);
                   



    //                decimal CurrentBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.GetUserID(),LoggedinUserDetail.PasswordForValidate));
                  
    //                decimal TotBalance = CurrentBalance + TotLiabality;
    //                if (TotBalance >= Convert.ToDecimal(Amount))
    //                {
    //                    return "True";
    //                }
    //                else
    //                {
    //                    return "Available balance is less then your amount";
    //                    //Fancy
    //                    if (lstrunners.Count == 1)
    //                    {
    //                        //ExternalAPI.TO.MarketBook objMarketBook = new ExternalAPI.TO.MarketBook();
    //                        //objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
    //                        //objMarketBook.Runners = lstrunners;
    //                        //objMarketBook.MarketId = MarketBookID;
    //                        long ProfitandLoss = 0;
    //                        var lstcurrentmarketbets = lstUserBets.Where(item=> item.MarketBookID == MarketBookID).ToList();
    //                        if (lstcurrentmarketbets.Count > 0)
    //                        {
    //                            var lstUserbetslay = lstcurrentmarketbets.Where(item => item.BetType == "lay" && item.isMatched==true).ToList();
    //                            var lstuserbetsback = lstcurrentmarketbets.Where(item => item.BetType == "back" && item.isMatched == true).ToList();
    //                            var maxlayodd = 0;
    //                            var maxbackodd = 0;
                              
    //                            if (lstUserbetslay.Count > 0)
    //                            {
    //                                maxlayodd=lstUserbetslay.Min(item => Convert.ToInt32(item.UserOdd));
                                   
    //                            }
    //                            if (lstuserbetsback.Count>0)
    //                            {
    //                                maxbackodd=lstuserbetsback.Max(item => Convert.ToInt32(item.UserOdd));
                                  
    //                            }
                              
    //                          // lstcurrentmarketbets.Where(item => item.BetType == "back" ).ToList().Sum(item => Convert.ToInt64(item.Amount));
    //                            ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = objUserBets.GetBookPosition(MarketBookID, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(), lstUserBets);
    //                            if (CurrentMarketProfitandloss.Runners != null)
    //                            {
    //                                ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
    //                            }
    //                            // TotLiabality = TotLiabality - ProfitandLoss;
    //                            if (BetType == "back")
    //                            {



    //                                if (Convert.ToInt32(Odd) <= maxlayodd && maxlayodd > 0 && Convert.ToDecimal(Amount) <= Convert.ToDecimal(-1 * ProfitandLoss))
    //                                {
    //                                    return "True";
    //                                }
    //                                else
    //                                {
    //                                    return "Available balance is less then your amount";
    //                                }



    //                            }
    //                            else
    //                            {
    //                                if (Convert.ToInt32(Odd) >= maxbackodd && maxbackodd > 0 && Convert.ToDecimal(Amount) <= Convert.ToDecimal(-1 * ProfitandLoss))
    //                                {
    //                                    return "True";
    //                                }
    //                                else
    //                                {
    //                                    return "Available balance is less then your amount";
    //                                }
    //                            }
    //                        }
    //                        else
    //                        {
    //                            return "Available balance is less then your amount";
    //                        }
                            
    //                    }
    //                    else
    //                    {
    //                        return "Available balance is less then your amount";
    //                    }
    //                    //
                       
    //                }

    //            }
    //            else
    //            {
    //                return "You are not allowed to perform this operation.";
    //            }
    //        }
    //        catch (System.Exception ex)
    //        {
    //           // LoggedinUserDetail.LogError(ex);
    //            return "False";
    //        }
    //    }
    //    public string InsertUserBetFancy(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string MarketBookid, string MarketBookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay)
    //    {
    //        try
    //        {
    //            if (LoggedinUserDetail.GetUserTypeID() == 3)
    //            {

    //                userOdd = Convert.ToDecimal(userOdd).ToString("G29");
    //                selecitonname = selecitonname.Trim();
    //              var objMaxOddBack = objUsersServiceCleint.GetMaxOddBackandLay(LoggedinUserDetail.GetUserID());
    //                var ID = objUsersServiceCleint.InsertUserBet(SelectionID, LoggedinUserDetail.GetUserID(), userOdd, amount, bettype, liveodd, ismatched, status, MarketBookid, DateTime.Now, DateTime.Now, selecitonname, MarketBookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), LoggedinUserDetail.PasswordForValidate);
    //                return "True";

    //            }
    //            else
    //            {
    //                return "You are not allowed to perform this operation";
    //            }
    //        }
    //        catch (System.Exception ex)
    //        {
    //            return "False";
    //        }

    //    }
    //    public string InsertUserBet(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string MarketBookid, string MarketBookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay)
    //    {
    //        try
    //        {
    //            if (LoggedinUserDetail.GetUserTypeID() == 3)
    //            {

    //                //if(PendingAmount < 0)
    //                //{
    //                //    double Amount =Convert.ToDouble( BetSize + PendingAmount);
    //                //    amount = Amount.ToString();
    //                //}
    //                selecitonname = selecitonname.Trim();
    //                userOdd = Convert.ToDecimal(userOdd).ToString("G29");
    //                var ID = objUsersServiceCleint.InsertUserBet(SelectionID, LoggedinUserDetail.GetUserID(), userOdd, amount, bettype, liveodd, ismatched, status, MarketBookid, DateTime.Now, DateTime.Now, selecitonname, MarketBookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, LoggedinUserDetail.PasswordForValidate);
    //                if (ID != "")
    //                {
    //                    return "True" + "|" + ID.ToString();
    //                }
    //                else
    //                {
    //                    return "False";
    //                }


    //            }
    //            else
    //            {
    //                return "You are not allowed to perform this operation";
    //            }
    //        }
    //        catch (System.Exception ex)
    //        {
    //            return "False";
    //        }


    //    }
    //    public string InsertUserBetAdmin(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string MarketBookid, string MarketBookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID, int UserID)
    //    {
    //        try
    //        {
    //            if (LoggedinUserDetail.GetUserTypeID() != 3)
    //            {
    //                selecitonname = selecitonname.Trim();
    //                var ID = objUsersServiceCleint.InsertUserBet(SelectionID, UserID, userOdd, amount, bettype, liveodd, ismatched, status, MarketBookid, DateTime.Now, DateTime.Now, selecitonname, MarketBookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID, LoggedinUserDetail.user.MaxOddBack, LoggedinUserDetail.user.MaxOddLay, LoggedinUserDetail.user.CheckforMaxOddBack, LoggedinUserDetail.user.CheckforMaxOddLay, LoggedinUserDetail.PasswordForValidate);
    //                return "True" + "|" + ID.ToString();
    //            }
    //            else
    //            {
    //                return "You are not allowed to perform this operation";
    //            }
    //        }
    //        catch (System.Exception ex)
    //        {
    //            return "False";
    //        }


    //    }
    //    public string UpdateUnMatchedStatustoComplete(string[] userbetsIDs)
    //    {

    //        long[] lstUserBetIDs = Array.ConvertAll(userbetsIDs, long.Parse);
    //        objUsersServiceCleint.UpdateUserBetUnMatchedStatusTocomplete(lstUserBetIDs.ToList(), LoggedinUserDetail.PasswordForValidate);
    //        return "True";

    //    }

    //}
}
