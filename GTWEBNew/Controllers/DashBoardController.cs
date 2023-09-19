using AccountsServiceReference;
using BettingServiceReference;
using bfnexchange.Services.DBModel;
using GTExchNew.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using UsersServiceReference;

namespace GTExchNew.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    [ApiController]
   
    public class DashBoardController : ControllerBase
    {
        UserServicesClient objUsersServiceCleint = new UserServicesClient();
        AccessRightsbyUserType objAccessrightsbyUserType;
        UserBetsUpdateUnmatcedBets objUserBets = new UserBetsUpdateUnmatcedBets();
        BettingServiceClient client = new BettingServiceClient();
        AccountsServiceClient objAccountsService = new AccountsServiceClient();
        List<ProfitandLossEventType> lstProfitandLossAll = new List<ProfitandLossEventType>();

        public async Task<ActionResult<AccessRightsbyUserType>> GetUserData(int userID,int typeID)
        {            
            Decimal CurrentLiabality = 0;

            LoggedinUserDetail.objBetSlipKeys = JsonConvert.DeserializeObject<BetSlipKeys>(objUsersServiceCleint.GetBetSlipKeys(userID));
            if (userID > 0)
            {
                List<UserLiabality> lstUserLiabality = JsonConvert.DeserializeObject<List<UserLiabality>>(objUsersServiceCleint.GetCurrentLiabality(userID));
                objAccessrightsbyUserType = new AccessRightsbyUserType();
                 var result = await objUsersServiceCleint.GetAccessRightsbyUserTypeAsync(typeID, LoggedinUserDetail.PasswordForValidate);
              
                objAccessrightsbyUserType = JsonConvert.DeserializeObject<AccessRightsbyUserType>(result);
                if (typeID != 1)
                {
                    if (lstUserLiabality.Count > 0)
                    {
                        CurrentLiabality = Convert.ToDecimal(lstUserLiabality.Sum(item => Convert.ToDecimal((item.Liabality))));
                        objAccessrightsbyUserType.CurrentLiabality = CurrentLiabality.ToString("F2");
                    }
                    else
                    {
                        objAccessrightsbyUserType.CurrentLiabality = "0.00";
                    }
                }
                string accountbalance = objUsersServiceCleint.GetCurrentBalancebyUser(LoggedinUserDetail.UserID, LoggedinUserDetail.PasswordForValidate);
                LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(accountbalance);
                objAccessrightsbyUserType.CurrentAvailableBalance = LoggedinUserDetail.CurrentAccountBalance + Convert.ToDouble(CurrentLiabality);
                //objAccessrightsbyUserType.Username = LoggedinUserDetail.GetUserName();
            }
            return Ok(new { isSuccess = true, objAccessrightsbyUserType });
        }


        public AccessRightsbyUserType GetBalnceDetails(int userID,int typeID, string password)
        {
            
                double CurrentAccountBalance = 0;
                string CurrentLiabality = "";
                try
                {
                    LoggedinUserDetail.CurrentAccountBalance = Convert.ToDouble(objUsersServiceCleint.GetStartingBalance(userID, password));
                    CurrentAccountBalance = Convert.ToDouble(objUsersServiceCleint.GetCurrentBalancebyUser(userID, password));
                }
                catch (System.Exception ex)
                {
                }
                if (typeID == 3)
                {
                    double laboddmarket = 0;
                    double othermarket = 0;
                    List<UserBets> lstUserBets = JsonConvert.DeserializeObject<List<Models.UserBets>>(objUsersServiceCleint.GetUserbetsbyUserID(userID, password));
                    List<UserBets> lstUserBetsF = lstUserBets.Where(x => x.location != "9").ToList();
                    List<UserBets> lstUserBetsfncy = lstUserBets.Where(x => x.location == "9").ToList();
                    laboddmarket = objUserBets.GetLiabalityofCurrentUser(userID, lstUserBetsF);
                    othermarket = objUserBets.GetLiabalityofCurrentUserfancy(userID, lstUserBetsfncy);
                    CurrentLiabality = (laboddmarket + othermarket).ToString("F2");
                    //ViewBag.CurrentLiabality = CurrentLiabality;
                    LoggedinUserDetail.CurrentAvailableBalance = CurrentAccountBalance + Convert.ToDouble(CurrentLiabality);
                }
                List<UserLiabality> lstUserLiabality = JsonConvert.DeserializeObject<List<UserLiabality>>(objUsersServiceCleint.GetCurrentLiabality(userID));

                objAccessrightsbyUserType = JsonConvert.DeserializeObject<AccessRightsbyUserType>(objUsersServiceCleint.GetAccessRightsbyUserType(typeID, password));
                if (typeID == 8)
                {
                    decimal TotAdminAmount = 0;
                    decimal TotAdmincommession = 0;
                    decimal TotalAdminAmountWithoutMarkets = 0;
                    List<UserAccounts> AgentCommission = new List<UserAccounts>();
                    List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByIDForSuper(userID, false, password));
                    if (lstUserAccountsForAgent.Count > 0)
                    {
                        AgentCommission = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle == "Commission").ToList();
                        lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                        foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                        {
                            if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                            {
                                int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                decimal superpercent = SuperRate - AgentRate;
                                if (ActualAmount > 0)
                                {
                                    decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                    decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                    decimal Comissionamount = 0;
                                    if (AgentRate == 100)
                                    {
                                        Comissionamount = 0;
                                    }
                                    else
                                    {
                                        Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                                    }

                                    TotAdminAmount += -1 * (ActualAmount - SuperAmount - AgentAmount);
                                }
                                else
                                {
                                    ActualAmount = -1 * ActualAmount;
                                    decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                    decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                    TotAdminAmount += ActualAmount - AgentAmount - SuperAmount;
                                }
                            }
                        }
                    }
                    decimal a = 0;
                    try
                    {

                        foreach (UserAccounts objuserAccounts in AgentCommission)
                        {
                            int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                            int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                            int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                            decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                            decimal superpercent = SuperRate - AgentRate;
                            ActualAmount = -1 * ActualAmount;
                            decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                            decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                            TotAdmincommession += ActualAmount - AgentAmount - SuperAmount;
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                    if (LoggedinUserDetail.IsCom == true)
                    {
                        a = (-1 * (TotAdminAmount));
                        //ViewBag.commission = TotAdmincommession;
                    }
                    else
                    {
                        a = (-1 * (TotAdminAmount) + (-1 * TotAdmincommession));
                    }
                    objAccessrightsbyUserType.NetBalance = a.ToString();
                }
                else
                {
                    if (typeID == 3)
                    {
                         objUsersServiceCleint.GetProfitorLossbyUserID(userID, false, password).ToString();
                    }

                    else
                    {
                        if (typeID == 2)
                        {
                            
                            decimal TotAdminAmount = 0;
                            decimal TotalAdminAmountWithoutMarkets = 0;
                            decimal SuperAmount = 0;
                            decimal SuperAmount1 = 0;
                            List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(userID, false, password));
                            if (lstUserAccountsForAgent.Count > 0)
                            {
                                lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                                foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                                {
                                    if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                                    {
                                        int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                        int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                        int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                        decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                        decimal superpercent = 0;
                                        if (SuperRate > 0)
                                        {
                                            superpercent = SuperRate - AgentRate;
                                        }
                                        else
                                        {
                                            superpercent = 0;
                                        }
                                        if (ActualAmount > 0)
                                        {
                                            SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                            decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                            decimal Comissionamount = 0;
                                            if (AgentRate == 100)
                                            {
                                                Comissionamount = 0;
                                            }
                                            else
                                            {
                                                Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                                            }

                                            TotAdminAmount += -1 * (ActualAmount - (AgentAmount + SuperAmount) - Comissionamount);
                                            SuperAmount1 += -1 * SuperAmount;
                                        }
                                        else
                                        {
                                            ActualAmount = -1 * ActualAmount;
                                            SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                            decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                            TotAdminAmount += ActualAmount - (AgentAmount + SuperAmount);
                                            SuperAmount1 += SuperAmount;
                                        }
                                    }
                                }
                            }
                            int createdbyid = objUsersServiceCleint.GetCreatedbyID(userID);
                            string a = "";
                            List<UserAccounts> lstAccountsDonebyAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(createdbyid, false, password));
                            if (lstAccountsDonebyAdmin.Count > 0)
                            {
                                List<UserAccounts> lstAccountsDonebyAdminagainstthisAgent = lstAccountsDonebyAdmin.Where(item => item.AccountsTitle.Contains("(UserID=" + userID.ToString() + ")")).ToList();
                                if (lstAccountsDonebyAdminagainstthisAgent.Count > 0)
                                {
                                    TotalAdminAmountWithoutMarkets = lstAccountsDonebyAdminagainstthisAgent.Sum(item => Convert.ToDecimal(item.Debit)) - lstAccountsDonebyAdminagainstthisAgent.Sum(item => Convert.ToDecimal(item.Credit));
                                }
                            }
                            decimal AgentCommission = 0;
                            decimal superCommission = 0;
                            try
                            {
                                AgentCommission = objUsersServiceCleint.GetTotalAgentCommissionbyAgentID(userID, password);
                            }
                            catch (System.Exception ex)
                            {
                            }
                            if (LoggedinUserDetail.IsCom == true)
                            {
                                a = ((-1 * (TotAdminAmount) + (-1 * TotalAdminAmountWithoutMarkets) + (-1 * (SuperAmount1)))).ToString();
                                //AgentCommission;
                            }
                            else
                            {
                                a = ((-1 * (TotAdminAmount) + (-1 * TotalAdminAmountWithoutMarkets) + (-1 * (SuperAmount1))) + AgentCommission).ToString();
                            }

                             //a;
                        }
                        else
                        {
                            if (typeID == 9)
                            {
                                
                                decimal TotAdminAmount = 0;
                                decimal TotAdmincommession = 0;
                                decimal TotalAdminAmountWithoutMarkets = 0;
                                List<UserAccounts> AgentCommission = new List<UserAccounts>();
                                List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByIDForSamiAdmin(userID, false, password));
                                if (lstUserAccountsForAgent.Count > 0)
                                {
                                    AgentCommission = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle == "Commission").ToList();
                                    lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                                    foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                                    {
                                        if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                                        {
                                            int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                            int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                            int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                            int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiadminRate);
                                            decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                            decimal superpercent = SuperRate - AgentRate;
                                            decimal samiadminpercent = SamiadminRate - (superpercent + AgentRate);
                                            if (ActualAmount > 0)
                                            {
                                                decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                                decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                decimal Comissionamount = 0;
                                                if (AgentRate == 100)
                                                {
                                                    Comissionamount = 0;
                                                }
                                                else
                                                {
                                                    Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                                                }

                                                TotAdminAmount += -1 * (ActualAmount - SuperAmount - AgentAmount - SamiadminAmount);
                                            }
                                            else
                                            {
                                                ActualAmount = -1 * ActualAmount;
                                                decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                                decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                TotAdminAmount += ActualAmount - AgentAmount - SuperAmount - SamiadminAmount;
                                            }
                                        }
                                    }
                                }

                                try
                                {
                                    foreach (UserAccounts objuserAccounts in AgentCommission)
                                    {
                                        int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                        int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                        int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                        int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiadminRate);
                                        decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                        decimal superpercent = SuperRate - AgentRate;
                                        decimal samiadminpercent = SamiadminRate - (superpercent + AgentRate);
                                        ActualAmount = -1 * ActualAmount;
                                        decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                        decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                        decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                        TotAdmincommession += ActualAmount - AgentAmount - SuperAmount - SamiadminAmount;
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                }
                                decimal a = (-1 * (TotAdminAmount) + (-1 * TotAdmincommession));

                                 a.ToString();
                            }
                        }

                    }
                }
            

            return objAccessrightsbyUserType;
        }

        public IActionResult UserDetails(int userID, int typeID,string password)
        {
            if (userID > 0)
            {

                UserDetails objuserDetails = getDetails(userID, typeID, password);
                if (typeID == 1)
                {
                    List<UserMarketSelection> lstAssignedMarket = JsonConvert.DeserializeObject<List<UserMarketSelection>>(objUsersServiceCleint.GetUserMarketforSelection(userID));
                    string[] AssingedMarket = lstAssignedMarket.Select(i => i.Items).Distinct().ToArray();
                    foreach (var item in AssingedMarket)
                    {
                        if (objuserDetails.AssignedMarket == null)
                        {
                            objuserDetails.AssignedMarket = item;
                        }
                        else
                        {
                            objuserDetails.AssignedMarket = objuserDetails.AssignedMarket + "," + item;
                        }

                    }

                }

                return Ok(objuserDetails);
            }
            else
            {
                UserDetails objuserDetails = new Models.UserDetails();
                return Ok(objuserDetails);
            }
        }

        public UserDetails getDetails(int UserID, int typeID,string password)
        {
            try
            {               
                var results = JsonConvert.DeserializeObject<UserDetails>(objUsersServiceCleint.GetUserDetailsbyID(UserID, password));
                UserDetails objuserDetails = new Models.UserDetails();
                objuserDetails = results;
                objuserDetails.Username = Crypto.Decrypt(objuserDetails.Username);
                objuserDetails.Password = Crypto.Decrypt(objuserDetails.Password);
                objuserDetails.RatePercent = Crypto.Decrypt(objuserDetails.RatePercent);

                if (typeID != 1)
                {
                }
                if (objuserDetails.isBlocked == true)
                {
                    objuserDetails.Status = "Blocked";
                }
                else
                {
                    if (objuserDetails.isDeleted == true)
                    {
                        objuserDetails.Status = "Deleted";
                    }
                    else
                    {
                        objuserDetails.Status = "Active";
                    }
                }

                return objuserDetails;
            }
            catch (System.Exception ex)
            {
                return new Models.UserDetails();
            }
        }

        
        [HttpGet]
        public IActionResult ManageUser(int typeID)
        {
            
            ManageUser user = new Models.ManageUser();
            objAccessrightsbyUserType = new AccessRightsbyUserType();
            objAccessrightsbyUserType = JsonConvert.DeserializeObject<AccessRightsbyUserType>(objUsersServiceCleint.GetAccessRightsbyUserType(typeID, LoggedinUserDetail.PasswordForValidate));
            user.CanBlockUser = objAccessrightsbyUserType.CanBlockUser;
            user.CanDeleteUser = objAccessrightsbyUserType.CanDeleteUser;
            user.CanChangeAgentRate = objAccessrightsbyUserType.CanChangeAgentRate;
            user.CanChangeLoggedIN = objAccessrightsbyUserType.CanChangeLoggedIN;

            return Ok(user);
        }
        [HttpGet]
        public IActionResult AllUsers(int userID, int typeID, string password)
        {
            List<UserIDandUserType> lstUsers= GetUsersbyUsersType(userID, typeID, password);
            return Ok(lstUsers);
        }

        public List<UserIDandUserType> GetUsersbyUsersType(int userID,int typeID,string password)
        {          

            var results = "";
            if (typeID != 3)
            {
                results = objUsersServiceCleint.GetAllUsersbyUserType(userID, typeID, password);
            }

            if (results != "")
            {

                List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);

                foreach (UserIDandUserType objuser in lstUsers)
                {
                    objuser.UserName = Crypto.Decrypt(objuser.UserName);
                    objuser.UserName = objuser.UserName + " (" + objuser.UserType + ")";
                }
                UserIDandUserType userdefult = new UserIDandUserType();
                userdefult.ID = 0;
                userdefult.UserName = "Please Select";
                lstUsers.Insert(0, userdefult);
                return lstUsers;

            }

            else
            {
                List<UserIDandUserType> lstUsers = new List<UserIDandUserType>();
                return lstUsers;
            }
        }
        public List<UserIDandUserType> GetUsersbyUsersType(int userID,int typeID)
        {
           
            var results = "";
            if (typeID != 3)
            {
                results = objUsersServiceCleint.GetAllUsersbyUserType(userID, typeID, LoggedinUserDetail.PasswordForValidate);
            }

            if (results != "")
            {

                List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);

                foreach (UserIDandUserType objuser in lstUsers)
                {
                    objuser.UserName = Crypto.Decrypt(objuser.UserName);
                    objuser.UserName = objuser.UserName + " (" + objuser.UserType + ")";
                }
                UserIDandUserType userdefult = new UserIDandUserType();
                userdefult.ID = 0;
                userdefult.UserName = "Please Select";
                lstUsers.Insert(0, userdefult);
                return lstUsers;

            }

            else
            {
                List<UserIDandUserType> lstUsers = new List<UserIDandUserType>();
                return lstUsers;
            }
        }
        [HttpGet]
        public IActionResult CuttingUsers()
        {
            List<CuttingUsers> lstusers = GetAllCuttingUsers();
            return Ok(lstusers);
        }
        public List<CuttingUsers> GetAllCuttingUsers()
        {
            
            var results = objUsersServiceCleint.GetAllCuttingUsers(LoggedinUserDetail.PasswordForValidate);
            if (results != "")
            {
                List<CuttingUsers> lstUsers = JsonConvert.DeserializeObject<List<CuttingUsers>>(results);

                foreach (CuttingUsers objuser in lstUsers)
                {
                    objuser.username = Crypto.Decrypt(objuser.username);

                }
                CuttingUsers userdefult = new CuttingUsers();
                userdefult.ID = 0;
                userdefult.username = "Please Select";
                lstUsers.Insert(0, userdefult);
                return lstUsers;

            }
            else
            {
                List<CuttingUsers> lstUsers = new List<CuttingUsers>();
                return lstUsers;
            }
        }
        public string GetUser(CreateUser User,int userID,int typeID)
        {
            if (!User.IsValid())
            {
                return "Enter correct fields.";
            }
            
            var result = objUsersServiceCleint.CheckifUserExists(Crypto.Encrypt(User.UserName.ToLower()));
            if (result == "0")
            {
                int CreatedbyID = userID;
                string AccountBalance = objUsersServiceCleint.GetCurrentBalancebyUser(CreatedbyID, LoggedinUserDetail.PasswordForValidate);
                Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);

                if (typeID == 2)
                {
                    int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(CreatedbyID);
                    int MaxbalancetransferLimit = objUsersServiceCleint.GetMaxBalanceTransferLimit(CreatedbyID);
                    int MaxagentrateLimit = objUsersServiceCleint.GetMaxAgentRate(CreatedbyID);
                    if (Convert.ToInt32(User.AgentRateC) > MaxagentrateLimit)
                    {
                        return "Agent Rate cannot greater than " + MaxagentrateLimit.ToString() + " %.";
                    }

                    if (User.AccountBalance > MaxbalancetransferLimit && userID != 73)
                    {
                        return "Account Balance should be not be greater than " + MaxbalancetransferLimit.ToString();
                    }
                    if (Newaccountbalance < User.AccountBalance)
                    {
                        return "Account Balance is more than available balance";
                    }
                    else
                    {
                        string userid = objUsersServiceCleint.AddUser(User.Name, User.PhoneNumber, User.EmailAddress, Crypto.Encrypt(User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(User.UserTypeID), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                        objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + User.UserName.ToString() + ")", "0.00", User.AccountBalance.ToString(), HawalaID, "", DateTime.Now, "", "", "", Newaccountbalance, false, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, User.AccountBalance, LoggedinUserDetail.PasswordForValidate);

                        List<AllUserMarkets> lstUserMarket = JsonConvert.DeserializeObject<List<AllUserMarkets>>(objUsersServiceCleint.GetAllUserMarketbyUserID(CreatedbyID));
                        if (lstUserMarket.Count > 0)
                        {
                            List<string> allusersmarket = new List<string>();
                            foreach (var usermarket in lstUserMarket)
                            {
                                var objusermarket = usermarket.EventTypeID.ToString() + "#" + usermarket.EventTypeName.ToString() + "#" + usermarket.CompetitionID.ToString() + "#" + usermarket.CompetitionName.ToString() + "#" + usermarket.EventID.ToString() + "#" + usermarket.EventName.ToString() + "#" + usermarket.MarketCatalogueID.ToString() + "#" + usermarket.MarketCatalogueName + "#NotInsert#" + usermarket.EventOpenDate.ToString();
                                allusersmarket.Add(objusermarket);
                            }
                            objUsersServiceCleint.InsertUserMarket(allusersmarket, Convert.ToInt32(userid), CreatedbyID, false);
                        }
                        LoggedinUserDetail.InsertActivityLog(CreatedbyID, "Created new user (" + User.UserName.ToString() + ")");
                        return "True";
                    }
                }
                else
                {
                    if (typeID == 8)
                    {

                        int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(CreatedbyID);
                        int MaxbalancetransferLimit = objUsersServiceCleint.GetMaxBalanceTransferLimit(CreatedbyID);
                        int MaxagentrateLimit = objUsersServiceCleint.GetMaxAgentRate(CreatedbyID);

                        if (Convert.ToInt32(User.AgentRateC) > MaxagentrateLimit)
                        {
                            return "Agent Rate cannot greater than " + MaxagentrateLimit.ToString() + " %.";
                        }

                        if (User.AccountBalance > MaxbalancetransferLimit && userID != 73)
                        {
                            return "Account Balance should be not be greater than " + MaxbalancetransferLimit.ToString();
                        }
                        if (Newaccountbalance < User.AccountBalance)
                        {
                            return "Account Balance is more than available balance";
                        }

                        string userid = "0";
                        if (Convert.ToInt32(User.UserTypeID) == 4)
                        {
                            userid = objUsersServiceCleint.AddUser(User.Name, User.PhoneNumber, User.EmailAddress, Crypto.Encrypt(User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(User.UserTypeID), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                        }
                        else
                        {
                            userid = objUsersServiceCleint.AddUser(User.Name, User.PhoneNumber, User.EmailAddress, Crypto.Encrypt(User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(User.UserTypeID), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                        }
                        if (Convert.ToInt32(User.UserTypeID) == 2)
                        {
                            try
                            {
                                List<AllUserMarkets> lstUserMarket = JsonConvert.DeserializeObject<List<AllUserMarkets>>(objUsersServiceCleint.GetAllUserMarketbyUserID(73));
                                if (lstUserMarket.Count > 0)
                                {
                                    List<string> allusersmarket = new List<string>();
                                    foreach (var usermarket in lstUserMarket)
                                    {
                                        var orignalopendate = Convert.ToDateTime(usermarket.EventOpenDate).ToString("s");
                                        var objusermarket = usermarket.EventTypeID.ToString() + "#" + usermarket.EventTypeName.ToString() + "#" + usermarket.CompetitionID.ToString() + "#" + usermarket.CompetitionName.ToString() + "#" + usermarket.EventID.ToString() + "#" + usermarket.EventName.ToString() + "#" + usermarket.MarketCatalogueID.ToString() + "#" + usermarket.MarketCatalogueName + "#NotInsert#" + usermarket.EventOpenDate.ToString();
                                        allusersmarket.Add(objusermarket);
                                    }
                                    objUsersServiceCleint.InsertUserMarket(allusersmarket, Convert.ToInt32(userid), CreatedbyID, false);
                                }
                            }
                            catch (System.Exception ex)
                            {

                            }

                            string useridHawala = objUsersServiceCleint.AddUser("Hawala", User.PhoneNumber, User.EmailAddress, Crypto.Encrypt("Hawala" + User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(7), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                            objUsersServiceCleint.UpdateHawalaIDbyUserID(Convert.ToInt32(useridHawala), Convert.ToInt32(userid));
                            objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + User.UserName.ToString() + ")", "0.00", User.AccountBalance.ToString(), HawalaID, "", DateTime.Now, "", "", "", Newaccountbalance, false, "", "", "", "", "");
                            objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, User.AccountBalance, LoggedinUserDetail.PasswordForValidate);
                        }

                        LoggedinUserDetail.InsertActivityLog(CreatedbyID, "Created new user (" + User.UserName + ")");

                        return "True" + "|" + userid.ToString();
                    }


                    else
                    {
                        string userid = objUsersServiceCleint.AddUser(User.Name, User.PhoneNumber, User.EmailAddress, Crypto.Encrypt(User.UserName.ToLower()), Crypto.Encrypt(User.Password), User.Location, User.AccountBalance, Convert.ToInt32(User.UserTypeID), CreatedbyID, Crypto.Encrypt(User.AgentRateC), User.BetLowerLimit, User.BetUpperLimit, true, User.BetLowerLimitHorsePlace, User.BetUpperLimitHorsePlace, User.BetLowerLimitGrayHoundWin, User.BetUpperLimitGrayHoundWin, User.BetLowerLimitGrayHoundPlace, User.BetUpperLimitGrayHoundPlace, User.BetLowerLimitMatchOdds, User.BetUpperLimitMatchOdds, User.BetLowerLimitInningRuns, User.BetUpperLimitInningRuns, User.BetLowerLimitCompletedMatch, User.BetUpperLimitCompletedMatch, User.BetLowerLimitMatchOddsSoccer, User.BetUpperLimitMatchOddsSoccer, User.BetLowerLimitMatchOddsTennis, User.BetUpperLimitMatchOddsTennis, User.BetUpperLimitTiedMatch, User.BetLowerLimitTiedMatch, User.BetUpperLimitWinner, User.BetLowerLimitWinner, LoggedinUserDetail.PasswordForValidate, 5000, 1000);
                       
                        objAccountsService.AddtoUsersAccounts("Amount removed from your account (User created " + User.UserName.ToString() + ")", "0.00", User.AccountBalance.ToString(), CreatedbyID, "", DateTime.Now, "", "", "", Newaccountbalance, false, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(CreatedbyID, User.AccountBalance, LoggedinUserDetail.PasswordForValidate);

                        LoggedinUserDetail.InsertActivityLog(CreatedbyID, "Created new user (" + User.UserName.ToString() + ")");
                        return "True" + "|" + userid.ToString();
                    }

                }
            }
            else
            {
                return "Username already exists.";
            }


        }
        public bool CheckUsername(string UserName)
        {
          
            var result = objUsersServiceCleint.CheckifUserExists(Crypto.Encrypt(UserName));
            if (result == "0")
            {
                return true;
            }
            else { return false; }
        }

        public string AddCredittoUser(string AccountBalance, int UserID, string Username, string AccountsTitle, bool IsCredit,int typeID, int addedbyID)
        {          
            int AddedbyID = addedbyID;
            Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);

            int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
            int MaxbalancetransferLimit = objUsersServiceCleint.GetMaxBalanceTransferLimit(AddedbyID);
            Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, LoggedinUserDetail.PasswordForValidate));
            if (typeID == 2)
            {
                decimal UserCurrentBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserID, LoggedinUserDetail.PasswordForValidate));
                if ((UserCurrentBalance + Newaccountbalance) > MaxbalancetransferLimit && addedbyID != 73)
                {
                    return "Account Balance should be not be greater than " + MaxbalancetransferLimit.ToString();
                }
                if (Newaccountbalance > CurrentAccountBalance)
                {
                    return "Account Balance is more than available balance.";
                }
                else
                {
                    objUsersServiceCleint.AddCredittoUser(Newaccountbalance, UserID, HawalaID, DateTime.Now, 0, true, AccountsTitle, IsCredit, LoggedinUserDetail.PasswordForValidate);
                    if (IsCredit == true)
                    {
                        objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                        // objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID, Newaccountbalance);
                    }

                    objAccountsService.AddtoUsersAccounts("Amount removed from your account ( " + AccountsTitle + " for " + Username + " )", "0.00", Newaccountbalance.ToString(), HawalaID, "", DateTime.Now, "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");
                    objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);

                    LoggedinUserDetail.InsertActivityLog(HawalaID, "Added Balance " + Newaccountbalance.ToString() + "  to user ( " + Username + " )");
                    return "True";
                }

            }
            else
            {
                if (typeID == 8)
                {


                    int HawalaIDSuper = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                    HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                    Newaccountbalance = Convert.ToDecimal(AccountBalance);
                    CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(AddedbyID, LoggedinUserDetail.PasswordForValidate));
                    objUsersServiceCleint.AddCredittoUser(Newaccountbalance, UserID, AddedbyID, DateTime.Now, 0, true, AccountsTitle, IsCredit, LoggedinUserDetail.PasswordForValidate);
                    if (HawalaID > 0)
                    {
                        objUsersServiceCleint.AddCredittoUser(Newaccountbalance, HawalaID, AddedbyID, DateTime.Now, 0, true, AccountsTitle, IsCredit, LoggedinUserDetail.PasswordForValidate);
                    }
                    if (IsCredit == true)
                    {

                        objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                        if (HawalaID > 0)
                        {
                            objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                        }
                        //  objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID, Newaccountbalance);
                    }
                    objAccountsService.AddtoUsersAccounts("Amount removed from your account( " + AccountsTitle + " for " + Username + "(UserID=" + UserID.ToString() + ") )", "0.00", Newaccountbalance.ToString(), AddedbyID, "", DateTime.Now, "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");
                    objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaIDSuper, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                    LoggedinUserDetail.InsertActivityLog(AddedbyID, "Added Balance " + Newaccountbalance.ToString() + " to user ( " + Username + " )");
                    return "True";
                }


                else
                {

                    objUsersServiceCleint.AddCredittoUser(Newaccountbalance, UserID, AddedbyID, DateTime.Now, 0, true, AccountsTitle, IsCredit, LoggedinUserDetail.PasswordForValidate);
                    if (IsCredit == true)
                    {

                        objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                        //  objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID, Newaccountbalance);
                    }
                    objAccountsService.AddtoUsersAccounts("Amount removed from your account( " + AccountsTitle + " for " + Username + " )", "0.00", Newaccountbalance.ToString(), AddedbyID, "", DateTime.Now, "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");
                    objUsersServiceCleint.UpdateAccountBalacnebyUser(AddedbyID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                    LoggedinUserDetail.InsertActivityLog(AddedbyID, "Added Balance " + Newaccountbalance.ToString() + " to user ( " + Username + " )");
                    return "True";

                }
            }
        }

        public string RemoveCredittoUser(string AccountBalance, int UserID, string Username, string AccountsTitle, bool IsCredit,int typeId,int addedbyID)
        {
            
            if (AccountBalance != "")
            {
                if (UserID > 0)
                {
                    int AddedbyID = addedbyID;

                    if (typeId == 2)
                    {

                        Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);
                        int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                        Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, LoggedinUserDetail.PasswordForValidate));
                        Decimal CurrentAccountBalanceofuser = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserID, LoggedinUserDetail.PasswordForValidate));
                        Decimal AlreadyBalance = Convert.ToDecimal(CurrentAccountBalance);
                        if (Newaccountbalance > CurrentAccountBalanceofuser)
                        {

                            return ("Amount is greater than current balance.");
                        }
                        objUsersServiceCleint.AddCredittoUser(0, UserID, AddedbyID, DateTime.Now, Newaccountbalance, true, AccountsTitle, IsCredit, LoggedinUserDetail.PasswordForValidate);

                        objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + AccountsTitle + " for " + Username + " )", Newaccountbalance.ToString(), "0.00", HawalaID, "", DateTime.Now, "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");
                        objUsersServiceCleint.UpdateAccountBalacnebyUser(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                        if (IsCredit == true)
                        {

                            objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            // objUsersServiceCleint.UpdateAccountsOpeningBalance(UserID,-1* Newaccountbalance);
                        }
                        LoggedinUserDetail.InsertActivityLog(AddedbyID, "Removed Balance " + Newaccountbalance.ToString() + "  from user ( " + Username + " )");
                        if (typeId == 2)
                        {
                            objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            LoggedinUserDetail.InsertActivityLog(HawalaID, "Added Balance " + Newaccountbalance.ToString() + "  to user ( " + Username + " )");
                        }
                        return "True";

                    }
                    else
                    {
                        if (typeId == 8)
                        {

                            int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                            int HawalaIDSuper = objUsersServiceCleint.GetHawalaAccountIDbyUserID(AddedbyID);
                            Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);
                            Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserID, LoggedinUserDetail.PasswordForValidate));
                            
                            Decimal AlreadyBalance = Convert.ToDecimal(CurrentAccountBalance);
                            if (Newaccountbalance > AlreadyBalance)
                            {

                                return ("Amount is greater than current balance.");

                            }
                            objUsersServiceCleint.AddCredittoUser(0, UserID, AddedbyID, DateTime.Now, Newaccountbalance, true, AccountsTitle, IsCredit, LoggedinUserDetail.PasswordForValidate);
                            objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + AccountsTitle + " for " + Username + "(UserID=" + UserID.ToString() + ") )", Newaccountbalance.ToString(), "0.00", AddedbyID, "", DateTime.Now, "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");
                            objUsersServiceCleint.UpdateAccountBalacnebyUser(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            if (HawalaID > 0)
                            {
                                objUsersServiceCleint.AddCredittoUser(0, HawalaID, AddedbyID, DateTime.Now, Newaccountbalance, true, AccountsTitle, IsCredit, LoggedinUserDetail.PasswordForValidate);

                                objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            }
                            if (IsCredit == true)
                            {

                                objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                                if (HawalaID > 0)
                                {
                                    objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                                }
                              }
                            objUsersServiceCleint.UpdateAccountBalacnebyUser(HawalaIDSuper, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            LoggedinUserDetail.InsertActivityLog(AddedbyID, "Removed Balance " + Newaccountbalance.ToString() + "  from user ( " + Username + " )");
                            return "True";
                        }

                        else
                        {
                            int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                            Decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, LoggedinUserDetail.PasswordForValidate));
                            Decimal Newaccountbalance = Convert.ToDecimal(AccountBalance);
                            objUsersServiceCleint.AddCredittoUser(0, UserID, AddedbyID, DateTime.Now, Newaccountbalance, true, AccountsTitle, IsCredit, LoggedinUserDetail.PasswordForValidate);
                            objAccountsService.AddtoUsersAccounts("Amount Added to your account( " + AccountsTitle + " for " + Username + " )", Newaccountbalance.ToString(), "0.00", AddedbyID, "", DateTime.Now, "", "", "", CurrentAccountBalance, IsCredit, "", "", "", "", "");

                            objUsersServiceCleint.UpdateAccountBalacnebyUser(UserID, Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            if (IsCredit == true)
                            {

                                objUsersServiceCleint.UpdateStartBalancebyUserID(UserID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                                if (HawalaID > 0)
                                {
                                    objUsersServiceCleint.UpdateStartBalancebyUserID(HawalaID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                                }
                                
                            }
                            objUsersServiceCleint.UpdateAccountBalacnebyUser(AddedbyID, -1 * Newaccountbalance, LoggedinUserDetail.PasswordForValidate);
                            LoggedinUserDetail.InsertActivityLog(AddedbyID, "Removed Balance " + Newaccountbalance.ToString() + "  from user ( " + Username + " )");
                            return "True";
                        }
                    }
                }
                else
                {
                    return "False";
                }

            }
            else
            {
                return "Please enter amount.";
            }
        }

        public string UpdateUserPasswordandStatus(int UserID, bool IsDeleted, bool isBlocked, string Password, string AgentRate, bool LoggedIn, decimal BetLowerLimit, decimal BetUpperLimit, bool isAllowedGrayHound, bool isAllowedHorse, decimal BetLowerLimitHorsePlace, decimal BetUpperLimitHorsePlace, decimal BetLowerLimitGrayHoundWin, decimal BetUpperLimitGrayHoundWin, decimal BetLowerLimitGrayHoundPlace, decimal BetUpperLimitGrayHoundPlace, decimal BetLowerLimitMatchOdds, decimal BetUpperLimitMatchOdds, decimal BetLowerLimitInningsRunns, decimal BetUpperLimitInningsRunns, decimal BetLowerLimitCompletedMatch, decimal BetUpperLimitCompletedMatch, bool isTennisAllowed, bool isSoccerAllowed, int CommissionRate, decimal BetLowerLimitMatchOddsSoccer, decimal BetUpperLimitMatchOddsSoccer, decimal BetLowerLimitMatchOddsTennis, decimal BetUpperLimitMatchOddsTennis, decimal BetUpperLimitTiedMatch, decimal BetLowerLimitTiedMatch, decimal BetUpperLimitWinner, decimal BetLowerLimitWinner,int typeID, int updatedBy)
        {

           
            if (UserID > 0)
            {
                int UpdatedBy = updatedBy;
                if (UpdatedBy > 0)
                {
                    if (AgentRate == "")
                    {
                        AgentRate = "0";

                    }
                    if (typeID == 2 || typeID == 8)
                    {
                        if (Convert.ToInt32(AgentRate) > 50)
                        {
                            return "Agent Rate cannot greater than 50 %.";
                        }
                    }
                    AgentRate = Crypto.Encrypt(AgentRate);
                    DateTime updatedtime = DateTime.Now;
                    objUsersServiceCleint.SetBlockedStatusofUser(UserID, isBlocked, LoggedinUserDetail.PasswordForValidate);
                    LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Block Status");
                    if (typeID == 1)
                    {
                        objUsersServiceCleint.SetDeleteStatusofUser(UserID, IsDeleted, LoggedinUserDetail.PasswordForValidate);
                        LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Delete Status");
                    }
                    if (Password.Length >= 6)
                    {
                        objUsersServiceCleint.ResetPasswordofUser(UserID, Crypto.Encrypt(Password), UpdatedBy, updatedtime, LoggedinUserDetail.PasswordForValidate);
                        LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Update User Password");
                    }
                    objUsersServiceCleint.SetAgentRateofUser(UserID, AgentRate, LoggedinUserDetail.PasswordForValidate);
                    LoggedinUserDetail.InsertActivityLog(UpdatedBy, "Set Rate of User " + UserID.ToString() + " " + Crypto.Decrypt(AgentRate).ToString());
                    objUsersServiceCleint.SetLoggedinStatus(UserID, LoggedIn);
                    if (typeID == 1)
                    {
                        objUsersServiceCleint.UpdateBetLowerLimit(UserID, BetLowerLimit, BetUpperLimit, isAllowedGrayHound, isAllowedHorse, BetLowerLimitHorsePlace, BetUpperLimitHorsePlace, BetLowerLimitGrayHoundWin, BetUpperLimitGrayHoundWin, BetLowerLimitGrayHoundPlace, BetUpperLimitGrayHoundPlace, BetLowerLimitMatchOdds, BetUpperLimitMatchOdds, BetLowerLimitInningsRunns, BetUpperLimitInningsRunns, BetLowerLimitCompletedMatch, BetUpperLimitCompletedMatch, isTennisAllowed, isSoccerAllowed, CommissionRate, BetLowerLimitMatchOddsSoccer, BetUpperLimitMatchOddsSoccer, BetLowerLimitMatchOddsTennis, BetUpperLimitMatchOddsTennis, BetUpperLimitTiedMatch, BetLowerLimitTiedMatch, BetUpperLimitWinner, BetLowerLimitWinner, LoggedinUserDetail.PasswordForValidate, 5000, 2000);
                    }
                    return "True";
                }
                else
                { return "False"; }
            }
            else { return "False"; }
        }

        //public PartialViewResult PoundRate()
        //{
        //    if (typeID == 1)
        //    {
        //        CurrencryConverterService objcurrency = new CurrencryConverterService();
        //        APIConfigServiceReference.APIConfigServiceClient objAPIConfig = new APIConfigServiceReference.APIConfigServiceClient();
        //        ViewBag.PoundRate = Convert.ToDecimal(Crypto.Decrypt(objAPIConfig.GetPoundRate()));
        //        ViewBag.CurrentRate = objcurrency.ConvertCurrencyRates(1).ToString("F2");
        //        return PartialView("PoundRate");
        //    }
        //    else
        //    {
        //        ViewBag.PoundRate = "";
        //        return PartialView("PoundRate");
        //    }

        //}
        public string ResetPoundRate(string Rate,int typeID)
        {
            if (typeID == 1)
            {
                if (Rate != "")
                {
                    APIConfigServiceReference.APIConfigServiceClient objAPIConfig = new APIConfigServiceReference.APIConfigServiceClient();
                    //objAPIConfig.SetPoundRate(Crypto.Encrypt(Rate));
                    return "True";
                }
                else
                {
                    return "Please enter amount";
                }
            }
            else
            {
                return "False";
            }
        }
        public string ConverttoJSONString(UserDetails result)
        {

            if (result != null)
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(result.GetType());
                MemoryStream memoryStream = new MemoryStream();
                serializer.WriteObject(memoryStream, result);

                // Return the results serialized as JSON
                string json = Encoding.Default.GetString(memoryStream.ToArray());
                return json;
            }
            else
            {
                return "";
            }
        }
        public string ConvertDateFormat(string datetoconvert)
        {
            string[] datearr = datetoconvert.Split('-');
            datetoconvert = datearr[2].ToString() + "-" + datearr[1].ToString() + "-" + datearr[0].ToString();
            return datetoconvert;
        }
        [HttpGet]
        public IActionResult ProfitandLoss(string DateFrom, string DateTo, int UserID, bool chkseassion, bool chkfancy, bool chkByMarket, bool chkByMarketCricket,int userID, int typeID)
        {
            DateFrom = ConvertDateFormat(DateFrom);
            DateTo = ConvertDateFormat(DateTo);

            if (chkfancy == true && chkseassion == true)
            {
                Getdataforfancybysession(DateFrom, DateTo, chkfancy, userID,typeID);

                return Ok(lstProfitandLossAll);
            }
            

            if (chkByMarket == false)
            {
                List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();
                if (typeID == 1)
                {
                    SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(UserID);
                    lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                    lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.CommisionAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                    objProfitandLossCommission.EventType = "Commission";
                    objProfitandLossCommission.NetProfitandLoss = lstProfitandlossEventtypeCommission.Sum(item => item.NetProfitandLoss);
                    lstProfitandlossEventtype.Add(objProfitandLossCommission);
                }
                else
                {
                    if (typeID == 8)
                    {
                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRange(UserID, DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                        List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                        lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.UserAccountsGetCommission(UserID, DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                        ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                        objProfitandLossCommission.EventType = "Commission";
                        objProfitandLossCommission.NetProfitandLoss = lstProfitandlossEventtypeCommission.Sum(item => item.NetProfitandLoss);
                        lstProfitandlossEventtype.Add(objProfitandLossCommission);
                    }
                    else
                    {
                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(UserID, DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    }
                }

                if (lstProfitandlossEventtype.Count > 0)
                {
                    if (chkfancy == true)
                    {
                        lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType.Contains("Fancy")).ToList();
                    }
                    if (chkByMarketCricket == true)
                    {
                        lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType == ("Cricket") || item.EventType == ("Fancy")).ToList();
                    }
                    // ViewBag.NetProfitorLoss1 = lstProfitandlossEventtype.Where(item => item.EventType != "Commission").Sum(item => item.NetProfitandLoss).ToString("N0");

                }
                else
                {

                }
                lstProfitandLossAll = lstProfitandlossEventtype;
            }
            else
            {
                try
                {

                    List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();
                    if (typeID == 1)
                    {
                        SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(UserID);
                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                        List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                        lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.CommisionAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                        ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                        foreach (var item in lstProfitandlossEventtypeCommission)
                        {
                            item.EventType = item.EventType + " (Commission)";
                        }

                        lstProfitandlossEventtype.AddRange(lstProfitandlossEventtypeCommission);
                    }

                    if (typeID == 8)
                    {
                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRange(UserID, DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                        List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                        lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.UserAccountsGetCommission(UserID, DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                        ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                        foreach (var item in lstProfitandlossEventtypeCommission)
                        {
                            item.EventType = item.EventType + " (Commission)";
                        }
                        lstProfitandlossEventtype.AddRange(lstProfitandlossEventtypeCommission);
                    }
                    else
                    {
                        lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRange(UserID, DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    }
                    if (lstProfitandlossEventtype.Count > 0)
                    {
                        if (chkfancy == true)
                        {
                            lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType.Contains("Fancy")).ToList();
                        }
                        if (chkByMarketCricket == true)
                        {
                            lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.Eventtype1 == ("Cricket") || item.Eventtype1 == ("Fancy")).ToList();
                        }
                    }
                    else
                    {

                    }
                    lstProfitandLossAll = lstProfitandlossEventtype;
                }
                catch (System.Exception ex)
                {
                }
            }
            return Ok(lstProfitandLossAll);
        }
        public void Getdataforfancybysession(string DateFrom, string DateTo, bool chkfancy,int userID,int typeID)
        {
            try
            {
                List<ProfitandLossEventType> lstProfitandlossEventtype = new List<ProfitandLossEventType>();
                if (typeID == 1)
                {
                    SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = objUsersServiceCleint.GetCommissionaccountIdandBookAccountbyUserID(userID);
                    lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRangeFancywithMArketName(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    List<ProfitandLossEventType> lstProfitandlossEventtypeCommission = new List<ProfitandLossEventType>();
                    lstProfitandlossEventtypeCommission = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventtypeuserIDandDateRange(Convert.ToInt32(objCommissionandBookAccountID.CommisionAccountID), DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                    lstProfitandlossEventtypeCommission = lstProfitandlossEventtypeCommission.Where(item => item.EventType.Contains("Fancy")).ToList();
                    ProfitandLossEventType objProfitandLossCommission = new ProfitandLossEventType();
                    foreach (var item in lstProfitandlossEventtypeCommission)
                    {
                        item.EventType = item.EventType + " (Commission)";
                    }

                    lstProfitandlossEventtype.AddRange(lstProfitandlossEventtypeCommission);
                }
                else
                {
                    lstProfitandlossEventtype = JsonConvert.DeserializeObject<List<ProfitandLossEventType>>(objUsersServiceCleint.GetAccountsDatabyEventNameuserIDandDateRangeFancywithMArketName(userID, DateFrom, DateTo, LoggedinUserDetail.PasswordForValidate));
                }


                if (lstProfitandlossEventtype.Count > 0)
                {
                    if (chkfancy == true)
                    {
                        lstProfitandlossEventtype = lstProfitandlossEventtype.Where(item => item.EventType.Contains("Fancy")).ToList();
                    }
                    lstProfitandlossEventtype = lstProfitandlossEventtype.OrderBy(o => o.EventID).ThenBy(o => o.EventType).ToList();

                    lstProfitandLossAll = lstProfitandlossEventtype;
                }
                else
                {

                }
            }
            catch (System.Exception ex)
            {

            }
        }
        [HttpGet]
        public IActionResult showcompletedevent(string DateFrom, string DateTo, string useridd, string eventtype,int userID,int typeID)
        {
            Decimal TotAdminAmount = 0;
            List<UserAccounts> lstUserAccounts = new List<UserAccounts>();
            var lstUsersSummaryEventtype = new List<PlusMinussummarybyEventTypeandUser>();

            if (eventtype != "")
            {
                List<AccountsServiceReference.SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result> results = new List<AccountsServiceReference.SP_UserAccounts_GetDatabyUserIDandDateRangeandEventType_Result>();
                if (typeID == 1)
                {
                    results = objAccountsService.GetAccountsDatabyUserIdDateRangeandEventType(Convert.ToInt32(useridd), DateFrom, DateTo, false, eventtype).ToList();
                }
                else
                {
                    results = objAccountsService.GetAccountsDatabyUserIdDateRangeandEventType(userID, DateFrom, DateTo, false, eventtype).ToList();

                }

                foreach (var item in results)
                {
                    UserAccounts objUserAccounts = new UserAccounts();
                    objUserAccounts.AccountsTitle = item.AccountsTitle;
                    objUserAccounts.CreatedDate = item.CreatedDate;
                    objUserAccounts.Credit = item.Credit;
                    objUserAccounts.Debit = item.Debit;
                    objUserAccounts.MarketBookID = item.MarketBookID;
                    objUserAccounts.OpeningBalance = (decimal)item.OpeningBalance;
                    objUserAccounts.UserID = item.UserID;
                    objUserAccounts.UserName = item.UserName;
                    objUserAccounts.AgentRate = item.AgentRate;
                    lstUserAccounts.Add(objUserAccounts);
                }

            }

            if (lstUserAccounts.Count > 0)
            {
                var lstUsers = lstUserAccounts.Select(item => item.UserID).Distinct().ToList();

                foreach (var userid in lstUsers)
                {
                    PlusMinussummarybyEventTypeandUser objplusminus = new PlusMinussummarybyEventTypeandUser();
                    var lstUserAccountsbyUser = lstUserAccounts.Where(item => item.UserID == userid).ToList();
                    decimal nettotalbyuser = lstUserAccountsbyUser.Sum(item => Convert.ToDecimal(item.Debit)) - lstUserAccountsbyUser.Sum(item => Convert.ToDecimal(item.Credit));
                    objplusminus.Username = Crypto.Decrypt(lstUserAccountsbyUser[0].UserName);
                    if (nettotalbyuser >= 0)
                    {
                        objplusminus.Plus = nettotalbyuser;
                        objplusminus.Minus = 0;

                    }
                    else
                    {
                        objplusminus.Plus = 0;
                        objplusminus.Minus = nettotalbyuser;
                    }
                    lstUsersSummaryEventtype.Add(objplusminus);
                }
                List<PlusMinussummarybyEventTypeandUser> lstPlusCustomers = lstUsersSummaryEventtype.Where(item => item.Plus > 0).ToList();
                List<PlusMinussummarybyEventTypeandUser> lstMinusCustomers = lstUsersSummaryEventtype.Where(item => item.Minus < 0).ToList();
                //dgvPlus.ItemsSource = lstPlusCustomers;
                // DGVMinus.ItemsSource = lstMinusCustomers;
            }
            else
            {
                List<PlusMinussummarybyEventTypeandUser> lstPlusCustomers = new List<PlusMinussummarybyEventTypeandUser>();
                List<PlusMinussummarybyEventTypeandUser> lstMinusCustomers = new List<PlusMinussummarybyEventTypeandUser>();
                //dgvPlus.ItemsSource = lstPlusCustomers;
                //DGVMinus.ItemsSource = lstMinusCustomers;
            }
            return Ok(lstUsersSummaryEventtype);
        }

        public IActionResult UserBetsbyAgent(int UserID,int typeID)
        {
            if (typeID != 3)
            {
                if (typeID == 2)
                {
                    List<UserBetsforAgent> lstUserbetsforagent = JsonConvert.DeserializeObject<List<Models.UserBetsforAgent>>(objUsersServiceCleint.GetUserBetsbyAgentID(UserID, LoggedinUserDetail.PasswordForValidate));
                    return Ok(lstUserbetsforagent);
                }
                else
                {
                    List<UserBetsforAgent> lstUserbetsforagent = new List<UserBetsforAgent>();
                    return Ok(lstUserbetsforagent);
                }
            }
            else
            {
                List<UserBetsforAgent> lstUserbetsforagent = new List<UserBetsforAgent>();
                return Ok(lstUserbetsforagent);
            }
        }

        public string ResetPassword(int userID, string Password)
        {
            
            if (Password.Length >= 6)
            {
                if (userID > 0)
                {
                    objUsersServiceCleint.ResetPasswordofUser(userID, Crypto.Encrypt(Password), userID, DateTime.Now, LoggedinUserDetail.PasswordForValidate);
                }

                return "True";
            }
            else
            {
                return "False";
            }
        }

        public IActionResult LedgerDetails(string DateFrom, string DateTo, int UserID, bool isCredit)
        {
            
            try
            {
                DateFrom = ConvertDateFormat(DateFrom);
                DateTo = ConvertDateFormat(DateTo);
                //if (UserID == 0)
                //{
                //    UserID = userID;
                //}
                List<UserAccounts> lstUserAccounts = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyUserIDandDateRange(UserID, DateFrom, DateTo, isCredit, LoggedinUserDetail.PasswordForValidate));
                if (lstUserAccounts.Count > 0)
                {
                    UserAccounts objUseraccounts = new UserAccounts();
                    objUseraccounts.AccountsTitle = "Opening Balance";
                    objUseraccounts.Debit = lstUserAccounts[0].OpeningBalance.ToString("F2");
                    objUseraccounts.Credit = "0.00";
                    objUseraccounts.CreatedDate = lstUserAccounts[0].CreatedDate;
                    objUseraccounts.OpeningBalance = lstUserAccounts[0].OpeningBalance;
                    lstUserAccounts.Insert(0, objUseraccounts);
                    for (int i = 0; i <= lstUserAccounts.Count - 1; i++)
                    {
                        if (i + 1 < lstUserAccounts.Count)
                        {
                            if (lstUserAccounts[i + 1].Debit == "" || lstUserAccounts[i + 1].Debit == "0.00")
                            {
                                lstUserAccounts[i + 1].OpeningBalance = lstUserAccounts[i].OpeningBalance - Convert.ToDecimal(lstUserAccounts[i + 1].Credit);
                                lstUserAccounts[i + 1].Credit = (-1 * Convert.ToDecimal(lstUserAccounts[i + 1].Credit)).ToString();
                                lstUserAccounts[i + 1].Debit = "0.00";
                            }
                            else
                            {
                                lstUserAccounts[i + 1].OpeningBalance = lstUserAccounts[i].OpeningBalance + Convert.ToDecimal(lstUserAccounts[i + 1].Debit);
                                lstUserAccounts[i + 1].Credit = "0.00";
                            }
                        }
                    }


                }
                return Ok(lstUserAccounts);
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                List<UserAccounts> lstUserAccounts = new List<UserAccounts>();
                return Ok(lstUserAccounts);
            }

        }

        public string GetCurrentBalanceofUser(int userID)
        {
            return objUsersServiceCleint.GetCurrentBalancebyUser(userID, LoggedinUserDetail.PasswordForValidate);
        }

        public string TransferBalanceFromOneUsertoAnother(decimal AccountBalance, int UserIDFrom, string UsernameFrom, string AccountsTitle, int UserIDTo, string UsernameTo,int userID,int typeID,int addedBy)
        {
            if (typeID != 3)
            {
                decimal UserFromBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserIDFrom, LoggedinUserDetail.PasswordForValidate));
                decimal UserToBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(UserIDTo, LoggedinUserDetail.PasswordForValidate));
                if (UserFromBalance < AccountBalance)
                {
                    return "Amount is less than available balance";
                }
                objAccountsService.AddtoUsersAccounts(AccountsTitle, AccountBalance.ToString(), "0.00", UserIDTo, "", DateTime.Now, "", "", "", UserToBalance, false, "", "", "", "", "");
                objUsersServiceCleint.AddCredittoUser(AccountBalance, UserIDTo, addedBy, DateTime.Now, 0, false, AccountsTitle, false, LoggedinUserDetail.PasswordForValidate);
                objAccountsService.AddtoUsersAccounts(AccountsTitle, "0.00", AccountBalance.ToString(), UserIDFrom, "", DateTime.Now, "", "", "", UserFromBalance, false, "", "", "", "", "");
                objUsersServiceCleint.AddCredittoUser(-1 * AccountBalance, UserIDFrom, addedBy, DateTime.Now, 0, false, AccountsTitle, false, LoggedinUserDetail.PasswordForValidate);
                objUsersServiceCleint.AddUserActivity("Transfer Balance From " + UsernameFrom + " To " + UsernameTo + " ( " + AccountBalance.ToString() + " ).", DateTime.Now, LoggedinUserDetail.GetIPAddress(), "", "", userID);
            }
            return "True";
        }

        public IActionResult BalanceSheet(int UserID, bool isCredit,int typeID)
        {           
            try
            {

              
                List<BalanceSheet> lstBalanceSheet = new List<Models.BalanceSheet>();
                if (typeID != 3)
                {
                    if (UserID == 73)
                    { }
                    List<UserAccounts> lstUserAccounts = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(UserID, isCredit, LoggedinUserDetail.PasswordForValidate));
                    if (lstUserAccounts.Count > 0)
                    {
                        var lstUserIDs = lstUserAccounts.Select(item1 => new { item1.UserID }).Distinct().ToArray();

                        foreach (var useritem in lstUserIDs)
                        {
                            List<UserAccounts> lstUseraccountsbyUser = lstUserAccounts.Where(item2 => item2.UserID == useritem.UserID).ToList();
                            if (lstUseraccountsbyUser.Count > 0)
                            {
                                BalanceSheet objBalanceSheet = new Models.BalanceSheet();
                                objBalanceSheet.Username = Crypto.Decrypt(lstUseraccountsbyUser[0].UserName);
                                objBalanceSheet.StartingBalance = lstUseraccountsbyUser[0].OpeningBalance;
                                decimal Profitorloss = lstUseraccountsbyUser.Sum(item3 => Convert.ToDecimal(item3.Debit)) - lstUseraccountsbyUser.Sum(item3 => Convert.ToDecimal(item3.Credit));
                                if (typeID == 8)
                                {
                                    if (Profitorloss < 0)
                                    {
                                        decimal a = -1 * (Profitorloss);

                                        objBalanceSheet.Profit = a;
                                    }
                                    else
                                    {
                                        decimal a = -1 * (Profitorloss);
                                        objBalanceSheet.Loss = a;
                                    }
                                }
                                else
                                {
                                    if (Profitorloss >= 0)
                                    {
                                        objBalanceSheet.Profit = Profitorloss;

                                    }
                                    else
                                    {
                                        objBalanceSheet.Loss = Profitorloss;
                                    }
                                }
                                lstBalanceSheet.Add(objBalanceSheet);
                            }
                        }
                    }

                    if (typeID != 8)
                    {
                        List<UserAccounts> lstUserAccountsAdmin = new List<UserAccounts>();
                        lstUserAccountsAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(UserID, isCredit, LoggedinUserDetail.PasswordForValidate));
                        BalanceSheet objBalanceSheet1 = new BalanceSheet();
                        objBalanceSheet1.Username = "Book account";
                        objBalanceSheet1.StartingBalance = lstUserAccountsAdmin[0].OpeningBalance;
                        if (UserID > 0 && UserID != 73)
                        {
                            lstUserAccountsAdmin = lstUserAccountsAdmin.Where(item => item.MarketBookID != "").ToList();
                        }
                        //
                        decimal Profitorloss1 = lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Debit)) - lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Credit));
                        if (Profitorloss1 >= 0)
                        {
                            objBalanceSheet1.Profit = Profitorloss1;
                        }
                        else
                        {
                            objBalanceSheet1.Loss = Profitorloss1;
                        }
                        lstBalanceSheet.Add(objBalanceSheet1);
                    }
                    int HawalaID = objUsersServiceCleint.GetHawalaAccountIDbyUserID(UserID);
                    decimal CurrentAccountBalance = Convert.ToDecimal(objUsersServiceCleint.GetCurrentBalancebyUser(HawalaID, LoggedinUserDetail.PasswordForValidate));

                    decimal StartingBalance = Convert.ToDecimal(objUsersServiceCleint.GetStartingBalance(HawalaID, LoggedinUserDetail.PasswordForValidate));
                    BalanceSheet objBalanceSheetHawala = new BalanceSheet();
                    objBalanceSheetHawala.Username = "Hawala account";
                    objBalanceSheetHawala.StartingBalance = StartingBalance;

                    if (CurrentAccountBalance >= 0)
                    {
                        objBalanceSheetHawala.Profit = CurrentAccountBalance;
                    }
                    else
                    {
                        objBalanceSheetHawala.Loss = CurrentAccountBalance;
                    }

                    lstBalanceSheet.Add(objBalanceSheetHawala);
                }

                if (typeID == 1)
                {
                    List<UserAccounts> lstUserAccountsAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(1, isCredit, LoggedinUserDetail.PasswordForValidate));
                    BalanceSheet objBalanceSheet1 = new Models.BalanceSheet();
                    objBalanceSheet1.Username = "Book account";
                    objBalanceSheet1.StartingBalance = lstUserAccountsAdmin[0].OpeningBalance;
                    lstUserAccountsAdmin = lstUserAccountsAdmin.Where(item => item.MarketBookID != "").ToList();
                    decimal Profitorloss = lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Debit)) - lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Credit));
                    if (Profitorloss >= 0)
                    {
                        objBalanceSheet1.Profit = Profitorloss;

                    }
                    else
                    {
                        objBalanceSheet1.Loss = Profitorloss;
                    }

                    lstBalanceSheet.Add(objBalanceSheet1);
                    List<UserAccounts> lstUserAccountsCommission = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForCommisionaccount(LoggedinUserDetail.PasswordForValidate));
                    BalanceSheet objBalanceSheet2 = new Models.BalanceSheet();
                    objBalanceSheet2.Username = "Commission";
                    objBalanceSheet2.StartingBalance = 0;
                    decimal Profitorloss1 = lstUserAccountsCommission.Sum(item3 => Convert.ToDecimal(item3.Credit));
                    if (Profitorloss1 >= 0)
                    {
                        objBalanceSheet2.Profit = Profitorloss1;
                    }
                    else
                    {
                        objBalanceSheet2.Loss = Profitorloss1;
                    }
                    lstBalanceSheet.Add(objBalanceSheet2);
                }

                if (typeID == 8)
                {
                    List<UserAccounts> lstUserAccountsAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(UserID, isCredit, LoggedinUserDetail.PasswordForValidate));
                    BalanceSheet objBalanceSheet1 = new Models.BalanceSheet();
                    objBalanceSheet1.Username = "Book account";
                    objBalanceSheet1.StartingBalance = lstUserAccountsAdmin[0].OpeningBalance;
                    lstUserAccountsAdmin = lstUserAccountsAdmin.Where(item => item.MarketBookID != "").ToList();
                    decimal Profitorloss = lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Debit)) - lstUserAccountsAdmin.Sum(item3 => Convert.ToDecimal(item3.Credit));
                    if (Profitorloss >= 0)
                    {
                        objBalanceSheet1.Profit = Profitorloss;
                    }
                    else
                    {
                        objBalanceSheet1.Loss = Profitorloss;
                    }

                    lstBalanceSheet.Add(objBalanceSheet1);
                }

                return Ok(lstBalanceSheet);
            }
            catch (System.Exception ex)
            {
                LoggedinUserDetail.LogError(ex);
                List<BalanceSheet> lstBalanceSheet = new List<BalanceSheet>();
                return Ok(lstBalanceSheet);
            }

        }
    }

}
