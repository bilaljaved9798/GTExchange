
using bfnexchange.Services.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Script.Services;

namespace bfnexchange.Services.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUserServices" in both code and config file together.
    [ServiceContract]
    public interface IUserServices
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
        string GetUserbyUsernameandPassword(string username, string password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetUserbyUsernameandPasswordNew(string username, string password);       
        [OperationContract]
        void UpdateCurrentLoggedInIDbyUserID(int userID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        void AddUserActivity(string Activityname, DateTime ActivityTime, string IPAddress, string Location, string Deviceinfo, int userID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAccessRightsbyUserType(int UserTypeID, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string AddUser(string Name, string Phonenumber, string EmailAddress, string username, string Password, string Location, decimal Accountbalance, int usertypeID, int createdByID, string RatePercent, decimal BetLowerLimit, decimal BetUpperLimit, bool CheckConditionsforPlacingBet, decimal BetLowerLimitHorsePlace, decimal BetUpperLimitHorsePlace, decimal BetLowerLimitGrayHoundWin, decimal BetUpperLimitGrayHoundWin, decimal BetLowerLimitGrayHoundPlace, decimal BetUpperLimitGrayHoundPlace, decimal BetLowerLimitMatchOdds, decimal BetUpperLimitMatchOdds, decimal BetLowerLimitInningsRunns, decimal BetUpperLimitInningsRunns, decimal BetLowerLimitCompletedMatch, decimal BetUpperLimitCompletedMatch, decimal BetLowerLimitMatchOddsSoccer, decimal BetUpperLimitMatchOddsSoccer, decimal BetLowerLimitMatchOddsTennis, decimal BetUpperLimitMatchOddsTennis, decimal BetUpperLimitTiedMatch, decimal BetLowerLimitTiedMatch, decimal BetUpperLimitWinner, decimal BetLowerLimitWinner, string Passwordforvalidate, decimal BetUpperLimitFancy, decimal BetLowerLimitFancy);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string CheckifUserExists(string username);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        void AddCredittoUser(decimal Amount, int userID, int AddedbyID, DateTime addedtime, decimal Amountremoved, bool AddtoUserAccounts, string AccountsTitle, bool isCreditAmount, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        void AddAdminAmountForSuper(string AccountsTitle, string debit, string Credit, int userID, string marketid, DateTime addedtime, string Password);
        [OperationContract]
        void UpdateStartBalancebyUserID(int userID, decimal newBalance, string Password);
        [OperationContract]
        void UpdateAccountsOpeningBalance(int userID, decimal Balance, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetCurrentBalancebyUser(int userid, string Password);
        [OperationContract]
        decimal GetStartingBalance(int UserID, string Password);
        [OperationContract]
        void AddKalijut(Nullable<int> userID, string eventTypeID, string competitonID, string eventID, string marketCatalogue, Nullable<int> updatedbyID, string eventTypeName, string competitionName, string eventName, string marketCatalogueName, Nullable<System.DateTime> eventOpenDate, string sheetName, string associateEventID);
        [OperationContract]
        void MarketCatalogueSelectionskalijut(string marketCatalogueID, string selectionID, string selectionName, string jockeyName, string wearing, string wearingDesc, string clothnumber, string stallDraw); 
        [OperationContract]
        void UpdateAccountBalacnebyUser(int userid, decimal AccountBalance, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAllCuttingUsers(string Passwordforvalidate);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAllUsersbyUserType(int userID, int usertypeID, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAllUsersbyUserTypeNew(int userID, int usertypeID, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetUserDetailsbyID(int userID, string Password);
        [OperationContract]
        string GetMarketbyEventID(string EventID);
        [OperationContract]
        string GetMarketbyEventID1(string EventID);
        [OperationContract] 
        string SetDeleteStatusofUser(int UserID, bool isDeleted, string Password);
        [OperationContract]
        string SetBlockedStatusofUser(int UserID, bool isBlocked, string Password);
        [OperationContract]
        string ResetPasswordofUser(int UserID, string Password, int Updatedby, DateTime updatedtime, string Passwordforvalidate);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetUserMArket(int userID);
        [WebInvoke(Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        string InsertUserMarket(string[] allmarketitems, int userID, int UpdatedbyID, bool DeleteOldMarkets);
        [OperationContract]
        string InsertUserMarketKJ(string[] kalijutt, string[] KaliCatelogIDs, string[] selections, int[] userIDs,string  EventID, int UpdatedbyID);
        [OperationContract]
        string InsertUserMarketSFig(string[] SmallFig, string[] smallFigCatelogIDs, string[] selections, int[] userIDs, string EventID, int UpdatedbyID);
        [OperationContract]       
        string InsertUserMarketFigure(string[] Figure, string[] FigureCatelogIDs, string[] Figureselections, string[] FigureselectionsName, int[] userIDs, string EventID, int UpdatedbyID);
        [OperationContract]
        //string GetUserMArket(int userID);
        [WebInvoke(Method = "POST",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json)]
        string InsertIndainFancy(ExternalAPI.TO.MarketBookForindianFancy[] allmarkets, List<ExternalAPI.TO.RunnerForIndianFancy> runners, int[] userIDs,string EventID, string Password);
        [OperationContract]

        string GetUserMarketforSelection(int UserID);
        [OperationContract]
        string GetFavoriteEventTypes(int userID);
        [OperationContract]
        void AddtoFavoriteEventTypes(string EventTypeID, int userID);
        [OperationContract]
        void DeleteFromFavoriteEventTypes(string EventTypeID, int userID);
        [OperationContract]
        string GetFavoriteEvents(int userID);
        [OperationContract]
        void AddtoFavoriteEvents(string EventID, int userID);
        [OperationContract]
        void DeleteFromFavoriteEvents(string EventID, int userID);
        [OperationContract]
        string GetFavoriteCompetitions(int userID);
        [OperationContract]
        void AddtoFavoriteCompetitions(string EventID, int userID);
        [OperationContract]
        void DeleteFromFavoriteCompetitions(string EventID, int userID);
        [OperationContract]
        string GetEventTypeIDs(int userID);
        [OperationContract] 
        string getlistuserids(int createdid);
        [OperationContract]
        string GetCompetitionIDs(string eventTypeID, int userID);
        [OperationContract]
        string GetEventsIDs(string CompetitionID, int userID);
        [OperationContract]
        string GetMarketCatalogueIDs(string eventID, int userID);
        [OperationContract]
        string InsertUserBet(string SelectionID, int userID, string UserOdd, string amount, string bettype, string LiveOdd, bool ismatched, string status, string marketbookId, DateTime createddate, DateTime updatedtime, string Selectionname, string Marketbookname, string Liability, string BetSize, decimal PendingAmount, string location, long ParentID, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay, string Password);
        [OperationContract]
        string InsertUserBetAdmin(string SelectionID, int userID, string UserOdd, string amount, string bettype, string LiveOdd, bool ismatched, string status, string marketbookId, DateTime createddate, DateTime updatedtime, string Selectionname, string Marketbookname, string Liability, string BetSize, decimal PendingAmount, string location, long ParentID, string Password);
        [OperationContract]
        bool UpdateUserBet(long ID, int userID, string UserOdd, string amount, string bettype, string LiveOdd, bool ismatched, string status, string marketbookId, DateTime createddate, DateTime updatedtime, string Liabality, string BetSize, decimal PendingAmount, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetUserbetsbyUserIDandMarketID(int UserID, string MarketID, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        SP_Users_GetMaxOddBackandLay_Result GetMaxOddBackandLay(int UserID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetMaxOddBackandLayStr(int UserID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetUserbetsbyUserID(int UserID, string Password);
        [OperationContract]
        void UpdateUserBetMatched(long[] ID, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetCurrentLiabality(int userID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAgentRate(int userID);
        [OperationContract]
        string GetSuperName(int userID);
        [OperationContract]
        void CheckforMatchCompleted();
        [OperationContract]
        void CloseAllClosedMarkets();
        [OperationContract]
        string SetAgentRateofUser(int UserID, string AgentRate, string Password);
        [OperationContract]
        void UpdateUserBetUnMatchedStatusTocomplete(long[] ID, string Password);
        [OperationContract]
        void UpdateUserBetUnMatchedStatusTocompleteforCuttingUser(long ID, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetMarketsOpenedbyUser(int userID);
        [OperationContract]
        void SetMarketBookOpenbyUSer(int userID, string MarketBookID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string SetMarketBookOpenbyUSerandGet(int userID, string MarketBookID);
        [OperationContract]
        string SetMarketBookOpenbyUSerandGet0(int userID, string MarketBookID);
        [OperationContract]
        string SetMarketBookOpenbyUSerandGet1(int userID, string MarketBookID);
        [OperationContract]
        string SetMarketBookOpenbyUSerandGet2(int userID, string MarketBookID);
        [OperationContract]
        void SetMarketBookClosedbyUser(int userID, string MarketBookID);
        [OperationContract]
        void SetMarketClosedAllUsers(string MarketbookID);
        [OperationContract]
        List<SP_MarketCatalogueSelections_Get_Result> GetSelectionNamesbyMarketID(string MarketID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAccountsDatabyUserIDandDateRange(int userID, string From, string To, bool isCreditAmount, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAccountsDatabyCreatedByID(int userID, bool isCreditAmount, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAccountsDatabyCreatedByIDForSuper(int userID, bool isCreditAmount, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAccountsDatabyCreatedByIDForSamiAdmin(int userID, bool isCreditAmount, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAccountsDataForAdmin(int UserID, bool isCreditAmount, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        decimal GetProfitorLossbyUserID(int userID, bool isCreditAmount, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        decimal GetProfitorLossforSuper(int userID, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAccountsDataForCommisionaccount(string Password);
        [OperationContract]
        void DownloadAllMarketHorseRace(string Password);
        [OperationContract]
        void DownloadAllMarketGrayHoundRace(string Password);
        [OperationContract]
        bool UpdateUserbetamountbyID(long ID, decimal amount, bool ismatched, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetInPlayMatches(int userID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetInPlayMatcheswithRunners(int userID);
        [OperationContract]
        string GetInPlayMatcheswithRunners1(int userID);
        [OperationContract]
        
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAllMatches(int userID);
        [OperationContract]
        string GetAllCricketMatches(int userID);
        [OperationContract]
        string GetAllSoccerMatches(int userID);
        [OperationContract]
        string GetAllTennisMatches(int userID);
        [OperationContract]
        bool UpdateUserbetamountbyParentID(long ID, decimal amount, string userodd, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetUserBetsbyAgentID(int AgentID, string Password);
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetUserBetsbySuperID(int SuperID, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetUserBetsbySamiAdmin(int SuperID, string Password);
        [OperationContract]
        
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetUserBetsbyAgentIDwithZeroReferer(int AgentID, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetUserbetsbyUserIDandAgentID(int AgentID, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetUserbetsForAdmin(string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetMarketsOpenedbyUsersofAgent(int AgentID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetMarketsOpenedbyUsersForAdmin();
        [OperationContract]
        void SetLoggedinStatus(int userID, bool LoggedIn);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetCompletedMatchedBetsbyUserID(int UserID, string MarketbookID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAllUserMarketbyUserID(int userID);
        [OperationContract]
        string GetUserStatus(int UserID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetLastLoginTimes(int UserID);
        [OperationContract]
        void UpdateBetSizebyID(long ID, string BetSize, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetUnMatchedBets(string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetSheetNamebyMarketID(string marketbookID);
        [OperationContract]
        void UpdateBetLowerLimit(int userID, decimal BetLowerLimit, decimal BetUpperLimit, bool isAllowedGrayHound, bool isAllowedHorse, decimal BetLowerLimitHorsePlace, decimal BetUpperLimitHorsePlace, decimal BetLowerLimitGrayHoundWin, decimal BetUpperLimitGrayHoundWin, decimal BetLowerLimitGrayHoundPlace, decimal BetUpperLimitGrayHoundPlace, decimal BetLowerLimitMatchOdds, decimal BetUpperLimitMatchOdds, decimal BetLowerLimitInningsRunns, decimal BetUpperLimitInningsRunns, decimal BetLowerLimitCompletedMatch, decimal BetUpperLimitCompletedMatch, bool isTennisAllowed, bool isSoccerAllowed, int CommissionRate, decimal BetLowerLimitMatchOddsSoccer, decimal BetUpperLimitMatchOddsSoccer, decimal BetLowerLimitMatchOddsTennis, decimal BetUpperLimitMatchOddsTennis, decimal BetUpperLimitTiedMatch, decimal BetLowerLimitTiedMatch, decimal BetUpperLimitWinner, decimal BetLowerLimitWinner, string Password, decimal BetUpperLimitFancy, decimal BetLowerLimitFancy);
        [OperationContract]
        void UpdateCheckConditionforPlaceBet(int UserID, bool CheckConditionforPlaceBet);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetTodayHorseRacing(int UserID, string EventTypeID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetTodayHorseRacingNew(int UserID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetDistinctmarketopened();
        [OperationContract]
        void UpdateOddsData(string oddsdata, string Oddtype);
        [OperationContract]
        int GetCommissionRatebyUserID(int UserID);
        [OperationContract]
        void UpdateUserPhoneandNamebyUserId(int userId, string Name, string Phone);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAccountsDatabyEventtypeuserIDandDateRange(int UserID, string From, string To, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetDatabyAgentIDForCommisionandDateRange(int UserID, string From, string To, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetDatabyAgentIDForCommisionandDateRangeByEventtype(int UserID, string From, string To, string Password);
        [OperationContract]
        string UserAccountsGetCommission(int UserID, string From, string To, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetEventTypeNamebyMarketID(string marketBookId);
        [OperationContract]
        void UpdateLiveOddbyID(long ID, string liveOdd, string Password);
        [OperationContract]
        void UpdateUserOddbyID(long ID, string UserOdd, string Password);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetAllowedMarketsbyUserID(int UserID);
        [OperationContract]
        void UpdateAllowedMarketsbyUserID(bool isCricketMatchOddsAllowedForBet, bool isCricketTiedMatchAllowedForBet, bool isCricketCompletedMatchAllowedForBet, bool isCricketInningsRunsAllowedForBet, bool isSoccerAllowedForBet, bool isTennisAllowedForBet, bool isHorseRaceWinAllowedForBet, bool isHorseRacePlaceAllowedForBet, bool isGrayHoundRaceWinAllowedForBet, bool isGrayHoundRacePlaceAllowedForBet, int UserID, bool isWinnerMarketAllowedForBet, string Password, bool isFancyAllowed);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        SP_Users_GetCommissionAccountIDandBookAccountID_Result GetCommissionaccountIdandBookAccountbyUserID(int UserID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        SP_Users_GetReferrerRateandReferrerIDbyUserID_Result GetReferrerRateandIDbyUserID(int UserID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        void UpdateRefererRateandIDbyUserID(int UserID, int ReffereID, int ReferrerRate);
        [OperationContract]
        void UpdateUsersAllBlock();
        [OperationContract]
        void UpdateUsersAllLoggedOut();
        [OperationContract]
        void UpdateMaxOddBackandLay(int UserID, decimal MaxOddBack, bool CheckForMaxOddBack, decimal MaxOddLay, bool CheckForMaxOddLay);
        [OperationContract]
        string GetIntervalandBetPlaceTimings(int UserID);
        [OperationContract]
        void UpdateIntervalandBetPlaceTimings(int HorseRaceTimerInterval, int HorseRaceBetPlaceWait, int GrayHoundTimerInterval, int GrayHoundBetPlaceWait, int CricketMatchOddsTimerInterval, int CricketMatchOddsBetPlaceWait, int CompletedMatchTimerInterval, int CompletedMatchBetPlaceWait, int TiedMatchTimerInterval, int TiedMatchBetPlaceWait, int InningsRunsTimerInterval, int InningsRunsBetPlaceWait, int WinnerTimerInterval, int WinnerBetPlaceWait, int TennisTimerInterval, int TennisBetPlaceWait, int SoccerTimerInterval, int SoccerBetPlaceWait, decimal PoundRate, int userID, int FancyTimerInterval, int FancyBetPlaceWait, int RaceMinutesBeforeStart, int CancelBetTime);
        [OperationContract]
        int GetHawalaAccountIDbyUserID(int UserID);
        [OperationContract]    
        int GetCreatedbyID(int UserID);
        [OperationContract]
        void UpdateHawalaIDbyUserID(int userID, int ParentID);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetMarketsforBettingAllowed(int userID);
        [OperationContract]
        void UpdateMarketAllowedBetting(int UserId, string MarketbookId, bool AllowedBetting);
        [OperationContract]
        void UpdateMarketAllowedBettingForAllAgents(List<int> UserIds, List<SP_UserMarket_GetMarketForAllowedBetting_Result> lstMarkets);
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        string GetBetSlipKeys(int UserID);
        [OperationContract]
        void UpdateBetSlipKeys(int UserID, string SimpleBtn1, string SimpleBtn2, string SimpleBtn3, string SimpleBtn4, string SimpleBtn5, string SimpleBtn6, string SimpleBtn7, string SimpleBtn8, string SimpleBtn9, string SimpleBtn10, string SimpleBtn11, string SimpleBtn12, string MutipleBtn1, string MutipleBtn2, string MutipleBtn3, string MutipleBtn4, string MutipleBtn5, string MutipleBtn6, string MutipleBtn7, string MutipleBtn8, string MutipleBtn9, string MutipleBtn10, string MutipleBtn11, string MutipleBtn12);

        [OperationContract]
        void UpdateBettingAllowed(string EventID, string BettingAllowed);

        [OperationContract]
        decimal GetPoundRatebyUserID(int UserID);
        [OperationContract]
        string GetMarqueeText();
        [OperationContract]
        void UpdateMarqueeText(string marqueetext);
        [OperationContract]
        void SendBalanceSheettoEmail(string Password);
        [OperationContract]
        void SendBalanceSheettoEmailAutomatic(string Password);
        [OperationContract]
        bool GetShowTV(int userID);
        [OperationContract]
        void UpdateShowTV(int userID, bool ShowTV);
        [OperationContract]
        SP_UserMarket_GetEventDetailsbyMarketID_Result GetEventDetailsbyMarketBook(string MarketbookID);
        [OperationContract]
        string GetLinevMarketsbyEventID(string EventID, DateTime EventOpenDate, int UserID);
        [OperationContract]
        string KJMarketsbyEventID(string EventID, int UserID);
        [OperationContract]     
        string GetLinevMarketsbyEventIDIN(string EventID, DateTime EventOpenDate, int UserID);
        [OperationContract]
        string GetMarketIDbyEventID(string EventID);
        [OperationContract]      
        string GetKalijut();
        [OperationContract]
        string GetFigureOdds();
        [OperationContract]
        string GetScoresbyEventIDandDate(string EventId, DateTime EventOpenDate);
        [OperationContract]
        string GetMarketRules();
        [OperationContract]
        string GetLineandMatchOddsforAssociation();
        [OperationContract]
        void UpdateAssociateEventID(string associateventID, string EventID);
        [OperationContract]
        string GetLiveTVChanels(string Passkey);
        [OperationContract]
        string GetScorebyEventIDandInnings(string EventID, DateTime EventOpenDate, int Innings);
        [OperationContract]
        string GetScorebyEventIDandInningsandOvers(string EventID, DateTime EventOpenDate, int Innings, int Overs);
        [OperationContract]
        void AddScoreToBallbyBallsummary(string EventID, string MarketCatalogueID, double over, int score, int innings, DateTime EventOpenDate, int wickets, string teamname, string matchstatus);
        [OperationContract]
        void CheckforMatchCompletedFancy(string MarketBookID, int ScoreforThisOver);
        [OperationContract]
        void CheckforMatchCompletedFancyIN(string MarketBookID, string Marketname, int ScoreforThisOver);
        [OperationContract]
        void CheckforMatchCompletedFancyKJ(string MarketBookID, int selectionID, int ScoreforThisOver);
        [OperationContract]

        void CheckforMatchCompletedSmallFig(string MarketBookID, int selectionID, int ScoreforThisOver);
        [OperationContract]
        void CheckforMatchCompletedFancyFig(string MarketBookID, int selectionID, int ScoreforThisOver);
        [OperationContract]
        bool InsertUserAccountsFancy(List<ExternalAPI.TO.MarketBook> marketbookstatus, int userID, string Password, int ScoreforThisOver);
        [OperationContract]
        bool GetIsComAllowbyUserID( int userID);
        [OperationContract]
        void UpdateMarketStatusbyMarketBookID(string MarketBookID, string MarketStatus);
        [OperationContract]
        bool GetFancyResultPostSetting();
        [OperationContract]
        void UpdateFancyResultPostSetting(bool fancyresultpost);
        [OperationContract]
        int GetCommissionRatebyUserIDFancy(int UserID);
        [OperationContract]
        void UpdateCommissionRatebyUserID(int UserID, int CommisionrateFancy);
        [OperationContract]
        void UpdateCricketAPIMatchKey(string EventID, string CricketAPIMatchKey);
        [OperationContract]
        string GetFancyResultsFrom();
        [OperationContract]
        void UpdateGetFancyResultsFrom(string ResultsFrom);
        [OperationContract]
        string GetRecentMatchesFromCricketAPI();
        [OperationContract]
        void UpdateGetDataFromForLoggingData(string EventID, string GetDataFrom);
        [OperationContract]
        string GetAccountsDatabyEventNameuserIDandDateRange(int UserID, string From, string To, string Password);
        [OperationContract]
        string GetAccountsDatabyEventNameuserIDandDateRangeFancywithMArketName(int UserID, string From, string To, string Password);
        [OperationContract]
        void UpdateTransferAdminAmount(int UserID, bool TransferAdminAmount, int TransferAgentID, bool TransferAdminAmountSoccer, bool TransferAdminAmountTennis, bool TransferAdminAmountHorse, bool TransferAdminAmountGreyHound);
        [OperationContract]
        SP_Users_GetTransferAdminAmount_Result GetTransferAdminAmount(int UserID);
        [OperationContract]
        string GetAllPendingAmountsbyDate(DateTime DueDate);
        [OperationContract]

        void AddAmountReceviables(int UserId, decimal Amount, DateTime DueDate, string Status, decimal AmountReceived);
        [OperationContract]

        void UpdateAmountReceviables(int ID, decimal Amount, DateTime DueDate, string Status);

        [OperationContract]
        bool GetBettingAllowedbyMarketIDandUserID(int UserId, string MarketBookID);

        [OperationContract]
        bool GetBettingAllowedbyMarketIDandUserIDInplay(int UserId);

        [OperationContract]
        void AddReferrerUsers(int UserID, int ReferrerID, int ReferrerRate);

        [OperationContract]
        void DeletReffererUSers(int UserID);

        [OperationContract]
        List<SP_Referrers_GetReferrerRateandReferrerIDbyUserID_Result> GetReferrerRatesbyUserID(int UserID);
        [OperationContract]
        void InsertUserBetNew(decimal userodd, string SelectionID, string Selectionname, string BetType, string nupdownAmount, string betslipamountlabel, decimal MaxOddBack, decimal MaxOddLay, bool CheckforMaxOddBack, bool CheckforMaxOddLay, int Clickedlocation, int UserID, string Betslipsize, string Password, string marketbookId, string Marketbookname, bool GetData);
        [OperationContract]
        string GetDistinctMarketsFromBets(string From, string To);
        [OperationContract]
        string GetDistinctMarketsFromAccounts(string From, string To);
        [OperationContract]
        bool UnPostUserAccountsbyUserIDandMarketID(string MarketBookId, int UserID, string Password);
        [OperationContract]
        bool UpdateUserBetsStatusbyMarketIDandUserID(string MarketBookId, int UserID, string Password);
        [OperationContract]
        void UpdateTotalOversbyMarket(string EventID, string TotalOvers);
        [OperationContract]
        string SetBlockedStatusofUserBMS(int UserID, bool isBlocked, string Password);
        [OperationContract]
        APIResponse GetMatchScoreCard(string strMatchKey, string Password);
        [OperationContract]
        string GetCricketMatchKey(string MarketCatalogueID);
        [OperationContract]
        void UpdateMarketsForView(int userID, bool isAllowedGrayHound, bool isAllowedHorse, bool isTennisAllowed, bool isSoccerAllowed, string Password);
        [OperationContract]
        void UpdateAllMarketClosedbyUserID(int UserID);
        [OperationContract]
        string GetURLsData();
        [OperationContract]
        bool GetTransferAgnetCommision(int UserID);
        [OperationContract]
        void UpdateTransferAgnetCommision(int UserID, bool TranserAgentCommision);
        [OperationContract]
        void UpdateFancySyncONorOFF(int UserId, string EventID, bool isopenenedbyuser);
        [OperationContract]
        int GetMaxBalanceTransferLimit(int UserID);
        [OperationContract]
        void UpdateMaxBalanceTransferLimit(int UserID, int MaxBalanceTransferLimit);
        [OperationContract]
        int GetMaxAgentRate(int UserID);

        [OperationContract]
        void UpdateMaxAgentRate(int UserID, int MaxAgentRate);
        [OperationContract]
        void UpdateFancySyncONorOFFbyMarketID(int UserId, string MarektID, bool isopenenedbyuser);
        [OperationContract]
        void UpdateKJSyncONorOFFbyMarketID(int UserId, string MarektID, bool isopenenedbyuser);
        [OperationContract]
        
        decimal GetTotalAgentCommissionbyAgentID(int UserId, string Password);
        [OperationContract]
        SP_UserMarket_GetToWinTheTossbyEventID_Result GetToWintheTossbyeventId(int UserId, string EventId);
        [OperationContract]
        SP_UserMarket_GetToTiedMarketbyEventID_Result GetToTiedMarketbyEventID(int UserId, string EventId); 
        [OperationContract]
       List<SP_UserMarket_GetSoccergoalbyEventID_Result> GetSoccergoalbyeventId(int UserId, string EventId);
        [OperationContract]
        
        void SetMarketOpenedbyuserinAPP();
    }
}
