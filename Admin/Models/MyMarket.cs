using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bftradeline.Models
{
    public class MyMarket
    {
        public string MarketID { get; set; }
        public string MarketName { get; set; }
        public string Liability { get; set; } = "0.00";
        
    }
}