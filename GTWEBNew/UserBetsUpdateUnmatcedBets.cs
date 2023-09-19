using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ExternalAPI.TO;
using GTExchNew.Models;
using UsersServiceReference;

namespace GTExchNew
{
    public class UserBetsUpdateUnmatcedBets
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        public long GetLiabalityofCurrentUserActual(int userID, string selectionID, string BetType, string MarketBookID, List<UserBets> lstUserBets, List<ExternalAPI.TO.Runner> lstRunners)
        {
            long OverAllLiabality = 0;
            string LastProfitandLoss = "";

            List<string> lstIDS = new List<string>();
            lstIDS.Add(MarketBookID); 
            var lstMarketIDS = lstIDS;
            foreach (var item in lstMarketIDS)
            {
                ExternalAPI.TO.MarketBook objMarketBook = new ExternalAPI.TO.MarketBook();
                objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                objMarketBook.Runners = lstRunners;               
                if (objMarketBook != null)
                {
                    objMarketBook.MarketId = item;
                    objMarketBook.DebitCredit = ceckProfitandLoss(objMarketBook, lstUserBets);
                }

                List<UserBets> MarketBooknames = new List<UserBets>();
                if (lstUserBets.Count > 0)
                {
                    MarketBooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketBook.MarketId).ToList();
                }
                var MarketBookname = "";
                if (MarketBooknames.Count > 0)
                {
                    MarketBookname = MarketBooknames[0].MarketBookname;
                }
                if (MarketBookname.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in objMarketBook.Runners)
                    {

                        long Profit = 0;
                        long Loss = 0;
                        Profit = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        Loss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

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
                    if (BetType == "back")
                    {
                        if (objMarketBook.Runners.Count == 1)
                        {
                            ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketBook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(), lstUserBets);
                            if (CurrentMarketProfitandloss.Runners != null)
                            {
                                CurrentMarketProfitandloss.Runners = CurrentMarketProfitandloss.Runners.Where(item1 => Convert.ToInt32(item1.SelectionId) <= Convert.ToInt32(selectionID)).ToList();
                                LastProfitandLoss = CurrentMarketProfitandloss.Runners.Min(item2 => item2.ProfitandLoss).ToString();
                            }
                        }
                        else
                        {
                            foreach (var runner in objMarketBook.Runners)
                            {
                                if (runner.SelectionId != selectionID)
                                {
                                    long ProfitandLoss = 0;

                                    ProfitandLoss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                                    if (LastProfitandLoss == "")
                                    {
                                        LastProfitandLoss = ProfitandLoss.ToString();
                                    }
                                    else
                                    {
                                        if (Convert.ToInt64(LastProfitandLoss) > ProfitandLoss)
                                        {
                                            LastProfitandLoss = ProfitandLoss.ToString();
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        if (objMarketBook.Runners.Count == 1)
                        {
                            long ProfitandLoss = 0;
                            ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketBook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(), lstUserBets);
                            if (CurrentMarketProfitandloss.Runners != null)
                            {
                                CurrentMarketProfitandloss.Runners = CurrentMarketProfitandloss.Runners.Where(item1 => Convert.ToInt32(item1.SelectionId) >= Convert.ToInt32(selectionID)).ToList();
                                LastProfitandLoss = CurrentMarketProfitandloss.Runners.Min(item2 => item2.ProfitandLoss).ToString();
                            }
                        }
                        else
                        {
                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == selectionID).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == selectionID).Sum(item2 => item2.Credit));
                            LastProfitandLoss = ProfitandLoss.ToString();
                        }                      
                    }
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

            return OverAllLiabality;
        }
        public long GetLiabalityofCurrentUserActualforOtherMarkets(int userID, string selectionID, string BetType, string MarketBookID, List<UserBets> lstUserBets)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;
            var lstMarketIDSOthers = lstUserBets.Where(item2 => item2.MarketBookID != MarketBookID).Select(item => item.MarketBookID).Distinct().ToArray();
            foreach (var item in lstMarketIDSOthers)
            {
                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                ExternalAPI.TO.MarketBook objMarketBook = new ExternalAPI.TO.MarketBook();
                objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                foreach (var selectionitem in selectionIDs)
                {
                    ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    objMarketBook.Runners.Add(objrunner);
                }
                if (objMarketBook != null)
                {
                    objMarketBook.MarketId = item;
                    objMarketBook.DebitCredit = ceckProfitandLoss(objMarketBook, lstUserBets);
                }
                List<UserBets> MarketBooknames = new List<UserBets>();
                if (lstUserBets.Count > 0)
                {
                    MarketBooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketBook.MarketId).ToList();
                }
                var MarketBookname = "";
                if (MarketBooknames.Count > 0)
                {
                    MarketBookname = MarketBooknames[0].MarketBookname;
                }
                if (MarketBookname.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in objMarketBook.Runners)
                    {

                        long Profit = 0;
                        long Loss = 0;
                        Profit = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        Loss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                        if (Profit > Loss)
                        {
                            profitandloss += Loss;
                        }
                        else
                        {
                            profitandloss += Profit;
                        }
                    }
                    LastProfitandLoss = profitandloss;
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }
                else
                {
                    if (objMarketBook.Runners.Count == 1)
                    {
                        long ProfitandLoss = 0;
                        ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketBook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(), lstUserBets);
                        if (CurrentMarketProfitandloss.Runners != null)
                        {
                            ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                        }
                        LastProfitandLoss = ProfitandLoss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in objMarketBook.Runners)
                        {
                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                            if (LastProfitandLoss > ProfitandLoss)
                            {
                                LastProfitandLoss = ProfitandLoss;
                            }
                        }

                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }

                }
            }

            return OverAllLiabality;
        }
        public long GetLiabalityofCurrentUserfancy(int userID, List<UserBets> lstUserBet)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;           
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

                long ProfitandLoss = 0;
                lstUserBets = lstUserBets.Where(s => s.location == "9").ToList();
                var slections = lstUserBets.Select(d => d.SelectionID).Distinct().ToList();
                foreach (var item2 in slections)
                {
                    ExternalAPI.TO.MarketBookForindianFancy CurrentMarketProfitandloss = GetBookPositionIN(objMarketbook.MarketId, item2, new List<Models.UserBetsForAdmin>(), new List<Models.UserBetsforSuper>(), new List<UserBetsforSamiadmin>(), new List<Models.UserBetsforAgent>(), lstUserBets);
                    if (CurrentMarketProfitandloss.RunnersForindianFancy != null)
                    {
                        ProfitandLoss += Convert.ToInt64(CurrentMarketProfitandloss.RunnersForindianFancy.Min(t => t.ProfitandLoss));
                    }

                    CurrentMarketProfitandloss = new ExternalAPI.TO.MarketBookForindianFancy();
                }
                LastProfitandLoss = ProfitandLoss;
                OverAllLiabality += LastProfitandLoss;
                LastProfitandLoss = 0;
            }

            return OverAllLiabality;
        }

        public long GetLiabalityofCurrentUser(int userID, List<UserBets> lstUserBet)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;           
            List<UserBets> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
            var lstMarketIDS = lstUserBets.Select(item => item.MarketBookID).Distinct().ToArray();

            foreach (var item in lstMarketIDS)
            {
                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                ExternalAPI.TO.MarketBook objMarketBook = new ExternalAPI.TO.MarketBook();
                objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                foreach (var selectionitem in selectionIDs)
                {
                    ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    objMarketBook.Runners.Add(objrunner);
                }

                if (objMarketBook != null)
                {
                    objMarketBook.MarketId = item;
                    objMarketBook.DebitCredit = ceckProfitandLoss(objMarketBook, lstUserBets);
                }
                List<UserBets> MarketBooknames = new List<UserBets>();
                if (lstUserBets.Count > 0)
                {
                    MarketBooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketBook.MarketId).ToList();
                }
                var MarketBookname = "";
                if (MarketBooknames.Count > 0)
                {
                    MarketBookname = MarketBooknames[0].MarketBookname;
                }
                if (MarketBookname.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in objMarketBook.Runners)
                    {
                        long Profit = 0;
                        long Loss = 0;
                        Profit = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        Loss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        if (Profit > Loss)
                        {
                            profitandloss += Loss;
                        }
                        else
                        {
                            profitandloss += Profit;
                        }
                    }

                    LastProfitandLoss = profitandloss;
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }
                else
                {
                    if (objMarketBook.Runners.Count == 1)
                    {
                        long ProfitandLoss = 0;
                        ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketBook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(), lstUserBets);

                        if (CurrentMarketProfitandloss.Runners != null)
                        {
                            ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                        }
                        LastProfitandLoss = ProfitandLoss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in objMarketBook.Runners)
                        {
                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                            if (LastProfitandLoss > ProfitandLoss)
                            {
                                LastProfitandLoss = ProfitandLoss;
                            }
                        }

                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                }
            }

            return OverAllLiabality;
        }
        public long GetLiabalityofCurrentUser(int userID, ExternalAPI.TO.MarketBook objMarketBook, List<UserBets> lstUserBets)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;

            if (objMarketBook.MarketBookName.Contains("To Be Placed"))
            {
                long profitandloss = 0;
                foreach (var runner in objMarketBook.Runners)
                {

                    long Profit = 0;
                    long Loss = 0;
                    Profit = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                    Loss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                    if (Profit > Loss)
                    {
                        profitandloss += Loss;
                    }
                    else
                    {
                        profitandloss += Profit;
                    }
                }

                LastProfitandLoss = profitandloss;
                OverAllLiabality += LastProfitandLoss;
                LastProfitandLoss = 0;
            }
            else
            {
                if (objMarketBook.Runners.Count == 1)
                {
                    long ProfitandLoss = 0;


                    ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketBook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(), lstUserBets);

                    if (CurrentMarketProfitandloss.Runners != null)
                    {
                        ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                    }


                    LastProfitandLoss = ProfitandLoss;
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }
                else
                {
                    foreach (var runner in objMarketBook.Runners)
                    {
                        long ProfitandLoss = 0;
                        ProfitandLoss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                        if (LastProfitandLoss > ProfitandLoss)
                        {
                            LastProfitandLoss = ProfitandLoss;
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
                ExternalAPI.TO.MarketBook objMarketBook = new ExternalAPI.TO.MarketBook();
                objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                foreach (var selectionitem in selectionIDs)
                {
                    ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                    objrunner.SelectionId = selectionitem.SelectionID;
                    objMarketBook.Runners.Add(objrunner);
                }
                if (objMarketBook != null)
                {
                    objMarketBook.MarketId = item;
                    objMarketBook.DebitCredit = ceckProfitandLoss(objMarketBook, lstUserBets);
                }
                List<UserBets> MarketBooknames = new List<UserBets>();
                if (lstUserBets.Count > 0)
                {
                    MarketBooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketBook.MarketId).ToList();
                }
                var MarketBookname = "";
                if (MarketBooknames.Count > 0)
                {
                    MarketBookname = MarketBooknames[0].MarketBookname;
                }
                if (MarketBookname.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in objMarketBook.Runners)
                    {

                        long Profit = 0;
                        long Loss = 0;
                        Profit = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        Loss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

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
                    objMarketLiabality.MarketBookName = MarketBookname;
                    objMarketLiabality.Liabality = profitandloss;
                    lstLiabalitybyMarket.Add(objMarketLiabality);
                    LastProfitandLoss = profitandloss;
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;
                }
                else
                {
                    if (objMarketBook.Runners.Count == 1)
                    {
                        long ProfitandLoss = 0;


                        ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketBook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(), lstUserBets);

                        if (CurrentMarketProfitandloss.Runners != null)
                        {
                            ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                        }


                        LastProfitandLoss = ProfitandLoss;
                        objMarketLiabality.MarketBookID = item;
                        objMarketLiabality.MarketBookName = MarketBookname;
                        objMarketLiabality.Liabality = LastProfitandLoss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in objMarketBook.Runners)
                        {
                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                            if (objMarketBook.Runners.Count == 1)
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
                        objMarketLiabality.MarketBookName = MarketBookname;
                        objMarketLiabality.Liabality = LastProfitandLoss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }

                }



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
        public long GetLiabalityofCurrentAgent(List<UserBetsforAgent> lstUserBet, ExternalAPI.TO.MarketBook MarketBook)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;





                if (MarketBook.MarketBookName.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in MarketBook.Runners)
                    {

                        long Profit = 0;
                        long Loss = 0;
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
                    if (MarketBook.Runners.Count == 1)
                    {
                        long ProfitandLoss = 0;


                        ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(MarketBook.MarketId, new List<UserBetsForAdmin>(), lstUserBet, new List<UserBetsforSuper>(), new List<UserBets>());

                        if (CurrentMarketProfitandloss.Runners != null)
                        {
                            ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                        }


                        LastProfitandLoss = ProfitandLoss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in MarketBook.Runners)
                        {
                            if (MarketBook.Runners.Count == 1)
                            {
                                if (runner.ProfitandLoss > 0)
                                {
                                    runner.ProfitandLoss = -1 * runner.ProfitandLoss;
                                }
                            }
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
                List<ExternalAPI.TO.MarketBook> lstMarketBooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketBook = new ExternalAPI.TO.MarketBook();
                    objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketBook.Runners.Add(objrunner);
                    }

                    if (objMarketBook != null)
                    {
                        objMarketBook.MarketId = item;
                        List<UserBetsforAgent> MarketBooknames = new List<UserBetsforAgent>();
                        if (lstUserBets.Count > 0)
                        {
                            MarketBooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketBook.MarketId).ToList();
                        }
                        var MarketBookname = "";
                        if (MarketBooknames.Count > 0)
                        {
                            MarketBookname = MarketBooknames[0].MarketBookname;
                            objMarketBook.MarketBookName = MarketBookname;
                        }

                        if (MarketBookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketBook.MarketId).ToList();
                                objMarketBook.DebitCredit = ceckProfitandLossAgent(objMarketBook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {

                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);

                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketBook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, loss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
                                        runner.Loss += Convert.ToInt64(-1 * profit);


                                    }
                                }
                            }
                        }
                        else
                        {
                            if (objMarketBook.Runners.Count == 1)
                            {
                                long ProfitandLoss = 0;


                                ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketBook.MarketId, new List<UserBetsForAdmin>(), lstUserBets, new List<UserBetsforSuper>(), new List<UserBets>());

                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                }
                                objMarketBook.Runners[0].ProfitandLoss = ProfitandLoss;
                            }
                            else
                            {
                                foreach (var userid in lstUsers)
                                {
                                    List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketBook.MarketId).ToList();
                                    objMarketBook.DebitCredit = ceckProfitandLossAgent(objMarketBook, lstuserbet);
                                    if (lstuserbet.Count > 0)
                                    {

                                        objMarketBook.DebitCredit = ceckProfitandLossAgent(objMarketBook, lstuserbet);

                                        foreach (var runner in objMarketBook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);


                                        }
                                    }

                                }
                            }
                        }

                    }
                    lstMarketBooks.Add(objMarketBook);

                }
                List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
                foreach (var MarketBook in lstMarketBooks)
                {
                    LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                    if (MarketBook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in MarketBook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            Profit = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            Loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

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
                        objMarketLiabality.MarketBookID = MarketBook.MarketId;
                        objMarketLiabality.MarketBookName = MarketBook.MarketBookName;
                        objMarketLiabality.Liabality = profitandloss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in MarketBook.Runners)
                        {
                            //if (MarketBook.Runners.Count == 1)
                            //{
                            //    if (runner.ProfitandLoss > 0)
                            //    {
                            //        runner.ProfitandLoss = -1 * runner.ProfitandLoss;
                            //    }
                            //}
                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }
                        objMarketLiabality.MarketBookID = MarketBook.MarketId;
                        objMarketLiabality.MarketBookName = MarketBook.MarketBookName;
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
                List<ExternalAPI.TO.MarketBook> lstMarketBooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketBook = new ExternalAPI.TO.MarketBook();
                    objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketBook.Runners.Add(objrunner);
                    }

                    if (objMarketBook != null)
                    {
                        objMarketBook.MarketId = item;
                        List<UserBetsForAdmin> MarketBooknames = new List<UserBetsForAdmin>();
                        if (lstUserBets.Count > 0)
                        {
                            MarketBooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketBook.MarketId).ToList();
                        }
                        var MarketBookname = "";
                        if (MarketBooknames.Count > 0)
                        {
                            MarketBookname = MarketBooknames[0].MarketBookname;
                            objMarketBook.MarketBookName = MarketBookname;
                        }
                        if (MarketBookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketBook.MarketId).ToList();
                                objMarketBook.DebitCredit = ceckProfitandLossAdmin(objMarketBook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();

                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketBook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
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
                            if (objMarketBook.Runners.Count == 1)
                            {
                                long ProfitandLoss = 0;


                                ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketBook.MarketId, lstUserBets, new List<UserBetsforAgent>(), new List<UserBetsforSuper>(), new List<UserBets>());

                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                }
                                objMarketBook.Runners[0].ProfitandLoss = ProfitandLoss;
                            }
                            else
                            {
                                foreach (var userid in lstUsers)
                                {
                                    List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketBook.MarketId).ToList();
                                    objMarketBook.DebitCredit = ceckProfitandLossAdmin(objMarketBook, lstuserbet);
                                    if (lstuserbet.Count > 0)
                                    {
                                        lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                                        decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                        bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                        foreach (var runner in objMarketBook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            if (objMarketBook.Runners.Count == 1)
                                            {
                                                if (profitorloss > 0)
                                                {
                                                    profitorloss = -1 * profitorloss;
                                                }
                                            }
                                            decimal profit = 0;
                                            if (TransferAdminAmount == false)
                                            {
                                                profit = ((100 - agentrate) / 100) * profitorloss;
                                            }
                                            else
                                            {
                                                profit = ((0) / 100) * profitorloss;
                                            }

                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        }
                                    }

                                }
                            }

                        }

                    }
                    lstMarketBooks.Add(objMarketBook);

                }

                foreach (var MarketBook in lstMarketBooks)
                {

                    if (MarketBook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in MarketBook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            //Profit = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            //Loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
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
                        foreach (var runner in MarketBook.Runners)
                        {
                            //if (MarketBook.Runners.Count == 1)
                            //{
                            //    if (runner.ProfitandLoss > 0)
                            //    {
                            //        runner.ProfitandLoss = -1 * runner.ProfitandLoss;
                            //    }
                            //}
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
        public long GetLiabalityofAdmin(List<UserBetsForAdmin> lstUserBet, ExternalAPI.TO.MarketBook MarketBook)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;

                if (MarketBook.MarketBookName.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in MarketBook.Runners)
                    {

                        long Profit = 0;
                        long Loss = 0;
                        //Profit = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        //Loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
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
                    if (MarketBook.Runners.Count == 1)
                    {
                        long ProfitandLoss = 0;


                        ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(MarketBook.MarketId, lstUserBet, new List<UserBetsforAgent>(), new List<UserBetsforSuper>(), new List<UserBets>());

                        if (CurrentMarketProfitandloss.Runners != null)
                        {
                            ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                        }


                        LastProfitandLoss = ProfitandLoss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in MarketBook.Runners)
                        {
                            //if (MarketBook.Runners.Count == 1)
                            //{
                            //    if (runner.ProfitandLoss > 0)
                            //    {
                            //        runner.ProfitandLoss = -1 * runner.ProfitandLoss;
                            //    }
                            //}
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
                List<ExternalAPI.TO.MarketBook> lstMarketBooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketBook = new ExternalAPI.TO.MarketBook();
                    objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketBook.Runners.Add(objrunner);
                    }

                    if (objMarketBook != null)
                    {
                        objMarketBook.MarketId = item;
                        List<UserBetsForAdmin> MarketBooknames = new List<UserBetsForAdmin>();
                        if (lstUserBets.Count > 0)
                        {
                            MarketBooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketBook.MarketId).ToList();
                        }
                        var MarketBookname = "";
                        if (MarketBooknames.Count > 0)
                        {
                            MarketBookname = MarketBooknames[0].MarketBookname;
                            objMarketBook.MarketBookName = MarketBookname;
                        }
                        if (MarketBookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketBook.MarketId).ToList();
                                objMarketBook.DebitCredit = ceckProfitandLossAdmin(objMarketBook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {


                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketBook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
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
                            if (objMarketBook.Runners.Count == 1)
                            {
                                long ProfitandLoss = 0;


                                ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketBook.MarketId, lstUserBets, new List<UserBetsforAgent>(), new List<UserBetsforSuper>(), new List<UserBets>());

                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                }
                                objMarketBook.Runners[0].ProfitandLoss = ProfitandLoss;
                            }
                            else
                            {
                                foreach (var userid in lstUsers)
                                {
                                    List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketBook.MarketId).ToList();
                                    objMarketBook.DebitCredit = ceckProfitandLossAdmin(objMarketBook, lstuserbet);
                                    if (lstuserbet.Count > 0)
                                    {
                                        decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                        bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                        foreach (var runner in objMarketBook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            if (objMarketBook.Runners.Count == 1)
                                            {
                                                if (profitorloss > 0)
                                                {
                                                    profitorloss = -1 * profitorloss;
                                                }
                                            }

                                            decimal profit = TransferAdminAmount == false ? ((100 - agentrate) / 100) * profitorloss : ((0) / 100) * profitorloss;
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        }
                                    }

                                }
                            }
                        }

                    }
                    lstMarketBooks.Add(objMarketBook);

                }
                List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
                foreach (var MarketBook in lstMarketBooks)
                {
                    LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                    if (MarketBook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in MarketBook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            //Profit = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            //Loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
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
                        objMarketLiabality.MarketBookID = MarketBook.MarketId;
                        objMarketLiabality.MarketBookName = MarketBook.MarketBookName;
                        objMarketLiabality.Liabality = profitandloss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in MarketBook.Runners)
                        {
                            //if (MarketBook.Runners.Count == 1)
                            //{
                            //    if (runner.ProfitandLoss > 0)
                            //    {
                            //        runner.ProfitandLoss = -1 * runner.ProfitandLoss;
                            //    }
                            //}
                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }
                        objMarketLiabality.MarketBookID = MarketBook.MarketId;
                        objMarketLiabality.MarketBookName = MarketBook.MarketBookName;
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
                List<ExternalAPI.TO.MarketBook> lstMarketBooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketBook = new ExternalAPI.TO.MarketBook();
                    objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketBook.Runners.Add(objrunner);
                    }

                    if (objMarketBook != null)
                    {
                        objMarketBook.MarketId = item;
                        List<UserBetsforSuper> MarketBooknames = new List<UserBetsforSuper>();
                        if (lstUserBets.Count > 0)
                        {
                            MarketBooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketBook.MarketId).ToList();
                        }
                        var MarketBookname = "";
                        if (MarketBooknames.Count > 0)
                        {
                            MarketBookname = MarketBooknames[0].MarketBookname;
                            objMarketBook.MarketBookName = MarketBookname;
                        }
                        if (MarketBookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketBook.MarketId).ToList();
                                objMarketBook.DebitCredit = ceckProfitandLossSuper(objMarketBook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();

                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketBook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
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
                            if (objMarketBook.Runners.Count == 1)
                            {
                                long ProfitandLoss = 0;


                                ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketBook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), lstUserBets, new List<UserBets>());

                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                }
                                objMarketBook.Runners[0].ProfitandLoss = ProfitandLoss;
                            }
                            else
                            {
                                foreach (var userid in lstUsers)
                                {
                                    List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketBook.MarketId).ToList();
                                    objMarketBook.DebitCredit = ceckProfitandLossSuper(objMarketBook, lstuserbet);
                                    if (lstuserbet.Count > 0)
                                    {
                                        lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                                        decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                        bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                        foreach (var runner in objMarketBook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            if (objMarketBook.Runners.Count == 1)
                                            {
                                                if (profitorloss > 0)
                                                {
                                                    profitorloss = -1 * profitorloss;
                                                }
                                            }
                                            decimal profit = 0;
                                            if (TransferAdminAmount == false)
                                            {
                                                profit = ((100 - agentrate) / 100) * profitorloss;
                                            }
                                            else
                                            {
                                                profit = ((0) / 100) * profitorloss;
                                            }

                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                                        }
                                    }

                                }
                            }

                        }

                    }
                    lstMarketBooks.Add(objMarketBook);

                }

                foreach (var MarketBook in lstMarketBooks)
                {

                    if (MarketBook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in MarketBook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            //Profit = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            //Loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
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
                        foreach (var runner in MarketBook.Runners)
                        {
                            //if (MarketBook.Runners.Count == 1)
                            //{
                            //    if (runner.ProfitandLoss > 0)
                            //    {
                            //        runner.ProfitandLoss = -1 * runner.ProfitandLoss;
                            //    }
                            //}
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
        public long GetLiabalityofSuper(List<UserBetsforSuper> lstUserBet, ExternalAPI.TO.MarketBook MarketBook)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;

                if (MarketBook.MarketBookName.Contains("To Be Placed"))
                {
                    long profitandloss = 0;
                    foreach (var runner in MarketBook.Runners)
                    {

                        long Profit = 0;
                        long Loss = 0;
                        //Profit = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        //Loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
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
                    if (MarketBook.Runners.Count == 1)
                    {
                        long ProfitandLoss = 0;


                        ExternalAPI.TO.MarketBook CurrentMarketProfitandloss = GetBookPosition(MarketBook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), lstUserBet, new List<UserBets>());

                        if (CurrentMarketProfitandloss.Runners != null)
                        {
                            ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                        }


                        LastProfitandLoss = ProfitandLoss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in MarketBook.Runners)
                        {
                            //if (MarketBook.Runners.Count == 1)
                            //{
                            //    if (runner.ProfitandLoss > 0)
                            //    {
                            //        runner.ProfitandLoss = -1 * runner.ProfitandLoss;
                            //    }
                            //}
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
                List<ExternalAPI.TO.MarketBook> lstMarketBooks = new List<ExternalAPI.TO.MarketBook>();
                foreach (var item in lstMarketIDS)
                {
                    var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                    ExternalAPI.TO.MarketBook objMarketBook = new ExternalAPI.TO.MarketBook();
                    objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                    foreach (var selectionitem in selectionIDs)
                    {
                        ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                        objrunner.SelectionId = selectionitem.SelectionID;
                        objMarketBook.Runners.Add(objrunner);
                    }

                    if (objMarketBook != null)
                    {
                        objMarketBook.MarketId = item;
                        List<UserBetsforSuper> MarketBooknames = new List<UserBetsforSuper>();
                        if (lstUserBets.Count > 0)
                        {
                            MarketBooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketBook.MarketId).ToList();
                        }
                        var MarketBookname = "";
                        if (MarketBooknames.Count > 0)
                        {
                            MarketBookname = MarketBooknames[0].MarketBookname;
                            objMarketBook.MarketBookName = MarketBookname;
                        }
                        if (MarketBookname.Contains("To Be Placed"))
                        {
                            foreach (var userid in lstUsers)
                            {
                                List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketBook.MarketId).ToList();
                                objMarketBook.DebitCredit = ceckProfitandLossSuper(objMarketBook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {


                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketBook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
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
                                List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketBook.MarketId).ToList();
                                objMarketBook.DebitCredit = ceckProfitandLossSuper(objMarketBook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                    foreach (var runner in objMarketBook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        if (objMarketBook.Runners.Count == 1)
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
                    lstMarketBooks.Add(objMarketBook);

                }
                List<LiabalitybyMarket> lstLiabalitybyMarket = new List<LiabalitybyMarket>();
                foreach (var MarketBook in lstMarketBooks)
                {
                    LiabalitybyMarket objMarketLiabality = new LiabalitybyMarket();
                    if (MarketBook.MarketBookName.Contains("To Be Placed"))
                    {
                        long profitandloss = 0;
                        foreach (var runner in MarketBook.Runners)
                        {

                            long Profit = 0;
                            long Loss = 0;
                            Profit = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                            Loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

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
                        objMarketLiabality.MarketBookID = MarketBook.MarketId;
                        objMarketLiabality.MarketBookName = MarketBook.MarketBookName;
                        objMarketLiabality.Liabality = profitandloss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
                        LastProfitandLoss = profitandloss;
                        OverAllLiabality += LastProfitandLoss;
                        LastProfitandLoss = 0;
                    }
                    else
                    {
                        foreach (var runner in MarketBook.Runners)
                        {

                            if (LastProfitandLoss > runner.ProfitandLoss)
                            {
                                LastProfitandLoss = runner.ProfitandLoss;
                            }
                        }
                        objMarketLiabality.MarketBookID = MarketBook.MarketId;
                        objMarketLiabality.MarketBookName = MarketBook.MarketBookName;
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

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossSuper(ExternalAPI.TO.MarketBook MarketBookstatus, List<UserBetsforSuper> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == MarketBookstatus.MarketId);
            if (MarketBookstatus.Runners.Count() == 1)
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
            if (MarketBookstatus.Runners[0].Handicap < 0)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        double handicap = MarketBookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in MarketBookstatus.Runners)
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
                        foreach (var runneritem in MarketBookstatus.Runners)
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
                        double handicap = MarketBookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in MarketBookstatus.Runners)
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
                        foreach (var runneritem in MarketBookstatus.Runners)
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
                        foreach (var runneritem in MarketBookstatus.Runners)
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
                        foreach (var runneritem in MarketBookstatus.Runners)
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

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLoss(ExternalAPI.TO.MarketBook MarketBookstatus, List<UserBets> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == MarketBookstatus.MarketId);
            if (lstUserbetsbyMarketID != null)
            {

                if (MarketBookstatus.Runners.Count() == 1)
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


                if (MarketBookstatus.Runners[0].Handicap < 0)
                {
                    foreach (var userbet in lstUserbetsbyMarketID)
                    {

                        var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                        var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                        if (userbet.BetType == "back")
                        {
                            double handicap = MarketBookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.SelectionID;
                            objDebitCredit.Debit = totamount;
                            objDebitCredit.Credit = 0;
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            double handicap = MarketBookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.SelectionID;
                            objDebitCredit.Debit = 0;
                            objDebitCredit.Credit = totamount;
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            foreach (var runneritem in MarketBookstatus.Runners)
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
            else
            {
                return lstDebitCredit;
            }


        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossFig(ExternalAPI.TO.MarketBook MarketBookstatus, List<UserBets> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == MarketBookstatus.MarketId);
            if (lstUserbetsbyMarketID != null)
            {

                if (MarketBookstatus.Runners.Count() == 1)
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


                if (MarketBookstatus.Runners[0].Handicap < 0)
                {
                    foreach (var userbet in lstUserbetsbyMarketID)
                    {

                        var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                        var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                        if (userbet.BetType == "back")
                        {
                            double handicap = MarketBookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.SelectionID;
                            objDebitCredit.Debit = totamount;
                            objDebitCredit.Credit = 0;
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            double handicap = MarketBookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.SelectionID;
                            objDebitCredit.Debit = 0;
                            objDebitCredit.Credit = totamount;
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            foreach (var runneritem in MarketBookstatus.Runners)
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

                        var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd) / 100);
                        var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                        if (userbet.BetType == "back")
                        {

                            objDebitCredit.SelectionID = userbet.SelectionID;
                            objDebitCredit.Debit = totamount;
                            objDebitCredit.Credit = 0;
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            foreach (var runneritem in MarketBookstatus.Runners)
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
            else
            {
                return lstDebitCredit;
            }


        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossAgent(ExternalAPI.TO.MarketBook MarketBookstatus, List<UserBetsforAgent> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == MarketBookstatus.MarketId);
            if (MarketBookstatus.Runners.Count() == 1)
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
            if (MarketBookstatus.Runners[0].Handicap < 0)
            {
                foreach (var userbet in lstUserbetsbyMarketID)
                {

                    var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                    var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                    if (userbet.BetType == "back")
                    {
                        double handicap = MarketBookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = totamount;
                        objDebitCredit.Credit = 0;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in MarketBookstatus.Runners)
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
                        foreach (var runneritem in MarketBookstatus.Runners)
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
                        double handicap = MarketBookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                        objDebitCredit.SelectionID = userbet.SelectionID;
                        objDebitCredit.Debit = 0;
                        objDebitCredit.Credit = totamount;
                        lstDebitCredit.Add(objDebitCredit);
                        foreach (var runneritem in MarketBookstatus.Runners)
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
                        foreach (var runneritem in MarketBookstatus.Runners)
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
                        foreach (var runneritem in MarketBookstatus.Runners)
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
                        foreach (var runneritem in MarketBookstatus.Runners)
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



        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossAdmin(ExternalAPI.TO.MarketBook MarketBookstatus, List<UserBetsForAdmin> lstUserBets)
        {
            try
            {

                List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
                var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == MarketBookstatus.MarketId);
                if (MarketBookstatus.Runners.Count() == 1)
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
                if (MarketBookstatus.Runners[0].Handicap < 0)
                {
                    foreach (var userbet in lstUserbetsbyMarketID)
                    {

                        var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd)) - Convert.ToDecimal(userbet.Amount);
                        var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                        if (userbet.BetType == "back")
                        {
                            double handicap = MarketBookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.SelectionID;
                            objDebitCredit.Debit = totamount;
                            objDebitCredit.Credit = 0;
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            double handicap = MarketBookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
                            objDebitCredit.SelectionID = userbet.SelectionID;
                            objDebitCredit.Debit = 0;
                            objDebitCredit.Credit = totamount;
                            lstDebitCredit.Add(objDebitCredit);
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            foreach (var runneritem in MarketBookstatus.Runners)
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
                            foreach (var runneritem in MarketBookstatus.Runners)
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
            catch (System.Exception ex)
            {
                return new List<ExternalAPI.TO.DebitCredit>();

            }
        }


        public ExternalAPI.TO.MarketBook GetBookPosition(string MarketBookID, List<UserBetsForAdmin> CurrentAdminBets, List<UserBetsforAgent> CurrentAgentBets, List<UserBetsforSuper> CurrentSuperBets, List<UserBets> CurrentUserBets)
        {

            ExternalAPI.TO.MarketBook objMarketBook = new ExternalAPI.TO.MarketBook();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                List<UserBetsForAdmin> lstCurrentBetsAdmin = CurrentAdminBets.Where(item => item.MarketBookID == MarketBookID && item.isMatched == true).ToList();
                if (lstCurrentBetsAdmin.Count > 0)
                {
                    lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                    objMarketBook.MarketId = MarketBookID;
                    objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                    ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                    objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                    objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                    objMarketBook.Runners.Add(objRunner1);
                    foreach (var userbet in lstCurrentBetsAdmin)
                    {
                        if (objMarketBook.Runners != null)
                        {
                            ExternalAPI.TO.Runner objexistingrunner = objMarketBook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                            if (objexistingrunner == null)
                            {
                                ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                objMarketBook.Runners.Add(objRunner);
                            }
                        }
                        else
                        {
                            ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                            objRunner.SelectionId = userbet.UserOdd;
                            objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                            objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                            objMarketBook.Runners.Add(objRunner);
                        }



                    }
                    ExternalAPI.TO.Runner objRunnerlast = new ExternalAPI.TO.Runner();
                    objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                    objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                    objMarketBook.Runners.Add(objRunnerlast);

                    ///calculation
                    var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                    foreach (var userid in lstUsers)
                    {
                        var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();
                        decimal agentrate = Convert.ToDecimal(lstCurrentBetsbyUser[0].AgentRate);
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        foreach (var userbet in lstCurrentBetsbyUser)
                        {

                            var totamount = TransferAdinAmount == false ? (Convert.ToDecimal(userbet.Amount) * ((100 - agentrate) / 100)) : (Convert.ToDecimal(userbet.Amount) * ((0) / 100));

                            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                            if (userbet.BetType == "back")
                            {
                                double handicap = objMarketBook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = totamount;
                                objDebitCredit.Credit = 0;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objMarketBook.Runners)
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
                                foreach (var runneritem in objMarketBook.Runners)
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
                                double handicap = objMarketBook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                objDebitCredit.SelectionID = userbet.UserOdd;
                                objDebitCredit.Debit = 0;
                                objDebitCredit.Credit = totamount;
                                lstDebitCredit.Add(objDebitCredit);
                                foreach (var runneritem in objMarketBook.Runners)
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
                                foreach (var runneritem in objMarketBook.Runners)
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

                    objMarketBook.DebitCredit = lstDebitCredit;
                    foreach (var runneritem in objMarketBook.Runners)
                    {


                        runneritem.ProfitandLoss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));

                        runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;


                    }




                }
            }
            else
            {
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    List<UserBetsforAgent> lstCurrentBetsAdmin = CurrentAgentBets.Where(item => item.MarketBookID == MarketBookID && item.isMatched == true).ToList();
                    if (lstCurrentBetsAdmin.Count > 0)
                    {
                        lstCurrentBetsAdmin = lstCurrentBetsAdmin.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                        objMarketBook.MarketId = MarketBookID;
                        objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                        ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                        objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin[0].UserOdd) - 1).ToString();
                        objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin[0].UserOdd) - 1);
                        objMarketBook.Runners.Add(objRunner1);
                        foreach (var userbet in lstCurrentBetsAdmin)
                        {
                            if (objMarketBook.Runners != null)
                            {
                                ExternalAPI.TO.Runner objexistingrunner = objMarketBook.Runners.Where(item => item.SelectionId == userbet.UserOdd).FirstOrDefault();
                                if (objexistingrunner == null)
                                {
                                    ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                    objRunner.SelectionId = userbet.UserOdd;
                                    objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);

                                    objMarketBook.Runners.Add(objRunner);
                                }
                            }
                            else
                            {
                                ExternalAPI.TO.Runner objRunner = new ExternalAPI.TO.Runner();
                                objRunner.SelectionId = userbet.UserOdd;
                                objRunner.Handicap = -1 * Convert.ToDouble(userbet.UserOdd);
                                objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                                objMarketBook.Runners.Add(objRunner);
                            }



                        }
                        ExternalAPI.TO.Runner objRunnerlast = new ExternalAPI.TO.Runner();
                        objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBetsAdmin.Last().UserOdd) + 1).ToString();
                        objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBetsAdmin.Last().UserOdd) + 1);
                        objMarketBook.Runners.Add(objRunnerlast);
                        ///calculation
                        var lstUsers = lstCurrentBetsAdmin.Select(item => new { item.UserID }).Distinct().ToArray();
                        foreach (var userid in lstUsers)
                        {
                            var lstCurrentBetsbyUser = lstCurrentBetsAdmin.Where(item => item.UserID.Value == userid.UserID).ToList();

                            foreach (var userbet in lstCurrentBetsbyUser)
                            {

                                decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
                                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                if (userbet.BetType == "back")
                                {
                                    double handicap = objMarketBook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = totamount;
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objMarketBook.Runners)
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
                                    foreach (var runneritem in objMarketBook.Runners)
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
                                    double handicap = objMarketBook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objMarketBook.Runners)
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
                                    foreach (var runneritem in objMarketBook.Runners)
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

                            }
                        }

                        objMarketBook.DebitCredit = lstDebitCredit;
                        foreach (var runneritem in objMarketBook.Runners)
                        {

                            runneritem.ProfitandLoss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                            runneritem.ProfitandLoss = -1 * runneritem.ProfitandLoss;
                        }
                    }
                }
                else
                {
                    if (LoggedinUserDetail.GetUserTypeID() == 3)
                    {

                        List<UserBets> lstCurrentBets = CurrentUserBets.Where(item => item.MarketBookID == MarketBookID && item.isMatched == true).ToList();
                        if (lstCurrentBets.Count > 0)
                        {
                            lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                            objMarketBook.MarketId = MarketBookID;
                            objMarketBook.Runners = new List<ExternalAPI.TO.Runner>();
                            for (int i = 1; i <= 500; i++)
                            {
                                ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                                objRunner1.SelectionId = (i).ToString();
                                objRunner1.Handicap = -1 * i;
                                objMarketBook.Runners.Add(objRunner1);
                            }

                            ///calculation
                            foreach (var userbet in lstCurrentBets)
                            {

                                var totamount = (Convert.ToDecimal(userbet.Amount));
                                var objDebitCredit = new ExternalAPI.TO.DebitCredit();
                                if (userbet.BetType == "back")
                                {
                                    double handicap = objMarketBook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = totamount;
                                    objDebitCredit.Credit = 0;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objMarketBook.Runners)
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
                                    foreach (var runneritem in objMarketBook.Runners)
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
                                    double handicap = objMarketBook.Runners.Where(item => item.SelectionId == userbet.UserOdd).Select(item => item.Handicap).First().Value;
                                    objDebitCredit.SelectionID = userbet.UserOdd;
                                    objDebitCredit.Debit = 0;
                                    objDebitCredit.Credit = totamount;
                                    lstDebitCredit.Add(objDebitCredit);
                                    foreach (var runneritem in objMarketBook.Runners)
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
                                    foreach (var runneritem in objMarketBook.Runners)
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
                            objMarketBook.DebitCredit = lstDebitCredit;
                            foreach (var runneritem in objMarketBook.Runners)
                            {

                                runneritem.ProfitandLoss = Convert.ToInt64(objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Debit) - objMarketBook.DebitCredit.Where(item2 => item2.SelectionID == runneritem.SelectionId).Sum(item2 => item2.Credit));
                            }


                        }
                    }
                }


            }
            return objMarketBook;
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





                                objmarketbook.MarketId = marketBookID;
                                objmarketbook.MarketBookName = lstCurrentBets[0].MarketBookname;
                                objmarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                                //for (int i = 1; i <= 500; i++)
                                //{
                                //    ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                                //    objRunner1.SelectionId = (i).ToString();
                                //    objRunner1.Handicap = -1 * i;
                                //    objmarketbook.Runners.Add(objRunner1);
                                //}
                                ExternalAPI.TO.Runner objRunner1 = new ExternalAPI.TO.Runner();
                                objRunner1.SelectionId = (Convert.ToInt32(lstCurrentBets[0].UserOdd) - 1).ToString();
                                objRunner1.Handicap = -1 * (Convert.ToDouble(lstCurrentBets[0].UserOdd) - 1);
                                objmarketbook.Runners.Add(objRunner1);
                                foreach (var userbet in lstCurrentBets)
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
                                objRunnerlast.SelectionId = (Convert.ToInt32(lstCurrentBets.Last().UserOdd) + 1).ToString();
                                objRunnerlast.Handicap = -1 * (Convert.ToDouble(lstCurrentBets.Last().UserOdd) + 1);
                                objmarketbook.Runners.Add(objRunnerlast);
                                // calculation
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