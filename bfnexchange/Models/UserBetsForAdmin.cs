﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bfnexchange.Models
{
    [Serializable()]
    public class UserBetsForAdmin
    {

        public string UserOdd { get; set; }
        public string Amount { get; set; }
        public string BetType { get; set; }
        public bool isMatched { get; set; }
        public string SelectionName { get; set; }
        public string MarketBookname { get; set; }
        public string CustomerName { get; set; }
        public string DealerName { get; set; }
        public Nullable<int> UserID { get; set; }
        public string MarketBookID { get; set; }
        public string SelectionID { get; set; }
        public long ID { get; set; }
        public string LiveOdd { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string AgentRate { get; set; }
        public string Liabality { get; set; }
        public string BetSize { get; set; }
        public Nullable<decimal> PendingAmount { get; set; }
        public string location { get; set; }
        public Nullable<long> ParentID { get; set; }
        public int UserTypeID { get; set; }
        public bool TransferAdmin { get; set; }
        public int TransferAgentIDB { get; set; }
        public int TransferAdminPercentage { get; set; }
        public int SuperAgentRateB { get; set; }
    }
}