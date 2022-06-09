using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bftradeline.Models
{
   
    public  class AmountReceivables
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime DueDate { get; set; }
        public string Status { get; set; }
        public decimal AmountReceived { get; set; }
        public string UserName { get; set; }
        public decimal Balance { get; set; }
        public string DueDateStr { get; set; }
    }
}
