using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using bfnexchange.Models;
using System.Net;
using System.Xml;
using System.Data;
using bfnexchange.UsersServiceReference;
using bfnexchange.APIConfigServiceReference;
using System.Web.Routing;
using System.Web.Mvc;
using System.Configuration;
using Betfair_Non_interactive_login;
using System.Security.Cryptography;
using System.Net.Http;
using System.IO;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Data.OleDb;
using System.Runtime.Serialization.Json;
using bfnexchange.HelperClasses;

namespace bfnexchange
{
    public static partial class LoggedinUserDetail
    {
        public static decimal PoundRate { get; set; }
        public static int AgentRate { get; set; }
        public static int UserID { get; set; }
        public static bool isWorkingonBets { get; set; } = false;
        public static int BetPlaceWait = 0;
        public static int BetWaitTimerInterval = 0;
        public static bool ShowTV = false;
        public static List<LiveTVChannels> TvChannels { get; set; }
        public static bfnexchange.Services.DBModel.SP_BetPlaceWaitandInterval_GetAllData_Result BetPlaceWaitandInterval = new bfnexchange.Services.DBModel.SP_BetPlaceWaitandInterval_GetAllData_Result();
        public static List<bfnexchange.Services.DBModel.SP_URLsData_GetAllData_Result> URLsData = new List<bfnexchange.Services.DBModel.SP_URLsData_GetAllData_Result>();
        public static string SecurityCode = "";
        public static string GetCricketDataFrom = "";
        public static BetSlipKeys objBetSlipKeys = new BetSlipKeys();
        public static string PasswordForValidate = "";
        public static string PasswordForValidateS = "";
        public static bool IsCom = false;
        public static bool isFancyMarketAllowed = false;
        public static double CurrentAccountBalance = 0;
        public static double CurrentAvailableBalance = 0;

