//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bfnexchange.Services.DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ActivityLog
    {
        public int ID { get; set; }
        public string ActivityName { get; set; }
        public System.DateTime ActivityTime { get; set; }
        public string IPAddress { get; set; }
        public string DeviceInfo { get; set; }
        public string Location { get; set; }
        public int UserID { get; set; }
    }
}
