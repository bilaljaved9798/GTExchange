using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using System.Net;
using System.Xml;
using System.Data;
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
using GTExchNew.HelperClasses;
using GTExchNew.Models;
using UsersServiceReference;

namespace GTExchNew
{
    public partial class SP_BetPlaceWaitandInterval_GetAllData_Result
    {
        public int HorseRaceTimerInterval { get; set; }
        public int HorseRaceBetPlaceWait { get; set; }
        public int GrayHoundTimerInterval { get; set; }
        public int GrayHoundBetPlaceWait { get; set; }
        public int CricketMatchOddsTimerInterval { get; set; }
        public int CricketMatchOddsBetPlaceWait { get; set; }
        public int CompletedMatchTimerInterval { get; set; }
        public int CompletedMatchBetPlaceWait { get; set; }
        public int TiedMatchTimerInterval { get; set; }
        public int TiedMatchBetPlaceWait { get; set; }
        public int InningsRunsTimerInterval { get; set; }
        public int InningsRunsBetPlaceWait { get; set; }
        public int WinnerTimerInterval { get; set; }
        public int WinnerBetPlaceWait { get; set; }
        public int TennisTimerInterval { get; set; }
        public int TennisBetPlaceWait { get; set; }
        public int SoccerTimerInterval { get; set; }
        public int SoccerBetPlaceWait { get; set; }
        public Nullable<decimal> PoundRate { get; set; }
        public Nullable<int> FancyTimerInterval { get; set; }
        public Nullable<int> FancyBetPlaceWait { get; set; }
        public Nullable<int> RaceMinutesBeforeStart { get; set; }
        public Nullable<int> CancelBetTime { get; set; }
    }
    public static partial class LoggedinUserDetail
    {
        public static decimal PoundRate { get; set; }
        public static int AgentRate { get; set; }
        public static int UserID { get; set; }
        public static bool isWorkingonBets { get; set; } = false;
        public static UserIDandUserType user { get; set; }
        public static bool isTVShown { get; set; } = false;
        public static List<HelperClasses.LiveTVChannels> TvChannels { get; set; }
        public static bool isMainFormShown { get; set; } = true;
        public static List<ExternalAPI.TO.MarketBook> MarketBooks { get; set; }
        public static List<string> OpenMarkets { get; set; }
        public static SP_BetPlaceWaitandInterval_GetAllData_Result BetPlaceWaitInterval { get; set; }
        public static HelperClasses.sessionHeader objCurrSession = new HelperClasses.sessionHeader();
        public static bool isInserting = false;
        public static List<UserBets> CurrentUserBets = new List<UserBets>();
        public static List<UserBetsforAgent> CurrentAgentBets = new List<UserBetsforAgent>();
        public static List<UserBetsForAdmin> CurrentAdminBets = new List<UserBetsForAdmin>();
        public static List<UserBetsforSuper> CurrentSuperBets = new List<UserBetsforSuper>();
        public static List<UserBetsForAdmin> CurrentAdminBetsNew = new List<UserBetsForAdmin>();
        public static double TotalLiabality = 0;

        public static double CurrentAvailableBalance = 0;
        public static double CurrentAccountBalance = 0;
        public static decimal NetBalance = 0;
        public static BetSlipKeys objBetSlipKeys = new BetSlipKeys();
        public static List<MarketRules> MarketRulesAll = new List<MarketRules>();
        public static List<UserIDandUserType> AllUsers = new List<UserIDandUserType>();
        public static AllowedMarkets AllowedMarketsForUser = new AllowedMarkets();
        public static List<SP_URLsData_GetAllData_Result> URLsData = new List<SP_URLsData_GetAllData_Result>();
        public static string SecurityCode = "";
        public static string GetCricketDataFrom = "";
        public static bool isTVON { get; set; } = false;
        public static string PasswordForValidate = "";
        public static string PasswordForValidateS = "";
        public static int MaxBalanceTransferLimit = 0;
        public static int MaxAgentRateLimit = 0;
        public static bool RefreshLiabality = false;
        public static bool IsCom = false;
        

        public static string FormatNumber(uint n)
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
            //    HttpBrowserCapabilities capability = HttpContext.Current.Request.Browser;
            //    var BrowserName = capability.Browser;
            //    var version = capability.Version;
            //    var platform = capability.Platform;
            //    return "Browser: " + BrowserName.ToString() + ", Version: " + version.ToString() + ", PlatForm: " + platform.ToString();
            return "";
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
                string location = GetUserCountryByIp(strIPAddress);
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

       

    }
}