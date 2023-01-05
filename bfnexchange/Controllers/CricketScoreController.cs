using bfnexchange.BettingServiceReference;
using bfnexchange.CricketScoreServiceReference;
using bfnexchange.UsersServiceReference;
using bftradeline.Models;
using ExternalAPI.TO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace bfnexchange.Controllers
{
    public class CricketScoreController : Controller
    {
        // GET: CricketScore
        public ActionResult Index()
        {
            return View();
        }
        Service1Client objCricketScoreClient = new Service1Client();
        UserServicesClient objUserServiceClient = new UserServicesClient();
        BettingServiceClient objBettingClient = new BettingServiceClient();
        public PartialViewResult InitializeScoreCard()
        {
            return PartialView("CricketScores");       
        }
        public PartialViewResult InitializeSoccerCard()
        {
            return PartialView("SoccerScores");
        }
        public PartialViewResult InitializeTinnesCard()
        {
            return PartialView("TinnesScores");
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
        public string UpdateScoresData(List<bftradeline.Models.MatchScores> scores)
        {
            
            if (scores.Count > 0)
            {
                bfnexchange.Services.MatchScoreCard objScoreCard = new Services.MatchScoreCard();
                string scoresonly = "";

              
                // lblMatchUpdates.SelectionAlignment = HorizontalAlignment.Center;
                if (scores.Count == 1)
                {
                    objScoreCard.TeamAScore = scores[0].TeamName + ":" + scores[0].Scores.ToString() + "/" + scores[0].Wickets.ToString() + " in " + scores[0].Overs.ToString() + " Overs";
                    objScoreCard.CurrRR ="CRR:"+ (Convert.ToDouble(scores[0].Scores) / Convert.ToDouble(scores[0].Overs)).ToString("N2");
                    if (scores[0].Overs.ToString().Contains("."))
                    {
                        string[] remainingovers = scores[0].Overs.ToString().Split('.');
                        double oversactual =Convert.ToDouble( remainingovers[0])  + (Convert.ToDouble(remainingovers[1]) / 6);
                     objScoreCard.CurrRR = "CRR:" + (Convert.ToDouble(scores[0].Scores) / Convert.ToDouble(oversactual)).ToString("N2");
                    }
                }
                else
                {
                    if (scores.Count == 2)
                    {
                        objScoreCard.TeamBScore = scores[0].TeamName + ":" + scores[0].Scores.ToString() + "/" + scores[0].Wickets.ToString() + " in " + scores[0].Overs.ToString() + " Overs";
                       
                        objScoreCard.TeamAScore = scores[1].TeamName + ":" + scores[1].Scores.ToString() + "/" + scores[1].Wickets.ToString() + " in " + scores[1].Overs.ToString() + " Overs";
                        objScoreCard.CurrRR = "CRR:" + (Convert.ToDouble(scores[1].Scores) / Convert.ToDouble(scores[1].Overs)).ToString("F2");
                        if (scores[1].Overs.ToString().Contains("."))
                        {
                            string[] remainingovers = scores[1].Overs.ToString().Split('.');
                            double oversactual = Convert.ToDouble(remainingovers[0]) + (Convert.ToDouble(remainingovers[1]) / 6);
                           
                            objScoreCard.CurrRR = "CRR:" + (Convert.ToDouble(scores[1].Scores) / Convert.ToDouble(oversactual)).ToString("N2");
                        }

                        if (scores[1].MatchType == "ODI" || scores[1].MatchType == "T20" || scores[1].MatchType == "LIMITED_OVER")
                        {

                           objScoreCard.RequireScore = scores[1].TeamName + " needs " + ((scores[0].Scores - scores[1].Scores) + 1).ToString() + " runs to win";
                            int Requirescore = (scores[0].Scores - scores[1].Scores) + 1;

                            if (scores[1].Overs.ToString().Contains("."))
                            {
                                string[] remainingovers = scores[1].Overs.ToString().Split('.');
                                string ballremain =( ((Convert.ToDouble(scores[1].TotalOvers) - 1) - Convert.ToDouble(remainingovers[0]))+ ((6-Convert.ToDouble(remainingovers[1]))/6)).ToString();
                                string ballremainstr = ((((Convert.ToDouble(scores[1].TotalOvers) - 1) - Convert.ToDouble(remainingovers[0]))*6) + (6-Convert.ToDouble(remainingovers[1]))).ToString();

                                double Requirerunrate = Requirescore / Convert.ToDouble(ballremain);
                               objScoreCard.ReqRR = " Req RR:" + Requirerunrate.ToString("N2");
                                objScoreCard.RequireScore = objScoreCard.RequireScore + " in " + ballremainstr.ToString() + " balls.";
                            }
                            else
                            {
                                double remaining = (Convert.ToDouble(scores[1].TotalOvers) - scores[1].Overs);
                                double Requirerunrate = Requirescore / remaining;
                                objScoreCard.ReqRR = " Req RR:" + Requirerunrate.ToString("N2");
                                objScoreCard.RequireScore = objScoreCard.RequireScore + " in " + (remaining * 6).ToString() + " balls.";
                            }
                        }

                    }
                    else
                    {
                        if (scores.Count == 3)
                        {
                           objScoreCard.TeamAScore = scores[0].TeamName + " " + scores[0].Scores.ToString() + " / " + scores[0].Wickets.ToString() + " in " + scores[0].Overs.ToString() + " Overs , " + scores[2].Scores.ToString() + " / " + scores[2].Wickets.ToString() + " in " + scores[2].Overs.ToString() + " Overs";
                           objScoreCard.TeamBScore = scores[1].TeamName + " " + scores[1].Scores.ToString() + " / " + scores[1].Wickets.ToString() + " in " + scores[1].Overs.ToString() + " Overs";

                         
                        }
                        else
                        {
                           objScoreCard.TeamAScore = scores[0].TeamName + " " + scores[0].Scores.ToString() + " / " + scores[0].Wickets.ToString() + " in " + scores[0].Overs.ToString() + " Overs , " + scores[2].Scores.ToString() + " / " + scores[2].Wickets.ToString() + " in " + scores[2].Overs.ToString() + " Overs";
                           objScoreCard.TeamBScore = scores[1].TeamName + " " + scores[1].Scores.ToString() + " / " + scores[1].Wickets.ToString() + " in " + scores[1].Overs.ToString() + " OVers , " + scores[3].Scores.ToString() + " / " + scores[3].Wickets.ToString() + " in " + scores[3].Overs.ToString() + " Overs";

                           

                        }
                    }


                }
                return ConverttoJSONString( objScoreCard);
               
            }
            else
            {
                return "";
            }
        }
        public string UpdateScoresDataNew(List<bftradeline.Models.MatchScores> scores)
        {

            if (scores.Count > 0)
            {
                bfnexchange.Services.MatchScoreCard objScoreCard = new Services.MatchScoreCard();
                string scoresonly = "";


                // lblMatchUpdates.SelectionAlignment = HorizontalAlignment.Center;
                if (scores.Count == 1)
                {
                    objScoreCard.TeamAName = scores[0].TeamName;
                    objScoreCard.TeamAScore = scores[0].Scores.ToString() + "/" + scores[0].Wickets.ToString();
                    objScoreCard.TeamAOver= scores[0].Overs.ToString() + " Overs";
                    
                }
                else
                {
                    if (scores.Count == 2)
                    {
                        objScoreCard.TeamBName = scores[0].TeamName;
                        objScoreCard.TeamBScore = scores[0].Scores.ToString() + "/" + scores[0].Wickets.ToString();
                        objScoreCard.TeamBOver = scores[0].Overs.ToString() + " Overs";

                        objScoreCard.TeamAName = scores[1].TeamName;
                        objScoreCard.TeamAScore = scores[1].Scores.ToString() + "/" + scores[1].Wickets.ToString();
                        objScoreCard.TeamAOver = scores[1].Overs.ToString() + " Overs";


                       

                        

                    }
                    else
                    {
                        if (scores.Count == 3)
                        {
                            objScoreCard.TeamAScore = scores[0].TeamName + " " + scores[0].Scores.ToString() + " / " + scores[0].Wickets.ToString() + " in " + scores[0].Overs.ToString() + " Overs , " + scores[2].Scores.ToString() + " / " + scores[2].Wickets.ToString() + " in " + scores[2].Overs.ToString() + " Overs";
                            objScoreCard.TeamBScore = scores[1].TeamName + " " + scores[1].Scores.ToString() + " / " + scores[1].Wickets.ToString() + " in " + scores[1].Overs.ToString() + " Overs";


                        }
                        else
                        {
                            objScoreCard.TeamAScore = scores[0].TeamName + " " + scores[0].Scores.ToString() + " / " + scores[0].Wickets.ToString() + " in " + scores[0].Overs.ToString() + " Overs , " + scores[2].Scores.ToString() + " / " + scores[2].Wickets.ToString() + " in " + scores[2].Overs.ToString() + " Overs";
                            objScoreCard.TeamBScore = scores[1].TeamName + " " + scores[1].Scores.ToString() + " / " + scores[1].Wickets.ToString() + " in " + scores[1].Overs.ToString() + " OVers , " + scores[3].Scores.ToString() + " / " + scores[3].Wickets.ToString() + " in " + scores[3].Overs.ToString() + " Overs";



                        }
                    }


                }
                return ConverttoJSONString(objScoreCard);

            }
            else
            {
                return "";
            }
        }
        public string CreateScoreCard(string matchCricketAPIKey,string EventId,string GetMatchUpdatesFrom)
        {
            try
            {
                if (GetMatchUpdatesFrom == "Local")
                {
                    var scores = JsonConvert.DeserializeObject<List<MatchScores>>(objUserServiceClient.GetScoresbyEventIDandDate(EventId, DateTime.Now));

                    return UpdateScoresDataNew(scores);
                }
                else
                {
                    if (GetMatchUpdatesFrom == "Other")
                    {
                        var newresult = objCricketScoreClient.GetMatchDatabyKey(matchCricketAPIKey, ConfigurationManager.AppSettings["PasswordForValidate"]);
                        return newresult;
                    }
                    else
                    {
                        return "";
                    }
                }
              
                //var results = JsonConvert.DeserializeObject<Services.MatchScoreCard>(objCricketScoreClient.GetMatchDatabyKey(matchCricketAPIKey, ConfigurationManager.AppSettings["PasswordForValidate"]));
                //if (results != null)
                //{
                //    return RenderRazorViewToString("CricketScores", results);
                //}
                //else
                //{
                //    return "";
                //}
            }
            catch (System.Exception ex)
            {
                return "";
            }

        }
        ExternalAPI.TO.Home root = new ExternalAPI.TO.Home();
        ExternalAPI.TO.UpdateNew rootnew = new ExternalAPI.TO.UpdateNew();
        public string CreateScoreCardNew(string EventId)
        {
            try
            {
                //root = objBettingClient.GetUpdate(EventId);
                rootnew = objBettingClient.GetUpdateNew(EventId);

                //var marketbook = objBettingClient.GetUpdate(EventId);
                //HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(string.Format("http://172.104.164.101:3000/mscore/" + EventId + ""));

                //WebReq.Method = "GET";
                //HttpWebResponse WebResp = (HttpWebResponse)WebReq.GetResponse();
                //Console.WriteLine(WebResp.StatusCode);
                //Console.WriteLine(WebResp.Server);

                string jsonString;
                jsonString = JsonConvert.SerializeObject(root);
                //using (Stream stream = WebResp.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
                //{
                //    StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                //    jsonString = reader.ReadToEnd();
                //}
                try
                {
                 // var result= JsonConvert.DeserializeObject<ExternalAPI.TO.Root>(jsonString);
                  
                    return jsonString;
                }
                catch (System.Exception ex)
                {
                   
                    APIConfig.LogError(ex);
                    return jsonString;
                }
            }
            catch (System.Exception ex)
            {
                return "";
            }

        }
        List<RootSCT> rootsct = new List<RootSCT>();
        public string CreateSoccerCardNew(string EventID)
        {
            try
            {
              
                string jsonString = objBettingClient.GetSoccorUpdate(EventID);
                //root = rootsct.Where(item => item.EventId == Convert.ToInt32(EventID)).FirstOrDefault();
                //string jsonString;
                //jsonString = JsonConvert.SerializeObject(root);
               
                try
                {
                    return jsonString;
                }
                catch (System.Exception ex)
                {
                    APIConfig.LogError(ex);
                    return jsonString;
                }
            }
            catch (System.Exception ex)
            {
                return "";
            }

        }
        public string CreateScoreCardWithTeams(bfnexchange.Services.MatchScoreCard scorecard,string currentteam)
        {

            try
            {

              //  return objCricketScoreClient.GetMatchDatabyKey(matchCricketAPIKey, ConfigurationManager.AppSettings["PasswordForValidate"]);
             //  var results = JsonConvert.DeserializeObject<Services.MatchScoreCard>(scorecard);
                if (scorecard != null)
                {
                    scorecard.CurrentTeamForSelection = currentteam;
                    return RenderRazorViewToString("ScoreCard", scorecard);
                }
                else
                {
                    return "";
                }
            }
            catch (System.Exception ex)
            {
                return "";
            }

        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}