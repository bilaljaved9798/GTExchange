using bfnexchange.BettingServiceReference;
using bfnexchange.UsersServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bfnexchange.Controllers
{
    public class SoccergoalController : Controller
    {
        // GET: Soccergoal
        UserServicesClient objUsersServiceCleint = new UserServicesClient();            
        public List<string> CheckforSoccergoalMarket(string EventID)
        {
            ViewBag.backgrod = "-webkit-linear-gradient(bottom, #1D9BF0, #0a0a0a) !important;";
            ViewBag.color = "white";
            List<string> data = new List<string>();
            int UserIDforLinevmarkets = 0;
            if (LoggedinUserDetail.GetUserTypeID() == 1)
            {
                UserIDforLinevmarkets = 73;
            }
            else
            {
                UserIDforLinevmarkets = LoggedinUserDetail.GetUserID();
            }
           var Soccergoalmarket = objUsersServiceCleint.GetSoccergoalbyeventId(UserIDforLinevmarkets, EventID);
            if (Soccergoalmarket != null)
            {
                foreach (var item in Soccergoalmarket)
                {
                    data.Add(item.MarketCatalogueID);
                }
                Session["SGMarket"] = data;
                return data;
            }
            Session["SGMarket"] = "";
            return data;
        }
    }
}