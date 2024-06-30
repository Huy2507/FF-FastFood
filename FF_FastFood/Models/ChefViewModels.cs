using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FF_Fastfood.Models
{
    public class OrdersViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
    }

    public class FoodChefViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Food> Foods { get; set; }
    }
    public class FoodChefViewModel1
    {
        public List<Category> Categories { get; set; }
        public Food Food { get; set; }
    }

    public class CreateFoodViewModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int category_id { get; set; }
        public string image_url { get; set; }
        public string enable { get; set; }
        public string seo_title { get; set; }
        public string seo_description { get; set; }
        public string seo_keywords { get; set; }
    }

    public class UpdateFoodViewModel
    {
        public int food_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public int category_id { get; set; }
        public string image_url { get; set; }
        public string enable { get; set; }
        public string seo_title { get; set; }
        public string seo_description { get; set; }
        public string seo_keywords { get; set; }
    }
}