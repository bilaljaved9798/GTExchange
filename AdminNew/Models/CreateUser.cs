using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace bftradeline.Models
{
    public class CreateUser
    {

        public int ID { get; set; }
      
        public string Name { get; set; }
       
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
       
        public string UserName { get; set; }
      
       
        public string Password { get; set; }
      
        public string Location { get; set; }
       
        public decimal AccountBalance { get; set; }

        public int UserTypeID { get; set; }
        public int CreatedbyID { get; set; }
        public string AgentRateC { get; set; }
        public decimal BetLowerLimit { get; set; } = 2000;
        public decimal BetUpperLimit { get; set; } = 25000;
        public decimal BetLowerLimitHorsePlace { get; set; } = 2000;
        public decimal BetUpperLimitHorsePlace { get; set; } = 25000;
        public decimal BetUpperLimitGrayHoundPlace { get; set; } = 25000;
        public decimal BetLowerLimitGrayHoundPlace { get; set; } = 2000;
        public decimal BetUpperLimitGrayHoundWin { get; set; } = 25000;
        public decimal BetLowerLimitGrayHoundWin { get; set; } = 2000;
        public decimal BetUpperLimitMatchOdds { get; set; } = 500000;
        public decimal BetLowerLimitMatchOdds { get; set; } = 2000;
        public decimal BetUpperLimitInningRuns { get; set; } = 500000;

        public decimal BetLowerLimitInningRuns { get; set; } = 2000;
        public decimal BetUpperLimitCompletedMatch { get; set; } = 500000;
        public decimal BetLowerLimitCompletedMatch { get; set; } = 2000;
        public bool isTennisAllowed { get; set; }
        public bool isSoccerAllowed { get; set; }
        public decimal BetUpperLimitMatchOddsSoccer { get; set; } = 500000;
        public decimal BetLowerLimitMatchOddsSoccer { get; set; } = 2000;
        public decimal BetUpperLimitMatchOddsTennis { get; set; } = 500000;
        public decimal BetLowerLimitMatchOddsTennis { get; set; } = 2000;

    }

}