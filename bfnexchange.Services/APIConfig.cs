using bfnexchange.Services.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Text;

namespace bfnexchange.Services
{
    public static class MatchScoresAll
    {
        public static List<MatchScoreCard> AllMatchScoreCards = new List<MatchScoreCard>();
        public static List<APIResponse> AllAPIResponse = new List<APIResponse>();
    }
    public static class APIConfig
    {
        public static string Url = "";
        public static string AppKey = "";
        public static string Certfilename = ConfigurationManager.AppSettings["BefatirCert"];
        private static string Username = "";
        private static string Password = "";
        public static string SessionKey = "";
        public static string UrlLive = "";
        public static string AppKeyLive = "";

        private static string UsernameLive = "";
        private static string PasswordLive = "";
        public static string SessionKeyLive = "";
        public static bool isUpdatingBetstocompelete = false;
        public static bool isUpdatingBettingAllowed = false;
        public static bool isSendingEmail = false;
        public static sessionHeader objCurrSession = new sessionHeader();
        public static string SessionforOtherdata="";
        public static List<bfnexchange.Services.DBModel.SP_URLsData_GetAllData_Result> URLsData = new List<bfnexchange.Services.DBModel.SP_URLsData_GetAllData_Result>();
        public static string SecurityCode = "";
        public static string GetCricketDataFrom = "";
        public static bool AutomaticDownloadData;
        public static IList<ExternalAPI.TO.MarketBook> LiveCricketMarketBooks = new List<ExternalAPI.TO.MarketBook>();
        public static IList<ExternalAPI.TO.MarketBook> LiveCricketMarketBooksFancy = new List<ExternalAPI.TO.MarketBook>();
        public static List<SampleResponse1> BFMarketBooks = new List<SampleResponse1>();
        public static IList<bfnexchange.wrBF.MarketBook> BFMarketBooksOther = new List<bfnexchange.wrBF.MarketBook>();
        public static List<ExternalAPI.TO.MarketBookString> BFMarketBooksOther123 = new List<ExternalAPI.TO.MarketBookString>();
        public static List<ExternalAPI.TO.GetDataFancy> MarketBookForindianFancy = new List<ExternalAPI.TO.GetDataFancy>();
        public static List<ExternalAPI.TO.Root> Listcricketupdate = new List<ExternalAPI.TO.Root>();
        public static List<ExternalAPI.TO.UpdateResult> ListcricketupdateNew = new List<ExternalAPI.TO.UpdateResult>();
        public static List<ExternalAPI.TO.LinevMarkets> LinevMarkets = new List<ExternalAPI.TO.LinevMarkets>();
        public static string FormatNumber(double n)
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
        public class LoginResponse
        {
            [JsonProperty(PropertyName = "sessionToken")]
            public string sessionToken { get; set; }

