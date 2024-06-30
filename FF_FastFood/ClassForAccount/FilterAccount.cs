using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FF_Fastfood.ClassForAccount
{
    public class FilterAccount
    {
        public int? account_id { get; set; }
        public string role { get; set; }
        public bool isActive { get; set; }
    }
}