using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bftradeline.HelperClasses
{
   public class ProfitandLossEventType
    {
        public decimal NetProfitandLoss { get; set; }
        public string EventType { get; set; }
        public string EventID { get; set; }
        public string Eventtype1 { get; set; }
        public bool isCheckedforTotal { get; set; } = true;
    }
}
