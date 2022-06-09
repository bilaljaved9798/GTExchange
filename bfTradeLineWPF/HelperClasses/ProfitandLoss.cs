
using globaltraders.Service123Reference;
using bftradeline.Models;
using globaltraders.UserServiceReference;
using ExternalAPI.TO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using globaltraders;

namespace bftradeline.HelperClasses
{
    public class ProfitandLoss
    {
      
      
        public List<MarketBook> CalculateProfitandLossEndUser(ExternalAPI.TO.MarketBook marketbookold, List<UserBets> lstUserBet)
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            var marketbook = marketbookold;
            //foreach (ExternalAPI.TO.Runner objrunner in marketbook.Runners)
            //{
            //    objrunner.ProfitandLoss = 0;
            //    objrunner.Loss = 0;
            //}
            List<UserBets> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook.MarketId).ToList();
            marketbook.DebitCredit = objUserBets.ceckProfitandLoss(marketbook, lstUserBets);
            if (marketbook.MarketBookName.Contains("To Be Placed"))
            {
                foreach (var runner in marketbook.Runners)
                {
                    runner.ProfitandLoss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                    runner.Loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));

                }
            }
            else
            {
                foreach (var runner in marketbook.Runners)
                {
                    runner.ProfitandLoss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                   if (marketbook.Runners.Count == 1)
                    {
                        if (runner.ProfitandLoss > 0)
                        {
                            runner.ProfitandLoss = -1 * runner.ProfitandLoss;
                        }
                   }
                }
            }
            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);
        }

        
        public List<MarketBook> CalculateProfitandLossEndUserFig(ExternalAPI.TO.MarketBook marketbookold, List<UserBets> lstUserBet)
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            var marketbook = marketbookold;
          
            List<UserBets> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook.MarketId).ToList();
            marketbook.DebitCredit = objUserBets.ceckProfitandLossFig(marketbook, lstUserBets);
                    
                foreach (var runner in marketbook.Runners)
                {
                    runner.ProfitandLoss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                    if (marketbook.Runners.Count == 1)
                    {
                        if (runner.ProfitandLoss > 0)
                        {
                            runner.ProfitandLoss = -1 * runner.ProfitandLoss;
                        }
                    }
                }
            
            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);
        }

        //public decimal GetProfitorlossbyAgentPercentageandTransferRate(int AgentsOwnBets, bool TransferAdminAmount, int TransferAgentID, int CreatedbyID, decimal profitorloss, decimal agentrate)
        //{
        //    decimal profit = 0;
        //    if (AgentsOwnBets == 1)
        //    {
        //        if (TransferAdminAmount == true)
        //        {
        //            if (CreatedbyID == TransferAgentID)
        //            {
        //                profit = (((agentrate) / 100) * profitorloss) + (((100 - (agentrate)) / 100) * profitorloss);
        //            }
        //            else
        //            {
        //                profit = (((agentrate) / 100) * profitorloss);
        //            }
        //        }
        //        else
        //        {
        //            profit = (((agentrate) / 100) * profitorloss);

        //        }
        //    }
        //    else
        //    {
        //        profit = (((100 - (agentrate)) / 100) * profitorloss);
        //    }
        //    return profit;
        //}
        public List<MarketBook> CalculateProfitandLossAgent(ExternalAPI.TO.MarketBook marketbookold, List<UserBetsforAgent> lstUserBet)
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();

            var marketbook = marketbookold;

            foreach (ExternalAPI.TO.Runner objrunner in marketbook.Runners)
            {
                objrunner.ProfitandLoss = 0;
                objrunner.Loss = 0;
            }
            List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook.MarketId).ToList();

            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
            foreach (var userid in lstUsers)
            {

                // var agentrate = 0;
                List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                marketbook.DebitCredit = objUserBets.ceckProfitandLossAgent(marketbook, lstuserbet);

                if (marketbook.MarketBookName.Contains("To Be Placed"))
                {
                    foreach (var runner in marketbook.Runners)
                    {
                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        long loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, loss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
                        runner.Loss += Convert.ToInt64(-1 * profit);


                    }
                }
                else
                {
                    foreach (var runner in marketbook.Runners)
                    {
                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        if (marketbook.Runners.Count == 1)
                        {
                            if (profitorloss > 0)
                            {
                                profitorloss = -1 * profitorloss;
                            }
                        }
                        decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                        //if (LoggedinUserDetail.GetUserTypeID() == 1)
                        //{
                        //    profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * profitorloss : ((Convert.ToDecimal(agentrate) / 100) * profitorloss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss);
                        //    //if (CreatedbyID == 73)
                        //    //{
                        //    //    profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * profitorloss : ((Convert.ToDecimal(agentrate) / 100) * profitorloss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss);
                        //    //}
                        //    //else
                        //    //{
                        //    //    profit = ((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss;
                        //    //}

                        //}
                        //else
                        //{
                        //    profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * profitorloss : ((Convert.ToDecimal(agentrate) / 100) * profitorloss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss);
                        //    //profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
                        //}



                    }
                }


            }
            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);
        }


        public List<MarketBook> CalculateProfitandLossAgentFig(ExternalAPI.TO.MarketBook marketbookold, List<UserBetsforAgent> lstUserBet)
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();

            var marketbook = marketbookold;

            foreach (ExternalAPI.TO.Runner objrunner in marketbook.Runners)
            {
                objrunner.ProfitandLoss = 0;
                objrunner.Loss = 0;
            }
            List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook.MarketId).ToList();
            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
            foreach (var userid in lstUsers)
            {
                // var agentrate = 0;
                List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                marketbook.DebitCredit = objUserBets.ceckProfitandLossAgentFig(marketbook, lstuserbet);
                        
                    foreach (var runner in marketbook.Runners)
                    {
                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        if (marketbook.Runners.Count == 1)
                        {
                            if (profitorloss > 0)
                            {
                                profitorloss = -1 * profitorloss;
                            }
                        }
                    // decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
                    runner.ProfitandLoss += profitorloss; //Convert.ToInt64(-1 * profit);                     
                    }               
            }
            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);
        }

        //public List<MarketBook> CalculateProfitandLossAgent(ExternalAPI.TO.MarketBook marketbookold, List<UserBetsforAgent> lstUserBet)
        //{
        //    UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
        //    var marketbook = marketbookold;
        //    foreach (ExternalAPI.TO.Runner objrunner in marketbook.Runners)
        //    {
        //        objrunner.ProfitandLoss = 0;
        //        objrunner.Loss = 0;
        //    }
        //    List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook.MarketId).ToList();

        //    var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
        //    foreach (var userid in lstUsers)
        //    {

        //        // var agentrate = 0;
        //        List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
        //        var agentrate = lstuserbet[0].AgentRate;
        //        bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
        //        int CreatedbyID = lstuserbet[0].CreatedbyID;
        //        marketbook.DebitCredit = objUserBets.ceckProfitandLossAgent(marketbook, lstuserbet);

        //        if (marketbook.MarketBookName.Contains("To Be Placed"))
        //        {
        //            foreach (var runner in marketbook.Runners)
        //            {
        //                long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
        //                long loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
        //                decimal profit = 0;
        //                if (LoggedinUserDetail.GetUserID() == 73 || LoggedinUserDetail.GetUserTypeID()==1) {
        //                    if (CreatedbyID == 73)
        //                    {
        //                        profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * profitorloss : ((Convert.ToDecimal(agentrate) / 100) * profitorloss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss);
        //                    }
        //                    else
        //                    {
        //                        profit = ((100- Convert.ToDecimal(agentrate)) / 100) * profitorloss;
        //                    }

        //                }
        //                else
        //                {
        //                    profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
        //                }
        //                runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

        //                if (LoggedinUserDetail.GetUserID() == 73 || LoggedinUserDetail.GetUserTypeID() == 1)
        //                {
        //                    if (CreatedbyID == 73)
        //                    {
        //                        profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * loss : ((Convert.ToDecimal(agentrate) / 100) * loss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * loss);
        //                    }
        //                    else
        //                    {
        //                        profit = ( (100-Convert.ToDecimal(agentrate)) / 100) * loss;
        //                    }


        //                }
        //                else
        //                {
        //                    profit = (Convert.ToDecimal(agentrate) / 100) * loss;
        //                }
        //              //  profit = (Convert.ToDecimal(agentrate) / 100) * loss;
        //                runner.Loss += Convert.ToInt64(-1 * profit);


        //            }
        //        }
        //        else
        //        {
        //            foreach (var runner in marketbook.Runners)
        //            {
        //                long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
        //                if (marketbook.Runners.Count == 1)
        //                {
        //                    if (profitorloss > 0)
        //                    {
        //                        profitorloss = -1 * profitorloss;
        //                    }
        //                }
        //                decimal profit = 0;
        //                if (LoggedinUserDetail.GetUserID() == 73 || LoggedinUserDetail.GetUserTypeID() == 1)
        //                {
        //                    if (CreatedbyID == 73)
        //                    {
        //                        profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * profitorloss : ((Convert.ToDecimal(agentrate) / 100) * profitorloss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss);
        //                    }
        //                    else
        //                    {
        //                        profit = ((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss;
        //                    }

        //                }
        //                else
        //                {
        //                    profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
        //                }

        //                runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

        //            }
        //        }



        //    }
        //    var marketbooks = new List<MarketBook>();
        //    marketbooks.Add(marketbook);
        //    return (marketbooks);
        //}
        public List<MarketBook> CalculateProfitandLossAdmin(ExternalAPI.TO.MarketBook marketbookold, List<UserBetsForAdmin> lstUserBet)
        {
                  UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
                  var marketbook = marketbookold;
                  foreach (ExternalAPI.TO.Runner objrunner in marketbook.Runners)
            {
                  objrunner.ProfitandLoss = 0;
                  objrunner.Loss = 0;
            }
            //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
            List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook.MarketId).ToList();

            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
            foreach (var userid in lstUsers)
            {

                // var agentrate = 0;

                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                var agentrate = lstuserbet[0].AgentRate;
                var superrate = lstuserbet[0].SuperAgentRateB;
                var samiadmin = lstuserbet[0].samiadminrate;
                bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                //var superpercent = superrate - Convert.ToInt32(agentrate);
                marketbook.DebitCredit = objUserBets.ceckProfitandLossAdmin(marketbook, lstuserbet);
                var superpercent = 0;
                if (superrate > 0)
                {
                    superpercent = superrate - Convert.ToInt32(agentrate);
                }
                else
                {
                    superpercent = 0;
                }
                var samiadminpercent = 0;
                if(samiadmin>0)
                {
                    samiadminpercent =Convert.ToInt32( samiadmin) - (superpercent + Convert.ToInt32(agentrate));
                }
                else
                {
                    samiadminpercent = 0;
                }

                if (marketbook.MarketBookName.Contains("To Be Placed"))
                {
                    foreach (var runner in marketbook.Runners)
                    {
                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        long loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate)- Convert.ToDecimal(superpercent): 0;
                        decimal profit = (adminrate / 100) * profitorloss;
                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        profit = (adminrate / 100) * loss;
                        runner.Loss += Convert.ToInt64(-1 * profit);
                    }
                }
                else
                {
                    foreach (var runner in marketbook.Runners)
                    {
                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        if (marketbook.Runners.Count == 1)
                        {
                            if (profitorloss > 0)
                            {
                                profitorloss = -1 * profitorloss;
                            }
                        }
                 
                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate)- Convert.ToDecimal(superpercent)- Convert.ToDecimal(samiadminpercent)  - Convert.ToDecimal(TransferAdminPercentage);
                        decimal profit = (adminrate / 100) * profitorloss;
                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                    }
                }




            }

            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);
            // return RenderRazorViewToString("MarketBookSelected", marketbooks);




        }

        public List<MarketBook> CalculateProfitandLossAdminFig(ExternalAPI.TO.MarketBook marketbookold, List<UserBetsForAdmin> lstUserBet)
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            var marketbook = marketbookold;
            foreach (ExternalAPI.TO.Runner objrunner in marketbook.Runners)
            {
                objrunner.ProfitandLoss = 0;
                objrunner.Loss = 0;
            }
            List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook.MarketId).ToList();

            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
            foreach (var userid in lstUsers)
            {

                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                var agentrate = lstuserbet[0].AgentRate;
                var superrate = lstuserbet[0].SuperAgentRateB;
                bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                //var superpercent = superrate - Convert.ToInt32(agentrate);
                marketbook.DebitCredit = objUserBets.ceckProfitandLossAdmin(marketbook, lstuserbet);
                var superpercent = 0;
                if (superrate > 0)
                {
                    superpercent = superrate - Convert.ToInt32(agentrate);
                }
                else
                {
                    superpercent = 0;
                }

                marketbook.DebitCredit = objUserBets.ceckProfitandLossAdminFig(marketbook, lstuserbet);


                foreach (var runner in marketbook.Runners)
                {
                    long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                    if (marketbook.Runners.Count == 1)
                    {
                        if (profitorloss > 0)
                        {
                            profitorloss = -1 * profitorloss;
                        }
                    }

                    decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(superpercent) - Convert.ToDecimal(TransferAdminPercentage);
                    decimal profit = (adminrate / 100) * profitorloss;
                    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                }
            }

            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);
            // return RenderRazorViewToString("MarketBookSelected", marketbooks);




        }


        public List<MarketBook> CalculateProfitandLossAdminKJ(string MarketId, List<UserBetsForAdmin> lstUserBet)
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            MarketBook marketbook = new MarketBook();
            //foreach (ExternalAPI.TO.Runner objrunner in marketbook.Runners)
            //{
            //    objrunner.ProfitandLoss = 0;
            //    objrunner.Loss = 0;
            //}
            //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
            List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketId).ToList();

            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
            foreach (var userid in lstUsers)
            {

                // var agentrate = 0;

                List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                var agentrate = lstuserbet[0].AgentRate;
                var superrate = lstuserbet[0].SuperAgentRateB;
                bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                //var superpercent = superrate - Convert.ToInt32(agentrate);
                marketbook.DebitCredit = objUserBets.ceckProfitandLossAdmin(marketbook, lstuserbet);
                var superpercent = 0;
                if (superrate > 0)
                {
                    superpercent = superrate - Convert.ToInt32(agentrate);
                }
                else
                {
                    superpercent = 0;
                }

                if (marketbook.MarketBookName.Contains("To Be Placed"))
                {
                    foreach (var runner in marketbook.Runners)
                    {
                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        long loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(superpercent) : 0;
                        decimal profit = (adminrate / 100) * profitorloss;
                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        profit = (adminrate / 100) * loss;
                        runner.Loss += Convert.ToInt64(-1 * profit);


                    }
                }
                else
                {
                    foreach (var runner in marketbook.Runners)
                    {
                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        if (marketbook.Runners.Count == 1)
                        {
                            if (profitorloss > 0)
                            {
                                profitorloss = -1 * profitorloss;
                            }
                        }

                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(superpercent) - Convert.ToDecimal(TransferAdminPercentage);
                        decimal profit = (adminrate / 100) * profitorloss;
                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                    }
                }




            }

            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);
            // return RenderRazorViewToString("MarketBookSelected", marketbooks);


        }


        public List<MarketBook> CalculateProfitandLossSuper(ExternalAPI.TO.MarketBook marketbookold, List<UserBetsforSuper> lstUserBet)
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            var marketbook = marketbookold;
            foreach (ExternalAPI.TO.Runner objrunner in marketbook.Runners)
            {
                objrunner.ProfitandLoss = 0;
                objrunner.Loss = 0;
            }
            //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
            List<UserBetsforSuper> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook.MarketId).ToList();

            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
            foreach (var userid in lstUsers)
            {

                // var agentrate = 0;

                List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                var agentrate = lstuserbet[0].AgentRate;
                var supertrate = lstuserbet[0].SuperAgentRateB;
                bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                var superpercent = supertrate - Convert.ToInt32(agentrate);
                var adminpercent = 100 - (superpercent + Convert.ToInt32(agentrate) + TransferAdminPercentage);

                marketbook.DebitCredit = objUserBets.ceckProfitandLossSuper(marketbook, lstuserbet);
                if (marketbook.MarketBookName.Contains("To Be Placed"))
                {
                    foreach (var runner in marketbook.Runners)
                    {
                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        long loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        decimal superfinal = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                        decimal profit = (superfinal / 100) * profitorloss;
                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        profit = (superfinal / 100) * loss;
                        runner.Loss += Convert.ToInt64(-1 * profit);


                    }
                }
                else
                {
                    foreach (var runner in marketbook.Runners)
                    {
                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        if (marketbook.Runners.Count == 1)
                        {
                            if (profitorloss > 0)
                            {
                                profitorloss = -1 * profitorloss;
                            }
                        }

                        //decimal adminrate1 = 100 - Convert.ToDecimal(agentrate);

                        //if (TransferAdminAmount == true)
                        //{

                        //    decimal adminrate = adminrate1 - Convert.ToDecimal(TransferAdminPercentage);
                        //    decimal profit = (adminrate / 100) * profitorloss;
                        //    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                        //}
                        //else
                        //{
                        //    decimal profit = (adminrate1 / 100) * profitorloss;
                        //    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        //}

                        //decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
                        //decimal profit = (adminrate / 100) * profitorloss;
                        //runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                        decimal profit = (adminrate / 100) * profitorloss;
                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                    }
                }




            }

            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);
            // return RenderRazorViewToString("MarketBookSelected", marketbooks);




        }

        public List<MarketBook> CalculateProfitandLossSuperFig(ExternalAPI.TO.MarketBook marketbookold, List<UserBetsforSuper> lstUserBet)
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            var marketbook = marketbookold;
            foreach (ExternalAPI.TO.Runner objrunner in marketbook.Runners)
            {
                objrunner.ProfitandLoss = 0;
                objrunner.Loss = 0;
            }
            //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
            List<UserBetsforSuper> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook.MarketId).ToList();

            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
            foreach (var userid in lstUsers)
            {

                List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                var agentrate = lstuserbet[0].AgentRate;
                var supertrate = lstuserbet[0].SuperAgentRateB;
                bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                var superpercent = supertrate - Convert.ToInt32(agentrate);
                var adminpercent = 100 - (superpercent + Convert.ToInt32(agentrate) + TransferAdminPercentage);

                marketbook.DebitCredit = objUserBets.ceckProfitandLossSuperFig(marketbook, lstuserbet);
                           
                    foreach (var runner in marketbook.Runners)
                    {
                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        if (marketbook.Runners.Count == 1)
                        {
                            if (profitorloss > 0)
                            {
                                profitorloss = -1 * profitorloss;
                            }
                        }                     
                       // decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                       // decimal profit = (adminrate / 100) * profitorloss;
                    runner.ProfitandLoss += profitorloss;//Convert.ToInt64(-1 * profit);

                    }               
            }
            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);          
        }


        public List<MarketBook> CalculateProfitandLossSamiAdmin(ExternalAPI.TO.MarketBook marketbookold, List<UserBetsforSamiAdmin> lstUserBet)
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            var marketbook = marketbookold;
            foreach (ExternalAPI.TO.Runner objrunner in marketbook.Runners)
            {
                objrunner.ProfitandLoss = 0;
                objrunner.Loss = 0;
            }
            //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
            List<UserBetsforSamiAdmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook.MarketId).ToList();

            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
            foreach (var userid in lstUsers)
            {
                // var agentrate = 0;
                List<UserBetsforSamiAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                var agentrate = lstuserbet[0].AgentRate;
                var supertrate = lstuserbet[0].SuperAgentRateB;
                var samiadminrate = lstuserbet[0].SamiAdminRate;
                bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                var superpercent = supertrate - Convert.ToInt32(agentrate);
                var samiadminpercent = samiadminrate- (superpercent + Convert.ToInt32(agentrate));
                var adminpercent = 100 - (samiadminpercent + superpercent + Convert.ToInt32(agentrate) + TransferAdminPercentage);

                marketbook.DebitCredit = objUserBets.ceckProfitandLossSamiadmin(marketbook, lstuserbet);
                if (marketbook.MarketBookName.Contains("To Be Placed"))
                {
                    foreach (var runner in marketbook.Runners)
                    {
                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        long loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        decimal superfinal = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                        decimal profit = (superfinal / 100) * profitorloss;
                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
                        profit = (superfinal / 100) * loss;
                        runner.Loss += Convert.ToInt64(-1 * profit);
                    }
                }
                else
                {
                    foreach (var runner in marketbook.Runners)
                    {
                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        if (marketbook.Runners.Count == 1)
                        {
                            if (profitorloss > 0)
                            {
                                profitorloss = -1 * profitorloss;
                            }
                        }

                      
                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate)- Convert.ToDecimal(superpercent)   - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                        decimal profit = (adminrate / 100) * profitorloss;
                        runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

                    }
                }
            }

            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);
            // return RenderRazorViewToString("MarketBookSelected", marketbooks);




        }

        public List<MarketBook> CalculateProfitandLossSamiAdminFig(ExternalAPI.TO.MarketBook marketbookold, List<UserBetsforSamiAdmin> lstUserBet)
        {
            UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
            var marketbook = marketbookold;
            foreach (ExternalAPI.TO.Runner objrunner in marketbook.Runners)
            {
                objrunner.ProfitandLoss = 0;
                objrunner.Loss = 0;
            }                     
            List<UserBetsforSamiAdmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook.MarketId).ToList();

            var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
            foreach (var userid in lstUsers)
            {

                List<UserBetsforSamiAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
                lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
                var agentrate = lstuserbet[0].AgentRate;
                var supertrate = lstuserbet[0].SuperAgentRateB;
                var samiadmintrate = lstuserbet[0].SamiAdminRate;
                bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
                var superpercent = supertrate - Convert.ToInt32(agentrate);
                var samiadminpercent = samiadmintrate -(superpercent + Convert.ToInt32(agentrate));
                var adminpercent = 100 - (samiadminpercent + superpercent + Convert.ToInt32(agentrate) + TransferAdminPercentage);

                marketbook.DebitCredit = objUserBets.ceckProfitandLossSamiadminFig(marketbook, lstuserbet);

                foreach (var runner in marketbook.Runners)
                {
                    long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                    if (marketbook.Runners.Count == 1)
                    {
                        if (profitorloss > 0)
                        {
                            profitorloss = -1 * profitorloss;
                        }
                    }
                    // decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
                    // decimal profit = (adminrate / 100) * profitorloss;
                    runner.ProfitandLoss += profitorloss;//Convert.ToInt64(-1 * profit);

                }
            }
            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);
        }

    }

}

