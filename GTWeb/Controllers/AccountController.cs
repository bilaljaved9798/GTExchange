using GTWeb.APIConfigServiceReference;
using GTWeb.Models;
using GTWeb.UsersServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using static GTWeb.ApplicationUserManager;

namespace GTWeb.Controllers
{
    //[Route("api/[controller]/[action]")]
    public class AccountController : ApiController
    {

       UserServicesClient objUserServiceClient = new UserServicesClient();
       
        [HttpGet, HttpPost]

        public HttpResponseMessage Login(string Username, string Password)
        {
            //var log = DB.EmployeeLogins.Where(x => x.Email.Equals(login.Email) && x.Password.Equals(login.Password)).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                //return View(model);
            }
            var result = objUserServiceClient.GetUserbyUsernameandPasswordNew(Crypto.Encrypt(Username), Crypto.Encrypt(Password));
            var results = JsonConvert.DeserializeObject<UserIDandUserType>(result);
            if (result != "")
            {
                LoggedinUserDetail.PasswordForValidate = results.PasswordforValidate;
                LoggedinUserDetail.PasswordForValidateS = results.PasswordforValidateS;
                if (results.isBlocked == true)
                {
                    ModelState.AddModelError("", "Account is Blocked.");
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                if (results.isDeleted == true)
                {
                    ModelState.AddModelError("", "Account is Deleted.");
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                if (results.UserTypeID != 1)
                {
                    if (results.Loggedin == true)
                    {

                        objUserServiceClient.SetLoggedinStatus(results.ID, false);

                        LoggedinUserDetail.user = results;
                        LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(LoggedinUserDetail.GetUserID()));
                        LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                        LoggedinUserDetail.user = results;
                        objUserServiceClient.SetLoggedinStatus(results.ID, true);
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        LoggedinUserDetail.user = results;
                        LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(LoggedinUserDetail.GetUserID()));
                        LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                        objUserServiceClient.SetLoggedinStatus(results.ID, true);
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                }
                else
                {
                    LoggedinUserDetail.user = results;
                    LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(LoggedinUserDetail.GetUserID()));

                    LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                    LoggedinUserDetail.user = results;
                    objUserServiceClient.SetLoggedinStatus(results.ID, true);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Code or Member Not Found");
            }
        }

       

    }
}
