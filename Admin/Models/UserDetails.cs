using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bftradeline.Models
{
    public class UserDetails
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public decimal AccountBalance { get; set; }
        public string LastLoginTime { get; set; }
        public string Status { get; set; }
        public string ProfitandLoss { get; set; }
        public bool isDeleted { get; set; }
        public bool isBlocked { get; set; }
        public string Password { get; set; }
        public string LastAmountAdded { get; set; }
        public string LastAmountRemoved { get; set; }
        public string AssignedMarket {get;set;}
        public string LastLocation { get; set; }
        public string LastIPAddress { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string RatePercent { get; set; }
        public bool Loggedin { get; set; }
        public decimal BetLowerLimit { get; set; }
        public decimal BetUpperLimit { get; set; }
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
        public bool isTennisAllowed { get; set; }
        public bool isSoccerAllowed { get; set; }
        public int CommissionRate { get; set; }
        public string PhoneNumber { get; set; }
        public decimal BetUpperLimitMatchOddsSoccer { get; set; }
        public decimal BetLowerLimitMatchOddsSoccer { get; set; }
        public decimal BetUpperLimitMatchOddsTennis { get; set; }
        public decimal BetLowerLimitMatchOddsTennis { get; set; }
        public decimal BetUpperLimitTiedMatch { get; set; }
        public decimal BetLowerLimitTiedMatch { get; set; }
        public decimal BetUpperLimitWinner { get; set; }
        public decimal BetLowerLimitWinner { get; set; }
        public decimal BetUpperLimitFancy { get; set; }
        public decimal BetLowerLimitFancy { get; set; }
        public bool isBMSBlocked { get; set; }
        public bool GTLogin { get; set; }

    }
}