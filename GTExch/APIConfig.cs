
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
using bfnexchange.Services;
using bfnexchange.Services.Services;
using APIConfigServiceReference;

namespace GTExch
{
    public static class APIConfig
    {
        public static string Url = "";
        public static string AppKey = "";
        public static string Certfilename = ConfigurationManager.AppSettings["BefatirCert"];
        private static string Username = "";
        private static string Password = "";
        public static string SessionKey = "";
        public class LoginResponse
        {
            [JsonProperty(PropertyName = "sessionToken")]
            public string sessionToken { get; set; }

            [JsonProperty(PropertyName = "loginStatus")]
            public string loginStatus { get; set; }
        }
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
        public static void InitiateAPIConfiguration()
        {
            APIConfigServiceClient objAPIConfigCleint = new APIConfigServiceClient();
            APIConfigData configData = JsonConvert.DeserializeObject<APIConfigData>(objAPIConfigCleint.GetAPIConfigData(4));
            Username = Crypto.Decrypt(configData.Username);
            Password = Crypto.Decrypt(configData.Password);
            AppKey = Crypto.Decrypt(configData.Appkey);
            Url = Crypto.Decrypt(configData.APIURL);
            SessionKey = Crypto.Decrypt(configData.SessionKey);
        }
        public static string Login(string username, string password)
        {
            string ssoid = String.Empty;
            string[] info;

            try
            {
                string uri = string.Format("https://identitysso.betfair.com/api/login?username={0}&password={1}&login=true&redirectMethod=POST&product=home.betfair.int&url=https://www.betfair.com/", username, password);

                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(uri);
                myRequest.Method = "POST";
                myRequest.Timeout = 5000;
                WebResponse thePage = myRequest.GetResponse();
                info = thePage.Headers.GetValues("Set-Cookie");
                int i = 0;
                while (ssoid == String.Empty && i < info.Length)
                {
                    if (info[i].Contains("ssoid="))
                    {
                        string[] sessionarr = info[i].ToString().Split(';');
                        ssoid = sessionarr[0].Replace("ssoid=", "");
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

        public static void GetSessionFromBeftair()
        {

            string sessionID = Login(Username, Password);
            SessionKey = sessionID;
            APIConfigServiceClient objAPIConfigCleint = new APIConfigServiceClient();
            objAPIConfigCleint.UpdateSession(Crypto.Encrypt(SessionKey), 4);
            KeepAlive(SessionKey, AppKey);
            return;
            //try
            //{
            //    string postData = "username=" + Username + "&password=" + Password + "";
            //    X509Certificate2 x509certificate = new X509Certificate2(Certfilename,"");
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://identitysso-api.betfair.com/api/certlogin");
            //    request.UseDefaultCredentials = true;
            //    request.Method = "POST";
            //    request.ContentType = "application/x-www-form-urlencoded";
            //    request.Headers.Add("X-Application", AppKey);
            //    request.ClientCertificates.Add(x509certificate);
            //    request.Accept = "*/*";
            //    request.Proxy = null;
            //    using (Stream stream = request.GetRequestStream())
            //    {
            //        using (StreamWriter writer = new StreamWriter(stream, Encoding.Default))
            //        {
            //            writer.Write(postData);
            //        }
            //    }
            //    using (Stream stream = ((HttpWebResponse)request.GetResponse()).GetResponseStream())
            //    {
            //        using (StreamReader reader = new StreamReader(stream, Encoding.Default))
            //        {
            //            var jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponse>(reader.ReadToEnd());
            //            if (jsonResponse.loginStatus == "SUCCESS")
            //            {
            //                // saveSession(jsonResponse.sessionToken);
            //                SessionKey = jsonResponse.sessionToken;
            //                APIConfigService objAPIConfigClient = new APIConfigService();
            //                objAPIConfigClient.UpdateSession(Crypto.Encrypt(SessionKey));
            //            }

            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    WriteErrorToDB(ex);
            //}


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
            catch (System.Exception ex1)
            {
                APIConfig.WriteErrorToDB(ex.Message + " " + ex.StackTrace + " " + ex.Source + " " + ex.TargetSite.ToString());
            }
        }
    }
}