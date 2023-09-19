

using ExternalAPI.TO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTWeb.Models;
using GTExchNew.Models;

namespace GTExchNew.HelperClasses
{
    //public class ProfitandLoss
    //{
      
      
    //    public List<MarketBook> CalculateProfitandLossEndUser(ExternalAPI.TO.MarketBook MarketBookold, List<UserBets> lstUserBet)
    //    {
    //        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
    //        var MarketBook = MarketBookold;
    //        //foreach (ExternalAPI.TO.Runner objrunner in MarketBook.Runners)
    //        //{
    //        //    objrunner.ProfitandLoss = 0;
    //        //    objrunner.Loss = 0;
    //        //}
    //        List<UserBets> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBook.MarketId).ToList();
    //        MarketBook.DebitCredit = objUserBets.ceckProfitandLoss(MarketBook, lstUserBets);
    //        if (MarketBook.MarketBookName.Contains("To Be Placed"))
    //        {
    //            foreach (var runner in MarketBook.Runners)
    //            {
    //                runner.ProfitandLoss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
    //                runner.Loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //            }
    //        }
    //        else
    //        {
    //            foreach (var runner in MarketBook.Runners)
    //            {
    //                runner.ProfitandLoss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //               if (MarketBook.Runners.Count == 1)
    //                {
    //                    if (runner.ProfitandLoss > 0)
    //                    {
    //                        runner.ProfitandLoss = -1 * runner.ProfitandLoss;
    //                    }
    //               }
    //            }
    //        }
    //        var MarketBooks = new List<MarketBook>();
    //        MarketBooks.Add(MarketBook);
    //        return (MarketBooks);
    //    }

        
    //    public List<MarketBook> CalculateProfitandLossEndUserFig(ExternalAPI.TO.MarketBook MarketBookold, List<UserBets> lstUserBet)
    //    {
    //        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
    //        var MarketBook = MarketBookold;
          
    //        List<UserBets> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBook.MarketId).ToList();
    //        MarketBook.DebitCredit = objUserBets.ceckProfitandLossFig(MarketBook, lstUserBets);
    //        if (MarketBook.MarketBookName.Contains("To Be Placed"))
    //        {
    //            foreach (var runner in MarketBook.Runners)
    //            {
    //                runner.ProfitandLoss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
    //                runner.Loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //            }
    //        }
    //        else
    //        {
    //            foreach (var runner in MarketBook.Runners)
    //            {
    //                runner.ProfitandLoss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //                if (MarketBook.Runners.Count == 1)
    //                {
    //                    if (runner.ProfitandLoss > 0)
    //                    {
    //                        runner.ProfitandLoss = -1 * runner.ProfitandLoss;
    //                    }
    //                }
    //            }
    //        }
    //        var MarketBooks = new List<MarketBook>();
    //        MarketBooks.Add(MarketBook);
    //        return (MarketBooks);
    //    }

    //    //public decimal GetProfitorlossbyAgentPercentageandTransferRate(int AgentsOwnBets, bool TransferAdminAmount, int TransferAgentID, int CreatedbyID, decimal profitorloss, decimal agentrate)
    //    //{
    //    //    decimal profit = 0;
    //    //    if (AgentsOwnBets == 1)
    //    //    {
    //    //        if (TransferAdminAmount == true)
    //    //        {
    //    //            if (CreatedbyID == TransferAgentID)
    //    //            {
    //    //                profit = (((agentrate) / 100) * profitorloss) + (((100 - (agentrate)) / 100) * profitorloss);
    //    //            }
    //    //            else
    //    //            {
    //    //                profit = (((agentrate) / 100) * profitorloss);
    //    //            }
    //    //        }
    //    //        else
    //    //        {
    //    //            profit = (((agentrate) / 100) * profitorloss);

    //    //        }
    //    //    }
    //    //    else
    //    //    {
    //    //        profit = (((100 - (agentrate)) / 100) * profitorloss);
    //    //    }
    //    //    return profit;
    //    //}
    //    public List<MarketBook> CalculateProfitandLossAgent(ExternalAPI.TO.MarketBook MarketBookold, List<UserBetsforAgent> lstUserBet)
    //    {
    //        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();

