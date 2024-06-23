using FF_Fastfood.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
using System.Linq;
using System.Web.Helpers;
using Microsoft.Ajax.Utilities;

public class MyAccountController : Controller
{
    FF_FastFoodEntities db = new FF_FastFoodEntities();
    public ActionResult Index()
    {
        if (Request.Cookies["UserCookie"] == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var user = Session["User"] as User;

        return View(user);
    }

    public ActionResult EditProfile()
    {
        var userCookie = Request.Cookies["UserCookie"];
        if (userCookie == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = userCookie.Values["UserId"];
        var cus = db.Customers.FirstOrDefault(c => c.account_id.ToString() == userId);
        if (cus == null)
        {
            return HttpNotFound();
        }

        var model = new EditProfileViewModel
        {
            userName = cus.name,
            Phone = cus.phone,
            Email = cus.email
        };

        return View(model);
    }

    // POST: Action để lưu thông tin đã chỉnh sửa
    [HttpPost]
    public ActionResult EditProfile(EditProfileViewModel updatedUser)
    {
        if (ModelState.IsValid)
        {
            var userCookie = Request.Cookies["UserCookie"];
            if (userCookie == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = userCookie.Values["UserId"];
            var cus = db.Customers.FirstOrDefault(x => x.account_id.ToString() == userId);
            if (cus != null)
            {
                // Cập nhật thông tin người dùng từ updatedUser
                cus.name = updatedUser.userName;
                cus.phone = updatedUser.Phone;
                cus.email = updatedUser.Email;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
            }
        }

        return RedirectToAction("EditProfile");
    }

    // Action để đổi mật khẩu
    public ActionResult ChangePassword()
    {
        if (Request.Cookies["UserCookie"] == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View();
    }

    // POST: Action để lưu mật khẩu mới
    [HttpPost]
    public ActionResult ChangePassword(ChangePasswordViewModel model)
    {
        var user = Request.Cookies["UserCookie"];
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = user.Values["UserId"];

        if (ModelState.IsValid)
        {
            var account = db.Accounts.FirstOrDefault(a => a.account_id.ToString() == userId);
            if (Crypto.VerifyHashedPassword(account.password, model.CurrentPassword) == false)
            {
                ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng.");
                return View(model);
            }
            var newhashPassword = Crypto.HashPassword(model.NewPassword);
            account.password = newhashPassword;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Nếu ModelState không hợp lệ, hiển thị lại form đổi mật khẩu với các lỗi
        return View(model);
    }

    public ActionResult YourAddresses()
    {
        var userCookie = Request.Cookies["UserCookie"];
        if ( userCookie == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = userCookie.Values["UserId"];
        var userName = userCookie.Values["UserName"];
        var cus = db.Customers.FirstOrDefault(c => c.account_id.ToString() == userId);
        var addresses = db.Addresses.Where(a => a.customer_id == cus.customer_id).ToList();

        var model = new YourAddressesViewModel
        {
            Addresses = addresses,
            userName = userName
        };

        return View(model);
    }

    [HttpPost]
    public ActionResult DeleteAddress(int id)
    {
        var address = db.Addresses.Find(id);
        if (address != null)
        {
            db.Addresses.Remove(address);
            db.SaveChanges();
            return Json(new { success = true });
        }
        return Json(new { success = false });
    }

    public ActionResult Orders()
    {
        var userCookie = Request.Cookies["UserCookie"];
        if (userCookie == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = userCookie.Values["UserId"];
        var userName = userCookie.Values["UserName"];
        var cus = db.Customers.FirstOrDefault(c=> c.account_id.ToString()==userId);
        var orders = db.Orders.Where(o => o.customer_id == cus.customer_id).ToList();
        var model = orders.Select(o => new OrderViewModel
        {
            userName = userName,
            OrderId = o.order_id,
            OrderDate = (DateTime)o.order_date,
            TotalAmount = o.total_amount
        }).ToList();

        return View(model);
    }

    // GET: MyAccount/OrderDetails/5
    public ActionResult OrderDetails(int id)
    {
        var order = db.Orders.Include("Order_Items").FirstOrDefault(o => o.order_id == id);
        if (order == null)
        {   
            return HttpNotFound();
        }

        var model = new OrderDetailViewModel
        {
            
            Address = db.Addresses.FirstOrDefault(a=> a.id == order.address_id ),
            
            Items = order.Order_Items.Select(i => new OrderItemViewModel
            {
                ProductName = db.Foods.FirstOrDefault(f => f.food_id == i.food_id).name,
                Quantity = i.quantity,
                Price = i.price
            }).ToList()
        };

        return PartialView("_OrderDetails", model); // Trả về partial view
    }
}
