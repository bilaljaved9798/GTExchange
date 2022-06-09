using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Net;
using System.Globalization;

namespace bfnexchange
{
    /// <summary>
    /// Summary description for CurrencryConverterService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CurrencryConverterService : System.Web.Services.WebService
    {

        [WebMethod]
        public decimal ConvertCurrencyRates(decimal amount)
        {
            string fromcountry = "USD";
            string tocountry = "PKR";
            WebClient web = new WebClient();
            string url = string.Format("https://www.google.com/finance/converter?a={2}&from={0}&to={1}", fromcountry.ToUpper(),tocountry.ToUpper(), amount);
    
            string response = web.DownloadString(url);

            var split = response.Split((new string[] { "<span class=bld>" }), StringSplitOptions.None);
            var value = split[1].Split(' ')[0];
            decimal rate = decimal.Parse(value, CultureInfo.InvariantCulture);
            return rate;
        }
    }
}
