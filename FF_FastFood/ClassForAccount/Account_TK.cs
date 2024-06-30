using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FF_Fastfood.ClassForAccount
{
    public class Account_TK
    {
        public int account_id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public bool isActive { get; set; }
    }
}