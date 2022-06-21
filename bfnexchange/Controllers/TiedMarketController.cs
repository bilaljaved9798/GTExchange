using bfnexchange.BettingServiceReference;
using bfnexchange.UsersServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bfnexchange.Controllers
{
    public class TiedMarketController : Controller
    {
        // GET: TiedMarket
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        BettingServiceClient objBettingClient = new BettingServiceClient();
        public string CheckforToWintheTossMarket(string EventID)
        {
            if (LoggedinUserDetail.GetUserTypeID() != 3)
            {
                ViewBag.backgrod = "#1D9BF0";
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
            var wintethossmarket = objUsersServiceCleint.GetToTiedMarketbyEventID(UserIDforLinevmarkets, EventID);
            // panelWidgets.Visible = true;
            // webBrowserWidget.Navigate(ConfigurationManager.AppSettings["WidgetURL"]);
            if (wintethossmarket != null)
            {
                if (wintethossmarket.MarketCatalogueID != null)
                {
                    Session["TM"] = wintethossmarket.MarketCatalogueID;
                    return wintethossmarket.MarketCatalogueID;
                }
            }
            Session["TM"] = "";
            return "";
        }
    }
}