        public  static string FormatNumber(uint n)
        {
            if (n < 1000)
                return n.ToString();

            if (n < 10000)
                return String.Format("{0:#,.##}K", n - 5);

            if (n < 100000)
                return String.Format("{0:#,.#}K", n - 50);

            if (n < 1000000)
                return String.Format("{0:#,.}K", n - 500);

            if (n < 10000000)
                return String.Format("{0:#,,.##}M", n - 5000);

            if (n < 100000000)
                return String.Format("{0:#,,.#}M", n - 50000);

            if (n < 1000000000)
                return String.Format("{0:#,,.}M", n - 500000);

            return String.Format("{0:#,,,.##}B", n - 5000000);
        }
        public static string ConverttoJSONString(object result)
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
        public static decimal GetProfitorlossbyAgentPercentageandTransferRate(int AgentsOwnBets, bool TransferAdminAmount, int TransferAgentID, int CreatedbyID, decimal profitorloss, decimal agentrate)
        {
            decimal profit = 0;
            if (AgentsOwnBets == 1)
            {
                if (TransferAdminAmount == true)
                {
                    if (CreatedbyID == TransferAgentID)
                    {
                        profit = (((agentrate) / 100) * profitorloss) + (((100 - (agentrate)) / 100) * profitorloss);
                    }
                    else
                    {
                        profit = (((agentrate) / 100) * profitorloss);
                    }
                }
                else
                {
                    profit = (((agentrate) / 100) * profitorloss);

                }
            }
            else
            {
                profit = (((100 - (agentrate)) / 100) * profitorloss);
            }
            return profit;
        }
        public static int GetBetPlacewaitTimerandInterval(string categoryname, string marketbookname,string Runnerscount)
        {
            if (categoryname == "Fancy")
            {
                BetPlaceWait = BetPlaceWaitandInterval.FancyBetPlaceWait.Value;
                BetWaitTimerInterval = BetPlaceWaitandInterval.FancyTimerInterval.Value;
            }
            else
            {
                if (categoryname.Contains("Horse Racing"))
                {
                    BetPlaceWait = BetPlaceWaitandInterval.HorseRaceBetPlaceWait;
                    BetWaitTimerInterval = BetPlaceWaitandInterval.HorseRaceTimerInterval;

                }
                else
                {

                    if (categoryname.Contains("Greyhound Racing"))
                    {
                        BetPlaceWait = BetPlaceWaitandInterval.GrayHoundBetPlaceWait;
                        BetWaitTimerInterval = BetPlaceWaitandInterval.GrayHoundTimerInterval;

                    }
                    else
                    {

                        if (marketbookname.Contains("Completed Match"))
                        {
                            BetPlaceWait = BetPlaceWaitandInterval.CompletedMatchBetPlaceWait;
                            BetWaitTimerInterval = BetPlaceWaitandInterval.CompletedMatchTimerInterval;

                        }
                        else
                        {
                            if (marketbookname.Contains("Innings Runs") || marketbookname.Contains("Inns Runs"))
                            {
                                BetPlaceWait = BetPlaceWaitandInterval.InningsRunsBetPlaceWait;
                                BetWaitTimerInterval = BetPlaceWaitandInterval.InningsRunsTimerInterval;

                            }
                            else
                            {
                                if (categoryname == "Tennis")
                                {
                                    BetPlaceWait = BetPlaceWaitandInterval.TennisBetPlaceWait;
                                    BetWaitTimerInterval = BetPlaceWaitandInterval.TennisTimerInterval;

                                }
                                else
                                {
                                    if (categoryname == "Soccer")
                                    {
                                        BetPlaceWait = BetPlaceWaitandInterval.SoccerBetPlaceWait;
                                        BetWaitTimerInterval = BetPlaceWaitandInterval.SoccerTimerInterval;

                                    }
                                    else
                                    {
                                        if (marketbookname.Contains("Tied Match"))
                                        {
                                            BetPlaceWait = BetPlaceWaitandInterval.TiedMatchBetPlaceWait;
                                            BetWaitTimerInterval = BetPlaceWaitandInterval.TiedMatchTimerInterval;

                                        }
                                        else
                                        {
                                            if (marketbookname.Contains("Winner") || marketbookname.Contains("To Win the Toss"))
                                            {
                                                BetPlaceWait = BetPlaceWaitandInterval.WinnerBetPlaceWait;
                                                BetWaitTimerInterval = BetPlaceWaitandInterval.WinnerTimerInterval;

                                            }
                                            else
                                            {
                                                
                                                    BetPlaceWait = BetPlaceWaitandInterval.CricketMatchOddsBetPlaceWait;
                                                    BetWaitTimerInterval = BetPlaceWaitandInterval.CricketMatchOddsTimerInterval;
                                               


                                            }
                                        }

                                    }
                                }

                            }


                        }
                    }
                }
            }
           
            return  BetWaitTimerInterval;
        }

