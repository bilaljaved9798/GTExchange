using bfnexchange.BettingServiceReference;
using bfnexchange.Models;
using bfnexchange.UsersServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace bfnexchange
{
    public class UserBetsUpdateUnmatcedBets
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceClient objBettingClient = new BettingServiceClient();
        public void UpdateUnMatchBetsforAllUsers()
        {
            try
            {
                using (UserServicesClient objUserServiceClient = new UserServicesClient())
                {
                    List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUserServiceClient.GetUnMatchedBets(ConfigurationManager.AppSettings["PasswordForValidate"]));
                    if (lstUserBets.Where(i => i.isMatched == false).Count() > 0)
                    {
                        List<string> lstMarketIDs = lstUserBets.Select(i => i.MarketBookID).Distinct().ToList();
                        foreach (var item in lstMarketIDs)
                        {
                            List<string> lstIDs = new List<string>();

                            //lstIDs.Add(objUserServiceClient.GetSheetNamebyMarketID(item));
                            lstIDs.Add(item);
                            //var marketbooks = obMarketdata.GetMarketDatabyID(lstIDs.ToArray(), "","","");
                            List<UserBets> lstUserbetsbymarketid = lstUserBets.Where(i => i.MarketBookID == item).ToList();
                            string eventtypename = objUserServiceClient.GetEventTypeNamebyMarketID(item);
                            var marketbooks = objBettingClient.GetMarketDatabyIDLive(lstIDs.ToArray(), lstUserbetsbymarketid[0].MarketBookname, DateTime.Now, eventtypename, ConfigurationManager.AppSettings["PasswordForValidate"], ConfigurationManager.AppSettings["PasswordForValidateS"]);
                            if (marketbooks.Count() > 0)
                            {
                                LoggedinUserDetail.isWorkingonBets = true;
                                if (marketbooks[0].MainSportsname.Contains("Racing"))
                                {
                                    UpdateunmatchbetsUSRacing(marketbooks.ToArray(), lstUserbetsbymarketid);
                                }
                                else
                                {
                                    Updateunmatchbets(marketbooks.ToArray(), lstUserbetsbymarketid);
                                }

                                LoggedinUserDetail.isWorkingonBets = false;
                            }

                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.isWorkingonBets = false;
                LoggedinUserDetail.LogError(ex);
                APIConfig.WriteErrorToDB(ex.Message);
            }
            // UpdateUnMatchBetsforAllUsers();

        }
        public string UpdateUnMatchedStatus(string[] userbetsIDs)
        {
            long[] lstUserBetIDs = Array.ConvertAll(userbetsIDs, long.Parse);
            objUsersServiceCleint.UpdateUserBetMatched(lstUserBetIDs, ConfigurationManager.AppSettings["PasswordForValidate"]);
            return "True";

        }
        public string UpdateUnMatchedStatustoComplete(string[] userbetsIDs)
        {

            long[] lstUserBetIDs = Array.ConvertAll(userbetsIDs, long.Parse);
            objUsersServiceCleint.UpdateUserBetUnMatchedStatusTocomplete(lstUserBetIDs, ConfigurationManager.AppSettings["PasswordForValidate"]);
            return "True";

        }

        public string UpdateUserBetByID(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string marketbookid, string marketbookname, string Liablaity, decimal BetSize, decimal PendingAmount)
        {

            try
            {
                objUsersServiceCleint.UpdateUserBet(Convert.ToInt64(SelectionID), LoggedinUserDetail.GetUserID(), userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, Liablaity, BetSize.ToString(), PendingAmount, ConfigurationManager.AppSettings["PasswordForValidate"]);
                return "True";
            }
            catch (System.Exception ex)
            {
                return "False";
            }
        }

        public string UpdateUserAmountbyID(string ID, decimal amount, bool ismatched)
        {

            objUsersServiceCleint.UpdateUserbetamountbyID(Convert.ToInt64(ID), amount, ismatched, ConfigurationManager.AppSettings["PasswordForValidate"]);

            return "True";
        }
        public void UpdatePricesbyUserPoundrate(ExternalAPI.TO.MarketBook marketbook, int UserPoundRate)
        {
            try
            {

                foreach (var item in marketbook.Runners)
                {
                    foreach (var exchangeprice in item.ExchangePrices.AvailableToBack)
                    {
                        exchangeprice.Size = Convert.ToInt64(exchangeprice.OrignalSize * UserPoundRate);
                    }
                    foreach (var exchangeprice in item.ExchangePrices.AvailableToLay)
                    {
                        exchangeprice.Size = Convert.ToInt64(exchangeprice.OrignalSize * UserPoundRate);
                    }
                }
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                APIConfig.WriteErrorToDB(ex.Message);
            }
        }
        public void Updateunmatchbets(ExternalAPI.TO.MarketBook[] lstMarketbooks, List<UserBets> lstUserBets)

        {
            try
            {
                // List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
                List<UserBets> lstMatchedBets = lstUserBets.Where(a => a.isMatched == true).ToList();

                List<UserBets> lstUnMatchedBets = lstUserBets.Where(a => a.isMatched == false).ToList();
                double newamount = 0;
                foreach (var unmatchbets in lstUnMatchedBets)
                {

                    List<UserBets> lstdoublebets = lstUnMatchedBets.Where(a => a.ParentID == unmatchbets.ParentID).ToList();
                    if (lstdoublebets.Count > 1)
                    {
                        decimal totamount = lstdoublebets.Sum(a => Convert.ToDecimal(a.Amount));
                        List<string> IDs = new List<string>();
                        // unmatchbets.Amount = totamount.ToString();
                        for (int i = 1; i <= lstdoublebets.Count - 1; i++)
                        {
                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), Convert.ToDecimal(lstdoublebets[i].Amount), false);
                            IDs.Add(lstdoublebets[i].ID.ToString());

                        }
                        UpdateUnMatchedStatustoComplete(IDs.ToArray());
                        return;
                    }

                    var marketbookitem = lstMarketbooks.Where(item => item.MarketId == unmatchbets.MarketBookID).FirstOrDefault();
                    if (marketbookitem != null)
                    {
                        if (marketbookitem.IsMarketDataDelayed == true)
                        {
                            return;
                        }
                        UpdatePricesbyUserPoundrate(marketbookitem, unmatchbets.PoundRateB);
                        if (marketbookitem.Runners.Count == 1)
                        {
                            if (unmatchbets.BetType == "back")
                            {
                                foreach (var runner in marketbookitem.Runners[0].ExchangePrices.AvailableToBack.Take(1))
                                {
                                    if (runner.Price <= Convert.ToDouble(unmatchbets.UserOdd) && runner.Price > 0)
                                    {
                                        objUsersServiceCleint.UpdateUserOddbyID(unmatchbets.ID, runner.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, runner.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var runner in marketbookitem.Runners[0].ExchangePrices.AvailableToLay.Take(1))
                                {
                                    if (runner.Price >= Convert.ToDouble(unmatchbets.UserOdd) && runner.Price > 0)
                                    {
                                        objUsersServiceCleint.UpdateUserOddbyID(unmatchbets.ID, runner.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, runner.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                        return;
                                    }
                                }
                            }
                            return;
                        }
                        Services.DBModel.SP_Users_GetMaxOddBackandLay_Result objMaxOddBack = objUsersServiceCleint.GetMaxOddBackandLay(unmatchbets.UserID);
                        var runners = marketbookitem.Runners;

                        var runneritem = runners.Where(item => item.SelectionId == unmatchbets.SelectionID).First();

                        if (unmatchbets.BetType == "back")
                        {
                            newamount = Convert.ToDouble(unmatchbets.Amount);
                            foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                            {
                                if (backitems.Price.ToString() == unmatchbets.UserOdd)
                                {
                                    //if (unmatchbets.BetSize == "")
                                    //{
                                    //    unmatchbets.BetSize = runneritem.ExchangePrices.AvailableToBack[0].Size.ToString();
                                    //}
                                    if (unmatchbets.BetSize != backitems.Size.ToString())
                                    {
                                        unmatchbets.BetSize = backitems.Size.ToString();
                                        objUsersServiceCleint.UpdateBetSizebyID(unmatchbets.ID, backitems.Size.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        if (backitems.Size >= Convert.ToDouble(unmatchbets.Amount))
                                        {

                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                            if (matcheditems.Count() > 0)
                                            {
                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(unmatchbets.Amount), unmatchbets.UserOdd);
                                                newamount = 0;
                                                List<string> IDs = new List<string>();
                                                IDs.Add(unmatchbets.ID.ToString());
                                                UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                            }
                                            else
                                            {
                                                objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                                //  InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.SelectionName, backitems.Price.ToString(), unmatchbets.Amount.ToString(), "back", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, unmatchbets.MarketBookname, "0", Convert.ToDecimal(backitems.Size), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID);
                                                newamount = 0;
                                            }
                                        }
                                        else
                                        {
                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(backitems.Size), false);
                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                            if (matcheditems.Count() > 0)
                                            {
                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(backitems.Size), unmatchbets.UserOdd);

                                            }
                                            else
                                            {

                                                objUsersServiceCleint.InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.UserID, backitems.Price.ToString(), backitems.Size.ToString(), "back", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, DateTime.Now, DateTime.Now, unmatchbets.SelectionName, unmatchbets.MarketBookname, "0", backitems.Size.ToString(), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                            }


                                            newamount = Convert.ToDouble(unmatchbets.Amount) - backitems.Size;
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            newamount = Convert.ToDouble(unmatchbets.Amount);
                            foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                            {
                                if (backitems.Price.ToString() == unmatchbets.UserOdd)
                                {
                                    if (unmatchbets.BetSize != backitems.Size.ToString())
                                    {
                                        unmatchbets.BetSize = backitems.Size.ToString();
                                        objUsersServiceCleint.UpdateBetSizebyID(unmatchbets.ID, backitems.Size.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        if (backitems.Size >= Convert.ToDouble(unmatchbets.Amount))
                                        {

                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                            if (matcheditems.Count() > 0)
                                            {
                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(unmatchbets.Amount), unmatchbets.UserOdd);
                                                newamount = 0;
                                                List<string> IDs = new List<string>();
                                                IDs.Add(unmatchbets.ID.ToString());
                                                UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                            }
                                            else
                                            {
                                                objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                                // InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.SelectionName, backitems.Price.ToString(), unmatchbets.Amount.ToString(), "lay", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, unmatchbets.MarketBookname, "0", Convert.ToDecimal(backitems.Size), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID);
                                                newamount = 0;
                                            }
                                        }
                                        else
                                        {
                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(backitems.Size), false);
                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                            if (matcheditems.Count() > 0)
                                            {
                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(backitems.Size), unmatchbets.UserOdd);

                                            }
                                            else
                                            {

                                                objUsersServiceCleint.InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.UserID, backitems.Price.ToString(), backitems.Size.ToString(), "lay", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, DateTime.Now, DateTime.Now, unmatchbets.SelectionName, unmatchbets.MarketBookname, "0", backitems.Size.ToString(), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                ///  InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.SelectionName, backitems.Price.ToString(), backitems.Size.ToString(), "lay", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, unmatchbets.MarketBookname, "0", Convert.ToDecimal(backitems.Size), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID);
                                            }


                                            newamount = Convert.ToDouble(unmatchbets.Amount) - backitems.Size;
                                        }
                                    }

                                }
                            }
                        }
                        List<string> useroddvisited = new List<string>();
                        useroddvisited.Add(unmatchbets.UserOdd);
                        if (newamount > 0)
                        {

                            var lstMatchedBetsbyParentID = lstMatchedBets.Where(item => item.ParentID == unmatchbets.ParentID && item.UserOdd != unmatchbets.UserOdd);
                            foreach (var matceditem in lstMatchedBetsbyParentID)
                            {
                                if (newamount > 0)
                                {
                                    useroddvisited.Add(matceditem.UserOdd);
                                    if (matceditem.BetType == "back")
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                                        {
                                            if (newamount > 0)
                                            {
                                                if (backitems.Price.ToString() == matceditem.UserOdd)
                                                {
                                                    if (matceditem.BetSize != backitems.Size.ToString())
                                                    {
                                                        unmatchbets.BetSize = backitems.Size.ToString();
                                                        //newamount = Convert.ToDouble(matceditem.BetSize);
                                                        if (backitems.Size >= newamount)
                                                        {
                                                            List<string> IDs = new List<string>();
                                                            IDs.Add(unmatchbets.ID.ToString());
                                                            UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                                            objUsersServiceCleint.UpdateLiveOddbyID(matceditem.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            UpdateUserAmountbyID(matceditem.ID.ToString(), Convert.ToDecimal(newamount), true);
                                                            newamount = 0;
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            objUsersServiceCleint.UpdateLiveOddbyID(matceditem.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            UpdateUserAmountbyID(matceditem.ID.ToString(), Convert.ToDecimal(backitems.Size), true);
                                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(backitems.Size), false);
                                                            newamount = newamount - backitems.Size;


                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                                        {
                                            if (newamount > 0)
                                            {
                                                if (backitems.Price.ToString() == matceditem.UserOdd)
                                                {
                                                    if (matceditem.BetSize != backitems.Size.ToString())
                                                    {
                                                        unmatchbets.BetSize = backitems.Size.ToString();
                                                        //newamount = Convert.ToDouble(matceditem.BetSize);
                                                        if (backitems.Size >= newamount)
                                                        {
                                                            List<string> IDs = new List<string>();
                                                            IDs.Add(unmatchbets.ID.ToString());
                                                            UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                                            objUsersServiceCleint.UpdateLiveOddbyID(matceditem.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            UpdateUserAmountbyID(matceditem.ID.ToString(), Convert.ToDecimal(newamount), true);
                                                            newamount = 0;
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            newamount = Convert.ToDouble(unmatchbets.BetSize) - backitems.Size;
                                                            objUsersServiceCleint.UpdateLiveOddbyID(matceditem.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            UpdateUserAmountbyID(matceditem.ID.ToString(), Convert.ToDecimal(backitems.Size), true);
                                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(backitems.Size), false);



                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }

                            }


                        }
                        if (newamount > 0)
                        {
                            if (unmatchbets.BetType == "back")
                            {
                                if (marketbookitem.MarketBookName.Contains("(US)") && marketbookitem.MainSportsname.Contains("Racing"))
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(1))
                                    {
                                        if (newamount > 0)
                                        {
                                            if (backitems.Price > Convert.ToDouble(unmatchbets.UserOdd))
                                            {
                                                if (!useroddvisited.Contains(backitems.Price.ToString()))
                                                {
                                                    if (unmatchbets.BetSize != backitems.Size.ToString())
                                                    {
                                                        unmatchbets.BetSize = backitems.Size.ToString();
                                                        objUsersServiceCleint.UpdateBetSizebyID(unmatchbets.ID, backitems.Size.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                        if (backitems.Size >= Convert.ToDouble(unmatchbets.Amount))
                                                        {

                                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                                            if (matcheditems.Count() > 0)
                                                            {
                                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(unmatchbets.Amount), unmatchbets.UserOdd);
                                                                newamount = 0;
                                                                List<string> IDs = new List<string>();
                                                                IDs.Add(unmatchbets.ID.ToString());
                                                                UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                                            }
                                                            else
                                                            {
                                                                objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                                                //  InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.SelectionName, backitems.Price.ToString(), unmatchbets.Amount.ToString(), "back", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, unmatchbets.MarketBookname, "0", Convert.ToDecimal(backitems.Size), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID);
                                                                newamount = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(backitems.Size), false);
                                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                                            if (matcheditems.Count() > 0)
                                                            {
                                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(backitems.Size), unmatchbets.UserOdd);

                                                            }
                                                            else
                                                            {

                                                                objUsersServiceCleint.InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.UserID, unmatchbets.UserOdd.ToString(), backitems.Size.ToString(), "back", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, DateTime.Now, DateTime.Now, unmatchbets.SelectionName, unmatchbets.MarketBookname, "0", backitems.Size.ToString(), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            }


                                                            newamount = Convert.ToDouble(unmatchbets.Amount) - backitems.Size;
                                                        }
                                                    }
                                                    /////NewCode for updateunmatched bet if odd is increase

                                                    // return;
                                                    /////////

                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {

                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(3))
                                    {
                                        if (newamount > 0)
                                        {
                                            if (backitems.Price > Convert.ToDouble(unmatchbets.UserOdd))
                                            {
                                                if (!useroddvisited.Contains(backitems.Price.ToString()))
                                                {
                                                    /////NewCode for updateunmatched bet if odd is increase
                                                    if (!marketbookitem.MainSportsname.Contains("Racing"))
                                                    {
                                                        objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                        UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                                        newamount = 0;
                                                    }
                                                    else
                                                    {
                                                        if (marketbookitem.MarketBookName.Contains("(US)"))
                                                        {

                                                        }
                                                        else
                                                        {
                                                            double oddamount = 0;
                                                            if (newamount > backitems.Size)
                                                            {
                                                                oddamount = backitems.Size;
                                                                newamount = newamount - oddamount;
                                                            }
                                                            else
                                                            {
                                                                oddamount = newamount;
                                                                newamount = 0;
                                                            }

                                                            objUsersServiceCleint.InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.UserID, backitems.Price.ToString(), oddamount.ToString(), "back", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, DateTime.Now, DateTime.Now, unmatchbets.SelectionName, unmatchbets.MarketBookname, "0", backitems.Size.ToString(), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            // InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.SelectionName, backitems.Price.ToString(), oddamount.ToString(), "back", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, unmatchbets.MarketBookname, "0", Convert.ToDecimal(backitems.Size), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID);
                                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(oddamount), false);
                                                            if (newamount == 0)
                                                            {
                                                                List<string> IDs = new List<string>();
                                                                IDs.Add(unmatchbets.ID.ToString());
                                                                UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                                            }
                                                        }

                                                    }
                                                    // return;
                                                    /////////

                                                }

                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                if (marketbookitem.MarketBookName.Contains("(US)") && marketbookitem.MainSportsname.Contains("Racing"))
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(1))
                                    {
                                        if (newamount > 0)
                                        {
                                            if (backitems.Price < Convert.ToDouble(unmatchbets.UserOdd) && backitems.Price > 1)
                                            {
                                                if (!useroddvisited.Contains(backitems.Price.ToString()))
                                                {
                                                    if (!marketbookitem.MainSportsname.Contains("Racing"))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        if (marketbookitem.MarketBookName.Contains("(US)"))
                                                        {
                                                            if (unmatchbets.BetSize != backitems.Size.ToString())
                                                            {
                                                                unmatchbets.BetSize = backitems.Size.ToString();
                                                                objUsersServiceCleint.UpdateBetSizebyID(unmatchbets.ID, backitems.Size.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                if (backitems.Size >= Convert.ToDouble(unmatchbets.Amount))
                                                                {

                                                                    var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                                                    if (matcheditems.Count() > 0)
                                                                    {
                                                                        objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                        UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(unmatchbets.Amount), unmatchbets.UserOdd);
                                                                        newamount = 0;
                                                                        List<string> IDs = new List<string>();
                                                                        IDs.Add(unmatchbets.ID.ToString());
                                                                        UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                                                    }
                                                                    else
                                                                    {
                                                                        objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                        UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                                                        //  InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.SelectionName, backitems.Price.ToString(), unmatchbets.Amount.ToString(), "back", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, unmatchbets.MarketBookname, "0", Convert.ToDecimal(backitems.Size), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID);
                                                                        newamount = 0;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(backitems.Size), false);
                                                                    var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                                                    if (matcheditems.Count() > 0)
                                                                    {
                                                                        objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                        UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(backitems.Size), unmatchbets.UserOdd);

                                                                    }
                                                                    else
                                                                    {

                                                                        objUsersServiceCleint.InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.UserID, unmatchbets.UserOdd.ToString(), backitems.Size.ToString(), "lay", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, DateTime.Now, DateTime.Now, unmatchbets.SelectionName, unmatchbets.MarketBookname, "0", backitems.Size.ToString(), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                    }


                                                                    newamount = Convert.ToDouble(unmatchbets.Amount) - backitems.Size;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {

                                                        }
                                                    }
                                                    // return;

                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(3))
                                    {
                                        if (newamount > 0)
                                        {
                                            if (backitems.Price < Convert.ToDouble(unmatchbets.UserOdd) && backitems.Price > 1)
                                            {
                                                if (!useroddvisited.Contains(backitems.Price.ToString()))
                                                {
                                                    if (!marketbookitem.MainSportsname.Contains("Racing"))
                                                    {
                                                        objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                        UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                                        newamount = 0;
                                                    }
                                                    else
                                                    {
                                                        if (marketbookitem.MarketBookName.Contains("(US)"))
                                                        {

                                                        }
                                                        else
                                                        {
                                                            double oddamount = 0;
                                                            if (newamount > backitems.Size)
                                                            {
                                                                oddamount = backitems.Size;
                                                                newamount = newamount - oddamount;
                                                            }
                                                            else
                                                            {
                                                                oddamount = newamount;
                                                                newamount = 0;
                                                            }
                                                            objUsersServiceCleint.InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.UserID, backitems.Price.ToString(), oddamount.ToString(), "lay", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, DateTime.Now, DateTime.Now, unmatchbets.SelectionName, unmatchbets.MarketBookname, "0", backitems.Size.ToString(), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            //  InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.SelectionName, backitems.Price.ToString(), oddamount.ToString(), "lay", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, unmatchbets.MarketBookname, "0", Convert.ToDecimal(backitems.Size), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID);
                                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(oddamount), false);
                                                            if (newamount == 0)
                                                            {
                                                                List<string> IDs = new List<string>();
                                                                IDs.Add(unmatchbets.ID.ToString());
                                                                UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                                            }
                                                        }
                                                    }
                                                    // return;

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
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                LoggedinUserDetail.isWorkingonBets = false;

            }

        }
        public void UpdateunmatchbetsUSRacing(ExternalAPI.TO.MarketBook[] lstMarketbooks, List<UserBets> lstUserBets)

        {
            try
            {
                // List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
                List<UserBets> lstMatchedBets = lstUserBets.Where(a => a.isMatched == true).ToList();

                List<UserBets> lstUnMatchedBets = lstUserBets.Where(a => a.isMatched == false).ToList();
                double newamount = 0;
                foreach (var unmatchbets in lstUnMatchedBets)
                {

                    List<UserBets> lstdoublebets = lstUnMatchedBets.Where(a => a.ParentID == unmatchbets.ParentID).ToList();
                    if (lstdoublebets.Count > 1)
                    {
                        decimal totamount = lstdoublebets.Sum(a => Convert.ToDecimal(a.Amount));
                        List<string> IDs = new List<string>();
                        // unmatchbets.Amount = totamount.ToString();
                        for (int i = 1; i <= lstdoublebets.Count - 1; i++)
                        {
                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), Convert.ToDecimal(lstdoublebets[i].Amount), false);
                            IDs.Add(lstdoublebets[i].ID.ToString());

                        }
                        UpdateUnMatchedStatustoComplete(IDs.ToArray());
                        return;
                    }

                    var marketbookitem = lstMarketbooks.Where(item => item.MarketId == unmatchbets.MarketBookID).FirstOrDefault();
                    if (marketbookitem != null)
                    {
                        if (marketbookitem.IsMarketDataDelayed == true)
                        {
                            return;
                        }
                        UpdatePricesbyUserPoundrate(marketbookitem, unmatchbets.PoundRateB);
                        if (marketbookitem.Runners.Count == 1)
                        {
                            if (unmatchbets.BetType == "back")
                            {
                                foreach (var runner in marketbookitem.Runners[0].ExchangePrices.AvailableToBack.Take(1))
                                {
                                    if (runner.Price <= Convert.ToDouble(unmatchbets.UserOdd) && runner.Price > 0)
                                    {
                                        objUsersServiceCleint.UpdateUserOddbyID(unmatchbets.ID, runner.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, runner.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var runner in marketbookitem.Runners[0].ExchangePrices.AvailableToLay.Take(1))
                                {
                                    if (runner.Price >= Convert.ToDouble(unmatchbets.UserOdd) && runner.Price > 0)
                                    {
                                        objUsersServiceCleint.UpdateUserOddbyID(unmatchbets.ID, runner.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, runner.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                        return;
                                    }
                                }
                            }
                            return;
                        }
                        Services.DBModel.SP_Users_GetMaxOddBackandLay_Result objMaxOddBack = objUsersServiceCleint.GetMaxOddBackandLay(unmatchbets.UserID);
                        var runners = marketbookitem.Runners;

                        var runneritem = runners.Where(item => item.SelectionId == unmatchbets.SelectionID).First();

                        if (unmatchbets.BetType == "back")
                        {
                            newamount = Convert.ToDouble(unmatchbets.Amount);
                            foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(1))
                            {
                                if (backitems.Price.ToString() == unmatchbets.UserOdd)
                                {
                                    //if (unmatchbets.BetSize == "")
                                    //{
                                    //    unmatchbets.BetSize = runneritem.ExchangePrices.AvailableToBack[0].Size.ToString();
                                    //}
                                    if (unmatchbets.BetSize != backitems.Size.ToString())
                                    {
                                        unmatchbets.BetSize = backitems.Size.ToString();
                                        objUsersServiceCleint.UpdateBetSizebyID(unmatchbets.ID, backitems.Size.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        if (backitems.Size >= Convert.ToDouble(unmatchbets.Amount))
                                        {

                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                            if (matcheditems.Count() > 0)
                                            {
                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(unmatchbets.Amount), unmatchbets.UserOdd);
                                                newamount = 0;
                                                List<string> IDs = new List<string>();
                                                IDs.Add(unmatchbets.ID.ToString());
                                                UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                            }
                                            else
                                            {
                                                objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                                newamount = 0;
                                            }
                                        }
                                        else
                                        {
                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(backitems.Size), false);
                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                            if (matcheditems.Count() > 0)
                                            {
                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(backitems.Size), unmatchbets.UserOdd);

                                            }
                                            else
                                            {

                                                objUsersServiceCleint.InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.UserID, backitems.Price.ToString(), backitems.Size.ToString(), "back", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, DateTime.Now, DateTime.Now, unmatchbets.SelectionName, unmatchbets.MarketBookname, "0", backitems.Size.ToString(), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                            }


                                            newamount = Convert.ToDouble(unmatchbets.Amount) - backitems.Size;
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            newamount = Convert.ToDouble(unmatchbets.Amount);
                            foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(1))
                            {
                                if (backitems.Price.ToString() == unmatchbets.UserOdd)
                                {
                                    if (unmatchbets.BetSize != backitems.Size.ToString())
                                    {
                                        unmatchbets.BetSize = backitems.Size.ToString();
                                        objUsersServiceCleint.UpdateBetSizebyID(unmatchbets.ID, backitems.Size.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                        if (backitems.Size >= Convert.ToDouble(unmatchbets.Amount))
                                        {

                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                            if (matcheditems.Count() > 0)
                                            {
                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(unmatchbets.Amount), unmatchbets.UserOdd);
                                                newamount = 0;
                                                List<string> IDs = new List<string>();
                                                IDs.Add(unmatchbets.ID.ToString());
                                                UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                            }
                                            else
                                            {
                                                objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                                // InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.SelectionName, backitems.Price.ToString(), unmatchbets.Amount.ToString(), "lay", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, unmatchbets.MarketBookname, "0", Convert.ToDecimal(backitems.Size), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID);
                                                newamount = 0;
                                            }
                                        }
                                        else
                                        {
                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(backitems.Size), false);
                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                            if (matcheditems.Count() > 0)
                                            {
                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(backitems.Size), unmatchbets.UserOdd);

                                            }
                                            else
                                            {

                                                objUsersServiceCleint.InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.UserID, backitems.Price.ToString(), backitems.Size.ToString(), "lay", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, DateTime.Now, DateTime.Now, unmatchbets.SelectionName, unmatchbets.MarketBookname, "0", backitems.Size.ToString(), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                ///  InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.SelectionName, backitems.Price.ToString(), backitems.Size.ToString(), "lay", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, unmatchbets.MarketBookname, "0", Convert.ToDecimal(backitems.Size), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID);
                                            }


                                            newamount = Convert.ToDouble(unmatchbets.Amount) - backitems.Size;
                                        }
                                    }

                                }
                            }
                        }
                        List<string> useroddvisited = new List<string>();
                        useroddvisited.Add(unmatchbets.UserOdd);
                        if (newamount > 0)
                        {

                            var lstMatchedBetsbyParentID = lstMatchedBets.Where(item => item.ParentID == unmatchbets.ParentID && item.UserOdd != unmatchbets.UserOdd);
                            foreach (var matceditem in lstMatchedBetsbyParentID)
                            {
                                if (newamount > 0)
                                {
                                    useroddvisited.Add(matceditem.UserOdd);
                                    if (matceditem.BetType == "back")
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(1))
                                        {
                                            if (newamount > 0)
                                            {
                                                if (backitems.Price.ToString() == matceditem.UserOdd)
                                                {
                                                    if (matceditem.BetSize != backitems.Size.ToString())
                                                    {
                                                        unmatchbets.BetSize = backitems.Size.ToString();
                                                        //newamount = Convert.ToDouble(matceditem.BetSize);
                                                        if (backitems.Size >= newamount)
                                                        {
                                                            List<string> IDs = new List<string>();
                                                            IDs.Add(unmatchbets.ID.ToString());
                                                            UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                                            objUsersServiceCleint.UpdateLiveOddbyID(matceditem.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            UpdateUserAmountbyID(matceditem.ID.ToString(), Convert.ToDecimal(newamount), true);
                                                            newamount = 0;
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            objUsersServiceCleint.UpdateLiveOddbyID(matceditem.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            UpdateUserAmountbyID(matceditem.ID.ToString(), Convert.ToDecimal(backitems.Size), true);
                                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(backitems.Size), false);
                                                            newamount = newamount - backitems.Size;


                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(1))
                                        {
                                            if (newamount > 0)
                                            {
                                                if (backitems.Price.ToString() == matceditem.UserOdd)
                                                {
                                                    if (matceditem.BetSize != backitems.Size.ToString())
                                                    {
                                                        unmatchbets.BetSize = backitems.Size.ToString();
                                                        //newamount = Convert.ToDouble(matceditem.BetSize);
                                                        if (backitems.Size >= newamount)
                                                        {
                                                            List<string> IDs = new List<string>();
                                                            IDs.Add(unmatchbets.ID.ToString());
                                                            UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                                            objUsersServiceCleint.UpdateLiveOddbyID(matceditem.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            UpdateUserAmountbyID(matceditem.ID.ToString(), Convert.ToDecimal(newamount), true);
                                                            newamount = 0;
                                                            return;
                                                        }
                                                        else
                                                        {
                                                            newamount = Convert.ToDouble(unmatchbets.BetSize) - backitems.Size;
                                                            objUsersServiceCleint.UpdateLiveOddbyID(matceditem.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            UpdateUserAmountbyID(matceditem.ID.ToString(), Convert.ToDecimal(backitems.Size), true);
                                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(backitems.Size), false);



                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }

                            }


                        }
                        if (newamount > 0)
                        {
                            if (unmatchbets.BetType == "back")
                            {
                                if (marketbookitem.MainSportsname.Contains("Racing"))
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToBack.Take(1))
                                    {
                                        if (newamount > 0)
                                        {
                                            if (backitems.Price > Convert.ToDouble(unmatchbets.UserOdd))
                                            {
                                                if (!useroddvisited.Contains(backitems.Price.ToString()))
                                                {
                                                    if (unmatchbets.BetSize != backitems.Size.ToString())
                                                    {
                                                        unmatchbets.BetSize = backitems.Size.ToString();
                                                        objUsersServiceCleint.UpdateBetSizebyID(unmatchbets.ID, backitems.Size.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                        if (backitems.Size >= Convert.ToDouble(unmatchbets.Amount))
                                                        {

                                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                                            if (matcheditems.Count() > 0)
                                                            {
                                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(unmatchbets.Amount), unmatchbets.UserOdd);
                                                                newamount = 0;
                                                                List<string> IDs = new List<string>();
                                                                IDs.Add(unmatchbets.ID.ToString());
                                                                UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                                            }
                                                            else
                                                            {
                                                                objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                                                //  InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.SelectionName, backitems.Price.ToString(), unmatchbets.Amount.ToString(), "back", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, unmatchbets.MarketBookname, "0", Convert.ToDecimal(backitems.Size), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID);
                                                                newamount = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(backitems.Size), false);
                                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                                            if (matcheditems.Count() > 0)
                                                            {
                                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(backitems.Size), unmatchbets.UserOdd);

                                                            }
                                                            else
                                                            {

                                                                objUsersServiceCleint.InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.UserID, unmatchbets.UserOdd.ToString(), backitems.Size.ToString(), "back", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, DateTime.Now, DateTime.Now, unmatchbets.SelectionName, unmatchbets.MarketBookname, "0", backitems.Size.ToString(), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            }


                                                            newamount = Convert.ToDouble(unmatchbets.Amount) - backitems.Size;
                                                        }
                                                    }


                                                }

                                            }
                                        }
                                    }
                                }


                            }
                            else
                            {
                                if (marketbookitem.MainSportsname.Contains("Racing"))
                                {
                                    foreach (var backitems in runneritem.ExchangePrices.AvailableToLay.Take(1))
                                    {
                                        if (newamount > 0)
                                        {
                                            if (backitems.Price < Convert.ToDouble(unmatchbets.UserOdd) && backitems.Price > 1)
                                            {
                                                if (!useroddvisited.Contains(backitems.Price.ToString()))
                                                {
                                                    if (unmatchbets.BetSize != backitems.Size.ToString())
                                                    {
                                                        unmatchbets.BetSize = backitems.Size.ToString();
                                                        objUsersServiceCleint.UpdateBetSizebyID(unmatchbets.ID, backitems.Size.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                        if (backitems.Size >= Convert.ToDouble(unmatchbets.Amount))
                                                        {

                                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                                            if (matcheditems.Count() > 0)
                                                            {
                                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(unmatchbets.Amount), unmatchbets.UserOdd);
                                                                newamount = 0;
                                                                List<string> IDs = new List<string>();
                                                                IDs.Add(unmatchbets.ID.ToString());
                                                                UpdateUnMatchedStatustoComplete(IDs.ToArray());
                                                            }
                                                            else
                                                            {
                                                                objUsersServiceCleint.UpdateLiveOddbyID(unmatchbets.ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                UpdateUserAmountbyID(unmatchbets.ID.ToString(), 0, true);
                                                                //  InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.SelectionName, backitems.Price.ToString(), unmatchbets.Amount.ToString(), "back", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, unmatchbets.MarketBookname, "0", Convert.ToDecimal(backitems.Size), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID);
                                                                newamount = 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            UpdateUserAmountbyID(unmatchbets.ID.ToString(), -1 * Convert.ToDecimal(backitems.Size), false);
                                                            var matcheditems = lstMatchedBets.Where(item => item.UserOdd == unmatchbets.UserOdd && item.ParentID == unmatchbets.ParentID).ToList();
                                                            if (matcheditems.Count() > 0)
                                                            {
                                                                objUsersServiceCleint.UpdateLiveOddbyID(matcheditems[0].ID, backitems.Price.ToString(), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                                UpdateUserAmountbyParentID(unmatchbets.ParentID.ToString(), Convert.ToDecimal(backitems.Size), unmatchbets.UserOdd);

                                                            }
                                                            else
                                                            {

                                                                objUsersServiceCleint.InsertUserBet(unmatchbets.SelectionID.ToString(), unmatchbets.UserID, unmatchbets.UserOdd.ToString(), backitems.Size.ToString(), "lay", backitems.Price.ToString(), true, "In-Complete", unmatchbets.MarketBookID, DateTime.Now, DateTime.Now, unmatchbets.SelectionName, unmatchbets.MarketBookname, "0", backitems.Size.ToString(), Convert.ToDecimal(newamount), "-1", unmatchbets.ParentID, Convert.ToDecimal(objMaxOddBack.MaxOddBack), Convert.ToDecimal(objMaxOddBack.MaxOddLay), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddBack), Convert.ToBoolean(objMaxOddBack.CheckforMaxOddLay), ConfigurationManager.AppSettings["PasswordForValidate"]);
                                                            }


                                                            newamount = Convert.ToDouble(unmatchbets.Amount) - backitems.Size;
                                                        }
                                                    }

                                                    // return;

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
            catch (System.Exception ex)
            {
                LoggedinUserDetail.isWorkingonBets = false;
                LoggedinUserDetail.LogError(ex);
            }

        }
        public string UpdateUserAmountbyParentID(string ID, decimal amount, string userodd)
        {

            objUsersServiceCleint.UpdateUserbetamountbyParentID(Convert.ToInt64(ID), amount, userodd, ConfigurationManager.AppSettings["PasswordForValidate"]);

            return "True";
        }

        public long GetLiabalityofCurrentUserActual(int userID, string selectionID, string BetType, string marketbookID, string Marketbookname, List<UserBets> lstUserBets)
        {
            long OverAllLiabality = 0;
            string LastProfitandLoss = "";
            //  List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userID, ConfigurationManager.AppSettings["PasswordForValidate"]));
            List<string> lstIDS = new List<string>();
            lstIDS.Add(marketbookID);
            // var lstMarketIDS = lstUserBets.Where(item2 => item2.SelectionID == selectionID).Select(item => item.MarketBookID).Distinct().ToArray();
            var lstMarketIDS = lstIDS;
            foreach (var item in lstMarketIDS)
            {
                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                objMarketbook.MarketBookName = Marketbookname;
                objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                foreach (var selectionitem in selectionIDs)
                {
                    ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    objMarketbook.Runners.Add(objrunner);
                }
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    objMarketbook.DebitCredit = ceckProfitandLoss(objMarketbook, lstUserBets);
                }

                List<UserBets> marketbooknames = new List<Models.UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }
                if (marketbookname.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in objMarketbook.Runners)
                    {

                        long Profit = 0;
                        long Loss = 0;
                        Profit = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        Loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                        if (Profit > Loss)
                        {
                            profitandloss += Loss;
                        }
                        else
                        {
                            profitandloss += Profit;
                        }

                    }
                    LastProfitandLoss = profitandloss.ToString();
                }
                else
                {
                    //if (objMarketbook.Runners.Count == 1)
                    //{
                    //    //if (BetType == "back")
                    //    //{
                    //    //    ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforAgent>(), lstUserBets);
                    //    //    if (CurrentMarketProfitandloss.Runners != null)
                    //    //    {
                    //    //        CurrentMarketProfitandloss.Runners = CurrentMarketProfitandloss.Runners.Where(item1 => Convert.ToInt32(item1.SelectionId) <= Convert.ToInt32(selectionID)).ToList();
                    //    //        LastProfitandLoss = CurrentMarketProfitandloss.Runners.Min(item2 => item2.ProfitandLoss).ToString();
                    //    //    }
                    //    //}
                    //    //else
                    //    //{
                    //    //    ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforAgent>(), lstUserBets);
                    //    //    if (CurrentMarketProfitandloss.Runners != null)
                    //    //    {
                    //    //        CurrentMarketProfitandloss.Runners = CurrentMarketProfitandloss.Runners.Where(item1 => Convert.ToInt32(item1.SelectionId) >= Convert.ToInt32(selectionID)).ToList();
                    //    //        LastProfitandLoss = CurrentMarketProfitandloss.Runners.Min(item2 => item2.ProfitandLoss).ToString();

                    //    //    }
                    //    //}

                    //}
                    //else
                    // {

                    if (BetType == "back")
                    {
                        foreach (var runner in objMarketbook.Runners)
                        {
                            if (runner.SelectionId != selectionID)
                            {
                                long ProfitandLoss = 0;
                                ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                if (LastProfitandLoss == "")
                                {
                                    LastProfitandLoss = ProfitandLoss.ToString();
                                }
                                else
                                {
                                    if (objMarketbook.Runners.Count == 1)
                                    {
                                        if (ProfitandLoss > 0)
                                        {
                                            ProfitandLoss = ProfitandLoss * 1;
                                        }
                                    }
                                    if (Convert.ToInt64(LastProfitandLoss) > ProfitandLoss)
                                    {
                                        LastProfitandLoss = ProfitandLoss.ToString();
                                    }
                                }

                                // return LastProfitandLoss;
                            }

                        }

                    }
                    else
                    {
                        long ProfitandLoss = 0;
                        ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == selectionID).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == selectionID).Sum(item2 => item2.Credit));
                        //ProfitandLoss = ProfitandLoss + Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID != runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID != runner.SelectionId).Sum(item2 => item2.Credit));
                        if (objMarketbook.Runners.Count == 1)
                        {
                            if (ProfitandLoss > 0)
                            {
                                ProfitandLoss = ProfitandLoss * 1;
                            }
                        }
                        LastProfitandLoss = ProfitandLoss.ToString();
                        // return LastProfitandLoss;
                        //if (LastProfitandLoss < ProfitandLoss)
                        //{
                        //    LastProfitandLoss = ProfitandLoss;
                        //}
                    }
                    //}

                }

            }
            if (LastProfitandLoss == "")
            {
                OverAllLiabality += 0;
            }
            else
            {
                OverAllLiabality += Convert.ToInt64(LastProfitandLoss);
            }

            LastProfitandLoss = "";
            // var currentMarketIDSOthers = lstUserBets.Where(item2 => item2.SelectionID == selectionID).Select(item => item.MarketBookID).FirstOrDefault();
            //OverAllLiabality += LastProfitandLoss;
            //LastProfitandLoss = 0;
            return OverAllLiabality;
        }




        public long GetLiabalityofCurrentUserActualforOtherMarkets(int userID, string selectionID, string BetType, string marketbookID, string Marketbookname, List<UserBets> lstUserBets)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;
            // List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userID, ConfigurationManager.AppSettings["PasswordForValidate"]));

            var lstMarketIDSOthers = lstUserBets.Where(item2 => item2.MarketBookID != marketbookID).Select(item => item.MarketBookID).Distinct().ToArray();
            foreach (var item in lstMarketIDSOthers)
            {
                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                objMarketbook.MarketBookName = Marketbookname;
                objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                foreach (var selectionitem in selectionIDs)
                {
                    ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    objMarketbook.Runners.Add(objrunner);
                }
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    if (objMarketbook.MarketId.Length > 8)
                    {
                        objMarketbook.DebitCredit = ceckProfitandLoss(objMarketbook, lstUserBets);
                    }
                }
                List<UserBets> marketbooknames = new List<Models.UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }
                if (marketbookname.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in objMarketbook.Runners)
                    {

                        long Profit = 0;
                        long Loss = 0;
                        Profit = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        Loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                        if (Profit > Loss)
                        {
                            profitandloss += Loss;
                        }
                        else
                        {
                            profitandloss += Profit;
                        }


                        // return LastProfitandLoss;


                    }
                    LastProfitandLoss = profitandloss;
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;


                }
                else
                {
                    if (objMarketbook.Runners.Count == 1)
                    {
                        long ProfitandLoss = 0;


                        ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforAgent>(), lstUserBets);
                        if (CurrentMarketProfitandloss.Runners != null)
                        {
                            ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                        }

                        LastProfitandLoss = ProfitandLoss;
                    }
                    else
                    {
                        foreach (var runner in objMarketbook.Runners)
                        {

                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                            if (objMarketbook.Runners.Count == 1)
                            {
                                if (ProfitandLoss > 0)
                                {
                                    ProfitandLoss = ProfitandLoss * 1;
                                }
                            }
                            if (LastProfitandLoss > ProfitandLoss)
                            {
                                LastProfitandLoss = ProfitandLoss;
                            }
                        }
                    }
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }


            }
            //OverAllLiabality += LastProfitandLoss;
            //LastProfitandLoss = 0;

            //OverAllLiabality += LastProfitandLoss;
            //LastProfitandLoss = 0;
            return OverAllLiabality;
        }
        public long GetLiabalityofCurrentUserfancy(int userID, List<UserBets> lstUserBet)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;
            //List<UserBets> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userID));
            List<UserBets> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
            var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();

            foreach (var item in lstMarketIDS)
            {

                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                objMarketbook.MarketId = item;
                foreach (var selectionitem in selectionIDs)
                {
                    ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    objMarketbook.Runners.Add(objrunner);
                }
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    objMarketbook.DebitCredit = ceckProfitandLossfancy(objMarketbook, lstUserBets);
                }
                List<UserBets> marketbooknames = new List<Models.UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }

                //if (objMarketbook.Runners.Count() == 1)
                //{
                    long ProfitandLoss = 0;

                    ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforAgent>(), lstUserBets);
                    if (CurrentMarketProfitandloss.Runners != null)
                    {
                        ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                    }

                    LastProfitandLoss = ProfitandLoss;
                //}
                //else
                //{
                //    foreach (var runner in objMarketbook.Runners)
                //    {
                //        long ProfitandLoss = 0;
                //        ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                //        if (LastProfitandLoss > ProfitandLoss)
                //        {
                //            LastProfitandLoss = ProfitandLoss;
                //        }
                //    }
                //}

                OverAllLiabality += LastProfitandLoss;
                LastProfitandLoss = 0;

            }

            return OverAllLiabality;
        }
        public long GetLiabalityofCurrentUser(int userID, List<UserBets> lstUserBet)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;
            //List<UserBets> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userID));
            List<UserBets> lstUserBets = lstUserBet.Where(item => item.isMatched == true && item.location != "8").ToList();
            var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();

            foreach (var item in lstMarketIDS)
            {

                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                foreach (var selectionitem in selectionIDs)
                {
                    ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    objMarketbook.Runners.Add(objrunner);
                }
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    objMarketbook.DebitCredit = ceckProfitandLoss(objMarketbook, lstUserBets);
                }
                List<UserBets> marketbooknames = new List<Models.UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }
                if (marketbookname.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in objMarketbook.Runners)
                    {

                        long Profit = 0;
                        long Loss = 0;
                        Profit = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        Loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                        if (Profit > Loss)
                        {
                            profitandloss += Loss;
                        }
                        else
                        {
                            profitandloss += Profit;
                        }


                        // return LastProfitandLoss;


                    }

                    LastProfitandLoss = profitandloss;
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }
                else
                {

                    if (objMarketbook.Runners.Count() == 1)
                    {
                        long ProfitandLoss = 0;


                        ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforAgent>(), lstUserBets);
                        if (CurrentMarketProfitandloss.Runners != null)
                        {
                            ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                        }

                        LastProfitandLoss = ProfitandLoss;
                    }
                    else
                    {
                        foreach (var runner in objMarketbook.Runners)
                        {
                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                            if (LastProfitandLoss > ProfitandLoss)
                            {
                                LastProfitandLoss = ProfitandLoss;
                            }
                        }
                    }




                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }



            }

            return OverAllLiabality;
        }
        public List<LiabalitybyMarket> GetLiabalityofCurrentUserbyMarkets(int userID, List<UserBets> lstUserBet)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;
            //List<UserBets> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userID));
            List<UserBets> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
            var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
            List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
            foreach (var item in lstMarketIDS)
            {
                LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                foreach (var selectionitem in selectionIDs)
                {
                    ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    objMarketbook.Runners.Add(objrunner);
                }
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    objMarketbook.DebitCredit = ceckProfitandLoss(objMarketbook, lstUserBets);
                }
                List<UserBets> marketbooknames = new List<Models.UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }
                if (marketbookname.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in objMarketbook.Runners)
                    {

                        long Profit = 0;
                        long Loss = 0;
                        Profit = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        Loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                        if (Profit > Loss)
                        {
                            profitandloss += Loss;
                        }
                        else
                        {
                            profitandloss += Profit;
                        }


                        // return LastProfitandLoss;


                    }
                    objMarketLiabality.MarketBookID = item;
                    objMarketLiabality.MarketBookName = marketbookname;
                    objMarketLiabality.Liabality = profitandloss;
                    lstLiabalitybyMarket.Add(objMarketLiabality);
                    LastProfitandLoss = profitandloss;
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }
                else
                {
                    foreach (var runner in objMarketbook.Runners)
                    {
                        long ProfitandLoss = 0;
                        ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        if (objMarketbook.Runners.Count == 1)
                        {
                            if (ProfitandLoss > 0)
                            {
                                ProfitandLoss = -1 * ProfitandLoss;
                            }
                        }

                        if (LastProfitandLoss > ProfitandLoss)
                        {
                            LastProfitandLoss = ProfitandLoss;
                        }
                    }
                    objMarketLiabality.MarketBookID = item;
                    objMarketLiabality.MarketBookName = marketbookname;
                    objMarketLiabality.Liabality = LastProfitandLoss;
                    lstLiabalitybyMarket.Add(objMarketLiabality);
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }



            }

            return lstLiabalitybyMarket;
        }

        public List<LiabalitybyMarket> GetLiabalityofCurrentUserbyMarketsfancy(int userID, List<UserBets> lstUserBet)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;
            //List<UserBets> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userID));
            List<UserBets> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
            var lstMarketIDS = lstUserBets.Select(item => item.MarketBookname).Distinct().ToArray();
            List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
            foreach (var item in lstMarketIDS)
            {
                LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                foreach (var selectionitem in selectionIDs)
                {
                    ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    objMarketbook.Runners.Add(objrunner);
                }
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    objMarketbook.DebitCredit = ceckProfitandLossfancy2(objMarketbook, lstUserBets);
                }
                List<UserBets> marketbooknames = new List<UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.MarketBookname == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }

                //foreach (var runner in objMarketbook.Runners)
                //{
                    long ProfitandLoss = 0;
                    ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == marketbooknames[0].SelectionID).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == marketbooknames[0].SelectionID).Sum(item2 => item2.Credit));
                    
                        if (ProfitandLoss > 0)
                        {
                            ProfitandLoss = -1 * ProfitandLoss;
                        }
                    

                    if (LastProfitandLoss > ProfitandLoss)
                    {
                        LastProfitandLoss = ProfitandLoss;
                    }
             //   }
                objMarketLiabality.MarketBookID = item;
                objMarketLiabality.MarketBookName = marketbookname;
                objMarketLiabality.Liabality = LastProfitandLoss;
                lstLiabalitybyMarket.Add(objMarketLiabality);
                OverAllLiabality += LastProfitandLoss;
                LastProfitandLoss = 0;

                //long ProfitandLoss = 0;
                //ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforSuper>(), new List<UserBetsforAgent>(), lstUserBets);
                //if (CurrentMarketProfitandloss.Runners != null)
                //{
                //    ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                //}
                //LastProfitandLoss = ProfitandLoss;
                //objMarketLiabality.MarketBookID = item;
                //objMarketLiabality.MarketBookName = marketbookname;
                //objMarketLiabality.Liabality = LastProfitandLoss;
                //lstLiabalitybyMarket.Add(objMarketLiabality);
                //OverAllLiabality += LastProfitandLoss;
                //LastProfitandLoss = 0;



            }

            return lstLiabalitybyMarket;
        }
        public long GetLiabalityofCurrentAgent(List<UserBetsforAgent> lstUserBet)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;

                List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
                var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                var lstUsers = lstUserBets.Select(item => new { item.UserID }).Distinct().ToArray();
                List<ExternalAPI.TO.MarketBook> lstmarketbooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                    objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketbook.Runners.Add(objrunner);
                    }

                    if (objMarketbook != null)
                    {
                        objMarketbook.MarketId = item;
                        List<UserBetsforAgent> marketbooknames = new List<Models.UserBetsforAgent>();
                        if (lstUserBets.Count > 0)
                        {
                            marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                        }
                        var marketbookname = "";
                        if (marketbooknames.Count > 0)
                        {
                            marketbookname = marketbooknames[0].MarketBookname;
                            objMarketbook.MarketBookName = marketbookname;
                        }

                        if (marketbookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {

                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;

                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, loss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.Loss += Convert.ToInt64(-1 * profit);


                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);
                                    if (objMarketbook.Runners.Count() == 1)
                                    {
                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                                            decimal profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
                                            //  decimal profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * profitorloss : ((Convert.ToDecimal(agentrate) / 100) * profitorloss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss);
                                            if (profit > 0)
                                            {
                                                profit = profit * -1;
                                            }
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        }
                                    }
                                    else
                                    {
                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);


                                        }
                                    }

                                }

                            }
                        }

                    }
                    lstmarketbooks.Add(objMarketbook);

                }

                foreach (var marketbook in lstmarketbooks)
                {

                    if (marketbook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in marketbook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            //Profit = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            //Loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                            Profit = runner.ProfitandLoss;
                            Loss = runner.Loss;
                            if (Profit < Loss)
                            {
                                profitandloss += Loss;
                            }
                            else
                            {
                                profitandloss += Profit;
                            }


                            // return LastProfitandLoss;


                        }
                        if (profitandloss >= 0)
                        {
                            profitandloss = profitandloss * -1;
                        }

                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in marketbook.Runners)
                        {

                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }

                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }


                }

                return OverAllLiabality;
            }
            catch (System.Exception ex)
            {
                return 0;
            }


        }
        public List<LiabalitybyMarket> GetLiabalityofCurrentAgentbyMarkets(List<UserBetsforAgent> lstUserBet)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;

                List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
                var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                var lstUsers = lstUserBets.Select(item => new { item.UserID }).Distinct().ToArray();
                List<ExternalAPI.TO.MarketBook> lstmarketbooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                    objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketbook.Runners.Add(objrunner);
                    }

                    if (objMarketbook != null)
                    {
                        objMarketbook.MarketId = item;
                        List<UserBetsforAgent> marketbooknames = new List<Models.UserBetsforAgent>();
                        if (lstUserBets.Count > 0)
                        {
                            marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                        }
                        var marketbookname = "";
                        if (marketbooknames.Count > 0)
                        {
                            marketbookname = marketbooknames[0].MarketBookname;
                            objMarketbook.MarketBookName = marketbookname;
                        }

                        if (marketbookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {

                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);


                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, loss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.Loss += Convert.ToInt64(-1 * profit);


                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);

                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (objMarketbook.Runners.Count == 1)
                                        {
                                            if (profitorloss > 0)
                                            {
                                                profitorloss = profitorloss * -1;
                                            }
                                        }
                                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate));
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);


                                    }
                                }

                            }
                        }

                    }
                    lstmarketbooks.Add(objMarketbook);

                }
                List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
                foreach (var marketbook in lstmarketbooks)
                {
                    LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                    if (marketbook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in marketbook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            Profit = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            Loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                            if (Profit < Loss)
                            {
                                profitandloss += Loss;
                            }
                            else
                            {
                                profitandloss += Profit;
                            }


                            // return LastProfitandLoss;


                        }
                        if (profitandloss >= 0)
                        {
                            profitandloss = profitandloss * -1;
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = profitandloss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in marketbook.Runners)
                        {

                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = LastProfitandLoss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }


                }

                return lstLiabalitybyMarket;
            }
            catch (System.Exception ex)
            {
                return new List<LiabalitybyMarket>();
            }


        }
        public long GetLiabalityofAdmin(List<UserBetsForAdmin> lstUserBet)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;
                //  List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsForAdmin());
                List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
                var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                var lstUsers = lstUserBets.Select(item => new { item.UserID }).Distinct().ToArray();
                List<ExternalAPI.TO.MarketBook> lstmarketbooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                    objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketbook.Runners.Add(objrunner);
                    }

                    if (objMarketbook != null)
                    {
                        objMarketbook.MarketId = item;
                        List<UserBetsForAdmin> marketbooknames = new List<Models.UserBetsForAdmin>();
                        if (lstUserBets.Count > 0)
                        {
                            marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                        }
                        var marketbookname = "";
                        if (marketbooknames.Count > 0)
                        {
                            marketbookname = marketbooknames[0].MarketBookname;
                            objMarketbook.MarketBookName = marketbookname;
                        }
                        if (marketbookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossAdmin(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {


                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;

                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        // decimal adminrate = 100 - Convert.ToDecimal(agentrate);
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (adminrate / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);

                                    }
                                }

                            }
                        }
                        else
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossAdmin(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    if (objMarketbook.Runners.Count() == 1)
                                    {
                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                                            decimal profit = ((100 - agentrate) / 100) * profitorloss;
                                            if (profit > 0)
                                            {
                                                profit = profit * -1;
                                            }
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        }
                                    }
                                    else
                                    {
                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                            decimal profit = (adminrate / 100) * profitorloss;
                                            // decimal profit = ((100 - agentrate) / 100) * profitorloss;
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        }
                                    }
                                }

                            }
                        }

                    }
                    lstmarketbooks.Add(objMarketbook);

                }

                foreach (var marketbook in lstmarketbooks)
                {

                    if (marketbook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in marketbook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            //Profit = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            //Loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                            Profit = runner.ProfitandLoss;
                            Loss = runner.Loss;
                            if (Profit < Loss)
                            {
                                profitandloss += Loss;
                            }
                            else
                            {
                                profitandloss += Profit;
                            }


                            // return LastProfitandLoss;


                        }
                        if (profitandloss >= 0)
                        {
                            profitandloss = profitandloss * -1;
                        }

                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in marketbook.Runners)
                        {

                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }

                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }


                }

                return OverAllLiabality;

            }
            catch (System.Exception ex)
            {
                return 0;
            }

        }
        public List<LiabalitybyMarket> GetLiabalityofAdminbyMarkets(List<UserBetsForAdmin> lstUserBet)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;
                //  List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsForAdmin());
                List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
                var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                var lstUsers = lstUserBets.Select(item => new { item.UserID }).Distinct().ToArray();
                List<ExternalAPI.TO.MarketBook> lstmarketbooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                    objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketbook.Runners.Add(objrunner);
                    }

                    if (objMarketbook != null)
                    {
                        objMarketbook.MarketId = item;
                        List<UserBetsForAdmin> marketbooknames = new List<Models.UserBetsForAdmin>();
                        if (lstUserBets.Count > 0)
                        {
                            marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                        }
                        var marketbookname = "";
                        if (marketbooknames.Count > 0)
                        {
                            marketbookname = marketbooknames[0].MarketBookname;
                            objMarketbook.MarketBookName = marketbookname;
                        }
                        if (marketbookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossAdmin(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {


                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        // decimal adminrate = 100 - Convert.ToDecimal(agentrate);
                                        // decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (adminrate / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);

                                    }
                                }

                            }
                        }
                        else
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossAdmin(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (objMarketbook.Runners.Count == 1)
                                        {
                                            if (profitorloss > 0)
                                            {
                                                profitorloss = profitorloss * -1;
                                            }
                                        }
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        //decimal profit = ((100 - agentrate) / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                    }
                                }

                            }
                        }

                    }
                    lstmarketbooks.Add(objMarketbook);

                }
                List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
                foreach (var marketbook in lstmarketbooks)
                {
                    LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                    if (marketbook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in marketbook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            Profit = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            Loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                            if (Profit < Loss)
                            {
                                profitandloss += Loss;
                            }
                            else
                            {
                                profitandloss += Profit;
                            }


                            // return LastProfitandLoss;


                        }
                        if (profitandloss >= 0)
                        {
                            profitandloss = profitandloss * -1;
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = profitandloss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in marketbook.Runners)
                        {

                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = LastProfitandLoss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }


                }

                return lstLiabalitybyMarket;

            }
            catch (System.Exception ex)
            {
                return new List<LiabalitybyMarket>();
            }

        }


        public long GetLiabalityofSuper(List<UserBetsforSuper> lstUserBet)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;
                //  List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsForAdmin());
                List<UserBetsforSuper> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
                var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                var lstUsers = lstUserBets.Select(item => new { item.UserID }).Distinct().ToArray();
                List<ExternalAPI.TO.MarketBook> lstmarketbooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                    objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketbook.Runners.Add(objrunner);
                    }

                    if (objMarketbook != null)
                    {
                        objMarketbook.MarketId = item;
                        List<UserBetsforSuper> marketbooknames = new List<Models.UserBetsforSuper>();
                        if (lstUserBets.Count > 0)
                        {
                            marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                        }
                        var marketbookname = "";
                        if (marketbooknames.Count > 0)
                        {
                            marketbookname = marketbooknames[0].MarketBookname;
                            objMarketbook.MarketBookName = marketbookname;
                        }
                        if (marketbookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossSuper(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {


                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;

                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        // decimal adminrate = 100 - Convert.ToDecimal(agentrate);
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (adminrate / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);

                                    }
                                }

                            }
                        }
                        else
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossSuper(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    if (objMarketbook.Runners.Count() == 1)
                                    {
                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                                            decimal profit = ((100 - agentrate) / 100) * profitorloss;
                                            if (profit > 0)
                                            {
                                                profit = profit * -1;
                                            }
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        }
                                    }
                                    else
                                    {
                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                            decimal profit = (adminrate / 100) * profitorloss;
                                            // decimal profit = ((100 - agentrate) / 100) * profitorloss;
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        }
                                    }
                                }

                            }
                        }

                    }
                    lstmarketbooks.Add(objMarketbook);

                }

                foreach (var marketbook in lstmarketbooks)
                {

                    if (marketbook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in marketbook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            //Profit = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            //Loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                            Profit = runner.ProfitandLoss;
                            Loss = runner.Loss;
                            if (Profit < Loss)
                            {
                                profitandloss += Loss;
                            }
                            else
                            {
                                profitandloss += Profit;
                            }


                            // return LastProfitandLoss;


                        }
                        if (profitandloss >= 0)
                        {
                            profitandloss = profitandloss * -1;
                        }

                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in marketbook.Runners)
                        {

                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }

                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }


                }

                return OverAllLiabality;

            }
            catch (System.Exception ex)
            {
                return 0;
            }

        }
        public List<LiabalitybyMarket> GetLiabalityofSuperbyMarkets(List<UserBetsforSuper> lstUserBet)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;
                //  List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsForAdmin());
                List<UserBetsforSuper> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
                var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                var lstUsers = lstUserBets.Select(item => new { item.UserID }).Distinct().ToArray();
                List<ExternalAPI.TO.MarketBook> lstmarketbooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                    objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketbook.Runners.Add(objrunner);
                    }

                    if (objMarketbook != null)
                    {
                        objMarketbook.MarketId = item;
                        List<UserBetsforSuper> marketbooknames = new List<Models.UserBetsforSuper>();
                        if (lstUserBets.Count > 0)
                        {
                            marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                        }
                        var marketbookname = "";
                        if (marketbooknames.Count > 0)
                        {
                            marketbookname = marketbooknames[0].MarketBookname;
                            objMarketbook.MarketBookName = marketbookname;
                        }
                        if (marketbookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossSuper(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {


                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        // decimal adminrate = 100 - Convert.ToDecimal(agentrate);
                                        // decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (adminrate / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);

                                    }
                                }

                            }
                        }
                        else
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossSuper(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (objMarketbook.Runners.Count == 1)
                                        {
                                            if (profitorloss > 0)
                                            {
                                                profitorloss = profitorloss * -1;
                                            }
                                        }
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        //decimal profit = ((100 - agentrate) / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                    }
                                }

                            }
                        }

                    }
                    lstmarketbooks.Add(objMarketbook);

                }
                List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
                foreach (var marketbook in lstmarketbooks)
                {
                    LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                    if (marketbook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in marketbook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            Profit = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            Loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                            if (Profit < Loss)
                            {
                                profitandloss += Loss;
                            }
                            else
                            {
                                profitandloss += Profit;
                            }


                            // return LastProfitandLoss;


                        }
                        if (profitandloss >= 0)
                        {
                            profitandloss = profitandloss * -1;
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = profitandloss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in marketbook.Runners)
                        {

                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = LastProfitandLoss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }


                }

                return lstLiabalitybyMarket;

            }
            catch (System.Exception ex)
            {
                return new List<LiabalitybyMarket>();
            }

        }


        public long GetLiabalityofSamiadmin(List<UserBetsforSamiadmin> lstUserBet)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;
                //  List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsForAdmin());
                List<UserBetsforSamiadmin> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
                var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                var lstUsers = lstUserBets.Select(item => new { item.UserID }).Distinct().ToArray();
                List<ExternalAPI.TO.MarketBook> lstmarketbooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                    objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketbook.Runners.Add(objrunner);
                    }

                    if (objMarketbook != null)
                    {
                        objMarketbook.MarketId = item;
                        List<UserBetsforSamiadmin> marketbooknames = new List<Models.UserBetsforSamiadmin>();
                        if (lstUserBets.Count > 0)
                        {
                            marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                        }
                        var marketbookname = "";
                        if (marketbooknames.Count > 0)
                        {
                            marketbookname = marketbooknames[0].MarketBookname;
                            objMarketbook.MarketBookName = marketbookname;
                        }
                        if (marketbookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforSamiadmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossSamiadmin(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {


                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;

                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;

                                        decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (adminrate / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);

                                    }
                                }

                            }
                        }
                        else
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforSamiadmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossSamiadmin(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    if (objMarketbook.Runners.Count() == 1)
                                    {
                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                                            decimal profit = ((100 - agentrate) / 100) * profitorloss;
                                            if (profit > 0)
                                            {
                                                profit = profit * -1;
                                            }
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        }
                                    }
                                    else
                                    {
                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                            decimal profit = (adminrate / 100) * profitorloss;
                                            // decimal profit = ((100 - agentrate) / 100) * profitorloss;
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        }
                                    }
                                }

                            }
                        }

                    }
                    lstmarketbooks.Add(objMarketbook);

                }

                foreach (var marketbook in lstmarketbooks)
                {

                    if (marketbook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in marketbook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            //Profit = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            //Loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                            Profit = runner.ProfitandLoss;
                            Loss = runner.Loss;
                            if (Profit < Loss)
                            {
                                profitandloss += Loss;
                            }
                            else
                            {
                                profitandloss += Profit;
                            }


                            // return LastProfitandLoss;


                        }
                        if (profitandloss >= 0)
                        {
                            profitandloss = profitandloss * -1;
                        }

                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in marketbook.Runners)
                        {

                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }

                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }


                }

                return OverAllLiabality;

            }
            catch (System.Exception ex)
            {
                return 0;
            }

        }
        public List<LiabalitybyMarket> GetLiabalityofSamiadminbyMarkets(List<UserBetsforSamiadmin> lstUserBet)
        {
            try
            {
                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;
                //  List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsForAdmin());
                List<UserBetsforSamiadmin> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
                var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();
                var lstUsers = lstUserBets.Select(item => new { item.UserID }).Distinct().ToArray();
                List<ExternalAPI.TO.MarketBook> lstmarketbooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                    objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketbook.Runners.Add(objrunner);
                    }

                    if (objMarketbook != null)
                    {
                        objMarketbook.MarketId = item;
                        List<UserBetsforSamiadmin> marketbooknames = new List<Models.UserBetsforSamiadmin>();
                        if (lstUserBets.Count > 0)
                        {
                            marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                        }
                        var marketbookname = "";
                        if (marketbooknames.Count > 0)
                        {
                            marketbookname = marketbooknames[0].MarketBookname;
                            objMarketbook.MarketBookName = marketbookname;
                        }
                        if (marketbookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforSamiadmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossSamiadmin(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {


                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        // decimal adminrate = 100 - Convert.ToDecimal(agentrate);
                                        // decimal profit = (adminrate / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (adminrate / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);

                                    }
                                }

                            }
                        }
                        else
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforSamiadmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossSamiadmin(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (objMarketbook.Runners.Count == 1)
                                        {
                                            if (profitorloss > 0)
                                            {
                                                profitorloss = profitorloss * -1;
                                            }
                                        }
                                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (adminrate / 100) * profitorloss;
                                        //decimal profit = ((100 - agentrate) / 100) * profitorloss;
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                    }
                                }

                            }
                        }

                    }
                    lstmarketbooks.Add(objMarketbook);

                }
                List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
                foreach (var marketbook in lstmarketbooks)
                {
                    LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                    if (marketbook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in marketbook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            Profit = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            Loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                            if (Profit < Loss)
                            {
                                profitandloss += Loss;
                            }
                            else
                            {
                                profitandloss += Profit;
                            }


                            // return LastProfitandLoss;


                        }
                        if (profitandloss >= 0)
                        {
                            profitandloss = profitandloss * -1;
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = profitandloss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in marketbook.Runners)
                        {

                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }
                        objMarketLiabality.MarketBookID = marketbook.MarketId;
                        objMarketLiabality.MarketBookName = marketbook.MarketBookName;
                        objMarketLiabality.Liabality = LastProfitandLoss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }


                }

                return lstLiabalitybyMarket;

            }
            catch (System.Exception ex)
            {
                return new List<LiabalitybyMarket>();
            }

        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLoss(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBets> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();


            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);

            if (marketbookstatus.Runners.Count() == 1)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.Credit = 0;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                    else
                    {
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                }
                return lstDebitCredit;
            }

            if (marketbookstatus.Runners[0].Handicap < 0)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
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
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }


                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }
            else
            {
                if (lstUserbetsbyMarketID.Count() > 0)
                {
                    if (lstUserbetsbyMarketID.FirstOrDefault().MarketBookname.Contains("To Be Placed"))
                    {
                        foreach (var userbet in lstUserbetsbyMarketID)
                        {

                            var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {

                                objDebitCredit.SelectionID = userbet.SelectionID;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = -1 * Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);


                            }
                            else
                            {
                                objDebitCredit.SelectionID = userbet.SelectionID;
                                objDebitCredit.Debit = -1 * totamount;
                                objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);
                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                        return lstDebitCredit;

                    }
                }
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {

                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
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
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }


        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossfancy(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBets> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();

            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId).ToList();
            if (lstUserbetsbyMarketID != null)
            {


                foreach (var userbet in lstUserbetsbyMarketID)
                {
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.Credit = 0;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                    else
                    {
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                }
                return lstDebitCredit;
            }
            else
            {
                return lstDebitCredit;
            }

        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossfancy2(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBets> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();

            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookname == marketbookstatus.MarketId).ToList();
            if (lstUserbetsbyMarketID != null)
            {


                foreach (var userbet in lstUserbetsbyMarketID)
                {
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.Credit = 0;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                    else
                    {
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                }
                return lstDebitCredit;
            }
            else
            {
                return lstDebitCredit;
            }

        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossFig(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBets> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();

            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);

            foreach (var userbet in lstUserbetsbyMarketID)
            {

                var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.BetSize) / 100);
                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                if (userbet.BetType == "back")
                {

                    objDebitCredit.SelectionID = userbet.SelectionID;
                    objDebitCredit.Debit = totamount;
                    objDebitCredit.Credit = 0;
                    lstDebitCredit.Add(objDebitCredit);
                    foreach (var runneritem in marketbookstatus.Runners)
                    {
                        if (runneritem.SelectionId != userbet.SelectionID)
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
                    objDebitCredit.SelectionID = userbet.SelectionID;
                    objDebitCredit.Debit = 0;
                    objDebitCredit.Credit = totamount;
                    lstDebitCredit.Add(objDebitCredit);
                    foreach (var runneritem in marketbookstatus.Runners)
                    {
                        if (runneritem.SelectionId != userbet.SelectionID)
                        {
                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            objDebitCredit.SelectionID = runneritem.SelectionId;
                            objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                            objDebitCredit.Credit = 0;
                            lstDebitCredit.Add(objDebitCredit);
                        }
                    }

                }

                //userbet.lstDebitCredit = new List<DebitCredit>();
                //userbet.lstDebitCredit = lstDebitCredit;

            }
            return lstDebitCredit;
        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossAgentFig(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBetsforAgent> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);

            foreach (var userbet in lstUserbetsbyMarketID)
            {
                decimal agentrate = (Convert.ToDecimal(userbet.AgentRate) / 100);
                decimal totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.BetSize) / 100);
                totamount = (totamount * (Convert.ToDecimal(userbet.AgentRate) / 100));
                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                if (userbet.BetType == "back")
                {

                    objDebitCredit.SelectionID = userbet.SelectionID;
                    objDebitCredit.Debit = 0;
                    objDebitCredit.Credit = totamount;
                    lstDebitCredit.Add(objDebitCredit);
                    foreach (var runneritem in marketbookstatus.Runners)
                    {
                        if (runneritem.SelectionId != userbet.SelectionID)
                        {
                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            objDebitCredit.SelectionID = runneritem.SelectionId;
                            objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * agentrate;
                            objDebitCredit.Credit = 0;
                            lstDebitCredit.Add(objDebitCredit);
                        }
                    }

                }
                else
                {
                    objDebitCredit.SelectionID = userbet.SelectionID;
                    objDebitCredit.Debit = totamount;
                    objDebitCredit.Credit = 0;
                    lstDebitCredit.Add(objDebitCredit);
                    foreach (var runneritem in marketbookstatus.Runners)
                    {
                        if (runneritem.SelectionId != userbet.SelectionID)
                        {
                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            objDebitCredit.SelectionID = runneritem.SelectionId;
                            objDebitCredit.Debit = 0;
                            objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * agentrate;
                            lstDebitCredit.Add(objDebitCredit);
                        }
                    }

                }

                //userbet.lstDebitCredit = new List<DebitCredit>();
                //userbet.lstDebitCredit = lstDebitCredit;

            }
            return lstDebitCredit;
        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossSuperFig(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBetsforSuper> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);

            foreach (var userbet in lstUserbetsbyMarketID)
            {
                decimal agentrate = Convert.ToDecimal(userbet.AgentRate);
                decimal superrate = Convert.ToDecimal(userbet.SuperAgentRateB);
                bool TransferAdinAmount = userbet.TransferAdmin;
                var TransferAdminPercentage = userbet.TransferAdminPercentage;
                decimal superpercent = superrate - agentrate;


                var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.BetSize) / 100);
                totamount = totamount * (superpercent / 100);
                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                if (userbet.BetType == "back")
                {

                    objDebitCredit.SelectionID = userbet.SelectionID;
                    objDebitCredit.Debit = 0;
                    objDebitCredit.Credit = totamount;
                    lstDebitCredit.Add(objDebitCredit);
                    foreach (var runneritem in marketbookstatus.Runners)
                    {
                        if (runneritem.SelectionId != userbet.SelectionID)
                        {
                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            objDebitCredit.SelectionID = runneritem.SelectionId;
                            objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
                            objDebitCredit.Credit = 0;
                            lstDebitCredit.Add(objDebitCredit);
                        }
                    }

                }
                else
                {
                    objDebitCredit.SelectionID = userbet.SelectionID;
                    objDebitCredit.Debit = totamount;
                    objDebitCredit.Credit = 0;
                    lstDebitCredit.Add(objDebitCredit);
                    foreach (var runneritem in marketbookstatus.Runners)
                    {
                        if (runneritem.SelectionId != userbet.SelectionID)
                        {
                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            objDebitCredit.SelectionID = runneritem.SelectionId;
                            objDebitCredit.Debit = 0;
                            objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
                            lstDebitCredit.Add(objDebitCredit);
                        }
                    }
                }

                //userbet.lstDebitCredit = new List<DebitCredit>();
                //userbet.lstDebitCredit = lstDebitCredit;

            }
            return lstDebitCredit;
        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossSamiadminFig(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBetsforSamiadmin> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);

            foreach (var userbet in lstUserbetsbyMarketID)
            {
                decimal agentrate = Convert.ToDecimal(userbet.AgentRate);
                decimal superrate = Convert.ToDecimal(userbet.SuperAgentRateB);
                decimal samiadminrate = Convert.ToDecimal(userbet.SamiAdminRate);
                bool TransferAdinAmount = userbet.TransferAdmin;
                var TransferAdminPercentage = userbet.TransferAdminPercentage;
                decimal superpercent = superrate - agentrate;
                decimal samiadminpercent = samiadminrate - (superpercent + agentrate);


                var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.BetSize) / 100);
                totamount = totamount * (samiadminpercent / 100);
                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                if (userbet.BetType == "back")
                {

                    objDebitCredit.SelectionID = userbet.SelectionID;
                    objDebitCredit.Debit = 0;
                    objDebitCredit.Credit = totamount;
                    lstDebitCredit.Add(objDebitCredit);
                    foreach (var runneritem in marketbookstatus.Runners)
                    {
                        if (runneritem.SelectionId != userbet.SelectionID)
                        {
                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            objDebitCredit.SelectionID = runneritem.SelectionId;
                            objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
                            objDebitCredit.Credit = 0;
                            lstDebitCredit.Add(objDebitCredit);
                        }
                    }

                }
                else
                {
                    objDebitCredit.SelectionID = userbet.SelectionID;
                    objDebitCredit.Debit = totamount;
                    objDebitCredit.Credit = 0;
                    lstDebitCredit.Add(objDebitCredit);
                    foreach (var runneritem in marketbookstatus.Runners)
                    {
                        if (runneritem.SelectionId != userbet.SelectionID)
                        {
                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            objDebitCredit.SelectionID = runneritem.SelectionId;
                            objDebitCredit.Debit = 0;
                            objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
                            lstDebitCredit.Add(objDebitCredit);
                        }
                    }
                }

                //userbet.lstDebitCredit = new List<DebitCredit>();
                //userbet.lstDebitCredit = lstDebitCredit;

            }
            return lstDebitCredit;
        }


        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossAgent(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBetsforAgent> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);
            if (marketbookstatus.Runners.Count() == 1)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.Credit = 0;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                    else
                    {
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                }
                return lstDebitCredit;
            }
            if (marketbookstatus.Runners[0].Handicap < 0)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
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
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }


                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }
            else
            {
                if (lstUserbetsbyMarketID.Count() > 0)
                {
                    if (lstUserbetsbyMarketID.FirstOrDefault().MarketBookname.Contains("To Be Placed"))
                    {
                        foreach (var userbet in lstUserbetsbyMarketID)
                        {

                            var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {

                                objDebitCredit.SelectionID = userbet.SelectionID;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = -1 * Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);


                            }
                            else
                            {
                                objDebitCredit.SelectionID = userbet.SelectionID;
                                objDebitCredit.Debit = -1 * totamount;
                                objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);
                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                        return lstDebitCredit;

                    }
                }
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {

                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
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
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }


        }
        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossAdmin(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBetsForAdmin> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);
            if (marketbookstatus.Runners.Count() == 1)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.Credit = 0;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                    else
                    {
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                }
                return lstDebitCredit;
            }
            if (marketbookstatus.Runners[0].Handicap < 0)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
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
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }


                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }
            else
            {
                if (lstUserbetsbyMarketID.Count() > 0)
                {
                    if (lstUserbetsbyMarketID.FirstOrDefault().MarketBookname.Contains("To Be Placed"))
                    {
                        foreach (var userbet in lstUserbetsbyMarketID)
                        {

                            var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {

                                objDebitCredit.SelectionID = userbet.SelectionID;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = -1 * Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);


                            }
                            else
                            {
                                objDebitCredit.SelectionID = userbet.SelectionID;
                                objDebitCredit.Debit = -1 * totamount;
                                objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);
                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                        return lstDebitCredit;

                    }
                }
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {

                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
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
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }


        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossSuper(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBetsforSuper> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);
            if (marketbookstatus.Runners.Count() == 1)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.Credit = 0;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                    else
                    {
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                }
                return lstDebitCredit;
            }
            if (marketbookstatus.Runners[0].Handicap < 0)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
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
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }


                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }
            else
            {
                if (lstUserbetsbyMarketID.Count() > 0)
                {
                    if (lstUserbetsbyMarketID.FirstOrDefault().MarketBookname.Contains("To Be Placed"))
                    {
                        foreach (var userbet in lstUserbetsbyMarketID)
                        {

                            var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {

                                objDebitCredit.SelectionID = userbet.SelectionID;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = -1 * Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);


                            }
                            else
                            {
                                objDebitCredit.SelectionID = userbet.SelectionID;
                                objDebitCredit.Debit = -1 * totamount;
                                objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);
                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                        return lstDebitCredit;

                    }
                }
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {

                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
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
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }


        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossSamiadmin(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBetsforSamiadmin> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);
            if (marketbookstatus.Runners.Count() == 1)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.Credit = 0;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                    else
                    {
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        lstDebitCredit.Add(objDebitCredit);
                    }
                }
                return lstDebitCredit;
            }
            if (marketbookstatus.Runners[0].Handicap < 0)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
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
                        double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }


                    }


                }
                return lstDebitCredit;
            }
            else
            {
                if (lstUserbetsbyMarketID.Count() > 0)
                {
                    if (lstUserbetsbyMarketID.FirstOrDefault().MarketBookname.Contains("To Be Placed"))
                    {
                        foreach (var userbet in lstUserbetsbyMarketID)
                        {

                            var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {

                                objDebitCredit.SelectionID = userbet.SelectionID;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = -1 * Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);


                            }
                            else
                            {
                                objDebitCredit.SelectionID = userbet.SelectionID;
                                objDebitCredit.Debit = -1 * totamount;
                                objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                lstDebitCredit.Add(objDebitCredit);
                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                        return lstDebitCredit;

                    }
                }
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {

                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
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
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in marketbookstatus.Runners)
                        {
                            if (runneritem.SelectionId != userbet.SelectionID)
                            {
                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                            }
                        }

                    }

                    //userbet.lstDebitCredit = new List<DebitCredit>();
                    //userbet.lstDebitCredit = lstDebitCredit;

                }
                return lstDebitCredit;
            }


        }


        public ExternalAPI.TO.MarketBook GetBookPosition(string marketBookID, List<UserBetsForAdmin> CurrentAdminBets, List<UserBetsforSuper> CurrentSuperBets, List<UserBetsforAgent> CurrentAgentBets, List<UserBets> CurrentUserBets)
        {

            ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                List<Models.UserBetsForAdmin> lstCurrentBetsAdmin = CurrentAdminBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbook.MarketId = marketBookID;
                    objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                    ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbook.Runners.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbook.Runners != null)
                        {
                            ExternalAPI.TO.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                objmarketbook.Runners.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                            objmarketbook.Runners.Add(objRunner);
                        }



                    }
                    ExternalAPI.TO.Runner objRunnerlast = new ExternalAPI.TO.Runner();
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objmarketbook.Runners.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        decimal superpercent = 0;
                        if (superrate > 0)
                        {
                            superpercent = superrate - agentrate;
                        }
                        else
                        {
                            superpercent = 0;
                        }
                        agentrate = agentrate + superpercent;
                        foreach (var userbet in lstCurrentBetsbyUser)
                        {

                            // var totamount = (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100));
                            var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((0) / 100));
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbook.Runners)
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
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }

                            }
                            else
                            {
                                double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbook.Runners)
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
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }


                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                    }

                    objmarketbook.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbook.Runners)
                    {


                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;


                    }

                }
            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 8)
                {

                    List<Models.UserBetsforSuper> lstCurrentBetsAdmin = CurrentSuperBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                    if (lstCurrentBetsAdmin.Count > 0)
                    {
                        lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                        objmarketbook.MarketId = marketBookID;
                        objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                        ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                        objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                        objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                        objmarketbook.Runners.Add(objRunner1);
                        foreach (var userbet in lstCurrentBetsAdmin)
                        {
                            if (objmarketbook.Runners != null)
                            {
                                ExternalAPI.TO.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                if (objexistingrunner == null)
                                {
                                    ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                    objmarketbook.Runners.Add(objRunner);
                                }
                            }
                            else
                            {
                                ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                                objmarketbook.Runners.Add(objRunner);
                            }



                        }
                        ExternalAPI.TO.Runner objRunnerlast = new ExternalAPI.TO.Runner();
                        objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                        objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                        objmarketbook.Runners.Add(objRunnerlast);

                        ///calculation
                        var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                        foreach (var userid in lstUsers)
                        {
                            var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                            decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                            decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                            bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                            decimal superpercent = superrate - agentrate;
                            foreach (var userbet in lstCurrentBetsbyUser)
                            {

                                // var totamount = (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100));
                                var totamount = (superpercent / 100) * (Convert.ToDecimal(userbet.Amount));
                                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                if (userbet.BetType == "back")
                                {
                                    double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = totamount;
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbook.Runners)
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
                                    foreach (var runneritem in objmarketbook.Runners)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = 0;
                                            objDebitCredit.Credit = totamount;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }

                                }
                                else
                                {
                                    double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objmarketbook.Runners)
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
                                    foreach (var runneritem in objmarketbook.Runners)
                                    {
                                        if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                        {
                                            objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                            objDebitCredit.SelectionID = runneritem.SelectionId;
                                            objDebitCredit.Debit = totamount;
                                            objDebitCredit.Credit = 0;
                                            lstDebitCredit.Add(objDebitCredit);
                                        }
                                    }


                                }

                                //userbet.lstDebitCredit = new List<DebitCredit>();
                                //userbet.lstDebitCredit = lstDebitCredit;

                            }
                        }

                        objmarketbook.DebitCredit = lstDebitCredit;
                        foreach (var runneritem in objmarketbook.Runners)
                        {
                            runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                            runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                        }

                    }
                }

                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 2)
                    {
                        List<Models.UserBetsforAgent> lstCurrentBetsAdmin = CurrentAgentBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                        if (lstCurrentBetsAdmin.Count > 0)
                        {
                            lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                            objmarketbook.MarketId = marketBookID;
                            objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                            ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                            objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                            objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                            objmarketbook.Runners.Add(objRunner1);
                            foreach (var userbet in lstCurrentBetsAdmin)
                            {
                                if (objmarketbook.Runners != null)
                                {
                                    ExternalAPI.TO.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                    if (objexistingrunner == null)
                                    {
                                        ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                        objRunner.SelectionId = userbet.UserOdd;
                                        objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                        objmarketbook.Runners.Add(objRunner);
                                    }
                                }
                                else
                                {
                                    ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                    objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                                    objmarketbook.Runners.Add(objRunner);
                                }



                            }
                            ExternalAPI.TO.Runner objRunnerlast = new ExternalAPI.TO.Runner();
                            objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                            objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                            objmarketbook.Runners.Add(objRunnerlast);
                            ///calculation
                            var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                            foreach (var userid in lstUsers)
                            {
                                var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();

                                foreach (var userbet in lstCurrentBetsbyUser)
                                {
                                    decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate));
                                    // var totamount = (Convert.ToDecimal(userbet.Amount) * ((agentrate) / 100));

                                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    if (userbet.BetType == "back")
                                    {
                                        double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                        objDebitCredit.SelectionID = userbet.UserOdd;
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                        foreach (var runneritem in objmarketbook.Runners)
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
                                        foreach (var runneritem in objmarketbook.Runners)
                                        {
                                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                            {
                                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                                objDebitCredit.Debit = 0;
                                                objDebitCredit.Credit = totamount;
                                                lstDebitCredit.Add(objDebitCredit);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                        objDebitCredit.SelectionID = userbet.UserOdd;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                        foreach (var runneritem in objmarketbook.Runners)
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
                                        foreach (var runneritem in objmarketbook.Runners)
                                        {
                                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                            {
                                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                                objDebitCredit.Debit = totamount;
                                                objDebitCredit.Credit = 0;
                                                lstDebitCredit.Add(objDebitCredit);
                                            }
                                        }


                                    }

                                    //userbet.lstDebitCredit = new List<DebitCredit>();
                                    //userbet.lstDebitCredit = lstDebitCredit;

                                }
                            }

                            objmarketbook.DebitCredit = lstDebitCredit;
                            foreach (var runneritem in objmarketbook.Runners)
                            {

                                runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                                runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;


                            }

                        }
                    }
                    else
                    {
                        if (LoggedinUserDetail.GetUserTypeID() == 3)
                        {

                            List<Models.UserBets> lstCurrentBets = CurrentUserBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                            if (lstCurrentBets.Count > 0)
                            {
                                lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                                objmarketbook.MarketId = marketBookID;
                                objmarketbook.MarketBookName = lstCurrentBets[0].MarketBookname;
                                objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                                for (int i = 1; i <= 500; i++)
                                {
                                    ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                                    objRunner1.SelectionId = (i).ToString();
                                    objRunner1.Handicap = -1 * i;
                                    objmarketbook.Runners.Add(objRunner1);
                                }
                                //ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                                //objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBets[0].UserOdd) - 1).ToString();
                                //objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
                                //objmarketbook.Runners.Add(objRunner1);
                                //foreach (var userbet in lstCurrentBets)
                                //{
                                //    if (objmarketbook.Runners != null)
                                //    {
                                //        ExternalAPI.TO.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                //        if (objexistingrunner == null)
                                //        {
                                //            ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                //            objRunner.SelectionId = userbet.UserOdd;
                                //            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                //            objmarketbook.Runners.Add(objRunner);
                                //        }
                                //    }
                                //    else
                                //    {
                                //        ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                //        objRunner.SelectionId = userbet.UserOdd;
                                //        objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                //        objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                                //        objmarketbook.Runners.Add(objRunner);
                                //    }



                                //}
                                //ExternalAPI.TO.Runner objRunnerlast = new ExternalAPI.TO.Runner();
                                //objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBets.Last().UserOdd) + 1).ToString();
                                //objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                                //objmarketbook.Runners.Add(objRunnerlast);
                                ///calculation
                                foreach (var userbet in lstCurrentBets)
                                {

                                    var totamount = (Convert.ToDecimal(userbet.Amount));
                                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    if (userbet.BetType == "back")
                                    {
                                        double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                        objDebitCredit.SelectionID = userbet.UserOdd;
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                        foreach (var runneritem in objmarketbook.Runners)
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
                                        foreach (var runneritem in objmarketbook.Runners)
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
                                        double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                        objDebitCredit.SelectionID = userbet.UserOdd;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                        foreach (var runneritem in objmarketbook.Runners)
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
                                        foreach (var runneritem in objmarketbook.Runners)
                                        {
                                            if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                            {
                                                objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                                objDebitCredit.SelectionID = runneritem.SelectionId;
                                                objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                                objDebitCredit.Credit = 0;
                                                lstDebitCredit.Add(objDebitCredit);
                                            }
                                        }


                                    }

                                    //userbet.lstDebitCredit = new List<DebitCredit>();
                                    //userbet.lstDebitCredit = lstDebitCredit;

                                }
                                objmarketbook.DebitCredit = lstDebitCredit;
                                foreach (var runneritem in objmarketbook.Runners)
                                {

                                    runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                                }


                            }
                        }
                    }

                }
            }
            return objmarketbook;
        }

        public ExternalAPI.TO.MarketBookForindianFancy GetBookPositionIN(string marketBookID, string selectionID, List<UserBetsForAdmin> CurrentAdminBets, List<UserBetsforSuper> CurrentSuperBets, List<UserBetsforSamiadmin> CurrentSamiadminBets, List<UserBetsforAgent> CurrentAgentBets, List<UserBets> CurrentUserBets)
        {

            ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
            ExternalAPI.TO.MarketBookForindianFancy objmarketbookin = new ExternalAPI.TO.MarketBookForindianFancy();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                List<Models.UserBetsForAdmin> lstCurrentBetsAdmin = CurrentAdminBets.Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbook.MarketId = marketBookID;
                    objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                    ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbook.Runners.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbook.Runners != null)
                        {
                            ExternalAPI.TO.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                objmarketbook.Runners.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                            objmarketbook.Runners.Add(objRunner);
                        }



                    }
                    ExternalAPI.TO.Runner objRunnerlast = new ExternalAPI.TO.Runner();
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objmarketbook.Runners.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = 0;
                        if (superrate > 0)
                        {
                            superpercent = superrate - agentrate;
                        }
                        else
                        {
                            superpercent = 0;
                        }
                        agentrate = agentrate + superpercent;
                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount * Convert.ToDecimal(num);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount * Convert.ToDecimal(num);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }

                            }
                            else
                            {
                                double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbook.Runners)
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
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount * Convert.ToDecimal(num);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }


                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                    }

                    objmarketbook.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbook.Runners)
                    {


                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;


                    }




                }
            }

            if (LoggedinUserDetail.GetUserTypeID() == 2)
            {
                int a, b;
                List<Models.UserBetsforAgent> lstCurrentBetsAdmin = CurrentAgentBets.Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();


                    double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
                    a = Convert.ToInt32(aa);
                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.StallDraw = (lstCurrentBetsAdmin[0].SelectionID).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbookin.RunnersForindianFancy != null)
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                objRunner.StallDraw = userbet.SelectionID;
                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objRunner.StallDraw = userbet.SelectionID;
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }

                    }

                    ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                    double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objRunnerlast.StallDraw = lstCurrentBetsAdmin.Last().SelectionID;
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                    ///calculation
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate));
                        double num = Convert.ToDouble(userbet.BetSize) / 100;
                        //var totamount = (Convert.ToDecimal(userbet.Amount));

                        var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                        if (userbet.BetType == "back")
                        {
                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.UserOdd;
                            objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                            objDebitCredit.Credit = 0;

                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);

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
                                    objDebitCredit.Credit = totamount;

                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }

                        }
                        else
                        {
                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.UserOdd;
                            objDebitCredit.Debit = 0;
                            objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = totamount;
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }


                        }

                        //userbet.lstDebitCredit = new List<DebitCredit>();
                        //userbet.lstDebitCredit = lstDebitCredit;

                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {

                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                    }
                }
            }

            if (LoggedinUserDetail.GetUserTypeID() == 3)
            {
                int a, b;

                List<Models.UserBets> lstCurrentBets = CurrentUserBets.Where(item => item.MarketBookID == marketBookID && item.SelectionID == selectionID).ToList();
                if (lstCurrentBets.Count > 0)
                {
                    lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
                    double aa = Convert.ToDouble(lstCurrentBets[0].UserOdd);

                    a = Convert.ToInt32(aa);
                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
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
                                objRunner.StallDraw = userbet.SelectionID;
                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objRunner.StallDraw = userbet.SelectionID;
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }

                    }
                    ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                    double bb = Convert.ToDouble(lstCurrentBets.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                    objRunnerlast.StallDraw = lstCurrentBets.Last().SelectionID;
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

                            objDebitCredit.Debit = totamount;
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

            if (LoggedinUserDetail.GetUserTypeID() == 8)
            {
                List<Models.UserBetsforSuper> lstCurrentBetsSuper = CurrentSuperBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();

                if (lstCurrentBetsSuper.Count > 0)
                {
                    lstCurrentBetsSuper = lstCurrentBetsSuper.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsSuper)
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
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper.Last().UserOdd) + 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsSuper.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsSuper.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = superrate - agentrate;

                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            //decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                            var totamount1 = (Convert.ToDecimal(userbet.Amount) * (Convert.ToDecimal(num)));
                            var totamount = (superpercent / 100) * (totamount1); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
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
                                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
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
                                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                    }

                }
            }
            if (LoggedinUserDetail.GetUserTypeID() == 9)
            {
                List<Models.UserBetsforSamiadmin> lstCurrentBetsSuper = CurrentSamiadminBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();

                if (lstCurrentBetsSuper.Count > 0)
                {
                    lstCurrentBetsSuper = lstCurrentBetsSuper.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsSuper)
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
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper.Last().UserOdd) + 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsSuper.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsSuper.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        decimal samiadminrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SamiAdminRate);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = superrate - agentrate;
                        decimal samiadminpercent = samiadminrate - (superpercent + agentrate);

                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            //decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                            var totamount1 = (Convert.ToDecimal(userbet.Amount) * (Convert.ToDecimal(num)));
                            var totamount = (samiadminpercent / 100) * (totamount1); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
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
                                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
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
                                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                    }

                }
            }
            return objmarketbookin;
        }
        public ExternalAPI.TO.MarketBookForindianFancy GetBookPositionINNew(string selectionID, List<UserBetsForAdmin> CurrentAdminBets, List<UserBetsforSuper> CurrentSuperBets, List<UserBetsforSamiadmin> CurrentSamiadminBets, List<UserBetsforAgent> CurrentAgentBets, List<UserBets> CurrentUserBets)
        {

            ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
            ExternalAPI.TO.MarketBookForindianFancy objmarketbookin = new ExternalAPI.TO.MarketBookForindianFancy();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                List<Models.UserBetsForAdmin> lstCurrentBetsAdmin = CurrentAdminBets.Where(item => item.SelectionID == selectionID).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbook.MarketId = selectionID;
                    objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                    ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbook.Runners.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbook.Runners != null)
                        {
                            ExternalAPI.TO.Runner objexistingrunner = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                objmarketbook.Runners.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                            objmarketbook.Runners.Add(objRunner);
                        }



                    }
                    ExternalAPI.TO.Runner objRunnerlast = new ExternalAPI.TO.Runner();
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objmarketbook.Runners.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = 0;
                        if (superrate > 0)
                        {
                            superpercent = superrate - agentrate;
                        }
                        else
                        {
                            superpercent = 0;
                        }
                        agentrate = agentrate + superpercent;
                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount * Convert.ToDecimal(num);
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount * Convert.ToDecimal(num);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = 0;
                                        objDebitCredit.Credit = totamount;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }

                            }
                            else
                            {
                                double handicap = objmarketbook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objmarketbook.Runners)
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
                                foreach (var runneritem in objmarketbook.Runners)
                                {
                                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                    {
                                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                        objDebitCredit.SelectionID = runneritem.SelectionId;
                                        objDebitCredit.Debit = totamount * Convert.ToDecimal(num);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }


                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                    }

                    objmarketbook.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbook.Runners)
                    {


                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;


                    }




                }
            }

            if (LoggedinUserDetail.GetUserTypeID() == 2)
            {
                int a, b;
                List<Models.UserBetsforAgent> lstCurrentBetsAdmin = CurrentAgentBets.Where(item => item.SelectionID == selectionID && item.isMatched == true).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();


                    double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
                    a = Convert.ToInt32(aa);
                    objmarketbookin.MarketId = selectionID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.StallDraw = (lstCurrentBetsAdmin[0].SelectionID).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbookin.RunnersForindianFancy != null)
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                objRunner.StallDraw = userbet.SelectionID;
                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objRunner.StallDraw = userbet.SelectionID;
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }

                    }

                    ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                    double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objRunnerlast.StallDraw = lstCurrentBetsAdmin.Last().SelectionID;
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                    ///calculation
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate));
                        double num = Convert.ToDouble(userbet.BetSize) / 100;
                        //var totamount = (Convert.ToDecimal(userbet.Amount));

                        var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                        if (userbet.BetType == "back")
                        {
                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.UserOdd;
                            objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                            objDebitCredit.Credit = 0;

                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);

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
                                    objDebitCredit.Credit = totamount;

                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }

                        }
                        else
                        {
                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.UserOdd;
                            objDebitCredit.Debit = 0;
                            objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    objDebitCredit.Debit = totamount;
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }


                        }

                        //userbet.lstDebitCredit = new List<DebitCredit>();
                        //userbet.lstDebitCredit = lstDebitCredit;

                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {

                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                    }
                }
            }

            if (LoggedinUserDetail.GetUserTypeID() == 3)
            {
                int a, b;

                List<Models.UserBets> lstCurrentBets = CurrentUserBets.Where(item => item.SelectionID == selectionID && item.isMatched == true).ToList();
                if (lstCurrentBets.Count > 0)
                {
                    lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
                    double aa = Convert.ToDouble(lstCurrentBets[0].UserOdd);

                    a = Convert.ToInt32(aa);
                    objmarketbookin.MarketId = selectionID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
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
                                objRunner.StallDraw = userbet.SelectionID;
                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objRunner.StallDraw = userbet.SelectionID;
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }

                    }
                    ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                    double bb = Convert.ToDouble(lstCurrentBets.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                    objRunnerlast.StallDraw = lstCurrentBets.Last().SelectionID;
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                    ///calculation
                    foreach (var userbet in lstCurrentBets)
                    {
                        double num = Convert.ToDouble(userbet.BetSize) / 100;
                        var totamount = (Convert.ToDecimal(userbet.Amount));
                        var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                        if (userbet.BetType == "back")
                        {
                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.UserOdd;
                            objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                            objDebitCredit.Credit = 0;

                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;

                                    objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    // objDebitCredit.Debit = totamount;
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
                                    //objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }

                        }
                        else
                        {
                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.UserOdd;
                            objDebitCredit.Debit = 0;
                            objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num); ;
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;

                                    objDebitCredit.Debit = 0;
                                    // objDebitCredit.Credit = totamount;
                                    objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;

                                    objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                    // objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }

                        }

                        //userbet.lstDebitCredit = new List<DebitCredit>();
                        //userbet.lstDebitCredit = lstDebitCredit;

                    }
                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {

                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                    }

                }
            }

            if (LoggedinUserDetail.GetUserTypeID() == 8)
            {
                List<Models.UserBetsforSuper> lstCurrentBetsSuper = CurrentSuperBets.Where(item => item.SelectionID == selectionID && item.isMatched == true).ToList();

                if (lstCurrentBetsSuper.Count > 0)
                {
                    lstCurrentBetsSuper = lstCurrentBetsSuper.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbookin.MarketId = selectionID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsSuper)
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
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper.Last().UserOdd) + 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsSuper.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsSuper.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = superrate - agentrate;

                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            //decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                            var totamount1 = (Convert.ToDecimal(userbet.Amount) * (Convert.ToDecimal(num)));
                            var totamount = (superpercent / 100) * (totamount1); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
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
                                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
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
                                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                    }

                }
            }
            if (LoggedinUserDetail.GetUserTypeID() == 9)
            {
                List<Models.UserBetsforSamiadmin> lstCurrentBetsSuper = CurrentSamiadminBets.Where(item => item.SelectionID == selectionID && item.isMatched == true).ToList();

                if (lstCurrentBetsSuper.Count > 0)
                {
                    lstCurrentBetsSuper = lstCurrentBetsSuper.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objmarketbookin.MarketId = selectionID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsSuper)
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
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsSuper.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsSuper.Last().UserOdd) + 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsSuper.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsSuper.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        decimal samiadminrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SamiAdminRate);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = superrate - agentrate;
                        decimal samiadminpercent = samiadminrate - (superpercent + agentrate);

                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            //decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
                            double num = Convert.ToDouble(userbet.BetSize) / 100;
                            var totamount1 = (Convert.ToDecimal(userbet.Amount) * (Convert.ToDecimal(num)));
                            var totamount = (samiadminpercent / 100) * (totamount1); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
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
                                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
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
                                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (samiadminpercent / 100);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                    }

                }
            }
            return objmarketbookin;
        }


        public ExternalAPI.TO.MarketBookForindianFancy GetBookPositioninKJ(string marketBookID, List<UserBetsForAdmin> CurrentAdminBets, List<UserBetsforSuper> CurrentSuperBets, List<UserBetsforSamiadmin> CurrentSamiadminBets, List<UserBetsforAgent> CurrentAgentBets, List<UserBets> CurrentUserBets)
        {

            ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
            ExternalAPI.TO.MarketBookForindianFancy objmarketbookin = new ExternalAPI.TO.MarketBookForindianFancy();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                int a, b;
                List<Models.UserBetsForAdmin> lstCurrentBetsAdmin = CurrentAdminBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
                    a = Convert.ToInt32(aa);
                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
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
                    objRunnerlast.SelectionId = (a + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = 0;
                        if (superrate > 0)
                        {
                            superpercent = superrate - agentrate;
                        }
                        else
                        {
                            superpercent = 0;
                        }
                        agentrate = agentrate + superpercent;
                        foreach (var userbet in lstCurrentBetsbyUser)
                        {
                            var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));

                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
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
                                        objDebitCredit.Credit = totamount;
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
                                        objDebitCredit.Debit = totamount;
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }


                            }

                            //userbet.lstDebitCredit = new List<DebitCredit>();
                            //userbet.lstDebitCredit = lstDebitCredit;

                        }
                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                    }
                }
            }

            if (LoggedinUserDetail.GetUserTypeID() == 2)
            {
                int a, b;
                List<Models.UserBetsforAgent> lstCurrentBetsAdmin = CurrentAgentBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();


                    double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
                    a = Convert.ToInt32(aa);
                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    // objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objmarketbookin.RunnersForindianFancy != null)
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objexistingrunner = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                // objRunner.StallDraw = userbet.SelectionID;
                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            //objRunner.StallDraw = userbet.SelectionID;
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }

                    }

                    ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                    double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    // objRunnerlast.StallDraw = lstCurrentBetsAdmin.Last().SelectionID;
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                    ///calculation
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        //decimal totamount = GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
                        decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate));
                        // var totamount = (Convert.ToDecimal(userbet.Amount));
                        var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                        if (userbet.BetType == "back")
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
                                    double num = Convert.ToDouble(userbet.BetSize) / 100;
                                    objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    // objDebitCredit.Debit = totamount;
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
                                    double num = Convert.ToDouble(userbet.BetSize) / 100;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount;
                                    //objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }

                        }
                        else
                        {
                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.UserOdd;
                            objDebitCredit.Debit = totamount;
                            objDebitCredit.Credit = 0;
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    double num = Convert.ToDouble(userbet.BetSize) / 100;
                                    objDebitCredit.Debit = 0;
                                    // objDebitCredit.Credit = totamount;
                                    objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    double num = Convert.ToDouble(userbet.BetSize) / 100;
                                    objDebitCredit.Debit = totamount;
                                    // objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }


                        }

                        //userbet.lstDebitCredit = new List<DebitCredit>();
                        //userbet.lstDebitCredit = lstDebitCredit;

                    }

                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {

                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;

                    }
                }
            }

            if (LoggedinUserDetail.GetUserTypeID() == 3)
            {
                int a, b;

                List<Models.UserBets> lstCurrentBets = CurrentUserBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                if (lstCurrentBets.Count > 0)
                {
                    lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();
                    double aa = Convert.ToDouble(lstCurrentBets[0].UserOdd);

                    a = Convert.ToInt32(aa);
                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.StallDraw = (lstCurrentBets[0].SelectionID).ToString();
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
                                objRunner.StallDraw = userbet.SelectionID;
                                objmarketbookin.RunnersForindianFancy.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.RunnerForIndianFancy objRunner = new ExternalAPI.TO.RunnerForIndianFancy();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objRunner.StallDraw = userbet.SelectionID;
                            objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                            objmarketbookin.RunnersForindianFancy.Add(objRunner);
                        }

                    }
                    ExternalAPI.TO.RunnerForIndianFancy objRunnerlast = new ExternalAPI.TO.RunnerForIndianFancy();
                    double bb = Convert.ToDouble(lstCurrentBets.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                    objRunnerlast.StallDraw = lstCurrentBets.Last().SelectionID;
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                    ///calculation
                    foreach (var userbet in lstCurrentBets)
                    {

                        var totamount = (Convert.ToDecimal(userbet.Amount));
                        var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                        if (userbet.BetType == "back")
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
                                    double num = Convert.ToDouble(userbet.BetSize) / 100;
                                    objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    // objDebitCredit.Debit = totamount;
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
                                    double num = Convert.ToDouble(userbet.BetSize) / 100;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
                                    //objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }

                        }
                        else
                        {
                            double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.UserOdd;
                            objDebitCredit.Debit = totamount;
                            objDebitCredit.Credit = 0;
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    double num = Convert.ToDouble(userbet.BetSize) / 100;
                                    objDebitCredit.Debit = 0;
                                    // objDebitCredit.Credit = totamount;
                                    objDebitCredit.Credit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }
                            foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                            {
                                if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.UserOdd)
                                {
                                    objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                    objDebitCredit.SelectionID = runneritem.SelectionId;
                                    double num = Convert.ToDouble(userbet.BetSize) / 100;
                                    objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
                                    // objDebitCredit.Debit = Convert.ToDecimal(totamount) * Convert.ToDecimal(num);
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                }
                            }

                        }

                        //userbet.lstDebitCredit = new List<DebitCredit>();
                        //userbet.lstDebitCredit = lstDebitCredit;

                    }
                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {

                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                    }


                }
            }


            if (LoggedinUserDetail.GetUserTypeID() == 8)
            {
                int a, b;
                List<Models.UserBetsforSuper> lstCurrentBetsAdmin = CurrentSuperBets.ToList().Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
                    a = Convert.ToInt32(aa);
                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);

                    foreach (var userbet in lstCurrentBetsAdmin)
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
                    double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                    ///calculation
                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = superrate - agentrate;

                        foreach (var userbet in lstCurrentBetsAdmin)
                        {
                            var totamount = (superpercent / 100) * ((Convert.ToDecimal(userbet.Amount)) * (Convert.ToDecimal(userbet.BetSize) / 100)); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                                                                                                                                                       //double num = Convert.ToDouble(userbet.BetSize) / 100;
                                                                                                                                                       //var totamount1 = (Convert.ToDecimal(userbet.Amount)* Convert.ToDecimal(num));
                                                                                                                                                       //var totamount = totamount1 * (superpercent / 100);
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
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
                                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
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
                                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (superpercent / 100);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }

                        }
                    }
                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                    }
                }
            }
            if (LoggedinUserDetail.GetUserTypeID() == 9)
            {
                int a, b;
                List<Models.UserBetsforSamiadmin> lstCurrentBetsAdmin = CurrentSamiadminBets.ToList().Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    double aa = Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd);
                    a = Convert.ToInt32(aa);
                    objmarketbookin.MarketId = marketBookID;
                    objmarketbookin.RunnersForindianFancy = new List<ExternalAPI.TO.RunnerForIndianFancy>();
                    ExternalAPI.TO.RunnerForIndianFancy objRunner1 = new ExternalAPI.TO.RunnerForIndianFancy();
                    objRunner1.SelectionId = (a - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunner1);

                    foreach (var userbet in lstCurrentBetsAdmin)
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
                    double bb = Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd);
                    b = Convert.ToInt32(bb);
                    objRunnerlast.SelectionId = ((b) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objmarketbookin.RunnersForindianFancy.Add(objRunnerlast);
                    ///calculation
                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        decimal superrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SuperAgentRateB);
                        decimal semiadminrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].SamiAdminRate);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        var TransferAdminPercentage = lstCurrentBetsbyUser[0].TransferAdminPercentage;
                        decimal superpercent = superrate - agentrate;
                        decimal semiadminpercent = semiadminrate - (superpercent + agentrate);

                        foreach (var userbet in lstCurrentBetsAdmin)
                        {
                            var totamount = (semiadminpercent / 100) * ((Convert.ToDecimal(userbet.Amount)) * (Convert.ToDecimal(userbet.BetSize) / 100)); //TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate - TransferAdminPercentage) / 100));
                                                                                                                                                           //double num = Convert.ToDouble(userbet.BetSize) / 100;
                                                                                                                                                           //var totamount1 = (Convert.ToDecimal(userbet.Amount)* Convert.ToDecimal(num));
                                                                                                                                                           //var totamount = totamount1 * (superpercent / 100);
                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objmarketbookin.RunnersForindianFancy.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
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
                                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount) * (semiadminpercent / 100);
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
                                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount) * (semiadminpercent / 100);
                                        objDebitCredit.Credit = 0;
                                        lstDebitCredit.Add(objDebitCredit);
                                    }
                                }
                            }

                        }
                    }
                    objmarketbookin.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objmarketbookin.RunnersForindianFancy)
                    {
                        runneritem.ProfitandLoss = Convert.ToInt64(objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objmarketbookin.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                    }
                }
            }

            return objmarketbookin;
        }
    }
}