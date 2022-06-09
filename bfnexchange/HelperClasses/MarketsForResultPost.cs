using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bfnexchange.HelperClasses
{
    public class MarketsForResultPost
    {
      public  List<bfnexchange.Services.DBModel.SP_UserMarket_GetEventLineMarketandOddsForAssociation_Result> lstEvents { get; set; }
        public bool FancyResultsAutomaticPost { get; set; }
    }
}