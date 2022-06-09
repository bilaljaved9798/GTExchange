using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTExch.Models
{
  public  class MarketRules
    {
        public int ID { get; set; }
        public string Category { get; set; }
        public string MarketType { get; set; }
        public string Rules { get; set; }
    }
}
