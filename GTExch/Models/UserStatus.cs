using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GTExch.Models
{
    public class UserStatus
    {
        public bool isBlocked { get; set; }
        public bool isDeleted { get; set; }
        public bool Loggedin { get; set; }
        public bool GTLogin { get; set; }
        public long CurrentLoggedInID { get; set; }
    }
    public partial class SP_URLsData_GetAllData_Result
    {
        public string URLForData { get; set; }
        public string EventType { get; set; }
        public string Scd { get; set; }
        public string GetDataFrom { get; set; }
    }
}