using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using FF_Fastfood.Attributes;
using System.Web.Mvc;
using FF_Fastfood.Models; // Đảm bảo rằng bạn đã thêm namespace cho mô hình của bạn.

namespace FF_Fastfood.Controllers
{
    [AuthorizeRole("Chef", "Admin")]
    public class ChefController : Controller
    {
        private FF_FastFoodEntities db = new FF_FastFoodEntities();

        public ActionResult PendingOrders()
        {
            var pendingOrders = db.Orders
                                  .Where(o => o.status == "Pending")
                                  .OrderByDescending(o => o.order_date)
                                  .Select(o => new OrdersViewModel
                                  {
                                      OrderId = o.order_id,
                                      OrderDate = (DateTime)o.order_date,
                                      TotalAmount = o.total_amount,
                                      CustomerName = db.Customers.FirstOrDefault(c => c.customer_id == o.customer_id).name,
                                      Status = o.status
                                  })
                                  .ToList();

            return View(pendingOrders);
        }


        // Action để hiển thị chi tiết đơn hàng
        public ActionResult OrderDetails(int? id)
        {
            var order = db.Orders.Include("Order_Items").FirstOrDefault(o => o.order_id == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var model = new OrderDetailViewModel
            {
                Items = order.Order_Items.Select(i => new OrderItemViewModel
                {
                    ProductName = db.Foods.FirstOrDefault(f => f.food_id == i.food_id).name,
                    Quantity = i.quantity,
                    Price = i.price
                }).ToList()
            };

            return PartialView("_OrderDetails", model);
        }


        [HttpPost]
        public ActionResult CompleteOrder(int id)
        {
            var order = db.Orders.FirstOrDefault(o => o.order_id == id);
            if (order == null)
            {
                return Json(new { success = false, message = "Order not found." });
            }

            order.status = "Waiting";
            db.SaveChanges();

            return Json(new { success = true });
        }

        public ActionResult Foods()
        {
            var foods = db.Foods.ToList(); // Lấy danh sách món ăn
            var categories = db.Categories.ToList(); // Lấy danh sách các danh mục món ăn

            var viewModel = new FoodChefViewModel
            {
                Foods = foods,
                Categories = categories
            };

            return View(viewModel); // Trả về view với dữ liệu là viewModel
        }

      

        public ActionResult FilterFoods(int categoryId)
        {
            var foods = db.Foods.ToList();
            if (categoryId != 0)
            {
                foods = db.Foods.Where(f => f.category_id == categoryId).ToList(); // Lấy danh sách món ăn
            }

            var viewModel = new FoodChefViewModel
            {
                Foods = foods
            };

            // Đưa dữ liệu về view dưới dạng JSON
            return PartialView("_FilterFoods", viewModel);
        }

        public ActionResult Create()
        {
            var categoryList = db.Categories.ToList();
            return PartialView("_Create", categoryList);
        }

        // POST: Chef/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateFoodViewModel model)
        {
            if (ModelState.IsValid)
            {
                var food = new Food
                {
                    name = model.name,
                    description = model.description,
                    price = model.price,
                    category_id = model.category_id,
                    image_url = model.image_url,
                    enable = model.enable,
                    seo_title=model.seo_title,
                    seo_description=model.seo_description,
                    seo_keywords=model.seo_keywords,
                    slug = SlugHelper.GenerateSlug(model.name),
                    created_at = DateTime.Now,
                };

                db.Foods.Add(food);
                db.SaveChanges();

                return RedirectToAction("Foods");
            } 

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            var food = db.Foods.FirstOrDefault(o => o.food_id == id);
            var cat = db.Categories.ToList();

            if (food == null)
            {
                return HttpNotFound();
            }
            var viewModel = new FoodChefViewModel1
            {
                Food = food,
                Categories = cat,
            };
            return PartialView("_Update", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpdateFoodViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Tìm món ăn cần sửa trong cơ sở dữ liệu
                var existingFood = db.Foods.Find(model.food_id);

                if (existingFood != null)
                {
                    // Cập nhật thông tin món ăn từ form
                    existingFood.name = model.name;
                    existingFood.description = model.description;
                    existingFood.price = model.price;
                    existingFood.category_id = model.category_id;
                    existingFood.image_url = model.image_url;
                    existingFood.enable = model.enable;
                    existingFood.seo_title = model.seo_title;
                    existingFood.seo_description = model.seo_description;
                    existingFood.seo_keywords = model.seo_keywords;

                    // Lưu các thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();

                    // Chuyển hướng về danh sách món ăn sau khi sửa thành công
                    return RedirectToAction("Foods");
                }
                else
                {
                    // Nếu không tìm thấy món ăn, hiển thị thông báo lỗi
                    ModelState.AddModelError("", "Không tìm thấy món ăn để sửa.");
                }
            }

            // Nếu ModelState không hợp lệ, hiển thị lại form với dữ liệu đã nhập và thông báo lỗi

            return PartialView("_Update");
        }

        
        [HttpPost]
        public ActionResult DeleteFood(int id)
        {
            // Tìm món ăn cần xóa trong cơ sở dữ liệu
            var foodToDelete = db.Foods.FirstOrDefault(f => f.food_id == id);
            if (foodToDelete == null)
            {
                return Json(new { success = false, message = "Order not found." });
            }
            // Xóa món ăn khỏi cơ sở dữ liệu
            db.Foods.Remove(foodToDelete);
            db.SaveChanges();

            // Chuyển hướng về danh sách món ăn sau khi xóa thành công
            return Json(new { success = true });
        }
    }
}
