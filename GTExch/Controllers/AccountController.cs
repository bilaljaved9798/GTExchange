using APIConfigServiceReference;
using GTExch.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using UsersServiceReference;

namespace GTExch.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        UserServicesClient objUserServiceClient = new UserServicesClient();
        APIConfigServiceClient objAPIConfigClient = new APIConfigServiceClient();

        [HttpGet]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult<UserIDandUserType>> Login(string Username, string Password)
        {
            if (!ModelState.IsValid)
            {
            }
            var result = await objUserServiceClient.GetUserbyUsernameandPasswordAsync(Crypto.Encrypt(Username), Crypto.Encrypt(Password));
            //    var result = objUserServiceClient.GetUserbyUsernameandPassword(Crypto.Encrypt(Username), Crypto.Encrypt(Password));
            var results = JsonConvert.DeserializeObject<UserIDandUserType>(result);
            if (result != "")
            {
                LoggedinUserDetail.PasswordForValidate = results.PasswordforValidate;
                LoggedinUserDetail.PasswordForValidateS = results.PasswordforValidateS;
                if (results.isBlocked == true)
                {
                    return Ok("Account is Blocked.");
                }
                if (results.isDeleted == true)
                {                
                    return Ok("Account is Deleted.");
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
                        APIConfigServiceClient objAPIConfigCleint = new APIConfigServiceClient();
                        LoggedinUserDetail.InsertActivityLog(results.ID, "Logged In");
                        return Ok(new { isSuccess = true, result });
                    }
                    else
                    {
                        LoggedinUserDetail.user = results;
                        LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(LoggedinUserDetail.GetUserID()));
                        LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                        objUserServiceClient.SetLoggedinStatus(results.ID, true);
                        APIConfigServiceClient objAPIConfigCleint = new APIConfigServiceClient();
                        LoggedinUserDetail.InsertActivityLog(results.ID, "Logged In");
                        return Ok(new { isSuccess = true, result });
                    }
                }
                else
                {
                    LoggedinUserDetail.user = results;
                    LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(LoggedinUserDetail.GetUserID()));
                    LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                    LoggedinUserDetail.user = results;
                    objUserServiceClient.SetLoggedinStatus(results.ID, true);
                    APIConfigServiceClient objAPIConfigCleint = new APIConfigServiceClient();
                    LoggedinUserDetail.InsertActivityLog(results.ID, "Logged In");
                    return Ok(new { isSuccess = true, result, });
                }
            }
            else
            {
                return Ok(new { status = 401, isSuccess = false, message = "Invalid User", });              
            }
        }
    }
}
