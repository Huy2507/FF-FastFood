using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using FF_Fastfood.Models;
using FF_Fastfood.Attributes;

namespace FF_Fastfood.Controllers
{
    public class FoodController : Controller
    {
        // GET: Food
        public ActionResult Index()
        {
            using (FF_FastFoodEntities db = new FF_FastFoodEntities())
            {
                var foodItems = db.Foods.Select(f => new FoodItem
                {
                    food_id = f.food_id,
                    name = f.name,
                    price = f.price,
                    description = f.description,
                    image_url = f.image_url,
                    category_name = f.Category.category_name
                })
                .ToList();

                var foodCategory = db.Categories.Select(c => new FoodCategory
                {
                    category_id = c.category_id,
                    category_name = c.category_name,
                    slug = c.slug,
                })
                .ToList();

                var foodViewModel = new FoodViewModel
                {
                    Categories = foodCategory,
                    Items = foodItems
                };

                return View(foodViewModel);
            }
        }
    }
}