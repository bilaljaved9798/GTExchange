using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bftradeline.Models
{
    [Serializable()]
    public class LiabalitybyMarket
    {
        public string MarketBookName { get; set; }
        public decimal Liabality { get; set; }
        public string MarketBookID { get; set; }
    }
}