using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using FF_Fastfood.Models;
using FF_Fastfood.Attributes;
using PagedList;

namespace FF_Fastfood.Controllers
{
    public class FoodController : Controller
    {
        public ActionResult Index(string slug, int? page, string categorySlug = null, string searchTerm = null)
        {
            using (FF_FastFoodEntities db = new FF_FastFoodEntities())
            {
                int pageNumber = page ?? 1;
                int pageSize = 4; // Set the page size as per your requirement

                var foodItemsQuery = db.Foods.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    foodItemsQuery = foodItemsQuery.Where(f => f.name.Contains(searchTerm) || f.description.Contains(searchTerm));
                }

                var foodItems = foodItemsQuery.Select(f => new FoodItem
                {
                    food_id = f.food_id,
                    name = f.name,
                    price = f.price,
                    description = f.description,
                    image_url = f.image_url,
                    category_name = f.Category.category_name
                }).ToList();

                var foodCategories = db.Categories.Select(c => new FoodCategory
                {
                    category_id = c.category_id,
                    category_name = c.category_name,
                    slug = c.slug,
                }).ToList();

                var pagedItems = new Dictionary<string, IPagedList<FoodItem>>();
                var categoriesWithItems = new List<FoodCategory>();

                foreach (var category in foodCategories)
                {
                    var itemsInCategory = foodItems
                        .Where(fi => fi.category_name == category.category_name)
                        .ToPagedList(category.slug == categorySlug ? pageNumber : 1, pageSize);

                    if (itemsInCategory.Any()) // Only add categories that have items
                    {
                        pagedItems[category.slug] = itemsInCategory;
                        categoriesWithItems.Add(category);
                    }
                }

                var foodViewModel = new FoodViewModel
                {
                    Categories = categoriesWithItems, // Use the filtered list of categories
                    PagedItems = pagedItems
                };

                ViewBag.Slug = slug;
                ViewBag.Page = pageNumber;
                ViewBag.CategorySlug = categorySlug;
                ViewBag.SearchTerm = searchTerm;

                return View(foodViewModel);
            }
        }

    }
}