    //        var MarketBook = MarketBookold;

    //        foreach (ExternalAPI.TO.Runner objrunner in MarketBook.Runners)
    //        {
    //            objrunner.ProfitandLoss = 0;
    //            objrunner.Loss = 0;
    //        }
    //        List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBook.MarketId).ToList();

    //        var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
    //        foreach (var userid in lstUsers)
    //        {

    //            // var agentrate = 0;
    //            List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
    //            lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
    //            MarketBook.DebitCredit = objUserBets.ceckProfitandLossAgent(MarketBook, lstuserbet);

    //            if (MarketBook.MarketBookName.Contains("To Be Placed"))
    //            {
    //                foreach (var runner in MarketBook.Runners)
    //                {
    //                    long profitorloss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
    //                    long loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //                    decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
    //                    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
    //                    profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, loss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
    //                    runner.Loss += Convert.ToInt64(-1 * profit);


    //                }
    //            }
    //            else
    //            {
    //                foreach (var runner in MarketBook.Runners)
    //                {
    //                    long profitorloss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //                    if (MarketBook.Runners.Count == 1)
    //                    {
    //                        if (profitorloss > 0)
    //                        {
    //                            profitorloss = -1 * profitorloss;
    //                        }
    //                    }
    //                    decimal profit = LoggedinUserDetail.GetProfitorlossbyAgentPercentageandTransferRate(lstuserbet[0].AgentOwnBets, lstuserbet[0].TransferAdmin, lstuserbet[0].TransferAgentIDB, lstuserbet[0].CreatedbyID, profitorloss, Convert.ToDecimal(lstuserbet[0].AgentRate), lstuserbet[0].TransferAdminPercentage);
    //                    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

    //                    //if (LoggedinUserDetail.GetUserTypeID() == 1)
    //                    //{
    //                    //    profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * profitorloss : ((Convert.ToDecimal(agentrate) / 100) * profitorloss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss);
    //                    //    //if (CreatedbyID == 73)
    //                    //    //{
    //                    //    //    profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * profitorloss : ((Convert.ToDecimal(agentrate) / 100) * profitorloss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss);
    //                    //    //}
    //                    //    //else
    //                    //    //{
    //                    //    //    profit = ((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss;
    //                    //    //}

    //                    //}
    //                    //else
    //                    //{
    //                    //    profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * profitorloss : ((Convert.ToDecimal(agentrate) / 100) * profitorloss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss);
    //                    //    //profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
    //                    //}



    //                }
    //            }



    //        }
    //        var MarketBooks = new List<MarketBook>();
    //        MarketBooks.Add(MarketBook);
    //        return (MarketBooks);
    //    }

    //    //public List<MarketBook> CalculateProfitandLossAgent(ExternalAPI.TO.MarketBook MarketBookold, List<UserBetsforAgent> lstUserBet)
    //    //{
    //    //    UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
    //    //    var MarketBook = MarketBookold;
    //    //    foreach (ExternalAPI.TO.Runner objrunner in MarketBook.Runners)
    //    //    {
    //    //        objrunner.ProfitandLoss = 0;
    //    //        objrunner.Loss = 0;
    //    //    }
    //    //    List<UserBetsforAgent> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBook.MarketId).ToList();

    //    //    var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
    //    //    foreach (var userid in lstUsers)
    //    //    {

    //    //        // var agentrate = 0;
    //    //        List<UserBetsforAgent> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
    //    //        var agentrate = lstuserbet[0].AgentRate;
    //    //        bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
    //    //        int CreatedbyID = lstuserbet[0].CreatedbyID;
    //    //        MarketBook.DebitCredit = objUserBets.ceckProfitandLossAgent(MarketBook, lstuserbet);