        public static long GetCurrentLoggedInID()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.CurrentLoggedInID;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetPoundRate()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.PoundRate;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static void UpdateCurrentLoggedInID()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    user.CurrentLoggedInID = user.CurrentLoggedInID + 1;
                }
                else
                {

                }
            }
            else
            {

            }
        }
        public static int GetUserID()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {
                    LoggedinUserDetail.UserID = user.ID;
                    return user.ID;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetLowerLimit()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetLowerLimit;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static bool isAllowedHorseRacing()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.isHorseRaceAllowed;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static bool isAllowedGrayHoundRacing()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.isGrayHoundRaceAllowed;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static decimal GetBetUpperLimit()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetUpperLimit;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetUpperLimitHorsePlace()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetUpperLimitHorsePlace;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetLowerLimitHorsePlace()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetLowerLimitHorsePlace;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetUpperLimitGrayHoundPlace()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetUpperLimitGrayHoundPlace;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetLowerLimitGrayHoundPlace()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetLowerLimitGrayHoundPlace;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static decimal GetBetUpperLimitGrayHoundWin()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetUpperLimitGrayHoundWin;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetLowerLimitGrayHoundWin()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetLowerLimitGrayHoundWin;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static decimal GetBetUpperLimitMatchOdds()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetUpperLimitMatchOdds;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetLowerLimitMatchOdds()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetLowerLimitMatchOdds;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Soccer
        /// </summary>
        /// <returns></returns>
        public static decimal GetBetUpperLimitMatchOddsSoccer()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetUpperLimitMatchOddsSoccer;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetLowerLimitMatchOddsSoccer()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetLowerLimitMatchOddsSoccer;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        //Endsoccer
        /// <summary>
        /// Tennis
        /// </summary>
        /// <returns></returns>
        public static decimal GetBetUpperLimitMatchOddsTennis()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetUpperLimitMatchOddsTennis;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetLowerLimitMatchOddsTennis()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetLowerLimitMatchOddsTennis;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        //End Tennis
        public static decimal GetBetUpperLimitInningsRuns()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetUpperLimitInningRuns;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetLowerLimitInningsRuns()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetLowerLimitInningRuns;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public static decimal GetBetUpperLimitCompletedMatch()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetUpperLimitCompletedMatch;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetLowerLimitCompletedMatch()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetLowerLimitCompletedMatch;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// Tied Match
        /// </summary>
        /// <returns></returns>
        public static decimal GetBetUpperLimitTiedMatch()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetUpperLimitTiedMatch;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetLowerLimitTiedMatch()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetLowerLimitTiedMatch;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        //EndTied Match
        /// <summary>
        /// Winner
        /// </summary>
        /// <returns></returns>
        public static decimal GetBetUpperLimitWinner()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetUpperLimitWinner;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetLowerLimitWinner()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetLowerLimitWinner;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetUpperLimitFancy()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetUpperLimitFancy;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static decimal GetBetLowerLimitFancy()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.BetLowerLimitFancy;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        //Endsoccer
        public static int CheckConditionsforPlaceBet()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {
                    if (user.CheckConditionsforPlacingBet == true)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
        public static void CheckifUserLogin()
        {
            if (GetUserID() == 0)
            {
                var context = new System.Web.Routing.RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), new RouteData());
                var urlHelper = new UrlHelper(context);
                var url = urlHelper.Action("Login", "Account");
                HttpContext.Current.Response.Redirect(urlHelper.Action("Login", "Account"), true);
            }
            else
            {

            }


        }
        public static int GetUserTypeID()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {
                    return Convert.ToInt32(user.UserTypeID);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public static string GetUserName()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {
                    return user.UserName;
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        public static string GetUserPassword()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {
                    return Crypto.Decrypt(user.Password);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        public static void InsertActivityLog(int UserID, string ActivityName)
        {
            try
            {
                string ipaddress = GetIPAddress();
                string location = GetLocation(ipaddress);
                DateTime currentdatetime = DateTime.Now;
                UserServicesClient objUserServiceClient = new UserServicesClient();
                objUserServiceClient.AddUserActivity(ActivityName, currentdatetime, ipaddress, location, GetDeviceInfo(), UserID);
            }
            catch (System.Exception ex)
            {

            }

        }
        public static string GetDeviceInfo()
        {
            HttpBrowserCapabilities capability = HttpContext.Current.Request.Browser;
            var BrowserName = capability.Browser;
            var version = capability.Version;
            var platform = capability.Platform;
            return "Browser: " + BrowserName.ToString() + ", Version: " + version.ToString() + ", PlatForm: " + platform.ToString();

        }
        public static void LogError(System.Exception ex)
        {
            try
            {

           
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            string path = HttpContext.Current.Server.MapPath("~/ErrorLog/ErrorLog.txt");
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(message);
                writer.Close();
            }
            }
            catch (Exception ex1)
            {


            }
        }
        public class IpInfo
        {

            [JsonProperty("ip")]
            public string Ip { get; set; }

            [JsonProperty("hostname")]
            public string Hostname { get; set; }

            [JsonProperty("city")]
            public string City { get; set; }

            [JsonProperty("region")]
            public string Region { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("loc")]
            public string Loc { get; set; }

            [JsonProperty("org")]
            public string Org { get; set; }

            [JsonProperty("postal")]
            public string Postal { get; set; }
        }
        public static string GetUserCountryByIp(string ip)
        {
            IpInfo ipInfo = new IpInfo();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                return ipInfo.City;
              
            }
            catch (Exception)
            {
                ipInfo.Country = null;
            }

            return ipInfo.Country;
        }
        public static string GetLocation(string strIPAddress)
        {
            try
            {
              string location=  GetUserCountryByIp(strIPAddress);
                return location;
            //XmlDocument doc = new XmlDocument();

            //string getdetails = "http://www.freegeoip.net/xml/" + strIPAddress;

            //doc.Load(getdetails);

            //XmlNodeList nodeLstCity = doc.GetElementsByTagName("City");

            //string location = nodeLstCity[0].InnerText;
            //return location;
            }
            catch (Exception ex)
            {
                return "";

            }
            //Create a WebRequest with the current Ip
            WebRequest _objWebRequest =
                WebRequest.Create("http://freegeoip.appspot.com/xml/" + strIPAddress.ToString());
            //Create a Web Proxy
            WebProxy _objWebProxy =
               new WebProxy("http://freegeoip.appspot.com/xml/"
                         + strIPAddress, true);

            //Assign the proxy to the WebRequest
            _objWebRequest.Proxy = _objWebProxy;

            //Set the timeout in Seconds for the WebRequest
            _objWebRequest.Timeout = 2000;
            string strVisitorCountry = "";
            try
            {
                //Get the WebResponse 
                WebResponse _objWebResponse = _objWebRequest.GetResponse();
                //Read the Response in a XMLTextReader
                XmlTextReader _objXmlTextReader
                    = new XmlTextReader(_objWebResponse.GetResponseStream());

                //Create a new DataSet
                DataSet _objDataSet = new DataSet();
                //Read the Response into the DataSet
                _objDataSet.ReadXml(_objXmlTextReader);
                DataTable _objDataTable = new DataTable();
                _objDataTable = _objDataSet.Tables[0];

                if (_objDataTable != null)
                {
                    if (_objDataTable.Rows.Count > 0)
                    {
                        strVisitorCountry = ", CITY: "
                                    + Convert.ToString(_objDataTable.Rows[0]["City"]).ToUpper()
                                    + ", COUNTRY: "
                                    + Convert.ToString(_objDataTable.Rows[0]["CountryName"]).ToUpper()
                                    + ", COUNTRY CODE: "
                                    + Convert.ToString(_objDataTable.Rows[0]["CountryCode"]).ToUpper();
                    }

                    else
                    {
                        strVisitorCountry = null;

                    }


                }

                return strVisitorCountry;
            }
            catch
            {
                return strVisitorCountry;
            }
        } // End of GetLocation	

        public static string GetIPAddress()
        {
            try
            {

          
            string ipaddress;
            ipaddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return ipaddress;
            }
            catch (Exception ex)
            {
                return "";

            }
        }


        //public static string GetSessionFromBeftair()
        //{

        //    var client = new AuthClient(AppKey);
        //    try
        //    {
        //        var newcertfilename = Certfilename.Replace(@"\\", @"\");
        //        var resp = client.doLogin(Username, Password, newcertfilename);

        //        if (resp.LoginStatus == "SUCCESS")
        //        {
        //            return resp.SessionToken.ToString();
        //        }
        //        else
        //        {
        //            return "False";
        //        }
        //    }
        //    catch (CryptographicException e)
        //    {
        //        return "False";
        //    }
        //    catch (HttpRequestException e)
        //    {
        //        return "False";
        //    }
        //    catch (WebException e)
        //    {
        //        return "False";
        //    }
        //    catch (Exception e)
        //    {
        //        return "False";
        //    }
        //}

        public static bool isAllowedBetSlipKeys()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.isBetSlipKeysAllowed;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool isAllowedJoriBet()
        {
            if (HttpContext.Current.Session["User"] != null)
            {
                UserIDandUserType user = HttpContext.Current.Session["User"] as UserIDandUserType;

                if (user != null)
                {

                    return user.isJoriBetAllowed;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static BetSlipKeys GetBetSlipKeys()
        {
            UserServicesClient objUserServiceClient = new UserServicesClient();
            BetSlipKeys obj = JsonConvert.DeserializeObject<bfnexchange.Models.BetSlipKeys>(objUserServiceClient.GetBetSlipKeys(LoggedinUserDetail.GetUserID()));
            return obj;
        }

    }
}