            [JsonProperty(PropertyName = "loginStatus")]
            public string loginStatus { get; set; }
        }
        public static void AddMarketstoStaticVariableLive(IList<ExternalAPI.TO.MarketBook> results)
        {
            foreach (var item in results)
            {
                if (APIConfig.LiveCricketMarketBooks != null)
                {
                    if (APIConfig.LiveCricketMarketBooks.Count() > 0)
                    {
                        var currbpmarket = APIConfig.LiveCricketMarketBooks.Where(item2 => item2.MarketId == item.MarketId).FirstOrDefault();
                        if (currbpmarket != null)
                        {
                            var index = APIConfig.LiveCricketMarketBooks.IndexOf(currbpmarket);

                            if (index != -1)
                                APIConfig.LiveCricketMarketBooks[index] = item;

                        }
                        else
                        {
                            APIConfig.LiveCricketMarketBooks.Add(item);
                        }
                    }
                    else
                    {
                        APIConfig.LiveCricketMarketBooks.Add(item);
                    }
                }
            }
        }
        public static void InitiateAPIConfiguration()
        {
            APIConfigService objAPIConfigCleint = new APIConfigService();
            APIConfigData configData = JsonConvert.DeserializeObject<APIConfigData>(objAPIConfigCleint.GetAPIConfigData(2));
            Username = Crypto.Decrypt(configData.Username);
            Password = Crypto.Decrypt(configData.Password);
            AppKey = Crypto.Decrypt(configData.Appkey);
            Url = Crypto.Decrypt(configData.APIURL);
            SessionKey = Crypto.Decrypt(configData.SessionKey);
        }
        public static void InitiateAPIConfigurationLive()
        {

            APIConfigService objAPIConfigCleint = new APIConfigService();
            APIConfigData configData = JsonConvert.DeserializeObject<APIConfigData>(objAPIConfigCleint.GetAPIConfigData(4));
            UsernameLive = Crypto.Decrypt(configData.Username);
            PasswordLive = Crypto.Decrypt(configData.Password);
            AppKeyLive = Crypto.Decrypt(configData.Appkey);
            UrlLive = Crypto.Decrypt(configData.APIURL);
            SessionKeyLive = Crypto.Decrypt(configData.SessionKey);
        }
        public static string Login(string username, string password)
        {
            string ssoid = String.Empty;
            string[] info;
            try
            {
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                string uri = string.Format("https://identitysso.betfair.com/api/login?username={0}&password={1}&login=true&redirectMethod=POST&product=home.betfair.int&url=https://www.betfair.com/", username, password);

                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(uri);
                myRequest.Method = "POST";
                myRequest.Timeout = 10000;
                WebResponse thePage = myRequest.GetResponse();
                info = thePage.Headers.GetValues("Set-Cookie");
                int i = 0;
                while (ssoid == String.Empty && i < info.Length)
                {
                    if (info[i].Contains("ssoid="))
                    {
                        string[] sessionarr = info[i].ToString().Split(';');
                        ssoid =sessionarr[0].Replace("ssoid=", "");
                    }

                       // ssoid = ExtractSSOID(info[i]);
                    ++i;
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

           // SessionID = ssoid;
            return ssoid;
        }
        public static void GetSessionFromBeftairLive()
        {

            string sessionID = Login(UsernameLive, PasswordLive);
            SessionKeyLive = sessionID;
            APIConfigService objAPIConfigClient = new APIConfigService();
            objAPIConfigClient.UpdateSession(Crypto.Encrypt(SessionKeyLive), 4);
            APIConfig.WriteErrorToDB("Get Session for Live");
            //KeepAlive(SessionKey, AppKey);
            return;



        }
        public static  void GetSessionFromBeftair()
        {

            string sessionID = Login(Username, Password);
            SessionKey = sessionID;
            APIConfigService objAPIConfigClient = new APIConfigService();
            objAPIConfigClient.UpdateSession(Crypto.Encrypt(SessionKey),2);
            //KeepAlive(SessionKey, AppKey);
            return;
            


        }
        public static void KeepAlive(string SessToken, string AppKey = "")
        {
            string Url = " https://identitysso.betfair.com/api/keepAlive";
            WebRequest request = null;
            WebResponse response = null;
            string strResponseStatus = "";
            try
            {
                request = WebRequest.Create(new Uri(Url));
                request.Method = "POST";
                request.ContentType = "application/json-rpc";
                request.Headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8");
                request.Headers.Add("X-Authentication", SessToken);
                if (!string.IsNullOrEmpty(AppKey))
                {
                    request.Headers.Add("X-Application", AppKey);
                }
                //~~> Get the response.
                response = request.GetResponse();
                //~~> Display the status below 
                strResponseStatus = "Status is " + ((HttpWebResponse)response).StatusDescription;
            }
            catch (Exception ex)
            {
                APIConfig.LogError(ex);
            }
           
            //~~~Clean Up
            response.Close();
        }
        public static void WriteErrorToDB(string exception)
        {
            UserServices objUSerservice = new UserServices();
            objUSerservice.AddUserActivity(exception, DateTime.Now, "", "", "", 1);
        }

        public static void LogError(Exception ex)
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
            //string path = HttpContext.Current.Server.MapPath("~/ErrorLog/ErrorLog.txt");
            //using (StreamWriter writer = new StreamWriter(path, true))
            //{
            //    writer.WriteLine(message);
            //    writer.Close();
            //}
            }
            catch (Exception ex1)
            {

                APIConfig.WriteErrorToDB(ex.Message + " " + ex.StackTrace + " " + ex.Source + " " + ex.TargetSite.ToString());

            }
        }
    }

    public static class OpenMarkets
    {
        public static List<DBModel.SP_UserMarket_GetDistinctMarketsOpenedNew_Result> OpenMarketlst = new List<DBModel.SP_UserMarket_GetDistinctMarketsOpenedNew_Result>();
    }
    public static class OpenMarketsFancy
    {
        public static List<DBModel.SP_UserMarket_GetDistinctMarketsOpenedNewFancy_Result> OpenMarketlstfancy = new List<DBModel.SP_UserMarket_GetDistinctMarketsOpenedNewFancy_Result>();
    }
}