    //    //        if (MarketBook.MarketBookName.Contains("To Be Placed"))
    //    //        {
    //    //            foreach (var runner in MarketBook.Runners)
    //    //            {
    //    //                long profitorloss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
    //    //                long loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //    //                decimal profit = 0;
    //    //                if (LoggedinUserDetail.GetUserID() == 73 || LoggedinUserDetail.GetUserTypeID()==1) {
    //    //                    if (CreatedbyID == 73)
    //    //                    {
    //    //                        profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * profitorloss : ((Convert.ToDecimal(agentrate) / 100) * profitorloss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss);
    //    //                    }
    //    //                    else
    //    //                    {
    //    //                        profit = ((100- Convert.ToDecimal(agentrate)) / 100) * profitorloss;
    //    //                    }

    //    //                }
    //    //                else
    //    //                {
    //    //                    profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
    //    //                }
    //    //                runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

    //    //                if (LoggedinUserDetail.GetUserID() == 73 || LoggedinUserDetail.GetUserTypeID() == 1)
    //    //                {
    //    //                    if (CreatedbyID == 73)
    //    //                    {
    //    //                        profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * loss : ((Convert.ToDecimal(agentrate) / 100) * loss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * loss);
    //    //                    }
    //    //                    else
    //    //                    {
    //    //                        profit = ( (100-Convert.ToDecimal(agentrate)) / 100) * loss;
    //    //                    }


    //    //                }
    //    //                else
    //    //                {
    //    //                    profit = (Convert.ToDecimal(agentrate) / 100) * loss;
    //    //                }
    //    //              //  profit = (Convert.ToDecimal(agentrate) / 100) * loss;
    //    //                runner.Loss += Convert.ToInt64(-1 * profit);


    //    //            }
    //    //        }
    //    //        else
    //    //        {
    //    //            foreach (var runner in MarketBook.Runners)
    //    //            {
    //    //                long profitorloss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //    //                if (MarketBook.Runners.Count == 1)
    //    //                {
    //    //                    if (profitorloss > 0)
    //    //                    {
    //    //                        profitorloss = -1 * profitorloss;
    //    //                    }
    //    //                }
    //    //                decimal profit = 0;
    //    //                if (LoggedinUserDetail.GetUserID() == 73 || LoggedinUserDetail.GetUserTypeID() == 1)
    //    //                {
    //    //                    if (CreatedbyID == 73)
    //    //                    {
    //    //                        profit = TransferAdminAmount == false ? (Convert.ToDecimal(agentrate) / 100) * profitorloss : ((Convert.ToDecimal(agentrate) / 100) * profitorloss) + (((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss);
    //    //                    }
    //    //                    else
    //    //                    {
    //    //                        profit = ((100 - Convert.ToDecimal(agentrate)) / 100) * profitorloss;
    //    //                    }

    //    //                }
    //    //                else
    //    //                {
    //    //                    profit = (Convert.ToDecimal(agentrate) / 100) * profitorloss;
    //    //                }

    //    //                runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

    //    //            }
    //    //        }



    //    //    }
    //    //    var MarketBooks = new List<MarketBook>();
    //    //    MarketBooks.Add(MarketBook);
    //    //    return (MarketBooks);
    //    //}
    //    public List<MarketBook> CalculateProfitandLossAdmin(ExternalAPI.TO.MarketBook MarketBookold, List<UserBetsForAdmin> lstUserBet)
    //    {
    //              UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
    //              var MarketBook = MarketBookold;
    //              foreach (ExternalAPI.TO.Runner objrunner in MarketBook.Runners)
    //        {
    //              objrunner.ProfitandLoss = 0;
    //              objrunner.Loss = 0;
    //        }
    //        //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
    //        List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBook.MarketId).ToList();

    //        var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
    //        foreach (var userid in lstUsers)
    //        {

    //            // var agentrate = 0;

    //            List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
    //            lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
    //            var agentrate = lstuserbet[0].AgentRate;
    //            var superrate = lstuserbet[0].SuperAgentRateB;
    //            bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
    //            var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
    //            //var superpercent = superrate - Convert.ToInt32(agentrate);
    //            MarketBook.DebitCredit = objUserBets.ceckProfitandLossAdmin(MarketBook, lstuserbet);
    //            var superpercent = 0;
    //            if (superrate > 0)
    //            {
    //                superpercent = superrate - Convert.ToInt32(agentrate);
    //            }
    //            else
    //            {
    //                superpercent = 0;
    //            }

