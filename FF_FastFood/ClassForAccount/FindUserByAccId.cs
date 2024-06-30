using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FF_Fastfood.Models;
namespace FF_Fastfood.ClassForAccount
{
    public class FindUserByCustomId
    {

        public int order_id { get; set; }
        public string name { get; set; }
        public Nullable<System.DateTime> order_date { get; set; }
        public decimal total_amount { get; set; }
        public string status { get; set; }

    }
    public class FindFoodByID
    {
        public int order_item_id { get; set; }
        public Nullable<int> order_id { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }
    }
}