
using GTWeb.APIConfigServiceReference;
using GTWeb.Models;
using GTWeb.UsersServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static GTWeb.ApplicationUserManager;

namespace GTWeb.Controllers
{
    //[Route("api/{controller}/editGroups")]
    //[Route("api/[controller]/[action]")]
    public class LoginController : ApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        UserServicesClient objUserServiceClient = new UserServicesClient();
        APIConfigServiceClient objAPIConfigClient = new APIConfigServiceClient();


        [HttpGet]
        public HttpResponseMessage Login()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet, HttpPost]

        public HttpResponseMessage Login(string Username, string Password)
        {
                     
            var results = objUserServiceClient.GetUserbyUsernameandPasswordNew(Crypto.Encrypt(Username), Crypto.Encrypt(Password));
            if (results != "")
            {
                var result = JsonConvert.DeserializeObject<UserIDandUserType>(results);
            
                LoggedinUserDetail.PasswordForValidate = result.PasswordforValidate;
                LoggedinUserDetail.PasswordForValidateS = result.PasswordforValidateS;
                if (result.isBlocked == true)
                {                 
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Account is Blocked.");               
                }
                if (result.isDeleted == true)
                {                                 
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Account is Deleted.");
                }
                if (result.UserTypeID != 1)
                {
                    if (result.Loggedin == true)
                    {
                        LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(result.AccountBalance);
                        LoggedinUserDetail.BetPlaceWaitandInterval.CancelBetTime = result.CancelBetTime;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchBetPlaceWait = result.CompletedMatchBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchTimerInterval = result.CompletedMatchTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsBetPlaceWait = result.CricketMatchOddsBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsTimerInterval = result.CricketMatchOddsTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.FancyBetPlaceWait = result.FancyBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.FancyTimerInterval = result.FancyTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundBetPlaceWait = result.GrayHoundBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundTimerInterval = result.GrayHoundTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceBetPlaceWait = result.HorseRaceBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceTimerInterval = result.HorseRaceTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsBetPlaceWait = result.InningsRunsBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsTimerInterval = result.InningsRunsTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.RaceMinutesBeforeStart = result.RaceMinutesBeforeStart;
                        LoggedinUserDetail.BetPlaceWaitandInterval.SoccerBetPlaceWait = result.SoccerBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.SoccerTimerInterval = result.SoccerTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TennisBetPlaceWait = result.TennisBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TennisTimerInterval = result.TennisTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchBetPlaceWait = result.TiedMatchBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchTimerInterval = result.TiedMatchBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.WinnerBetPlaceWait = result.WinnerBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.WinnerTimerInterval = result.WinnerTimerInterval;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn1 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn10 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn11 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn12 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn2 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn3 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn4 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn5 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn6 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn7 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn8 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn9 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn1 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn10 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn11 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn12 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn2 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn3 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn4 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn5 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn6 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn7 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn8 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn9 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.UserID = result.ID;
 
                        LoggedinUserDetail.user = result;
                        objUserServiceClient.SetLoggedinStatus(result.ID, true);            
                        //LoggedinUserDetail.InsertActivityLog(results.ID, "Logged In");                                 
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                    else
                    {
                        LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(result.AccountBalance);
                        LoggedinUserDetail.BetPlaceWaitandInterval.CancelBetTime = result.CancelBetTime;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchBetPlaceWait = result.CompletedMatchBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchTimerInterval = result.CompletedMatchTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsBetPlaceWait = result.CricketMatchOddsBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsTimerInterval = result.CricketMatchOddsTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.FancyBetPlaceWait = result.FancyBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.FancyTimerInterval = result.FancyTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundBetPlaceWait = result.GrayHoundBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundTimerInterval = result.GrayHoundTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceBetPlaceWait = result.HorseRaceBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceTimerInterval = result.HorseRaceTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsBetPlaceWait = result.InningsRunsBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsTimerInterval = result.InningsRunsTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.RaceMinutesBeforeStart = result.RaceMinutesBeforeStart;
                        LoggedinUserDetail.BetPlaceWaitandInterval.SoccerBetPlaceWait = result.SoccerBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.SoccerTimerInterval = result.SoccerTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TennisBetPlaceWait = result.TennisBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TennisTimerInterval = result.TennisTimerInterval;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchBetPlaceWait = result.TiedMatchBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchTimerInterval = result.TiedMatchBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.WinnerBetPlaceWait = result.WinnerBetPlaceWait;
                        LoggedinUserDetail.BetPlaceWaitandInterval.WinnerTimerInterval = result.WinnerTimerInterval;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn1 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn10 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn11 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn12 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn2 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn3 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn4 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn5 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn6 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn7 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn8 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn9 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn1 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn10 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn11 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn12 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn2 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn3 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn4 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn5 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn6 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn7 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn8 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn9 = result.MutipleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.UserID = result.ID;

                        LoggedinUserDetail.user = result;
                        return Request.CreateResponse(HttpStatusCode.OK, result);
                    }
                }
                else
                {
                    LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(result.AccountBalance);
                    LoggedinUserDetail.BetPlaceWaitandInterval.CancelBetTime = result.CancelBetTime;
                    LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchBetPlaceWait = result.CompletedMatchBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.CompletedMatchTimerInterval = result.CompletedMatchTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsBetPlaceWait = result.CricketMatchOddsBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.CricketMatchOddsTimerInterval = result.CricketMatchOddsTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.FancyBetPlaceWait = result.FancyBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.FancyTimerInterval = result.FancyTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundBetPlaceWait = result.GrayHoundBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.GrayHoundTimerInterval = result.GrayHoundTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceBetPlaceWait = result.HorseRaceBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.HorseRaceTimerInterval = result.HorseRaceTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsBetPlaceWait = result.InningsRunsBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.InningsRunsTimerInterval = result.InningsRunsTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.RaceMinutesBeforeStart = result.RaceMinutesBeforeStart;
                    LoggedinUserDetail.BetPlaceWaitandInterval.SoccerBetPlaceWait = result.SoccerBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.SoccerTimerInterval = result.SoccerTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.TennisBetPlaceWait = result.TennisBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.TennisTimerInterval = result.TennisTimerInterval;
                    LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchBetPlaceWait = result.TiedMatchBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.TiedMatchTimerInterval = result.TiedMatchBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.WinnerBetPlaceWait = result.WinnerBetPlaceWait;
                    LoggedinUserDetail.BetPlaceWaitandInterval.WinnerTimerInterval = result.WinnerTimerInterval;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn1 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn10 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn11 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn12 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn2 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn3 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn4 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn5 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn6 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn7 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn8 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn9 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn1 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn10 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn11 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn12 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn2 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn3 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn4 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn5 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn6 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn7 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn8 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn9 = result.MutipleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.UserID = result.ID;
                    LoggedinUserDetail.user = result;
                    //Session["User"] = result;
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

            }
            else             
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid Code or Member Not Found");
        }


        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage LogOff(int ID)
        {
            int UserID = ID;
            objUserServiceClient.SetLoggedinStatus(UserID, false);
            LoggedinUserDetail.InsertActivityLog(UserID, "Logged Out");
            //Session["user"] = new UserIDandUserType();
            //Session["firsttimeload"] = true;
            return Request.CreateResponse(HttpStatusCode.OK, "Logged Out");         
        }
    }
}
