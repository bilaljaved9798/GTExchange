using bfnexchange.Services.DBModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.Text;

namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BettingServiceRest" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BettingServiceRest.svc or BettingServiceRest.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class BettingServiceRest : IBettingServiceRest
    {
      //  NExchangeEntities dbEntities = new NExchangeEntities();
        public string GetMarketDatabyIDJsonString(string sheetname, string MainSportsCategory)
        {
            try
            {
                if (MainSportsCategory == "Tennis")
                {
                    return (APIConfigforResults.JsonResultsArrTennis);
                }
                else
                {
                    if (MainSportsCategory == "Soccer")
                    {
                        return (APIConfigforResults.JsonResultsArrSoccer);
                    }
                    else
                    {
                        if (MainSportsCategory == "Horse Racing")
                        {
                            if (sheetname.Contains("To Be Placed"))
                            {
                                return (APIConfigforResults.JsonResultsArrHorseRacePlace);
                            }
                            else
                            {
                                return (APIConfigforResults.JsonResultsArrHorseRaceWin);
                            }

                        }
                        else
                        {
                            if (MainSportsCategory == "Greyhound Racing")
                            {
                                if (sheetname.Contains("To Be Placed"))
                                {
                                    return (APIConfigforResults.JsonResultsArrGrayHoundPlace);
                                }
                                else
                                {
                                    return (APIConfigforResults.JsonResultsArrGrayHoundWin);
                                }

                            }
                            else
                            {
                                if (MainSportsCategory == "Cricket")
                                {
                                    if (sheetname.Contains("Match Odds"))
                                    {
                                        return (APIConfigforResults.JsonResultsArrCricketMatchOdds);
                                    }
                                    else
                                    {

                                        if (sheetname.Contains("Completed Match") || sheetname.Contains("Tied Match"))
                                        {
                                            return (APIConfigforResults.JsonResultsArrCricketCompletedMatch);
                                        }
                                        else
                                        {
                                            if (sheetname.Contains("Winner"))
                                            {
                                                return (APIConfigforResults.JsonResultsArrWinner);
                                            }
                                            else
                                            {
                                                return (APIConfigforResults.JsonResultsArrCricketInningsRuns);
                                            }

                                        }

                                    }

                                }
                                else
                                {
                                    return "";
                                }
                            }
                        }
                    }
                }






            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);

                return "";
            }
        }

        public string GetUserbetsForAdmin()
        {
            using(NExchangeEntities dbEntities=new NExchangeEntities())
            {
                var results = dbEntities.SP_UserBets_GetDataForAdmin().ToList<SP_UserBets_GetDataForAdmin_Result>();
                return ConverttoJSONString(results);
            }
         
        }
        public string GetUserbetsbyUserID(int UserID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            using (NExchangeEntities dbEntities = new NExchangeEntities())
            {
                var results = dbEntities.SP_UserBets_GetDatabyUserID(UserID).ToList<SP_UserBets_GetDatabyUserID_Result>();
                return ConverttoJSONString(results);
            }
                
        }
        public string GetUserBetsbyAgentIDNew(int AgentID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            using (NExchangeEntities dbEntities = new NExchangeEntities())
            {
                var results = dbEntities.SP_UserBets_GetDatabyAgentID(AgentID).ToList<SP_UserBets_GetDatabyAgentID_Result>();
                return ConverttoJSONString(results);
            }
        }
        public string GetUserBetsbyAgentIDwithZeroReferer(int AgentID)
        {
            using (NExchangeEntities dbEntities = new NExchangeEntities())
            {
                var results = dbEntities.SP_UserBets_GetDatabyAgentIDWithRefererZero(AgentID).ToList<SP_UserBets_GetDatabyAgentIDWithRefererZero_Result>();
                return ConverttoJSONString(results);
            }
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
        public string ConverttoJSONString(object result)
        {
            if (result != null)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(result.GetType());
                MemoryStream memoryStream = new MemoryStream();
                serializer.WriteObject(memoryStream, result);

                // Return the results serialized as JSON
                string json = Encoding.Default.GetString(memoryStream.ToArray());
                return json;
            }
            else
            {
                return "";
            }
        }

        public string GetAllMarketsBP(string marketID)
        {
            string[] marketIDs = marketID.Split(',');
            List<SampleResponse1> lstClientMarkes = new List<SampleResponse1>();
            if (APIConfig.GetCricketDataFrom == "BP")

            {
                if (APIConfig.BFMarketBooks != null)
                {
                    // var admins = APIConfig.BFMarketBooks.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
                    foreach (var marketitem in marketIDs)
                    {
                        var currmarketbook = APIConfig.BFMarketBooks.ToList().Where(item => item.MarketId == marketitem).FirstOrDefault();
                        if (currmarketbook != null)
                        {
                            lstClientMarkes.Add(currmarketbook);
                        }

                    }
                }
            }

            return ConverttoJSONString(lstClientMarkes);
        }

        //public string GetAllMarketsBPFancy(string marketID)
        //{
        //    string[] marketIDs = marketID.Split(',');
        //    List<SampleResponse1> lstClientMarkes = new List<SampleResponse1>();
        //    if (APIConfig.GetCricketDataFrom == "BP")

        //    {
        //        if (APIConfigforResults.BFMarketBooksFancy != null)
        //        {
        //            // var admins = APIConfig.BFMarketBooks.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
        //            foreach (var marketitem in marketIDs)
        //            {
        //                var currmarketbook = APIConfigforResults.BFMarketBooksFancy.ToList().Where(item => item.MarketId == marketitem).FirstOrDefault();
        //                if (currmarketbook != null)
        //                {



        //                    lstClientMarkes.Add(currmarketbook);
        //                }

        //            }

        //        }
        //    }
        //    return ConverttoJSONString(lstClientMarkes);
        //}

        public ExternalAPI.TO.Root GetAllMarketsBPFancy(string marketID)
        {
            var marketbooks = new ExternalAPI.TO.Root();
            try
            {

                if (APIConfig.Listcricketupdate != null)
                {
                    var currmarketbook = APIConfig.Listcricketupdate.ToList().Where(item => item.Result.Room == Convert.ToInt32(marketID)).FirstOrDefault();

                    marketbooks = currmarketbook;

                }
                return marketbooks;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                var marketbook = new ExternalAPI.TO.Root();
                return marketbook;
            }
        }

        public ExternalAPI.TO.UpdateResult GetAllMarketsBPFancyNew(string marketID)
        {
            var marketbooks = new ExternalAPI.TO.UpdateResult();
            try
            {

                if (APIConfig.Listcricketupdate != null)
                {
                    var currmarketbook = APIConfig.ListcricketupdateNew.ToList().Where(item => item.Result.Room == Convert.ToInt32(marketID)).FirstOrDefault();

                    marketbooks = currmarketbook;

                }
                return marketbooks;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                var marketbook = new ExternalAPI.TO.UpdateResult();
                return marketbook;
            }
        }

        public List<ExternalAPI.TO.MarketBookString> GetAllMarketsOthers(string marketID)
        {

            string[] marketIDs = marketID.Split(',');
            List<ExternalAPI.TO.MarketBookString> lstClientMarkes = new List<ExternalAPI.TO.MarketBookString>();
            if (APIConfig.GetCricketDataFrom == "Other")

            {
                if (APIConfig.BFMarketBooksOther123 != null)
                {
                    //  var admins = APIConfig.BFMarketBooksOther.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
                    try
                    {
                        foreach (var marketitem in marketIDs)
                        {
                            var marketlist = APIConfig.BFMarketBooksOther123.ToList();
                            var currmarketbook = marketlist.Where(item => item.MarketBookId == marketitem).FirstOrDefault();
                            if (currmarketbook != null)
                            {

                                lstClientMarkes.Add(currmarketbook);
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        APIConfig.WriteErrorToDB("Get Data from rest " + ex.Message.ToString());
                    }
                }
            }
            return (lstClientMarkes);
        }
        //public List<ExternalAPI.TO.MarketBookString> GetAllMarketsOthersFancy(string marketID)
        //{
        //    string[] marketIDs = marketID.Split(',');
        //    List<ExternalAPI.TO.MarketBookString> lstClientMarkes = new List<ExternalAPI.TO.MarketBookString>();
        //    if (APIConfig.GetCricketDataFrom == "Other")

        //    {
        //        if (APIConfig.BFMarketBooksOther123 != null)
        //        {
        //            //  var admins = APIConfig.BFMarketBooksOther.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
        //            try
        //            {
        //                foreach (var marketitem in marketIDs)
        //                {
        //                    var marketlist = APIConfig.BFMarketBooksOther123.ToList();
        //                    var currmarketbook = marketlist.Where(item => item.MarketBookId == marketitem).FirstOrDefault();
        //                    if (currmarketbook != null)
        //                    {
        //                        lstClientMarkes.Add(currmarketbook);
        //                    }
        //                    else
        //                    {

        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //                APIConfig.WriteErrorToDB("Get Data from rest fancy " + ex.Message.ToString());
        //            }

        //        }
        //    }
        //    return (lstClientMarkes);
        //}


        public List<ExternalAPI.TO.MarketBook> GetAllMarkets(string marketID)
        {
            try
            {
                string[] marketIDs = marketID.Split(',');
                List<ExternalAPI.TO.MarketBook> lstClientMarkes = new List<ExternalAPI.TO.MarketBook>();

                if (APIConfig.LiveCricketMarketBooks != null)
                {
                    // var admins = APIConfig.BFMarketBooks.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
                    foreach (var marketitem in marketIDs)
                    {
                        var currmarketbook = APIConfig.LiveCricketMarketBooks.Where(item => item.MarketId == marketitem).FirstOrDefault();
                        if (currmarketbook != null)
                        {
                            lstClientMarkes.Add(currmarketbook);
                        }

                    }

                }

                return (lstClientMarkes);
            }
            catch (System.Exception ex)
            {
                return new List<ExternalAPI.TO.MarketBook>();

            }
        }
        public List<ExternalAPI.TO.MarketBookString> GetAllMarketsOthersFancy(string marketID)
        {
            string[] marketIDs = marketID.Split(',');
            List<ExternalAPI.TO.MarketBookString> lstClientMarkes = new List<ExternalAPI.TO.MarketBookString>();
            if (APIConfig.GetCricketDataFrom == "Other")

            {
                if (APIConfig.BFMarketBooksOther123 != null)
                {
                    //  var admins = APIConfig.BFMarketBooksOther.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
                    try
                    {
                        foreach (var marketitem in marketIDs)
                        {
                            var marketlist = APIConfig.BFMarketBooksOther123.ToList();
                            var currmarketbook = marketlist.Where(item => item.MarketBookId == marketitem).FirstOrDefault();
                            if (currmarketbook != null)
                            {
                                lstClientMarkes.Add(currmarketbook);
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        APIConfig.WriteErrorToDB("Get Data from rest fancy " + ex.Message.ToString());
                    }

                }
            }
            return (lstClientMarkes);
        }

        public ExternalAPI.TO.GetDataFancy GetAllMarketsOthersFancyIN(string marketID)
        {
            
            ExternalAPI.TO.GetDataFancy lstClientMarkes = new ExternalAPI.TO.GetDataFancy();

            if (APIConfig.MarketBookForindianFancy != null)
            {

                var currmarketbook = APIConfig.MarketBookForindianFancy.ToList().Where(item => item.MarketID == marketID).FirstOrDefault();
                lstClientMarkes = currmarketbook;
            }
            return (lstClientMarkes);
        }
        public List<ExternalAPI.TO.MarketBook> GetAllMarketsFancy(string marketID)
        {
            string[] marketIDs = marketID.Split(',');
            List<ExternalAPI.TO.MarketBook> lstClientMarkes = new List<ExternalAPI.TO.MarketBook>();

            if (APIConfig.LiveCricketMarketBooksFancy != null)
            {
                // var admins = APIConfig.BFMarketBooks.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
                foreach (var marketitem in marketIDs)
                {
                    var currmarketbook = APIConfig.LiveCricketMarketBooksFancy.Where(item => item.MarketId == marketitem).FirstOrDefault();
                    if (currmarketbook != null)
                    {



                        lstClientMarkes.Add(currmarketbook);
                    }

                }

            }

            return (lstClientMarkes);
        }

        public string GetAllMarketsFancyIN(string marketID)
        {
            ExternalAPI.TO.GetDataFancy marketbooks = new ExternalAPI.TO.GetDataFancy();
            try
            {

                if (APIConfig.MarketBookForindianFancy != null)
                {

                    var currmarketbook = APIConfig.MarketBookForindianFancy.ToList().Where(item => item.MarketID == marketID).FirstOrDefault();
                    marketbooks = currmarketbook;

                }
                string s = JsonConvert.SerializeObject(marketbooks);
                return s;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                var marketbook = new ExternalAPI.TO.GetDataFancy();
                return marketbook.ToString();
            }
           
        }

        public string GetAllMarketsString(string marketID)
        {
            try
            {


                string[] marketIDs = marketID.Split(',');
                List<ExternalAPI.TO.MarketBook> lstClientMarkes = new List<ExternalAPI.TO.MarketBook>();

                if (APIConfig.LiveCricketMarketBooks != null)
                {
                    // var admins = APIConfig.BFMarketBooks.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
                    foreach (var marketitem in marketIDs)
                    {
                        var currmarketbook = APIConfig.LiveCricketMarketBooks.Where(item => item.MarketId == marketitem).FirstOrDefault();
                        if (currmarketbook != null)
                        {
                            lstClientMarkes.Add(currmarketbook);
                        }

                    }

                }

                return ConverttoJSONString(lstClientMarkes);
            }
            catch (System.Exception ex)
            {
                return "";

            }
        }

        public string GetAllMarketsFancyString(string marketID)
        {
            string[] marketIDs = marketID.Split(',');
            List<ExternalAPI.TO.MarketBook> lstClientMarkes = new List<ExternalAPI.TO.MarketBook>();

            if (APIConfig.LiveCricketMarketBooksFancy != null)
            {
                // var admins = APIConfig.BFMarketBooks.Where(admin => string.Join(",", marketIDs).Split(',').Contains(admin.MarketId));
                foreach (var marketitem in marketIDs)
                {
                    var currmarketbook = APIConfig.LiveCricketMarketBooksFancy.Where(item => item.MarketId == marketitem).FirstOrDefault();
                    if (currmarketbook != null)
                    {



                        lstClientMarkes.Add(currmarketbook);
                    }

                }

            }

            return ConverttoJSONString(lstClientMarkes);
        }


        public string GetScoresbyEventIDandDate(string EventId)
        {
            return "";
            //var results = dbEntities.SP_BallbyBallSummary_GetScoresbyEventIDbyInnings(EventId, DateTime.Now, 1).ToList();
            //return ConverttoJSONString(results);

        }
        public string GetOddsDatabyType(string oddtype)
        {
            return "";
            //return dbEntities.SP_OddsData_GetData(oddtype).FirstOrDefault();
        }

    }
}
