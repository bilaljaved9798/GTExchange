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
    
    public partial class EmailConfig
    {
        public int ID { get; set; }
        public string FromEmail { get; set; }
        public string FromPassword { get; set; }
        public string ToEmail { get; set; }
        public string ToPassword { get; set; }
        public string SMTP { get; set; }
        public string PortNum { get; set; }
        public Nullable<System.TimeSpan> EmailTime { get; set; }
    }
}
