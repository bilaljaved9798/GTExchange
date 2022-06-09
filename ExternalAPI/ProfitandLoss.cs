

using ExternalAPI.TO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBFExchange
{
   public class ProfitandLoss
    {


        public List<MarketBook> CalculateProfitandLossEndUser(ExternalAPI.TO.MarketBook marketbookold, List<UserBets> lstUserBet)
        {
          
            var marketbook = marketbookold;
            //foreach (ExternalAPI.TO.Runner objrunner in marketbook.Runners)
            //{
            //    objrunner.ProfitandLoss = 0;
            //    objrunner.Loss = 0;
            //}
          //  List<UserBets> lstUserBets = lstUserBet.Where(item2 => item2.isMatched == true && item2.MarketBookID == marketbook.MarketId).ToList();
            marketbook.DebitCredit =ceckProfitandLoss(marketbook, lstUserBet);
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
                }
            }
            var marketbooks = new List<MarketBook>();
            marketbooks.Add(marketbook);
            return (marketbooks);
        }
        public List<ExternalAPI.TO.DebitCredit> ceckProfitandLoss(ExternalAPI.TO.MarketBook marketbookstatus, List<UserBets> lstUserBets)
        {

            List<ExternalAPI.TO.DebitCredit> lstDebitCredit = new List<ExternalAPI.TO.DebitCredit>();
            var lstUserbetsbyMarketID = lstUserBets.Where(item => item.MarketBookID == marketbookstatus.MarketId);



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
    }
    
}

