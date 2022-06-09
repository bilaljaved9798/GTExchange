using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTExch.Models
{
    public class MyMarket
    {
        public string MarketID { get; set; }
        public string MarketName { get; set; }
        public string Liability { get; set; } = "0.00";
        
    }
}