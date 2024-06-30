using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FF_Fastfood.Models
{
    public class FoodViewModel
    {
        public List<FoodCategory> Categories { get; set; }
        public Dictionary<string, IPagedList<FoodItem>> PagedItems { get; set; }
        public int PageNumber { get; set; }
    }

    public class FoodCategory
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string slug { get; set; }
    }
    public class FoodItem
    {
        public int food_id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }
        public string description { get; set; }
        public string image_url { get; set; }
        public string category_name { get; set; }
    }
}