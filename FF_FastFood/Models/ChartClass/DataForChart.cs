using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FF_Fastfood.Models
{
    public class DataForChart
    {
        
        public String order_date { get; set; }
        public decimal total_amount { get; set; }
    }
}