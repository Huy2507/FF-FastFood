using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FF_Fastfood.Models
{
    public class OrderDeliveryViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public List<OrderItemDeliveryViewModel> OrderItems { get; set; }

        public string DeliveryAddress => $"{Street}, {Ward}, {District}, {City}";

        public class OrderItemDeliveryViewModel
        {
            public string FoodName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }
    }
}