    //            if (MarketBook.MarketBookName.Contains("To Be Placed"))
    //            {
    //                foreach (var runner in MarketBook.Runners)
    //                {
    //                    long profitorloss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
    //                    long loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //                    decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate)- Convert.ToDecimal(superpercent): 0;
    //                    decimal profit = (adminrate / 100) * profitorloss;
    //                    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
    //                    profit = (adminrate / 100) * loss;
    //                    runner.Loss += Convert.ToInt64(-1 * profit);


    //                }
    //            }
    //            else
    //            {
    //                foreach (var runner in MarketBook.Runners)
    //                {
    //                    long profitorloss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //                    if (MarketBook.Runners.Count == 1)
    //                    {
    //                        if (profitorloss > 0)
    //                        {
    //                            profitorloss = -1 * profitorloss;
    //                        }
    //                    }
                 
    //                    decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate)- Convert.ToDecimal(superpercent) - Convert.ToDecimal(TransferAdminPercentage);
    //                    decimal profit = (adminrate / 100) * profitorloss;
    //                    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

    //                }
    //            }




    //        }

    //        var MarketBooks = new List<MarketBook>();
    //        MarketBooks.Add(MarketBook);
    //        return (MarketBooks);
    //        // return RenderRazorViewToString("MarketBookSelected", MarketBooks);




    //    }

    //    public List<MarketBook> CalculateProfitandLossAdminKJ(string MarketId, List<UserBetsForAdmin> lstUserBet)
    //    {
    //        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
    //        MarketBook MarketBook = new MarketBook();
    //        //foreach (ExternalAPI.TO.Runner objrunner in MarketBook.Runners)
    //        //{
    //        //    objrunner.ProfitandLoss = 0;
    //        //    objrunner.Loss = 0;
    //        //}
    //        //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
    //        List<UserBetsForAdmin> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketId).ToList();

    //        var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
    //        foreach (var userid in lstUsers)
    //        {

    //            // var agentrate = 0;

    //            List<UserBetsForAdmin> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
    //            lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
    //            var agentrate = lstuserbet[0].AgentRate;
    //            var superrate = lstuserbet[0].SuperAgentRateB;
    //            bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
    //            var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
    //            //var superpercent = superrate - Convert.ToInt32(agentrate);
    //            MarketBook.DebitCredit = objUserBets.ceckProfitandLossAdmin(MarketBook, lstuserbet);
    //            var superpercent = 0;
    //            if (superrate > 0)
    //            {
    //                superpercent = superrate - Convert.ToInt32(agentrate);
    //            }
    //            else
    //            {
    //                superpercent = 0;
    //            }

    //            if (MarketBook.MarketBookName.Contains("To Be Placed"))
    //            {
    //                foreach (var runner in MarketBook.Runners)
    //                {
    //                    long profitorloss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
    //                    long loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //                    decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(superpercent) : 0;
    //                    decimal profit = (adminrate / 100) * profitorloss;
    //                    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
    //                    profit = (adminrate / 100) * loss;
    //                    runner.Loss += Convert.ToInt64(-1 * profit);


    //                }
    //            }
    //            else
    //            {
    //                foreach (var runner in MarketBook.Runners)
    //                {
    //                    long profitorloss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //                    if (MarketBook.Runners.Count == 1)
    //                    {
    //                        if (profitorloss > 0)
    //                        {
    //                            profitorloss = -1 * profitorloss;
    //                        }
    //                    }

    //                    decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(superpercent) - Convert.ToDecimal(TransferAdminPercentage);
    //                    decimal profit = (adminrate / 100) * profitorloss;
    //                    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

    //                }
    //            }




    //        }

    //        var MarketBooks = new List<MarketBook>();
    //        MarketBooks.Add(MarketBook);
    //        return (MarketBooks);
    //        // return RenderRazorViewToString("MarketBookSelected", MarketBooks);


    //    }


