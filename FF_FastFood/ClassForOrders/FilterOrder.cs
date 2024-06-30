using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FF_Fastfood.ClassForOrders
{
    public class FilterOrder
    {
        public String customerName { get; set; }
        public DateTime order_date { get; set; }
        public string status { get; set; }
    }
}