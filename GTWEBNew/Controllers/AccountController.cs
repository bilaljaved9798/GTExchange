
using APIConfigServiceReference;
using GTExchNew.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UsersServiceReference;

namespace GTExchNew.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        UserServicesClient objUserServiceClient = new UserServicesClient();
        APIConfigServiceClient objAPIConfigClient = new APIConfigServiceClient();
        private IConfiguration _config;

        public AccountController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<UserIDandUserType>> Login(string Username, string Password)
        {
            if (!ModelState.IsValid)
            {
            }
            IActionResult response = Unauthorized();

            var result = await objUserServiceClient.GetUserbyUsernameandPasswordAsync(Crypto.Encrypt(Username), Crypto.Encrypt(Password));
            
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
                        LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(results.ID));
                        LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                        LoggedinUserDetail.user = results;
                        objUserServiceClient.SetLoggedinStatus(results.ID, true);
                        APIConfigServiceClient objAPIConfigCleint = new APIConfigServiceClient();
                        LoggedinUserDetail.InsertActivityLog(results.ID, "Logged In");
                        if (result != null)
                        {
                            results.token = GenerateJSONWebToken(results);
                            //response = Ok(new { token = tokenString });
                        }
                        return Ok(new { isSuccess = true, results });
                    }
                    else
                    {
                        LoggedinUserDetail.user = results;
                        LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(results.ID));
                        LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                        objUserServiceClient.SetLoggedinStatus(results.ID, true);
                        APIConfigServiceClient objAPIConfigCleint = new APIConfigServiceClient();
                        LoggedinUserDetail.InsertActivityLog(results.ID, "Logged In");
                        if (result != null)
                        {
                            var tokenString = GenerateJSONWebToken(results);
                            response = Ok(new { token = tokenString });
                        }
                        return Ok(new { isSuccess = true, response });
                    }
                }
                else
                {
                    LoggedinUserDetail.user = results;
                    LoggedinUserDetail.BetPlaceWaitInterval = JsonConvert.DeserializeObject<SP_BetPlaceWaitandInterval_GetAllData_Result>(objUserServiceClient.GetIntervalandBetPlaceTimings(results.ID));
                    LoggedinUserDetail.PoundRate = LoggedinUserDetail.BetPlaceWaitInterval.PoundRate.Value;
                    LoggedinUserDetail.user = results;
                    objUserServiceClient.SetLoggedinStatus(results.ID, true);
                    APIConfigServiceClient objAPIConfigCleint = new APIConfigServiceClient();
                    LoggedinUserDetail.InsertActivityLog(results.ID, "Logged In");
                    if (result != null)
                    {
                        var tokenString = GenerateJSONWebToken(results);
                        response = Ok(new { token = tokenString });
                    }
                    return Ok(new { isSuccess = true, response, });
                }
            }
            else
            {
                return Ok(new { status = 401, isSuccess = false, message = "Invalid User", });
            }
        }

        private string GenerateJSONWebToken(UserIDandUserType userInfo)
        {
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
