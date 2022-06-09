


using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using globaltraders.UserServiceReference;
using ExternalAPI;
using ExternalAPI.TO;

using System.Configuration;
using bftradeline.Models;
using globaltraders.Service123Reference;

namespace globaltraders
{
    public class UserBetsUpdateUnmatcedBets
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
       
       
        public long GetLiabalityofCurrentUserActual(int userID, string selectionID, string BetType, string marketbookID, List<UserBets> lstUserBets, List<ExternalAPI.TO.Runner> lstRunners)
        {
            long OverAllLiabality = 0;
            string LastProfitandLoss = "";

            List<string> lstIDS = new List<string>();
            lstIDS.Add(marketbookID);
            
            var lstMarketIDS = lstIDS;
            foreach (var item in lstMarketIDS)
            {
                
                ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                objMarketbook.Runners = lstRunners;
               
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    objMarketbook.DebitCredit = ceckProfitandLoss(objMarketbook, lstUserBets);
                }
              
                List<UserBets> marketbooknames = new List<UserBets>();
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
                    if (BetType == "back")
                    {
                        if (objMarketbook.Runners.Count == 1)
                        {
                            MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), lstUserBets);
                            if (CurrentMarketProfitandloss.Runners != null)
                            {
                                CurrentMarketProfitandloss.Runners = CurrentMarketProfitandloss.Runners.Where(item1 => Convert.ToInt32(item1.SelectionId) <= Convert.ToInt32(selectionID)).ToList();
                                LastProfitandLoss = CurrentMarketProfitandloss.Runners.Min(item2 => item2.ProfitandLoss).ToString();

                            }
                           
                        }
                        else
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
                        if (objMarketbook.Runners.Count == 1)
                        {
                            long ProfitandLoss = 0;

                            MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), lstUserBets);

                            if (CurrentMarketProfitandloss.Runners != null)
                            {
                                CurrentMarketProfitandloss.Runners = CurrentMarketProfitandloss.Runners.Where(item1 => Convert.ToInt32(item1.SelectionId) >= Convert.ToInt32(selectionID)).ToList();
                                LastProfitandLoss = CurrentMarketProfitandloss.Runners.Min(item2 => item2.ProfitandLoss).ToString();                             
                            }
                        }
                        else
                        {
                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == selectionID).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == selectionID).Sum(item2 => item2.Credit));
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
            // var currentMarketIDSOthers = lstUserBets.Where(item2 => item2.SelectionID == selectionID).Select(item => item.MarketBookID).FirstOrDefault();
            //OverAllLiabality += LastProfitandLoss;
            //LastProfitandLoss = 0;
            return OverAllLiabality;
        }
        public long GetLiabalityofCurrentUserActualforOtherMarkets(int userID, string selectionID, string BetType, string marketbookID, List<UserBets> lstUserBets)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;


            var lstMarketIDSOthers = lstUserBets.Where(item2 => item2.MarketBookID != marketbookID).Select(item => item.MarketBookID).Distinct().ToArray();
            foreach (var item in lstMarketIDSOthers)
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
                List<UserBets> marketbooknames = new List<UserBets>();
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


                        MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), lstUserBets);
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
                        foreach (var runner in objMarketbook.Runners)
                        {

                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

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
            //List<UserBets> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userID));
            lstUserBet = lstUserBet.Where(x => x.location == "8" || x.location == "9").ToList();
            List<UserBets> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
            var lstMarketIDS = lstUserBets.Select(item => item.MarketBookname).Distinct().ToArray();

            foreach (var item in lstMarketIDS)
            {

                var selectionIDs = objUsersServiceCleint.GetSelectionNamesbyMarketID(item);//This is data get from marketcatelog selection
                ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
                //foreach (var selectionitem in selectionIDs)
                //{
                    ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                    objrunner.SelectionId = lstUserBets.Where(item1=>item1.SelectionName==item).ToString();
                    objMarketbook.Runners.Add(objrunner);
               // }
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    objMarketbook.DebitCredit = ceckProfitandLossfancy(objMarketbook, lstUserBets);
                }
                List<UserBets> marketbooknames = new List<UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.MarketBookID == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }
                     
                        foreach (var runner in objMarketbook.Runners)
                        {
                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                            if (LastProfitandLoss > ProfitandLoss)
                            {
                                LastProfitandLoss = ProfitandLoss;
                            }
                        }
                   
                    OverAllLiabality += LastProfitandLoss;
                    LastProfitandLoss = 0;                
            }

            return OverAllLiabality;
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
                var selectionIDs = item; //objUsersServiceCleint.GetSelectionNamesbyMarketID(item);
                ExternalAPI.TO.MarketBook objMarketbook = new ExternalAPI.TO.MarketBook();
                objMarketbook.Runners = new List<ExternalAPI.TO.Runner>();
               // foreach (var selectionitem in selectionIDs)
                //{
                    ExternalAPI.TO.Runner objrunner = new ExternalAPI.TO.Runner();
                    objrunner.SelectionId = lstUserBets.Where(item1 => item1.SelectionName == item).ToString(); //selectionitem.SelectionID;
                    objMarketbook.Runners.Add(objrunner);
               // }
                if (objMarketbook != null)
                {
                    objMarketbook.MarketId = item;
                    objMarketbook.DebitCredit = ceckProfitandLossfancy(objMarketbook, lstUserBets);
                }
                List<UserBets> marketbooknames = new List<UserBets>();
                if (lstUserBets.Count > 0)
                {
                    marketbooknames = lstUserBets.Where(item2 => item2.SelectionName == objMarketbook.MarketId).ToList();
                }
                var marketbookname = "";
                if (marketbooknames.Count > 0)
                {
                    marketbookname = marketbooknames[0].MarketBookname;
                }
                
               
                    //if (objMarketbook.Runners.Count == 1)
                    //{
                    //    long ProfitandLoss = 0;
                    //    MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), lstUserBets);
                    //    if (CurrentMarketProfitandloss.Runners != null)
                    //    {
                    //        ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                    //    }
                    //    LastProfitandLoss = ProfitandLoss;
                    //    objMarketLiabality.MarketBookID = item;
                    //    objMarketLiabality.MarketBookName = marketbookname;
                    //    objMarketLiabality.Liabality = LastProfitandLoss;
                    //    lstLiabalitybyMarket.Add(objMarketLiabality);
                    //    OverAllLiabality += LastProfitandLoss;
                    //    LastProfitandLoss = 0;
                    //}
                    //else
                    //{
                        foreach (var runner in objMarketbook.Runners)
                        {
                            long ProfitandLoss = 0;
                    //ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                    ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where( item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
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
                   // }
                
            }

            return lstLiabalitybyMarket;
        }

        public long GetLiabalityofCurrentUser(int userID, List<UserBets> lstUserBet)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;
            //List<UserBets> lstUserBet = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userID));
            lstUserBet = lstUserBet.Where(x => x.location != "8" && x.location != "9").ToList();
            List<UserBets> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
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
                List<UserBets> marketbooknames = new List<UserBets>();
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


                        MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), lstUserBets);

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
                        foreach (var runner in objMarketbook.Runners)
                        {
                            long ProfitandLoss = 0;
                            ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

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

        

        public long GetLiabalityofCurrentUser(int userID, MarketBook objMarketbook, List<UserBets> lstUserBets)
        {
            long OverAllLiabality = 0;
            long LastProfitandLoss = 0;

            if (objMarketbook.MarketBookName.Contains("To Be Placed"))
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


                    MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), lstUserBets);

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
                    foreach (var runner in objMarketbook.Runners)
                    {
                        long ProfitandLoss = 0;
                        ProfitandLoss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

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
                List<UserBets> marketbooknames = new List<UserBets>();
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
                    if (objMarketbook.Runners.Count == 1)
                    {
                        long ProfitandLoss = 0;
                        MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), lstUserBets);
                        if (CurrentMarketProfitandloss.Runners != null)
                        {
                            ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                        }
                        LastProfitandLoss = ProfitandLoss;
                        objMarketLiabality.MarketBookID = item;
                        objMarketLiabality.MarketBookName = marketbookname;
                        objMarketLiabality.Liabality = LastProfitandLoss;
                        lstLiabalitybyMarket.Add(objMarketLiabality);
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
                        List<UserBetsforAgent> marketbooknames = new List<UserBetsforAgent>();
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
                                    lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                                   
                                    foreach (var runner in objMarketbook.Runners)
                                    {
                                        long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                                        long loss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, loss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
                                        runner.Loss += Convert.ToInt64(-1 * profit);
                                        //decimal profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
                                        //runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        //profit = (Convert.ToDecimal(agentrate) / 100) * loss;
                                        //runner.Loss += Convert.ToInt64(-1 * profit);


                                    }
                                }
                            }
                        }
                        else
                        {
                            if (objMarketbook.Runners.Count == 1)
                            {
                                long ProfitandLoss = 0;


                                MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), lstUserBets, new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), new List<UserBets>());

                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                }
                                objMarketbook.Runners[0].ProfitandLoss = ProfitandLoss;
                            }
                            else
                            {
                                foreach (var userid in lstUsers)
                                {
                                    List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                    objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);
                                    if (lstuserbet.Count > 0)
                                    {
                                        lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                                       
                                        objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);

                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            if (objMarketbook.Runners.Count == 1)
                                            {
                                                if (profitorloss > 0)
                                                {
                                                    profitorloss = -1 * profitorloss;
                                                }
                                            }
                                            decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
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
                            if (marketbook.Runners.Count == 1)
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return 0;
            }


        }
        public long GetLiabalityofCurrentAgent(List<UserBetsforAgent> lstUserBet, MarketBook marketbook)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;





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
                    if (marketbook.Runners.Count == 1)
                    {
                        long ProfitandLoss = 0;


                        MarketBook CurrentMarketProfitandloss = GetBookPosition(marketbook.MarketId, new List<UserBetsForAdmin>(), lstUserBet, new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), new List<UserBets>());

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
                        foreach (var runner in marketbook.Runners)
                        {
                            if (marketbook.Runners.Count == 1)
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                        List<UserBetsforAgent> marketbooknames = new List<UserBetsforAgent>();
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
                            if (objMarketbook.Runners.Count == 1)
                            {
                                long ProfitandLoss = 0;


                                MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), lstUserBets, new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), new List<UserBets>());

                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                }
                                objMarketbook.Runners[0].ProfitandLoss = ProfitandLoss;
                            }
                            else
                            {
                                foreach (var userid in lstUsers)
                                {
                                    List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                    objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);
                                    if (lstuserbet.Count > 0)
                                    {
                                        
                                        objMarketbook.DebitCredit = ceckProfitandLossAgent(objMarketbook, lstuserbet);

                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
                                            runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                            

                                        }
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
                            //if (marketbook.Runners.Count == 1)
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                        List<UserBetsForAdmin> marketbooknames = new List<UserBetsForAdmin>();
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
                                    lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();

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
                            if (objMarketbook.Runners.Count == 1)
                            {
                                long ProfitandLoss = 0;


                                MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, lstUserBets, new List<UserBetsforAgent>(), new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), new List<UserBets>());

                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                }
                                objMarketbook.Runners[0].ProfitandLoss = ProfitandLoss;
                            }
                            else
                            {
                                foreach (var userid in lstUsers)
                                {
                                    List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                    objMarketbook.DebitCredit = ceckProfitandLossAdmin(objMarketbook, lstuserbet);
                                    if (lstuserbet.Count > 0)
                                    {
                                        lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                                        decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                        bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            if (objMarketbook.Runners.Count == 1)
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
                            //if (marketbook.Runners.Count == 1)
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return 0;
            }

        }
        public long GetLiabalityofAdmin(List<UserBetsForAdmin> lstUserBet, MarketBook marketbook)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;

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
                    if (marketbook.Runners.Count == 1)
                    {
                        long ProfitandLoss = 0;


                        MarketBook CurrentMarketProfitandloss = GetBookPosition(marketbook.MarketId, lstUserBet, new List<UserBetsforAgent>(), new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), new List<UserBets>());

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
                        foreach (var runner in marketbook.Runners)
                        {
                            //if (marketbook.Runners.Count == 1)
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                        List<UserBetsForAdmin> marketbooknames = new List<UserBetsForAdmin>();
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
                                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                                        profit = (adminrate / 100) * loss;
                                        runner.Loss += Convert.ToInt64(-1 * profit);

                                    }
                                }

                            }
                        }
                        else
                        {
                            if (objMarketbook.Runners.Count == 1)
                            {
                                long ProfitandLoss = 0;


                                MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, lstUserBets, new List<UserBetsforAgent>(), new List<UserBetsforSuper>(),new List<UserBetsforSamiAdmin>(), new List<UserBets>());

                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                }
                                objMarketbook.Runners[0].ProfitandLoss = ProfitandLoss;
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
                            //if (marketbook.Runners.Count == 1)
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                        List<UserBetsforSuper> marketbooknames = new List<UserBetsforSuper>();
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
                                    lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();

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
                            if (objMarketbook.Runners.Count == 1)
                            {
                                long ProfitandLoss = 0;


                                MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), lstUserBets,new List<UserBetsforSamiAdmin>(), new List<UserBets>());

                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                }
                                objMarketbook.Runners[0].ProfitandLoss = ProfitandLoss;
                            }
                            else
                            {
                                foreach (var userid in lstUsers)
                                {
                                    List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                    objMarketbook.DebitCredit = ceckProfitandLossSuper(objMarketbook, lstuserbet);
                                    if (lstuserbet.Count > 0)
                                    {
                                        lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                                        decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                        bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            if (objMarketbook.Runners.Count == 1)
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
                            //if (marketbook.Runners.Count == 1)
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return 0;
            }

        }

        public long GetLiabalityofSuper(List<UserBetsforSuper> lstUserBet, MarketBook marketbook)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;

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
                    if (marketbook.Runners.Count == 1)
                    {
                        long ProfitandLoss = 0;


                        MarketBook CurrentMarketProfitandloss = GetBookPosition(marketbook.MarketId, new List<UserBetsForAdmin>() , new List<UserBetsforAgent>(), lstUserBet,new List<UserBetsforSamiAdmin>(), new List<UserBets>());

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
                        foreach (var runner in marketbook.Runners)
                        {
                            //if (marketbook.Runners.Count == 1)
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
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
                        List<UserBetsforSuper> marketbooknames = new List<UserBetsforSuper>();
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
                                    decimal superrate = Convert.ToDecimal(lstuserbet[0].SuperAgentRateB);
                                    decimal superpercent = superrate - agentrate;
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
                                       // decimal adminrate = TransferAdminAmount == false ? 100 - superpercent - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (superpercent / 100) * profitorloss;
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new List<LiabalitybyMarket>();
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
            }
            return lstDebitCredit;
            }

        public long GetLiabalityofSamiadmin(List<UserBetsforSamiAdmin> lstUserBet)
        {
            try
            {
                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;
                //  List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsForAdmin());
                List<UserBetsforSamiAdmin> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
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
                        List<UserBetsforSamiAdmin> marketbooknames = new List<UserBetsforSamiAdmin>();
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
                                List<UserBetsforSamiAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossSamiadmin(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();

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
                            if (objMarketbook.Runners.Count == 1)
                            {
                                long ProfitandLoss = 0;


                                MarketBook CurrentMarketProfitandloss = GetBookPosition(objMarketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(),new List<UserBetsforSuper>(), lstUserBets, new List<UserBets>());

                                if (CurrentMarketProfitandloss.Runners != null)
                                {
                                    ProfitandLoss = CurrentMarketProfitandloss.Runners.Min(t => t.ProfitandLoss);
                                }
                                objMarketbook.Runners[0].ProfitandLoss = ProfitandLoss;
                            }
                            else
                            {
                                foreach (var userid in lstUsers)
                                {
                                    List<UserBetsforSamiAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                    objMarketbook.DebitCredit = ceckProfitandLossSamiadmin(objMarketbook, lstuserbet);
                                    if (lstuserbet.Count > 0)
                                    {
                                        lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                                        decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                        bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                                        foreach (var runner in objMarketbook.Runners)
                                        {
                                            long profitorloss = Convert.ToInt64(objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - objMarketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                                            if (objMarketbook.Runners.Count == 1)
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
                            //if (marketbook.Runners.Count == 1)
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return 0;
            }

        }

        public long GetLiabalityofsamiadmin(List<UserBetsforSamiAdmin> lstUserBet, MarketBook marketbook)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;

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
                    if (marketbook.Runners.Count == 1)
                    {
                        long ProfitandLoss = 0;


                        MarketBook CurrentMarketProfitandloss = GetBookPosition(marketbook.MarketId, new List<UserBetsForAdmin>(), new List<UserBetsforAgent>(), new List<UserBetsforSuper>(), lstUserBet, new List<UserBets>());

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
                        foreach (var runner in marketbook.Runners)
                        {
                            //if (marketbook.Runners.Count == 1)
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return 0;
            }

        }

        public List<LiabalitybyMarket> GetLiabalityofSamiadminbyMarkets(List<UserBetsforSamiAdmin> lstUserBet)
        {
            try
            {


                long OverAllLiabality = 0;
                long LastProfitandLoss = 0;
                //  List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsForAdmin());
                List<UserBetsforSamiAdmin> lstUserBets = lstUserBet.Where(item => item.isMatched == true).ToList();
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
                        List<UserBetsforSamiAdmin> marketbooknames = new List<UserBetsforSamiAdmin>();
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
                                List<UserBetsforSamiAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
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
                                List<UserBetsforSamiAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID) && item2.MarketBookID == objMarketbook.MarketId).ToList();
                                objMarketbook.DebitCredit = ceckProfitandLossSamiadmin(objMarketbook, lstuserbet);
                                if (lstuserbet.Count > 0)
                                {
                                    decimal agentrate = Convert.ToDecimal(lstuserbet[0].AgentRate);
                                    decimal superrate = Convert.ToDecimal(lstuserbet[0].SuperAgentRateB);
                                    decimal samiadminrate = Convert.ToDecimal(lstuserbet[0].SamiAdminRate);
                                    decimal superpercent = superrate - agentrate;
                                    decimal samiadminpercent = samiadminrate - (superpercent + agentrate);
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
                                       // decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                                        decimal profit = (samiadminpercent / 100) * profitorloss;
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
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return new List<LiabalitybyMarket>();
            }

        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossSamiadmin(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBetsforSamiAdmin> lstUserBets)
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

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossSamiadminFig(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBetsforSamiAdmin> lstUserBets)
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
                decimal samiadminpercent = samiadminrate -(superpercent + agentrate);

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
            }
            return lstDebitCredit;
        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLoss(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBets> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);
            if (lstUserbetsbyMarketID != null)
            {
                
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
            else
            {
                return lstDebitCredit;
            }


        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossfancy(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBets> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();

            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.SelectionName== marketbookstatus.MarketId).ToList();
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
               


            //    if (marketbookstatus.Runners[0].Handicap < 0)
            //    {
            //        foreach (var userbet in lstUserbetsbyMarketID)
            //        {

            //            var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.BetSize)/100) - Convert.ToDecimal(userbet.Amount);
            //            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
            //            if (userbet.BetType == "back")
            //            {
            //                double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
            //                objDebitCredit.SelectionID = userbet.SelectionID;
            //                objDebitCredit.Debit = totamount;
            //                objDebitCredit.Credit = 0;
            //                lstDebitCredit.Add(objDebitCredit);
            //                foreach (var runneritem in marketbookstatus.Runners)
            //                {
            //                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
            //                    {
            //                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
            //                        objDebitCredit.SelectionID = runneritem.SelectionId;
            //                        objDebitCredit.Debit = totamount;
            //                        objDebitCredit.Credit = 0;
            //                        lstDebitCredit.Add(objDebitCredit);
            //                    }
            //                }
            //                foreach (var runneritem in marketbookstatus.Runners)
            //                {
            //                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
            //                    {
            //                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
            //                        objDebitCredit.SelectionID = runneritem.SelectionId;
            //                        objDebitCredit.Debit = 0;
            //                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
            //                        lstDebitCredit.Add(objDebitCredit);
            //                    }
            //                }

            //            }
            //            else
            //            {
            //                double handicap = marketbookstatus.Runners.Where(item => item.SelectionId == userbet.SelectionID).Select(item => item.Handicap).First().Value;
            //                objDebitCredit.SelectionID = userbet.SelectionID;
            //                objDebitCredit.Debit = 0;
            //                objDebitCredit.Credit = totamount;
            //                lstDebitCredit.Add(objDebitCredit);
            //                foreach (var runneritem in marketbookstatus.Runners)
            //                {
            //                    if (runneritem.Handicap < handicap && runneritem.SelectionId != userbet.SelectionID)
            //                    {
            //                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
            //                        objDebitCredit.SelectionID = runneritem.SelectionId;
            //                        objDebitCredit.Debit = 0;
            //                        objDebitCredit.Credit = totamount;
            //                        lstDebitCredit.Add(objDebitCredit);
            //                    }
            //                }
            //                foreach (var runneritem in marketbookstatus.Runners)
            //                {
            //                    if (runneritem.Handicap > handicap && runneritem.SelectionId != userbet.SelectionID)
            //                    {
            //                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
            //                        objDebitCredit.SelectionID = runneritem.SelectionId;
            //                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
            //                        objDebitCredit.Credit = 0;
            //                        lstDebitCredit.Add(objDebitCredit);
            //                    }
            //                }


            //            }

                      

            //        }
            //        return lstDebitCredit;
            //    }
            //    else
            //    {
                   
            //        foreach (var userbet in lstUserbetsbyMarketID)
            //        {

            //            var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.BetSize))/100;
            //            var objDebitCredit = new ExternalAPI.TO.DebitCredit();
            //            if (userbet.BetType == "back")
            //            {

            //                objDebitCredit.SelectionID = userbet.SelectionID;
            //                objDebitCredit.Debit = totamount;
            //                objDebitCredit.Credit = 0;
            //                lstDebitCredit.Add(objDebitCredit);
            //                foreach (var runneritem in marketbookstatus.Runners)
            //                {
            //                    if (runneritem.SelectionId != userbet.SelectionID)
            //                    {
            //                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
            //                        objDebitCredit.SelectionID = runneritem.SelectionId;
            //                        objDebitCredit.Debit = 0;
            //                        objDebitCredit.Credit = Convert.ToDecimal(userbet.Amount);
            //                        lstDebitCredit.Add(objDebitCredit);
            //                    }
            //                }

            //            }
            //            else
            //            {
            //                objDebitCredit.SelectionID = userbet.SelectionID;
            //                objDebitCredit.Debit = 0;
            //                objDebitCredit.Credit = totamount;
            //                lstDebitCredit.Add(objDebitCredit);
            //                foreach (var runneritem in marketbookstatus.Runners)
            //                {
            //                    if (runneritem.SelectionId != userbet.SelectionID)
            //                    {
            //                        objDebitCredit = new ExternalAPI.TO.DebitCredit();
            //                        objDebitCredit.SelectionID = runneritem.SelectionId;
            //                        objDebitCredit.Debit = Convert.ToDecimal(userbet.Amount);
            //                        objDebitCredit.Credit = 0;
            //                        lstDebitCredit.Add(objDebitCredit);
            //                    }
            //                }

            //            }

            //        }
            //        return lstDebitCredit;
            //    }

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
            if (lstUserbetsbyMarketID != null)
            {
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
            else
            {
                return lstDebitCredit;
            }
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

            }
            return lstDebitCredit;
            }      

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossAdmin(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBetsForAdmin> lstUserBets)
        {
            try
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
            catch (System.Exception ex)
            {
                return new List<ExternalAPI.TO.DebitCredit>();

            }
        }

        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLossAdminFig(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBetsForAdmin> lstUserBets)
        {
            try
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
                        decimal totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.BetSize) / 100);
                      //  var totamount = (Convert.ToDecimal(userbet.Amount) * Convert.ToDecimal(userbet.UserOdd));
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
            catch (System.Exception ex)
            {
                return new List<ExternalAPI.TO.DebitCredit>();

            }
        }


        public MarketBook GetBookPosition(string marketBookID, List<UserBetsForAdmin> CurrentAdminBets, List<UserBetsforAgent> CurrentAgentBets, List<UserBetsforSuper> CurrentSuperBets, List<UserBetsforSamiAdmin> CurrentSamiadminrBets, List<UserBets> CurrentUserBets)
        {

            ExternalAPI.TO.MarketBook objmarketbook = new ExternalAPI.TO.MarketBook();
            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {

                List<bftradeline.Models.UserBetsForAdmin> lstCurrentBetsAdmin = CurrentAdminBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
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
                        bool TransferAdinAmount = lstCurrentBetsbyUser[0].TransferAdmin;
                        foreach (var userbet in lstCurrentBetsbyUser)
                        {

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
                if (LoggedinUserDetail.GetUserTypeID() == 2)
                {
                    List<bftradeline.Models.UserBetsforAgent> lstCurrentBetsAdmin = CurrentAgentBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
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
                              
                                decimal totamount = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(userbet.AgentOwnBets, userbet.TransferAdmin, userbet.TransferAgentIDB, userbet.CreatedbyID, Convert.ToDecimal(userbet.Amount), Convert.ToDecimal(userbet.AgentRate), userbet.TransferAdminPercentage);
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

                        List<bftradeline.Models.UserBets> lstCurrentBets = CurrentUserBets.Where(item => item.MarketBookID == marketBookID && item.isMatched == true).ToList();
                        if (lstCurrentBets.Count > 0)
                        {
                            lstCurrentBets = lstCurrentBets.OrderBy(item => Convert.ToDouble(item.UserOdd)).ToList();

                            objmarketbook.MarketId = marketBookID;
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
            return objmarketbook;
        }

    }

}