//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bfnexchange.Services.DBModel
{
    using System;
    
    public partial class SP_Users_GetUserbyUsernameandPassword_Result2
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public Nullable<int> UserTypeID { get; set; }
        public bool isBlocked { get; set; }
        public bool isDeleted { get; set; }
        public string Password { get; set; }
        public string PasswordforValidate { get; set; }
        public string PasswordforValidateS { get; set; }
        public string RatePercent { get; set; }
        public Nullable<bool> Loggedin { get; set; }
        public Nullable<decimal> BetLowerLimit { get; set; }
        public decimal BetUpperLimit { get; set; }
        public bool CheckConditionsforPlacingBet { get; set; }
        public Nullable<bool> isGrayHoundRaceAllowed { get; set; }
        public Nullable<bool> isHorseRaceAllowed { get; set; }
        public Nullable<decimal> BetLowerLimitHorsePlace { get; set; }
        public Nullable<decimal> BetUpperLimitHorsePlace { get; set; }
        public Nullable<decimal> BetUpperLimitGrayHoundPlace { get; set; }
        public Nullable<decimal> BetLowerLimitGrayHoundPlace { get; set; }
        public Nullable<decimal> BetUpperLimitGrayHoundWin { get; set; }
        public Nullable<decimal> BetLowerLimitGrayHoundWin { get; set; }
        public Nullable<decimal> BetUpperLimitMatchOdds { get; set; }
        public Nullable<decimal> BetLowerLimitMatchOdds { get; set; }
        public Nullable<decimal> BetUpperLimitInningRuns { get; set; }
        public Nullable<decimal> BetLowerLimitInningRuns { get; set; }
        public Nullable<decimal> BetUpperLimitCompletedMatch { get; set; }
        public Nullable<decimal> BetLowerLimitCompletedMatch { get; set; }
        public Nullable<bool> isTennisAllowed { get; set; }
        public Nullable<bool> isSoccerAllowed { get; set; }
        public Nullable<decimal> BetUpperLimitMatchOddsSoccer { get; set; }
        public Nullable<decimal> BetLowerLimitMatchOddsSoccer { get; set; }
        public Nullable<decimal> BetUpperLimitMatchOddsTennis { get; set; }
        public Nullable<decimal> BetLowerLimitMatchOddsTennis { get; set; }
        public Nullable<decimal> BetUpperLimitTiedMatch { get; set; }
        public Nullable<decimal> BetLowerLimitTiedMatch { get; set; }
        public Nullable<decimal> BetUpperLimitWinner { get; set; }
        public Nullable<decimal> BetLowerLimitWinner { get; set; }
        public Nullable<decimal> MaxOddBack { get; set; }
        public Nullable<decimal> MaxOddLay { get; set; }
        public Nullable<bool> CheckforMaxOddBack { get; set; }
        public Nullable<bool> CheckforMaxOddLay { get; set; }
        public Nullable<long> CurrentLoggedInID { get; set; }
        public Nullable<decimal> BetLowerLimitFancy { get; set; }
        public Nullable<decimal> BetUpperLimitFancy { get; set; }
        public Nullable<bool> TransferAdminAmount { get; set; }
        public Nullable<bool> isBetSlipKeysAllowed { get; set; }
        public Nullable<bool> isJoriBetAllowed { get; set; }
        public Nullable<bool> GTLogin { get; set; }
        public Nullable<bool> iscom { get; set; }
    }
}
