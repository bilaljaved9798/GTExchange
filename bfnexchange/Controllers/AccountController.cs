using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using bfnexchange.Models;

using System.Web.Security;
using System.Data;
using System.Net;
using System.Xml;
using Newtonsoft.Json;

using bfnexchange.APIConfigServiceReference;
using bfnexchange.UsersServiceReference;
using System.Collections.Generic;
using System.Diagnostics;

namespace bfnexchange.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        UserServicesClient objUserServiceClient = new UserServicesClient();
        APIConfigServiceClient objAPIConfigClient = new APIConfigServiceClient();
        public AccountController()
        {

        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;

        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            if (Session["User"] == null)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
            else
            {
                var user = Session["User"] as UserIDandUserType;
                if (user.UserName == null)
                {
                    ViewBag.ReturnUrl = returnUrl;
                    return View();
                }
                else
                {
                    if (returnUrl == null)
                    {
                        return RedirectToAction("Index", "DashBoard");
                    }
                    if (returnUrl.Contains("Login"))
                    {
                        return RedirectToAction("Index", "DashBoard");
                    }
                    else
                    {
                        string[] urls = returnUrl.Split('/');
                        return RedirectToAction(urls[2], urls[1]);

                    }

                }
            }



        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
          
            var results = objUserServiceClient.GetUserbyUsernameandPasswordNew(Crypto.Encrypt(model.Username), Crypto.Encrypt(model.Password));
            if (results != "")
            {
                var result = JsonConvert.DeserializeObject<UserIDandUserType>(results);               
                if (result.UserTypeID != 1)
                {
                    if (result.Loggedin == true)
                    {
                        if (result.isBlocked == true)
                        {
                            ModelState.AddModelError("", "Account is Blocked.");
                            return View(model);
                        }
                        if (result.isDeleted == true)
                        {
                            ModelState.AddModelError("", "Account is Deleted.");
                            return View(model);
                        }
                        //System.Threading.Thread.Sleep(10000);
                        result.PoundRate = result.PoundRate;
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
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn2 = result.MutipleBtn2;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn3 = result.MutipleBtn3;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn4 = result.MutipleBtn4;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn5 = result.MutipleBtn5;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn6 = result.MutipleBtn6;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn7 = result.MutipleBtn7;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn8 = result.MutipleBtn8;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn9 = result.MutipleBtn9;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn10 = result.MutipleBtn10;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn11 = result.MutipleBtn11;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn12 = result.MutipleBtn12;

                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn1 = result.SimpleBtn1;                     
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn2 = result.SimpleBtn2;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn3 = result.SimpleBtn3;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn4 = result.SimpleBtn4;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn5 = result.SimpleBtn5;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn6 = result.SimpleBtn6;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn7 = result.SimpleBtn7;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn8 = result.SimpleBtn8;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn9 = result.SimpleBtn9;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn10 = result.SimpleBtn10;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn11 = result.SimpleBtn11;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn12 = result.SimpleBtn12;
                        LoggedinUserDetail.objBetSlipKeys.UserID = result.ID;
                       
                        Session["User"] = result;                                       
                        Session["Runnserdata"] =null;                  
                        Session["firsttimeload"] = true;
                        return RedirectToAction("Index", "DashBoard");                     
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
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn2 = result.MutipleBtn2;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn3 = result.MutipleBtn3;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn4 = result.MutipleBtn4;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn5 = result.MutipleBtn5;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn6 = result.MutipleBtn6;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn7 = result.MutipleBtn7;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn8 = result.MutipleBtn8;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn9 = result.MutipleBtn9;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn10 = result.MutipleBtn10;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn11 = result.MutipleBtn11;
                        LoggedinUserDetail.objBetSlipKeys.MutipleBtn12 = result.MutipleBtn12;

                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn1 = result.SimpleBtn1;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn2 = result.SimpleBtn2;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn3 = result.SimpleBtn3;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn4 = result.SimpleBtn4;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn5 = result.SimpleBtn5;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn6 = result.SimpleBtn6;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn7 = result.SimpleBtn7;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn8 = result.SimpleBtn8;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn9 = result.SimpleBtn9;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn10 = result.SimpleBtn10;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn11 = result.SimpleBtn11;
                        LoggedinUserDetail.objBetSlipKeys.SimpleBtn12 = result.SimpleBtn12;
                        LoggedinUserDetail.objBetSlipKeys.UserID = result.ID;
                        ViewBag["color"] = "Red";
                        ViewBag.color = "Red";
                        Session["Runnserdata"] =null;
                       result.PoundRate = result.PoundRate;
                        Session["User"] = result;                   
                        Session["firsttimeload"] = true;  
                        return RedirectToAction("Index", "DashBoard");
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
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn2 = result.MutipleBtn2;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn3 = result.MutipleBtn3;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn4 = result.MutipleBtn4;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn5 = result.MutipleBtn5;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn6 = result.MutipleBtn6;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn7 = result.MutipleBtn7;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn8 = result.MutipleBtn8;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn9 = result.MutipleBtn9;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn10 = result.MutipleBtn10;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn11 = result.MutipleBtn11;
                    LoggedinUserDetail.objBetSlipKeys.MutipleBtn12 = result.MutipleBtn12;

                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn1 = result.SimpleBtn1;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn2 = result.SimpleBtn2;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn3 = result.SimpleBtn3;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn4 = result.SimpleBtn4;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn5 = result.SimpleBtn5;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn6 = result.SimpleBtn6;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn7 = result.SimpleBtn7;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn8 = result.SimpleBtn8;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn9 = result.SimpleBtn9;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn10 = result.SimpleBtn10;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn11 = result.SimpleBtn11;
                    LoggedinUserDetail.objBetSlipKeys.SimpleBtn12 = result.SimpleBtn12;
                    LoggedinUserDetail.objBetSlipKeys.UserID = result.ID;
                    Session["Runnserdata"] = null;
                    result.PoundRate = result.PoundRate;
                    Session["User"] = result;                
                    Session["firsttimeload"] = true;
                   
                
                    return RedirectToAction("Index", "DashBoard");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }         
        }
        //  Get User Completed

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        //  [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff(int ID)
        {
            int UserID = ID;
            objUserServiceClient.SetLoggedinStatus(UserID, false);
            LoggedinUserDetail.InsertActivityLog(UserID, "Logged Out");
            Session["user"] = new UserIDandUserType();
            Session["firsttimeload"] = true;
            return RedirectToAction("Login", "Account");
            //   AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //  return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion


    }
}