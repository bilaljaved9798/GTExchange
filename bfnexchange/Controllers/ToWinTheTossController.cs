using bfnexchange.BettingServiceReference;
using bfnexchange.UsersServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bfnexchange.Controllers
{
    public class ToWinTheTossController : Controller
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceClient objBettingClient = new BettingServiceClient();
        // GET: ToWinTheToss
        public string CheckforToWintheTossMarket(string EventID)
        {
            if (LoggedinUserDetail.GetUserTypeID() != 3)
            {
                ViewBag.backgrod = "-webkit-linear-gradient(bottom, #1D9BF0, #0a0a0a) !important;";
                ViewBag.color = "white";
            }
            int UserIDforLinevmarkets = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLinevmarkets = 73;
            }
            else
            {
                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            }
            var wintethossmarket = objUsersServiceCleint.GetToWintheTossbyeventId(UserIDforLinevmarkets,EventID);
            // panelWidgets.Visible = true;
            // webBrowserWidget.Navigate(ConfigurationManager.AppSettings["WidgetURL"]);
            if (wintethossmarket != null)
            {
                if (wintethossmarket.MarketCatalogueID != null)
                {
                    Session["TWT"] = wintethossmarket.MarketCatalogueID;
                    return wintethossmarket.MarketCatalogueID;                   
                }
            }
            Session["TWT"] = "";
            return "";
        }
    }
}