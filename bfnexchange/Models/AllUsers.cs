using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bfnexchange.Models
{
    public class AllUsers
    {
        public IEnumerable<SelectListItem> Items { get; set; }
    }
}