using System.Collections.Generic;
using System;
using System.Linq;
using System.Web.Mvc;
using FF_Fastfood.Models;
using Microsoft.AspNet.Identity;

public class CartController : Controller
{
    private FF_FastFoodEntities1 dbContext = new FF_FastFoodEntities1();

    [HttpPost]
    public JsonResult AddToCart(int foodId)
    {
        // Lấy giá trị UserId từ Cookie
        var userCookie = Request.Cookies["UserCookie"];
        int? userId = null;

        if (userCookie != null)
        {
            int parsedUserId;
            if (int.TryParse(userCookie.Values["UserId"], out parsedUserId))
            {
                userId = parsedUserId;
            }
        }

        if (userId == null)
        {
            return Json(new { success = false, message = "User is not logged in, please login to continue!" });
        }

        var customer = dbContext.Customers.FirstOrDefault(c => c.account_id == userId);
        if (customer == null)
        {
            return Json(new { success = false, message = "Invalid customer ID" });
        }

        var cart = dbContext.Carts.Include("Cart_Items")
                                  .FirstOrDefault(c => c.customer_id == customer.customer_id);

        if (cart == null)
        {
            cart = new Cart
            {
                customer_id = customer.customer_id,
                created_at = DateTime.Now,
                updated_at = DateTime.Now,
                Cart_Items = new List<Cart_Items>()
            };
            dbContext.Carts.Add(cart);
        }

        var foodItem = dbContext.Foods.Find(foodId);
        if (foodItem != null)
        {
            var existingItem = cart.Cart_Items.FirstOrDefault(ci => ci.food_id == foodId);
            if (existingItem != null)
            {
                existingItem.quantity++;
                existingItem.updated_at = DateTime.Now;
            }
            else
            {
                cart.Cart_Items.Add(new Cart_Items
                {
                    food_id = foodItem.food_id,
                    quantity = 1,
                    created_at = DateTime.Now,
                    updated_at = DateTime.Now,
                    Food = foodItem
                });
            }

            cart.updated_at = DateTime.Now;
            dbContext.SaveChanges();

            return Json(new { success = true });
        }

        return Json(new { success = false, message = "Food item not found" });
    }


    [HttpPost]
    public ActionResult UpdateQuantity(int cartItemId, string changeQuantity)
    {
        var cartItem = dbContext.Cart_Items.Find(cartItemId);

        if (cartItem == null)
        {
            return HttpNotFound();
        }

        if (changeQuantity == "+")
        {
            cartItem.quantity++;
        }
        else if (changeQuantity == "-" && cartItem.quantity > 1)
        {
            cartItem.quantity--;
        }

        cartItem.updated_at = DateTime.Now;
        dbContext.SaveChanges();

        // Tính tổng tiền của toàn bộ giỏ hàng
        var totalCartPrice = dbContext.Cart_Items
            .Where(c => c.cart_id == cartItem.cart_id)
            .Sum(c => c.Food.price * c.quantity);

        // Trả về dữ liệu mới của cartItem để cập nhật giao diện
        return Json(new
        {
            quantity = cartItem.quantity,
            itemTotalPrice = cartItem.Food.price * cartItem.quantity,
            totalCartPrice = totalCartPrice
        });
    }

    [HttpPost]
    public ActionResult RemoveItem(int cartItemId)
    {
        var cartItem = dbContext.Cart_Items.Find(cartItemId);
        if (cartItem != null)
        {
            

            // Tính tổng tiền của toàn bộ giỏ hàng trừ đi sản phẩm hiện tại, sử dụng kiểu nullable
            var totalCartPrice = (dbContext.Cart_Items
                .Where(c => c.cart_id == cartItem.cart_id)
                .Sum(c => (decimal?)c.Food.price * c.quantity) ?? 0)
                -
                (dbContext.Cart_Items
                .Where(c => c.cart_item_id == cartItem.cart_item_id)
                .Sum(c => (decimal?)c.Food.price * c.quantity) ?? 0);
            //Xóa Sản Phẩm
                dbContext.Cart_Items.Remove(cartItem);
                dbContext.SaveChanges();
            // Kiểm tra số lượng mục hàng còn lại trong giỏ hàng
            // Lấy giá trị UserId từ Cookie
            var userCookie = Request.Cookies["UserCookie"];
            int? userId = null;

            if (userCookie != null)
            {
                int parsedUserId;
                if (int.TryParse(userCookie.Values["UserId"], out parsedUserId))
                {
                    userId = parsedUserId;
                }
            }
            var cus = dbContext.Customers.FirstOrDefault(c => c.account_id.ToString() == userId.ToString());

            var cart = dbContext.Carts.FirstOrDefault(c => c.customer_id == cus.customer_id );
            var remainingItemsCount = dbContext.Cart_Items.Count(c => c.cart_id == cart.cart_id);

            return Json(new { success = true, totalCartPrice = totalCartPrice, isEmpty = remainingItemsCount}); ;
        }
        return Json(new { success = false, message = "Cart item not found" });
    }




    public ActionResult Index()
    {
        // Lấy giá trị UserId từ Cookie
        var userCookie = Request.Cookies["UserCookie"];
        int? userId = null;

        if (userCookie != null)
        {
            int parsedUserId;
            if (int.TryParse(userCookie.Values["UserId"], out parsedUserId))
            {
                userId = parsedUserId;
            }
        }
        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var customer = dbContext.Customers.FirstOrDefault(c => c.account_id == userId);
        if (customer == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var cart = dbContext.Carts.Include("Cart_Items.Food")
                                  .FirstOrDefault(c => c.customer_id == customer.customer_id);

        return View(cart);
    }
}
