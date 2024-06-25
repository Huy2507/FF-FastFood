using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FF_Fastfood.Models
{
    public class HomeViewModel
    {
        public List<Banner> banners { get; set; }
        public List<Category> categories { get; set; }
        public List<Food> foods { get; set; }
    }
}