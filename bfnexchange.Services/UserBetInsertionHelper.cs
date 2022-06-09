using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace bfnexchange.Services
{
    public class UserBetInsertionHelper
    {
        public decimal clickedodd = 0;
        public double clickedbetsize = 0;
        public double newamount = 0;
        public int loadedlocation = -1;
        public int ParentID = 0;

        public bfnexchange.Services.Services.UserServices objUsersServiceCleint = new Services.UserServices();
        public string CalculateAmounts(string betslipamountlabel, decimal odd, decimal amount, string runnerscount, string BetType)
        {
            string betoddlabel = "";
            betslipamountlabel = amount.ToString("F2");
            if (runnerscount != "1")
            {
                betoddlabel = ((amount * odd) - amount).ToString("F2");
                if (BetType == "lay")
                {
                    string betslipamount = betslipamountlabel;
                    betslipamountlabel = betoddlabel;
                    betoddlabel = betslipamount;
                }
            }
            else
            {
                betoddlabel = amount.ToString("F2");
                if (BetType == "lay")
                {
                    string betslipamount = betslipamountlabel;
                    betslipamountlabel = betoddlabel;
                    betoddlabel = betslipamount;
                }
            }
            return betslipamountlabel;

        }
        private bool ValidatePassword(string Password)
        {
            if (Password == ConfigurationManager.AppSettings["PasswordForValidate"])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        ExternalAPI.TO.MarketBook currmarketbookforbet = new ExternalAPI.TO.MarketBook();
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
                APIConfig.WriteErrorToDB(ex.Message);
            }
        }
        public void insertbetslip(decimal userodd, string SelectionID, string Selectionname, string BetType, string nupdownAmount, string betslipamountlabel, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay, int Clickedlocation, int UserID, string Betslipsize, string Password, string marketbookId, string Marketbookname, bool GetData)
        {
            try
            {

                if (ValidatePassword(Password) == false)
                {
                    return;
                }
                if (Clickedlocation == 8 || Clickedlocation==9)
                {
                  
                    var data = InsertUserBetFancyIN(SelectionID, Selectionname, userodd.ToString(), nupdownAmount.ToString(), BetType, userodd.ToString(), false, "In-Complete", marketbookId, Marketbookname, betslipamountlabel, Convert.ToDecimal(Betslipsize), Convert.ToDecimal(0), Clickedlocation.ToString(), ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, UserID, currmarketbookforbet);
                    return;
                }
                if (GetData == true)
                {
                    BettingService objBettingClient = new BettingService();

                    List<string> lstIDs = new List<string>();

                    lstIDs.Add(marketbookId);
                    // GetMarketData obMarketdata = new GetMarketData();
                    string eventtypename = objUsersServiceCleint.GetEventTypeNamebyMarketID(marketbookId);

                    var marketbooks = objBettingClient.GetMarketDatabyIDLive(lstIDs.ToArray(), Marketbookname, DateTime.Now, eventtypename, Password, ConfigurationManager.AppSettings["PasswordForValidateS"]);

                    if (marketbooks.Count == 0)
                    {
                        return;
                    }
                    if (marketbooks[0].IsMarketDataDelayed == true)
                    {
                        return;
                    }
                    try
                    {
                        int UserPoundRate = Convert.ToInt32(objUsersServiceCleint.GetPoundRatebyUserID(UserID));
                        UpdatePricesbyUserPoundrate(marketbooks[0], UserPoundRate);
                    }
                    catch (Exception ex)
                    {

                    }

                    currmarketbookforbet = marketbooks[0];
                }

                //if (checkNetConnection() == false)
                //{
                //    return;
                //}

                double liveodd = 0;
                var ismatched = false;
                var marketbookID = currmarketbookforbet.MarketId;
                var location = 0;
                var marketbookname = currmarketbookforbet.MarketBookName;


                double betslipsize = 0;

                if (clickedodd == 0)
                {
                    clickedodd = userodd;

                }

              
                if (currmarketbookforbet.Runners.Count() == 1)
                {
                    var data = InsertUserBetFancy(SelectionID, Selectionname, userodd.ToString(), nupdownAmount.ToString(), BetType, liveodd.ToString(), false, "In-Complete", marketbookID, marketbookname, betslipamountlabel, Convert.ToDecimal(betslipsize), Convert.ToDecimal(0), location.ToString(), ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, UserID, currmarketbookforbet);
                    return;
                }
                var selectedrunner = currmarketbookforbet.Runners.Where(item => item.SelectionId == SelectionID).FirstOrDefault();
                if (BetType == "back")
                {
                    try
                    {
                        clickedbetsize = selectedrunner.ExchangePrices.AvailableToBack[Clickedlocation].Size;
                    }
                    catch (System.Exception ex)
                    {
                        clickedbetsize = 0;
                    }

                    if (loadedlocation == -1)
                    {
                        liveodd = selectedrunner.ExchangePrices.AvailableToBack[0].Price;
                        betslipsize = selectedrunner.ExchangePrices.AvailableToBack[0].Size;
                        location = 0;
                    }
                    else
                    {
                        location = loadedlocation;
                        liveodd = selectedrunner.ExchangePrices.AvailableToBack[location].Price;
                        betslipsize = selectedrunner.ExchangePrices.AvailableToBack[location].Size;
                    }

                    if ((liveodd) >= Convert.ToDouble(userodd))
                    {
                        userodd = Convert.ToDecimal(liveodd);

                        betslipamountlabel = CalculateAmounts(betslipamountlabel, userodd, Convert.ToDecimal(nupdownAmount), currmarketbookforbet.Runners.Count.ToString(), BetType);
                    }

                    if (Convert.ToDouble(userodd) == liveodd && clickedodd <= Convert.ToDecimal(liveodd))
                    {
                        ismatched = true;
                    };

                }
                else
                {

                    try
                    {
                        clickedbetsize = selectedrunner.ExchangePrices.AvailableToLay[Clickedlocation].Size;
                    }
                    catch (System.Exception ex)
                    {
                        clickedbetsize = 0;
                    }

                    if (loadedlocation == -1)
                    {
                        liveodd = selectedrunner.ExchangePrices.AvailableToLay[0].Price;
                        betslipsize = selectedrunner.ExchangePrices.AvailableToLay[0].Size;
                        location = 0;
                    }
                    else
                    {
                        location = loadedlocation;
                        liveodd = selectedrunner.ExchangePrices.AvailableToLay[location].Price;
                        betslipsize = selectedrunner.ExchangePrices.AvailableToLay[location].Size;
                    }

                    if (Convert.ToDecimal(liveodd) <= (userodd))
                    {
                        userodd = Convert.ToDecimal(liveodd);

                        betslipamountlabel = CalculateAmounts(betslipamountlabel, userodd, Convert.ToDecimal(nupdownAmount), currmarketbookforbet.Runners.Count.ToString(), BetType);
                    }
                    if (Convert.ToDouble(userodd) == liveodd && clickedodd >= Convert.ToDecimal(liveodd))
                    {
                        ismatched = true;
                    }

                }
                if (loadedlocation == -1)
                {
                    loadedlocation = 0;
                    location = 0;
                }
                Betslipsize = selectedrunner.ExchangePrices.AvailableToLay[location].Size.ToString();
                // $("#clickedbetsize").val(betslipsize);
                //if ($("#clickedbetsize").val() == "") {


                //}

                double pendingbetamount = 0;
                double Amount = Convert.ToDouble(nupdownAmount);

                if (ismatched == true)
                {
                    if (betslipsize < Amount)
                    {
                        Amount = betslipsize;
                        pendingbetamount = Convert.ToDouble(nupdownAmount) - Amount;
                    }
                    if (location < 2)
                    {

                        if (pendingbetamount > 0)
                        {
                            newamount = pendingbetamount;
                            var data = InsertUserBet(SelectionID, Selectionname, userodd.ToString(), Amount.ToString(), BetType, liveodd.ToString(), ismatched, "In-Complete", marketbookID, marketbookname, betslipamountlabel, Convert.ToDecimal(betslipsize), Convert.ToDecimal(0), location.ToString(), ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, UserID);
                            if (data.Contains("True"))
                            {
                                var ID = data.Split('|');
                                if (ParentID == 0)
                                {
                                    ParentID = Convert.ToInt32(ID[1]);
                                }



                            }
                            else
                            {
                                //  MessageBox.Show("Something went wrong. Please bet again or call the vendor.");
                            }

                            if (BetType == "back")
                            {
                                loadedlocation = (location + 1);
                                location = location + 1;

                                if (location > 2)
                                {
                                    data = InsertUserBet(SelectionID, Selectionname, userodd.ToString(), newamount.ToString(), BetType, liveodd.ToString(), false, "In-Complete", marketbookID, marketbookname, betslipamountlabel, Convert.ToDecimal(Betslipsize), Convert.ToDecimal(pendingbetamount), (location - 1).ToString(), ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, UserID);

                                }
                                else
                                {
                                    userodd = Convert.ToDecimal(selectedrunner.ExchangePrices.AvailableToBack[location].Price);

                                    nupdownAmount = pendingbetamount.ToString();
                                    Betslipsize = selectedrunner.ExchangePrices.AvailableToBack[location].Size.ToString();

                                    insertbetslip(userodd, SelectionID, Selectionname, BetType, nupdownAmount, betslipamountlabel, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, Clickedlocation, UserID, Betslipsize, Password, marketbookId, marketbookname, false);
                                }

                            }
                            else
                            {
                                loadedlocation = (location + 1);
                                location = location + 1;

                                if (location > 2)
                                {
                                    data = InsertUserBet(SelectionID, Selectionname, userodd.ToString(), newamount.ToString(), BetType, liveodd.ToString(), false, "In-Complete", marketbookID, marketbookname, betslipamountlabel, Convert.ToDecimal(Betslipsize), Convert.ToDecimal(pendingbetamount), (location - 1).ToString(), ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, UserID);
                                    if (!data.Contains("True"))
                                    {
                                        //   MessageBox.Show("Something went wrong. Please bet again or call the vendor.");
                                    }
                                }
                                else
                                {
                                    userodd = Convert.ToDecimal(selectedrunner.ExchangePrices.AvailableToLay[location].Price);

                                    nupdownAmount = pendingbetamount.ToString();
                                    Betslipsize = selectedrunner.ExchangePrices.AvailableToLay[location].Size.ToString();

                                    insertbetslip(userodd, SelectionID, Selectionname, BetType, nupdownAmount, betslipamountlabel, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, Clickedlocation, UserID, Betslipsize, Password, marketbookId, Marketbookname, false);
                                }


                            }
                        }
                        else
                        {
                            var data = InsertUserBet(SelectionID, Selectionname, userodd.ToString(), Amount.ToString(), BetType, liveodd.ToString(), ismatched, "In-Complete", marketbookID, marketbookname, betslipamountlabel, Convert.ToDecimal(Betslipsize), Convert.ToDecimal(pendingbetamount), (location).ToString(), ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, UserID);
                            if (!data.Contains("True"))
                            {
                                // MessageBox.Show("Something went wrong. Please bet again or call the vendor.");
                            }
                        }
                    }
                    else
                    {
                        var data = InsertUserBet(SelectionID, Selectionname, userodd.ToString(), Amount.ToString(), BetType, liveodd.ToString(), ismatched, "In-Complete", marketbookID, marketbookname, betslipamountlabel, Convert.ToDecimal(betslipsize), Convert.ToDecimal(pendingbetamount), location.ToString(), ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, UserID);
                        if (data.Contains("True"))
                        {
                            var ID = data.Split('|');
                            if (ParentID == 0)
                            {
                                ParentID = Convert.ToInt32(ID[1]);
                            }



                        }
                        else
                        {
                            if (!data.Contains("True"))
                            {
                                //  MessageBox.Show("Something went wrong. Please bet again or call the vendor.");
                            }
                        }


                        if (pendingbetamount > 0)
                        {
                            data = InsertUserBet(SelectionID, Selectionname, clickedodd.ToString(), pendingbetamount.ToString(), BetType, liveodd.ToString(), false, "In-Complete", marketbookID, marketbookname, betslipamountlabel, Convert.ToDecimal(clickedbetsize), Convert.ToDecimal(pendingbetamount), Clickedlocation.ToString(), ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, UserID);
                            if (!data.Contains("True"))
                            {
                                // MessageBox.Show("Something went wrong. Please bet again or call the vendor.");
                            }
                        }
                    }

                }
                else
                {
                    var data = InsertUserBet(SelectionID, Selectionname, clickedodd.ToString(), Amount.ToString(), BetType, liveodd.ToString(), ismatched, "In-Complete", marketbookID, marketbookname, betslipamountlabel, Convert.ToDecimal(clickedbetsize), Convert.ToDecimal(0), location.ToString(), ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, UserID);
                    if (!data.Contains("True"))
                    {
                        //  MessageBox.Show("Something went wrong. Please bet again or call the vendor.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                // MessageBox.Show(ex.Message.ToString());
            }


        }
        public string InsertUserBetFancy(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string marketbookid, string marketbookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay, int UserID, ExternalAPI.TO.MarketBook marketbook)
        {
            try
            {
                if (3 == 3)
                {

                    userOdd = Convert.ToDecimal(userOdd).ToString("G29");
                    selecitonname = selecitonname.Trim();
                    ismatched = false;
                    if (bettype == "back")
                    {
                        foreach (var runner in marketbook.Runners[0].ExchangePrices.AvailableToBack.Take(1))
                        {
                            if (Convert.ToDouble(userOdd) >= runner.Price)
                            {
                                ismatched = true;
                                liveodd = runner.Price.ToString();
                                userOdd = runner.Price.ToString();

                            }
                        }


                    }
                    else
                    {
                        foreach (var runner in marketbook.Runners[0].ExchangePrices.AvailableToLay.Take(1))
                        {
                            if (Convert.ToDouble(userOdd) <= runner.Price)
                            {
                                ismatched = true;
                                liveodd = runner.Price.ToString();
                                userOdd = runner.Price.ToString();
                            }
                        }

                    }
                    var ID = objUsersServiceCleint.InsertUserBet(SelectionID, UserID, userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, selecitonname, marketbookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, ConfigurationManager.AppSettings["PasswordForValidate"]);
                    return "True";

                }
                else
                {
                    return "You are not allowed to perform this operation";
                }
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                return "False";
            }

        }

        public string InsertUserBetFancyIN(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string marketbookid, string marketbookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay, int UserID, ExternalAPI.TO.MarketBook marketbook)
        {
            try
            {
                if (3 == 3)
                {

                    userOdd = Convert.ToDecimal(userOdd).ToString("G29");
                    selecitonname = selecitonname.Trim();
                    ismatched = true;
                  
                    var ID = objUsersServiceCleint.InsertUserBet(SelectionID, UserID, userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, selecitonname, marketbookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, ConfigurationManager.AppSettings["PasswordForValidate"]);
                    return "True";

                }
                else
                {
                    return "You are not allowed to perform this operation";
                }
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                return "False";
            }

        }
        public string InsertUserBet(string SelectionID, string selecitonname, string userOdd, string amount, string bettype, string liveodd, bool ismatched, string status, string marketbookid, string marketbookname, string Liablaity, decimal BetSize, decimal PendingAmount, string location, long ParentID, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay, int UserID)
        {
            try
            {
                if (3 == 3)
                {


                    selecitonname = selecitonname.Trim();
                    userOdd = Convert.ToDecimal(userOdd).ToString("G29");
                    var ID = objUsersServiceCleint.InsertUserBet(SelectionID, UserID, userOdd, amount, bettype, liveodd, ismatched, status, marketbookid, DateTime.Now, DateTime.Now, selecitonname, marketbookname, Liablaity, BetSize.ToString(), PendingAmount, location, ParentID, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, ConfigurationManager.AppSettings["PasswordForValidate"]);
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
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                return "False";
            }


        }
    }
}