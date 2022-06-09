using bfnexchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace bfnexchange
{
    public class Redirection : Controller
    {
        public void RedirectToLoginPage()
        {
            int UserID = LoggedinUserDetail.GetUserID();
            LoggedinUserDetail.InsertActivityLog(UserID, "Log Out (session expires)");
            //  Session["user"] = new UserIDandUserType();
            var context = new System.Web.Routing.RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), new RouteData());
            var urlHelper = new UrlHelper(context);
            var url = urlHelper.Action("Login", "Account");
            Response.Redirect(url);
            // return RedirectToAction("Login", "Account");

        }

    }
}