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
    
    public partial class APIConfig
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AppKey { get; set; }
        public string SessionKey { get; set; }
        public string APIUrl { get; set; }
        public string PoundRate { get; set; }
        public string ComissionRate { get; set; }
        public string MarqueeText { get; set; }
        public Nullable<bool> AutomaticDownloadData { get; set; }
    }
}
