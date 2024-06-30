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

        // Các thuộc tính mới cho địa chỉ
        public string NewStreet { get; set; }
        public string NewWard { get; set; }
        public string NewDistrict { get; set; }
        public string NewCity { get; set; }
    }
}