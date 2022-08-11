
using bfnexchange.Services.DBModel;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using bfnexchange;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Data.Entity;


namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserServices" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserServices.svc or UserServices.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
    public class UserServices : IUserServices
    {
        NExchangeEntities dbEntities = new NExchangeEntities();

        public string GetUserbyUsernameandPassword(string username, string password)
        {
            var users = dbEntities.SP_Users_GetUserbyUsernameandPassword(username, password).FirstOrDefault<SP_Users_GetUserbyUsernameandPassword_Result2>();
            if (users != null)
            {
                if (users.isBlocked == false && users.isDeleted == false && users.GTLogin == true)
                {
                    users.PasswordforValidate = ConfigurationManager.AppSettings["PasswordForValidate"];
                    if (users.UserTypeID == 1 || users.UserTypeID == 2)
                    {
                        users.PasswordforValidateS = ConfigurationManager.AppSettings["PasswordForValidateS"];
                    }
                }
                if (users.isBlocked == true && users.isDeleted == false && users.GTLogin == false)
                {
                    users.PasswordforValidate = ConfigurationManager.AppSettings["PasswordForValidate"];
                    if (users.UserTypeID == 1 || users.UserTypeID == 2)
                    {
                        users.PasswordforValidateS = ConfigurationManager.AppSettings["PasswordForValidateS"];
                    }
                }
                if (users.isBlocked == false && users.isDeleted == false && users.GTLogin == false)
                {
                    users.PasswordforValidate = ConfigurationManager.AppSettings["PasswordForValidate"];
                    if (users.UserTypeID == 1 || users.UserTypeID == 2)
                    {
                        users.PasswordforValidateS = ConfigurationManager.AppSettings["PasswordForValidateS"];
                    }
                }

                //GetClientAddress();
            }
            return ConverttoJSONString(users);


        }

        public string GetUserbyUsernameandPasswordNew(string username, string password)
        {
            var users = dbEntities.SP_Users_GetUserbyUsernameandPasswordWeb(username, password).FirstOrDefault<SP_Users_GetUserbyUsernameandPasswordWeb_Result>();
            if (users != null)
            {
                if (users.isBlocked == false && users.isDeleted == false && users.GTLogin == true)
                {
                    users.PasswordforValidate = ConfigurationManager.AppSettings["PasswordForValidate"];
                    if (users.UserTypeID == 1 || users.UserTypeID == 2)
                    {
                        users.PasswordforValidateS = ConfigurationManager.AppSettings["PasswordForValidateS"];
                    }
                }
                if (users.isBlocked == true && users.isDeleted == false && users.GTLogin == false)
                {
                    users.PasswordforValidate = ConfigurationManager.AppSettings["PasswordForValidate"];
                    if (users.UserTypeID == 1 || users.UserTypeID == 2)
                    {
                        users.PasswordforValidateS = ConfigurationManager.AppSettings["PasswordForValidateS"];
                    }
                }
                if (users.isBlocked == false && users.isDeleted == false && users.GTLogin == false)
                {
                    users.PasswordforValidate = ConfigurationManager.AppSettings["PasswordForValidate"];
                    if (users.UserTypeID == 1 || users.UserTypeID == 2)
                    {
                        users.PasswordforValidateS = ConfigurationManager.AppSettings["PasswordForValidateS"];
                    }
                }


            }
            return ConverttoJSONString(users);
           

        }

        public string getlistuserids(int createdid)
        {
            List<SP_Users_GetUserbyCreatedbyID_Result> lstUSerss = dbEntities.SP_Users_GetUserbyCreatedbyID(createdid).ToList<SP_Users_GetUserbyCreatedbyID_Result>();
            return ConverttoJSONString(lstUSerss);
        }
        public void UpdateCurrentLoggedInIDbyUserID(int userID)
        {
            dbEntities.SP_Users_UpdateCurrentLoggedInIDbyUserID(userID);
        }
        public bool IsValidUser(string username, string password)
        {
            var users = dbEntities.SP_Users_GetUserbyUsernameandPassword(username, password).FirstOrDefault<SP_Users_GetUserbyUsernameandPassword_Result2>();
            if (users != null) { return true; }
            else { return false; }
        }
        public void SetLoggedinStatus(int userID, bool LoggedIn)
        {
            dbEntities.SP_Users_SetLoggedInStatus(userID, LoggedIn);
        }
        private string GetClientAddress()
        {
            try
            {

                // creating object of service when request comes   
                OperationContext context = OperationContext.Current;
                //Getting Incoming Message details   
                MessageProperties prop = context.IncomingMessageProperties;
                //Getting client endpoint details from message header   
                RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                AddUserActivity(endpoint.Address, DateTime.Now, "", "", "", 1);
                return endpoint.Address;
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }
        private bool ValidatePassword(string Password)
        {
            if (Password == ConfigurationManager.AppSettings["PasswordForValidate"])
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private bool ValidatePasswordS(string Password)
        {
            if (Password == ConfigurationManager.AppSettings["PasswordForValidateS"])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetAllUsersbyUserType(int userID, int usertypeID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var users = dbEntities.SP_Users_GetAllUsersbyUserType(usertypeID, userID).ToList<SP_Users_GetAllUsersbyUserType_Result>();
            return ConverttoJSONString(users);
        }
        public string GetAllUsersbyUserTypeNew(int userID, int usertypeID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var users = dbEntities.SP_Users_GetAllUsersbyUserTypeNew(usertypeID, userID).ToList<SP_Users_GetAllUsersbyUserTypeNew_Result>();
            return ConverttoJSONString(users);
        }
        public string GetAllCuttingUsers(string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var users = dbEntities.SP_Users_GetAllCuttingUsers().ToList<SP_Users_GetAllCuttingUsers_Result>();
            return ConverttoJSONString(users);
        }
        public void AddUserActivity(string Activityname, DateTime ActivityTime, string IPAddress, string Location, string Deviceinfo, int userID)
        {
            //dbEntities.SP_ActivityLog_Insert(Activityname, ActivityTime, IPAddress, Deviceinfo, Location, userID);
        }
        public string GetAccessRightsbyUserType(int UserTypeID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var result = dbEntities.SP_AccessRigths_GetAccessRightsbyUserTypeID(UserTypeID).FirstOrDefault<SP_AccessRigths_GetAccessRightsbyUserTypeID_Result>();
            return ConverttoJSONString(result);
        }
        public string AddUser(string Name, string Phonenumber, string EmailAddress, string username, string Password, string Location, decimal Accountbalance, int usertypeID, int createdByID, string RatePercent, decimal BetLowerLimit, decimal BetUpperLimit, bool CheckConditionsforPlacingBet, decimal BetLowerLimitHorsePlace, decimal BetUpperLimitHorsePlace, decimal BetLowerLimitGrayHoundWin, decimal BetUpperLimitGrayHoundWin, decimal BetLowerLimitGrayHoundPlace, decimal BetUpperLimitGrayHoundPlace, decimal BetLowerLimitMatchOdds, decimal BetUpperLimitMatchOdds, decimal BetLowerLimitInningsRunns, decimal BetUpperLimitInningsRunns, decimal BetLowerLimitCompletedMatch, decimal BetUpperLimitCompletedMatch, decimal BetLowerLimitMatchOddsSoccer, decimal BetUpperLimitMatchOddsSoccer, decimal BetLowerLimitMatchOddsTennis, decimal BetUpperLimitMatchOddsTennis, decimal BetUpperLimitTiedMatch, decimal BetLowerLimitTiedMatch, decimal BetUpperLimitWinner, decimal BetLowerLimitWinner, string Passwordforvalidate, decimal BetUpperLimitFancy, decimal BetLowerLimitFancy)
        {
            if (ValidatePassword(Passwordforvalidate) == false)
            {
                return "";
            }

            try
            {
                dbEntities.SP_Users_Insert(Name, Phonenumber, EmailAddress, username, Password, Location, Accountbalance.ToString(), usertypeID, createdByID, DateTime.Now, RatePercent, BetLowerLimit, BetUpperLimit, CheckConditionsforPlacingBet, BetLowerLimitHorsePlace, BetUpperLimitHorsePlace, BetLowerLimitGrayHoundWin, BetUpperLimitGrayHoundWin, BetLowerLimitGrayHoundPlace, BetUpperLimitGrayHoundPlace, BetLowerLimitMatchOdds, BetUpperLimitMatchOdds, BetLowerLimitInningsRunns, BetUpperLimitInningsRunns, BetLowerLimitCompletedMatch, BetUpperLimitCompletedMatch, 2, BetUpperLimitMatchOddsSoccer, BetLowerLimitMatchOddsSoccer, BetUpperLimitMatchOddsTennis, BetLowerLimitMatchOddsTennis, BetUpperLimitTiedMatch, BetLowerLimitTiedMatch, BetUpperLimitWinner, BetLowerLimitWinner, BetUpperLimitFancy, BetLowerLimitFancy).FirstOrDefault<SP_Users_Insert_Result>();
                SP_Users_GetUserbyUsernameandPassword_Result2 user = dbEntities.SP_Users_GetUserbyUsernameandPassword(username, Password).FirstOrDefault<SP_Users_GetUserbyUsernameandPassword_Result2>(); 
                return user.ID.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public void AddKalijut(Nullable<int> userID, string eventTypeID, string competitonID, string eventID, string marketCatalogue, Nullable<int> updatedbyID, string eventTypeName, string competitionName, string eventName, string marketCatalogueName, Nullable<System.DateTime> eventOpenDate, string sheetName, string associateEventID)
        {

            try
            {

                dbEntities.SP_UserMarket_Insert(userID, eventTypeID, competitonID, eventID, marketCatalogue, updatedbyID, eventTypeName, competitionName, eventName, marketCatalogueName, eventOpenDate, sheetName, associateEventID);
            }
            catch (Exception ex)
            {
                //return "";
            }
        }

        public void MarketCatalogueSelectionskalijut(string marketCatalogueID, string selectionID, string selectionName, string jockeyName, string wearing, string wearingDesc, string clothnumber, string stallDraw)
        {

            try
            {

                dbEntities.SP_MarketCatalogueSelections_Insert(marketCatalogueID, selectionID, selectionName, jockeyName, wearing, wearingDesc, clothnumber, stallDraw);
            }
            catch (Exception ex)
            {
                //return "";
            }
        }

        public void UpdateMarketsForView(int userID, bool isAllowedGrayHound, bool isAllowedHorse, bool isTennisAllowed, bool isSoccerAllowed, string Password)
        {
            if (ValidatePassword(Password) == true)
            {
                dbEntities.SP_Users_UpdateMarketsForView(userID, isAllowedGrayHound, isAllowedHorse, isTennisAllowed, isSoccerAllowed);
            }
        }
        public void UpdateBetLowerLimit(int userID, decimal BetLowerLimit, decimal BetUpperLimit, bool isAllowedGrayHound, bool isAllowedHorse, decimal BetLowerLimitHorsePlace, decimal BetUpperLimitHorsePlace, decimal BetLowerLimitGrayHoundWin, decimal BetUpperLimitGrayHoundWin, decimal BetLowerLimitGrayHoundPlace, decimal BetUpperLimitGrayHoundPlace, decimal BetLowerLimitMatchOdds, decimal BetUpperLimitMatchOdds, decimal BetLowerLimitInningsRunns, decimal BetUpperLimitInningsRunns, decimal BetLowerLimitCompletedMatch, decimal BetUpperLimitCompletedMatch, bool isTennisAllowed, bool isSoccerAllowed, int CommissionRate, decimal BetLowerLimitMatchOddsSoccer, decimal BetUpperLimitMatchOddsSoccer, decimal BetLowerLimitMatchOddsTennis, decimal BetUpperLimitMatchOddsTennis, decimal BetUpperLimitTiedMatch, decimal BetLowerLimitTiedMatch, decimal BetUpperLimitWinner, decimal BetLowerLimitWinner, string Password, decimal BetUpperLimitFancy, decimal BetLowerLimitFancy)
        {
            if (ValidatePassword(Password) == true)
            {


                dbEntities.SP_Users_UpdateBetLowerLimit(userID, BetLowerLimit, BetUpperLimit, isAllowedGrayHound, isAllowedHorse, BetLowerLimitHorsePlace, BetUpperLimitHorsePlace, BetLowerLimitGrayHoundWin, BetUpperLimitGrayHoundWin, BetLowerLimitGrayHoundPlace, BetUpperLimitGrayHoundPlace, BetLowerLimitMatchOdds, BetUpperLimitMatchOdds, BetLowerLimitInningsRunns, BetUpperLimitInningsRunns, BetLowerLimitCompletedMatch, BetUpperLimitCompletedMatch, isTennisAllowed, isSoccerAllowed, CommissionRate, BetUpperLimitMatchOddsSoccer, BetLowerLimitMatchOddsSoccer, BetUpperLimitMatchOddsTennis, BetLowerLimitMatchOddsTennis, BetUpperLimitTiedMatch, BetLowerLimitTiedMatch, BetUpperLimitWinner, BetLowerLimitWinner, BetUpperLimitFancy, BetLowerLimitFancy);
            }
        }
        public string GetCurrentBalancebyUser(int userid, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var result = dbEntities.SP_Users_GetCurrentBalance(userid).FirstOrDefault<SP_Users_GetCurrentBalance_Result1>();
            return result.AccountBalance.ToString();
        }
        public int GetCommissionRatebyUserID(int UserID)
        {
            return Convert.ToInt32(dbEntities.SP_Users_GetCommissionRate(UserID).FirstOrDefault());
        }
        public int GetCommissionRatebyUserIDFancy(int UserID)
        {
            return Convert.ToInt32(dbEntities.SP_Users_GetCommissionRateFancy(UserID).FirstOrDefault());
        }
        public void UpdateCommissionRatebyUserID(int UserID, int CommisionrateFancy)
        {
            dbEntities.SP_Users_UpdateCommissionRateFancy(UserID, CommisionrateFancy);
        }
        public SP_Users_GetReferrerRateandReferrerIDbyUserID_Result GetReferrerIDandReferrerRatebyUserID(int userID)
        {
            return dbEntities.SP_Users_GetReferrerRateandReferrerIDbyUserID(userID).FirstOrDefault();
        }
        public decimal GetStartingBalance(int userid, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return 0;
            }
            var result = dbEntities.SP_Users_GetStartingBalance(userid);
            return result.FirstOrDefault().Value;
        }

      
        public void UpdateAccountBalacnebyUser(int userid, decimal AccountBalance, string Password)
        {
            if (ValidatePassword(Password) == true)
            {


                dbEntities.SP_Users_UpdateAccountBalancebyUserID(userid, AccountBalance);
            }
        }

        public string CheckifUserExists(string username)
        {
            var result = dbEntities.SP_Users_GetUSernamebyUsername(username).FirstOrDefault<SP_Users_GetUSernamebyUsername_Result>();
            return result.Username;
        }
        public void UpdateStartBalancebyUserID(int userID, decimal newBalance, string Password)
        {
            if (ValidatePassword(Password) == true)
            {


                dbEntities.SP_Users_UpdateStartingBalancebyUserID(userID, newBalance);
            }
        }
        public void UpdateAccountsOpeningBalance(int userID, decimal Balance, string Password)
        {
            if (ValidatePassword(Password) == true)
            {


                dbEntities.SP_UserAccounts_UpdateOpeningBalancebyUserID(userID, Balance);
            }
        }

        public void AddAdminAmountForSuper(string AccountsTitle,string debit,string Credit, int userID,string marketid, DateTime addedtime, string Password)
        {
            if (ValidatePassword(Password) == true)
            {
            
                dbEntities.SP_AdminAmountForSuper_Insert(AccountsTitle, debit,Credit, userID, marketid, addedtime);
            }
        }

        public void AddCredittoUser(decimal Amount, int userID, int AddedbyID, DateTime addedtime, decimal Amountremoved, bool AddtoUserAccounts, string AccountsTitle, bool isCreditAmount, string Password)
        {
            if (ValidatePassword(Password) == true)
            {



                if (AddtoUserAccounts == true)
                {
                    if (Amount > 0)
                    {
                        objAccountsService.AddtoUsersAccounts(AccountsTitle, Amount.ToString(), "0.00", userID, "", DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)),(GetSuperRate(userID)).ToString(), (GetSamiadminRate(userID)).ToString(), GetCommissionRatebyUserID(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), isCreditAmount, "", "", "", "", "");

                    }
                    else
                    {
                        objAccountsService.AddtoUsersAccounts(AccountsTitle, "0.00", Amountremoved.ToString(), userID, "", DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), (GetSuperRate(userID)).ToString(), (GetSamiadminRate(userID)).ToString(), GetCommissionRatebyUserID(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), isCreditAmount, "", "", "", "", "");
                    }
                }
                dbEntities.SP_USers_AddCredit(Amount, userID, AddedbyID, addedtime, Amountremoved);
            }
        }
        public string GetUserDetailsbyID(int userID, string Password)
        {
            try
            {


                if (ValidatePassword(Password) == false)
                {
                    return "";
                }
                var results = dbEntities.SP_Users_GetDetailsofUserbyID(userID).FirstOrDefault<SP_Users_GetDetailsofUserbyID_Result>();
                return ConverttoJSONString(results);
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }
        public string SetDeleteStatusofUser(int UserID, bool isDeleted, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            dbEntities.SP_Users_Delete(UserID, isDeleted);
            return "True";
        }
        public string SetAgentRateofUser(int UserID, string AgentRate, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            dbEntities.SP_Users_UpdateAgentRate(UserID, AgentRate);
            return "True";
        }
        public string SetBlockedStatusofUserBMS(int UserID, bool isBlocked, string Password)
        {
            try
            {


                if (ValidatePassword(Password) == false)
                {
                    return "";
                }
                dbEntities.SP_Users_BlockBMS(UserID, isBlocked);
                return "True";
            }
            catch (System.Exception ex)
            {
                APIConfig.WriteErrorToDB(ex.InnerException.ToString());
                return "";
            }
        }
        public string SetBlockedStatusofUser(int UserID, bool isBlocked, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            dbEntities.SP_Users_Block(UserID, isBlocked);
            return "True";
        }

       
        public string ResetPasswordofUser(int UserID, string Password, int Updatedby, DateTime updatedtime, string PasswordForValidate)
        {
            if (ValidatePassword(PasswordForValidate) == false)
            {
                return "";
            }
            dbEntities.SP_Users_ResetPassword(UserID, Password, Updatedby, updatedtime);
            return "True";
        }
        public string GetTodayHorseRacing(int UserID, string EventTypeID)
        {
            bool ioc = GetIsComAllowbyUserID(UserID);
            var results = dbEntities.SP_UserMarket_GetTodaysHorseRacing(EventTypeID, UserID).ToList<SP_UserMarket_GetTodaysHorseRacing_Result>();
            return ConverttoJSONString(results);
        }
        public string GetTodayHorseRacingNew(int UserID)
        {
            var results = dbEntities.SP_UserMarket_GetTodaysHorseRacingauto(UserID).ToList<SP_UserMarket_GetTodaysHorseRacingauto_Result>();
            return ConverttoJSONString(results);
        }
        

        public void DownloadAllMarketHorseRace(string Password)
        {

            if (dbEntities.SP_APIConfig_GetAutomaticDownloadData().FirstOrDefault().Value == true)
            {

                dbEntities.Configuration.AutoDetectChangesEnabled = false;
                dbEntities.Configuration.ValidateOnSaveEnabled = false;


                try
                {
                    string[] lstNotAllowedRaces = ConfigurationManager.AppSettings["NotAllowedRaces"].ToString().Split(',');
                    string[] AllowedUsers = ConfigurationManager.AppSettings["AllowedUsers"].ToString().Split(',');
                    BettingService objBettingService = new BettingService();
                    var events = objBettingService.listEvents("7", false, Password);
                    List<UserMarket> lstUserMarket = new List<UserMarket>();
                    List<MarketCatalogueSelection> lstMarketCatalogueSelections = new List<MarketCatalogueSelection>();
                    foreach (var eventitem in events)
                    {
                        List<String> lstIDs = new List<string>();
                        var marketcatalouges = objBettingService.listMarketCatalogue(eventitem.Event.Id, lstIDs, false, Password);

                        List<SP_Users_GetUserIDsforAllowedHorseRace_Result> lstUsersHorserace = dbEntities.SP_Users_GetUserIDsforAllowedHorseRace(0).ToList<SP_Users_GetUserIDsforAllowedHorseRace_Result>();
                        foreach (var marketcatalogueitem in marketcatalouges)
                        {

                            foreach (var userid in lstUsersHorserace)
                            {
                                UserMarket objUserMarket = new UserMarket();
                                objUserMarket.AssociateEventID = "";
                                if (AllowedUsers.Contains(userid.ID.ToString()))
                                {
                                    objUserMarket.BettingAllowed = true;
                                }
                                else
                                {
                                    if (lstNotAllowedRaces.Any(NotAllowedRace => eventitem.Event.Name.Contains(NotAllowedRace)))
                                    {
                                        objUserMarket.BettingAllowed = false;
                                    }
                                    else
                                    {
                                        objUserMarket.BettingAllowed = true;
                                    }

                                }


                                objUserMarket.EventTypeID = "7";
                                objUserMarket.CompetitionID = "7";
                                objUserMarket.EventID = eventitem.Event.Id;
                                objUserMarket.MarketCatalogueID = marketcatalogueitem.MarketId;
                                objUserMarket.UpdatedbyID = 1;
                                objUserMarket.EventTypeName = "Horse Racing";
                                objUserMarket.CompetitionName = "Horse Racing";
                                objUserMarket.EventName = eventitem.Event.Name;
                                objUserMarket.MarketCatalogueName = marketcatalogueitem.MarketName;
                                objUserMarket.EventOpenDate = marketcatalogueitem.Description.MarketTime;
                                objUserMarket.SheetName = "";
                                objUserMarket.UserID = userid.ID;
                                objUserMarket.CountryCode = marketcatalogueitem.Event.CountryCode;
                                lstUserMarket.Add(objUserMarket);
                                //dbEntities.SP_UserMarket_Insert(userid.ID, "7", "7", eventitem.Event.Id, marketcatalogueitem.MarketId, 1, "Horse Racing", "Horse Racing", eventitem.Event.Name, marketcatalogueitem.MarketName, marketcatalogueitem.Description.MarketTime, "", "");
                            }

                            dbEntities.SP_MarketCatalogueSelections_Delete(marketcatalogueitem.MarketId);
                            foreach (var selectionitem in marketcatalogueitem.Runners)
                            {
                                var jockeyname = "Not";
                                var wearing = "Not";
                                var waringdesc = "Not";
                                var clothnumber = "Not";
                                var stallDraw = "Not";
                                try
                                {
                                    if (selectionitem.Metadata.ContainsKey("JOCKEY_NAME"))
                                    {
                                        jockeyname = selectionitem.Metadata["JOCKEY_NAME"];
                                        wearing = "http://content-cache.betfair.com/feeds_images/Horses/SilkColours/" + selectionitem.Metadata["COLOURS_FILENAME"];
                                        waringdesc = selectionitem.Metadata["COLOURS_DESCRIPTION"];
                                        clothnumber = selectionitem.Metadata["CLOTH_NUMBER"];
                                        stallDraw = selectionitem.Metadata["STALL_DRAW"];
                                    }

                                }
                                catch (System.Exception ex)
                                {

                                }
                                MarketCatalogueSelection objMarketCatalogue = new MarketCatalogueSelection();
                                objMarketCatalogue.MarketCatalogueID = marketcatalogueitem.MarketId;
                                objMarketCatalogue.SelectionID = selectionitem.SelectionId.ToString();
                                objMarketCatalogue.SelectionName = selectionitem.RunnerName;
                                objMarketCatalogue.JockeyName = jockeyname;
                                objMarketCatalogue.Wearing = wearing;
                                objMarketCatalogue.WearingDesc = waringdesc;
                                objMarketCatalogue.ClothNumber = clothnumber;
                                objMarketCatalogue.StallDraw = stallDraw;
                                lstMarketCatalogueSelections.Add(objMarketCatalogue);

                                //  dbEntities.SP_MarketCatalogueSelections_Insert(marketcatalogueitem.MarketId, selectionitem.SelectionId.ToString(), selectionitem.RunnerName, jockeyname, wearing, waringdesc, clothnumber, stallDraw);

                            }
                        }
                    }
                    dbEntities.UserMarkets.AddRange(lstUserMarket);
                    dbEntities.MarketCatalogueSelections.AddRange(lstMarketCatalogueSelections);
                    dbEntities.SaveChanges();
                }

                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
                catch (System.Data.Entity.Core.EntityCommandCompilationException ex)
                {
                    Console.WriteLine(ex.InnerException);
                }
                catch (System.Data.Entity.Core.UpdateException ex)
                {
                    Console.WriteLine(ex.InnerException);
                }

                catch (System.Data.Entity.Infrastructure.DbUpdateException ex) //DbContext
                {
                    Console.WriteLine(ex.InnerException);
                }

                DownloadAllMarketGrayHoundRace(Password);
                DownloadAllMarketSoccer(Password);
                DownloadAllMarketTennis(Password);
                dbEntities.SP_UserMarket_DeleteAllDuplicateRows();
                dbEntities.SP_UserMarket_DeleteAllMarketsofPreviousDay();
            }
        }

        public void DownloadAllMarketGrayHoundRace(string Password)
        {



            try
            {
                BettingService objBettingService = new BettingService();
                var events = objBettingService.listEvents("4339", false, Password);
                List<UserMarket> lstUserMarket = new List<UserMarket>();
                List<MarketCatalogueSelection> lstMarketCatalogueSelections = new List<MarketCatalogueSelection>();
               
                foreach (var eventitem in events)
                {
                    List<String> lstIDs = new List<string>();
                    var marketcatalouges = objBettingService.listMarketCatalogue(eventitem.Event.Id, lstIDs, false, Password);
                    
                    List<SP_Users_GetUserIDsforAllowedGrayHoundRace_Result> lstUsersGrayrace = dbEntities.SP_Users_GetUserIDsforAllowedGrayHoundRace(0).ToList<SP_Users_GetUserIDsforAllowedGrayHoundRace_Result>();
                    foreach (var marketcatalogueitem in marketcatalouges)
                    {

                        foreach (var userid in lstUsersGrayrace)
                        {
                            UserMarket objUserMarket = new UserMarket();
                            objUserMarket.AssociateEventID = "";

                            objUserMarket.BettingAllowed = true;

                            objUserMarket.EventTypeID = "4339";
                            objUserMarket.CompetitionID = "4339";
                            objUserMarket.EventID = eventitem.Event.Id;
                            objUserMarket.MarketCatalogueID = marketcatalogueitem.MarketId;
                            objUserMarket.UpdatedbyID = 1;
                            objUserMarket.EventTypeName = "Greyhound Racing";
                            objUserMarket.CompetitionName = "Greyhound Racing";
                            objUserMarket.EventName = eventitem.Event.Name;
                            objUserMarket.MarketCatalogueName = marketcatalogueitem.MarketName;
                            objUserMarket.EventOpenDate = marketcatalogueitem.Description.MarketTime;
                            objUserMarket.SheetName = "";
                            objUserMarket.UserID = userid.ID;
                            objUserMarket.CountryCode = marketcatalogueitem.Event.CountryCode;
                            lstUserMarket.Add(objUserMarket);

                            //  dbEntities.SP_UserMarket_Insert(userid.ID, "4339", "4339", eventitem.Event.Id, marketcatalogueitem.MarketId, 1, "Greyhound Racing", "Greyhound Racing", eventitem.Event.Name, marketcatalogueitem.MarketName, marketcatalogueitem.Description.MarketTime, "", "");
                        }

                        dbEntities.SP_MarketCatalogueSelections_Delete(marketcatalogueitem.MarketId);
                        foreach (var selectionitem in marketcatalogueitem.Runners)
                        {
                            var jockeyname = "Not";
                            var wearing = "Not";
                            var waringdesc = "Not";
                            var clothnumber = "Not";
                            var stallDraw = "Not";
                            try
                            {
                                if (selectionitem.Metadata.ContainsKey("JOCKEY_NAME"))
                                {
                                    jockeyname = selectionitem.Metadata["JOCKEY_NAME"];
                                    wearing = "http://content-cache.betfair.com/feeds_images/Horses/SilkColours/" + selectionitem.Metadata["COLOURS_FILENAME"];
                                    waringdesc = selectionitem.Metadata["COLOURS_DESCRIPTION"];
                                    clothnumber = selectionitem.Metadata["CLOTH_NUMBER"];
                                    stallDraw = selectionitem.Metadata["STALL_DRAW"];
                                }

                            }
                            catch (System.Exception ex)
                            {

                            }
                            MarketCatalogueSelection objMarketCatalogue = new MarketCatalogueSelection();
                            objMarketCatalogue.MarketCatalogueID = marketcatalogueitem.MarketId;
                            objMarketCatalogue.SelectionID = selectionitem.SelectionId.ToString();
                            objMarketCatalogue.SelectionName = selectionitem.RunnerName;
                            objMarketCatalogue.JockeyName = jockeyname;
                            objMarketCatalogue.Wearing = wearing;
                            objMarketCatalogue.WearingDesc = waringdesc;
                            objMarketCatalogue.ClothNumber = clothnumber;
                            objMarketCatalogue.StallDraw = stallDraw;
                            lstMarketCatalogueSelections.Add(objMarketCatalogue);
                            // dbEntities.SP_MarketCatalogueSelections_Insert(marketcatalogueitem.MarketId, selectionitem.SelectionId.ToString(), selectionitem.RunnerName, jockeyname, wearing, waringdesc, clothnumber, stallDraw);

                        }



                    }
                }
                dbEntities.UserMarkets.AddRange(lstUserMarket);
                dbEntities.MarketCatalogueSelections.AddRange(lstMarketCatalogueSelections);
                dbEntities.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
            {
                APIConfig.LogError(ex);
            }
            catch (System.Data.Entity.Core.EntityCommandCompilationException ex)
            {
                APIConfig.LogError(ex);
            }
            catch (System.Data.Entity.Core.UpdateException ex)
            {
                APIConfig.LogError(ex);
            }

            catch (System.Data.Entity.Infrastructure.DbUpdateException ex) //DbContext
            {
                APIConfig.LogError(ex);
            }
            catch (Exception ex)
            {
                APIConfig.LogError(ex);
            }

        }
        public void DownloadAllMarketTennis(string Password)
        {
            var jockeyname = "Not";
            var wearing = "Not";
            var waringdesc = "Not";
            var clothnumber = "Not";

            try
            {
                BettingService objBettingService = new BettingService();
                List<UserMarket> lstUserMarket = new List<UserMarket>();
                List<MarketCatalogueSelection> lstMarketCatalogueSelections = new List<MarketCatalogueSelection>();
                var adminID = 1;


                var competitions = objBettingService.listCompetitions("2", false, Password);
                foreach (var competition in competitions)
                {
                    var events = objBettingService.listEvents(competition.Competition.Id, false, Password);
                    foreach (var eventitem in events)
                    {
                        List<String> lstIDs = new List<string>();
                        var marketcatalouges = objBettingService.listMarketCatalogue(eventitem.Event.Id, lstIDs, false, Password);
                        List<SP_Users_GetUserIDsforAllowedTennis_Result> lstUsersTennis = dbEntities.SP_Users_GetUserIDsforAllowedTennis(0).ToList<SP_Users_GetUserIDsforAllowedTennis_Result>();
                        foreach (var marketcatalogueitem in marketcatalouges)
                        {
                            foreach (var userid in lstUsersTennis)
                            {
                                UserMarket objUserMarket = new UserMarket();
                                objUserMarket.AssociateEventID = "";

                                if (userid.ID == 238)
                                {
                                    objUserMarket.BettingAllowed = true;
                                }
                                else
                                {
                                    objUserMarket.BettingAllowed = false;
                                }

                                objUserMarket.EventTypeID = "2";
                                objUserMarket.CompetitionID = competition.Competition.Id;
                                objUserMarket.EventID = eventitem.Event.Id;
                                objUserMarket.MarketCatalogueID = marketcatalogueitem.MarketId;
                                objUserMarket.UpdatedbyID = 1;
                                objUserMarket.EventTypeName = "Tennis";
                                objUserMarket.CompetitionName = competition.Competition.Name;
                                objUserMarket.EventName = eventitem.Event.Name;
                                objUserMarket.MarketCatalogueName = marketcatalogueitem.MarketName;
                                objUserMarket.EventOpenDate = marketcatalogueitem.Description.MarketTime;
                                objUserMarket.SheetName = "";
                                objUserMarket.UserID = userid.ID;
                                objUserMarket.CountryCode = marketcatalogueitem.Event.CountryCode;
                                lstUserMarket.Add(objUserMarket);
                                // dbEntities.SP_UserMarket_Insert(userid.ID, "2", competition.Competition.Id, eventitem.Event.Id, marketcatalogueitem.MarketId, adminID, "Tennis", competition.Competition.Name, eventitem.Event.Name, marketcatalogueitem.MarketName, marketcatalogueitem.Description.MarketTime, "", "");
                            }

                            dbEntities.SP_MarketCatalogueSelections_Delete(marketcatalogueitem.MarketId);
                            foreach (var selectionitem in marketcatalogueitem.Runners)
                            {

                                MarketCatalogueSelection objMarketCatalogue = new MarketCatalogueSelection();
                                objMarketCatalogue.MarketCatalogueID = marketcatalogueitem.MarketId;
                                objMarketCatalogue.SelectionID = selectionitem.SelectionId.ToString();
                                objMarketCatalogue.SelectionName = selectionitem.RunnerName;
                                objMarketCatalogue.JockeyName = jockeyname;
                                objMarketCatalogue.Wearing = wearing;
                                objMarketCatalogue.WearingDesc = waringdesc;
                                objMarketCatalogue.ClothNumber = clothnumber;
                                objMarketCatalogue.StallDraw = "";
                                lstMarketCatalogueSelections.Add(objMarketCatalogue);
                                //  dbEntities.SP_MarketCatalogueSelections_Insert(marketcatalogueitem.MarketId, selectionitem.SelectionId.ToString(), selectionitem.RunnerName, jockeyname, wearing, waringdesc, clothnumber, "");
                            }
                        }
                    }
                }
                dbEntities.UserMarkets.AddRange(lstUserMarket);
                dbEntities.MarketCatalogueSelections.AddRange(lstMarketCatalogueSelections);
                dbEntities.SaveChanges();

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
            {
                APIConfig.LogError(ex);
            }
            catch (System.Data.Entity.Core.EntityCommandCompilationException ex)
            {
                APIConfig.LogError(ex);
            }
            catch (System.Data.Entity.Core.UpdateException ex)
            {
                APIConfig.LogError(ex);
            }

            catch (System.Data.Entity.Infrastructure.DbUpdateException ex) //DbContext
            {
                APIConfig.LogError(ex);
            }
            catch (Exception ex)
            {
                APIConfig.LogError(ex);
            }

        }
        public void DownloadAllMarketSoccer(string Password)
        {
            var jockeyname = "Not";
            var wearing = "Not";
            var waringdesc = "Not";
            var clothnumber = "Not";

            try
            {
                BettingService objBettingService = new BettingService();

                var adminID = 1;
                List<UserMarket> lstUserMarket = new List<UserMarket>();
                List<MarketCatalogueSelection> lstMarketCatalogueSelections = new List<MarketCatalogueSelection>();

                var competitions = objBettingService.listCompetitions("1", false, Password);
                foreach (var competition in competitions)
                {
                    var events = objBettingService.listEvents(competition.Competition.Id, false, Password);
                    foreach (var eventitem in events)
                    {
                        List<String> lstIDs = new List<string>();
                        var marketcatalouges = objBettingService.listMarketCatalogue(eventitem.Event.Id, lstIDs, false, Password);
                        List<SP_Users_GetUserIDsforAllowedSoccer_Result> lstUsersTennis = dbEntities.SP_Users_GetUserIDsforAllowedSoccer(0).ToList<SP_Users_GetUserIDsforAllowedSoccer_Result>();
                        foreach (var marketcatalogueitem in marketcatalouges)
                        {
                            foreach (var userid in lstUsersTennis)
                            {
                                UserMarket objUserMarket = new UserMarket();
                                objUserMarket.AssociateEventID = "";

                                if (userid.ID == 238)
                                {
                                    objUserMarket.BettingAllowed = true;
                                }
                                else
                                {
                                    objUserMarket.BettingAllowed = false;
                                }

                                objUserMarket.EventTypeID = "1";
                                objUserMarket.CompetitionID = competition.Competition.Id;
                                objUserMarket.EventID = eventitem.Event.Id;
                                objUserMarket.MarketCatalogueID = marketcatalogueitem.MarketId;
                                objUserMarket.UpdatedbyID = 1;
                                objUserMarket.EventTypeName = "Soccer";
                                objUserMarket.CompetitionName = competition.Competition.Name;
                                objUserMarket.EventName = eventitem.Event.Name;
                                objUserMarket.MarketCatalogueName = marketcatalogueitem.MarketName;
                                objUserMarket.EventOpenDate = marketcatalogueitem.Description.MarketTime;
                                objUserMarket.SheetName = "";
                                objUserMarket.UserID = userid.ID;
                                objUserMarket.CountryCode = marketcatalogueitem.Event.CountryCode;
                                lstUserMarket.Add(objUserMarket);
                                //  dbEntities.SP_UserMarket_Insert(userid.ID, "1", competition.Competition.Id, eventitem.Event.Id, marketcatalogueitem.MarketId, adminID, "Soccer", competition.Competition.Name, eventitem.Event.Name, marketcatalogueitem.MarketName, marketcatalogueitem.Description.MarketTime, "", "");
                            }

                            dbEntities.SP_MarketCatalogueSelections_Delete(marketcatalogueitem.MarketId);
                            foreach (var selectionitem in marketcatalogueitem.Runners)
                            {

                                MarketCatalogueSelection objMarketCatalogue = new MarketCatalogueSelection();
                                objMarketCatalogue.MarketCatalogueID = marketcatalogueitem.MarketId;
                                objMarketCatalogue.SelectionID = selectionitem.SelectionId.ToString();
                                objMarketCatalogue.SelectionName = selectionitem.RunnerName;
                                objMarketCatalogue.JockeyName = jockeyname;
                                objMarketCatalogue.Wearing = wearing;
                                objMarketCatalogue.WearingDesc = waringdesc;
                                objMarketCatalogue.ClothNumber = clothnumber;
                                objMarketCatalogue.StallDraw = "";
                                lstMarketCatalogueSelections.Add(objMarketCatalogue);
                                // dbEntities.SP_MarketCatalogueSelections_Insert(marketcatalogueitem.MarketId, selectionitem.SelectionId.ToString(), selectionitem.RunnerName, jockeyname, wearing, waringdesc, clothnumber, "");
                            }
                        }
                    }
                }
                dbEntities.UserMarkets.AddRange(lstUserMarket);
                dbEntities.MarketCatalogueSelections.AddRange(lstMarketCatalogueSelections);
                dbEntities.SaveChanges();

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
            {
                APIConfig.LogError(ex);
            }
            catch (System.Data.Entity.Core.EntityCommandCompilationException ex)
            {
                APIConfig.LogError(ex);
            }
            catch (System.Data.Entity.Core.UpdateException ex)
            {
                APIConfig.LogError(ex);
            }

            catch (System.Data.Entity.Infrastructure.DbUpdateException ex) //DbContext
            {
                APIConfig.LogError(ex);
            }
            catch (Exception ex)
            {
                APIConfig.LogError(ex);
            }

        }
        public string InsertUserMarket(string[] allmarketitems, int userID, int UpdatedbyID, bool DeleteOldMarkets)
        {
            try
            {
                if (DeleteOldMarkets == true)
                {
                    dbEntities.SP_UserMarket_DeletebyUserID(userID);
                }

                List<UserMarket> lstUserMarket = new List<UserMarket>();
                List<MarketCatalogueSelection> lstMarketCatalogueSelections = new List<MarketCatalogueSelection>();

                if (allmarketitems != null)
                {
                    foreach (var item in allmarketitems)
                    {
                        string[] items = item.Split('#');
                        string associateeventID = "";

                        UserMarket objUserMarket = new UserMarket();
                        if (items.Length >= 11)
                        {
                            associateeventID = items[10];
                            try
                            {


                                objUserMarket.GetMatchUpdatesFrom = items[11];
                                objUserMarket.TotalOvers = items[12];
                                objUserMarket.CountryCode = items[13];
                            }
                            catch (System.Exception ex)
                            {

                            }
                        }
                        objUserMarket.AssociateEventID = associateeventID;

                        if (items[1] == "Cricket")
                        {
                            objUserMarket.BettingAllowed = false;
                        }
                        else
                        {
                            objUserMarket.BettingAllowed = true;
                        }
                        objUserMarket.EventTypeID = items[0];
                        objUserMarket.CompetitionID = items[2];
                        objUserMarket.EventID = items[4];
                        objUserMarket.MarketCatalogueID = items[6];
                        objUserMarket.UpdatedbyID = UpdatedbyID;
                        objUserMarket.EventTypeName = items[1];
                        objUserMarket.CompetitionName = items[3];
                        objUserMarket.EventName = items[5];
                        objUserMarket.MarketCatalogueName = items[7];
                        if (objUserMarket.MarketCatalogueName.Contains("Line"))
                        {
                            objUserMarket.EventName = "Line v Markets";
                            objUserMarket.AssociateEventID = objUserMarket.EventID;

                        }
                        if (objUserMarket.MarketCatalogueName.Contains("Match Odds"))
                        {

                            objUserMarket.GetMatchUpdatesFrom = "Local";
                        }
                        objUserMarket.EventOpenDate = Convert.ToDateTime(items[9]);
                        objUserMarket.SheetName = "";
                        objUserMarket.UserID = userID;
                        lstUserMarket.Add(objUserMarket);
                        // dbEntities.SP_UserMarket_Insert(userID, items[0], items[2], items[4], items[6], UpdatedbyID, items[1], items[3], items[5], items[7], Convert.ToDateTime(items[9]), "", associateeventID);
                        try
                        {
                            if (items[8] != "NotInsert")
                            {
                                if (DeleteOldMarkets == true)
                                {
                                    dbEntities.SP_MarketCatalogueSelections_Delete(items[6]);
                                }
                                string[] selectionitems = items[8].Split('=');
                                for (int i = 0; i <= selectionitems.Count() - 1; i++)
                                {
                                    var selectionitemandname = selectionitems[i].Split('|');
                                    MarketCatalogueSelection objMarketCatalogue = new MarketCatalogueSelection();
                                    objMarketCatalogue.MarketCatalogueID = items[6];
                                    objMarketCatalogue.SelectionID = selectionitemandname[0];
                                    objMarketCatalogue.SelectionName = selectionitemandname[1];
                                    objMarketCatalogue.JockeyName = selectionitemandname[2];
                                    objMarketCatalogue.Wearing = selectionitemandname[4];
                                    objMarketCatalogue.WearingDesc = selectionitemandname[3];
                                    objMarketCatalogue.ClothNumber = selectionitemandname[5];
                                    objMarketCatalogue.StallDraw = "";
                                    lstMarketCatalogueSelections.Add(objMarketCatalogue);
                                    //  dbEntities.SP_MarketCatalogueSelections_Insert(items[6].ToString(), selectionitemandname[0], selectionitemandname[1], selectionitemandname[2], selectionitemandname[4], selectionitemandname[3], selectionitemandname[5], "");

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            APIConfig.LogError(ex);
                        }

                    }

                }
                List<SP_Users_GetUserbyCreatedbyID_Result> lstUSers = dbEntities.SP_Users_GetUserbyCreatedbyID(userID).ToList<SP_Users_GetUserbyCreatedbyID_Result>();
                foreach (var user in lstUSers)
                {
                    if (DeleteOldMarkets == true)
                    {
                        dbEntities.SP_UserMarket_DeletebyUserID(user.ID);
                    }
                    if (allmarketitems != null)
                    {
                        try
                        {
                            foreach (var item in allmarketitems)
                            {
                                string[] items = item.Split('#');
                                string associateeventID = "";
                                //if (items.Length == 11)
                                //{
                                //    associateeventID = items[10];
                                //}
                                UserMarket objUserMarket = new UserMarket();
                                if (items.Length >= 11)
                                {
                                    associateeventID = items[10];
                                    try
                                    {
                                        objUserMarket.GetMatchUpdatesFrom = items[11];
                                        objUserMarket.TotalOvers = items[12];
                                        objUserMarket.CountryCode = items[13];
                                    }
                                    catch (System.Exception ex)
                                    {

                                    }
                                }
                                objUserMarket.AssociateEventID = associateeventID;
                                if (items[1] == "Cricket")
                                {
                                    objUserMarket.BettingAllowed = false;
                                }
                                else
                                {
                                    objUserMarket.BettingAllowed = true;
                                }
                                objUserMarket.EventTypeID = items[0];
                                objUserMarket.CompetitionID = items[2];
                                objUserMarket.EventID = items[4];
                                objUserMarket.MarketCatalogueID = items[6];
                                objUserMarket.UpdatedbyID = UpdatedbyID;
                                objUserMarket.EventTypeName = items[1];
                                objUserMarket.CompetitionName = items[3];
                                objUserMarket.EventName = items[5];
                                objUserMarket.MarketCatalogueName = items[7];
                                if (objUserMarket.MarketCatalogueName.Contains("Line"))
                                {
                                    objUserMarket.EventName = "Line v Markets";
                                    objUserMarket.AssociateEventID = objUserMarket.EventID;

                                }
                                if (objUserMarket.MarketCatalogueName.Contains("Match Odds"))
                                {

                                    objUserMarket.GetMatchUpdatesFrom = "Local";
                                }
                                objUserMarket.EventOpenDate = Convert.ToDateTime(items[9]);
                                objUserMarket.SheetName = "";
                                objUserMarket.UserID = user.ID;
                                lstUserMarket.Add(objUserMarket);
                                //  dbEntities.SP_UserMarket_Insert(user.ID, items[0], items[2], items[4], items[6], UpdatedbyID, items[1], items[3], items[5], items[7], Convert.ToDateTime(items[9]), "", associateeventID);

                            }
                        }
                        catch (System.Exception ex)
                        {
                            APIConfig.LogError(ex);
                        }

                    }
                }
                dbEntities.UserMarkets.AddRange(lstUserMarket);

                dbEntities.MarketCatalogueSelections.AddRange(lstMarketCatalogueSelections);
                dbEntities.SaveChanges();

                return "True";

            }
            catch (Exception ex)
            {
                APIConfig.LogError(ex);
                return "False";
            }


        }
        public string InsertIndainFancy(ExternalAPI.TO.MarketBookForindianFancy[] allmarkets,List<ExternalAPI.TO.RunnerForIndianFancy> runners, int[] userIDs, string EventID, string Password)
        {
            ExternalAPI.TO.RunnerForIndianFancy obj = new ExternalAPI.TO.RunnerForIndianFancy();
            List<SP_UserMarket_GetUserMarketbyEventID1_Result> lstkalijuttmarket = new List<SP_UserMarket_GetUserMarketbyEventID1_Result>();
            lstkalijuttmarket = JsonConvert.DeserializeObject<List<SP_UserMarket_GetUserMarketbyEventID1_Result>>(GetMarketbyEventID1(EventID));

            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            List<UserMarket> lstUserMarket = new List<UserMarket>();
            List<MarketCatalogueSelection> lstMarketCatalogueSelections = new List<MarketCatalogueSelection>();
            foreach (var UserID in userIDs)
            {
                foreach (var usermarket in allmarkets)
                {
                    int i = 1;
                    foreach (var runner in runners)
                    {                      
                        UserMarket objUserMarket = new UserMarket();
                        objUserMarket.UserID = UserID;
                        objUserMarket.EventTypeID = "4";
                        objUserMarket.CompetitionID = lstkalijuttmarket[0].CompetitionID;
                        objUserMarket.EventID = EventID;
                        objUserMarket.MarketCatalogueID = lstkalijuttmarket[0].CompetitionID+i;
                        objUserMarket.UpdatedbyID = 1;
                        objUserMarket.EventTypeName = lstkalijuttmarket[0].EventTypeName;
                        objUserMarket.CompetitionName = lstkalijuttmarket[0].CompetitionName;
                        objUserMarket.EventName = "Line v Markets";
                        objUserMarket.MarketCatalogueName = runner.RunnerName;
                        objUserMarket.isOpenedbyUser = true;
                        objUserMarket.EventOpenDate = lstkalijuttmarket[0].EventOpenDate;
                        objUserMarket.SheetName = "";
                        objUserMarket.BettingAllowed = true;
                        objUserMarket.AssociateEventID = EventID;
                        objUserMarket.MarketStatus = "";
                        lstUserMarket.Add(objUserMarket);
                        i++;
                        //dbEntities.SpInsertIndianFancy1(obj.SelectionId, obj.RunnerName, obj.MarketBookID);
                    }
                }
            }
            foreach (var usermarket in allmarkets)
            {
                int i = 1;
                foreach (var item in runners)
                {                  
                        MarketCatalogueSelection objMarketCatalogue = new MarketCatalogueSelection();
                        var selectionID = item.SelectionId;
                        var SelectionName = item.RunnerName;
                        objMarketCatalogue.SelectionID = selectionID;
                        objMarketCatalogue.SelectionName = SelectionName;
                    objMarketCatalogue.MarketCatalogueID = lstkalijuttmarket[0].CompetitionID + i;
                        objMarketCatalogue.JockeyName = "Not";
                        objMarketCatalogue.Wearing = "Not";
                        objMarketCatalogue.WearingDesc = "Not";
                        objMarketCatalogue.ClothNumber = "Not";
                        objMarketCatalogue.StallDraw = "";
                        lstMarketCatalogueSelections.Add(objMarketCatalogue);
                    i++;
                        //objUsersServiceCleint.MarketCatalogueSelectionskalijut((firstlis).ToString(), selectionID.ToString(), selectionName.ToString(), "Not", "Not", "Not", "Not", "");                  
                }
            }
            dbEntities.UserMarkets.AddRange(lstUserMarket);

            dbEntities.MarketCatalogueSelections.AddRange(lstMarketCatalogueSelections);
            dbEntities.SaveChanges();
            return "True";
        }
        public string InsertUserMarketFigure(string[] Figure, string[] FigureCatelogIDs, string[] Figureselections, string[] FigureselectionsName, int[] userIDs, string EventID, int UpdatedbyID)
        {
            try
            {
                List<SP_UserMarket_GetUserMarketbyEventID1_Result> lstkalijuttmarket = new List<SP_UserMarket_GetUserMarketbyEventID1_Result>();
                lstkalijuttmarket = JsonConvert.DeserializeObject<List<SP_UserMarket_GetUserMarketbyEventID1_Result>>(GetMarketbyEventID1(EventID));

                List<UserMarket> lstUserMarket = new List<UserMarket>();
                List<MarketCatalogueSelection> lstMarketCatalogueSelections = new List<MarketCatalogueSelection>();
                foreach (var UserID in userIDs)
                {

                    foreach (var item in FigureCatelogIDs.Zip(Figure, (x, y) => new { A = x, B = y }))
                    {
                        var MArketcatID = item.A;
                        var MArketCatNAme = item.B;

                        UserMarket objUserMarket = new UserMarket();
                        objUserMarket.UserID = UserID;
                        objUserMarket.EventTypeID = "4";
                        objUserMarket.CompetitionID = lstkalijuttmarket[0].CompetitionID;
                        objUserMarket.EventID = EventID;
                        objUserMarket.MarketCatalogueID = MArketcatID;
                        objUserMarket.UpdatedbyID = 1;
                        objUserMarket.EventTypeName = lstkalijuttmarket[0].EventTypeName;
                        objUserMarket.CompetitionName = lstkalijuttmarket[0].CompetitionName;
                        objUserMarket.EventName = "Figure";
                        objUserMarket.MarketCatalogueName = MArketCatNAme;
                        objUserMarket.isOpenedbyUser = false;
                        objUserMarket.EventOpenDate = lstkalijuttmarket[0].EventOpenDate;
                        objUserMarket.SheetName = "";
                        objUserMarket.BettingAllowed = false;
                        objUserMarket.AssociateEventID = EventID;
                        objUserMarket.MarketStatus = "";

                        lstUserMarket.Add(objUserMarket);
                    }
                }
                
                foreach (var firstlis in FigureCatelogIDs)
                {     
                    foreach (var item in Figureselections.Zip(FigureselectionsName, (x, y) => new { A = x, B = y }))
                    {
                        MarketCatalogueSelection objMarketCatalogue = new MarketCatalogueSelection();
                        var selectionID = item.A;
                        var SelectionName = item.B;
                        objMarketCatalogue.SelectionID = selectionID;
                        objMarketCatalogue.SelectionName = SelectionName;
                        objMarketCatalogue.MarketCatalogueID = firstlis;
                        objMarketCatalogue.JockeyName = "Not";
                        objMarketCatalogue.Wearing = "Not";
                        objMarketCatalogue.WearingDesc = "Not";
                        objMarketCatalogue.ClothNumber = "Not";
                        objMarketCatalogue.StallDraw = "";
                        lstMarketCatalogueSelections.Add(objMarketCatalogue);
                        //objUsersServiceCleint.MarketCatalogueSelectionskalijut((firstlis).ToString(), selectionID.ToString(), selectionName.ToString(), "Not", "Not", "Not", "Not", "");
                    }
                    
                }

                //List<SP_Users_GetUserbyCreatedbyID_Result> lstUSers = dbEntities.SP_Users_GetUserbyCreatedbyID(userID).ToList<SP_Users_GetUserbyCreatedbyID_Result>();


                dbEntities.UserMarkets.AddRange(lstUserMarket);

                dbEntities.MarketCatalogueSelections.AddRange(lstMarketCatalogueSelections);
                dbEntities.SaveChanges();

                return "True";


            }
            catch (Exception ex)
            {
                APIConfig.LogError(ex);
                return "False";
            }


        }
        public string InsertUserMarketKJ(string[] kalijutt, string[] KaliCatelogIDs, string[] selections, int[] userIDs,string EventID, int UpdatedbyID)
        {
            try
            {
                List<SP_UserMarket_GetUserMarketbyEventID1_Result> lstkalijuttmarket = new List<SP_UserMarket_GetUserMarketbyEventID1_Result>();
                lstkalijuttmarket = JsonConvert.DeserializeObject<List<SP_UserMarket_GetUserMarketbyEventID1_Result>>(GetMarketbyEventID1(EventID));

                List<UserMarket> lstUserMarket = new List<UserMarket>();
                List<MarketCatalogueSelection> lstMarketCatalogueSelections = new List<MarketCatalogueSelection>();
                foreach (var UserID in userIDs)
                {
                    
                    foreach (var item in KaliCatelogIDs.Zip(kalijutt, (x, y) => new { A = x, B = y }))
                    {
                        var MArketcatID = item.A;
                        var MArketCatNAme = item.B;
                        
                            UserMarket objUserMarket = new UserMarket();
                            objUserMarket.UserID = UserID;
                            objUserMarket.EventTypeID = "4";
                            objUserMarket.CompetitionID = lstkalijuttmarket[0].CompetitionID;
                            objUserMarket.EventID = EventID;
                            objUserMarket.MarketCatalogueID = MArketcatID;
                            objUserMarket.UpdatedbyID = 1;
                            objUserMarket.EventTypeName = lstkalijuttmarket[0].EventTypeName;
                            objUserMarket.CompetitionName = lstkalijuttmarket[0].CompetitionName;
                            objUserMarket.EventName = "Kali v Jut";
                            objUserMarket.MarketCatalogueName = MArketCatNAme;
                            objUserMarket.isOpenedbyUser = false;
                            objUserMarket.EventOpenDate = lstkalijuttmarket[0].EventOpenDate;
                            objUserMarket.SheetName = "";
                            objUserMarket.BettingAllowed = false;
                            objUserMarket.AssociateEventID = EventID;
                            objUserMarket.MarketStatus = "";

                            lstUserMarket.Add(objUserMarket);
                        }
                
                }
               
                foreach (var firstlis in KaliCatelogIDs)
                {
                   
                    MarketCatalogueSelection objMarketCatalogue = new MarketCatalogueSelection();
                    
                        objMarketCatalogue.SelectionID = selections[0].ToString();
                        objMarketCatalogue.SelectionName = selections[1].ToString();               
                        objMarketCatalogue.MarketCatalogueID = firstlis;                     
                        objMarketCatalogue.JockeyName = "Not";
                        objMarketCatalogue.Wearing = "Not";
                        objMarketCatalogue.WearingDesc = "Not";
                        objMarketCatalogue.ClothNumber = "Not";
                        objMarketCatalogue.StallDraw = "";
                        lstMarketCatalogueSelections.Add(objMarketCatalogue);
                        //objUsersServiceCleint.MarketCatalogueSelectionskalijut((firstlis).ToString(), selectionID.ToString(), selectionName.ToString(), "Not", "Not", "Not", "Not", "");
                    
                }

                //List<SP_Users_GetUserbyCreatedbyID_Result> lstUSers = dbEntities.SP_Users_GetUserbyCreatedbyID(userID).ToList<SP_Users_GetUserbyCreatedbyID_Result>();


                dbEntities.UserMarkets.AddRange(lstUserMarket);

                dbEntities.MarketCatalogueSelections.AddRange(lstMarketCatalogueSelections);
                dbEntities.SaveChanges();

                return "True";


            }
            catch (Exception ex)
            {
                APIConfig.LogError(ex);
                return "False";
            }


        }

        public string InsertUserMarketSFig(string[] SmallFig, string[] smallFigCatelogIDs, string[] selections, int[] userIDs, string EventID, int UpdatedbyID)
        {
            try
            {
                List<SP_UserMarket_GetUserMarketbyEventID1_Result> lstkalijuttmarket = new List<SP_UserMarket_GetUserMarketbyEventID1_Result>();
                lstkalijuttmarket = JsonConvert.DeserializeObject<List<SP_UserMarket_GetUserMarketbyEventID1_Result>>(GetMarketbyEventID1(EventID));

                List<UserMarket> lstUserMarket = new List<UserMarket>();
                List<MarketCatalogueSelection> lstMarketCatalogueSelections = new List<MarketCatalogueSelection>();
                foreach (var UserID in userIDs)
                {

                    foreach (var item in smallFigCatelogIDs.Zip(SmallFig, (x, y) => new { A = x, B = y }))
                    {
                        var MArketcatID = item.A;
                        var MArketCatNAme = item.B;

                        UserMarket objUserMarket = new UserMarket();
                        objUserMarket.UserID = UserID;
                        objUserMarket.EventTypeID = "4";
                        objUserMarket.CompetitionID = lstkalijuttmarket[0].CompetitionID;
                        objUserMarket.EventID = EventID;
                        objUserMarket.MarketCatalogueID = MArketcatID;
                        objUserMarket.UpdatedbyID = 1;
                        objUserMarket.EventTypeName = lstkalijuttmarket[0].EventTypeName;
                        objUserMarket.CompetitionName = lstkalijuttmarket[0].CompetitionName;
                        objUserMarket.EventName = "SmallFig";
                        objUserMarket.MarketCatalogueName = MArketCatNAme;
                        objUserMarket.isOpenedbyUser = false;
                        objUserMarket.EventOpenDate = lstkalijuttmarket[0].EventOpenDate;
                        objUserMarket.SheetName = "";
                        objUserMarket.BettingAllowed = false;
                        objUserMarket.AssociateEventID = EventID;
                        objUserMarket.MarketStatus = "";

                        lstUserMarket.Add(objUserMarket);
                    }

                }

                foreach (var firstlis in smallFigCatelogIDs)
                {

                    MarketCatalogueSelection objMarketCatalogue = new MarketCatalogueSelection();

                    objMarketCatalogue.SelectionID = selections[0].ToString();
                    objMarketCatalogue.SelectionName = selections[1].ToString();
                    objMarketCatalogue.MarketCatalogueID = firstlis;
                    objMarketCatalogue.JockeyName = "Not";
                    objMarketCatalogue.Wearing = "Not";
                    objMarketCatalogue.WearingDesc = "Not";
                    objMarketCatalogue.ClothNumber = "Not";
                    objMarketCatalogue.StallDraw = "";
                    lstMarketCatalogueSelections.Add(objMarketCatalogue);
                    //objUsersServiceCleint.MarketCatalogueSelectionskalijut((firstlis).ToString(), selectionID.ToString(), selectionName.ToString(), "Not", "Not", "Not", "Not", "");

                }

                //List<SP_Users_GetUserbyCreatedbyID_Result> lstUSers = dbEntities.SP_Users_GetUserbyCreatedbyID(userID).ToList<SP_Users_GetUserbyCreatedbyID_Result>();


                dbEntities.UserMarkets.AddRange(lstUserMarket);

                dbEntities.MarketCatalogueSelections.AddRange(lstMarketCatalogueSelections);
                dbEntities.SaveChanges();

                return "True";


            }
            catch (Exception ex)
            {
                APIConfig.LogError(ex);
                return "False";
            }


        }

        //public string InsertUserMarket(string[] allmarketitems, int userID, int UpdatedbyID)
        //{
        //    try
        //    {
        //        dbEntities.SP_UserMarket_DeletebyUserID(userID);
        //        if (allmarketitems != null)
        //        {
        //            foreach (var item in allmarketitems)
        //            {
        //                string[] items = item.Split('+');

        //                dbEntities.SP_UserMarket_Insert(userID, Regex.Replace(items[0], @"[^0-9a-zA-Z]+", ""), Regex.Replace(items[2], @"[^0-9a-zA-Z]+", ""), Regex.Replace(items[4], @"[^0-9a-zA-Z]+", ""), items[6], UpdatedbyID, items[1], items[3], items[5], items[7], DateTime.Now,items[9]);
        //                try
        //                {
        //                    if (items[8] != "NotInsert")
        //                    {
        //                        dbEntities.SP_MarketCatalogueSelections_Delete(items[6]);
        //                        string[] selectionitems = items[8].Split('=');
        //                        for (int i = 0; i <= selectionitems.Count() - 1; i++)
        //                        {
        //                            var selectionitemandname = selectionitems[i].Split('|');
        //                            selectionitemandname[0]= Regex.Replace(selectionitemandname[0], @"[^0-9a-zA-Z]+", "");
        //                            dbEntities.SP_MarketCatalogueSelections_Insert(items[6].ToString(), selectionitemandname[0], selectionitemandname[1]);

        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {

        //                }

        //            }
        //        }
        //        List<SP_Users_GetUserbyCreatedbyID_Result> lstUSers = dbEntities.SP_Users_GetUserbyCreatedbyID(userID).ToList<SP_Users_GetUserbyCreatedbyID_Result>();
        //        foreach (var user in lstUSers)
        //        {
        //            dbEntities.SP_UserMarket_DeletebyUserID(user.ID);
        //            if (allmarketitems != null)
        //            {
        //                foreach (var item in allmarketitems)
        //                {
        //                    string[] items = item.Split('+');

        //                    dbEntities.SP_UserMarket_Insert(user.ID, Regex.Replace(items[0], @"[^0-9a-zA-Z]+", ""), Regex.Replace(items[2], @"[^0-9a-zA-Z]+", ""), Regex.Replace(items[4], @"[^0-9a-zA-Z]+", ""), items[6], UpdatedbyID, items[1], items[3], items[5], items[7], DateTime.Now,items[9]);

        //                }
        //            }
        //        }
        //        return "True";

        //    }
        //    catch (Exception ex)
        //    {

        //        return "False";
        //    }


        //}


        public string GetUserMArket(int userID)
        {
            var results = dbEntities.SP_UserMarket_GetUserMarketbyUserID(userID).ToList<SP_UserMarket_GetUserMarketbyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public string GetUserMarketforSelection(int UserID)
        {
            var results = dbEntities.SP_UserMarket_GetMarketAssignedtoUser(UserID).ToList<SP_UserMarket_GetMarketAssignedtoUser_Result>();
            return ConverttoJSONString(results);
        }
        public string GetFavoriteEventTypes(int userID)
        {
            try
            {
                var results = dbEntities.SP_FavoriteEventTypes_GetdatabyUserID(userID).ToList<SP_FavoriteEventTypes_GetdatabyUserID_Result>();
                return ConverttoJSONString(results);
            }
            catch (Exception ex)
            {
                return "";
            }

        }
        public void AddtoFavoriteEventTypes(string EventTypeID, int userID)
        {
            dbEntities.SP_FavoriteEventType_Insert(EventTypeID, userID);

        }

        public void DeleteFromFavoriteEventTypes(string EventTypeID, int userID)
        {
            dbEntities.SP_FavoriteEventType_Delete(EventTypeID, userID);

        }

        public string GetFavoriteEvents(int userID)
        {
            var results = dbEntities.SP_FavoriteEvents_GetdatabyUserID(userID).ToList<SP_FavoriteEvents_GetdatabyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public void AddtoFavoriteEvents(string EventID, int userID)
        {
            dbEntities.SP_FavoriteEvents_Insert(EventID, userID);

        }

        public void DeleteFromFavoriteEvents(string EventID, int userID)
        {
            dbEntities.SP_FavoriteEvents_Delete(EventID, userID);

        }

        public string GetFavoriteCompetitions(int userID)
        {
            var results = dbEntities.SP_FavoriteCompetitions_GetdatabyUserID(userID).ToList<SP_FavoriteCompetitions_GetdatabyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public void AddtoFavoriteCompetitions(string EventID, int userID)
        {
            dbEntities.SP_FavoriteCompetitions_Insert(EventID, userID);

        }
        public void DeleteFromFavoriteCompetitions(string EventID, int userID)
        {
            dbEntities.SP_FavoriteCompetitions_Delete(EventID, userID);

        }

        public string GetEventTypeIDs(int userID)
        {
            var results = dbEntities.SP_UserMarket_GetEventypeIDs(userID).ToList<SP_UserMarket_GetEventypeIDs_Result>();
            return ConverttoJSONString(results);
        }
        public string GetCompetitionIDs(string eventTypeID, int userID)
        {
            var results = dbEntities.SP_UserMarket_GetCompetitionIDs(userID, eventTypeID).ToList<SP_UserMarket_GetCompetitionIDs_Result>();
            return ConverttoJSONString(results);
        }

        public string GetEventsIDs(string CompetitionID, int userID)
        {
            var results = dbEntities.SP_UserMarket_GetEventIDs(userID, CompetitionID).ToList<SP_UserMarket_GetEventIDs_Result>();
            return ConverttoJSONString(results);
        }
        public string GetMarketbyEventID(string EventID)
        {
            var results = dbEntities.SP_UserMarket_GetUserMarketbyEventID(EventID).ToList<SP_UserMarket_GetUserMarketbyEventID_Result>();
            return ConverttoJSONString(results);
        }
        public string GetMarketbyEventID1(string EventID)
        {
            var results = dbEntities.SP_UserMarket_GetUserMarketbyEventID1(EventID).ToList<SP_UserMarket_GetUserMarketbyEventID1_Result>();
            return ConverttoJSONString(results);
        }
        public string GetMarketCatalogueIDs(string eventID, int userID)
        {
            var results = dbEntities.SP_UserMarket_GetMarketCatalogueIDs(userID, eventID).ToList<SP_UserMarket_GetMarketCatalogueIDs_Result>();
            return ConverttoJSONString(results);
        }
        public SP_Users_GetMaxOddBackandLay_Result GetMaxOddBackandLay(int UserID)
        {
            return dbEntities.SP_Users_GetMaxOddBackandLay(UserID).FirstOrDefault();
        }
        public string GetMaxOddBackandLayStr(int UserID)
        {
            return ConverttoJSONString(dbEntities.SP_Users_GetMaxOddBackandLay(UserID).FirstOrDefault());
        }
        public void UpdateMaxOddBackandLay(int UserID, decimal MaxOddBack, bool CheckForMaxOddBack, decimal MaxOddLay, bool CheckForMaxOddLay)
        {
            dbEntities.SP_Users_UpdateMaxOddBackandLay(UserID, MaxOddBack, CheckForMaxOddBack, MaxOddLay, CheckForMaxOddLay);
        }
        public string InsertUserBet(string SelectionID, int userID, string UserOdd, string amount, string bettype, string LiveOdd, bool ismatched, string status, string marketbookId, DateTime createddate, DateTime updatedtime, string Selectionname, string Marketbookname, string Liability, string BetSize, decimal PendingAmount, string location, long ParentID, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            try
            {
                if (UserOdd == "0" && location != "8")
                {
                    return "";
                }

                if (location == "8" || location == "9" )
                {
                    var ID = dbEntities.SP_UserBets_Insert(userID, SelectionID, UserOdd, amount, bettype, LiveOdd, ismatched, status, marketbookId, DateTime.Now, DateTime.Now, Selectionname, Marketbookname, Crypto.Decrypt(GetAgentRate(userID)), Liability, BetSize, PendingAmount, location, ParentID).FirstOrDefault().Value.ToString();

                    return ID;
                }              
                else
                {
                        string eventtypename = GetEventTypeNamebyMarketID(marketbookId);
                        if (CheckforMaxOddBack == true && eventtypename.Contains("Racing") && bettype == "back")
                        {
                            if (Convert.ToDecimal(LiveOdd) > MaxOddBack)
                            {
                                LiveOdd = MaxOddBack.ToString("G29");
                            }
                            if (Convert.ToDecimal(UserOdd) > MaxOddBack)
                            {
                                UserOdd = MaxOddBack.ToString("G29");
                            }

                        }
                    var ID = dbEntities.SP_UserBets_Insert(userID, SelectionID, UserOdd, amount, bettype, LiveOdd, ismatched, status, marketbookId, DateTime.Now, DateTime.Now, Selectionname, Marketbookname, Crypto.Decrypt(GetAgentRate(userID)), Liability, BetSize, PendingAmount, location, ParentID).FirstOrDefault().Value.ToString();

                    return ID;
                }
                
               
            }

            catch (Exception ex)
            {
                APIConfig.WriteErrorToDB(ex.Message);
                APIConfig.LogError(ex);
                return "";
            }
        }
        //public string InsertUserBet(string SelectionID, int userID, string UserOdd, string amount, string bettype, string LiveOdd, bool ismatched, string status, string marketbookId, DateTime createddate, DateTime updatedtime, string Selectionname, string Marketbookname, string Liability, string BetSize, decimal PendingAmount, string location, long ParentID, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay, string Password)
        //{
        //    if (ValidatePassword(Password) == false)
        //    {
        //        return "";
        //    }
        //    try
        //    {
        //        BettingService objBettingClient = new BettingService();

        //        List<string> lstIDs = new List<string>();

        //        lstIDs.Add(marketbookId);
        //        // GetMarketData obMarketdata = new GetMarketData();
        //        string eventtypename = GetEventTypeNamebyMarketID(marketbookId);
        //        var marketbooks = objBettingClient.GetMarketDatabyID(lstIDs.ToArray(), Marketbookname, DateTime.Now, eventtypename);

        //        if (marketbooks.Count == 0)
        //        {
        //            return "False";
        //        }
        //        if (marketbooks[0].IsMarketDataDelayed == true)
        //        {
        //            return "False";
        //        }
        //        if (marketbooks[0].Runners.Count == 1)
        //        {
        //            ismatched = false;
        //            if (bettype == "back")
        //            {
        //                foreach (var runner in marketbooks[0].Runners[0].ExchangePrices.AvailableToBack.Take(1))
        //                {
        //                    if (Convert.ToDouble(UserOdd) >= runner.Price)
        //                    {
        //                        ismatched = true;
        //                        LiveOdd = runner.Price.ToString();
        //                        UserOdd = runner.Price.ToString();

        //                    }
        //                }
        //                if (Convert.ToDouble(UserOdd) > 0)
        //                {
        //                    var ID1 = dbEntities.SP_UserBets_Insert(userID, SelectionID, UserOdd, amount, bettype, LiveOdd, ismatched, status, marketbookId, DateTime.Now, DateTime.Now, Selectionname, Marketbookname, Crypto.Decrypt(GetAgentRate(userID)), Liability, BetSize, PendingAmount, location, ParentID).FirstOrDefault().Value.ToString();
        //                    return ID1;
        //                }
        //                else
        //                {
        //                    return "False";
        //                }

        //            }
        //            else
        //            {
        //                foreach (var runner in marketbooks[0].Runners[0].ExchangePrices.AvailableToLay.Take(1))
        //                {
        //                    if (Convert.ToDouble(UserOdd) <= runner.Price)
        //                    {
        //                        ismatched = true;
        //                        LiveOdd = runner.Price.ToString();
        //                        UserOdd = runner.Price.ToString();
        //                    }
        //                }
        //                if (Convert.ToDouble(UserOdd) > 0)
        //                {
        //                    var ID1 = dbEntities.SP_UserBets_Insert(userID, SelectionID, UserOdd, amount, bettype, LiveOdd, ismatched, status, marketbookId, DateTime.Now, DateTime.Now, Selectionname, Marketbookname, Crypto.Decrypt(GetAgentRate(userID)), Liability, BetSize, PendingAmount, location, ParentID).FirstOrDefault().Value.ToString();
        //                    return ID1;
        //                }
        //                else
        //                {
        //                    return "False";
        //                }
        //            }


        //        }
        //        if (ismatched == true)
        //        {

        //            var selectedrunner = marketbooks[0].Runners.Where(item => item.SelectionId == SelectionID).FirstOrDefault();
        //            if (bettype == "back")
        //            {


        //                if (Convert.ToInt32(location) == 0)
        //                {
        //                    if (selectedrunner.ExchangePrices.AvailableToBack[0].Price.ToString() != UserOdd)
        //                    {
        //                        ismatched = false;
        //                        LiveOdd = selectedrunner.ExchangePrices.AvailableToBack[0].Price.ToString();
        //                    }
        //                }
        //                else
        //                {
        //                    if (Convert.ToInt32(location) == 1)
        //                    {
        //                        if (selectedrunner.ExchangePrices.AvailableToBack[1].Price.ToString() != UserOdd)
        //                        {
        //                            ismatched = false;
        //                            LiveOdd = selectedrunner.ExchangePrices.AvailableToBack[1].Price.ToString();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (Convert.ToInt32(location) == 2)
        //                        {
        //                            if (selectedrunner.ExchangePrices.AvailableToBack[2].Price.ToString() != UserOdd)
        //                            {
        //                                ismatched = false;
        //                                LiveOdd = selectedrunner.ExchangePrices.AvailableToBack[2].Price.ToString();
        //                            }
        //                        }
        //                    }
        //                }
        //                //if (CheckforMaxOddBack == true && eventtypename.Contains("Racing"))
        //                //{
        //                //    if (Convert.ToDecimal(LiveOdd) > MaxOddBack)
        //                //    {
        //                //        LiveOdd = MaxOddBack.ToString("G29");

        //                //    }
        //                //    if (Convert.ToDecimal(UserOdd) > MaxOddBack)
        //                //    {
        //                //        UserOdd = MaxOddBack.ToString("G29");
        //                //    }
        //                //}

        //            }
        //            else
        //            {
        //                if (Convert.ToInt32(location) == 0)
        //                {
        //                    if (selectedrunner.ExchangePrices.AvailableToLay[0].Price.ToString() != UserOdd)
        //                    {

        //                        ismatched = false;
        //                        LiveOdd = selectedrunner.ExchangePrices.AvailableToLay[0].Price.ToString();
        //                    }
        //                }
        //                else
        //                {
        //                    if (Convert.ToInt32(location) == 1)
        //                    {
        //                        if (selectedrunner.ExchangePrices.AvailableToLay[1].Price.ToString() != UserOdd)
        //                        {

        //                            ismatched = false;
        //                            LiveOdd = selectedrunner.ExchangePrices.AvailableToLay[1].Price.ToString();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (Convert.ToInt32(location) == 2)
        //                        {
        //                            if (selectedrunner.ExchangePrices.AvailableToLay[2].Price.ToString() != UserOdd)
        //                            {

        //                                ismatched = false;
        //                                LiveOdd = selectedrunner.ExchangePrices.AvailableToLay[2].Price.ToString();
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (CheckforMaxOddBack == true && eventtypename.Contains("Racing") && bettype == "back")
        //        {
        //            if (Convert.ToDecimal(LiveOdd) > MaxOddBack)
        //            {
        //                LiveOdd = MaxOddBack.ToString("G29");

        //            }
        //            if (Convert.ToDecimal(UserOdd) > MaxOddBack)
        //            {
        //                UserOdd = MaxOddBack.ToString("G29");
        //            }
        //        }
        //        var ID = dbEntities.SP_UserBets_Insert(userID, SelectionID, UserOdd, amount, bettype, LiveOdd, ismatched, status, marketbookId, DateTime.Now, DateTime.Now, Selectionname, Marketbookname, Crypto.Decrypt(GetAgentRate(userID)), Liability, BetSize, PendingAmount, location, ParentID).FirstOrDefault().Value.ToString();
        //        return ID;

        //    }

        //    catch (Exception ex)
        //    {
        //        APIConfig.WriteErrorToDB(ex.Message);
        //        APIConfig.LogError(ex);
        //        return "";
        //    }
        //}
        public string InsertUserBetAdmin(string SelectionID, int userID, string UserOdd, string amount, string bettype, string LiveOdd, bool ismatched, string status, string marketbookId, DateTime createddate, DateTime updatedtime, string Selectionname, string Marketbookname, string Liability, string BetSize, decimal PendingAmount, string location, long ParentID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            try
            {

                var ID = dbEntities.SP_UserBets_Insert(userID, SelectionID, UserOdd, amount, bettype, LiveOdd, ismatched, status, marketbookId, createddate, updatedtime, Selectionname, Marketbookname, Crypto.Decrypt(GetAgentRate(userID)), Liability, BetSize, PendingAmount, location, ParentID).FirstOrDefault().Value.ToString();
                return ID;

            }

            catch (Exception ex)
            {
                return "";
            }
        }
        public bool UpdateUserBet(long ID, int userID, string UserOdd, string amount, string bettype, string LiveOdd, bool ismatched, string status, string marketbookId, DateTime createddate, DateTime updatedtime, string Liabality, string BetSize, decimal PendingAmount, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return false;
            }
            dbEntities.SP_UserBets_Update(ID, UserOdd, amount, LiveOdd, ismatched, updatedtime, GetAgentRate(userID), Liabality, BetSize, PendingAmount);
            return true;
        }
        public bool UpdateUserbetamountbyID(long ID, decimal amount, bool ismatched, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return false;
            }
            dbEntities.SP_UserBets_UpdateuserBetbyID(ID, amount, ismatched, DateTime.Now);
            return true;
        }
        public string GetAllUserMarketbyUserID(int userID)
        {
            var results = dbEntities.SP_UsersMarket_GetAllDatabyUserID(userID).ToList<SP_UsersMarket_GetAllDatabyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public bool UpdateUserbetamountbyParentID(long ID, decimal amount, string userodd, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return false;
            }
            dbEntities.SP_UserBets_UpdateuserBetbyParentID(ID, amount, userodd, DateTime.Now);
            return true;
        }
        public string GetUserbetsbyUserIDandMarketID(int UserID, string MarketID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserBets_GetDatabyUserIDandMArketID(UserID, MarketID).ToList<SP_UserBets_GetDatabyUserIDandMArketID_Result>();
            return ConverttoJSONString(results);
        }
        public string GetUserbetsbyUserID(int UserID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserBets_GetDatabyUserID(UserID).ToList<SP_UserBets_GetDatabyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public string GetUserbetsbyUserIDandAgentID(int AgentID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserBets_GetDatabyAgentID(AgentID).ToList<SP_UserBets_GetDatabyAgentID_Result>();
            return ConverttoJSONString(results);
        }

        public void UpdateUserBetMatched(long[] ID, string Password)
        {
            if (ValidatePassword(Password) == true)
            {


                foreach (var item in ID)
                {
                    dbEntities.SP_UserBets_UpdateisMatched(item);
                }
            }

        }
        public void UpdateLiveOddbyID(long ID, string liveOdd, string Password)
        {
            if (ValidatePassword(Password) == true)
            {


                dbEntities.SP_UserBets_UpdateuserBetLiveOddbyID(ID, liveOdd, DateTime.Now);
            }
        }
        public void UpdateUserOddbyID(long ID, string UserOdd, string Password)
        {
            if (ValidatePassword(Password) == true)
            {
                dbEntities.SP_UserBets_UpdateuserBetUserOddbyID(ID, UserOdd, DateTime.Now);
            }
        }
        public void UpdateUserBetUnMatchedStatusTocompleteforCuttingUser(long ID, string Password)
        {
            if (ValidatePassword(Password) == true)
            {
                dbEntities.SP_UserBets_UpdateStatustoCompletebyIDforCuttingUser(ID);
            }
        }
        public void UpdateUserBetUnMatchedStatusTocomplete(long[] ID, string Password)
        {
            if (ValidatePassword(Password) == true)
            {
                foreach (var item in ID)
                {
                    dbEntities.SP_UserBets_UpdateStatustoCompletebyID(item);
                }
            }

        }
        public string GetCurrentLiabality(int userID)
        {
            var results = dbEntities.SP_UserBets_GetCurrentLiabality(userID).ToList<SP_UserBets_GetCurrentLiabality_Result>();
            return ConverttoJSONString(results);
        }
        public string GetAgentRate(int userID)
        {
            var results = dbEntities.SP_Users_GetAgentRate(userID).FirstOrDefault<SP_Users_GetAgentRate_Result>();

            return results.RatePercent.ToString();
        }
        //GetSuperName
        public string GetSuperName(int userID)
        {
            var results = dbEntities.SP_Users_GetAgentRate(userID).FirstOrDefault<SP_Users_GetAgentRate_Result>();

            return results.RatePercent.ToString();
        }
        public void CloseAllClosedMarkets()
        {
            try
            {


                string marketIDsDB = dbEntities.SP_UserMarket_GetDistinctMarketsOpened().FirstOrDefault();
                if (marketIDsDB != "")
                {
                    string[] marketIDs = Regex.Split(marketIDsDB, @", ");
                    foreach (var marketID in marketIDs)
                    {
                        BettingService objBettingService = new BettingService();
                        var marketbookstatus = objBettingService.GetMarketDatabyIDResultsOnly(marketID.ToString());
                        if (marketbookstatus.Count > 0)
                        {
                            if (marketbookstatus[0].Status == ExternalAPI.TO.MarketStatus.CLOSED)
                            {
                                List<SP_UserBets_GetUnCompleteMarketIDbyMarketID_Result> lstMarkets = dbEntities.SP_UserBets_GetUnCompleteMarketIDbyMarketID(marketID).ToList<SP_UserBets_GetUnCompleteMarketIDbyMarketID_Result>();
                                if (lstMarkets.Count == 0)
                                {
                                    DateTime eventstartdate = dbEntities.SP_UserMarket_GetEventOpenDatebyMarketID(marketID).FirstOrDefault().Value;
                                    eventstartdate = eventstartdate.AddHours(5);
                                    TimeSpan datediff = DateTime.Now - eventstartdate;
                                    if (datediff.Minutes > 30 && datediff.Hours == 0)
                                    {
                                        dbEntities.SP_UserMarket_SetMarketCatalogueIDClosed(marketID);
                                    }
                                    else
                                    {
                                        if (datediff.Hours > 0)
                                        {
                                            dbEntities.SP_UserMarket_SetMarketCatalogueIDClosed(marketID);
                                        }
                                    }

                                }

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                APIConfig.LogError(ex);

            }

        }
        //public List<ExternalAPI.TO.MarketBook> GetMarketDatabyIDResultsOnly(string marketID)
        //{
        //    try
        //    {
        //        var marketbooks = new List<ExternalAPI.TO.MarketBook>();
        //        string url = "http://www.betfair.com/www/sports/exchange/readonly/v1/bymarket?alt=json&currencyCode=GBP&locale=en_GB&marketIds=" + marketID + "&rollupLimit=4&rollupModel=STAKE&types=MARKET_STATE,RUNNER_STATE,RUNNER_EXCHANGE_PRICES_BEST,RUNNER_DESCRIPTION";
        //        string JsonResultsArr = "";
        //        try
        //        {

        //            using (var client = new WebClient())
        //            {


        //                JsonResultsArr = client.DownloadString(url);
        //            }
        //        }
        //        catch (System.Exception ex)
        //        {
        //            APIConfig.LogError(ex);
        //            //using (var client = new WebClient())
        //            //{


        //            //    JsonResultsArr = client.DownloadString(url);
        //            //}
        //        }
        //        //JsonResultsArr = dbEntities.SP_OddsData_GetData().FirstOrDefault();
        //        var marketbook1 = new RootObject();
        //        marketbook1 = JsonConvert.DeserializeObject<RootObject>(JsonResultsArr);
        //        MarketNode marketbookorignal = marketbook1.eventTypes.SelectMany(eventtype => eventtype.eventNodes)
        //                    .SelectMany(eventnode => eventnode.marketNodes).FirstOrDefault(marketnode => marketnode.marketId == marketID);

        //        var marketbook = new ExternalAPI.TO.MarketBook();

        //        marketbook.MarketId = marketbookorignal.marketId;
        //        marketbook.OpenDate = marketbookorignal.OpenDate;
        //        marketbook.NumberOfWinners = marketbookorignal.state.numberOfWinners;
        //        //INACTIVE, OPEN, SUSPENDED, CLOSED
        //        if (marketbookorignal.state.status == "OPEN")
        //        {
        //            marketbook.Status = ExternalAPI.TO.MarketStatus.OPEN;
        //        }
        //        else
        //        {
        //            if (marketbookorignal.state.status == "SUSPENDED")
        //            {
        //                marketbook.Status = ExternalAPI.TO.MarketStatus.SUSPENDED;
        //            }
        //            else
        //            {
        //                if (marketbookorignal.state.status == "CLOSED")
        //                {
        //                    marketbook.Status = ExternalAPI.TO.MarketStatus.CLOSED;
        //                }
        //                else
        //                {
        //                    if (marketbookorignal.state.status == "INACTIVE")
        //                    {
        //                        marketbook.Status = ExternalAPI.TO.MarketStatus.INACTIVE;
        //                    }
        //                }
        //            }
        //        }
        //        List<ExternalAPI.TO.Runner> lstRunners = new List<ExternalAPI.TO.Runner>();
        //        foreach (var runneritem in marketbookorignal.runners)
        //        {
        //            // ACTIVE, WINNER, LOSER, REMOVED_VACANT, REMOVED
        //            var runner = new ExternalAPI.TO.Runner();
        //            runner.SelectionId = runneritem.selectionId.ToString();
        //            runner.RunnerName = runneritem.description.runnerName;
        //            runner.AdjustmentFactor = runneritem.state.adjustmentFactor;
        //            if (runneritem.state.status == "WINNER")
        //            {
        //                runner.Status = ExternalAPI.TO.RunnerStatus.WINNER;
        //            }
        //            else
        //            {
        //                if (runneritem.state.status == "ACTIVE")
        //                {
        //                    runner.Status = ExternalAPI.TO.RunnerStatus.ACTIVE;
        //                }
        //                else
        //                {
        //                    if (runneritem.state.status == "LOSER")
        //                    {
        //                        runner.Status = ExternalAPI.TO.RunnerStatus.LOSER;
        //                    }
        //                    else
        //                    {
        //                        if (runneritem.state.status == "REMOVED_VACANT")
        //                        {
        //                            runner.Status = ExternalAPI.TO.RunnerStatus.REMOVED_VACANT;
        //                        }
        //                        else
        //                        {
        //                            if (runneritem.state.status == "REMOVED")
        //                            {
        //                                runner.Status = ExternalAPI.TO.RunnerStatus.REMOVED;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            lstRunners.Add(runner);
        //        }


        //        marketbook.Runners = new List<ExternalAPI.TO.Runner>(lstRunners);



        //        marketbooks.Add(marketbook);


        //        return marketbooks;
        //    }
        //    catch (System.Exception ex)
        //    {
        //        APIConfig.LogError(ex);
        //        var marketbooks = new List<ExternalAPI.TO.MarketBook>();
        //        return marketbooks;
        //    }
        //}
        public void CheckforMatchCompleted()
        {
            try
            {
                if (APIConfig.isUpdatingBetstocompelete == false)
                {
                    List<SP_UserBets_GetUnCompleteMarketIDandUserID_Result> lstUnCompleteMarketbooks = dbEntities.SP_UserBets_GetUnCompleteMarketIDandUserID().ToList<SP_UserBets_GetUnCompleteMarketIDandUserID_Result>();
                    if (lstUnCompleteMarketbooks.Count > 0)
                    {
                        foreach (var item in lstUnCompleteMarketbooks)
                        {
                            BettingService objBettingService = new BettingService();
                            var marketbookstatus = objBettingService.GetMarketDatabyIDResultsOnly(item.MarketBookID);

                            if (marketbookstatus.Count > 0)
                            {
                                if (marketbookstatus[0].Status == ExternalAPI.TO.MarketStatus.CLOSED)
                                {
                                    List<SP_Userbets_GetUserIDsbyMarketID_Result> lstUserIDs = dbEntities.SP_Userbets_GetUserIDsbyMarketID(item.MarketBookID).ToList<SP_Userbets_GetUserIDsbyMarketID_Result>();
                                    if (lstUserIDs.Count > 0)
                                    {
                                        if (marketbookstatus[0].Runners.Count == 1)
                                        {
                                            if (GetFancyResultPostSetting() == true)
                                            {
                                                string WinnerScoreforFacny = "0";

                                                var marketclosedtime = dbEntities.SP_UserMarket_GetMarketClosedTime(marketbookstatus[0].MarketId).FirstOrDefault();
                                                if (marketclosedtime == null)
                                                {
                                                    dbEntities.SP_UserMarket_UpdateMarketClosedTime(marketbookstatus[0].MarketId, DateTime.Now.AddMinutes(2));
                                                }
                                                else
                                                {
                                                    if (DateTime.Now > marketclosedtime)
                                                    {
                                                        var eventdetails = dbEntities.SP_UserMarket_GetEventDetailsbyMarketID(marketbookstatus[0].MarketId).FirstOrDefault();
                                                        // var matchscores = dbEntities.SP_BallbyBallSummary_GetResultsbyEventID(eventdetails.associateeventID, eventdetails.EventOpenDate).ToList();
                                                        var inningsnameandover = eventdetails.MarketCatalogueName.Split(new string[] { " Innings " }, StringSplitOptions.None);
                                                        string innings = Regex.Match(inningsnameandover[0], @"\d+").Value;
                                                        string over = Regex.Match(inningsnameandover[1], @"\d+").Value;
                                                        WinnerScoreforFacny = GetScoreFormLocalDatabase(eventdetails.associateeventID, eventdetails.EventOpenDate.Value, eventdetails.MarketCatalogueName).ToString();
                                                        //if (GetFancyResultsFrom() == "Local")
                                                        //{
                                                        //    WinnerScoreforFacny = GetScoreFormLocalDatabase(eventdetails.associateeventID, eventdetails.EventOpenDate.Value, eventdetails.MarketCatalogueName).ToString();
                                                        //}
                                                        //else
                                                        //{
                                                        //    string matchkeyCricketAPI = dbEntities.SP_UserMarket_GetCricketAPIMatchKey(marketbookstatus[0].MarketId).FirstOrDefault();
                                                        //    if (matchkeyCricketAPI != "")
                                                        //    {
                                                        //        WinnerScoreforFacny = GetScoreFromCricketAPI(matchkeyCricketAPI, innings, over).ToString();

                                                        //    }
                                                        //}

                                                        if (Convert.ToInt32(WinnerScoreforFacny) > 0)
                                                        {
                                                            APIConfig.isUpdatingBetstocompelete = true;
                                                            foreach (var useritem in lstUserIDs)
                                                            {
                                                                InsertUserAccounts(marketbookstatus, Convert.ToInt32(useritem.userID), ConfigurationManager.AppSettings["PasswordForValidate"], WinnerScoreforFacny);
                                                            }
                                                            APIConfig.isUpdatingBetstocompelete = false;
                                                        }
                                                    }

                                                }

                                            }
                                            else
                                            {
                                                try
                                                {
                                                    UpdateMarketStatusbyMarketBookID(marketbookstatus[0].MarketId, "Closed");
                                                }
                                                catch (System.Exception ex)
                                                {
                                                    APIConfig.LogError(ex);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            dbEntities.SP_UserMarket_UpdateMarketClosedTime(marketbookstatus[0].MarketId, DateTime.Now);
                                            APIConfig.isUpdatingBetstocompelete = true;
                                            foreach (var useritem in lstUserIDs)
                                            {
                                                InsertUserAccounts(marketbookstatus, Convert.ToInt32(useritem.userID), ConfigurationManager.AppSettings["PasswordForValidate"], "0");
                                            }
                                            APIConfig.isUpdatingBetstocompelete = false;
                                        }

                                    }

                                    //List<SP_UserBets_GetUnCompleteMarketIDbyMarketID_Result> lstMarkets = dbEntities.SP_UserBets_GetUnCompleteMarketIDbyMarketID(item.MarketBookID).ToList<SP_UserBets_GetUnCompleteMarketIDbyMarketID_Result>();
                                    //if (lstMarkets.Count > 0)
                                    //{

                                    //}
                                }
                            }
                        }


                    }
                }

            }
            catch (System.Exception ex)
            {
                APIConfig.isUpdatingBetstocompelete = false;
                APIConfig.WriteErrorToDB(ex.Message);
            }
        }

        public void CheckforMatchCompletedFancy(string MarketBookID, int ScoreforThisOver)
        {
            try
            {
                if (1 == 1)
                {
                    List<SP_UserBets_GetUnCompleteMarketIDandUserID_Result> lstUnCompleteMarketbooks = dbEntities.SP_UserBets_GetUnCompleteMarketIDandUserID().ToList<SP_UserBets_GetUnCompleteMarketIDandUserID_Result>();
                    if (lstUnCompleteMarketbooks.Count > 0)
                    {
                        lstUnCompleteMarketbooks = lstUnCompleteMarketbooks.Where(item => item.MarketBookID == MarketBookID).ToList();
                        foreach (var item in lstUnCompleteMarketbooks)
                        {
                            BettingService objBettingService = new BettingService();
                            var marketbookstatus = objBettingService.GetMarketDatabyIDResultsOnly(item.MarketBookID);
                            if (marketbookstatus.Count > 0)
                            {
                                if (marketbookstatus[0].Runners.Count == 1)
                                {
                                    List<SP_Userbets_GetUserIDsbyMarketID_Result> lstUserIDs = dbEntities.SP_Userbets_GetUserIDsbyMarketID(item.MarketBookID).ToList<SP_Userbets_GetUserIDsbyMarketID_Result>();
                                    if (lstUserIDs.Count > 0)
                                    {                               
                                        foreach (var useritem in lstUserIDs)
                                        {
                                            InsertUserAccountsFancy(marketbookstatus, Convert.ToInt32(useritem.userID), ConfigurationManager.AppSettings["PasswordForValidate"], ScoreforThisOver);
                                        }
                                       
                                    }
                                  
                                }
                            }
                        }


                    }
                }

            }
            catch (System.Exception ex)
            {
                // APIConfig.isUpdatingBetstocompelete = false;
                APIConfig.WriteErrorToDB(ex.Message);
            }
        }

        public void CheckforMatchCompletedFancyIN(string MarketBookID,string Marketname ,int ScoreforThisOver)
        {
            try
            {
                if (1 == 1)
                {
                    List<SP_UserBets_GetUnCompleteMarketIDandUserID_Result> lstUnCompleteMarketbooks = dbEntities.SP_UserBets_GetUnCompleteMarketIDandUserID().ToList<SP_UserBets_GetUnCompleteMarketIDandUserID_Result>();
                    if (lstUnCompleteMarketbooks.Count > 0)
                    {
                        lstUnCompleteMarketbooks = lstUnCompleteMarketbooks.Where(item => item.MarketBookID == MarketBookID).ToList();
                        foreach (var item in lstUnCompleteMarketbooks)
                        {
                           
                            List<SP_Userbets_GetUserIDsbyMarketID_Result> lstUserIDs = dbEntities.SP_Userbets_GetUserIDsbyMarketID(item.MarketBookID).ToList<SP_Userbets_GetUserIDsbyMarketID_Result>();
                            if (lstUserIDs.Count > 0)
                            {
                                //  APIConfig.isUpdatingBetstocompelete = true;
                                foreach (var useritem in lstUserIDs)
                                {
                                    InsertUserAccountsFancyIN(MarketBookID, Marketname, Convert.ToInt32(useritem.userID), ConfigurationManager.AppSettings["PasswordForValidate"], ScoreforThisOver);
                                }
                                //  APIConfig.isUpdatingBetstocompelete = false;
                            }
                           
                        }


                    }
                }

            }
            catch (System.Exception ex)
            {
                // APIConfig.isUpdatingBetstocompelete = false;
                APIConfig.WriteErrorToDB(ex.Message);
            }
        }

        public void CheckforMatchCompletedFancyKJ(string MarketBookID,int selectionID, int ScoreforThisOver)
        {
            try
            {
                if (1 == 1)
                {
                    List<SP_UserBets_GetUnCompleteMarketIDandUserID_Result> lstUnCompleteMarketbooks = dbEntities.SP_UserBets_GetUnCompleteMarketIDandUserID().ToList<SP_UserBets_GetUnCompleteMarketIDandUserID_Result>();
                    if (lstUnCompleteMarketbooks.Count > 0)
                    {
                        lstUnCompleteMarketbooks = lstUnCompleteMarketbooks.Where(item => item.MarketBookID == MarketBookID).ToList();
                        foreach (var item in lstUnCompleteMarketbooks)
                        {

                            List<SP_Userbets_GetUserIDsbyMarketID_Result> lstUserIDs = dbEntities.SP_Userbets_GetUserIDsbyMarketID(item.MarketBookID).ToList<SP_Userbets_GetUserIDsbyMarketID_Result>();
                            if (lstUserIDs.Count > 0)
                            {
                                //  APIConfig.isUpdatingBetstocompelete = true;
                                foreach (var useritem in lstUserIDs)
                                {
                                    InsertUserAccountsFancyKJ(MarketBookID, selectionID, Convert.ToInt32(useritem.userID), ConfigurationManager.AppSettings["PasswordForValidate"], ScoreforThisOver);
                                }
                                //  APIConfig.isUpdatingBetstocompelete = false;
                            }

                        }


                    }
                }

            }
            catch (System.Exception ex)
            {
                // APIConfig.isUpdatingBetstocompelete = false;
                APIConfig.WriteErrorToDB(ex.Message);
            }
        }

        public void CheckforMatchCompletedSmallFig(string MarketBookID, int selectionID, int ScoreforThisOver)
        {
            try
            {
                if (1 == 1)
                {
                    List<SP_UserBets_GetUnCompleteMarketIDandUserID_Result> lstUnCompleteMarketbooks = dbEntities.SP_UserBets_GetUnCompleteMarketIDandUserID().ToList<SP_UserBets_GetUnCompleteMarketIDandUserID_Result>();
                    if (lstUnCompleteMarketbooks.Count > 0)
                    {
                        lstUnCompleteMarketbooks = lstUnCompleteMarketbooks.Where(item => item.MarketBookID == MarketBookID).ToList();
                        foreach (var item in lstUnCompleteMarketbooks)
                        {
                            List<SP_Userbets_GetUserIDsbyMarketID_Result> lstUserIDs = dbEntities.SP_Userbets_GetUserIDsbyMarketID(item.MarketBookID).ToList<SP_Userbets_GetUserIDsbyMarketID_Result>();
                            if (lstUserIDs.Count > 0)
                            {
                                //  APIConfig.isUpdatingBetstocompelete = true;
                                foreach (var useritem in lstUserIDs)
                                {
                                    InsertUserAccountsFancySmallFig(MarketBookID, selectionID, Convert.ToInt32(useritem.userID), ConfigurationManager.AppSettings["PasswordForValidate"], ScoreforThisOver);
                                }
                                //  APIConfig.isUpdatingBetstocompelete = false;
                            }
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                // APIConfig.isUpdatingBetstocompelete = false;
                APIConfig.WriteErrorToDB(ex.Message);
            }
        }

        public void CheckforMatchCompletedFancyFig(string MarketBookID, int selectionID, int ScoreforThisOver)
        {
            try
            {
                if (1 == 1)
                {
                    List<SP_UserBets_GetUnCompleteMarketIDandUserID_Result> lstUnCompleteMarketbooks = dbEntities.SP_UserBets_GetUnCompleteMarketIDandUserID().ToList<SP_UserBets_GetUnCompleteMarketIDandUserID_Result>();
                    if (lstUnCompleteMarketbooks.Count > 0)
                    {
                        lstUnCompleteMarketbooks = lstUnCompleteMarketbooks.Where(item => item.MarketBookID == MarketBookID).ToList();
                        foreach (var item in lstUnCompleteMarketbooks)
                        {

                            List<SP_Userbets_GetUserIDsbyMarketID_Result> lstUserIDs = dbEntities.SP_Userbets_GetUserIDsbyMarketID(item.MarketBookID).ToList<SP_Userbets_GetUserIDsbyMarketID_Result>();
                            if (lstUserIDs.Count > 0)
                            {
                                //  APIConfig.isUpdatingBetstocompelete = true;
                                foreach (var useritem in lstUserIDs)
                                {
                                    InsertUserAccountsFancyFig(MarketBookID, selectionID, Convert.ToInt32(useritem.userID), ConfigurationManager.AppSettings["PasswordForValidate"], ScoreforThisOver);
                                }
                                //  APIConfig.isUpdatingBetstocompelete = false;
                            }

                        }


                    }
                }

            }
            catch (System.Exception ex)
            {
                // APIConfig.isUpdatingBetstocompelete = false;
                APIConfig.WriteErrorToDB(ex.Message);
            }
        }

        public List<SP_MarketCatalogueSelections_Get_Result> GetSelectionNamesbyMarketID(string MarketID)
        {
            List<SP_MarketCatalogueSelections_Get_Result> lstSelectionResults = dbEntities.SP_MarketCatalogueSelections_Get(MarketID).ToList<SP_MarketCatalogueSelections_Get_Result>();
            return lstSelectionResults;
        }
       
        AccountsService objAccountsService = new AccountsService();
        APIConfigService objAPIConfigService = new APIConfigService();
        public string GetResultsFromBetfair(string SportsID, string MarketbookID)
        {
            string url = "http://rss.betfair.com/RSS.aspx?format=rss&sportID=" + SportsID;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Parse;
            XmlReader reader = XmlReader.Create(url, settings);

            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            foreach (SyndicationItem item in feed.Items)
            {
                string[] marketidandsports = item.Links[0].Uri.ToString().Split(new string[] { "marketID=" }, StringSplitOptions.None);

                if (marketidandsports[1] == MarketbookID)
                {
                    return item.Summary.Text.ToString().Trim();
                }

            }
            return "";
        }
        public void CopyFiletonewfile()
        {

            // System.IO.File.Copy(oldfilepath, filepath, true);

        }

        public bool GetFancyResultPostSetting()
        {
            return dbEntities.SP_ResultPostSettings_GetFancyPostResultSetting().FirstOrDefault().Value;
        }
        public void UpdateFancyResultPostSetting(bool fancyresultpost)
        {
            dbEntities.SP_ResultPostSettings_UpdateFancyPostResultSetting(fancyresultpost);
        }
        public int GetScoreFormLocalDatabase(string EventID, DateTime eventOpendate, string MarketCatalogueName)
        {
            try
            {


                var matchscores = dbEntities.SP_BallbyBallSummary_GetResultsbyEventID(EventID, eventOpendate).ToList();
                var inningsnameandover = MarketCatalogueName.Split(new string[] { " Innings " }, StringSplitOptions.None);
                string innings = Regex.Match(inningsnameandover[0], @"\d+").Value;
                string over = Regex.Match(inningsnameandover[1], @"\d+").Value;
                var matchresultforthisover = matchscores.Where(item1 => item1.Innings == Convert.ToInt32(innings) && item1.Overs == Convert.ToDouble(over)).FirstOrDefault();
                if (matchresultforthisover == null)
                {
                    matchresultforthisover = matchscores.Where(item1 => item1.Innings == Convert.ToInt32(innings)).LastOrDefault();
                }
                return Convert.ToInt32(matchresultforthisover.Scores);
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }
        public bool InsertUserAccounts(List<ExternalAPI.TO.MarketBook> marketbookstatus, int userID, string Password, string winnnerscoreforfancy)
        {
            if (ValidatePassword(Password) == false)
            {
                return false;
            }
            try
            {
                List<SP_UserBets_GetDatabyUserIDandMArketID_Result> lstUserBets = dbEntities.SP_UserBets_GetDatabyUserIDandMArketID(userID, marketbookstatus[0].MarketId).ToList<SP_UserBets_GetDatabyUserIDandMArketID_Result>();
                decimal WinnerTotal = 0;
                decimal WinnerDebit = 0;
                decimal WinnerCredit = 0;
                decimal LosserTotal = 0;
                decimal LosserDebit = 0;
                decimal LosserCredit = 0;
                decimal TotalAmount = 0;
                string Winnername = "";
                foreach (var item in marketbookstatus)
                {
                    if (item.Status == ExternalAPI.TO.MarketStatus.CLOSED)
                    {

                        if (item.Runners.Count == 1)
                        {
                            if (GetFancyResultPostSetting() == true)
                            {
                                try
                                {

                                    var lstBackBets = lstUserBets.Where(item1 => item1.BetType == "back" && item1.isMatched == true).ToList();
                                    var lstLayBets = lstUserBets.Where(item1 => item1.BetType == "lay" && item1.isMatched == true).ToList();

                                    Winnername = winnnerscoreforfancy;
                                    int winnerscore = Convert.ToInt32(winnnerscoreforfancy);
                                    foreach (var backbetitem in lstBackBets)
                                    {

                                        if (Convert.ToInt32(backbetitem.UserOdd) <= winnerscore)
                                        {
                                            WinnerDebit += Convert.ToInt32(backbetitem.Amount);
                                        }
                                        else
                                        {
                                            WinnerCredit += Convert.ToInt32(backbetitem.Amount);
                                        }
                                    }
                                    foreach (var backbetitem in lstLayBets)
                                    {

                                        if (Convert.ToInt32(backbetitem.UserOdd) > winnerscore)
                                        {
                                            WinnerDebit += Convert.ToInt32(backbetitem.Amount);
                                        }
                                        else
                                        {
                                            WinnerCredit += Convert.ToInt32(backbetitem.Amount);
                                        }
                                    }
                                    dbEntities.SP_UserBets_UpdateStatus(item.MarketId, userID);
                                    dbEntities.SP_UserMarket_SetMarketCatalogueIDClosed(item.MarketId);
                                    dbEntities.SP_UserMarket_UpdateMarketForAllowedBetting(userID, item.MarketId, false);
                                    try
                                    {
                                        UpdateMarketStatusbyMarketBookID(item.MarketId, "Closed");
                                    }
                                    catch (System.Exception ex)
                                    {
                                        APIConfig.LogError(ex);
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    APIConfig.LogError(ex);
                                    return false;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            dbEntities.SP_UserBets_UpdateStatus(item.MarketId, userID);
                            dbEntities.SP_UserMarket_SetMarketCatalogueIDClosed(item.MarketId);
                            try
                            {
                                UpdateMarketStatusbyMarketBookID(item.MarketId, "Closed");
                            }
                            catch (System.Exception ex)
                            {
                                APIConfig.LogError(ex);
                            }


                            decimal ActualWinners = item.Runners.Where(item1 => item1.Status == ExternalAPI.TO.RunnerStatus.WINNER).Count();
                            var lstWinnersIDs = item.Runners.Where(item1 => item1.Status == ExternalAPI.TO.RunnerStatus.WINNER).Select(item1 => new { item1.SelectionId }).ToList();
                            decimal noofwinners = item.NumberOfWinners;
                            int winnerbetscount = 0;
                            foreach (var winnerid in lstWinnersIDs)
                            {
                                winnerbetscount += lstUserBets.Where(a => a.SelectionID == winnerid.SelectionId).Select(item1 => new { item1.SelectionID }).Distinct().Count();
                            }
                            foreach (var runner in item.Runners)
                            {
                                if (runner.Status == ExternalAPI.TO.RunnerStatus.WINNER)
                                {
                                    Winnername = runner.RunnerName;
                                    List<SP_UserBets_GetDatabyUserIDandMArketID_Result> lstWinnerBets = lstUserBets.Where(a => a.SelectionID == runner.SelectionId && a.isMatched == true).ToList();

                                    foreach (var winneritem in lstWinnerBets)
                                    {
                                        decimal newamountdeadhit = 0;
                                        decimal newuserodddeadhit = 0;
                                        decimal D16 = 0;
                                        var totamount = (Convert.ToDecimal(winneritem.Amount) * Convert.ToDecimal(winneritem.UserOdd)) - Convert.ToDecimal(winneritem.Amount);
                                        if (winneritem.BetType == "back")
                                        {
                                            if (ActualWinners > noofwinners && lstWinnersIDs.Count != winnerbetscount)
                                            {

                                                newamountdeadhit = (Convert.ToDecimal(winneritem.Amount) * noofwinners) / ActualWinners;
                                                D16 = Convert.ToDecimal(winneritem.Amount) - newamountdeadhit;
                                                newamountdeadhit = (newamountdeadhit * (Convert.ToDecimal(winneritem.UserOdd) - 1)) - D16;
                                                if (newamountdeadhit < 0)
                                                {
                                                    newamountdeadhit = -1 * newamountdeadhit;
                                                }
                                                WinnerDebit += newamountdeadhit;
                                            }
                                            else
                                            {
                                                WinnerDebit += totamount;
                                            }


                                        }
                                        else
                                        {
                                            if (ActualWinners > noofwinners && lstWinnersIDs.Count != winnerbetscount)
                                            {
                                                newamountdeadhit = (1 - (noofwinners / ActualWinners)) * Convert.ToDecimal(winneritem.Amount);
                                                D16 = Convert.ToDecimal(winneritem.Amount) - newamountdeadhit;
                                                newamountdeadhit = newamountdeadhit - D16 * (Convert.ToDecimal(winneritem.UserOdd) - 1);
                                                if (newamountdeadhit < 0)
                                                {
                                                    newamountdeadhit = -1 * newamountdeadhit;
                                                }
                                                WinnerCredit += newamountdeadhit;
                                            }
                                            else
                                            {
                                                WinnerCredit += totamount;
                                            }
                                        }

                                    }

                                }
                                if (runner.Status == ExternalAPI.TO.RunnerStatus.LOSER)
                                {
                                    List<SP_UserBets_GetDatabyUserIDandMArketID_Result> lstLosserBets = lstUserBets.Where(a => a.SelectionID == runner.SelectionId && a.isMatched == true).ToList();
                                    foreach (var losseritem in lstLosserBets)
                                    {
                                        decimal newamountdeadhit = 0;
                                        decimal newuserodddeadhit = 0;
                                        decimal D16 = 0;
                                        if (losseritem.BetType == "back")
                                        {
                                            if (ActualWinners > noofwinners && 1 == 2)
                                            {
                                                newamountdeadhit = (Convert.ToDecimal(losseritem.Amount) * noofwinners) / ActualWinners;
                                                D16 = Convert.ToDecimal(losseritem.Amount) - newamountdeadhit;
                                                newamountdeadhit = (newamountdeadhit * (Convert.ToDecimal(losseritem.UserOdd) - 1)) - D16;
                                                WinnerCredit += newamountdeadhit;
                                            }
                                            else
                                            {
                                                WinnerCredit += Convert.ToDecimal(losseritem.Amount);
                                            }
                                            // var totamount = (Convert.ToDecimal(losseritem.Amount) * Convert.ToDecimal(losseritem.UserOdd)) - Convert.ToDecimal(losseritem.Amount);
                                            //  LosserCredit += Convert.ToDecimal(losseritem.Amount);

                                        }
                                        else
                                        {
                                            if (ActualWinners > noofwinners && 1 == 2)
                                            {
                                                newamountdeadhit = (1 - (noofwinners / ActualWinners)) * Convert.ToDecimal(losseritem.Amount);
                                                D16 = Convert.ToDecimal(losseritem.Amount) - newamountdeadhit;
                                                newamountdeadhit = newamountdeadhit - D16 * (Convert.ToDecimal(losseritem.UserOdd) - 1);
                                                WinnerDebit += newamountdeadhit;
                                            }
                                            else
                                            {
                                                WinnerDebit += Convert.ToDecimal(losseritem.Amount);
                                            }

                                            // LosserDebit += Convert.ToDecimal(losseritem.Amount);
                                        }

                                    }
                                    LosserTotal += LosserDebit - LosserCredit;
                                }
                            }
                        }
                        WinnerTotal = WinnerDebit - WinnerCredit;

                        if (LosserTotal < 0)
                        {
                            LosserTotal = -1 * (LosserTotal);
                        }
                        //  TotalAmount = WinnerTotal - LosserTotal;
                        TotalAmount = Math.Round(WinnerTotal, 2);
                        int samiadminID = 0;
                        int AdminID = 0;
                        int CreatedbyID = GetCreatedbyID1(userID);
                        //bool isAgentcom = GetIsComAllowbyUserID(CreatedbyID);                     
                        int superID = GetCreatedbyID1(CreatedbyID);
                        //bool isSupercom = GetIsComAllowbyUserID(superID);
                       // bool issamiadmincom = false;
                       
                        if (superID == 1)
                        {
                            AdminID = 1;
                            //AdminID = samiadminID;
                        }
                        else
                        {
                            if (superID != 1)
                            {
                                samiadminID = GetCreatedbyID1(superID);
                               // issamiadmincom = GetIsComAllowbyUserID(samiadminID);
                                if (samiadminID==1)
                                {
                                    AdminID = 1;
                                }
                                else
                                {
                                    AdminID = GetCreatedbyID1(samiadminID);
                                }
                                //int samiadminID = GetCreatedbyID1(superID);
                            }
                        }
                        SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionAccountIDandBookAccountID = dbEntities.SP_Users_GetCommissionAccountIDandBookAccountID(AdminID).FirstOrDefault();                     
                        var usernameandtype = dbEntities.SP_Users_GetUSernamebyUserID(userID.ToString()).FirstOrDefault();
                        string username = Crypto.Decrypt(usernameandtype.Username);
                        string EventTypename = dbEntities.SP_UserMarket_GetEventTypeNamebyMarketID(item.MarketId).FirstOrDefault().ToString();
                        var eventdetails = dbEntities.SP_UserMarket_GetEventDetailsbyMarketID(item.MarketId).FirstOrDefault();
                        if (item.Runners.Count == 1)
                        {
                            EventTypename = "Fancy";
                        }
                        decimal Comissionamount = 0;
                        string commisionratestr = "";
                        if (item.Runners.Count == 1)
                        {
                            Comissionamount = Math.Round(((Convert.ToDecimal(GetCommissionRatebyUserIDFancy(userID)) / 100) * TotalAmount), 2);
                            commisionratestr = GetCommissionRatebyUserIDFancy(userID).ToString();
                        }
                        else
                        {
                            Comissionamount = Math.Round(((Convert.ToDecimal(GetCommissionRatebyUserID(userID)) / 100) * TotalAmount), 2);
                            commisionratestr = GetCommissionRatebyUserID(userID).ToString();
                        }
                        bool TransferAgentCommision = GetTransferAgnetCommision(CreatedbyID);

                        if (TotalAmount >= 0)
                        {
                            // int agentpercent = Convert.ToInt32(Crypto.Decrypt(GetAgentRate(userID)));
                            int agentpercent = Convert.ToInt32(lstUserBets[0].AgentRate);
                            int superpercent = Convert.ToInt32(lstUserBets[0].SuperAgentRateB);
                            int samiadminpercent = Convert.ToInt32(lstUserBets[0].SamiAdminRate);

                            int transferadminpercentage = Convert.ToInt32(lstUserBets[0].TransferAdminPercentage);
                            decimal ActualAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(GetCommissionRatebyUserID(userID)) / 100) * TotalAmount), 2);

                            decimal AgentAmount = 0;
                            decimal AdminAmount = 0;
                            decimal superpercentfinal = 0;
                            decimal samiadminpercentfinal = 0;
                            decimal SuperAmount1 = 0;
                            decimal SamiadminAmount1 = 0;
                            decimal ComissionAmountAgent = 0;
                            decimal ComissionAmountSuper = 0;
                            decimal ComissionAmountSamiadin = 0;


                            if ( CreatedbyID !=73)
                            {
                                superpercentfinal = superpercent - agentpercent;
                                if(superpercentfinal > 0)
                                {
                                    SuperAmount1 = Math.Round((Convert.ToDecimal(superpercentfinal) / 100) * TotalAmount, 2);
                                }
                               else
                                {
                                    SuperAmount1 = 0;
                                }
                                if (samiadminID != 1)
                                {
                                    samiadminpercentfinal = samiadminpercent - (superpercentfinal + agentpercent);
                                    if (samiadminpercentfinal > 0)
                                    {
                                        SamiadminAmount1 = Math.Round((Convert.ToDecimal(samiadminpercentfinal) / 100) * TotalAmount, 2);
                                    }
                                    else
                                    {
                                        SamiadminAmount1 = 0;
                                    }
                                }
                                AgentAmount = Math.Round((Convert.ToDecimal(agentpercent) / 100) * TotalAmount, 2);

                                if (agentpercent == 100)
                                {
                                    AdminAmount = Math.Round(TotalAmount - (AgentAmount+ SuperAmount1 + SamiadminAmount1), 2);

                                }
                                else
                                {
                                    AdminAmount = Math.Round(TotalAmount - (AgentAmount+ SuperAmount1+ SamiadminAmount1 + Comissionamount), 2);
                                }

                            }
                            else
                            {
                                AdminAmount = Math.Round(TotalAmount - Comissionamount, 2);
                            }

                            var results = objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname, TotalAmount.ToString(), "0.00", userID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(TotalAmount, userID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            if (usernameandtype.UserTypeID != 4)
                            {

                                objAccountsService.AddtoUsersAccounts("Commission", "0.00", Comissionamount.ToString(), userID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1 * (Comissionamount), userID, AdminID, DateTime.Now, 0, false, "", false, Password);


                                if (TransferAgentCommision == true && agentpercent > 0)
                                {
                                    if (superpercentfinal > 0)
                                    {
                                        ComissionAmountSuper = Math.Round((Convert.ToDecimal(superpercentfinal) / 100) * Comissionamount, 2);
                                        objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountSuper.ToString(), "0.00", Convert.ToInt32(superID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);

                                        //if (isSupercom == false)
                                        //{
                                            AddCredittoUser(ComissionAmountSuper, Convert.ToInt32(superID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                        //}
                                    }
                                    else
                                    {
                                        ComissionAmountSuper = 0;
                                    }

                                    if (samiadminpercentfinal > 0)
                                    {
                                        ComissionAmountSamiadin = Math.Round((Convert.ToDecimal(samiadminpercentfinal) / 100) * Comissionamount, 2);
                                        objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountSamiadin.ToString(), "0.00", Convert.ToInt32(samiadminID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);

                                        //if ( issamiadmincom== false)
                                        //{
                                            AddCredittoUser(ComissionAmountSamiadin, Convert.ToInt32(superID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                        //}
                                    }
                                    else
                                    {
                                        ComissionAmountSamiadin = 0;
                                    }

                                    ComissionAmountAgent = Math.Round((Convert.ToDecimal(agentpercent) / 100) * Comissionamount, 2);

                                    decimal Comissionamounadmin = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper + ComissionAmountSamiadin);
                                    objAccountsService.AddtoComissionAccounts(lstUserBets[0].Marketbookname + " (" + username + ")", Comissionamounadmin.ToString(), Convert.ToInt64(results), DateTime.Now);
                                    objAccountsService.AddtoUsersAccounts("Commission", Comissionamounadmin.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                    AddCredittoUser(Comissionamounadmin, Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                    //i am here
                                    objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountAgent.ToString(), "0.00", Convert.ToInt32(CreatedbyID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                    //if (isAgentcom == false)
                                    //{
                                        AddCredittoUser(ComissionAmountAgent, Convert.ToInt32(CreatedbyID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                    //}
                                }
                                else
                                {
                                    decimal Comissionamounadmin = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper + ComissionAmountSamiadin);
                                    objAccountsService.AddtoComissionAccounts(lstUserBets[0].Marketbookname + " (" + username + ")", Comissionamounadmin.ToString(), Convert.ToInt64(results), DateTime.Now);
                                    objAccountsService.AddtoUsersAccounts("Commission", Comissionamounadmin.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                    AddCredittoUser(Comissionamounadmin, Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                }
                            }

                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + agentpercent.ToString() + "%) )", "0.00", AgentAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);

                            if (superID != 1)
                            {
                                //admin hisab
                                //AdminAmount = AdminAmount + Comissionamount;
                                //decimal Comissionamountadmin = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper + ComissionAmountSamiadin);
                                //AdminAmount = AdminAmount - Comissionamountadmin;
                                ////agent hisab
                                //decimal ComissionAmountAgent1  = Comissionamount - (ComissionAmountSuper + Comissionamountadmin + ComissionAmountSamiadin);
                                //AgentAmount = AgentAmount - ComissionAmountAgent1;
                                ////samiadminhisab
                                //decimal ComissionAmountsamiadmin = Comissionamount - (ComissionAmountSuper + Comissionamountadmin + ComissionAmountAgent1);
                                //SamiadminAmount1 = SamiadminAmount1 - ComissionAmountsamiadmin;
                                //
                                // decimal ComissionAmountSuper1 = Comissionamount - (ComissionAmountAgent + Comissionamountadmin + ComissionAmountSuper);
                                //SuperAmount1 = SuperAmount1 - ComissionAmountSuper1;
                                //decimal AdminAmountforsuper1 = AdminAmount + AgentAmount + SamiadminAmount1;
                                SuperAmount1= TotalAmount * (superpercentfinal / 100);
                               // SuperAmount1 = SuperAmount1 - ComissionAmountSuper;
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + superpercentfinal.ToString() + "%) )", "0.00", SuperAmount1.ToString(), superID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1*(SuperAmount1), superID, AdminID, DateTime.Now, 0, false, "", false, Password);
                                //AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + agentpercent.ToString() + "%) )", (AdminAmount).ToString(),"", superID, item.MarketId, DateTime.Now, Password);

                            }
                            if (samiadminpercentfinal>0)
                            {                               
                                SamiadminAmount1 = TotalAmount * (samiadminpercentfinal / 100);                              
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + samiadminpercentfinal.ToString() + "%) )", "0.00", SamiadminAmount1.ToString(), samiadminID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1 * (SamiadminAmount1), samiadminID, AdminID, DateTime.Now, 0, false, "", false, Password);
                                //AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + agentpercent.ToString() + "%) )", (AdminAmount).ToString(),"", superID, item.MarketId, DateTime.Now, Password);

                            }
                            if (CreatedbyID == 73)
                            {
                                AddCredittoUser(-1 * (AgentAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                            else
                            {
                                //admin hisab
                                AdminAmount = AdminAmount + Comissionamount;
                                decimal Comissionamountadmin = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper + ComissionAmountSamiadin);
                                AdminAmount = AdminAmount - Comissionamountadmin;
                                //Super hisab         
                                decimal ComissionAmountSuper1 = Comissionamount - (ComissionAmountAgent + Comissionamountadmin + ComissionAmountSamiadin);
                                SuperAmount1 = SuperAmount1 - ComissionAmountSuper1;
                                //samiadmin hisab
                                decimal ComissionAmountsamiadmin = Comissionamount - (ComissionAmountSuper + Comissionamountadmin + ComissionAmountAgent);
                                SamiadminAmount1 = SamiadminAmount1 - ComissionAmountsamiadmin;

                                
                                decimal amount = AdminAmount + SuperAmount1+ SamiadminAmount1;
                                AddCredittoUser( (amount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            }

                            if (lstUserBets[0].TransferAdmin == false)
                            {
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1 * (AdminAmount), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                            else
                            {
                                decimal transferadminamount1 = 0;
                                decimal AdminAmount1 = 0;
                                transferadminamount1 = Math.Round((Convert.ToDecimal(transferadminpercentage) / 100) * TotalAmount);
                                AdminAmount1 = AdminAmount - transferadminamount1;


                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (transferadminpercentage).ToString() + "%) )", "0.00", transferadminamount1.ToString(), lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount1.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);

                                AddCredittoUser(-1 * (AdminAmount1), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                AddCredittoUser(-1 * (transferadminamount1), lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                            }

                          
                            var lstresultsrefferer = GetReferrerRatesbyUserID(userID);
                            foreach (var resultsrefferer in lstresultsrefferer)
                            {
                                decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser((RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), resultsrefferer.ReferrerID.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(resultsrefferer.ReferrerID.Value, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1 * (RefferAmount), resultsrefferer.ReferrerID.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                            }

                         
                        }
                        else
                        {
                            TotalAmount = -1 * TotalAmount;
                            int agentpercent = Convert.ToInt32(lstUserBets[0].AgentRate);
                            int superpercent = Convert.ToInt32(lstUserBets[0].SuperAgentRateB);
                            int samiadminpercent = Convert.ToInt32(lstUserBets[0].SamiAdminRate);
                            int transferadminpercentage = Convert.ToInt32(lstUserBets[0].TransferAdminPercentage);
                            // decimal ActualAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(objAPIConfigService.GetCommisionRate())) / 100) * TotalAmount), 2);
                            //decimal Comissionamount = Math.Round(((Convert.ToDecimal(Crypto.Decrypt(objAPIConfigService.GetCommisionRate())) / 100) * TotalAmount), 2);
                            //  decimal AgentAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(GetAgentRate(userID))) / 100) * TotalAmount), 2);
                            //decimal AdminAmount = Math.Round(TotalAmount - AgentAmount, 2);
                            decimal AgentAmount = 0;
                            decimal SuperAmount = 0;
                            decimal SamiadminAmount = 0;
                            decimal SuperAmount1 = 0;
                            decimal SamiadminAmount1 = 0;
                            decimal AdminAmount = 0;
                           
                            if ( CreatedbyID !=73)
                            {
                                SuperAmount = superpercent - agentpercent;
                                if (SuperAmount >= 0)
                                {
                                    SuperAmount1 = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * TotalAmount, 2);
                                }
                                else
                                {
                                    SuperAmount1 = 0;
                                }
                                if (samiadminID != 1)
                                {
                                    SamiadminAmount = samiadminpercent - (SuperAmount + agentpercent);
                                    if (SamiadminAmount > 0)
                                    {
                                        SamiadminAmount1 = Math.Round((Convert.ToDecimal(SamiadminAmount) / 100) * TotalAmount, 2);
                                    }
                                    else
                                    {
                                        SamiadminAmount1 = 0;
                                    }
                                }
                                //  AgentAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(GetAgentRate(userID))) / 100) * TotalAmount), 2);
                                AgentAmount = Math.Round((Convert.ToDecimal(agentpercent) / 100) * TotalAmount, 2);
                                AdminAmount = Math.Round(TotalAmount - (AgentAmount + SuperAmount1+ SamiadminAmount1), 2);
                            }
                            else
                            {
                                AdminAmount = Math.Round(TotalAmount, 2);
                            }

                            var results = objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname, "0.00", TotalAmount.ToString(), userID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (TotalAmount), userID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            //  objAccountsService.AddtoComissionAccounts("CommissionAmount", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + agentpercent.ToString() + "%) )", AgentAmount.ToString(), "0.00", CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            if (superID != 1)
                            {
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + SuperAmount.ToString() + "%) )", SuperAmount1.ToString(), "0.00", superID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1*(SuperAmount1), superID, AdminID, DateTime.Now, 0, false, "", false, Password);
                               // AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + agentpercent.ToString() + "%) )", "", (AdminAmount).ToString(), superID, item.MarketId, DateTime.Now, Password);
                            }
                            if (samiadminID != 1 && samiadminID !=0)
                            {
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + SamiadminAmount.ToString() + "%) )", SamiadminAmount1.ToString(), "0.00", samiadminID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1 * (SamiadminAmount1), samiadminID, AdminID, DateTime.Now, 0, false, "", false, Password);
                                // AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + agentpercent.ToString() + "%) )", "", (AdminAmount).ToString(), superID, item.MarketId, DateTime.Now, Password);
                            }


                            if (CreatedbyID == 73)
                            {
                                AddCredittoUser((AgentAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                            else
                            {
                                decimal amount = AdminAmount + SuperAmount1;
                                AddCredittoUser(-1*(amount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                            if (lstUserBets[0].TransferAdmin == false)
                            {                           
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(AdminAmount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                            else
                            {
                                decimal transferadminamount1 = 0;
                                decimal AdminAmount1 = 0;
                                transferadminamount1 = Math.Round((Convert.ToDecimal(transferadminpercentage) / 100) * TotalAmount);
                                AdminAmount1 = AdminAmount - transferadminamount1;

                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (transferadminpercentage).ToString() + "%) )", transferadminamount1.ToString(), "0.00", lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount1.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(AdminAmount1, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);

                                AddCredittoUser(transferadminamount1, lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);
                             }

                            //  var resultsrefferer = GetReferrerIDandReferrerRatebyUserID(userID);
                            var lstresultsrefferer = GetReferrerRatesbyUserID(userID);
                            foreach (var resultsrefferer in lstresultsrefferer)
                            {
                                decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1 * (RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", resultsrefferer.ReferrerID.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), commisionratestr, Convert.ToDecimal(GetCurrentBalancebyUser(resultsrefferer.ReferrerID.Value, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser((RefferAmount), resultsrefferer.ReferrerID.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                            }
                           
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

                return false;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                return false;
            }
        }
        public bool InsertUserAccountsFancy(List<ExternalAPI.TO.MarketBook> marketbookstatus, int userID, string Password, int ScoreforThisOver)
        {
            if (ValidatePassword(Password) == false)
            {
                return false;
            }
            try
            {
                List<SP_UserBets_GetDatabyUserIDandMArketID_Result> lstUserBets = dbEntities.SP_UserBets_GetDatabyUserIDandMArketID(userID, marketbookstatus[0].MarketId).ToList<SP_UserBets_GetDatabyUserIDandMArketID_Result>();
                decimal WinnerTotal = 0;
                decimal WinnerDebit = 0;
                decimal WinnerCredit = 0;
                decimal LosserTotal = 0;
                decimal LosserDebit = 0;
                decimal LosserCredit = 0;
                decimal TotalAmount = 0;
                string Winnername = "";
                foreach (var item in marketbookstatus)
                {
                    if (1 == 1)
                    {

                        if (item.Runners.Count == 1)
                        {
                            dbEntities.SP_UserBets_UpdateStatus(item.MarketId, userID);
                            dbEntities.SP_UserMarket_SetMarketCatalogueIDClosed(item.MarketId);
                            dbEntities.SP_UserMarket_UpdateMarketForAllowedBetting(userID, item.MarketId, false);
                            try
                            {
                                UpdateMarketStatusbyMarketBookID(item.MarketId, "Closed");
                            }
                            catch (System.Exception ex)
                            {
                                APIConfig.LogError(ex);
                            }
                           
                            var lstBackBets = lstUserBets.Where(item1 => item1.BetType == "back" && item1.isMatched == true).ToList();
                            var lstLayBets = lstUserBets.Where(item1 => item1.BetType == "lay" && item1.isMatched == true).ToList();

                            Winnername = ScoreforThisOver.ToString();
                            int winnerscore = ScoreforThisOver;
                            foreach (var backbetitem in lstBackBets)
                            {

                                if (Convert.ToInt32(backbetitem.UserOdd) <= winnerscore)
                                {
                                    WinnerDebit += Convert.ToInt32(backbetitem.Amount);
                                }
                                else
                                {
                                    WinnerCredit += Convert.ToInt32(backbetitem.Amount);
                                }
                            }
                            foreach (var backbetitem in lstLayBets)
                            {

                                if (Convert.ToInt32(backbetitem.UserOdd) > winnerscore)
                                {
                                    WinnerDebit += Convert.ToInt32(backbetitem.Amount);
                                }
                                else
                                {
                                    WinnerCredit += Convert.ToInt32(backbetitem.Amount);
                                }
                            }

                        }
                        else
                        {


                        }
                        WinnerTotal = WinnerDebit - WinnerCredit;
                        if (LosserTotal < 0)
                        {
                            LosserTotal = -1 * (LosserTotal);
                        }
                        //  TotalAmount = WinnerTotal - LosserTotal;
                        TotalAmount = WinnerTotal;

                        int AdminID = 0;
                        int CreatedbyID = GetCreatedbyID1(userID);
                        int superID = GetCreatedbyID1(CreatedbyID);
                        if (superID == 1)
                        {
                            AdminID = 1;
                        }
                        if (superID != 1)
                        {
                            AdminID = GetCreatedbyID1(superID);
                        }

                        SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionAccountIDandBookAccountID = dbEntities.SP_Users_GetCommissionAccountIDandBookAccountID(AdminID).FirstOrDefault();
                        //  SP_Users_GetCommissionAccountIDandBookAccountID_Result objAgentBookAccountID = dbEntities.SP_Users_GetCommissionAccountIDandBookAccountID(CreatedbyID).FirstOrDefault();
                        //  var user = (from users in dbEntities.Users where users.ID == userID select users).First();
                        //string username = Crypto.Decrypt(user.UserName).ToString();
                        var usernameandtype = dbEntities.SP_Users_GetUSernamebyUserID(userID.ToString()).FirstOrDefault();
                        string username = Crypto.Decrypt(usernameandtype.Username);
                        string EventTypename = dbEntities.SP_UserMarket_GetEventTypeNamebyMarketID(item.MarketId).FirstOrDefault().ToString();
                        var eventdetails = dbEntities.SP_UserMarket_GetEventDetailsbyMarketID(item.MarketId).FirstOrDefault();
                        if (item.Runners.Count == 1)
                        {
                            EventTypename = "Fancy";
                        }
                        if (TotalAmount >= 0)
                        {
                            // int agentpercent = Convert.ToInt32(Crypto.Decrypt(GetAgentRate(userID)));
                            int agentpercent = Convert.ToInt32(lstUserBets[0].AgentRate);
                            int superpercent = Convert.ToInt32(lstUserBets[0].SuperAgentRateB);
                            int samiadminpercent = Convert.ToInt32(lstUserBets[0].SamiAdminRate);
                            int transferadminpercentage = Convert.ToInt32(lstUserBets[0].TransferAdminPercentage);
                            decimal ActualAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(GetCommissionRatebyUserID(userID)) / 100) * TotalAmount), 2);
                            decimal Comissionamount = 0;
                            decimal ComissionAmountAgent = 0;
                            decimal ComissionAmountSuper = 0;
                            if (item.Runners.Count == 1)
                            {
                                Comissionamount = Math.Round(((Convert.ToDecimal(GetCommissionRatebyUserIDFancy(userID)) / 100) * TotalAmount), 2);
                            }
                            else
                            {
                                Comissionamount = Math.Round(((Convert.ToDecimal(GetCommissionRatebyUserID(userID)) / 100) * TotalAmount), 2);
                            }
                            decimal AgentAmount = 0;
                            decimal SuperAmount = 0;
                            decimal SuperAmount1 = 0;
                            decimal AdminAmount = 0;
                            bool TransferAgentCommision = GetTransferAgnetCommision(CreatedbyID);
                           
                            if ( CreatedbyID !=73)
                            {
                                SuperAmount = superpercent - agentpercent;
                                if (SuperAmount > 0)
                                {
                                    SuperAmount1 = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * TotalAmount, 2);
                                }
                                else
                                {
                                    SuperAmount1 = 0;
                                }
                                AgentAmount = Math.Round((Convert.ToDecimal(agentpercent) / 100) * TotalAmount, 2);
                                if (agentpercent == 100)
                                {
                                    AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1, 2);

                                }
                                else
                                {
                                    AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1 - Comissionamount, 2);
                                }

                            }
                            else
                            {
                                AdminAmount = Math.Round(TotalAmount - Comissionamount, 2);
                            }

                            //GetCommissionRatebyUserIDFancy(userID).ToString()
                            var results = objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname, TotalAmount.ToString(), "0.00", userID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(TotalAmount, userID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            if (usernameandtype.UserTypeID != 4)
                            {


                                objAccountsService.AddtoUsersAccounts("Commission", "0.00", Comissionamount.ToString(), userID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1 * (Comissionamount), userID, AdminID, DateTime.Now, 0, false, "", false, Password);


                                if (TransferAgentCommision == true && agentpercent > 0)
                                {
                                    if (SuperAmount > 0)
                                    {
                                        ComissionAmountSuper = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * Comissionamount, 2);

                                        //supercommision
                                        objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountSuper.ToString(), "0.00", Convert.ToInt32(superID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                        AddCredittoUser(ComissionAmountSuper, Convert.ToInt32(superID), AdminID, DateTime.Now, 0, false, "", false, Password);

                                    }
                                    else
                                    {
                                        ComissionAmountSuper = 0;
                                    }
                                    ComissionAmountAgent = Math.Round((Convert.ToDecimal(agentpercent) / 100) * Comissionamount, 2);

                                    Comissionamount = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper);

                                    objAccountsService.AddtoComissionAccounts(lstUserBets[0].Marketbookname + " (" + username + ")", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                                    objAccountsService.AddtoUsersAccounts("Commission", Comissionamount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                    AddCredittoUser(Comissionamount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                    //agentcommision
                                    objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountAgent.ToString(), "0.00", Convert.ToInt32(CreatedbyID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                    AddCredittoUser(ComissionAmountAgent, Convert.ToInt32(CreatedbyID), AdminID, DateTime.Now, 0, false, "", false, Password);

                                }
                                else
                                {
                                    objAccountsService.AddtoComissionAccounts(lstUserBets[0].Marketbookname + " (" + username + ")", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                                    objAccountsService.AddtoUsersAccounts("Commission", Comissionamount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                    AddCredittoUser(Comissionamount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                }
                            }
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + agentpercent.ToString() + "%) )", "0.00", AgentAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + superpercent.ToString() + "%) )", "0.00", AgentAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), SuperAmount.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            if (superID != 1)
                            {
                                Comissionamount = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper);
                                decimal finaladminforsuper = AdminAmount - Comissionamount;
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + SuperAmount.ToString() + "%) )", "0.00", AgentAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser((finaladminforsuper), superID, AdminID, DateTime.Now, 0, false, "", false, Password);
                               // AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + SuperAmount.ToString() + "%) )", (AdminAmount).ToString(), "", superID, item.MarketId, DateTime.Now, Password);

                            }

                            if (CreatedbyID == 73)
                            {
                                AddCredittoUser(-1 * (AgentAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                            else
                            {

                                AddCredittoUser((AdminAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            }

                            if (lstUserBets[0].TransferAdmin == false)
                            {
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1 * (AdminAmount), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                            else
                            {
                                decimal transferadminamount1 = 0;
                                decimal AdminAmount1 = 0;
                                transferadminamount1 = Math.Round((Convert.ToDecimal(transferadminpercentage) / 100) * TotalAmount);
                                AdminAmount1 = AdminAmount - transferadminamount1;

                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (transferadminpercentage).ToString() + "%) )", "0.00", transferadminamount1.ToString(), lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount1.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1 * (AdminAmount1), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                AddCredittoUser(-1 * (transferadminamount1), lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                                //objAccountsService.AddtoUsersAccounts("Transfer " + lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                //AddCredittoUser(-1 * (AdminAmount), lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                            // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                            //  AddCredittoUser(-1 * (AdminAmount), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            //  var resultsrefferer = GetReferrerIDandReferrerRatebyUserID(userID);
                            var lstresultsrefferer = GetReferrerRatesbyUserID(userID);
                            foreach (var resultsrefferer in lstresultsrefferer)
                            {

                                decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser((RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), resultsrefferer.ReferrerID.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(resultsrefferer.ReferrerID.Value, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1 * (RefferAmount), resultsrefferer.ReferrerID.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                            }
                            //if (resultsrefferer.ReferrerID > 0 && resultsrefferer.ReferrerRate > 0)
                            //{
                            //    decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                            //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", CreatedbyID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                            //    AddCredittoUser((RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), resultsrefferer.ReferrerID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                            //    AddCredittoUser(-1 * (RefferAmount), resultsrefferer.ReferrerID, AdminID, DateTime.Now, 0, false, "", false, Password);

                            //}
                        }
                        else
                        {
                            TotalAmount = -1 * TotalAmount;
                            int agentpercent = Convert.ToInt32(lstUserBets[0].AgentRate);
                            int superpercent = Convert.ToInt32(lstUserBets[0].SuperAgentRateB);
                            int samiadminrpercent = Convert.ToInt32(lstUserBets[0].SamiAdminRate);
                            int transferadminpercentage = Convert.ToInt32(lstUserBets[0].TransferAdminPercentage);
                            // decimal ActualAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(objAPIConfigService.GetCommisionRate())) / 100) * TotalAmount), 2);
                            //decimal Comissionamount = Math.Round(((Convert.ToDecimal(Crypto.Decrypt(objAPIConfigService.GetCommisionRate())) / 100) * TotalAmount), 2);
                            //  decimal AgentAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(GetAgentRate(userID))) / 100) * TotalAmount), 2);
                            //decimal AdminAmount = Math.Round(TotalAmount - AgentAmount, 2);
                            decimal AgentAmount = 0;
                            decimal SuperAmount = 0;
                            decimal SuperAmount1 = 0;
                            decimal AdminAmount = 0;
                           
                            if (CreatedbyID !=73)
                            {
                                SuperAmount = superpercent - agentpercent;
                                if (SuperAmount > 0)
                                {
                                    SuperAmount1 = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * TotalAmount, 2);
                                }
                                else
                                {
                                    SuperAmount1 = 0;
                                }
                                AgentAmount = Math.Round((Convert.ToDecimal(agentpercent) / 100) * TotalAmount, 2);
                                if (agentpercent == 100)
                                {
                                    AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1, 2);

                                }
                                else
                                {
                                    AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1 , 2);
                                }

                            }
                            else
                            {
                                AdminAmount = Math.Round(TotalAmount, 2);
                            }
                            var results = objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname, "0.00", TotalAmount.ToString(), userID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (TotalAmount), userID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            //  objAccountsService.AddtoComissionAccounts("CommissionAmount", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + agentpercent.ToString() + "%) )", AgentAmount.ToString(), "0.00", CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);

                            if (superID != 1)
                            {
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + SuperAmount.ToString() + "%) )", SuperAmount1.ToString(), "0.00", superID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1 * (AdminAmount), superID, AdminID, DateTime.Now, 0, false, "", false, Password);
                                //AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + SuperAmount.ToString() + "%) )", "", (AdminAmount).ToString(), superID, item.MarketId, DateTime.Now, Password);

                            }


                            if (CreatedbyID == 73)
                            {
                                AddCredittoUser((AgentAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                            else
                            {

                                AddCredittoUser(-1 * (AdminAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                            if (lstUserBets[0].TransferAdmin == false)
                            {
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(AdminAmount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                            else
                            {
                                decimal transferadminamount1 = 0;
                                decimal AdminAmount1 = 0;
                                transferadminamount1 = Math.Round((Convert.ToDecimal(transferadminpercentage) / 100) * TotalAmount);
                                AdminAmount1 = AdminAmount - transferadminamount1;

                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (transferadminpercentage).ToString() + "%) )", transferadminamount1.ToString(), "0.00", lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount1.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(AdminAmount1, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                AddCredittoUser(transferadminamount1, lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                                //objAccountsService.AddtoUsersAccounts("Transfer " + lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                //AddCredittoUser(AdminAmount, lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                            // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                            //  AddCredittoUser(AdminAmount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            // var resultsrefferer = GetReferrerIDandReferrerRatebyUserID(userID);
                            var lstresultsrefferer = GetReferrerRatesbyUserID(userID);
                            foreach (var resultsrefferer in lstresultsrefferer)
                            {
                                decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(-1 * (RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                                objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", resultsrefferer.ReferrerID.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(resultsrefferer.ReferrerID.Value, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser((RefferAmount), resultsrefferer.ReferrerID.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                            }
                            //if (resultsrefferer.ReferrerID > 0 && resultsrefferer.ReferrerRate > 0)
                            //{
                            //    decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                            //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                            //    AddCredittoUser(-1 * (RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", resultsrefferer.ReferrerID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                            //    AddCredittoUser((RefferAmount), resultsrefferer.ReferrerID, AdminID, DateTime.Now, 0, false, "", false, Password);

                            //}
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

                return false;
            }
            catch (System.Exception ex)
            {
                APIConfig.WriteErrorToDB(ex.Message);
                APIConfig.LogError(ex);
                return false;
            }
        }

        public bool InsertUserAccountsFancyIN(string marketid, string Marketname, int userID, string Password, int ScoreforThisOver)
        {
            if (ValidatePassword(Password) == false)
            {
                return false;
            }
            try
            {
                List<SP_UserBets_GetDatabyUserIDandMArketIDIN_Result> lstUserBets = dbEntities.SP_UserBets_GetDatabyUserIDandMArketIDIN(userID, Marketname, marketid).ToList<SP_UserBets_GetDatabyUserIDandMArketIDIN_Result>();

                decimal WinnerTotal = 0;
                decimal WinnerDebit = 0;
                decimal WinnerCredit = 0;
                decimal LosserTotal = 0;
                decimal LosserDebit = 0;
                decimal LosserCredit = 0;
                decimal TotalAmount = 0;
                string Winnername = "";
              
                if (1 == 1)
                {
                    dbEntities.SP_UserBets_UpdateStatusIN(marketid, Marketname, userID);

                    var lstBackBets = lstUserBets.Where(item1 => item1.BetType == "back" && item1.isMatched == true).ToList();
                    var lstLayBets = lstUserBets.Where(item1 => item1.BetType == "lay" && item1.isMatched == true).ToList();

                    Winnername = ScoreforThisOver.ToString();
                    int winnerscore = ScoreforThisOver;
                    foreach (var backbetitem in lstBackBets)
                    {
                        int odds = Convert.ToInt32(backbetitem.UserOdd);
                        double size = Convert.ToDouble(backbetitem.BetSize);
                        if (odds <= winnerscore)
                        {

                            double itemamount =Convert.ToDouble(backbetitem.Amount);
                            double amount = ((size) / 100) * itemamount;
                            WinnerDebit += Convert.ToInt32(amount);
                        }
                        else
                        {
                            WinnerCredit += Convert.ToInt32(backbetitem.Amount);
                        }
                    }
                    foreach (var backbetitem in lstLayBets)
                    {
                        int aa = Convert.ToInt32(backbetitem.UserOdd);
                        double size = Convert.ToDouble(backbetitem.BetSize);                   
                        if (aa > winnerscore)
                        {
                            WinnerDebit += Convert.ToInt32(backbetitem.Amount);
                        }
                        else
                        {

                            double itemamount = Convert.ToDouble(backbetitem.Amount);
                            double amount = (Convert.ToDouble(size) / 100) * itemamount;
                            WinnerCredit += Convert.ToInt32(amount);
                        }
                    }
                 

                    WinnerTotal = WinnerDebit - WinnerCredit;
                    if (LosserTotal < 0)
                    {
                        LosserTotal = -1 * (LosserTotal);
                    }
                    //  TotalAmount = WinnerTotal - LosserTotal;
                    TotalAmount = WinnerTotal;

                    int samiadminID = 0;
                    int AdminID = 0;
                    int CreatedbyID = GetCreatedbyID1(userID);
                    //bool isAgentcom = GetIsComAllowbyUserID(CreatedbyID);
                    int superID = GetCreatedbyID1(CreatedbyID);
                    //bool isSupercom = GetIsComAllowbyUserID(superID);
                    //bool issamiadmincom = false;

                    if (superID == 1)
                    {
                        AdminID = 1;
                        //AdminID = samiadminID;
                    }
                    else
                    {
                        if (superID != 1)
                        {
                            samiadminID = GetCreatedbyID1(superID);
                            //issamiadmincom = GetIsComAllowbyUserID(samiadminID);
                            if (samiadminID == 1)
                            {
                                AdminID = 1;
                            }
                            else
                            {
                                AdminID = GetCreatedbyID1(samiadminID);
                            }
                            //int samiadminID = GetCreatedbyID1(superID);
                        }
                    }
                    bool TransferAgentCommision = GetTransferAgnetCommision(CreatedbyID);
                    SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionAccountIDandBookAccountID = dbEntities.SP_Users_GetCommissionAccountIDandBookAccountID(AdminID).FirstOrDefault();
                   
                    var usernameandtype = dbEntities.SP_Users_GetUSernamebyUserID(userID.ToString()).FirstOrDefault();
                    string username = Crypto.Decrypt(usernameandtype.Username);
                    string EventTypename = "Fancy";
                    //var eventdetails = dbEntities.SP_UserMarket_GetEventDetailsbyMarketID(marketid).FirstOrDefault();
                    decimal Comissionamount = 0;
                    Comissionamount = Math.Round(((Convert.ToDecimal(GetCommissionRatebyUserIDFancy(userID)) / 100) * TotalAmount), 2);
                  
                    if (TotalAmount >= 0)
                    {

                        int agentpercent = Convert.ToInt32(lstUserBets[0].AgentRate);
                        int superpercent = Convert.ToInt32(lstUserBets[0].SuperAgentRateB);
                        int samiadminpercent = Convert.ToInt32(lstUserBets[0].SamiAdminRate);
                        int transferadminpercentage = Convert.ToInt32(lstUserBets[0].TransferAdminPercentage);
                        decimal ActualAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(GetCommissionRatebyUserID(userID)) / 100) * TotalAmount), 2);


                        decimal AgentAmount = 0;
                        decimal AdminAmount = 0;
                        decimal superpercentfinal = 0;
                        decimal samiadminpercentfinal = 0;
                        decimal SuperAmount1 = 0;
                        decimal SamiadminAmount1 = 0;
                        decimal ComissionAmountAgent = 0;
                        decimal ComissionAmountSuper = 0;
                        decimal ComissionAmountSamiadin = 0;


                        if (CreatedbyID != 73)
                        {
                            superpercentfinal = superpercent - agentpercent;
                            if (superpercentfinal > 0)
                            {
                                SuperAmount1 = Math.Round((Convert.ToDecimal(superpercentfinal) / 100) * TotalAmount, 2);
                            }
                            else
                            {
                                SuperAmount1 = 0;
                            }
                            if (samiadminID != 1)
                            {
                                samiadminpercentfinal = samiadminpercent - (superpercentfinal + agentpercent);
                                if (samiadminpercentfinal > 0)
                                {
                                    SamiadminAmount1 = Math.Round((Convert.ToDecimal(samiadminpercentfinal) / 100) * TotalAmount, 2);
                                }
                                else
                                {
                                    SamiadminAmount1 = 0;
                                }
                            }
                            AgentAmount = Math.Round((Convert.ToDecimal(agentpercent) / 100) * TotalAmount, 2);

                            if (agentpercent == 100)
                            {
                                AdminAmount = Math.Round(TotalAmount - (AgentAmount + SuperAmount1 + SamiadminAmount1), 2);

                            }
                            else
                            {
                                AdminAmount = Math.Round(TotalAmount - (AgentAmount + SuperAmount1 + SamiadminAmount1 + Comissionamount), 2);
                            }

                        }
                        else
                        {
                            AdminAmount = Math.Round(TotalAmount - Comissionamount, 2);
                        }
                        // int agentpercent = Convert.ToInt32(Crypto.Decrypt(GetAgentRate(userID)));
                       
                        var results = objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname, TotalAmount.ToString(), "0.00", userID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                        AddCredittoUser(TotalAmount, userID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        if (usernameandtype.UserTypeID != 4)
                        {
                            objAccountsService.AddtoUsersAccounts("Commission", "0.00", Comissionamount.ToString(), userID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (Comissionamount), userID, AdminID, DateTime.Now, 0, false, "", false, Password);

                            if (TransferAgentCommision == true && agentpercent > 0)
                            {
                                if (superpercentfinal > 0)
                                {
                                    ComissionAmountSuper = Math.Round((Convert.ToDecimal(superpercentfinal) / 100) * Comissionamount, 2);
                                    objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountSuper.ToString(), "0.00", Convert.ToInt32(superID), lstUserBets[0].MarketBookID, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);

                                    //if (isSupercom == false)
                                    //{
                                        AddCredittoUser(ComissionAmountSuper, Convert.ToInt32(superID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                    //}
                                }
                                else
                                {
                                    ComissionAmountSuper = 0;
                                }

                                if (samiadminpercentfinal > 0)
                                {
                                    ComissionAmountSamiadin = Math.Round((Convert.ToDecimal(samiadminpercentfinal) / 100) * Comissionamount, 2);
                                    objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountSamiadin.ToString(), "0.00", Convert.ToInt32(samiadminID), lstUserBets[0].MarketBookID, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);

                                    //if (issamiadmincom == false)
                                    //{
                                        AddCredittoUser(ComissionAmountSamiadin, Convert.ToInt32(superID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                    //}
                                }
                                else
                                {
                                    ComissionAmountSamiadin = 0;
                                }

                                ComissionAmountAgent = Math.Round((Convert.ToDecimal(agentpercent) / 100) * Comissionamount, 2);

                                decimal Comissionamounadmin = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper + ComissionAmountSamiadin);
                                objAccountsService.AddtoComissionAccounts(lstUserBets[0].Marketbookname + " (" + username + ")", Comissionamounadmin.ToString(), Convert.ToInt64(results), DateTime.Now);
                                objAccountsService.AddtoUsersAccounts("Commission", Comissionamounadmin.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), lstUserBets[0].MarketBookID, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                                AddCredittoUser(Comissionamounadmin, Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                //i am here
                                objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountAgent.ToString(), "0.00", Convert.ToInt32(CreatedbyID), lstUserBets[0].MarketBookID, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                                //if (isAgentcom == false)
                                //{
                                    AddCredittoUser(ComissionAmountAgent, Convert.ToInt32(CreatedbyID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                //}
                            }
                            else
                            {
                                decimal Comissionamounadmin = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper);
                                objAccountsService.AddtoComissionAccounts(lstUserBets[0].Marketbookname + " (" + username + ")", Comissionamounadmin.ToString(), Convert.ToInt64(results), DateTime.Now);
                                objAccountsService.AddtoUsersAccounts("Commission", Comissionamounadmin.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                                AddCredittoUser(Comissionamounadmin, Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                        }

                        objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + agentpercent.ToString() + "%) )", "0.00", AgentAmount.ToString(), CreatedbyID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                        if (superID != 1)
                        {
                            AdminAmount = AdminAmount + Comissionamount;
                            decimal Comissionamountadmin = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper);
                            AdminAmount = AdminAmount - Comissionamountadmin;
                            decimal ComissionAmountAgent1 = Comissionamount - (ComissionAmountSuper + Comissionamountadmin);
                            AgentAmount = AgentAmount - ComissionAmountAgent1;
                            decimal AdminAmountforsuper1 = AdminAmount + AgentAmount;
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + superpercentfinal.ToString() + "%) )", "0.00", SuperAmount1.ToString(), superID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (AdminAmountforsuper1), superID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            //AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + agentpercent.ToString() + "%) )", (AdminAmount).ToString(),"", superID, item.MarketId, DateTime.Now, Password);

                        }
                        if (CreatedbyID == 73)
                        {
                            AddCredittoUser(-1 * (AgentAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {
                            AdminAmount = AdminAmount + Comissionamount;
                            decimal Comissionamountadmin = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper);
                            AdminAmount = AdminAmount - Comissionamountadmin;
                            decimal ComissionAmountSuper1 = Comissionamount - (ComissionAmountAgent + Comissionamountadmin);
                            SuperAmount1 = SuperAmount1 - ComissionAmountSuper1;
                            ComissionAmountAgent = ComissionAmountSuper1 + Comissionamountadmin;
                            decimal amount = AdminAmount + SuperAmount1;
                            AddCredittoUser((amount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }

                        if (lstUserBets[0].TransferAdmin == false)
                        {
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (AdminAmount), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {
                            decimal transferadminamount1 = 0;
                            decimal AdminAmount1 = 0;
                            transferadminamount1 = Math.Round((Convert.ToDecimal(transferadminpercentage) / 100) * TotalAmount);
                            AdminAmount1 = AdminAmount - transferadminamount1;


                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (transferadminpercentage).ToString() + "%) )", "0.00", transferadminamount1.ToString(), lstUserBets[0].TransferAgentIDB.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount1.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);

                            AddCredittoUser(-1 * (AdminAmount1), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            AddCredittoUser(-1 * (transferadminamount1), lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                        }


                        var lstresultsrefferer = GetReferrerRatesbyUserID(userID);
                        foreach (var resultsrefferer in lstresultsrefferer)
                        {
                            decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", CreatedbyID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                            AddCredittoUser((RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), resultsrefferer.ReferrerID.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(resultsrefferer.ReferrerID.Value, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (RefferAmount), resultsrefferer.ReferrerID.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                        }


                    }
                    else
                    {
                        TotalAmount = -1 * TotalAmount;
                        int agentpercent = Convert.ToInt32(lstUserBets[0].AgentRate);
                        int superpercent = Convert.ToInt32(lstUserBets[0].SuperAgentRateB);
                        int samiadminpercent = Convert.ToInt32(lstUserBets[0].SamiAdminRate);
                        int transferadminpercentage = Convert.ToInt32(lstUserBets[0].TransferAdminPercentage);
                        // decimal ActualAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(objAPIConfigService.GetCommisionRate())) / 100) * TotalAmount), 2);
                        //decimal Comissionamount = Math.Round(((Convert.ToDecimal(Crypto.Decrypt(objAPIConfigService.GetCommisionRate())) / 100) * TotalAmount), 2);
                        //  decimal AgentAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(GetAgentRate(userID))) / 100) * TotalAmount), 2);
                        //decimal AdminAmount = Math.Round(TotalAmount - AgentAmount, 2);
                       

                        decimal AgentAmount = 0;
                        decimal SuperAmount = 0;
                        decimal SamiadminAmount = 0;
                        decimal SuperAmount1 = 0;
                        decimal SamiadminAmount1 = 0;
                        decimal AdminAmount = 0;

                        if (CreatedbyID != 73)
                        {
                            SuperAmount = superpercent - agentpercent;
                            if (SuperAmount >= 0)
                            {
                                SuperAmount1 = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * TotalAmount, 2);
                            }
                            else
                            {
                                SuperAmount1 = 0;
                            }
                            if (samiadminID != 1)
                            {
                                SamiadminAmount = samiadminpercent - (SuperAmount + agentpercent);
                                if (SamiadminAmount > 0)
                                {
                                    SamiadminAmount1 = Math.Round((Convert.ToDecimal(SamiadminAmount) / 100) * TotalAmount, 2);
                                }
                                else
                                {
                                    SamiadminAmount1 = 0;
                                }
                            }
                            //  AgentAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(GetAgentRate(userID))) / 100) * TotalAmount), 2);
                            AgentAmount = Math.Round((Convert.ToDecimal(agentpercent) / 100) * TotalAmount, 2);
                            AdminAmount = Math.Round(TotalAmount - (AgentAmount + SuperAmount1 + SamiadminAmount1), 2);
                        }
                        else
                        {
                            AdminAmount = Math.Round(TotalAmount, 2);
                        }
                        var results = objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname, "0.00", TotalAmount.ToString(), userID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                        AddCredittoUser(-1 * (TotalAmount), userID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        //  objAccountsService.AddtoComissionAccounts("CommissionAmount", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                        objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + agentpercent.ToString() + "%) )", AgentAmount.ToString(), "0.00", CreatedbyID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);

                        if (superID != 1)
                        {
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + SuperAmount.ToString() + "%) )", SuperAmount1.ToString(), "0.00", superID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (AdminAmount), superID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            
                        }


                        if (CreatedbyID == 73)
                        {
                            AddCredittoUser((AgentAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {

                            AddCredittoUser(-1 * (AdminAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        if (lstUserBets[0].TransferAdmin == false)
                        {
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                            AddCredittoUser(AdminAmount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {
                            decimal transferadminamount1 = 0;
                            decimal AdminAmount1 = 0;
                            transferadminamount1 = Math.Round((Convert.ToDecimal(transferadminpercentage) / 100) * TotalAmount);
                            AdminAmount1 = AdminAmount - transferadminamount1;

                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (transferadminpercentage).ToString() + "%) )", transferadminamount1.ToString(), "0.00", lstUserBets[0].TransferAgentIDB.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount1.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                            AddCredittoUser(AdminAmount1, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            AddCredittoUser(transferadminamount1, lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                            //objAccountsService.AddtoUsersAccounts("Transfer " + lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            //AddCredittoUser(AdminAmount, lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //  AddCredittoUser(AdminAmount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        // var resultsrefferer = GetReferrerIDandReferrerRatebyUserID(userID);
                        var lstresultsrefferer = GetReferrerRatesbyUserID(userID);
                        foreach (var resultsrefferer in lstresultsrefferer)
                        {
                            decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), CreatedbyID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", resultsrefferer.ReferrerID.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(resultsrefferer.ReferrerID.Value, Password)), false, EventTypename, Winnername, lstUserBets[0].MarketBookID, lstUserBets[0].Selectionname, lstUserBets[0].Marketbookname);
                            AddCredittoUser((RefferAmount), resultsrefferer.ReferrerID.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                        }
                        //if (resultsrefferer.ReferrerID > 0 && resultsrefferer.ReferrerRate > 0)
                        //{
                        //    decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser(-1 * (RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", resultsrefferer.ReferrerID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser((RefferAmount), resultsrefferer.ReferrerID, AdminID, DateTime.Now, 0, false, "", false, Password);

                        //}
                    }
                    return true;
                }
                else
                {
                    return false;
                }

                //}

                return false;
            }
            catch (System.Exception ex)
            {
                APIConfig.WriteErrorToDB(ex.Message);
                APIConfig.LogError(ex);
                return false;
            }
        }

        public bool InsertUserAccountsFancyKJ(string marketid, int selectionID, int userID, string Password, int ScoreforThisOver)
        {
            if (ValidatePassword(Password) == false)
            {
                return false;
            }
            try
            {
                List<SP_UserBets_GetDatabyUserIDandMArketID_Result> lstUserBets = dbEntities.SP_UserBets_GetDatabyUserIDandMArketID(userID,marketid).ToList<SP_UserBets_GetDatabyUserIDandMArketID_Result>();
                decimal WinnerTotal = 0;
                decimal WinnerDebit = 0;
                decimal WinnerCredit = 0;
                decimal LosserTotal = 0;
                decimal LosserDebit = 0;
                decimal LosserCredit = 0;
                decimal TotalAmount = 0;
                string Winnername = "";

                if (1 == 1)
                {
                    dbEntities.SP_UserBets_UpdateStatus(marketid, userID);

                    var lstBackBets = lstUserBets.Where(item1 => item1.BetType == "back" && item1.isMatched == true).ToList();
                    var lstLayBets = lstUserBets.Where(item1 => item1.BetType == "lay" && item1.isMatched == true).ToList();

                    Winnername = ScoreforThisOver.ToString();
                    int winnerscores = ScoreforThisOver;
                    int winnerscore = winnerscores % 2;
                    foreach (var backbetitem in lstBackBets)
                    {
                        double odds = Convert.ToDouble(backbetitem.UserOdd);
                        double size = Convert.ToDouble(backbetitem.BetSize);
                        if (winnerscore==1)
                        {
                            double itemamount = Convert.ToInt32(backbetitem.Amount);
                            double amount = ((size) / 100) * itemamount;
                            WinnerDebit += Convert.ToInt32(amount);
                        }           
                        else
                        {                          
                                WinnerCredit += Convert.ToInt32(backbetitem.Amount);                          
                        }
                                               
                    }
                    foreach (var backbetitem in lstLayBets)
                    {
                        double aa = Convert.ToDouble(backbetitem.UserOdd);
                        double size1 = Convert.ToDouble(backbetitem.BetSize);
                        if (winnerscore == 0)
                        {
                            WinnerDebit += Convert.ToInt32(backbetitem.Amount);
                        }
                        else
                        {                          
                            double itemamount1 = Convert.ToInt32(backbetitem.Amount);
                            double amount1 = ((size1) / 100) * itemamount1;
                            WinnerCredit += Convert.ToInt32(amount1);                            
                        }
                      }
                   


                    WinnerTotal = WinnerDebit - WinnerCredit;
                    if (LosserTotal < 0)
                    {
                        LosserTotal = -1 * (LosserTotal);
                    }
                    //  TotalAmount = WinnerTotal - LosserTotal;
                    TotalAmount = WinnerTotal;

                    int AdminID = 0;
                    int CreatedbyID = GetCreatedbyID1(userID);
                    int superID = GetCreatedbyID1(CreatedbyID);
                    if (superID == 1)
                    {
                        AdminID = 1;
                    }
                    if (superID != 1)
                    {
                        AdminID = GetCreatedbyID1(superID);
                    }

                    SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionAccountIDandBookAccountID = dbEntities.SP_Users_GetCommissionAccountIDandBookAccountID(AdminID).FirstOrDefault();

                    var usernameandtype = dbEntities.SP_Users_GetUSernamebyUserID(userID.ToString()).FirstOrDefault();
                    string username = Crypto.Decrypt(usernameandtype.Username);
                    string EventTypename = "FancyKali";
                    var eventdetails = dbEntities.SP_UserMarket_GetEventDetailsbyMarketID(marketid).FirstOrDefault();
                    //if (item.Runners.Count == 1)
                    //{
                    //    EventTypename = "Fancy";
                    //}
                    if (TotalAmount >= 0)
                    {
                        // int agentpercent = Convert.ToInt32(Crypto.Decrypt(GetAgentRate(userID)));
                        int agentpercent = Convert.ToInt32(lstUserBets[0].AgentRate);
                        int superpercent = Convert.ToInt32(lstUserBets[0].SuperAgentRateB);
                        int samiadminpercent = Convert.ToInt32(lstUserBets[0].SamiAdminRate);
                        int transferadminpercentage = Convert.ToInt32(lstUserBets[0].TransferAdminPercentage);
                        decimal ActualAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(GetCommissionRatebyUserID(userID)) / 100) * TotalAmount), 2);
                        decimal Comissionamount = 0;
                        decimal Comissionamount1 = 0;
                        decimal ComissionAmountAgent = 0;
                        decimal ComissionAmountSuper = 0;
                        // if (item.Runners.Count == 1)
                        // {
                        Comissionamount = Math.Round(((Convert.ToDecimal(GetCommissionRatebyUserIDFancy(userID)) / 100) * TotalAmount), 2);
                         Comissionamount1 = Comissionamount;
                        //}
                        //else
                        //{
                        //    Comissionamount = Math.Round(((Convert.ToDecimal(GetCommissionRatebyUserID(userID)) / 100) * TotalAmount), 2);
                        //}
                        decimal AgentAmount = 0;
                        decimal SuperAmount = 0;
                        decimal SuperAmount1 = 0;
                        decimal AdminAmount = 0;
                        bool TransferAgentCommision = GetTransferAgnetCommision(CreatedbyID);

                        if (CreatedbyID != 73)
                        {
                            SuperAmount = superpercent - agentpercent;
                            if (SuperAmount > 0)
                            {
                                SuperAmount1 = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * TotalAmount, 2);
                            }
                            else
                            {
                                SuperAmount1 = 0;
                            }
                            AgentAmount = Math.Round((Convert.ToDecimal(agentpercent) / 100) * TotalAmount, 2);
                            if (agentpercent == 100)
                            {
                                AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1, 2);

                            }
                            else
                            {
                                AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1 - Comissionamount, 2);
                            }

                        }
                        else
                        {
                            AdminAmount = Math.Round(TotalAmount - Comissionamount, 2);
                        }

                        //GetCommissionRatebyUserIDFancy(userID).ToString()
                        var results = objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname, TotalAmount.ToString(), "0.00", userID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                        AddCredittoUser(TotalAmount, userID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        if (usernameandtype.UserTypeID != 4)
                        {

                            objAccountsService.AddtoUsersAccounts("Commission", "0.00", Comissionamount.ToString(), userID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (Comissionamount), userID, AdminID, DateTime.Now, 0, false, "", false, Password);


                            if (TransferAgentCommision == true && agentpercent > 0)
                            {
                                if (SuperAmount > 0)
                                {
                                    ComissionAmountSuper = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * Comissionamount, 2);

                                    //supercommision
                                    objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountSuper.ToString(), "0.00", Convert.ToInt32(superID),marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                    AddCredittoUser(ComissionAmountSuper, Convert.ToInt32(superID), AdminID, DateTime.Now, 0, false, "", false, Password);

                                }
                                else
                                {
                                    ComissionAmountSuper = 0;
                                }
                                ComissionAmountAgent = Math.Round((Convert.ToDecimal(agentpercent) / 100) * Comissionamount, 2);

                                Comissionamount = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper);

                                objAccountsService.AddtoComissionAccounts(lstUserBets[0].Selectionname + " (" + username + ")", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                                objAccountsService.AddtoUsersAccounts("Commission", Comissionamount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID),marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(Comissionamount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                //agentcommision
                                objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountAgent.ToString(), "0.00", Convert.ToInt32(CreatedbyID),marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(ComissionAmountAgent, Convert.ToInt32(CreatedbyID), AdminID, DateTime.Now, 0, false, "", false, Password);

                            }
                            else
                            {
                                objAccountsService.AddtoComissionAccounts(lstUserBets[0].Selectionname + " (" + username + ")", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                                objAccountsService.AddtoUsersAccounts("Commission", Comissionamount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(Comissionamount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                        }
                        objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + agentpercent.ToString() + "%) )", "0.00", AgentAmount.ToString(), CreatedbyID,marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                        // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + superpercent.ToString() + "%) )", "0.00", AgentAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), SuperAmount.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                        if (superID != 1)
                        {
                            Comissionamount = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper);
                            decimal finaladminforsuper = AdminAmount - Comissionamount;
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + SuperAmount.ToString() + "%) )", "0.00", SuperAmount1.ToString(), superID,marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser((SuperAmount), superID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            // AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + SuperAmount.ToString() + "%) )", (AdminAmount).ToString(), "", superID, item.MarketId, DateTime.Now, Password);

                        }

                        if (CreatedbyID == 73)
                        {
                            AddCredittoUser(-1 * (AgentAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {

                            AddCredittoUser((AdminAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }

                        if (lstUserBets[0].TransferAdmin == false)
                        {
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID),marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (AdminAmount), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {
                            decimal transferadminamount1 = 0;
                            decimal AdminAmount1 = 0;
                            transferadminamount1 = Math.Round(((Convert.ToDecimal(transferadminpercentage) / 100) * TotalAmount)-Comissionamount1);
                            AdminAmount1 = AdminAmount - transferadminamount1;

                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (transferadminpercentage).ToString() + "%) )", "0.00", transferadminamount1.ToString(), lstUserBets[0].TransferAgentIDB.Value,marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount1.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID),marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (AdminAmount1), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            AddCredittoUser(-1 * (transferadminamount1), lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                            //objAccountsService.AddtoUsersAccounts("Transfer " + lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            //AddCredittoUser(-1 * (AdminAmount), lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //  AddCredittoUser(-1 * (AdminAmount), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        //  var resultsrefferer = GetReferrerIDandReferrerRatebyUserID(userID);
                        var lstresultsrefferer = GetReferrerRatesbyUserID(userID);
                        foreach (var resultsrefferer in lstresultsrefferer)
                        {

                            decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", CreatedbyID,marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser((RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), resultsrefferer.ReferrerID.Value,marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(resultsrefferer.ReferrerID.Value, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (RefferAmount), resultsrefferer.ReferrerID.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                        }
                        //if (resultsrefferer.ReferrerID > 0 && resultsrefferer.ReferrerRate > 0)
                        //{
                        //    decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", CreatedbyID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser((RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), resultsrefferer.ReferrerID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser(-1 * (RefferAmount), resultsrefferer.ReferrerID, AdminID, DateTime.Now, 0, false, "", false, Password);

                        //}
                    }
                    else
                    {
                        TotalAmount = -1 * TotalAmount;
                        int agentpercent = Convert.ToInt32(lstUserBets[0].AgentRate);
                        int superpercent = Convert.ToInt32(lstUserBets[0].SuperAgentRateB);
                        int samiadminpercent = Convert.ToInt32(lstUserBets[0].SamiAdminRate);
                        int transferadminpercentage = Convert.ToInt32(lstUserBets[0].TransferAdminPercentage);
                        // decimal ActualAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(objAPIConfigService.GetCommisionRate())) / 100) * TotalAmount), 2);
                        //decimal Comissionamount = Math.Round(((Convert.ToDecimal(Crypto.Decrypt(objAPIConfigService.GetCommisionRate())) / 100) * TotalAmount), 2);
                        //  decimal AgentAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(GetAgentRate(userID))) / 100) * TotalAmount), 2);
                        //decimal AdminAmount = Math.Round(TotalAmount - AgentAmount, 2);
                        decimal AgentAmount = 0;
                        decimal SuperAmount = 0;
                        decimal SuperAmount1 = 0;
                        decimal AdminAmount = 0;

                        if (CreatedbyID != 73)
                        {
                            SuperAmount = superpercent - agentpercent;
                            if (SuperAmount > 0)
                            {
                                SuperAmount1 = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * TotalAmount, 2);
                            }
                            else
                            {
                                SuperAmount1 = 0;
                            }
                            AgentAmount = Math.Round((Convert.ToDecimal(agentpercent) / 100) * TotalAmount, 2);
                            if (agentpercent == 100)
                            {
                                AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1, 2);

                            }
                            else
                            {
                                AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1, 2);
                            }

                        }
                        else
                        {
                            AdminAmount = Math.Round(TotalAmount, 2);
                        }
                        var results = objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname, "0.00", TotalAmount.ToString(), userID,marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                        AddCredittoUser(-1 * (TotalAmount), userID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        //  objAccountsService.AddtoComissionAccounts("CommissionAmount", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                        objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + agentpercent.ToString() + "%) )", AgentAmount.ToString(), "0.00", CreatedbyID,marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);

                        if (superID != 1)
                        {
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + SuperAmount.ToString() + "%) )", SuperAmount1.ToString(), "0.00", superID,marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (SuperAmount), superID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            //AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + SuperAmount.ToString() + "%) )", "", (AdminAmount).ToString(), superID, item.MarketId, DateTime.Now, Password);

                        }


                        if (CreatedbyID == 73)
                        {
                            AddCredittoUser((AgentAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {

                            AddCredittoUser(-1 * (AdminAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        if (lstUserBets[0].TransferAdmin == false)
                        {
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID),marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(AdminAmount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {
                            decimal transferadminamount1 = 0;
                            decimal AdminAmount1 = 0;
                            transferadminamount1 = Math.Round((Convert.ToDecimal(transferadminpercentage) / 100) * TotalAmount);
                            AdminAmount1 = AdminAmount - transferadminamount1;

                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (transferadminpercentage).ToString() + "%) )", transferadminamount1.ToString(), "0.00", lstUserBets[0].TransferAgentIDB.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount1.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID),marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(AdminAmount1, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            AddCredittoUser(transferadminamount1, lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                            //objAccountsService.AddtoUsersAccounts("Transfer " + lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            //AddCredittoUser(AdminAmount, lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //  AddCredittoUser(AdminAmount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        // var resultsrefferer = GetReferrerIDandReferrerRatebyUserID(userID);
                        var lstresultsrefferer = GetReferrerRatesbyUserID(userID);
                        foreach (var resultsrefferer in lstresultsrefferer)
                        {
                            decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), CreatedbyID,marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", resultsrefferer.ReferrerID.Value,marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(resultsrefferer.ReferrerID.Value, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser((RefferAmount), resultsrefferer.ReferrerID.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                        }
                        //if (resultsrefferer.ReferrerID > 0 && resultsrefferer.ReferrerRate > 0)
                        //{
                        //    decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser(-1 * (RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", resultsrefferer.ReferrerID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser((RefferAmount), resultsrefferer.ReferrerID, AdminID, DateTime.Now, 0, false, "", false, Password);

                        //}
                    }
                    return true;
                }
                else
                {
                    return false;
                }

                //}

                return false;
            }
            catch (System.Exception ex)
            {
                APIConfig.WriteErrorToDB(ex.Message);
                APIConfig.LogError(ex);
                return false;
            }
        }

        public bool InsertUserAccountsFancySmallFig(string marketid, int selectionID, int userID, string Password, int ScoreforThisOver)
        {
            if (ValidatePassword(Password) == false)
            {
                return false;
            }
            try
            {
                List<SP_UserBets_GetDatabyUserIDandMArketID_Result> lstUserBets = dbEntities.SP_UserBets_GetDatabyUserIDandMArketID(userID, marketid).ToList<SP_UserBets_GetDatabyUserIDandMArketID_Result>();
                decimal WinnerTotal = 0;
                decimal WinnerDebit = 0;
                decimal WinnerCredit = 0;
                decimal LosserTotal = 0;
                decimal LosserDebit = 0;
                decimal LosserCredit = 0;
                decimal TotalAmount = 0;
                string Winnername = "";

                if (1 == 1)
                {
                    dbEntities.SP_UserBets_UpdateStatus(marketid, userID);

                    var lstBackBets = lstUserBets.Where(item1 => item1.BetType == "back" && item1.isMatched == true).ToList();
                    var lstLayBets = lstUserBets.Where(item1 => item1.BetType == "lay" && item1.isMatched == true).ToList();

                    Winnername = ScoreforThisOver.ToString();
                    int winnerscores = ScoreforThisOver;
                    
                    foreach (var backbetitem in lstBackBets)
                    {
                        double odds = Convert.ToDouble(backbetitem.UserOdd);
                        double size = Convert.ToDouble(backbetitem.BetSize);
                        if (winnerscores <= 4)
                        {
                            double itemamount = Convert.ToInt32(backbetitem.Amount);
                            double amount = ((size) / 100) * itemamount;
                            WinnerDebit += Convert.ToInt32(amount);
                        }
                        else
                        {
                            WinnerCredit += Convert.ToInt32(backbetitem.Amount);
                        }

                    }
                    foreach (var backbetitem in lstLayBets)
                    {
                        double aa = Convert.ToDouble(backbetitem.UserOdd);
                        double size1 = Convert.ToDouble(backbetitem.BetSize);
                        if (winnerscores >= 5)
                        {
                            WinnerDebit += Convert.ToInt32(backbetitem.Amount);
                        }
                        else
                        {
                            double itemamount1 = Convert.ToInt32(backbetitem.Amount);
                            double amount1 = ((size1) / 100) * itemamount1;
                            WinnerCredit += Convert.ToInt32(amount1);
                        }
                    }

                    WinnerTotal = WinnerDebit - WinnerCredit;

                    if (LosserTotal < 0)
                    {
                        LosserTotal = -1 * (LosserTotal);
                    }
                    //  TotalAmount = WinnerTotal - LosserTotal;
                    TotalAmount = WinnerTotal;

                    int AdminID = 0;
                    int CreatedbyID = GetCreatedbyID1(userID);
                    int superID = GetCreatedbyID1(CreatedbyID);
                    if (superID == 1)
                    {
                        AdminID = 1;
                    }
                    if (superID != 1)
                    {
                        AdminID = GetCreatedbyID1(superID);
                    }

                    SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionAccountIDandBookAccountID = dbEntities.SP_Users_GetCommissionAccountIDandBookAccountID(AdminID).FirstOrDefault();

                    var usernameandtype = dbEntities.SP_Users_GetUSernamebyUserID(userID.ToString()).FirstOrDefault();
                    string username = Crypto.Decrypt(usernameandtype.Username);
                    string EventTypename = "Small-Fig";
                    var eventdetails = dbEntities.SP_UserMarket_GetEventDetailsbyMarketID(marketid).FirstOrDefault();
                    //if (item.Runners.Count == 1)
                    //{
                    //    EventTypename = "Fancy";
                    //}
                    if (TotalAmount >= 0)
                    {
                        // int agentpercent = Convert.ToInt32(Crypto.Decrypt(GetAgentRate(userID)));
                        int agentpercent = Convert.ToInt32(lstUserBets[0].AgentRate);
                        int superpercent = Convert.ToInt32(lstUserBets[0].SuperAgentRateB);
                        int samiadminpercent = Convert.ToInt32(lstUserBets[0].SamiAdminRate);
                        int transferadminpercentage = Convert.ToInt32(lstUserBets[0].TransferAdminPercentage);
                        decimal ActualAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(GetCommissionRatebyUserID(userID)) / 100) * TotalAmount), 2);
                        decimal Comissionamount = 0;
                        decimal Comissionamount1 = 0;
                        decimal ComissionAmountAgent = 0;
                        decimal ComissionAmountSuper = 0;
                        // if (item.Runners.Count == 1)
                        // {
                        Comissionamount = Math.Round(((Convert.ToDecimal(GetCommissionRatebyUserIDFancy(userID)) / 100) * TotalAmount), 2);
                        Comissionamount1 = Comissionamount;
                        //}
                        //else
                        //{
                        //    Comissionamount = Math.Round(((Convert.ToDecimal(GetCommissionRatebyUserID(userID)) / 100) * TotalAmount), 2);
                        //}
                        decimal AgentAmount = 0;
                        decimal SuperAmount = 0;
                        decimal SuperAmount1 = 0;
                        decimal AdminAmount = 0;
                        bool TransferAgentCommision = GetTransferAgnetCommision(CreatedbyID);

                        if (CreatedbyID != 73)
                        {
                            SuperAmount = superpercent - agentpercent;
                            if (SuperAmount > 0)
                            {
                                SuperAmount1 = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * TotalAmount, 2);
                            }
                            else
                            {
                                SuperAmount1 = 0;
                            }
                            AgentAmount = Math.Round((Convert.ToDecimal(agentpercent) / 100) * TotalAmount, 2);
                            if (agentpercent == 100)
                            {
                                AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1, 2);

                            }
                            else
                            {
                                AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1 - Comissionamount, 2);
                            }

                        }
                        else
                        {
                            AdminAmount = Math.Round(TotalAmount - Comissionamount, 2);
                        }

                        //GetCommissionRatebyUserIDFancy(userID).ToString()
                        var results = objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname, TotalAmount.ToString(), "0.00", userID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString() ,GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                        AddCredittoUser(TotalAmount, userID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        if (usernameandtype.UserTypeID != 4)
                        {

                            objAccountsService.AddtoUsersAccounts("Commission", "0.00", Comissionamount.ToString(), userID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (Comissionamount), userID, AdminID, DateTime.Now, 0, false, "", false, Password);


                            if (TransferAgentCommision == true && agentpercent > 0)
                            {
                                if (SuperAmount > 0)
                                {
                                    ComissionAmountSuper = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * Comissionamount, 2);

                                    //supercommision
                                    objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountSuper.ToString(), "0.00", Convert.ToInt32(superID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                    AddCredittoUser(ComissionAmountSuper, Convert.ToInt32(superID), AdminID, DateTime.Now, 0, false, "", false, Password);

                                }
                                else
                                {
                                    ComissionAmountSuper = 0;
                                }
                                ComissionAmountAgent = Math.Round((Convert.ToDecimal(agentpercent) / 100) * Comissionamount, 2);

                                Comissionamount = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper);

                                objAccountsService.AddtoComissionAccounts(lstUserBets[0].Selectionname + " (" + username + ")", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                                objAccountsService.AddtoUsersAccounts("Commission", Comissionamount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(Comissionamount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                //agentcommision
                                objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountAgent.ToString(), "0.00", Convert.ToInt32(CreatedbyID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(ComissionAmountAgent, Convert.ToInt32(CreatedbyID), AdminID, DateTime.Now, 0, false, "", false, Password);

                            }
                            else
                            {
                                objAccountsService.AddtoComissionAccounts(lstUserBets[0].Selectionname + " (" + username + ")", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                                objAccountsService.AddtoUsersAccounts("Commission", Comissionamount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(Comissionamount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                        }
                        objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + agentpercent.ToString() + "%) )", "0.00", AgentAmount.ToString(), CreatedbyID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                        // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + superpercent.ToString() + "%) )", "0.00", AgentAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), SuperAmount.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                        if (superID != 1)
                        {
                            Comissionamount = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper);
                            decimal finaladminforsuper = AdminAmount - Comissionamount;
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + SuperAmount.ToString() + "%) )", "0.00", SuperAmount1.ToString(), superID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser((SuperAmount1), superID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            // AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + SuperAmount.ToString() + "%) )", (AdminAmount).ToString(), "", superID, item.MarketId, DateTime.Now, Password);

                        }

                        if (CreatedbyID == 73)
                        {
                            AddCredittoUser(-1 * (AgentAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {

                            AddCredittoUser((AdminAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }

                        if (lstUserBets[0].TransferAdmin == false)
                        {
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (AdminAmount), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {
                            decimal transferadminamount1 = 0;
                            decimal AdminAmount1 = 0;
                            transferadminamount1 = Math.Round(((Convert.ToDecimal(transferadminpercentage) / 100) * TotalAmount) - Comissionamount1);
                            AdminAmount1 = AdminAmount - transferadminamount1;

                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (transferadminpercentage).ToString() + "%) )", "0.00", transferadminamount1.ToString(), lstUserBets[0].TransferAgentIDB.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount1.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (AdminAmount1), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            AddCredittoUser(-1 * (transferadminamount1), lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                            //objAccountsService.AddtoUsersAccounts("Transfer " + lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            //AddCredittoUser(-1 * (AdminAmount), lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //  AddCredittoUser(-1 * (AdminAmount), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        //  var resultsrefferer = GetReferrerIDandReferrerRatebyUserID(userID);
                        var lstresultsrefferer = GetReferrerRatesbyUserID(userID);
                        foreach (var resultsrefferer in lstresultsrefferer)
                        {

                            decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", CreatedbyID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser((RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), resultsrefferer.ReferrerID.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(resultsrefferer.ReferrerID.Value, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (RefferAmount), resultsrefferer.ReferrerID.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                        }
                        //if (resultsrefferer.ReferrerID > 0 && resultsrefferer.ReferrerRate > 0)
                        //{
                        //    decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", CreatedbyID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser((RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), resultsrefferer.ReferrerID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser(-1 * (RefferAmount), resultsrefferer.ReferrerID, AdminID, DateTime.Now, 0, false, "", false, Password);

                        //}
                    }
                    else
                    {
                        TotalAmount = -1 * TotalAmount;
                        int agentpercent = Convert.ToInt32(lstUserBets[0].AgentRate);
                        int superpercent = Convert.ToInt32(lstUserBets[0].SuperAgentRateB);
                        int samiadminpercent = Convert.ToInt32(lstUserBets[0].SamiAdminRate);
                        int transferadminpercentage = Convert.ToInt32(lstUserBets[0].TransferAdminPercentage);
                        // decimal ActualAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(objAPIConfigService.GetCommisionRate())) / 100) * TotalAmount), 2);
                        //decimal Comissionamount = Math.Round(((Convert.ToDecimal(Crypto.Decrypt(objAPIConfigService.GetCommisionRate())) / 100) * TotalAmount), 2);
                        //  decimal AgentAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(GetAgentRate(userID))) / 100) * TotalAmount), 2);
                        //decimal AdminAmount = Math.Round(TotalAmount - AgentAmount, 2);
                        decimal AgentAmount = 0;
                        decimal SuperAmount = 0;
                        decimal SuperAmount1 = 0;
                        decimal AdminAmount = 0;

                        if (CreatedbyID != 73)
                        {
                            SuperAmount = superpercent - agentpercent;
                            if (SuperAmount > 0)
                            {
                                SuperAmount1 = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * TotalAmount, 2);
                            }
                            else
                            {
                                SuperAmount1 = 0;
                            }
                            AgentAmount = Math.Round((Convert.ToDecimal(agentpercent) / 100) * TotalAmount, 2);
                            if (agentpercent == 100)
                            {
                                AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1, 2);

                            }
                            else
                            {
                                AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1, 2);
                            }

                        }
                        else
                        {
                            AdminAmount = Math.Round(TotalAmount, 2);
                        }
                        var results = objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname, "0.00", TotalAmount.ToString(), userID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                        AddCredittoUser(-1 * (TotalAmount), userID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        //  objAccountsService.AddtoComissionAccounts("CommissionAmount", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                        objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + agentpercent.ToString() + "%) )", AgentAmount.ToString(), "0.00", CreatedbyID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);

                        if (superID != 1)
                        {
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + SuperAmount.ToString() + "%) )", SuperAmount1.ToString(), "0.00", superID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (SuperAmount1), superID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            //AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + SuperAmount.ToString() + "%) )", "", (AdminAmount).ToString(), superID, item.MarketId, DateTime.Now, Password);

                        }


                        if (CreatedbyID == 73)
                        {
                            AddCredittoUser((AgentAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {

                            AddCredittoUser(-1 * (AdminAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        if (lstUserBets[0].TransferAdmin == false)
                        {
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(AdminAmount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {
                            decimal transferadminamount1 = 0;
                            decimal AdminAmount1 = 0;
                            transferadminamount1 = Math.Round((Convert.ToDecimal(transferadminpercentage) / 100) * TotalAmount);
                            AdminAmount1 = AdminAmount - transferadminamount1;

                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (transferadminpercentage).ToString() + "%) )", transferadminamount1.ToString(), "0.00", lstUserBets[0].TransferAgentIDB.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount1.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(AdminAmount1, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            AddCredittoUser(transferadminamount1, lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                            //objAccountsService.AddtoUsersAccounts("Transfer " + lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            //AddCredittoUser(AdminAmount, lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //  AddCredittoUser(AdminAmount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        // var resultsrefferer = GetReferrerIDandReferrerRatebyUserID(userID);
                        var lstresultsrefferer = GetReferrerRatesbyUserID(userID);
                        foreach (var resultsrefferer in lstresultsrefferer)
                        {
                            decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), CreatedbyID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", resultsrefferer.ReferrerID.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(resultsrefferer.ReferrerID.Value, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser((RefferAmount), resultsrefferer.ReferrerID.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                        }
                        //if (resultsrefferer.ReferrerID > 0 && resultsrefferer.ReferrerRate > 0)
                        //{
                        //    decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser(-1 * (RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", resultsrefferer.ReferrerID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser((RefferAmount), resultsrefferer.ReferrerID, AdminID, DateTime.Now, 0, false, "", false, Password);

                        //}
                    }
                    return true;
                }
                else
                {
                    return false;
                }

                //}

                return false;
            }
            catch (System.Exception ex)
            {
                APIConfig.WriteErrorToDB(ex.Message);
                APIConfig.LogError(ex);
                return false;
            }
        }
       
        public bool InsertUserAccountsFancyFig(string marketid, int selectionID, int userID, string Password, int ScoreforThisOver)
        {
            if (ValidatePassword(Password) == false)
            {
                return false;
            }
            try
            {
                List<SP_UserBets_GetDatabyUserIDandMArketID_Result> lstUserBets = dbEntities.SP_UserBets_GetDatabyUserIDandMArketID(userID, marketid).ToList<SP_UserBets_GetDatabyUserIDandMArketID_Result>();
                decimal WinnerTotal = 0;
                decimal WinnerDebit = 0;
                decimal WinnerCredit = 0;
                decimal LosserTotal = 0;
                decimal LosserDebit = 0;
                decimal LosserCredit = 0;
                decimal TotalAmount = 0;
                string Winnername = "";

                if (1 == 1)
                {
                    dbEntities.SP_UserBets_UpdateStatus(marketid, userID);

                    var lstBackBets = lstUserBets.Where(item1 => item1.BetType == "back" && item1.isMatched == true).ToList();
                    var lstLayBets = lstUserBets.Where(item1 => item1.BetType == "lay" && item1.isMatched == true).ToList();

                    Winnername = ScoreforThisOver.ToString();
                    int winnerscores = ScoreforThisOver;
                    //int winnerscore = winnerscores % 2;
                    foreach (var backbetitem in lstBackBets)
                    {
                        int odds = Convert.ToInt32(backbetitem.UserOdd);
                        double size = Convert.ToDouble(backbetitem.BetSize);
                        if (winnerscores == odds)
                        {
                            double itemamount = Convert.ToInt32(backbetitem.Amount);
                            double amount = ((size) / 100) * itemamount;
                            WinnerDebit += Convert.ToInt32(amount);
                        }
                        else
                        {
                            WinnerCredit += Convert.ToInt32(backbetitem.Amount);
                        }
                    }
                    foreach (var backbetitem in lstLayBets)
                    {
                        int aa = Convert.ToInt32(backbetitem.UserOdd);
                        double size = Convert.ToDouble(backbetitem.BetSize);
                        int odds = Convert.ToInt32(aa);
                        if (winnerscores == aa)
                        {
                            double itemamount = Convert.ToInt32(backbetitem.Amount);
                            double amount = ((size) / 100) * itemamount;
                            WinnerCredit += Convert.ToInt32(amount);
                           
                        }
                        else
                        {

                            WinnerDebit += Convert.ToInt32(backbetitem.Amount);
                        }
                    }


                    WinnerTotal = WinnerDebit - WinnerCredit;
                    if (LosserTotal < 0)
                    {
                        LosserTotal = -1 * (LosserTotal);
                    }
                    //  TotalAmount = WinnerTotal - LosserTotal;
                    TotalAmount = WinnerTotal;

                    int AdminID = 0;
                    int CreatedbyID = GetCreatedbyID1(userID);
                    int superID = GetCreatedbyID1(CreatedbyID);
                    if (superID == 1)
                    {
                        AdminID = 1;
                    }
                    if (superID != 1)
                    {
                        AdminID = GetCreatedbyID1(superID);
                    }

                    SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionAccountIDandBookAccountID = dbEntities.SP_Users_GetCommissionAccountIDandBookAccountID(AdminID).FirstOrDefault();

                    var usernameandtype = dbEntities.SP_Users_GetUSernamebyUserID(userID.ToString()).FirstOrDefault();
                    string username = Crypto.Decrypt(usernameandtype.Username);
                    string EventTypename = "FancyFigure";
                    var eventdetails = dbEntities.SP_UserMarket_GetEventDetailsbyMarketID(marketid).FirstOrDefault();
                    //if (item.Runners.Count == 1)
                    //{
                    //    EventTypename = "Fancy";
                    //}
                    if (TotalAmount >= 0)
                    {
                        // int agentpercent = Convert.ToInt32(Crypto.Decrypt(GetAgentRate(userID)));
                        int agentpercent = Convert.ToInt32(lstUserBets[0].AgentRate);
                        int superpercent = Convert.ToInt32(lstUserBets[0].SuperAgentRateB);
                        int samiadminpercent = Convert.ToInt32(lstUserBets[0].SamiAdminRate);
                        int transferadminpercentage = Convert.ToInt32(lstUserBets[0].TransferAdminPercentage);
                        decimal ActualAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(GetCommissionRatebyUserID(userID)) / 100) * TotalAmount), 2);
                        decimal Comissionamount = 0;
                        decimal ComissionAmountAgent = 0;
                        decimal ComissionAmountSuper = 0;
                        // if (item.Runners.Count == 1)
                        // {
                        Comissionamount = Math.Round(((Convert.ToDecimal(GetCommissionRatebyUserIDFancy(userID)) / 100) * TotalAmount), 2);
                        //}
                        //else
                        //{
                        //    Comissionamount = Math.Round(((Convert.ToDecimal(GetCommissionRatebyUserID(userID)) / 100) * TotalAmount), 2);
                        //}
                        decimal AgentAmount = 0;
                        decimal SuperAmount = 0;
                        decimal SuperAmount1 = 0;
                        decimal AdminAmount = 0;
                        bool TransferAgentCommision = GetTransferAgnetCommision(CreatedbyID);

                        if (CreatedbyID != 73)
                        {
                            SuperAmount = superpercent - agentpercent;
                            if (SuperAmount > 0)
                            {
                                SuperAmount1 = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * TotalAmount, 2);
                            }
                            else
                            {
                                SuperAmount1 = 0;
                            }
                            AgentAmount = Math.Round((Convert.ToDecimal(agentpercent) / 100) * TotalAmount, 2);
                            if (agentpercent == 100)
                            {
                                AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1, 2);

                            }
                            else
                            {
                                AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1 - Comissionamount, 2);
                            }

                        }
                        else
                        {
                            AdminAmount = Math.Round(TotalAmount - Comissionamount, 2);
                        }

                        //GetCommissionRatebyUserIDFancy(userID).ToString()
                        var results = objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname, TotalAmount.ToString(), "0.00", userID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                        AddCredittoUser(TotalAmount, userID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        if (usernameandtype.UserTypeID != 4)
                        {

                            objAccountsService.AddtoUsersAccounts("Commission", "0.00", Comissionamount.ToString(), userID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (Comissionamount), userID, AdminID, DateTime.Now, 0, false, "", false, Password);


                            if (TransferAgentCommision == true && agentpercent > 0)
                            {
                                if (SuperAmount > 0)
                                {
                                    ComissionAmountSuper = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * Comissionamount, 2);

                                    //supercommision
                                    objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountSuper.ToString(), "0.00", Convert.ToInt32(superID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                    AddCredittoUser(ComissionAmountSuper, Convert.ToInt32(superID), AdminID, DateTime.Now, 0, false, "", false, Password);

                                }
                                else
                                {
                                    ComissionAmountSuper = 0;
                                }
                                ComissionAmountAgent = Math.Round((Convert.ToDecimal(agentpercent) / 100) * Comissionamount, 2);

                                Comissionamount = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper);

                                objAccountsService.AddtoComissionAccounts(lstUserBets[0].Selectionname + " (" + username + ")", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                                objAccountsService.AddtoUsersAccounts("Commission", Comissionamount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(Comissionamount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                                //agentcommision
                                objAccountsService.AddtoUsersAccounts("Commission", ComissionAmountAgent.ToString(), "0.00", Convert.ToInt32(CreatedbyID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(ComissionAmountAgent, Convert.ToInt32(CreatedbyID), AdminID, DateTime.Now, 0, false, "", false, Password);

                            }
                            else
                            {
                                objAccountsService.AddtoComissionAccounts(lstUserBets[0].Selectionname + " (" + username + ")", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                                objAccountsService.AddtoUsersAccounts("Commission", Comissionamount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                                AddCredittoUser(Comissionamount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.CommisionAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            }
                        }
                        objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + agentpercent.ToString() + "%) )", "0.00", AgentAmount.ToString(), CreatedbyID, marketid, DateTime.Now, agentpercent.ToString(), SuperAmount.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                        // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + superpercent.ToString() + "%) )", "0.00", AgentAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, agentpercent.ToString(), SuperAmount.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                        if (superID != 1)
                        {
                            Comissionamount = Comissionamount - (ComissionAmountAgent + ComissionAmountSuper);
                            decimal finaladminforsuper = AdminAmount - Comissionamount;
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + SuperAmount.ToString() + "%) )", "0.00", SuperAmount1.ToString(), superID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser((finaladminforsuper), superID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            // AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + SuperAmount.ToString() + "%) )", (AdminAmount).ToString(), "", superID, item.MarketId, DateTime.Now, Password);

                        }

                        if (CreatedbyID == 73)
                        {
                            AddCredittoUser(-1 * (AgentAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {

                            AddCredittoUser((AdminAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }

                        if (lstUserBets[0].TransferAdmin == false)
                        {
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (AdminAmount), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {
                            decimal transferadminamount1 = 0;
                            decimal AdminAmount1 = 0;
                            transferadminamount1 = Math.Round((Convert.ToDecimal(transferadminpercentage) / 100) * TotalAmount);
                            AdminAmount1 = AdminAmount - transferadminamount1;

                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (transferadminpercentage).ToString() + "%) )", "0.00", transferadminamount1.ToString(), lstUserBets[0].TransferAgentIDB.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount1.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (AdminAmount1), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            AddCredittoUser(-1 * (transferadminamount1), lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                            //objAccountsService.AddtoUsersAccounts("Transfer " + lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            //AddCredittoUser(-1 * (AdminAmount), lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", "0.00", AdminAmount.ToString(), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //  AddCredittoUser(-1 * (AdminAmount), Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        //  var resultsrefferer = GetReferrerIDandReferrerRatebyUserID(userID);
                        var lstresultsrefferer = GetReferrerRatesbyUserID(userID);
                        foreach (var resultsrefferer in lstresultsrefferer)
                        {

                            decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", CreatedbyID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser((RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), resultsrefferer.ReferrerID.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(resultsrefferer.ReferrerID.Value, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (RefferAmount), resultsrefferer.ReferrerID.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                        }
                        //if (resultsrefferer.ReferrerID > 0 && resultsrefferer.ReferrerRate > 0)
                        //{
                        //    decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", CreatedbyID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser((RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), resultsrefferer.ReferrerID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser(-1 * (RefferAmount), resultsrefferer.ReferrerID, AdminID, DateTime.Now, 0, false, "", false, Password);

                        //}
                    }
                    else
                    {
                        TotalAmount = -1 * TotalAmount;
                        int agentpercent = Convert.ToInt32(lstUserBets[0].AgentRate);
                        int superpercent = Convert.ToInt32(lstUserBets[0].SuperAgentRateB);
                        int samiadminrpercent = Convert.ToInt32(lstUserBets[0].SamiAdminRate);
                        int transferadminpercentage = Convert.ToInt32(lstUserBets[0].TransferAdminPercentage);
                        // decimal ActualAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(objAPIConfigService.GetCommisionRate())) / 100) * TotalAmount), 2);
                        //decimal Comissionamount = Math.Round(((Convert.ToDecimal(Crypto.Decrypt(objAPIConfigService.GetCommisionRate())) / 100) * TotalAmount), 2);
                        //  decimal AgentAmount = Math.Round(TotalAmount - ((Convert.ToDecimal(Crypto.Decrypt(GetAgentRate(userID))) / 100) * TotalAmount), 2);
                        //decimal AdminAmount = Math.Round(TotalAmount - AgentAmount, 2);
                        decimal AgentAmount = 0;
                        decimal SuperAmount = 0;
                        decimal SuperAmount1 = 0;
                        decimal AdminAmount = 0;

                        if (CreatedbyID != 73)
                        {
                            SuperAmount = superpercent - agentpercent;
                            if (SuperAmount > 0)
                            {
                                SuperAmount1 = Math.Round((Convert.ToDecimal(SuperAmount) / 100) * TotalAmount, 2);
                            }
                            else
                            {
                                SuperAmount1 = 0;
                            }
                            AgentAmount = Math.Round((Convert.ToDecimal(agentpercent) / 100) * TotalAmount, 2);
                            if (agentpercent == 100)
                            {
                                AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1, 2);

                            }
                            else
                            {
                                AdminAmount = Math.Round(TotalAmount - AgentAmount - SuperAmount1, 2);
                            }

                        }
                        else
                        {
                            AdminAmount = Math.Round(TotalAmount, 2);
                        }
                        
                        var results = objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname, "0.00", TotalAmount.ToString(), userID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(),samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(userID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                        AddCredittoUser(-1 * (TotalAmount), userID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        //  objAccountsService.AddtoComissionAccounts("CommissionAmount", Comissionamount.ToString(), Convert.ToInt64(results), DateTime.Now);
                        objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + agentpercent.ToString() + "%) )", AgentAmount.ToString(), "0.00", CreatedbyID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);

                        if (superID != 1)
                        {
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + SuperAmount.ToString() + "%) )", SuperAmount1.ToString(), "0.00", superID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (AdminAmount), superID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            //AddAdminAmountForSuper(lstUserBets[0].Marketbookname + " (" + username + " (" + SuperAmount.ToString() + "%) )", "", (AdminAmount).ToString(), superID, item.MarketId, DateTime.Now, Password);

                        }


                        if (CreatedbyID == 73)
                        {
                            AddCredittoUser((AgentAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {

                            AddCredittoUser(-1 * (AdminAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        if (lstUserBets[0].TransferAdmin == false)
                        {
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(AdminAmount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        else
                        {
                            decimal transferadminamount1 = 0;
                            decimal AdminAmount1 = 0;
                            transferadminamount1 = Math.Round((Convert.ToDecimal(transferadminpercentage) / 100) * TotalAmount);
                            AdminAmount1 = AdminAmount - transferadminamount1;

                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (transferadminpercentage).ToString() + "%) )", transferadminamount1.ToString(), "0.00", lstUserBets[0].TransferAgentIDB.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount1.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(AdminAmount1, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                            AddCredittoUser(transferadminamount1, lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                            //objAccountsService.AddtoUsersAccounts("Transfer " + lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", lstUserBets[0].TransferAgentIDB.Value, item.MarketId, DateTime.Now, agentpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            //AddCredittoUser(AdminAmount, lstUserBets[0].TransferAgentIDB.Value, AdminID, DateTime.Now, 0, false, "", false, Password);
                        }
                        // objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + (100 - agentpercent).ToString() + "%) )", AdminAmount.ToString(), "0.00", Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(AdminID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //  AddCredittoUser(AdminAmount, Convert.ToInt32(objCommissionAccountIDandBookAccountID.BookAccountID), AdminID, DateTime.Now, 0, false, "", false, Password);
                        // var resultsrefferer = GetReferrerIDandReferrerRatebyUserID(userID);
                        var lstresultsrefferer = GetReferrerRatesbyUserID(userID);
                        foreach (var resultsrefferer in lstresultsrefferer)
                        {
                            decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), CreatedbyID, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser(-1 * (RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                            objAccountsService.AddtoUsersAccounts(lstUserBets[0].Selectionname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", resultsrefferer.ReferrerID.Value, marketid, DateTime.Now, agentpercent.ToString(), superpercent.ToString(), samiadminrpercent.ToString(), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(resultsrefferer.ReferrerID.Value, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName, lstUserBets[0].Marketbookname);
                            AddCredittoUser((RefferAmount), resultsrefferer.ReferrerID.Value, AdminID, DateTime.Now, 0, false, "", false, Password);

                        }
                        //if (resultsrefferer.ReferrerID > 0 && resultsrefferer.ReferrerRate > 0)
                        //{
                        //    decimal RefferAmount = Math.Round((Convert.ToDecimal(resultsrefferer.ReferrerRate) / 100) * TotalAmount, 2);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", "0.00", RefferAmount.ToString(), CreatedbyID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser(-1 * (RefferAmount), CreatedbyID, AdminID, DateTime.Now, 0, false, "", false, Password);
                        //    objAccountsService.AddtoUsersAccounts(lstUserBets[0].Marketbookname + " (" + username + " (" + resultsrefferer.ReferrerRate.ToString() + "%) )", RefferAmount.ToString(), "0.00", resultsrefferer.ReferrerID, item.MarketId, DateTime.Now, Crypto.Decrypt(GetAgentRate(userID)), GetCommissionRatebyUserIDFancy(userID).ToString(), Convert.ToDecimal(GetCurrentBalancebyUser(CreatedbyID, Password)), false, EventTypename, Winnername, eventdetails.EventID, eventdetails.EventName);
                        //    AddCredittoUser((RefferAmount), resultsrefferer.ReferrerID, AdminID, DateTime.Now, 0, false, "", false, Password);

                        //}
                    }
                    return true;
                }
                else
                {
                    return false;
                }

                //}

                return false;
            }
            catch (System.Exception ex)
            {
                APIConfig.WriteErrorToDB(ex.Message);
                APIConfig.LogError(ex);
                return false;
            }
        }

        public decimal GetProfitorLossbyUserID(int userID, bool isCreditAmount, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return 0;
            }
            return dbEntities.SP_UserAccounts_GetDatabyUserIDProfitandLoss(userID, isCreditAmount).FirstOrDefault().Value;
        }

        public decimal GetProfitorLossforSuper(int userID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return 0;
            }
            return dbEntities.SP_GetAdminPLForSuper1(userID).FirstOrDefault().Value;
        }
        public string GetMarketsOpenedbyUser(int userID)
        {
            var results = dbEntities.SP_UserMarket_GetMarketCatalogueIDsOpendByUser(userID, "").ToList<SP_UserMarket_GetMarketCatalogueIDsOpendByUser_Result>();
            if (results.Count > 0)
            {
                return ConverttoJSONString(results);
            }
            else
            {
                return "";
            }

        }
        public string GetMarketsOpenedbyUsersofAgent(int AgentID)
        {
            var results = dbEntities.SP_UserMarket_GetMarketCatalogueIDsOpendByUsersofAgent(AgentID).ToList<SP_UserMarket_GetMarketCatalogueIDsOpendByUsersofAgent_Result>();
            if (results.Count > 0)
            {
                return ConverttoJSONString(results);
            }
            else
            {
                return "";
            }

        }

        public void SetMarketBookOpenbyUSer(int userID, string MarketBookID)
        {
            dbEntities.SP_UserMarket_SetMarketCatalogueIDOpendByUser(userID, MarketBookID);

        }
        public string SetMarketBookOpenbyUSerandGet(int userID, string MarketBookID)
        {
            var results = dbEntities.SP_UserMarket_SetMarketCatalogueIDOpendByUserandGet(userID, MarketBookID).ToList<SP_UserMarket_SetMarketCatalogueIDOpendByUserandGet_Result>();
            return ConverttoJSONString(results);
        }
        public string SetMarketBookOpenbyUSerandGet0(int userID, string MarketBookID)
        {
            var results = dbEntities.SP_UserMarket_SetMarketCatalogueIDOpendByUserandGet(userID, MarketBookID).ToList<SP_UserMarket_SetMarketCatalogueIDOpendByUserandGet_Result>();
            return ConverttoJSONString(results);
        }
        public string SetMarketBookOpenbyUSerandGet1(int userID, string MarketBookID)
        {
            var results = dbEntities.SP_UserMarket_SetMarketCatalogueIDOpendByUserandGet(userID, MarketBookID).ToList<SP_UserMarket_SetMarketCatalogueIDOpendByUserandGet_Result>();
            return ConverttoJSONString(results);
        }
        public string SetMarketBookOpenbyUSerandGet2(int userID, string MarketBookID)
        {
            var results = dbEntities.SP_UserMarket_SetMarketCatalogueIDOpendByUserandGet(userID, MarketBookID).ToList<SP_UserMarket_SetMarketCatalogueIDOpendByUserandGet_Result>();
            return ConverttoJSONString(results);
        }
        public void SetMarketBookClosedbyUser(int userID, string MarketBookID)
        {
            dbEntities.SP_UserMarket_SetMarketCatalogueIDClosedByUser(userID, MarketBookID);
        }
        public void SetMarketClosedAllUsers(string MarketbookID)
        {
            dbEntities.SP_UserMarket_SetMarketCatalogueIDClosed(MarketbookID);
        }
        public string GetAccountsDatabyUserIDandDateRange(int userID, string From, string To, bool isCreditAmount, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            return ConverttoJSONString(objAccountsService.GetAccountsDatabyUserIDandDateRange(userID, From, To, isCreditAmount));
        }
        public string GetAccountsDatabyCreatedByID(int userID, bool isCreditAmount, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            return ConverttoJSONString(objAccountsService.GetAccountsDatabyCreatedbyID(userID, isCreditAmount));
        }

        public string GetAccountsDatabyCreatedByIDForSuper(int userID, bool isCreditAmount, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            return ConverttoJSONString(objAccountsService.GetAccountsDatabyCreatedByIDForSuper(userID, isCreditAmount));
        }
        public string GetAccountsDatabyCreatedByIDForSamiAdmin(int userID, bool isCreditAmount, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            return ConverttoJSONString(objAccountsService.GetAccountsDatabyCreatedByIDForSamiAdmin(userID, isCreditAmount));
        }
        
        public string GetAccountsDataForAdmin(int UserID, bool isCreditAmount, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            return ConverttoJSONString(objAccountsService.GetAccountsDatabyAdmin(UserID, isCreditAmount));
        }

        public string GetAccountsDataForSuper(int UserID, bool isCreditAmount, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            return ConverttoJSONString(objAccountsService.GetAccountsDataForSuper(UserID, isCreditAmount));
        }
        public string GetAccountsDataForSamiAdmin(int UserID, bool isCreditAmount, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            return ConverttoJSONString(objAccountsService.GetAccountsDataForSamiAdmin(UserID, isCreditAmount));
        }
        
        public string GetAccountsDataForCommisionaccount(string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            return ConverttoJSONString(objAccountsService.GetAccountsDatabyCommision());
        }

        public string GetInPlayMatches(int userID)
        {
            var results = dbEntities.SP_UserBets_GetInPlayMatchesbyUserID(userID).ToList<SP_UserBets_GetInPlayMatchesbyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public string GetInPlayMatcheswithRunners(int userID)
        {
            var results = dbEntities.SP_UserBets_GetInPlayMatcheswithRunnersbyUserID(userID).ToList<SP_UserBets_GetInPlayMatcheswithRunnersbyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public string GetInPlayMatcheswithRunners1(int userID)
        {
            var results = dbEntities.SP_UserBets_GetInPlayMatcheswithRunnersbyUserID1(userID).ToList<SP_UserBets_GetInPlayMatcheswithRunnersbyUserID1_Result>();
            return ConverttoJSONString(results);
        }

        //public string GetInPlayMatcheswithRunners2(int userID)
        //{
        //    var results = dbEntities.SP_UserBets_GetInPlayMatcheswithRunnersbyUserID1(userID).ToList<SP_UserBets_GetInPlayMatcheswithRunnersbyUserID1_Result>();
        //    return ConverttoJSONString(results);
        //}

       

        public string GetAllMatches(int userID)
        {
            var results = dbEntities.SP_UserBets_GetAllMatchesbyUserID(userID).ToList<SP_UserBets_GetAllMatchesbyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public string GetAllCricketMatches(int userID)
        {
            var results = dbEntities.SP_UserBets_GetAllMatchesbyUserID(userID).ToList<SP_UserBets_GetAllMatchesbyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public string GetAllSoccerMatches(int userID)
        {
            var results = dbEntities.SP_UserBets_GetAllMatchesbyUserID(userID).ToList<SP_UserBets_GetAllMatchesbyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public string GetAllTennisMatches(int userID)
        {
            var results = dbEntities.SP_UserBets_GetAllMatchesbyUserID(userID).ToList<SP_UserBets_GetAllMatchesbyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public string GetUserBetsbyAgentID(int AgentID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserBets_GetDatabyAgentID(AgentID).ToList<SP_UserBets_GetDatabyAgentID_Result>();
            return ConverttoJSONString(results);
        }


        public string GetUserBetsbySuperID(int SuperID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserBets_GetDatabySuperID2(SuperID).ToList<SP_UserBets_GetDatabySuperID2_Result>();
            return ConverttoJSONString(results);
        }

        public string GetUserBetsbySamiAdmin(int SuperID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserBets_GetDatabySamiAdminByID1(SuperID).ToList<SP_UserBets_GetDatabySamiAdminByID1_Result>();
            return ConverttoJSONString(results);
        }


        public string GetUserBetsbyAgentIDwithZeroReferer(int AgentID, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserBets_GetDatabyAgentIDWithRefererZero(AgentID).ToList<SP_UserBets_GetDatabyAgentIDWithRefererZero_Result>();
            return ConverttoJSONString(results);
        }
        public string GetCompletedMatchedBetsbyUserID(int UserID, string MarketbookID)
        {
            var results = dbEntities.SP_UserBets_GetCompletedMatchedBetsbyUserID(UserID, MarketbookID).ToList<SP_UserBets_GetCompletedMatchedBetsbyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public string GetUserStatus(int UserID)
        {
            var results = dbEntities.SP_Users_GetBlockStatus(UserID).First<SP_Users_GetBlockStatus_Result>();
            return ConverttoJSONString(results);
        }
        public string GetLastLoginTimes(int UserID)
        {
            var results = dbEntities.SP_Users_GetLastLoginTimes(UserID).ToList<SP_Users_GetLastLoginTimes_Result>();
            return ConverttoJSONString(results);

        }
        public bool GetShowTV(int userID)
        {
            return dbEntities.SP_BetPlaceWaitandInterval_GetShowTV(userID).FirstOrDefault().Value;
        }

        public void UpdateShowTV(int userID, bool ShowTV)
        {
            dbEntities.SP_BetPlaceWaitandInterval_UpdateShowTV(userID, ShowTV);
        }
        public string ConverttoJSONString(object result)
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

        public string GetUserbetsForAdmin(string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserBets_GetDataForAdmin().ToList<SP_UserBets_GetDataForAdmin_Result>();
            return ConverttoJSONString(results);
        }

        public string GetMarketsOpenedbyUsersForAdmin()
        {
            var results = dbEntities.SP_UserMarket_GetMarketCatalogueIDsOpendByUsersforAdmin().ToList<SP_UserMarket_GetMarketCatalogueIDsOpendByUsersforAdmin_Result>();
            return ConverttoJSONString(results);
        }
        public void UpdateBetSizebyID(long ID, string BetSize, string Password)
        {
            if (ValidatePassword(Password) == true)
            {


                dbEntities.SP_UserBets_UpdateBetSizebyID(ID, BetSize);
            }
        }
        public string GetSheetNamebyMarketID(string marketbookID)
        {
            return dbEntities.SP_UserBets_GetSheetNamebyMarketID(marketbookID).FirstOrDefault().ToString();
        }
        public string GetUnMatchedBets(string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserBets_GetUnMatchedBets().ToList<SP_UserBets_GetUnMatchedBets_Result>();
            return ConverttoJSONString(results);
        }
        public void UpdateCheckConditionforPlaceBet(int UserID, bool CheckConditionforPlaceBet)
        {
            dbEntities.SP_Users_UpdateCheckConditionsforPlaceBet(UserID, CheckConditionforPlaceBet);
        }

        public string GetDatabyAgentIDForCommisionandDateRange(int UserID, string From, string To, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }

            return ConverttoJSONString(dbEntities.SP_UserAccounts_GetDatabyAgentIDForCommisionandDateRange(UserID, false, From, To).FirstOrDefault());
            //return ConverttoJSONString(results);
        }
       
        public string GetDatabyAgentIDForCommisionandDateRangeByEventtype(int UserID, string From, string To, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserAccounts_GetDatabyAgentIDForCommisionbyEventtype(UserID, false, From, To).ToList<SP_UserAccounts_GetDatabyAgentIDForCommisionbyEventtype_Result>();
            return ConverttoJSONString(results);
        }
        public string GetDistinctmarketopened()
        {
            return dbEntities.SP_UserMarket_GetDistinctMarketsOpened().FirstOrDefault();
        }
        public void UpdateOddsData(string oddsdata, string Oddtype)
        {
            dbEntities.SP_OddsData_Update(oddsdata, Oddtype);
        }
        public void UpdateUserPhoneandNamebyUserId(int userId, string Name, string Phone)
        {
            dbEntities.SP_Users_UpdateNameandPhonebyUserID(userId, Name, Phone);
        }
        public string GetAccountsDatabyEventtypeuserIDandDateRange(int UserID, string From, string To, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserAccounts_GetDatabyEventTypeandDateRange(UserID, false, From, To).ToList<SP_UserAccounts_GetDatabyEventTypeandDateRange_Result>();
            return ConverttoJSONString(results);
        }

        public string UserAccountsGetCommission(int UserID, string From, string To, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserAccounts_GetCommission(UserID, false, From, To).ToList<SP_UserAccounts_GetCommission_Result>();
            return ConverttoJSONString(results);
        }
        public string GetAccountsDatabyEventNameuserIDandDateRange(int UserID, string From, string To, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserAccounts_GetDatabyEventNameandDateRange(UserID, false, From, To).ToList<SP_UserAccounts_GetDatabyEventNameandDateRange_Result>();
            return ConverttoJSONString(results);
        }
        public string GetAccountsDatabyEventNameuserIDandDateRangeFancywithMArketName(int UserID, string From, string To, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return "";
            }
            var results = dbEntities.SP_UserAccounts_GetDatabyEventNameandDateRangeFancywithMarketName(UserID, false, From, To).ToList<SP_UserAccounts_GetDatabyEventNameandDateRangeFancywithMarketName_Result>();
            return ConverttoJSONString(results);
        }
        public string GetEventTypeNamebyMarketID(string marketBookId)
        {
            return dbEntities.SP_UserMarket_GetEventTypeNamebyMarketID(marketBookId).FirstOrDefault().ToString();
        }
        public string GetAllowedMarketsbyUserID(int UserID)
        {
            var results = dbEntities.SP_Users_GetAllowedMarketsbyUserID(UserID).FirstOrDefault<SP_Users_GetAllowedMarketsbyUserID_Result>();
            return ConverttoJSONString(results);
        }
        public void UpdateAllowedMarketsbyUserID(bool isCricketMatchOddsAllowedForBet, bool isCricketTiedMatchAllowedForBet, bool isCricketCompletedMatchAllowedForBet, bool isCricketInningsRunsAllowedForBet, bool isSoccerAllowedForBet, bool isTennisAllowedForBet, bool isHorseRaceWinAllowedForBet, bool isHorseRacePlaceAllowedForBet, bool isGrayHoundRaceWinAllowedForBet, bool isGrayHoundRacePlaceAllowedForBet, int UserID, bool isWinnerMarketAllowedForBet, string Password, bool isFancyAllowed)
        {
            if (ValidatePassword(Password) == true)
            {


                dbEntities.SP_Users_UpdateAllowedMarketsbyUserID(UserID, isCricketMatchOddsAllowedForBet, isCricketTiedMatchAllowedForBet, isCricketCompletedMatchAllowedForBet, isCricketInningsRunsAllowedForBet, isSoccerAllowedForBet, isTennisAllowedForBet, isHorseRaceWinAllowedForBet, isHorseRacePlaceAllowedForBet, isGrayHoundRaceWinAllowedForBet, isGrayHoundRacePlaceAllowedForBet, isWinnerMarketAllowedForBet, isFancyAllowed);
            }
        }
        public SP_Users_GetCommissionAccountIDandBookAccountID_Result GetCommissionaccountIdandBookAccountbyUserID(int UserID)
        {
            return dbEntities.SP_Users_GetCommissionAccountIDandBookAccountID(UserID).FirstOrDefault<SP_Users_GetCommissionAccountIDandBookAccountID_Result>();

        }
        public SP_Users_GetReferrerRateandReferrerIDbyUserID_Result GetReferrerRateandIDbyUserID(int UserID)
        {
            return dbEntities.SP_Users_GetReferrerRateandReferrerIDbyUserID(UserID).FirstOrDefault();
        }
        public void UpdateRefererRateandIDbyUserID(int UserID, int ReffereID, int ReferrerRate)
        {
            dbEntities.SP_Users_UpdateReferrerRateandReferrerIDbyUserID(UserID, ReffereID, ReferrerRate);
        }
        public void UpdateUsersAllBlock()
        {
            dbEntities.SP_Users_BlockUnBlockAllEndUsersandAgents();
        }
        public void UpdateUsersAllLoggedOut()
        {
            dbEntities.SP_Users_LoggedOutAllEndUsersandAgents();
        }
        public string GetIntervalandBetPlaceTimings(int UserID)
        {
            return ConverttoJSONString(dbEntities.SP_BetPlaceWaitandInterval_GetAllData(UserID).FirstOrDefault());
        }
        public void UpdateIntervalandBetPlaceTimings(int HorseRaceTimerInterval, int HorseRaceBetPlaceWait, int GrayHoundTimerInterval, int GrayHoundBetPlaceWait, int CricketMatchOddsTimerInterval, int CricketMatchOddsBetPlaceWait, int CompletedMatchTimerInterval, int CompletedMatchBetPlaceWait, int TiedMatchTimerInterval, int TiedMatchBetPlaceWait, int InningsRunsTimerInterval, int InningsRunsBetPlaceWait, int WinnerTimerInterval, int WinnerBetPlaceWait, int TennisTimerInterval, int TennisBetPlaceWait, int SoccerTimerInterval, int SoccerBetPlaceWait, decimal PoundRate, int userID, int FancyTimerInterval, int FancyBetPlaceWait, int RaceMinutesBeforeStart, int CancelBetTime)
        {
            dbEntities.SP_BetPlaceWaitandInterval_Update(HorseRaceTimerInterval, HorseRaceBetPlaceWait, GrayHoundTimerInterval, GrayHoundBetPlaceWait, CricketMatchOddsTimerInterval, CricketMatchOddsBetPlaceWait, CompletedMatchTimerInterval, CompletedMatchBetPlaceWait, TiedMatchTimerInterval, TiedMatchBetPlaceWait, InningsRunsTimerInterval, InningsRunsBetPlaceWait, WinnerTimerInterval, WinnerBetPlaceWait, TennisTimerInterval, TennisBetPlaceWait, SoccerTimerInterval, SoccerBetPlaceWait, PoundRate, userID, FancyTimerInterval, FancyBetPlaceWait, RaceMinutesBeforeStart, CancelBetTime);
        }
        public int GetHawalaAccountIDbyUserID(int UserID)
        {
            return Convert.ToInt32(dbEntities.SP_Users_GetHwalaAccountIDbyUserID(UserID).FirstOrDefault());
        }
        public int GetCreatedbyID(int UserID)
        {
            return Convert.ToInt32(dbEntities.SP_Get_CreatrdbyID(UserID).FirstOrDefault());     
        }

        public int GetCreatedbyID1(int UserID)
        {
            var result = dbEntities.SP_Users_GetCreatedbyIDbyUserID(UserID).FirstOrDefault<SP_Users_GetCreatedbyIDbyUserID_Result>();
            return result.CreatedbyID;
        }

        public bool GetIsComAllowbyUserID(int UserID)
        {
            return dbEntities.SP_Users_GetIsComAllowbyUserIDNew(UserID).FirstOrDefault().Value;
               
        }
        public void UpdateHawalaIDbyUserID(int userID, int ParentID)
        {
            dbEntities.SP_Users_UpdateHwalaAccountIDbyUserID(userID, ParentID);
        }
        public string GetMarketsforBettingAllowed(int userID)
        {
            return ConverttoJSONString(dbEntities.SP_UserMarket_GetMarketForAllowedBetting(userID).ToList());
        }
        public void UpdateMarketAllowedBetting(int UserId, string MarketbookId, bool AllowedBetting)
        {
            dbEntities.SP_UserMarket_UpdateMarketForAllowedBetting(UserId, MarketbookId, AllowedBetting);
        }
        public void UpdateMarketAllowedBettingForAllAgents(List<int> UserIds, List<SP_UserMarket_GetMarketForAllowedBetting_Result> lstMarkets)
        {
            try
            {
                APIConfig.isUpdatingBettingAllowed = false;
                foreach (var userid in UserIds)
                {
                    foreach (var marketitem in lstMarkets)
                    {
                        dbEntities.SP_UserMarket_UpdateMarketForAllowedBetting(userid, marketitem.MarketCatalogueID, marketitem.BettingAllowed);
                    }
                }
                APIConfig.isUpdatingBettingAllowed = false;
            }
            catch (System.Exception ex)
            {
                APIConfig.isUpdatingBettingAllowed = false;
            }


        }
        public string GetBetSlipKeys(int UserID)
        {
            return ConverttoJSONString(dbEntities.SP_BetSlipKeys_Get(UserID).FirstOrDefault());
        }
        public void UpdateBetSlipKeys(int UserID, string SimpleBtn1, string SimpleBtn2, string SimpleBtn3, string SimpleBtn4, string SimpleBtn5, string SimpleBtn6, string SimpleBtn7, string SimpleBtn8, string SimpleBtn9, string SimpleBtn10, string SimpleBtn11, string SimpleBtn12, string MutipleBtn1, string MutipleBtn2, string MutipleBtn3, string MutipleBtn4, string MutipleBtn5, string MutipleBtn6, string MutipleBtn7, string MutipleBtn8, string MutipleBtn9, string MutipleBtn10, string MutipleBtn11, string MutipleBtn12)
        {
            dbEntities.SP_BetSlipKeys_Update(UserID, SimpleBtn1, SimpleBtn2, SimpleBtn3, SimpleBtn4, SimpleBtn5, SimpleBtn6, SimpleBtn7, SimpleBtn8, SimpleBtn9, SimpleBtn10, SimpleBtn11, SimpleBtn12, MutipleBtn1, MutipleBtn2, MutipleBtn3, MutipleBtn4, MutipleBtn5, MutipleBtn6, MutipleBtn7, MutipleBtn8, MutipleBtn9, MutipleBtn10, MutipleBtn11, MutipleBtn12);
        }

        public void UpdateBettingAllowed(string EventID, string BettingAllowed)
        {
            dbEntities.SP_BettingAllowed_Update(EventID, BettingAllowed);
        }

        public decimal GetPoundRatebyUserID(int UserID)
        {
            return dbEntities.SP_BetPlaceWaitandInterval_GetPoundRate(UserID).FirstOrDefault().Value;
        }

        public string GetMarqueeText()
        {
            return dbEntities.SP_APIConfig_GetMarqueeText().FirstOrDefault().ToString();
        }
        public void UpdateMarqueeText(string marqueetext)
        {
            dbEntities.SP_APIConfig_UpdateMarqueeText(marqueetext);
        }
        public void SendEmailBalanceSheetToClient(SP_EMAILConfig_GetData_Result objEmailConfig, string BalanceSheetDocPath, string Password)
        {
            if (ValidatePassword(Password) == true)
            {
                bool flag = false;

                MailMessage mailMsg = new MailMessage();

                MailAddress mailAddress = null;

                try

                {
                    // To

                    mailMsg.To.Add(objEmailConfig.ToEmail);
                    mailAddress = new MailAddress(objEmailConfig.FromEmail);

                    mailMsg.From = mailAddress;
                    mailMsg.Subject = "Balance sheet for " + DateTime.Now.Date.ToShortDateString();

                    mailMsg.Body = "Balance sheet for " + DateTime.Now.Date.ToShortDateString();

                    mailMsg.Attachments.Add(new Attachment(BalanceSheetDocPath));
                    var smtpClient = new SmtpClient(objEmailConfig.SMTP, Convert.ToInt32(objEmailConfig.PortNum))
                    {
                        Credentials = new NetworkCredential(objEmailConfig.FromEmail, objEmailConfig.FromPassword),
                        EnableSsl = true
                    };

                    smtpClient.Send(mailMsg);
                    flag = true;

                    //   MyLogEvent.WriteEntry("Mail Send Successfully");

                }
                catch (Exception ex)

                {
                    APIConfig.WriteErrorToDB(ex.ToString() + "---" + ex.StackTrace.ToString());
                    APIConfig.LogError(ex);

                    // MyLogEvent.WriteEntry("Error occured");

                    //Response.Write(ex.Message);

                }
                finally

                {
                    mailMsg = null;

                    mailAddress = null;

                }
            }
        }

        public void SendBalanceSheettoEmailAutomatic(string Password)
       {
            if (ValidatePassword(Password) == true)
            {
                try
                {
                    var emailconfigResAll = dbEntities.SP_EMAILConfig_GetData();
                    DateTime currentdatetime = DateTime.Now;
                    foreach (var emailconfigRes in emailconfigResAll)
                    {
                        if (emailconfigRes.EmailTime.Value.Hours == DateTime.Now.AddHours(0).TimeOfDay.Hours && emailconfigRes.EmailTime.Value.Minutes == DateTime.Now.AddHours(0).TimeOfDay.Minutes)
                        {
                            if (APIConfig.isSendingEmail == false)
                            {
                                APIConfig.isSendingEmail = true;
                                List<BalanceSheet> lstBalanceSheet = BalanceSheet(Convert.ToInt32(73), false, Password);
                                List<BalanceSheet> lstPlusCustomers = lstBalanceSheet.Where(item => item.Profit > 0).ToList();
                                List<BalanceSheet> lstMinusCustomers = lstBalanceSheet.Where(item => item.Loss < 0).ToList();
                                decimal totPlus = lstPlusCustomers.Sum(item => item.Profit);
                                decimal totminus = lstMinusCustomers.Sum(item => item.Loss);

                                string filepath = ExportToPdf(lstPlusCustomers, lstMinusCustomers, totPlus, totminus);
                                SendEmailBalanceSheetToClient(emailconfigRes, filepath, Password);
                                APIConfig.isSendingEmail = false;
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    APIConfig.WriteErrorToDB(ex.ToString());
                    APIConfig.LogError(ex);
                    APIConfig.isSendingEmail = false;
                }
            }
        }
        public void SendBalanceSheettoEmail(string Password)
        {
            if (ValidatePassword(Password) == true)
            {
                try
                {
                    var emailconfigRes = dbEntities.SP_EMAILConfig_GetData().FirstOrDefault();
                    if (1 == 1)
                    {

                        if (1 == 1)
                        {
                            List<BalanceSheet> lstBalanceSheet = BalanceSheet(Convert.ToInt32(73), false, Password);
                            List<BalanceSheet> lstPlusCustomers = lstBalanceSheet.Where(item => item.Profit > 0).ToList();
                            List<BalanceSheet> lstMinusCustomers = lstBalanceSheet.Where(item => item.Loss < 0).ToList();
                            decimal totPlus = lstPlusCustomers.Sum(item => item.Profit);
                            decimal totminus = lstMinusCustomers.Sum(item => item.Loss);

                            string filepath = ExportToPdf(lstPlusCustomers, lstMinusCustomers, totPlus, totminus);
                            SendEmailBalanceSheetToClient(emailconfigRes, filepath, Password);
                            APIConfig.isSendingEmail = false;
                        }
                    }
                }
                catch (System.Exception ex)
                {

                    APIConfig.WriteErrorToDB(ex.ToString());
                    APIConfig.LogError(ex);
                    APIConfig.isSendingEmail = false;
                }
            }
        }
        List<UserIDandUserType> lstonlysuperAll = new List<UserIDandUserType>();
        List<UserIDandUserType> lstonlysamiadminAll = new List<UserIDandUserType>();
        List<UserIDandUserType> lstonlyAgentsAll = new List<UserIDandUserType>();
        public List<BalanceSheet> BalanceSheet(int UserID, bool isCredit, string Password)
        {
            try
            {
                List<UserAccounts> lstAccountsDonebyAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(GetAccountsDataForAdmin(1, false, Password));

                var results = GetAllUsersbyUserType(1, 1, Password);
                if (results != "")
                {
                    List<UserIDandUserType> lstUsers = JsonConvert.DeserializeObject<List<UserIDandUserType>>(results);

                    foreach (UserIDandUserType objuser in lstUsers)
                    {
                        objuser.UserName = Crypto.Decrypt(objuser.UserName);
                        objuser.UserName = objuser.UserName + " (" + objuser.UserType + ")";
                    }
                    List<UserIDandUserType> lstonlysuper = lstUsers.Where(item => item.UserTypeID == 8 && item.CreatedbyID==1).ToList();
                    lstonlysuperAll = lstonlysuper;
                    List<UserIDandUserType> lstonlysamiadmin = lstUsers.Where(item => item.UserTypeID == 9 && item.CreatedbyID == 1).ToList();
                    lstonlysamiadminAll = lstonlysamiadmin;
                   List<UserIDandUserType> lstonlyAgents = lstUsers.Where(item =>item.UserTypeID == 2 && item.CreatedbyID == 1).ToList();
                    lstonlyAgentsAll = lstonlyAgents;
                }
                List<BalanceSheet> lstBalanceSheet = new List<BalanceSheet>();
                Decimal TotAdminAmount = 0;
                Decimal TotAdmincommession = 0;
                decimal TotalAdminAmountWithoutMarkets = 0;
                if (1 == 1)
                {
                    List<UserAccounts> lstUserAccounts = JsonConvert.DeserializeObject<List<UserAccounts>>(GetAccountsDatabyCreatedByID(UserID, isCredit, Password));
                    if (UserID == 73)
                    {
                        foreach (var item in lstonlyAgentsAll)
                        {
                            if (item.ID > 0 && item.ID != 73 && item.ID != 3)
                            {
                                List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(GetAccountsDatabyCreatedByID(item.ID, isCredit, Password));
                                if (lstUserAccountsForAgent.Count > 0)
                                {
                                    lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                                    foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                                    {
                                        if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                                        {
                                            int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                            int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                            decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);

                                            if (ActualAmount > 0)
                                            {
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

                                                TotAdminAmount += -1 * (ActualAmount - (AgentAmount) - Comissionamount);
                                            }
                                            else
                                            {
                                                ActualAmount = -1 * ActualAmount;
                                                decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                TotAdminAmount += ActualAmount - AgentAmount;
                                            }


                                        }
                                    }
                                    UserAccounts objNewUseAccount = new UserAccounts();
                                    objNewUseAccount.UserName = item.UserName;
                                    objNewUseAccount.UserType = "Agent";
                                    objNewUseAccount.UserID = item.ID;
                                    objNewUseAccount.OpeningBalance = GetStartingBalance(item.ID, Password);
                                    TotAdminAmount = -1 * TotAdminAmount;
                                    if (TotAdminAmount >= 0)
                                    {
                                        objNewUseAccount.Debit = TotAdminAmount.ToString();
                                        objNewUseAccount.Credit = "0.00";
                                    }
                                    else
                                    {
                                        objNewUseAccount.Credit = (-1 * TotAdminAmount).ToString();
                                        objNewUseAccount.Debit = "0.00";

                                    }
                                    lstUserAccounts.Add(objNewUseAccount);
                                    TotAdminAmount = 0;
                                }
                               // List<UserAccounts> lstAccountsDonebyAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(GetAccountsDataForAdmin(1, false, Password));
                                if (lstAccountsDonebyAdmin.Count > 0)
                                {
                                    List<UserAccounts> lstAccountsDonebyAdminagainstthisAgent = lstAccountsDonebyAdmin.Where(item1 => item1.AccountsTitle.Contains("(UserID=" + item.ID.ToString() + ")")).ToList();
                                    if (lstAccountsDonebyAdminagainstthisAgent.Count > 0)
                                    {
                                        TotalAdminAmountWithoutMarkets = lstAccountsDonebyAdminagainstthisAgent.Sum(item1 => Convert.ToDecimal(item1.Debit)) - lstAccountsDonebyAdminagainstthisAgent.Sum(item1 => Convert.ToDecimal(item1.Credit));

                                    }
                                }
                                List<UserAccounts> objCurrentUserAccountlst = lstUserAccounts.Where(itemagent => itemagent.UserID.ToString() == item.ID.ToString()).ToList();
                                if (objCurrentUserAccountlst.Count > 0)
                                {
                                    UserAccounts objCurrentUserAccount = objCurrentUserAccountlst[0];
                                    if (TotalAdminAmountWithoutMarkets < 0)
                                    {
                                        objCurrentUserAccount.Debit = (Convert.ToDecimal(objCurrentUserAccount.Debit) + TotalAdminAmountWithoutMarkets).ToString();
                                    }
                                    else
                                    {
                                        objCurrentUserAccount.Credit = (Convert.ToDecimal(objCurrentUserAccount.Credit) + TotalAdminAmountWithoutMarkets).ToString();
                                    }
                                    if (Convert.ToDecimal(objCurrentUserAccount.Debit) < 0)
                                    {
                                        objCurrentUserAccount.Debit = (-1 * Convert.ToDecimal(objCurrentUserAccount.Debit)).ToString();
                                    }
                                    if (Convert.ToDecimal(objCurrentUserAccount.Credit) < 0)
                                    {
                                        objCurrentUserAccount.Credit = (-1 * Convert.ToDecimal(objCurrentUserAccount.Credit)).ToString();
                                    }
                                    TotalAdminAmountWithoutMarkets = 0;
                                }
                                else
                                {
                                    UserAccounts objNewUseAccount = new UserAccounts();
                                    objNewUseAccount.UserName = item.UserName;
                                    objNewUseAccount.UserType = "Agent";
                                    objNewUseAccount.UserID = item.ID;
                                    objNewUseAccount.OpeningBalance = GetStartingBalance(item.ID, Password);
                                    TotalAdminAmountWithoutMarkets = -1 * TotalAdminAmountWithoutMarkets;
                                    if (TotalAdminAmountWithoutMarkets >= 0)
                                    {
                                        objNewUseAccount.Debit = TotalAdminAmountWithoutMarkets.ToString();
                                        objNewUseAccount.Credit = "0.00";
                                    }
                                    else
                                    {
                                        objNewUseAccount.Credit = (-1 * TotalAdminAmountWithoutMarkets).ToString();
                                        objNewUseAccount.Debit = "0.00";

                                    }
                                    lstUserAccounts.Add(objNewUseAccount);
                                    TotalAdminAmountWithoutMarkets = 0;
                                }
                                //commisionagent
                                decimal AgentCommission = 0;
                                try
                                {
                                    AgentCommission = GetTotalAgentCommissionbyAgentID(item.ID, Password);
                                    UserAccounts objNewUseAccount = new UserAccounts();
                                    objNewUseAccount.UserName = item.UserName;
                                    objNewUseAccount.UserType = "Agent";
                                    objNewUseAccount.UserID = item.ID;
                                    objNewUseAccount.OpeningBalance = GetStartingBalance(item.ID, Password);

                                    if (AgentCommission >= 0)
                                    {
                                        objNewUseAccount.Debit = AgentCommission.ToString();
                                        objNewUseAccount.Credit = "0.00";
                                    }
                                    else
                                    {
                                        objNewUseAccount.Credit = (-1 * AgentCommission).ToString();
                                        objNewUseAccount.Debit = "0.00";

                                    }
                                    lstUserAccounts.Add(objNewUseAccount);
                                    AgentCommission = 0;
                                }
                                catch (System.Exception ex)
                                {


                                }
                            }
                        }
                        //List<UserAccounts> lstUserAccounts1 = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(LoggedinUserDetail.GetUserID(), isCredit));
                        //foreach(UserAccounts objusrAccounts in lstUserAccounts1)
                        //{
                        //    if(objusrAccounts.UserID != 73  )
                        //    {
                        //        objusrAccounts.UserName = Crypto.Decrypt(objusrAccounts.UserName)+ "(Agent)";
                        //        objusrAccounts.UserType = "Agent";
                        //        lstUserAccounts.Add(objusrAccounts);
                        //    }
                        //}

                    }
                    if (lstUserAccounts.Count > 0)
                    {

                        var lstUserIDs = lstUserAccounts.Select(item1 => new { item1.UserID }).Distinct().ToArray();

                        foreach (var useritem in lstUserIDs)
                        {
                            List<UserAccounts> lstUseraccountsbyUser = lstUserAccounts.Where(item2 => item2.UserID == useritem.UserID).ToList();
                            if (lstUseraccountsbyUser.Count > 0)
                            {
                                BalanceSheet objBalanceSheet = new BalanceSheet();
                                if (lstUseraccountsbyUser[0].UserType == null)
                                {
                                    objBalanceSheet.Username = Crypto.Decrypt(lstUseraccountsbyUser[0].UserName);
                                }
                                else
                                {
                                    objBalanceSheet.Username = lstUseraccountsbyUser[0].UserName;
                                }
                                objBalanceSheet.StartingBalance = lstUseraccountsbyUser[0].OpeningBalance;
                                objBalanceSheet.UserID = Convert.ToInt32(lstUseraccountsbyUser[0].UserID);
                                decimal Profitorloss = lstUseraccountsbyUser.Sum(item3 => Convert.ToDecimal(item3.Debit)) - lstUseraccountsbyUser.Sum(item3 => Convert.ToDecimal(item3.Credit));
                                if (Profitorloss >= 0)
                                {
                                    objBalanceSheet.Profit = Profitorloss;

                                }
                                else
                                {
                                    objBalanceSheet.Loss = Profitorloss;
                                }
                                lstBalanceSheet.Add(objBalanceSheet);
                            }
                        }

                    }
                    List<UserAccounts> lstUserAccountsAdmin = new List<UserAccounts>();
                    if (1 == 1)
                    {
                        bfnexchange.Services.DBModel.SP_Users_GetCommissionAccountIDandBookAccountID_Result objCommissionandBookAccountID = GetCommissionaccountIdandBookAccountbyUserID(1);
                        //  lstUserAccountsAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(Convert.ToInt32(objCommissionandBookAccountID.BookAccountID), isCredit));
                    }
                    else
                    {
                        //lstUserAccountsAdmin = JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDataForAdmin(UserID, isCredit));
                    }
                    if (lstUserAccountsAdmin.Count > 0)
                    {


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
                }
                if (1 == 1)
                {
                    List<UserAccounts> lstUserAccounts = new List<UserAccounts>(); //JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(UserID, isCredit, LoggedinUserDetail.PasswordForValidate));

                    lstonlysuperAll = lstonlysuperAll.Where(s => s.UserTypeID == 8).ToList();
                    decimal TotAdminAmount1 = 0;
                    decimal TotalAdminAmountWithoutMarkets1 = 0;
                    foreach (var item in lstonlysuperAll)
                    {
                        UserAccounts objNewUseAccount = new UserAccounts();
                        List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(GetAccountsDatabyCreatedByIDForSuper(item.ID, false, Password));
                        if (lstUserAccountsForAgent.Count > 0)
                        {
                            List<UserAccounts> AgentCommission= lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle == "Commission").ToList();
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

                                        TotAdminAmount1 += -1 * (ActualAmount - (SuperAmount + AgentAmount));
                                    }
                                    else
                                    {
                                        ActualAmount = -1 * ActualAmount;
                                        decimal SuperAmount = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                        decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                        TotAdminAmount1 += ActualAmount - (AgentAmount + SuperAmount);
                                    }
                                }
                            }
                            // NetBalance =;
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
                        }
                        objNewUseAccount.UserName = item.UserName;
                        objNewUseAccount.UserType = "Super";
                        objNewUseAccount.UserID = item.ID;
                        BalanceSheet objBalanceSheet4 = new BalanceSheet();
                        objBalanceSheet4.Username = item.UserName;
                        objBalanceSheet4.UserID = item.ID;
                        decimal Profitorloss = (-1 * (TotAdminAmount1) + (-1 * TotAdmincommession));
                        if (Profitorloss >= 0)
                        {
                            objBalanceSheet4.Profit = Profitorloss;

                        }
                        else
                        {
                            objBalanceSheet4.Loss = Profitorloss;
                        }
                        lstBalanceSheet.Add(objBalanceSheet4);

                        TotAdmincommession = 0;
                        TotAdminAmount1 = 0;
                        TotalAdminAmountWithoutMarkets1 = 0;

                    }



                    if (lstUserAccounts.Count > 0)
                    {

                        //var lstUserIDs = lstUserAccounts.Select(item1 => new { item1.UserID }).Distinct().ToArray();

                        foreach (var useritem in lstUserAccounts)
                        {
                            //  List<UserAccounts> lstUseraccountsbyUser = lstUserAccounts.Where(item2 => item2.UserID == useritem.UserID).ToList();
                            //if (lstUseraccountsbyUser.Count > 0)
                            //{
                           
                            //}
                        }
                    }
                    //return lstBalanceSheet;
                }

                if (1 == 1)
                {
                    List<UserAccounts> lstUserAccounts = new List<UserAccounts>(); //JsonConvert.DeserializeObject<List<UserAccounts>>(objUsersServiceCleint.GetAccountsDatabyCreatedByID(UserID, isCredit, LoggedinUserDetail.PasswordForValidate));

                    //lstonlysuperAll = .Where(s => s.UserTypeID == 8).ToList();
                    decimal TotAdminAmount1 = 0;
                    decimal TotalAdminAmountWithoutMarkets1 = 0;
                    foreach (var items in lstonlysamiadminAll)
                    {
                        UserAccounts objNewUseAccount = new UserAccounts();
                        var resultss =  GetAllUsersbyUserType(items.ID,9,Password);
                        if (resultss != "")
                        {
                            List<UserIDandUserType> lstonlySuperAllforSamiadmin = JsonConvert.DeserializeObject<List<UserIDandUserType>>(resultss);
                            lstonlySuperAllforSamiadmin = lstonlySuperAllforSamiadmin.Where(x => x.UserTypeID == 8).ToList();
                            foreach (var item2 in lstonlySuperAllforSamiadmin)
                            {
                                var resultss1 = GetAllUsersbyUserType(item2.ID, 8, Password);
                                List<UserIDandUserType> lstonlyAgentAllforSuper = JsonConvert.DeserializeObject<List<UserIDandUserType>>(resultss1);
                                lstonlyAgentAllforSuper = lstonlyAgentAllforSuper.Where(x => x.UserTypeID == 2).ToList();
                                foreach (var item in lstonlyAgentAllforSuper)
                                { 
                               
                                List<UserAccounts> lstUserAccountsForAgent = JsonConvert.DeserializeObject<List<UserAccounts>>(GetAccountsDatabyCreatedByID(item.ID, false, Password));
                                if (lstUserAccountsForAgent.Count > 0)
                                {
                                       
                                        List<UserAccounts> samiadminCommission = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle == "Commission").ToList();
                                        lstUserAccountsForAgent = lstUserAccountsForAgent.Where(item1 => item1.AccountsTitle != "Commission").ToList();
                                    foreach (UserAccounts objuserAccounts in lstUserAccountsForAgent)
                                    {
                                            if (objuserAccounts.AccountsTitle != "Commission" && objuserAccounts.MarketBookID != "")
                                            {
                                                int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                                int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                                int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                                int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiAdminRate);
                                                decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                                decimal superpercent = SuperRate - AgentRate;
                                                decimal samiadminpercent = SamiadminRate - (superpercent + AgentRate);
                                                if (ActualAmount > 0)
                                                {
                                                    decimal SuperAmount11 = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                    decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                    decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                                    decimal Comissionamount = 0;
                                                    if (AgentRate == 100)
                                                    {
                                                        Comissionamount = 0;
                                                    }
                                                    else
                                                    {
                                                        Comissionamount = Math.Round(((Convert.ToDecimal(commissionrate) / 100) * ActualAmount), 2);
                                                    }

                                                    TotAdminAmount += -1 * (ActualAmount - (SuperAmount11 + AgentAmount + SamiadminAmount));
                                                }
                                                else
                                                {
                                                    ActualAmount = -1 * ActualAmount;
                                                    decimal SuperAmount11 = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                    decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                    decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                                    TotAdminAmount += ActualAmount - (AgentAmount + SuperAmount11 + SamiadminAmount);
                                                }
                                            }
                                        }
                                        try
                                        {
                                            foreach (UserAccounts objuserAccounts in samiadminCommission)
                                            {
                                                int commissionrate = Convert.ToInt32(objuserAccounts.ComissionRate);
                                                int AgentRate = Convert.ToInt32(objuserAccounts.AgentRate);
                                                int SuperRate = Convert.ToInt32(objuserAccounts.SuperRate);
                                                int SamiadminRate = Convert.ToInt32(objuserAccounts.SamiAdminRate);
                                                decimal ActualAmount = Convert.ToDecimal(objuserAccounts.Debit) - Convert.ToDecimal(objuserAccounts.Credit);
                                                decimal superpercent = SuperRate - AgentRate;
                                                decimal samiadminpercent = SamiadminRate - (superpercent + AgentRate);
                                                ActualAmount = -1 * ActualAmount;
                                                decimal SuperAmount12 = Math.Round((Convert.ToDecimal(superpercent) / 100) * ActualAmount, 2);
                                                decimal AgentAmount = Math.Round((Convert.ToDecimal(AgentRate) / 100) * ActualAmount, 2);
                                                decimal SamiadminAmount = Math.Round((Convert.ToDecimal(samiadminpercent) / 100) * ActualAmount, 2);
                                                TotAdmincommession += ActualAmount - (AgentAmount + SuperAmount12 + SamiadminAmount);
                                            }
                                            //AgentCommission = objUsersServiceCleint.GetTotalAgentCommissionbyAgentID(LoggedinUserDetail.GetUserID(), LoggedinUserDetail.PasswordForValidate);
                                        }
                                        catch (System.Exception ex)
                                        {


                                        }

                                        // NetBalance =;
                                    }
                                }
                        }
                    }
                       

                        objNewUseAccount.UserName = items.UserName;
                        objNewUseAccount.UserType = "Samiadmin";
                        objNewUseAccount.UserID = items.ID;

                        objNewUseAccount.NetProfitorLoss = (-1 * (TotAdminAmount) + (-1 * TotAdmincommession));

                        lstUserAccounts.Add(objNewUseAccount);
                        TotAdminAmount1 = 0;
                        TotalAdminAmountWithoutMarkets1 = 0;

                    }
                    if (lstUserAccounts.Count > 0)
                    {
                        foreach (var useritem in lstUserAccounts)
                        {
                           
                            BalanceSheet objBalanceSheet4 = new BalanceSheet();

                            objBalanceSheet4.Username = useritem.UserName;

                            objBalanceSheet4.StartingBalance = useritem.OpeningBalance;
                            objBalanceSheet4.UserID = Convert.ToInt32(useritem.UserID);
                            decimal Profitorloss = Convert.ToDecimal(useritem.NetProfitorLoss);
                            if (Profitorloss >= 0)
                            {
                                objBalanceSheet4.Profit = Profitorloss;

                            }
                            else
                            {
                                objBalanceSheet4.Loss = Profitorloss;
                            }
                            lstBalanceSheet.Add(objBalanceSheet4);
                            //}
                        }
                    }
                    //return lstBalanceSheet;
                }
                lstBalanceSheet = lstBalanceSheet.OrderBy(item => item.Username).ToList();
                return lstBalanceSheet;
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
                List<BalanceSheet> lstBalanceSheet = new List<BalanceSheet>();
                return lstBalanceSheet;
            }

        }
        public string ExportToPdf(List<BalanceSheet> PlusCsutomers, List<BalanceSheet> MinusCustomers, decimal totplus, decimal TotMinus) //Datatable 
        {
            //Here set page size as A4

            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);

            try
            {
                string currenttime = string.Format("{0:hh:mm:ss tt}", DateTime.Now);
                currenttime = currenttime.Replace(":", "-");
                string filepath = "d:\\Balance Sheet of " + DateTime.Now.Date.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + " " + currenttime + ".pdf";
                PdfWriter wri = PdfWriter.GetInstance(pdfDoc, new FileStream(filepath, FileMode.Create));
                //PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                pdfDoc.Open();

                //Set Font Properties for PDF File
                var FontColour = new BaseColor(0, 0, 0);
                iTextSharp.text.Font fnt = FontFactory.GetFont("Times New Roman", 10, FontColour);
                var FontColour1 = new BaseColor(26, 178, 41);
                iTextSharp.text.Font fntgreen = FontFactory.GetFont("Times New Roman", 10, FontColour1);
                var FontColour2 = new BaseColor(244, 66, 66);
                iTextSharp.text.Font fntred = FontFactory.GetFont("Times New Roman", 10, FontColour2);
                Paragraph paragraphheading = new Paragraph("Balance Sheet of " + DateTime.Now.Date.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + " ( " + "Faisal(Agent)" + " )");
                paragraphheading.Alignment = Element.ALIGN_CENTER;
                Paragraph paragraphspace = new Paragraph("\n");
                Paragraph paragraphPlusCustomers = new Paragraph("Plus Customers");
                paragraphPlusCustomers.Alignment = Element.ALIGN_CENTER;
                Paragraph paragraphMinusCustomers = new Paragraph("Minus Customers");
                paragraphMinusCustomers.Alignment = Element.ALIGN_CENTER;


                pdfDoc.Add(paragraphheading);
                pdfDoc.Add(paragraphspace);


                //  pdfDoc.Add(paragraphPlusCustomers);
                // pdfDoc.Add(paragraphspace);

                if (PlusCsutomers.Count > 0)
                {

                    PdfPTable PdfTable = new PdfPTable(2);
                    PdfTable.TotalWidth = 200f;
                    PdfTable.LockedWidth = true;
                    PdfPCell PdfPCell = null;

                    //Here we create PDF file tables
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("Customers", fnt)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("Plus", fnt)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    for (int rows = 0; rows < PlusCsutomers.Count; rows++)
                    {

                        PdfPCell = new PdfPCell(new Phrase(new Chunk(PlusCsutomers[rows].Username.ToString(), fnt)));
                        PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        PdfTable.AddCell(PdfPCell);
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(PlusCsutomers[rows].Profit.ToString("N0").Replace(",", ""), fntgreen)));
                        PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        PdfTable.AddCell(PdfPCell);
                        //for (int column = 0; column < 2; column++)
                        //{
                        //    if (dt.Columns[column].Visible == true)
                        //    {
                        //        PdfPCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[rows].Cells[column].Value.ToString(), fnt)));
                        //        PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        //        PdfTable.AddCell(PdfPCell);
                        //    }
                        //}
                    }
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("-", fnt)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("-", fntgreen)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("Total", fnt)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk(totplus.ToString("N0").Replace(",", ""), fntgreen)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    PdfTable.WriteSelectedRows(0, -1, pdfDoc.Left + 100, pdfDoc.Top - 40, wri.DirectContent);
                    // Finally Add pdf table to the document 
                    //pdfDoc.Add(PdfTable);
                }

                //   pdfDoc.Add(paragraphMinusCustomers);
                //  pdfDoc.Add(paragraphspace);



                if (MinusCustomers.Count > 0)
                {

                    PdfPTable PdfTable = new PdfPTable(2);
                    PdfTable.TotalWidth = 200f;
                    PdfTable.LockedWidth = true;
                    PdfPCell PdfPCell = null;

                    //Here we create PDF file tables
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("Customers", fnt)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("Minus", fnt)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    for (int rows = 0; rows < MinusCustomers.Count; rows++)
                    {

                        PdfPCell = new PdfPCell(new Phrase(new Chunk(MinusCustomers[rows].Username.ToString(), fnt)));
                        PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                        PdfTable.AddCell(PdfPCell);
                        PdfPCell = new PdfPCell(new Phrase(new Chunk(MinusCustomers[rows].Loss.ToString("N0").Replace(",", ""), fntred)));
                        PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        PdfTable.AddCell(PdfPCell);

                    }
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("-", fnt)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("-", fntred)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk("Total", fnt)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                    PdfTable.AddCell(PdfPCell);
                    PdfPCell = new PdfPCell(new Phrase(new Chunk(TotMinus.ToString("N0").Replace(",", ""), fntred)));
                    PdfPCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    PdfTable.AddCell(PdfPCell);
                    PdfTable.WriteSelectedRows(0, -1, pdfDoc.Left + 320, pdfDoc.Top - 40, wri.DirectContent);
                }
                pdfDoc.Close();
                return filepath;
                //Response.ContentType = "application/pdf";

                ////Set default file Name as current datetime
                //Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMdd") + ".pdf");

                //System.Web.HttpContext.Current.Response.Write(pdfDoc);

                //Response.Flush();
                //Response.End();

            }
            catch (Exception ex)
            {
                return "";
                // Response.Write(ex.ToString());
            }
        }
        public SP_UserMarket_GetEventDetailsbyMarketID_Result GetEventDetailsbyMarketBook(string MarketbookID)
        {
            return dbEntities.SP_UserMarket_GetEventDetailsbyMarketID(MarketbookID).FirstOrDefault();
        }
       
        public string GetLinevMarketsbyEventID(string EventID, DateTime EventOpenDate, int UserID)
        {
            return ConverttoJSONString(dbEntities.SP_UserMarket_GetDistinctLinevMarketsbyEventID(EventID, EventOpenDate, UserID).ToList());
        }
        public string GetLinevMarketsbyEventID2()
        {
            return ConverttoJSONString(dbEntities.SP_UserMarket_GetDistinctLinevMarketsbyEventID2().ToList());
        }

        public string KJMarketsbyEventID(string EventID, int UserID)
        {
            return ConverttoJSONString(dbEntities.SP_UserMarket_GetDistinctKJMarketsbyEventIDs(EventID, UserID).ToList());
        }
       
        
        public string GetLinevMarketsbyEventIDIN(string EventID, DateTime EventOpenDate, int UserID)
        {
            return ConverttoJSONString(dbEntities.SP_UserMarket_GetDistinctLinevMarketsbyEventIDIN(EventID).ToList());
        }
        public string GetMarketIDbyEventID(string EventID)
        {
            return ConverttoJSONString(dbEntities.SP_UserMarket_GetDistinctMarketIDByEventID(EventID).FirstOrDefault());
        }

        public string GetKalijut()
        {
            return ConverttoJSONString(dbEntities.Sp_GetKalijutodds().ToList());
        }
      
        public string GetFigureOdds()
        {
            return ConverttoJSONString(dbEntities.Sp_GetFigureOdd().ToList());
        }
        public string GetScoresbyEventIDandDate(string EventId, DateTime EventOpenDate)
        {

            var results = dbEntities.SP_BallbyBallSummary_GetScoresbyEventIDbyInnings(EventId, EventOpenDate, 1).ToList();
            return ConverttoJSONString(results);
            // List<SP_BallbyBallSummary_GetScoresbyEventIDbyInnings_Result> lstScores = new List<SP_BallbyBallSummary_GetScoresbyEventIDbyInnings_Result>();
            //if (results != null)
            //{
            //    lstScores.Add(results);
            //}
            //results = dbEntities.SP_BallbyBallSummary_GetScoresbyEventIDbyInnings(EventId, EventOpenDate, 2).FirstOrDefault();
            //if (results != null)
            //{
            //    lstScores.Add(results);
            //}
            //results = dbEntities.SP_BallbyBallSummary_GetScoresbyEventIDbyInnings(EventId, EventOpenDate, 3).FirstOrDefault();
            //if (results != null)
            //{
            //    lstScores.Add(results);
            //}
            //results = dbEntities.SP_BallbyBallSummary_GetScoresbyEventIDbyInnings(EventId, EventOpenDate, 4).FirstOrDefault();
            //if (results != null)
            //{
            //    lstScores.Add(results);
            //}
            //return ConverttoJSONString(lstScores);
        }
        public string GetMarketRules()
        {
            return ConverttoJSONString(dbEntities.SP_MarketRules_GetData().ToList());
        }
        public string GetLineandMatchOddsforAssociation()
        {
            return ConverttoJSONString(dbEntities.SP_UserMarket_GetEventLineMarketandOddsForAssociation().ToList());
        }
        public void UpdateAssociateEventID(string associateventID, string EventID)
        {
            dbEntities.SP_UserMarket_UpdateAssociateEventID(EventID, associateventID);
        }
        public string GetLiveTVChanels(string Passkey)
        {
            if (ValidatePassword(Passkey) == false)
            {
                return "";
            }
            else
            {
                return ConverttoJSONString(dbEntities.SP_LiveTVChanels_GetAlldata().ToList());
            }
        }
        public string GetScorebyEventIDandInnings(string EventID, DateTime EventOpenDate, int Innings)
        {
            return ConverttoJSONString(dbEntities.SP_BallbyBallSummary_GetScoresbyEventIDbyInnings(EventID, EventOpenDate, Innings).FirstOrDefault());
        }
        public string GetScorebyEventIDandInningsandOvers(string EventID, DateTime EventOpenDate, int Innings, int Overs)
        {
            return ConverttoJSONString(dbEntities.SP_BallbyBallSummary_GetScoresbyEventIDbyInningsandOvers(EventID, EventOpenDate, Innings, Overs).FirstOrDefault());
        }
        public void AddScoreToBallbyBallsummary(string EventID, string MarketCatalogueID, double over, int score, int innings, DateTime EventOpenDate, int wickets, string teamname, string matchstatus)
        {
            dbEntities.SP_BallbyBallSummary_Insert(EventID.ToString(), MarketCatalogueID, Convert.ToDouble(over), Convert.ToInt32(score), Convert.ToInt32(innings), Convert.ToDateTime(EventOpenDate), Convert.ToInt32(wickets), teamname, matchstatus, "", "");
        }
        public void UpdateMarketStatusbyMarketBookID(string MarketBookID, string MarketStatus)
        {

            dbEntities.SP_UserMarket_UpdateMarketStatusbyMarektBookID(MarketBookID, MarketStatus);
        }
        public class LZWebClient : WebClient
        {
            protected override WebRequest GetWebRequest(Uri address)
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                return request;
            }
        }
        public void InitializeCricketAPIConfig()
        {
            if (CricketAPIConfig.AccessKey == "")
            {
                SP_CricketAPIConfig_GetData_Result objCricketAPIConfig = dbEntities.SP_CricketAPIConfig_GetData().FirstOrDefault();
                CricketAPIConfig.AccessKey = objCricketAPIConfig.AccessKey;
                CricketAPIConfig.AppID = objCricketAPIConfig.AppID;
                CricketAPIConfig.DeviceID = objCricketAPIConfig.DeviceID;
                CricketAPIConfig.SecretKey = objCricketAPIConfig.SecretKey;
                CricketAPIConfig.SessionKey = objCricketAPIConfig.SessionKey;
            }
        }
        public class CricketScoreAllMatches
        {
            public int id { get; set; }
            public string t2 { get; set; }
            public string t1 { get; set; }
        }
        public string GetRecentMatchesFromCricketAPI()
        {
            List<RecentMatches> lstRecentMatches = new List<RecentMatches>();
            //System.Net.WebClient wc = new System.Net.WebClient();


            //string result = wc.DownloadString("http://cricscore-api.appspot.com/csa");
            //List<CricketScoreAllMatches> cricketscoreallmatches = new List<CricketScoreAllMatches>();
            //cricketscoreallmatches = JsonConvert.DeserializeObject < List<CricketScoreAllMatches>>(result);
            //foreach(var item in cricketscoreallmatches)
            //{
            //    RecentMatches objRecentMatch = new RecentMatches();
            //    objRecentMatch.Key = item.id.ToString();
            //    objRecentMatch.MatchName = item.t1.ToString() + " vs " + item.t2.ToString();
            //    lstRecentMatches.Add(objRecentMatch);
            //}
            //return ConverttoJSONString(lstRecentMatches);
            InitializeCricketAPIConfig();
            // List<RecentMatches> lstRecentMatches = new List<RecentMatches>();
            LZWebClient recentmatchesClient = new LZWebClient();

            string recentmatchesstr = recentmatchesClient.DownloadString("https://rest.cricketapi.com/rest/v2/recent_matches/?access_token=" + CricketAPIConfig.SessionKey);
            AllMatches objAllMatches = JsonConvert.DeserializeObject<AllMatches>(recentmatchesstr);
            if (objAllMatches.status_code == 200)
            {
                foreach (var item in objAllMatches.data.cards)
                {
                    RecentMatches objRecentMatch = new RecentMatches();
                    objRecentMatch.Key = item.key;
                    objRecentMatch.MatchName = item.name.ToString() + " " + Convert.ToDateTime(item.start_date.iso).ToString("d MMM yyyy");
                    lstRecentMatches.Add(objRecentMatch);
                }
            }
            else
            {
                GetSessionFromCricketAPI();
                recentmatchesstr = recentmatchesClient.DownloadString("https://rest.cricketapi.com/rest/v2/recent_matches/?access_token=" + CricketAPIConfig.SessionKey);
                objAllMatches = JsonConvert.DeserializeObject<AllMatches>(recentmatchesstr);
                foreach (var item in objAllMatches.data.cards)
                {
                    RecentMatches objRecentMatch = new RecentMatches();
                    objRecentMatch.Key = item.key;
                    objRecentMatch.MatchName = item.name.ToString() + "-" + Convert.ToDateTime(item.start_date.iso).ToString("d MMM yyyy");
                    lstRecentMatches.Add(objRecentMatch);
                }
            }
            return ConverttoJSONString(lstRecentMatches);
        }
        public void UpdateCricketAPIMatchKey(string EventID, string CricketAPIMatchKey)
        {
            dbEntities.SP_UserMarket_UpdateCricketAPIMatchKey(EventID, CricketAPIMatchKey);
        }
        public string GetFancyResultsFrom()
        {
            return dbEntities.SP_ResultPostSettings_GetResultsFrom().FirstOrDefault();
        }
        public void UpdateGetFancyResultsFrom(string ResultsFrom)
        {
            dbEntities.SP_ResultPostSettings_UpdateGetResultsFrom(ResultsFrom);
        }
        public void UpdateTransferAdminAmount(int UserID, bool TransferAdminAmount, int TransferAgentID, bool TransferAdminAmountSoccer, bool TransferAdminAmountTennis, bool TransferAdminAmountHorse, bool TransferAdminAmountGreyHound)
        {
            dbEntities.SP_Users_UpdateTransferAdminAmount(UserID, TransferAdminAmount, TransferAgentID, TransferAdminAmountSoccer, TransferAdminAmountTennis, TransferAdminAmountHorse, TransferAdminAmountGreyHound);
        }
        public SP_Users_GetTransferAdminAmount_Result GetTransferAdminAmount(int UserID)
        {
            return dbEntities.SP_Users_GetTransferAdminAmount(UserID).FirstOrDefault();
        }

        public void GetBallbyBallSummaryFromCricketAPI(string MarketCatalogueID, string EventID, DateTime EventOpenDate)
        {
            try
            {


                InitializeCricketAPIConfig();
                string over = "";
                string score = "";
                string innings = "";
                string wickets = "";
                string teamname = "";
                string matchstatus = "";
                string strmatchkey = dbEntities.SP_UserMarket_GetCricketAPIMatchKey(MarketCatalogueID).FirstOrDefault();
                if (strmatchkey != null && strmatchkey != "")
                {
                    LZWebClient recentballClient = new LZWebClient();
                    string recentmatchstr = recentballClient.DownloadString("https://rest.cricketapi.com/rest/v2/match/" + strmatchkey + "/?access_token=" + CricketAPIConfig.SessionKey + "&card_type=summary_card");
                    APIResponse objReponse = JsonConvert.DeserializeObject<APIResponse>(recentmatchstr);
                    if (objReponse.status_code == 200)
                    {
                        dynamic objResultcard = objReponse.data;
                        dynamic objBalls = objResultcard.card;
                        dynamic balls = objBalls.now;
                        score = balls.runs.ToString();
                        string[] scoreandovers = balls.runs_str.ToString().Split(new string[] { " in " }, StringSplitOptions.None);
                        over = scoreandovers[1];
                        //if (balls.balls < 6)
                        //{
                        //    over = "0." + balls.balls;
                        //}
                        //else
                        //{
                        //    over = (Convert.ToDecimal(balls.balls) / 6).ToString();
                        //}
                        wickets = balls.wicket.ToString();
                        matchstatus = objBalls.status_overview;
                        if (matchstatus == "in_play")
                        {
                            matchstatus = "InPlay";
                        }
                        if (balls.batting_team == "a")
                        {
                            teamname = objBalls.teams.a.name;
                        }
                        else
                        {
                            teamname = objBalls.teams.b.name;
                        }
                        if (objBalls.format == "t20" || objBalls.format == "t20" || objBalls.format == "t20")
                        {


                            if (Convert.ToInt32(objBalls.innings.b_1.runs) > 0 && Convert.ToInt32(objBalls.innings.a_1.runs) > 0)
                            {
                                innings = "2";
                            }
                            else
                            {
                                innings = "1";
                            }
                            dbEntities.SP_BallbyBallSummary_Insert(EventID.ToString(), MarketCatalogueID, Convert.ToDouble(over), Convert.ToInt32(score), Convert.ToInt32(innings), Convert.ToDateTime(EventOpenDate), Convert.ToInt32(wickets), teamname, matchstatus, "", "");
                        }
                        if (objBalls.format == "test")
                        {
                            if (Convert.ToInt32(objBalls.innings.b_1.runs) > 0 && Convert.ToInt32(objBalls.innings.a_1.runs) > 0)
                            {
                                innings = "2";
                            }
                            else
                            {
                                innings = "1";
                            }
                            dbEntities.SP_BallbyBallSummary_Insert(EventID.ToString(), MarketCatalogueID, Convert.ToDouble(over), Convert.ToInt32(score), Convert.ToInt32(innings), Convert.ToDateTime(EventOpenDate), Convert.ToInt32(wickets), teamname, matchstatus, "", "");
                        }
                        //dynamic lastball = balls.last_ball;
                        dynamic bettingorder = objBalls.batting_order;

                    }
                    else
                    {
                        GetSessionFromCricketAPI();
                    }
                }
            }
            catch (System.Exception ex)
            {

            }


        }
        public string GetCricketMatchKey(string MarketCatalogueID)
        {
            return dbEntities.SP_UserMarket_GetCricketAPIMatchKey(MarketCatalogueID).FirstOrDefault();
        }
        public void GetMatchUpdatesFromCricketAPI(string MarketCatalogueID, string EventID, DateTime EventOpenDate)
        {
            try
            {


                InitializeCricketAPIConfig();

                string strmatchkey = dbEntities.SP_UserMarket_GetCricketAPIMatchKey(MarketCatalogueID).FirstOrDefault();
                if (strmatchkey != null && strmatchkey != "")
                {
                    LZWebClient recentballClient = new LZWebClient();
                    string recentmatchstr = recentballClient.DownloadString("https://rest.cricketapi.com/rest/v2/match/" + strmatchkey + "/?access_token=" + CricketAPIConfig.SessionKey + "&card_type=full_card");
                    APIResponse objReponse = JsonConvert.DeserializeObject<APIResponse>(recentmatchstr);
                    if (objReponse.status_code == 200)
                    {

                        dynamic match = JsonConvert.DeserializeObject(recentmatchstr);
                        match = match.data.card;
                        MatchScoreCard objScoreCard = new MatchScoreCard();
                        objScoreCard.MatchKey = strmatchkey;
                        try
                        {

                            string bettingteamkey = match.now.batting_team;
                            if (bettingteamkey != null)
                            {


                                string bettinginningskey = match.now.innings;
                                string bowlingteamkey = match.now.bowling_team;
                                string bowlinginngingskey = match.now.innings;
                                string strikerkey = match.now.striker;
                                string nonstrikerkey = match.now.nonstriker;
                                string bowlerkey = match.now.bowler;
                                objScoreCard.Teams = new List<TeamsNew>();
                                TeamsNew objTeam = new TeamsNew();
                                objTeam.TeamKey = match.teams["a"].key.Value;
                                objTeam.TeamName = match.teams["a"].name.Value;
                                objScoreCard.Teams.Add(objTeam);
                                objTeam = new TeamsNew();
                                objTeam.TeamKey = match.teams["b"].key.Value;
                                objTeam.TeamName = match.teams["b"].name.Value;
                                objScoreCard.Teams.Add(objTeam);
                                try
                                {


                                    objScoreCard.TeamAScore = match.teams[bettingteamkey].key.Value.ToString().ToUpper() + ": " + match.innings[bettingteamkey + '_' + bettinginningskey].runs.Value + "/" + match.innings[bettingteamkey + '_' + bettinginningskey].wickets.Value + " in " + match.innings[bettingteamkey + '_' + bettinginningskey].overs.Value;
                                }
                                catch (System.Exception ex)
                                {

                                }
                                try
                                {
                                    dynamic teambscores = match.innings[bowlingteamkey + "_" + bowlinginngingskey].batting_order;
                                    var teambscorearr = JArray.Parse(teambscores.ToString());
                                    if (teambscorearr.Count > 0)
                                    {
                                        objScoreCard.TeamBScore = match.teams[bowlingteamkey].key.Value.ToString().ToUpper() + ": " + match.innings[bowlingteamkey + '_' + bowlinginngingskey].runs.Value + "/" + match.innings[bowlingteamkey + '_' + bowlinginngingskey].wickets.Value + " in " + match.innings[bowlingteamkey + '_' + bowlinginngingskey].overs.Value;
                                    }

                                }
                                catch (System.Exception ex)
                                {

                                }
                                try
                                {
                                    objScoreCard.CurrRR = "CRR:" + match.now.run_rate;
                                }
                                catch (System.Exception ex)
                                {

                                }
                                try
                                {
                                    objScoreCard.ReqRR = "Req RR:" + match.now.req.runs_rate;
                                }
                                catch (System.Exception ex)
                                {

                                }
                                try
                                {
                                    objScoreCard.LastBallComment = match.now.last_ball.comment == null ? "" : match.now.last_ball.comment;
                                }
                                catch (System.Exception ex)
                                {

                                }
                                objScoreCard.Msgs = new MsgsNew();
                                try
                                {
                                    objScoreCard.Msgs.match_comments = match.msgs.match_comments;
                                }
                                catch (System.Exception ex)
                                {

                                }
                                try
                                {

                                    objScoreCard.Msgs.completed = match.msgs.completed;
                                }
                                catch (System.Exception ex)
                                {

                                }
                                try
                                {

                                    objScoreCard.Msgs.info = match.msgs.info;
                                }
                                catch (System.Exception ex)
                                {

                                }
                                try
                                {


                                    objScoreCard.RequireScore = match.now.req.runs_str;
                                }
                                catch (System.Exception ex)
                                {

                                }
                                try
                                {

                                    if (strikerkey != null)
                                    {
                                        objScoreCard.StrickerStr = match.players[strikerkey].name.Value + " " + match.players[strikerkey].match.innings["1"].batting.runs.Value + " in " + match.players[strikerkey].match.innings["1"].batting.balls.Value;
                                    }

                                }
                                catch (System.Exception ex)
                                {

                                }
                                try
                                {

                                    if (nonstrikerkey != null)
                                    {
                                        objScoreCard.NonStrickerStr = match.players[nonstrikerkey].name.Value + " " + match.players[nonstrikerkey].match.innings["1"].batting.runs.Value + " in " + match.players[nonstrikerkey].match.innings["1"].batting.balls.Value + Environment.NewLine;
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                                try
                                {


                                    var stops = JArray.Parse(match.now.recent_overs_str[0][1].ToString());
                                    //var teamA = JArray.Parse(match.teams["a"].match.playing_xi.ToString());
                                    //var teamB = JArray.Parse(match.teams["b"].match.playing_xi.ToString());
                                    //objScoreCard.Teams[0].Players = new List<Player>();
                                    //objScoreCard.Teams[1].Players = new List<Player>();
                                    //try
                                    //{


                                    //    foreach (var player in teamA)
                                    //    {
                                    //        Player objPlayer = new Player();
                                    //        objPlayer.PlayerName = match.players[player.Value.ToString()].fullname.Value;
                                    //        objPlayer.PlayerKey = player.Value.ToString();
                                    //        //try
                                    //        //{
                                    //        //    objPlayer.PlayerBetting = match.players[player.Value.ToString()].match.innings["1"].batting;
                                    //        //    objPlayer.PlayerBowling = match.players[player.Value.ToString()].match.innings["1"].bowling;
                                    //        //    objPlayer.PlayerFeilding = match.players[player.Value.ToString()].match.innings["1"].fielding;
                                    //        //}
                                    //        //catch (System.Exception ex)
                                    //        //{

                                    //        //}
                                    //        if (match.players[player.Value.ToString()].identified_roles.batsman.Value == true && match.players[player.Value.ToString()].identified_roles.bowler.Value == true)
                                    //        {
                                    //            objPlayer.PlayerRoll = "All rounder";
                                    //        }
                                    //        else if (match.players[player.Value.ToString()].identified_roles.batsman.Value == false && match.players[player.Value.ToString()].identified_roles.bowler.Value == true)
                                    //        {
                                    //            objPlayer.PlayerRoll = "Bowler";
                                    //        }
                                    //        else if (match.players[player.Value.ToString()].identified_roles.batsman.Value == true && match.players[player.Value.ToString()].identified_roles.bowler.Value == false)
                                    //        {
                                    //            objPlayer.PlayerRoll = "Batsman";
                                    //        }
                                    //        else if (match.players[player.Value.ToString()].identified_roles.keeper.Value == true)
                                    //        {
                                    //            objPlayer.PlayerRoll = "Keeper";
                                    //        }
                                    //        if (match.teams["a"].match.captain == player.Value.ToString())
                                    //        {
                                    //            objPlayer.PlayerRoll = objPlayer.PlayerRoll + " (C)";
                                    //        }
                                    //        if (match.teams["a"].match.keeper == player.Value.ToString())
                                    //        {
                                    //            objPlayer.PlayerRoll = objPlayer.PlayerRoll + " (WK)";
                                    //        }
                                    //        objScoreCard.Teams[0].Players.Add(objPlayer);
                                    //    }
                                    //    foreach (var player in teamB)
                                    //    {
                                    //        Player objPlayer = new Player();
                                    //        objPlayer.PlayerName = match.players[player.Value.ToString()].fullname.Value;
                                    //        objPlayer.PlayerKey = player.Value.ToString();
                                    //        //try
                                    //        //{
                                    //        //    objPlayer.PlayerBetting = match.players[player.Value.ToString()].match.innings["1"].batting;
                                    //        //    objPlayer.PlayerBowling = match.players[player.Value.ToString()].match.innings["1"].bowling;
                                    //        //    objPlayer.PlayerFeilding = match.players[player.Value.ToString()].match.innings["1"].fielding;
                                    //        //}
                                    //        //catch(System.Exception ex)
                                    //        //{

                                    //        //}
                                    //        if (match.players[player.Value.ToString()].identified_roles.batsman.Value == true && match.players[player.Value.ToString()].identified_roles.bowler.Value == true)
                                    //        {
                                    //            objPlayer.PlayerRoll = "All rounder";
                                    //        }
                                    //        else if (match.players[player.Value.ToString()].identified_roles.batsman.Value == false && match.players[player.Value.ToString()].identified_roles.bowler.Value == true)
                                    //        {
                                    //            objPlayer.PlayerRoll = "Bowler";
                                    //        }
                                    //        else if (match.players[player.Value.ToString()].identified_roles.batsman.Value == true && match.players[player.Value.ToString()].identified_roles.bowler.Value == false)
                                    //        {
                                    //            objPlayer.PlayerRoll = "Batsman";
                                    //        }
                                    //        else if (match.players[player.Value.ToString()].identified_roles.keeper.Value == true)
                                    //        {
                                    //            objPlayer.PlayerRoll = "Keeper";
                                    //        }
                                    //        if (match.teams["b"].match.captain == player.Value.ToString())
                                    //        {
                                    //            objPlayer.PlayerRoll = objPlayer.PlayerRoll + " (C)";
                                    //        }
                                    //        if (match.teams["b"].match.keeper == player.Value.ToString())
                                    //        {
                                    //            objPlayer.PlayerRoll = objPlayer.PlayerRoll + " (WK)";
                                    //        }
                                    //        objScoreCard.Teams[1].Players.Add(objPlayer);
                                    //    }
                                    //}
                                    //catch (System.Exception ex)
                                    //{

                                    //}
                                    objScoreCard.RecentOverBalls = new List<string>();
                                    foreach (var ball in stops)
                                    {
                                        if (ball.Value == "e1,wd")
                                        {
                                            objScoreCard.RecentOverBalls.Add("1 wd");
                                        }
                                        else if (ball.Value == "e2,wd")
                                        {
                                            objScoreCard.RecentOverBalls.Add("2wd");
                                        }
                                        else if (ball.Value == "e3,wd")
                                        {
                                            objScoreCard.RecentOverBalls.Add("3wd");
                                        }
                                        else if (ball.Value == "b4;e1,nb")
                                        {
                                            objScoreCard.RecentOverBalls.Add("4 & 1nb");
                                        }
                                        else if (ball.Value == "e4,lb")
                                        {
                                            objScoreCard.RecentOverBalls.Add("4 & 1nb");
                                        }
                                        else if (ball.Value == "b4;e1,nb")
                                        {
                                            objScoreCard.RecentOverBalls.Add("4 lb");
                                        }
                                        else if (ball.Value == "e1,lb")
                                        {
                                            objScoreCard.RecentOverBalls.Add("1 lb");
                                        }
                                        else if (ball.Value == "b")
                                        {
                                            objScoreCard.RecentOverBalls.Add("");
                                        }
                                        else if (ball.Value == "r")
                                        {
                                            objScoreCard.RecentOverBalls.Add("");
                                        }
                                        else if (ball.Value == "r1;w")
                                        {
                                            objScoreCard.RecentOverBalls.Add("1 & W");
                                        }
                                        else if (ball.Value == "r1;e1,nb")
                                        {
                                            objScoreCard.RecentOverBalls.Add("1 & 1 nb");
                                        }
                                        else if (ball.Value == "r0")
                                        {
                                            objScoreCard.RecentOverBalls.Add("0");
                                        }
                                        else if (ball.Value == "r1")
                                        {
                                            objScoreCard.RecentOverBalls.Add("1");
                                        }
                                        else if (ball.Value == "r2")
                                        {
                                            objScoreCard.RecentOverBalls.Add("2");
                                        }
                                        else if (ball.Value == "r3")
                                        {
                                            objScoreCard.RecentOverBalls.Add("3");
                                        }
                                        else if (ball.Value == "b4")
                                        {
                                            objScoreCard.RecentOverBalls.Add("4");
                                        }
                                        else if (ball.Value == "b6")
                                        {
                                            objScoreCard.RecentOverBalls.Add("6");
                                        }
                                        else if (ball.Value == "w")
                                        {
                                            objScoreCard.RecentOverBalls.Add("W");
                                        }

                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                                try
                                {


                                    if (bowlerkey != null)
                                    {


                                        objScoreCard.BowlerStr = match.players[bowlerkey].name.Value + ":" + match.players[bowlerkey].match.innings["1"].bowling.runs.Value + "/" + match.players[bowlerkey].match.innings["1"].bowling.wickets.Value + " in " + match.players[bowlerkey].match.innings["1"].bowling.overs.Value;
                                    }
                                }
                                catch (System.Exception ex)
                                {

                                }
                                if (MatchScoresAll.AllMatchScoreCards.Count > 0)
                                {
                                    var currScoreCard = MatchScoresAll.AllMatchScoreCards.Where(item => item.MatchKey == strmatchkey).FirstOrDefault();
                                    if (currScoreCard != null)
                                    {
                                        currScoreCard = objScoreCard;
                                    }
                                    else
                                    {
                                        MatchScoresAll.AllMatchScoreCards.Add(objScoreCard);
                                    }

                                }
                                else
                                {
                                    MatchScoresAll.AllMatchScoreCards.Add(objScoreCard);
                                }
                                //string[] recentballsarr = match.now.recent_overs_str[0][1].ToString().Split(',');

                                //if (match.allInnings[(match.now.bowling_team+ '_'+match.now.innings)].batting_order.length > 0)
                                //{
                                // txt+=   match.teams[match.now.bowling_team].key + ":"+ match.innings[match.now.bowling_team + '_' + match.now.innings].runs+"/"+ match.innings[match.now.bowling_team + '_' + match.now.innings].wickets + " in " + match.innings[match.now.bowling_team + '_' + match.now.innings].overs;

                                //}
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //string teamname = match.teams[match.now.batting_team].key;
                        }

                    }
                    else
                    {
                        GetSessionFromCricketAPI();
                    }
                }
            }
            catch (System.Exception ex)
            {
                APIConfig.LogError(ex);
            }


        }
        public void UpdateGetDataFromForLoggingData(string EventID, string GetDataFrom)
        {
            dbEntities.SP_UserMarket_UpdateGetMatchUpdatesFrom(EventID, GetDataFrom);
        }
        public int GetScoreFromCricketAPI(string strmatchkey, string innings, string overs)
        {
            int Scoreforthisoverandinnings = 0;
            try
            {



                if (CricketAPIConfig.AccessKey == "")
                {
                    SP_CricketAPIConfig_GetData_Result objCricketAPIConfig = dbEntities.SP_CricketAPIConfig_GetData().FirstOrDefault();
                    CricketAPIConfig.AccessKey = objCricketAPIConfig.AccessKey;
                    CricketAPIConfig.AppID = objCricketAPIConfig.AppID;
                    CricketAPIConfig.DeviceID = objCricketAPIConfig.DeviceID;
                    CricketAPIConfig.SecretKey = objCricketAPIConfig.SecretKey;
                    CricketAPIConfig.SessionKey = objCricketAPIConfig.SessionKey;
                }
                LZWebClient recentballClient = new LZWebClient();
                string oversummary = recentballClient.DownloadString("https://rest.cricketapi.com/rest/v2/match/" + strmatchkey + "/overs_summary/?access_token=" + CricketAPIConfig.SessionKey);

                OverSummary objOverSummary = JsonConvert.DeserializeObject<OverSummary>(oversummary);
                if (objOverSummary.status_code == 200)
                {
                    string battinginnings = "";
                    if (innings == "1")
                    {
                        battinginnings = objOverSummary.data.batting_order[0][0].ToString() + "_" + objOverSummary.data.batting_order[0][1].ToString();
                        if (battinginnings == "b_1")
                        {
                            var requirematchoversummary = objOverSummary.data.innings.b_1.overs_summary.Where(item => item.over == Convert.ToInt32(overs)).FirstOrDefault();
                            if (requirematchoversummary != null)
                            {
                                string[] runsandscore = requirematchoversummary.match.score.Split(new string[] { " in " }, StringSplitOptions.None);
                                if (Convert.ToDecimal(runsandscore[1]) == Convert.ToDecimal(overs))
                                {
                                    Scoreforthisoverandinnings = requirematchoversummary.match.runs;
                                }
                                else
                                {
                                    string[] wickets = runsandscore[0].Split('/');
                                    if (wickets[1] == "10")
                                    {
                                        Scoreforthisoverandinnings = requirematchoversummary.match.runs;
                                    }

                                }
                            }
                            else
                            {
                                Scoreforthisoverandinnings = objOverSummary.data.innings.b_1.overs_summary.LastOrDefault().match.runs;
                            }
                        }
                        else
                        {
                            //  Scoreforthisoverandinnings = objOverSummary.data.innings.a_1.overs_summary.Where(item => item.over == Convert.ToInt32(overs)).FirstOrDefault().match.runs;
                            var requirematchoversummary = objOverSummary.data.innings.a_1.overs_summary.Where(item => item.over == Convert.ToInt32(overs)).FirstOrDefault();
                            if (requirematchoversummary != null)
                            {
                                string[] runsandscore = requirematchoversummary.match.score.Split(new string[] { " in " }, StringSplitOptions.None);
                                if (Convert.ToDecimal(runsandscore[1]) == Convert.ToDecimal(overs))
                                {
                                    Scoreforthisoverandinnings = requirematchoversummary.match.runs;
                                }
                                else
                                {
                                    string[] wickets = runsandscore[0].Split('/');
                                    if (wickets[1] == "10")
                                    {
                                        Scoreforthisoverandinnings = requirematchoversummary.match.runs;
                                    }

                                }
                            }
                            else
                            {
                                Scoreforthisoverandinnings = objOverSummary.data.innings.a_1.overs_summary.LastOrDefault().match.runs;
                            }
                        }

                    }
                    else
                    {
                        battinginnings = objOverSummary.data.batting_order[1][0].ToString() + "_" + objOverSummary.data.batting_order[1][1].ToString();
                        if (battinginnings == "b_1")
                        {
                            var requirematchoversummary = objOverSummary.data.innings.b_1.overs_summary.Where(item => item.over == Convert.ToInt32(overs)).FirstOrDefault();
                            if (requirematchoversummary != null)
                            {
                                string[] runsandscore = requirematchoversummary.match.score.Split(new string[] { " in " }, StringSplitOptions.None);
                                if (Convert.ToDecimal(runsandscore[1]) == Convert.ToDecimal(overs))
                                {
                                    Scoreforthisoverandinnings = requirematchoversummary.match.runs;
                                }
                                else
                                {
                                    string[] wickets = runsandscore[0].Split('/');
                                    if (wickets[1] == "10")
                                    {
                                        Scoreforthisoverandinnings = requirematchoversummary.match.runs;
                                    }

                                }
                            }
                            else
                            {
                                Scoreforthisoverandinnings = objOverSummary.data.innings.b_1.overs_summary.LastOrDefault().match.runs;
                            }
                        }
                        else
                        {
                            //  Scoreforthisoverandinnings = objOverSummary.data.innings.a_1.overs_summary.Where(item => item.over == Convert.ToInt32(overs)).FirstOrDefault().match.runs;
                            var requirematchoversummary = objOverSummary.data.innings.a_1.overs_summary.Where(item => item.over == Convert.ToInt32(overs)).FirstOrDefault();
                            if (requirematchoversummary != null)
                            {
                                string[] runsandscore = requirematchoversummary.match.score.Split(new string[] { " in " }, StringSplitOptions.None);
                                if (Convert.ToDecimal(runsandscore[1]) == Convert.ToDecimal(overs))
                                {
                                    Scoreforthisoverandinnings = requirematchoversummary.match.runs;
                                }
                                else
                                {
                                    string[] wickets = runsandscore[0].Split('/');
                                    if (wickets[1] == "10")
                                    {
                                        Scoreforthisoverandinnings = requirematchoversummary.match.runs;
                                    }

                                }
                            }
                            else
                            {
                                Scoreforthisoverandinnings = objOverSummary.data.innings.a_1.overs_summary.LastOrDefault().match.runs;
                            }
                        }
                        //if (battinginnings == "b_1")
                        //{
                        //    Scoreforthisoverandinnings = objOverSummary.data.innings.b_1.overs_summary.Where(item => item.over == Convert.ToInt32(overs)).FirstOrDefault().match.runs;
                        //}
                        //else
                        //{
                        //    Scoreforthisoverandinnings = objOverSummary.data.innings.a_1.overs_summary.Where(item => item.over == Convert.ToInt32(overs)).FirstOrDefault().match.runs;
                        //}
                    }
                }
                else
                {
                    GetSessionFromCricketAPI();
                    //GetScoreFromCricketAPI(strmatchkey, innings, overs);

                }
            }
            catch (System.Exception ex)
            {
                Scoreforthisoverandinnings = 0;
            }
            return Scoreforthisoverandinnings;

        }
        public void GetSessionFromCricketAPI()
        {
            // string currentsession = "2s150253235610869s896703613236300235";
            if (CricketAPIConfig.AccessKey == "")
            {
                SP_CricketAPIConfig_GetData_Result objCricketAPIConfig = dbEntities.SP_CricketAPIConfig_GetData().FirstOrDefault();
                CricketAPIConfig.AccessKey = objCricketAPIConfig.AccessKey;
                CricketAPIConfig.AppID = objCricketAPIConfig.AppID;
                CricketAPIConfig.DeviceID = objCricketAPIConfig.DeviceID;
                CricketAPIConfig.SecretKey = objCricketAPIConfig.SecretKey;
                CricketAPIConfig.SessionKey = objCricketAPIConfig.SessionKey;
            }
            using (WebClient client = new WebClient())
            {

                byte[] response =
                client.UploadValues("https://rest.cricketapi.com/rest/v2/auth/", new NameValueCollection()
                {
           { "access_key", CricketAPIConfig.AccessKey },
           { "secret_key",CricketAPIConfig.SecretKey },
                    {"app_id",CricketAPIConfig.AppID },
                    {"device_id",CricketAPIConfig.DeviceID }
                });

                string result = System.Text.Encoding.UTF8.GetString(response);

                try
                {
                    AuthDescription objAuth = JsonConvert.DeserializeObject<AuthDescription>(result);
                    if (objAuth.status_code == 200)
                    {
                        CricketAPIConfig.SessionKey = objAuth.auth.access_token;
                        dbEntities.SP_CricketAPIConfig_UpdateSessionKey(objAuth.auth.access_token);
                    }
                }
                catch (System.Exception ex)
                {

                }
            }


        }
        public APIResponse GetMatchScoreCard(string strMatchKey, string Password)
        {
            if (ValidatePassword(Password) == false)
            {
                return new APIResponse();
            }
            if (MatchScoresAll.AllAPIResponse.Count > 0)
            {
                var currScoreCard = MatchScoresAll.AllAPIResponse.Where(item => item.matchkey == strMatchKey).FirstOrDefault();
                if (currScoreCard != null)
                {
                    return currScoreCard;
                }
                else
                {
                    return new APIResponse();
                }
            }
            else
            {
                return new APIResponse();
            }
        }
        public string GetAllPendingAmountsbyDate(DateTime DueDate)
        {
            return ConverttoJSONString(dbEntities.SP_AmountReceiveables_GetDatabyDate(DueDate).ToList());
        }
        public void AddAmountReceviables(int UserId, decimal Amount, DateTime DueDate, string Status, decimal AmountReceived)
        {
            dbEntities.SP_AmountReceiveables_Insert(UserId, Amount, DueDate, Status, AmountReceived);

        }
        public void UpdateAmountReceviables(int ID, decimal Amount, DateTime DueDate, string Status)
        {
            dbEntities.SP_AmountReceiveables_Update(ID, Amount, DueDate, Status);

        }
        public bool GetBettingAllowedbyMarketIDandUserID(int UserId, string MarketBookID)

        {
            return dbEntities.SP_UserMarket_GetMarketForAllowedBettingbtMarketIDandUserID(UserId, MarketBookID).FirstOrDefault().Value;
        }
        public bool GetBettingAllowedbyMarketIDandUserIDInplay(int UserId)
        {
            return dbEntities.SP_UserMarket_GetMarketForAllowedBettingbtMarketIDandUserIDInPlay(UserId).FirstOrDefault().Value;
        }
        public void AddReferrerUsers(int UserID, int ReferrerID, int ReferrerRate)
        {
            dbEntities.SP_Referrers_Insert(UserID, ReferrerID, ReferrerRate);
        }
        public void DeletReffererUSers(int UserID)
        {
            dbEntities.SP_Referrers_DeletebyUserID(UserID);
        }
        public List<SP_Referrers_GetReferrerRateandReferrerIDbyUserID_Result> GetReferrerRatesbyUserID(int UserID)
        {
            return dbEntities.SP_Referrers_GetReferrerRateandReferrerIDbyUserID(UserID).ToList();
        }
        public void InsertUserBetNew(decimal userodd, string SelectionID, string Selectionname, string BetType, string nupdownAmount, string betslipamountlabel, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay, int Clickedlocation, int UserID, string Betslipsize, string Password, string marketbookId, string Marketbookname, bool GetData)
        {
            try
            {
                UserBetInsertionHelper objUserBetHelper = new UserBetInsertionHelper();
                objUserBetHelper.insertbetslip(userodd, SelectionID, Selectionname, BetType, nupdownAmount, betslipamountlabel, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, Clickedlocation, UserID, Betslipsize, Password, marketbookId, Marketbookname, true);
            }
            catch (System.Exception ex)
            {

                //  UserBetInsertionHelper objUserBetHelper = new UserBetInsertionHelper();
                //  objUserBetHelper.insertbetslip(userodd, SelectionID, Selectionname, BetType, nupdownAmount, betslipamountlabel, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, Clickedlocation, UserID, Betslipsize, Password, marketbookId, Marketbookname, true);
                APIConfig.LogError(ex);
                APIConfig.WriteErrorToDB(ex.Message);
            }
            //finally
            //{
            //    UserBetInsertionHelper objUserBetHelper = new UserBetInsertionHelper();
            //    objUserBetHelper.insertbetslip(userodd, SelectionID, Selectionname, BetType, nupdownAmount, betslipamountlabel, MaxOddBack, MaxOddLay, CheckforMaxOddBack, CheckforMaxOddLay, Clickedlocation, UserID, Betslipsize, Password, marketbookId, Marketbookname, true);
            //    APIConfig.LogError(ex);
            //}

        }
        public string GetDistinctMarketsFromBets(string From, string To)
        {
            return ConverttoJSONString(dbEntities.SP_UserBets_GetDistinctMarket(From, To).ToList());
        }
        public string GetDistinctMarketsFromAccounts(string From, string To)
        {
            return ConverttoJSONString(dbEntities.SP_UserAccounts_GetDistinctMarket(From, To).ToList());
        }
        public bool UnPostUserAccountsbyUserIDandMarketID(string MarketBookId, int UserID, string Password)
        {
            var useraccounts = dbEntities.SP_UserAccounts_GetDatabyMarketID(MarketBookId).ToList();
            var useraccountsbyUserID = new List<SP_UserAccounts_GetDatabyMarketID_Result>();
            if (UserID > 0)
            {
                useraccountsbyUserID = useraccounts.Where(item => item.UserID == UserID).ToList();
            }
            else
            {
                useraccountsbyUserID = useraccounts;
            }
            foreach (var item in useraccountsbyUserID)
            {
                if (Convert.ToDecimal(item.Debit) > 0)
                {
                    var results = objAccountsService.AddtoUsersAccounts(item.AccountsTitle, "0.00", item.Debit, item.UserID.Value, item.MarketBookID, DateTime.Now, item.AgentRate, item.SuperRate,"", item.ComissionRate, Convert.ToDecimal(GetCurrentBalancebyUser(item.UserID.Value, Password)), false, item.EventType, item.WinnerName, item.EventID, item.EventName, item.MareketBookNameAcc);
                    AddCredittoUser(-1 * Convert.ToDecimal(item.Debit), item.UserID.Value, 1, DateTime.Now, 0, false, "", false, Password);
                }
                else
                {
                    var results = objAccountsService.AddtoUsersAccounts(item.AccountsTitle, item.Credit, "0.00", item.UserID.Value, item.MarketBookID, DateTime.Now, item.AgentRate,item.SuperRate,"", item.ComissionRate, Convert.ToDecimal(GetCurrentBalancebyUser(item.UserID.Value, Password)), false, item.EventType, item.WinnerName, item.EventID, item.EventName, item.MareketBookNameAcc);
                    AddCredittoUser(Convert.ToDecimal(item.Credit), item.UserID.Value, 1, DateTime.Now, 0, false, "", false, Password);
                }
            }

            return true;
        }
        public bool UpdateUserBetsStatusbyMarketIDandUserID(string MarketBookId, int UserID, string Password)
        {
            dbEntities.SP_UserBets_UpdateStatustoInCompletebyMarketIDandUserID(MarketBookId, UserID);
            return true;
        }
        public void UpdateTotalOversbyMarket(string EventID, string TotalOvers)
        {
            dbEntities.SP_UserMarket_UpdateTotalOvers123(EventID, TotalOvers);
        }
        public void UpdateAllMarketClosedbyUserID(int UserID)
        {
            dbEntities.SP_UserMarket_SetAllMarketCatalogueIDClosedByUser(UserID);
        }
        public string GetURLsData()
        {
            return ConverttoJSONString(dbEntities.SP_URLsData_GetAllData().ToList());
        }
        public bool GetTransferAgnetCommision(int UserID)
        {
            return dbEntities.SP_Users_GetTransferAgentCommission(UserID).FirstOrDefault().Value;
        }
        public void UpdateTransferAgnetCommision(int UserID, bool TranserAgentCommision)
        {
            dbEntities.SP_Users_UpdateTransferAgentCommission(UserID, TranserAgentCommision);
        }
        public int GetMaxBalanceTransferLimit(int UserID)
        {
            return dbEntities.SP_Users_GetMaxBalanceTransferLimit(UserID).FirstOrDefault().Value;
        }
        public void UpdateMaxBalanceTransferLimit(int UserID, int MaxBalanceTransferLimit)
        {
            dbEntities.SP_Users_UpdateMaxBalanceTransferLimit(UserID, MaxBalanceTransferLimit);
        }
        public int GetMaxAgentRate(int UserID)
        {
            return dbEntities.SP_Users_GetMaxMaxAgentRate(UserID).FirstOrDefault().Value;
        }
        public int GetSuperRate(int UserID)
        {
            return Convert.ToInt32(dbEntities.SP_Users_GetSuperRate(UserID).FirstOrDefault());
        }
        public int GetSamiadminRate(int UserID)
        {
            return Convert.ToInt32(dbEntities.SP_Users_GetSamiadminRate(UserID).FirstOrDefault());
        }
        public int GetAhmadRate(int UserID)
        {
            return dbEntities.SP_Users_GetAhmadRate(UserID).FirstOrDefault().Value;
        }
        public void UpdateMaxAgentRate(int UserID, int MaxAgentRate)
        {
            dbEntities.SP_Users_UpdateMaxAgentRate(UserID, MaxAgentRate);
        }
       
        public void UpdateAhmadRate(int UserID, int AhmadRate)
        {
            dbEntities.SP_Users_UpdateAhmadRate(UserID, AhmadRate);
        }
        public void UpdateSuperRate(int UserID, int SuperRate)
        {
            dbEntities.SP_Users_UpdateSuperAgentRate(UserID, SuperRate);
        }
        public void UpdateFancySyncONorOFF(int UserId, string EventID, bool isopenenedbyuser)
        {
            dbEntities.SP_UserMarket_SetMarketCatalogueIDOpendorClosedByEventIDLine(UserId, EventID, isopenenedbyuser);
        }
        public void UpdateFancySyncONorOFFbyMarketID(int UserId, string MarektID, bool isopenenedbyuser)
        {
            dbEntities.SP_UserMarket_SetMarketCatalogueIDOpendorClosedByMarketIDLine(UserId, MarektID, isopenenedbyuser);
        }

        public void UpdateKJSyncONorOFFbyMarketID(int UserId, string MarektID, bool isopenenedbyuser)
        {
            dbEntities.SP_UserMarket_SetMarketCatalogueIDOpendorClosedByMarketIDKJs(UserId, MarektID, isopenenedbyuser);
        }

        
        public decimal GetTotalAgentCommissionbyAgentID(int UserId, string Password)
        {
            return dbEntities.SP_UserAccounts_GetDatabyAgentIDForCommision(UserId, false).FirstOrDefault().Value;
        }
        public SP_UserMarket_GetToWinTheTossbyEventID_Result GetToWintheTossbyeventId(int UserId,string EventId)
        {
            return dbEntities.SP_UserMarket_GetToWinTheTossbyEventID(EventId, DateTime.Now, UserId).FirstOrDefault();
        }
        public SP_UserMarket_GetToTiedMarketbyEventID_Result GetToTiedMarketbyEventID(int UserId, string EventId)
        {
            return dbEntities.SP_UserMarket_GetToTiedMarketbyEventID(EventId, DateTime.Now, UserId).FirstOrDefault();
        }
        public List<SP_UserMarket_GetSoccergoalbyEventID_Result> GetSoccergoalbyeventId(int UserId, string EventId)
        {
            return dbEntities.SP_UserMarket_GetSoccergoalbyEventID(EventId, DateTime.Now, UserId).ToList();
        }
        public void SetMarketOpenedbyuserinAPP()
        {
            try
            {
                OpenMarkets.OpenMarketlst = dbEntities.SP_UserMarket_GetDistinctMarketsOpenedNew().ToList();
                OpenMarketsFancy.OpenMarketlstfancy = dbEntities.SP_UserMarket_GetDistinctMarketsOpenedNewFancy().ToList();
            }
            catch (Exception)
            {

              
            }
        }

    }
}