    //    public List<MarketBook> CalculateProfitandLossSuper(ExternalAPI.TO.MarketBook MarketBookold, List<UserBetsforSuper> lstUserBet)
    //    {
    //        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
    //        var MarketBook = MarketBookold;
    //        foreach (ExternalAPI.TO.Runner objrunner in MarketBook.Runners)
    //        {
    //            objrunner.ProfitandLoss = 0;
    //            objrunner.Loss = 0;
    //        }
    //        //  List<Userbets> lstUserBet = JsonConvert.DeserializeObject<List<>>(objUserServices.GetUserbetsbyUserID(LoggedinUserDetail.GetUserID()));
    //        List<UserBetsforSuper> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == MarketBook.MarketId).ToList();

    //        var lstUsers = lstUserBets.Select(item1 => new { item1.UserID }).Distinct().ToArray();
    //        foreach (var userid in lstUsers)
    //        {

    //            // var agentrate = 0;

    //            List<UserBetsforSuper> lstuserbet = lstUserBets.Where(item2 => item2.UserID == Convert.ToInt32(userid.UserID)).ToList();
    //            lstuserbet = lstuserbet.OrderBy(o => o.ID).ToList();
    //            var agentrate = lstuserbet[0].AgentRate;
    //            var supertrate = lstuserbet[0].SuperAgentRateB;
    //            bool TransferAdminAmount = lstuserbet[0].TransferAdmin;
    //            var TransferAdminPercentage = lstuserbet[0].TransferAdminPercentage;
    //            var superpercent = supertrate - Convert.ToInt32(agentrate);
    //            var adminpercent = 100 - (superpercent + Convert.ToInt32(agentrate) + TransferAdminPercentage);

    //            MarketBook.DebitCredit = objUserBets.ceckProfitandLossSuper(MarketBook, lstuserbet);
    //            if (MarketBook.MarketBookName.Contains("To Be Placed"))
    //            {
    //                foreach (var runner in MarketBook.Runners)
    //                {
    //                    long profitorloss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit));
    //                    long loss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //                    decimal superfinal = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(TransferAdminPercentage) - Convert.ToDecimal(adminpercent) : 0;
    //                    decimal profit = (superfinal / 100) * profitorloss;
    //                    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
    //                    profit = (superfinal / 100) * loss;
    //                    runner.Loss += Convert.ToInt64(-1 * profit);


    //                }
    //            }
    //            else
    //            {
    //                foreach (var runner in MarketBook.Runners)
    //                {
    //                    long profitorloss = Convert.ToInt64(MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Debit) - MarketBook.DebitCredit.Where(item2 => item2.SelectionID == runner.SelectionId).Sum(item2 => item2.Credit));
    //                    if (MarketBook.Runners.Count == 1)
    //                    {
    //                        if (profitorloss > 0)
    //                        {
    //                            profitorloss = -1 * profitorloss;
    //                        }
    //                    }

    //                    //decimal adminrate1 = 100 - Convert.ToDecimal(agentrate);

    //                    //if (TransferAdminAmount == true)
    //                    //{

    //                    //    decimal adminrate = adminrate1 - Convert.ToDecimal(TransferAdminPercentage);
    //                    //    decimal profit = (adminrate / 100) * profitorloss;
    //                    //    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

    //                    //}
    //                    //else
    //                    //{
    //                    //    decimal profit = (adminrate1 / 100) * profitorloss;
    //                    //    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
    //                    //}

    //                    //decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 0;
    //                    //decimal profit = (adminrate / 100) * profitorloss;
    //                    //runner.ProfitandLoss += Convert.ToInt64(-1 * profit);
    //                    decimal adminrate = TransferAdminAmount == false ? 100 - Convert.ToDecimal(agentrate) : 100 - Convert.ToDecimal(agentrate) - Convert.ToDecimal(adminpercent) - Convert.ToDecimal(TransferAdminPercentage);
    //                    decimal profit = (adminrate / 100) * profitorloss;
    //                    runner.ProfitandLoss += Convert.ToInt64(-1 * profit);

    //                }
    //            }




    //        }

    //        var MarketBooks = new List<MarketBook>();
    //        MarketBooks.Add(MarketBook);
    //        return (MarketBooks);
    //        // return RenderRazorViewToString("MarketBookSelected", MarketBooks);




    //    }

    //}

}

