using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bftradeline.Models;
using System.Web;
using System.Net;
using System.Data;
using System.Xml;
using System.IO;
using globaltraders.UserServiceReference;
using System.Net.NetworkInformation;
using System.Net.Http;
using bftradeline.HelperClasses;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Globalization;
using bftradeline.Models123;
using globaltraders.Models;

namespace globaltraders
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
    public partial class SP_Users_GetUserbyCreatedbyID_Result
    {
        public int ID { get; set; }
    }
    public partial class SP_UserMarket_GetUserMarketbyEventID1_Result
    {
        public long ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string EventTypeID { get; set; }
        public string CompetitionID { get; set; }
        public string EventID { get; set; }
        public string MarketCatalogueID { get; set; }
        public Nullable<int> UpdatedbyID { get; set; }
        public string EventTypeName { get; set; }
        public string CompetitionName { get; set; }
        public string EventName { get; set; }
        public string MarketCatalogueName { get; set; }
        public Nullable<bool> isOpenedbyUser { get; set; }
        public Nullable<System.DateTime> EventOpenDate { get; set; }
        public string SheetName { get; set; }
        public bool BettingAllowed { get; set; }
        public string AssociateEventID { get; set; }
        public string MarketStatus { get; set; }
        public string CricketAPIMatchKey { get; set; }
        public Nullable<System.DateTime> marketClosedTime { get; set; }
        public string GetMatchUpdatesFrom { get; set; }
        public string CountryCode { get; set; }
        public string TotalOvers { get; set; }
        public Nullable<bool> RaceInPlayAllow { get; set; }
    }

    public partial class SP_UserMarket_GetUserMarketbyEventID_Result
    {
        public long ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string EventTypeID { get; set; }
        public string CompetitionID { get; set; }
        public string EventID { get; set; }
        public string MarketCatalogueID { get; set; }
        public Nullable<int> UpdatedbyID { get; set; }
        public string EventTypeName { get; set; }
        public string CompetitionName { get; set; }
        public string EventName { get; set; }
        public string MarketCatalogueName { get; set; }
        public Nullable<bool> isOpenedbyUser { get; set; }
        public Nullable<System.DateTime> EventOpenDate { get; set; }
        public string SheetName { get; set; }
        public bool BettingAllowed { get; set; }
        public string AssociateEventID { get; set; }
        public string MarketStatus { get; set; }
        public string CricketAPIMatchKey { get; set; }
        public Nullable<System.DateTime> marketClosedTime { get; set; }
        public string GetMatchUpdatesFrom { get; set; }
        public string CountryCode { get; set; }
        public string TotalOvers { get; set; }
    }
    public static class LoggedinUserDetail
    {

        public static decimal PoundRate { get; set; }
        public static int AgentRate { get; set; }
        public static int UserID { get; set; }
        public static bool isWorkingonBets { get; set; } = false;
        public static UserIDandUserType user { get; set; }
        public static bool isTVShown { get; set; } = false;
        public static List<LiveTVChannels> TvChannels { get; set; }
        public static bool isMainFormShown { get; set; } = true;
        public static List<ExternalAPI.TO.MarketBook> MarketBooks { get; set; }
        public static List<string> OpenMarkets { get; set; }
        public static SP_BetPlaceWaitandInterval_GetAllData_Result BetPlaceWaitInterval { get; set; }
        public static bftradeline.HelperClasses.sessionHeader objCurrSession = new bftradeline.HelperClasses.sessionHeader();
        public static bool isInserting = false;
        public static List<UserBets> CurrentUserBets = new List<UserBets>();
        public static List<UserBetsforAgent> CurrentAgentBets = new List<UserBetsforAgent>();
        public static List<UserBetsForAdmin> CurrentAdminBets = new List<UserBetsForAdmin>();
        public static List<UserBetsforSuper> CurrentSuperBets = new List<UserBetsforSuper>();
        public static List<UserBetsforSamiAdmin> CurrentsamiadminBets = new List<UserBetsforSamiAdmin>();
        public static ObservableCollection<UserBetsForAdmin> CurrentAdminBetsNew = new ObservableCollection<UserBetsForAdmin>();
        public static double TotalLiabality = 0;
     
        public static double CurrentAvailableBalance = 0;
        public static double CurrentAccountBalance = 0;
        public static decimal NetBalance = 0;
        public static HelperClasses.BetSlipKeys objBetSlipKeys = new HelperClasses.BetSlipKeys();
        public static List<MarketRules> MarketRulesAll = new List<bftradeline.Models.MarketRules>();
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
        public static void InsertActivityLog(int UserID, string ActivityName)


        {
            try
            {
                string location = "";
                string ipaddress = GetIPAddress();
                try
                {
                 location   = GetUserCountryByIp(ipaddress);
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {

                    
                }
              
                DateTime currentdatetime = DateTime.Now;
                UserServicesClient objUserServiceClient = new UserServicesClient();
                objUserServiceClient.AddUserActivityAsync(ActivityName, currentdatetime, ipaddress, location, GetDeviceInfo(), UserID);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {

            }

        }
        public static decimal GetProfitorlossbyAgentPercentageandTransferRate(int AgentsOwnBets, bool TransferAdminAmount, int TransferAgentID, int CreatedbyID, decimal profitorloss, decimal agentrate, int TransferAdminPercentage)
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
                if ((CreatedbyID != TransferAgentID) && (TransferAdminAmount == true)) 
                {
                    
                    var profit1 = profitorloss;
                    //var adminPercent = (100 - (agentrate));
                    //var profit2 = profit1 * Convert.ToDecimal(adminPercent) / 100;
                    profit = profit1 * Convert.ToDecimal(TransferAdminPercentage) / 100;
                }
                else
                {
                //var profit1 = (((100 - (agentrate)) / 100)  * profitorloss);
                profit = (((100 - (agentrate)) / 100)  *profitorloss);
                }
    
              }
            return profit;
        }


        public static string GetPublicIP()
        {
            string url = "http://checkip.dyndns.org";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            return a4;
        }
       public static string getOSInfo()
        {
            //Get Operating system information.
            OperatingSystem os = Environment.OSVersion;
            //Get version information about the os.
            Version vs = os.Version;

            //Variable to hold our return value
            string operatingSystem = "";

            if (os.Platform == PlatformID.Win32Windows)
            {
                //This is a pre-NT version of Windows
                switch (vs.Minor)
                {
                    case 0:
                        operatingSystem = "95";
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                            operatingSystem = "98SE";
                        else
                            operatingSystem = "98";
                        break;
                    case 90:
                        operatingSystem = "Me";
                        break;
                    default:
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 3:
                        operatingSystem = "NT 3.51";
                        break;
                    case 4:
                        operatingSystem = "NT 4.0";
                        break;
                    case 5:
                        if (vs.Minor == 0)
                            operatingSystem = "2000";
                        else
                            operatingSystem = "XP";
                        break;
                    case 6:
                        if (vs.Minor == 0)
                            operatingSystem = "Vista";
                        else if (vs.Minor == 1)
                            operatingSystem = "7";
                        else if (vs.Minor == 2)
                            operatingSystem = "8";
                        else
                            operatingSystem = "8.1";
                        break;
                    case 10:
                        operatingSystem = "10";
                        break;
                    default:
                        break;
                }
            }
            //Make sure we actually got something in our OS check
            //We don't want to just return " Service Pack 2" or " 32-bit"
            //That information is useless without the OS version.
            if (operatingSystem != "")
            {
                //Got something.  Let's prepend "Windows" and get more info.
                operatingSystem = "Windows " + operatingSystem;
                //See if there's a service pack installed.
                if (os.ServicePack != "")
                {
                    //Append it to the OS name.  i.e. "Windows XP Service Pack 3"
                    operatingSystem += " " + os.ServicePack;
                }
                //Append the OS architecture.  i.e. "Windows XP Service Pack 3 32-bit"
                //operatingSystem += " " + getOSArchitecture().ToString() + "-bit";
            }
            //Return the information we've gathered.
            return operatingSystem;
        }
        public static string GetDeviceInfo()
        {

            return "";

            // OperatingSystem os_info = System.Environment.OSVersion;
            //return os_info.VersionString +
            //     " Windows " + GetOsName(os_info);

            //HttpBrowserCapabilities capability = HttpContent.Current.Request.Browser;
            //var BrowserName = capability.Browser;
            //var version = capability.Version;
            //var platform = capability.Platform;
            //return "Browser: " + BrowserName.ToString() + ", Version: " + version.ToString() + ", PlatForm: " + platform.ToString();


        }
        private static string GetOsName(OperatingSystem os_info)
        {
            string version =
                os_info.Version.Major.ToString() + "." +
                os_info.Version.Minor.ToString();
            switch (version)
            {
                case "10.0": return "10/Server 2016";
                case "6.3": return "8.1/Server 2012 R2";
                case "6.2": return "8/Server 2012";
                case "6.1": return "7/Server 2008 R2";
                case "6.0": return "Server 2008/Vista";
                case "5.2": return "Server 2003 R2/Server 2003/XP 64-Bit Edition";
                case "5.1": return "XP";
                case "5.0": return "2000";
            }
            return "Unknown";
        }
        public static void LogError(System.Exception ex)
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

            // string path = Application.StartupPath("~/ErrorLog/ErrorLog.txt");
            //string path = System.IO.Path.GetFullPath("/ ErrorLog / ErrorLog.txt");
            //using (StreamWriter writer = new StreamWriter(path, true))
            //{
            //    writer.WriteLine(message);
            //    writer.Close();
            //}
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
#pragma warning disable CS0162 // Unreachable code detected
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
#pragma warning restore CS0162 // Unreachable code detected
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo.Country = null;
            }

            return ipInfo.Country;
        }
        public static string GetLocation(string strIPAddress)
        {
            XmlDocument doc = new XmlDocument();

            string getdetails = "http://www.freegeoip.net/xml/" + strIPAddress;

            doc.Load(getdetails);

            XmlNodeList nodeLstCity = doc.GetElementsByTagName("City");

            string location = nodeLstCity[0].InnerText;
            return location;
            //Create a WebRequest with the current Ip
#pragma warning disable CS0162 // Unreachable code detected
            WebRequest _objWebRequest =
#pragma warning restore CS0162 // Unreachable code detected
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
        public static IPAddress GetIPAddress(string hostName)
        {
            Ping ping = new Ping();
            var replay = ping.Send(hostName);

            if (replay.Status == IPStatus.Success)
            {
                return replay.Address;
            }
            return null;
        }
        private static IPAddress GetExternalIPAddress()
        {
            IPHostEntry myIPHostEntry = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress myIPAddress in myIPHostEntry.AddressList)
            {
                byte[] ipBytes = myIPAddress.GetAddressBytes();

                if (myIPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    if (!IsPrivateIP(myIPAddress))
                    {
                        return myIPAddress;
                    }
                }
            }

            return null;
        }
        private static bool IsPrivateIP(IPAddress myIPAddress)
        {
            if (myIPAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                byte[] ipBytes = myIPAddress.GetAddressBytes();

                // 10.0.0.0/24 
                if (ipBytes[0] == 10)
                {
                    return true;
                }
                // 172.16.0.0/16
                else if (ipBytes[0] == 172 && ipBytes[1] == 16)
                {
                    return true;
                }
                // 192.168.0.0/16
                else if (ipBytes[0] == 192 && ipBytes[1] == 168)
                {
                    return true;
                }
                // 169.254.0.0/16
                else if (ipBytes[0] == 169 && ipBytes[1] == 254)
                {
                    return true;
                }
            }

            return false;
        }
        public static string GetIPAddress()
        {
            try
            {
                return GetPublicIP();
                //  IPAddress[] hostAddresses = Dns.GetHostAddresses("");

                //  foreach (IPAddress hostAddress in hostAddresses)
                //  {
                //      if (hostAddress.AddressFamily == AddressFamily.InterNetwork &&
                //          !IPAddress.IsLoopback(hostAddress) &&  // ignore loopback addresses
                //          !hostAddress.ToString().StartsWith("192.168."))  // ignore link-local addresses
                //          return hostAddress.ToString();
                //  }
                //var result2=  GetIPAddress(Dns.GetHostName());
                //string strHostName = System.Net.Dns.GetHostName();
                //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
                //IPAddress[] addr = ipEntry.AddressList;
                //string ip1 = addr[1].ToString();
                //IPHostEntry Host = default(IPHostEntry);
                //string Hostname = null;
                //Hostname = System.Environment.MachineName;
                //Host = Dns.GetHostEntry(Hostname);
                //foreach (IPAddress IP in Host.AddressList)
                //{
                //    if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                //    {
                //      return Convert.ToString(IP);
                //    }
                //}
               
                //var host = Dns.GetHostEntry(Dns.GetHostName());
                //var results= (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                //return "";
                //string ipaddress = "" ;
                ////ipaddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                ////if (ipaddress == "" || ipaddress == null)
                ////    ipaddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                //return ipaddress;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch(System.Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return "";
            }
           
        }
        public static int GetUserID()
        {
            if (user.ID != 0)
            {
               
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
            if ((user.ID != 0))
            {
               

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
            if (user.ID != 0)
            {
               

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
            if (user.ID != 0)
            {
              

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
            if (user.ID != 0)
            {
              

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
            if (user.ID != 0)
            {
               

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
            if (user.ID != 0)
            {
              

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
            if (user.ID != 0)
            {
             

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
            if (user.ID != 0)
            {
               

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
            if (user.ID != 0)
            {
               

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
            if (user.ID != 0)
            {
              

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
            if (user.ID != 0)
            {
               

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
            if (user.ID != 0)
            {
              

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
        //Soccer
        public static decimal GetBetUpperLimitMatchOddsSoccer()
        {
            if (user.ID != 0)
            {


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
            if (user.ID != 0)
            {


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

        //Tennis
        public static decimal GetBetUpperLimitMatchOddsTennis()
        {
            if (user.ID != 0)
            {


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
            if (user.ID != 0)
            {


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

        // Tied Match

        public static decimal GetBetUpperLimitTiedMatch()
        {
            if (user.ID != 0)
            {


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
            if (user.ID != 0)
            {


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
        // Winner

        public static decimal GetBetUpperLimitWinner()
        {
            if (user.ID != 0)
            {


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
            if (user.ID != 0)
            {


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

        public static decimal GetBetUpperLimitInningsRuns()
        {
            if (user.ID != 0)
            {
              

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
            if (user.ID != 0)
            {
              

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
            if (user.ID != 0)
            {
               

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
            if (user.ID != 0)
            {
               

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
        //Fancy
        public static decimal GetBetUpperLimitFancy()
        {
            if (user.ID != 0)
            {


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
            if (user.ID != 0)
            {


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

        public static int CheckConditionsforPlaceBet()
        {
            if (user.ID != 0)
            {
              

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
              
              
            }
            else
            {

            }


        }
        public static int GetUserTypeID()
        {
            if (user.ID != 0)
            {
              

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
            if (user.ID != 0)
            {
               

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
            return user.Password;
            //if (user.ID != 0)
            //{
            //    UserIDandUserType user =Session["User"] as UserIDandUserType;

            //    if (user != null)
            //    {
            //        return Crypto.Decrypt(user.Password);
            //    }
            //    else
            //    {
            //        return "";
            //    }
            //}
            //else
            //{
            //    return "";
            //}
        }
    }
}
