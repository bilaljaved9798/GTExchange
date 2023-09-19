using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace GTExchNew.Models
{
    public class AllUsers
    {
        public IEnumerable<SelectListItem> Items { get; set; }
    }
}