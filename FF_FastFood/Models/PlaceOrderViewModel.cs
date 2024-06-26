using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FF_Fastfood.Models
{
    public class PlaceOrderViewModel
    {
        public List<Cart_Items> CartItems { get; set; }
        public Customer Customer { get; set; }
        public string PaymentMethod { get; set; } // New field for payment method
    }
}