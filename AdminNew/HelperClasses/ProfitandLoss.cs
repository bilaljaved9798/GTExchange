
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

                     


                    }
                }



            }
            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);
        }

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
                bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
                var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
              
                var superpercent = 0;
                if (superrate > 0)
                {
                    superpercent = superrate - Convert.ToInt32(agentrate);
                }
                else
                {
                    superpercent = 0;
                }

                marketbook.DebitCredit = objUserBets.ceckProfitandLossAdmin(marketbook, lstuserbet);
                if (marketbook.MarketBookName.Contains("To Be Placed"))
                {
                    foreach (var runner in marketbook.Runners)
                    {                      
                        //decimal adminrate1 = ((100 - Convert.ToDecimal(TransferAdminPercentage)) / 100);
                        //decimal adminrateafteragnetrateminus = (100 - Convert.ToDecimal(agentrate));
                        //decimal adminrateacc = adminrateafteragnetrateminus * adminrate1;

                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
                        long loss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(superpercent) - Convert.ToDecimal(TransferAdminPercentage);
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
                       
                        //decimal adminrate1 = ((100 - Convert.ToDecimal(TransferAdminPercentage)) / 100);
                        //decimal adminrateafteragnetrateminus = (100 - Convert.ToDecimal(agentrate));
                        //decimal adminrateacc = adminrateafteragnetrateminus * adminrate1;

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

                        //decimal adminrate1 = ((100 - Convert.ToDecimal(TransferAdminPercentage)) / 100);
                        //decimal adminrateafteragnetrateminus = (100 - Convert.ToDecimal(agentrate));
                        //decimal adminrateacc = adminrateafteragnetrateminus * adminrate1;

                        long profitorloss = Convert.ToInt64(marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - marketbook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
                        

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


     
    }

}

