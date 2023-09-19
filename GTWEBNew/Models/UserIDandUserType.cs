using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTExchNew.Models
{
    [Serializable()]
    public class UserIDandUserType
    {
        public int ID { get; set; } = 0;
        public string UserName { get; set; }
        public string UserType { get; set; }
        public Nullable<int> UserTypeID { get; set; }
        public bool isBlocked { get; set; }
        public bool isDeleted { get; set; }
        public string Password { get; set; }
        public string RatePercent { get; set; }
        public bool Loggedin { get; set; }
        public decimal BetLowerLimit { get; set; }
        public decimal BetUpperLimit { get; set; }
        public bool CheckConditionsforPlacingBet { get; set; }
        public bool isGrayHoundRaceAllowed { get; set; }
        public bool isHorseRaceAllowed { get; set; }
        public decimal BetLowerLimitHorsePlace { get; set; }
        public decimal BetUpperLimitHorsePlace { get; set; }
        public decimal BetUpperLimitGrayHoundPlace { get; set; }
        public decimal BetLowerLimitGrayHoundPlace { get; set; }
        public decimal BetUpperLimitGrayHoundWin { get; set; }
        public decimal BetLowerLimitGrayHoundWin { get; set; }
        public decimal BetUpperLimitMatchOdds { get; set; }
        public decimal BetLowerLimitMatchOdds { get; set; }
        public decimal BetUpperLimitInningRuns { get; set; }

        public decimal BetLowerLimitInningRuns { get; set; }
        public decimal BetUpperLimitCompletedMatch { get; set; }
        public decimal BetLowerLimitCompletedMatch { get; set; }
        public decimal BetUpperLimitMatchOddsSoccer { get; set; }
        public decimal BetLowerLimitMatchOddsSoccer { get; set; }
        public decimal BetUpperLimitMatchOddsTennis { get; set; }
        public decimal BetLowerLimitMatchOddsTennis { get; set; }
        public decimal BetUpperLimitTiedMatch { get; set; }
        public decimal BetLowerLimitTiedMatch { get; set; }
        public decimal BetUpperLimitWinner { get; set; }
        public decimal BetLowerLimitWinner { get; set; }
        public decimal MaxOddBack { get; set; }
        public decimal MaxOddLay { get; set; }
        public bool CheckforMaxOddBack { get; set; }
        public bool CheckforMaxOddLay { get; set; }
        public long CurrentLoggedInID { get; set; }
        public decimal BetUpperLimitFancy { get; set; }
        public decimal BetLowerLimitFancy { get; set; }
        public string PasswordforValidate { get; set; }
        public string PasswordforValidateS { get; set; }
        public int CreatedbyID { get; set; }
        public string token { get; set; }

    }

}